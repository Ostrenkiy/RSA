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
            LongBit a = new LongBit(Int32.Parse(Console.ReadLine()));
            LongBit b = new LongBit(Int32.Parse(Console.ReadLine()));
            Console.WriteLine((a + b).ToInt());
            Console.WriteLine((a - b).ToInt());
            Console.WriteLine((a * b).ToInt());
            Console.ReadLine();
        }

    }
}
