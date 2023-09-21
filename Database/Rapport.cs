using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapport
{
    abstract class Rapport
    {
        public abstract string Naam();
        public abstract Task<string> Genereer();
        public async Task VoerUit() => await File.WriteAllTextAsync(Naam() + ".txt", await Genereer());
        public async Task VoerPeriodiekUit(Func<bool> stop)
        {
            while (!stop())
            {
                await VoerUit();
                await Task.Delay(1000);
            }
        }
    }
}
