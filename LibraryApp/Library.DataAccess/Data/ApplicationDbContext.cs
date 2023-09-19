namespace Library.DataAccess.Data
{
	using Library.Models;
	using Microsoft.EntityFrameworkCore;

	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "History" },
				new Category { Id = 2, Name = "Action" },
				new Category { Id = 3, Name = "Sci-Fi" }
			);

			modelBuilder.Entity<Product>().HasData(
				new Product
				{
					Id = 1,
					Title = "Fortune of Time",
					Author = "Billy Spark",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "SWD9999001",
					Price = 90
				},
				new Product
				{
					Id = 2,
					Title = "Dark Skies",
					Author = "Nancy Hoover",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "CAW777777701",
					Price = 30
				},
				new Product
				{
					Id = 3,
					Title = "Vanish in the Sunset",
					Author = "Julian Button",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "RITO5555501",
					Price = 50
				},
				new Product
				{
					Id = 4,
					Title = "Cotton Candy",
					Author = "Abby Muscles",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "WS3333333301",
					Price = 65
				},
				new Product
				{
					Id = 5,
					Title = "Rock in the Ocean",
					Author = "Ron Parker",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "SOTJ1111111101",
					Price = 27
				},
				new Product
				{
					Id = 6,
					Title = "Leaves and Wonders",
					Author = "Laura Phantom",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "FOT000000001",
					Price = 23
				}
			);
		}
	}
}
