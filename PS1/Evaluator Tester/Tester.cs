using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluator_Tester
{
    class Tester
    {
        public static Dictionary<string, int> testDictionary = new Dictionary<string, int>();
        static void Main(string[] args)
        {
            testDictionary.Add("B1B1", 0);
            testDictionary.Add("CC123", 4);
            int answer = FormulaEvaluator.Evaluator.Evaluate("5 + 3 * 7 - 8 / (CC123 + 3) - 2 / 2", TestDelegate);
            Console.WriteLine(answer);
            Console.Read();
        }

        public static int TestDelegate(String v)
        {
            int value = testDictionary[v];
            return value;
        }
    }
}
