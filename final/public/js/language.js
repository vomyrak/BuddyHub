var lang = "en";

function changeLanguage(code) {
  // Hide all elements involves language display.
  $('.lang').hide();
  // Show the elements with the given language code.
  $('.' + code).show();
  // Store the language code.
  lang = code;
}
