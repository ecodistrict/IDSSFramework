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
            _serJson = new DataContractJsonSerializer(typeof(Number), _settings);
            Input inp = new Number(label: "aLabel", id: "aId");
            string expected = "{aId:{\"label\":\"aLabel\",\"type\":\"number\",\"max\":null,\"min\":null,\"value\":null}}";

            // act
            _serJson.WriteObject(_mStream, inp);
            _mStream.Position = 0;
            StreamReader sr = new StreamReader(_mStream);

            // assert
            string actual = sr.ReadToEnd();
            Assert.AreEqual(expected, actual, false, "Number not Json-seralized correctly");
        }
    }
}
