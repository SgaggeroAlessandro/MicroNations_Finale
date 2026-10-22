using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Globalization;
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
            if (!File.Exists("partite.json"))
            {
                return;
            }
            string json = File.ReadAllText("partite.json");
            
            List<PartitaPublic> dati = JsonSerializer.Deserialize<List<PartitaPublic>>(json);
            if(dati == null)
            {
                return;
            }

            foreach(PartitaPublic p in dati)
            {
                CPartita nuova = null;

                if(p == null)
                {
                    continue;
                }

                if(p.Tipo == "finale")
                {
                    CPartitaFinale partita = new CPartitaFinale(p.Giornata, p.SquadraCasa, p.SquadraOspite, p.GolCasa, p.GolOspite); 
                    if(p.GolCasa == p.GolOspite)
                    {
                        partita.RigoriCasa = p.RigoriCasa;
                        partita.RigoriOspite = p.RigoriOspite;

                        if (!partita.RigoriValidi())
                        {
                            continue;
                        }
                    }

                    nuova = partita;

                }else if(p.Tipo == "eliminazione" )
                {
                    CPartitaEliminazione partita = new CPartitaEliminazione(p.Giornata, p.SquadraCasa, p.SquadraOspite, p.GolCasa, p.GolOspite);
                    if (p.GolCasa == p.GolOspite)
                    {
                        partita.RigoriCasa = p.RigoriCasa;
                        partita.RigoriOspite = p.RigoriOspite;

                        if (!partita.RigoriValidi())
                        {
                            continue;
                        }
                    }

                    nuova = partita;


                }
                else if(p.Tipo == "girone")
                {
                    nuova = new CPartitaGirone(p.Giornata, p.SquadraCasa, p.SquadraOspite, p.GolCasa, p.GolOspite);
                }
                else
                {
                    continue;
                }

                if(nuova != null)
                {
                    Aggiungi(nuova);
                }
            }
        }

        public void Salva()
        {
            List<PartitaPublic> dati = new List<PartitaPublic>();
            foreach (CPartita p in partite)
            {
                PartitaPublic partita = new PartitaPublic();

                if(p is CPartitaFinale)
                {
                    partita.Tipo = "finale";
                }else if(p is CPartitaEliminazione)
                {
                    partita.Tipo = "eliminazione";
                }
                else
                {
                    partita.Tipo = "girone";
                }

                partita.Giornata = p.Giornata;
                partita.SquadraCasa = p.nCasa;
                partita.SquadraOspite = p.nOspite;
                partita.GolCasa = p.gCasa;
                partita.GolOspite = p.gOspite;


                if(p is CPartitaEliminazione eliminazione)
                {
                    partita.RigoriCasa = eliminazione.RigoriCasa;
                    partita.RigoriOspite = eliminazione.RigoriOspite;
                }

                dati.Add(partita);
            }

            string json  = JsonSerializer.Serialize(dati);
            File.WriteAllText("partite.json", json);
        }
    }
}
