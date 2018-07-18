# Information about the MongoDB database #

The database is hosed on Amazon AWS, and its public DNS address is hardcoded into backend.js with log-on string. 
To authenticate log-on, create a file called "auth.json" in the same directory with log-on detail which I will announce elseware *wink wink*

For all development features of Mongo, go to *https://www.mongodb.com/* to download community server and "Compass" graphical interface for MongoDB. Note that they do **not** run on x86 Linux OS.

To access the database within Mongo shell, use command **mongo 35.177.37.56 -u \[username] -p \[password] --authenticationDatabase admin**

**Remember to untrack the json file from git if it is tracked!**
