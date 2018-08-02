function resizeText(multiplier) {
  if (document.getElementById("myBody").style.fontSize == "") {
    document.getElementById("myBody").style.fontSize = "1.0em";
  }
  document.getElementById("myBody").style.fontSize = parseFloat(document.getElementById("myBody").style.fontSize) + (multiplier * 0.5) + "em";
}

