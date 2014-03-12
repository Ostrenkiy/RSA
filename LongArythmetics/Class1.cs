using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongArythmetics
{
    public class LongBit
    {
        public int[] Value = new int[1000];

        public int Length = 0;

        /// <summary>
        /// From decimal to 2-bit
        /// </summary>
        /// <param name="a"></param>
        public LongBit(int a)
        {
            int i = 0;

            if (a == 0)
            {
                this[0] = a;
                return;
            }

            while(a > 0)
            {
                this[i] = a % 2;
                a = a / 2;
                i++;
            }
        }

        public LongBit():this(0)
        { }
        
        public int ToInt()
        {
            int res = 0;
            int powOfTwo = 1;
            for(int i = 0; i < Length; i++)
            {
                res += powOfTwo * this[i];
                powOfTwo *= 2;
            }
            return res;
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            for(int i = Length - 1; i >= 0; i--)
                res.Append(this[i]);

            return res.ToString();
        }

        public int this[int i]
        {
            get { return this.Value[i]; }
            set 
            {
                if(i < Length)
                    this.Value[i] = value;
                if(i >= Length)
                {
                    this.Value[i] = value;
                    this.Length = i + 1;
                }
            }
        }

        private static LongBit GetSum(LongBit a, LongBit b)
        {
            LongBit res = new LongBit();
            int um = 0;
            for(int i = 0; i < Math.Max(a.Length, b.Length); i++ )
            {
                res[i] = (a[i] + b[i] + um)%2;
                um = (a[i] + b[i] + um) / 2;
            }

            if(um != 0)
                res[res.Length] = um;

            return res;
        }

        /// <summary>
        /// Вычисляет разность чисел а и b при a > b  (!!!!!!!)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static LongBit GetMinus(LongBit a, LongBit b)
        {
            LongBit aClone = a;
            LongBit res = new LongBit();
            for (int i = 0; i < Math.Max(a.Length, b.Length); i++)
            {
                if(a[i] < b[i])
                {
                    for(int j = i; j < Math.Max(a.Length, b.Length); j++)
                        if(a[j] != 0)    
                        {
                            a[j]--;
                            break;
                        }
                        else
                            a[j] = 1;
                    a[i] = 2;
                }
                res[i] = a[i] - b[i];
            }
            a = aClone;
            return res;
        }

        private static LongBit GetSlowMultiplication(LongBit a, LongBit b)
        {
            LongBit res = new LongBit();
            for(int i = 0; i < b.Length; i++)
            {
                for(int j = 0; j < a.Length; j++)
                {
                    res[i + j] += a[j] * b[i];
                }
            }

            for(int i = 0; i < res.Length; i++ )
            {
                if (res[i] > 1)
                {
                    res[i + 1] += res[i] / 2;
                    res[i] = res[i] % 2;
                }
            }
            
            return res;
        }

        public static LongBit operator +(LongBit a, LongBit b)
        {
            return GetSum(a, b);
        }

        public static LongBit operator -(LongBit a, LongBit b)
        {
            return GetMinus(a, b);
        }

        public static LongBit operator *(LongBit a, LongBit b)
        {
            return GetSlowMultiplication(a, b);
        }
    }
}
