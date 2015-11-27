using System;
using System.IO;

namespace Tower.Watson.ClaimsReserving.Library
{
    /// <summary>
    /// Static class to read CSV file.
    /// </summary>
    public static class CSVParser
    {
        /// <summary>
        /// Open and Reads CSV File into StreamReader.
        /// </summary>
        /// <returns>StreamReader containing the CSV file contents.</returns>
        public static StreamReader ReadCSVFile(string csvFileName)
        {
            StreamReader csvFileReader = null;
            //Reading CSV File
            try
            {
                csvFileReader = new StreamReader(File.OpenRead(csvFileName));
                var headerRow = csvFileReader.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return csvFileReader;
        }
    }
}
