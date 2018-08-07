var socket;
socket = io.connect("http://localhost:3000/");
socket.on("connect", function() {
	this.on("newTemp", (data) => {
		$("#temperature-reading").text(data);
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
	    setInterval($('#countdownExample .values').toggleClass("alert"), 1000);
	});
}
