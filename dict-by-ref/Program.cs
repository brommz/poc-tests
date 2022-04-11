using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace dict_by_ref
{
    public class Program
    {
        static void Main(string[] args)
        {
            Debug.Assert(Vector.IsHardwareAccelerated);

            var s1 = "Ahmed";
            var s2 = "Ahmed";
            Console.WriteLine(s1 == s2);
            Console.WriteLine(Object.ReferenceEquals(s1, s2));


            var dict = new System.Collections.Generic.Dictionary<string, TestingStruct>()
            {
                { "k1", new TestingStruct(1) },
                { "k10", new TestingStruct(10) }
            };

            var value1 = dict["k1"];
            value1.Number = 100;
            System.Console.Out.WriteLine($"Normay way (1): {value1.Number == dict["k1"].Number}");

            dict.TryGetValue("k1", out var value1New);
            value1New.Number = 100;
            System.Console.Out.WriteLine($"Normay way (2): {value1New.Number == dict["k1"].Number}");

            ref var value1Ref = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, "k1", out _);
            value1Ref.Number = 200;
            value1Ref.Nested.X = 200;

            System.Console.Out.WriteLine($"With CollectionsMarshal: {value1Ref.Number == dict["k1"].Number}");
            System.Console.Out.WriteLine($"Value: {dict["k1"].Number}");
            System.Console.Out.WriteLine($"NestedValue: {dict["k1"].Nested.X}");
        }
    }

    public struct TestingStruct
    {
        public int Number;
        public Nested Nested;

        public TestingStruct(int num)
        {
            Number = num;
            Nested = new Nested()
            {
                X = num
            };
        }
    }

    public struct Nested
    {
        public int X;
    }
}
