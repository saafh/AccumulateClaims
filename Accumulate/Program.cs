using System;
using System.Collections.Generic;
using Tower.Watson.ClaimsReserving;

/// <summary>
/// In this project I am using CSV file (as an input) to parse for Products, OriginYear, 
/// DevelopmentYear and IncrementalValue of the claims.
/// 
/// I am calculating the amount in double. It can be even decimal, if the numbers are too big.
/// 
/// THE FLOW:
/// 1. First, create a triangle of Claims instance.
///    The constructor accepts one mandatory argument "filename" of CSV file. Path to input /CSV file.
/// 
/// 2. Call method of the instance created "CreateCumulativeTriangleOfClaims()"
///    This method will...
///    a. Read CSV file, and validate filename if it is a CSV file and also check the path if it exists
///    b. It creates the list of AccumulatedClaims after calculating the claims. The Accumulated Claims are referred to as TriangleOfClaims
/// 
/// 3. The results can be written into a file and displayed on Screen by calling WriteToFile() method.
/// </summary>
namespace Tower.Watson.TriangleOfPayment
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1) //Not enough number of arguments
            {
                throw new Exception("Invalid Number of arguments!");
            }
            try
            {
                //Reading argument as filename
                string fileName = args[0];

                //Initializing the traingle of Claims instance
                TriangleOfClaims triangleOfClaims = new TriangleOfClaims(fileName);

                //Calculating accumulated claims by calling the method below.
                List<AccumulatedClaims> accumulatedClaims = triangleOfClaims.CreateCumulativeTriangleOfClaims();

                //Writing the result to a file.
                triangleOfClaims.WriteToFile(accumulatedClaims, @"C:\temp\AccumulatedClaims.txt");
            }
            catch (Exception ex)
            {
                //If the validation of either filename or loan amount is failed, an exception will raise
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine(@"USAGE: cmd> [application] [csv_file]");
                Console.WriteLine();
                Console.WriteLine(@"Example1: cmd>accumulate claims.csv");
                Console.WriteLine(@"Example2: cmd>accumulate c:\temp\claims.csv");
                Console.WriteLine();
                Console.WriteLine("Program has terminated!");
                return;
            }
        }
    }
}
