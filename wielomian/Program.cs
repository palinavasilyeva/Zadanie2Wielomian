using System;
using MyMath;
using W = MyMath.Wielomian;
using MyExtensions;
using System.Collections.Generic;

namespace wielomian
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("== Test implementacji klasy Wielomian! ==");

            Console.WriteLine("== Konstrukcja ==");

            Wielomian w0 = new Wielomian();
            Console.WriteLine($"wielomian domyślny W() = {w0}, stopień = {w0.Stopien}");

            Wielomian w01 = new W(-1);
            Console.WriteLine($"W(-1) = {w01}, stopień = {w01.Stopien}");

            Wielomian w02 = new W(0);
            Console.WriteLine($"W(0) = {w02}, stopień = {w02.Stopien}");

            Wielomian w03 = new W(0, 0);
            Console.WriteLine($"W(0,0) = {w03}, stopień = {w03.Stopien}");

            var w11 = new Wielomian(1, 0);
            Console.WriteLine($"W(1,0) = {w11}, stopień = {w11.Stopien}");

            var w12 = new Wielomian(1, -1);
            Console.WriteLine($"W(1,-1) = {w12}, stopień = {w12.Stopien}");

            var w3 = new W(-2, 0, 1, -3);
            Console.WriteLine($"W(-2, 0, 1, -3) = {w3}, stopień = {w3.Stopien}");

            var w31 = new W(0, 0, 0, -2, 0, 1, -3);
            Console.WriteLine($"W(0, 0, 0, -2, 0, 1, -3) = {w31}, stopień = {w31.Stopien}");

            int[] wsp = { 3, 1, 3, 0, -2, 4, 5, 1, 0, 0 };
            var wX = new Wielomian(wsp);
            Console.WriteLine($"wielomian z tablicy = {wX}, stopień = {wX.Stopien}");

            try
            {
                new Wielomian(null);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine($"W( null ) --> NullReferenceException");
            }

            try
            {
                new Wielomian(new int[0]);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"W( int[0] ) --> ArgumentException: {e.Message}");
            }

            Console.WriteLine("== Równość, nierówność ==");

            Console.WriteLine($"W(1, 2).Equals(W(1, 2)) : {(new W(1, 2)).Equals(new W(1, 2))}");
            Console.WriteLine($"wX.Equals(wX) : {wX.Equals(wX)}");
            Console.WriteLine($"wX.Equals(null) : {wX.Equals(null)}");
            Console.WriteLine($"wX.Equals(Object) : {wX.Equals(new Object())}");
            Console.WriteLine($"W(1, 2) == W(1, 2) : {new W(1, 2) == new W(1, 2)}");
            Console.WriteLine($"W(1, 2) == W(2, 1) : {new W(1, 2) == new W(2, 1)}");
            Console.WriteLine($"W(1, 2) != W(2, 1) : {new W(1, 2) != new W(2, 1)}");
            Console.WriteLine($"W(1, 2) == W(0, 1, 2) : {new W(1, 2) == new W(0, 1, 2)}");
            Console.WriteLine($"W(1, 2) != W(0, 1, 2) : {new W(1, 2) != new W(0, 1, 2)}");

            Console.WriteLine("== Operacje arytmetyczne ==");

            var w21 = new W(1, 2, 3);
            var w22 = new W(0, 1, 2);
            Console.WriteLine($"({w21}) + ({w22}) = {w21 + w22}");
            Console.WriteLine($"({w22}) + ({w21}) = {w22 + w21}");
            Console.WriteLine($"({w21}) - ({w22}) = {w21 - w22}");
            Console.WriteLine($"({w22}) - ({w21}) = {w22 - w21}");

            Console.WriteLine($"({w21}) + {10} = {w21 + 10}");
            Console.WriteLine($"{10} + ({w21}) = {10 + w21}");
            Console.WriteLine($"({w21}) - {10} = {w21 - 10}");
            Console.WriteLine($"{10} - ({w21}) = {10 - w21}");

            Console.WriteLine("== Konwersje ==");

            Wielomian w = 10;
            int[] t = (int[])(new W(1));
            Console.WriteLine($"int[{t.Length}]: [{String.Join(',', t)}]");

            int y = (int)(new W(1));
            Console.WriteLine($"int y: {y}");

            int[] t11 = (int[])(new W(1, 2));
            Console.WriteLine($"int[{t11.Length}]: [{String.Join(',', t11)}]");

            try
            {
                int t12 = (int)(new W(1, 2));
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine($"InvalidCastException: {e.Message}");
            }

            Console.WriteLine("== indexer ==");

            Console.WriteLine("W(1,2,3): ");
            for (int i = 0; i <= w21.Stopien; i++)
                Console.WriteLine($"  w{i} = {w21[i]}");

            Console.WriteLine("== enumerator ==");

            Console.Write("W(1,2,3): ");
            foreach (var x in w21)
                Console.Write($"{x} ");
            Console.WriteLine();

            Console.WriteLine("== konstruktor dla string ==");

            Wielomian wS = Wielomian.Parse("3x^2 - 2x^1 + 1");
            Console.WriteLine("Wielomian.Parse(\"3x^2 - 2x^1 + 1\") = {0}", wS);
            Console.WriteLine($"W(3, -2, 1) == Wielomian.Parse(\"3x^2 - 2x^1 + 1\"): {new Wielomian(3, -2, 1) == wS}");

            Console.WriteLine("== Metoda rozszerzająca Eval ==");

            Console.WriteLine($"Metoda rozszerzająca: new W(1,2,1).Eval(2.0) = {new W(1, 2, 1).Eval(2.0)}");

            Console.WriteLine("== lista wielomianów, sortowanie ==");

            var lista = new List<W>
            {
                new W(),
                new W(-2,1,2),
                new W(2,1),
                new W(-2,1),
                new W(-1)
            };
            Console.WriteLine("- lista przed sortowaniem -");
            lista.ForEach( ww => Console.WriteLine(ww) );

            lista.Sort( MyMathExtensions.WielomianPoprzedzaComparison );
            Console.WriteLine("- lista po sortowaniu -");
            lista.ForEach(ww => Console.WriteLine(ww));

            System.Console.WriteLine("== koniec testu ==");
        }
    }
}