var express = require("express"),
    app = express(),
    bodyParser = require("body-parser"),
    mongoose = require("mongoose"),
    server = require('http').Server(app),
    io = require('socket.io')(server);

const uri = "mongodb://cooking:wsurop18@ds223268.mlab.com:23268/wsurop_cooking";
mongoose.connect(uri, {useNewUrlParser: true});

app.use(bodyParser.urlencoded({extended: true}));
app.use(express.static(__dirname + '/public'));
app.set("view engine", "ejs");

// SCHEMA setup
var Step = require("./models/step"),
    Recipe = require("./models/recipe"),
    Temperature = require("./models/temp");

// SOCKET setup
io.on('connection', function(socket) {
    function getTemp() {
        Temperature.findOne({}, {}, {sort: {"_id": -1}}, function(err, data) {
            if(err){
                console.log(err);
            }
            else {
                socket.emit('newTemp', data.temperature);
            }
        });
    }
    setInterval(getTemp, 1000);
});

//ROUTES
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
    Recipe.findById(req.params.id).populate("instruction").exec(function(err, foundRecipe){
        if(err){
            console.log(err);
        } else {
            res.render("show", {recipe: foundRecipe});
        }
    });
});

// SHOW STEPS - shows more info about a recipe
app.get("/recipe/:id/:stepid", function(req,res){
    Recipe.findById(req.params.id).populate("instruction").exec(function(err, foundRecipe){
        if(err){
            console.log(err);
        } else {
            res.render("step", {recipe: foundRecipe, stepid: req.params.stepid});
        }
    });
});

app.post("/recipes", function(req,res){
    var name = req.body.name,
        image = req.body.image,
        servings = req.body.servings,
        instruction = req.body.instruction,
        cooking_time = req.body.cooking_time,
        ingredients = req.body.ingredients,
        nof_steps = req.body.steps;
    var newRecipe = {name: name, image: image, cooking_time: cooking_time, servings: servings, ingredients: ingredients, instruction: []};
        for (var i=0;i<nof_steps;i++){
                var tempstep = req.body['step' + i];
                var tempimg = req.body['img' + i];
                var tempsteptemp = req.body['temperature' + i];  // temporary step object temperature parameter
                var tempsteptime = req.body['time' + i];
                newRecipe.instruction.push({
                  stepnumber: i+1,
                  step: tempstep,
                  image: tempimg,
                  time: tempsteptime,
                  temperature: tempsteptemp
                });
          };
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

server.listen(3000, process.env.IP, function(){
    console.log("BuddyCook server started");
});
