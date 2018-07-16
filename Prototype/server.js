const express = require('express');
const app = express();
const server = require('http').createServer(app);

var bodyParser = require('body-parser')
app.use(bodyParser.json()); // to support JSON-encoded bodies
app.use(bodyParser.urlencoded({ // to support URL-encoded bodies
  extended: true
}));

const pg = require('pg');
// Connect to users database
const pool = new pg.Pool({
  user: 'joan',
  host: '127.0.0.1',
  database: 'wsurop18',
  password: process.env.PASSWORD,
  port: '5432'
});


// A dictionary of online users
const users = {};

const port = 8000;
server.listen(port);

// Specify to serve files from the public directory
app.use(express.static(__dirname + '/public'));

// Set the view engine to ejs
app.set('view engine', 'ejs');

// HTML directory
const html_dir = __dirname + '/public/html/';

app.get('/', function(req, res) {
  // Direct to home page
  // Render the page with all output devices in the menu
  const listQuery = "SELECT DISTINCT device FROM outputdevice";
  pool.query(listQuery, (listErr, listResult) => {
    console.log(process.env.PASSWORD);
    console.log(listErr);
    res.render('index', {
      devices: listResult.rows
    });
  });
});
