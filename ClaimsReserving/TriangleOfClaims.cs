using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Tower.Watson.ClaimsReserving.Interface;
using Tower.Watson.ClaimsReserving.Library;

namespace Tower.Watson.ClaimsReserving
{
    public class TriangleOfClaims : ITriangleOfClaims
    {
        private string CSVFileName;
        private char CSVDelimiterChar;
        private List<Claims> claims;
        private FileValidationStatus fileValidationStatus;
        private static List<AccumulatedClaims> cumulativeTriangles = new List<AccumulatedClaims>();

        private TriangleOfClaims()
        {
        }

        /// <summary>
        /// Constructor to create triangle of claims.
        /// </summary>
        /// <param name="csvFileName">CSV file name which has claims data</param>
        /// <param name="csvDelimiterChar">csv delimiter. Default is ','.</param>
        public TriangleOfClaims(string csvFileName, char csvDelimiterChar = ',')
        {
            //Validating file name and location
            fileValidationStatus = csvFileName.ValidateFileName();
            if (fileValidationStatus == FileValidationStatus.Validated)
            {
                CSVFileName = csvFileName;
                CSVDelimiterChar = csvDelimiterChar;
            }
            else
            {
                throw new Exception(fileValidationStatus.ToString() + " - Check whether the file exists or the filename is correct");
            }
        }

        /// <summary>
        /// Returns AccumulatedTriangle in the form of, for example for a product 'Comp' and years 1995-1997
        /// Comp
        /// 1995	100	150	350
        /// 1996	80	120 ?
        /// 1997	120	?	?
        /// </summary>
        /// <returns>List of Accumulated Claims of type AccumulatedClaims</returns>
        public List<AccumulatedClaims> CreateCumulativeTriangleOfClaims()
        {
            ParseCSVFile();

            //Distinct Products
            string[] products = claims.Select(x => x.Product).Distinct().ToArray();
            Array.Sort(products);

            //Distinct Years of the claims
            int[] totalYears = claims.Select(x => x.OriginYear).Distinct().ToArray();
            Array.Sort(totalYears);

            //For every product in the claims, go through the years and calculate accumulated values
            foreach (var product in products)
            {
                //Temp claim
                List<Claims> tempClaim = claims.Where(x => x.Product == product).OrderBy(x => x.OriginYear).ToList();

                //Temporary dictionary to hold accumulated values par claim, to add to the final Matrix (List of claims)
                Dictionary<int, double[]> tempDict = new Dictionary<int, double[]>();

                //loop for every year present in the input file. Used as Origin Year
                for (int i = 0; i < totalYears.Length; i++)
                {
                    double cumulativeSum = 0.0;
                    double incrementValue = 0.0;

                    //For the values to be added in right development year to make triangle
                    int indexForCumulativeClaims = 0; 

                    //this is the index of the value against each year. It is decreasing for every year by one from its previous year.
                    double[] Claims = new double[totalYears.Length - i]; 

                    //Development Years loop (all the years in the input file)
                    for (int j = i; j < totalYears.Length; j++)
                    {
                        //Looking for a claim withing the Origin and development year. If doesn't found, null is reutrned
                        Claims val = tempClaim.Find(x => x.OriginYear == totalYears[i] && x.DevelopmentYear == totalYears[j]);

                        //null is taken as 0. Becuase Zero supposed to be placed agains missing year for a particular claim
                        incrementValue = Convert.ToDouble(val == null ? 0.0 : val.IncrementalValue);

                        //Accumulating
                        //like (for 1990) 0, 0, 5, (5+10)=15, (15+7)=22 ..
                        cumulativeSum += incrementValue;
                        Debug.WriteLine(product + " - " + totalYears[i] + " - " + totalYears[j] + " : " + incrementValue);

                        //A value is placed (or added and placed) in a row.
                        //1990 0, 0, 5 ..
                        Claims[indexForCumulativeClaims] = cumulativeSum;

                        //Index of the array is being incremented
                        indexForCumulativeClaims++;
                    }

                    //A temporary dictionary is maintained for each claim. year by Year
                    //1990  0,0,5,10,35
                    //1991  10, 15, 25
                    tempDict.Add(totalYears[i], Claims);
                }

                //Every time a product is processed for al the years, a calim is added to a matrix
                cumulativeTriangles.Add(new AccumulatedClaims
                {
                    Product = product,
                    ClaimsPerYear = tempDict
                });
            }

            return cumulativeTriangles;
        }

        /// <summary>
        /// Writes output to a Text file and also prints on Console.
        /// </summary>
        /// <param name="accumulatedClaims">Accumulated claims object is returned after method "CreateCumulativeTriangleOfClaims()" is executed </param>
        /// <param name="outputFilePath">Path to output file. Only TXT file is acceptable as a output. For example: C:\temp\output.txt</param>
        /// <param name="writeToFile">If false, the file won't be created instead the output is shown on Console only. Default is 'true'</param>
        public void WriteToFile(List<AccumulatedClaims> accumulatedClaims, string outputFilePath, bool writeToFile = true)
        {
            //If the accumulated claims are to be printed and the object is empty, 
            //the program will try to fetch the claims
            if(accumulatedClaims == null)
            {
                try
                {
                    accumulatedClaims = this.CreateCumulativeTriangleOfClaims();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

            if (accumulatedClaims != null && outputFilePath.Length > 0)
            {
                string text = string.Empty;
                if (accumulatedClaims != null)
                {
                    //Getting the first key value of the dictionary which is the first year of any claim
                    text += accumulatedClaims[0].ClaimsPerYear.Keys.First().ToString();

                    //Number of years altogether
                    text += ", " + accumulatedClaims[0].ClaimsPerYear.Count;

                    //Fetching accumulated claims one by one
                    foreach (var claim in accumulatedClaims)
                    {
                        text += "\r\n";
                        text += claim.Product;

                        //Fetching years against each claim
                        foreach (var item in claim.ClaimsPerYear)
                        {
                            //Fetching the values for each year
                            for (int i = 0; i < item.Value.Length; i++)
                            {
                                text += ", " + item.Value[i].ToString("F0"); //This line can also be written as text += ", " + item.Value[i].ToString("C2", Cultures.UnitedKingdom)
                            }
                        }
                    }
                }
                if (writeToFile)
                {
                    System.IO.File.WriteAllText(@outputFilePath, text);
                }
                Console.WriteLine(text);
            }
        }

        /// <summary>
        /// Parses the CSV file to get a Claims over the years. The List is ordered by Product (in ascending order).
        /// </summary>
        /// <returns>List: Pool of Claims </returns>
        private List<Claims> ParseCSVFile()
        {
            var csvFileReader = CSVParser.ReadCSVFile(CSVFileName);
            if (csvFileReader != null)
            {
                claims = new List<Claims>();
                try
                {
                    //Populating list of claims from the CSV file.
                    while (!csvFileReader.EndOfStream)
                    {
                        Claims triangle = new Claims();
                        var line = csvFileReader.ReadLine();
                        var values = line.Split(CSVDelimiterChar);

                        triangle.Product = values[0].Trim();
                        triangle.OriginYear = Convert.ToInt16(values[1].Trim()); 
                        triangle.DevelopmentYear = Convert.ToInt16(values[2].Trim());
                        triangle.IncrementalValue = double.Parse(values[3].Trim(), CultureInfo.InvariantCulture); //some cultures use "," as decimal and some use "."

                        claims.Add(triangle);
                    }
                    //Sort the claims by Product (Ascending).
                    claims = claims.OrderBy(p => p.Product).ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            //List of claims over the years
            return claims;
        }

    }
}
