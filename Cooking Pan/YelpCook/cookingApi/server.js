var express = require('express'),
  app = express(),
  port = process.env.PORT || 3000,
  mongoose = require('mongoose'),
  Recipe = require('./api/models/cookingModel'), //created model loading here
  bodyParser = require('body-parser');

// mongoose instance connection url connection
mongoose.Promise = global.Promise;
const uri = "mongodb://cooking:wsurop18@ds223268.mlab.com:23268/wsurop_cooking";
mongoose.connect(uri, {useNewUrlParser: true});


app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

app.use(function(req, res) {
  res.status(404).send({url: req.originalUrl + ' not found'})
});

var routes = require('./api/routes/cookingRoutes'); //importing route
routes(app); //register the route


app.listen(port);


console.log('cooking app RESTful API server started on: ' + port);
