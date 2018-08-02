var express = require("express"),
    app = express(),
    bodyParser = require("body-parser"),
    mongoose = require("mongoose"),
    server = require('http').Server(app),
    io = require('socket.io')(server);

mongoose.connect("mongodb://cooking:wsurop18@ds223268.mlab.com:23268/wsurop_cooking", { useNewUrlParser: true });
app.use(bodyParser.urlencoded({extended: true}));
app.set("view engine", "ejs");

// SCHEMA setup
var Step = require("./models/step"),
    Recipe = require("./models/recipe")

var tempSchema = new mongoose.Schema({
    temperature: Number,
    units: String
});

var Temperature = mongoose.model("Temperature", tempSchema);

Temperature.on('change', function(data){
    console.log(data);
});

io.on('connection', function(socket) {
    function getTemp() {
        Temperature.findOne({}, {}, {sort: {"created_at": -1}}, function(err, data) {
            if(err){
                console.log(err);
            }
            else {
                console.log(data.temperature);
                socket.emit('newTemp', data.temperature);
            }
        });
    }
    setInterval(getTemp, 100);
});

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

server.listen(3000, process.env.IP, function(){
    console.log("YelpCook server started");
});
