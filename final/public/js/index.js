var deviceName, btn, methods;

function getObject(value) {
  deviceName = value;
}


$(document).ready(function() {
  $('body').on('click', '#create_me', function() {
    var index = $('.nav-tabs li').length + 1;
    var unspacedDeviceName = deviceName.replace(/\s/g, '');
    // Create the tab when user requires it, and link it to the tab content
    $("#home-tab").append("<li class=\"active\"><a href=\"#tab" + unspacedDeviceName + "\">" + deviceName + "</a></li>");

    $('a[href="#tab' + unspacedDeviceName + '"]').click();
  })

  $('.nav-tabs').on('click', 'li > a', function(event) {
    event.preventDefault(); //stop browser to take action for clicked anchor

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
