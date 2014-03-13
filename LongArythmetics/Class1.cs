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

        public LongBit(LongBit a)
        {
            this.Value = (int[])a.Value.Clone();
            this.Length = a.Length;
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

        private void CountLength()
        {
            for(int i = this.Length - 1; i > 0; i--)
                if(this[i] != 0)
                    break;
                else
                    this.Length--;
        }

        /// <summary>
        /// Вычисляет разность чисел а и b при a > b  (!!!!!!!)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static LongBit GetMinus(LongBit a, LongBit b)
        {
            LongBit aClone = new LongBit(a);
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

            res.CountLength();

            a.Value = aClone.Value;
            a.Length = aClone.Length;
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
            while (this[this.Length-1] > 1)
            {
                this[this.Length] += this[this.Length - 1] / 2;
                this[this.Length - 1] = this[this.Length - 1] % 2;
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

        /// <summary>
        /// Сравнивает два числа 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>-1 , если a < b; 0, если а = b и 1, если a > b </returns>
        private static int Compare(LongBit a, LongBit b)
        {
            if(a.Length > b.Length)
                return 1;
            if(a.Length < b.Length)
                return -1;
            if(String.Compare(a.ToString(), b.ToString()) > 0)
                return 1;
            if(String.Compare(a.ToString(), b.ToString()) < 0)
                return -1;
            return 0;
        }

        /// <summary>
        /// Сдвиг числа на l битов влево
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        private LongBit LeftShift(int l)
        {
            LongBit res = new LongBit();
            for(int i = l; i < l+this.Length; i++)
            {
                res[i] = this[i - l];
            }
            return res;
        }

        private LongBit RightShift(int l)
        {
            return this.Copy(l, this.Length - l);
        }

        private static LongBit Mod(LongBit a, LongBit b)
        {
            LongBit aClone = new LongBit(a);
            while (a >= b)
            {
                if (a >= (b << (a.Length - b.Length)))
                {
                    a = a - (b << (a.Length - b.Length));
                }
                else
                {
                    a = a - (b << (a.Length - b.Length - 1));
                }
            }
            LongBit res = a;
            a = aClone;
            return res;
        }

        private static LongBit Xor(LongBit a, LongBit b)
        {
            LongBit res = new LongBit();
            for(int i = 0; i < Math.Max(a.Length, b.Length); i++)
            {
                if(a[i] == b[i])
                    res[i] = 0;
                else
                    res[i] = 1;
            }
            res.CountLength();
            return res;
        }

        public static LongBit operator ^(LongBit a, LongBit b)
        {
            return Xor(a, b);
        }
        public static LongBit operator %(LongBit a, LongBit b)
        {
            return Mod(a, b);
        }
        public static LongBit operator <<(LongBit a, int l)
        {
            return a.LeftShift(l);
        }
        public static LongBit operator >>(LongBit a, int l)
        {
            return a.RightShift(l);
        }
        public static bool operator ==(LongBit a, LongBit b)
        {
            return (Compare(a, b) == 0);
        }
        public static bool operator >(LongBit a, LongBit b)
        {
            return (Compare(a, b) == 1);
        }
        public static bool operator <(LongBit a, LongBit b)
        {
            return (Compare(a, b) == -1);
        }
        public static bool operator >=(LongBit a, LongBit b)
        {
            return (Compare(a, b) >= 0);
        }
        public static bool operator <=(LongBit a, LongBit b)
        {
            return (Compare(a, b) == 0);
        }
        public static bool operator !=(LongBit a, LongBit b)
        {
            return (Compare(a, b) != 0);
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
