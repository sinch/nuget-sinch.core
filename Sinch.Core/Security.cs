using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sinch.Core {
    public class Security {
        private readonly string _applicationKey;
        private readonly string _applicationSecret;

        public Security()
        {
            throw new Exception("Must be initialized with a application key and secret");

        }
        public Security(string applicationKey, string applicationSecret)
        {
            _applicationKey = applicationKey;
            _applicationSecret = applicationSecret;
        }

        private string SignString(string stringtoSign, string secret) {
            var sha256 = new HMACSHA256(Convert.FromBase64String(secret));
            var signature = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(stringtoSign)));
            return signature;
        }

        public string MD5Body(string body)
        {
            return Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(body)));
        }

        public string SignRequest(string httpMethod, string requestBody, string url, string timeStamp) {

            string tosign = httpMethod + "\n" +
                           MD5Body(requestBody) + "\n" +
                           "application/json; charset=utf-8\n" +
                           "x-timestamp:" + timeStamp + "\n" +
                           url;
            return _applicationKey + ":" + SignString(tosign, _applicationSecret);
        }
        public string SignGetRequest(string httpMethod, string url, string timeStamp) {

            string tosign = httpMethod + "\n" +
                            "\n" +
                           "\n" +
                           "x-timestamp:" + timeStamp + "\n" +
                           url;
            return _applicationKey + ":" + SignString(tosign, _applicationSecret);
        }
    }
}
