using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroNations_Finale
{
    public class CPartitaGirone : CPartita
    {
        public override string Fase
        {
            get
            {
                return "Girone";
            }
        }

        public CPartitaGirone(int Giornata, string nCasa, string nOspite, int gCasa, int gOspite) : base(Giornata, nCasa, nOspite, gCasa, gOspite) { }

        public override int PuntiPer(string squadra)
        {
            bool casa = squadra.ToLower() == nCasa.ToLower();
            bool ospite = squadra.ToLower() == nOspite.ToLower();

            if (!casa && !ospite)
            {
                throw new ArgumentException("La squadra selezionata non ha giocato questa partita");
            }
            Esito esito = CalcolaEsito();

            if (esito == Esito.Pareggio)
            {
                return 1;
            }
            else if (esito == Esito.VittoriaCasa)
            {
                return casa ? 3 : 0;
            }
            else
            {
                return ospite ? 3 : 0;
            }

        }
    }
}
