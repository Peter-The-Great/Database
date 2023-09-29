using System.ComponentModel.DataAnnotations.Schema;

namespace Database;
[Owned]
class Coordinaat
{
    public int x { get; set; }
    public int y { get; set; }
}

class GastInfo
{
    public int Id { get; set; }
    public Coordinaat Coordinaat { get; set; } = new Coordinaat();
    public string LaatstBezochteURL { get; set; } = "";
    public Gast Gast { get; set; }
    protected GastInfo()
    {
        Gast = null!;
    }
    public GastInfo(Gast gast)
    {
        Gast = gast;
    }
}

[Table("Gebruiker")]
class Gebruiker
{
    public int Id { get; set; }
    public string Email { get; set; }
    public Gebruiker(string email)
    {
        Email = email;
    }
}

[Table("Gast")]
class Gast : Gebruiker
{
    //public int Id { get; set; }
    public Gast? Begeleider { get; set; }
    public GastInfo GastInfo { get; set; }
    public int GastInfoId { get; set; }
    public int Credits { get; set; }
    public DateTime GeboorteDatum { get; set; }
    public DateTime EersteBezoek { get; set; }
    public Attractie? Favoriet { get; set; }
    public IEnumerable<Reservering> Reserveringen { get; set; } = new List<Reservering>();
    public Gast(string email) : base(email)
    {
        GastInfo = new GastInfo(this);
    }
}

[Table("Medewerker")]
class Medewerker : Gebruiker
{
    public IEnumerable<Onderhoud> Doet { get; set; } = new List<Onderhoud>();
    public IEnumerable<Onderhoud> Coordineert { get; set; } = new List<Onderhoud>();
    public Medewerker(string email) : base(email)
    {
    }
}

class Onderhoud
{
    public int Id { get; set; }
    public DateTimeBereik DateTimeBereik { get; set; }
    public string Probleem { get; set; }
    public IEnumerable<Medewerker> Medewerker { get; set; } = new List<Medewerker>();
    public IEnumerable<Medewerker> Coordinators { get; set; } = new List<Medewerker>();
    public Attractie Attractie { get; set; }
    protected Onderhoud()
    {
        DateTimeBereik = null!;
        Probleem = null!;
        Attractie = null!;
    }
    public Onderhoud(DateTimeBereik dateTimeBereik, string probleem, Attractie attractie)
    {
        DateTimeBereik = dateTimeBereik;
        Probleem = probleem;
        Attractie = attractie;
    }
}

[Owned]
public class DateTimeBereik
{
    public DateTimeBereik(DateTime begin, TimeSpan span)
    {
        Begin = begin;
        Eind = begin + span;
    }
    public DateTimeBereik(DateTime begin, DateTime? eind)
    {
        Begin = begin;
        Eind = eind;
    }
    public DateTime Begin { get; set; }
    public DateTime? Eind { get; set; }
    public bool Eindigt => Eind != null;
    public bool Overlapt(DateTimeBereik that)
    {
        return (Eind != null && that.Eind != null && this.Begin < that.Eind && that.Begin < this.Eind) ||
               (Eind == null && that.Eind != null && this.Begin < that.Eind) ||
               (Eind != null && that.Eind == null && that.Begin < this.Eind) ||
               (Eind == null && that.Eind == null);
    }
}

class Reservering
{
    public int Id { get; set; }
    public DateTimeBereik DateTimeBereik { get; set; }
    public Gast? Gast { get; set; }
    public Attractie Attractie { get; set; }
    protected Reservering()
    {
        DateTimeBereik = null!;
        Gast = null!;
        Attractie = null!;
    }
    public Reservering(DateTimeBereik dateTimeBereik, Gast gast, Attractie attractie)
    {
        DateTimeBereik = dateTimeBereik;
        Gast = gast;
        Attractie = attractie;
    }
}

class Attractie
{
    public readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);
    public int Id { get; set; }
    public string Naam { get; set; }
    // public int Capaciteit { get; set; } // <- dit is erg moeilijk
    public IEnumerable<Reservering> Reserveringen { get; set; } = new List<Reservering>();
    public IEnumerable<Onderhoud> Onderhoud { get; set; } = new List<Onderhoud>();
    public Attractie(string naam)
    {
        Naam = naam;
    }
    public async Task<bool> OnderhoudBezig(DatabaseContext context)
    {
        await context.Entry(this).Collection(a => a.Onderhoud).LoadAsync();
        return Onderhoud.Any(o => !o.DateTimeBereik.Eindigt);
    }
    public async Task<bool> Vrij(DatabaseContext context, DateTimeBereik dateTimeBereik)
    {
        await context.Entry(this).Collection(a => a.Reserveringen).LoadAsync();
        foreach (Reservering reservering in Reserveringen)
            if (reservering.DateTimeBereik.Overlapt(dateTimeBereik))
                return false;
        return true;
    }
}