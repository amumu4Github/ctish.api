using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Text;
using MySql.Data.MySqlClient;


namespace ctish.api.ADO
{
    public class ModelAdo<T> : BaseAdo
    {
        private Hashtable ColumnMapping = new Hashtable();
        private bool PrimaryKeyIsStr = false;
        private string PrimaryMember = "";
        public ModelAdo()
            : base()
        {
            Type t = typeof(T);
            if (t.IsDefined(typeof(TablenameAttribute), false))
            {
                Object[] objAttrs = t.GetCustomAttributes(typeof(TablenameAttribute), false);
                TablenameAttribute tn = objAttrs[0] as TablenameAttribute;
                this.ViewName = tn.ViewName;
                this.TableName = tn.TableName;
                this.PrimaryKey = tn.PrimaryKey;
            }
            if (this.TableName.Length == 0 || this.PrimaryKey.Length == 0)
                throw new Exception("未给T设置类属性TablenameAttribute");
            else
            {
                PropertyInfo[] fis = t.GetProperties();
                foreach (PropertyInfo item in fis)
                {
                    if (item.IsDefined(typeof(ColumnameAttribute), false))
                    {
                        Object[] objAttrs = item.GetCustomAttributes(typeof(ColumnameAttribute), false);
                        ColumnameAttribute tn = objAttrs[0] as ColumnameAttribute;
                        ColumnMapping[item.Name] = tn.Name;
                        if (tn.Name == this.PrimaryKey)
                        {
                            this.PrimaryMember = item.Name;
                            if (item.PropertyType == typeof(string))
                            {
                                this.PrimaryKeyIsStr = true;
                            }
                        }
                    }
                }
                if (this.PrimaryMember.Length == 0)
                    throw new Exception("未在类上标记主键字段");
            }
        }
        /// <summary>
        /// 获取Model List
        /// </summary>
        /// <param name="sql">sql查询语句，修改</param>
        /// <param name="param">参数列表</param>
        /// <returns></returns>
        public List<T> GetList(string sql, params MySqlParameter[] param)
        {
            DataSet ds = this.GetDataSet(sql, param);
            if (ds != null)
            {
                return this.GetList(ds.Tables[0]);
            }
            return null;
        }
        /// <summary>
        /// 获取Model List
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="columns">要查询的字段</param>
        /// <param name="param">参数列表 </param>
        /// <returns></returns>
        public List<T> GetList(string where, string orderby, string columns = "*", params MySqlParameter[] param)
        {
            DataSet ds = this.GetDataSet(where, orderby, columns, param);
            if (ds != null)
            {
                return this.GetList(ds.Tables[0]);
            }
            return null;
        }
        /// <summary>
        /// 获取分页Model List
        /// </summary>
        /// <param name="pagenumber">要查询的页数</param>
        /// <param name="where">where条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="columns">要查询的字段</param>
        /// <param name="param">参数列表</param>
        /// <returns></returns>
        public List<T> GetList(int pagenumber, string where, string orderby, out int totalCount, string columns = "*", params MySqlParameter[] param)
        {
            DataSet ds = this.GetDataSet(pagenumber, where, orderby, out totalCount, columns, param);
            if (ds != null)
            {
                return this.GetList(ds.Tables[0]);
            }
            return null;
        }
        /// <summary>
        /// 根据主键ID获取一个Model
        /// </summary>
        /// <param name="id">ID号</param>
        /// <param name="columns">要查询的字段</param>
        /// <returns></returns>
        public T GetModelByID(string id, string columns = "*")
        {
            DataSet ds = this.GetDataByID(id, columns);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
                return this.GetModelByDatarow(ds.Tables[0].Rows[0]);
            return default(T);
        }
        /// <summary>
        /// 获取一个Model
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="columns">要查询的字段</param>
        /// <param name="param">参数列表</param>
        /// <returns></returns>
        public T GetModel(string where, string columns = "*", params MySqlParameter[] param)
        {
            DataSet ds = this.GetDataSet(where, "", "", param);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return this.GetModelByDatarow(ds.Tables[0].Rows[0]);
            }
            return default(T);
        }

        private List<T> GetList(DataTable table)
        {
            if (table != null)
            {
                List<T> list = new List<T>();
                foreach (DataRow item in table.Rows)
                {
                    list.Add(this.GetModelByDatarow(item));
                }
                return list;
            }
            return null;
        }
        /// <summary>
        /// 根据Model修改一条数据，此修改会以ID作为修改条件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(T obj)
        {
            if (obj != null)
            {
                Type t = typeof(T);
                PropertyInfo fi = t.GetProperty(this.PrimaryMember);
                string id = fi.GetValue(obj, null) + "";
                if (id.Length > 0)
                {
                    string sql = "update {0} set {1} where {2}=?" + this.PrimaryMember;
                    List<MySqlParameter> listParam = new List<MySqlParameter>();
                    StringBuilder sb = new StringBuilder("");
                    foreach (string item in this.ColumnMapping.Keys)
                    {
                        PropertyInfo f = t.GetProperty(item);
                        Object[] objAttrs = f.GetCustomAttributes(typeof(ColumnameAttribute), false);
                        ColumnameAttribute tn = objAttrs[0] as ColumnameAttribute;
                        if (tn.canUpdate || f.Name == this.PrimaryMember)
                        {
                            object value = this.GetValue(f, obj);
                            if (value == null && tn.DefaultValue != null)
                                value = tn.DefaultValue;
                            if (item != this.PrimaryMember)
                                sb.Append(string.Format(",{0}=?{1}", this.ColumnMapping[item], item));
                            listParam.Add(new MySqlParameter("?" + item, value));
                        }
                    }
                    if (sb.Length > 0)
                        sb.Remove(0, 1);
                    sql = string.Format(sql, this.TableName, sb.ToString(), this.PrimaryKey);
                    return this.ExecuteSql(sql, listParam.ToArray());
                }
            }
            return 0;
        }
        /// <summary>
        /// 根据主键ID删除一条数据
        /// </summary>
        /// <param name="id">要删除的数据</param>
        /// <returns></returns>
        public int Delete(string id)
        {
            string sql = "delete from {0} where {1}=?id";
            sql = string.Format(sql, this.TableName, this.PrimaryKey);
            return this.ExecuteSql(sql, new MySqlParameter("?id", id));
        }
        /// <summary>
        /// 根据Model插入一条数据。若主键字段为int型，则将主键看作为自增长不会显示插入；若主键字段为string型，可显示指定ID值，放空将调用GET_NEWID获取一个新的ID
        /// </summary>
        /// <param name="obj">要插入的Model</param>
        /// <returns></returns>
        public int Insert(T obj)
        {
            if (obj != null)
            {
                Type t = typeof(T);
                PropertyInfo fi = t.GetProperty(this.PrimaryMember);
                string id = fi.GetValue(obj, null) + "";
                if (id.Length == 0 && this.PrimaryKeyIsStr)
                    id = this.GetNewID();
                StringBuilder columns = new StringBuilder();
                StringBuilder values = new StringBuilder();
                List<MySqlParameter> listParam = new List<MySqlParameter>();
                foreach (string item in this.ColumnMapping.Keys)
                {
                    if (item != this.PrimaryMember)
                    {
                        PropertyInfo f = t.GetProperty(item);
                        Object[] objAttrs = f.GetCustomAttributes(typeof(ColumnameAttribute), false);
                        ColumnameAttribute tn = objAttrs[0] as ColumnameAttribute;
                        if (tn.canInsert && f.Name != this.PrimaryMember)
                        {
                            object value = this.GetValue(f, obj);
                            if (value == null && tn.DefaultValue != null)
                                value = tn.DefaultValue;
                            columns.Append("," + this.ColumnMapping[item]);
                            values.Append(",?" + item);
                            listParam.Add(new MySqlParameter("?" + item, value));
                        }
                    }
                }
                if (columns.Length > 0)
                {
                    columns.Remove(0, 1);
                    values.Remove(0, 1);
                }
                string sql = "insert into {0}({3}{1}) values({4}{2})";
                if (this.PrimaryKeyIsStr)
                {
                    sql = "insert into {0}({3}{1}) select {4}{2} from dual where not exists (select * from {0} where " + this.PrimaryKey + "=?" + this.PrimaryKey + ")";
                    sql = string.Format(sql, this.TableName, columns.ToString(), values.ToString(), this.PrimaryKey + ",", "?" + this.PrimaryMember + ",");
                    listParam.Add(new MySqlParameter("?" + this.PrimaryMember, id));
                    listParam.Add(new MySqlParameter("?" + this.PrimaryKey, id));
                }
                else
                {
                    sql = string.Format(sql, this.TableName, columns.ToString(), values.ToString(), "", "");
                }
                return this.ExecuteSql(sql, listParam.ToArray());
            }
            return 0;
        }
        private void SetValue(PropertyInfo pInfo, object obj, object value)
        {
            if (obj != null && pInfo != null)
            {
                if (pInfo.PropertyType == typeof(bool))
                {
                    string boolv = value + "";
                    if (boolv.ToLower() == "false" || boolv == "0")
                        pInfo.SetValue(obj, false, null);
                    else
                        pInfo.SetValue(obj, true, null);
                }
                else
                    pInfo.SetValue(obj, value, null);
            }
        }
        private object GetValue(PropertyInfo pInfo, object obj)
        {
            if (obj != null && pInfo != null)
            {
                object value = pInfo.GetValue(obj, null);
                if (pInfo.PropertyType == typeof(bool))
                {
                    string v = value + "";
                    if (v.ToLower() == "false" || v == "0" || v.Length == 0)
                        return false;
                    else
                        return true;
                }
                else
                    return value;
            }
            else
                return null;
        }
        private T GetModelByDatarow(DataRow row)
        {
            if (row != null)
            {
                Type t = typeof(T);
                Assembly ass = Assembly.GetAssembly(t);
                T obj = (T)ass.CreateInstance(t.FullName);
                foreach (string item in ColumnMapping.Keys)
                {
                    PropertyInfo fi = t.GetProperty(item);
                    object value = row[ColumnMapping[item] + ""];
                    if (!Convert.IsDBNull(value))
                    {
                        this.SetValue(fi, obj, value);
                    }
                }
                return obj;
            }
            else
                return default(T);
        }
    }
}