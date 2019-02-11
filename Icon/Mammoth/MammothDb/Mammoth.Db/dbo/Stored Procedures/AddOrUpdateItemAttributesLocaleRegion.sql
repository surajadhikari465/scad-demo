CREATE PROCEDURE [dbo].[AddOrUpdateItemAttributesLocaleRegion] @region NVARCHAR(2)
	,@dateStamp DATETIME
	,@transactionId VARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @sql NVARCHAR(max) = 'SET NOCOUNT ON;
  IF OBJECT_ID(''tempdb..#item'') IS NOT NULL DROP TABLE #item;

  SELECT A.Region,
         B.ItemID,
         A.BusinessUnitID,
         A.Discount_Case,
         A.Discount_TM,
         A.Restriction_Age,
         A.Restriction_Hours,
         A.Authorized,
         A.Discontinued,
         A.LocalItem,
         A.ScaleItem,
         A.OrderedByInfor,
         A.DefaultScanCode,
         A.LabelTypeDesc,
         A.Product_Code,
         A.RetailUnit,
         A.Sign_Desc,
         A.Locality,
         A.Sign_RomanceText_Long,
         A.Sign_RomanceText_Short,
         A.AltRetailUOM,
         A.AltRetailSize,
         A.MSRP, A.IrmaItemKey, ''' + Convert(VARCHAR(25), @dateStamp, 121) + ''' DateNow,
         Row_Number()over(partition by B.ItemID, A.BusinessUnitID order by A.Timestamp DESC) rowID
  INTO #item
  FROM stage.ItemLocale A
  INNER JOIN Items B ON B.ScanCode = A.ScanCode
  INNER JOIN Locales_' + @region + 
		' C ON C.BusinessUnitID = A.BusinessUnitID
  LEFT OUTER JOIN ItemAttributes_Locale_' + @region + ' D ON D.ItemID = B.ItemID AND D.BusinessUnitID = A.BusinessUnitID
  WHERE A.Region = ''' + @region + ''' AND A.TransactionId = ''' + @transactionId + ''';

  MERGE dbo.ItemAttributes_Locale_' + @region + 
		' WITH (updlock, rowlock) A
	  USING (SELECT * FROM #item WHERE rowID = 1) B  ON B.ItemID = A.ItemID and B.BusinessUnitID = A.BusinessUnitID
  
  WHEN MATCHED THEN
    UPDATE SET
       Discount_Case = B.Discount_Case
      ,Discount_TM = B.Discount_TM
      ,Restriction_Age = B.Restriction_Age
      ,Restriction_Hours = B.Restriction_Hours
      ,Authorized = B.Authorized
      ,Discontinued = B.Discontinued
      ,LocalItem = B.LocalItem
      ,ScaleItem = B.ScaleItem
      ,OrderedByInfor = B.OrderedByInfor
      ,DefaultScanCode = B.DefaultScanCode
      ,LabelTypeDesc = B.LabelTypeDesc
      ,Product_Code = B.Product_Code
      ,RetailUnit = B.RetailUnit
      ,Sign_Desc = B.Sign_Desc
      ,Locality = B.Locality
      ,Sign_RomanceText_Long = B.Sign_RomanceText_Long
      ,Sign_RomanceText_Short = B.Sign_RomanceText_Short
      ,AltRetailUOM = B.AltRetailUOM
      ,AltRetailSize = B.AltRetailSize
      ,MSRP = B.MSRP
      ,ModifiedDate = DateNow
      ,IrmaItemKey = B.IrmaItemKey
      
  WHEN NOT MATCHED THEN
    INSERT( Region
      ,ItemID
      ,BusinessUnitID
      ,Discount_Case
      ,Discount_TM
      ,Restriction_Age
      ,Restriction_Hours
      ,Authorized
      ,Discontinued
      ,LocalItem
      ,ScaleItem
      ,OrderedByInfor
      ,DefaultScanCode
      ,LabelTypeDesc
      ,Product_Code
      ,RetailUnit
      ,Sign_Desc
      ,Locality
      ,Sign_RomanceText_Long
      ,Sign_RomanceText_Short
      ,AltRetailUOM
      ,AltRetailSize
      ,MSRP
      ,AddedDate
      ,IrmaItemKey)
  VALUES(Region
      ,ItemID
      ,BusinessUnitID
      ,Discount_Case
      ,Discount_TM
      ,Restriction_Age
      ,Restriction_Hours
      ,Authorized
      ,Discontinued
      ,LocalItem
      ,ScaleItem
      ,OrderedByInfor
      ,DefaultScanCode
      ,LabelTypeDesc
      ,Product_Code
      ,RetailUnit
      ,Sign_Desc
      ,Locality
      ,Sign_RomanceText_Long
      ,Sign_RomanceText_Short
      ,AltRetailUOM
      ,AltRetailSize
      ,MSRP
      ,DateNow
      ,IrmaItemKey);      

  IF OBJECT_ID(''tempdb..#item'') IS NOT NULL DROP TABLE #item;'

	EXECUTE sp_executesql @sql;
END
GO