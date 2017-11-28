using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Configuration;
namespace Tool
{
    public static class OleDbHelper
    {
        private static OleDbConnection connection;
        /// <summary>
        /// 获得一个唯一的CONNECTION 实例
        /// </summary>
        public static OleDbConnection Connection
        {
            get
            {
                string connectionstring =
                "Provider=Microsoft.Jet.OLEDB.4.0;  data source=" + System.Web.HttpContext.Current.Server.MapPath("app_data/data.mdb") + ";Persist Security Info=False;";

                if (connection == null)
                {
                    connection = new OleDbConnection(connectionstring);
                    connection.Open();
                }
                else if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();

                }
                else if (connection.State == System.Data.ConnectionState.Broken)
                {
                    connection.Close();
                    connection.Open();
                }
                return connection;
            }
        }
        /// <summary>
        /// 返回执行SQL 语句所影响数据的行数
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <returns>影响行数</returns>
        public static int ExecuteCommand(string sql)
        {
            OleDbCommand com = new OleDbCommand(sql, Connection);
            int result = com.ExecuteNonQuery();
            return result;
        }
        /// <summary>
        /// 获取结果集的第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int GetScalar(string sql)
        {
            OleDbCommand com = new OleDbCommand(sql, Connection);
            int result = 0;
            try
            {
                result = (int)com.ExecuteScalar();
            }
            catch (OleDbException)
            {
                result = 0;
            }
            return result;
        }
        /// <summary>
        ///  执行Sql语句,获取DataTable结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string sql)
        {
            DataSet dataset = new DataSet();
            OleDbCommand com = new OleDbCommand(sql, Connection);
            OleDbDataAdapter da = new OleDbDataAdapter(com);
            da.Fill(dataset);
            return dataset.Tables[0];
        }
        /// <summary>
        /// 执行Sql语句,获取DataSet结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string sql)
        {
            DataSet dataset = new DataSet();
            OleDbCommand com = new OleDbCommand(sql, Connection);
            OleDbDataAdapter da = new OleDbDataAdapter(com);
            da.Fill(dataset);
            return dataset;
        }
        /// <summary>
        /// 获取OleDbDataReader结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>OleDbDataReader</returns>
        public static OleDbDataReader GetReader(string sql)
        {
            OleDbCommand com = new OleDbCommand(sql, Connection);
            OleDbDataReader reader = com.ExecuteReader();
            return reader;
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="cmdParms">参数</param>
        /// <returns>影响行数</returns>
        public static int ExecuteSql(string SQLString, params OleDbParameter[] cmdParms)
        {
            OleDbCommand com = new OleDbCommand(SQLString, Connection);
            PrepareCommand(com, Connection, null, SQLString, cmdParms);
            int rows = com.ExecuteNonQuery();
            com.Parameters.Clear();
            return rows;
        }
        /// <summary>
        /// 获取DataSet结果集
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="cmdParms">参数</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string SQLString, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand(SQLString, Connection);
            PrepareCommand(cmd, Connection, null, SQLString, cmdParms);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                da.Fill(ds, "ds");
                cmd.Parameters.Clear();
            }
            catch
            {

            }
            finally
            {

            }
            return ds;
        }
        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, string cmdText, OleDbParameter[] cmdParms)
        {
            cmd.CommandText = cmdText;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (OleDbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}