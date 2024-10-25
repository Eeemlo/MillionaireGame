using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillionaireGame
{
    public class Scenario
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public List<double> FinancialImpacts { get; set; }
        public List<int> KarmaImpacts { get; set; }
        public List <int> SocialImpacts { get; set; }

        public Scenario (string question, List<string> options, List<double> financialImpacts, List<int> karmaImpacts, List<int> socialImpacts)
        {
            Question = question;
            Options = options;
            FinancialImpacts = financialImpacts;
            KarmaImpacts = karmaImpacts;
            SocialImpacts = socialImpacts;
        }
    }
}
