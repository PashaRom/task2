using Microsoft.Extensions.Configuration;
using App.Entities;


namespace App.Test.AppConfig
{
    public static class Configuration
    {
        private const string configFile = "testconfig.json";        
        private static User user;
        private static string dirToCsvFile;
        static Configuration() {
            var builder = new ConfigurationBuilder().AddJsonFile("testconfig.json");
            Get = builder.Build();
            BuildUser();
            dirToCsvFile = Get["dirToCsvFile"];
        }
        public static IConfiguration Get { get; set; }

        public static string NameConfigFile {
            get {
                return configFile;
            }
        }

        private static void BuildUser() {
            user = new User();
            user.Login = Get["userCred:login"];
            user.Password = Get["userCred:password"];
        }

        public static User User {
            get {
                return user;
            }
        }

        public static string DirToCsvFile {
            get {
                return dirToCsvFile;
            }
        }
    }
}
