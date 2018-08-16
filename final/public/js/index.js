var device, deviceName, btn;
var tabs = [];

function getObject(value, text) {
  device = value;
}

function addTab() {
  var index = $('.nav-tabs li').length + 1;
  var unspacedDeviceName = device.replace(/\s/g, '');
  var deviceName_en = $("#" + unspacedDeviceName + "en").val();
  var deviceName_zh = $("#" + unspacedDeviceName + "zh").val();
  // Remove the space in the device name to find tab content with
  // corresponding html id.
  // If a tab of selected device had not been created,
  // Create a tab for each language, and link it to the tab content.
  if (!tabs.includes(device) ) {
    $(".components").append("<li><a class=\"tabtext lang zh\" href=\"/device?selected=" + device + "\" id=\"#tabname" + unspacedDeviceName + "\">" + deviceName_zh + "</a></li>");
    $(".components").append("<li><a class=\"tabtext lang en\" href=\"/device?selected=" + device + "\" id=\"#tabname" + unspacedDeviceName + "\">" + deviceName_en + "</a></li>");
    // Use changeLanguage function to hide tabs of other languages.
    changeLanguage(lang);
    // Add the device to the tabs array
    // to indicate a tab for this device is created.
    tabs.push(device);
  }
}


$(document).ready(function() {

  console.log(document.cookie);
  $('body').on('click', '#create_me', function() {
    // If the device name is not defined but an device is selected
    if (device == undefined) {
      if ($('#device  option:selected').text() == "Select device") {
        alert("Please select a Device");
      } else {
        device = $('#device  option:selected').val();
      }
    }
    addTab();
  });

  $('body').on('click', '#create_me-zh', function() {
    // If the device name is not defined but an device is selected
    if (deviceName == undefined) {
      if ($('#device-zh  option:selected').text() == "Select device") {
        alert("Please select a Device");
      } else {
        device = $('#device-zh  option:selected').val();
        deviceName = $('#device-zh  option:selected').text();
      }
    }
    addTab();
  });
});
