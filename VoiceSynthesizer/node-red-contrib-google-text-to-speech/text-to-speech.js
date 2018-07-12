const textToSpeech = require('@google-cloud/text-to-speech');
const fs = require('fs');

const client = new textToSpeech.TextToSpeechClient();

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

            //Send the msg to output, i.e. the next node
            node.send(msg);
        });
    }
    RED.nodes.registerType("text-to-speech",TTSNode);
}

