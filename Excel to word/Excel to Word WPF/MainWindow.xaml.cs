using System;
using System.Collections.Generic;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using CodePack = Microsoft.WindowsAPICodePack.Dialogs;
namespace Excel_to_Word_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static List<Klacht> _klachten = null;
        private static string _bronPad = null;
        private static string _doelPad = null;
        public MainWindow()
        {
            InitializeComponent();
            makeDocs.IsEnabled = false;
        }

        private void selectBronFile_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "export"; // Default file name
            dlg.DefaultExt = ".xls"; // Default file extension
            dlg.Filter = "Excel Worksheet (.xls)|*.xls"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                _bronPad = dlg.FileName;
                bronFile.Text = dlg.FileName;
            }
        }

        private void selectDoelMap_Click(object sender, RoutedEventArgs e)
        {
            CodePack.CommonOpenFileDialog dialog = new CodePack.CommonOpenFileDialog();
            //dialog.InitialDirectory = "C:\\Users\\mathi\\Documents\\testomgeving"; //!
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CodePack.CommonFileDialogResult.Ok)
            {
                _doelPad = dialog.FileName;
                doelMap.Text = dialog.FileName;
            }
        }

        private void toonDocs_Click(object sender, RoutedEventArgs e)
        {
            if (_doelPad != null && _bronPad != null)
            {
                GetKlachtenVanExcel();
                result.Text = Print(_klachten);
                toonDocs.IsEnabled = false;
                makeDocs.IsEnabled = true;
            }
            else
            {
                result.Text = "selecteer map/file";
            }

        }
        private void makeDocs_Click(object sender, RoutedEventArgs e)
        {
            result.Text = "docs gemaakt!\n" + result.Text;
            MaakWordDocs(_klachten);
            makeDocs.IsEnabled = false;
        }

        private static void MaakWordDocs(List<Klacht> klachten)
        {
            Word.Application wordApp = new Word.Application();
            Word.Document doc = null;

            string huidigeFolderPath = Environment.CurrentDirectory;
            var huidigeFolderDirectoryInfo = new System.IO.DirectoryInfo(huidigeFolderPath);
            object pad = huidigeFolderDirectoryInfo.FullName + "\\template.docx";

            object fileName = pad; //!!!


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

                //string 

                object nieuwFile = (object)@_doelPad + "\\klacht_" + klacht.Ronde + "_n" + i.ToString() + ".docx";
                doc.SaveAs2(nieuwFile, missing, missing, missing);
                doc.Close(false, missing, missing);
                i++;
            }
            wordApp.Quit(false, false, false);
            Marshal.ReleaseComObject(wordApp);
        }

        private static void GetKlachtenVanExcel()
        {
            _klachten = new List<Klacht>();

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@_bronPad);
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
                if (!flag) _klachten.Add(klacht);
                else break;
            }
            //cleanup
            xlWorkbook.Close(true, null, null);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlApp);

            foreach (Klacht klacht in _klachten)
            {
                if (klacht.Adres.Contains("\n"))
                {
                    string nieuwAdres = klacht.Adres;

                    int startIndex = nieuwAdres.IndexOf("\n");
                    nieuwAdres = nieuwAdres.Remove(startIndex, 31);

                    klacht.Adres = nieuwAdres;
                }
            }
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
