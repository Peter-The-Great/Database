namespace Database;
class DatabaseContext : DbContext
{
    public DbSet<Attractie> Attracties => Set<Attractie>();
    public DbSet<Gebruiker> Gebruikers => Set<Gebruiker>();
    public DbSet<Gast> Gasten => Set<Gast>();
    public DbSet<Medewerker> Medewerkers => Set<Medewerker>();
    public DbSet<Reservering> Reserveringen => Set<Reservering>();
    public DbSet<Onderhoud> Onderhoud => Set<Onderhoud>();

    public async Task<bool> Boek(Gast gast, Attractie attractie, DateTimeBereik dateTimeBereik)
    {
        await attractie.Semaphore.WaitAsync();
        try
        {
            if (!await attractie.OnderhoudBezig(this) && await attractie.Vrij(this, dateTimeBereik))
            {
                Database.BeginTransaction();
                gast.Credits--;
                Reserveringen.Add(new Reservering(dateTimeBereik, gast, attractie));
                SaveChanges();
                Database.CommitTransaction();
                return true;
            }
        }
        finally
        {
            attractie.Semaphore.Release();
        }
        return false;
    }
    protected void OnmodelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
        .Entity<Gast>()
        .HasMany(g => g.Reserveringen)
        .WithOne(g => g.Gast)
        .OnDelete(DeleteBehavior.ClientSetNull);
        modelBuilder.Entity<Onderhoud>()
        .HasMany(r => r.Coordinators)
        .WithMany(r => r.Coordineert);
        modelBuilder.Entity<Onderhoud>()
        .HasMany(r => r.Medewerkers)
        .WithMany(r => r.Doet);
        //Must be tested!
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Administratie;Integrated Security=True");
    }
}
