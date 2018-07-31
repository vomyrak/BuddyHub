var express = require("express"),
    app = express(),
    bodyParser = require("body-parser"),
    mongoose = require("mongoose");

mongoose.connect("mongodb://cooking:wsurop18@ds223268.mlab.com:23268/wsurop_cooking", { useNewUrlParser: true });
app.use(bodyParser.urlencoded({extended: true}));
app.set("view engine", "ejs");

// SCHEMA setup
var Insctruction {
  step 1: String
  step 1 image: String
  step 2:
  step 3:
}

var recipeSchema = new mongoose.Schema({
    name: String,
    image: String,
    temperature: Number,
    cooking_time: Number,
    instruction: Instruction
});

var Recipe = mongoose.model("Recipe", recipeSchema);

app.get("/", function(req, res){
    res.render("landing");
});

app.get("/recipes", function(req, res){
    Recipe.find({}, function(err, recipes){
        if(err){
            console.log(err);
        } else {
            res.render("index", {recipes:recipes});
        }
    })
});

app.get("/recipes/new", function(req, res){
    res.render("new");
});

// SHOW - shows more info about a recipe
app.get("/recipes/:id", function(req,res){
    Recipe.findById(req.params.id, function(err, foundRecipe){
        if(err){
            console.log(err);
        } else {
            res.render("show", {recipe: foundRecipe});
        }
    });
});

app.post("/recipes", function(req,res){
    var name = req.body.name;
    var image = req.body.image;
    var instruction = req.body.instruction;
    var temperature = req.body.temperature;
    var cooking_time = req.body.cooking_time;
    var newRecipe = {name: name, image: image, instruction: instruction, temperature: temperature, cooking_time:cooking_time};
            Recipe.create(newRecipe
        , function(err, recipe){
        if(err){
            console.log(err);
        } else {
        console.log(recipe);
             res.redirect("/recipes");
        }
    });
});

app.listen(3000, process.env.IP, function(){
    console.log("YelpCook server started");
});
