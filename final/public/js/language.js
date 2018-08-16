var username;
var lang = "en";

function changeLanguage(code) {
  // Hide all elements involves language display.
  $('.lang').hide();
  // Show the elements with the given language code.
  $('.' + code).show();
  // Change greeting on side bar
  if (code == "en") {
    $("h2").html("<span>Hi</span> " + username + "!");
  } else if (code == "zh") {
    $("h2").html("<span>你好</span> " + username + "!");
  }
  // Store the language code.
  lang = code;
}
