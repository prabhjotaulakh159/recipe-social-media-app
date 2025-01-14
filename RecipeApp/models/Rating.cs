namespace RecipeApp.Models;

/// <summary>
/// For rating of recipes, contains stars, description, and user/author of the rating.
/// </summary>
public class Rating {
    private int _stars;
    private string _description = null!;
    private User _user = null!;

    public Recipe? Recipe {
        get; 
        set;
    }

    public int RecipeId { get; set; }

    public int RatingId { 
        get; 
        set; 
    }

    public int Stars {
        get => _stars; 
        set {
            if (value > 5 || value < 0) 
                throw new ArgumentException("Stars must be between 0 to 5");
            _stars = value;
        }
    }

    public string? Description{
        get => _description; 
        set {
            if (string.IsNullOrWhiteSpace(value)) 
                value = "";
            if (value.Length > Constants.MAX_DESCRIPTION_LENGTH) 
                throw new ArgumentException("Max description length exceeded!");
            else 
                _description = value;
        }
    }

    public User User {
        get => _user; 
        set {
            _user = value ?? throw new ArgumentException("User cannot be null");
        }
    }

    public int UserId { get; set; }

     

    /// <summary>
    /// Constructor of Rating.
    /// </summary>
    /// <param name="stars">Stars for rating out of 5</param>
    /// <param name="desc">Dscription of the rating</param>
    /// <param name="user">User who created the rating</param>
    /// <exception cref="ArgumentException">Throws exceptions if user disrespects contraints</exception>
    public Rating(int stars, string description, User user){
        Stars = stars;
        Description = description;
        User = user;
    }

    /// <summary>
    /// Empty constructor for Entity framework
    /// </summary>
    public Rating() {

    }

    /// <summary>
    /// Overriden ToString()
    /// </summary>
    /// <returns>String representation of a rating</returns>
    public override string ToString() {
        return "User: " + User.Name + "\n" + "Stars: " + Stars + "\n" + "Description: " + Description;
    }
}