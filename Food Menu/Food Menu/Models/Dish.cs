

// Define the namespace for the class, indicating that it belongs to the "Menu.Models" namespace.
namespace Food_Menu.Models
{

    // This class represents identifier for each dish in the database
   
    public class Dish
    {
        // Primary Key : Unique identifier for each dish in the database 
        public int Id { get; set; }

        // Name of the dish
        public string Name { get; set; }

        // URL for an image of the dish

        public string ImageUrl { get; set; }

        // price of the dish
        public double Price { get; set; }

        // Navigation property: Represents the relationship between Dish and DishIngredient
        // A dish can have multiple ingredients
        // Nullable means the property can be null

        public List<DishIngredient>? DishIngredients { get;set; }



    }
}
