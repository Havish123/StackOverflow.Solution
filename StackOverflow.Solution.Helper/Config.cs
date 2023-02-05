using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Solution.Helper
{
    public class Config
    {
        

        public static readonly IConfigurationRoot AppSettings= GetConfigurationRoot();
        public static IConfigurationRoot GetConfigurationRoot()
        {
            IConfigurationRoot configuration = null;
            var basePath = System.AppContext.BaseDirectory;

            var configurationBuilder = new ConfigurationBuilder().SetBasePath(basePath)
                .AddJsonFile($"appsettings.json");
            configuration = configurationBuilder.Build();

            return configuration;
        }
        public static string ConnectionString = AppSettings["ConnectionString"];
        public static string LoginApiTokenKey = AppSettings["LoginApiTokenKey"];
        public static string WebKey = AppSettings["WebKey"];
        public static string EncryptionKey = AppSettings["EncryptionKey"];
        public static string VectorKey = AppSettings["VectorKey"];
        public static string tokenAPi = AppSettings["tokenAPi"];
        public static double firstlevelliftTime = double.Parse(AppSettings["firstlevelliftTime"]);


    }
}
