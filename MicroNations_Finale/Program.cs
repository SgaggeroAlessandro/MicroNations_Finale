using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
namespace MicroNations_Finale
{
    internal class Program
    {
        static RepositoryPartite repository = new RepositoryPartite();
        static void Main(string[] args)
        {
            repository.Carica();
            int scelta;
            do
            {
                do
                {
                    MostraMenu();
                } while (!int.TryParse(Console.ReadLine(), out scelta) || scelta < 0 || scelta > 6);

                switch (scelta)
                {
                    case 1:
                        VisualizzaNaturale();
                        break;
                    case 2:
                        VisualizzaConCriterio();
                        break;
                    case 3:
                        InserisciPartita();
                        break;
                    case 4:
                        MostraClassifica();
                        break;
                    case 5:
                        CercaSquadra();
                        break;
                    case 6:
                        EliminaPartita();
                        break;
                    case 0:
                        Console.WriteLine("Uscita dal programma.");
                        break;
                }

            } while (scelta != 0);
        }

        static void MostraMenu()
        {

            Console.WriteLine("\n1. Visualizza partite (ordine naturale)");
            Console.WriteLine("2. Visualizza partite (altro criterio)");
            Console.WriteLine("3. Inserisci partita");
            Console.WriteLine("4. Classifica gironi");
            Console.WriteLine("5. Cerca partite di una squadra");
            Console.WriteLine("6. Elimina partita");
            Console.WriteLine("0. Esci");
            Console.WriteLine("Scelta: \n");
        }

        static void VisualizzaNaturale()
        {
            CPartita.Criterio = "naturale";
            List<CPartita> lista = new List<CPartita>(repository.GetElenco());
            lista.Sort();
            StampaLista(lista);
        }

        static void VisualizzaConCriterio()
        {
            Console.WriteLine("Criteri: naturale, gol, scarto, casaAZ, casaZA");
            Console.Write("Scegli criterio: ");
            string criterio = Console.ReadLine();

            if (criterio != "naturale" && criterio != "gol" && criterio != "scarto" && criterio != "casaAZ" && criterio != "casaZA")
            {
                Console.WriteLine("Criterio non valido.");
                return;
            }

            CPartita.Criterio = criterio;
            List<CPartita> lista = new List<CPartita>(repository.GetElenco());
            lista.Sort();
            StampaLista(lista);
        }

        static void StampaLista(List<CPartita> lista)
        {
            if (lista.Count == 0)
            {
                Console.WriteLine("Nessuna partita presente.");
                return;
            }

            foreach (CPartita p in lista)
            {
                Console.WriteLine(p.Descrizione());
            }
        }

        static void InserisciPartita()
        {
            try
            {
                Console.Write("Scrivi il tipo della partita tra girone, eliminazione e finale): ");
                string tipo = Console.ReadLine().ToLower();


                int giornata;
                do
                {
                    Console.Write("Giornata: ");
                } while (!int.TryParse(Console.ReadLine(), out giornata));



                Console.Write("Squadra di casa: ");
                string casa = Console.ReadLine();

                Console.Write("Squadra ospite: ");
                string ospite = Console.ReadLine();


                int golCasa;
                do
                {
                    Console.Write("Gol casa: ");
                } while (!int.TryParse(Console.ReadLine(), out golCasa));
                int golOspite;

                do
                {
                    Console.Write("Gol ospite: ");
                } while (!int.TryParse(Console.ReadLine(), out golOspite));



                CPartita nuova = null;

                if (tipo == "girone")
                {
                    nuova = new CPartitaGirone(giornata, casa, ospite, golCasa, golOspite);
                }
                else if (tipo == "eliminazione" || tipo == "finale")
                {
                    CPartitaEliminazione partitaElim;

                    if (tipo == "finale")
                    {
                        partitaElim = new CPartitaFinale(giornata, casa, ospite, golCasa, golOspite);
                    }
                    else
                    {
                        partitaElim = new CPartitaEliminazione(giornata, casa, ospite, golCasa, golOspite);
                    }

                    if (golCasa == golOspite)
                    {
                        Console.WriteLine("Pareggio: servono i rigori.");
                        int rigCasa;
                        do
                        {
                            Console.Write("Rigori casa: ");
                        } while (!int.TryParse(Console.ReadLine(), out rigCasa));
                        int rigOspite;
                        do
                        {
                            Console.Write("Rigori ospite: ");
                        } while (!int.TryParse(Console.ReadLine(), out rigOspite));
                        partitaElim.RigoriCasa = rigCasa;
                        partitaElim.RigoriOspite = rigOspite;

                        if (!partitaElim.RigoriValidi())
                        {
                            Console.WriteLine("Rigori non validi perché non possono finire in pareggio. Partita non salvata.");
                            return;
                        }
                    }

                    nuova = partitaElim;
                }
                else
                {
                    Console.WriteLine("Tipo non valido.");
                    return;
                }

                if (repository.Aggiungi(nuova))
                {
                    repository.Salva();
                    Console.WriteLine("Partita inserita.");
                }
                else
                {
                    Console.WriteLine("Partita già presente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore: " + ex.Message);
            }
        }

        static void MostraClassifica()
        {
            List<StatisticheSquadra> classifica = new List<StatisticheSquadra>();

            foreach (CPartita p in repository.GetElenco())
            {
                if (p is CPartitaGirone girone)
                {
                    AggiornaStatistica(classifica, girone.nCasa, girone, true);
                    AggiornaStatistica(classifica, girone.nOspite, girone, false);
                }
            }

            classifica.Sort();

            Console.WriteLine("Squadra Pt G V N P GF GS DR");
            foreach (StatisticheSquadra s in classifica)
            {
                Console.WriteLine($"{s.Nome} {s.Punti} {s.Giocate} {s.Vinte} {s.Pareggiate} {s.Perse} {s.GolFatti} {s.GolSubiti} {s.CalcolaDifferenzaReti()}");
            }
        }

        static void AggiornaStatistica(List<StatisticheSquadra> classifica, string squadra, CPartitaGirone p, bool isCasa)
        {
            StatisticheSquadra stat = null;

            foreach (StatisticheSquadra s in classifica)
            {
                if (s.Nome.ToLower() == squadra.ToLower())
                {
                    stat = s;
                    break;
                }
            }

            if (stat == null)
            {
                stat = new StatisticheSquadra(squadra);
                classifica.Add(stat);
            }

            stat.Giocate++;

            int golFatti = isCasa ? p.gCasa : p.gOspite;
            int golSubiti = isCasa ? p.gOspite : p.gCasa;
            stat.GolFatti += golFatti;
            stat.GolSubiti += golSubiti;
            stat.Punti += p.PuntiPer(squadra);

            Esito esito = p.CalcolaEsito();
            bool haVinto = (esito == Esito.VittoriaCasa && isCasa) || (esito == Esito.VittoriaOspite && !isCasa);

            if (esito == Esito.Pareggio)
            {
                stat.Pareggiate++;
            }
            else if (haVinto)
            {
                stat.Vinte++;
            }
            else
            {
                stat.Perse++;
            }
        }


        static void CercaSquadra()
        {
            Console.Write("Nome squadra: ");
            string squadra = Console.ReadLine().ToLower();

            List<CPartita> trovate = new List<CPartita>();
            foreach (CPartita p in repository.GetElenco())
            {
                if (p.nCasa.ToLower() == squadra || p.nOspite.ToLower() == squadra)
                {
                    trovate.Add(p);
                }
            }

            StampaLista(trovate);
        }

        static void EliminaPartita()
        {
            try
            {

                int giornata;
                do
                {
                    Console.Write("Giornata: ");
                } while (!int.TryParse(Console.ReadLine(), out giornata));

                Console.Write("Squadra di casa: ");
                string casa = Console.ReadLine();
                Console.Write("Squadra ospite: ");
                string ospite = Console.ReadLine();

                CPartita fittizia = new CPartitaGirone(giornata, casa, ospite, 0, 0);

                if (repository.Rimuovi(fittizia))
                {
                    repository.Salva();
                    Console.WriteLine("Partita eliminata.");
                }
                else
                {
                    Console.WriteLine("Partita non trovata.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore: " + ex.Message);
            }
        }
    }
}


