using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Solution.Services.Common
{
    public static class Config
    {
        public static string WebsitePhysicalPath = ConfigurationManager.AppSettings["WebsitePhysicalPath"];
        public static string EncryptionKey = ConfigurationManager.AppSettings["EncryptionKey"];
        public static string VectorKey = ConfigurationManager.AppSettings["VectorKey"];
    }
}
