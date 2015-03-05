using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ecodistrict.Messaging;

namespace EcodistrictMessagingTests
{
    [TestClass]
    public class InputJsonTests
    {
        DataContractJsonSerializerSettings _settings = new DataContractJsonSerializerSettings();
        MemoryStream _mStream = new MemoryStream();
        DataContractJsonSerializer _serJson;

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
                inputSpec.Add(key: "number", item: new Number(label: "A Label"));
                string expected = "{" +
                                    "\"number\":{\"type\":\"number\",\"label\":\"A Label\",\"order\":null,\"value\":null,\"unit\":null,\"min\":null,\"max\":null}" +
                                  "}";

                // act
                string actual = inputSpec.ToJson();

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
                inputSpec.Add(key: "text", item: new Text(label: "A Label"));
                string expected = "{" +
                                        "\"text\":{\"type\":\"text\",\"label\":\"A Label\",\"order\":null,\"value\":null}" +
                                   "}";

                // act
                string actual = inputSpec.ToJson();

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
                inputSpec.Add(key: "cheese-type", item: new Select(label: "Cheese type", options: opt, value: "brie-cheese"));  //TODO value = brie-cheese makes room for error in dashboard, shuld be connected to the options.
                string expected = "{" +
                                        "\"cheese-type\":{\"type\":\"select\",\"label\":\"Cheese type\",\"order\":null,\"value\":\"brie-cheese\"" + "," +
                                                         "\"options\":[" +
                                                           "{\"value\":\"alp-cheese\",\"label\":\"Alpku00e4se\"}" + "," +
                                                           "{\"value\":\"edam-cheese\",\"label\":\"Edammer\"}" + "," +
                                                           "{\"value\":\"brie-cheese\",\"label\":\"Brie\"}" +
                                                                     "]" +
                                                        "}" +
                                   "}";

                // act
                string actual = inputSpec.ToJson();

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
                                     "\"name\":{\"type\":\"text\",\"label\":\"Name\",\"order\":null,\"value\":null}" + "," +
                                     "\"shoe-size\":{\"type\":\"number\",\"label\":\"Shoe size\",\"order\":null,\"value\":null,\"unit\":null,\"min\":null,\"max\":null}" +
                                  "}";                                                 
                // act
                string actual = inputSpec.ToJson();

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
                                    "\"name\":{\"type\":\"text\",\"label\":\"Parent name\",\"order\":null,\"value\":null}" + "," +
                                    "\"age\":{\"type\":\"number\",\"label\":\"Parent age\",\"order\":null,\"value\":null,\"unit\":null,\"min\":null,\"max\":null}" + "," +
                                    "\"child\":{\"type\":\"list\",\"label\":\"Children\"," +
                                                "\"inputs\":{" +
                                                    "\"name\":{\"type\":\"text\",\"label\":\"Child name\",\"order\":null,\"value\":null}" + "," +
                                                     "\"age\":{\"type\":\"number\",\"label\":\"Child age\",\"order\":null,\"value\":null,\"unit\":null,\"min\":null,\"max\":null}" +
                                                           "}" +
                                          "}" +
                                  "}";

                // act
                string actual = inputSpec.ToJson();

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
