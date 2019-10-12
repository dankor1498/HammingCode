using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2
{
    class Word
    {
        List<byte> bits;
        int m;
        int k;

        bool IfOneOrZero(int value)
        {
            if (value == 1 || value == 0)
                return true;
            return false;
        }

        public Word(int _n)
        {
            try
            {
                m = _n;
                bits = new List<byte>(m);

                string input = Console.ReadLine();
                int var;

                for (int i = 0; i < m && i < input.Length; i++)
                {
                    var = Int32.Parse(input[i].ToString());
                    if (IfOneOrZero(var))
                        bits.Add(checked((byte)(var)));
                    else
                        throw new Exception();
                }
            }
            catch
            {
                Console.WriteLine("Error input.");
            }   
        }

        public Word(int _n, string b)
        {
            try
            {
                m = _n;
                bits = new List<byte>(m);

                if (b != "")
                {
                    int var;
                    for (int i = 0; i < b.Length && i < m; i++)
                    {
                        var = Int32.Parse(b[i].ToString());
                        if (IfOneOrZero(var))
                            bits.Add(checked((byte)(var)));
                        else
                            throw new Exception();
                    }
                    return;
                }
            }
            catch
            {
                Console.WriteLine("Error input.");
            }
        }

        public void PrintWord()
        {
            if(bits.Count == 0 || bits == null)
            {
                Console.WriteLine("No information.");
                return;
            }
            foreach (var b in bits)
            {
                Console.Write(b + " ");
            }
            Console.WriteLine();
        }

        void SetLength()
        {
            if (m == 1) k = 2;
            if (m >= 2 && m <= 4) k = 3;
            if (m >= 5 && m <= 11) k = 4;
            if (m >= 12 && m <= 26) k = 5;
            Console.WriteLine($"k = {k}");
            Console.WriteLine($"m = {m}");
            Console.WriteLine($"k + m = {k + m}");
        }

        class SB
        {
            public string Key { get; set; }
            public byte Value { get; set; }
            public SB(string s, byte b)
            {
                Key = s;
                Value = b;
            }
        }

        public void CodeWords()
        {
            SetLength();
            Console.ForegroundColor = ConsoleColor.Red;            
            Console.WriteLine("54321 | ");
            Console.WriteLine("------|--------");
            Console.ResetColor();

            List<SB> stringsK = new List<SB>(m);
            for (int i = 1, j = 0, l = 0; i <= k + m; i++)
            {
                if (i != Math.Pow(2, j))
                {
                    string s = Convert.ToString(i, 2).PadLeft(5, '0');
                    stringsK.Add(new SB(s, bits[l++]));
                    continue;
                }
                j++;
            }

            for (int i = 1, j = 0, r = 1, k = 1; i <= this.k + m; i++)
            {
                string str = Convert.ToString(i, 2).PadLeft(5, '0');
                Console.Write(str + " | ");
                if (i == Math.Pow(2, j))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("r" + r + " = ");
                    
                    byte res = 0;
                    Console.Write(ReturnR(r, stringsK, ref res));
                    Console.Write(" = " + res);
                    Console.WriteLine();
                    Console.ResetColor();

                    r++;
                    j++;
                }
                else
                {
                    Console.Write("k" + k + " = " + bits[k - 1]);
                    k++;
                    Console.WriteLine();
                }
            }
        }

        private string ReturnR(int r, List<SB> stringsK, ref byte result)
        {
            string s = "";
            bool flag = true;
            for (int i = 0; i < stringsK.Count; i++)
            {
                if (stringsK[i].Key[5 - r] == '1')
                {
                    s += "k" + (i + 1).ToString() + '^';
                    if(flag == true)
                    {
                        result = bits[i];
                        flag = false;
                        continue;
                    }
                    result ^= stringsK[i].Value;
                }
            }
            return s.Remove(s.Length - 1, 1);
        }
    }
}
