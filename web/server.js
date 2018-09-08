const express = require('express');
const app = express();
const server = require('http').createServer(app);
const mongoose = require("mongoose");

var bodyParser = require('body-parser');
app.use(bodyParser.json()); // to support JSON-encoded bodies
app.use(bodyParser.urlencoded({ // to support URL-encoded bodies
  extended: true
}));

// Get process.env variables
require('dotenv').config();

// Connect to the database
require('./db/connection');

// Dependencies for authentication
var passport = require("passport");
// Enable login method using local strategy
var localStrategy = require("passport-local");
var User = require("./db/models/user");
const auth = require('./routes/auth');

// Passport Configuration
app.use(require("express-session")({
        secret : "WSUROP2018",
        resave : false,
        saveUninitialized : false
}));
app.use(passport.initialize());
app.use(passport.session());
passport.use(new localStrategy(User.authenticate()));
passport.serializeUser(User.serializeUser());
passport.deserializeUser(User.deserializeUser());

const port = 8000;
server.listen(port);

// Specify to serve files from the public directory
app.use(express.static(__dirname + '/public'));

// Set the view engine to ejs
app.set('view engine', 'ejs');

// Middleware so that req.user will be available in every single template
app.use(function(req,res,next){
   res.locals.currentUser = req.user;
   next();
});

//-------------------------//
//----------PAGES----------//
//-------------------------//

var pages = require('./routes/pages');

app.get('/', function(req, res) { pages.renderWithAllDevices(res, 'index'); });

app.get('/contact', auth.isLoggedIn, function(req, res) { pages.renderWithAllDevices(res, 'contact'); });

app.post('/feedback', auth.isLoggedIn, pages.storeSuggestionAndRedirect);

app.get('/submitted', function(req, res) { pages.renderWithAllDevices(res, 'submitted'); });

app.get('/device', auth.isLoggedIn, pages.renderWithSelectedDevice);

//-------------------------//
//----------APIs-----------//
//-------------------------//

app.post('/buddycook', function(req, res) {
  res.send("https://buddy-cook.herokuapp.com/");
});

var tts = require('./routes/tts')
app.post('/tts', tts.texttospeech);


//-------------------------//
//-----AUTHENTICATION------//
//-------------------------//

// Auth routes //

// Handle sign up logic
app.post("/register", auth.register);

// Handling login logic
app.post("/login", auth.login);

// Logout Route
app.get("/logout", auth.logout);
