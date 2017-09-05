using System;
using System.Timers;
using System.Windows.Media;

namespace Tetris
{
    class Program
    {
        private static Blok _huidigBlok;
        private static Timer _timer = new Timer();
        private static Veld _veld = new Veld();
        private static bool _nieuwBlokNodig;
        private static Random _rnd = new Random();
        static void Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.Unicode;


            _timer.Interval = 500;
            _timer.Enabled = true;
            _timer.Elapsed += new ElapsedEventHandler(TimerTikEvent);

            Console.WriteLine(Visual()); //flicker => wpf buttons
            while (true)//...
            {
                int random = _rnd.Next(7);
                switch (random)
                {
                    case 0:
                        _huidigBlok = new Vierkant(_veld);
                        break;
                    case 1:
                        _huidigBlok = new Lange(_veld);
                        break;
                    case 2:
                        _huidigBlok = new Vork(_veld);
                        break;
                    case 3:
                        _huidigBlok = new Z(_veld);
                        break;
                    case 4:
                        _huidigBlok = new GespiegeldeZ(_veld);
                        break;
                    case 5:
                        _huidigBlok = new El(_veld);
                        break;
                    case 6:
                        _huidigBlok = new GespiegeldeEl(_veld);
                        break;
                }
                _nieuwBlokNodig = false;
                Console.Clear();
                Console.WriteLine(Visual());
                while (!_nieuwBlokNodig)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo cki = Console.ReadKey();
                        switch (cki.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                _huidigBlok.MoveHorizontal(_veld, -1);
                                Console.Clear();
                                Console.WriteLine(Visual());
                                break;
                            case ConsoleKey.RightArrow:
                                _huidigBlok.MoveHorizontal(_veld, 1);
                                Console.Clear();
                                Console.WriteLine(Visual());
                                break;
                            case ConsoleKey.UpArrow:
                                _huidigBlok.Kantel(_veld);
                                Console.Clear();
                                Console.WriteLine(Visual());
                                break;
                            case ConsoleKey.DownArrow:
                                _huidigBlok.MoveDown(_veld);
                                Console.Clear();
                                Console.WriteLine(Visual());
                                if (!_huidigBlok.KanZakken(_veld)) _nieuwBlokNodig = true;//efficienter, eerst op kanzakken check
                                break;
                        }
                    }
                }
                _veld.CheckVerwijderVolleLijnen();
                Console.Clear();
                Console.WriteLine(Visual());
            }
        }
        private static void TimerTikEvent(object sender, EventArgs e)
        {
            if (_huidigBlok != null)
            {
                _huidigBlok.MoveDown(_veld);//efficienter, eerst op kanzakken check
                if (!_huidigBlok.KanZakken(_veld)) _nieuwBlokNodig = true;
                Console.Clear();
                Console.WriteLine(Visual());
            }
        }
        private static string Visual()
        {
            string bord = "";
            for (int y = 0; y < 20; y++)
            {
                bord = bord + " |";
                for (int x = 0; x < 10; x++)
                {
                    if (_veld[x, y].Kleur != null)
                    {
                        bord = bord + "\u25a1";
                    }
                    else bord = bord + " ";
                }
                bord = bord + "|\n";
            }
            return bord;
        }
    }
}
