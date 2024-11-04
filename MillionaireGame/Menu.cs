/*Denna klass hanterar spelets menysystem*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillionaireGame
{
    internal class Menu
    {
        private Player _player; // En referens till spelaren som representerar användarens karaktär i spelet

        public Menu(Player player) // Konstruktör som tar emot en Player-instans
        {
            _player = player; // Tilldela den mottagna spelaren till den privata variabeln
        }

        // Huvudmeny
        public static void ShowMainMenu(Game game)
        {
            Console.Clear();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("      VÄLKOMMEN TILL KAPITALTRÄSK!           ");
            Console.WriteLine("En absurd utmaning i kapitalismens värld");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("-------------- HUVUDMENY ----------------");
            Console.WriteLine("1. Starta nytt spel");
            Console.WriteLine("2. Avsluta");
            Console.WriteLine("-----------------------------------------");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    game.GameIntro();
                    game.StartScenario(0);
                    break;
                case "2":
                    game.ExitGame();
                    break;
                default:
                    Console.WriteLine("Ogiltigt val, försök igen!");
                    ShowMainMenu(game);
                    break;
            }
        }

        public static void GameMenu(Game game)
        {
            game.Player.ShowPlayerInfo(game.Player); // Visa den aktuella spelarens information

            Console.WriteLine("\n\n------------VART VILL DU GÅ NU?----------------\n");
            Console.WriteLine("1. Banken");
            Console.WriteLine("2. Börsen");
            Console.WriteLine("3. Monumentet Profitsson");
            Console.WriteLine("4. Arbetsförmedlingen");
            Console.WriteLine("5. Vallokalen");
            Console.WriteLine("6. Kafé Kaffe & Kapital");
            Console.WriteLine("7. Golfklubben");
            Console.WriteLine("8. Casinot");
            Console.WriteLine("9. Välgörenhetsgalan");
            Console.WriteLine("____________________________________________________");
            Console.WriteLine("10. Gå till spelets startmeny för att avsluta");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    game.StartScenario(1);
                    break;
                case "2":
                    game.StartScenario(2);
                    break;
                case "3":
                    game.StartScenario(3);
                    break;
                case "4":
                    game.StartScenario(4);
                    break;
                case "5":
                    game.StartScenario(5);
                    break;
                case "6":
                    game.StartScenario(6);
                    break;
                case "7":
                    game.StartScenario(7);
                    break;
                case "8":
                    game.StartScenario(8);
                    break;
                case "9":
                    game.StartScenario(9);
                    break;
                case "10":
                    ShowMainMenu(game);
                    break;
                default:
                    Console.WriteLine("Ogiltigt val, försök igen!");
                    GameMenu(game);
                    break;
            }
        }
    }
}