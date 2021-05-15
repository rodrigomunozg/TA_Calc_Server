/*
 Developed by Rodrigo Muñoz for Rockstar Games interview process. May2021.
 */
using System;
using System.Collections.Generic;

namespace TA_Calc_Server
{
    /// <summary>
    /// This class receives the mathematical operation from the ServerConnMgr, operates it and send the result to the ServerConnMgr.
    /// </summary>
    class ServerCalc
    {
        double numA;        //First number of the mathematic operation
        double numB;        //Second number of the mathematic operation (if necessary)
        double numMem;      //Allows to store temporarily the result of the mathematical operation in case it needs to be stored in memory
        List<double> mem;   //This list represents and emules the memory of the calculator.
        string[] numbers;   //Array that store the numbers after making a split (by the mathematical operator)

        public ServerCalc()
        {
            mem = new List<double>();
        }

        /// <summary>
        /// This function receives the mathematical operation in string format. This string is splitted by the mathematical operator and each number is stored in a position of the numbers array.
        /// The mathematical operation is defined by detecting the symbol in the string. Once detected, the operation is solved, the result stored and returned.
        /// In the case of the memory operations (M+, M-, AvgMem and MemClear) here is not mathematical symbol involved but a specific string that triggers the correspondent operation.
        /// </summary>
        /// <param name="mathOp">The matematical operation.</param>
        /// <returns>
        /// The result of the mathematical operation.
        /// </returns>
        public double evaluateMathOperator(string mathOp)
        {
            if (mathOp.Contains("+"))       //addition
            {
                numbers = mathOp.Split("+");
                numA = Convert.ToDouble(numbers[0]);
                numB = Convert.ToDouble(numbers[1].Replace("<EOF>", ""));
                Console.WriteLine("First number: " + numA);
                Console.WriteLine("Second number: " + numB);
                Console.WriteLine("Result to send: " + (numA + numB));
                numMem = numA + numB;
                return numMem;
            }
            else if (mathOp.Contains("-"))  //substraction
            {
                numbers = mathOp.Split("-");
                numA = Convert.ToDouble(numbers[0]);
                numB = Convert.ToDouble(numbers[1].Replace("<EOF>", ""));
                Console.WriteLine("First number: " + numA);
                Console.WriteLine("Second number: " + numB);
                Console.WriteLine("Result to send: " + (numA - numB));
                numMem = numA - numB;
                return numMem;
            }
            else if (mathOp.Contains("*"))      //multiplication
            {
                numbers = mathOp.Split("*");
                numA = Convert.ToDouble(numbers[0]);
                numB = Convert.ToDouble(numbers[1].Replace("<EOF>", ""));
                Console.WriteLine("First number: " + numA);
                Console.WriteLine("Second number: " + numB);
                Console.WriteLine("Result to send: " + (numA * numB));
                numMem = numA * numB;
                return numMem;
            }
            else if (mathOp.Contains("/"))      //division
            {
                numbers = mathOp.Split("/");
                numA = Convert.ToDouble(numbers[0]);
                numB = Convert.ToDouble(numbers[1].Replace("<EOF>", ""));
                Console.WriteLine("First number: " + numA);
                Console.WriteLine("Second number: " + numB);
                Console.WriteLine("Result to send: " + (numA / numB));
                numMem = numA / numB;
                return numMem;
            }
            else if (mathOp.Contains("^"))      //exponentiation
            {
                numbers = mathOp.Split("^");
                numA = Convert.ToDouble(numbers[0]);
                numB = Convert.ToDouble(numbers[1].Replace("<EOF>", ""));
                Console.WriteLine("Base: " + numA);
                Console.WriteLine("Exponent: " + numB);
                Console.WriteLine("Result to send: " + Math.Pow(numA, numB));
                numMem = Math.Pow(numA, numB);
                return numMem;
            }
            else if (mathOp.Contains("Sqrt("))  //square root
            {
                string num = mathOp.Replace("Sqrt(", "");
                num = num.Replace(")<EOF>", "");
                numA = Convert.ToDouble(num);
                Console.WriteLine("Number to sqrt: " + numA);
                Console.WriteLine("Result to send: " + Math.Sqrt(numA));
                numMem = Math.Sqrt(numA);
                return numMem;
            }
            else if (mathOp == "Result added to memory<EOF>")   //Add result to memory
            {
                mem.Add(numMem);
                Console.WriteLine("result added to memory: " + numMem);
                foreach (double number in mem)
                { Console.WriteLine("results stored: " + number); }
                return 0;
            }
            else if (mathOp == "Last result removed from memory<EOF>")  //remove from the memory the last result stored 
            {
                Console.WriteLine("result removed from memory: " + mem[mem.Count - 1]);
                mem.RemoveAt(mem.Count - 1);
                foreach (double number in mem)
                { Console.WriteLine("results stored: " + number); }
                return 0;
            }
            else if (mathOp == "Memory cleared<EOF>")   //Remove all results of the memory
            {
                Console.WriteLine("Clearing Memory");
                mem.Clear();
                foreach (double number in mem)
                { Console.WriteLine("results stored: " + number.ToString()); }
                return 0;
            }
            else if (mathOp == "Calculating average of memory<EOF>")    //Calculate the average of all the results stored in the memory
            {
                double avg = 0;
                if (mem.Count > 0)
                {
                    Console.WriteLine("calculating average of results stored");
                    double count = 0;
                    foreach (double number in mem)
                    { Console.WriteLine("results stored: " + number); }
                    foreach (double number in mem)
                    { count += number; }
                    avg = count / mem.Count;
                    Console.WriteLine("result to send: " + avg);
                }
                else { Console.WriteLine("No results on memory, result to send: " + avg); }
                return avg;
            }
            return 0;
        }
    }
}
