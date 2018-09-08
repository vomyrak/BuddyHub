var passport = require("passport");
var User = require("../db/models/user");

// Function for registering a new user
exports.register = function(req,res){
   // Making a new user (1)
    var newUser = new User({username : req.body.username});
    User.register(newUser, req.body.password, function(err,user) {
        if(err){
            console.log(err);
            return res.render("home");
        }
        // Then loging them in using passport.authenticate (2)
        passport.authenticate("local")(req,res, function(){
            res.redirect("/");
        });
    }

    )
};

// Function for logging in
// Both successful and failed login attempt will redirect to route '/'
exports.login = passport.authenticate("local",
    {
        successRedirect: "/",
        failureRedirect: "/"
    }
);

// Function for logging out
exports.logout = function(req,res){
    req.logout();
    res.redirect("/");
};

// Middleware to check if user is logged in
// If user not logged in, redirect to route '/'
exports.isLoggedIn = function (req,res, next){
    if(req.isAuthenticated()){
        return next();
    }
    res.redirect("/");
}
