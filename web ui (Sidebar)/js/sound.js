var audio = new Audio('http://www.soundjay.com/button/beep-07.wav');

var sound1 = function(){
    audio = new Audio('http://www.soundjay.com/button/beep-07.wav');
}

var sound2 = function(){
    audio = new Audio('https://www.soundjay.com/button/button-30.wav');
}

var sound3 = function(){
    audio = new Audio('https://www.soundjay.com/button/button-14.wav');
}

var sound4 = function(){
    audio = new Audio('https://www.soundjay.com/button/button-35.wav');
}

var sound5 = function(){
    audio = new Audio('https://www.soundjay.com/button/button-21.wav');
}

function playSound () {
    audio.play();
    
}


function enableMute() { 
    audio.muted = true;
} 

function disableMute() { 
    audio.muted = false;
} 

