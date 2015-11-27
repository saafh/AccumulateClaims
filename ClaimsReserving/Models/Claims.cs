namespace Tower.Watson.ClaimsReserving
{
    /// <summary>
    /// Contains claims read by CSV file.
    /// Product (string): Product Name
    /// OriginYear (int): Origin Year
    /// DevelopmentYear (int): Development Year
    /// IncrementalValue (double): Incremental Value
    /// </summary>
    public sealed class Claims
    {
        /// <summary>
        /// Product Name
        /// </summary>
        public string Product { get; set; }


        /// <summary>
        /// Origin Year
        /// </summary>
        public int OriginYear { get; set; }

        /// <summary>
        /// Development Year
        /// </summary>
        public int DevelopmentYear { get; set; }

        /// <summary>
        /// Incremental Value
        /// </summary>
        public double IncrementalValue { get; set; }
    }
}
