using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroNations_Finale
{
    public interface IRepository<T>
    {
        IReadOnlyList<T> GetElenco();
        bool Aggiungi(T elemento);
        bool Rimuovi(T elemento);
        void Carica();
        void Salva();
    }
}
