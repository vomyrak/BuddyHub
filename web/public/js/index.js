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
    if (!tabs.includes(deviceName)) {
      $(".components").append("<li><a onclick=\"playSound()\" class=\"tabtext\" href=\"/device?selected=" + deviceName + "\" id=\"#tabname" + unspacedDeviceName + "\">" + deviceName + "</a></li>");
      tabs.push(deviceName);
    }
  })
});

