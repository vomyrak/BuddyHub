<<<<<<< HEAD
var callback_map = {"decode_and_play_audio": decodeAndPlay};

function sendRequest(button) {
  var http_method = document.getElementById("http_method" + button.value).value;
  var link = document.getElementById("link" + button.value).value;
  var data = document.getElementById("data" + button.value).value;
  var headers = JSON.parse(document.getElementById("headers" + button.value).value);
  var callback_function = document.getElementById("callback_function" + button.value).value;
  var text_input = document.getElementById("text_input_field" + button.value).value;

  // Add the text input to data if required
  if (text_input != '') {
    var input = document.getElementById("textbox" + button.value).value;
    var json = JSON.parse(data);
    var strarr = text_input.split(".");
    console.log(text_input.split("."));
    if (!text_input.includes(".")) {
      json[text_input] = input
    } else if (strarr.length = 2) {
      json[strarr[0]][strarr[1]] = input;
    }
    data = JSON.stringify(json);
    console.log(data);
  }

  // Strat a http request
  var xhttp = new XMLHttpRequest();
  xhttp.onreadystatechange = function() {
    if (this.readyState == 4 && this.status == 200) {
      // If the http request is successful
      console.log("success");
      // If there is a callback function,
      // find the coresponding function in the callback_map and apply.
      if (callback_function != '') {
        // console.log(this.responseText);
        console.log(this.responseText);
        callback_map[callback_function](this.response);
      }
    } else {
      console.log(this.responseText);
    }
  };
  // Open Connection
  xhttp.open(http_method, link, true);
  // Set headers of the http request
  for(var i = 0; i < headers.length; i++) {
    var obj = headers[i];
    xhttp.setRequestHeader(obj.header_name, obj.header_field);
  }
  // Send the http request with the data
  xhttp.send(data);
}

function decodeAndPlay(encodedMP3) {
  // Play the audio on the server
  var audio = new Audio('/synthesize-text-audio.mp3');
  audio.play();
}
=======
var callback_map = {"decode_and_play_audio": decodeAndPlay};

function sendRequest(button) {
  var http_method = document.getElementById("http_method" + button.value).value;
  var link = document.getElementById("link" + button.value).value;
  var data = document.getElementById("data" + button.value).value;
  var headers = JSON.parse(document.getElementById("headers" + button.value).value);
  var callback_function = document.getElementById("callback_function" + button.value).value;
  var text_input = document.getElementById("text_input_field" + button.value).value;

  // Add the text input to data if required
  if (text_input != '') {
    var input = document.getElementById("textbox" + button.value).value;
    var json = JSON.parse(data);
    var strarr = text_input.split(".");
    console.log(text_input.split("."));
    if (!text_input.includes(".")) {
      json[text_input] = input
    } else if (strarr.length = 2) {
      json[strarr[0]][strarr[1]] = input;
    }
    data = JSON.stringify(json);
    console.log(data);
  }

  // Strat a http request
  var xhttp = new XMLHttpRequest();
  xhttp.onreadystatechange = function() {
    if (this.readyState == 4 && this.status == 200) {
      // If the http request is successful
      console.log("success");
      // If there is a callback function,
      // find the coresponding function in the callback_map and apply.
      if (callback_function != '') {
        // console.log(this.responseText);
        console.log(this.responseText);
        callback_map[callback_function](this.response);
      }
    } else {
      console.log(this.responseText);
    }
  };
  // Open Connection
  xhttp.open(http_method, link, true);
  // Set headers of the http request
  for(var i = 0; i < headers.length; i++) {
    var obj = headers[i];
    xhttp.setRequestHeader(obj.header_name, obj.header_field);
  }
  // Send the http request with the data
  xhttp.send(data);
}

function decodeAndPlay(encodedMP3) {
  // Play the audio on the server
  var audio = new Audio('/synthesize-text-audio.mp3');
  audio.play();
}
>>>>>>> prototype-vk
