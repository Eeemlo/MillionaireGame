﻿/* Denna klass hanterar spelets logik och flödet av spelet. Den innehåller metoder för att starta ett nytt spel, ladda ett sparat spel, investera, starta företag och avsluta spelet. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MillionaireGame
{
    internal class Game
    {
        private Player _player; // En referens till spelaren som representerar användarens karaktär i spelet
        private List<Scenario> _scenarios;
        
        public Player Player // Offentlig egenskap för att komma åt spelaren
        {
            get { return _player; } // Getter för att hämta spelarinformation
        }

        public Game()
        {
            _scenarios = InitializeScenarios();
        }


        // Metod för att starta spelet. Visar introduktion och huvudmeny.
        public void Start()
        {
            ShowIntro(); // Visar introduktionen till spelet
            Menu.ShowMainMenu(this); // Visar huvudmenyn
        }

        // Introduktion till spelet
        private void ShowIntro()
        {
            Console.Clear(); // Rensar konsolen
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("              MILJONÄRSRESAN            ");
            Console.WriteLine("En absurd utmaning i kapitalismens värld");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Tryck på [Enter] för att fortsätta.."); // Instruktion till användaren
        }

        // Metod för att starta ett nytt spel
        public void StartNewGame()
        {
            Console.Clear(); // Rensar konsolen
            Console.WriteLine("------------- NYTT SPEL ---------------");
            Console.WriteLine("Skriv ditt namn:"); // Ber användaren om sitt namn
            string playerName = Console.ReadLine(); // Tar emot spelarens namn
            _player = new Player(playerName, 10000, 100, 100); // Skapar en ny spelare med startkapital och karma
            Console.WriteLine($"Välkommen {_player.Name}! Spelet börjar nu! Lycka till!"); // Välkomnar spelaren
            Console.WriteLine("Tryck på [Enter] för att fortsätta..."); // Instruktion för att fortsätta
            Console.ReadLine(); // Väntar på att spelaren trycker Enter
            GameIntro(); // Startar bakgrundsbeskrivning
        }

        public void GameIntro()
        {
            Console.Clear();
            Console.WriteLine("----------- BAKGRUND --------------");
            Console.WriteLine("Du är en ung entreprenör som sedan barnsben fått lära dig att prestation och monetär framgång är de ");
            Console.WriteLine("enda vägarna till lycka. Under din uppväxt har du matats med budskapet att det som verkligen betyder ");
            Console.WriteLine("något är hur mycket pengar du tjänar och hur snabbt du kan klättra på den kapitalistiska stegen. ");
            Console.WriteLine("");
            Console.WriteLine("Du bestämmer dig för att flytta till staden Kapitalträsk, där varje beslut kan leda antingen till förmögenhet ");
            Console.WriteLine("eller fördömelse. Här kan du investera i allt från tvivelaktiga aktier, som ”Slavarbete AB”, till att starta ");
            Console.WriteLine("företag som säljer ”eksklusiva” skräpföremål. För varför fokusera på kvalitet när kvantitet och snabba ");
            Console.WriteLine("vinster är det enda som räknas?");
            Console.WriteLine("");
            Console.WriteLine("Spelets mål är att du framgångsrikt ska sälja din själ för att uppnå den eftertraktade statusen miljonär. Du ");
            Console.WriteLine("bjuds in till en värld där din värdegrund ställs på prov (om du ens har någon) och där moral och etik är mer ");
            Console.WriteLine("som rekommendationer än regler. ");
            Console.WriteLine("");
            Console.WriteLine("Tryck på [Enter] för att fortsätta...");
            Console.ReadLine();
            StartScenario(); //Startar första scenariot

        }

        // Metod för att ladda ett sparat spel
        public void LoadSavedGame()
        {
            Console.Clear(); // Rensar konsolen
            Console.WriteLine("----------- LADDAR SPEL --------------");
            // Här kan du implementera logik för att ladda ett sparat spel
            Console.WriteLine("Inga sparade spel hittades. Tryck på [Enter] för att återgå till huvudmenyn..."); // Meddelande till användaren
            Console.ReadLine(); // Väntar på att spelaren trycker Enter
            Menu.ShowMainMenu(this); // Går tillbaka till huvudmenyn
        }

        
        // Metod för att avsluta spelet
        public void ExitGame()
        {
            Console.Clear(); // Rensar konsolen
            Console.WriteLine("Tack för att du spelade Kapitalets Utmaning!"); // Avslutningsmeddelande
            Environment.Exit(0); // Stänger ner applikationen
        }

        // Metod för att initiera scenarier
        private List<Scenario> InitializeScenarios()
        {
            return new List<Scenario>
            {
                new Scenario(
                    "Som nyinflyttad i Kapitalträsk inser du att dina 30000 SEK inte räcker till särskilt mycket. Först och främst behöver du ha någonstans att bo. Vilket alternativ väljer du?",
                    new List<string>
                    {
                        "Bo i en flashig takvåning för 20000 SEK i månaden.",
                        "Flytta in i en tvåa på 50 kvm för 10000 SEK i månaden.",
                        "Sova i trappuppgångar första tiden."
                    },
                    new List<double> { -20000, -10000, 0 },
                    new List<int> { -5, -2, 0 }, // Karma påverkan (exempelvärden)
                    new List<int> { -10, -5, 0 } // Social påverkan (exempelvärden)
                ),
                new Scenario(
                    "Det är dags för dig att starta ditt första företag. Du har tre alternativ:",
                    new List<string>
                    {
                        "AppSolutely Addictive AB - Ett appföretag som specialiserar sig på att skapa beroendeframkallande mobilspel.",
                        "Green Future - Ett miljövänligt företag som fokuserar på hållbara produkter.",
                        "KaffeKraft - En kaffebutik som erbjuder olika sorters kaffe och bakverk."
                    },
                    new List<double> { -15000, -12000, -8000 },
                    new List<int> { -5, -2, 0 }, // Karma påverkan (exempelvärden)
                    new List<int> { -10, -5, 0 } // Social påverkan (exempelvärden)
                )
            };
        }

        // Metod för att hantera spelarnas val i scenarier
        public void StartScenario()
        {
            foreach (var scenario in _scenarios)
            {
                Console.Clear();
                Console.WriteLine(scenario.Question);
                for (int i = 0; i < scenario.Options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {scenario.Options[i]}");
                }

                int optionIndex = -1; // Variabel för spelarens val

                // Använd en while-loop för att hantera ogiltiga val
                while (optionIndex < 0 || optionIndex >= scenario.Options.Count)
                {
                    string choice = Console.ReadLine();
                    if (int.TryParse(choice, out optionIndex) && optionIndex > 0)
                    {
                        optionIndex--; // Justera för att passa indexet i listan
                        double impact = scenario.FinancialImpacts[optionIndex];
                        _player.UpdateCapital(impact);
                        Console.WriteLine($"Du valde: {scenario.Options[optionIndex]}.");
                        Console.WriteLine($"Ekonomisk påverkan: {impact} SEK.");
                    }
                    else
                    {
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        optionIndex = -1; // Återställ val
                    }
                }

                Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
            }
        }
    } // Avslutande klammerparentes för Game-klassen
} // Avslutande klammerparentes för namespace MillionaireGame