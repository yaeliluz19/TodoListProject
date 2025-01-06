// // ﻿using System;
// // using System.Collections.Generic;
// // using Microsoft.EntityFrameworkCore;
// // using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

// // namespace TodoApi;

// // public partial class ToDoDbContext : DbContext
// // {
// //     public ToDoDbContext()
// //     {
// //     }

// //     public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
// //         : base(options)
// //     {
// //     }

// //     public virtual DbSet<Item> Items { get; set; }

// //     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// //         => optionsBuilder.UseMySql("name=ToDodb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql"));

// //     protected override void OnModelCreating(ModelBuilder modelBuilder)
// //     {
// //         modelBuilder
// //             .UseCollation("utf8mb4_0900_ai_ci")
// //             .HasCharSet("utf8mb4");

// //         modelBuilder.Entity<Item>(entity =>
// //         {
// //             entity.HasKey(e => e.Id).HasName("PRIMARY");

// //             entity.ToTable("items");

// //             entity.Property(e => e.Name).HasMaxLength(100);
// //         });

// //         OnModelCreatingPartial(modelBuilder);
// //     }

// //     partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
// // }
// using System;
// using System.Collections.Generic;
// using Microsoft.EntityFrameworkCore;
// using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

// namespace TodoApi;

// public partial class ToDoDbContext : DbContext
// {
//     // DbSet של משתמשים
//     public virtual DbSet<User> Users { get; set; }

//     public virtual DbSet<Item> Items { get; set; }

//     public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
//         : base(options)
//     {
//     }

//     // הגדרת חיבור למסד הנתונים
//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// {
//     Console.WriteLine("OnConfiguring is being called"); // לוג פשוט


//     try
// {
//     var connectionString = "Server=bd2ivxjpmx5a7gpoxc5j-mysql.services.clever-cloud.com;User=uyb9xnwwuyygrbpc;Password=crvceLArTRBtAmA8rhKG;Database=bd2ivxjpmx5a7gpoxc5j;";
//     //Configuration.GetConnectionString("ToDoDB");
//     optionsBuilder.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql"));
//     Console.WriteLine("success");
// }
// catch (Exception ex)
// {
//     Console.WriteLine($"Database connection failed: {ex.Message}");
//     Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
// }


// }

//     // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     //     => optionsBuilder.UseMySql("name=ToDodb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql"));

//     // אפיון טבלאות ותכונות
//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         modelBuilder
//             .UseCollation("utf8mb4_0900_ai_ci")
//             .HasCharSet("utf8mb4");

//         // הגדרת טבלת Items
//         modelBuilder.Entity<Item>(entity =>
//         {
//             entity.HasKey(e => e.Id).HasName("PRIMARY");

//             entity.ToTable("items");

//             entity.Property(e => e.Name).HasMaxLength(100);
//         });

//         // הגדרת טבלת Users (למשתמשים)
//         modelBuilder.Entity<User>(entity =>
//         {
//             entity.HasKey(e => e.Id).HasName("PRIMARY");

//             entity.ToTable("users");

//             entity.Property(e => e.Username).HasMaxLength(50).IsRequired();
//             entity.Property(e => e.Password).HasMaxLength(255).IsRequired();
//         });

//         OnModelCreatingPartial(modelBuilder);
//     }

//     partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
// }
using System;
using Microsoft.EntityFrameworkCore;

namespace TodoApi
{
    public partial class ToDoDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Item> Items { get; set; }

        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine("OnConfiguring is being called");

            try
            {
                var connectionString = "Server=bd2ivxjpmx5a7gpoxc5j-mysql.services.clever-cloud.com;User=uyb9xnwwuyygrbpc;Password=crvceLArTRBtAmA8rhKG;Database=bd2ivxjpmx5a7gpoxc5j;";
                optionsBuilder.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql"));
                Console.WriteLine("Database connection success");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection failed: {ex.Message}");
                throw; // Optionally rethrow for further handling
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
                entity.ToTable("items");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
                entity.ToTable("users");
                entity.Property(e => e.Username).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Password).HasMaxLength(255).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    // Example User class
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // Example Item class
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
