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
  var mutebuttontextzh = audio.muted ? "靜音模式 - 啓動" : "靜音模式 - 關閉";
  $("#mutefxn-zh").text(mutebuttontextzh);

  var cookieshake = readCookie("shake");
  stylesheet.disabled = cookieshake ? JSON.parse(cookieshake) : false;
  var shakebuttontext = stylesheet.disabled ? "Shake - No" : "Shake - Yes";
  $("#shake").text(shakebuttontext);
  var shakebuttontextzh = stylesheet.disabled ? "震動模式 - 關閉" : "震動模式 - 啓動";
  $("#shake-zh").text(shakebuttontextzh);


  var cookiefontsize = readCookie("fontsize");
  document.body.style.fontSize = cookiefontsize ? cookiefontsize : "1.0em";

  var cookielang = readCookie("lang");
  var lang = cookielang ? cookielang : "en";
  changeLanguage(lang);

  var cookiename = readCookie("name");
  var greeting;
  if (lang == "en") {
    greeting = cookiename ? "<span>Hi</span> " + cookiename + "!" : "<h2>Welcome!</h2>";
  } else if (lang == "zh") {
    greeting = cookiename ? "<span>你好</span> " + cookiename + "!" : "<h2>歡迎!</h2>";
  }
  $("h2").html(greeting);

  for (i = 0; i < tabs.length; i++) {
    // Create tabs for eah device stored in tabs array
    var device = tabs[i];
    var unspacedDeviceName = device.replace(/\s/g, '');
    var deviceName_en = $("#" + unspacedDeviceName + "en").val();
    var deviceName_zh = $("#" + unspacedDeviceName + "zh").val();
    $(".components").append("<li><a class=\"tabtext lang zh\" href=\"/device?selected=" + device + "\" id=\"#tabname" + unspacedDeviceName + "\">" + deviceName_zh + "</a></li>");
    $(".components").append("<li><a class=\"tabtext lang en\" href=\"/device?selected=" + device + "\" id=\"#tabname" + unspacedDeviceName + "\">" + deviceName_en + "</a></li>");
    changeLanguage(lang);
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
  if (username) {
    document.cookie = "name=" + username + "";
  }
  document.cookie = "lang=" + lang + "";
}
