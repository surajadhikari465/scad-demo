using Dapper;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MoreLinq;
using System;
using System.Data;
using System.Linq;

namespace Mammoth.Esb.AttributeListener.Commands
{
    public class AddOrUpdateAttributesCommandHandler : ICommandHandler<AddOrUpdateAttributesCommand>
    {
        readonly IDbProvider db;

        public AddOrUpdateAttributesCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(AddOrUpdateAttributesCommand data)
        {
            AddOrUpdateAttributes(data);
        }

        private void AddOrUpdateAttributes(AddOrUpdateAttributesCommand data)
        {
            const string sql = @"
UPDATE dbo.Attributes
SET AttributeDesc = @Description
	,ModifiedDate = GETDATE()
    ,AttributeGroupId = @groupId
WHERE AttributeCode = @TraitCode

IF @@rowcount = 0
	INSERT INTO dbo.Attributes (
		AttributeCode
		,AttributeDesc
        ,AttributeGroupId
		,AddedDate
		)
	VALUES (
		@TraitCode
		,@Description
		,@groupId
		,GETDATE()
		)
	";

            foreach (var attr in data.Attributes.Attribute)
            {
                int? groupId = db.Connection.Query<int?>($@"
SELECT AttributeGroupId
FROM dbo.AttributeGroups
WHERE AttributeGroupDesc = '{attr.Group}'
").FirstOrDefault();

                db.Connection.Execute(sql, new { attr.TraitCode, attr.Description, groupId });
            }
        }
    }
}