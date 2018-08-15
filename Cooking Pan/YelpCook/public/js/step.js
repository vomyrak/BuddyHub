$("#start-step").on("click", function() {
	var time = ($(this).attr("value"));
	getTimer(time * 60);
})

var socket;
var reading = 0;
var tempLimit = Number($("#temperature-reading").attr("value"));
var tempPlayed = false;
var tempAudio = new SpeechSynthesisUtterance("The pan is too hot, turn the heat down!");
var tempAudio2 = new SpeechSynthesisUtterance("The pan is not yet hot enough, please keep waiting");
var tempAudio3 = new SpeechSynthesisUtterance("The required temperature has been reached");

socket = io.connect("http://localhost:3000");
socket.on("connect", function() {
	this.on("newTemp", (data) => {
		if (data !== reading) {
			reading = data;
		}
		$("#temperature-reading").text(reading);
		if(reading > tempLimit) {
			$("#temperature-reading").toggleClass("alert");
			if (!tempPlayed) {
				window.speechSynthesis.speak(tempAudio);
			//	tempPlayed = true;
			}
		}
		else if (reading < tempLimit) {	
		//	$("#temperature-reading").toggleClass("alert");
			if (!tempPlayed) {
				window.speechSynthesis.speak(tempAudio2);
			//	tempPlayed = true;
			}
				else {
		//	$("#temperature-reading").removeClass("alert");
				window.speechSynthesis.speak(tempAudio3);
			//  tempPlayed = true;
			}

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
		var msg = new SpeechSynthesisUtterance("Well done! Time's up, go to the next step!");
		window.speechSynthesis.speak(msg);
	    setInterval(function() {
	    	$('#countdownExample .values').toggleClass("alert");
	    }, 1000);
	});
}