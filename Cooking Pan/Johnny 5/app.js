var express = require('express');
var app = express();
var server = require('http').createServer(app);
var io = require('socket.io')(server);
var five = require('johnny-five');

var board = new five.Board();
var temperature;

server.listen(3000);

app.use(express.static(__dirname + '/node_modules'));
app.get('/', function(req, res, next) {
  res.sendFile(__dirname + '/index.html');
});

board.on('ready', function(){
  temperature = new five.Thermometer( {
    controller: 'TMP36',
    pin: 'A0'
  });

  temperature.on('change', function() {
    board.emit('tempChange', temperature.C);
  });
});

io.sockets.on('connection', function(socket) {
  board.on('tempChange', function(data) {
    socket.emit('newData', data);
  })
})