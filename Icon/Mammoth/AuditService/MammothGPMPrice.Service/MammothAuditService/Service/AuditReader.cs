using System;
using System.Data;
using System.Data.SqlClient;

namespace Audit
{
	public class AuditReader : IDisposable
	{
		public enum Status { Failed = -1, None = 0, Executing = 1, Ready = 2, Empty = 3 }

		int groupID;
		readonly SpecInfo spec;
		SqlConnection connection;
		readonly AuditOutputInfo auditInfo;

		public Status CurrentStatus { get; private set; }
		public SqlDataReader DataReader { get; private set; }

		public AuditReader(SpecInfo spec, AuditOutputInfo auditInfo, int groupID)
		{
			this.spec = spec;
			this.groupID = groupID;
			this.auditInfo = auditInfo;
			this.CurrentStatus = Status.None;
		}

		void Close()
		{
			try
			{
				if(this.connection != null && this.connection.State != ConnectionState.Closed)
				{
					this.connection.Close();
				}
			}
			catch { }
			finally
			{
				this.DataReader = null;
				this.connection = null;
			}
		}

		public void Dispose() { Close(); }

		public void Execute()
		{
			this.CurrentStatus = Status.Executing;

			try
			{
				this.connection = new SqlConnection(this.spec.ConnectionString);
				using(var sqlCommand = new SqlCommand($"{this.auditInfo.Query} @action = 'Get', @groupID = {groupID}", this.connection) { CommandTimeout = this.spec.CommandTimeOut })
				{
					this.connection.Open();
					this.DataReader = sqlCommand.ExecuteReader();

					this.CurrentStatus = this.DataReader.HasRows ? Status.Ready : Status.Empty;
					if(this.CurrentStatus == Status.Empty)
					{
						this.DataReader.Close();
						Close();
					}
				}
			}
			catch
			{
				this.CurrentStatus = Status.Failed;
				throw;
			}
		}
	}
}