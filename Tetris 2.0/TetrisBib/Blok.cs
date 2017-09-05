using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Tetris
{
    public abstract class Blok : List<Positie>
    {
        private static YComparer _ycomp = new YComparer();
        //geen semafoor nodig?
        private int _sem = 0;

        public SolidColorBrush Kleur { get; set; }
        public Positie MiddelpuntRotatiePositie { get; set; } //midd
        public virtual void NeemIn()
        {
            for (int i = 0; i < 4; i++)
            {
                this[i].Kleur = Kleur;
            }
        }
        public virtual int[] GetDisplayIndexen() { return null; }

        //Movement:

        //posities in blok sorteren op X of Y
        //hoogste/laagste posities X of Y checken
        //als hoogste/laagste positie X of Y +/- 1 niet ingenomen/out of bounds, positie swap
        //sorteren ook nodig voor juist te swappen (niet overschrijven)
        public bool KanZakken(Veld veld)
        {
            while (_sem == 1)
            {
                //do nothing
            }
            _sem = 1;
            List<int> xArray = new List<int>();
            bool moveOk = true;
            this.Sort(_ycomp);
            this.Reverse();
            foreach (Positie pos in this)
            {
                if (pos.Y == 19)
                {
                    moveOk = false;
                    break;
                }
                else
                {
                    if (xArray.TrueForAll(x => pos.X != x))
                    {
                        if (veld[pos.X, pos.Y + 1].Kleur != null)
                        {
                            moveOk = false;
                            break;
                        }
                        xArray.Add(pos.X);
                    }
                }
            }
            if (moveOk == false) OnKanNietZakken(); //raise KanNietZakken Event
            _sem = 0;
            return moveOk;
        }
        public virtual void MoveDown(Veld veld)
        {
            if (KanZakken(veld))
            {
                MiddelpuntRotatiePositie.Y += 1;
                while (_sem == 1)
                {
                    //do nothing
                }
                _sem = 1;
                for (int i = 0; i < 4; i++)
                {
                    this[i].Kleur = null;
                    Positie nieuwePos = veld[this[i].X, this[i].Y + 1];
                    this[i] = nieuwePos;
                    this[i].Kleur = this.Kleur;//
                }
                _sem = 0;
            }
        }
        public virtual void MoveHorizontal(Veld veld, int richting)
        {
            while (_sem == 1)
            {
                //do nothing
            }
            _sem = 1;
            this.Sort();
            if (richting == 1) this.Reverse();
            List<int> yArray = new List<int>();
            bool moveOk = true;
            foreach (Positie pos in this)
            {
                if ((pos.X == 0 && richting == -1) || (pos.X == 9 && richting == 1))
                {
                    moveOk = false;
                    break;
                }
                else
                {
                    if (yArray.TrueForAll(y => pos.Y != y))
                    {
                        if (veld[pos.X + richting, pos.Y].Kleur != null)
                        {
                            moveOk = false;
                            break;
                        }
                        yArray.Add(pos.Y);
                    }
                }
            }
            if (moveOk)
            {
                MiddelpuntRotatiePositie.X += richting;
                for (int i = 0; i < 4; i++)
                {
                    this[i].Kleur = null;
                    Positie nieuwePos = veld[this[i].X + richting, this[i].Y];
                    this[i] = nieuwePos;
                    this[i].Kleur = this.Kleur;
                }
            }
            _sem = 0;
        }
        public virtual void Kantel(Veld veld)
        {
            while (_sem == 1)
            {
                //do nothing
            }
            _sem = 1;
            //lijst opstellen met nieuwe kantelposities
            List<Positie> gekanteldBlok = new List<Positie>();
            //ingenomen false, wnt sommige blokposities worden nieuwe posities van andere
            for (int i = 0; i < 4; i++)
            {
                this[i].Kleur = null;
            }
            //nieuwe blokposities toevoegen als niet ingenomen, of pos == centerpunt blok
            for (int i = 0; i < 4; i++)
            {
                //nieuwe blokposities toevoegen als pos == centerpunt blok
                if (this[i].X == MiddelpuntRotatiePositie.X && this[i].Y == MiddelpuntRotatiePositie.Y)
                {
                    gekanteldBlok.Add(this[i]);
                }
                else
                {
                    //90 graden rotatie formule
                    int x = this[i].X - this.MiddelpuntRotatiePositie.X;
                    int y = this[i].Y - this.MiddelpuntRotatiePositie.Y;
                    int newX = -y;
                    int newY = x;
                    newX = newX + this.MiddelpuntRotatiePositie.X;
                    newY = newY + this.MiddelpuntRotatiePositie.Y;

                    //nieuwe blokposities toevoegen als binnen index & newpos niet ingenomen 
                    if (newX < 10 && newX >= 0 && newY < 19 && newY >= 0 && (veld[newX, newY].Kleur == null))
                    {
                        gekanteldBlok.Add(veld[newX, newY]);
                    }
                    else break;
                }
            }
            if (gekanteldBlok.Count == 4) //alle nieuwe posities zijn niet reeds ingenomen
            {
                for (int i = 0; i < 4; i++)
                {
                    this[i] = gekanteldBlok[i];
                    this[i].Kleur = this.Kleur;
                }
            }
            else //rotatie niet mogelijk, oude posities terug op Ingenomen
                for (int i = 0; i < 4; i++)
                {
                    this[i].Kleur = this.Kleur;
                }
            _sem = 0;
        }

        //event stuff
        public delegate void KanNietZakkenEventHandler(object source, EventArgs args);
        public event KanNietZakkenEventHandler KanNietZakken;
        protected virtual void OnKanNietZakken()
        {
            if (KanNietZakken != null) KanNietZakken(this, EventArgs.Empty);
        }
    }
    //Blok Startposities && centerpostie (voor kantel)
    public class Vierkant : Blok
    {
        public Vierkant(Veld veld)
        {
            Kleur = Brushes.Gray;
            MiddelpuntRotatiePositie = new Tetris.Positie(4, 2);
            Add(veld[4, 2]);
            Add(veld[5, 2]);
            Add(veld[4, 3]);
            Add(veld[5, 3]);

        }
        private int[] _displayIndexen = new int[4] { 1, 2, 5, 6 };
        public override int[] GetDisplayIndexen()
        {
            return _displayIndexen;
        }
    }
    public class Lange : Blok
    {
        public Lange(Veld veld)
        {
            Kleur = Brushes.OrangeRed;
            MiddelpuntRotatiePositie = new Tetris.Positie(4, 2);
            Add(veld[3, 2]);
            Add(veld[4, 2]);
            Add(veld[5, 2]);
            Add(veld[6, 2]);

        }
        private int[] _displayIndexen = new int[4] { 4, 5, 6, 7 };
        public override int[] GetDisplayIndexen()
        {
            return _displayIndexen;
        }
    }
    public class Vork : Blok
    {
        public Vork(Veld veld)
        {
            Kleur = Brushes.Yellow;
            MiddelpuntRotatiePositie = new Tetris.Positie(5, 3);
            Add(veld[5, 2]);
            Add(veld[5, 3]);
            Add(veld[5, 4]);
            Add(veld[6, 3]);

        }
        private int[] _displayIndexen = new int[4] { 1, 5, 6, 9 };
        public override int[] GetDisplayIndexen()
        {
            return _displayIndexen;
        }
    }
    public class Z : Blok
    {
        public Z(Veld veld)
        {
            Kleur = Brushes.LimeGreen;
            MiddelpuntRotatiePositie = new Tetris.Positie(6, 3);
            Add(veld[6, 2]);
            Add(veld[6, 3]);
            Add(veld[5, 3]);
            Add(veld[5, 4]);

        }
        private int[] _displayIndexen = new int[4] { 2, 6, 5, 9 };
        public override int[] GetDisplayIndexen()
        {
            return _displayIndexen;
        }
    }
    public class GespiegeldeZ : Blok
    {
        public GespiegeldeZ(Veld veld)
        {
            Kleur = Brushes.LightBlue;
            MiddelpuntRotatiePositie = new Tetris.Positie(4, 3);
            Add(veld[4, 2]);
            Add(veld[4, 3]);
            Add(veld[5, 3]);
            Add(veld[5, 4]);

        }
        private int[] _displayIndexen = new int[4] { 1, 5, 6, 10 };
        public override int[] GetDisplayIndexen()
        {
            return _displayIndexen;
        }
    }
    public class El : Blok
    {
        public El(Veld veld)
        {
            Kleur = Brushes.DarkBlue;
            MiddelpuntRotatiePositie = new Tetris.Positie(5, 3);
            Add(veld[5, 2]);
            Add(veld[4, 2]);
            Add(veld[5, 3]);
            Add(veld[5, 4]);

        }
        private int[] _displayIndexen = new int[4] { 1, 2, 6, 10 };
        public override int[] GetDisplayIndexen()
        {
            return _displayIndexen;
        }
    }
    public class GespiegeldeEl : Blok
    {
        public GespiegeldeEl(Veld veld)
        {
            Kleur = Brushes.Purple;
            MiddelpuntRotatiePositie = new Tetris.Positie(5, 3);
            Add(veld[5, 2]);
            Add(veld[6, 2]);
            Add(veld[5, 3]);
            Add(veld[5, 4]);

        }
        private int[] _displayIndexen = new int[4] { 1, 2, 5, 9 };
        public override int[] GetDisplayIndexen()
        {
            return _displayIndexen;
        }
    }
}
