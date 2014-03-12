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


        /// <summary>
        /// Возвращает число типа LongBit, состоящее из length символов, начиная с l-того
        /// </summary>
        /// <param name="l"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private LongBit Copy(int l, int length)
        {
            LongBit res = new LongBit();
            for(int i = l; i - l < length; i++)
                res[i - l] = this[i];

            return res;
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

        private void Normalize()
        {
            for(int i = 0; i < this.Length; i++)
            {
                if(this[i] > 1)
                {
                    this[i + 1] += this[i] / 2;
                    this[i] = this[i] % 2;
                }
            }
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

            res.Normalize();
            
            return res;
        }

        private static LongBit GetFastMultiplication(LongBit a, LongBit b)
        {
            if(a.Length < 40 || b.Length < 40)
                return GetSlowMultiplication(a, b);

            LongBit res = new LongBit();

            LongBit A0 = a.Copy(0, a.Length / 2);
            LongBit A1 = a.Copy(a.Length, a.Length / 2 + a.Length % 2);
            LongBit B0 = b.Copy(0, b.Length / 2);
            LongBit B1 = b.Copy(b.Length, b.Length / 2 + b.Length % 2);

            LongBit A0B0 = GetFastMultiplication(A0, B0);
            LongBit A1B1 = GetFastMultiplication(A1, B1);
            LongBit AsBs = GetFastMultiplication(A0 + A1, B0 + B1);

            LongBit middle = AsBs - A0B0 - A1B1;
            res = A0B0;
            for(int i = A0B0.Length; i < A0B0.Length + A1B1.Length; i++)
                res[i] = A1B1[i - A0B0.Length];

            for(int i = 0; i < middle.Length; i++)
                res[i + A0.Length] += middle[i];

            res.Normalize();

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
            return GetFastMultiplication(a, b);
        }
    }
}
