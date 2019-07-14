using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace KDWechat.Common
{
    public static class EnumHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="ddl"></param>
        public static void BindEnumForDropDownListSource(Type enumType, DropDownList ddl, string strFirstNullTxt = "", string strDefaultValue = "")
        {
            BindEnumDDLSource(enumType, ddl);
            if (strFirstNullTxt.Length > 0)
            {
                ddl.Items.Insert(0, new ListItem(strFirstNullTxt, ""));
            }

            if (!string.IsNullOrEmpty(strDefaultValue))
                ddl.SelectedValue = strDefaultValue;
        }

        /// <summary>
        /// 将一个枚举绑定到drpodownlist
        /// </summary>
        /// <param name="enumType">枚举的类型，请使用typeof(xxx)</param>
        /// <param name="ddl">需要绑定的dropdownlist</param>
        /// <returns></returns>
        public static void BindEnumDDLSource(Type enumType, DropDownList ddl)
        {
            var ddlValues = Enum.GetValues(enumType);
            List<Tuple<int, string>> sourceTuple = new List<Tuple<int, string>>();
            foreach (int val in ddlValues)
            {
                if (val < 0)//负值枚举，为删除,取消数据
                {
                    continue;
                }
                var name = Enum.GetName(enumType, val);
                sourceTuple.Add(new Tuple<int, string>(val, name));
            }
            ddl.DataSource = sourceTuple;
            ddl.DataTextField = "Item2";
            ddl.DataValueField = "Item1";
            ddl.DataBind();
        }

        /// <summary>
        /// 将一个枚举绑定到drpodownlist
        /// </summary>
        /// <param name="enumType">枚举的类型，请使用typeof(xxx)</param>
        /// <param name="ddl">需要绑定的dropdownlist</param>
        /// <returns></returns>
        public static void BindEnumDDLSourceByNameAndDesc(Type enumType, DropDownList ddl)
        {
            var ddlValues = Enum.GetValues(enumType);
            List<Tuple<string, string>> sourceTuple = new List<Tuple<string, string>>();
            foreach (int val in ddlValues)
            {
                var name = Enum.GetName(enumType, val);

                var desc = GetEnumDescription((Enum)Enum.Parse(enumType, name, true));
                sourceTuple.Add(new Tuple<string, string>(name, desc));
            }
            ddl.DataSource = sourceTuple;
            ddl.DataTextField = "Item2";
            ddl.DataValueField = "Item1";
            ddl.DataBind();
        }

        /// <summary>
        /// 获取对应枚举值的名称
        /// </summary>
        /// <param name="enumType">枚举的类型，请使用typeof</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string GetNameByValue(Type enumType, object value)
        {
            if (value == null)
            {
                return "";
            }
            var intValue = Utils.StrToInt(value.ToString(), 0);
            return Enum.GetName(enumType, intValue);
        }

        /// <summary>
        /// 获取对应枚举名称的值
        /// </summary>
        /// <param name="enumType">枚举的类型，请使用typeof</param>
        /// <param name="value">枚举名称</param>
        /// <returns></returns>
        public static int GetValueByName(Type enumType, string name)
        {
            try
            {
                return (int) Enum.Parse(enumType, name);
            }
            catch 
            {
                return -9999;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="rbl"></param>
        /// <param name="strFirstNullTxt"></param>
        public static void BindEnumForRadioButtonListSource(Type enumType, RadioButtonList rbl)
        {
            BindEnumRBLSource(enumType, rbl);
        }


        /// <summary>
        /// 将枚举绑定到 RadioButtonList
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="rbl"></param>
        public static void BindEnumRBLSource(Type enumType, RadioButtonList rbl)
        {
            List<Tuple<int, string>> sourceTuple = GetTupleList(enumType);
            rbl.DataSource = sourceTuple;
            rbl.DataTextField = "Item2";
            rbl.DataValueField = "Item1";
            rbl.DataBind();
        }


        public static List<Tuple<int, string>> GetTupleList(Type enumType)
        {
            List<Tuple<int, string>> sourceTuple = new List<Tuple<int, string>>();
            var ddlValues = Enum.GetValues(enumType);
            foreach (int val in ddlValues)
            {
                var name = Enum.GetName(enumType, val);
                sourceTuple.Add(new Tuple<int, string>(val, name));
            }
            return sourceTuple;
        }

        #region 处理枚举，ceiling add
        /// <summary>
        /// 获取枚举描述信息
        /// </summary>
        /// <param name="en">枚举</param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum en)
        {
            Type type = en.GetType();
            System.Reflection.MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }
        #endregion
    }
}
