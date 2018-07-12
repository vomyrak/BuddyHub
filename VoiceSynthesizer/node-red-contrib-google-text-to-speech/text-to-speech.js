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
            //input from previous node is msg.payload
            //put the input data into a JSON object
            const data = {
              input: {text: msg.payload},
              voice: {languageCode: 'en-US', ssmlGender: 'FEMALE'},
              audioConfig: {audioEncoding: 'MP3'},
            };
            //function goes here
            //output goes to msg.payload as well
            var xhttp = new XMLHttpRequest();
            xhttp.onreadystatechange = function() {
              var response = this.responseText;
              //console.log(JSON.parse(this.responseText));
              //console.log(JSON.parse(this.responseText).audioContent);
              if (this.readyState == 4 && this.status == 200) {
                fs.writeFile(outputFile, this.responseText, 'binary', err => {
                  if (err) {
                    console.error('ERROR:', err);
                    return;
                  }
                  console.log(`Audio content written to file: ${outputFile}`);
                  exec('sed \'s|audioContent| |\' < ./output.txt > ./tmp-output.txt && tr -d \'\n ":{}\' < ./tmp-output.txt > ./tmp-output-2.txt && base64 ./tmp-output-2.txt --decode > ./synthesize-text-audio.mp3 && rm ./tmp-output*.txt && rm ./output.txt', (err, stdout, stderr) => {
                    if (err) {
                       console.error('ERROR:', err);
                      // node couldn't execute the command
                    return;
                    }
                    msg.payload = "./synthesize-text-audio.mp3";
                    node.send(msg);
                  });
                });
              }
            };
            xhttp.open("POST", "https://texttospeech.googleapis.com/v1beta1/text:synthesize", true);
            xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
            xhttp.setRequestHeader("X-Goog-Api-Key",  msg.key);
            xhttp.send(JSON.stringify(data));
            //Send the msg to output, i.e. the next node
            
        });
    }
    RED.nodes.registerType("text-to-speech",TTSNode);
}

