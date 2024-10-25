using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using TxnManager.Infrastructure;
using TxnManager.Web.Attributes;

namespace TxnManager.Web.Dto
{
    public class UploadDocumentDto
    {
        [Required]
        [AllowedFileExtension(new FileExtension[]{FileExtension.Csv, FileExtension.Xml})]
        [AllowedFileSize(1024*1024)]
        public IFormFile FormFile { get; set; }

        public FileExtension GetExtension()
        {
            var extension = FormFile?.FileName?.Split('.').Last();
            if (Enum.TryParse(typeof(FileExtension), extension, true, out object ext))
            {
                return (FileExtension) ext;
            }

            throw new Exception($"{extension} extension is not supported.");
        }
    }
}