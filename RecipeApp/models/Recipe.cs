namespace RecipeApp.Models;

using System.Text;

/// <summary>
/// Recipe schema
/// </summary>
public class Recipe {
    private string _name;
    private string _description;
    private int _servings;
    private List<Ingredient> _ingredients;
    private List<Step> _steps;
    private List<Rating> _ratings;
    private List<Tag> _tags;

    public string Name { 
        get => _name;  
        set {
            if (value == null) 
                throw new ArgumentException("Name cannot be null");
            if (value.Length == 0) 
                throw new ArgumentException("Name cannot be empty");
            _name = value;
        } 
    }

    public string Description { 
        get => _description; 
        set {
            value ??= "";
            if (value.Length > Constants.MAX_DESCRIPTION_LENGTH) 
                throw new ArgumentException("Recipe description cannot exceed " + Constants.MAX_DESCRIPTION_LENGTH + " characters");
            _description = value;
        } 
    }

    public int Servings { 
        get => _servings; 
        set {
            if (value < Constants.MIN_SERVINGS) 
                throw new ArgumentException("Serving(s) must be greater than 0");
            _servings = value;
        }
    }

    public List<Ingredient> Ingredients { 
        get => _ingredients; 
        set {
            CheckIngredients(value);
            PopulateIngredients(value);
            _ingredients = value;
        } 
    }

    public List<Step> Steps { 
        get => _steps; 
        set {
            CheckSteps(value);
            PopulateSteps(value);
            _steps = value;
        } 
    }
    public List<Rating> Ratings { 
        get => _ratings; 
        set {
            if (value == null) 
                throw new ArgumentException("Ratings cannot be null");
            PopulateRatings(value);
        }
    }

    public List<Tag> Tags { 
        get => _tags; 
        set {
            CheckTags(value);
            PopulateTags(value);

        }
    }
    
    public User User { get; private set; }

    /// <summary>
    /// Constructor with user, ingredients, steps and ratings
    /// </summary>
    /// <param name="user">User who made the recipe</param>
    /// <param name="description">Short description of recipe</param>
    /// <param name="servings">Serving amount</param>
    /// <param name="ingredients">List of required ingredients</param>
    /// <param name="steps">Steps to complete the recipe</param>
    /// <param name="ratings">Recipe ratings</param>
    /// <param name="tags">List of tags</param>
    /// <exception cref="ArgumentException">If any fields are null, empty or doesn't respect certain constraints</exception>
    public Recipe(string name, User user, string description, int servings, List<Ingredient> ingredients, 
            List<Step> steps, List<Rating> ratings, List<Tag> tags) {
        if (name == null) 
            throw new ArgumentException("Name cannot be null");
        if (name.Length == 0) 
            throw new ArgumentException("Name cannot be empty");
        if (user == null) 
            throw new ArgumentException("User cannot be null");
        if (servings < Constants.MIN_SERVINGS) 
            throw new ArgumentException("Serving(s) must be greater than 0");
        if (ratings == null) 
            throw new ArgumentException("Ratings cannot be null");
        description ??= "";
        if (description.Length > Constants.MAX_DESCRIPTION_LENGTH) {
            throw new ArgumentException("Recipe description cannot exceed " + Constants.MAX_DESCRIPTION_LENGTH + " characters");
        }

        CheckIngredients(ingredients);
        CheckTags(tags);
        CheckSteps(steps);
        
        Name = name;
        User = new(user.Name, user.Description, user.Password, user.Favorites, user.MadeRecipes);
        Description = description;
        Servings = servings;
        
        PopulateIngredients(ingredients);
        PopulateRatings(ratings);
        PopulateTags(tags);
        PopulateSteps(steps);
    }

    /// <summary>
    /// Validates the list of ingredients
    /// </summary>
    /// <param name="ingredients">List of ingredients</param>
    /// <exception cref="ArgumentException">If ingredients null or empty</exception>
    private static void CheckIngredients(List<Ingredient> ingredients) {
        if (ingredients == null) 
            throw new ArgumentException("Ingredients cannot be null");
        if (ingredients.Count == 0) 
            throw new ArgumentException("Ingredients cannot be empty");
    }

    /// <summary>
    /// Validates the list of tags
    /// </summary>
    /// <param name="tags">List of tags</param>
    /// <exception cref="ArgumentException">If null or too many tags</exception>
    private static void CheckTags(List<Tag> tags) {
        if (tags == null) 
            throw new ArgumentException("Tags cannot be null");
        if (tags.Count > Constants.MAX_TAGS) 
            throw new ArgumentException("Recipe can have a maximum of 3 tags");
    }

    /// <summary>
    /// Validates the list of steps
    /// </summary>
    /// <param name="steps">List of steps</param>
    /// <exception cref="ArgumentException">If steps null or empty</exception>
    private static void CheckSteps(List<Step> steps) {
        if (steps == null) 
            throw new ArgumentException("Steps cannot be null");
        if (steps.Count == 0) 
            throw new ArgumentException("Steps cannot be empty"); 
    }
    
    /// <summary>
    /// Makes a deep copy for Ingredients
    /// </summary>
    /// <param name="ingredients">Reference to constructor param</param>
    private void PopulateIngredients(List<Ingredient> ingredients) {
        Ingredients = new();
        foreach (Ingredient ingredient in ingredients) {
            Ingredients.Add(ingredient);
        }
    }

    /// <summary>
    /// Makes a deep copy for Steps
    /// </summary>
    /// <param name="steps">Reference to constructor param</param>
    private void PopulateSteps(List<Step> steps) {
        Steps = new();
        foreach (Step step in steps) {
            Steps.Add(step);
        }
    }

    /// <summary>
    /// Makes a deep copy for ratings
    /// </summary>
    /// <param name="ratings">Reference to constructor param</param>
    private void PopulateRatings(List<Rating> ratings) {
        Ratings = new();
        foreach (Rating rating in ratings) {
            Ratings.Add(rating);
        }
    }

    /// <summary>
    /// Makes a deep copy for tags
    /// </summary>
    /// <param name="tags">Reference to constructor param</param>
    private void PopulateTags(List<Tag> tags) {
        Tags = new();
        foreach (Tag tag in tags) {
            Tags.Add(tag);
        }
    }

    /// <summary>
    /// Gets the total time to cook for a recipe 
    /// </summary>
    /// <returns>Total time to complete all steps</returns>
    public int GetTimeToCook() {
        int timeToCook = 0;
        foreach (Step step in this.Steps) {
            timeToCook += step.TimeInMinutes;
        }
        return timeToCook;
    }

    public double GetTotalPrice() {
        double price = 0;
        foreach (Ingredient ingredient in Ingredients) {
            price += ingredient.Price;
        }
        return price;
    }

    public override string ToString() {
        StringBuilder builder = new();
        builder.Append("Username: " + User.Name + "\n");
        builder.Append("Description: " + Description + "\n");
        builder.Append("Ingredients: \n");
        builder.Append("Servinsg: " + Servings + "\n"); 
        foreach (Ingredient ingredient in Ingredients) {
            builder.Append(ingredient.ToString() + "\n");
        }
        builder.Append("Steps: \n");
        foreach (Step step in Steps) {
            builder.Append(step.ToString() + "\n");
        }
        builder.Append("Tags: \n");
        foreach (Tag tag in Tags) {
            builder.Append(tag.ToString() + "\n");
        }
        builder.Append("Reviews: \n");
        foreach (Rating rating in Ratings) {
            builder.Append(rating.ToString() + "\n");
        }
        return builder.ToString();
    }
}
