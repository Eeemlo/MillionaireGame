/* Denna klass hanterar spelets logik och flödet av spelet. Den innehåller metoder för att starta ett nytt spel, ladda ett sparat spel, investera, starta företag och avsluta spelet. */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MillionaireGame
{
    internal class Game
    {
        private Player _player; // En referens till spelaren som representerar användarens karaktär i spelet
        private List<Scenario> _scenarios;
        private Stopwatch _stopwatch; //Tidtagare
        private double _monthlyRent; //Månadshyra

        public Player Player // Offentlig egenskap för att komma åt spelaren
        {
            get { return _player; } // Getter för att hämta spelarinformation
        }

        public Game()
        {
            _scenarios = InitializeScenarios();
            _stopwatch = new Stopwatch();
        }


        // Metod för att starta spelet. Visar introduktion och huvudmeny.
        public void Start()
        {
            Menu.ShowMainMenu(this); // Visar huvudmenyn
        }

        // Metod för att starta ett nytt spel
        public void StartNewGame()
        {
            Console.Clear(); // Rensar konsolen
            Console.WriteLine("------------- NYTT SPEL ---------------");
            Console.WriteLine("");
            Console.WriteLine("Skriv ditt namn:"); // Ber användaren om sitt namn

            string playerName = Console.ReadLine(); // Tar emot spelarens namn
            _player = new Player(playerName, 30000, 100, 100, this); // Skapar en ny spelare med startkapital och karma

            if (string.IsNullOrWhiteSpace(playerName))
            {
                Console.WriteLine("Du måste ange ett spelarnamn... Försök igen!");
                Utilities.Pause();
                StartNewGame();
            }
            else
            {
                welcomePlayer();
            }

        }

        public void welcomePlayer()
        {
            Console.Clear();
            Console.WriteLine($"Välkommen {_player.Name}!"); // Välkomnar spelaren
            Console.WriteLine("Du har precis tagit dina första steg i Kapitalträsk, där beslut om pengar och moral väger tungt.");
            Console.WriteLine("");
            Console.WriteLine("Spelets regler:");
            Console.WriteLine("- Du startar med 30 000 SEK i kapital, 100 i karma och 100 i social status.");
            Console.WriteLine("- Varje beslut du tar kommer att påverka ditt kapital, karma och sociala status.");
            Console.WriteLine("- Din tid i spelet är begränsad, och varje månad (5 minuter i speltid) kommer du att behöva betala dina fasta utgifter.");
            Console.WriteLine("- Målet är att uppnå miljonärstatus genom att fatta strategiska beslut och hålla dig över vattenytan.");
            Console.WriteLine("");
            Console.WriteLine("Är du redo att bli nästa stjärnskott i Kapitalträsk? Det är upp till dig att göra rätt val – eller de mest lönsamma!\r\n");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Tryck på [Enter] för att börja ditt äventyr!");
            Console.ReadLine(); // Väntar på att spelaren trycker Enter
            _stopwatch.Start();
            StartScenario(0); //Startar första scenariot
        }

        public void GameIntro()
        {
            Console.Clear();
            Console.WriteLine("----------- BAKGRUND --------------");
            Console.WriteLine("");
            Console.WriteLine("Du är en ung entreprenör som sedan barnsben fått lära dig att prestation och monetär framgång är de ");
            Console.WriteLine("enda vägarna till lycka. Under din uppväxt har du matats med budskapet att det som verkligen betyder ");
            Console.WriteLine("något är hur mycket pengar du tjänar och hur snabbt du kan klättra på den kapitalistiska stegen. ");
            Console.WriteLine("");
            Console.WriteLine("Du bestämmer dig för att flytta till staden Kapitalträsk, där varje beslut kan leda antingen till förmögenhet ");
            Console.WriteLine("eller fördömelse. Här kan du investera i allt från tvivelaktiga aktier, som ”Slavarbete AB”, till att starta ");
            Console.WriteLine("företag som säljer ”eksklusiva” skräpföremål. För varför fokusera på kvalitet när kvantitet och snabba ");
            Console.WriteLine("vinster är det enda som räknas?");
            Console.WriteLine("");
            Console.WriteLine("Spelets mål är att du ska försöka bli miljonär så snabbt som möjligt utan att förlora din själ på vägen. Du ");
            Console.WriteLine("bjuds in till en värld där din värdegrund ställs på prov och där moral och etik är mer som rekommendationer än regler. ");
            Console.WriteLine("Klarar du att klättra till toppen utan att offra allt du står för, eller kommer vägen till rikedom att kosta dig din mänsklighet?");
            Console.WriteLine("");
            Console.WriteLine("Tryck på [Enter] för att fortsätta...");
            Console.ReadLine();
            StartNewGame();

        }

        // Metod för att ladda ett sparat spel
        public void LoadSavedGame()
        {
            Console.Clear(); // Rensar konsolen
            Console.WriteLine("----------- VÄLJ SPARAT SPEL --------------");
            // Här kan du implementera logik för att ladda ett sparat spel
            Console.WriteLine("Inga sparade spel hittades. Tryck på [Enter] för att återgå till huvudmenyn..."); // Meddelande till användaren
            Console.ReadLine(); // Väntar på att spelaren trycker Enter
            Menu.ShowMainMenu(this); // Går tillbaka till huvudmenyn
        }



        // Omvandla tid till månader
        public (int years, int months) GetTotalTimePlayed()
        {
            double totalMinutes = _stopwatch.Elapsed.TotalMinutes;
            int totalMonths = (int)(totalMinutes / 5); // 5 minuter = 1 månad

            int years = totalMonths / 12; // Beräkna antal år
            int months = totalMonths % 12; // Beräkna kvarvarande månader

            return (years, months);
        }

        // Metod för att skriva ut tiden som är spelad
        public void PrintTotalTimePlayed()
        {
            var (years, months) = GetTotalTimePlayed();
            string timeString = "";

            // Formatera utskriften
            if (years > 0)
            {
                timeString += $"{years} år";
            }

            if (months > 0)
            {
                if (years > 0)
                {
                    timeString += " och "; // Lägg till "och" om det finns både år och månader
                }
                timeString += $"{months} månader";
            }

            if (timeString == "")
            {
                timeString = "Nyinflyttad"; // Om ingen tid har spelats

            }

            Console.WriteLine($"| Total tid spelad: {timeString} |");
        }

        /*
        public bool IsScenarioAccessible(Scenario scenario, double playerCapital, int playerKarma, int playerSocialStatus)
        {
            // Här kan du lägga till logik för att kolla spelarens status innan tillträde.
            if (scenario == börsscenario && playerCapital < 50000) return false;
            if (scenario == svartaMarknaden && playerKarma > 0) return false;
            // etc.
            return true;
        }*/

        // Metod för att initiera scenarier
        private List<Scenario> InitializeScenarios()
        {

            var scenarios = new List<Scenario>
            {
                /*0*/
                new Scenario(
                    "BOSTADSFÖRMEDLINGEN",
                    "En plats där hoppet om ett förstahandskontrakt lever, åtminstone för de få som inte ger upp innan de når pensionsåldern. \nHär förmedlas allt från kvadratmeter-skrubbar till överprisade etagevåningar. \nVilken bostad väljer du?\r\n",
                    new List <string>
                    {
                        "Takvåningen",
                        "Andrahandstvåan",
                        "Kollektivet"
                    },

                   new List<string>
                    {
                        "Slå till på en överprisad takvåning för 25,000 SEK i månaden – komplett med minimalistisk inredning och en designad \nkaffemaskin som du inte vet hur man använder. Dina rika grannar är SÅÅ imponerade, eller ja... de låtsas åtminstone \natt de vet vem du är.",
                        "Flytta in i en andrahandstvåa på 50 kvm för 10,000 SEK i månaden. Helt okej komfort, och ingen behöver veta att ditt 'kontor' \negentligen är matbordet. Du kanske inte vinner några sociala statuspoäng hos toppfolket, men grannarna nickar artigt \ni trapphuset.",
                        "Bo i ett kollektiv med stadens mest omtänksamma själar. Där delas såväl ekologiska grönsaker som djupa samtal om \norättvisor. Visst, du riskerar att bli utstött av stadens kostymklädda opportunister, men ekonomiskt är det ju en \nvinst!"
                    },
                    new List<double> { -20000, -10000, 0 }, //Ekonomisk påverkan
                    new List<int> { -5, +1, +5 }, // Karmapåverkan
                    new List<int> { +5, +2, -10 } // Social påverkan

                ),
                /*1*/
                new Scenario(
                    "BANKEN",
                    "Här kan du hantera dina pengar och investera i din framtid, men var beredd på att möta snåriga regler och tuffa beslut. \nBanken ger ett intryck av trygghet, men är också en plats där dina ekonomiska drömmar kan prövas. \nVad vill du göra?\r\n",
                                        new List <string>
                    {
                        "Spara på räntekonto"
                    },
                    new List<string>
                    {
                        "Du får en fantastisk ränta på 0,01% – perfekt för att se dina besparingar förvandlas till en skugga av vad de en gång \nvar! Kom ihåg: ju mer du sparar, desto mindre kan du faktiskt köpa. \nEn garanterad förlust i kampen mot inflationen!"
                    },
                    new List<double> { -15000}, //Ekonomisk påverkan
                    new List<int> { -5 }, // Karmapåverkan
                    new List<int> { -10 } // Social påverkan (exempelvärden)
                ),
                /*2*/
                  new Scenario(
                      "BÖRSEN",
                    "Här är snabba affärer och spekulationer regel snarare än undantag, och de privilegierade spelar ett riskfyllt spel med andras \nframtid. Medan finansvärlden jublar över sina vinster, kämpar många med att få ekonomin att gå \nihop. Välkommen till cirkusen där de få blir rika på de många – om du vågar delta..\nVad vill du investera i?\r\n",
                                        new List <string>
                    {
                        "Fluff & Fusk AB",
                        "Arbetsmiljökatastrof AB",
                        "Slit & släng AB"
                    },
                    new List<string>
                    {
                        "Hylla Profitsson och delta i en ‘Monumentfotoutmaning’: “Fotografera dig själv vid monumentet och posta med en hyllningstext. En okänd sponsor belönar fotot.” (Social Status +5, Kapital +5000 SEK, Karma -5)",
                        "Strunta i monumentet och gå vidare: “Du ignorerar hela spektaklet, vilket låter dig undvika både bra och dåliga reaktioner.” (Social Status 0, Kapital +0, Karma +2)",
                        "Placera en Protestskylt: “Du gör din röst hörd genom att sätta upp en protestskylt vid monumentet, vilket lockar uppmärksamhet från en lokal aktivistgrupp som erbjuder dig en symbolisk belöning.” (Karma +5, Social Status -3, Kapital +500 SEK)"
                    },
                    new List<double> { -15000, -12000, -8000 },
                    new List<int> { -5, -2, 0 }, // Karma påverkan (exempelvärden)
                    new List<int> { -10, -5, 0 } // Social påverkan (exempelvärden)
                ),
                  /*3*/
                   new Scenario(
                      "MONUMENTET PROFITSSON",
                    "Monumentet hyllar den kontroversiella industrimagnaten Torsten Profitsson, vars förmögenhet byggdes på girighet och exploatering. \nOmrådet är en pulserande knutpunkt för både beundrare som vill föreviga sig själva i Profitssons skugga och \naktivister som tar ställning mot hans arv. Välkommen till platsen där moral och makt krockar, och \ndär varje besökare tvingas välja sida.\nVad väljer du?\r\n",
                                       new List <string>
                    {
                        "Takvåningen",
                        "Andrahandstvåan",
                        "Kollektivet"
                    },
                    new List<string>
                    {
                        "Hylla Profitsson och delta i en ‘Monumentfotoutmaning’: “Fotografera dig själv vid monumentet och posta med en hyllningstext. En okänd sponsor belönar fotot.” (Social Status +5, Kapital +5000 SEK, Karma -5)",
                        "Strunta i monumentet och gå vidare: “Du ignorerar hela spektaklet, vilket låter dig undvika både bra och dåliga reaktioner.” (Social Status 0, Kapital +0, Karma +2)",
                        "Placera en Protestskylt: “Du gör din röst hörd genom att sätta upp en protestskylt vid monumentet, vilket lockar uppmärksamhet från en lokal aktivistgrupp som erbjuder dig en symbolisk belöning.” (Karma +5, Social Status -3, Kapital +500 SEK)"
                    },
                    new List<double> { -15000, -12000, -8000 },
                    new List<int> { -5, -2, 0 }, // Karma påverkan (exempelvärden)
                    new List<int> { -10, -5, 0 } // Social påverkan (exempelvärden)
                ),
                   /*4*/
                    new Scenario(
                      "ARBETSFÖRMEDLINGEN",
                    "Här samlas arbetssökande i hopp om jobb, men ofta möts de av ett virrvarr av byråkrati och systematiska brister. Medan några får \nhjälp att navigera, lämnas många kvar i limbo, bortglömda av systemet. Välkommen till platsen där hopp \noch besvikelse dansar en ständig tango..\nVill du skaffa ett jobb?\r\n",
                                        new List <string>
                    {
                        "Takvåningen",
                        "Andrahandstvåan",
                        "Kollektivet"
                    },
                    new List<string>
                    {
                        "Hylla Profitsson och delta i en ‘Monumentfotoutmaning’: “Fotografera dig själv vid monumentet och posta med en hyllningstext. En okänd sponsor belönar fotot.” (Social Status +5, Kapital +5000 SEK, Karma -5)",
                        "Strunta i monumentet och gå vidare: “Du ignorerar hela spektaklet, vilket låter dig undvika både bra och dåliga reaktioner.” (Social Status 0, Kapital +0, Karma +2)",
                        "Placera en Protestskylt: “Du gör din röst hörd genom att sätta upp en protestskylt vid monumentet, vilket lockar uppmärksamhet från en lokal aktivistgrupp som erbjuder dig en symbolisk belöning.” (Karma +5, Social Status -3, Kapital +500 SEK)"
                    },
                    new List<double> { -15000, -12000, -8000 },
                    new List<int> { -5, -2, 0 }, // Karma påverkan (exempelvärden)
                    new List<int> { -10, -5, 0 } // Social påverkan (exempelvärden)
                ),
                    /*5*/
                     new Scenario(
                      "NÄTVERKSTRÄFFEN",
                    "En exklusiv tillställning där självgoda entreprenörer samlas för att utbyta visitkort och prisa sin senaste ”geniala” affärsidé. \nHär flödar champagnen medan det diskuteras hur man ska maximera vinsten utan att blanda in tankar om etik eller socialt ansvar. \nVälkommen till en arena där relationer är allt och där de som har mest kapital alltid får sista ordet.\nVad vill du göra här?\r\n",
                                        new List <string>
                    {
                        "Takvåningen",
                        "Andrahandstvåan",
                        "Kollektivet"
                    },
                    new List<string>
                    {
                        "Hylla Profitsson och delta i en ‘Monumentfotoutmaning’: “Fotografera dig själv vid monumentet och posta med en hyllningstext. En okänd sponsor belönar fotot.” (Social Status +5, Kapital +5000 SEK, Karma -5)",
                        "Strunta i monumentet och gå vidare: “Du ignorerar hela spektaklet, vilket låter dig undvika både bra och dåliga reaktioner.” (Social Status 0, Kapital +0, Karma +2)",
                        "Placera en Protestskylt: “Du gör din röst hörd genom att sätta upp en protestskylt vid monumentet, vilket lockar uppmärksamhet från en lokal aktivistgrupp som erbjuder dig en symbolisk belöning.” (Karma +5, Social Status -3, Kapital +500 SEK)"
                    },
                    new List<double> { -15000, -12000, -8000 },
                    new List<int> { -5, -2, 0 }, // Karma påverkan (exempelvärden)
                    new List<int> { -10, -5, 0 } // Social påverkan (exempelvärden)
                ),
                     /*6*/
                      new Scenario(
                      "KAFÉ KAFFE & KAPITAL",
                    "Kaféet är det självklara valet för den som vill njuta av en överprisad espresso medan de diskuterar hur man kan maximera sin vinst \npå bekostnad av allt annat. Här samlas eliten för att prata affärer, medan de sippar på drycker som hade kunnat betala \nen månads hyra för en ensamstående förälder i förorten.\nVad vill du göra?\r\n",
                                        new List <string>
                    {
                        "Takvåningen",
                        "Andrahandstvåan",
                        "Kollektivet"
                    },
                    new List<string>
                    {
                        "Hylla Profitsson och delta i en ‘Monumentfotoutmaning’: “Fotografera dig själv vid monumentet och posta med en hyllningstext. En okänd sponsor belönar fotot.” (Social Status +5, Kapital +5000 SEK, Karma -5)",
                        "Strunta i monumentet och gå vidare: “Du ignorerar hela spektaklet, vilket låter dig undvika både bra och dåliga reaktioner.” (Social Status 0, Kapital +0, Karma +2)",
                        "Placera en Protestskylt: “Du gör din röst hörd genom att sätta upp en protestskylt vid monumentet, vilket lockar uppmärksamhet från en lokal aktivistgrupp som erbjuder dig en symbolisk belöning.” (Karma +5, Social Status -3, Kapital +500 SEK)"
                    },
                    new List<double> { -15000, -12000, -8000 },
                    new List<int> { -5, -2, 0 }, // Karma påverkan (exempelvärden)
                    new List<int> { -10, -5, 0 } // Social påverkan (exempelvärden)
                ),
                      /*7*/
                       new Scenario(
                      "GOLFKLUBBEN",
                    "Golfklubben är en fristad för de som älskar att slå på en liten boll medan de diskuterar hur de kan förädla sina skattefuskstrategier. \nHär klipper man gräset lika noggrant som man klipper bort de oönskade medlemmarna – du vet, de som fortfarande tror på jämlikhet. \nVad vill du göra?\r\n",
                                        new List <string>
                    {
                        "Takvåningen",
                        "Andrahandstvåan",
                        "Kollektivet"
                    },
                    new List<string>
                    {
                        "Hylla Profitsson och delta i en ‘Monumentfotoutmaning’: “Fotografera dig själv vid monumentet och posta med en hyllningstext. En okänd sponsor belönar fotot.” (Social Status +5, Kapital +5000 SEK, Karma -5)",
                        "Strunta i monumentet och gå vidare: “Du ignorerar hela spektaklet, vilket låter dig undvika både bra och dåliga reaktioner.” (Social Status 0, Kapital +0, Karma +2)",
                        "Placera en Protestskylt: “Du gör din röst hörd genom att sätta upp en protestskylt vid monumentet, vilket lockar uppmärksamhet från en lokal aktivistgrupp som erbjuder dig en symbolisk belöning.” (Karma +5, Social Status -3, Kapital +500 SEK)"
                    },
                    new List<double> { -15000, -12000, -8000 },
                    new List<int> { -5, -2, 0 }, // Karma påverkan (exempelvärden)
                    new List<int> { -10, -5, 0 } // Social påverkan (exempelvärden)
                ),
                       /*8*/
                        new Scenario(
                      "CASINOT",
                    "Casinot är en glittrande oas för de som har råd att leka med sina drömmar. Här kastas pengar bort som konfetti i en fest där lycka \när en kortvarig gäst. Med varje kortdragning och snurr glöms den ekonomiska verkligheten bort i jakten \npå jackpottar, men de flesta lämnar med tomma fickor och dyra minnen.\nVad vill du göra?\r\n",
                                       new List <string>
                    {
                        "Takvåningen",
                        "Andrahandstvåan",
                        "Kollektivet"
                    },
                    new List<string>
                    {
                        "Hylla Profitsson och delta i en ‘Monumentfotoutmaning’: “Fotografera dig själv vid monumentet och posta med en hyllningstext. En okänd sponsor belönar fotot.” (Social Status +5, Kapital +5000 SEK, Karma -5)",
                        "Strunta i monumentet och gå vidare: “Du ignorerar hela spektaklet, vilket låter dig undvika både bra och dåliga reaktioner.” (Social Status 0, Kapital +0, Karma +2)",
                        "Placera en Protestskylt: “Du gör din röst hörd genom att sätta upp en protestskylt vid monumentet, vilket lockar uppmärksamhet från en lokal aktivistgrupp som erbjuder dig en symbolisk belöning.” (Karma +5, Social Status -3, Kapital +500 SEK)"
                    },
                    new List<double> { -15000, -12000, -8000 },
                    new List<int> { -5, -2, 0 }, // Karma påverkan (exempelvärden)
                    new List<int> { -10, -5, 0 } // Social påverkan (exempelvärden)
                ),
            };

            return scenarios; // Returnera listan av scenarier
        }

        // Metod för att hantera spelarnas val i scenarier
        public void StartScenario(int scenarioIndex)
        {
            // Kolla att indexet är giltigt
            if (scenarioIndex < 0 || scenarioIndex >= _scenarios.Count)
            {
                Console.WriteLine("Ogiltigt scenario.");
                return;
            }

            var scenario = _scenarios[scenarioIndex]; // Hämta det aktuella scenariot
            Console.Clear();
            Player.ShowPlayerInfo(_player);
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{scenario.Localisation}");
            Console.WriteLine($"\n{scenario.Question}");
            Console.ResetColor();
            scenario.showImpacts();
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Ange [1], [2] eller [3] för att göra ditt val..");
            Console.ResetColor();

            int optionIndex = -1; // Variabel för spelarens val

            // Använd en while-loop för att hantera ogiltiga val
            while (optionIndex < 0 || optionIndex >= scenario.Options.Count)
            {
                string choice = Console.ReadLine();
                // Validera spelarens val
                if (int.TryParse(choice, out optionIndex) && optionIndex > 0 && optionIndex <= scenario.Options.Count)
                {
                    optionIndex--; // Justera för att passa indexet i listan

                    // Hämta påverkan baserat på spelarens val
                    double financialImpact = scenario.FinancialImpacts[optionIndex];
                    int karmaImpact = scenario.KarmaImpacts[optionIndex];
                    int socialImpact = scenario.SocialImpacts[optionIndex];

                    // Kontrollera om det är bankens scenario och om det är valet att spara
                    if (scenarioIndex == 1 && optionIndex == 0) // 0 representerar alternativet att spara
                    {
                        SaveMoney(); // Anropa metoden för att spara pengar
                    }
                    else
                    {
                        // Om användaren inte valde att spara, uppdatera spelarens status
                        _player.UpdateCapital(financialImpact);
                        _player.UpdateKarma(karmaImpact);
                        _player.UpdateSocialStatus(socialImpact);

                        // Visa resultatet av spelarens val
                        Console.Clear();
                        Player.ShowPlayerInfo(_player);
                        Console.WriteLine($"\nDu valde: {scenario.OptionName[optionIndex]}.");
                        Console.WriteLine($"\nEkonomisk påverkan: {financialImpact} SEK.");
                        Console.WriteLine($"Karmapåverkan: {karmaImpact}.");
                        Console.WriteLine($"Påverkan på social status: {socialImpact}.");
                        Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                        Console.ReadKey();
                    }

                    // Gå till spelmeny
                    Menu.GameMenu(this);

                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                    optionIndex = -1; // Återställ val
                }
            }


            Console.WriteLine("");
            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
            Console.ReadKey();
        }

        // Metod för att hantera sparande på räntekonto
        private void SaveMoney()
        {
            Console.Write("Hur mycket vill du spara? (SEK): ");
            string input = Console.ReadLine();
            double savingAmount;

            // Validera input och kontrollera att sparbeloppet är giltigt
            if (double.TryParse(input, out savingAmount) && savingAmount > 0)
            {
                // Kontrollera att spelaren har tillräckligt med kapital för att spara det angivna beloppet
                if (savingAmount <= _player.Capital) 
                {
                    double interestRate = 0.0001; // 0.01% ränta
                    double interestEarned = savingAmount * interestRate;

                    // Uppdatera spelarens kapital med enbart räntan
                    _player.UpdateCapital(interestEarned); // Lägg bara till räntan

                    // Visa meddelanden till spelaren
                    Console.WriteLine($"Du har valt att spara {savingAmount} SEK.");
                    Console.WriteLine($"Räntan på ditt sparande blir {interestEarned:F2} SEK.");
                    Console.WriteLine($"Ditt totala kapital är nu: {_player.Capital:F2} SEK."); // Visar det nuvarande kapitalet
                }
                else
                {
                    Console.WriteLine("Du har inte tillräckligt med kapital för att spara det angivna beloppet.");
                }
            }
            else
            {
                Console.WriteLine("Ogiltig input, vänligen ange ett giltigt belopp.");
            }

            // Återgå till menyn efter att ha sparat
            Console.WriteLine("Tryck på valfri tangent för att återgå till menyn...");
            Console.ReadKey();
        }


        // Metod för att avsluta spelet
        public void ExitGame()
        {
            _stopwatch.Stop(); // Stoppa tidtagaren när spelet avslutas
            TimeSpan totalTime = _stopwatch.Elapsed; // Hämta den totala tiden
            Console.Clear();
            Console.WriteLine("Tack för att du spelade Miljonärsresan!");
            Console.WriteLine($"Total speltid: {totalTime.Minutes} minuter och {totalTime.Seconds} sekunder.");
            Environment.Exit(0);
        }
    } // Avslutande klammerparentes för Game-klassen
} // Avslutande klammerparentes för namespace MillionaireGame
