using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Storichain.Models;
using Storichain.Models.Biz;

using System.Drawing;
using System.Net;
using System.IO;

namespace Storichain
{
    public class WorkHelper
    {
        static Biz_Message biz         = new Biz_Message();
        static Biz_Device biz_device   = new Biz_Device();
        static Biz_User biz_user       = new Biz_User();

        public static bool isDuplicateWork(string biz_type, int event_idx, int user_idx, double limitSec) 
        {
            return MongoDBCommon.DuplicateChecker(biz_type, event_idx, user_idx, limitSec);

            //if(DataCacher.DataNoDuplicate == null) 
            //{
            //    DataCacher.DataNoDuplicate = new DataTable();
            //    DataCacher.DataNoDuplicate.Columns.Add("biz_type", typeof(string));
            //    DataCacher.DataNoDuplicate.Columns.Add("event_idx", typeof(int));
            //    DataCacher.DataNoDuplicate.Columns.Add("user_idx", typeof(int));
            //    DataCacher.DataNoDuplicate.Columns.Add("date", typeof(DateTime));
            //}

            //DataRow[] rowCol = DataCacher.DataNoDuplicate.Select(string.Format("event_idx = {0} AND biz_type = '{1}'", 
            //                                                        event_idx, 
            //                                                        biz_type));

            //if(rowCol.Length > 0) 
            //{
            //    DateTime dateOne    = DateTime.Now;
            //    DateTime dateTwo    = (DateTime)rowCol[0]["date"];
            //    TimeSpan diff       = dateOne.Subtract(dateTwo);

            //    if(diff.TotalSeconds <= limitSec) 
            //    {
            //        rowCol[0]["date"] = DateTime.Now;
            //        DataCacher.DataNoDuplicate.AcceptChanges();

            //        return true;
            //    }
            //    else 
            //    {
            //        rowCol[0]["date"] = DateTime.Now;
            //        DataCacher.DataNoDuplicate.AcceptChanges();
            //    }
            //}
            //else 
            //{
            //    DataRow dr          = DataCacher.DataNoDuplicate.NewRow();
            //    dr["biz_type"]      = biz_type;
            //    dr["event_idx"]     = event_idx;
            //    dr["user_idx"]      = user_idx;
            //    dr["date"]          = DateTime.Now;
            //    DataCacher.DataNoDuplicate.Rows.Add(dr);
            //    DataCacher.DataNoDuplicate.AcceptChanges();
            //}

            //return false;
        }

    }
}