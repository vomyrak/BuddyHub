function sendRequest(button) {
  var http_method = document.getElementById("http_method" + button.value).value;
  var link = document.getElementById("link" + button.value).value;
  var data = document.getElementById("data" + button.value).value;
  var headers = document.getElementById("headers" + button.value).value;

  var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function() {
      if (this.readyState == 4 && this.status == 200) {
        console.log("success");
      }
    };
    xhttp.open(http_method, link, true);
    xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhttp.send(data);
}
