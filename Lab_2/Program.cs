using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Default;

            int m;
            string inputWord, inputWordError;
            Word word;

            Console.Write("Введіть довжину m(від 1 до 57): ");
            m = int.Parse(Console.ReadLine());

            Console.WriteLine($"Введіть текст для кодування(в бітовому вигляді), довжиною {m}:");
            inputWord = Console.ReadLine();

            Console.WriteLine($"Введіть текст з можливими помилками, довжиною {m}:");
            inputWordError = Console.ReadLine();

            word = new Word(m, inputWord, inputWordError);

            //word = new Word(30, "000000000000000110000001010000", "000000000000000110000001010000");
            //word = new Word(57, "000000000000000110000001010000000000000000000110000001010", 
            //    "000000000000000110000001010000000000000000000110000001010");
            //word = new Word(4,"0101", "0111");
            //word = new Word(4,"0101", "1111");//звичайне подвійне
            //word = new Word(3, "101", "111");
            //word = new Word(1, "1", "0");
            //word = new Word(5, "11110", "11011");//подвійна, перевищує довжину
            //word = new Word(4, "1000", "0001");//подвійна, на надлишковий біт

            word.PrintWord();
            word.CodeWords();
            word.Decode();

            Console.ReadLine();
        }
    }
}
