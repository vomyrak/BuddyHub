var deviceName, btn;
var tabs = [];

function getObject(value) {
  deviceName = value;
}


$(document).ready(function() {
  console.log(document.cookie);
  $('body').on('click', '#create_me', function() {
    // If the device name is not defined but an device is selected
    if (deviceName == undefined) {
      if ($('#device  option:selected').text() == "Select") {
        alert("Please select a Device");
      } else {
        deviceName = $('#device  option:selected').text();
      }
    }
    var index = $('.nav-tabs li').length + 1;
    var unspacedDeviceName = deviceName.replace(/\s/g, '');
    // Remove the space in the device name to find tab content with
    // corresponding html id.
    // If a tab of selected device had not been created,
    // Create the tab, and link it to the tab content
    if (!tabs.includes(unspacedDeviceName)) {
      $("#mCSB_1_container").append("<ul class=\"list-unstyled components tabs\"><li class=\"active\"><a class=\"tabtext\" href=\"/device?selected=" + deviceName + "\" id=\"#tabname" + unspacedDeviceName + "\">" + deviceName + "</a></li></ul>");
      tabs.push(unspacedDeviceName);
    }
  })
});

window.onload = function(e) {
  var cookie = readCookie("tabs");
  tabs = cookie ? cookie : [];
  for (i = 0; i < title.length; i++) {
    $("#mCSB_1_container").append("<ul class=\"list-unstyled components tabs\"><li class=\"active\"><a class=\"tabtext\" href=\"/device?selected=" + deviceName + "\" id=\"#tabname" + unspacedDeviceName + "\">" + deviceName + "</a></li></ul>");
  }
}

window.onunload = function(e) {
    // document.cookie = "tabs="+JSON.stringify(tabs)+"";
}
