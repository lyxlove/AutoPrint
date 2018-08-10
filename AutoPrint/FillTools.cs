using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AutoPrint
{
    public  class FillTools
    {
        public static T FillEntity<T>(DataTable dt)
        {
            T t = Activator.CreateInstance<T>();
            if (dt.Rows.Count > 0)
            {
                PropertyInfo[] properties = t.GetType().GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    if (dt.Columns.Contains(info.Name))
                    {
                        if (dt.Rows[0][info.Name] != DBNull.Value)
                        {
                            info.SetValue(t, Convert.ChangeType(dt.Rows[0][info.Name], info.PropertyType), null);
                        }
                    }
                }
            }
            return t;
        }

        public static List<T> FillList<T>(DataTable dt)
        {
            List<T> list = new List<T>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                T t = Activator.CreateInstance<T>();
                PropertyInfo[] properties = t.GetType().GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    if (dt.Columns.Contains(info.Name))
                    {
                        if (dt.Rows[i][info.Name] != DBNull.Value)
                        {
                            info.SetValue(t, Convert.ChangeType(dt.Rows[i][info.Name], info.PropertyType), null);
                        }
                    }
                }
                list.Add(t);
            }
            return list;
        }
    }
}
