using System;
using System.Configuration;
using System.Collections.Specialized;

namespace CodingBatExtractor {
    class Program {
        static void Main(string[] args) {
            var config = ConfigurationManager.AppSettings;
            string username = config.Get("username");
            string password = config.Get("password");
            string filepath = config.Get("filepath");
            Console.WriteLine("Hello World!");


        }
    }
}
