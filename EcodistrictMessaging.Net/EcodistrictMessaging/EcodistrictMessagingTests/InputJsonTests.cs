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
        public void NumberToJSON()
        {
            // arrange
            Input inp = new Number(label: "A Label");
            string expected = "{\"type\":\"number\",\"label\":\"A Label\",\"min\":null,\"max\":null,\"value\":null}";

            // act
            string actual = inp.ToJson();

            // assert
            Assert.AreEqual(expected, actual, false, "Number not Json-seralized correctly");
        }

        [TestMethod]
        public void ISpecToJSON_E1()
        {
            // arrange
            InputSpecification inputSpec = new InputSpecification();
            inputSpec.Add("name", new Text(label: "Name"));
            inputSpec.Add("shoe-size", new Number(label: "Shoe size"));
            string expected = "{" +
                     "\"name\":{\"type\":\"text\",\"label\":\"Name\"}" + "," +
                "\"shoe-size\":{\"type\":\"number\",\"label\":\"Shoe size\",\"min\":null,\"max\":null,\"value\":null}" +
                "}";

            // act
            string actual = inputSpec.ToJson();

            // assert
            Assert.AreEqual(expected, actual, false, "\nInputSpecification not Json-seralized correctly:\n\n" + expected + "\n\n" + actual);
        }

        [TestMethod]
        ///
        /// Keys in nested inputs
        ///
        public void ISpecToJSON_E2()
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
                                "\"name\":{\"type\":\"text\",\"label\":\"Parent name\"}" + "," +
                                "\"age\":{\"type\":\"number\",\"label\":\"Parent age\",\"min\":null,\"max\":null,\"value\":null}" + "," +
                                "\"child\":{\"type\":\"list\",\"label\":\"Children\"," +
                                            "\"inputs\":{" +
                                                "\"name\":{\"type\":\"text\",\"label\":\"Child name\"}" + "," +
                                                 "\"age\":{\"type\":\"number\",\"label\":\"Child age\",\"min\":null,\"max\":null,\"value\":null}" +
                                                       "}" +
                                      "}" +
                              "}";

            // act
            string actual = inputSpec.ToJson();

            // assert
            Assert.AreEqual(expected, actual, false, "\nInputSpecification not Json-seralized correctly:\n\n" + expected + "\n\n" + actual);
        }
    }
}
