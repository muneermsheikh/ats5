using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.Admin;
using core.Entities.Attachments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    public class FileUploadController : BaseApiController
    {
        private readonly ILogger<FileUploadController> _logger;
        const string FILE_PATH = @"D:\Samples\";
        
          public FileUploadController(ILogger<FileUploadController> logger)
          {
            _logger = logger;
          }

        
        [HttpPost]
        public IActionResult Post([FromBody]FileToUpload theFile) 
        {
            var filePathName = FILE_PATH + Path.GetFileNameWithoutExtension(theFile.FileName) + "-" +
            DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") +
            Path.GetExtension(theFile.FileName);
            
            if (theFile.FileAsBase64.Contains(","))
            {
                theFile.FileAsBase64 = theFile.FileAsBase64.Substring(theFile.FileAsBase64.IndexOf(",") + 1);
            }

            theFile.FileAsByteArray = Convert.FromBase64String(theFile.FileAsBase64);
            
            using (var fs = new FileStream(filePathName, FileMode.CreateNew)) 
            {
                fs.Write(theFile.FileAsByteArray, 0, theFile.FileAsByteArray.Length);
            }


            return Ok();
        }

       [HttpGet("LoadDocument")]
    
        public LoadedDocument LoadDocument()
        {
            string documentName = "invoice.docx";
            LoadedDocument document = new LoadedDocument()
            {
            DocumentData = Convert.ToBase64String(
                System.IO.File.ReadAllBytes("App_Data/" + documentName)),
            DocumentName = documentName
            };

            return document;
        }
    }
}