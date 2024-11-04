/* Denna klass hanterar spelets logik och flödet av spelet. Den innehåller metoder för att starta ett nytt spel, ladda ett sparat spel, investera, starta företag och avsluta spelet. */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MillionaireGame
{
    internal class Game
    {
        private Player _player; // Referens till spelaren, ny instans av Player-klassen
        private List<Scenario> _scenarios; // Lagrar en ny tom lista över scenarios
        private Stopwatch _stopwatch; // Initierar tidtagare för total speltid
        private double _monthlyRent; // Initierar variabel som ska lagra hyresbelopp
        private double _totalTimePlayedInMinutes; // Lagrar sammanlagd speltid i minuter
        private Menu _menu; // Initierar menyhantering, ny instans av klassen Menu.
        private System.Timers.Timer _timer; // Sätter upp timern för kontinuerlig hyresdragning


        public Player Player // Offentlig egenskap för att komma åt spelaren
        {
            get { return _player; } // Getter för att hämta spelarinformation
        }

        public Game()
        {
            _scenarios = InitializeScenarios();
            _stopwatch = new Stopwatch();
            _menu = new Menu(_player);
        }



        /***** Metod för att starta spelet. Visar introduktion och huvudmeny. ******/
        public void Start()
        {
            Menu.ShowMainMenu(this); // Visar huvudmenyn
        }

        /***** Metod för att ladda ett sparat spel EV TA BORT?? *****/
        public void LoadSavedGame()
        {
            Console.Clear(); 
            Console.WriteLine("----------- VÄLJ SPARAT SPEL --------------");
            // Implementera logik för att ladda ett sparat spel if/else-sats
            Console.WriteLine("Inga sparade spel hittades. Tryck på [Enter] för att återgå till huvudmenyn..."); 
            Console.ReadLine(); 
            Menu.ShowMainMenu(this); // Går tillbaka till huvudmenyn
        }

        /***** Metod som ger spelintroduktion direkt efter att spelet startats *****/
        public void GameIntro()
        {
            Console.Clear();
            Console.WriteLine("----------- BAKGRUND --------------");
            Console.WriteLine("");
            Console.WriteLine("Du är en ung entreprenör som sedan barnsben fått lära dig att prestation och monetär framgång är de ");
            Console.WriteLine("enda vägarna till lycka. Under din uppväxt har du matats med budskapet att det som verkligen betyder ");
            Console.WriteLine("något är hur mycket pengar du tjänar och hur snabbt du kan klättra på den kapitalistiska stegen. ");
            Console.WriteLine("");
            Console.WriteLine("Du bestämmer dig för att flytta till staden Kapitalträsk, där varje beslut kan leda antingen till ");
            Console.WriteLine("förmögenhet eller fördömelse. Här kan du investera i tvivelaktiga aktier, diskutera strategier för ");
            Console.WriteLine("skattefusk och gå till Casinot.");
            Console.WriteLine("");
            Console.WriteLine("Ditt mål är enkelt: bli miljonär. Men inte ens en miljonär står ut med sig själv efter för många ");
            Console.WriteLine("tvivelaktiga beslut. Om din karma sjunker till noll, är spelet över – ingen summa pengar kan kompensera ");
            Console.WriteLine("för en tom själ. Kapitalträsk mäter också din sociala status; faller den för lågt kan dina möjligheter ");
            Console.WriteLine("att mingla med den rätta sortens folk försvinna, och med dem chanserna till de riktigt lönsamma affärerna.");
            Console.WriteLine("");
            Console.WriteLine("Så kan du bli stadens nästa miljonär utan att helt förlora dig själv?");
            Console.WriteLine("");
            Console.WriteLine("Tryck på [Enter] för att fortsätta...");
            Console.ReadLine();
            StartNewGame(); // Startar det nya spelet
        }


        /****** Metod för att startar en ny spelomgång spel *****/
        public void StartNewGame()
        {
            Console.Clear();
            Console.WriteLine("------------- NYTT SPEL ---------------");
            Console.WriteLine("");
            Console.WriteLine("Skriv ditt namn:");

            string playerName = Console.ReadLine(); // Tar emot spelarens namn
            _player = new Player(playerName, 70000, 70, this); // Skapar en ny spelare med startkapital och karma

            // Felhantering
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


        /***** Metod för att starta och ställa in timer ******/
        public void StartTimer()
        {
                _timer = new System.Timers.Timer(180000); // Timer inställd på 2 minuter (120000ms)
                _timer.Elapsed += CheckRent; // Anropa OnTimedEvent varje gång timern tickar
                _timer.AutoReset = true; // Återställ timern till 0
                _timer.Enabled = true; // Starta timern
            }


        /****** Metod för hyresdragning som går att använda med Elapsed-händelsen ******/
        private void CheckRent(Object source, ElapsedEventArgs e)
        {
            _player.Capital += _monthlyRent; // Dra hyran från spelarens kapital

            // Informera spelaren om hyresbetalningen
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nDu har betalat hyra: {_monthlyRent} SEK. Ditt kapital är nu: {_player.Capital} SEK.");
            Console.ResetColor();

            CheckGameStatus(_player); // Kontrollera spelstatus
        }


        /***** Metod som välkomnar spelare med namn och förklarar regler *****/
        public void welcomePlayer()
        {
            Console.Clear();
            Console.WriteLine($"Välkommen {_player.Name}!"); 
            Console.WriteLine("Du har precis tagit dina första steg i Kapitalträsk, där beslut om pengar och moral väger tungt.");
            Console.WriteLine("");
            Console.WriteLine("Spelets regler:");
            Console.WriteLine("- Du startar med 70 000 SEK i kapital och 70 karmapoäng");
            Console.WriteLine("- Varje beslut du tar kommer att påverka ditt kapital och karma positivt eller negativt.");
            Console.WriteLine("- Varje månad (3 minuter i speltid) kommer du att behöva betala din hyra.");
            Console.WriteLine("- Målet är att uppnå miljonärstatus genom att fatta strategiska beslut och hålla dig över vattenytan.");
            Console.WriteLine("");
            Console.WriteLine("Är du redo att bli nästa stjärnskott i Kapitalträsk? Det är upp till dig att göra rätt val – eller de mest lönsamma!\r\n");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Tryck på [Enter] för att börja ditt äventyr!");
            Console.ReadLine(); 
            _stopwatch.Start(); // Startar tidtagning för total speltid
            StartTimer(); // Startar timer för hyresdragning
            StartScenario(0); //Startar första scenariot
        }

        /***** Metod som hantera spelarens boendeval *****/
        public void HandlePlayerHouseChoice(int optionIndex)
        {
            Console.Clear();

            // Kolla om _scenarios[0] och FinancialImpacts finns
            if (_scenarios != null && _scenarios[0]?.FinancialImpacts != null)
            {
                // Kontrollera om det angivna indexet för boendeval är giltigt
                if (optionIndex >= 0 && optionIndex <= _scenarios[0].FinancialImpacts.Count)
                {
                    //Hämta hyrsebeloppet för det valda alternativet
                    double? rentValue = _scenarios[0].FinancialImpacts[optionIndex];
                    _monthlyRent = rentValue.Value; // Lagra månadshyra i _monthlyRent

                    Console.WriteLine($"Hyra satt till {_monthlyRent} SEK för alternativ {optionIndex}.");
                }
                else
                {
                    Console.WriteLine("Ogiltigt val, ingen hyra satt..");
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt scenario..");
            }
        }



        /***** Metod som omvandlar och returnerar tid från stopwatch till år och månader ****/
        public (int years, int months) GetTotalTimePlayed()
        {
            double totalMinutes = _stopwatch.Elapsed.TotalMinutes;
            int totalMonths = (int)(totalMinutes / 3); // 2 minuter = 1 månad

            int years = totalMonths / 12; // Beräkna antal år
            int months = totalMonths % 12; // Beräkna kvarvarande månader

            return (years, months);
        }


        /***** Metod för att skriva ut den omvandlade tiden *****/
        public void PrintTotalTimePlayed()
        {
            
            var (years, months) = GetTotalTimePlayed(); // Hämta den fiktiva speltiden
            string timeString = "";  // Lagra den formaterade tiden

            // Formatera utskriften baserat på år
            if (years > 0)
            {
                timeString += $"{years} år"; // Lägg till år i tidssträngen
            }

            // Formetera utskriften baserat på månader
            if (months > 0)
            {
                if (years > 0)
                {
                    timeString += " och "; // Lägg till "och" om det finns både år och månader
                }
                timeString += $"{months} månader"; // Lägg till månader i tidssträngen
            }

            // Standardtext om ingen tid spelats än
            if (timeString == "")
            {
                timeString = "Nyinflyttad";

            }

            Console.WriteLine($"| Total tid spelad: {timeString} |");
        }

        /**** Metod för att visa spelarparametrar consolen ****/
        public void DisplayPlayerInfo()
        {
            Console.Clear(); // Rensa konsolen
            Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information
        }

            /***** Metod för att initiera en lista med olika scenarier *****/
            private List<Scenario> InitializeScenarios()
        {

            var scenarios = new List<Scenario>
            {
                /*index 0*/
                new Scenario(
                    "VÄLKOMMEN TILL BOSTADSFÖRMEDLINGEN",
                    "Som  nyinflyttad i Kapitalträsk behöver du någonstans att bo. Du har därför tagit dig till bostadsförmedlingen - en \nplats där hoppet om ett förstahandskontrakt lever, åtminstone för de få som inte ger upp innan de når pensionsåldern. \nHär förmedlas allt från kvadratmeter-skrubbar till överprisade etagevåningar. \nVilken bostad väljer du?\r\n",
                    
                    new List <string>
                    {
                        "Takvåningen",
                        "Andrahandstvåan",
                        "Kollektivet"
                    },

                   new List<string>
                    {
                        "Slå till på en överprisad takvåning för 25,000 SEK i månaden – komplett med minimalistisk inredning och en designad \nkaffemaskin som du inte vet hur man använder. Dina rika grannar är SÅÅ imponerade, eller ja... de låtsas åtminstone \natt de vet vem du är.",
                        "Flytta in i en andrahandstvåa på 50 kvm för 10,000 SEK i månaden. Helt okej komfort, och ingen behöver veta att ditt \n'kontor' egentligen är matbordet. Du kanske inte vinner några sociala statuspoäng hos toppfolket, men grannarna \nnickar artigt i trapphuset.",
                        "Bo i ett kollektiv med stadens mest omtänksamma själar. Där delas såväl ekologiska grönsaker som djupa samtal om \norättvisor. Visst, du riskerar att bli utstött av stadens kostymklädda opportunister, men ekonomiskt är det ju en \nvinst!"
                    },
                    
                    new List<int> { -5, +1, +3 }, // Karmapåverkan
                    new List<double?> { -25000, -10000, -1000 }, //Ev. Fast ekonomisk påverkan
                    new List<double?> { null, null, null } //Ev. Procentuell påverkan på investerat kapital

                ),
                /*index 1*/
                new Scenario(
                    "BANKEN",
                    "Här kan du hantera dina pengar och investera i din framtid, men var beredd på att möta snåriga regler och tuffa beslut. \nBanken ger ett intryck av trygghet, men är också en plats där dina ekonomiska drömmar kan prövas. \nVad vill du göra?\r\n",
                                        
                    new List <string>
                    {
                        "Spara på räntekonto"
                    },

                    new List<string>
                    {
                        "Du får en fantastisk ränta på 0,1% – perfekt för att se dina besparingar förvandlas till en skugga av vad de en gång \nvar! En garanterad förlust i kampen mot inflationen!"
                    },

                    new List<int> {-1}, // Karmapåverkan
                    new List<double?> { null }, // Ev. Fast ekonomisk påverkan
                    new List<double?> {0.01} // Ev. Procentuell påverkan på investerat kapital
                ),
                /*index 2*/
                  new Scenario(
                    "BÖRSEN",
                    "Här är snabba affärer och spekulationer regel snarare än undantag, och de privilegierade spelar ett riskfyllt spel med \nandras framtid. Medan finansvärlden jublar över sina vinster, kämpar många med att få ekonomin att gå ihop. Välkommen \ntill cirkusen där de få blir rika på de många – om du vågar delta..\nVad vill du investera i?\r\n",
                                        
                    new List <string>
                    {
                        "Makt & Profit AB",
                        "Arbetsmiljökatastrof AB",
                        "Slit & släng Mode AB"
                    },

                    new List<string>
                    {
                        "Ett företag som är djupt involverat i den globala vapenhandeln och försörjer nationer som prioriterar styrka framför \nfred. Med starka kopplingar till maktgalna regimer erbjuder de en rad produkter och tjänster som hjälper dessa \nländer att stärka sin militära kapacitet. Men hey, det man inte ser själv lider man inte av.. Eller hur?",
                        "Detta företag har blivit känt för sina usla arbetsvillkor och cyniska affärsstrategier. Genom att drastiskt reducera \npersonalstyrkan har de kvarvarande anställda tvingats arbeta dygnet runt, utan möjlighet till pauser. För ledningen \nhandlar allt om att maximera vinsten på bekostnad av sina anställdas hälsa och välbefinnande.",
                        "Företaget erbjuder kläder som är avsedda att användas en gång och sedan kastas bort, vilket är helt i enlighet med \nsamhällets norm. Med lockande priser och ständigt nya kollektioner står de i centrum av en modeindustri som \nprioriterar kvantitet framför kvalitet. "
                    },
                    
                    new List<int> { -20, -15, -10 }, // Karma påverkan (exempelvärden)
                    new List<double?> { null, null, null }, // Ev. Fast ekonomisk påverkan
                    new List<double?> {0.8, 0.6, 0.4} // Ev. Procentuell påverkan på investerat kapital
                ),
                  /*index 3*/
                   new Scenario(
                    "MONUMENTET PROFITSSON",
                    "Monumentet hyllar den kontroversiella industrimagnaten Torsten Profitsson, vars förmögenhet byggdes på girighet och \nexploatering. Området är en pulserande knutpunkt för både beundrare som vill föreviga sig själva i Profitssons skugga \noch aktivister som tar ställning mot hans arv. Välkommen till platsen där moral och makt krockar, och \ndär varje besökare tvingas välja sida.\nVad väljer du?\r\n",
                                       
                    new List <string>
                    {
                        "Monumentfotoutmaning",
                        "Ignorera Monumentet",
                        "Sätt upp en protestskylt"
                    },

                    new List<string>
                    {
                        "Hylla Profitsson och delta i en Monumentfotoutmaning genom att otografera dig själv vid monumentet och posta med \nhashtagen #InspiredByProfitsson. En okänd sponsor belönar fotot med 1000 kr!",
                        "Strunta i monumentet och gå vidare, du låter dig undvika både bra och dåliga reaktioner.",
                        "Du gör din röst hörd genom att sätta upp en protestskylt vid monumentet, vilket lockar uppmärksamhet från en lokal \naktivistgrupp som erbjuder dig en symbolisk belöning om 500 kronor."
                    },
                    
                    new List<int> { -5, 0, +10 }, // Karma påverkan (exempelvärden)
                    new List<double?> { +1000, 0, +500 }, // Ev. Fast ekonomisk påverkan
                    new List<double?> {null, null, null} // Ev. Procentuell påverkan på investerat kapital
                ),
                   /*index 4*/
                    new Scenario(
                    "ARBETSFÖRMEDLINGEN",
                    "Här samlas arbetssökande i hopp om jobb, men ofta möts de av ett virrvarr av byråkrati och systematiska brister. Medan \nnågra får hjälp att navigera, lämnas många kvar i limbo, bortglömda av systemet. Välkommen till platsen där hopp \noch besvikelse dansar en ständig tango..\nVill du skaffa ett jobb?\r\n",
                                        
                    new List <string>
                    {
                        "Telefonförsäljare för UnicornWonders",
                        "Städare på skandalhotell",
                        "PR-koordinator på Lifestyle-byrå"
                    },
                    new List<string>
                    {
                        "Jobbet går ut på att ringa ensamma pensionärer och övertyga dem om att prenumerera på månatliga enhörningshattar! \nVarje köp sägs ge “ungdomlig energi”. Hög provision väntar, men kollegorna varnar för besvärade anhöriga. ",
                        "Som hotellstädare på stadens ökända skandalhotell kommer du att få uppleva glansen av en aldrig avslutad fest! \nFörutom att hantera spår från gårdagens VIP-gäster – ett smörgåsbord av märkliga fläckar, övergivna \ndyrgripar och en och annan söndrig minibar – ingår även sanering av rykande toaletter och omsorgsfullt bortplockande \nav opassande souvenirer.",
                        "Din uppgift är att skriva pressmeddelanden, kalla till event och springa runt och ordna allt från lyxiga goodiebags \ntill platser på trendiga restauranger. Lönen må vara knapp, men du får chans att “bygga ditt nätverk” \noch, om du har tur, ta en selfie i minglet för dina egna sociala medier."
                    },

                    new List<int> { -10, +5, +5 }, // Karma påverkan (exempelvärden)
                    new List<double?> { +20000, +10000, +1000 }, // Ev. Fast ekonomisk påverkan
                    new List<double?> {null, null, null} // Ev. Procentuell påverkan på investerat kapital
                 ),
                    /*index 5*/
                     new Scenario(
                    "VALLOKALEN",
                    "Välkommen till vallokalen, där den demokratiska illusionen frodas! Här har du chansen att rösta på ett av tre \nfantastiska partier – välj klokt, för din framtid hänger på att du gör det bästa valet bland det sämsta \nsom erbjuds! Lägg en röst vetja!\r\n",
                                        
                    new List <string>
                    {
                        "Framgångspartiet (FP)",
                        "Partiet för öppen exkludering (PFÖE)",
                        "Kollektiva partiet (KP)"
                    },
                    new List<string>
                    {
                        "Rösta på oss för en skattelättnad på 10 000 kronor! Visst, barn, äldre och sjuka kanske drabbas av en tynande välfärd, \nmen det påverkar ju inte dig. Låt din framgång vara det enda som räknas!",
                        "Vi skapar en värld där alla som passar in i vår snäva mall får sin plats – resten får gärna stanna utanför. En röst \npå oss innebär 20 000 kronor mer i din plånbok tack vare vår effektiviserade integrationsstrategi som \nprioriterar avhumanisering framför empati.",
                        "Vi skapar en värld där miljö och välfärd går hand i hand. Genom att rösta på oss bidrar du till en grönare framtid, \nmed investeringar i hållbara lösningar och ett stärkt socialt skyddsnät. Din skatt går till att förbättra \nlivskvaliteten för alla, med fokus på ren energi, utbildning och sjukvård. Du betalar 5 000 kronor \nmer i skatt som gör du en betydande skillnad för samhället."
                    },

                    new List<int> { -10, -20, 5 }, // Karma påverkan (exempelvärden)
                    new List<double?> { +10000, +20000, -5000 }, // Ev. Fast ekonomisk påverkan
                    new List<double?> {null, null, null} // Ev. Procentuell påverkan på investerat kapital
                ),
                     /*index 6*/
                      new Scenario(
                      "KAFÉ KAFFE & KAPITAL",
                      "Kaféet är det självklara valet för den som vill njuta av en överprisad espresso medan de diskuterar hur man kan \nmaximera sin vinst på bekostnad av allt annat. Här samlas eliten för att prata affärer, medan de sippar på \ndrycker som hade kunnat betala en månads hyra för en ensamstående förälder i förorten.\nVad vill du göra?\r\n",
                                        
                      new List <string>
                    {
                        "Bli kaffe-influencer",
                        "Sälj in din egenskapade kaffesyrup",
                        "Köp en överprisad espresso"
                    },

                    new List<string>
                    {
                        "Bli en kaffe-influencer och åk runt till olika kaféer för att promota deras överprisade kaffe som 'sååå lyxigt och \nprisvärt'. Dela dina upplevelser på sociala medier och tjäna 5 000 kr för varje kopp du recenserar!",
                        "Sälj in din egen sirap med den förbluffande smaken av fuktig kattsträva! Perfekt för att förvandla vilken dryck som \nhelst till en vild smakupplevelse. Tjäna 3 000 kronor på att göra denna udda delikatess till en hit på \nkaféet!",
                        "Köp en överprisad espresso för att visa att du också tillhör eliten. Varje klunk påminner dig om både kaffets styrka \noch pengarnas makt över vardagen. Betala 200 kr för att sitta kvar och kanske höra ett eller annat hemligt \ninvesteringstips från de andra kafégästerna!"
                    },
                    
                    new List<int> { -5, -3, -1}, // Karma påverkan (exempelvärden)
                    new List<double?> { 5000, 3000, -200}, // Ev. Fast ekonomisk påverkan
                    new List<double?> {null, null, null} // Ev. Procentuell påverkan på investerat kapital
                ),
                      /*index 7*/
                       new Scenario(
                      "GOLFKLUBBEN",
                      "Golfklubben är en fristad för de som älskar att slå på en liten boll medan de diskuterar hur de kan förädla sina \nskattefuskstrategier. Här klipper man gräset lika noggrant som man klipper bort de oönskade medlemmarna – du vet, \nde som fortfarande tror på jämlikhet. Vad vill du göra?\r\n",
                                        
                      new List <string>
                    {
                        "Anordna en föreläsning",
                        "Investera i luftslott",
                        "Ta ett jobb som caddy"
                    },

                    new List<string>
                    {
                        "Håll en föreläsning om hur man kan använda sina golfaktiviteter för att skapa avdrag i skattedeklarationen. Tjäna \n10 000 kr genom att sälja biljetter till denna exklusiva session.",
                        "Bjud ut två av dina manliga affärskontakter på en golfrunda. Under spelets gång öppnar sig möjligheten att investera \ni ett nystartat företag som säljer luftslott. Garanterat 100% i avkastning!",
                        "Som caddy får du chansen att gå i elitens fotspår – bokstavligt talat! Tjäna 15 000 kr på att bära deras tunga \nklubbor medan du elegant undviker deras ständiga skrytande om framgångar och affärer."
                    },
                   
                    new List<int> { -10, -5, +3 }, // Karma påverkan (exempelvärden)
                    new List<double?> { 10000, null, 5000 }, // Ev. Fast ekonomisk påverkan
                    new List<double?> {null, 1, null} // Ev. Procentuell påverkan på investerat kapital
                ),
                       /*index 8*/
                      new Scenario(
                      "CASINOT",
                      "Casinot är en glittrande oas för de som har råd att leka med sina drömmar. Här kastas pengar bort som konfetti i en \nfest där lycka är en kortvarig gäst. Med varje kortdragning och snurr glöms den ekonomiska verkligheten bort i \njakten på jackpottar, men de flesta lämnar med tomma fickor och dyra minnen. Vad vill du göra?\r\n",
                                       
                      new List <string>
                    {
                        "Spela roulette",
                        "Spela sic bo",
                        "Spela på enarmad bandit"
                    },

                    new List<string>
                    {
                        "Sätt dina marker på bordet och känn pulsen stiga! Här har du 50% chans att dubbla dina satsade pengar. Kommer du att \nse rött eller svart?",
                        "Satsa på tärningarnas utfall i detta spännande tärningsspel! Om du satsar på ett specifikt nummer och vinner, har du \n16% chans att få upp till 6 gånger din insats! Det är tur som gäller när tärningarna rullas!",
                        "Dra i spaken och hoppas på det bästa! Här har du endast 1% chans att vinna hela 10 gånger din satsning. Kommer du att \nbli den lyckliga som går hem med storvinsten, eller är du bara ännu en spelare som lämnar med tomma fickor?"
                    },
                    
                    new List<int> { -1, -1, -1 }, // Karma påverkan (exempelvärden)
                    new List<double?> { null, null, null }, // Ev. Fast ekonomisk påverkan
                    new List<double?> {null, null, null} // Ev. Procentuell påverkan på investerat kapital
                ),
                   /*índex 9*/
                        new Scenario(
                      "VÄLGÖRENHETSGALAN",
                      "Välgörenhetsgalan anordnas för att hjälpa stadens utsatta och samla in medel till lokala välgörenhetsorganisationer. \nDeltagarna får möjlighet att bidra till olika ändamål som stödjer utbildning för barn, djurens välbefinnande och \nmiljöskydd. Varje donation gör en skillnad! \r\n",
                                       
                      new List <string>
                    {
                        "Donera till Barnens Framtid",
                        "Stöd Djurens Hem",
                        "Bidra till miljöinsatser"
                    },
                    
                      new List<string>
                    {
                        "Det kanske inte ligger i din natur att donera dina surt förvärvade slantar – du måste ju försörja ditt dyra leverne! \nMen vad gör man inte för en rejäl karmaboost? Med en donation på 20 000 kronor kan du ge barn i utsatthet en chans \ntill utbildning och en bättre framtid.",
                        "För 4 000 kronor kan du ge hemlösa djur en trygg plats. Visst kan det kännas jobbigt att släppa ifrån dig en del av \nditt hårt tjänade kapital, men tänk på att även de fyrbenta vännerna förtjänar en chans. Karma i retur, kanske?",
                        "Till och med 500 kronor kan kännas jobbigt att skänka... Men det är bättre än att bara klaga. Ditt stöd hjälper till \natt bevara planeten – och kanske ger det din karma en uppgradering inför nästa semester. Varför inte?"
                    },

                    new List<int> { +40, +20, +5 }, // Karma påverkan (exempelvärden)
                    new List<double?> { -20000, -4000, -500 }, // Ev. Fast ekonomisk påverkan
                    new List<double?> {null, null, null} // Ev. Procentuell påverkan på investerat kapital
                ),
            };

            return scenarios; // Returnera listan av scenarier
        }

        /***** Metod för att starta det scenario användaren väljer via spelmenyn ****/
    public void StartScenario(int scenarioIndex)
        {
            Console.Clear();
            Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information

            // Kolla att scenarioindexet är giltigt
            if (scenarioIndex < 0 || scenarioIndex >= _scenarios.Count)
            {
                Console.WriteLine("Ogiltigt scenario.");
                return;
            }

            var scenario = _scenarios[scenarioIndex]; // Hämta det valda scenariot
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{scenario.Localisation}"); // Skriv ut scenariorubrik
            Console.WriteLine($"\n{scenario.Question}"); // Skriv ut scenariobeskrivning
            Console.ResetColor();
            scenario.ShowImpacts(); // Skriv ut ekonomisk- och karmapåverkan
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Ange siffran för ditt val eller tryck [0] för att gå tillbaka till spelmeny..");
            Console.ResetColor();

            int optionIndex = GetValidOption(scenario); // Hämta giltigt val baserat på spelarens inmatning

            // Anropa HandlePlayerHouseChoice oavsett scenarioIndex (fick inte till det på annat sätt)
            HandlePlayerHouseChoice(optionIndex);

            if (HasReturn(scenario, optionIndex))
            {
                HandleInvestmentScenario(scenario, optionIndex); //Hantera scenarier med procentuell avkastning
            }
            else
            {
                HandleNonReturnScenario(scenario, optionIndex, scenarioIndex); //Hantera scenarier med fast avkastning
            }
        }

        /***** Metod för att hämta ett giltigt alternativ/option från scenarie från användaren ****/
        private int GetValidOption(Scenario scenario)
        {
            int optionIndex = -1; // Initiera indexet för alternativ till ogiltigt värde

            // Fortsätt be om inmatning av alternativ tills ett giltigt val görs
            while (optionIndex < 0 || optionIndex >= scenario.Options.Count)
            {
                string choice = Console.ReadLine();

                // Kontrollera om spelaren vill gå tillbaka till menyn
                if (choice == "0") 
                {
                    Menu.GameMenu(this);
                    return -1; // Returnera ogiltigt värde för att signalera att användaren valt att gå tillbaka
                }

                if (int.TryParse(choice, out optionIndex) && optionIndex > 0 && optionIndex <= scenario.Options.Count)
                {
                    optionIndex--; // Justera för index
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                    optionIndex = -1;
                }
            }
            return optionIndex;
        }


        /***** Metod för att kontrollera om scenariot har en procentuell avkastning *****/
        private bool HasReturn(Scenario scenario, int optionIndex)
        {
            // Kontrollera att Returns-listan existerar och att indexet är giltigt för denna lista
            if (scenario.Returns == null || optionIndex < 0 || optionIndex >= scenario.Returns.Count)
            {
                return false; // Ingen avkastning om någon av dessa är ogiltiga
            }

            // Kontrollera om det finns en faktisk avkastning för det valda alternativet
            return scenario.Returns[optionIndex].HasValue;
        }


        /****** Metod för att hantera scenarion som innefattar investering och procentuell avkastning ****/
        private void HandleInvestmentScenario(Scenario scenario, int optionIndex)
        {
            Console.Clear();
            Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information

            Console.WriteLine($"Hur mycket vill du investera i {scenario.OptionName[optionIndex]}?");
            double investmentAmount; // initiera variabel som ska lagra investerad summa

            // Kontrollera att användaren har tillräckligt med kapital för att investera vald summa
            while (!double.TryParse(Console.ReadLine(), out investmentAmount) || investmentAmount <= 0 || investmentAmount > _player.Capital)
            {
                Console.WriteLine(investmentAmount > _player.Capital
                    ? "Du har inte tillräckligt med kapital för denna investering. Försök igen."
                    : "Ogiltigt belopp. Ange ett positivt belopp för din investering.");
            }
            
            Random random = new Random(); // Generera randomiserat nummer mellan 1 och 2
                                         
            bool SuccessfulInvestment = (optionIndex == 1) || (random.NextDouble() <= 0.9);  // Kontrollera om det är scenario med index 1 och ge 100% chans till framgång, annars 80%


            DisplayPlayerInfo(); // Visa spelarinformation
            Console.WriteLine($"Du har investerat {investmentAmount:F2} SEK i {scenario.OptionName[optionIndex]}.");

            double profit; // Variabel som ska lagra eventuell avkastning

            // Kontrollera om investeringen lyckades eller inte
            if (SuccessfulInvestment)
            {
                // Vinst om investeringen lyckades
                double returnRate = scenario.Returns[optionIndex].Value;
                profit = investmentAmount * returnRate;
                Console.WriteLine($"Investeringen gick bra! Din avkastning blev {profit:F2} SEK.");
            }
            else
            {
                //Förlust om investeringen misslyckades
                profit = -investmentAmount;
                Console.WriteLine($"Investeringen misslyckades, du förlorade dina {investmentAmount} SEK");
            }

            // Hämta karmapåverkan
            int karmaImpact = scenario.KarmaImpacts[optionIndex]; // Hämta karmapåverkan från listan

            // Uppdatera spelarparametrar och kontrollera så att de inte är 0
            _player.UpdateCapital(profit);
            _player.UpdateKarma(karmaImpact);
            CheckGameStatus(_player);

            Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
            Console.ReadKey();

            Menu.GameMenu(this); // Återgå till menyn
        }


        /***** Metod för att hantera casinoscenario och scenarion med fast ekonomisk påverkan *****/
        private void HandleNonReturnScenario(Scenario scenario, int optionIndex, int scenarioIndex)
        {
            Console.Clear();

            // Kontrollera om detta är ett casinospel utan ekonomisk påverkan eller avkastning
            if (scenarioIndex == 8)
            {
                // Skicka spelaren till rätt spelmetod baserat på val
                switch (optionIndex)
                {
                    case 0:
                        PlayRoulette(scenario, optionIndex);
                        break;
                    case 1:
                        PlaySicBo(scenario, optionIndex);
                        break;
                    case 2:
                        PlayBandit(scenario, optionIndex);
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val.");
                        break;
                }

                Menu.GameMenu(this);
                return; // Avsluta för att undvika att fortsätta nedan
            }

            // Hantera vanliga scenarion med fast ekonomisk påverkan
            double financialImpact = scenario.FinancialImpacts != null && optionIndex < scenario.FinancialImpacts.Count
                ? scenario.FinancialImpacts[optionIndex] ?? 0 : 0; // Om FinancialImpacts finns och optionIndex är giltigt, hämta värdet; annars sätt till 0.

            int karmaImpact = scenario.KarmaImpacts != null && optionIndex < scenario.KarmaImpacts.Count
                ? scenario.KarmaImpacts[optionIndex] : 0; // Om KarmaImpacts finns och optionIndex är giltigt, hämta värdet; annars sätt till 0.

            // Uppdatera spelarparametrar och kontrollera så att de inte är 0.
            _player.UpdateCapital(financialImpact);
            _player.UpdateKarma(karmaImpact);
            CheckGameStatus(_player);

            Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information

            Console.WriteLine($"\nDu valde: {scenario.OptionName[optionIndex]}.\n");
            Console.WriteLine($"Ekonomisk påverkan: {financialImpact} SEK.");
            Console.WriteLine($"Karmapåverkan: {karmaImpact}.");
            Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
            Console.ReadKey();

            Menu.GameMenu(this); // Återgå till spelmeny
        }


        /**** Metod för att hantera spel på roulette *****/
        private void PlayRoulette(Scenario scenario, int optionIndex)
        {
            Console.Clear();
            Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information

            Console.WriteLine($"\nDu valde: {scenario.OptionName[optionIndex]}.\n");
            Console.WriteLine("Vill du satsa på [1] rött eller [2] svart?");
            string colorChoice = Console.ReadLine();

            // Kontrollera att val är 1 eller 2
            if (colorChoice != "1" && colorChoice != "2")
            {
                Console.WriteLine("Ogiltigt val. Försök igen!");
                return;
            }

            Console.WriteLine("Vilken summa vill du satsa?");
            string input = Console.ReadLine();
            double investmentAmount; // Variabel som ska lagra satsad summa

            // Om det går att omvandla spelarens input till ett double-värde, kör kod:
            if (double.TryParse(input, out investmentAmount) || investmentAmount <= 0)
            {
                // Kontrollera att spelaren har tillräckligt med kapital för att spara det angivna beloppet
                if (investmentAmount <= _player.Capital)
                {
                    Random rand = new Random(); // Slumpa ett nummer mellan 1-2
                    int winningColor = rand.Next(1, 3); // 1= röd, 2 = svart

                    // Kontrollera om spelarens val matchar slumpat resultat
                    if (colorChoice == winningColor.ToString())
                    {
                        // Om ja, spelare vinner
                        _player.UpdateCapital(investmentAmount * 2);
                        _player.UpdateKarma(scenario.KarmaImpacts[0]);
                        CheckGameStatus(_player);
                        Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information
                        Console.WriteLine($"Grattis, du vann {investmentAmount * 2} SEK!");
                        Console.WriteLine($"Ditt totala kapital är nu: {_player.Capital:F2} SEK");
                    }
                    else
                    {
                        // Om nej, spelare förlorar
                        _player.UpdateCapital(-investmentAmount);
                        _player.UpdateKarma(scenario.KarmaImpacts[0]);
                        CheckGameStatus(_player);
                        Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information
                        Console.WriteLine($"Tyvärr, du förlorade de {investmentAmount} SEK du satsade");
                        Console.WriteLine($"Ditt totala kapital är nu: {_player.Capital:F2} SEK");
                    }

                    Console.WriteLine("Tryck på [Enter] för att återgå till menyn..");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Du har inte tillräckligt med kapital för att spara det angivna beloppet.");
                    return;
                }
            }

            else
            {
                Console.WriteLine("Ogiltig input, vänligen ange ett giltigt belopp.");
                return;
            }
        }

        //Metod för att spela på Sic Bo
        private void PlaySicBo(Scenario scenario, int optionIndex)
        {
            Console.Clear();
            Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information

            Console.WriteLine($"\nDu valde: {scenario.OptionName[optionIndex]}.\n");
            Console.WriteLine("Välj ett nummer att satsa på [1-6]");

            int numberChoice; // Variabel som ska lagra satsad siffra


            if (!int.TryParse(Console.ReadLine(), out numberChoice) || numberChoice < 1 || numberChoice > 6)
            {
                Console.WriteLine("Ogiltigt nummer. Försök igen.");
                return;
            }

            // Satsning
            Console.WriteLine("Vilken summa vill du satsa?");
            string input = Console.ReadLine();
            double investmentAmount; // Variabel som ska lagra satsad summa

            // Om det går att omvandla spelarens input till ett double-värde, kör kod:
            if (double.TryParse(input, out investmentAmount) || investmentAmount <= 0)
            {
                // Kontrollera att spelaren har tillräckligt med kapital för att spara det angivna beloppet
                if (investmentAmount <= _player.Capital)
                {
                    Random rand = new Random(); // Slumpa resultat av roulette
                    int winningNumber = rand.Next(1, 7); // Motsvarar siffrorna på en tärning

                    // Kontrollera om slumpad siffra matchar spelarens valda siffra
                    if (numberChoice == winningNumber)
                    {
                      
                        // Om ja, spelaren vinner
                        _player.UpdateCapital(investmentAmount * 6);
                        _player.UpdateKarma(scenario.KarmaImpacts[0]);
                        CheckGameStatus(_player);
                        Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information
                        Console.WriteLine($"Grattis, du vann {investmentAmount * 6:F2} SEK!");
                        Console.WriteLine($"Ditt totala kapital är nu: {_player.Capital:F2} SEK");
                    }
                    else
                    {
                       // Om nej, spelaren förlorar
                        _player.UpdateCapital(-investmentAmount);
                        _player.UpdateKarma(scenario.KarmaImpacts[0]);
                        CheckGameStatus(_player);
                        Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information
                        Console.WriteLine($"Tyvärr, du förlorade de {investmentAmount} SEK du satsade");
                        Console.WriteLine($"Ditt totala kapital är nu: {_player.Capital:F2} SEK");
                    }

                    Console.WriteLine("Tryck på [Enter] för att återgå till menyn..");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Du har inte tillräckligt med kapital för att spara det angivna beloppet.");
                    return;
                }
            }

            else
            {
                Console.WriteLine("Ogiltig input, vänligen ange ett giltigt belopp.");
                return;
            }
        }

        /***** Metod för att spela på enarmad bandit *****/
        private void PlayBandit(Scenario scenario, int optionIndex)
        {
            Console.Clear();
            Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information

            Console.WriteLine($"\nDu valde: {scenario.OptionName[optionIndex]}.\n");
            Console.WriteLine("Hur mycket vill du satsa?");
            string input = Console.ReadLine();
            double investmentAmount; // Variabel som ska lagra satsad summa


            // Om det går att omvandla spelarens input till ett double-värde, kör kod:
            if (double.TryParse(input, out investmentAmount) || investmentAmount <= 0)
            {
                // Kontrollera att spelaren har tillräckligt med kapital för att spara det angivna beloppet
                if (investmentAmount <= _player.Capital)
                {
                    //Slumpa resultat av enarmad bandit genom att generera slumpmässigt tal mellan 0.0 och 1.0
                    Random rand = new Random();
                    double chance = rand.NextDouble();

                    // Kontrollera om det slumpade talet är lika med eller mindre än 0.01 (1% chans att vinna)
                    if (chance <= 0.01)
                    {  
                        // Om ja, spelare vinner
                        _player.UpdateCapital(investmentAmount * 10);
                        _player.UpdateKarma(scenario.KarmaImpacts[0]);
                        CheckGameStatus(_player);
                        Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information
                        Console.WriteLine($"Grattis, du vann {investmentAmount * 10:F2} SEK!");
                        Console.WriteLine($"Ditt totala kapital är nu: {_player.Capital:F2} SEK");
                    }
                    else
                    {
                        // Om nej, spelare förlorar
                        _player.UpdateCapital(-investmentAmount);
                        _player.UpdateKarma(scenario.KarmaImpacts[0]);
                        CheckGameStatus(_player);
                        Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information
                        Console.WriteLine($"Tyvärr, du förlorade de {investmentAmount} SEK du satsade");
                        Console.WriteLine($"Ditt totala kapital är nu: {_player.Capital:F2} SEK");
                    }

                    Console.WriteLine("Tryck på [Enter] för att återgå till menyn..");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Du har inte tillräckligt med kapital för att spara det angivna beloppet.");
                    return;
                }
            }

            else
            {
                Console.WriteLine("Ogiltig input, vänligen ange ett giltigt belopp.");
                return;
            }
        }
        
       
        /**** Metod för Game Over-meddelande när kapitalet tar slut *****/
        private void GameOverCapital()
        {
            Console.Clear();
            Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information
            Console.WriteLine("---------- GAME OVER ----------");
            Console.WriteLine("\nKapitalet är slut! Utan pengar kan du inte överleva i Kapitalträsk. Utan cash är du lika");
            Console.WriteLine("ointressant som gårdagens börsnotering. Kanske var kapitalism inte riktigt din grej trots allt?");
            Console.WriteLine("\nTryck på valfri tangent för att avsluta...");
            Console.ReadKey();
            Environment.Exit(0); // Avslutar programmet
        }

        /***** Metod för Game OVer-meddelande när karma når noll *****/
        private void GameOverKarma()
        {
            Console.Clear();
            Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information
            Console.WriteLine("---------- GAME OVER ----------");
            Console.WriteLine("\nNu är karmakontot nere på noll! Alla broar är brända, och Kapitalträsk har inte längre plats för dig.");
            Console.WriteLine("I jakten på rikedom glömde du kanske att till och med Kapitalträsk kräver lite mänsklighet. Dags att");
            Console.WriteLine("omvärdera... kanske blir det ett enklare liv i nästa spelomgång?");
            Console.WriteLine("\nTryck på valfri tangent för att avsluta...");
            Console.ReadKey();
            Environment.Exit(0); // Avslutar programmet
        }

        /***** Metod för vinst-meddelande *****/
        private void PlayerWon()
        {
            Console.Clear();
            Player.ShowPlayerInfo(_player); // Visa den aktuella spelarens information
            Console.WriteLine("---------- GRATTIS! DU HAR VUNNIT ----------");
            Console.WriteLine("\nSå, du står här som miljonär, omgiven av blänkande sedlar och förhoppningar.");
            Console.WriteLine("Visst, vägen hit har kanske inneburit att du har gått över ett par lik, men vem räknar?");
            Console.WriteLine("Nu har du råd att investera i både lyx och illusioner. Lycka till med att köpa tillbaka din själ!\n");
            Console.WriteLine("\nTryck på valfri tangent för att avsluta.");
            Console.ReadKey();
            Environment.Exit(0); // Avslutar programmet
        }

        /***** Metod för att kontrollera spelets status baserat på kapital, karma och social status och visa Game Over eller vinst-meddelande ****/
        private void CheckGameStatus(Player player)
        {
             if (player.Capital >= 1000000)
            {
                PlayerWon();
            }
            else if (player.Capital <= 0)
            {
                GameOverCapital();
            }
            else if (player.Karma <= 0)
            {
                GameOverKarma();
            }

        }

        /****** Metod för att avsluta spelet *****/
        public void ExitGame()
        {
            _stopwatch.Stop(); // Stoppa tidtagaren när spelet avslutas
            TimeSpan totalTime = _stopwatch.Elapsed; // Hämta den totala tiden
            Console.Clear();
            Console.WriteLine("Tack för att du spelade Kapitalträsk!");
            Console.WriteLine($"Total speltid: {totalTime.Minutes} minuter och {totalTime.Seconds} sekunder.");
            Environment.Exit(0);
        }
    } 
} 
