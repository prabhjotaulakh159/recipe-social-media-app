using RecipeApp.Context;
using RecipeApp.Models;

namespace RecipeApp.Searcher;

/// <summary>
/// Searcher for users
/// </summary>
public class SearchAllUsers
{

    private readonly string _criteria;
    private readonly SplankContext _context;

    /// <summary>
    /// Constructor for searchAllUsers, validates username, assigns username
    /// and context.
    /// </summary>
    /// <param name="context">The splank context</param>
    /// <param name="username">the username of specified by input.</param>
    /// <exception cref="ArgumentException">If username is invalid</exception>
    public SearchAllUsers(SplankContext context, string username)
    {
        if (username == null) throw new ArgumentException("Username cannot be null");
        if (username.Length == 0) throw new ArgumentException("Tag name cannot be empty");
        _criteria = username;
        _context = context;
    }

    /// <summary>
    /// Returns a list of users by name
    /// </summary>
    /// <returns>Returns a list of users by name</returns>
    public List<User> GetUserByName()
    {
        List<User> userFiltered = _context.Users
                                .Where(user => user.Name.ToLower().Contains(_criteria.ToLower()))
                                .ToList();
        return userFiltered;
    }

}