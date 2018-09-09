using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO; 
using System.Web.Hosting; 
using System.Web.Mvc;

namespace Storichain.Controllers
{
    public class VideoResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context) 
        { 
            //The File Path 
            var videoFilePath = HostingEnvironment.MapPath("~/Content/movie/sample.mp4"); 
            //The header information 
            context.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=sample.mp4"); 
            context.HttpContext.Response.ContentType = "video/mpeg";
            var file = new FileInfo(videoFilePath); 
            //Check the file exist,  it will be written into the response 
            if (file.Exists) 
            { 
                var stream = file.OpenRead(); 
                var bytesinfile = new byte[stream.Length]; 
                stream.Read(bytesinfile, 0, (int)file.Length); 
                context.HttpContext.Response.BinaryWrite(bytesinfile); 
            } 
        } 
    }
}