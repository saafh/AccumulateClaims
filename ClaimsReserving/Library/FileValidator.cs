namespace Tower.Watson.ClaimsReserving
{
    /// <summary>
    /// Validates the validity of CSV filename and path.
    /// </summary>
    public static class FileValidator
    {
        /// <summary>
        /// Valdates the file given filename
        /// </summary>
        /// <param name="fileName">Input file as CSV</param>
        /// <returns></returns>
        public static FileValidationStatus ValidateFileName(this string fileName)
        {
            FileValidationStatus fileValidationStatus = FileValidationStatus.Validated;

            //Check if the file extension of the filename is csv
            if (System.IO.Path.GetExtension(fileName).ToLower() != ".csv")
            {
                fileValidationStatus = FileValidationStatus.CSVFileExtension_Error;
                return fileValidationStatus;
            }

            //Check if the file Exists
            if (!System.IO.File.Exists(fileName))
            {
                fileValidationStatus = FileValidationStatus.CSVFileDoesNotExist_Error;
                return fileValidationStatus;
            }

            return fileValidationStatus;
        }
    }
}
