CREATE PROCEDURE dbo.AuditItemLocale
  @region varchar(2)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @msg varchar(Max) = null,
          @cols NVARCHAR(Max) = null,
          @query NVARCHAR(Max) = null;

  IF(NOT Exists(SELECT 1 FROM Regions WHERE Region = @region))
  BEGIN
    SET @msg = 'Invalid region specified ' + @region + '.';
    RAISERROR (@msg, 16, 1);
    RETURN;
  END

  DECLARE @attributes table(AttributeDesc nvarchar(255));
  INSERT INTO @attributes(AttributeDesc) VALUES
    ('CFS Send to Scale'),
    ('Chicago Baby'),
    ('Color Added'),
    ('Country of Processing'),
    ('Electronic Shelf Tag'),
    ('Exclusive'),
    ('Force Tare'),
    ('Global Pricing Program'),
    ('Linked Scan Code'),
    ('Number of Digits Sent To Scale'),
    ('Ordered By Infor'),
    ('Origin'),
    ('Scale Extra Text'),
    ('Shelf Life'),
    ('Tag UOM'),
    ('Use By EAB'),
    ('Unwrapped Tare Weight'),
    ('Wrapped Tare Weight');
 
  SELECT @cols = COALESCE(@cols + ',', '') + QUOTENAME(AttributeDesc) 
  FROM (SELECT DISTINCT A.AttributeDesc FROM Attributes A 
        JOIN @attributes B ON B.AttributeDesc = A.AttributeDesc) A;

  SET @query = '
  SELECT Region,
         ItemID,
         BusinessUnitID,
         Discount_Case,
         Discount_TM,
         Restriction_Age,
         Restriction_Hours,
         Authorized,
         Discontinued,
         LocalItem,
         ScaleItem,
         OrderedByInfor,
         DefaultScanCode,
         LabelTypeDesc,
         Product_Code,
         RetailUnit,
         Sign_Desc,
         Locality,
         Sign_RomanceText_Long,
         Sign_RomanceText_Short,
         AltRetailUOM,
         AltRetailSize,
         MSRP,
         IrmaItemKey, ' + @cols + ' 
  FROM (SELECT A.Region,
               A.ItemID,
               BusinessUnitID,
               Discount_Case,
               Discount_TM,
               Restriction_Age,
               Restriction_Hours,
               Authorized,
               Discontinued,
               LocalItem,
               ScaleItem,
               OrderedByInfor,
               DefaultScanCode,
               LabelTypeDesc,
               Product_Code,
               RetailUnit,
               Sign_Desc,
               Locality,
               Sign_RomanceText_Long,
               Sign_RomanceText_Short,
               AltRetailUOM,
               AltRetailSize,
               MSRP,
               IrmaItemKey,
               B.AttributeValue,
               C.AttributeDesc
        FROM ItemAttributes_Locale_' + @region + ' A
        JOIN ItemAttributes_Locale_' + @region + '_Ext B on B.ItemID = A.ItemID
        JOIN Attributes C on C.AttributeID = B.AttributeID) A

        PIVOT
          (Max(AttributeValue) for AttributeDesc IN(' + @cols + ')) B';

  EXEC sp_executesql @query;

  SET NOCOUNT OFF;
END
GO