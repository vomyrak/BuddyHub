$("input[type='text']").keypress(function(event) {
  // Change the name display on the sidebar when the enter key is pressed.
  if (event.which === 13) {
    username = $(this).val();
    $(this).val("");
    if (lang == "en") {
      $("h2").html("<span>Hi</span> " + username + "!");
    } else if (lang == "zh") {
      $("h2").html("<span>你好</span> " + username + "!");
    }
  }
});
