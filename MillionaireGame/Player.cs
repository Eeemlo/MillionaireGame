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
        public int SocialStatus { get; set; }

        //Referens till spel
        private Game _game;

        public Player(string name, double initialCapital, int initialKarma, int initialStatus, Game game)
        {
            Name = name;
            Capital = initialCapital;
            Karma = initialKarma;
            SocialStatus = initialStatus;
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

        //Metod för att uppådatera social status
        public void UpdateSocialStatus(int amount)
        {
            SocialStatus += amount;
        }

        //Statisk metod för att visa spelarens information
        public void ShowPlayerInfo(Player player)
        {

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("_____________________________________________________________________________________________________________________");
            Console.Write($"| Kapitalist: {player.Name} | Saldo: {player.Capital:F2} | Social status: {player.SocialStatus} | Karma: {player.Karma} ");
            _game.PrintTotalTimePlayed();
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("");
            // Återställ färgen till standard
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            //Visa varningsmeddelanden om låga värden
            if (player.Capital < 5000)
            {
                Console.WriteLine("Kapitalträsk är ingen plats för pankisar! Du börjar få ont om kapital – dags att fixa kosing om du ska hålla näsan \növer vattenytan!\n");
            }

            if (player.SocialStatus < 20 )
            {
                Console.WriteLine("Din sociala status är nere på bottenvåningen! Dags att klättra i mingelhierarkin om du vill behålla din plats i \nKapitalträsk.\n");
            }

            if (player.Karma < 20)
            {
                Console.WriteLine("Även själlösa typer i Kapitalträsk behöver ett uns karma! Höj den snart, annars är det utgång med huvud före.\n");
            }


            Console.ResetColor();

        }
    }
}