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
TODO
