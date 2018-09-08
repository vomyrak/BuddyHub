var passport = require("passport");
//enable login method using local strategy
var localStrategy = require("passport-local");
var User = require("../public/js/user");

exports.register = function(req,res){
   //making a new user (1)
    var newUser = new User({username : req.body.username});
    User.register(newUser, req.body.password, function(err,user) {
        if(err){
            console.log(err);
            return res.render("home");
        }
        //then loging them in using passport.authenticate (2)
        passport.authenticate("local")(req,res, function(){
            res.redirect("/");
        });
    }

    )
};

//using middleware to call authenticate method
exports.login = passport.authenticate("local",
    {
        successRedirect: "/",
        failureRedirect: "/"
    }
);

exports.logout = function(req,res){
    req.logout();
    res.redirect("/");
};

//middleware to check if user is logged in
exports.isLoggedIn = function (req,res, next){
    if(req.isAuthenticated()){
        return next();
    }
    res.redirect("/");
}
