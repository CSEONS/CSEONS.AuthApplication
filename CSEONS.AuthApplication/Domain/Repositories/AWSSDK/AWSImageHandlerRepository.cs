using Amazon.S3;
using Amazon.S3.Model;
using CSEONS.AuthApplication.Domain.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSEONS.AuthApplication.Domain.Repositories.AWSSDK
{
    public class AWSImageRepository : IImageHandlerRepository
    {
        private AmazonS3Client client;
        private string bucketName;
        private string path = "images/";
        private string baseExtension = ".jpg";

        public AWSImageRepository(AmazonS3Client client, string bucketName)
        {
            this.client = client;
            this.bucketName = bucketName;
        }

        public void DeleteImage(string fileName)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = $"{path}{fileName}{baseExtension}"
            };

            client.DeleteObjectAsync(request);
        }

        public string GetImageUrl(string fielName)
        {
            var request = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Expires = DateTime.UtcNow.AddMinutes(1),
                Key = $"{path}{fielName}{baseExtension}"
            };

            return client.GetPreSignedURL(request);
        }

        public Task UploadImage(IFormFile formFile, string fileName)
        {
            var request = new PutObjectRequest()
            {
                Key = $"{path}{fileName}{baseExtension}",
                InputStream = formFile.OpenReadStream(),
                BucketName = bucketName,
            };

            request.Metadata.Add("Content-Type", formFile.ContentType);

            return client.PutObjectAsync(request);
        }
    }
}
