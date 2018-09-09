using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Storichain.Models.Biz;
using Newtonsoft.Json;
using System.Globalization;

namespace Storichain.Controllers
{
    public class StepController : Controller
    {
        Biz_Step biz = new Biz_Step();

        public ActionResult Get() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            Biz_Product biz_product = new Biz_Product();
            DataTable dtProduct = biz_product.GetProductByStep(WebUtility.GetRequestByInt("event_idx"), 0);

            DataTable dtStep = biz.GetStep (    WebUtility.GetRequestByInt("event_idx"), 
                                                WebUtility.GetRequestByInt("step_idx"));

            dtStep.Columns.Add("product_data", typeof(DataTable));
            dtStep.Columns.Add("product_data_count", typeof(int));

            foreach(DataRow drStep in dtStep.Rows) 
            {
                drStep["product_data"] = DataTypeUtility.FilterSortDataTable(dtProduct, string.Format("step_idx = {0}", drStep["step_idx"]));
                drStep["product_data_count"] = ((DataTable)drStep["product_data"]).Rows.Count;
            }

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dtStep);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetVideoLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz.GetStepVideoLists(WebUtility.GetRequest("sort_order", "hot"), 
                                                 WebUtility.GetRequestByInt("step_type_idx", 0), 
                                                 WebUtility.GetRequestByInt("page", 1), 
                                                 WebUtility.GetRequestByInt("page_rows", 20),
                                                 out total_count,
                                                 out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetAll() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            Biz_Product biz_product = new Biz_Product();
            DataTable dtProduct = biz_product.GetProductByStep(WebUtility.GetRequestByInt("event_idx"), 0);

            DataTable dtStep = biz.GetStepAll ( WebUtility.GetRequestByInt("event_idx"), 
                                                WebUtility.GetRequestByInt("step_idx"));

            dtStep.Columns.Add("product_data", typeof(DataTable));
            dtStep.Columns.Add("product_data_count", typeof(int));

            foreach(DataRow drStep in dtStep.Rows) 
            {
                drStep["product_data"] = DataTypeUtility.FilterSortDataTable(dtProduct, string.Format("step_idx = {0}", drStep["step_idx"]));
                drStep["product_data_count"] = ((DataTable)drStep["product_data"]).Rows.Count;
            }

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dtStep);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        [ValidateInput(false)]
        public ActionResult Add() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            int step_idx;

            try
            {
                if(WebUtility.GetRequest("cover_check_yn", "N").Equals("Y")) 
                {
                    string file_path1 = "step";
                    string image_key1 = "";
                    ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                    int event_idx = WebUtility.GetRequestByInt("event_idx");

                    int file_idx = 0;

                    if(WebUtility.GetRequest("sticker_yn", "N").Equals("N"))
                        file_idx = (WebUtility.GetRequestByInt("step_file_idx") > 0)? WebUtility.GetRequestByInt("step_file_idx") : BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1);
                    else
                        file_idx = WebUtility.GetRequestByInt("sticker_file_idx");

                    object start_date = DBNull.Value;
                    object end_date = DBNull.Value;

                    if(WebUtility.GetRequestByInt("step_data_type_idx", 1) == 2)
                    {
                        start_date  = DateTime.ParseExact(WebUtility.GetRequest("start_date") + "000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                        end_date    = DateTime.ParseExact(WebUtility.GetRequest("end_date") + "235959", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    }
                    
                    step_idx = biz.AddStep( event_idx, 
                                            WebUtility.GetRequest("step_name"), 
                                            WebUtility.GetRequest("step_desc"),
                                            WebUtility.GetRequest("step_v_id"), 
                                            WebUtility.GetRequest("step_v_name"), 
                                            WebUtility.GetRequest("cover_yn", "N"), 
                                            WebUtility.GetRequestByFloat("step_pos_x"), 
                                            WebUtility.GetRequestByFloat("step_pos_y"), 
                                            WebUtility.GetRequest("step_url").Trim(), 
                                            WebUtility.GetRequest("step_pic_name"), 
                                            WebUtility.GetRequest("step_writer_name"),
                                            file_idx,
                                            WebUtility.GetRequest("sticker_yn", "N"),
                                            0,
                                            WebUtility.GetDeviceTypeIdx(),
                                            WebUtility.GetRequestByInt("step_data_type_idx", 1), 
                                            start_date,
                                            end_date,
                                            DateTime.Now, 
                                            WebUtility.UserIdx());


                    Biz_Supply biz_s = new Biz_Supply(event_idx);

                    if(biz_s.File_Idx == 0) 
                    {
                         DataTable dt = biz.GetStepOrigin(event_idx, step_idx);

                        if(dt.Rows.Count > 0) 
                        {
                            biz_s.ModifySupplyFile(event_idx, DataTypeUtility.GetToInt32(dt.Rows[0]["step_file_idx"]));
                        }
                    }    
                }
                else 
                {
                    string file_path1 = "step";
                    string image_key1 = "";
                    ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                    int event_idx = WebUtility.GetRequestByInt("event_idx");

                    int file_idx = 0;

                    if(WebUtility.GetRequest("sticker_yn", "N").Equals("N"))
                        file_idx = (WebUtility.GetRequestByInt("step_file_idx") > 0)? WebUtility.GetRequestByInt("step_file_idx") : BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1);
                    else
                        file_idx = WebUtility.GetRequestByInt("sticker_file_idx");

                    object start_date = DBNull.Value;
                    object end_date = DBNull.Value;

                    if(WebUtility.GetRequestByInt("step_data_type_idx", 1) == 2)
                    {
                        start_date  = DateTime.ParseExact(WebUtility.GetRequest("start_date") + "000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                        end_date    = DateTime.ParseExact(WebUtility.GetRequest("end_date") + "235959", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    }

                    step_idx = biz.AddStep( event_idx, 
                                            WebUtility.GetRequest("step_name"), 
                                            WebUtility.GetRequest("step_desc"),
                                            WebUtility.GetRequest("step_v_id"),
                                            WebUtility.GetRequest("step_v_name"),
                                            WebUtility.GetRequest("cover_yn", "N"),
                                            WebUtility.GetRequestByFloat("step_pos_x"), 
                                            WebUtility.GetRequestByFloat("step_pos_y"), 
                                            WebUtility.GetRequest("step_url").Trim(), 
                                            WebUtility.GetRequest("step_pic_name"), 
                                            WebUtility.GetRequest("step_writer_name"),
                                            file_idx,
                                            WebUtility.GetRequest("sticker_yn", "N"),
                                            0, 
                                            WebUtility.GetDeviceTypeIdx(),
                                            WebUtility.GetRequestByInt("step_data_type_idx", 1), 
                                            start_date,
                                            end_date,
                                            DateTime.Now, 
                                            WebUtility.UserIdx());
                }

                if(step_idx > 0) 
                {
                    DataTable dt = biz.GetStep (WebUtility.GetRequestByInt("event_idx"), step_idx);

                    if(WebUtility.GetRequest("cover_yn", "N").Equals("Y")) 
                    {
                        Biz_Event biz_ee  = new Biz_Event();
                        DataTable dt1     = BizUtility.GetImageData(biz_ee.GetEvent(WebUtility.GetRequestByInt("event_idx")));
                        MongoDBCommon.UpdateSupplyCoverImage(dt1);
                    }

                    // DB_T tbl_content 수기 UPDATE
                    Biz_Event bizE = new Biz_Event(WebUtility.GetRequestByInt("event_idx"));
                    if (bizE.PIdx != 0)
                    {
                        //Biz_Content bizC = new Biz_Content();
                        //bizC.ModifySelfDataYnByAdmin(bizE.PIdx);
                    }

                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
                }
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

        [ValidateInput(false)]
        public ActionResult Modify() 
        {
            string json = "";

            try
            {
                string file_path1 = "step";
                string image_key1 = "";
                ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                int file_idx = 0;

                if(WebUtility.GetRequest("sticker_yn", "N").Equals("N"))
                    file_idx = (WebUtility.GetRequestByInt("step_file_idx") > 0)? WebUtility.GetRequestByInt("step_file_idx") : BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1);
                else
                    file_idx = WebUtility.GetRequestByInt("sticker_file_idx");

                object start_date = DBNull.Value;
                object end_date = DBNull.Value;

                if(WebUtility.GetRequestByInt("step_data_type_idx", 1) == 2)
                {
                    start_date  = DateTime.ParseExact(WebUtility.GetRequest("start_date") + "000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    end_date    = DateTime.ParseExact(WebUtility.GetRequest("end_date") + "235959", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                }

                bool isOK = biz.ModifyStep( WebUtility.GetRequestByInt("event_idx"), 
                                            WebUtility.GetRequestByInt("step_idx"), 
                                            WebUtility.GetRequest("step_name"), 
                                            WebUtility.GetRequest("step_desc"), 
                                            WebUtility.GetRequestByInt("step_num", 0), 
                                            WebUtility.GetRequest("step_v_id", ""), 
                                            WebUtility.GetRequest("step_v_name", ""), 
                                            WebUtility.GetRequest("cover_yn", ""),
                                            WebUtility.GetRequest("step_url").Trim(), 
                                            WebUtility.GetRequest("step_pic_name"), 
                                            WebUtility.GetRequest("step_writer_name"),
                                            file_idx,
                                            WebUtility.GetRequest("sticker_yn", ""),
                                            WebUtility.GetRequestByInt("step_data_type_idx", 1), 
                                            start_date,
                                            end_date,
                                            DateTime.Now, 
                                            WebUtility.UserIdx());

                if(isOK) 
                {
                    if(WebUtility.GetRequest("cover_yn", "").Equals("Y")) 
                    {
                        Biz_Event biz_ee  = new Biz_Event();
                        DataTable dt1     = BizUtility.GetImageData(biz_ee.GetEvent(WebUtility.GetRequestByInt("event_idx")));
                        MongoDBCommon.UpdateSupplyCoverImage(dt1);    
                    }

                    DataTable dt = biz.GetStep (WebUtility.GetRequestByInt("event_idx"), WebUtility.GetRequestByInt("step_idx"));

                    // DB_T tbl_content 수기 UPDATE
                    Biz_Event bizE = new Biz_Event(WebUtility.GetRequestByInt("event_idx"));
                    if (bizE.PIdx != 0)
                    {
                        //Biz_Content bizC = new Biz_Content();
                        //bizC.ModifySelfDataYnByAdmin(bizE.PIdx);
                    }

                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
                }
                    
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

        public ActionResult Remove() 
        {
            string json = "";

            try
            {
                bool isOK = biz.RemoveStep( WebUtility.GetRequestByInt("event_idx"), 
                                            WebUtility.GetRequestByInt("step_idx"));

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

        public ActionResult RemoveAll() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("step_idxs")))
                message += "step_idxs is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                bool isOK = biz.RemoveStepAll(  WebUtility.GetRequestByInt("event_idx"), 
                                                WebUtility.GetRequest("step_idxs"));

                DataTable dt1 = biz.GetStep (    WebUtility.GetRequestByInt("event_idx"), 
                                                0);

                //if(dt1.Rows.Count == 0) 
                //{
                    Biz_Event biz_ee  = new Biz_Event();
                    DataTable dt      = BizUtility.GetImageData(biz_ee.GetEvent(WebUtility.GetRequestByInt("event_idx")));
                    MongoDBCommon.UpdateSupplyCoverImage(dt);    
                //}

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

        public ActionResult ChangeSort()
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("sort_values")))
                message += "sort_values is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                bool isOK = biz.ModifySort( WebUtility.GetRequestByInt("event_idx"),
                                            WebUtility.GetRequest("sort_values"),
                                            DateTime.Now, 
                                            WebUtility.UserIdx());

                if(isOK)
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                else
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);

                return Content(json, "application/json", System.Text.Encoding.UTF8);
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
