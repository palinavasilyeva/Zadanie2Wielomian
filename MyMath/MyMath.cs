using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMath
{
    public sealed class Wielomian : IEquatable<Wielomian>, IEnumerable<int>
    {
        private readonly int[] _coefficients;

        public int Stopien => _coefficients.Length - 1;

        public Wielomian() : this(new int[] { 0 }) { }

        public Wielomian(params int[] coefficients)
        {
            if (coefficients == null) throw new NullReferenceException();
            if (coefficients.Length == 0) throw new ArgumentException("wielomian nie moze być pusty");
            _coefficients = TrimLeadingZeros(coefficients);
        }

        private static int[] TrimLeadingZeros(int[] coefficients)
        {
            int nonZeroIndex = Array.FindLastIndex(coefficients, x => x != 0);
            if (nonZeroIndex == -1) return new int[] { 0 };
            return coefficients.Take(nonZeroIndex + 1).ToArray();
        }

        public override string ToString()
        {
            if (Stopien == 0 && _coefficients[0] == 0) return "0";
            var sb = new StringBuilder();
            for (int i = Stopien; i >= 0; i--)
            {
                int coef = _coefficients[i];
                if (coef == 0) continue;
                string sign = coef > 0 && sb.Length > 0 ? " + " : (coef < 0 ? " - " : "");
                int absCoef = Math.Abs(coef);
                string coefStr = absCoef == 1 && i > 0 ? "" : absCoef.ToString();
                string power = i > 1 ? $"x^{i}" : i == 1 ? "x" : "";
                sb.Append($"{sign}{coefStr}{power}");
            }
            return sb.Length > 0 ? sb.ToString() : "0";
        }

        public bool Equals(Wielomian other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Stopien != other.Stopien) return false;
            return _coefficients.SequenceEqual(other._coefficients);
        }

        public override bool Equals(object obj) => Equals(obj as Wielomian);

        public override int GetHashCode() => _coefficients.Aggregate(0, (hash, coef) => hash * 31 + coef);

        public static bool operator ==(Wielomian left, Wielomian right) => Equals(left, right);
        public static bool operator !=(Wielomian left, Wielomian right) => !Equals(left, right);

        public static Wielomian operator +(Wielomian left, Wielomian right)
        {
            int maxDegree = Math.Max(left.Stopien, right.Stopien);
            int[] result = new int[maxDegree + 1];
            for (int i = 0; i <= maxDegree; i++)
            {
                int leftCoef = i <= left.Stopien ? left[i] : 0;
                int rightCoef = i <= right.Stopien ? right[i] : 0;
                result[i] = leftCoef + rightCoef;
            }
            return new Wielomian(result);
        }

        public static Wielomian operator -(Wielomian left, Wielomian right)
        {
            int maxDegree = Math.Max(left.Stopien, right.Stopien);
            int[] result = new int[maxDegree + 1];
            for (int i = 0; i <= maxDegree; i++)
            {
                int leftCoef = i <= left.Stopien ? left[i] : 0;
                int rightCoef = i <= right.Stopien ? right[i] : 0;
                result[i] = leftCoef - rightCoef;
            }
            return new Wielomian(result);
        }

        public static Wielomian operator +(Wielomian left, int right) => left + new Wielomian(right);
        public static Wielomian operator +(int left, Wielomian right) => new Wielomian(left) + right;
        public static Wielomian operator -(Wielomian left, int right) => left - new Wielomian(right);
        public static Wielomian operator -(int left, Wielomian right) => new Wielomian(left) - right;

        public static Wielomian operator *(Wielomian left, Wielomian right)
        {
            int[] result = new int[left.Stopien + right.Stopien + 1];
            for (int i = 0; i <= left.Stopien; i++)
            {
                for (int j = 0; j <= right.Stopien; j++)
                {
                    result[i + j] += left[i] * right[j];
                }
            }
            return new Wielomian(result);
        }

        public static (Wielomian Quotient, Wielomian Remainder) operator /(Wielomian dividend, Wielomian divisor)
        {
            if (divisor.Stopien == -1 || divisor._coefficients[0] == 0)
                throw new DivideByZeroException("Dzielnik nie może być zerem");

            var quotient = new int[dividend.Stopien - divisor.Stopien + 1];
            var remainder = dividend._coefficients.ToArray();

            for (int i = dividend.Stopien - divisor.Stopien; i >= 0; i--)
            {
                quotient[i] = remainder[divisor.Stopien + i] / divisor[divisor.Stopien];
                for (int j = 0; j <= divisor.Stopien; j++)
                {
                    remainder[i + j] -= quotient[i] * divisor[j];
                }
            }
            return (new Wielomian(quotient), new Wielomian(remainder));
        }

        public static implicit operator Wielomian(int value) => new Wielomian(value);

        public static explicit operator int[](Wielomian w) => w._coefficients.Reverse().ToArray();

        public static explicit operator int(Wielomian w)
        {
            if (w.Stopien != 0) throw new InvalidCastException("wielomian nie jest stopnia zerowego");
            return w._coefficients[0];
        }

        public int this[int index]
        {
            get
            {
                if (index < 0 || index > Stopien) throw new IndexOutOfRangeException();
                return _coefficients[index];
            }
        }

        public IEnumerator<int> GetEnumerator() => ((IEnumerable<int>)_coefficients).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static Wielomian Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) throw new ArgumentException("Nieprawidłowy format ciągu");
            if (s.Trim() == "0") return new Wielomian();

            var terms = s.Replace(" ", "").Split(new[] { "+", "-" }, StringSplitOptions.RemoveEmptyEntries);
            var coefficients = new Dictionary<int, int>();
            int maxDegree = 0;

            int sign = 1;
            int pos = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '+') { sign = 1; continue; }
                if (s[i] == '-') { sign = -1; continue; }
                if (s[i] == ' ') continue;

                var term = terms[pos++];
                int coef = 1, degree = 0;
                if (term.Contains("x"))
                {
                    var parts = term.Split(new[] { "x^", "x" }, StringSplitOptions.None);
                    coef = parts[0] == "" || parts[0] == "-" ? (parts[0] == "-" ? -1 : 1) : int.Parse(parts[0]);
                    degree = parts.Length > 1 && parts[1] != "" ? int.Parse(parts[1]) : 1;
                }
                else
                {
                    coef = int.Parse(term);
                }
                coef *= sign;
                coefficients[degree] = coef;
                if (degree > maxDegree) maxDegree = degree;
            }

            var result = new int[maxDegree + 1];
            foreach (var kvp in coefficients)
            {
                result[kvp.Key] = kvp.Value;
            }

            return new Wielomian(result.Reverse().ToArray());
        }
    }
}
