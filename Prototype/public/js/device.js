function sendRequest(button) {
  var http_method = document.getElementById("http_method" + button.value).value;
  var link = document.getElementById("link" + button.value).value;
  var data = document.getElementById("data" + button.value).value;
  var headers = document.getElementById("headers" + button.value).value;
  var callback_function = document.getElementById("callback_function" + button.value).value;

  // Strat a http request
  var xhttp = new XMLHttpRequest();
  xhttp.onreadystatechange = function() {
    if (this.readyState == 4 && this.status == 200) {
      // If the http request is successful
      console.log("success");
    }
  };
  // Open Connection
  xhttp.open(http_method, link, true);
  // Set headers of the http request
  xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
  // Send the http request with the data
  xhttp.send(data);
}
