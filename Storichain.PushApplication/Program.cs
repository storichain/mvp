using System;
using System.IO;
using System.Data;
using System.Text;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using PushSharp.Apple;
using PushSharp.Google;

namespace Storichain.PushApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = WebUtility.GetConfig("RABBITMQ_HOST"), UserName = WebUtility.GetConfig("RABBITMQ_ID"), Password = WebUtility.GetConfig("RABBITMQ_HOST") };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: WebUtility.GetConfig("RABBITMQ_QUE_NAME"),
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);

                PushNotificationMessageData message = null;
                    
                consumer.Received += (model, ea) => {
                    var body    = ea.Body;
                    var messageString = Encoding.UTF8.GetString(body);

                    message = JsonConvert.DeserializeObject<PushNotificationMessageData>(messageString);

                    if (message.to_user_idx_data != null)
                    {
                        if (message.to_user_idx_data.Rows.Count > 0)
                        {
                            ShowConsoleMessage(message.to_user_idx_data, message.user_idx.ToString(), message.biz_type);

                            string p12Filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, WebUtility.GetConfig("IOS_CERT_FILE_NAME", "production.p12"));
                            ApnsConfiguration.ApnsServerEnvironment env = ApnsConfiguration.ApnsServerEnvironment.Production;

                            if (WebUtility.GetConfig("IOS_CERT_PRODUCTION_YN", "Y").Equals("Y"))
                            {
                                p12Filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, WebUtility.GetConfig("IOS_CERT_FILE_NAME", "production.p12"));
                                env = ApnsConfiguration.ApnsServerEnvironment.Production;
                            }
                            else
                            {
                                p12Filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, WebUtility.GetConfig("IOS_CERT_FILE_NAME_DEV", "dev.p12"));
                                env = ApnsConfiguration.ApnsServerEnvironment.Sandbox;
                            }

                            var appleCert = File.ReadAllBytes(p12Filename);
                            ApnsServiceBroker broker_apns = new ApnsServiceBroker(new ApnsConfiguration(env, appleCert, WebUtility.GetConfig("IOS_CERT_FILE_PWD")));
                            broker_apns.OnNotificationFailed += (notification, exception) =>
                            {

                                Console.WriteLine("=========== APN Fail : " + notification.DeviceToken + " Msg : " + exception.Message);
                            };
                            broker_apns.OnNotificationSucceeded += (notification) =>
                            {

                                Console.WriteLine("=========== APN Sent: " + notification.DeviceToken);
                            };

                            broker_apns.Start();

                            var config_gcm = new GcmConfiguration(WebUtility.GetConfig("ANDROID_AUTH_TOKEN"));
                            var broker_gcm = new GcmServiceBroker(config_gcm);
                            broker_gcm.OnNotificationFailed += (notification, exception) =>
                            {
                                Console.WriteLine("=========== GCM Fail : " + notification.RegistrationIds[0] + " Msg : " + exception.Message);
                            };
                            broker_gcm.OnNotificationSucceeded += (notification) =>
                            {
                                Console.WriteLine("=========== GCM Sent: " + notification.RegistrationIds[0]);
                            };

                            broker_gcm.Start();

                            foreach (DataRow dr in message.to_user_idx_data.Rows)
                            {
                                if (DataTypeUtility.GetToInt32(dr["device_type_idx"]) == 1)
                                {
                                    broker_apns.QueueNotification(new ApnsNotification
                                    {

                                        DeviceToken = dr["device_token"].ToString(),
                                        Payload = JObject.Parse("{\"aps\" : {\"alert\" : \"" + dr["body_text"].ToString() + "\",\"badge\" : " + Convert.ToInt32(dr["not_view_count"].ToString()) + ",\"sound\" : \"default\"}," +
                                                         "\"from_user_idx\" : " + message.user_idx.ToString() + "," +
                                                         "\"to_user_idx\":" + dr["to_user_idx"].ToString() + "," +
                                                         "\"biz_type\" : \"" + message.biz_type + "\"," +
                                                         "\"custom_data\":\"" + dr["custom_data"].ToString() + "\"," +
                                                         "\"notice_idx\":\"" + dr["notice_idx"].ToString() + "\"" +
                                                         "}")
                                    });
                                }
                                else if (DataTypeUtility.GetToInt32(dr["device_type_idx"]) == 2)
                                {
                                    broker_gcm.QueueNotification(new GcmNotification
                                    {

                                        RegistrationIds = new List<string> {
                                                    dr["device_token"].ToString()
                                                },
                                        Data = JObject.Parse("{" +
                                                                    "\"title\":\"" + dr["body_text"].ToString() + "\"," +
                                                                    "\"badge\":" + dr["not_view_count"].ToString() + "," +
                                                                    "\"from_user_idx\":" + message.user_idx.ToString() + "," +
                                                                    "\"to_user_idx\":" + dr["to_user_idx"].ToString() + "," +
                                                                    "\"biz_type\":\"" + message.biz_type + "\"," +
                                                                    "\"custom_data\":\"" + dr["custom_data"].ToString() + "\"," +
                                                                    "\"notice_idx\":\"" + dr["notice_idx"].ToString() + "\"," +
                                                                    "\"sound\":\"default\"}")
                                    });
                                }
                            }

                            broker_apns.Stop();
                            broker_gcm.Stop();
                        }
                    }
                    else
                    {
                        if (message.device_type_idx == 1)
                        {
                            string p12Filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, WebUtility.GetConfig("IOS_CERT_FILE_NAME", "production.p12"));
                            ApnsConfiguration.ApnsServerEnvironment env = ApnsConfiguration.ApnsServerEnvironment.Production;

                            if (WebUtility.GetConfig("IOS_CERT_PRODUCTION_YN", "Y").Equals("Y"))
                            {
                                p12Filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, WebUtility.GetConfig("IOS_CERT_FILE_NAME", "production.p12"));
                                env = ApnsConfiguration.ApnsServerEnvironment.Production;
                            }
                            else
                            {
                                p12Filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, WebUtility.GetConfig("IOS_CERT_FILE_NAME_DEV", "dev.p12"));
                                env = ApnsConfiguration.ApnsServerEnvironment.Sandbox;
                            }

                            var appleCert = File.ReadAllBytes(p12Filename);
                            var broker = new ApnsServiceBroker(new ApnsConfiguration(env, appleCert, WebUtility.GetConfig("IOS_CERT_FILE_PWD")));

                            broker.OnNotificationFailed += (notification, exception) =>
                            {

                                Console.WriteLine("=========== APN Fail : " + notification.DeviceToken + " Msg : " + exception.Message);
                            };
                            broker.OnNotificationSucceeded += (notification) =>
                            {

                                Console.WriteLine("=========== APN Sent: " + notification.DeviceToken);
                            };

                            broker.Start();

                            broker.QueueNotification(new ApnsNotification
                            {
                                DeviceToken = message.device_token,
                                Payload = JObject.Parse("{\"aps\" : {\"alert\" : \"" + message.message + "\",\"badge\" : " + message.badge + ",\"sound\" : \"default\"}," +
                                                         "\"from_user_idx\" : " + message.user_idx.ToString() + "," +
                                                         "\"biz_type\" : \"" + message.biz_type + "\"" +
                                                         "}")
                            });

                            broker.Stop();
                        }
                        else if (message.device_type_idx == 2)
                        {
                            var config = new GcmConfiguration(WebUtility.GetConfig("ANDROID_AUTH_TOKEN"));
                            var broker = new GcmServiceBroker(config);
                            broker.OnNotificationFailed += (notification, exception) =>
                            {
                                Console.WriteLine("=========== GCM Fail : " + notification.RegistrationIds[0] + " Msg : " + exception.Message);
                            };
                            broker.OnNotificationSucceeded += (notification) =>
                            {
                                Console.WriteLine("=========== GCM Sent: " + notification.RegistrationIds[0]);
                            };

                            broker.Start();

                            broker.QueueNotification(new GcmNotification
                            {
                                RegistrationIds = new List<string> {
                                message.device_token
                            },
                                Data = JObject.Parse("{" +
                                                        "\"title\":\"" + message.message + "\"," +
                                                        "\"badge\":" + message.badge + "," +
                                                        "\"from_user_idx\":" + message.user_idx.ToString() + "," +
                                                        "\"biz_type\":\"" + message.biz_type + "\"," +
                                                        "\"sound\":\"default\"}")
                            });

                            broker.Stop();
                        }

                        ShowConsoleMessage(message);
                    }

                    //Console.WriteLine(" [x] Received {0}", message);
                    //int dots = message.Split('.').Length - 1;
                    //Thread.Sleep(dots * 1000);

                    //Console.WriteLine(" [x] Done");

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                channel.BasicConsume(queue: WebUtility.GetConfig("RABBITMQ_QUE_NAME"),
                                        autoAck: false,
                                        consumer: consumer);
            }

            string strProduction = (WebUtility.GetConfig("IOS_CERT_PRODUCTION_YN", "Y").Equals("Y")) ? "Production" : "Development";
            Console.WriteLine(WebUtility.GetConfig("APP_TITLE", "") + " " + strProduction + "~");

            Console.WriteLine("Hi~ Press enter to exit...");
            Console.ReadLine();
        }

        static void ShowConsoleMessage(PushNotificationMessageData message)
        {
            Console.WriteLine();
            Console.WriteLine("---------------------------------");
            Console.WriteLine("from_user_idx : " + message.user_idx);
            Console.WriteLine("to_user_idx : " + message.to_user_idx);
            Console.WriteLine("device_type_idx : " + message.device_type_idx);
            Console.WriteLine("send date : " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
            Console.WriteLine("device token : " + message.device_token);
            Console.WriteLine("badge : " + message.badge);
            Console.WriteLine("body : " + message.body);
            Console.WriteLine("message : " + message.message);
            Console.WriteLine("sound : " + message.sound);
            Console.WriteLine("biz_type : " + message.biz_type);

            if (message.biz_type.Equals("message"))
                Console.WriteLine("message_idx : " + message.message_idx);
            else if (message.biz_type.Equals("notice"))
                Console.WriteLine("notice_idx : " + message.notice_idx);

            Console.WriteLine("---------------------------------");
        }

        static void ShowConsoleMessage(DataRow dr, string from_user_idx, string biz_type)
        {
            Console.WriteLine();
            Console.WriteLine("---------------------------------");
            Console.WriteLine("from_user_idx : " + from_user_idx);
            Console.WriteLine("to_user_idx : " + dr["to_user_idx"].ToString());
            Console.WriteLine("device_type_idx : " + dr["device_type_idx"].ToString());
            Console.WriteLine("send date : " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
            Console.WriteLine("device_token : " + dr["device_token"].ToString());
            Console.WriteLine("badge : " + dr["not_view_count"].ToString());
            Console.WriteLine("body : " + dr["body_text"].ToString());
            Console.WriteLine("biz_type : " + biz_type);
            Console.WriteLine("---------------------------------");
        }

        static void ShowConsoleMessage(DataTable dataTable, string from_user_idx, string biz_type)
        {
            if (dataTable.Rows.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("---------------------------------");
                Console.WriteLine("total count : " + dataTable.Rows.Count);
                Console.WriteLine("from_user_idx : " + from_user_idx);
                Console.WriteLine("send date : " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
                Console.WriteLine("biz_type : " + biz_type);
                Console.WriteLine("---------------------------------");
            }
        }
    }
}

