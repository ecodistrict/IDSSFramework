using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Ecodistrict.Messaging;

namespace EcodistrictMessagingTests
{

    [TestClass]
    public class InputJsonTests
    {
        DataContractJsonSerializerSettings _settings = new DataContractJsonSerializerSettings();
        MemoryStream _mStream = new MemoryStream();

        public InputJsonTests()
        {            
            _settings.EmitTypeInformation = EmitTypeInformation.Never;
        }
        
        [TestMethod]
        public void AtomicNumber()
        {
            try
            {
                // arrange
                InputSpecification inputSpec = new InputSpecification();
                inputSpec.Add("number", new Number(label: "A label"));
                var message = File.ReadAllText(@"../../TestData/Json/InputSpecification/AtomicNumber.txt");
                object obj = JsonConvert.DeserializeObject(message);                
                string expected = JsonConvert.SerializeObject(obj);

                // act
                string actual = Serialize.ToJsonString(inputSpec);
                //string actual = Newtonsoft.Json.JsonConvert.SerializeObject(inputSpec);

                // assert
                Assert.AreEqual(expected, actual, false, "\nNumber not Json-seralized correctly:\n\n" + expected + "\n\n" + actual);
             }           
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void AtomicText()
        {
            try
            {
                // arrange
                InputSpecification inputSpec = new InputSpecification();
                inputSpec.Add("text", new Text(label: "A Label"));
                string expected = "{" +
                                        "\"text\":{\"label\":\"A Label\",\"type\":\"text\"}" +
                                   "}";

                // act
                string actual = Serialize.ToJsonString(inputSpec);

                // assert
                Assert.AreEqual(expected, actual, false, "\nText not Json-seralized correctly:\n\n" + expected + "\n\n" + actual);
            }           
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void AtomicSelect()
        {
            try
            {
                // arrange
                InputSpecification inputSpec = new InputSpecification();
                Options opt = new Options();
                opt.Add(new Option(value: "alp-cheese", label: "Alpku00e4se"));
                opt.Add(new Option(value: "edam-cheese", label: "Edammer"));
                opt.Add(new Option(value: "brie-cheese", label: "Brie"));
                inputSpec.Add("cheese-type", new Select(label: "Cheese type", options: opt, value: "brie-cheese"));  //TODO value = brie-cheese makes room for error in dashboard, shuld be connected to the options.
                string expected = "{" +
                                        "\"cheese-type\":{" +
                                                         "\"options\":[" +
                                                           "{\"value\":\"alp-cheese\",\"label\":\"Alpku00e4se\"}" + "," +
                                                           "{\"value\":\"edam-cheese\",\"label\":\"Edammer\"}" + "," +
                                                           "{\"value\":\"brie-cheese\",\"label\":\"Brie\"}" +
                                                                     "]" + "," +
                                                         "\"value\":\"brie-cheese\",\"label\":\"Cheese type\",\"type\":\"select\"" +
                                                        "}" +
                                   "}";

                // act
                string actual = Serialize.ToJsonString(inputSpec);

                // assert
                Assert.AreEqual(expected, actual, false, "\nSelect not Json-seralized correctly:\n\n" + expected + "\n\n" + actual);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void ISpecToJSON_E1()
        {
            try
            {
                // arrange
                InputSpecification inputSpec = new InputSpecification();
                inputSpec.Add("name", new Text(label: "Name"));
                inputSpec.Add("shoe-size", new Number(label: "Shoe size"));
                string expected = "{" +
                                     "\"name\":{\"label\":\"Name\",\"type\":\"text\"}" + "," +
                                     "\"shoe-size\":{\"label\":\"Shoe size\",\"type\":\"number\"}" +
                                  "}";                                                 
                // act
                string actual = Serialize.ToJsonString(inputSpec);

                // assert
                Assert.AreEqual(expected, actual, false, "\nInputSpecification not Json-seralized correctly:\n\n" + expected + "\n\n" + actual);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void ISpecToJSON_E2()
        {
            try
            {
                // arrange
                InputSpecification inputSpec = new InputSpecification();
                inputSpec.Add("name", new Text(label: "Parent name"));
                inputSpec.Add("age", new Number(label: "Parent age"));
                List aList = new List("Children");
                aList.Add("name", new Text(label: "Child name"));
                aList.Add("age", new Number(label: "Child age"));
                inputSpec.Add("child", aList);
                string expected = "{" +
                                    "\"name\":{\"label\":\"Parent name\",\"type\":\"text\"}" + "," +
                                    "\"age\":{\"label\":\"Parent age\",\"type\":\"number\"}" + "," +
                                    "\"child\":{\"inputs\":{" +
                                                    "\"name\":{\"label\":\"Child name\",\"type\":\"text\"}" + "," +
                                                     "\"age\":{\"label\":\"Child age\",\"type\":\"number\"}" +
                                                           "}," +
                                                "\"label\":\"Children\",\"type\":\"list\"" +
                                          "}" +
                                  "}";

                // act
                string actual = Serialize.ToJsonString(inputSpec);

                // assert
                Assert.AreEqual(expected, actual, false, "\nInputSpecification not Json-seralized correctly:\n\n" + expected + "\n\n" + actual);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
