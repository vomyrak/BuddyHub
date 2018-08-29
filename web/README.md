# BuddyHub
## Install and run the code on local machine

1. Do `npm install` in this folder.
2. Create a mongo database, put the details in *auth.json* in this folder in the following format:
  ```
  {
    "user": "<username of your database>",
    "pass": "<password of your database>",
    "dns": "<IP address of your databse>"
  }
  ```
3. If you are not using the text to speech on the server, just create *keys.json* in this folder. Else, obtain a API key from Google API service, put the key in *keys.json* in this folder as follow:
  ```
  {
    "google": "<your API key>"
  }
  ```
4. If you are not using the device suggestion functionality, just create *email.json* in this folder. Else, put the email and password in *email.json* in this folder as follow:
  ```
  {
    "email": "<your email>",
    "password": "<your password>"
  }
  ```
5. run `node server.js` and open [http://localhost:8000](http://localhost:8000) in your browser.

## Set up Mongo database
1. [Install MongoDB](https://docs.mongodb.com/manual/installation/)
2. Add your own device.
  * devices with APIs:
  ```
    "device" : "<device name(String)>",
    "device_name" : "<device display name(String)>",
    "methods" : [ {
      "method" : "<method name(String)>",
      "description" : "turn on the lamp(String)",
      "http_method" : "<GET | POST | PUT | DELETE(String)>",
      "link" : <URL to the API call(String)>,
      "data" : <JSON data to be sent(String)>,
      "headers" : <stringified JSON data that contains header info(String)>e.g.:"[{\"header_name\":\"Content-Type\", \"header_field\":\"application/json;charset=UTF-8\"}]",
      "callback_function" : <Callback function name(String), refer to mapping in http.js>,
      "text_input_field" : <field in JSON data that the text input goes>, } ],
    "apiType" : "Http"
  ```
  * devices with C# library
  ```
    "device": <Name of class that the functions are applied to(String)>,
    "device_name" : <display name of device(String)>,
    "methods" : [ {
      "method" : "<method name(String)>",
      "description" : "turn on the lamp(String)",
      "http_method" : "<GET | POST | PUT | DELETE(String)>",
      "link" : <URL to the API call(String)(format: http://<your ip or localhost>:8080/<class name>/<button index>)>,
      "data" : <JSON data to be sent(String)>,
      "headers" : <stringified JSON data that contains header info(String)>e.g.:"[{\"header_name\":\"Content-Type\", \"header_field\":\"application/json;charset=UTF-8\"}]",
      "callback_function" : <Callback function name(String), refer to mapping in http.js>,
      "text_input_field" : <field in JSON data that the text input goes>, } ],
    "assembly" : <Library name(String)>,
    "vid" : <VID of device(String)>,
    "pid" : <PID of device(String)>,
    "function" : [ {
      "name" : <function name(String)>, 
      "buttonIndex" : <button position(Int)(refer below)> } ],
    "apiType" : 'LocalLib'
  ```
    Button Index:
    -------------------------
    |   0   |   1   |   2   |
    -------------------------
    |   3   |   4   |   5   |
    -------------------------
    |   6   |   7   |   8   |
    -------------------------
