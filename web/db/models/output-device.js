const mongoose = require("mongoose");

const outputSchema = new mongoose.Schema({
  device: String,
  device_name: String,
  methods: [{
    method: String,
    description: String,
    http_method: String,
    link: String,
    data: String,
    headers: String,
    callback_function: String,
    text_input_field: String,
    params: [{
      param_field: String,
      param_choices: [Number]
    }]
  }]
});

module.exports = mongoose.model('outputDevices3', outputSchema, 'outputDevices3');
