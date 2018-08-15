var mongoose = require("mongoose");
var Step = require("./step");

// INSTRUCTION STEP SCHEMA

var stepSchema = new mongoose.Schema({
    stepnumber: Number,
    image: String,
    step: String,
    time: Number,
    temperature: Number
});

// RECIPE Schema

var recipeSchema = new mongoose.Schema({
    name: String,
    image: String,
    cooking_time: Number,
    steps: Number,
    ingredients: String,
    servings: Number,
    instruction: [stepSchema
      // Below is another way of embedding data (by reference), which is currently commented out
/*      {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Step"
      } */
    ]
});

var Recipe = mongoose.model("Recipe", recipeSchema);

module.exports = mongoose.model("Recipe", recipeSchema);
