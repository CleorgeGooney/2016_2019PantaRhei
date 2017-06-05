using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        static private int[] blok0 = new int[] { 0, 1, 2, 9, 10, 11, 18, 19, 20 };
        static private int[] blok1 = new int[] { 3, 4, 5, 12, 13, 14, 21, 22, 23 };
        static private int[] blok2 = new int[] { 6, 7, 8, 15, 16, 17, 24, 25, 26 };
        static private int[] blok3 = new int[] { 27, 28, 29, 36, 37, 38, 45, 46, 47 };
        static private int[] blok4 = new int[] { 30, 31, 32, 39, 40, 41, 48, 49, 50 };
        static private int[] blok5 = new int[] { 33, 34, 35, 42, 43, 44, 51, 52, 53 };
        static private int[] blok6 = new int[] { 54, 55, 56, 63, 64, 65, 72, 73, 74 };
        static private int[] blok7 = new int[] { 57, 58, 59, 66, 67, 68, 75, 76, 77 };
        static private int[] blok8 = new int[] { 60, 61, 62, 69, 70, 71, 78, 79, 80 };
        static private List<int[]> _blokken = new List<int[]>() { blok0, blok1, blok2, blok3, blok4, blok5, blok6, blok7, blok8 };
        static void Main(string[] args)
        {
            int[] sudoku = new int[] {9,0,6,8,1,3,5,4,0,
                                      2,0,1,0,4,5,0,6,3,
                                      0,4,0,0,0,0,0,0,0,
                                      0,0,0,6,2,0,0,0,9,
                                      0,0,9,0,0,0,2,0,0,
                                      7,0,0,0,3,4,0,0,0,
                                      0,0,0,0,0,0,0,9,0,
                                      5,9,0,3,6,0,1,0,4,
                                      0,2,7,4,5,9,3,0,6  };
            int[] sudoku2 = new int[]{0,1,0,0,0,0,7,4,0,
                       8,0,0,2,0,0,0,0,3,
                       0,3,7,4,9,0,0,0,0,
                       6,0,5,0,2,0,0,0,0,
                       0,0,0,8,1,4,0,0,5,
                       0,0,0,5,0,0,4,9,7,
                       3,0,0,0,0,9,2,0,0,
                       0,5,1,0,0,0,0,0,9,
                       0,7,0,0,4,6,0,1,0};

            int[] sudoku3 = new int[] { 0,9,3,1,0,5,6,4,0,
                     7,0,0,0,0,0,0,0,5,
                     5,0,1,2,0,9,3,0,7,
                     2,0,0,0,0,0,0,0,3,
                     0,3,6,9,0,7,5,2,0,
                     9,0,0,0,0,0,0,0,1,
                     3,0,2,4,0,8,1,0,9,
                     6,0,0,0,0,0,0,0,4,
                     0,4,7,3,0,2,8,5,0};

            int[] sudoku4 = new int[] {8,0,0,0,0,0,0,0,0,
                      0,0,3,6,0,0,0,0,0,
                      0,7,0,0,9,0,2,0,0,
                      0,5,0,0,0,7,0,0,0,
                      0,0,0,0,4,5,7,0,0,
                      0,0,0,1,0,0,0,3,0,
                      0,0,1,0,0,0,0,6,8,
                      0,0,8,5,0,0,0,1,0,
                      0,9,0,0,0,0,4,0,0 };
           

            Console.WriteLine(Print(sudoku2));
                
            for (int index = 0; index < sudoku2.Length; index++)
            {
                if (sudoku2[index] == 0)
                {
                    VeranderWaarde(index, sudoku2);
                }
            }

            Console.WriteLine(Print(sudoku2));

            Console.ReadLine();

            Console.WriteLine(Print(sudoku3));

            for (int index = 0; index < sudoku3.Length; index++)
            {
                if (sudoku3[index] == 0)
                {
                    VeranderWaarde(index, sudoku3);
                }
            }
            Console.WriteLine(Print(sudoku3));
                               
            Console.ReadLine();

            Console.WriteLine(Print(sudoku4));

            for (int index = 0; index < sudoku4.Length; index++)
            {
                if (sudoku4[index] == 0)
                {
                    VeranderWaarde(index, sudoku4);
                }
            }
            Console.WriteLine(Print(sudoku4));

            Console.ReadLine();
        }
        static void VeranderWaarde(int index, int[] sudoku)
        {
            if (sudoku[index] == 0 && !(index == sudoku.Length))
            {
                int teller = 0;
                while (teller < 9)
                {
                    sudoku[index]++;
                    if (!ZitInRij(index, sudoku) && !ZitInKolom(index, sudoku) && !ZitInBlok(index, sudoku))
                    {
                            int teCallenIndex = 0;
                            for (int startIndex = index + 1; startIndex < sudoku.Length; startIndex++)
                            {
                                if (sudoku[startIndex] == 0)
                                {
                                    teCallenIndex = startIndex;
                                    break;
                                }
                            }
                           
                            VeranderWaarde(teCallenIndex, sudoku);
                            int controle2 = 0;
                            foreach (int getal in sudoku)
                            {
                                if (getal != 0)
                                {
                                    controle2++;
                                }
                            }
                            if (controle2 == sudoku.Length)
                            {
                                break;
                            }
                        
                    }
                    if (sudoku[index] == 9 && (ZitInRij(index, sudoku) || ZitInKolom(index, sudoku) || ZitInBlok(index, sudoku)))
                    {
                        sudoku[index] = 0;
                    }
                    teller++;
                }
            }
        }
        static bool ZitInRij(int index, int[] sudoku)
        {
            int kolom = index % 9;
            int startWaarde = index - kolom;
            for (int sW = startWaarde; sW < startWaarde + 9; sW++)
            {
                if (sW == index)
                {

                }
                else if (sudoku[sW] == sudoku[index])
                {
                    return true;
                }
            }
            return false;
        }
        static bool ZitInKolom(int index, int[] sudoku)
        {
            int startIndex = index;
            while (startIndex > 8)
            {
                startIndex = startIndex - 9;
            }
            for (int sW = startIndex; sW < startIndex + (8 * 9); sW = sW + 9)
            {
                if (sW == index)
                {

                }
                else if (sudoku[sW] == sudoku[index])
                {
                    return true;
                }
            }
            return false;
        }
        static bool ZitInBlok(int index, int[] sudoku)
        {
            foreach (int[] blok in _blokken)
            {
                bool gevonden = false;
                foreach (int teVindenIndex in blok)
                {
                    if (teVindenIndex == index)
                    {
                        gevonden = true;
                    }
                }
                if (gevonden)
                {
                    foreach (int teVindenIndex in blok)
                    {
                        if (teVindenIndex == index)
                        { }
                        else if (sudoku[index] == sudoku[teVindenIndex]) return true;
                    }
                }
            }
            return false;
        }
        static string Print(int[] sudoku)
        {
            string output = "";
            int teller = 0;
            foreach (int getal in sudoku)
            {
                if (teller % 9 == 0)
                {
                    output += "\n" + getal;
                }
                else output += getal;
                teller++;
            }
            return output;
        }
    }
}
