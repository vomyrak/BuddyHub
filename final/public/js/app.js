
// Create UCdevices object
var UCdevices = {
    RobotArm: ["Up", "Down", "Left", "Right"],
    AlexaCommands: ["Weather", "Uber", "Read News"],
    RoomLights: ["On", "Off"]
};

var deviceName, getfunc, btn;
function getObject(value){

    deviceName = value;
    getfunc = UCdevices[deviceName];
//
//    for (var i=0;i<UCdevices[deviceName].length;i++){
//
//        var objbtn = UCdevices[deviceName][i];
//        btn.push(objbtn);
//    }
//
////Remove last child
// btn.pop();
//
}


$(document).ready(function() {
  $('body').on('click','#create_me',function(){
    var index = $('.nav-tabs li').length+1;
    $("#home-tab").append("<li class=\"active\"><a href=\"#tab" + index + "\">" + deviceName + "</a></li>");
    $('.ui-page').append("<section id=\"tab" + index + "\" class=\"tab-content active\" />" );
    $.each(getfunc, (key, value) =>{
      $('#tab' + index).append("<button type=\"button\" class=\"ui-btn ui-corner-all ui-shadow ui-btn-b ui-btn-icon-left ui-icon-check\">"+value+"</button>" );
    })

    $('a[href="#tab'+index+'"]').click();
  })

  $('.nav-tabs').on('click','li > a',function(event){
    event.preventDefault();//stop browser to take action for clicked anchor

    //get displaying tab content jQuery selector
    var active_tab_selector = $('.nav-tabs > li.active > a').attr('href');

    //find actived navigation and remove 'active' css
    var actived_nav = $('.nav-tabs > li.active');
    actived_nav.removeClass('active');

    //add 'active' css into clicked navigation
    $(this).parents('li').addClass('active');

    //hide displaying tab content
    $(active_tab_selector).removeClass('active');
    $(active_tab_selector).addClass('hide');

    //show target tab content
    var target_tab_selector = $(this).attr('href');

    $(target_tab_selector).removeClass('hide');
    $(target_tab_selector).addClass('active');
  });
});



//
//var selectedFunc = [];
//function displayFunc(value) {
//
//    //Clean id=functions
//    while (document.getElementById("functions").firstChild) {
//        document.getElementById("functions").removeChild(document.getElementById("functions").firstChild);
//    }
//
//    //Clean id=demo
//    document.getElementById("demo").innerHTML = "";
//
//    var deviceFunc;
//    //Populate id=functions
//	for (deviceFunc in UCdevices[value])
//    {
//        var node = document.createElement("LI"); // Create a <li> node
//        var textnode = document.createTextNode(UCdevices[value][deviceFunc]); // Create a text node
//        node.appendChild(textnode); // Append the text to <li>
//        document.getElementById("functions").appendChild(node); // Append <li> to <ul> with id="functions"
//    }
//
//    //Get selected functions
//    function getEventTarget(e) {
//        e = e || window.event;
//        return e.target || e.srcElement;
//    }
//
//    var ul = document.getElementById('functions');
//    var array = [];
//    var x=0;
//    ul.onclick = function(event) {
//
//        var e = [];
//        var target = getEventTarget(event);
//
//        array[x] =target.innerHTML;
//
//        target.style.fontWeight="bold";
//
//        for (var i=0;i<x;i++){
//            if(array[x] == array[i] && x>0){
//                array.splice(i,1); //Delete reselected node
//                array.pop(); //Delete duplicate
//                x = array.length-1;
//                target.style.fontWeight = "normal";
//            }
//        }
//
//        x++;
//
//        for (var y=0; y<array.length; y++){
//        e = e + array[y] + "/";
//        }
//
//        selectedFunc = e.split("/");
//
//        //Print selected functions
//        // document.getElementById("demo").innerHTML = selectedFunc;
//    }
//}
//
