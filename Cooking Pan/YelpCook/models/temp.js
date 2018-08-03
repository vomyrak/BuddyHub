var mongoose = require("mongoose");

// Temp SCHEMA
var tempSchema = new mongoose.Schema({
    temperature: Number,
    units: String
});

module.exports = mongoose.model("Temperature", tempSchema);