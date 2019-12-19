using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using static OOSCommon.DataAccess.DataAccessExtensions;

namespace OOSCommon.DataAccess
{

    public interface IDataAccess : IDisposable
    {
        DataTable ExecuteQuery(string sql, OracleParameter[] paramters = null);
        void ExecuteNonQuery(string sql, OracleParameter[] paramters = null);
        List<T> ReturnList<T>(CommandType commandType, string commandText, OracleParameter[] parameters = null) where T : new();
    }
    public class OracleDataAccess : IDataAccess
    {
        private readonly OracleConnection _cn;
        private readonly int _commandTimeout;

        public OracleDataAccess(string connectionString, int commandTimeout=300)
        {
            _commandTimeout = commandTimeout;
            _cn = new OracleConnection(connectionString);
            _cn.Open();
        }

        public DataTable ExecuteQuery(string sql, OracleParameter[] paramters=null)
        {
            var dt = new DataTable();
            var cmd = new OracleCommand(sql, _cn);
            cmd.CommandTimeout = _commandTimeout;
            if (paramters != null) cmd.Parameters.AddRange(paramters);

            var da = new OracleDataAdapter(cmd);
            da.Fill(dt);
            da.Dispose();
            cmd.Dispose();

            return dt;
        }

        public void ExecuteNonQuery(string sql, OracleParameter[] paramters=null)
        {
            var cmd = new OracleCommand(sql, _cn);
            cmd.CommandTimeout = _commandTimeout;
            if (paramters != null) cmd.Parameters.AddRange(paramters);
            cmd.ExecuteNonQuery();
        }

        public List<T> ReturnList<T>(CommandType commandType, string commandText, OracleParameter[] parameters = null) where T : new()
        {
            var dt = new DataTable();
            var cmd = new OracleCommand(commandText);
            cmd.Connection = _cn;
            cmd.CommandTimeout = _commandTimeout;
            if (parameters != null) cmd.Parameters.AddRange(parameters);

            var da = new OracleDataAdapter(cmd);
            da.Fill(dt);
            var list = ToList<T>(dt);

            da.Dispose();
            cmd.Dispose();

            return list;
        }


        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~OracleDataAccess()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
                if (_cn.State!=ConnectionState.Closed) _cn.Close();
                _cn?.Dispose();
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

    }

    public static class DataAccessExtensions
    {
        public static List<T> ToList<T>(DataTable datatable) where T : new()
        {
            var temp = new List<T>();
            try
            {
                var columnsNames = new List<string>();
                foreach (DataColumn dataColumn in datatable.Columns)
                    columnsNames.Add(dataColumn.ColumnName);
                temp = datatable.AsEnumerable().ToList().ConvertAll<T>(row => GetObject<T>(row, columnsNames));
                return temp;
            }
            catch { return temp; }
        }
        private static T GetObject<T>(DataRow row, List<string> columnsName) where T : new()
        {
            T obj = new T();
            try
            {
                var properties = typeof(T).GetProperties();
                foreach (PropertyInfo objProperty in properties)
                {
                    var columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());
                    if (!string.IsNullOrEmpty(columnname))
                    {
                        var value = row[columnname].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            {
                                value = row[columnname].ToString().Replace("$", "").Replace(",", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[columnname].ToString().Replace("%", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                            }
                        }
                    }
                }
                return obj;
            }
            catch { return obj; }
        }
    }
}
