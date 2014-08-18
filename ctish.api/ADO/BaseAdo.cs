using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Reflection;
using MySql.Data.MySqlClient;


namespace ctish.api.ADO
{
    public class BaseAdo
    {
        public string ConnectionStr { get; set; }
        public int PageSize { get; set; }
        private string _TableName = "";
        public string TableName { get { return _TableName; } set { _TableName = value; if (string.IsNullOrEmpty(this.ViewName)) this.ViewName = value; } }
        public string PrimaryKey { get; set; }
        public string ViewName { get; set; }

        public BaseAdo()
        {
            this.ConnectionStr = ConfigurationManager.AppSettings["ConnectionString"];
            string pagesize = ConfigurationManager.AppSettings["pageSize"];
            int p = 10;
            if (!int.TryParse(pagesize, out p))
                p = 10;
            this.PageSize = p;
        }
        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数列表</param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql, params MySqlParameter[] param)
        {
            DataSet ds = MySqlHelper.ExecuteDataset(this.ConnectionStr, sql, param);
            return ds;
        }
        /// <summary>
        /// 执行一条sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数列表</param>
        /// <returns></returns>
        public int ExecuteSql(string sql, params MySqlParameter[] param)
        {
            return MySqlHelper.ExecuteNonQuery(this.ConnectionStr, sql, param);
        }
        /// <summary>
        /// 根据where条件获取数据集
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="columns">要查询的字段</param>
        /// <param name="param">参数列表</param>
        /// <returns></returns>
        public DataSet GetDataSet(string where, string orderby, string columns = "*", params MySqlParameter[] param)
        {
            if (where.Length > 0)
                where = "where " + where;
            if (orderby.Length > 0)
                orderby = "order by " + orderby;
            if (columns.Length == 0)
                columns = "*";
            string sql = "select {0} from {1} {2} {3}";
            sql = string.Format(sql, columns, this.ViewName, where, orderby);
            return this.GetDataSet(sql, param);
        }
        /// <summary>
        /// 获取分页数据集
        /// </summary>
        /// <param name="pagenumber">页码</param>
        /// <param name="where">where条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="columns">要查询的字段</param>
        /// <param name="param">参数列表</param>
        /// <returns></returns>
        public DataSet GetDataSet(int pagenumber, string where, string orderby, out int totalCount, string columns = "*", params MySqlParameter[] param)
        {
            totalCount = 0;
            totalCount = this.GetRecordCount(where, param);
            int start = this.PageSize * (pagenumber - 1);
            int end = this.PageSize * pagenumber;
            if (where.Length > 0)
                where = "where " + where;
            if (columns.Length == 0)
                columns = "*";

            if (orderby.Length == 0)
                orderby = this.PrimaryKey;
            else if (!orderby.Contains(this.PrimaryKey))
                orderby = orderby + "," + this.PrimaryKey;
            orderby = "order by " + orderby;
            string sql = @"SELECT {0} FROM {1} {2} {3} LIMIT {4},{5}";
            sql = string.Format(sql, columns, this.ViewName, where, orderby, start, end);

            return this.GetDataSet(sql, param);
        }
        /// <summary>
        /// 根据主键ID获取数据
        /// </summary>
        /// <param name="id">ID号</param>
        /// <param name="columns">要查询的字段</param>
        /// <returns></returns>
        public DataSet GetDataByID(string id, string columns = "*")
        {
            if (columns.Length == 0)
                columns = "*";
            string sql = "select {0} from {1} where {2}=?id";
            sql = string.Format(sql, columns, this.ViewName, this.PrimaryKey);
            DataSet ds = this.GetDataSet(sql, new MySqlParameter("?id", id));
            return ds;
        }
        /// <summary>
        /// 获取表最新的ID号，仅限有存储过程PROC_GETID和表ID01时使用
        /// </summary>
        /// <returns></returns>
        public string GetNewID()
        {
            MySqlCommand command = new MySqlCommand();
            command.CommandText = "PROC_GETID";
            command.CommandType = CommandType.StoredProcedure;
            MySqlParameter param = new MySqlParameter("?OUTID", MySqlDbType.VarChar, 50);
            param.Direction = ParameterDirection.Output;
            command.Parameters.Add(new MySqlParameter("?TABLENAME", this.TableName));
            command.Parameters.Add(param);

            using (MySqlConnection connection = new MySqlConnection(this.ConnectionStr))
            {
                connection.Open();
                command.Connection = connection;
                int rows = command.ExecuteNonQuery();
                return param.Value + "";
            }
        }
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="param">sql参数</param>
        /// <returns></returns>
        public int GetRecordCount(string where = "", params MySqlParameter[] param)
        {
            string count = this.GetScalar("count(1)", where, param);
            int result = 0;
            if (int.TryParse(count, out result))
                return result;
            else
                return 0;
        }
        /// <summary>
        /// 获取第一条的第一个结果
        /// </summary>
        /// <param name="column">要查询的字段</param>
        /// <param name="where">where条件</param>
        /// <param name="param">sql参数</param>
        /// <returns></returns>
        public string GetScalar(string column, string where = "", params MySqlParameter[] param)
        {
            string sql = "select " + column + " from " + this.ViewName;
            if (where.Trim().Length > 0)
            {
                sql += " where " + where;
            }
            DataSet ds = this.GetDataSet(sql, param);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][0] + "";
            else
                return "";
        }
    }
}