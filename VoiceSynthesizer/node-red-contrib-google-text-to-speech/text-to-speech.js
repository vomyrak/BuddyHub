const textToSpeech = require('@google-cloud/text-to-speech');
const fs = require('fs');

const client = new textToSpeech.TextToSpeechClient();

const outputFile = '/tmp/output.mp3';

module.exports = function(RED) {
    function TTSNode(config) {
        RED.nodes.createNode(this,config);
        var node = this;
        node.on('input', function(msg) {
            //input from previous node is msg.payload
            //put the input data into a JSON object
            const request = {
              input: {text: msg.payload},
              voice: {languageCode: 'en-US', ssmlGender: 'FEMALE'},
              audioConfig: {audioEncoding: 'MP3'},
            };
            //function goes here
            //output goes to msg.payload as well
            client.synthesizeSpeech(request, (err, response) => {
              if (err) {
                console.error('ERROR:', err);
                return;
              }

              fs.writeFile(outputFile, response.audioContent, 'binary', err => {
                if (err) {
                  console.error('ERROR:', err);
                  return;
                }
                console.log(`Audio content written to file: ${outputFile}`);
              });
            });
            //Send the msg to output, i.e. the next node
            node.send(msg);
        });
    }
    RED.nodes.registerType("text-to-speech",TTSNode);
}

