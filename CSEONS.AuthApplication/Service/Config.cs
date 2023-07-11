using Amazon;
using Microsoft.Extensions.Configuration;

namespace CSEONS.AuthApplication
{
    public class Config
    {
        public static string ConnectionString { get; set; }
        public static string AWSAccesToken { get; set; }
        public static string AWSSecretAccesToken { get; set; }
        public static string ServiceURL { get; set; }
        public static string BucketName { get; set; }
    }
}
