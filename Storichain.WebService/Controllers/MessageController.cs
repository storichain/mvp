using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Text;
using Storichain.Models.Biz;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Storichain.Controllers
{
    public class MessageController : Controller
    {
        Biz_Message biz         = new Biz_Message();
        Biz_Device biz_device   = new Biz_Device();
        Biz_User biz_user       = new Biz_User();

        protected override void Initialize( System.Web.Routing.RequestContext rc) 
        {
            base.Initialize(rc);
        }

        public ActionResult Get()
        {
            string json = "";

            DataTable dt = biz.GetMessage(  WebUtility.GetRequestByInt("message_idx"),
                                            WebUtility.GetRequestByInt("from_user_idx"),
                                            WebUtility.GetRequestByInt("to_user_idx"),
                                            WebUtility.GetRequest("msg_status_type"));

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Count()
        {
            string json     = "";
            string message  = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
                message += "to_user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            int badge = biz.GetCount(WebUtility.GetRequestByInt("to_user_idx"), "1");
            json = DataTypeUtility.JSon("1000", "", Config.R_SUCCESS, badge.ToString());
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult NotReadList()
        {
            string json     = "";
            string message  = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
                message += "to_user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetMessageByFromIDX(WebUtility.GetRequestByInt("to_user_idx"), "1");

            DataTable dtNot = new DataTable();
            DataRow drNot;
            dtNot.Columns.Add("from_user_idx", typeof(int));
            dtNot.Columns.Add("message_count", typeof(int));
            dtNot.Columns.Add("message", typeof(DataTable));

            foreach(DataRow dr in dt.Rows) 
            {
                drNot = dtNot.NewRow();
                drNot["from_user_idx"]  = dr["from_user_idx"];
                drNot["message_count"]  = dr["message_count"];
                drNot["message"]        = biz.GetMessage(   0,
                                                            (int)dr["from_user_idx"],
                                                            WebUtility.GetRequestByInt("to_user_idx"),
                                                            "1");
                dtNot.Rows.Add(drNot);
            }

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dtNot);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult NotReadData()
        {
            string json     = "";
            string message  = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
                message += "to_user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetMessageByFromIDX(WebUtility.GetRequestByInt("to_user_idx"), "1");

            DataTable dtNot = new DataTable();
            DataRow drNot;
            dtNot.Columns.Add("from_user_idx", typeof(int));
            dtNot.Columns.Add("message_count", typeof(int));
            dtNot.Columns.Add("message", typeof(DataTable));

            foreach(DataRow dr in dt.Rows) 
            {
                drNot = dtNot.NewRow();
                drNot["from_user_idx"]  = dr["from_user_idx"];
                drNot["message_count"]  = dr["message_count"];
                drNot["message"]        = biz.GetMessageTop( 0,
                                                            (int)dr["from_user_idx"],
                                                            WebUtility.GetRequestByInt("to_user_idx"),
                                                            "1");
                dtNot.Rows.Add(drNot);
            }

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dtNot);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult NotReadCount()
        {
            string json     = "";
            string message  = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
                message += "to_user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            int count = biz.GetMessageCount(WebUtility.GetRequestByInt("to_user_idx"), "1");

            Dictionary<string, int> dic = new Dictionary<string,int>();
            dic.Add("message_count", count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        private static int m_idx_temp;

        [ValidateInput(false)]
        public ActionResult Send()
        {
            string json = "";

            int from_idx         = WebUtility.GetRequestByInt("from_user_idx");
            string[] strIDXs     = WebUtility.GetRequest("to_user_idx").Split(',');
            string msg           = WebUtility.GetRequest("message", true);
            string msg_type      = WebUtility.GetRequest("msg_status_type", "1");
            string msg_data_type = WebUtility.GetRequest("msg_data_type");

            int device_type_idx1 = 0;

            if(WebUtility.GetDeviceTypeIdx() == 0)
                device_type_idx1 =  WebUtility.GetRequestByInt("device_type_idx", 2);
            else
                device_type_idx1 =  WebUtility.GetDeviceTypeIdx();

            string user_data     = WebUtility.GetRequest("user_data");
            string sticker_yn    = WebUtility.GetRequest("sticker_yn", "N");

            if(WebUtility.GetConfig("BLOCK_USE_YN", "N").Equals("Y"))
            {
                Biz_UserBlock biz_block = new Biz_UserBlock(WebUtility.GetRequestByInt("from_user_idx"));
                if(biz_block.Message_Block_YN.Equals("Y") || biz_block.User_Block_YN.Equals("Y"))
                {
                    json = DataTypeUtility.JSon("2500", Config.R_SUCCESS, "block", null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }
                else if(biz_block.Enable_YN.Equals("N"))
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    json = DataTypeUtility.JSon("2500", Config.R_SUCCESS, "Logout", null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }    
            }

            //int from_idx        = 2;
            //string[] strIDXs    = string.Format("3").Split(',');
            //string msg          = "test";
            //string msg_type     = "1";

            for(int i = 0; i < strIDXs.Length; i++) 
            {
                if(WorkHelper.isDuplicateWork("Message/Send", i, WebUtility.GetRequestByInt("from_user_idx"), 0.3))
                {
                    json = DataTypeUtility.JSon("2001", Config.R_PROCESSING_NOW, "", null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                int to_idx      = DataTypeUtility.GetToInt32(strIDXs[i]);

                Biz_Report biz_report = new Biz_Report();
                DataTable dtReport = biz_report.GetUserBlock(from_idx, to_idx);

                if(dtReport.Rows.Count > 0) 
                {
                    if(DataTypeUtility.GetValue(dtReport.Rows[0]["block_yn"], "N").Equals("Y"))
                    {
                        json = DataTypeUtility.JSon("2500", Config.R_SUCCESS, "block", null);
                        return Content(json, "application/json", System.Text.Encoding.UTF8);
                    }
                }

                string deviceToken  = "";
                string user_name    = "";
                int device_type_idx = 0;
                string logined_yn   = "N";
                    
                DataTable dt        = biz_device.GetDevice(to_idx, "", "");
                DataTable dtUser    = biz_user.GetUser(from_idx, 0, "", "Y");

                string file_path1 = "message";
                string image_key1 = "";
                ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                if(dt.Rows.Count > 0)
                {
                    deviceToken  = dt.Rows[0]["device_token"].ToString();
                    logined_yn   = dt.Rows[0]["logined_yn"].ToString();
                }
                else
                {
                    //BizUtility.SendErrorLog(new Exception(string.Format("message => {0}의 토큰이 없습니다. ", to_idx)), msg);

                    m_idx_temp = biz.AddMessage( from_idx,
                                                to_idx,
                                                msg,
                                                msg_type,
                                                msg_data_type,
                                                DateTime.Now,
                                                sticker_yn,
                                                BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1),
                                                DateTime.Now,
                                                from_idx);

                    break;
                    //continue;
                }

                if (dtUser.Rows.Count > 0) 
                {
                    user_name    = dtUser.Rows[0]["nick_name"].ToString();
                }

                DataTable dtIdx = biz_device.GetDevice(0, "", deviceToken);

                if(dtIdx.Rows.Count > 0)
                    device_type_idx    = DataTypeUtility.GetToInt32(dtIdx.Rows[0]["device_type_idx"]);

                int m_idx = biz.AddMessage( from_idx,
                                            to_idx,
                                            msg,
                                            msg_type,
                                            msg_data_type,
                                            DateTime.Now,
                                            sticker_yn,
                                            BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1),
                                            DateTime.Now,
                                            from_idx);

                m_idx_temp = m_idx;

                /// 웹방식
                //var push = new PushBroker();

                //push.OnNotificationSent += NotificationSent;
                //push.OnChannelException += ChannelException;
                //push.OnServiceException += ServiceException;
                //push.OnNotificationFailed += NotificationFailed;
                //push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
                //push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
                //push.OnChannelCreated += ChannelCreated;
                //push.OnChannelDestroyed += ChannelDestroyed;

                //if(device_type_idx == 1) 
                //{
                //    var appleCert = System.IO.File.ReadAllBytes(Server.MapPath("../production.p12"));
                //    push.RegisterAppleService(new ApplePushChannelSettings(appleCert, "1111"));
                    
                //    push.QueueNotification(new AppleNotification()
                //                               .ForDeviceToken(deviceToken)
                //                               .WithAlert(string.Format("{0}님의 메세지", user_name))
                //                               .WithBadge(biz.GetCount(to_idx, "1"))
                //                               .WithSound("default"));
                //}
                //else if(device_type_idx == 2) 
                //{
                //    push.RegisterGcmService(new GcmPushChannelSettings("AIzaSyCZk9WQzZzfMIoO9H-YyjjawVd70RMtiUU"));
                //    push.QueueNotification(new GcmNotification().ForDeviceRegistrationId(deviceToken)
                //        .WithJson("{\"alert\":\"" + string.Format("{0}님의 메세지", user_name) + "\",\"badge\":" + biz.GetCount(to_idx, "1").ToString() + ",\"sound\":\"default\"}"));
                //}

                if(!deviceToken.Equals("") && logined_yn.Equals("Y")) 
                {
                    //큐방식
                    var factory = new ConnectionFactory() { HostName = WebUtility.GetConfig("RABBITMQ_HOST"), UserName = WebUtility.GetConfig("RABBITMQ_ID"), Password = WebUtility.GetConfig("RABBITMQ_HOST") };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel()) {
                        channel.QueueDeclare(queue: WebUtility.GetConfig("RABBITMQ_QUE_NAME"),
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;

                        var messageData = new PushNotificationMessageData 
                        {
                            device_token    = deviceToken,   
                            device_type_idx = device_type_idx,
                            badge           = biz.GetCount(to_idx, "1"),
                            body            = string.Format("{0}님의 메세지", user_name),
                            message         = msg,
                            sound           = "default",
                            biz_type        = "message",
                            user_idx   = from_idx,
                            to_user_idx     = to_idx,
                            message_idx     = m_idx
                        };

                        var messageString = JsonConvert.SerializeObject(messageData);
                        var body = Encoding.UTF8.GetBytes(messageString);

                        channel.BasicPublish(exchange: "",
                                             routingKey: WebUtility.GetConfig("RABBITMQ_QUE_NAME"),
                                             basicProperties: properties,
                                             body: body);
                    }
                }
            }

            DataTable dt2 = null;

            if(m_idx_temp > 0) 
            {
                dt2 = biz.GetMessage(m_idx_temp,0,0,"");

                dt2.Columns.Add("user_data", typeof(string));

                foreach(DataRow dr in dt2.Rows) 
                {
                    dr["user_data"] = user_data;
                }
            }

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt2);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult ModifyStatus()
        {
            string json     = "";
            string message  = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("from_user_idx")))
                message += "from_user_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
                message += "to_user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            bool isOK = false;

            isOK = biz.ModifyMessageStatusType( WebUtility.GetRequestByInt("from_user_idx"),
                                                WebUtility.GetRequestByInt("to_user_idx"),
                                                WebUtility.GetRequest("msg_status_type", "0"),
                                                DateTime.Now);
            if(isOK) 
                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
            else
                json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Modify()
        {
            string json = "";

            bool isOK = false;

            string[] m_idxs = WebUtility.GetRequest("message_idx").Split(',');

            foreach(string m_idx in m_idxs) 
            {
                isOK = biz.ModifyMessageStatusType( DataTypeUtility.GetToInt32(m_idx),
                                                    WebUtility.GetRequest("msg_status_type"),
                                                    DateTime.Now);
            }                

            if(isOK) 
                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
            else
                json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }




        //public void DeviceSubscriptionChanged(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification)
        //{
        //    //Currently this event will only ever happen for Android GCM
        //    Console.WriteLine("Device Registration Changed:  Old-> " + oldSubscriptionId + "  New-> " + newSubscriptionId + " -> " + notification);
        //}

        //public void NotificationSent(object sender, INotification notification)
        //{
        //    Console.WriteLine("Sent: " + sender + " -> " + notification);
        //}

        //public void NotificationFailed(object sender, INotification notification, Exception notificationFailureException)
        //{
        //    Console.WriteLine("Failure: " + sender + " -> " + notificationFailureException.Message + " -> " + notification);
        //}

        //public void ChannelException(object sender, IPushChannel channel, Exception exception)
        //{
        //    Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
        //}

        //public void ServiceException(object sender, Exception exception)
        //{
        //    Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
        //}

        //public void DeviceSubscriptionExpired(object sender, string expiredDeviceSubscriptionId, DateTime timestamp, INotification notification)
        //{
        //    Console.WriteLine("Device Subscription Expired: " + sender + " -> " + expiredDeviceSubscriptionId);
        //}

        //public void ChannelDestroyed(object sender)
        //{
        //    Console.WriteLine("Channel Destroyed for: " + sender);
        //}

        //public void ChannelCreated(object sender, IPushChannel pushChannel)
        //{
        //    Console.WriteLine("Channel Created for: " + sender);
        //}



    }
}
