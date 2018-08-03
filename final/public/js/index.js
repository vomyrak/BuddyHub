var deviceName, btn, tabs;

function getObject(value) {
  deviceName = value;
}


$(document).ready(function() {
  tabs= [];
  $('body').on('click', '#create_me', function() {
    // If the device name is not defined but an device is selected
    if (deviceName == undefined && $('#device  option:selected').text() != "Select") {
      deviceName = $('#device  option:selected').text();
    }
    var index = $('.nav-tabs li').length + 1;
    // Remove the space in the device name to find tab content with
    // corresponding html id.
    var unspacedDeviceName = deviceName.replace(/\s/g, '');
    // If a tab of selected device had not been created,
    // Create the tab, and link it to the tab content
    if (!tabs.includes(unspacedDeviceName)) {
      $("#home-tab").append("<li class=\"active\"><a href=\"#tab" + unspacedDeviceName + "\" id=\"#tabname" + unspacedDeviceName + "\">" + deviceName + "</a></li>");
      tabs.push(unspacedDeviceName);
    }


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
