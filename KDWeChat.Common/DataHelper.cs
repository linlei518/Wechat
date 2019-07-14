using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDWechat.Common
{
    public class DataHelper
    {
        /// <summary>
        /// 比较两个值，若可空值为空则返回默认值，否则返回可空值
        /// </summary>
        /// <param name="nullableValue">可空值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetRealValue(string nullableValue, string defaultValue)
        {
            return string.IsNullOrEmpty(nullableValue) ? defaultValue : nullableValue;
        }

        /// <summary>
        /// 比较两个值，若可空值为0则返回默认值，否则返回可空值
        /// </summary>
        /// <param name="nullableValue">可空值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int GetRealValue(int nullableValue, int? defaultValue)
        {
            if (defaultValue != null)
                return nullableValue == 0 ? (int)defaultValue : nullableValue;
            else
                return nullableValue;
        }

    }
}
