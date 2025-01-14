using Microsoft.EntityFrameworkCore;
using RecipeApp.Context;
using RecipeApp.Models;

namespace RecipeApp.Searcher;

/// <summary>
/// Gets a list of recipes that includes a users favourite
/// </summary>
public class SearchByUserFavorite : SearcherBase
{
    private readonly User _user;

    /// <summary>
    /// Constructor for user, checking if null first.
    /// </summary>
    /// <param name="user">The user object of this searcher close</param>
    /// <exception cref="ArgumentNullException">If the user input was null.</exception>
    public SearchByUserFavorite(SplankContext context, User user) : base(context){
        if (user == null){
            throw new ArgumentNullException("user cannot be null");
        }
        _user = user;
    }

    /// <summary>
    /// Get filtered list of recipes using favorites list.
    /// Comparing the users from the list from the _user, if so, take recipes and
    /// makes list.
    /// </summary>
    /// <returns>The list of recipes from user's favorites.</returns>
    public override List<Recipe> FilterRecipes(){
        List<Recipe> filteredRecipes = Context.Favourites
            .Where(favorite => favorite.User!.Equals(_user))
            .Include(fav => fav.Recipe!.Ingredients)    
            .Include(fav => fav.Recipe!.Steps)
            .Include(fav => fav.Recipe!.Tags)
            .Include(fav => fav.Recipe!.Ratings)
            .Select(favorite => favorite.Recipe)
            .ToList()!;
        return filteredRecipes;
    }
}