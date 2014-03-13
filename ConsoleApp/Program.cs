using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LongArythmetics;


namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Random gen = new Random();
            for(int i = 0; i < 100000; i++)
            {
                int a = gen.Next(1, 1000);
                int b = gen.Next(1, a);
                LongBit aL = new LongBit(a);
                LongBit bL = new LongBit(b);
                if((a + b) != (aL + bL).ToInt())
                {
                    Console.WriteLine("Числа {0} и {1}, ошибка в +", a, b);
                    break;
                }
                if((a - b) != (aL - bL).ToInt())
                {
                    Console.WriteLine("Числа {0} и {1}, ошибка в -", a, b);
                    break;
                }

                if((a * b) != (aL * bL).ToInt())
                {
                    Console.WriteLine("Числа {0} и {1}, ошибка в *", a, b);
                    break;
                }
                if((a % b) != (aL % bL).ToInt())
                {
                    Console.WriteLine("Числа {0} и {1}, ошибка в %", a, b);
                    break;
                }

                if(a << 5 != (aL << 5).ToInt())
                {
                    Console.WriteLine("Числа {0} и {1}, ошибка в <<", a, b);
                    break;
                }
                if(a >> 5 != (aL >> 5).ToInt())
                {
                    Console.WriteLine("Числа {0} и {1}, ошибка в >>", a, b);
                    break;
                }
                if((a ^ b) != (aL ^ bL).ToInt())
                {
                    Console.WriteLine("Числа {0} и {1}, ошибка в ^", a, b);
                    break;
                }

                Console.WriteLine("Числа {0} и {1}, ОК", a, b);
            }
            Console.ReadLine();
        }

    }
}
