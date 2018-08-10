'use strict';
var mongoose = require('mongoose');
var Schema = mongoose.Schema;


// INSTRUCTION STEP SCHEMA

var stepSchema = new Schema({
    stepnumber: Number,
    image: String,
    step: String,
    time: Number,
    temperature: Number
});

// RECIPE Schema

var recipeSchema = new Schema({
    name: String,
    image: String,
    temperature: Number,
    cooking_time: Number,
    steps: Number,
    instruction: [stepSchema
      // Below is another way of embedding data (by reference), which is currently commented out
/*      {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Step"
      } */
    ]
});

module.exports = mongoose.model("Recipe", recipeSchema);
