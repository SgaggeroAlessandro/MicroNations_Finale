using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroNations_Finale
{
    public class StatisticheSquadra : IEquatable<StatisticheSquadra>, IComparable<StatisticheSquadra>
    {
        public string Nome { get; set; }
        public int Giocate { get; set; }
        public int Vinte { get; set; }
        public int Pareggiate { get; set; }
        public int Perse { get; set; }
        public int GolFatti { get; set; }
        public int GolSubiti { get; set; }
        public int Punti { get; set; }

        public StatisticheSquadra(string Nome)
        {
            this.Nome = Nome;
        }

        public int CalcolaDifferenzaReti()
        {
            return GolFatti - GolSubiti;
        }

        public bool Equals(StatisticheSquadra other)
        {
            if (other == null)
            {
                return false;
            }
            return Nome.ToLower() == other.Nome.ToLower();
        }
        public int CompareTo(StatisticheSquadra other)
        {
            if (other == null)
            {
                return 1;
            }
            int confrontaPunti = other.Punti.CompareTo(this.Punti);
            if (confrontaPunti != 0)
            {
                return confrontaPunti;
            }

            int confrontaDifferenza = other.CalcolaDifferenzaReti().CompareTo(this.CalcolaDifferenzaReti());
            if (confrontaDifferenza != 0)
            {
                return confrontaDifferenza;
            }

            int confrontaGol = other.GolFatti.CompareTo(this.GolFatti);
            if (confrontaGol != 0)
            {
                return confrontaGol;
            }
            return this.Nome.ToLower().CompareTo(other.Nome.ToLower());
        }
    }
}
