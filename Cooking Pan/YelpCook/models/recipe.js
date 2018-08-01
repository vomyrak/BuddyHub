var mongoose = require("mongoose");

// RECIPE Schema

var recipeSchema = new mongoose.Schema({
    name: String,
    image: String,
    temperature: Number,
    cooking_time: Number,
    steps: Number,
    instruction: [
      {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Step"
      }
    ]
});

var Recipe = mongoose.model("Recipe", recipeSchema);


module.exports = mongoose.model("Recipe", recipeSchema);
