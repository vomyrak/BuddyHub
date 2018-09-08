# BuddyHub (Installation Guide for Contributors)
## Install and run the code on local machine

1. Do `npm install` in this folder.
2. Create a mongo database(Refer to set up Mondo database below), put the details in *.env* file in this folder in the following format:
  ```
  DB_DNS=<IP address of your database>
  DB_USER="<username of your database>"
  DB_PASS="<password of your database>"
  DB_NAME="<name of your database>"
  ```
3. If you are not using the text to speech on the server, ignore this. Else, obtain a API key from Google API service, put the key in *.env* file in this folder as follow:
  ```
  GOOGLE="<your API key>"
  ```
4. If you are not using the device suggestion functionality, ignore this. Else, put the email and password in *.env* in this folder as follow:
  ```
  EMAIL="<your email>"
  EMAIL_PWD="<password of your email>"
  ```
5. run `node -r dotenv/config server.js` and open [http://localhost:8000](http://localhost:8000) in your browser.

## Set up Mongo database
1. [Install MongoDB](https://docs.mongodb.com/manual/installation/)
2. Set up a database and open a collection inside it called "OutputDevices3"
3. Add your own device in the collection.
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
  * devices with C# library (need to implement our IDevice interface, more than 9 functions on same device might not work due to the limit of our button index, hope to be improved in the future):
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
