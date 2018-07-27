const mongoose = require("mongoose");
const filereader = require("./auth.json");

// Construct log-on string
var options = {useNewUrlParser: true, auth: {authdb: "admin"}};
options.user = filereader.user;
options.pass = filereader.pass;
var connectString = "mongodb://"+filereader.dns+":27017/uc";

// Connect to remote database
mongoose.connect(connectString, options)
    .then(() => console.log('Connected to MongoDB...'))
    .catch(error => console.error('Failed to connect',error));

// Construct a schema (format of data) 
const outputSchema = new mongoose.Schema({
    device: String,
    function: [{name: String, param: [Number]}]
});

// Compile the schema into the model
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