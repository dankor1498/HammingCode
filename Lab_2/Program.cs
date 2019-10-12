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
            //Word word = new Word(26, "00000000000000011000000101");
            //Word word = new Word(4, "0101");
            //Word word = new Word(3, "101");
            //Word word = new Word(10, "1111101111");
            int n = Int32.Parse(Console.ReadLine());
            Word word = new Word(n);
            word.PrintWord();
            word.CodeWords();
            Console.ReadLine();            
        }
    }
}
