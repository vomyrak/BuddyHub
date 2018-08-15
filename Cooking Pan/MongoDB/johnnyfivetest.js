var request = require("request"),
	five = require("johnny-five");

var board = new five.Board();

var getOptions = {
  host: "http://jsonplaceholder.typicode.com/users/1",
};

var postOptions = {
	url: "http://ptsv2.com/t/u4i5v-1533811142/post",
	method: "POST",
	json: true,
	body: {
		arduino: "wireless"
	}
};

board.on('ready', function(){
  console.log("Ready");
  postRequest();
});

function getRequest() {
	console.log("Making GET request");
	request.get(getOptions.host, getOptions, function(err, res, body) {
		if (!err && res.statusCode == 200) {
			console.log(res.statusCode);
			console.log(body);
		}
		else {
			console.log("error");
		}
	})
}

function postRequest() {
	console.log("Making POST request");
	request.post(postOptions, function (err, res, body) {
        if (!err && res.statusCode == 200) {
        	console.log(res.statusCode);
            console.log("POST request success");
            console.log(body);
        }
    })
}