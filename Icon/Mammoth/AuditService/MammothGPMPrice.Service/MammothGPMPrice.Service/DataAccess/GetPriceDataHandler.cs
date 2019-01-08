using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothGpmService.Models;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

namespace MammothGpmService.DataAccess
{
	public class GetPriceDataHandler : IGetPriceDataHandler
	{
		private string filepath = ConfigurationManager.AppSettings["LocalFilePath"].ToString();
		const string PIPE = "|";

		public bool GetAuditFileByRegion(string region)
		{
			var regionFile = filepath + "_" + region + ".txt";
			if (File.Exists(regionFile))
			{
				File.Delete(regionFile);
			}

			using (var fl = new StreamWriter(regionFile))
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString))
			using (var sqlCommand = new SqlCommand("exec MammothGpmPriceExtract @Region = '" + region + "'", connection))
			{
				connection.Open();
				var reader = sqlCommand.ExecuteReader();
				if (reader.Read())
				{
					while (reader.Read())
					{
						var data = new object[reader.FieldCount];
						reader.GetValues(data);

						fl.Write(String.Format("{0}{1}", String.Join(PIPE, data.Select(x => x.ToString())), PIPE));
					}
					return true;
				}
				else
					return false;
			}
		}

		public void DeleteFile(string region)
		{
			var regionFile = filepath + "_" + region + ".txt";
			if (File.Exists(regionFile))
			{
				File.Delete(regionFile);
			}
		}
	}
}
