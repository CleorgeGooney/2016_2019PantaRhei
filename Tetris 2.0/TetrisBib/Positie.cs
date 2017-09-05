using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Tetris
{
    public class Positie : IComparable<Positie>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public SolidColorBrush Kleur { get; set; }
        public Positie(int x, int y)
        {
            X = x;
            Y = y;           
        }
        //op x sorteren om movehorizontal te laten werken
        public int CompareTo(Positie other)
        {
            return this.X.CompareTo(other.X);
        }
    }
    //Op y sorteren om MoveDown check te laten werken
    public class YComparer : Comparer<Positie>
    {
        public override int Compare(Positie pos1, Positie pos2)
        {
            return pos1.Y.CompareTo(pos2.Y);
        }
    }
}
