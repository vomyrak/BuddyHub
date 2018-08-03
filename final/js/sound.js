var audio = document.getElementById("play");

function playSound () {
    audio.play();
    
}


function enableMute() { 
    audio.muted = true;
} 

function disableMute() { 
    audio.muted = false;
} 

