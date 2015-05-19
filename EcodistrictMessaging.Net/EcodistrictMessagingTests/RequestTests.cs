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
        public void GetModulesRequestTestType()
        {
            try
            {
                // arrange
                string jsonmessage = File.ReadAllText(@"../../TestData/Json/ModuleRequest/GetModulesRequest.txt");
                object obj = JsonConvert.DeserializeObject(jsonmessage);
                jsonmessage = JsonConvert.SerializeObject(obj);
                Type expected = typeof(GetModulesRequest);

                // act
                IMessage message = Deserialize.JsonString(jsonmessage);
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
        public void SelectModuleRequestTestType()
        {
            try
            {
                // arrange
                string jsonmessage = File.ReadAllText(@"../../TestData/Json/ModuleRequest/SelectModuleRequest.txt");
                object obj = JsonConvert.DeserializeObject(jsonmessage);
                jsonmessage = JsonConvert.SerializeObject(obj);
                Type expected = typeof(SelectModuleRequest);

                // act
                IMessage message = Deserialize.JsonString(jsonmessage);
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
        public void StartModuleRequestTestType()
        {
            try
            {
                // arrange
                string jsonmessage = File.ReadAllText(@"../../TestData/Json/ModuleRequest/StartModuleRequest.txt");
                object obj = JsonConvert.DeserializeObject(jsonmessage);
                jsonmessage = JsonConvert.SerializeObject(obj);
                Type expected = typeof(StartModuleRequest);

                // act
                IMessage message = Deserialize.JsonString(jsonmessage);
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
        public void StartModuleRequestComplexTestType()
        {
            try
            {
                // arrange
                string jsonmessage = File.ReadAllText(@"../../TestData/Json/ModuleRequest/StartModuleRequestComplex.txt");
                object obj = JsonConvert.DeserializeObject(jsonmessage);
                jsonmessage = JsonConvert.SerializeObject(obj);
                Type expected = typeof(StartModuleRequest);

                // act
                IMessage message = Deserialize.JsonString(jsonmessage);
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
