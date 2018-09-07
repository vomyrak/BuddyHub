# Accessibility Settings for BuddyHub

### Web UI
#### 1. Stylesheet Switcher
This provides an alternate theme for the user to choose from. In between your `<head>` tags in the `.ejs` file, link a second stylesheet (`style2.css`). Duplicate your existing stylesheet and change the stylings to create `style2.css`.

```html
<link rel="stylesheet" title="default" type="text/css" href="./css/style.css" />
<link rel="alternate stylesheet" type="text/css" href="./css/style2.css" title="alternate" />

```

Create buttons into your settings modal box  to allow selection.

```html
<button class="settingsbtn" onclick=" setActiveStyleSheet('default'); return false;">Light</button> | 
<button class="settingsbtn" onclick=" setActiveStyleSheet('alternate'); return false;">Dark</button>
```

Create a `stylesheetswitcher.js` file and add the following code. Currently, cookies are being used to store the data when a stylesheet is being selected.

```javascript
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

var cookie = readCookie("style");
var title = cookie ? cookie : getPreferredStyleSheet();
setActiveStyleSheet(title);
```


#### 2. Button shake
Allow users to enable a visual feedback when clicking on buttons. In between your `<head>` tags in the `.ejs` file, link the buttonshake stylesheet (`buttonshake.css`) as the **_first_** stylesheet.

```html
<link rel="stylesheet" href="./css/buttonshake.css" />
```

The `buttonshake.css` file should have the following code.
```css
button:hover {
  animation: shake 2s;
  animation-iteration-count: infinite;
}

@keyframes shake {
  0% { transform: translate(1px, 1px) rotate(0deg); }
  10% { transform: translate(-1px, -2px) rotate(-1deg); }
  20% { transform: translate(-3px, 0px) rotate(1deg); }
  30% { transform: translate(3px, 2px) rotate(0deg); }
  40% { transform: translate(1px, -1px) rotate(1deg); }
  50% { transform: translate(-1px, 2px) rotate(-1deg); }
  60% { transform: translate(-3px, 1px) rotate(0deg); }
  70% { transform: translate(3px, 1px) rotate(-1deg); }
  80% { transform: translate(-1px, -1px) rotate(1deg); }
  90% { transform: translate(1px, 2px) rotate(0deg); }
  100% { transform: translate(1px, -2px) rotate(-1deg); }
}

```
Create buttons into your settings modal box  to allow selection.

```html
<button class="settingsbtn" id="shake">Shake - No</button> 
```

Create a `buttonshake.js` file and add the following code. As stated above, the stylesheet variable stores the data of the first stylesheet (`document.styleSheets[0]`) of the `.ejs` file.

```javascript
var stylesheet = document.styleSheets[0];


$("#shake").click( function (){
    if(stylesheet.disabled === false) {
          stylesheet.disabled = true; 
        $("#shake").text("Shake - No");
}
    else {
     stylesheet.disabled = false; 
        $("#shake").text("Shake - Yes");
}
    }
  );
```

#### 3. Font size
Allow users to increase or decrease the overall font size of the page. 

Create buttons into your settings modal box  to allow selection.

```html
<button class="settingsbtn" onclick="resizeText(1)" id="plustext">+</button> |
<button class="settingsbtn" onclick="resizeText(-1)" id="minustext">-</button>
```

A `resizetext.js` file should include the following code. 

```javascript
function resizeText(multiplier) {
  if (document.body.style.fontSize == "") {
    document.body.style.fontSize = "1.0em";
  }
  document.body.style.fontSize = parseFloat(document.body.style.fontSize) + (multiplier * 0.5) + "em";
}


```

#### 4. Font for users with dyslexia
The Lexie Readable font was designed with accessibility and legibility in mind, an attempt to capture the strength and clarity of Comic Sans without the comic book associations. Add the following into css files.
```css
@font-face {
    font-family: 'lexie_readableregular';
    src: url('fonts/lexiereadable-regular-webfont.eot');
    src: url('fonts/lexiereadable-regular-webfont.eot?#iefix') format('embedded-opentype'),
         url('fonts/lexiereadable-regular-webfont.woff2') format('woff2'),
         url('fonts/lexiereadable-regular-webfont.woff') format('woff'),
         url('fonts/lexiereadable-regular-webfont.ttf') format('truetype'),
         url('fonts/lexiereadable-regular-webfont.svg#lexie_readableregular') format('svg');
    font-weight: normal;
    font-style: normal;
```

Add the following [files](https://github.com/vomyrak/WSUROP2018/tree/master/web/public/css/fonts) into your css/fonts folder. 

#### 5. Sound
Enable user to choose between having an audio feedback or not when clicking on `<a>, <button>` and  `<select>` tags. The following code also consists of 5 other choices of sound that the user can choose from.

```javascript
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


$("#mutefxn").click( function (){
    if(audio.muted===false ) {
          audio.muted=true;
        $("#mutefxn").text("Mute sound - Yes");
    } else {
     audio.muted=false;
        $("#mutefxn").text("Mute sound - No");
    }
  });
```
- Sound enabler

```html
<button class="settingsbtn" id="mutefxn" >Mute sound - No</button> 
```
- Sound choices
```html
<button class="settingsbtn" id="sound1">1</button> |
<button class="settingsbtn" id="sound2">2</button> |
<button class="settingsbtn" id="sound3">3</button> |
<button class="settingsbtn" id="sound4">4</button> |
<button class="settingsbtn" id="sound5">5</button>
```

#### 6. Cookies
The following code saves the selection of all the settings in cookies.
```javascript
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

```
