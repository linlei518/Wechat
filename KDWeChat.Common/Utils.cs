using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Configuration;
using System.Web;
using System.Security.Cryptography;
using System.Linq;
using System.Collections;
using System.Data;
using System.Reflection;

namespace KDWechat.Common
{
    public class Utils
    {


        #region MD5加密
        public static string MD5(string pwd)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(pwd);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            string str = "";
            for (int i = 0; i < md5data.Length; i++)
            {
                str += md5data[i].ToString("x").PadLeft(2, '0');

            }
            return str;
        }

        /// <summary>
        /// 验证码api的时间戳算法
        /// </summary>
        /// <param name="oldCode"></param>
        /// <param name="md5key"></param>
        /// <returns></returns>
        public static string ValidateApiTimeCode(string oldCode, string md5key = "Companycn")
        {

            //unix时间戳（10位）-随机串（6位）-fp（未知）
            //1439538035-damos2-xxxxxx
            if (string.IsNullOrWhiteSpace(oldCode))
                return "";


            var oldList = oldCode.Split('-');//根据oldCode分解成4组string
            if (oldList.Length != 3)
                return "";

            var lengthArr = new int[] { 10, 6 };
            for (int i = 0; i < 2; i++)
                if (oldList[i].Length != lengthArr[i])
                    return "";



            var sortedList = oldList.OrderBy(x => x).ToList();
            var str = MD5(GetArrayStr(sortedList, "^"));
            str = MD5(str.Insert(10, md5key));
            return str;
        }

        #endregion


        #region 对象转换处理
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            if (expression != null)
                return IsNumeric(expression.ToString());

            return false;

        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object expression)
        {
            if (expression != null)
                return Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");

            return false;
        }

        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
        }
        public static bool IsValidDoEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }

        /// <summary>
        /// 将字符串转换为数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字符串数组</returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new char[',']);
        }

        /// <summary>
        /// 将数组转换为字符串
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="speater">分隔符</param>
        /// <returns>String</returns>
        public static string GetArrayStr<T>(IEnumerable<T> list, char speater)
        {
            StringBuilder sb = new StringBuilder();
            var count = list.Count();
            foreach (var item in list)
            {
                sb.Append(item);
                sb.Append(speater);
            }
            return sb.ToString().TrimEnd(speater);
        }

        /// <summary>
        /// 将数组转换为字符串
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="speater">分隔符</param>
        /// <returns>String</returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// 将数组转换为字符串
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="speater">分隔符</param>
        /// <returns>String</returns>
        public static string GetIntArrayStr(List<int> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i].ToString());
                }
                else
                {
                    sb.Append(list[i].ToString());
                    sb.Append(speater.ToString());
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 将数组转换为字符串
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="speater">分隔符</param>
        /// <returns>String</returns>
        public static string GetArrayStr(string[] list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Length; i++)
            {
                if (i == list.Length - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// object型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
                return StrToBool(expression, defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            if (expression != null)
            {
                if (string.Compare(expression, "true", true) == 0)
                    return true;
                else if (string.Compare(expression, "false", true) == 0)
                    return false;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjToInt(object expression, int defValue)
        {
            if (expression != null)
                return StrToInt(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// 将字符串转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string expression, int defValue)
        {
            if (string.IsNullOrEmpty(expression) || expression.Trim().Length >= 11 || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            int rv;
            if (Int32.TryParse(expression, out rv))
                return rv;

            try
            {
                return Convert.ToInt32(StrToFloat(expression, defValue));
            }
            catch
            {
                return defValue;
            }
        }

        public static byte StrToByte(string str, byte defValue)
        {
            byte rv;
            if (byte.TryParse(str, out rv))
                return rv;
            else
                return 0;


        }

        public static bool ObjToBool(object expression, bool defValue = false)
        {
            if (expression != null)
            {
                bool result = false;
                bool.TryParse(expression.ToString(), out result);
                return result;
            }
            return defValue;
        }

        /// <summary>
        /// 将字符串转换为double类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的double类型结果</returns>
        public static double StrToDouble(string expression, double defValue)
        {
            var overLength = true;
            if (expression.Contains("."))
            {
                var array = expression.Split('.');
                if (array.Length != 2)
                    return defValue;
                else if (array[0].Length >= 11)
                    return defValue;
                else
                    overLength = false;
            }
            else
            {
                var intValue = StrToInt(expression, -int.MaxValue);
                return intValue == -int.MaxValue ? defValue : intValue;
            }
            if (string.IsNullOrEmpty(expression) || overLength || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            double rv;
            if (double.TryParse(expression, out rv))
                return rv;

            return Convert.ToDouble(defValue);
        }

        /// <summary>
        /// Object型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal ObjToDecimal(object expression, decimal defValue)
        {
            if (expression != null)
                return StrToDecimal(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(string expression, decimal defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            decimal intValue = defValue;
            if (expression != null)
            {
                bool IsDecimal = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsDecimal)
                    decimal.TryParse(expression, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// Object型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjToFloat(object expression, float defValue)
        {
            if (expression != null)
                return StrToFloat(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string expression, float defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            float intValue = defValue;
            if (expression != null)
            {
                bool IsFloat = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(expression, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str, DateTime defValue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime dateTime;
                if (DateTime.TryParse(str, out dateTime))
                    return dateTime;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str)
        {
            return StrToDateTime(str, DateTime.Now);
        }

        /// <summary>
        /// 将object转换为字符串，为空时返回""
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string objectToDateTime(object str)
        {
            if (str != null)
            {
                return ObjectToDateTime(str).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 中文转unicode
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns></returns>
        public static string StrToUnicode(string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                outStr = HttpUtility.UrlEncodeUnicode(str).Replace("%", "\\");
            }
            return outStr;
        }
        /// <summary>
        /// Unicode转中文字符串
        /// </summary>
        /// <param name="str">要转换的Unicode字符串</param>
        /// <returns></returns>
        public static string UnicodeToStr(string unicode_str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(unicode_str))
            {
                outStr = HttpUtility.UrlDecode(unicode_str.Replace("\\", "%"));
            }
            return outStr;
        }


        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj)
        {
            return StrToDateTime(obj.ToString());
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj, DateTime defValue)
        {
            return StrToDateTime(obj.ToString(), defValue);
        }

        /// <summary>
        /// 将对象转换为字符串
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的string类型结果</returns>
        public static string ObjectToStr(object obj)
        {
            if (obj == null)
                return "";
            return obj.ToString().Trim();
        }
        #endregion

        #region 分割字符串
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];
            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }
        #endregion

        #region 删除最后结尾的一个逗号
        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            if (str.Length < 1)
            {
                return "";
            }
            return str.Substring(0, str.LastIndexOf(","));
        }
        #endregion

        #region 删除最后结尾的指定字符后的字符
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            if (str.LastIndexOf(strchar) >= 0 && str.LastIndexOf(strchar) == str.Length - 1)
            {
                return str.Substring(0, str.LastIndexOf(strchar));
            }
            return str;
        }
        #endregion

        #region 生成指定长度的字符串
        /// <summary>
        /// 生成指定长度的字符串,即生成strLong个str字符串
        /// </summary>
        /// <param name="strLong">生成的长度</param>
        /// <param name="str">以str生成字符串</param>
        /// <returns></returns>
        public static string StringOfChar(int strLong, string str)
        {
            string ReturnStr = "";
            for (int i = 0; i < strLong; i++)
            {
                ReturnStr += str;
            }

            return ReturnStr;
        }
        #endregion

        #region 生成日期随机码
        /// <summary>
        /// 生成日期随机码
        /// </summary>
        /// <returns></returns>
        public static string GetRamCode()
        {
            #region
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
            #endregion
        }
        #endregion

        #region 生成随机字母或数字
        /// <summary>
        /// 生成一批非负随机整数
        /// </summary>
        /// <param name="max">随机数最大值</param>
        /// <param name="count">随机数数量</param>
        /// <returns>随机数列表</returns>
        public static List<int> GetRandomNumbers(int max,int count,bool distinct=true)
        {
            if (distinct && max <= count)
                return null;
            List<int> list = new List<int>();//随机生成随机数集合
            Random rd = new Random();
            do
            {
                do
                {
                    list.Add(rd.Next(1, max));//添加一个随机数
                } while (list.Count <= count - 1);//是否够数
                if(distinct)
                    list = list.Distinct().ToList();//去掉重复的
            } while (list.Count <= count - 1);//是否够数？
            return list;
        }
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns></returns>
        public static string Number(int Length)
        {
            return Number(Length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Number(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }
        /// <summary>
        /// 生成随机字母字符串(数字字母混和)
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        public static string GetCheckCode(int codeCount)
        {
            string str = string.Empty;
            int rep = 0;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
        /// <summary>
        /// 根据日期和随机码生成订单号
        /// </summary>
        /// <returns></returns>
        public static string GetOrderNumber()
        {
            string num = DateTime.Now.ToString("yyMMddHHmmss");//yyyyMMddHHmmssms
            return num + Number(2).ToString();
        }
        private static int Next(int numSeeds, int length)
        {
            byte[] buffer = new byte[length];
            System.Security.Cryptography.RNGCryptoServiceProvider Gen = new System.Security.Cryptography.RNGCryptoServiceProvider();
            Gen.GetBytes(buffer);
            uint randomResult = 0x0;//这里用uint作为生成的随机数  
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)buffer[i] << ((length - 1 - i) * 8));
            }
            return (int)(randomResult % numSeeds);
        }
        #endregion

        #region 截取字符长度
        /// <summary>
        /// 截取字符长度
        /// </summary>
        /// <param name="inputString">字符</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string CutString(string inputString, int len)
        {
            if (string.IsNullOrEmpty(inputString))
                return "";
            inputString = DropHTML(inputString);
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号 
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString += "…";
            return tempString;
        }
        /// <summary>
        /// Eval长度截取
        /// </summary>
        public static string CutString(object evalInput, int length)
        {
            string input = evalInput.ToString();
            if (length <= 0)
                return string.Empty;
            return input.Length < length ? input : input.Substring(0, length);
        }
        #endregion

        #region 清除HTML标记
        public static string DropHTML(string Htmlstring,bool is_sql=false)
        {
            if (string.IsNullOrEmpty(Htmlstring)) return "";
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"alert[(][\s\S]*[)]", "", RegexOptions.IgnoreCase);  //alert的字符
            Htmlstring = Regex.Replace(Htmlstring, @"<iframe[^>]*?>.*?</iframe>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"iframe[\s\S]*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"javascript:[\s\t\r\n]*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"[+][\s\S]*[+]", "", RegexOptions.IgnoreCase);  //加号内的字符

            //Htmlstring = Regex.Replace(Htmlstring, @"[""][\s\S]*[""]", "", RegexOptions.IgnoreCase);  //双引号内的字符
            //删除HTML  
            if (!Htmlstring.Contains("<>"))
            {
                Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            }
          
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);

            //删除与数据库相关的词
            Htmlstring = Regex.Replace(Htmlstring, "insert", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "delete from", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "count''", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "drop table", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "truncate", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, " asc ", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, " mid ", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, " char ", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "exec master", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "net localgroup administrators", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, " and ", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, " or ", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "net user", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "%20", "", RegexOptions.IgnoreCase);
            //

            
            if (!is_sql)
            {
                Htmlstring = Htmlstring.Replace("'", "");
                Htmlstring = Htmlstring.Replace(">", "");
                Htmlstring = Htmlstring.Replace("<", "");
                //Htmlstring = Regex.Replace(Htmlstring, "==", "", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, "--", "", RegexOptions.IgnoreCase);

            }
           
          
            Htmlstring = Htmlstring.Replace("\r\n", "");
            //Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring.Trim();
        }

        /// <summary>
        /// 只删除html标签
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string DropHTMLOnly(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring)) return "";
            //删除脚本  
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML  
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            return Htmlstring.Trim();
        }
        #endregion

        #region 清除HTML标记且返回相应的长度
        public static string DropHTML(string Htmlstring, int strLen)
        {
            return CutString(DropHTML(Htmlstring), strLen);
        }
        #endregion

        #region TXT代码转换成HTML格式
        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="chr">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把TXT代码转换成HTML格式
        public static String ToHtml(string Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\r\n", "<br />");
            sb.Replace("\n", "<br />");
            sb.Replace("\t", " ");
            sb.Replace("(", "（");
            sb.Replace(")", "）");
            Input = sb.ToString();
            int counter = 0;
            int counter2 = 0;
            var inputArray = Input.ToCharArray();
            for(int i=0;i<sb.Length;i++)
            {
                if (inputArray[i] == '\"')
                {
                    if (counter % 2 == 0)
                    {
                        sb.Replace('\"', '“',i,1);
                    }
                    else
                    {
                        sb.Replace('\"', '”',i,1);
                    }
                    counter++;
                }
                else if (inputArray[i] == '\'')
                {
                    if (counter2 % 2 == 0)
                    {
                        sb.Replace('\'', '‘', i, 1);
                    }
                    else
                    {
                        sb.Replace('\'', '’', i, 1);
                    }
                    counter2++;
                }
            }
            Input = sb.ToString();
            //sb.Replace("\"","")
            //sb.Replace(" ", "&nbsp;");
            return Input;
        }
        #endregion

        #region HTML代码转换成TXT格式
        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="chr">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把HTML代码转换成TXT格式
        public static String ToTxt(String Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("&nbsp;", " ");
            sb.Replace("<br>", "\r\n");
            sb.Replace("<br>", "\n");
            sb.Replace("<br />", "\n");
            sb.Replace("<br />", "\r\n");
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");
            sb.Replace("&amp;", "&");
            return sb.ToString();
        }
        #endregion

        #region 检测是否有Sql危险字符
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 检查危险字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Filter(string sInput)
        {
            if (sInput == null || sInput == "")
                return null;
            string sInput1 =sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("字符串中含有非法字符!");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output.Trim();
        }

        /// <summary> 
        /// 检查过滤设定的危险字符
        /// </summary> 
        /// <param name="InText">要过滤的字符串 </param> 
        /// <returns>如果参数存在不安全字符，则返回true </returns> 
        public static bool SqlFilter(string word, string InText)
        {
            if (InText == null)
                return false;
            foreach (string i in word.Split('|'))
            {
                if ((InText.ToLower().IndexOf(i + " ") > -1) || (InText.ToLower().IndexOf(" " + i) > -1))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 过滤特殊字符
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Htmls(string Input)
        {
            if (Input != string.Empty && Input != null)
            {
                string ihtml = Input.ToLower();
                ihtml = ihtml.Replace("<script", "&lt;script");
                ihtml = ihtml.Replace("script>", "script&gt;");
                ihtml = ihtml.Replace("<%", "&lt;%");
                ihtml = ihtml.Replace("%>", "%&gt;");
                ihtml = ihtml.Replace("<$", "&lt;$");
                ihtml = ihtml.Replace("$>", "$&gt;");
                return ihtml;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region 检查是否为IP地址
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion

        #region 获得配置文件节点XML文件的绝对路径
        public static string GetXmlMapPath(string xmlName)
        {
            return GetMapPath(ConfigurationManager.AppSettings[xmlName].ToString());
        }
        #endregion

        #region 获得当前绝对路径
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http"))
            {
                return strPath;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion

        #region 文件操作
        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        public static bool DeleteFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return false;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除上传的文件(及缩略图)
        /// </summary>
        /// <param name="_filepath"></param>
        public static void DeleteUpFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return;
            }
            string fullpath = GetMapPath(_filepath); //原图
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
            if (_filepath.LastIndexOf("/") >= 0)
            {
                string thumbnailpath = _filepath.Substring(0, _filepath.LastIndexOf("/")) + "mall_" + _filepath.Substring(_filepath.LastIndexOf("/") + 1);
                string fullTPATH = GetMapPath(thumbnailpath); //宿略图
                if (File.Exists(fullTPATH))
                {
                    File.Delete(fullTPATH);
                }
            }
        }

        /// <summary>
        /// 删除指定文件夹
        /// </summary>
        /// <param name="_dirpath">文件相对路径</param>
        public static bool DeleteDirectory(string _dirpath)
        {
            if (string.IsNullOrEmpty(_dirpath))
            {
                return false;
            }
            string fullpath = GetMapPath(_dirpath);
            if (Directory.Exists(fullpath))
            {
                Directory.Delete(fullpath, true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改指定文件夹名称
        /// </summary>
        /// <param name="old_dirpath">旧相对路径</param>
        /// <param name="new_dirpath">新相对路径</param>
        /// <returns>bool</returns>
        public static bool MoveDirectory(string old_dirpath, string new_dirpath)
        {
            if (string.IsNullOrEmpty(old_dirpath))
            {
                return false;
            }
            string fulloldpath = GetMapPath(old_dirpath);
            string fullnewpath = GetMapPath(new_dirpath);
            if (Directory.Exists(fulloldpath))
            {
                Directory.Move(fulloldpath, fullnewpath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 返回文件大小KB
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>int</returns>
        public static int GetFileSize(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return 0;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                FileInfo fileInfo = new FileInfo(fullpath);
                return ((int)fileInfo.Length) / 1024;
            }
            return 0;
        }

        /// <summary>
        /// 返回文件扩展名，不含“.”
        /// </summary>
        /// <param name="_filepath">文件全名称</param>
        /// <returns>string</returns>
        public static string GetFileExt(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return "";
            }
            if (_filepath.LastIndexOf(".") > 0)
            {
                return _filepath.Substring(_filepath.LastIndexOf(".") + 1); //文件扩展名，不含“.”
            }
            return "";
        }

        /// <summary>
        /// 返回文件名，不含路径
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>string</returns>
        public static string GetFileName(string _filepath)
        {
            return _filepath.Substring(_filepath.LastIndexOf(@"/") + 1);
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>bool</returns>
        public static bool FileExists(string _filepath)
        {
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获得远程字符串
        /// </summary>
        public static string GetDomainStr(string key, string uriPath)
        {
            string result = CacheHelper.Get(key) as string;
            if (result == null)
            {
                System.Net.WebClient client = new System.Net.WebClient();
                try
                {
                    client.Encoding = System.Text.Encoding.UTF8;
                    result = client.DownloadString(uriPath);
                }
                catch
                {
                    result = "暂时无法连接!";
                }
                CacheHelper.Insert(key, result, 60);
            }

            return result;
        }

        #endregion

        #region 读取或写入cookie
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            strName = DESEncrypt.Encrypt(strName);
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = DESEncrypt.Encrypt(UrlEncode(strValue));
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue)
        {
            strName = DESEncrypt.Encrypt(strName);
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[DESEncrypt.Encrypt(key)] = DESEncrypt.Encrypt(UrlEncode(strValue));
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="key">键</param>
        /// <param name="strValue">值</param>
        ///  <param name="expires">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string key, string strValue, int expires)
        {
            strName = DESEncrypt.Encrypt(strName);
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[DESEncrypt.Encrypt(key)] = DESEncrypt.Encrypt(UrlEncode(strValue));
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            strName = DESEncrypt.Encrypt(strName);
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = DESEncrypt.Encrypt(UrlEncode(strValue));
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            strName = DESEncrypt.Encrypt(strName);
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
                return DropHTML(UrlDecode(DESEncrypt.Decrypt(HttpContext.Current.Request.Cookies[strName].Value.ToString())));
            return "";
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName, string key)
        {
            strName = DESEncrypt.Encrypt(strName);
            key = DESEncrypt.Encrypt(key);
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
                return DropHTML(UrlDecode(DESEncrypt.Decrypt(HttpContext.Current.Request.Cookies[strName][key].ToString())));

            return "";
        }
        #endregion

        #region 替换指定的字符串
        /// <summary>
        /// 替换指定的字符串
        /// </summary>
        /// <param name="originalStr">原字符串</param>
        /// <param name="oldStr">旧字符串</param>
        /// <param name="newStr">新字符串</param>
        /// <returns></returns>
        public static string ReplaceStr(string originalStr, string oldStr, string newStr)
        {
            if (string.IsNullOrEmpty(oldStr))
            {
                return "";
            }
            return originalStr.Replace(oldStr, newStr);
        }
        #endregion

        #region 显示分页
        /// <summary>
        /// 返回分页页码,请先引用function.js 
        /// </summary>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="linkUrl">链接地址，__id__代表页码</param>
        /// <param name="centSize">中间页码数量</param>
        /// <returns></returns>
        public static string OutPageList(int pageSize, int pageIndex, int totalCount, string linkUrl, int centSize)
        {
            //计算页数
            if (totalCount < 1 || pageSize < 1)
            {
                return "";
            }
            int pageCount = totalCount / pageSize;
            if (pageCount < 1)
            {
                return "";
            }
            if (totalCount % pageSize > 0)
            {
                pageCount += 1;
            }
            if (pageCount <= 1)
            {
                return "";
            }
            StringBuilder pageStr = new StringBuilder();
            string pageId = "__id__";
            string firstBtn = "<a href=\"" + ReplaceStr(linkUrl, pageId, (pageIndex - 1).ToString()) + "\" class=\"prev\">‹</a>";
            string lastBtn = "<a href=\"" + ReplaceStr(linkUrl, pageId, (pageIndex + 1).ToString()) + "\" class=\"next\">›</a>";
            string firstStr = "<a href=\"" + ReplaceStr(linkUrl, pageId, "1") + "\">1</a>";
            string lastStr = "<a href=\"" + ReplaceStr(linkUrl, pageId, pageCount.ToString()) + "\" >" + pageCount.ToString() + "</a>";

            if (pageIndex <= 1)
            {
                firstBtn = "<a class=\"prev\">‹</a>";
            }
            if (pageIndex >= pageCount)
            {
                lastBtn = "<a class=\"next\">›</a>";
            }
            if (pageIndex == 1)
            {
                firstStr = "<a class=\"current\">1</a>";
            }
            if (pageIndex == pageCount)
            {
                lastStr = "<a class=\"current\">" + pageCount.ToString() + "</a>";
            }
            int firstNum = pageIndex - (centSize / 2); //中间开始的页码
            if (pageIndex < centSize)
                firstNum = 2;
            int lastNum = pageIndex + centSize - ((centSize / 2) + 1); //中间结束的页码
            if (lastNum >= pageCount)
                lastNum = pageCount - 1;
            pageStr.Append("<div class=\"info\">共有<em>" + pageCount + "</em>页，<em>" + totalCount + "</em>条信息</div>");
            pageStr.Append(firstBtn + firstStr);
            if (pageIndex >= centSize)
            {
                pageStr.Append("<a>...</a>\n");
            }
            for (int i = firstNum; i <= lastNum; i++)
            {
                if (i == pageIndex)
                {
                    pageStr.Append("<a class=\"current\">" + i + "</a>");
                }
                else
                {
                    pageStr.Append("<a href=\"" + ReplaceStr(linkUrl, pageId, i.ToString()) + "\">" + i + "</a>");
                }
            }
            if (pageCount - pageIndex > centSize - ((centSize / 2)))
            {
                pageStr.Append("<a>...</a>");
            }
            pageStr.Append(lastStr + lastBtn);

            //添加跳转页 ，请先引用function.js 
            pageStr.Append("<input type=\"text\" value=\"" + pageIndex + "\" id=\"txtpage\" onafterpaste=\"this.value=this.value.replace(/\\D/,'')\" onkeyup=\"this.value=this.value.replace(/\\D/,'')\" maxlength=\"" + pageCount.ToString().Length + "\"  class=\"txt\"  /><input type=\"button\" class=\"btn\" value=\"跳转\"  onclick=\"goLink('" + linkUrl + "'," + pageCount + ")\">");


            return pageStr.ToString();
        }
        #endregion

        #region URL处理
        /// <summary>
        /// URL字符编码
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.Replace("'", "");
            return HttpContext.Current.Server.UrlEncode(str);
        }

        /// <summary>
        /// URL字符解码
        /// </summary>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return HttpContext.Current.Server.UrlDecode(str);
        }

        /// <summary>
        /// 组合URL参数
        /// </summary>
        /// <param name="_url">页面地址</param>
        /// <param name="_keys">参数名称</param>
        /// <param name="_values">参数值</param>
        /// <returns>String</returns>
        public static string CombUrlTxt(string _url, string _keys, params string[] _values)
        {
            StringBuilder urlParams = new StringBuilder();
            try
            {
                string[] keyArr = _keys.Split(new char[] { '&' });
                for (int i = 0; i < keyArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(_values[i]) && _values[i] != "0")
                    {
                        _values[i] = UrlEncode(_values[i]);
                        urlParams.Append(string.Format(keyArr[i], _values) + "&");
                    }
                }
                if (!string.IsNullOrEmpty(urlParams.ToString()) && _url.IndexOf("?") == -1)
                    urlParams.Insert(0, "?");
            }
            catch
            {
                return _url;
            }
            return _url + DelLastChar(urlParams.ToString(), "&");
        }
        #endregion

        #region URL请求数据
        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string HttpPost(string url, string param)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }

            return responseStr;
        }

        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            WebResponse response = null;
            string responseStr = null;

            try
            {
                response = request.GetResponse();

                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                response = null;
            }

            return responseStr;
        }

        /// <summary>
        /// 执行URL获取页面内容
        /// </summary>
        public static string UrlExecute(string urlPath)
        {
            if (string.IsNullOrEmpty(urlPath))
            {
                return "error";
            }
            StringWriter sw = new StringWriter();
            try
            {
                HttpContext.Current.Server.Execute(urlPath, sw);
                return sw.ToString();
            }
            catch (Exception)
            {
                return "error";
            }
            finally
            {
                sw.Close();
                sw.Dispose();
            }
        }
        #endregion



        #region 替换URL
        /// <summary>
        /// 替换扩展名
        /// </summary>
        public static string GetUrlExtension(string urlPage, string staticExtension)
        {
            int indexNum = urlPage.LastIndexOf('.');
            if (indexNum > 0)
            {
                return urlPage.Replace(urlPage.Substring(indexNum), "." + staticExtension);
            }
            return urlPage;
        }
        /// <summary>
        /// 替换扩展名，如没有扩展名替换默认首页
        /// </summary>
        public static string GetUrlExtension(string urlPage, string staticExtension, bool defaultVal)
        {
            int indexNum = urlPage.LastIndexOf('.');
            if (indexNum > 0)
            {
                return urlPage.Replace(urlPage.Substring(indexNum), "." + staticExtension);
            }
            if (defaultVal)
            {
                if (urlPage.EndsWith("/"))
                {
                    return urlPage + "index." + staticExtension;
                }
                else
                {
                    return urlPage + "/index." + staticExtension;
                }
            }
            return urlPage;
        }
        #endregion

        #region 补足位数
        /// <summary>
        /// 指定字符串的固定长度，如果字符串小于固定长度，
        /// 则在字符串的前面补足零，可设置的固定长度最大为9位
        /// </summary>
        /// <param name="text">原始字符串</param>
        /// <param name="limitedLength">字符串的固定长度</param>
        public static string RepairZero(string text, int limitedLength)
        {
            //补足0的字符串
            string temp = "";

            //补足0
            for (int i = 0; i < limitedLength - text.Length; i++)
            {
                temp += "0";
            }

            //连接text
            temp += text;

            //返回补足0的字符串
            return temp;
        }
        #endregion

        #region 获得用户IP
        /// <summary>
        /// 获得用户IP
        /// </summary>
        public static string GetUserIp()
        {
            string ip;
            string[] temp;
            bool isErr = false;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"] == null)
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            else
                ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"].ToString();
            if (ip.Length > 15)
                isErr = true;
            else
            {
                temp = ip.Split('.');
                if (temp.Length == 4)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].Length > 3) isErr = true;
                    }
                }
                else
                    isErr = true;
            }

            if (isErr)
                return "1.1.1.1";
            else
                return ip;
        }
        #endregion

        #region 密码相关
        /// <summary>
        /// 通过明文密码和
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string CreatePassword(string pwd, string salt)
        {
            return DESEncrypt.Encrypt(pwd, salt);
        }
        public static string GetPasswordBySalt(string pwd, string salt)
        {
            return DESEncrypt.Decrypt(pwd, salt);
        }
        public static string CreateSalt()
        {
            string cl = DateTime.Now.ToString();
            string pwd = "";
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("X");
            }
            return pwd.Substring(0, 9);
        }

        #endregion

        #region 操作权限菜单
        /// <summary>
        /// 获取操作权限
        /// </summary>
        /// <returns>Dictionary</returns>
        public static Dictionary<string, string> ActionType()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic.Add("Show", "显示");
            dic.Add("View", "查看");
            dic.Add("Add", "添加");
            dic.Add("Edit", "修改");
            dic.Add("Delete", "删除");
            dic.Add("Import", "导入");
            dic.Add("Export", "导出");
            dic.Add("Release", "发布");
            dic.Add("Reply", "回复");
            dic.Add("Aduit", "审核");
            dic.Add("Manage", "代管");
            //dic.Add("Cancel", "取消");
            //dic.Add("Invalid", "作废");
            //dic.Add("Build", "生成");
            //dic.Add("Instal", "安装");
            //dic.Add("Unload", "卸载");
            //dic.Add("Back", "备份");
            //dic.Add("Restore", "还原");
            //dic.Add("Replace", "替换");
            return dic;
        }
        #endregion

        #region 编辑器表情转微信表情
        /// <summary>
        /// 将编辑器表情转变为微信表情
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ChangeToWeChatEmotion(string x)
        {
            if (!string.IsNullOrWhiteSpace(x))
            {

                x = Filter(x);
                string imgString = "0.gif_1.gif_10.gif_100.gif_101.gif_102.gif_103.gif_104.gif_11.gif_12.gif_13.gif_14.gif_15.gif_16.gif_17.gif_18.gif_19.gif_2.gif_20.gif_21.gif_22.gif_23.gif_24.gif_25.gif_26.gif_27.gif_28.gif_29.gif_3.gif_30.gif_31.gif_32.gif_33.gif_34.gif_35.gif_36.gif_37.gif_38.gif_39.gif_4.gif_40.gif_41.gif_42.gif_43.gif_44.gif_45.gif_46.gif_47.gif_48.gif_49.gif_5.gif_50.gif_51.gif_52.gif_53.gif_54.gif_55.gif_56.gif_57.gif_58.gif_59.gif_6.gif_60.gif_61.gif_62.gif_63.gif_64.gif_65.gif_66.gif_67.gif_68.gif_69.gif_7.gif_70.gif_71.gif_72.gif_73.gif_74.gif_75.gif_76.gif_77.gif_78.gif_79.gif_8.gif_80.gif_81.gif_82.gif_83.gif_84.gif_85.gif_86.gif_87.gif_88.gif_89.gif_9.gif_90.gif_91.gif_92.gif_93.gif_94.gif_95.gif_96.gif_97.gif_98.gif_99.gif";
                string emoString = "/::)_/::~_/::-|_/:#-0_[街舞]_/:kiss_/:<&_/:&>_/::@_/::P_/::D_/::O_/::(_/::+_/:–b_/::Q_/::T_/::B_/:,@P_/:,@-D_/::d_/:,@o_/::g_/:|-)_/::!_/::L_/::>_/::,@_/::|_/:,@f_/::-S_/:?_/:,@x_/:,@@_/::8_/:,@!_/:!!!_/:xx_/:bye_/:8-)_/:wipe_/:dig_/:handclap_/:&-(_/:B-)_/:<@_/:@>_/::-O_/:>-|_/:P-(_/::<_/::’|_/:X-)_/::*_/:@x_/:8*_/:pd_/:<W>_/:beer_/:basketb_/:oo_/::$_/:coffee_/:eat_/:pig_/:rose_/:fade_/:showlove_/:heart_/:break_/:cake_/:li_/::X_/:bome_/:kn_/:footb_/:ladybug_/:shit_/:moon_/:sun_/:gift_/:hug_/:strong_/::Z_/:weak_/:share_/:v_/:@)_/:jj_/:@@_/:bad_/:lvu_/:no_/:ok_/::’(_/:love_/:<L>_/:jump_/:shake_/:<O>_/:circle_/:kotow_/:turn_/:skip_[挥手]";
                string[] imgList = imgString.Split('_');
                string[] emoList = emoString.Split('_');
                Dictionary<string, string> dic = new Dictionary<string, string>();
                for (int i = 0; i < imgList.Length; i++)
                {
                    dic.Add(imgList[i], emoList[i]);
                }
                x = x.Replace("<a", "[a");
                x = x.Replace("</a", "[/a");
                var match = Regex.Matches(x, "\\d+.gif");
                foreach (Match y in match)
                {
                    string emo = dic[y.ToString()];
                    emo = emo.Replace("<", "《《《《");
                    emo = emo.Replace(">", "》》》》");
                    x = x.Replace("/" + y.ToString(), ">" + emo + "<");
                }
                x = System.Text.RegularExpressions.Regex.Replace(x, "<\\/?[^>]+>", "");
                x = x.Replace("[a", "<a");
                x = x.Replace("[/a", "</a");
                x = x.Replace("《《《《", "<");
                x = x.Replace("》》》》", ">");
            }
            return x;
        }

        #endregion

        #region 微信表情转编辑器表情
        /// <summary>
        /// 将微信表情转换为编辑器表情
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ChangeToEditorEmotion(string x)
        {
            if (!string.IsNullOrWhiteSpace(x))
            {
                x = Filter(x);
                string imgString = "0.gif_1.gif_10.gif_100.gif_101.gif_102.gif_103.gif_104.gif_11.gif_12.gif_13.gif_14.gif_15.gif_16.gif_17.gif_18.gif_19.gif_2.gif_20.gif_21.gif_22.gif_23.gif_24.gif_25.gif_26.gif_27.gif_28.gif_29.gif_3.gif_30.gif_31.gif_32.gif_33.gif_34.gif_35.gif_36.gif_37.gif_38.gif_39.gif_4.gif_40.gif_41.gif_42.gif_43.gif_44.gif_45.gif_46.gif_47.gif_48.gif_49.gif_5.gif_50.gif_51.gif_52.gif_53.gif_54.gif_55.gif_56.gif_57.gif_58.gif_59.gif_6.gif_60.gif_61.gif_62.gif_63.gif_64.gif_65.gif_66.gif_67.gif_68.gif_69.gif_7.gif_70.gif_71.gif_72.gif_73.gif_74.gif_75.gif_76.gif_77.gif_78.gif_79.gif_8.gif_80.gif_81.gif_82.gif_83.gif_84.gif_85.gif_86.gif_87.gif_88.gif_89.gif_9.gif_90.gif_91.gif_92.gif_93.gif_94.gif_95.gif_96.gif_97.gif_98.gif_99.gif";
                string emoString = "/::)_/::~_/::-|_/:#-0_[街舞]_/:kiss_/:<&_/:&>_/::@_/::P_/::D_/::O_/::(_/::+_/:–b_/::Q_/::T_/::B_/:,@P_/:,@-D_/::d_/:,@o_/::g_/:|-)_/::!_/::L_/::>_/::,@_/::|_/:,@f_/::-S_/:?_/:,@x_/:,@@_/::8_/:,@!_/:!!!_/:xx_/:bye_/:8-)_/:wipe_/:dig_/:handclap_/:&-(_/:B-)_/:<@_/:@>_/::-O_/:>-|_/:P-(_/::<_/::’|_/:X-)_/::*_/:@x_/:8*_/:pd_/:<W>_/:beer_/:basketb_/:oo_/::$_/:coffee_/:eat_/:pig_/:rose_/:fade_/:showlove_/:heart_/:break_/:cake_/:li_/::X_/:bome_/:kn_/:footb_/:ladybug_/:shit_/:moon_/:sun_/:gift_/:hug_/:strong_/::Z_/:weak_/:share_/:v_/:@)_/:jj_/:@@_/:bad_/:lvu_/:no_/:ok_/::’(_/:love_/:<L>_/:jump_/:shake_/:<O>_/:circle_/:kotow_/:turn_/:skip_[挥手]";
                string[] imgList = imgString.Split('_');
                string[] emoList = emoString.Split('_');
                for (int i = 0; i < imgList.Length; i++)
                {
                    x = x.Replace(emoList[i], "<img src=\"/editor/plugins/emoticons/images/" + imgList[i] + "\" />");
                }
            }
            return x;
        }

        #endregion

        #region 日期列表相关
        /// <summary>
        /// 通过起始日期和最终日期获取日期列表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<DateTime> GetDateListByStartAndEnd(DateTime start, DateTime end)
        {
            List<DateTime> list = new List<DateTime>();
            end = end.AddDays(1);
            while (end.Date != start.Date)
            {
                list.Add(start.Date);
                start = start.AddDays(1);
            }
            return list;
        }

        /// <summary>
        /// 通过日期列表获取,分割的日期字符串(带引号版本)
        /// </summary>
        /// <param name="dateList">日期列表</param>
        /// <returns></returns>
        public static string GetDateXAxisByDateList(List<DateTime> dateList)
        {
            string date_string = "";
            for(int i=0;i<dateList.Count;i++)
            {
                date_string += "\"" + dateList[i].ToString("MM-dd") + "\",";
            }
            if (date_string.Length > 0)
                date_string = date_string.Substring(0, date_string.Length - 1);
            return date_string;
        }
        #endregion

        #region 验证上传格式

        /// <summary>
        /// 是否为图片文件
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        public static bool IsImage(string _fileExt)
        {
            ArrayList al = new ArrayList();
            al.Add("bmp");
            al.Add("jpeg");
            al.Add("jpg");
            al.Add("gif");
            al.Add("png");
            if (al.Contains(_fileExt.ToLower()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否为视频文件
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        public static bool IsVideo(string _fileExt)
        {
            ArrayList al = new ArrayList();
            al.Add("mp4");
            al.Add("rm");
            al.Add("avi");
            al.Add("rmvb");
            al.Add("wmv");
            al.Add("mpg");
            al.Add("mpeg");
            if (al.Contains(_fileExt.ToLower()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否为音频文件
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        public static bool IsVoice(string _fileExt)
        {
            ArrayList al = new ArrayList();
            al.Add("mp3");
            al.Add("amr");
            //al.Add("wma");
            //al.Add("wav");
            if (al.Contains(_fileExt.ToLower()))
            {
                return true;
            }
            return false;
        }
 
        #endregion

        #region string[]转int[]
        public static int[] GetIntArray(string[] strArray)
        {
            int[] intArray = new int[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                intArray[i] = StrToInt(strArray[i], 0);
            }
            return intArray;
        }
        #endregion

        #region 语言标记转文字
        public static string GetNameByLanguageSymbol(string symbol)
        {
            string nameString = "简体中文(中国),繁体中文(台湾地区),繁体中文(香港),英语(香港),英语(美国),英语(英国),英语(全球),英语(加拿大),英语(澳大利亚),英语(爱尔兰),英语(芬兰),芬兰语(芬兰),英语(丹麦),丹麦语(丹麦),英语(以色列),希伯来语(以色列),英语(南非),英语(印度),英语(挪威),英语(新加坡),英语(新西兰),英语(印度尼西亚),英语(菲律宾),英语(泰国),英语(马来西亚),英语(阿拉伯),韩文(韩国),日语(日本),荷兰语(荷兰),荷兰语(比利时),葡萄牙语(葡萄牙),葡萄牙语(巴西),法语(法国),法语(卢森堡),法语(瑞士),法语(比利时),法语(加拿大),西班牙语(拉丁美洲),西班牙语(西班牙),西班牙语(阿根廷),西班牙语(美国),西班牙语(墨西哥),西班牙语(哥伦比亚),西班牙语(波多黎各),德语(德国),德语(奥地利),德语(瑞士),俄语(俄罗斯),意大利语(意大利),希腊语(希腊),挪威语(挪威),匈牙利语(匈牙利),土耳其语(土耳其),捷克语(捷克共和国),斯洛文尼亚语,波兰语(波兰),瑞典语(瑞典)";
            string symbolString = "zh-cn,zh-tw,zh-hk,en-hk,en,en-gb,en-ww,en-ca,en-au,en-ie,en-fi,fi-fi,en-dk,da-dk,en-il,he-il,en-za,en-in,en-no,en-sg,en-nz,en-id,en-ph,en-th,en-my,en-xa,ko,ja,nl-nl,nl-be,pt-pt,pt-br,fr-fr,fr-lu,fr-ch,fr-be,fr-ca,es-la,es-es,es-ar,es-us,es-mx,es-co,es-pr,de-de,de-at,de-ch,ru-ru,it-it,el-gr,no-no,hu-hu,tr-tr,cs-cz,sl-sl,pl-pl,sv-se";
            symbol = symbol.ToLower();
            symbol = symbol.Replace("_", "-");
            string[] nameList = nameString.Split(',');
            string[] symbolList = symbolString.Split(',');
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < nameList.Count(); i++)
            {
                dic.Add(symbolList[i], nameList[i]);
            }
            return dic.Keys.Contains(symbol) ? dic[symbol] : symbol;
        }
        #endregion

        #region 微信时间处理
        public static DateTime GetWeChatDate(double time)
        {
            return DateTime.Parse("1970-1-1").AddSeconds(time);
        }
        public static DateTime GetWeChatDate(string time)
        {
            double tim=StrToDouble(time,0);
            if(tim!=0)
                return DateTime.Parse("1970-1-1").AddSeconds(tim);
            return DateTime.Now;
        }
        public static double GetLinuxTime(DateTime time)
        {
            return (int)((time - DateTime.Parse("1970-1-1")).TotalSeconds);
        }

        #endregion

        #region highMap对应转换
        public static string GetKeyByProvinceName(string name)
        {
            string keyString = "cn-sh,cn-zj,tw-ph,tw-km,tw-lk,tw-tw,tw-cs,cn-3664,cn-3681,tw-tp,tw-ch,tw-tt,tw-pt,cn-6657,cn-6663,cn-6665,cn-6666,cn-6667,cn-gs,cn-6669,cn-6670,cn-6671,tw-kh,tw-hs,tw-hh,cn-nx,cn-sa,tw-cl,cn-3682,tw-ml,cn-6655,cn-ah,cn-hu,tw-ty,cn-6656,tw-cg,cn-6658,tw-hl,tw-nt,tw-th,cn-6659,cn-6660,cn-6661,tw-yl,cn-6662,cn-6664,cn-6668,cn-gd,cn-fj,cn-bj,cn-hb,cn-sd,tw-tn,cn-tj,tw-il,cn-js,cn-ha,cn-qh,cn-jl,cn-xz,cn-xj,cn-he,cn-nm,cn-hl,cn-yn,cn-gx,cn-ln,cn-sc,cn-cq,cn-gz,cn-hn,cn-sx,cn-jx";
            string ProvinceName = "上海,浙江,澎湖,金门,连江县,台北市,嘉义市,南沙群岛,澳门,新台北,嘉义,台东,屏东,南区,西贡,葵青,荃湾,屯门,甘肃,北区,大埔,离岛,高雄市,新竹市,新竹,宁夏回族自治区,陕西,基隆市,中西区,苗栗,湾仔,安徽,湖北,桃园,东区,彰化,油尖旺,台湾,南投,台中市,九龙城,深水埗,黄大仙区,云林,观塘,沙田,香港,广东,福建,北京,河北,山东,台南市,天津,宜兰,江苏,海南,青海,吉林,西藏,新疆,河南,内蒙古,黑龙江,云南,广西,辽宁,四川,重庆,贵州,湖南,山西,江西"; 
            string[] keyArray = keyString.Split(',');
            string[] nameArray = ProvinceName.Split(',');
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < keyArray.Length; i++)
            {
                dic.Add(nameArray[i], keyArray[i]);
            }
            if (dic.Keys.Contains(name))
                return dic[name];
            else
                return null;
        }
        #endregion

        #region 去除引号，分号
        public static string RemoveQuotes(string input)
        {
            return input.Replace('\'', '‘').Replace('\"', '“').Replace(';',' ');
        }
        #endregion


        #region 利用正则表达式匹配图片
        /// <summary>
        /// 利用正则表达式获取文本中图片的src
        /// </summary>
        /// <param name="sHtmlText">文本</param>
        /// <returns>返回图片路径集合</returns>
        public static string[] GetHtmlImageSrcList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签，返回src
            //Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            //返回width
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            //返回style

            // 搜索匹配的字符串
            MatchCollection matches = regImg.Matches(sHtmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];
            // 取得匹配项列表
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            return sUrlList;
        }

        /// <summary>
        /// 利用正则表达式获取以style匹配img，返回整个img标签
        /// </summary>
        /// <param name="sHtmlText">要匹配的内容</param>
        /// <returns></returns>
        public static string[] GetHtmlImageStyleList(string sHtmlText)
        {
            Regex regStyle = new Regex(@"<img\b[^<>]*?\bstyle=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            // 搜索匹配的字符串
            MatchCollection matches = regStyle.Matches(sHtmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];
            // 取得匹配项列表
            foreach (Match match in matches)
                sUrlList[i++] = match.Value;
            return sUrlList;
        }
        #endregion

        public class ModelHandler<T> where T : new()
        {

            #region 实体类转换成DataTable

            /// <summary>
            /// 实体类转换成DataSet
            /// </summary>
            /// <param name="modelList">实体类列表</param>
            /// <returns></returns>
            public static DataSet FillDataSet(List<T> modelList)
            {
                if (modelList == null || modelList.Count == 0)
                {
                    return new DataSet();
                }
                else
                {
                    DataSet ds = new DataSet();
                    ds.Tables.Add(FillDataTable(modelList));
                    return ds;
                }
            }

            /// <summary>
            /// 实体类转换成DataTable
            /// </summary>
            /// <param name="modelList">实体类列表</param>
            /// <returns></returns>
            public static DataTable FillDataTable(List<T> modelList)
            {
                if (modelList == null || modelList.Count == 0)
                {
                    return null;
                }
                DataTable dt = CreateData(modelList[0]);

                foreach (T model in modelList)
                {
                    DataRow dataRow = dt.NewRow();
                    foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                    {
                        dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
                    }
                    dt.Rows.Add(dataRow);
                }
                return dt;
            }

            /// <summary>
            /// 根据实体类得到表结构
            /// </summary>
            /// <param name="model">实体类</param>
            /// <returns></returns>
            private static DataTable CreateData(T model)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    DataColumn customerId = new DataColumn();
                    customerId.AllowDBNull = true;
                    customerId.ColumnName = propertyInfo.Name;
                    customerId.DataType = propertyInfo.PropertyType;
                    dataTable.Columns.Add(customerId);
                }
                return dataTable;
            }

            #endregion

        }

    }
}
