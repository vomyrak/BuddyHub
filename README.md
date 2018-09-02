# BuddyHub
### The Universal Controller Platform for Assistive Technology
**BuddyHub is a free and open-source platform that leverages the Internet of Things (IoT) to allow people with different disabilities to control various output devices in their environment via accesible inputs through a simple-to-navigate and user-friendly interface.**

## About
BuddyHub was born out of a joint collaboration between Imperial College London and Wooden Spoon: The Children's Charity of Rugby, the latter of whom invests heavily in assistive technologies and aims to promote their development and make them more accessible to end users.

BuddyHub aims to be an affordable, end-to-end solution for people with disabilities to utilize smart devices in their surroundings through a variety of accessible input devices that caters to their individual needs in a plug-and-play fashion. This is all done through the user interface of BuddyHub, which was designed with a special needs audience in mind. For instance, a user with limited motion capabilities could control smart appliances, robotic arms and other forms of hardware they might need, as long as they are integrated into BuddyHub's system.

This project also aims to promote the development of assistive technology by providing a platform for developers and companies to build on and integrate their products with. The ultimate goal is to make such technologies more affordable as well as integrate them with the Internet of Things for the benefit of end users. As more devices become interconnected, this universal controller platform can act as a central hub of control for these devices, giving users with various disabilities the ability to utilize a wide range of devices that cater to their individual needs.

The team that developed the platform focused on establishing the groundwork and software architecture to achieve the above goals. The first iteration of the product demonstrates the viability and usefulness of such a universal and accessible software platform that can be used for controlling technologies in a familiar and easy manner by people with disabilities. Due to time constraints, not all features we wanted to implement / issues we wanted to fix were resolved; the team hopes that a community of developers / other students could collaborate and improve on what has been done so far.

## Installation and Setup
BuddyHub currently has web and desktop (Windows) versions, both of which are functional and augment the intent of having a user-friendly and accessible UI for end users.

To access the web version of BuddyHub, please visit [BuddyHub (Web)](http://wsurop18-universal-controller.herokuapp.com/).

To install the Windows version, follow the steps below:
* [Download zip file from here.](http://wsurop18-universal-controller.herokuapp.com/buddyhub-2018-08-31.zip) Extract the zip file to desired location. Among all other files, there should be two separate executables that include the server and the desktop UI.
* To run BuddyHub, run server.exe followed by UCUI.exe to launch the UI.

## Guide to BuddyHub's UI
Detailed below are the accessibility settings that we have implemented on the web and desktop versions of BuddyHub's UI. This should assist future developers should they wish to edit current / add new accessibility features.
### BuddyHub (Web)
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
### BuddyHub (Desktop)

## List of Compatible Devices (Updated: 30/8/18)
To demonstrate the universal plug-and-play capabilities of the prototype that the team developed, we cross-tested the universal controller platform with the following input and output devices:

#### Input Devices:
(_Any input devices that are natively mapped to a PC's mouse and keyboard inputs can be used to interact with BuddyHub's UI._)
* Tobii Eye Tracker 4C
* Most accesible joysticks (we used _Slimline Joystick_)
* Buddy buttons

#### Output Devices:
* Web-based Voice Synthesizer for Alexa (allows a user with limited vocal capabilities to interact with Alexa through BuddyHub)
* Philips Hue Go ([API Documentation](https://www.developers.meethue.com/documentation/getting-started))
* Robotic arm (we wrote a **C# web API and wrapper** to interact with the robotic arm's native C# firmware)

## Add a Device (Developer Documentation)
Wish to integrate a device with BuddyHub?

#### Requirements:
* RESTful Web API that accepts HTTP requests (for devices with a TCP/UDP connection)
* C# wrapper that implements our IDevice interface (for USB devices)
* Easy-to-understand device names and descriptions for hardware functionality
* Supports API authentication via OAuth 2.0 and hardware authentication (_to be implemented_)

#### Guide:
* Visit [BuddyHub@Developers](https://developers-buddyhub.herokuapp.com/) to add a new device
* Choose to add a device with either a RESTful API or a C# interface
* Fill up the form with user-friendly names and descriptions (these will be what users see in the main BuddyHub UI)
* Specify the HTTP methods and resources that BuddyHub will use to interact with your device
* Select if a text input interface is required for your device
* Submit your device for approval by the admin team and we'll get back to you as soon as possible

#### Device suggestions:
Users can also suggest devices to be added to BuddyHub by clicking on the ["Suggest Device"](https://wsurop18-universal-controller.herokuapp.com/contact) button on BuddyHub's UI. Simply just fill in the form and the details will be sent for a review. The admin team will look into adding the suggested devices on a regular basis.

## Future Improvements
Wish to contribute to BuddyHub? Fork the repo and follow the installation guide as shown [here](https://github.com/vomyrak/WSUROP2018/blob/master/web/README.md).

List of features we wish to implement but have not:
#### Accesibility
* One and two-switch modes for navigation
* Tremor filtering for push buttons
* Adjustable wait time for click-on-hover option
* Accesibility options for the visually impaired such as screen reader support
* Implementing clear and intuitive button icons for the web interface
* Ability to save user settings and preferences
* Ability to add futher language options
* Integration with other softwares and open APIs to contain day-to-day applications within BuddyHub's interface (e.g. instant-messaging apps, Fitbit etc.)
* Integration of an AAC (Augmentative and Alternative Communication) board alongside the current text-to-speech capability
* Setting up of macros
* Android and iOS mobile app
* More audio-visual feedback options

#### Security
* API authentication via OAuth 2.0
* Hardware authentication to register device's IP Address
* GDPR compliance to ensure user's data security

#### Development
* An interface for admins to approve and process device suggestions and submissions
* Improve on the workflow for developers to integrate new devices to BuddyHub
* CLI (Command Line Interface) tool for the integration of new devices
* Adding more devices to BuddyHub and possible integration with other smart hubs such as Apple HomeKit and Samsung SmartThings

List of issues can be found [here](https://github.com/vomyrak/WSUROP2018/issues).

## Contributors and Acknowledgements
This project was initiated and funded by Wooden Spoon: The Children's Charity of Rugby, and was developed by a team of 10 undergraduate students from Imperial College London under the supervision and guidance of _Dr. Ian Radcliffe_ from Imperial College London, Department of Bioengineering.

The team comprised of _Maksim Lavrov, May McCool, Terence Seow, Rachel Tan, Yong Shen Tan, Sung Kyun Yi, Fiona Boyce, Joan Tso, Balint Hodossy and Husheng Deng_.

The team would like to thank the following people for their contributions and advice to the team throughout the course of the project:
  
  * _Mahendran Subramanian_, _Balasundaram Kadirvelu_ and _Pavel Orlov_ from Dr. Faisal's Research Group, Imperial College London
  * _Paschal Egan_ and _Niraj Kanabar_ from Imperial College London, Department of Bioengineering
  * _Dr. Thomas Clarke_ from Imperial College London
  * _Ben Crundwell_ from Cambridge Design Partnership
  * _Barrie Ellis_ from Special Effects
  * _Charlie Danger_, _Simon Bull_, _Christian Dryden_, _Rachel Moore_, _Hélio Lourenço_ and _Diane Arthurs_ from ACE Center

## Contact
Should you have any queries about the above project or wish to find out more about how you could contribute, please kindly reach out to us at wsurop2018@gmail.com.
