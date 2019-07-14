using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Security.Application;

namespace KDWechat.DBUtility
{
    public static class SQLExecuteFilter
    {
        public static int ExecuteNonQueryFilter(this SqlCommand cmd,bool filter =true)
        {
            if (filter)
            {
                if ((cmd.CommandText.ToLower().Contains("insert") || cmd.CommandText.ToLower().Contains("update")))
                {
                    cmd.CommandText = HttpUtility.HtmlDecode( Sanitizer.GetSafeHtmlFragment(cmd.CommandText ?? ""));//Regex.Replace(cmd.CommandText, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                }
                if (cmd.Parameters != null && cmd.Parameters.Count > 0)
                {
                    foreach (SqlParameter par in cmd.Parameters)
                    {
                        if (par.DbType == DbType.String)
                        {
                            par.Value = HttpUtility.HtmlDecode(Sanitizer.GetSafeHtmlFragment(par.Value == null ? "" : par.Value.ToString()));//Regex.Replace(par.Value.ToString(), @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                        }
                    }
                }
            }
            return cmd.ExecuteNonQuery();
        }

        public static SqlDataReader ExecuteReaderFilter(this SqlCommand cmd,CommandBehavior behav,bool filter = true)
        {
            if (filter)
            {
                if (cmd.CommandText.ToLower().Contains("insert") || cmd.CommandText.ToLower().Contains("update"))
                {
                    cmd.CommandText = cmd.CommandText = HttpUtility.HtmlDecode( Sanitizer.GetSafeHtmlFragment(cmd.CommandText ?? ""));//Regex.Replace(cmd.CommandText, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                }
                if (cmd.Parameters != null && cmd.Parameters.Count > 0)
                {
                    foreach (SqlParameter par in cmd.Parameters)
                    {
                        if (par.DbType == DbType.String)
                        {
                            par.Value = cmd.CommandText = HttpUtility.HtmlDecode(Sanitizer.GetSafeHtmlFragment(par.Value == null ? "" : par.Value.ToString()));//Regex.Replace(par.Value.ToString(), @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                        }
                    }
                }
            }
            return cmd.ExecuteReader(behav);
        }

    }
}
