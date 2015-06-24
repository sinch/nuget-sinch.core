using System;
using System.Runtime.Remoting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sinch.Core.Tests {
    [TestClass]
    public class ClientTests {
        string applicationKey = "";
        string applicationSecret = "";
        private string key = "";
        private string secret = "";

        [TestMethod]
        public void TestMD5() {
            var sec = new Security(applicationKey, applicationSecret);
            var result = sec.MD5Body(@"{""message"":""Hello world""}");
            Assert.AreEqual("jANzQ+rgAHyf1MWQFSwvYw==", result);
        }
        [TestMethod]
        public void TestSign()
        {
            var sec = new Security(applicationKey, applicationSecret);
            var result = sec.SignRequest("POST", @"{""message"":""Hello world""}", "/v1/sms/+46700000000",
                "2014-06-04T13:41:58Z");
            Assert.AreEqual("5F5C418A0F914BBC8234A9BF5EDDAD97:olZ+EGML5mit6eOO7LzHdUuPkSeavDnPPXJ1T1A3nqI=", result);

        }

    
        [TestMethod]
        public void SendSMSFrom() {
            var smsRequest = new { Message = "Hello world", From = "+12023679434" };
            var url = "https://messagingapi.sinch.com/v1/sms/+15612600684";
            var httpClient = new Sinch.Core.Client(key, secret);
            var result = httpClient.PostAsJsonAsync(url, smsRequest).Result;
            Assert.IsTrue(result.IsSuccessStatusCode);
            var content = result.Content.ReadAsStringAsync().Result;
            Assert.AreEqual("ok", content);

        }

        [TestMethod]
        public void SendSMS() {
            var smsRequest = new { Message = "Hello world"};
            var url = "https://messagingapi.sinch.com/v1/sms/+46722223355";
            var httpClient = new Sinch.Core.Client(key, secret);
            var result = httpClient.PostAsJsonAsync(url, smsRequest).Result;
            Assert.IsTrue(result.IsSuccessStatusCode);
            var content = result.Content.ReadAsStringAsync().Result;
            

        }
    }
}
