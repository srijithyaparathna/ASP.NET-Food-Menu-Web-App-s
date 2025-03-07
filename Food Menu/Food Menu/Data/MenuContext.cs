
//Required namespace for using Entity Framework Core and accessing models
using Microsoft.EntityFrameworkCore;
using Menu.Models;
using Food_Menu.Models;

namespace Food_Menu.Data
{
    // MenuContedxt is the database context that connects the application to the database 
    public class MenuContext : DbContext
    {

        // Constructor : Accepts databse options (like connection string) and passes them to the base DbContext
        public MenuContext(DbContextOptions<MenuContext> options) : base(options)
        { 
        }

        // Method to configure entity relationships and seed initial data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Define the composite primary key for the many-to-many relationship
            modelBuilder.Entity<DishIngredient>().HasKey(di => new
            { 
                di.DishId, // Primary key part 1
                di.IngredientId // Primary key part 2 
            
            }
            );

            // Define the relationship: One Dish can have many DishIngredients
            modelBuilder.Entity<DishIngredient>()
                .HasOne(d => d.Dish) // Each DishIngredient has one Dish
                .WithMany(di => di.DishIngredients) // Each Dish can have many DishIngredients
                .HasForeignKey(d => d.DishId); // Foreign key reference to Dish


            // Define the relationship : One Ingredient can have many DIshIngredients
            modelBuilder.Entity<DishIngredient>()
                .HasOne(i => i.Ingredient)  // Each DishIngredient has one Ingredient
                .WithMany(di => di.DishIngredients) // Each Ingredient can be in many DishIngredients
                .HasForeignKey(i => i.IngredientId); // Foreign key reference to Ingredient


            // Seed Data: Add initial data for Dish table
            modelBuilder.Entity<Dish>().HasData(
                new Dish
                {
                    Id = 1,
                    Name = "Margheritta",
                    Price = 7.50,
                    ImageUrl = "https://cdn.shopify.com/s/files/1/0205/9582/articles/20220211142347-margherita-9920_ba86be55-674e-4f35-8094-2067ab41a671.jpg?crop=center&height=915&v=1644590192&width=1200"
                }
            );


            // Seed Data: Add initial data for Ingredient table
            modelBuilder.Entity<Ingredient>().HasData(
                new Ingredient { Id = 1, Name = "Tomato Sauce" },
                new Ingredient { Id = 2, Name = "Mozzarella" }
            );

            // Seed Data: Link Dish (Margheritta) with its Ingredients using DishIngredient table
            modelBuilder.Entity<DishIngredient>().HasData(
                new DishIngredient { DishId = 1, IngredientId = 1 }, // Margheritta contains Tomato Sauce
                new DishIngredient { DishId = 1, IngredientId = 2 }  // Margheritta contains Mozzarella
            );

            // Call the base method to complete entity configuration.
            base.OnModelCreating(modelBuilder);


        }




        // Define database tables using DbSet properties.
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }






    }
}


/* 
 What is the Database Relationships
1. Establishing Database Relationships
    This code defines a many-to-many relationship between Dish and Ingredient using the
    DishIngredient table
    It explicitly sets composite primary keys (DishId,IngredientId) for DishIngredient
2. Seeding initial Data
    It pre-population the database with
        One dish: margherita Pizza(dish tabel)
        Two ingredients:Tomato Sauce and Mozzarella(Ingredient table)
        Relationship:Margherita pizza contains both ingredients(DishIngredient table)
3. Database Context Definition
    DbSet<Dish> → Represents the Dishes table.
    DbSet<Ingredient> → Represents the Ingredients table.
    DbSet<DishIngredient> → Represents the DishIngredients linking table.
 */