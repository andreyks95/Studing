using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Runtime.InteropServices;
using Microsoft.CSharp.RuntimeBinder;

namespace Excel_Lab_6_KPP
{
    public class ExcelCode
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Запуск Microsoft Excel...");
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null)
            {
                Console.WriteLine("Excel is not properly installed!!");
                return;
            }
            xlApp.Visible =  true; 
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            string[,] text = new string[,]
            {
                {"Наименование показателя", "1 квартал, млн. грн.", "2 квартал, млн. грн.", "3 квартал, млн. грн.","4 квартал, млн. грн."},
                {" Остатки на расчетных и текущих счетах ", "5000", "8000", "1300", "6000"},
                {" Депозиты предприятий и кооперативов ", "1000", "3000", "1000", "5000"},
                {" Межбанковские кредиты ", "1200", "1200", "7000", "2600"},
                {" Вклады граждан ", "2000", "7000", "700", "3000"}
            };
            int rows = text.GetLength(0),
                columns = text.GetLength(1);

            //Вставляем значения в ячейки
            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= columns; j++)
                {
                    xlWorkSheet.Cells[i, j] = text[i-1, j-1];
                    if(j==1)
                        ((Excel.Range)xlWorkSheet.Columns[1]).ColumnWidth = 40;//устанавливаем ширину для первого столбца
                    else
                        ((Excel.Range)xlWorkSheet.Columns[j]).ColumnWidth = 20;//устанавливаем ширину для остальных столбцов
                }
            }

            string[] range = { "B2:B5","C2:C5","D2:D5","E2:E5"};//диапазоны для расчетов
            double[] sumResult = new double[range.Length];//сумма для каждого диапазона значений
            double[] avgResult = new double[range.Length];//среднее для каждого диапазона значений
            Excel.Range xlRng;
            for (int i = 0; i < range.Length; i++)
            {
                xlRng = xlWorkSheet.Range[range[i]];
                sumResult[i] = xlApp.WorksheetFunction.Sum(xlRng);
                avgResult[i] = xlApp.WorksheetFunction.Average(xlRng);
            }
            xlWorkSheet.Cells[6,1] ="Сумма в каждом квартале: ";
            xlWorkSheet.Cells[7,1] = "Среднее в каждом квартале: ";
            for (int j = 0; j < range.Length; j++)
            {
                xlWorkSheet.Cells[6, j+2] = sumResult[j];
                xlWorkSheet.Cells[7, j+2] = avgResult[j];
            }

            range = new string[2] { "B6:E6", "B7:E7" };
            sumResult = new double[1];
            avgResult = new double[1];
            xlRng = xlWorkSheet.Range[range[0]];
            sumResult[0] = xlApp.WorksheetFunction.Sum(xlRng);
            xlRng = xlWorkSheet.Range[range[1]];
            avgResult[0] = xlApp.WorksheetFunction.Average(xlRng);
            xlWorkSheet.Cells[8, 1] = "Сумма всех кварталов: ";
            xlWorkSheet.Cells[9, 1] = "Среднее всех кварталов: ";
            xlWorkSheet.Cells[8, 2] = sumResult[0];
            xlWorkSheet.Cells[9, 2] = avgResult[0];

            Console.WriteLine("Таблица успешно создана!");
            Console.ReadKey();
        }
    }
}

