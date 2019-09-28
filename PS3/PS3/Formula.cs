// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        /// 

        private string normalizer;
        private bool validChecker = false;
        private string[] formulaToEvaluate;
        private List<string> variableCount;
        int leftParen = 0;
        int rightParen = 0;

        public Formula(String formula) :
            this(formula, s => s, s => true)

        { }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {

            variableCount = new List<string>();
            //of the formula does not hava a token throws an exception.
            if (!oneTokenRule(formula))
            {
                throw new FormulaFormatException("empty formula");
            }
         
            //splits token and adds everything to a formulaTEvaluate
            formulaToEvaluate = GetTokens(formula).ToArray();
            if(formulaToEvaluate.Count() == 2) {
                foreach (string oper in formulaToEvaluate)
                {
                    if (oper != "+" || oper != "-" || oper != "*" || oper != "/")
                    {
                        throw new FormulaFormatException("No operators");
                    }

                }
                
            }

            if (!startTokenRule(formulaToEvaluate[0]))
            {
                throw new FormulaFormatException("start token is wrong");
            }
          

            else {

              


                bool match = Regex.IsMatch(formulaToEvaluate[0], @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*");

                if (formulaToEvaluate[0].Equals("("))
                {
                    leftParen++;//adding onto leftparen.
                }

                if (match)
                {

                    normalizer = normalize(formulaToEvaluate[0]);

                    //checking if nomalizer is valid and storing it to variableCount.
                    if (isValid(normalizer))
                    {
                        formulaToEvaluate[0] = normalizer;

                        if (!variableCount.Contains(formulaToEvaluate[0]))
                        {
                            variableCount.Add(formulaToEvaluate[0]);
                        }
                    }
                    else
                    {
                        throw new FormulaFormatException("invalid");
                    }

                }
           

            }


            //starting from 1 and only looping through 2 less since we check for startin and
            //ending at the very beginning.
            for (int i = 1; i < formulaToEvaluate.Count() - 1; i++)
            {


                if (parsing(formulaToEvaluate[i]))
                {
                    bool match = Regex.IsMatch(formulaToEvaluate[i], @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*");

                    if (match)
                    {


                        normalizer = normalize(formulaToEvaluate[i]);

                        if (isValid(normalizer))
                        {
                            formulaToEvaluate[i] = normalizer;
                            if (!variableCount.Contains(formulaToEvaluate[i]))
                            {
                                variableCount.Add(formulaToEvaluate[i]);
                            }
                        }
                        else
                        {
                            throw new FormulaFormatException("invalid");
                        }

                    }

                    //if conditions to check if the formula is balanced.
                    if (formulaToEvaluate[i].Equals("("))
                    {
                        leftParen++;
                    }
                    if (formulaToEvaluate[i].Equals(")"))
                    {
                        rightParen++;
                    }
                    //throwing exceptions if different method for different rules fail.
                    if (!rightParenRule(formulaToEvaluate[i]))
                    {
                        throw new FormulaFormatException("extra right parameters");
                    }

                    if (!parenFollowingRule(formulaToEvaluate[i], formulaToEvaluate[i + 1]))
                    {
                        throw new FormulaFormatException("wrong operators and operands.");
                    }
                   
                    if (!extraParenRule(formulaToEvaluate[i], formulaToEvaluate[i + 1]))
                    {
                            throw new FormulaFormatException("wrong operators and operands.");
                    }
                

                }

            }
            if (!endingTokenRule(formulaToEvaluate[formulaToEvaluate.Length - 1]))
            {
                throw new FormulaFormatException("ending token wrong");
            }
            else 
            {
                bool match = Regex.IsMatch(formulaToEvaluate[formulaToEvaluate.Length - 1], @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*");
                if (formulaToEvaluate[formulaToEvaluate.Length - 1].Equals(")"))
                {
                    rightParen++;
                }

                if (match)
                {



                    normalizer = normalize(formulaToEvaluate[formulaToEvaluate.Length - 1]);

                    if (isValid(normalizer))
                    {
                        formulaToEvaluate[formulaToEvaluate.Length - 1] = normalizer;
                        if (!variableCount.Contains(formulaToEvaluate[formulaToEvaluate.Length - 1]))
                        {
                            variableCount.Add(formulaToEvaluate[formulaToEvaluate.Length - 1]);
                        }
                    }
                    else
                    {
                        throw new FormulaFormatException("invalid");
                    }

                }
             

            }

            if (rightParen != leftParen)
            {
                throw new FormulaFormatException("parens doesnt match");
            }

        }


        /// <summary>
        /// Takes in string formula uses regex, parse double.
        /// </summary>
        /// <param name="formula"></param>
        /// <returns>true if string formula equals (,)+-*or/ false otherwise.</returns>
        public bool parsing(string formula)
        {

            bool match = Regex.IsMatch(formula, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*");
            double parsedDouble = 0.0;
            bool parsed = double.TryParse(formula, out parsedDouble);

            //checking if formulas are valid tokens.
            if (formula.Equals("(") || formula.Equals(")") || formula.Equals("+") || formula.Equals("-") || formula.Equals("*") ||
                    formula.Equals("/") || (parsed == true) || (match == true))
            {
                return true;
            }

            return true;

        }


        /// <summary>
        /// takes in string formula, gets the token out of formula loops through it.
        /// </summary>
        /// <param name="formula"></param>
        /// <returns>true if not empty false otherwise.</returns>
        public bool oneTokenRule(string formula)
        {

            formulaToEvaluate = GetTokens(formula).ToArray();
            if (formulaToEvaluate.Count().Equals(0))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if number of right paren is greater than number of left paren.
        /// </summary>
        /// <param name="formula"></param>
        /// <returns>boolean</returns>
        public bool rightParenRule(string formula)
        {
            if (rightParen > leftParen)
            {
                return false;
            }
            return true;
        }



        /// <summary>
        /// Start token rule
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public bool startTokenRule(string formula)
        {
            double parsedFirstDouble = 0.0;
            bool parsedFirst = double.TryParse(formula, out parsedFirstDouble);
            bool parsedVariable = Regex.IsMatch(formula, @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*");

            //checking if the starting token matches.
            if (parsedFirst == true || parsedVariable == true || formula.Equals("("))
            {
              
                return true;
            }

            return false;
        }

        /// <summary>
        /// takes a string and checks the ending token of that string. 
        /// </summary>
        /// <param name="formula"></param>
        /// <returns>Boolean if ending token is Valid</returns>
        public bool endingTokenRule(string formula)
        {
            double parsedFirstDouble = 0.0;
            bool parsedFirst = double.TryParse(formula, out parsedFirstDouble);
            bool parsedVariable = Regex.IsMatch(formula, @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*");
            //checking if the ending token matches.
            if (parsedFirst == true || parsedVariable == true || formula.Equals(")"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Verifies if the operator follows number, variable or "("
        /// </summary>
        /// <param name="currentToken"></param>
        /// <param name="nextToken"></param>
        /// <returns>true if it does false otherwise</returns>
        public bool parenFollowingRule(string currentToken, string nextToken)
        {
            double a;
            bool isNumber = true;
            bool isVariable = true;
            isNumber = Double.TryParse(nextToken, out a);
            isVariable = Regex.IsMatch(nextToken, @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*");

            //checking id the parenthesis rule is followed.
            if (currentToken.Equals("(") || currentToken.Equals("+") || currentToken.Equals("-") || currentToken.Equals("*") || currentToken.Equals("/"))
            {
                if (!isNumber && !isVariable && !nextToken.Equals("("))
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Verifies weather the number, variable or ")" follows the required token.
        /// </summary>
        /// <param name="currentToken"></param>
        /// <param name="nextToken"></param>
        /// <returns>booleank</returns>
        public bool extraParenRule(string currentToken, string nextToken)
        {


            double a;
            bool isNumber = true;
            bool isVariable = true;

            isNumber = Double.TryParse(currentToken, out a);
            isVariable = Regex.IsMatch(currentToken, @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*");

            //checking ig the extra paren rule is followed.
            if (isNumber || isVariable || currentToken.Equals(")"))
            {
                if (!nextToken.Equals("+") && !nextToken.Equals("-") && !nextToken.Equals("*") && !nextToken.Equals("/") &&
                    !nextToken.Equals(")"))
                {
                    return false;
                }
            }

            return true;
        }



        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {

            string exp = this.ToString();

            Stack<double> value = new Stack<double>();
            Stack<string> oper = new Stack<string>();

            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            //for loop to loop through the string.
            for (int i = 0; i < substrings.Length; i++)
            {
                string removedWhiteSpace = substrings[i].Trim();

                double j = 0.0;
                bool parsed = false;
                //try parsed returns a boolean and an parsed value, if the value get parsed.
                parsed = double.TryParse(removedWhiteSpace, out j);
                if (parsed && j >= 0)
                {
                    //if the count is not zero and peek is equal to * and / it enters the if statement or else pushes the value 
                    //in value stack.
                    if (!oper.Count().Equals(0) && (oper.Peek().Equals("*") || oper.Peek().Equals("/")))
                    {
                        double usingOperator;
                        if (value.Count().Equals(0))
                        {
                            return new FormulaError("No operands");
                        }
                        double first = value.Pop();
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
                                return new FormulaError("Division point error");

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
                            try
                            {
                                double lookUp = lookup(removedWhiteSpace);
                                //this is similar to integer instead deals with variable.
                                if (!oper.Count().Equals(0) && (oper.Peek().Equals("*") || oper.Peek().Equals("/")))
                                {
                                    double usingOperatorforLookup;
                                    if (value.Count().Equals(0))
                                    {
                                        return new FormulaError("No valid value");
                                    }
                                    double first = value.Pop();
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
                                            return new FormulaError("Invalid request check divisor");
                                        }
                                        oper.Pop();
                                        usingOperatorforLookup = first / lookUp;
                                        value.Push(usingOperatorforLookup);
                                    }
                                }
                                else
                                    value.Push(lookUp);
                            }
                            catch (ArgumentException)
                            {
                                return new FormulaError("Delegate");
                            }
                        }

                        //if the removed whitespace is + and - enters the loop and runs the algorithm.
                        else if (removedWhiteSpace.Equals("+") || removedWhiteSpace.Equals("-"))
                        {
                            if (!oper.Count.Equals(0) && (oper.Peek().Equals("+") || oper.Peek().Equals("-")))
                            {
                                if (value.Count() < 2)
                                {
                                    return new FormulaError("Error(valueCount is more)");

                                }
                                double first = value.Pop();
                                double second = value.Pop();
                                double addOrSubsValue = 0;
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
                                    return new FormulaError("Value count error");
                                }
                                double valFirst = value.Pop();
                                double valSec = value.Pop();
                                double total = 0;

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
                                return new FormulaError("Invalid formula");
                            }

                            if (!oper.Count().Equals(0) && (oper.Peek().Equals("*") || oper.Peek().Equals("/")))
                            {
                                //if value stack count is less than 2 throws an exception.
                                if (value.Count() < 2)
                                {
                                    return new FormulaError("value count error");
                                }
                                double finFValue = value.Pop();
                                double finSValue = value.Pop();
                                double total = 0;
                                if (oper.Peek().Equals("*"))
                                {
                                    oper.Pop();
                                    total = finFValue * finSValue;
                                    value.Push(total);

                                }
                                else if (oper.Peek().Equals("/"))
                                {
                                    if (finFValue == 0)
                                    {
                                        return new FormulaError("No operator");
                                    }
                                    oper.Pop();
                                    total = finSValue / finFValue;
                                    value.Push(total);
                                }
                            }
                        }
                    }
                }
            }

            double totalValue = 0;
            //if oper count is 0 and valur count is 1 throws an exception if not pops the value from the operator stack and
            //returns it.
            if (oper.Count().Equals(0))
            {
                if (!value.Count().Equals(1))
                {
                    return new FormulaError("Invalid value Count");
                }
                return value.Pop();
            }
            //if oper count is 1 and value count is 2, pops boths values and uses the operator.
            else if (oper.Count.Equals(1) && value.Count.Equals(2))
            {
                double firstValue = value.Pop();
                double secondValue = value.Pop();

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
                    return new FormulaError("No operator");
            }
            else
            {
                return new FormulaError("No operator/operands");
            }
            return totalValue;
        }






        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            
          
            return variableCount;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {

            string concat = "";
            for (int i = 0; i < formulaToEvaluate.Count(); i++)
            {
                //string f = formulaToEvaluate[i].Trim();
                //concatinating strings.
               if( double.TryParse(formulaToEvaluate[i], out double formulaDouble))
                {
                    formulaToEvaluate[i] = formulaDouble + "";
                        
                }

                concat = concat + formulaToEvaluate[i];
            }

            return concat;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Formula))
            {
                return false;
            }

            Formula a = (Formula)obj;

            string[] tokens = GetTokens(a.ToString()).ToArray();

            for (int i = 0; i < tokens.Length; i++)
            {

                //if token is a number parse it first and change it to string before comparing.
                if (!tokens[i].Equals(formulaToEvaluate[i]))
                {
                    double parsedFirstDouble1 = 0.0;
                    bool parsedFirst = double.TryParse(tokens[i], out parsedFirstDouble1);

                    double parsedFirstDouble2 = 0.0;
                    bool parsedSecond = double.TryParse(formulaToEvaluate[i], out parsedFirstDouble2);

                    //checking if parsed first and parsed second is true.
                    if (parsedFirst && parsedSecond)
                    {
                        string first = parsedFirstDouble1.ToString();
                        string second = parsedFirstDouble2.ToString();

                        if (first != second)
                        {
                            return false;
                        }
                       
                    }

                    else
                        return false;
                }




            }



            return true;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            //using reference since using == will result to an stack overflow.
            if (ReferenceEquals(f1, null) && ReferenceEquals(f2, null))
            {
                return true;
            }
            if ((ReferenceEquals(f1, null) && !ReferenceEquals(f2, null)) || (!ReferenceEquals(f1, null) && ReferenceEquals(f2, null)))
            {
                return false;
            }
            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            if (ReferenceEquals(f1, null) && ReferenceEquals(f2, null))
            {
                return false;
            }
            if ((ReferenceEquals(f1, null) && !ReferenceEquals(f2, null)) || (!ReferenceEquals(f1, null) && ReferenceEquals(f2, null)))
            {
                return true;
            }
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {



            return this.ToString().GetHashCode(); ;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }

}