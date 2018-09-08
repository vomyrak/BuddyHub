const express = require('express');
const app = express();
const fs = require('fs');
const server = require('http').createServer(app);
const XMLHttpRequest = require('xmlhttprequest').XMLHttpRequest;
const exec = require('child_process').exec;
const mongoose = require("mongoose");

var bodyParser = require('body-parser');
app.use(bodyParser.json()); // to support JSON-encoded bodies
app.use(bodyParser.urlencoded({ // to support URL-encoded bodies
  extended: true
}));

var passport = require("passport");
//enable login method using local strategy
var localStrategy = require("passport-local");
var User = require("./public/js/user");

// Get env variables
require('dotenv').config();
// Connect to the database
require('./db/connection');

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

const OutputDevice = require('./db/models/output-device');
const DeviceSuggestion = require('./db/models/device-suggestion');

const port = 8000;
server.listen(port);

// Specify to serve files from the public directory
app.use(express.static(__dirname + '/public'));

// Set the view engine to ejs
app.set('view engine', 'ejs');

//middleware so that req.user will be available in every single template
app.use(function(req,res,next){
   res.locals.currentUser = req.user;
    next();
});

app.get('/', function(req, res) {
  // Direct to home page
  // Render the page with all output devices in the dropdown

  var query = OutputDevice.find().sort('device');

  query.exec(function(err, devices) {
    if (err) return handleError(err);

    res.render('index', {
      devices: devices
    });
  });
});

app.get('/contact', isLoggedIn, function(req, res) {
  // Direct to device suggestion page
  // Render the page with all output devices in the dropdown

  var query = OutputDevice.find().sort('device');

  query.exec(function(err, devices) {
    if (err) return handleError(err);

    res.render('contact', {
      devices: devices
    });
  });
});

app.post('/feedback', isLoggedIn, function(req, res) {
  // Upload the details o the device suggestion to the database.
  // The "processed" field is set to false until the admin process this
  // device suggestion.

  var name = req.body.name;
  var email = req.body.email;
  var device = req.body.device;
  var description = req.body.description;

  var suggestion = new DeviceSuggestion({
    name: name,
    email: email,
    device: device,
    description: description,
    approved: false,
    processed: false
  });
  suggestion.save(function(err) {
    if (err) return handleError(err);
  });

  // Send a confirmation email to user
  var emailSender = require('./utils/email');
  emailSender.sendConfEmail(email, name, device, description);

  res.redirect('/submitted');
});

app.get('/submitted', function(req, res) {
  // Direct to summited page
  // Render the page with all output devices in the dropdown

  var query = OutputDevice.find().sort('device');

  query.exec(function(err, devices) {
    if (err) return handleError(err);

    res.render('submitted', {
      devices: devices
    });
  });
});

app.get('/device', isLoggedIn, function(req, res) {
  // Render the page with all output devices in the menu
  // Render the page with methods of the selected device
  var query = OutputDevice.find().sort('device');

  query.exec(function(err, devices) {
    if (err) return handleError(err);

    var query2 = OutputDevice.findOne({
      device: req.query.selected
    });
    query2.exec(function(error, selected) {
      if (error) return handleError(error);

      res.render('device', {
        devices: devices,
        device: selected
      });
    });
  });
});

app.post('/buddycook', function(req, res) {
  res.send("https://buddy-cook.herokuapp.com/");
});

app.post('/tts', function(req, res) {
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
      // Generate a random number between 10,000,000 and 99,999,999 to name the output files
      var number = Math.floor(Math.random() * 90000000) + 10000000;
      const outputFile = './output' + number + '.txt';
      fs.writeFile(outputFile, this.responseText, 'binary', err => {
        if (err) {
          console.error('ERROR:', err);
          return;
        }
        console.log(`Audio content written to file: ${outputFile}`);
        // Execute the command to turn the response text to an mp3 file
        // See: https://cloud.google.com/text-to-speech/docs/create-audio#text-to-speech-text-protocol
        exec('sed \'s|audioContent| |\' < ./output' + number + '.txt > ./tmp-output' + number + '.txt && tr -d \'\n ":{}\' < ./tmp-output' + number + '.txt > ./tmp-output-2' + number + '.txt && base64 ./tmp-output-2' + number + '.txt --decode > ./public/audio/synthesize-text-audio' + number + '.mp3 && rm ./tmp-output*.txt && rm ./output' + number + '.txt', (err, stdout, stderr) => {
          if (err) {
            console.error('ERROR:', err);
            // Node couldn't execute the command
            return;
          }
          // Send the path of the generated mp3 as response
          res.send('/audio/synthesize-text-audio' + number + '.mp3');
        });
      });
    }
  };
  // Open Connection
  xhttp.open("POST", "https://texttospeech.googleapis.com/v1beta1/text:synthesize", true);
  // Set headers of the http request
  xhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
  xhttp.setRequestHeader("X-Goog-Api-Key", process.env.GOOGLE);
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
