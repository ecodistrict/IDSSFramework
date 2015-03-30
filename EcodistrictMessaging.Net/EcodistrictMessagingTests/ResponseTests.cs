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
        public void GetModulesResponseTest()
        {
            try
            {                
                // arrange
                List<string> kpiList = new List<string>{"cheese-taste-kpi","cheese-price-kpi"};
                GetModulesResponse mResponse = new GetModulesResponse(name: "Cheese Module", moduleId: "foo-bar_cheese-Module-v1-0", 
                    description: "A Module to assess cheese quality.", kpiList: kpiList);
                var message = File.ReadAllText(@"../../TestData/Json/ModuleResponse/GetModulesResponse.txt");
                object obj = JsonConvert.DeserializeObject(message);
                string expected = JsonConvert.SerializeObject(obj);

                // act
                string actual = Serialize.ToJsonString(mResponse);

                // assert
                Assert.AreEqual(expected, actual, false, "\nNot Json-seralized correctly:\n\n" + expected + "\n\n" + actual); //TODO is unordered => makes comparisson hard.
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void SelectModuleResponseTest()
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
                
                SelectModuleResponse mResponse = new SelectModuleResponse(moduleId: "foo-bar_cheese-Module-v1-0",
                    variantId: "503f191e8fcc19729de860ea", kpiId: "cheese-taste-kpi", inputSpecification: iSpec);
                var message = File.ReadAllText(@"../../TestData/Json/ModuleResponse/SelectModuleResponse.txt");
                object obj = JsonConvert.DeserializeObject(message);
                string expected = JsonConvert.SerializeObject(obj);

                // act
                string actual = Serialize.ToJsonString(mResponse);

                // assert
                Assert.AreEqual(expected, actual, false, "\nNot Json-seralized correctly:\n\n" + expected + "\n\n" + actual); //TODO is unordered => makes comparisson hard.
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void StartModuleResponseTest()
        {
            try
            {
                // arrange
                StartModuleResponse smResponse = new StartModuleResponse(moduleId: "foo-bar_cheese-Module-v1-0",
                    variantId: "503f191e8fcc19729de860ea", kpiId: "cheese-taste-kpi", status: ModuleStatus.Processing);
                var message = File.ReadAllText(@"../../TestData/Json/ModuleResponse/StartModuleResponse.txt");
                object obj = JsonConvert.DeserializeObject(message);
                string expected = JsonConvert.SerializeObject(obj);

                // act
                string actual = Serialize.ToJsonString(smResponse);

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
