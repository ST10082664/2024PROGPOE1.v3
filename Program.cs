using _2024PROGPOE1.v3;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2024PROGPOE1.v3
{
    namespace RecipeApplication
    {
        internal class Program
        {
            // Dictionary to store recipes with the recipe name as the key
            static Dictionary<string, Recipe> recipes = new Dictionary<string, Recipe>();

            // Delegate to check calorie limit
            delegate void CalorieCheck(double totalCalories);

            // ------------------------------
            // Main entry point of the application
            // ------------------------------
            static void Main(string[] args)
            {
                Console.WriteLine("Welcome to the Recipe Application!");
                string choice;
                do
                {
                    Console.WriteLine("\nWhat would you like to do?");
                    Console.WriteLine("1. Add a new recipe");
                    Console.WriteLine("2. View all recipes");
                    Console.WriteLine("3. Filter recipes by ingredient or name");
                    Console.WriteLine("4. Sort recipes by total calories");
                    Console.WriteLine("5. Exit");
                    choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddNewRecipe();
                            break;
                        case "2":
                            ViewAllRecipes();
                            break;
                        case "3":
                            FilterRecipes();
                            break;
                        case "4":
                            SortRecipesByCalories();
                            break;
                        case "5":
                            Console.WriteLine("Thank you for using the Recipe Application. Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                } while (choice != "5");
            }
            
            // ------------------------------
            // Method to add a new recipe to the collection
            // ------------------------------
            static void AddNewRecipe()
            {
                Console.WriteLine("\nEnter the name of the recipe:");
                string name = Console.ReadLine();

                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Recipe name cannot be empty. Please try again.");
                    return;
                }

                if (recipes.ContainsKey(name))
                {
                    Console.WriteLine("A recipe with this name already exists. Please enter a unique name.");
                    return;
                }

                var recipe = new Recipe { Name = name };

                Console.WriteLine("\nDetails of the ingredients:");
                string ingredientChoice = "";
                do
                {
                    var ingredient = new Ingredient();

                    Console.WriteLine("\nEnter the name of the ingredient:");
                    ingredient.Name = Console.ReadLine();
                    if (string.IsNullOrEmpty(ingredient.Name))
                    {
                        Console.WriteLine("Ingredient name cannot be empty. Please try again.");
                        continue;
                    }

                    Console.WriteLine("Enter the quantity of the ingredient:");
                    if (!double.TryParse(Console.ReadLine(), out double quantity) || quantity <= 0)
                    {
                        Console.WriteLine("Invalid quantity. Please enter a positive number.");
                        continue;
                    }
                    ingredient.Quantity = quantity;
                    ingredient.OriginalQuantity = quantity;

                    Console.WriteLine("Enter the unit of the ingredient:");
                    ingredient.Unit = Console.ReadLine();
                    if (string.IsNullOrEmpty(ingredient.Unit))
                    {
                        Console.WriteLine("Unit cannot be empty. Please try again.");
                        continue;
                    }
                    Console.WriteLine("Enter the calories of the ingredient:");
                    if (!double.TryParse(Console.ReadLine(), out double calories) || calories < 0)
                    {
                        Console.WriteLine("Invalid calorie value. Please enter a non-negative number.");
                        continue;
                    }
                    ingredient.Calories = calories;

                    Console.WriteLine("Select the food group of the ingredient:");
                    DisplayFoodGroups();
                    if (!int.TryParse(Console.ReadLine(), out int foodGroupChoice) || foodGroupChoice < 1 || foodGroupChoice > 7)
                    {
                        Console.WriteLine("Invalid food group choice. Please try again.");
                        continue;
                    }
                    ingredient.FoodGroup = GetFoodGroupName(foodGroupChoice);

                    recipe.Ingredients.Add(ingredient);

                    Console.WriteLine("Add another ingredient? (yes/no)");
                    ingredientChoice = Console.ReadLine().ToLower();
                } while (ingredientChoice == "yes");

                Console.WriteLine("\nEnter the steps for the recipe:");
                int stepNumber = 1;
                string stepChoice = "";
                do
                {
                    var step = new Step
                    {
                        Number = stepNumber++,
                        Description = Console.ReadLine()
                    };

                    if (string.IsNullOrEmpty(step.Description))
                    {
                        Console.WriteLine("Step description cannot be empty. Please try again.");
                        continue;
                    }

                    recipe.Steps.Add(step);

                    Console.WriteLine("Add another step? (yes/no)");
                    stepChoice = Console.ReadLine().ToLower();
                } while (stepChoice == "yes");

                recipes.Add(recipe.Name, recipe);
                Console.WriteLine("\nRecipe added successfully!");

                // Check for calorie limit
                CalorieCheck calorieCheck = CheckCalorieLimit;
                calorieCheck(recipe.CalculateTotalCalories());
            }

            // ------------------------------
            // Method to display food groups
            // ------------------------------
            static void DisplayFoodGroups()
            {
                Console.WriteLine("1. Starchy foods");
                Console.WriteLine("2. Vegetables and fruits");
                Console.WriteLine("3. Dry beans, peas, lentils and soya");
                Console.WriteLine("4. Chicken, fish, meat and eggs");
                Console.WriteLine("5. Milk and dairy products");
                Console.WriteLine("6. Fats and oil");
                Console.WriteLine("7. Water");
            }

            // ------------------------------
            // Method to get food group name based on user choice
            // ------------------------------
            static string GetFoodGroupName(int choice)
            {
                switch (choice)
                {
                    case 1:
                        return "Starchy foods";
                    case 2:
                        return "Vegetables and fruits";
                    case 3:
                        return "Dry beans, peas, lentils and soya";
                    case 4:
                        return "Chicken, fish, meat and eggs";
                    case 5:
                        return "Milk and dairy products";
                    case 6:
                        return "Fats and oil";
                    case 7:
                        return "Water";
                    default:
                        throw new ArgumentException("Invalid food group choice");
                }
            }

            // ------------------------------
            // Method to check if the recipe's total calories exceed 300 and trigger the alert
            // ------------------------------
            static void CheckCalorieLimit(double totalCalories)
            {
                if (totalCalories >= 300)
                {
                    PrintCalorieAlert(totalCalories);
                }
            }

            // ------------------------------
            // Method to print the calorie alert
            // ------------------------------
            static void PrintCalorieAlert(double totalCalories)
            {
                Console.WriteLine($"ALERT! {totalCalories} calories\nThis amount may be considered a light meal for many adults, but it could be appropriate for others.\nFun Fact: In science, 1 calorie (1 cal) is the unit of measure of energy contained in 1g of food. And an average adult needs 2000-2500 kcal per day and not per meal.");
            }

            // ------------------------------
            // Method to display all available recipes and provide options to view, clear, or go back to the main menu
            // ------------------------------
            static void ViewAllRecipes()
            {
                if (recipes.Count == 0)
                {
                    Console.WriteLine("\nNo recipes found.");
                    return;
                }

                Console.WriteLine("\nAvailable Recipes:");
                foreach (var recipe in recipes.OrderBy(r => r.Key))
                {
                    Console.WriteLine($"Recipe Name: {recipe.Key}");
                }

                Console.WriteLine("\nSelect:");
                Console.WriteLine("1. A recipe");
                Console.WriteLine("2. Clear all");
                Console.WriteLine("3. Go back to main menu");
                Console.Write("Enter your option: ");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        DisplayRecipe();
                        break;
                    case "2":
                        ClearAllRecipes();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }

            // ------------------------------
            // Method to display the details of a selected recipe
            // ------------------------------
            static void DisplayRecipe()
            {
                if (recipes.Count == 0)
                {
                    Console.WriteLine("\nNo recipes found.");
                    return;
                }

                Console.WriteLine("\nAvailable Recipes:");
                foreach (var recipe in recipes.OrderBy(r => r.Key))
                {
                    Console.WriteLine($"Recipe Name: {recipe.Key}");
                }

                Console.WriteLine("Enter the name of the recipe you want to view, or type 'back' to return to the main menu:");
                string input = Console.ReadLine();

                if (input.ToLower() == "back")
                {
                    return; // Go back to main menu
                }

                if (recipes.ContainsKey(input))
                {
                    Recipe selectedRecipe = recipes[input];
                    DisplayFullRecipe(selectedRecipe);
                    RecipeOptions(selectedRecipe);
                }
                else
                {
                    Console.WriteLine("Recipe not found. Please try again.");
                }
            }

            // ------------------------------
            // Method to display the full details of a recipe including ingredients and steps
            // ------------------------------
            static void DisplayFullRecipe(Recipe recipe)
            {
                Console.WriteLine($"\nRecipe Name: {recipe.Name}");
                Console.WriteLine("Ingredients:");
                foreach (var ingredient in recipe.Ingredients)
                {
                    Console.WriteLine($"- {ingredient.Name}: {ingredient.Quantity:0.##} {ingredient.Unit} ({ingredient.Calories} calories, {ingredient.FoodGroup})");
                }

                Console.WriteLine("Steps:");
                foreach (var step in recipe.Steps)
                {
                    Console.WriteLine($"Step {step.Number}: {step.Description}");
                }

                double totalCalories = recipe.CalculateTotalCalories();
                Console.WriteLine($"\nTotal Calories: {totalCalories}");
                if (totalCalories >= 300)
                {
                    PrintCalorieAlert(totalCalories);
                }
            }

            // ------------------------------
            // Method to provide options for actions on a selected recipe such as scaling, deleting, or navigating
            // ------------------------------
            static void RecipeOptions(Recipe recipe)
            {
                Console.WriteLine("\nWould you like to:");
                Console.WriteLine("1. Scale the recipe");
                Console.WriteLine("2. Delete the recipe");
                Console.WriteLine("3. Go back to recipes list");
                Console.WriteLine("4. Go back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ScaleRecipe(recipe);
                        break;
                    case "2":
                        DeleteRecipe(recipe.Name);
                        break;
                    case "3":
                        DisplayRecipe(); // Recursive call to list recipes again
                        break;
                    case "4":
                        return; // Exiting this method brings user back to the main menu
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        RecipeOptions(recipe); // Recursive call to handle incorrect options
                        break;
                }
            }

            // ------------------------------
            // Method to scale the ingredients of a recipe by a specified factor
            // ------------------------------
            static void ScaleRecipe(Recipe recipe)
            {
                Console.WriteLine("Choose a scale factor:");
                Console.WriteLine("1. Scale to half (0.5)");
                Console.WriteLine("2. Scale to double (2)");
                Console.WriteLine("3. Scale to triple (3)");
                Console.WriteLine("4. Reset to original quantities");
                Console.Write("Enter your option: ");

                string scaleOption = Console.ReadLine();
                double scale = 1;  // Default no scaling

                switch (scaleOption)
                {
                    case "1":
                        scale = 0.5;
                        break;
                    case "2":
                        scale = 2;
                        break;
                    case "3":
                        scale = 3;
                        break;
                    case "4":
                        ResetRecipeQuantities(recipe);
                        return;
                    default:
                        Console.WriteLine("Invalid option. No scaling applied.");
                        return;
                }

                recipe.ScaleIngredients(scale);
                Console.WriteLine($"Recipe scaled by a factor of {scale}.");
                DisplayFullRecipe(recipe);  // Display the full recipe after scaling
            }

            // ------------------------------
            // Method to reset the ingredients of a recipe to their original quantities
            // ------------------------------
            static void ResetRecipeQuantities(Recipe recipe)
            {
                recipe.ResetIngredients();
                Console.WriteLine("Recipe quantities have been reset to original values.");
                DisplayFullRecipe(recipe);  // Display the full recipe after resetting
            }

            // ------------------------------
            // Method to delete a recipe from the collection
            // ------------------------------
            static void DeleteRecipe(string recipeName)
            {
                if (recipes.ContainsKey(recipeName))
                {
                    recipes.Remove(recipeName);
                    Console.WriteLine($"Recipe '{recipeName}' has been deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Recipe not found.");
                }
            }

            // ------------------------------
            // Method to clear all recipes from the collection after user confirmation
            // ------------------------------
            static void ClearAllRecipes()
            {
                // Ask for confirmation before clearing
                Console.WriteLine("Are you sure you want to clear all recipes? (yes/no)");
                string confirmation = Console.ReadLine();

                if (confirmation.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    recipes.Clear();
                    Console.WriteLine("All recipes have been cleared.");
                }
                else
                {
                    Console.WriteLine("Clear operation canceled.");
                }
            }

            // ------------------------------
            // Method to filter recipes based on a keyword using LINQ
            // ------------------------------
            static void FilterRecipes()
            {
                Console.WriteLine("Enter a keyword to filter recipes (by ingredient name or recipe name):");
                string keyword = Console.ReadLine().ToLower();

                var filteredRecipes = recipes.Values
                    .Where(r => r.Name.ToLower().Contains(keyword) || r.Ingredients.Any(i => i.Name.ToLower().Contains(keyword)))
                    .ToList();

                if (filteredRecipes.Count == 0)
                {
                    Console.WriteLine("No recipes match your search criteria.");
                    return;
                }

                Console.WriteLine("Filtered Recipes:");
                foreach (var recipe in filteredRecipes)
                {
                    DisplayFullRecipe(recipe);
                }
            }

            // ------------------------------
            // Method to sort recipes by total calories using LINQ
            // ------------------------------
            static void SortRecipesByCalories()
            {
                var sortedRecipes = recipes.Values
                    .OrderBy(r => r.CalculateTotalCalories())
                    .ToList();

                Console.WriteLine("Recipes sorted by total calories:");
                foreach (var recipe in sortedRecipes)
                {
                    DisplayFullRecipe(recipe);
                }
            }
        }
    }

}
