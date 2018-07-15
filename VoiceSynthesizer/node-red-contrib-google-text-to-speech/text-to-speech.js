const textToSpeech = require('@google-cloud/text-to-speech');
const fs = require('fs');
var XMLHttpRequest = require("xmlhttprequest").XMLHttpRequest;
const { exec } = require('child_process');

const client = new textToSpeech.TextToSpeechClient();

const outputFile = './output.txt';

module.exports = function(RED) {
    function TTSNode(config) {
        RED.nodes.createNode(this,config);
        var node = this;
        node.on('input', function(msg) {
            // Input from previous node is msg.payload
            // Put the input data into a JSON object
            const data = {
              input: {text: msg.payload},
              voice: {languageCode: 'en-US', ssmlGender: 'FEMALE'},
              audioConfig: {audioEncoding: 'MP3'},
            };
            // Create an http request to the Google text to speech API
            var xhttp = new XMLHttpRequest();
            // Add callback function for the http request
            xhttp.onreadystatechange = function() {
              if (this.readyState == 4 && this.status == 200) {
                // Write the response text to the output file
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
                    // Set the output message to path of the audio file generated
                    msg.payload = "./synthesize-text-audio.mp3";
                    // Send the msg to output, i.e. the next node
                    node.send(msg);
                  });
                });
              }
            };
            // Open the Connection
            xhttp.open("POST", "https://texttospeech.googleapis.com/v1beta1/text:synthesize", true);
            // Set the request headers
            xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
            xhttp.setRequestHeader("X-Goog-Api-Key",  msg.key);
            // Send the request with the JSON object containing input and other info
            xhttp.send(JSON.stringify(data));
            
        });
    }
    RED.nodes.registerType("text-to-speech",TTSNode);
}

