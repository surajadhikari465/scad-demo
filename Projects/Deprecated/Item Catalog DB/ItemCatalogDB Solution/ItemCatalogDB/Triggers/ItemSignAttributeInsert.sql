IF EXISTS (SELECT * FROM sysobjects WHERE TYPE = 'TR' AND NAME = 'ItemSignAttributeInsert')
	BEGIN
		DROP TRIGGER ItemSignAttributeInsert
	END
GO

CREATE TRIGGER [dbo].[ItemSignAttributeInsert]
	ON [dbo].[ItemSignAttribute] FOR INSERT
AS
BEGIN
	INSERT INTO PriceBatchDetail
	(
		Store_No,
		Item_Key,
		ItemChgTypeID,
		InsertApplication
	)
	SELECT 
		s.Store_No,
		INSERTED.Item_Key,
		2,
		'ItemSignAttributeInsert'
	FROM   
		INSERTED
		INNER JOIN Item (NOLOCK) i ON INSERTED.Item_Key = i.Item_Key
		CROSS JOIN (SELECT Store_No FROM Store (NOLOCK) WHERE WFM_Store = 1 OR Mega_Store = 1) s
	WHERE  
		(i.Remove_Item = 0 AND i.Deleted_Item = 0)

		AND (
				INSERTED.CheeseRaw = 1
				OR INSERTED.GlutenFree = 1
				OR INSERTED.UomRegulationChicagoBaby IS NOT NULL
				OR INSERTED.UomRegulationTagUom IS NOT NULL
			)

		AND (
				dbo.fn_HasPendingItemChangePriceBatchDetailRecord(INSERTED.Item_Key, s.Store_No) = 0
			)
END
GO
