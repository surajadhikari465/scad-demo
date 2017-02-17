using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;

namespace MammothWebApi.DataAccess.Commands
{
    public class AddOrUpdateItemLocaleCommandHandler : ICommandHandler<AddOrUpdateItemLocaleCommand>
    {
        private IDbProvider db;

        public AddOrUpdateItemLocaleCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(AddOrUpdateItemLocaleCommand data)
        {
            string sql = @"MERGE dbo.ItemAttributes_Locale_{0} WITH (updlock, rowlock) il
		                    USING
		                    (
			                    SELECT DISTINCT
				                    sil.Region,
				                    i.ItemID,
				                    l.BusinessUnitID,
				                    sil.Discount_Case,
				                    sil.Discount_TM,
				                    sil.Restriction_Age,
				                    sil.Restriction_Hours,
				                    sil.Authorized,
				                    sil.Discontinued,
				                    sil.LabelTypeDesc,
				                    sil.LocalItem,  
				                    sil.Product_Code,
				                    sil.RetailUnit,
				                    sil.Sign_Desc,
				                    sil.Locality,
				                    sil.Sign_RomanceText_Long,
				                    sil.Sign_RomanceText_Short,
                                    sil.Msrp
			                    FROM 
				                    stage.ItemLocale	        sil
				                    INNER JOIN dbo.Items		i	on sil.ScanCode = i.ScanCode
				                    INNER JOIN dbo.Locales_{0}  l   on sil.BusinessUnitID = l.BusinessUnitID  -- to ensure the Locale exists in Mammoth
			                    WHERE 
				                    sil.Region = @Region
				                    AND sil.TransactionId = @TransactionId
		                    )	s
		                    ON
			                    il.Region = s.Region
			                    AND il.ItemID = s.ItemID
			                    AND il.BusinessUnitID = s.BusinessUnitID
		                    WHEN MATCHED THEN
			                    UPDATE
			                    SET
				                    il.Discount_Case			= s.Discount_Case,
				                    il.Discount_TM				= s.Discount_TM,
				                    il.Restriction_Age			= s.Restriction_Age,
				                    il.Restriction_Hours		= s.Restriction_Hours,
				                    il.Authorized				= s.Authorized,
				                    il.Discontinued				= s.Discontinued,
				                    il.LocalItem				= s.LocalItem,
				                    il.LabelTypeDesc			= s.LabelTypeDesc,
				                    il.Product_Code				= s.Product_Code,
				                    il.RetailUnit				= s.RetailUnit,
				                    il.Sign_Desc				= s.Sign_Desc,
				                    il.Locality					= s.Locality,
				                    il.Sign_RomanceText_Long	= s.Sign_RomanceText_Long,
				                    il.Sign_RomanceText_Short	= s.Sign_RomanceText_Short,
                                    il.MSRP                     = s.Msrp,
				                    il.ModifiedDate				= @Timestamp
		                    WHEN NOT MATCHED THEN
			                    INSERT
			                    (
				                    ItemID,
				                    BusinessUnitID,
				                    Discount_Case,
				                    Discount_TM,
				                    Restriction_Age,
				                    Restriction_Hours,
				                    Authorized,
				                    Discontinued,
				                    LocalItem,
				                    LabelTypeDesc,
				                    Product_Code,
				                    RetailUnit,
				                    Sign_Desc,
				                    Locality,
				                    Sign_RomanceText_Long,
				                    Sign_RomanceText_Short,
                                    MSRP,
				                    AddedDate
			                    ) 
			                    VALUES
			                    (
				                    s.ItemID,
				                    s.BusinessUnitID,
				                    s.Discount_Case,
				                    s.Discount_TM,
				                    s.Restriction_Age,
				                    s.Restriction_Hours,
				                    s.Authorized,
				                    s.Discontinued,
				                    s.LocalItem,
				                    s.LabelTypeDesc,
				                    s.Product_Code,
				                    s.RetailUnit,
				                    s.Sign_Desc,
				                    s.Locality,
				                    s.Sign_RomanceText_Long,
				                    s.Sign_RomanceText_Short,
                                    s.Msrp,
				                    @Timestamp
			                    );";

            int affectedRows = this.db.Connection.Execute(String.Format(sql, data.Region),
                new { Region = new DbString { Value = data.Region, Length = 2 }, Timestamp = data.Timestamp, TransactionId = data.TransactionId },
                transaction: this.db.Transaction);
            return;            
        }
    }
}
