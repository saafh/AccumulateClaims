using System.Collections.Generic;

namespace Tower.Watson.ClaimsReserving
{
    /// <summary>
    /// Maintains Accumulated Claims created by TriangleOfClaims. It has two members
    /// Product (string): It holds product name
    /// ClaimsPerYear (Dictionary <int, double[]>): Contains Claims per year. Year as Key, number of claims for that purticular year as double[]
    /// </summary>
    public sealed class AccumulatedClaims
    {
        /// <summary>
        /// Product (string): It holds product name
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// ClaimsPerYear (Dictionary <int, double[]>): Contains Claims per year. Year as Key, number of claims for that purticular year as double[]
        /// </summary>
        public Dictionary<int, double[]> ClaimsPerYear { get; set; }
    }
}
