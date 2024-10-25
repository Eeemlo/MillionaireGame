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
            Console.WriteLine("___________________________");
            Console.WriteLine($"Kapitalist: {player.Name}");
            Console.WriteLine($"Saldo: {player.Capital}");
            Console.WriteLine($"Social status: {player.SocialStatus}");
            Console.WriteLine($"Karma: {player.Karma}");
            _game.PrintTotalTimePlayed();
            Console.WriteLine("---------------------------");
            Console.WriteLine("");
         
        }
    }
}