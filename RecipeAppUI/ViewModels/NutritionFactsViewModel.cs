namespace RecipeAppUI.ViewModels;

using RecipeApp.Api;
using RecipeApp.Models;
using ReactiveUI;
using System;

/// <summary>
/// Displays nutrition facts about a chosen recipe
/// </summary>
public class NutritionFactsViewModel : ViewModelBase {
    private readonly NutritionFactFetcher _nutrientationFetcher = new();
    private MainWindowViewModel _mainWindowViewModel = null!;
    private string? _errorMessage;
    private double _calories;
    private double _totalFat;
    private double _saturatedFat;
    private double _protein;
    private double _sodium;
    private double _cholesterol;
    private double _carbohydrates;
    private double _fiber;
    private double _sugar;

    public MainWindowViewModel MainWindowViewModel {
        get => _mainWindowViewModel;
        set => _mainWindowViewModel = value;
    }

    public string? ErrorMessage {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }

    public double Calories {
        get => _calories;
        set => this.RaiseAndSetIfChanged(ref _calories, value);
    }

    public double TotalFat {
        get => _totalFat;
        set => this.RaiseAndSetIfChanged(ref _totalFat, value);
    }

    public double SaturatedFat {
        get => _saturatedFat;
        set => this.RaiseAndSetIfChanged(ref _saturatedFat, value);
    }

    public double Protein {
        get => _protein;
        set => this.RaiseAndSetIfChanged(ref _protein, value);
    }

    public double Sodium {
        get => _sodium;
        set => this.RaiseAndSetIfChanged(ref _sodium, value);
    }

    public double Cholesterol {
        get => _cholesterol;
        set => this.RaiseAndSetIfChanged(ref _cholesterol, value);
    }

    public double Carbohydrates {
        get => _carbohydrates;
        set => this.RaiseAndSetIfChanged(ref _carbohydrates, value);
    }

    public double Fiber {
        get => _fiber;
        set => this.RaiseAndSetIfChanged(ref _fiber, value);
    }

    public double Sugar {
        get => _sugar;
        set => this.RaiseAndSetIfChanged(ref _sugar, value);
    }

    /// <summary>
    /// Constructs a NutritionFactsViewModel and 
    /// immedietly fetches the nutrition facts, and sets 
    /// the error messages if the process fails
    /// </summary>
    /// <param name="recipe">Recipe to get nutrition from</param>
    public NutritionFactsViewModel(Recipe recipe) {
        try {
            NutritionResponse response = (NutritionResponse) _nutrientationFetcher.Fetch(recipe);
            Calories = response.calories;
            TotalFat = response.fat_total_g;
            SaturatedFat = response.fat_saturated_g;
            Protein = response.protein_g;
            Sodium = response.sodium_mg;
            Cholesterol = response.cholesterol_mg;
            Carbohydrates = response.carbohydrates_total_g;
            Fiber = response.fiber_g;
            Sugar = response.sugar_g;
        } catch (ArgumentException e) {
            ErrorMessage = e.Message;
        } catch (Exception) {
            ErrorMessage = "Sorry, unable to get nutrition facts for this recipe :(";
        }
    }
}