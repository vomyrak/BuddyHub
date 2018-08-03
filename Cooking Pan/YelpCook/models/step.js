var mongoose = require("mongoose");

// INSTRUCTION STEP SCHEMA

var stepSchema = new mongoose.Schema({
    stepnumber: Number,
    image: String,
    step: String,
    time: Number,
    temperature: Number
});

module.exports = mongoose.model("Step", stepSchema);
