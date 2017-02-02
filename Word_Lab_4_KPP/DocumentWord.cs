using System;
using System.Collections.Generic;
using System.Linq;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Threading;

namespace Word_Lab_4_KPP
{
    class DocumentWord
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
            Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref isVisible, ref miss, ref miss, ref miss, ref miss);
            //получааем текст
            string text = GetText(docs);
            //выводим весь текст с файла
            Console.WriteLine(text);
            Console.Write("\nВведите букву с которой будет начинаться слово \nВыполните ввод: ");
            //считываем нажатую клавишу
            char symbol = (char)Console.Read();
            //получаем результаты: количество и слова которые найдены
            var tupleGetCount = GetCount(symbol, text);
            int count = tupleGetCount.Item1; //количество 
            List<string> wordsFind = new List<string>(); //для слов
            wordsFind = tupleGetCount.Item2; //слова
            string result = "Найдено: " + count + " слова, которые начинаются с буквы: " + symbol;
            result += "\nНайденные слова которые начинаются с буквы " + symbol;
            //LINQ вместо foreach
            result = wordsFind.Aggregate(result, (current, i) => current + ("\n" + i));
            Console.WriteLine(result);

            //вставляем результаты выполнения приложения в doc файл.
            docs.Content.InsertAfter("\r\n" + result);

            //сохраняем документ
            docs.SaveAs(ref path, ref miss,
                                ref miss, ref miss, ref miss,
                                ref miss, ref readOnly, ref miss,
                                ref miss, ref miss, ref miss,
                                ref miss, ref miss, ref miss,
                                ref miss, ref miss);
            docs.Close(ref miss, ref miss, ref miss);
            word.Quit(false);          
            if (docs != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(docs);
            if (word!= null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(word);
            docs = null;
            word = null;
            //собираем мусор
            GC.Collect();
            Console.WriteLine("Результат выполнения приложения записан в соответствующий файл. ");
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

        //получить количество слов, начинающихся на  букву
        //вернуть в кортеже сразу два значения
        private static Tuple<int, List<string>> GetCount(char symbolSearch, string textForSearch)
        {
            string text = textForSearch.ToLower();
            string symbolCache = symbolSearch.ToString().ToLower();
            char symbol = char.Parse(symbolCache);
            int count = 0;
            List<string> words = new List<string>();//сюда будут записываться найденные слова
            for (int i = 0; i < text.Length; i++)
            {
                //найти слово которое начинается с символа.
                if (text[i] == ' ')
                    if (text[i + 1] == symbol)
                    {
                        count++; //увеличиваем кол. слов найденных
                        string findWord = null;
                        //записываем найденное слово
                        for (int j = i + 1; j < text.Length; j++)
                        {
                            if (text[j] == ' ') break; //если найденное слово закончилось выходим (обрезаем строку)
                            else
                                findWord += text[j];
                        }
                        //добавим слово в общий список найденных слов
                        words.Add(findWord);
                    }
            }
            return new Tuple<int, List<string>>(count,words);
        }

    }

    
}
