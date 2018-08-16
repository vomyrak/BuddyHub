var audio, audiolink;


$("button, a, select").not("#sound1, #sound2, #sound3, #sound4, #sound5").on('click', function() {
  audio.play();
});

$("#sound1").click(function() {
  audiolink = 'http://www.soundjay.com/button/beep-07.wav';
  audio = new Audio('http://www.soundjay.com/button/beep-07.wav');
  audio.play();
});

$("#sound2").click(function() {
  audiolink = 'https://www.soundjay.com/button/button-30.wav';
  audio = new Audio('https://www.soundjay.com/button/button-30.wav');
  audio.play();
});

$("#sound3").click(function() {
  audiolink = 'https://www.soundjay.com/button/button-14.wav';
  audio = new Audio('https://www.soundjay.com/button/button-14.wav');
  audio.play();
});

$("#sound4").click(function() {
  audiolink = 'https://www.soundjay.com/button/button-35.wav';
  audio = new Audio('https://www.soundjay.com/button/button-35.wav');
  audio.play();
});

$("#sound5").click(function() {
  audiolink = 'https://www.soundjay.com/button/button-21.wav';
  audio = new Audio('https://www.soundjay.com/button/button-21.wav');
  audio.play();
});


$("#mutefxn").click(function() {
  if (audio.muted === false) {
    audio.muted = true;
    $("#mutefxn").text("Mute sound - Yes");
  } else {
    audio.muted = false;
    $("#mutefxn").text("Mute sound - No");
  }
});

$("#mutefxn-zh").click(function() {
  if (audio.muted === false) {
    audio.muted = true;
    $("#mutefxn-zh").text("靜音模式 - 啓動");
  } else {
    audio.muted = false;
    $("#mutefxn-zh").text("靜音模式 - 關閉");
  }
});
