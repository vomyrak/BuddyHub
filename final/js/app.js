

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
    
}
    

$(document).ready(function() {
  $('body').on('click','#create_me',function(){
      
      //Error handling
    if(!deviceName){
        alert("Please select a Device");
    }else {
        var index = $('.nav-tabs li').length+1;
        $("#home-tab").append("<li class=\"active\"><a href=\"#tab" + index + "\">" + deviceName + "</a></li>");
        
        $('.ui-page').append("<section id=\"tab" + index + "\" class=\"tab-content active\" />" );
        $.each(getfunc, (key, value) =>{
        $('#tab' + index).append("<button type=\"button\" style=\"padding:30px 30px; margin: 15px auto; width:300px\" class=\"ui-btn ui-corner-all ui-shadow ui-btn-b ui-btn-icon-left ui-icon-check\">"+value+"</button>" );
    
    })
        
    $('a[href="#tab'+index+'"]').click();
       }
    
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



