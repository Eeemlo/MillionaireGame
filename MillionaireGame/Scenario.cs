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
        public List<int> KarmaImpacts { get; set; }
        public List<double?> FinancialImpacts { get; set; }
        public List<double?> Returns { get; set; }

        public Scenario(string localisation, string question, List<string> optionName, List<string> options, List<int> karmaImpacts, List<double?> financialImpacts = null, List<double?> returns = null)
        {
            Localisation = localisation;
            Question = question;
            OptionName = optionName;
            Options = options;
            FinancialImpacts = financialImpacts;
            KarmaImpacts = karmaImpacts;
            Returns = returns;
        }

        //Ändra färg på påverkan

        public void ShowImpacts()
        {
            // Om det bara finns ett alternativ
            if (Options.Count == 1) // Specifikt för scenariot med ett alternativ
            {
                Console.WriteLine($"1. {OptionName[0]}"); // Skriv ut endast ett alternativ
                Console.WriteLine($"{Options[0]}"); // Beskrivning för det alternativet

                Console.ForegroundColor = ConsoleColor.Magenta;

                // Kontrollera om vi har en fast påverkan (FinancialImpacts) eller procentuell avkastning (Returns)
                if (FinancialImpacts != null && FinancialImpacts.Count > 0 && FinancialImpacts[0] != null)
                {
                    // Om det är fast påverkan
                    double impact = FinancialImpacts[0] ?? 0;
                    Console.WriteLine($"(Påverkan: {impact} SEK, Karmapåverkan: {KarmaImpacts[0]})\n");
                }
                else if (Returns != null && Returns.Count > 0 && Returns[0] != null)
                {
                    // Om det är procentuell avkastning
                    double impact = Returns[0] ?? 0;
                    Console.WriteLine($"(Avkastning: {impact * 100}%, Karmapåverkan: {KarmaImpacts[0]})\n");
                }
                else
                {
                    // Om ingen ekonomisk påverkan finns, skriv ut endast social status och karmapåverkan
                    Console.WriteLine($"(Karmapåverkan: {KarmaImpacts[0]})\n");
                }

                Console.ResetColor();
            }
            else // Om det finns flera alternativ
            {
                for (int i = 0; i < Options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {OptionName[i]}"); // Skriv ut varje alternativ
                    Console.WriteLine($"{Options[i]}"); // Beskrivning för varje alternativ

                    Console.ForegroundColor = ConsoleColor.Magenta;

                    // Kontrollera om vi har en fast påverkan eller procentuell avkastning
                    if (FinancialImpacts != null && FinancialImpacts.Count > i && FinancialImpacts[i] != null)
                    {
                        // Om det är fast påverkan
                        double impact = FinancialImpacts[i] ?? 0;
                        Console.WriteLine($"(Påverkan: {impact} SEK, Karmapåverkan: {KarmaImpacts[i]})\n");
                    }
                    else if (Returns != null && Returns.Count > i && Returns[i] != null)
                    {
                        // Om det är procentuell avkastning
                        double impact = Returns[i] ?? 0;
                        Console.WriteLine($"(Avkastning: {impact * 100}%, Karmapåverkan: {KarmaImpacts[i]})\n");
                    }
                    else
                    {
                        // Om ingen ekonomisk påverkan finns, skriv ut endast social status och karmapåverkan
                        Console.WriteLine($"(Karmapåverkan: {KarmaImpacts[i]})\n");
                    }

                    Console.ResetColor();
                }
            }
        }


    }
}
