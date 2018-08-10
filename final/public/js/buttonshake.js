var stylesheet = document.styleSheets[0];


$("#shake").click( function (){
    if(stylesheet.disabled === false) {
          stylesheet.disabled = true; 
        $("#shake").text("Shake - No");
}
    else {
     stylesheet.disabled = false; 
        $("#shake").text("Shake - Yes");
}
    }
  );