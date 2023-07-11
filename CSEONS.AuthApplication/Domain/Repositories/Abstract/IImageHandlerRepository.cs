using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSEONS.AuthApplication.Domain.Repositories.Abstract
{
    public interface IImageHandlerRepository
    {
        Task UploadImage(IFormFile stream, string name);
        string GetImageUrl(string name);
        void DeleteImage(string name);

    }
}
