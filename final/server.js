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

// Model for mongo database
const outputSchema = new mongoose.Schema({
  device: String,
  device_en: String,
  device_zh: String,
  methods: [{
    method: String,
    method_zh: String,
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

const OutputDevice = mongoose.model('outputDevices2', outputSchema, 'outputDevices2');

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

app.get('/', function(req, res) {
  // Direct to home page
  // Render the page with all output devices in the dropdown

  var query = OutputDevice.find().sort('device');

  query.exec(function(err, devices) {
    if (err) return handleError(err);

    console.log(devices);

    res.render('index', {
      devices: devices
    });
  });
});

app.get('/device', function(req, res) {
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
      var number = Math.floor(Math.random()*90000000) + 10000000;
      const outputFile = './output'+ number + '.txt';
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
  xhttp.setRequestHeader("X-Goog-Api-Key", filereader2.google);
  // Send the http request with the data
  xhttp.send(JSON.stringify(data));
});
