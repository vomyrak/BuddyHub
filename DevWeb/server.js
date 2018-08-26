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


// Model for mongo database
const apiSchema = new mongoose.Schema({
  name: String,
  email: String,
  device: String,
  methods: [{
    method: String,
    http_method: String,
    link: String,
    data: String,
    headers: String,
    text_input_field: String,
    continuous: Boolean,
  }],
  processed: Boolean,
});

const PendingAPI = mongoose.model('pendingAPIs', apiSchema, 'pendingAPIs');



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

  var name = req.body.name;
  var email = req.body.email;
  var device = req.body.device;
  var methods = [];

  if (req.body.method[1] != undefined) {
    for (var i = 0; i < req.body.method.length; i++) {
      var method = {
        method: req.body.method[i],
        http_method: req.body.httpmethod[i],
        link: req.body.link[i],
        data: req.body.data[i],
        headers: req.body.headers[i],
        text_input_field: req.body.textinput[i],
        continuous: req.body.continuous[i] == 'true'
      }
      methods.push(method);
    }
  } else {
    methods = [{
      method: req.body.method,
      http_method: req.body.httpmethod,
      link: req.body.link,
      data: req.body.data,
      headers: req.body.headers,
      text_input_field: req.body.textinput,
      continuous: req.body.continuous == 'true'
    }]
  }

  var api = new PendingAPI({
    name: name,
    email: email,
    device: device,
    methods: methods,
    processed: false
  });
  api.save(function(err) {
    if (err) return handleError(err);
  });

  res.redirect("/submitted");
});

app.get('/submitted', function(req, res) {
  res.render('submitted');
});
