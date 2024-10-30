using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillionaireGame
{
    public class Scenario
    {
        public string Localisation { get; set; };
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public List<double> FinancialImpacts { get; set; }
        public List<int> KarmaImpacts { get; set; }
        public List <int> SocialImpacts { get; set; }
        public List<int> NextScenarios { get; set; }

        public Scenario (string localisation, string question, List<string> options, List<double> financialImpacts, List<int> karmaImpacts, List<int> socialImpacts, List<int> nextScenarios)
        {
            Localisation = localisation;
            Question = question;
            Options = options;
            FinancialImpacts = financialImpacts;
            KarmaImpacts = karmaImpacts;
            SocialImpacts = socialImpacts;
            NextScenarios = nextScenarios;
        }

        //Ändra färg på påverkan

        
        public void showImpacts()
        {
            for (int i = 0; i < Options.Count; i++) 
                {
                Console.Write($"{i + 1}. {Options[i]}");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"(Kapital: {FinancialImpacts[i]}, Social status: {SocialImpacts[i]}, Karmapåverkan: {KarmaImpacts[i]})\r\n");

                Console.ResetColor();
                Console.WriteLine("");
            }
        }
    }
}
