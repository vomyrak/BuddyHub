var audio, audiolink;

var sound1 = function() {
  audiolink = 'http://www.soundjay.com/button/beep-07.wav';
  audio = new Audio('http://www.soundjay.com/button/beep-07.wav');
}

var sound2 = function() {
  audiolink = 'https://www.soundjay.com/button/button-30.wav';
  audio = new Audio('https://www.soundjay.com/button/button-30.wav');
}

var sound3 = function() {
  audiolink = 'https://www.soundjay.com/button/button-14.wav';
  audio = new Audio('https://www.soundjay.com/button/button-14.wav');
}

var sound4 = function() {
  audiolink = 'https://www.soundjay.com/button/button-35.wav';
  audio = new Audio('https://www.soundjay.com/button/button-35.wav');
}

var sound5 = function() {
  audiolink = 'https://www.soundjay.com/button/button-21.wav';
  audio = new Audio('https://www.soundjay.com/button/button-21.wav');
}

function playSound1() {
  var audio1 = new Audio('http://www.soundjay.com/button/beep-07.wav');
  audio1.play();
}

function playSound2() {
  var audio1 = new Audio('https://www.soundjay.com/button/button-30.wav');
  audio1.play();
}

function playSound3() {
  var audio1 = new Audio('https://www.soundjay.com/button/button-14.wav');
  audio1.play();
}

function playSound4() {
  var audio1 = new Audio('https://www.soundjay.com/button/button-35.wav');
  audio1.play();
}

function playSound5() {
  var audio1 = new Audio('https://www.soundjay.com/button/button-21.wav');
  audio1.play();
}

$("button, a, select").click(function() {
  audio.play();
});


function enableMute() {
  audio.muted = true;
}

function disableMute() {
  audio.muted = false;
}
