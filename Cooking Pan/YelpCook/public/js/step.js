$("#start-step").on("click", function() {
	var time = ($(this).attr("value"));
	getTimer(time * 60);
})

var socket;
var reading;
var tempLimit = Number($("#temperature-reading").attr("value"));
console.log(tempLimit);

socket = io.connect("http://localhost:3000/");
socket.on("connect", function() {
	this.on("newTemp", (data) => {
		$("#temperature-reading").text(data);
		reading = data;
		if(reading > tempLimit) {
			$("#temperature-reading").toggleClass("alert");
		}
	})
})

function getTimer(stepTime) {
	var timer = new Timer();
	timer.start({countdown: true, startValues: {seconds: stepTime}});
	$('#countdownExample .values').html(timer.getTimeValues().toString());
	timer.addEventListener('secondsUpdated', function (e) {
	    $('#countdownExample .values').html(timer.getTimeValues().toString());
	});
	timer.addEventListener('targetAchieved', function (e) {
	    setInterval(function() {
	    	$('#countdownExample .values').toggleClass("alert");
	    }, 1000);
	});
}
