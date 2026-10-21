using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
namespace MicroNations_Finale
{
    public class RepositoryPartite : IRepository<CPartita>
    {
        private List<CPartita> partite = new List<CPartita>();
        public IReadOnlyList<CPartita> GetElenco()
        {
            return new List<CPartita>(partite);
        }
        public bool Aggiungi(CPartita elemento)
        {
            if (elemento == null)
            {
                return false;
            }

            foreach (CPartita p in partite)
            {
                if (p.Equals(elemento))
                {
                    return false;
                }
            }
            partite.Add(elemento);
            return true;
        }
        public bool Rimuovi(CPartita elemento)
        {
            if (elemento == null)
            {
                return false;
            }
            CPartita trovato = null;
            foreach (CPartita p in partite)
            {
                if (p.Equals(elemento))
                {
                    trovato = p;
                    break;
                }
            }

            if (trovato == null)
            {
                return false;
            }

            partite.Remove(trovato);
            return true;
        }

        public void Carica()
        {

        }

        public void Salva()
        {

        }
    }
}
