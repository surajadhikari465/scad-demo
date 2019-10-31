using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebSupport.DataAccess
{
  public class DBAdapter : IDisposable
  {
    SqlConnection connection;

    public DBAdapter(string dbKey)
    {
      var value = ConfigurationManager.ConnectionStrings[$"IRMA_{dbKey}"];
      
      connection = value == null ? null : new SqlConnection(value.ConnectionString);
      if(connection == null) throw new Exception($"Missing configuration: IRMA_{dbKey}");

      Open();
      Close();
    }

    public void Dispose(){ Close(true); }

    void Close(bool bDispose = false)
    {
      try { if(connection != null && connection.State != ConnectionState.Closed) connection.Close(); }
      catch{}
      finally { if(bDispose) connection = null; }
    }

    void Open()
    {
      if(connection.State != System.Data.ConnectionState.Closed) connection.Close();
      connection.Open();
    }

    public DataSet ExecuteDataSet(string spName, CommandType cmdType = CommandType.StoredProcedure, SqlParameter[] parameters = null)
    {
      try
      {
        using(var cmd = new SqlCommand(spName, connection) { CommandType = cmdType })
        using(var adapter = new SqlDataAdapter(cmd))
        {
		  cmd.CommandTimeout = 60000;
          if(parameters != null && parameters.Where(x => x != null).Any())
            cmd.Parameters.AddRange(parameters.Where(x => x != null).ToArray());
         
          Open();
          var dSet = new DataSet();
          adapter.Fill(dSet);
          return dSet;
        }
      }
      catch { throw; }
      finally { Close(); }
    }

    public void ExecuteNonQuery(string spName, CommandType cmdType = CommandType.StoredProcedure, SqlParameter[] parameters = null)
    {
      try
      {
        using(var cmd = new SqlCommand(spName, connection) { CommandType = cmdType, CommandTimeout = 300 })
        {
          if(parameters != null && parameters.Where(x => x != null).Any())
            cmd.Parameters.AddRange(parameters.Where(x => x != null).ToArray());

          Open();
          cmd.ExecuteNonQuery();
        }
      }
      catch { throw; }
      finally { Close(); }
    }

     public object ExecuteScalar(string sql, CommandType cmdType, params SqlParameter[] parameters)
    {
      try
      {
        using(var cmd = new SqlCommand(sql, connection) { CommandType = cmdType, CommandTimeout = 300 })
        {
          if(parameters != null && parameters.Where(x => x != null).Any())
            cmd.Parameters.AddRange(parameters.Where(x => x != null).ToArray());

          Open();
          return cmd.ExecuteScalar();
        }
      }
      catch { throw; }
      finally { Close(); }
    }
  }
}