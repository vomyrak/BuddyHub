var mongoose = require('mongoose');
var five = require('johnny-five');

mongoose.connect('mongodb://cooking:wsurop18@ds223268.mlab.com:23268/wsurop_cooking');
var board = new five.Board();
var temperature;

var tempSchema = new mongoose.Schema({
  temperature: Number,
  units: String
});

var Reading = mongoose.model("Temperature", tempSchema);

function pushData(tempData) {
	Reading.create({
		temperature: tempData,
		units: "Celsius"
	}), function (err, temperature){
		if(err){
			console.log("error");
		} else {
			console.log(tempData);
		}
	}
}

board.on('ready', function(){
  temperature = new five.Thermometer({
    controller: 'TMP36',
    pin: 'A0'
  });

  temperature.on('change', function() {
  	console.log(temperature.C);
    pushData(temperature.C);
  });
});