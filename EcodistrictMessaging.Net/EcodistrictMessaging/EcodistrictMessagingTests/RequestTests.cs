using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

using Ecodistrict.Messaging;

namespace EcodistrictMessagingTests
{
    [TestClass]
    public class RequestTests
    {
        [TestMethod]
        public void GetModelsRequestTestType()
        {
            try
            {
                // arrange
                string jsonmessage = File.ReadAllText(@"../../TestData/Json/ModelRequest/GetModelsRequest.txt");
                object obj = JsonConvert.DeserializeObject(jsonmessage);
                jsonmessage = JsonConvert.SerializeObject(obj);
                Type expected = typeof(GetModelsRequest);

                // act
                IMessage message = Deserialize.JsonMessage(jsonmessage);
                Type actual = message.GetType();

                // assert                
                Assert.IsTrue(expected == actual, "Expected: " + expected.ToString() + "\n" +
                    "Actual: " + actual.ToString());
                     
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void SelectModelRequestTestType()
        {
            try
            {
                // arrange
                string jsonmessage = File.ReadAllText(@"../../TestData/Json/ModelRequest/SelectModelRequest.txt");
                object obj = JsonConvert.DeserializeObject(jsonmessage);
                jsonmessage = JsonConvert.SerializeObject(obj);
                Type expected = typeof(SelectModelRequest);

                // act
                IMessage message = Deserialize.JsonMessage(jsonmessage);
                Type actual = message.GetType();

                // assert                
                Assert.IsTrue(expected == actual, "Expected: " + expected.ToString() + "\n" +
                    "Actual: " + actual.ToString());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void StartModelRequestTestType()
        {
            try
            {
                // arrange
                string jsonmessage = File.ReadAllText(@"../../TestData/Json/ModelRequest/StartModelRequest.txt");
                object obj = JsonConvert.DeserializeObject(jsonmessage);
                jsonmessage = JsonConvert.SerializeObject(obj);
                Type expected = typeof(StartModelRequest);

                // act
                IMessage message = Deserialize.JsonMessage(jsonmessage);
                Type actual = message.GetType();

                // assert                
                Assert.IsTrue(expected == actual, "Expected: " + expected.ToString() + "\n" +
                    "Actual: " + actual.ToString());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
