'use strict';
module.exports = function(app) {
  var recipe = require('../controllers/cookingController');

  // recipe Routes
  app.route('/recipes')
    .get(recipe.list_all_recipes)
    .post(recipe.create_a_recipe);


  app.route('/recipes/:id')
    .get(recipe.read_a_recipe)
    .put(recipe.update_a_recipe)
    .delete(recipe.delete_a_recipe);
};
