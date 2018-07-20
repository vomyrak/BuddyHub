const express = require('express');
const app = express();
const fs = require('fs');
const server = require('http').createServer(app);
const XMLHttpRequest = require('xmlhttprequest').XMLHttpRequest;
const { exec } = require('child_process');
const mongoose = require("mongoose");
const filereader = require("./auth.json");
const filereader2 = require("./keys.json");

var bodyParser = require('body-parser');
app.use(bodyParser.json()); // to support JSON-encoded bodies
app.use(bodyParser.urlencoded({ // to support URL-encoded bodies
  extended: true
}));

mongoose.connect('mongodb://localhost:27017/uc', {useNewUrlParser: true});

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
      text_input_field:String,
      params: [{
        param_field: String,
        param_choices: [Number]}]
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

app.get('/', function(req, res) {
  // Direct to home page
  // Render the page with all output devices in the menu

  var query = OutputDevice.find().sort('device');
  query.select('device');


  query.exec(function (err, devices) {
    if (err) return handleError(err);

    res.render('index', {
      devices: devices
    });
  });
});

app.get('/device', function(req, res) {
  // Render the page with all output devices in the menu
  var listQuery = OutputDevice.find().sort('device');
  listQuery.select('device');


  listQuery.exec(function (err, devices) {
    if (err) return handleError(err);

    var query = OutputDevice.findOne({device: req.query.selected});
    query.exec(function (error, selected) {
      if (error) return handleError(error);

      res.render('device', {
        devices: devices,
        methods: selected.methods
      });
    });
  });
});

app.post('/tts', function(req, res) {
  var input = req.body.input
  var data = {
    input :{
      text: input
    },
    voice :{
      languageCode:'en-gb',
      name:'en-GB-Standard-A',
      ssmlGender:'FEMALE'
    },
    audioConfig:{
      audioEncoding:'MP3'
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
        exec('sed \'s|audioContent| |\' < ./output.txt > ./tmp-output.txt && tr -d \'\n ":{}\' < ./tmp-output.txt > ./tmp-output-2.txt && base64 ./tmp-output-2.txt --decode > ./synthesize-text-audio.mp3 && rm ./tmp-output*.txt && rm ./output.txt', (err, stdout, stderr) => {
          if (err) {
            console.error('ERROR:', err);
            // Node couldn't execute the command
            return;
          }

          var filePath = "./synthesize-text-audio.mp3";
          var stat = fs.statSync(filePath);

          res.writeHead(200, {
            'Content-Type': 'audio/mpeg',
            'Content-Length': stat.size
          });

          var readStream = fs.createReadStream(filePath);
          // We replaced all the event handlers with a simple call to util.pump()
          readStream.pipe(res);
          // Set the output message to path of the audio file generated
          // res.sendfile("./synthesize-text-audio.mp3");
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

app.get('/synthesize-text-audio.mp3', function(req, res) {
  var filePath = "./synthesize-text-audio.mp3";
  var stat = fs.statSync(filePath);

  res.writeHead(200, {
    'Content-Type': 'audio/mpeg',
    'Content-Length': stat.size
  });

  var readStream = fs.createReadStream(filePath);
  // We replaced all the event handlers with a simple call to util.pump()
  readStream.pipe(res);
});
