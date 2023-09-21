using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace Database;
public class DatabaseContext : DbContext
{
    private object options;

    public DatabaseContext(object options)
    {
        this.options = options;
    }

    public DbSet<Gebruiker> Gebruikers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Hier configureer je de verbinding met SQL Server
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Administratie;Integrated Security=true";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Hier kun je de Fluent API-configuratie voor je model toevoegen, indien nodig
        modelBuilder.Entity<Gebruiker>().ToTable("Gebruikers");

    }
}
[Table("Gebruikers")]
public class Gebruiker
{
    [Key]
    public int GebruikerId { get; set; }
    
    [Column("Naam", TypeName = "varchar(50)")]
    [MaxLength(50)]
    public string Naam { get; set; }

    [Column("Email", TypeName = "varchar(50)")]
    [MaxLength(50)]
    public string Email { get; set; }

    public Gebruiker(string naam, string email)
    {
        Naam = naam;
        Email = email;
    }
}
public class Gast: Gebruiker
{
    [Column("Credits", TypeName = "int(50)")]
    public int Credits { get; set; }
    [Column("GeboorteDatum", TypeName = "DateTime")]
    public DateTime GeboorteDatum { get; set; }
    [Column("EersteBezoek", TypeName = "DateTime")]
    public DateTime EersteBezoek { get; set; }
    public Gast(string naam, string email) : base(naam, email)
    {
        Naam = naam;
        Email = email;
    }
}
public class Medewerker: Gebruiker
{
    //public string? Functie { get; set; }
    public Medewerker(string naam, string email) : base(naam, email)
    {
    }
}
