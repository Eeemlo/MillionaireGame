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
            Console.WriteLine("-------------- HUVUDMENY ----------------");
            Console.WriteLine("1. Starta nytt spel");
            Console.WriteLine("2. Ladda tidigare spel");
            Console.WriteLine("3. Avsluta");
            Console.WriteLine("---------------------------------------");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    game.StartNewGame();
                    game.StartScenario();
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
    }
}