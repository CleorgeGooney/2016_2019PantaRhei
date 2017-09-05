using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region velden
        private static Veld _veld = new Veld();
        private static Blok _huidigBlok;
        private static Queue<Blok> _wachtrij = new Queue<Blok>();
        private static System.Timers.Timer _timer = new System.Timers.Timer();
        private static Random _rnd = new Random();
        private static bool _pauze = true;
        private static int _aantalLijnen = 0;
        private static double _score = 0;
        private static string[] _ranks = new string[10] { "Novice", "Farmer", "Industrieel", "Krijger", "Lobbyist", "Koning", "Kabinetchef", "Wijze", "Demigod", "Tetrisgod" };
        private static string _rank = _ranks[0];

        private static Grid _bord = new Grid();
        private static Border _rand = new Border();
        private static Label[] _posLabels = new Label[200];
        private static TextBlock _tekstBlok = new TextBlock();
        private static TextBlock _info = new TextBlock();
        private static ComboBox _kiesRankComboBox = new ComboBox();
        private static Button _nieuwSpelBtn = new Button();
        private static Button _kiesRankBtn = new Button();
        private static Grid _volgendBlok = new Grid();
        private static Label[] _volgendBlokLabels = new Label[12];
        #endregion
        public MainWindow()
        {
            InitializeComponent();

            _timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerTikEventHandler); //EventHandler inschrijven bij Elapsed

            #region wpfstuff
            //grid
            _bord.Background = Brushes.Black;
            _bord.Height = 840;
            _bord.Width = 820;
            _bord.Margin = new Thickness(0, 0, 0, 0);
            int i = 0; //elk label is één positie
            for (int y = 0; y < 800; y += 40)
            {
                for (int x = 0; x < 400; x += 40)
                {
                    _posLabels[i] = new Label();
                    _posLabels[i].Name = "pos" + i;
                    _posLabels[i].Width = 40;
                    _posLabels[i].Height = 40;
                    _posLabels[i].Margin = new Thickness(x, y, 0, 0);
                    _posLabels[i].Background = Brushes.Black;
                    _posLabels[i].VerticalAlignment = VerticalAlignment.Top;
                    _posLabels[i].HorizontalAlignment = HorizontalAlignment.Left;
                    _bord.Children.Add(_posLabels[i]);
                    i++;
                }
            }
            this.AddChild(_bord);

            //volgendBlokGrid
            _volgendBlok.Background = Brushes.Red;
            _volgendBlok.Margin = new Thickness(400, 300, 0, 0);
            _volgendBlok.Height = 120;
            _volgendBlok.Width = 160;
            _volgendBlok.VerticalAlignment = VerticalAlignment.Top;
            int labeli = 0;
            for (int y = 0; y < 120; y += 40)
            {
                for (int x = 0; x < 160; x += 40)
                {
                    _volgendBlokLabels[labeli] = new Label();
                    _volgendBlokLabels[labeli].Margin = new Thickness(x, y, 0, 0);
                    _volgendBlokLabels[labeli].Width = 40;
                    _volgendBlokLabels[labeli].Name = "pos" + labeli;
                    _volgendBlokLabels[labeli].Height = 40;
                    _volgendBlokLabels[labeli].Background = Brushes.Black;
                    _volgendBlokLabels[labeli].VerticalAlignment = VerticalAlignment.Top;
                    _volgendBlokLabels[labeli].HorizontalAlignment = HorizontalAlignment.Left;
                    _volgendBlok.Children.Add(_volgendBlokLabels[labeli]);
                    labeli++;
                }
            }
            _bord.Children.Add(_volgendBlok);


            //tekstblok met scores
            _tekstBlok.Name = "info";
            _tekstBlok.HorizontalAlignment = HorizontalAlignment.Left;
            _tekstBlok.VerticalAlignment = VerticalAlignment.Top;
            _tekstBlok.Height = 200;
            _tekstBlok.Margin = new Thickness(400, 50, 0, 0);
            _tekstBlok.Background = Brushes.Black;
            _tekstBlok.TextWrapping = TextWrapping.Wrap;
            _tekstBlok.Width = 420;
            _tekstBlok.Foreground = Brushes.LimeGreen;
            _tekstBlok.TextAlignment = TextAlignment.Center;
            _tekstBlok.FontSize = 24;
            _tekstBlok.FontFamily = new FontFamily("Consolas");
            _bord.Children.Add(_tekstBlok);

            //tekstblok met "p"
            _info.Name = "info";
            _info.HorizontalAlignment = HorizontalAlignment.Center;
            _info.Height = 30;
            _info.Width = 200;
            _info.Margin = new Thickness(400, 770, 0, 0);
            _info.Background = Brushes.Black;
            _info.TextWrapping = TextWrapping.Wrap;
            _info.Text = "Druk op 'P' om te pauzeren.";
            _info.VerticalAlignment = VerticalAlignment.Top;
            _info.Foreground = Brushes.LimeGreen;
            _info.TextAlignment = TextAlignment.Center;
            _info.FontSize = 12;
            _info.FontFamily = new FontFamily("Consolas");
            _bord.Children.Add(_info);

            //buttons
            _nieuwSpelBtn.Click += nieuwSpelBtn_Klik;
            _nieuwSpelBtn.Height = 33;
            _nieuwSpelBtn.Width = 100;
            _nieuwSpelBtn.VerticalAlignment = VerticalAlignment.Top;
            _nieuwSpelBtn.Margin = new Thickness(400, 570, 0, 0);
            _nieuwSpelBtn.Content = "Nieuw spel";
            _nieuwSpelBtn.Background = Brushes.Black;
            _nieuwSpelBtn.FontFamily = new FontFamily("Consolas");
            _nieuwSpelBtn.Foreground = Brushes.LimeGreen;

            _kiesRankBtn.Click += kiesRankBtn_Klik;
            _kiesRankBtn.Content = "Kies rank";
            _kiesRankBtn.Height = 33;
            _kiesRankBtn.Width = 100;
            _kiesRankBtn.VerticalAlignment = VerticalAlignment.Top;
            _kiesRankBtn.Margin = new Thickness(400, 640, 0, 0);
            _kiesRankBtn.Background = Brushes.Black;
            _kiesRankBtn.FontFamily = new FontFamily("Consolas");
            _kiesRankBtn.Foreground = Brushes.LimeGreen;

            _bord.Children.Add(_nieuwSpelBtn);
            _bord.Children.Add(_kiesRankBtn);

            //combobox
            _kiesRankComboBox.VerticalAlignment = VerticalAlignment.Top;
            _kiesRankComboBox.Height = 33;
            _kiesRankComboBox.Width = 100;
            _kiesRankComboBox.Margin = new Thickness(400, 640, 0, 0);
            _kiesRankComboBox.Background = Brushes.Black;
            _kiesRankComboBox.FontFamily = new FontFamily("Consolas");
            foreach (string rank in _ranks) _kiesRankComboBox.Items.Add(rank);

            //rand
            _bord.Children.Add(_rand);
            _rand.HorizontalAlignment = HorizontalAlignment.Left;
            _rand.VerticalAlignment = VerticalAlignment.Top;
            _rand.Margin = new Thickness(0, 0, 0, 0);
            _rand.BorderThickness = new Thickness(0, 0, 1, 0);
            _rand.Height = 840;
            _rand.Width = 400;
            _rand.BorderBrush = Brushes.White;
            #endregion
        }
        //Nieuwspelbutton
        private void nieuwSpelBtn_Klik(object sender, RoutedEventArgs e)
        {
            //Stop
            if (_huidigBlok != null)
            {
                if (!_pauze) Pauze();
                var boodschap = "Wil je zeker stoppen?";
                var title = "Stop";
                var venster = MessageBox.Show(boodschap, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (venster)
                {
                    case MessageBoxResult.Yes:
                        _wachtrij.Clear();
                        _veld.LijnenGemaakt -= OnLijnenGemaakt;
                        _huidigBlok = null;
                        foreach (Positie pos in _veld) pos.Kleur = null;
                        this.Dispatcher.Invoke(() =>
                        {
                            _tekstBlok.Text = "";
                            _nieuwSpelBtn.Content = "Nieuw spel";
                            foreach (Label lab in _volgendBlokLabels) lab.Background = Brushes.Black;
                            _bord.Children.Add(_kiesRankBtn);
                            _kiesRankComboBox.SelectedIndex = -1;
                        });
                        ChangeColor();
                        break;
                }
            }
            //Nieuw spel
            else
            {
                //rank bepalen (en timer)
                int rankIndex = _kiesRankComboBox.SelectedIndex;
                if (rankIndex != -1)
                {
                    _rank = _ranks[rankIndex];
                    _timer.Interval = 550 - (rankIndex * 50);
                }
                else
                {
                    //default
                    _rank = _ranks[0];
                    _timer.Interval = 550;
                }
                //defaults
                foreach (Positie pos in _veld) pos.Kleur = null;
                _score = 0;
                _aantalLijnen = 0;
                _pauze = false;
                _timer.Enabled = true;
                _veld.LijnenGemaakt += OnLijnenGemaakt;
                //UI wijzigen
                this.Dispatcher.Invoke(() =>
                {
                    _bord.Children.Remove(_kiesRankComboBox);
                    _bord.Children.Remove(_kiesRankBtn);
                    _nieuwSpelBtn.Content = "Stop";
                    _tekstBlok.Text = StandaardText();
                });
                ChangeColor();
                //start spel
                _wachtrij.Enqueue(RandomBlok());
                OnKanNietZakken(null, null);
            }
        }
        
        //nieuw blok in random queue(2)
        private void OnKanNietZakken(object sender, EventArgs e)
        {
            _veld.CheckVerwijderVolleLijnen();
            _huidigBlok = _wachtrij.Dequeue();
            if (_huidigBlok.TrueForAll(pos => pos.Kleur == null))
            {
                _huidigBlok.NeemIn();
                Blok display = RandomBlok();
                _wachtrij.Enqueue(display);
                this.Dispatcher.Invoke(() =>
                {
                    for (int i = 0; i < _volgendBlokLabels.Length; i++)
                    {
                        _volgendBlokLabels[i].Background = Brushes.Black;
                    }
                    int[] indexen = display.GetDisplayIndexen();
                    foreach (int i in indexen)
                    {
                        _volgendBlokLabels[i].Background = display.Kleur;
                    }
                });
                _huidigBlok.KanNietZakken += OnKanNietZakken; //OnKanNietZakken inschrijven bij new Blok
                ChangeColor();
            }
            else //game over
            {
                Pauze();
                _huidigBlok = null;
                _wachtrij.Clear();
                _veld.LijnenGemaakt -= OnLijnenGemaakt;
                this.Dispatcher.Invoke(() =>
                {
                    _bord.Children.Add(_kiesRankBtn);
                    _kiesRankComboBox.SelectedIndex = -1;
                    _nieuwSpelBtn.Content = "Nieuw spel";
                    _tekstBlok.Text = StandaardText() + "\nGame Over!";
                });
            }
        }
        
        //switch tussen alle blokken
        private static Blok RandomBlok()
        {
            int random = _rnd.Next(7);
            switch (random)
            {
                case 0:
                    return new Vierkant(_veld);
                case 1:
                    return new Lange(_veld);
                case 2:
                    return new Vork(_veld);
                case 3:
                    return new Z(_veld);
                case 4:
                    return new GespiegeldeZ(_veld);
                case 5:
                    return new El(_veld);
                default:
                    return new GespiegeldeEl(_veld);
            }
        }

        //KeyboardEventHandler, reference in Xaml code KeyDown="OnKeyDownHandler"
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.P: //enkel (on)pauzeren tijdens spel
                    if (_huidigBlok != null) Pauze();
                    break;
                case Key.Left:
                    if (!_pauze)
                    {
                        _huidigBlok.MoveHorizontal(_veld, -1);
                        ChangeColor();
                    }
                    break;
                case Key.Right:
                    if (!_pauze)
                    {
                        _huidigBlok.MoveHorizontal(_veld, 1);
                        ChangeColor();
                    }
                    break;
                case Key.Up:
                    if (!_pauze)
                    {
                        _huidigBlok.Kantel(_veld);
                        ChangeColor();
                    }
                    break;
                case Key.Down:
                    if (!_pauze)
                    {
                        _huidigBlok.MoveDown(_veld);
                        ChangeColor();
                    }
                    break;
            }
        }
        
        //update labels naargelang pos.Kleur waarde
        private void ChangeColor()
        {
            //te zware berekening?
            try
            {

                this.Dispatcher.Invoke(() => //anders thread exception
                {
                    for (int i = 0; i < 200; i++)
                    {
                        SolidColorBrush kleur = _veld[i].Kleur;
                        _posLabels[i].Background = (kleur == null) ? Brushes.Black : _veld[i].Kleur;
                    }
                });
            }
            catch { }
        }
        
        //score en rank updaten
        private void OnLijnenGemaakt(object sender, LijnenGemaaktEventArgs e)
        {
            //1. score updaten
            double multiplier = 1;
            string boodschap = "";

            //multiplier bepalen
            switch (e.AantalLijnen)
            {
                case 1:
                    multiplier = 1;
                    boodschap = "Mooie lijn!";
                    break;
                case 2:
                    multiplier = 1.25;
                    boodschap = "Double multiplier!";
                    break;
                case 3:
                    multiplier = 1.5;
                    boodschap = "Triple multiplier!";
                    break;
                case 4:
                    multiplier = 2;
                    boodschap = "Monster multiplier!";
                    break;
            }
            _score += (e.AantalLijnen * 500) * multiplier;

            //2. bepaal rank
            if (_rank != _ranks[9])
            {
                int voorGemaaktAantal = _aantalLijnen;
                int aantal = _aantalLijnen += e.AantalLijnen;
                for (int teller = voorGemaaktAantal + 1; teller <= aantal; teller++)
                {
                    if (teller % 6 == 0)
                    {
                        for (int i = 0; i < _ranks.Length; i++)
                        {
                            if (_rank == _ranks[i] && (i + 1) < _ranks.Length)
                            {
                                _rank = _ranks[i + 1];
                                _timer.Interval = _timer.Interval - 50;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            //_tekstBlok wijzigen
            this.Dispatcher.Invoke(() =>
            {
                _tekstBlok.Text = StandaardText() + "\n" + boodschap;
            });
        }
        
        //als timer elapsed, blok zakken
        private void TimerTikEventHandler(object sender, EventArgs e)
        {
            if (_huidigBlok != null) //bij init, timer start voor eerste blok
            {
                _huidigBlok.MoveDown(_veld);
                this.ChangeColor();
            }
        }

        //Timer disable
        private void Pauze()
        {
            if (_huidigBlok != null)
            {
                if (!_pauze)
                {
                    _pauze = true;
                    this.Dispatcher.Invoke(() =>
                    {
                        _tekstBlok.Text += "\n\nGepauzeerd";
                    });
                    _timer.Enabled = false;
                }
                else
                {
                    _pauze = false;
                    this.Dispatcher.Invoke(() =>
                    {
                        _tekstBlok.Text = StandaardText();
                    });
                    _timer.Enabled = true;
                }
            }
        }

        private void kiesRankBtn_Klik(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                _bord.Children.Add(_kiesRankComboBox);
                _bord.Children.Remove(_kiesRankBtn);
            });
        }
        private static string StandaardText()
        {
            return "Score: " + _score.ToString() + "\nRank: " + _rank + "\nTimer: " + _timer.Interval.ToString();
        }
    }
}
