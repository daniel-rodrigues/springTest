﻿using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using SpringTest.Core.EntityMapConfiguration;
using SpringTest.Domain.Entities;

namespace SpringTest.Core.EfContext {
	public class EfDbContext : DbContext {
		public EfDbContext() : base("SpringTestConnection") {
			Configuration.LazyLoadingEnabled = false;
			Configuration.ProxyCreationEnabled = true;
		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
			modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

			modelBuilder.Properties<string>()
					.Configure(p => p.HasColumnType("varchar"));

			modelBuilder.Properties<DateTime>()
				.Configure(p => p.HasColumnType("datetime2"));

			modelBuilder.Properties<string>()
					.Configure(p => p.HasMaxLength(50));

			modelBuilder.Entity<Category>()
				.HasKey(x => x.Id)
				.HasMany(x => x.Products);

			modelBuilder.Entity<Product>()
				.HasRequired(x => x.Category)
				.WithMany(x => x.Products)
				.HasForeignKey(x => x.CategoryId);

			modelBuilder.Configurations.Add(new CategoryConfiguration());
			modelBuilder.Configurations.Add(new ProductConfiguration());
		}		
	}
}
