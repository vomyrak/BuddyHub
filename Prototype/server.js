const express = require('express');
const app = express();
const fs = require('fs');
const server = require('http').createServer(app);
const XMLHttpRequest = require('xmlhttprequest').XMLHttpRequest;
const { exec } = require('child_process');
util = require('util');

var bodyParser = require('body-parser');
app.use(bodyParser.json()); // to support JSON-encoded bodies
app.use(bodyParser.urlencoded({ // to support URL-encoded bodies
  extended: true
}));

const pg = require('pg');
// Connect to users database
const pool = new pg.Pool({
  user: 'joan',
  host: '127.0.0.1',
  database: 'wsurop18',
  password: process.env.PASSWORD,
  port: '5432'
});


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
  const listQuery = "SELECT DISTINCT device FROM outputdevice";
  pool.query(listQuery, (listErr, listResult) => {
    res.render('index', {
      devices: listResult.rows
    });
  });
});

app.get('/device', function(req, res) {
  // Render the page with all output devices in the menu
  const listQuery = "SELECT DISTINCT device FROM outputdevice";
  const query = {
    // give the query a unique name
    name: 'get-methods',
    text: 'SELECT * FROM outputdevice WHERE device = $1',
    values: [req.query.selected]
  };
  pool.query(query, (err, result) => {
    pool.query(listQuery, (listErr, listResult) => {
      res.render('device', {
        methods: result.rows,
        devices: listResult.rows
      });
    });
  });
});

app.post('/get_key', function(req, res) {
  const query = "SELECT * FROM keys WHERE header_name = \'" + req.body.header + "\'";
  pool.query(query, (err, result) => {
    res.send(result.rows[0].key);
  });
});

app.post('/tts', function(req, res) {
  var input = req.body.input
  const query = "SELECT * FROM keys WHERE header_name = 'X-Goog-Api-Key'";
  pool.query(query, (err, result) => {
    var key = result.rows[0].key;
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
        // console.log(this.responseText);
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
      } else {
        console.log(this.responseText);
      }
    };
    // Open Connection
    xhttp.open("POST", "https://texttospeech.googleapis.com/v1beta1/text:synthesize", true);
    // Set headers of the http request
    xhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xhttp.setRequestHeader("X-Goog-Api-Key", key);
    // Send the http request with the data
    xhttp.send(JSON.stringify(data));
  });
});
