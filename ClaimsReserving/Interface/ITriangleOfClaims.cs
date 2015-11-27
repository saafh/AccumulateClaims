using System.Collections.Generic;

namespace Tower.Watson.ClaimsReserving.Interface
{
    /// <summary>
    /// Interface of ITriangleOfClaims 
    /// </summary>
    public interface ITriangleOfClaims
    {
        List<AccumulatedClaims> CreateCumulativeTriangleOfClaims();
        void WriteToFile(List<AccumulatedClaims> accumulatedClaims, string outputFilePath, bool writeToFile = true);
    }
}
