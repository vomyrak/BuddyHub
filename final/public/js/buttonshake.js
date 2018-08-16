var stylesheet = document.styleSheets[0];


$("#shake").click(function() {
  if (stylesheet.disabled === false) {
    stylesheet.disabled = true;
    $("#shake").text("Shake - No");
  } else {
    stylesheet.disabled = false;
    $("#shake").text("Shake - Yes");
  }
});

$("#shake-zh").click(function() {
  if (stylesheet.disabled === false) {
    stylesheet.disabled = true;
    $("#shake-zh").text("震動模式 - 關閉");
  } else {
    stylesheet.disabled = false;
    $("#shake-zh").text("震動模式 - 啓動");
  }
});
