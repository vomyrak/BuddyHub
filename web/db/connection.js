const mongoose = require("mongoose");

// Config and connect to mongo database
var options = {
  useNewUrlParser: true,
  auth: {
    authdb: "admin"
  }
};
options.user = process.env.DB_USER;
options.pass = process.env.DB_PASS;
var connectString = "mongodb://" + process.env.DB_DNS + ":27017/" + process.env.DB_NAME;
mongoose.connect(connectString, options)
  .then(() => console.log('Connected to MongoDB...'))
  .catch(error => console.error('Failed to connect', error));
