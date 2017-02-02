using System;
using System.Collections.Generic;
using System.Linq;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Threading;

namespace Word_Lab_5_KPP
{
    public class TableWord
    {
        private static void Main(string[] args)
        {
            //создаём документ
            Word.Application word = new Word.Application();
            object miss = Missing.Value;
            object path = @"C:\Users\Андрей\Documents\Visual Studio 2015\Projects\Studing.git\Word_Lab_4_KPP\Doc.docx";
            object readOnly = false;
            object isVisible = false;
            word.Visible = false;
           // Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss,
            //    ref miss, ref miss, ref miss, ref miss, ref miss, ref isVisible, ref miss, ref miss, ref miss, ref miss);
            Console.WriteLine("Запуск построения таблиц в файле");

            #region 1-й вариант путем непосредственного использования коллекции Tables объекта Document.

            string[,] text = new string[,] { { "Наименование показателя", "1 квартал, млн. грн.", "2 квартал, млн. грн.", "3 квартал, млн. грн." , "4 квартал, млн. грн."  },
            { "Остатки на расчетных и текущих счетах ", "5000", "8000", "1300", "6000" },
            { "Депозиты предприятий и кооперативов ", "1000", "3000", "1000", "5000" },
            { "Межбанковские кредиты ", "1200", "1200", "7000", "2600" },
            { "Вклады граждан ", "2000", "7000", "700", "3000" }};

            object start = 0;
            object end = 0;
            word.Visible = true;
            object oEndOfDoc = "\\endofdoc";
            Word.Document docs = word.Documents.Add(ref miss, ref miss, ref miss, ref miss);

            var selection = word.Selection;
            //Range означает захват промежутка данных между объектами документа (текста, параграфа, таблицы и т.д.)
            Word.Range range = selection.Range;
            var paragraph = docs.Paragraphs.Add(ref miss);
            paragraph.Range.Text = "1-й вариант";
            paragraph.Range.Font.Bold = 1;
            paragraph.Range.InsertParagraphAfter();

            Word.Table table;
            //Word.Range wrdRng = docs.Bookmarks.get_Item(ref path).Range;
            Word.Range wrdRng = docs.Bookmarks.get_Item(ref oEndOfDoc).Range;
            table = docs.Tables.Add(wrdRng, 5, 5, ref miss, ref miss);
            table.Range.Font.Name = "Verdana";
            table.Range.Font.Size = 12F;
            table.Range.Font.Bold = 0;
            table.AllowAutoFit = true;
            for (int i = 1; i <= 5; i++)
                for (int j = 1; j <= 5; j++)
                {
                   //добавляем наш текст в ячейки
                    table.Cell(i, j).Range.Text = text[i - 1, j - 1];
                    //первая строка "шапка" будет жирным
                    if (i == 1)
                        table.Rows[i].Range.Font.Bold = 1;
                    //размещаем текст в первом столбце под "шапкой" по левому краю
                    if(j==1 && i>1)
                        table.Cell(i, j).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    //размещаем текст в ячейках по центру
                    else
                        table.Cell(i, j).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    table.Cell(i, j).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Cell(i, j).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Cell(i, j).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Cell(i, j).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                }         

            #endregion
            
            #region 2-й вариант путем преобразования текста в таблицу с помощью метода ConvertToTable объекта Range.

            word.Selection.TypeParagraph();
            Word.Range wordrange = docs.Range(ref start, ref end);
            var paragraph1 = docs.Paragraphs.Add(wordrange);
            paragraph1.Range.Text = "2 вариант";
            paragraph1.Range.Font.Bold = 1;
            paragraph1.Range.InsertParagraphAfter();
            selection = word.Selection;
            range = selection.Range;
            //range.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            string outText;
            for (int i = 0; i < 5; i++)
            {
                outText = "";
                for (int j = 0; j < 5; j++)
                {
                    outText += text[i, j] + "|";
                }
                //добавляем наш текст сначала встроку ячейки будут разбиваться по "|"
                range.InsertAfter(outText);
                range.InsertParagraphAfter();
            }
            //конвертируем содержимое (строки) в таблицу по "|"           
            range.ConvertToTable("|");
            //форматируем внешний вид таблицы
            Word.Table tbl = docs.Tables[1];
            tbl.Range.Font.Name = "Verdana";
            tbl.Range.Font.Size = 12F;
            tbl.Range.Font.Bold = 0;
            //tbl.AllowAutoFit = true;
            for (int i = 1; i <= 5; i++)
                for (int j = 1; j <= 5; j++)
                {
                    
                    //первая строка "шапка" будет жирным
                    if (i == 1)
                    {
                        tbl.Rows[i].Range.Font.Bold = 1;
                       
                    }
                    tbl.Cell(i, 0).Width = 115;
                    tbl.Cell(i, j).Width = 85;
                    //размещаем текст в первом столбце под "шапкой" по левому краю
                    if (j == 1 && i > 1)
                        tbl.Cell(i, j).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    //размещаем текст в ячейках по центру
                    else
                        tbl.Cell(i, j).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    tbl.Cell(i, j).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    tbl.Cell(i, j).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    tbl.Cell(i, j).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    tbl.Cell(i, j).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                }

            #endregion


            CloseDocument(path, word, docs, miss, readOnly);            
            Console.ReadKey();

        }

        private static void CloseDocument(object path, Word.Application word, Word.Document docs, object miss, object readOnly)
        {
            //сохраняем документ
            /* docs.SaveAs(ref path, ref miss,
                                 ref miss, ref miss, ref miss,
                                 ref miss, ref readOnly, ref miss,
                                 ref miss, ref miss, ref miss,
                                 ref miss, ref miss, ref miss,
                                 ref miss, ref miss);
             docs.Close(ref miss, ref miss, ref miss);*/
            try
            {
                docs.Save();
                docs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " текущие изменения файла не сохранены!");
            }
            finally
            {
                word.Quit();
                
                if (docs != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(docs);
                if (word != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(word);
                docs = null;
                word = null;
                //собираем мусор
                GC.Collect();
                Console.WriteLine("Нажмите Ввод для выхода");
            }
        }
    }
}