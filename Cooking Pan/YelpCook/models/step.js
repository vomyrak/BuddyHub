var mongoose = require("mongoose");

// INSTRUCTION STEP SCHEMA

var stepSchema = new mongoose.Schema({
    stepnumber: Number,
    step: String
});

module.exports = mongoose.model("Step", stepSchema);
