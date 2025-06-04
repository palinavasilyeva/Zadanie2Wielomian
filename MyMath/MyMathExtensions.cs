using System;
using MyMath;

namespace MyExtensions
{
    public static class MyMathExtensions
    {
        public static double Eval(this Wielomian w, double x)
        {
            double result = 0;
            for (int i = w.Stopien; i >= 0; i--)
            {
                result = result * x + w[i];
            }
            return result;
        }

        public static int WielomianPoprzedzaComparison(Wielomian w1, Wielomian w2)
        {
            if (w1 == null || w2 == null) return w1 == null && w2 == null ? 0 : w1 == null ? -1 : 1;
            if (w1.Stopien != w2.Stopien) return w1.Stopien.CompareTo(w2.Stopien);
            for (int i = w1.Stopien; i >= 0; i--)
            {
                if (w1[i] != w2[i]) return w1[i].CompareTo(w2[i]);
            }
            return 0;
        }
    }
}