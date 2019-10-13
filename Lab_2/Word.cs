using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2
{
    class Word
    {
        List<byte> bits;//вхідні біти
        List<byte> bitsError;
        List<byte> rbits;//значення r
        List<List<int>> kplaces;//номерки k які використовуються в розрахунках r
        List<int> places;//місця k в результі
        byte bitError;//біт помилки
        int m;
        int k;
        string codeResult = "";
        string inputResultError = "";
        StringBuilder resultError;

        private void SetError()
        {     
            byte res = 0;
            string sRes = "";
            for (int i = 0; i < rbits.Count; i++)
            {
                Console.Write("C" + (i + 1).ToString() + " = " + "r" + (i + 1).ToString() +
                    "^" + ReturnR(rbits[i], i, ref res, resultError.ToString()) + " = ");
                Console.WriteLine(res);
                sRes += res;
            }
            sRes = new string(sRes.ToCharArray().Reverse().ToArray());
            int sResDec = Convert.ToInt32(sRes, 2);
            
            Console.WriteLine("Переведемо синдром в десяткову систему числення: 0b" + sRes + " = " + sResDec);
            if (sResDec == 0)
            {
                Console.WriteLine("Синдром рівний нулю, спотворення відсутнє.");
                return;
            }
            Console.WriteLine("При передачі виявлено помилки.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Визначимо тип помилки та спробуємо її виправити:");
            Console.ResetColor();
            Console.WriteLine("Позиція можливої помилки: " + sResDec);

            bool flag = true;
            for(int i =0; i < places.Count; i++)
            {
                if(sResDec - 1 == places[i])
                {
                    flag = false;
                }
            }
            if (sResDec > m + k)
            {
                Console.WriteLine("Позиція перевищує довжину слова, подвійна помилка, повторна передача.");
                return;
            }
            if (flag)
            {
                Console.WriteLine("Позиція вказує на надлишковий біт, подвійна помилка, повторна передача.");
                return;
            }

            byte b = byte.Parse(resultError[sResDec - 1].ToString());
            b = (byte)(b ^ 1);
            resultError[sResDec - 1] = char.Parse(b.ToString());

            Console.Write("Після інверсії помилкового біта та видалення надлишкових бітів маємо результат: ");
            foreach (var i in places)
            {
                Console.Write(resultError[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Біт помилки при передачі(BitOfError): " + bitError);
            byte resErr = byte.Parse(resultError[places[0]].ToString());
            for (int i = 1; i < places.Count; i++)
            {
                resErr ^= byte.Parse(resultError[places[i]].ToString());
            }
            resErr = (byte)(bitError ^ resErr);

            Console.WriteLine("Визначимо тип помилки: ");
            for (int i = 0; i < places.Count; i++)
            {
                Console.Write("k" + (i + 1).ToString() + "^");
            }
            Console.Write($"BitOfError = {resultError[places[0]]}^");

            for (int i = 1; i < places.Count; i++)
            {
                Console.Write(resultError[places[i]] + "^");
            }
            Console.WriteLine(bitError + " = " + resErr);
            
            if (resErr == 0)
            {
                Console.WriteLine("Результат рівний 0, отже спотворення одиничне.");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Виправлений та кінцевий результат: ");
                foreach (var i in places)
                {
                    Console.Write(resultError[i] + " ");
                }
                Console.WriteLine();
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Результат рівний 1, отже спотворення подвійне, повторна передача.");
            }
        }

        bool IfOneOrZero(int value)
        {
            if (value == 1 || value == 0)
                return true;
            return false;
        }

        public Word(int _n, string b, string e)
        {
            try
            {
                m = _n;
                inputResultError = e;
                bits = new List<byte>(m);
                bitsError = new List<byte>(m);

                if (b != "")
                {
                    int var;
                    for (int i = 0; i < b.Length && i < m; i++)
                    {
                        var = Int32.Parse(b[i].ToString());
                        if (IfOneOrZero(var))
                        {
                            bits.Add(checked((byte)(var)));
                            var = Int32.Parse(e[i].ToString());
                            bitsError.Add(checked((byte)(var)));
                        }
                        else
                            throw new Exception();
                    }

                    bitError = bits[0];
                    for (int i = 1; i < bits.Count; i++)
                    {
                        bitError ^= bits[i];
                    }

                    return;
                }
            }
            catch
            {
                Console.WriteLine("Помилка введення.");
            }
        }

        public void PrintWord()
        {
            if(bits.Count == 0 || bits == null)
            {
                Console.WriteLine("Інформація відсутня.");
                return;
            }
            Console.Write("Послідовність інформаціних бітів для пересилки(правильна): ");
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
            if (m >= 27 && m <= 57) k = 6;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Сформуємо дані для задачі: ");
            Console.ResetColor();                     

            rbits = new List<byte>(k);
            kplaces = new List<List<int>>(k);
            Console.WriteLine($"Кількість надлишкових символів = {k}");
            Console.WriteLine($"Кількість інформаційних бітів = {m}");
            Console.WriteLine($"Довжина закодованого слова разом = {k + m}");
            Console.WriteLine();
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
            places = new List<int>(m);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Закодуємо інформацію за алгоритмом:");
            Console.WriteLine("654321 | ");
            Console.WriteLine("-------|");
            Console.ResetColor();

            List<SB> stringsK = new List<SB>(m);
            for (int i = 1, j = 0, l = 0; i <= k + m; i++)
            {
                if (i != Math.Pow(2, j))
                {
                    string s = Convert.ToString(i, 2).PadLeft(6, '0');
                    stringsK.Add(new SB(s, bits[l++]));
                    continue;
                }
                j++;
            }

            for (int i = 1, j = 0, r = 1, k = 1; i <= this.k + m; i++)
            {
                string str = Convert.ToString(i, 2).PadLeft(6, '0');
                Console.Write(str + " | ");
                if (i == Math.Pow(2, j))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("r" + r + " = ");
                    
                    byte res = 0;
                    Console.Write(ReturnR(r, stringsK, ref res));
                    Console.Write(" = " + res);
                    rbits.Add(res);
                    Console.WriteLine();
                    Console.ResetColor();

                    codeResult += res;

                    r++;
                    j++;
                }
                else
                {
                    places.Add(i - 1);
                    Console.Write("k" + k + " = " + bits[k - 1]);
                    codeResult += bits[k - 1];
                    k++;
                    Console.WriteLine();
                }                
            }

            resultError = new StringBuilder(codeResult);
            for (int i = 0; i < places.Count; i++)
            {
                resultError[places[i]] = inputResultError[i];
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Закодований результат для пересилки(правильний):   {codeResult}");
            Console.WriteLine($"Отриманий на виході результат(потребує перевірки): {resultError}");
            Console.ResetColor();            
        }

        public void Decode()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Декодуємо інформацію та знайдемо можливі помилки: ");
            Console.ResetColor();
            Console.WriteLine("Обчислимо синдром:");
            SetError();        
        }

        private string ReturnR(byte r, int position, ref byte result, string cResult)
        {
            string s = "";
            string s2 = "";
            bool flag = true;
            for (int i = 0; i < kplaces[position].Count; i++)
            {
                s += "k" + kplaces[position][i] + '^';
                if (flag == true)
                {
                    result = r;
                    s2 += r + "^";
                    flag = false;
                }
                s2 += cResult[places[kplaces[position][i] - 1]] + "^";
                result ^= byte.Parse(cResult[places[kplaces[position][i] - 1]].ToString());
            }
            return s.Remove(s.Length - 1, 1) + " = " + s2.Remove(s2.Length - 1, 1);
        }

        private string ReturnR(int r, List<SB> stringsK, ref byte result)
        {
            kplaces.Add(new List<int>());
            string s = "";
            string s2 = "";
            bool flag = true;
            for (int i = 0; i < stringsK.Count; i++)
            {
                if (stringsK[i].Key[6 - r] == '1')
                {
                    s += "k" + (i + 1).ToString() + '^';
                    kplaces[kplaces.Count - 1].Add(i + 1);
                    if (flag == true)
                    {
                        result = stringsK[i].Value;
                        s2 += stringsK[i].Value + "^";
                        flag = false;
                        continue;
                    }
                    s2 += stringsK[i].Value + "^";
                    result ^= stringsK[i].Value;
                }
            }
            return s.Remove(s.Length - 1, 1) + " = " + s2.Remove(s2.Length - 1, 1);
        }
    }
}
