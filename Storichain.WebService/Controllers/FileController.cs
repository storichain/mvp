using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Text;
using System.Drawing.Imaging;

using System.Web.Script.Serialization;
using System.Runtime.InteropServices;

using Newtonsoft.Json;

using Storichain.Models.Biz;
using System.Drawing;
using System.Collections;

namespace Storichain.Controllers
{
    public class FileController : Controller
    {
        public ActionResult Upload()
		{
            string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("file_path")))
				message += "file_path is null.";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("temp_key")))
				message += "temp_key is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

            int file_idx        = WebUtility.GetRequestByInt("file_idx");
            string file_path    = WebUtility.GetRequest("file_path", "default");
            string temp_key     = WebUtility.GetRequest("temp_key");
            string file_key1    = WebUtility.GetRequest("file_key", file_path);
            string overwrite    = WebUtility.GetRequest("overwrite", "N");
            ArrayList list      = ImageUtility.GetImageSizes(file_path, ref file_key1);

            int f_idx = BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path, file_idx, list, file_key1);

            if(f_idx <= 0)
            {
                json = DataTypeUtility.JSon("2000", Config.R_FAIL, "not file : " + f_idx.ToValue(), null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            Biz_File biz_file   = new Biz_File();
            DataTable dt = biz_file.GetFile(f_idx);
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult Get()
		{
            string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("file_idx")))
				message += "file_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

            Biz_File biz_file   = new Biz_File();
            DataTable dt = biz_file.GetFile(WebUtility.GetRequestByInt("file_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult CheckMovieFile()
		{
            string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("file_idx")))
				message += "file_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

            Biz_File biz_file   = new Biz_File();
            DataTable dt = biz_file.GetFile(WebUtility.GetRequestByInt("file_idx"));

            foreach(DataRow dr in dt.Rows) 
            {
                if(dr["image_key"].ToValue().Contains("_small")) 
                {
                    string fileName = dr["image_url"].ToValue().Replace(WebUtility.GetConfig("DOWNLOAD_URL"), WebUtility.GetConfig("UPLOAD_DIR") + "/");

                    if(System.IO.File.Exists(fileName)) 
                    {
                        try
                        {
                           using (Stream stream = new FileStream(fileName, FileMode.Open))
                           {
                                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
			                return Content(json, "application/json", System.Text.Encoding.UTF8);
                           }
                        } 
                        catch 
                        {
                            json = DataTypeUtility.JSon("2702", Config.R_FAIL, "creating file", null);
			                return Content(json, "application/json", System.Text.Encoding.UTF8);
                        }
                    }
                    else 
                    {
                        json = DataTypeUtility.JSon("2701", Config.R_FAIL, "not file", null);
			            return Content(json, "application/json", System.Text.Encoding.UTF8);
                    }
                }
            }

			json = DataTypeUtility.JSon("2701", Config.R_FAIL, "not file", null);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult Remove()
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("file_idx")))
                message += "file_idx is null.";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("file_seq")))
                message += "file_seq is null.";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                Biz_File biz_file   = new Biz_File();
                int file_index = 0;
                
                string file_path = WebUtility.GetRequest("file_path", "none");

                if(file_path.Equals("none"))
                {
                    file_index = 1;
                }
                else 
                {
                    string file_key1 = "";
                    ArrayList list = ImageUtility.GetImageSizes(file_path, ref file_key1);
                    file_index = list.Count + 1;
                }

                for(int i = 0; i < file_index; i++) 
                {
                    biz_file.RemoveFile(WebUtility.GetRequestByInt("file_idx"), 
                                        WebUtility.GetRequestByInt("file_seq") + i);

                    int cache_type_idx = DataTypeUtility.GetToInt32(WebUtility.GetConfig("CACHE_TYPE_IDX", "0"));

                    if(cache_type_idx == 2) 
                        MongoDBCommon.RemoveFile(WebUtility.GetRequestByInt("file_idx"), WebUtility.GetRequestByInt("file_seq") + i);
                }

                DataTable dt = biz_file.GetFile(WebUtility.GetRequestByInt("file_idx"));

                //if(isOK)
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, file_index.ToString(), dt);
                //else
                //    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            }
            catch(Exception ex)
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult RemoveAll()
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("file_idx")))
                message += "file_idx is null.";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                Biz_File biz_file   = new Biz_File();
                bool isOK = biz_file.RemoveFileAll(WebUtility.GetRequestByInt("file_idx"));

                if(isOK)
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                else
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            }
            catch(Exception ex)
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }


	}
}










//Biz_File biz_file   = new Biz_File();
            //var fileCol         = Request.Files;
            //int file_idx        = WebUtility.GetRequestByInt("file_idx");
            //int file_seq        = 1;
            //string file_path    = WebUtility.GetRequest("file_path", "default");
            //string temp_key     = WebUtility.GetRequest("temp_key");
            //string file_key1    = WebUtility.GetRequest("file_key", file_path);
            //DataTable dt        = null;

            //ArrayList list = BizUtility.GetImageSizes(file_path, ref file_key1);

            //List<int> seqList = new List<int>();

            //foreach (string key in fileCol.AllKeys) 
            //{ 
            //    HttpPostedFileBase file = fileCol[key] as HttpPostedFileBase;

            //    if (file.ContentLength == 0)
            //        continue;

            //    string strDate  = "IMG_" + DateTime.Now.ToString("yyyy_MM_dd");
            //    string dir = string.Format("{0}/{1}/{2}", WebUtility.GetConfig("UPLOAD_DIR"), file_path, strDate);

            //    if(!Directory.Exists(dir))
            //        Directory.CreateDirectory(dir);

            //    string file_size        = fileCol[key].InputStream.Length.ToString();
            //    string file_key         = file_key1;
            //    string mime_type        = fileCol[key].ContentType;
            //    //string mime_type      = BizUtility.GetMimeFromFile(fileCol[key].InputStream);
            //    string file_ext         = Path.GetExtension(fileCol[key].FileName);
            //    string fileName         = "";
            //    string savedFileName    = "";
            //    DateTime dateTime       = DateTime.Now;

            //    biz_file.AddFileByWeb(  ref file_idx, 
            //                            ref file_seq, 
            //                            mime_type, 
            //                            "", 
            //                            file_ext, 
            //                            file_size, 
            //                            file_path, 
            //                            file_key,
            //                            temp_key,
            //                            0,
            //                            dateTime, 
            //                            0);

            //    if(file_idx == 0)
            //    {
            //        json = DataTypeUtility.JSon("2000", Config.R_FAIL, "not file", null);
            //        return Content(json, "application/json", System.Text.Encoding.UTF8);
            //    }

            //    fileName = Path.GetFileName(string.Format("{0}{1}", file_idx.ToString().PadLeft(9, '0'), file_seq.ToString().PadLeft(3, '0')));
            //    fileName += file_ext;
            //    savedFileName =  string.Format("{0}/{1}", dir, fileName);
            //    file.SaveAs(savedFileName);

            //    seqList.Add(file_seq);
            //    file_seq++;

            //    if(mime_type.Contains("video")) 
            //    {
            //        if(WebUtility.GetConfig("VIDEO_USE_YN", "N").Equals("Y")) 
            //        {
            //            string file_video_key = "video";
            //            ArrayList list_v = BizUtility.GetImageSizes(file_video_key, ref file_video_key);

            //            for(int v = 1; v <= list_v.Count; v++) 
            //            {
            //                Dictionary<string, object> item_v = (Dictionary<string, object>)list_v[v - 1];

            //                string file_name1 = Path.GetFileName(string.Format("{0}{1}", file_idx.ToString().PadLeft(9, '0'), v.ToString().PadLeft(3, '0'))) + ".jpg";
            //                string file_path1 = string.Format("{0}/{1}", dir, file_name1);

            //                if(DataTypeUtility.GetVideoThumbnail(WebUtility.GetConfig("FFMPEG_DIR") + "\\ffmpeg.exe", savedFileName, file_path1, string.Format("{0}*{1}", item_v["width"], item_v["height"])))
            //                {
            //                    biz_file.AddFileByWeb(  ref file_idx,
            //                                            ref file_seq,
            //                                            "image/jpeg",
            //                                            "",
            //                                            ".jpg",
            //                                            "0",
            //                                            file_path,
            //                                            item_v["image_key"].ToValue(),
            //                                            temp_key,
            //                                            0,
            //                                            dateTime,
            //                                            0);

            //                    biz_file.ModifyFile(file_idx,
            //                                        file_seq,
            //                                        "image/jpeg",
            //                                        file_name1,
            //                                        ".jpg",
            //                                        "0",
            //                                        file_path,
            //                                        item_v["image_key"].ToValue(),
            //                                        dateTime,
            //                                        0);

            //                    seqList.Add(file_seq);
            //                    file_seq++;;
            //                }
            //            }
            //        }

            //        var fileInfo = new FileInfoData
            //        {
            //            file_idx        = file_idx,
            //            file_seq        = file_seq,
            //            file_path       = file_path,
            //            file_dir        = dir,
            //            file_name       = fileName,
            //            file_save_path  = savedFileName,
            //            file_ext        = file_ext,
            //            file_size       = file_size,
            //            file_key        = file_key,
            //            mime_type       = mime_type,
            //            temp_key        = temp_key,
            //            create_date     = dateTime
            //        };

            //        var mdsClient = new MDSClient("w3wp");
            //        mdsClient.Connect();

            //        var requestMessage = mdsClient.CreateMessage();
            //        requestMessage.DestinationApplicationName = "MovieConverter";
            //        requestMessage.MessageData = GeneralHelper.SerializeObject(fileInfo);
            //        requestMessage.Send();
                
            //        mdsClient.Disconnect();
            //    }

            //    if(mime_type.Contains("video")) 
            //    {
            //        continue;
            //    }
            //    else 
            //    {
            //        Image img1 = Image.FromFile(savedFileName);

            //        int limitSise = 1280;

            //        if (img1.Width > limitSise || img1.Height > limitSise) 
            //        {
            //            var newImage = BizUtility.ScaleImage(img1, limitSise, limitSise);
            //            savedFileName = string.Format("{0}/{1}", dir, "mod_" + fileName);
            //            newImage.Save(savedFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            //            newImage.Dispose();

            //            biz_file.ModifyFile(file_idx,
            //                                file_seq,
            //                                mime_type,
            //                                "mod_" + fileName,
            //                                file_ext,
            //                                file_size,
            //                                file_path,
            //                                file_key,
            //                                dateTime,
            //                                0);
            //        }

            //        img1.Dispose();

            //        BizUtility.SaveRotation(file_path);
            //        main_file_path = savedFileName;

            //        if(list != null) 
            //        {
            //            for(int i = 0; i < list.Count; i++) 
            //            {
            //                Dictionary<string, object> item = (Dictionary<string, object>)list[i];

            //                Image imageOrigin = Image.FromFile(main_file_path);
            //                Image img = DataTypeUtility.Crop(imageOrigin, (int)item["width"] * 2, (int)item["height"] * 2);

            //                string file_name = Path.GetFileName(string.Format("{0}{1}", file_idx.ToString().PadLeft(9, '0'), file_seq.ToString().PadLeft(3, '0')));
            //                file_name += file_ext;
            //                string savedFileName2 = string.Format("{0}/{1}", dir, file_name);
            //                img.Save(savedFileName2, System.Drawing.Imaging.ImageFormat.Png);
            //                img.Dispose();
            //                imageOrigin.Dispose();

            //                biz_file.AddFileByWeb(  ref file_idx, 
            //                                        ref file_seq, 
            //                                        mime_type, 
            //                                        file_name, 
            //                                        file_ext, 
            //                                        new FileInfo(savedFileName2).Length.ToString(), 
            //                                        file_path, 
            //                                        item["image_key"].ToString(),
            //                                        temp_key,
            //                                        0,
            //                                        dateTime, 
            //                                        0);

            //                seqList.Add(file_seq);
            //                file_seq++;
            //            }
            //        }
            //    }
            //}

            //dt = null;
            //foreach(int seq in seqList) 
            //{
            //    if(dt == null) 
            //        dt = biz_file.GetFileWithCacheData(file_idx, seq);
            //    else
            //        dt.Merge(biz_file.GetFileWithCacheData(file_idx, seq));
            //}