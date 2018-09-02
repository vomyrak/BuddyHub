const express = require('express');
const app = express();
const fs = require('fs');
const server = require('http').createServer(app);
const XMLHttpRequest = require('xmlhttprequest').XMLHttpRequest;
const exec = require('child_process').exec;
const mongoose = require("mongoose");
const filereader = require("./auth.json");
const filereader2 = require("./keys.json");

var bodyParser = require('body-parser');


var passport = require("passport");
//enable login method using local strategy
var localStrategy = require("passport-local");
var passportLocalMongoose = require("passport-local-mongoose");
var User = require("./public/js/user");

app.use(bodyParser.json()); // to support JSON-encoded bodies
app.use(bodyParser.urlencoded({ // to support URL-encoded bodies
  extended: true
}));

// Config and connect to mongo database
var options = {
  useNewUrlParser: true,
  auth: {
    authdb: "admin"
  }
};
options.user = filereader.user;
options.pass = filereader.pass;
var connectString = "mongodb://" + filereader.dns + ":27017/uc";
mongoose.connect(connectString, options)
  .then(() => console.log('Connected to MongoDB...'))
  .catch(error => console.error('Failed to connect', error));


//Passport Configuration
app.use(require("express-session")({
        secret : "WSUROP2018",
        resave : false,
        saveUninitialized : false
}));
app.use(passport.initialize());
app.use(passport.session());
passport.use(new localStrategy(User.authenticate()));
passport.serializeUser(User.serializeUser());
passport.deserializeUser(User.deserializeUser());



// Model for mongo database
const outputSchema = new mongoose.Schema({
  device: String,
  methods: [{
    method: String,
    description: String,
    http_method: String,
    link: String,
    data: String,
    headers: String,
    callback_function: String,
    text_input_field: String,
    params: [{
      param_field: String,
      param_choices: [Number]
    }]
  }]
});

const OutputDevice = mongoose.model('outputDevices', outputSchema, 'outputDevices');

// A dictionary of online users
const users = {};

const port = 8000;
server.listen(port);

// Specify to serve files from the public directory
app.use(express.static(__dirname + '/public'));

// Set the view engine to ejs
app.set('view engine', 'ejs');

// HTML directory
const html_dir = __dirname + '/public/html/';

//middleware so that req.user will be available in every single template 
app.use(function(req,res,next){
   res.locals.currentUser = req.user;
    next();
});

app.get('/', function(req, res) {
  // Direct to home page
  // Render the page with all output devices in the dropdown

  var query = OutputDevice.find().sort('device');
  //query.select('device');


  query.exec(function(err, devices) {
    if (err) return handleError(err);

    res.render('index', {
      devices: devices
    });
  });
});

app.get('/device', isLoggedIn, function(req, res) {
  // Render the page with all output devices in the menu
  // Render the page with methods of the selected device
  var query = OutputDevice.findOne({
    device: req.query.selected
  });
  query.exec(function(error, selected) {
    if (error) return handleError(error);
  
    res.render('device', {
      device: selected
    });
  });
});

app.post('/tts', isLoggedIn, function(req, res) {
  var input = req.body.input
  // Config json object to be send to the google tts API
  var data = {
    input: {
      text: input
    },
    voice: {
      languageCode: 'en-gb',
      name: 'en-GB-Standard-A',
      ssmlGender: 'FEMALE'
    },
    audioConfig: {
      audioEncoding: 'MP3'
    }
  }
  xhttp = new XMLHttpRequest();
  xhttp.onreadystatechange = function() {
    if (this.readyState == 4 && this.status == 200) {
      // If the http request is successful
      // Write the response text to the output file
      const outputFile = './output.txt';
      fs.writeFile(outputFile, this.responseText, 'binary', err => {
        if (err) {
          console.error('ERROR:', err);
          return;
        }
        console.log(`Audio content written to file: ${outputFile}`);
        // Execute the command to turn the response text to an mp3 file
        // See: https://cloud.google.com/text-to-speech/docs/create-audio#text-to-speech-text-protocol
        exec('sed \'s|audioContent| |\' < ./output.txt > ./tmp-output.txt && tr -d \'\n ":{}\' < ./tmp-output.txt > ./tmp-output-2.txt && base64 ./tmp-output-2.txt --decode > ./public/synthesize-text-audio.mp3 && rm ./tmp-output*.txt && rm ./output.txt', (err, stdout, stderr) => {
          if (err) {
            console.error('ERROR:', err);
            // Node couldn't execute the command
            return;
          }
          res.sendStatus(200);
        });
      });
    }
  };
  // Open Connection
  xhttp.open("POST", "https://texttospeech.googleapis.com/v1beta1/text:synthesize", true);
  // Set headers of the http request
  xhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
  xhttp.setRequestHeader("X-Goog-Api-Key", filereader2.google);
  // Send the http request with the data
  xhttp.send(JSON.stringify(data));
});

//-------------------------//
//-----AUTHENTICATION------//
//-------------------------//


//Auth routes//

 
//handle sign up logic
app.post("/register", function(req,res){
   //making a new user (1)
    var newUser = new User({username : req.body.username});
    User.register(newUser, req.body.password, function(err,user) {
        if(err){
            console.log(err);
            return res.render("home");
        }
        //then loging them in using passport.authenticate (2)
        passport.authenticate("local")(req,res, function(){
            res.redirect("/");
        });   
    }
                  
    )
});

//handling login logic
//using middleware to call authenticate method
app.post("/login", passport.authenticate("local", 
    {
        successRedirect: "/",
        failureRedirect: "/"
    }
    ), function(req,res){
});

//Logout Route
app.get("/logout", function(req,res){
    req.logout();
    res.redirect("/");
});

//middleware to check if user is logged in
function isLoggedIn(req,res, next){
    if(req.isAuthenticated()){
        return next();
    } 
    res.redirect("/");
}