const mongoose = require("mongoose");

const deviceSchema = new mongoose.Schema({
  name: String,
  email: String,
  device: String,
  description: String,
  approved: Boolean,
  processed: Boolean
});

module.exports = mongoose.model('deviceSuggestion', deviceSchema, 'deviceSuggestion');
