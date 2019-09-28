using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FormulaEvaluator;
using System.Threading.Tasks;

namespace EvaluatorTest
{
    class Program
    {
        /// <summary>
        /// main to run test the algorithm.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            if (ValidTest("(a1 + 3)", testLookUp, 4))//checks variable plus 3.
            {
                Console.WriteLine("correct");
            }
            else
                Console.WriteLine("failed");

            if (ValidTest("(a8 + a7)/2", testLookUp1, 3))//checks variable division/
            {
                Console.WriteLine("correct");
            }
            else
                Console.WriteLine("failed");

            if (ValidTest("((a8 + 3 + 9 * 2)/2)/2", testLookUp1, 6))//checks variable addition and division.
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");

            if (InValid("1/0", testLookUp1))//checks divide by zero.
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");
            if (InValid("a8/0", testLookUp1))//variable divided by zero.
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");
            if (ValidTest("1-8", testLookUp1, -7))//checks for negative value.
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");

            if (InValid("-1-8", testLookUp1))//checks with negative infront.
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");
            if (InValid("1--8", testLookUp1))//checks with two negatives.
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");
            if (ValidTest("a8-8", testLookUp1, -5))//variable negation.
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");
            if (InValid("a8--8", testLookUp1))
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");
            if (InValid("-a8--8", testLookUp1))
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");
            if (ValidTest("(2 + 5) + (a8 + 10) - (5*7) + (100*500)/2", testLookUp1, 24985))//complex equation.
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");
            if (ValidTest("((((a8)))) - ((((a8))))", testLookUp1, 0))//with more brackets.
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");
            if (InValid("999 9999", testLookUp1))
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");
            if (ValidTest("2/1", testLookUp1, 2))
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");
            if (ValidTest("1/2", testLookUp1, 0))
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");
            if (ValidTest("a8/4", testLookUp1, 0))
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");
            if (ValidTest("a8/1", testLookUp1, 3))
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");
            if (InValid("(1-)", testLookUp1))
            {
                Console.WriteLine("Correct");
            }
            else
                Console.WriteLine("Failed");





            Console.Read();
        }
        /// <summary>
        /// Valid tests for valid expression
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="L"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static bool ValidTest(string expr, Evaluator.Lookup L, int expected)
        {
            try
            {
                int a = Evaluator.Evaluate(expr, L);
                if (Evaluator.Evaluate(expr, L) == expected)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine(a);
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }



        }
        /// <summary>
        /// methods for invalid expressions.
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="L"></param>
        /// <returns></returns>
        public static bool InValid(string exp, Evaluator.Lookup L)
        {
            try
            {
                Evaluator.Evaluate(exp, L);
                return false;
            }
            catch (ArgumentException)
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        /// <summary>
        ///lookups for 1.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int testLookUp(string a)
        {
            return 1;
        }
        public static int testLookUp1(string a)
        {
            return 3;
        }
        public static int testLookUp2(string a)
        {
            return 1;
        }
    }
}
