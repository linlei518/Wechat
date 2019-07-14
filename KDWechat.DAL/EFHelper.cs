using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using LinqKit;
using EntityFramework.Extensions;
using System.Text.RegularExpressions;
using System.Reflection;
using Microsoft.Security.Application;
using System.Web;

namespace KDWechat.DAL
{
    /// <summary>
    /// 这个类用于扩展实体类，其中包含更加方便的插入，更新操作，具体方式为：
    /// 
    /// EFHelper.AddXXX（xxx为数据上下文的名称，本系统中为WeChat,User,Log）
    /// EFHelper.UpdateXXX(XXX同上)--old
    /// 
    /// 为兼容之前版本代码，在此处保留了EFHelper的Beta版本，最新版本请使用Companycn.Core.EntityFramework.EFHelper
    /// EFHelper.AddModel<TContext,TEntity>(TContext model)最新方法（update为UpdateModel
    /// Damos 加
    /// </summary>
    public static class EFHelper
    {
        public static T ReturnSecurityObject<T>(T model)
        where T : class
        {
            Type t = model.GetType();
            foreach (PropertyInfo mi in t.GetProperties())
            {
                if (mi.PropertyType == "".GetType())
                {
                    var sx = HttpUtility.HtmlDecode(Sanitizer.GetSafeHtmlFragment((mi.GetValue(model) ?? "").ToString()));
                    //Regex.Replace((mi.GetValue(model)??"").ToString(), @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                    mi.SetValue(model, sx);
                }
            }
            return model;
        }

        #region 添加方法
        /// <summary>
        /// 添加WeChat相关表数据-------Old Method推荐使用AddModel或UpdateModel
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="entity">实例</param>
        /// <returns>实例</returns>
        public static T AddWeChat<T>(T entity, bool filter = true) where T : class, new()
        {
            creater_wxEntities db = new creater_wxEntities();
            if (filter)
                entity = ReturnSecurityObject<T>(entity);
            entity = db.Add<T>(entity);
            db.Dispose();
            return entity;
        }

        /// <summary>
        /// 添加log相关表数据-------Old Method推荐使用AddModel或UpdateModel
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="entity">实例</param>
        /// <returns>实例</returns>
        public static T AddLog<T>(T entity, bool filter = true) where T : class, new()
        {
            creater_wxEntities db = new creater_wxEntities();
            if (filter)
                entity = ReturnSecurityObject<T>(entity);
            entity = db.Add<T>(entity);
            db.Dispose();
            return entity;
        }

        /// <summary>
        /// 添加user相关表数据-------Old Method推荐使用AddModel或UpdateModel
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="entity">实例</param>
        /// <returns>实例</returns>
        public static T AddUser<T>(T entity, bool filter = true) where T : class, new()
        {
            creater_wxEntities db = new creater_wxEntities();
            if (filter)
                entity = ReturnSecurityObject<T>(entity);
            entity = db.Add<T>(entity);
            db.Dispose();
            return entity;
        }

        #endregion

        #region 修改方法
        /// <summary>
        /// 修改WeChat相关表数据-------Old Method推荐使用AddModel或UpdateModel
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="entity">实例</param>
        /// <returns>实例</returns>
        public static T UpdateWeChat<T>(T entity, bool filter = true) where T : class, new()
        {
            creater_wxEntities db = new creater_wxEntities();
            if (filter)
                entity = ReturnSecurityObject<T>(entity);
            entity = db.Update<T>(entity);
            db.Dispose();
            return entity;
        }


        /// <summary>
        /// 修改log相关表数据-------Old Method推荐使用AddModel或UpdateModel
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="entity">实例</param>
        /// <returns>实例</returns>
        public static bool UpdateLog<T>(T entity, bool filter = true) where T : class, new()
        {
            creater_wxEntities db = new creater_wxEntities();
            if (filter)
                entity = ReturnSecurityObject<T>(entity);
            entity = db.Update<T>(entity);
            bool isOk = entity != null;
            db.Dispose();
            return isOk;
        }


        /// <summary>
        /// 修改user相关表数据-------Old Method推荐使用AddModel或UpdateModel
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="entity">实例</param>
        /// <returns>实例</returns>
        public static bool UpdateUser<T>(T entity, bool filter = true) where T : class, new()
        {
            creater_wxEntities db = new creater_wxEntities();
            if (filter)
                entity = ReturnSecurityObject<T>(entity);
            bool isOK = db.UpdateOk<T>(entity);
            db.Dispose();
            return isOK;
        }

        #endregion



    }

    /// <summary>
    /// 这个类用于Lambda表达式的合成
    /// </summary>
    public static class DynamicLinqExpressions
    {

        public static Expression<Func<T, bool>> True<T>() { return f => true; }//永真表达式 -Damos
        public static Expression<Func<T, bool>> False<T>() { return f => false; }//永假表达式 -Damos

        //这个方法用于把两个lambda表达式合成一个||（OR）操作 -Damos
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }
        //这个方法用于把两个lambda表达式合成一个&&（AND）操作 -Damos
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }
        #region 扩展方法
        /// <summary>
        /// 注意！！！此为扩展方法，请勿直接调用！
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T Add<T>(this creater_wxEntities db, T entity) where T : class, new()
        {
            db.Set<T>().Add(entity);
            db.SaveChanges();

            return entity;
        }



        /// <summary>
        /// 注意！！！此为扩展方法，请勿直接调用！
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T Update<T>(this creater_wxEntities db, T entity) where T : class, new()
        {
            if (entity != null)
            {
                db.Set<T>().Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return entity;
        }



        /// <summary>
        /// 注意！！！此为扩展方法，请勿直接调用！
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool UpdateOk<T>(this creater_wxEntities db, T entity) where T : class, new()
        {
            bool isFinish = false;
            if (entity != null)
            {
                db.Set<T>().Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                isFinish = db.SaveChanges() > 0;
            }
            return isFinish;
        }

        #endregion

    }

}
