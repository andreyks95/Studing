﻿using Excel = Microsoft.Office.Interop.Excel;
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
            //настраиваем запуск Excel и книгу и листы.
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null)
            {
                Console.WriteLine("Excel is not properly installed!!");
                return;
            }
            xlApp.Visible = true;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet) xlWorkBook.Worksheets.get_Item(1);

            //предыдущая таблица создавалась для 11 варианта 
            string[,] text = new string[,] { 
                        {"Привлеченные средства коммерческого банка", "Сумма млн.грн." },
                        {"Депозиты государственных предприятий","2000"},
                        {"Депозиты с/ х предприятий","850"},
                        {"Депозиты СП","700"},
                        {"Вклады населения","4000"},
                        {"Депозиты внебюджетных фондов","1000"},
                        {"Депозиты АО и ТОО","1200"},
                        {"Остатки на расчетных и текущих счетах клиентов","8000"},
                        {"Депозиты юридических лиц в валюте(в грн.)","5000"}
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

            string[] range = { "B2:B9"};//диапазоны для расчетов по столбцам"B2:B5","C2:C5","D2:D5","E2:E5"//
            double[] sumResult = new double[range.Length];//сумма для каждого диапазона значений
            double[] avgResult = new double[range.Length];//среднее для каждого диапазона значений
            Excel.Range xlRng;
            //считаем среднее и сумму всех строк
            for (int i = 0; i < range.Length; i++)
            {
                xlRng = xlWorkSheet.Range[range[i]];//получаем диапазон значений
                sumResult[i] = xlApp.WorksheetFunction.Sum(xlRng);//считаем сумму 
                avgResult[i] = xlApp.WorksheetFunction.Average(xlRng);//считаем среднее
            }
            xlWorkSheet.Cells[10, 1] = "Общая сумма млн. грн.:";//"Сумма в каждом квартале: ";
            xlWorkSheet.Cells[11, 1] = "Среднее млн. грн.";//"Среднее в каждом квартале: ";
            //выводим итоги
            for (int j = 0; j < range.Length; j++)
            {
                xlWorkSheet.Cells[10, j+2] = sumResult[j];
                xlWorkSheet.Cells[11, j+2] = avgResult[j];
            }

            #region Общий итог. То есть сумма и среднее всех предыдущих столбцов
            //ОБЩИЙ ИТОГ
            /*range = new string[2] { "B6:E6", "B7:E7" };
            sumResult = new double[1];
            avgResult = new double[1];
            xlRng = xlWorkSheet.Range[range[0]];
            sumResult[0] = xlApp.WorksheetFunction.Sum(xlRng);
            xlRng = xlWorkSheet.Range[range[1]];
            avgResult[0] = xlApp.WorksheetFunction.Average(xlRng);
            xlWorkSheet.Cells[8, 1] = "Сумма всех кварталов: ";
            xlWorkSheet.Cells[9, 1] = "Среднее всех кварталов: ";
            xlWorkSheet.Cells[8, 2] = sumResult[0];
            xlWorkSheet.Cells[9, 2] = avgResult[0];*/
            #endregion

            Console.WriteLine("Таблица успешно создана!");
            Console.ReadKey();
        }
    }
}

