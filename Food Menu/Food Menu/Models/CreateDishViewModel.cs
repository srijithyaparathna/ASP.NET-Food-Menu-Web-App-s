using System.ComponentModel.DataAnnotations;
using Food_Menu.Models; // For Ingredient
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Menu.Models;

namespace Food_Menu.ViewModels
{
    public class CreateDishViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        // This property will hold the uploaded image file.
        public IFormFile? ImageFile { get; set; }

        // List of ingredient IDs selected by the user.
        public List<int> SelectedIngredientIds { get; set; } = new List<int>();

        // List of available ingredients to display in the form.
        public List<Ingredient>? Ingredients { get; set; }
    }
}
