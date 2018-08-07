function setActiveStyleSheet(title) {
  var i, a, main;
  for (i = 0;
    (a = document.getElementsByTagName("link")[i]); i++) {
    if (a.getAttribute("rel").indexOf("style") != -1 && a.getAttribute("title")) {
      a.disabled = true;
      if (a.getAttribute("title") == title) a.disabled = false;
    }
  }
}

function getActiveStyleSheet() {
  var i, a;
  for (i = 0;
    (a = document.getElementsByTagName("link")[i]); i++) {
    if (a.getAttribute("rel").indexOf("style") != -1 && a.getAttribute("title") && !a.disabled) return a.getAttribute("title");
  }
  return null;
}

function getPreferredStyleSheet() {
  var i, a;
  for (i = 0;
    (a = document.getElementsByTagName("link")[i]); i++) {
    if (a.getAttribute("rel").indexOf("style") != -1 &&
      a.getAttribute("rel").indexOf("alt") == -1 &&
      a.getAttribute("title")
    ) return a.getAttribute("title");
  }
  return null;
}

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

  var cookieshake = readCookie("shake");
  stylesheet.disabled = cookieshake ? JSON.parse(cookieshake) : false;

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

var cookie = readCookie("style");
var title = cookie ? cookie : getPreferredStyleSheet();
setActiveStyleSheet(title);
