var audio, audiolink;


$("button, a, select").not("#sound1, #sound2, #sound3, #sound4, #sound5").on('click', function() {
  audio.play();
});

$("#sound1").click(function(){
    audiolink = 'http://www.soundjay.com/button/beep-07.wav';
    audio = new Audio('http://www.soundjay.com/button/beep-07.wav');
    audio.play();
});

$("#sound2").click(function(){
    audiolink = 'https://www.soundjay.com/button/button-30.wav';
    audio = new Audio('https://www.soundjay.com/button/button-30.wav');
    audio.play();
});

$("#sound3").click(function(){
    audiolink = 'https://www.soundjay.com/button/button-14.wav';
    audio = new Audio('https://www.soundjay.com/button/button-14.wav');
    audio.play();
});

$("#sound4").click(function(){
    audiolink = 'https://www.soundjay.com/button/button-35.wav';
    audio = new Audio('https://www.soundjay.com/button/button-35.wav');
    audio.play();
});

$("#sound5").click(function(){
    audiolink = 'https://www.soundjay.com/button/button-21.wav';
    audio = new Audio('https://www.soundjay.com/button/button-21.wav');
    audio.play();
});


function enableMute() {
  audio.muted = true;
}

function disableMute() {
  audio.muted = false;
}
