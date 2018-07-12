module.exports = function(RED) {
    function TTSNode(config) {
        RED.nodes.createNode(this,config);
        var node = this;
        node.on('input', function(msg) {
            //input = msg.payload
            //function goes here
            //output goes to msg.payload as well

            //Send the msg to output, i.e. the next node
            node.send(msg);
        });
    }
    RED.nodes.registerType("text-to-speech",TTSNode);
}

