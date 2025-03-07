
// Define the namespace for this class, meaning it belongs to the "Menu.Models" namespace.
using Menu.Models;

namespace Food_Menu.Models
{

    // This class acts as a junvtion table(linking table) to connect Dishes and ingredients
    public class DishIngredient
    {

        // Foreign Key : Indentifies the associated dish
        public int DishId { get; set; }

        // Navigation Property : References the related Dish entity
        public Dish Dish { get; set; }

        // Foreign Key: Identifies the associated Ingredient.
        public int IngredientId { get; set; }

        // Navigation Property: References the related Ingredient entity.
        public Ingredient Ingredient { get; set; }

    }
}
