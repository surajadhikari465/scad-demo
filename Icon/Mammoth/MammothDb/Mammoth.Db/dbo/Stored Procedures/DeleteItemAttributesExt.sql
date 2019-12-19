CREATE PROCEDURE dbo.DeleteItemAttributesExt
  @extAttributesItemIds dbo.IntListType READONLY
AS
BEGIN

  SELECT DISTINCT *
  INTO #extAttributesItemIds
  FROM @extAttributesItemIds;

  DELETE ext
  FROM dbo.ItemAttributes_Ext ext
  JOIN #extAttributesItemIds id ON id.Value = ext.ItemID;
END
GO

GRANT EXECUTE ON dbo.DeleteItemAttributesExt TO [MammothRole];
GO