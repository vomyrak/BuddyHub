const OutputDevice = require('../db/models/output-device');
const DeviceSuggestion = require('../db/models/device-suggestion');

exports.renderWithAllDevices = function(res, pagename) {
  var query = OutputDevice.find().sort('device');

  query.exec(function(err, devices) {
    if (err) return handleError(err);

    res.render(pagename, {
      devices: devices
    });
  });
};

exports.renderWithSelectedDevice = function(req, res) {
  // Render the page with all output devices in the menu
  // Render the page with methods of the selected device
  var query = OutputDevice.find().sort('device');

  query.exec(function(err, devices) {
    if (err) return handleError(err);

    var query2 = OutputDevice.findOne({
      device: req.query.selected
    });
    query2.exec(function(error, selected) {
      if (error) return handleError(error);

      res.render('device', {
        devices: devices,
        device: selected
      });
    });
  });
};

exports.storeSuggestionAndRedirect = function(req, res) {
  // Upload the details o the device suggestion to the database.
  // The "processed" field is set to false until the admin process this
  // device suggestion.

  var name = req.body.name;
  var email = req.body.email;
  var device = req.body.device;
  var description = req.body.description;

  var suggestion = new DeviceSuggestion({
    name: name,
    email: email,
    device: device,
    description: description,
    approved: false,
    processed: false
  });
  suggestion.save(function(err) {
    if (err) return handleError(err);
  });

  // Send a confirmation email to user
  var emailSender = require('./utils/email');
  emailSender.sendConfEmail(email, name, device, description);

  res.redirect('/submitted');
};
