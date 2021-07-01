using System;
using System.Net;
using System.Linq;
using HtmlAgilityPack;
using System.Net.Http;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodingBatExtractor {
    class Program {
        static void Main(string[] args) {
            string username = args[0];
            string password = args[1];
            string filepath = args[2];

            string codingbat = "https://codingbat.com";
            //Login
            var cookieContainer = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            HttpClient client = new HttpClient(handler) { BaseAddress = new Uri(codingbat) };
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "uname", username },
                { "pw", password },
                { "dologin", "log in" },
                { "fromurl", "/java" }
            });

            var login = client.PostAsync("/login", content).Result;

            //Parse
            var doneHtml = client.GetStringAsync("/done").Result;
            var doc = new HtmlDocument();
            doc.OptionEmptyCollection = true;
            doc.LoadHtml(doneHtml);
            var parsed = doc.DocumentNode.SelectNodes("//a[contains(@href,'/prob/')]").Select(prob => {
                var innertext = prob.InnerText.Split();
                var link = prob.Attributes["href"].Value;
                var probHtml = client.GetStringAsync(link).Result;
                var probDoc = new HtmlDocument();
                probDoc.LoadHtml(probHtml);
                return new CodingBatProblem(
                    link: codingbat + link,
                    problemGroup: innertext[0],
                    problemName: innertext[1],
                    description: probDoc.DocumentNode.SelectSingleNode("//p[@class='max2']").InnerText,
                    implementation: probDoc.DocumentNode.SelectSingleNode("//div[@id='ace_div']").InnerText,
                    language: prob.NextSibling.InnerText.Contains("python") ? Language.Python : Language.Java);
            }).ToList();

            //Process
            parsed.ForEach(prob => {
                var formatted = prob.Format();
                var path = Path.Combine(filepath, prob.Language == Language.Java ? "java" : "python", prob.ProblemGroup);
                //Create directory doesn't do anything if directory already exists
                //So don't need to check
                Directory.CreateDirectory(path);

                using (FileStream fs = File.Create(Path.Combine(path, prob.ProblemName + (prob.Language == Language.Java ? ".java" : ".py")))) {
                    byte[] bytes = new UTF8Encoding().GetBytes(formatted);
                    fs.Write(bytes, 0, bytes.Length);
                }
            });
        }
    }
}
