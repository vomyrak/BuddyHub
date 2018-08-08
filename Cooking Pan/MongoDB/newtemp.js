var mongoose = require('mongoose');

mongoose.connect('mongodb://cooking:wsurop18@ds223268.mlab.com:23268/wsurop_cooking');
var temp = 90;

var tempSchema = new mongoose.Schema({
  temperature: Number,
  units: String
});

var Reading = mongoose.model("Temperature", tempSchema);

function pushData() {
	console.log("Temp: " + temp);
	Reading.create({
		temperature: temp,
		units: "Celsius"
	}, function (err, temperature){
		if(err){
			console.log("error");
		} else {
			console.log(temperature);
		}
	});
	temp++;
}

setInterval(pushData, 2000);