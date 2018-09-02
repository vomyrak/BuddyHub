function createCookie(name, value, days) {
  if (days) {
    var date = new Date();
    date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
    var expires = "; expires=" + date.toGMTString();
  } else expires = "";
  document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
  var nameEQ = name + "=";
  var ca = document.cookie.split(';');
  for (var i = 0; i < ca.length; i++) {
    var c = ca[i];
    while (c.charAt(0) == ' ') c = c.substring(1, c.length);
    if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
  }
  return null;
}

window.onload = function(e) {
  var cookiestyle = readCookie("style");
  var title = cookiestyle ? cookiestyle : getPreferredStyleSheet();
  setActiveStyleSheet(title);

  var cookietab = readCookie("tabs");
  tabs = cookietab ? JSON.parse(cookietab) : [];

  var cookiesound = readCookie("sound");
  audiolink = cookiesound ? cookiesound : 'http://www.soundjay.com/button/beep-07.wav';
  audio = new Audio(audiolink);
  var cookiemuted = readCookie("muted");
  audio.muted = cookiemuted ? JSON.parse(cookiemuted) : false;
  var mutebuttontext = audio.muted ? "Mute sound - Yes" : "Mute sound - No";
  $("#mutefxn").text(mutebuttontext);


  var cookieshake = readCookie("shake");
  stylesheet.disabled = cookieshake ? JSON.parse(cookieshake) : false;
  var shakebuttontext = stylesheet.disabled ? "Shake - No" : "Shake - Yes";
  $("#shake").text(shakebuttontext);


  var cookiefontsize = readCookie("fontsize");
  document.body.style.fontSize = cookiefontsize ? cookiefontsize : "1.0em";

  for (i = 0; i < tabs.length; i++) {
    var deviceName = tabs[i];
    var unspacedDeviceName = deviceName.replace(/\s/g, '');
    $(".components").append("<li><a  onclick=\"playSound()\" class=\"tabtext\" href=\"/device?selected=" + deviceName + "\" id=\"#tabname" + unspacedDeviceName + "\">" + deviceName + "</a></li>");
  }
}

window.onunload = function(e) {
  var title = getActiveStyleSheet();
  createCookie("style", title, 365);
  document.cookie = "tabs=" + JSON.stringify(tabs) + "";
  document.cookie = "sound=" + audiolink + "";
  document.cookie = "fontsize=" + document.body.style.fontSize + "";
  document.cookie = "muted=" + audio.muted + "";
  document.cookie = "shake=" + stylesheet.disabled + ""; 
   
}
