using Food_Menu.Data;
using Food_Menu.Models;
using Food_Menu.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Menu.Controllers
{
    public class MenuController : Controller
    {
        private readonly MenuContext _context;
        private readonly IWebHostEnvironment _environment;

        // Inject both the database context and the web hosting environment.
        public MenuController(MenuContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: /Menu/Index
        // Retrieves all dishes and displays them.
        public async Task<IActionResult> Index()
        {
            var dishes = await _context.Dishes.ToListAsync();
            return View(dishes);
        }

        // GET: /Menu/Details/{id}
        // Retrieves a specific dish with its ingredients.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(d => d.DishIngredients)          // Load the junction records.
                .ThenInclude(di => di.Ingredient)           // Load each ingredient.
                .FirstOrDefaultAsync(m => m.Id == id);

            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: /Menu/Create
        // Loads the form to create a new dish.
        public async Task<IActionResult> Create()
        {
            // Load available ingredients to show in the form.
            var ingredients = await _context.Ingredients.ToListAsync();

            var model = new CreateDishViewModel
            {
                Ingredients = ingredients
            };

            return View(model);
        }

        // POST: /Menu/Create
        // Processes form submission to create a new dish.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDishViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload if an image file was provided.
                string imageUrl = "";
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    // Define the folder to store images (e.g., wwwroot/images/dishes)
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "dishes");

                    // Create the folder if it doesn't exist.
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Create a unique file name to avoid collisions.
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }
                    // Set the relative URL for the image.
                    imageUrl = "/images/dishes/" + uniqueFileName;
                }

                // Create a new Dish entity from the view model data.
                var dish = new Dish
                {
                    Name = model.Name,
                    Price = model.Price,
                    ImageUrl = imageUrl
                };

                // Save the Dish first to generate its ID.
                _context.Dishes.Add(dish);
                await _context.SaveChangesAsync();

                // Create a DishIngredient record for each selected ingredient.
                foreach (var ingId in model.SelectedIngredientIds)
                {
                    var dishIngredient = new DishIngredient
                    {
                        DishId = dish.Id,
                        IngredientId = ingId
                    };
                    _context.Add(dishIngredient);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If model validation fails, reload available ingredients.
            model.Ingredients = await _context.Ingredients.ToListAsync();
            return View(model);
        }

        // GET: /Menu/Edit/{id}
        // Loads the dish for editing.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Optionally include related ingredients if needed.
            var dish = await _context.Dishes
                        .Include(d => d.DishIngredients)
                        .FirstOrDefaultAsync(d => d.Id == id);

            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        // POST: /Menu/Edit/{id}
        // Processes the edited dish details.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Dish dish)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: /Menu/Delete/{id}
        // Loads the confirmation page to delete a dish.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        // POST: /Menu/Delete/{id}
        // Deletes the dish after confirmation.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if a dish exists.
        private bool DishExists(int id)
        {
            return _context.Dishes.Any(d => d.Id == id);
        }
    }
}
