using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower.Watson.ClaimsReserving.Tests
{
    /// <summary>
    /// For this test case, a file payments.csv was created in the same directory as the test
    /// The test looks for the file with its name. If the file has incorrect name, extension 
    /// or the file couldn't be located it will raise an exception, otherwise "Validated" will be returned.
    /// Enum: FileValidationStatus is used to check the values.
    /// </summary>
    [TestClass()]
    public class FileValidatorTests
    {
        [TestMethod()]
        public void ValidateFileNameForIncorrectFileExtension1_Test()
        {
            //The file name is incorrect. 'market.ddv' does not exist. Should raise an exception!
            Assert.AreEqual(FileValidator.ValidateFileName("payments.ddv"), FileValidationStatus.CSVFileExtension_Error);
        }

        [TestMethod()]
        public void ValidateFileNameForIncorrectFileExtension2_Test()
        {
            //The file name is incorrect. 'market' does not exist. Should raise an exception!
            Assert.AreEqual(FileValidator.ValidateFileName("payments"), FileValidationStatus.CSVFileExtension_Error);
        }

        [TestMethod()]
        public void ValidateFileNameForIncorrectFileName_Test()
        {
            //The file name is incorrect. 'market1.csv' does not exist. Should raise an exception!
            Assert.AreEqual(FileValidator.ValidateFileName("payments1.csv"), FileValidationStatus.CSVFileDoesNotExist_Error);
        }

        [TestMethod()]
        public void ValidateFileNameForIncorrectFilePath_Test()
        {
            //The file name is incorrect. 'market1.csv' does not exist. Should raise an exception!
            Assert.AreEqual(FileValidator.ValidateFileName(@"c:\payments.csv"), FileValidationStatus.CSVFileDoesNotExist_Error);
        }

        [TestMethod()]
        public void ValidateFileNameForCorrectFileName_Test()
        {
            //The file exists. Should return Validated!
            Assert.AreEqual(FileValidator.ValidateFileName("payments.csv"), FileValidationStatus.Validated);
        }
    }
}