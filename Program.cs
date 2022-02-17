using System.Runtime.InteropServices;

namespace dict_by_ref
{
    public class Program
    {
        static void Main(string[] args)
        {
            var dict = new System.Collections.Generic.Dictionary<string, TestingStruct>()
            {
                { "k1", new TestingStruct(1) },
                { "k10", new TestingStruct(10) }
            };

            var value1 = dict["k1"];
            value1.Number = 100;

            System.Console.Out.WriteLine($"Normay way: {value1.Number == dict["k1"].Number}");

            ref var value1Ref = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, "k1", out _);
            value1Ref.Number = 200;

            System.Console.Out.WriteLine($"With CollectionsMarshal: {value1Ref.Number == dict["k1"].Number}");
            System.Console.Out.WriteLine($"Value: {dict["k1"].Number}");
        }
    }

    public struct TestingStruct
    {
        public int Number;
        public TestingStruct(int num)
        {
            Number = num;
        }
    }
}
