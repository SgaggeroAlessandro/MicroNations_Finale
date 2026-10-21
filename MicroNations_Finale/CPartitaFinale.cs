using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroNations_Finale
{
    public class CPartitaFinale : CPartitaEliminazione
    {
        public override string Fase
        {
            get
            {
                return "Finale";
            }
        }

        public CPartitaFinale(int Giornata, string nCasa, string nOspite, int gCasa, int gOspite) : base(Giornata, nCasa, nOspite, gCasa, gOspite) { }
        public int PesoStorico()
        {
            return CalcolaGolTotali() * 2;
        }
    }
}
