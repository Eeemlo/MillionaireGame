using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillionaireGame
{
    public class Scenario
    {
        public string Localisation { get; set; }
        public string Question { get; set; }
        public List<string> OptionName { get; set; }
        public List<string> Options { get; set; }
        public List<double> FinancialImpacts { get; set; }
        public List<int> KarmaImpacts { get; set; }
        public List<int> SocialImpacts { get; set; }

        public Scenario(string localisation, string question, List<string> optionName, List<string> options, List<double> financialImpacts, List<int> karmaImpacts, List<int> socialImpacts)
        {
            Localisation = localisation;
            Question = question;
            OptionName = optionName;
            Options = options;
            FinancialImpacts = financialImpacts;
            KarmaImpacts = karmaImpacts;
            SocialImpacts = socialImpacts;
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
