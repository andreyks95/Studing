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
            //object path = @"C:\Users\Андрей\Documents\Visual Studio 2015\Projects\Studing.git\Word_Lab_5_KPP\Doc.docx";
            object readOnly = false;
            //object isVisible = false;
            word.Visible = false;
            // Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss,
            //    ref miss, ref miss, ref miss, ref miss, ref miss, ref isVisible, ref miss, ref miss, ref miss, ref miss);
            Console.WriteLine("Запуск построения таблиц в файле");

            string[,] text = new string[,]
            {
                {"Привлеченные средства коммерческого банка",  "Сумма, млн. грн."},
                {"Депозиты государственных предприятий", "2000"},
                {"Депозиты с/х предприятий", "850"},
                {"Депозиты СП", "700"},
                {"Вклады населения", "4000"},
                {"Депозиты внебюджетных фондов", "1000"},
                {"Депозиты АО и ТОО", "1200"},
                {"Остатки на расчетных и текущих счетах клиентов",  "8000"},
                {"Депозиты юридических лиц в валюте (в грн.)", "5000"}
            };
            int rows = text.GetLength(0); //количество строк
            int columns = text.GetLength(1); //количество столбцов
            word.Visible = true;
            object oEndOfDoc = "\\endofdoc";
            Word.Document docs = word.Documents.Add(ref miss, ref miss, ref miss, ref miss);

            #region 1-й вариант путем непосредственного использования коллекции Tables объекта Document.

            string msg = "1-й вариант путем непосредственного использования коллекции Tables объекта Document";
            InsertParagraph(docs, msg); //вставляем параграф перед текстом

            var selection = word.Selection;
            //Range означает захват промежутка данных между объектами документа (текста, параграфа, таблицы и т.д.)
            Word.Range range = selection.Range;
            Word.Table table;         
            Word.Range wrdRng = docs.Bookmarks.get_Item(ref oEndOfDoc).Range; //вставляем таблицу после параграфа то есть в конце документа
            table = docs.Tables.Add(wrdRng, rows, columns, ref miss, ref miss);
            for (int i = 1; i <= rows; i++)
                for (int j = 1; j <= columns; j++)
                    table.Cell(i, j).Range.Text = text[i - 1, j - 1];
            SetFormatTable(table, rows, columns);

            #endregion

            #region 2-й вариант путем преобразования текста в таблицу с помощью метода ConvertToTable объекта Range

            msg = "2-й вариант путем преобразования текста в таблицу с помощью метода ConvertToTable объекта Range";
            InsertParagraph(docs, msg);
            
            range = docs.Bookmarks.get_Item(ref oEndOfDoc).Range;  //вставляем таблицу после параграфа то есть в конце документа        
            string outText;
            for (int i = 0; i < rows; i++)
            {
                outText = "";
                for (int j = 0; j < columns; j++)
                    outText += text[i, j] + "|";
                //добавляем наш текст сначала встроку ячейки будут разбиваться по "|"
                range.InsertAfter(outText);
                range.InsertParagraphAfter();
            }
            //конвертируем содержимое (строки) в таблицу по "|"           
            range.ConvertToTable("|");           
            //форматируем внешний вид таблицы
            Word.Table tbl = docs.Tables[2];
            SetFormatTable(tbl, rows, columns);

            #endregion

            CloseDocument(/*path, */word, docs, miss, readOnly);            
            Console.ReadKey();
        }

        //добавляем параграф и настраиваем стили отображения параграфа
        private static void InsertParagraph(Word.Document docs, string msg)
        {          
            Word.Paragraph par = docs.Paragraphs.Add(Missing.Value);
            par.Range.InsertParagraphBefore();
            par.Range.Font.Name = "Times New Roman";
            par.Range.Font.Size = 14F;
            par.Range.Text = msg;
            par.Range.Font.Bold = 1;
            par.Range.InsertParagraphAfter();
        }

        //настраиваем стили отображения таблицы
        private static void SetFormatTable(Word.Table tbl, int rows, int columns)
        {
            tbl.Range.Font.Name = "Verdana";
            tbl.Range.Font.Size = 12F;
            tbl.Range.Font.Bold = 0;
            //tbl.AllowAutoFit = true;
            for (int i = 1; i <= rows; i++)
                for (int j = 1; j <= columns; j++)
                {
                    //первая строка "шапка" будет жирным
                    if (i == 1)
                        tbl.Rows[i].Range.Font.Bold = 1;
                    tbl.Cell(i, 0).Width = 110;
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
        }


        //Закрываем документ
        private static void CloseDocument(/*object path, */Word.Application word, Word.Document docs, object miss, object readOnly)
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