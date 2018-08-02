var mongoose = require("mongoose");
var Step = require("./step");

// INSTRUCTION STEP SCHEMA

var stepSchema = new mongoose.Schema({
    stepnumber: Number,
    step: String
});

// RECIPE Schema

var recipeSchema = new mongoose.Schema({
    name: String,
    image: String,
    temperature: Number,
    cooking_time: Number,
    steps: Number,
    instruction: [stepSchema
/*      {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Step"
      } */
    ]
});

var Recipe = mongoose.model("Recipe", recipeSchema);

module.exports = mongoose.model("Recipe", recipeSchema);
