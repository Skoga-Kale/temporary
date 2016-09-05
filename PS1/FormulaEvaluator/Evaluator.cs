using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;




namespace FormulaEvaluator
{
    /// <summary>
    /// This Class contaoins methods that can be used to evalute a String as a mathematical expression. 
    /// </summary>
    public class Evaluator
    {
        public delegate int Lookup(String v);
        /// <summary>
        /// This is the primary method in this class. Takes in an expression as a String and a delegate. The String is broken up into 
        /// individual tokens and then used in a 2 stack algorithm. 
        /// </summary>
        /// The String which will be evaluated.
        /// <param name="exp"></param>
        /// The Delegate that will be used to look up variable values if needed.
        /// <param name="variableEvaluator"></param>
        /// <returns></returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            int value;
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            // Creates the two stacks we will be working with in our algorithm.
            Stack valueStack = new Stack();
            Stack operatorStack = new Stack();
            // goes through the entire string expression passed in
            for (int i = 0; i < substrings.Length; i++)
            {
                substrings[i] = substrings[i].Trim();
                // Checks to see if item is a blank space
                if (substrings[i].Equals(""))
                {
                    // proceed to next item
                }
                // Checks to see if item is a value or a variable
                else if (isValue(substrings[i]) || isVariable(substrings[i]))
                {
                    // Converts String form of value or Variable into an actual number.
                    if (isVariable(substrings[i]))
                    {
                        value = variableEvaluator(substrings[i]);
                    }
                    else
                    {
                        Int32.TryParse(substrings[i], out value);
                    }
                    // Corner Case handling.
                    if (valueStack.Count == 0)
                    {
                        valueStack.Push(value);
                    }
                    // Checks to see if there is a operator with presedence.
                    // Multiplication
                    else if (operatorStack.Peek().Equals("*"))
                    {
                        operatorStack.Pop();
                        value = value * (int)valueStack.Pop();
                        valueStack.Push(value);
                    }
                    // Division
                    else if (operatorStack.Peek().Equals("/"))
                    {
                        operatorStack.Pop();
                        if (value == 0)
                        {
                            throw new DivideByZeroException("Cannot divide by 0");
                        }
                        value = (int)valueStack.Pop() / value;
                        valueStack.Push(value);
                    }
                    else
                    {
                        valueStack.Push(value);
                    }
                }
                // Addition or Subtraction
                else if (substrings[i].Equals("+") || substrings[i].Equals("-"))
                {
                    if (operatorStack.Count == 0) ;
                    else if (operatorStack.Peek().Equals("+"))
                    {
                        if (valueStack.Count < 2)
                        {
                            throw new ArgumentException("The expression was not entered correctly.");
                        }
                        operatorStack.Pop();
                        value = (int)valueStack.Pop() + (int)valueStack.Pop();
                        valueStack.Push(value);
                    }
                    else if (operatorStack.Peek().Equals("-"))
                    {
                        if (valueStack.Count < 2)
                        {
                            throw new ArgumentException("The expression was not entered correctly.");
                        }
                        operatorStack.Pop();
                        int minusValue1 = (int)valueStack.Pop();
                        int minusValue2 = (int)valueStack.Pop();
                        value = minusValue2 - minusValue1;
                        valueStack.Push(value);
                    }
                    operatorStack.Push(substrings[i]);


                }
                // Multiplication or Division
                else if (substrings[i].Equals("*") || substrings[i].Equals("/"))
                {
                    operatorStack.Push(substrings[i]);
                }
                else if (substrings[i].Equals("("))
                {
                    operatorStack.Push(substrings[i]);
                }
                else if (substrings[i].Equals(")"))
                {

                    if (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                    {
                        if (valueStack.Count < 2)
                        {
                            throw new ArgumentException("The expression entered was not formatted correctly");
                        }
                        else if (operatorStack.Peek().Equals("+"))
                        {
                            operatorStack.Pop();
                            value = (int)valueStack.Pop() + (int)valueStack.Pop();
                            valueStack.Push(value);
                        }
                        else if (operatorStack.Peek().Equals("-"))
                        {
                            operatorStack.Pop();
                            int minusValue1 = (int)valueStack.Pop();
                            int minusValue2 = (int)valueStack.Pop();
                            value = minusValue2 - minusValue1;
                            valueStack.Push(value);
                        }
                    }
                    operatorStack.Pop();
                    if (operatorStack.Count != 0 && (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/")))
                    {
                        if (valueStack.Count < 2)
                        {
                            throw new ArgumentException("The expression was not entered correctly.");
                        }
                        else if (operatorStack.Peek().Equals("*"))
                        {
                            operatorStack.Pop();
                            value = (int)valueStack.Pop() * (int)valueStack.Pop();
                            valueStack.Push(value);
                            operatorStack.Push(substrings[i]);
                        }
                        else if (operatorStack.Peek().Equals("/"))
                        {
                            operatorStack.Pop();
                            int divideValue1 = (int)valueStack.Pop();
                            int divideValue2 = (int)valueStack.Pop();
                            if (divideValue2 == 0)
                            {
                                throw new DivideByZeroException("Cannot divide by 0");
                            }
                            value = divideValue1 / divideValue2;
                            valueStack.Push(value);
                            operatorStack.Push(substrings[i]);
                        }

                    }
                }
                else
                {
                    throw new ArgumentException("An argument passed in does not meet criteria for a variable");
                }
            }

            if (operatorStack.Count == 0)
            {

                if (valueStack.Count != 1)
                {
                    throw new ArgumentException("The expression was not entered correctly.");
                } else
                {
                    return (int)valueStack.Pop();
                }
            } else if (operatorStack.Count != 0) {
                if (valueStack.Count != 2)
                {
                    throw new ArgumentException("The expression was not entered correctly.");
                }
                if (!(operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-")))
                {
                    throw new ArgumentException("The expression was not entered correctly.");
                } else if (operatorStack.Peek().Equals("+"))
                {
                    value = (int)valueStack.Pop() + (int)valueStack.Pop();
                    return value;
                }
                else if (operatorStack.Peek().Equals("-"))
                {
                    int minusValue1 = (int)valueStack.Pop();
                    int minusValue2 = (int)valueStack.Pop();
                    value = minusValue2 - minusValue1;
                    return value;
                }
            }
            return (int)valueStack.Pop();
        }
        
        /// <summary>
        /// Determines weather v can be interpreted as a number.
        /// </summary>
        /// <param name="v"></param> The string you want to determine weather is a value.
        /// <returns></returns>
        private static bool isValue(string v)
        {
            int testValue;
            if (Int32.TryParse(v, out testValue))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks to see weather one of the characters passed in is one of the operators accepted by this algorithim. 
        /// </summary>
        /// <param name="v"></param> The string you want to determine weather is a operator.
        /// <returns></returns> True if v is an operator, False if not.
        private static bool isOperator(string v)
        {
            if (v.Equals("+") || v.Equals("-") || v.Equals("*") || v.Equals("/") || v.Equals("(") || v.Equals(")"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// This function is used to determine weather one of the characters passed in is a variable. A variable can only be 
        /// something that starts with a letter and ends with a number.
        /// </summary>
        /// <param name="v"></param> The string that you want to determine weather is a variable.
        /// <returns></returns> True if v is a variable, False if not.
        private static bool isVariable(string v)
        {
            bool letter = false;
            bool number = false;
            for (int i = 0; i < v.Length; i++)
            {
                if (char.IsLetter(v[i]))
                {
                    if (number)
                    {
                        return false;
                    }
                    letter = true;
                } else if (char.IsNumber(v[i]))
                {
                    if (!letter)
                    {
                        return false;
                    }
                    number = true;
                }
            }
            if (letter && number)
            {
                return true;
            }
            return false;
        }
    }
}
