using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Data;

namespace Yaeher.Common
{
    public static  class LinqHelper
    {
        /// <summary> 
        /// 分页  
        /// <summary> 
        /// <typeparam name="T">type<param> 
        /// <param name="List">实现IEnumerable<param> 
        /// <param name="FunWhere">delegate检索条件<param> 
        /// <param name="FunOrder">delegate排序<param> 
        /// <param name="PageSize">每页显示数<param> 
        /// <param name="PageIndex">当前页码<param> 
        /// <returns>returns> 
       public static IEnumerable<T> GetIenumberable<T>(IEnumerable<T> List, Func<T,
         bool> FunWhere, Func<T, string> FunOrder, int PageSize, int PageIndex)
       {
           var rance = List.Where(FunWhere).OrderByDescending(FunOrder).
           Select(t => t).Skip((PageIndex - 1) * PageSize).Take(PageSize);
           return rance;
       }
       /// <summary>
       ///      region 利用反射把DataTable的数据写到单个实体类
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="dtSource"></param>
       /// <returns></returns>
         public static T ToSingleEntity<T>(this System.Data.DataTable dtSource)
        {
            if (dtSource == null)
            {
                return default(T);
            }
 
            if (dtSource.Rows.Count != 0)
            {
                Type type = typeof(T);
                Object entity = Activator.CreateInstance(type);         //创建实例               
                foreach (PropertyInfo entityCols in type.GetProperties())
                {
                    if (!string.IsNullOrEmpty(dtSource.Rows[0][entityCols.Name].ToString()))
                    {
                        entityCols.SetValue(entity, dtSource.Rows[0][entityCols.Name], null);
                    }
                }
                return (T)entity;
            }
            return default(T);
        }
    
 
        /// <summary>
        /// 利用反射把DataTable的数据写到集合实体类里
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dtSource"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToListEntity<T>(this System.Data.DataTable dtSource)
        {
            if (dtSource == null)
            {
                return null;
            }
 
            List<T> list = new List<T>();
            Type type = typeof(T);
            foreach (DataRow dataRow in dtSource.Rows)
            {
                Object entity = Activator.CreateInstance(type);         //创建实例               
                foreach (PropertyInfo entityCols in type.GetProperties())
                {
                    if (!string.IsNullOrEmpty(dataRow[entityCols.Name].ToString()))
                    {
                        entityCols.SetValue(entity, dataRow[entityCols.Name], null);
                    }
                }
                list.Add((T)entity);
            }
            return list;
        }

    }
}
