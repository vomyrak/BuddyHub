$("input[type='text']").keypress(function(event) {
  if (event.which === 13) {
    var name = $(this).val();
    $(this).val("");
    $("h2").html("<span>Hi</span> " + name + "!");
  }
});
