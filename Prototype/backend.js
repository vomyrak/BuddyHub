const mongoose = require("mongoose");
const filereader = require("./auth.json");
var options = {useNewUrlParser: true, auth: {authdb: "admin"}};
options.user = filereader.user;
options.pass = filereader.pass;
var connectString = "mongodb://"+filereader.dns+":27017/uc";
mongoose.connect('mongodb://35.177.37.56:27017/uc', options)
    .then(() => console.log('Connected to MongoDB...'))
    .catch(error => console.error('Failed to connect',error));

const outputSchema = new mongoose.Schema({
    device: String,
    function: [{name: String, param: [Number]}]
});

const OutputDevice = mongoose.model('outputData', outputSchema, 'outputData');

async function createOutput(){
    const device = new OutputDevice({
        device: "test",
        function: [{name: "test", param:[1]}]
    }
    );
    const a = await device.save();
    console.log(a);
}
async function getOutput(){
    const testSchema = await OutputDevice.find();
    console.log(testSchema);
}

getOutput();