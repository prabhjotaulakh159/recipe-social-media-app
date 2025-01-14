namespace RecipeAppUI.ViewModels;
using ReactiveUI;
using RecipeApp.Services;
using RecipeApp.Context;
using RecipeApp.Models;
using System.Reactive;
using System;
using RecipeApp.Exceptions;
using RecipeAppUI.Session;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RecipeAppUI.ViewModels;
using System.Linq;

public class CreateRecipeViewModel : ViewModelBase
{

    private string _name = null!;
    private string _description = null!;
    private int _servings;
    private ObservableCollection<Ingredient> _ingredients = null!;
    private ObservableCollection<Step> _steps  = null!;
    private ObservableCollection<Tag> _tags  = null!;
    private readonly RecipeService _recipeService  = null!;
    private MainWindowViewModel _mainWindowViewModel  = null!;
    private string _errorMessage = null!;

    public string ErrorMessage { get => _errorMessage; set => this.RaiseAndSetIfChanged(ref _errorMessage, value); }
    // --------- Inredient stuff-------------
    private string _ingredientName = null!;
    private UnitOfMeasurement _unitOfMeasurement;
    private double _ingredientPrice;
    private int _ingredientQuantity;
    private string _selectedIndex = "0";



    public UnitOfMeasurement UnitOfMeasurement{
        get => _unitOfMeasurement;
        set => this.RaiseAndSetIfChanged(ref _unitOfMeasurement, value);
    }

    public double IngredientPrice{
        get => _ingredientPrice;
        set => this.RaiseAndSetIfChanged(ref _ingredientPrice, value);
    }

    public int IngredientQuantity{
        get => _ingredientQuantity;
        set => this.RaiseAndSetIfChanged(ref _ingredientQuantity, value);
    }
    public string IngredientName{
        get => _ingredientName;
        set => this.RaiseAndSetIfChanged(ref _ingredientName, value);
    }

    public string SelectedIndex{
            get => _selectedIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedIndex, value);
    }
// ----------------------------------------------------------------
// -------------------------- Steps -------------------------------

    private int _stepTime;
    private string _stepInstruction = null!;

    public int StepTime{
        get => _stepTime;
        set => this.RaiseAndSetIfChanged(ref _stepTime, value);
    }
    public string StepInstruction{
        get => _stepInstruction;
        set => this.RaiseAndSetIfChanged(ref _stepInstruction, value);
    }

// ----------------------------------------------------------------
// -----------------------------Tags-------------------------------
    private string _tagName = null!;
    public string TagName{
        get => _tagName;
        set => this.RaiseAndSetIfChanged(ref _tagName, value);
    }
// ---------------------------------------------
// ----------------------------------------------------------------
// --------------------------Recipe-----------------------------
    public string Name {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    } 

    public string Description {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public int Servings {
        get => _servings;
        set => this.RaiseAndSetIfChanged(ref _servings, value);
    }

    public ObservableCollection<Ingredient> Ingredients {
        get => _ingredients;
        set => this.RaiseAndSetIfChanged(ref _ingredients, value);
    }

    public ObservableCollection<Step> Steps {
        get => _steps;
        set => this.RaiseAndSetIfChanged(ref _steps, value);
    }

    public ObservableCollection<Tag> Tags {
        get => _tags;
        set => this.RaiseAndSetIfChanged(ref _tags, value);
    }
// ----------------------------------------------------------------------
// --------------------------Commands------------------------------------
    public ReactiveCommand<Unit,Unit> ChangeToAddIngredientViewCommand { get;} = null!;
    public ReactiveCommand<Unit,Unit> ChangeToDashboardViewCommand { get;} = null!;
    public MainWindowViewModel MainWindowViewModel { get => _mainWindowViewModel; private set => _mainWindowViewModel = value; }
    public ReactiveCommand<Unit, Unit> CreateIngredient { get; } = null!;
    public ReactiveCommand<Ingredient, Unit> RemoveIngredient { get; } = null!;
    public ReactiveCommand<Step, Unit> RemoveStep { get; } = null!;
    public ReactiveCommand<Tag, Unit> RemoveTag { get; } = null!;
    public ReactiveCommand<Unit, Unit> CreateStep { get; } = null!;
    public ReactiveCommand<Unit, Unit> CreateTag { get; } = null!;
    public ReactiveCommand<Unit, Unit> CreateRecipe { get; } = null!;
    
    /// <summary>
    /// Constructor to create the CreateRecipeViewModel
    /// </summary>
    /// <param name="context">DB context</param>
    /// <param name="mainWindowViewModel">Instance of the MainWindowViewModel</param>
    public CreateRecipeViewModel(SplankContext context, MainWindowViewModel mainWindowViewModel) 
    {
        Ingredients = new ObservableCollection<Ingredient>();
        Steps = new ObservableCollection<Step>();
        Tags = new ObservableCollection<Tag>();
        _recipeService = new RecipeService(context);
        CreateIngredient = ReactiveCommand.Create(CreateIngredientCommand);
        CreateStep = ReactiveCommand.Create(CreateStepCommand);
        CreateTag = ReactiveCommand.Create(CreateTagCommand);
        CreateRecipe= ReactiveCommand.Create(CreateRecipeCommand);
        RemoveIngredient = ReactiveCommand.Create<Ingredient>(RemoveIngredientCommand);
        RemoveStep = ReactiveCommand.Create<Step>(RemoveStepCommand);
        RemoveTag = ReactiveCommand.Create<Tag>(RemoveTagCommand);
        MainWindowViewModel = mainWindowViewModel;
    }

    /// <summary>
    /// Command that creates a recipe
    /// </summary>
    public void CreateRecipeCommand() {
        try {

            Recipe recipe = new Recipe(Name, UserSingleton.GetInstance(), Description, Servings, Ingredients.ToList(), Steps.ToList(), new() ,Tags.ToList());
            _recipeService.CreateRecipe(recipe);
            MainWindowViewModel.ChangeToDashboardView();
        }
        catch(ArgumentException e){
            ErrorMessage = e.Message;
        }
    }

    /// <summary>
    /// Command to create a new ingredient and adds it to the list of ingredients
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public void CreateIngredientCommand() {
        try {
            ChangeUnit();
            Ingredient ingredient = new Ingredient {
                Name = IngredientName,
                Quantity = IngredientQuantity,
                UnitOfMeasurement = UnitOfMeasurement,
                Price = IngredientPrice
            };
            Ingredients.Add(ingredient);
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException(e.Message);

        }
    }

    /// <summary>
    /// Removes an Ingredient from the List of Ingredient
    /// </summary>
    /// <param name="ingredient">Ingredient to remove</param>
    public void RemoveIngredientCommand(Ingredient ingredient) {
        Ingredients.Remove(ingredient);
    }

    /// <summary>
    /// Removes a step from the List of Steps
    /// </summary>
    /// <param name="stepToRemove">The step to remove</param>
    private void RemoveStepCommand(Step stepToRemove)
    {
        Steps.Remove(stepToRemove);
    }

    /// <summary>
    /// Removes the tag from the list of tags
    /// </summary>
    /// <param name="tagToRemove">Tag to remove</param>
    private void RemoveTagCommand(Tag tagToRemove)
    {
        Tags.Remove(tagToRemove);
    }

    /// <summary>
    /// Changes the unit of measurement of the ingredient added depending on the combo box index
    /// </summary>
    public void ChangeUnit() {
        UnitOfMeasurement unit = UnitOfMeasurement.SPOONS;
        switch (_selectedIndex) {
            case "0":
                unit = UnitOfMeasurement.SPOONS;
                break;
            case "1":
                unit = UnitOfMeasurement.GRAMS;
                break;
            case "2":
                unit = UnitOfMeasurement.CUPS;
                break;
            case "3":
                unit = UnitOfMeasurement.TEASPOONS;
                break;
            case "4":
                unit = UnitOfMeasurement.AMOUNT;
                break;
        }
        _unitOfMeasurement = unit;   
    }

    /// <summary>
    /// Command that Creates a new step and adds it to the list of steps
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public void CreateStepCommand() {
        try {
            Step step = new Step(StepTime, StepInstruction); 
            Steps.Add(step);
        } catch (ArgumentException e) {
            throw new ArgumentException(e.Message);
        }
    }

    /// <summary>
    /// Command that creates a new Tag and adds it to the List of tags
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public void CreateTagCommand() {
        try {
            Tag tag = new Tag(TagName);
            Tags.Add(tag);
        } catch (ArgumentException e) {
            throw new ArgumentException(e.Message);
        }
    }

}
