namespace RecipeApp.Searcher;

using RecipeApp.Models;

public class SearchByIngredients: ISearcher{

    private readonly string __criteria;

    /// <summary>
    /// Constructor contains the criteria, corresponding to the crtieria specified.
    /// </summary>
    /// <param name="criteria">Ingredient name  </param>
    public SearchByIngredients(string ingredientName){
        __criteria = ingredientName;
    }

    /// <summary>
    /// Loops through each recipe in List, gets each ingredients of recipe and compares name to criteria. 
    /// </summary>
    /// <param name="recipes">The list of recipes passed through</param>
    /// <returns>Returned a filtered list of recipes containing the ingredient.</returns>
    public List<Recipe> FilterRecipes(List<Recipe> recipes){
        List<Recipe> filteredRecipes =new ();
        foreach(Recipe r in recipes){
            foreach(Ingredient ing in ingredientsOfRecipe(r)){
                if(ing.Name.ToLower().Contains(__criteria.ToLower())){
                    filteredRecipes.Add(r);
                }
            }
        }
        return filteredRecipes;
    }


    /// <summary>
    /// Getss list of ingredients from a recipe
    /// </summary>
    /// <param name="r">The recipe being used</param>
    /// <returns>The list of ingredients of recipe.</returns>
    private static List<Ingredient> ingredientsOfRecipe(Recipe r){
        return r.Ingredients;
    }

}