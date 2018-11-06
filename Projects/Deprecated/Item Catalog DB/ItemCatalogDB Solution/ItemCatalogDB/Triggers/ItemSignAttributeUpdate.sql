IF EXISTS (SELECT * FROM sysobjects WHERE TYPE = 'TR' AND NAME = 'ItemSignAttributeUpdate')
	BEGIN
		DROP TRIGGER ItemSignAttributeUpdate
	END
GO

CREATE TRIGGER [dbo].[ItemSignAttributeUpdate]
	ON [dbo].[ItemSignAttribute] FOR UPDATE
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
		'ItemSignAttributeUpdate'
	FROM   
		INSERTED
		INNER JOIN DELETED ON DELETED.Item_Key = INSERTED.Item_Key
		INNER JOIN Item (NOLOCK) i ON INSERTED.Item_Key = i.Item_Key
		CROSS JOIN (SELECT Store_No FROM Store (NOLOCK) WHERE WFM_Store = 1 OR Mega_Store = 1) s
	WHERE  
		(i.Remove_Item = 0 AND i.Deleted_Item = 0)

		AND (
				((INSERTED.CheeseRaw = 1 AND DELETED.CheeseRaw = 0) OR (INSERTED.CheeseRaw = 0 AND DELETED.CheeseRaw = 1) OR (INSERTED.CheeseRaw = 1 AND DELETED.CheeseRaw IS NULL) OR (INSERTED.GlutenFree IS NULL AND DELETED.GlutenFree = 1))
				OR ((INSERTED.GlutenFree = 1 AND DELETED.GlutenFree = 0) OR (INSERTED.GlutenFree = 0 AND DELETED.GlutenFree = 1) OR (INSERTED.GlutenFree = 1 AND DELETED.GlutenFree IS NULL) OR (INSERTED.GlutenFree IS NULL AND DELETED.GlutenFree = 1))
				OR INSERTED.UomRegulationChicagoBaby <> DELETED.UomRegulationChicagoBaby
				OR INSERTED.UomRegulationTagUom <> DELETED.UomRegulationTagUom
			)

		AND (
				dbo.fn_HasPendingItemChangePriceBatchDetailRecord(INSERTED.Item_Key, s.Store_No) = 0
			)
END
GO
