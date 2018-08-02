
  <% recipe.instruction.forEach(function(step){ %>
    <% if (step.stepnumber === recipe.instruction.length) { %>
      <p style="text-align: center">
        <strong><%= step.stepnumber %></strong> - <%= step.step %>
        <center><a href="/recipes" class="btn btn-primary">This is the last step</a></center>
      </p>
  <%  } else if (step.id === stepid) { %>
        <p style="text-align: center">
          <strong><%= step.stepnumber %></strong> - <%= step.step %>
          <center><a href="/recipe/<%= recipe._id%>/<%= recipe.instruction[step.stepnumber]._id%>" class="btn btn-primary">Next step</a></center>
        </p>
    <%  } %>

  <%});%>


/*

Recipe.findOne({name: "Noodles"}, function(err,recipe){
  if (err){
    console.log(err);
  } else {
    recipe.instruction.push({
      stepnumber: 1,
      step: "Put noodles in the pot"
    });
    recipe.save(function(err, recipe){
      if(err){
        console.log(err);
      } else {
        console.log(recipe);
      }
    });
  }
});

*/

/*

var newRecipe = new Recipe({
  name: "Noodles",
  steps: 3,
  image: "https://www.budgetbytes.com/wp-content/uploads/2009/12/Garlic-Noodles-front.jpg",
  instruction: []
});

newRecipe.save(function(err, recipe){
  if(err){
    console.log(err);
  } else {
    console.log(recipe);
  }
});

*/

/*

var newStep = new Step({
    stepnumber: 1,
    step: "Put noodles in a pot"
});

*/
