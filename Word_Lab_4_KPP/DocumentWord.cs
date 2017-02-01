using System;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;

namespace Word_Lab_4_KPP
{
    class DocumentWord
    {
        private static void Main(string[] args)
        {
            //создаём документ
            Word.Application word = new Word.Application();
            object miss = Missing.Value;
            object path = @"C:\Users\Андрей\Desktop\WordDocument.docx";
            object readOnly = true;
            Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
            //получааем текст
            string text = GetText(docs);
            //выводим весь текст с файла
            Console.WriteLine(text);
            Console.Write("Введите букву с которой будет начинаться слово \nВыполните ввод: ");
            //считываем нажатую клавишу
            char symbol = (char)Console.Read();
            Console.WriteLine("Найдено: " + GetCount(symbol, text.ToLower()) + " слова, которые начинаются с буквы: " + symbol);
            //закрываем документ без сохранения и выходим
            docs.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
            word.Quit();
            Console.ReadKey();

        }

        //получить весь текст в файле
        private static string GetText(Word.Document docs)
        {
            string totaltext = "";
            //вытаскиваем текст
            for (int i = 0; i < docs.Paragraphs.Count; i++)
            {
                totaltext += " \r\n " + docs.Paragraphs[i+1].Range.Text.ToString();
            }
            return totaltext;
        }

        //получить количество слов, начинающихся на такую букву
        private static int GetCount(char symbol, string text)
        {
            int count = 0;
            for (int i=0; i < text.Length; i++)
            {
                if (text[i] == ' ')
                   if (text[i + 1] == symbol)
                       count++;
            }
            return count;
        }

    }

    
}
