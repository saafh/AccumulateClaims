using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Tower.Watson.ClaimsReserving.Tests
{
    [TestClass()]
    public class TriangleOfClaimsTests
    {
        TriangleOfClaims triangleofClaims;
        public void Setup(string fileName = "payments.csv")
        {
            triangleofClaims = new TriangleOfClaims(fileName);
        }

        [TestMethod()]
        public void TriangleOfClaims_AccumulatedClaimsCount_Test()
        {
            Setup();
            Assert.AreEqual(triangleofClaims.CreateCumulativeTriangleOfClaims().Count, 2);
        }

        [TestMethod()]
        public void TriangleOfClaims_AccumulatedClaimsFirstYear_Test()
        {
            Setup();
            Assert.AreEqual(triangleofClaims.CreateCumulativeTriangleOfClaims()[0].ClaimsPerYear.Keys.First(), 1990);
        }

        [TestMethod()]
        public void TriangleOfClaims_AccumulatedClaimsLastYear_Test()
        {
            Setup();
            Assert.AreEqual(triangleofClaims.CreateCumulativeTriangleOfClaims()[0].ClaimsPerYear.Keys.Last(), 1993);
        }

        [TestMethod()]
        public void TriangleOfClaims_AccumulatedClaimsProductNames_Test()
        {
            Setup();
            int count = 0;
            List<AccumulatedClaims> accumulatedClaims = triangleofClaims.CreateCumulativeTriangleOfClaims();
            foreach (var item in accumulatedClaims)
            {
                switch (count)
                {
                    case 0:
                        Assert.AreEqual(accumulatedClaims[count].Product, "Comp");
                        break;
                    case 1:
                        Assert.AreEqual(accumulatedClaims[count].Product, "Non-Comp");
                        break;
                }
                count++;
            }
            Assert.AreEqual(triangleofClaims.CreateCumulativeTriangleOfClaims()[0].ClaimsPerYear.Keys.Last(), 1993);
        }

        [TestMethod()]
        public void TriangleOfClaims_AccumulatedClaimsDictionaryForDictionaries_Test()
        {
            Setup();
            string text = string.Empty;
            List<AccumulatedClaims> accumulatedClaims = triangleofClaims.CreateCumulativeTriangleOfClaims();
            foreach (var claim in accumulatedClaims)
            {
                Assert.AreEqual(claim.ClaimsPerYear.Count, 4);
                //Fetching years against each claim
                foreach (var claimperyear in claim.ClaimsPerYear)
                {
                    //Fetching the values for each year
                    for (int i = 0; i < claimperyear.Value.Length; i++)
                    {
                        text += claimperyear.Value[i].ToString("F0") + ", ";
                    }
                }
                //Testing agains one product only "Comp" as in the payments file
                Assert.AreEqual(text, "0, 0, 0, 0, 0, 0, 0, 110, 280, 200, ");
                break;
            }
        }
    }
}