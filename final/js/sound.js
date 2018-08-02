function playSound () {
    var audio = document.getElementById("play").play();
    
}


var audio = document.getElementById("play");
function enableMute() { 
    audio.muted = true;
} 

function disableMute() { 
    audio.muted = false;
} 

