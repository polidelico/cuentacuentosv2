using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace Wevideo
{
    class Requester
    {
        private string api_key;
        private string api_secret;
        private string base_url;
        private string restOfTheURL;
        private string data = "{}";
        private string authURL;
        private string method = "POST";

        
        private LoginInfo loginInfo;
        
        public LoginInfo LoginInfo { get { return loginInfo; } }
        public bool IsLoggedIn { get { return loginInfo != null; } }
        private DateTime date;
        private string authheadval;
        private string url { get { return GetFullURL(); } }

        public Requester(string key, string secret, string base_url,string  authURL)
        {
            api_key = key;
            api_secret = secret;
            this.authURL = authURL;
            this.base_url = base_url;
        }

        public async Task<string> Auth(string email)
        {
            SetData("email", email);
            SetRestOfURL(authURL);
            SetMethod("POST");
            var result = await Request();
            if (result.Contains("Token"))
            {
                loginInfo = new LoginInfo(result);
                Console.WriteLine(loginInfo.ToString());
            }
          
            ClearData();
            //var login = await Login();
            //Console.WriteLine(login.ToString());
            return result;
        }

        public async Task<string> Login()
        {
            SetRestOfURL("/3/sso/login/" + loginInfo.Token);
            SetMethod("GET");
            var result = await Request(false);
            Console.WriteLine(result);
            return result;
        }
        public void SetMethod(string method)
        {
            this.method = method;
        }

        public string GetFullURL()
        {
            var result = string.IsNullOrEmpty(restOfTheURL) ? base_url : base_url + restOfTheURL;
            return result;
        }

        public void SetRestOfURL(string url)
        {
            restOfTheURL = url;
        }

        public void SetData(string name, string value)
        {
            JObject obj = null;
            if (!string.IsNullOrEmpty(data))
                obj = JObject.Parse(data);
            else
                obj = new JObject();
            
            obj.Add(name, value);
            data = obj.ToString();
        }

        public void ClearData()
        {
            data = "{}";
        }

        private void Sign()
        {
            date = DateTime.Now.ToUniversalTime();
            var hash = System.Security.Cryptography.MD5CryptoServiceProvider.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
            var hex = BitConverter.ToString(hash).Replace("-", "").ToLower();
            var toSign = method + "\n" + hex + "\n" + date.ToString("R") + "\n" + url;
            var hmacsha256 = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(api_secret));
            var sigbytes = hmacsha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(toSign));
            authheadval = System.Convert.ToBase64String(sigbytes);
        }

        public async Task<string> Request(bool sign = true)
        {
            if (sign)
                Sign();
            var client = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
            client.Date = date;
            if (sign)
            {
                client.Headers.Add("Authorization", "WEV " + api_key + ":" + authheadval);
            }
            client.Accept = "application/json";
            client.ContentType = "application/json";
            client.Method = method;

            if (method != "GET") {
            var stream = client.GetRequestStream();
            var stuff = System.Text.Encoding.UTF8.GetBytes(data);
            stream.Write(stuff, 0, stuff.Length);
            stream.Close();
            }
            string responseFromServer = string.Empty;
            try
            {
                var resp = await client.GetResponseAsync();
                var stat = ((System.Net.HttpWebResponse)resp).StatusDescription;
                var respStream = resp.GetResponseStream();
                var reader = new System.IO.StreamReader(respStream);
                 responseFromServer = reader.ReadToEnd();
                
            } catch (WebException e)
            {
                responseFromServer = e.ToString();
                var respStream = e.Response.GetResponseStream();
                var reader = new StreamReader(respStream);
                var log = reader.ReadToEnd();
            }
            Console.WriteLine(responseFromServer);
            
            return responseFromServer;
        }



    }

    public class LoginInfo
    {

        public string SessionID {  get { return sessionID; } }
        public string Token { get { return token; } }
        public string Email { get { return email; } }
        public string UserID { get { return userID; } }

        private string sessionID;
        private string token;
        private string email;
        private string userID;

        private const string SessionKEY = "sessionId";
        private const string emailKEY = "email";
        private const string tokenKEY = "loginToken";
        private const string userIdKey = "userId";


        public LoginInfo(string json)
        {
            var jObj = JObject.Parse(json);
            var userID = jObj[userIdKey];
            if(userID != null)
            {
                this.userID = userID.Value<string>();
                sessionID = jObj[SessionKEY].Value<string>();
                email = jObj[emailKEY].Value<string>();
                token = jObj[tokenKEY].Value<string>();
            }
        }

        public override string ToString()
        {
            string result = "UserID: " + userID + "\n" + "E-mail: " + email + "\n" + "Token: " + token + "\n"
                + "SessionID " + sessionID;
            return result;
        }

    }

}
