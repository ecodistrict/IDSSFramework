using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ecodistrict.Messaging;

namespace EcodistrictMessagingTests
{
    [TestClass]
    public class ResultTests
    {
        [TestMethod]
        public void ModelResultTest()
        {
            try
            {
                // arrange
                Outputs outputs = new Outputs();
                outputs.Add(new Kpi(1,"info","unit"));
                ModuleResult mResult = new ModuleResult("moduleId", "variantId", "KpiId", outputs);
                string str1 = Serialize.ToJsonString(mResult);

                // act
                ModuleResult mResult2 = (ModuleResult)Deserialize.JsonString(str1);
                string str2 = Serialize.ToJsonString(mResult2);

                // assert
                Assert.AreEqual(str1, str2, false, "\nNot Json-seralized or deserialized correctly:\n\n" + str1 + "\n\n" + str2); //TODO is unordered => makes comparisson hard.
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
