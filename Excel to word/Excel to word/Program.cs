using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;

namespace Excel_to_word
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Klacht> klachten = GetKlachtenVanExcel();
            Console.WriteLine(Print(klachten));

            MaakWordDocs(klachten);

            Console.ReadLine();
        }

        private static void MaakWordDocs(List<Klacht> klachten)
        {
            Word.Application wordApp = new Word.Application();
            Word.Document doc = null;
            object fileName = @"C:\Users\mathi\Documents\testomgeving\template.docx";
            object missing = Type.Missing;
            int i = 1;
            foreach (Klacht klacht in klachten)
            {

                doc = wordApp.Documents.Open(fileName, missing, missing);
                wordApp.Selection.Find.ClearFormatting();
                wordApp.Selection.Find.Replacement.ClearFormatting();

                wordApp.Selection.Find.Execute("<Adres>", missing, missing, missing, missing, missing, missing, missing, missing, klacht.Adres, 2);
                wordApp.Selection.Find.Execute("<Datum>", missing, missing, missing, missing, missing, missing, missing, missing, klacht.Datum, 2);
                wordApp.Selection.Find.Execute("<Soort>", missing, missing, missing, missing, missing, missing, missing, missing, klacht.Soort, 2);
                wordApp.Selection.Find.Execute("<Ronde>", missing, missing, missing, missing, missing, missing, missing, missing, klacht.Ronde, 2);
                wordApp.Selection.Find.Execute("<Krant>", missing, missing, missing, missing, missing, missing, missing, missing, klacht.Krant, 2);

                object nieuwFile = (object)"C:\\Users\\mathi\\Documents\\testomgeving\\newfile" + i.ToString() + ".docx";
                doc.SaveAs2(nieuwFile, missing, missing, missing);
                doc.Close(false, missing, missing);
                i++;
            }
            wordApp.Quit(false, false, false);
            Marshal.ReleaseComObject(wordApp);
        }

        private static List<Klacht> GetKlachtenVanExcel()
        {
            List<Klacht> klachten = new List<Klacht>();

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\mathi\Documents\testomgeving\export.xls");
            Excel.Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int[] xWaarden = new int[5] { 2, 4, 5, 9, 10 };
            bool flag = false;
            for (int y = 2; y <= 50; y++)
            {
                Klacht klacht = new Klacht();
                foreach (int x in xWaarden)
                {
                    if (xlRange.Cells[y, x].Value2 != null && xlRange.Cells[y, x] != null)
                    {
                        switch (x)
                        {
                            case 2:
                                klacht.Datum = xlRange.Cells[y, x].Value2.ToString();
                                break;
                            case 4:

                                klacht.Ronde = xlRange.Cells[y, x].Value2.ToString();

                                break;
                            case 5:

                                klacht.Krant = xlRange.Cells[y, x].Value2.ToString();

                                break;
                            case 9:
                                klacht.Adres = xlRange.Cells[y, x].Value2.ToString();
                                break;
                            case 10:

                                klacht.Soort = xlRange.Cells[y, x].Value2.ToString();
                                break;
                        }
                    }
                    else
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag) klachten.Add(klacht);
                else break;
            }
            //cleanup
            xlWorkbook.Close(true, null, null);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlApp);

            foreach (Klacht klacht in klachten)
            {
                if (klacht.Adres.Contains("\n"))
                {
                    string nieuwAdres = klacht.Adres;

                    int startIndex = nieuwAdres.IndexOf("\n");
                    //int stopIndex = nieuwAdres.IndexOf("B", startIndex);
                    //int count = stopIndex - startIndex;
                    nieuwAdres = nieuwAdres.Remove(startIndex, 31);

                    klacht.Adres = nieuwAdres;
                }
            }
            return klachten;
        }
        private static string Print(List<Klacht> klachten)
        {
            string output = "";
            foreach (Klacht klacht in klachten)
            {
                output += klacht.Adres + " | " + klacht.Datum + " | " + klacht.Krant + " | " + klacht.Ronde + " | " + klacht.Soort + "\n";
            }
            return output;
        }
    }
    class Klacht
    {
        public string Adres { get; set; }
        public string Krant { get; set; }
        public string Ronde { get; set; }
        public string Soort { get; set; }
        public string Datum { get; set; }
    }

}
