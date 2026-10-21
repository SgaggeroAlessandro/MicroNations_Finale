using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroNations_Finale
{
    public class CPartitaEliminazione : CPartita
    {
        public override string Fase
        {
            get
            {
                return "Fase ad eliminazione diretta";
            }
        }
        private int? rigoriCasa;
        public int? RigoriCasa
        {
            get
            {
                return rigoriCasa;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("I rigori segnati dalla squadra di casa non possono essere negativi");
                if (value != null && CalcolaEsito() != Esito.Pareggio)
                    throw new ArgumentException("I rigori vengono effettuati solo in caso di parità");
                rigoriCasa = value;
            }
        }

        private int? rigoriOspite;

        public int? RigoriOspite
        {
            get
            {
                return rigoriOspite;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("I rigori segnati dalla squadra ospite non possono essere negativi");
                if (value != null && CalcolaEsito() != Esito.Pareggio)
                    throw new ArgumentException("I rigori vengono effettuati solo in caso di parità");
                rigoriOspite = value;
            }
        }

        public CPartitaEliminazione(int Giornata, string nCasa, string nOspite, int gCasa, int gOspite) : base(Giornata, nCasa, nOspite, gCasa, gOspite) { }
        public override int PuntiPer(string squadra)
        {
            return 0;
        }

        public bool RigoriValidi()
        {
            if (CalcolaEsito() != Esito.Pareggio)
            {
                return true;
            }

            if (RigoriCasa == null || RigoriOspite == null)
            {
                return false;
            }
            if (RigoriCasa == RigoriOspite)
            {
                return false;
            }
            return true;
        }

        public override string? CalcolaVincitore()
        {
            if (CalcolaEsito() != Esito.Pareggio)
            {
                return base.CalcolaVincitore();
            }

            if (RigoriCasa == null || RigoriOspite == null || RigoriOspite == RigoriCasa)
            {
                return null;
            }

            return RigoriCasa > RigoriOspite ? nCasa : nOspite;
        }

        public override string Descrizione()
        {

            string testo = base.Descrizione();
            if (CalcolaEsito() == Esito.Pareggio && RigoriCasa != null && RigoriOspite != null)
            {
                testo += $"d.c.r {RigoriCasa} - {RigoriOspite}, passa {CalcolaVincitore()}";
            }
            return testo;
        }

    }
}
