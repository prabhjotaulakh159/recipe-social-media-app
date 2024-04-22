using RecipeApp.Api;
using RecipeApp.Models;

namespace RecipeAppTest.Api;

[TestClass]
public class NutritionFactFetcherTest {
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FetchNullRecipeThrowsArgumentException() {
        NutritionFactFetcher nutritionFactFetcher = new NutritionFactFetcher();
        nutritionFactFetcher.Fetch(null);
    }

    [TestMethod]
    public void FetchRecipeReceivesRespoonse() {
        NutritionFactFetcher nutritionFactFetcher = new NutritionFactFetcher();
        List<Ingredient> ingredients = new List<Ingredient>() {
            new Ingredient("Potato", 2, UnitOfMeasurement.AMOUNT, 20)
        };
        List<Step> steps = new List<Step>() {
            new Step(5, "Do potato")
        };
        User user = new User("Rida", "This is rida", "This is my password", new(), new(), "Salt");
        Recipe recipe = new Recipe("Potato", user, "Potato", 2, ingredients, steps, new(), new());
        NutritionResponse apiResponse = (NutritionResponse) nutritionFactFetcher.Fetch(recipe);
        Assert.IsNotNull(apiResponse);
        Assert.IsTrue(apiResponse.calories > 0);
    }    
}