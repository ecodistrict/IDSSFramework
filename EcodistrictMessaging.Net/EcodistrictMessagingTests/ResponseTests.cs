using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

using Ecodistrict.Messaging;

namespace EcodistrictMessagingTests
{
    [TestClass]
    public class ResponseTests
    {

        [TestMethod]
        public void GetModelsResponseTest()
        {
            try
            {                
                // arrange
                List<string> kpiList = new List<string>{"cheese-taste-kpi","cheese-price-kpi"};
                GetModelsResponse mResponse = new GetModelsResponse(name: "Cheese Model", moduleId: "foo-bar_cheese-model-v1-0", 
                    description: "A model to assess cheese quality.", kpiList: kpiList);
                var message = File.ReadAllText(@"../../TestData/Json/ModelResponse/GetModelsResponse.txt");
                object obj = JsonConvert.DeserializeObject(message);
                string expected = JsonConvert.SerializeObject(obj);

                // act
                string actual = Serialize.Message(mResponse);

                // assert
                Assert.AreEqual(expected, actual, false, "\nNot Json-seralized correctly:\n\n" + expected + "\n\n" + actual); //TODO is unordered => makes comparisson hard.
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void SelectModelResponseTest()
        {
            try
            {
                // arrange
                InputSpecification iSpec = new InputSpecification();
                
                iSpec.Add("age", new Number(label: "Age", min: 0, unit: "years"));
                
                Options opt = new Options();
                opt.Add(new Option(value: "alp-cheese", label: "Alpk\u00e4se"));
                opt.Add(new Option(value: "edam-cheese", label: "Edammer"));
                opt.Add(new Option(value: "brie-cheese", label: "Brie"));
                iSpec.Add("cheese-type", new Select(label: "Cheese type", options: opt, value: "brie-cheese"));  //TODO value = brie-cheese makes room for error in dashboard, shuld be connected to the options.
                
                SelectModelResponse mResponse = new SelectModelResponse(moduleId: "foo-bar_cheese-model-v1-0",
                    variantId: "503f191e8fcc19729de860ea", kpiId: "cheese-taste-kpi", inputSpecification: iSpec);
                var message = File.ReadAllText(@"../../TestData/Json/ModelResponse/SelectModelResponse.txt");
                object obj = JsonConvert.DeserializeObject(message);
                string expected = JsonConvert.SerializeObject(obj);

                // act
                string actual = Serialize.Message(mResponse);

                // assert
                Assert.AreEqual(expected, actual, false, "\nNot Json-seralized correctly:\n\n" + expected + "\n\n" + actual); //TODO is unordered => makes comparisson hard.
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void StartModelResponseTest()
        {
            try
            {
                // arrange
                StartModelResponse smResponse = new StartModelResponse(moduleId: "foo-bar_cheese-model-v1-0",
                    variantId: "503f191e8fcc19729de860ea", kpiId: "cheese-taste-kpi", status: "processing");
                var message = File.ReadAllText(@"../../TestData/Json/ModelResponse/StartModelResponse.txt");
                object obj = JsonConvert.DeserializeObject(message);
                string expected = JsonConvert.SerializeObject(obj);

                // act
                string actual = Serialize.Message(smResponse);

                // assert
                Assert.AreEqual(expected, actual, false, "\nNot Json-seralized correctly:\n\n" + expected + "\n\n" + actual); //TODO is unordered => makes comparisson hard.
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

    }
}
