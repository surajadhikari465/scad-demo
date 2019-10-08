CREATE PROCEDURE [dbo].[AddOrUpdateItemAttributesLocale_FromStaging]
	@Region NCHAR(2),
	@Timestamp DATETIME
AS
BEGIN

	-- =========================================
	-- Locale Variables
	-- =========================================
	DECLARE @region_local NCHAR(2);
	DECLARE @timestamp_local DATETIME;
	DECLARE @SQL NVARCHAR(MAX);

	SET @region_local = @Region;
	SET @timestamp_local = @Timestamp;	

	-- =========================================
	-- Main
	-- =========================================
	PRINT 'Updating dbo.ItemAttributes_Locale table for ' + @region_local;
	SET @SQL = '
		MERGE dbo.ItemAttributes_Locale_' + @region_local + ' WITH (updlock, rowlock) il
		USING
		(
			SELECT DISTINCT
				' + QUOTENAME(@region_local, '''') + ' as Region,
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
				sil.Sign_RomanceText_Short
			FROM 
				Staging.dbo.ItemLocale	sil
				INNER JOIN Items		i	on sil.ScanCode = i.ScanCode
				INNER JOIN Locales_' + @region_local + ' l on sil.BusinessUnitID = l.BusinessUnitID
			WHERE 
				sil.Region = ' + QUOTENAME(@region_local,'''') + '
				AND sil.Timestamp = ''' + CONVERT(nvarchar, @timestamp_local, 121) + '''
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
				il.ModifiedDate				= ''' + CONVERT(nvarchar, @timestamp_local, 121) + '''
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
				''' + CONVERT(nvarchar, @timestamp_local, 121) + '''
			);'

	PRINT @SQL
	EXEC sp_executesql @SQL
END
GO

