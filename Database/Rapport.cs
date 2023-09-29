using System.Linq;

namespace Database;
abstract class Rapport
{
    public abstract string Naam();
    public abstract Task<string> Genereer();
    public async Task VoerUit()
    {
        await File.WriteAllTextAsync(Naam() + ".txt", await Genereer());
    }
    public async Task VoerPeriodiekUit(Func<bool> stop)
    {
        while (!stop())
        {
            await VoerUit();
            await Task.Delay(1000);
        }
    }
}

class DemografischRapport : Rapport
{
    private DatabaseContext context;
    public DemografischRapport(DatabaseContext context)
    {
        this.context = context;
    }
    public override string Naam() => "Demografie";
    public override async Task<string> Genereer()
    {
        string ret = "Dit is een demografisch rapport: \n";
        ret += $"Er zijn in totaal {await AantalGebruikers()} gebruikers van dit platform (dat zijn gasten en medewerkers)\n";
        var dateTime = new DateTime(2000, 1, 1);
        ret += $"Er zijn {await AantalSinds(dateTime,false)} bezoekers sinds {dateTime}\n";
        if (await AlleGastenHebbenReservering())
            ret += "Alle gasten hebben een reservering\n";
        else
            ret += "Niet alle gasten hebben een reservering\n";
        ret += $"Het percentage bejaarden is {await PercentageBejaarden()}\n";

        ret += $"De oudste gast heeft een leeftijd van {await HoogsteLeeftijd()} \n";

        ret += "De verdeling van de gasten per dag is als volgt: \n";
        var dagAantallen = await VerdelingPerDag();
        var totaal = dagAantallen.Select(t => t.aantal).Max();
        foreach (var dagAantal in dagAantallen)
            ret += $"{dagAantal.dag}: {new string('#', (int)(dagAantal.aantal / (double)totaal * 20))}\n";

        ret += $"{await FavorietCorrect()} gasten hebben de favoriete attractie inderdaad het vaakst bezocht. \n";

        return ret;
    }
    private async Task<int> AantalGebruikers() => await context.Gebruikers.CountAsync();
    private async Task<bool> AlleGastenHebbenReservering() => !await context.Gasten.AnyAsync(g => g.Reserveringen.Count() == 0);
    private async Task<int> AantalSinds(DateTime sinds, bool uniek) => await ((Func<IQueryable<Gast>, IQueryable<Gast>>)(uniek ? (x) => x.Distinct() : (x) => x))(context.Gasten.Where((Gast g) => g.EersteBezoek > sinds)).CountAsync();
    private async Task<Gast?> GastBijEmail(string email) => await context.Gasten.FirstOrDefaultAsync(g => g.Email == email);
    private async Task<Gast> GastBijGeboorteDatum(DateTime d) => await context.Gasten.SingleAsync(g => g.GeboorteDatum == d);
    private async Task<double> PercentageBejaarden() => await context.Gasten.Select(g => EF.Functions.DateDiffDay(g.GeboorteDatum, DateTime.Now) > 365 * 80 ? 0 : 1).AverageAsync();
    private async Task<int> HoogsteLeeftijd() => (int)await context.Gasten.MaxAsync(g => EF.Functions.DateDiffDay(g.GeboorteDatum, DateTime.Now) / 365.25);
    private async Task<(string dag, int aantal)[]> VerdelingPerDag() =>
        (await context
        .Gasten
        .ToListAsync())
        .GroupBy(g => g.EersteBezoek.DayOfWeek)
        .Select(g => new { dag = g.Key.ToString(), aantal = g.Count() })
        .Select(g => (g.dag, g.aantal))
        .ToArray();
    private async Task<int> FavorietCorrect() => await context.Gasten.Where(g => g.Favoriet != null && g.Favoriet == g.Reserveringen.GroupBy(b => b.Attractie).OrderByDescending(g => g.Count()).First().Key).CountAsync();
    // await context.Gasten.Where(g => g.Favoriet != null).Join()
}