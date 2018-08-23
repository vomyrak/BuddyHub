var express = require("express");
var app = express();

var mongoose = require("mongoose");
var bodyParser = require("body-parser");

var passport = require("passport");
//enable login method using local strategy
var localStrategy = require("passport-local");
var passportLocalMongoose = require("passport-local-mongoose");

var User = require("./models/user");




//27017 - default mongo port
mongoose.connect('mongodb://localhost:27017/auth_demo',{ useNewUrlParser: true }) 

app.use(bodyParser.urlencoded({extended: true}));
app.set("view engine", "ejs");

//Passport Configuration
app.use(require("express-session")({
        secret : "Rusty",
        resave : false,
        saveUninitialized : false
}));
app.use(passport.initialize());
app.use(passport.session());
passport.use(new localStrategy(User.authenticate()));
passport.serializeUser(User.serializeUser());
passport.deserializeUser(User.deserializeUser());




app.get("/secret", isLoggedIn, function(req,res){
    res.render("secret");
})


app.get("/", function(req,res){
    //passing req.user variable to be called in home.ejs
    res.render("home" /*, {currentUser: req.user}*/);
});

//middleware so that req.user will be available in every single template 
app.use(function(req,res,next){
   res.locals.currentUser = req.user;
    next();
});

//Auth routes//
//show register form            
app.get("/register", function(req,res){
   res.render("register") ;
});
 
//handle sign up logic
app.post("/register", function(req,res){
   //making a new user (1)
    var newUser = new User({username : req.body.username});
    User.register(newUser, req.body.password, function(err,user) {
        if(err){
            console.log(err);
            return res.render("register");
        }
        //then loging them in using passport.authenticate (2)
        passport.authenticate("local")(req,res, function(){
            res.redirect("/secret");
        });   
    }
                  
    )
});


//Login Route
app.get("/login", function(req,res){
    res.render("login");
});

//handling login logic
//using middleware to call authenticate method
app.post("/login", passport.authenticate("local", 
    {
        successRedirect: "/secret",
        failureRedirect: "/login"
    }
    ), function(req,res){
});

//Logout Route
app.get("/logout", function(req,res){
    req.logout();
    res.redirect("/");
});

//middleware to check if user is logged in
function isLoggedIn(req,res, next){
    if(req.isAuthenticated()){
        return next();
    } 
    res.redirect("/login");
}

app.listen(3000, function(){
    console.log("Server started..")
});

