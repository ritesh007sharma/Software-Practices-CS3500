using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator
{
  
    public class Evaluator
    {
        public delegate int Lookup(String v);

        /// <summary>
        /// This Method is used to compute the mathematical value of the spreadsheet. It uses two different stacks 
        /// value and a operator stack to compute the value.
        /// </summary>
        /// <param string="exp"></param>
        /// <param Lookup="variableEvaluator"></param>
        /// <returns>Computed Integer</returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            
            Stack<int> value = new Stack<int>();
            Stack<string> oper = new Stack<string>();

            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            //for loop to loop through the string.
            for (int i = 0; i < substrings.Length; i++)
            {
                String removedWhiteSpace = substrings[i].Trim();

                int j = 0;
                bool parsed = false;
                //try parsed returns a boolean and an parsed value, if the value get parsed.
                parsed = int.TryParse(removedWhiteSpace, out j);
                if (parsed && j >= 0)
                {
                    //if the count is not zero and peek is equal to * and / it enters the if statement or else pushes the value 
                    //in value stack.
                    if (!oper.Count().Equals(0) && (oper.Peek().Equals("*") || oper.Peek().Equals("/")))
                    {
                        int usingOperator;
                        if (value.Count().Equals(0))
                        {
                            throw new ArgumentException();
                        }
                        int first = value.Pop();
                        if (oper.Peek().Equals("*"))
                        {
                            oper.Pop();
                            usingOperator = j * first;
                            value.Push(usingOperator);
                        }
                        else if (oper.Peek().Equals("/"))
                        {
                            if (j == 0)
                            {
                                throw new ArgumentException();

                            }
                            oper.Pop();
                            usingOperator = first / j;
                            value.Push(usingOperator);
                        }



                    }
                    else
                    {
                        value.Push(j);
                    }
                }
                else
                {
                    //checks for whitespace after trimming.
                    if (!removedWhiteSpace.Equals(""))
                    {
                        //regex to check if starts with letter and ends with digit.
                        bool match = Regex.IsMatch(removedWhiteSpace, "(^[a-zA-Z]+[0-9]+$)");

                        if (match)
                        {
                            int lookUp = variableEvaluator(removedWhiteSpace);
                            //this is similar to integer instead deals with variable.
                            if (!oper.Count().Equals(0) && (oper.Peek().Equals("*") || oper.Peek().Equals("/")))
                            {
                                int usingOperatorforLookup;
                                if (value.Count().Equals(0))
                                {
                                    throw new ArgumentException();
                                }
                                int first = value.Pop();
                                if (oper.Peek().Equals("*"))//if peek is * pops it and uses * operator to compute the value.
                                {
                                    oper.Pop();
                                    usingOperatorforLookup = lookUp * first;
                                    value.Push(usingOperatorforLookup);
                                }
                                else if (oper.Peek().Equals("/"))
                                {
                                    if (lookUp == 0)
                                    {
                                        throw new ArgumentException();
                                    }
                                    oper.Pop();
                                    usingOperatorforLookup = first / lookUp;
                                    value.Push(usingOperatorforLookup);
                                }
                            }
                            else
                                value.Push(lookUp);

                        }

                        //if the removed whitespace is + and - enters the loop and runs the algorithm.
                        else if (removedWhiteSpace.Equals("+") || removedWhiteSpace.Equals("-"))
                        {
                            if (!oper.Count.Equals(0) && (oper.Peek().Equals("+") || oper.Peek().Equals("-")))
                            {
                                if (value.Count() < 2)
                                {
                                    throw new ArgumentException();
                                }
                                int first = value.Pop();
                                int second = value.Pop();
                                int addOrSubsValue = 0;
                                if (oper.Peek().Equals("+"))
                                {
                                    oper.Pop();
                                    addOrSubsValue = first + second;
                                    value.Push(addOrSubsValue);

                                }
                                else if (oper.Peek().Equals("-"))
                                {
                                    oper.Pop();
                                    addOrSubsValue = second - first;
                                    value.Push(addOrSubsValue);

                                }
                            }

                            oper.Push(removedWhiteSpace);
                        }
                        
                        else if (removedWhiteSpace.Equals("*") || removedWhiteSpace.Equals("/"))
                        {
                            oper.Push(removedWhiteSpace);
                        }
                        else if (removedWhiteSpace.Equals("("))
                        {
                            oper.Push(removedWhiteSpace);
                        }
                        //if ) then enters the loop and runs the algorithm.
                        else if (removedWhiteSpace.Equals(")"))
                        {
                            if (!oper.Count().Equals(0) && (oper.Peek().Equals("+") || oper.Peek().Equals("-")))
                            {
                                //if the value count is less than 2 throws an exception.
                                if (value.Count() < 2)
                                {
                                    throw new ArgumentException();
                                }
                                int valFirst = value.Pop();
                                int valSec = value.Pop();
                                int total = 0;

                                if (!oper.Count.Equals(0) && oper.Peek().Equals("+"))
                                {
                                    oper.Pop();
                                    total = valFirst + valSec;
                                    value.Push(total);

                                }
                                else if (!oper.Count().Equals(0) && oper.Peek().Equals("-"))
                                {
                                    oper.Pop();
                                    total = valSec - valFirst;
                                    value.Push(total);
                                }
                            }
                            if (!oper.Count.Equals(0) && oper.Peek().Equals("("))
                            {

                                oper.Pop();
                            }
                            else
                            {
                                throw new ArgumentException();
                            }

                            if (!oper.Count().Equals(0) && (oper.Peek().Equals("*") || oper.Peek().Equals("/")))
                            {
                                //if value stack count is less than 2 throws an exception.
                                if (value.Count() < 2)
                                {
                                    throw new ArgumentException();
                                }
                                int finFValue = value.Pop();
                                int finSValue = value.Pop();
                                int total = 0;
                                if (oper.Peek().Equals("*"))
                                {
                                    oper.Pop();
                                    total = finFValue * finSValue;
                                    value.Push(total);

                                }
                                else if (oper.Peek().Equals("/"))
                                {
                                    if (finSValue == 0)
                                    {
                                        throw new ArgumentException();
                                    }
                                    oper.Pop();
                                    total = finFValue / finSValue;
                                    value.Push(total);
                                }
                            }
                        }
                    }
                }
            }
            
            int totalValue = 0;
            //if oper count is 0 and valur count is 1 throws an exception if not pops the value from the operator stack and
            //returns it.
            if (oper.Count().Equals(0))
            {
                if (!value.Count().Equals(1))
                {
                    throw new ArgumentException();
                }
                return value.Pop();
            }
            //if oper count is 1 and value count is 2, pops boths values and uses the operator.
            else if (oper.Count.Equals(1) && value.Count.Equals(2))
            {
                int firstValue = value.Pop();
                int secondValue = value.Pop();

                if (oper.Peek().Equals("+"))
                {
                    oper.Pop();
                    totalValue = firstValue + secondValue;
                }
                else if (oper.Peek().Equals("-"))
                {
                    oper.Pop();
                    totalValue = secondValue - firstValue;
                }
                else
                    throw new ArgumentException();//throws an exception other wise.
            }
            else
            {
                throw new ArgumentException();
            }
            return totalValue;
        }
    }
}

