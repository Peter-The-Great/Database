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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Gebruiker>().HasData(new Post { BlogId = 1, PostId = 1, Title = "First post", Content = "Test 1" });
        modelBuilder
            .Entity<Gast>()
            .HasMany(e => e.Reserveringen)
            .WithOne(e => e.Gast)
            .OnDelete(DeleteBehavior.ClientSetNull);
        modelBuilder.Entity<Onderhoud>()
             .HasMany(b => b.Coordinators)
             .WithMany(b => b.Coordineert);
        modelBuilder.Entity<Onderhoud>()
             .HasMany(b => b.Medewerker)
             .WithMany(b => b.Doet);
        // test dit!
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
      => options.UseSqlServer("server=.;database=Administratie;User Id=userman; Password=eigeel;TrustServerCertificate=True;");
    //(localdb)\\MSSQLLocalDB;Initial Catalog=Administratie;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
    //options.UseSqlite($"Data Source=C:\\Users\\InstallUser\\source\\repos\\ORMLINQ\\ORMLINQ\\data.db");
}