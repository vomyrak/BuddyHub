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


// Model for mongo database (API devices)
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

// Model for mongo database (C Sharp library devices)
const cSharpSchema = new mongoose.Schema({
  name: String,
  email: String,
  device: String,
  class_name: String,
  vid: String,
  pid: String,
  library_name: String,
  functions: [{
    name: String,
    continuous: Boolean,
  }],
  processed: Boolean,
});

const PendingCSharp = mongoose.model('pendingCSharps', cSharpSchema, 'pendingCSharps');



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
  // Render the homepage
  res.render('index');
});

app.get('/add-api', function(req, res) {
  // Render the API form page
  res.render('add-api');
});

app.get('/add-c-sharp', function(req, res) {
  // Render the C Sharp form page
  res.render('add-c-sharp');
});

app.post('/upload-api', function(req, res) {
  // Get the fields from the form
  var name = req.body.name;
  var email = req.body.email;
  var device = req.body.device;
  var methods = [];
  var methods_text = "";

  // Since when there is only one method,
  // method[0] becomes the first char of the method name.
  // While a method with a single char is not allowed,
  // req.body.method[0].length == 1 will indicated that
  // only one method is submitted, therefore shall not enters the while loop.
  if (req.body.method[0].length != 1) {
    // If more than one method is submitted,
    // add each method to the methods array
    // and add the formatted text for the confirmation email to methods_text.
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

      methods_text += "  Method" + (i + 1) + ": \n" +
        "    Method Name: " + req.body.method[i] + "\n" +
        "    Http Method: " + req.body.httpmethod[i] + "\n" +
        "    Link: " + req.body.link[i] + "\n" +
        "    Data: " + req.body.data[i] + "\n" +
        "    Headers: " + req.body.headers[i] + "\n" +
        "    Text Input Field: " + req.body.textinput[i] + "\n" +
        "    Continuous: " + method.continuous + "\n";
    }
  } else {
    // If only one method is submitted,
    // add the method to the methods array
    // and format the text for the confirmation email.
    methods = [{
      method: req.body.method,
      http_method: req.body.httpmethod,
      link: req.body.link,
      data: req.body.data,
      headers: req.body.headers,
      text_input_field: req.body.textinput,
      continuous: req.body.continuous == 'true'
    }];
    methods_text = "  Method1: \n" +
      "    Method Name: " + req.body.method + "\n" +
      "    Http Method: " + req.body.httpmethod + "\n" +
      "    Link: " + req.body.link + "\n" +
      "    Data: " + req.body.data + "\n" +
      "    Headers: " + req.body.headers + "\n" +
      "    Text Input Field: " + req.body.textinput + "\n" +
      "    Continuous: " + req.body.continuous + "\n";
  }

  // Put the fields together into the PendingAPI Schema.
  var api = new PendingAPI({
    name: name,
    email: email,
    device: device,
    methods: methods,
    processed: false
  });
  // Save the details of the API into the database
  api.save(function(err) {
    if (err) return handleError(err);
  });

  // Send a confirmation email to user
  var mailOptions = {
    from: filereader3.email,
    to: email,
    subject: 'Device API Upload Form Received',
    text: 'Dear ' + name + ',\n\n' +
    'Your device suggestion form had been received. ' +
    'We will notice you once we have reviewed your suggestion.\n\n' +
    'Device: ' + device + '\n' +
    'Methods: \n' + methods_text + '\n\n' +
    'Thank you for choosing BuddyHub!\n\n'
  };

  transporter.sendMail(mailOptions, function(error, info) {
    if (error) {
      console.log(error);
    } else {
      console.log('Email sent: ' + info.response);
    }
  });

  res.redirect("/submitted");
});

app.post('/upload-c-sharp', function(req, res) {
  // Get the fields from the form
  var name = req.body.name;
  var email = req.body.email;
  var device = req.body.device;
  var class_name = req.body.class;
  var vid = req.body.vid;
  var pid = req.body.pid;
  var library = req.body.library;
  var functions = [];
  var functions_text = "";

  // Since when there is only one function,
  // function[0] becomes the first char of the function name.
  // While a function with a single char is not allowed,
  // req.body.function[0].length == 1 will indicated that
  // only one function is submitted, therefore shall not enters the while loop.
  if (req.body.function[0].length != 1) {
    // If more than one function is submitted,
    // add each function to the functions array
    // and add the formatted text for the confirmation email to functions_text.
    for (var i = 0; i < req.body.function.length; i++) {
      var func = {
        name: req.body.function[i],
        continuous: req.body.continuous[i] == 'true'
      }
      functions.push(func);

      functions_text += "  Function" + (i + 1) + ": \n" +
        "    Function Name: " + req.body.function[i] + "\n" +
        "    Continuous: " + func.continuous + "\n";
    }
  } else {
    // If only one function is submitted,
    // add the function to the functions array
    // and format the text for the confirmation email.
    functions = [{
      name: req.body.function,
      continuous: req.body.continuous == 'true'
    }];
    functions_text = "  Function1: \n" +
      "    Function Name: " + req.body.function + "\n" +
      "    Continuous: " + req.body.continuous + "\n";
  }

  // Put the fields together into the PendingCSharp Schema.
  var csharp = new PendingCSharp({
    name: name,
    email: email,
    device: device,
    class_name: class_name,
    vid: vid,
    pid: pid,
    library_name: library,
    functions: functions,
    processed: false
  });
  // Save the details of the C Sharp library into the database
  csharp.save(function(err) {
    if (err) return handleError(err);
  });

  // Send a confirmation email to user
  var mailOptions = {
    from: filereader3.email,
    to: email,
    subject: 'Device API Upload Form Received',
    text: 'Dear ' + name + ',\n\n' +
    'Your device suggestion form had been received. ' +
    'We will notice you once we have reviewed your suggestion.\n\n' +
    'Device: ' + device + '\n' +
    'Class Name: ' + class_name + '\n' +
    'vid: ' + vid + '\n' +
    'pid: ' + pid + '\n' +
    'Library Name: ' + library + '\n' +
    'Functions: \n' + functions_text + '\n\n' +
    'Thank you for choosing BuddyHub!\n\n'
  };

  transporter.sendMail(mailOptions, function(error, info) {
    if (error) {
      console.log(error);
    } else {
      console.log('Email sent: ' + info.response);
    }
  });

  res.redirect("/submitted");
});

app.get('/submitted', function(req, res) {
  res.render('submitted');
});
