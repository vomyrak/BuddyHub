// var Timer = require('./dist/easytimer.min.js')
var timer = new Timer();
timer.start();
timer.addEventListener('secondsUpdated', function (e) {
    $('#basicUsage').html(timer.getTimeValues().toString());
});
