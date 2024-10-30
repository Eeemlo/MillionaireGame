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

        // Huvudmeny
        public static void ShowMainMenu(Game game)
        {
            Console.Clear();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("              MILJONÄRSRESAN            ");
            Console.WriteLine("En absurd utmaning i kapitalismens värld");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("-------------- HUVUDMENY ----------------");
            Console.WriteLine("1. Starta nytt spel");
            Console.WriteLine("2. Ladda tidigare spel");
            Console.WriteLine("3. Avsluta");
            Console.WriteLine("-----------------------------------------");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    game.GameIntro();
                    game.StartScenario(0);
                    break;
                case "2":
                    game.LoadSavedGame();
                    ShowMainMenu(game);
                    break;
                case "3":
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
            Console.WriteLine("------------VART VILL DU GÅ NU?----------------");
            Console.WriteLine("1. Bostadsförmedlingen");
            Console.WriteLine("2. Banken");
            Console.WriteLine("3. Börsen");
            Console.WriteLine("4. Monumentet Profitsson");
            Console.WriteLine("5. Arbetsförmedlingen");
            Console.WriteLine("6. Nätverksträffen");
            Console.WriteLine("7. Kafé Kaffe & Kapital");
            Console.WriteLine("8. Golfklubben");
            Console.WriteLine("9. Casinot");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    game.StartScenario(0);
                    break;
                case "2":
                    game.StartScenario(1);
                    break;
                case "3":
                    game.StartScenario(2);
                    break;
                case "4":
                    game.StartScenario(3);
                    break;
                case "5":
                    game.StartScenario(4);
                    break;
                case "6":
                    game.StartScenario(5);
                    break;
                case "7":
                    game.StartScenario(6);
                    break;
                case "8":
                    game.StartScenario(7);
                    break;
                case "9":
                    game.StartScenario(8);
                    break;
                default:
                    Console.WriteLine("Ogiltigt val, försök igen!");
                    GameMenu(game);
                    break;
            }
        }
    }
}