/*Denna klass innehåller information om spelaren - namn och kapital*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillionaireGame
{
    internal class Player
    {
        public string Name { get; set; }
        public double Capital { get; set; }
        public int Karma { get; set; }

        //Referens till spel
        private Game _game;

        public Player(string name, double initialCapital, int initialKarma, Game game)
        {
            Name = name;
            Capital = initialCapital;
            Karma = initialKarma;
            _game = game;
        }

        //Metoder för att uppdatera kapital osv.

        // Uppdaterar kapitalet baserat på vinst eller förlust
        public void UpdateCapital(double amount)
        {
            Capital += amount;
            if (Capital < 0) Capital = 0; // Ingen negativ kapital
        }

        //Metod för att uppådatera karma
        public void UpdateKarma(int amount)
        {
            Karma += amount;
        }


        //Statisk metod för att visa spelarens information
        public void ShowPlayerInfo(Player player)
        {

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("_____________________________________________________________________________________________________________________");
            Console.Write($"| Kapitalist: {player.Name} | Saldo: {player.Capital:F2} | Karma: {player.Karma} ");
            _game.PrintTotalTimePlayed();
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("");
            // Återställ färgen till standard
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            //Visa varningsmeddelanden om låga värden
            if (player.Capital < 10000)
            {
                Console.WriteLine("Kapitalträsk är ingen plats för pankisar! Du börjar få ont om kapital – dags att fixa kosing om du ska hålla näsan \növer vattenytan!\n");
            }

            if (player.Karma < 20)
            {
                Console.WriteLine("Även själlösa typer i Kapitalträsk behöver ett uns karma! Höj den snart, annars är det utgång med huvud före.\n");
            }


            Console.ResetColor();

        }
    }
}