// ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using ProdutosMVC.Models; // Adjust this using directive based on your actual models namespace

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}
