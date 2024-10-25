/* Denna klass hanterar spelets logik och flödet av spelet. Den innehåller metoder för att starta ett nytt spel, ladda ett sparat spel, investera, starta företag och avsluta spelet. */

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
        private List<Challenge> _investments; // Lista över olika utmaningar/spelmoment för investeringar

        public Player Player // Offentlig egenskap för att komma åt spelaren
        {
            get { return _player; } // Getter för att hämta spelarinformation
        }

        // Konstruktor för Game-klassen.
        public Game()
        {

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
            _player = new Player(playerName, 10000, 100); // Skapar en ny spelare med startkapital och karma
            Console.WriteLine($"Välkommen {_player.Name}! Spelet börjar nu! Lycka till!"); // Välkomnar spelaren
            Console.WriteLine("Tryck på [Enter] för att fortsätta..."); // Instruktion för att fortsätta
            Console.ReadLine(); // Väntar på att spelaren trycker Enter
            Menu.ShowGameMenu(this); // Går till spelmenyn
        }

        // Metod för att ladda ett sparat spel
        public void LoadSavedGame()
        {
            Console.Clear(); // Rensar konsolen
            Console.WriteLine("----------- LADDAR SPEL --------------");
            // Här kan du implementera logik för att ladda ett sparat spel
            Console.WriteLine("Inga sparade spel hittades. Tryck på [Enter] för att återgå till huvudmenyn..."); // Meddelande till användaren
            Console.ReadLine(); // Väntar på att spelaren trycker Enter
            Menu.ShowGameMenu(this); // Går tillbaka till spelmenyn
        }

        // Metod för att investera i olika företag
        public void Invest()
        {
            Console.Clear(); // Rensar konsolen
            Console.WriteLine("Välj företag att investera i"); // Ber användaren att välja en investeringsmöjlighet
            Console.WriteLine("1. Hög risk: Stor potentiell vinst, men hög förlust om det går dåligt."); // Beskrivning av alternativ 1
            Console.WriteLine("2. Medium risk: Måttlig potentiell vinst och mindre risk för förlust"); // Beskrivning av alternativ 2
            Console.WriteLine("3. Låg risk: Låg vinst men minimal risk för förlust"); // Beskrivning av alternativ 3

            string choice = Console.ReadLine(); // Tar emot spelarens val
            double potentialReward = 0; // Variabel för att lagra potentiell belöning
            int karmaImpact = 0; // Variabel för att lagra karmaeffekt

            switch (choice)
            {
                case "1": // Hög risk
                    potentialReward = 10000; // Sätter belöning för hög risk
                    karmaImpact = -10; // Negativ påverkan på karma
                    PerformInvestment(potentialReward, karmaImpact, 50); // Utför investeringen
                    break;
                case "2": // Medium risk
                    potentialReward = 5000; // Sätter belöning för medelrisk
                    karmaImpact = 0; // Ingen påverkan på karma
                    PerformInvestment(potentialReward, karmaImpact, 70); // Utför investeringen
                    break;
                case "3": // Låg risk
                    potentialReward = 2000; // Sätter belöning för låg risk
                    karmaImpact = 5; // Positiv påverkan på karma
                    PerformInvestment(potentialReward, karmaImpact, 90); // Utför investeringen
                    break;
                default: // Ogiltigt val
                    Console.WriteLine("Ogiltigt val. Försök igen."); // Meddelande om ogiltigt val
                    Invest(); // Återkallar investeringsmetoden för ny inmatning
                    break;
            }
        }

        // Privat metod för att hantera logik bakom en investering
        private void PerformInvestment(double reward, int karmaImpact, int successChance)
        {
            Random rand = new Random(); // Skapar en ny instans av Random för slumptalsgenerering
            if (rand.Next(1, 101) <= successChance) // Kontrollerar om investeringen lyckas baserat på sannolikheten
            {
                _player.UpdateCapital(reward); // Om investeringen lyckas, uppdaterar spelarens kapital
                Console.WriteLine($"Investeringen lyckades! Du tjänade {reward} SEK."); // Meddelar spelaren om framgång
            }
            else // Om investeringen misslyckas
            {
                double loss = reward / 2; // Beräknar förlusten (50% av belöningen)
                _player.UpdateCapital(-loss); // Uppdaterar spelarens kapital med förlusten
                Console.WriteLine($"Tyvärr misslyckades investeringen. Du förlorade {loss} SEK."); // Meddelar spelaren om misslyckande
            }

            _player.UpdateKarma(karmaImpact); // Uppdaterar spelarens karma baserat på effekten av investeringen
            Utilities.Pause(); // Pausar spelet för att låta spelaren läsa meddelandet
        }

        // Metod för att starta ett företag
        public void StartCompany()
        {
            Console.Clear(); // Rensar konsolen
            Console.WriteLine("Välj en företagstyp:"); // Ber användaren att välja företagstyp
            Console.WriteLine("1. Hög-risk företag: Höga vinster, men mycket ansvar påverkar karma negativt."); // Beskrivning av hög-risk företag
            Console.WriteLine("2. Medium-risk företag: Balans mellan vinst och karma."); // Beskrivning av medium-risk företag
            Console.WriteLine("3. Låg-risk företag: Mindre vinst men bättre karma."); // Beskrivning av låg-risk företag

            string choice = Console.ReadLine(); // Tar emot spelarens val
            double potentialProfit = 0; // Variabel för att lagra potentiell vinst
            int karmaImpact = 0; // Variabel för att lagra karmaeffekt

            switch (choice)
            {
                case "1": // Hög-risk företag
                    potentialProfit = 20000; // Sätter vinstpotentialen
                    karmaImpact = -20; // Negativ påverkan på karma
                    PerformCompany(potentialProfit, karmaImpact, 50); // Utför företagets logik
                    break;
                case "2": // Medium-risk företag
                    potentialProfit = 10000; // Sätter vinstpotentialen
                    karmaImpact = -5; // Liten negativ påverkan på karma
                    PerformCompany(potentialProfit, karmaImpact, 70); // Utför företagets logik
                    break;
                case "3": // Låg-risk företag
                    potentialProfit = 4000; // Sätter vinstpotentialen
                    karmaImpact = 10; // Positiv påverkan på karma
                    PerformCompany(potentialProfit, karmaImpact, 90); // Utför företagets logik
                    break;
                default: // Ogiltigt val
                    Console.WriteLine("Ogiltigt val. Försök igen."); // Meddelande om ogiltigt val
                    StartCompany(); // Återkallar metoden för ny inmatning
                    break;
            }
        }

        // Privat metod för att hantera logik bakom företagets resultat
        private void PerformCompany(double profit, int karmaImpact, int successChance)
        {
            Random rand = new Random(); // Skapar en ny instans av Random för slumptalsgenerering
            if (rand.Next(1, 101) <= successChance) // Kontrollerar om företaget lyckas
            {
                _player.UpdateCapital(profit); // Uppdaterar spelarens kapital med vinsten
                Console.WriteLine($"Företaget gick bra! Du tjänade {profit} SEK."); // Meddelar spelaren om framgång
            }
            else // Om företaget misslyckas
            {
                double loss = profit / 2; // Beräknar förlusten (50% av vinsten)
                _player.UpdateCapital(-loss); // Uppdaterar spelarens kapital med förlusten
                Console.WriteLine($"Tyvärr gick företaget dåligt. Du förlorade {loss} SEK."); // Meddelar spelaren om misslyckande
            }

            _player.UpdateKarma(karmaImpact); // Uppdaterar spelarens karma baserat på företagets resultat
            Utilities.Pause(); // Pausar spelet för att låta spelaren läsa meddelandet
        }

        // Metod för att avsluta spelet
        public void ExitGame()
        {
            Console.Clear(); // Rensar konsolen
            Console.WriteLine("Tack för att du spelade Kapitalets Utmaning!"); // Avslutningsmeddelande
            Environment.Exit(0); // Stänger ner applikationen
        }
    } // Avslutande klammerparentes för Game-klassen
} // Avslutande klammerparentes för namespace MillionaireGame
