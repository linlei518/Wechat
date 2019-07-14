using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDWechat.Common
{
    /// <summary>
    /// 获取汉字字符串首字母
    /// </summary>
    public class HzToPy
    {
        /// <summary>
        /// 汉字转拼音并获取首字母
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        static public string GetChineseSpell(string strText)
        {
            int len = strText.Length;
            string myStr = "";
            for (int i = 0; i < len; i++)
            {
                myStr += getSpell(strText.Substring(i, 1));
            }
            return myStr;
        }

        /// <summary>
        /// 获取汉字字符串第一个字首字母

        /// </summary>
        /// <param name="cnChar">汉字</param>
        /// <returns><string/returns>
        static public string getSpell2(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "*";
            }
            else return cnChar;
        }

        /// <summary>
        /// 获取汉字字符串第一个字首字母

        /// </summary>
        /// <param name="cnChar">汉字</param>
        /// <returns><string/returns>
        public static string getSpell(string cnChar)
        {
            if (cnChar.CompareTo("吖") < 0)
            {
                string s = cnChar.Substring(0, 1).ToUpper();
                if (char.IsNumber(s, 0))
                {
                    return "0";
                }
                else
                {
                    return s;
                }

            }
            else if (cnChar.CompareTo("八") < 0)
            {
                return "A";
            }
            else if (cnChar.CompareTo("嚓") < 0)
            {
                return "B";
            }
            else if (cnChar.CompareTo("咑") < 0)
            {
                return "C";
            }
            else if (cnChar.CompareTo("妸") < 0)
            {
                return "D";
            }
            else if (cnChar.CompareTo("发") < 0)
            {
                return "E";
            }
            else if (cnChar.CompareTo("旮") < 0)
            {
                return "F";
            }
            else if (cnChar.CompareTo("铪") < 0)
            {
                return "G";
            }
            else if (cnChar.CompareTo("讥") < 0)
            {
                return "H";
            }
            else if (cnChar.CompareTo("咔") < 0)
            {
                return "J";
            }
            else if (cnChar.CompareTo("垃") < 0)
            {
                return "K";
            }
            else if (cnChar.CompareTo("呒") < 0)
            {
                return "L";
            }
            else if (cnChar.CompareTo("拏") < 0)
            {
                return "M";
            }
            else if (cnChar.CompareTo("噢") < 0)
            {
                return "N";
            }
            else if (cnChar.CompareTo("妑") < 0)
            {
                return "O";
            }
            else if (cnChar.CompareTo("七") < 0)
            {
                return "P";
            }
            else if (cnChar.CompareTo("亽") < 0)
            {
                return "Q";
            }
            else if (cnChar.CompareTo("仨") < 0)
            {
                return "R";
            }
            else if (cnChar.CompareTo("他") < 0)
            {
                return "S";
            }
            else if (cnChar.CompareTo("哇") < 0)
            {
                return "T";
            }
            else if (cnChar.CompareTo("夕") < 0)
            {
                return "W";
            }
            else if (cnChar.CompareTo("丫") < 0)
            {
                return "X";
            }
            else if (cnChar.CompareTo("帀") < 0)
            {
                return "Y";
            }
            else if (cnChar.CompareTo("咗") < 0)
            {
                return "Z";
            }
            else
            {
                return "0";
            }
        }
    }
}
