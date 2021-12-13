using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Service
{
    public interface IUnitOfWork
    {
        void UploadFile(IFormFile file, string userId);
    }

    public class UnitOfWork : IUnitOfWork
    {
        private IHostingEnvironment hostingEnvironment;
        public UnitOfWork(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async void UploadFile(IFormFile file, string userId)
        {
            long totalBytes = file.Length;
            string filename = file.FileName.Trim('"');
            filename = EnsureFileName(filename);
            byte[] buffer = new byte[16 * 1024];
            using (FileStream output = System.IO.File.Create(GetPathAndFileName(filename,userId)))
            {
                using(Stream input = file.OpenReadStream())
                {
                    int readBytes;
                    while((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        await output.WriteAsync(buffer, 0, readBytes);
                        totalBytes += readBytes;
                    }
                }
            }
        }

        private string GetPathAndFileName(string filename, string userId)
        {
            string path = hostingEnvironment.WebRootPath + "\\uploads\\";
            if (!Directory.Exists(path))

                Directory.CreateDirectory(path);
            string fullPath = userId + filename;
            return path + fullPath;
        }

        private string EnsureFileName(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }
    }
}
