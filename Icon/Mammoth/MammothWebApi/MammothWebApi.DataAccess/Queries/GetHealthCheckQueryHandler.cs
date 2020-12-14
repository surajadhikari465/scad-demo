﻿using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MammothWebApi.DataAccess.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetHealthCheckQueryHandler : IQueryHandler<GetHealthCheckQuery, int >
    {
        private IDbProvider db;

        public GetHealthCheckQueryHandler(IDbProvider db)
        {
            this.db = db;
        }

        public int Search(GetHealthCheckQuery parameters)
        {
            string sql = @"SELECT TOP 1 CheckId 
                            FROM [dbo].[HealthCheck]";
            int checkId = this.db.Connection.Query<int>(sql,
                this.db.Transaction).Single();
            return checkId;
        }
    }
}
