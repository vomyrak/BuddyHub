const express = require('express');
const app = express();
const fs = require('fs');
const server = require('http').createServer(app);
const XMLHttpRequest = require('xmlhttprequest').XMLHttpRequest;
const mongoose = require("mongoose");
const filereader = require("./auth.json");
const filereader2 = require("./keys.json");
const filereader3 = require("./email.json");

var bodyParser = require('body-parser');
app.use(bodyParser.json()); // to support JSON-encoded bodies
app.use(bodyParser.urlencoded({ // to support URL-encoded bodies
  extended: true
}));

// Config and connect to mongo database
var options = {
  useNewUrlParser: true,
  auth: {
    authdb: "admin"
  }
};
options.user = filereader.user;
options.pass = filereader.pass;
var connectString = "mongodb://" + filereader.dns + ":27017/uc";
mongoose.connect(connectString, options)
  .then(() => console.log('Connected to MongoDB...'))
  .catch(error => console.error('Failed to connect', error));



// Email Configuration

var nodemailer = require('nodemailer');

var transporter = nodemailer.createTransport({
  service: 'gmail',
  auth: {
    user: filereader3.email,
    pass: filereader3.password
  }
});

// A dictionary of online users
const users = {};

const port = 8000;
server.listen(port);

// Specify to serve files from the public directory
app.use(express.static(__dirname + '/public'));

// Set the view engine to ejs
app.set('view engine', 'ejs');

app.get('/', function(req, res) {
  res.render('index');
});

app.get('/add-api', function(req, res) {
  res.render('add-api');
});

app.get('/add-c-sharp', function(req, res) {
  res.render('add-c-sharp');
});

app.post('/upload-api', function(req, res) {
  console.log(req.body.method.length);
  for (var i = 0; i < req.body.method.length; i++) {
    console.log(req.body.method[i]);
  }
  res.redirect("/submitted");
});

app.get('/submitted', function(req, res) {
  res.render('submitted');
});
