using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Tetris
{
    public class Veld : List<Positie>
    {
        //Constructor
        public Veld()
        {
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Positie pos = new Positie(x, y);
                    Add(pos);
                }
            }
        }
        //method om gemaakte lijnen te detecteren
        public void CheckVerwijderVolleLijnen()
        {
            //checken op volle lijnen
            List<int> yAssen = new List<int>();
            for (int y = 19; y >= 0; y--)
            {
                int teller = 0;
                for (int x = 0; x < 10; x++)
                {
                    if (this[x, y].Kleur != null)
                    {
                        teller++;
                    }
                }
                if (teller == 10)
                {
                    yAssen.Add(y);
                    if (yAssen.Count == 4) break;
                }
            }
            if (yAssen.Count > 0)
            {
                //verwijder de lijnen
                yAssen.Reverse();
                for (int iLijnen = 0; iLijnen < yAssen.Count; iLijnen++)
                {
                    int y = yAssen[iLijnen];
                    for (int x = 0; x < 10; x++)
                    {
                        this[x, y].Kleur = null;
                    }
                    int index = y * 10 - 1;
                    for (int i = index; i >= 0; i--)
                    {
                        if (this[i].Kleur != null)
                        {
                            this[i + 10].Kleur = this[i].Kleur;
                            this[i].Kleur = null;
                        }
                    }
                }
                //raise event voor _score Window
                OnLijnenGemaakt(yAssen.Count);
            }
        }
        public Positie this[int x, int y]
        {
            get
            {
                int index = (y * 10 + x);
                return this[index];
            }
        }
        //event stuff
        public delegate void LijnenGemaaktEventHandler(object source, LijnenGemaaktEventArgs args);
        public event LijnenGemaaktEventHandler LijnenGemaakt;
        protected virtual void OnLijnenGemaakt(int aantal)
        {
            if (LijnenGemaakt != null)
            {
                LijnenGemaakt(this, new LijnenGemaaktEventArgs { AantalLijnen = aantal });
            }
        }
    }
    public class LijnenGemaaktEventArgs : EventArgs
    {
        public int AantalLijnen { get; set; }
    }
}
