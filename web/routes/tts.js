const fs = require('fs');
const exec = require('child_process').exec;
const XMLHttpRequest = require('xmlhttprequest').XMLHttpRequest;

exports.texttospeech = function(req, res) {
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
}
