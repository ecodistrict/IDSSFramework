using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ecodistrict.Messaging;
using Ecodistrict.Messaging.Requests;
using Ecodistrict.Messaging.Responses;
using Ecodistrict.Messaging.Results;

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
                Ecodistrict.Messaging.Output.Outputs outputs = new Ecodistrict.Messaging.Output.Outputs();
                outputs.Add(new Ecodistrict.Messaging.Output.Kpi(1, "info", "unit"));
                ModuleResult mResult = new ModuleResult("moduleId", "variantId","userId", "KpiId", outputs);
                string str1 = Serialize.ToJsonString(mResult);

                // act
                ModuleResult mResult2 = Deserialize <ModuleResult>.JsonString(str1);
                string str2 = Serialize.ToJsonString(mResult2);

                // assert
                Assert.AreEqual(str1, str2, false, "\nNot Json-serialized or deserialized correctly:\n\n" + str1 + "\n\n" + str2); //TODO is unordered => makes comparisson hard.
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
