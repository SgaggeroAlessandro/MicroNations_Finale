using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroNations_Finale
{
    public class PartitaPublic
    {
        public string Tipo { get; set; }
        public int Giornata { get; set; }
        public string SquadraCasa { get; set; }
        public string SquadraOspite { get; set; }
        public int GolCasa { get; set; }
        public int GolOspite { get; set; }
        public int? RigoriCasa { get; set; }
        public int? RigoriOspite { get; set; }
    }
}
