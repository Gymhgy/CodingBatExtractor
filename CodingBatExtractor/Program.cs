using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;

namespace CodingBatExtractor {
    class Program {
        static void Main(string[] args) {
            var config = ConfigurationManager.AppSettings;
            string username = config.Get("username");
            string password = config.Get("password");
            string filepath = config.Get("filepath");
            Console.WriteLine("Hello World!");

            //Login
            var cookieContainer = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            HttpClient client = new HttpClient(handler) { BaseAddress = new Uri("codingbat.com/") };

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "uname", username },
                { "password", password },
                { "dologin", "login" },
                { "fromurl", "/java" }
            });

            var login = client.PostAsync("/login", content).Result;
        }
    }
}
