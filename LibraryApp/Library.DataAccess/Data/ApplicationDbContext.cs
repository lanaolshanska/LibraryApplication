﻿namespace Library.DataAccess.Data
{
	using Library.Models;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;

	public class ApplicationDbContext : IdentityDbContext<IdentityUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		public DbSet<ShoppingCart> ShoppingCarts { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderProduct> OrderProduct { get; set; }
		public DbSet<UserAddress> UserAddress { get; set; }
		public DbSet<ShipmentDetail> ShipmentDetail { get; set; }
		public DbSet<PaymentDetail> PaymentDetail { get; set; }
		public DbSet<ProductImage> ProductImage { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ApplicationUser>()
			.HasMany(e => e.Addresses)
			.WithOne(e => e.ApplicationUser)
			.HasForeignKey(e => e.ApplicationUserId)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.IsRequired();

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
					Price = 90,
					CategoryId = 1
				},
				new Product
				{
					Id = 2,
					Title = "Dark Skies",
					Author = "Nancy Hoover",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "CAW777777701",
					Price = 30,
					CategoryId = 2
				},
				new Product
				{
					Id = 3,
					Title = "Vanish in the Sunset",
					Author = "Julian Button",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "RITO5555501",
					Price = 50,
					CategoryId = 3
				},
				new Product
				{
					Id = 4,
					Title = "Cotton Candy",
					Author = "Abby Muscles",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "WS3333333301",
					Price = 65,
					CategoryId = 1
				},
				new Product
				{
					Id = 5,
					Title = "Rock in the Ocean",
					Author = "Ron Parker",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "SOTJ1111111101",
					Price = 27,
					CategoryId = 2
				},
				new Product
				{
					Id = 6,
					Title = "Leaves and Wonders",
					Author = "Laura Phantom",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "FOT000000001",
					Price = 23,
					CategoryId = 3
				}
			);
		}
	}
}
