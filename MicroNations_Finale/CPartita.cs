using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroNations_Finale
{
    public abstract class CPartita : IRisultato, IStampabile, IEquatable<CPartita>, IComparable<CPartita>
    {
        public abstract string Fase { get; }

        public static string Criterio = "naturale";
        private int giornata;
        public int Giornata
        {
            get
            {
                return giornata;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("La giornata non può essere negativa o nulla");
                giornata = value;
            }
        }

        private string ncasa;
        public string nCasa
        {
            get
            {
                return ncasa;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Il nome della squadra di casa non può essere costituito da soli spazi");
                if (nospite != null && value.ToLower() == nospite.ToLower())
                    throw new ArgumentException("Una squadra non può giocare contro se stessa");
                ncasa = value;
            }
        }

        private string nospite;

        public string nOspite
        {
            get
            {
                return nospite;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Il nome della squadra ospite non può essere costituito da soli spazi");
                if (ncasa != null && value.ToLower() == ncasa.ToLower())
                    throw new ArgumentException("Una squadra non può giocare contro se stessa");

                nospite = value;
            }
        }

        private int gcasa;
        public int gCasa
        {
            get
            {
                return gcasa;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("I gol della squadra di casa non possono essere negativi");
                gcasa = value;
            }
        }

        private int gospite;

        public int gOspite
        {
            get
            {
                return gospite;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("I goal della squadra ospite non posso essere negativi");
                gospite = value;
            }
        }

        public CPartita(int Giornata, string nCasa, string nOspite, int gCasa, int gOspite)
        {
            this.Giornata = Giornata;
            this.nCasa = nCasa;
            this.nOspite = nOspite;
            this.gCasa = gCasa;
            this.gOspite = gOspite;
        }

        public abstract int PuntiPer(string squadra);

        public virtual string Descrizione()
        {
            return $"Giornata: {Giornata} \n{nCasa} {gCasa} - {gOspite} {nOspite}";
        }
        public int CalcolaGolTotali()
        {
            return gCasa + gOspite;
        }

        public int CalcolaScarto()
        {
            int scarto = gCasa - gOspite;
            return Math.Abs(scarto);
        }

        public virtual string? CalcolaVincitore()
        {
            if (gCasa > gOspite)
            {
                return nCasa;
            }
            else if (gCasa < gOspite)
            {
                return nOspite;
            }
            return null;
        }

        public bool Equals(CPartita other)
        {

            if (other is null)
            {
                return false;
            }

            if (this.Giornata != other.Giornata)
            {
                return false;
            }
            if (this.nCasa.ToLower() != other.nCasa.ToLower())
            {
                return false;
            }
            if (this.nOspite.ToLower() != other.nOspite.ToLower())
            {
                return false;
            }


            return true;
        }

        public override bool Equals(object obj)
        {
            CPartita altra = obj as CPartita;
            if (altra is null)
            {
                return false;
            }
            return Equals(altra);
        }

        public int CompareTo(CPartita other)
        {
            if (other == null)
            {
                return 1;
            }

            if (Criterio == "gol")
            {
                return other.CalcolaGolTotali().CompareTo(this.CalcolaGolTotali());
            }

            if (Criterio == "scarto")
            {
                return other.CalcolaScarto().CompareTo(this.CalcolaScarto());
            }
            if (Criterio == "casaAZ")
            {
                return this.nCasa.ToLower().CompareTo(other.nCasa.ToLower());
            }
            if (Criterio == "casaZA")
            {
                return other.nCasa.ToLower().CompareTo(this.nCasa.ToLower());
            }

            int confrontaGiornata = this.Giornata.CompareTo(other.Giornata);
            if (confrontaGiornata != 0)
            {
                return confrontaGiornata;
            }

            return this.nCasa.ToLower().CompareTo(other.nCasa.ToLower());

        }

        public Esito CalcolaEsito()
        {
            if (gCasa > gOspite)
            {
                return Esito.VittoriaCasa;
            }
            else if (gCasa < gOspite)
            {
                return Esito.VittoriaOspite;
            }
            else
            {
                return Esito.Pareggio;
            }
        }

        public static bool operator ==(CPartita a, CPartita b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            if (a is null || b is null)
            {
                return false;
            }
            return a.Equals(b);
        }

        public static bool operator !=(CPartita a, CPartita b)
        {
            return !(a == b);
        }
    }
}
