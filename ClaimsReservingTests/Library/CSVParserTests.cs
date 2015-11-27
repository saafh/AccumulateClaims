using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower.Watson.ClaimsReserving.Library.Tests
{
    [TestClass()]
    public class CSVParserTests
    {
        [TestMethod()]
        public void ReadCSVFileShouldNotReturnNull_Test()
        {
            var fileReader = CSVParser.ReadCSVFile(@"payments.csv");
            //FileReader must not be null, becuase the file exists
            Assert.AreNotEqual(fileReader, null);
        }

        [TestMethod()]
        public void ReadCSVFileFirstLineIsNotAHeaderLine_Test()
        {
            //The first line of the reader must not be a header line. 
            var fileReader = CSVParser.ReadCSVFile(@"payments.csv");
            var firstLine = fileReader.ReadLine().Split(',');
            Assert.AreEqual(firstLine[0].Trim(), "Comp");
            Assert.AreEqual(firstLine[1].Trim(), "1992");
            Assert.AreEqual(firstLine[2].Trim(), "1992");
            Assert.AreEqual(firstLine[3].Trim(), "110.0");
        }
    }
}