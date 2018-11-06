CREATE TABLE [dbo].[ItemSignAttribute] (
    [ItemSignAttributeID]      INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Item_Key]                 INT            NOT NULL,
    [Locality]                 NVARCHAR (50)  NULL,
    [SignRomanceTextLong]      NVARCHAR (300) NULL,
    [SignRomanceTextShort]     NVARCHAR (140) NULL,
    [AnimalWelfareRating]      NVARCHAR (10)  NULL,
    [Biodynamic]               BIT            NULL,
    [CheeseMilkType]           NVARCHAR (40)  NULL,
    [CheeseRaw]                BIT            NULL,
    [EcoScaleRating]           NVARCHAR (30)  NULL,
    [GlutenFree]               BIT            NULL,
    [HealthyEatingRating]      NVARCHAR (10)  NULL,
    [Kosher]                   BIT            NULL,
    [NonGmo]                   BIT            NULL,
    [Organic]                  BIT            NULL,
    [PremiumBodyCare]          BIT            NULL,
    [ProductionClaims]         NVARCHAR (30)  NULL,
    [FreshOrFrozen]            NVARCHAR (30)  NULL,
    [SeafoodCatchType]         NVARCHAR (15)  NULL,
    [Vegan]                    BIT            NULL,
    [Vegetarian]               BIT            NULL,
    [WholeTrade]               BIT            NULL,
    [UomRegulationChicagoBaby] NVARCHAR (50)  NULL,
    [UomRegulationTagUom]      INT            NULL,
    [Msc]                      BIT            NULL,
    [GrassFed]                 BIT            NULL,
    [PastureRaised]            BIT            NULL,
    [FreeRange]                BIT            NULL,
    [DryAged]                  BIT            NULL,
    [AirChilled]               BIT            NULL,
    [MadeInHouse]              BIT            NULL,
    [Exclusive]                DATETIME       NULL,
    [ColorAdded]               BIT            NULL,
    CONSTRAINT [PK_ItemSignAttribute_ItemSignAttributeID] PRIMARY KEY CLUSTERED ([ItemSignAttributeID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ItemSignAttribute_Item_Item_Key] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key])
);


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
		si.Store_No,
		inserted.Item_Key,
		2,
		'ItemSignAttributeUpdate'
	FROM   
		inserted
		INNER JOIN deleted ON deleted.Item_Key = inserted.Item_Key
		INNER JOIN Item (NOLOCK) i ON inserted.Item_Key = i.Item_Key
		INNER JOIN StoreItem (NOLOCK) si ON i.Item_Key = si.Item_Key
		LEFT JOIN fn_GetInstanceDataFlagStoreValues ('BatchTagUomChanges') tag on si.Store_No = tag.Store_No
		LEFT JOIN fn_GetInstanceDataFlagStoreValues ('BatchChicagoBabyChanges') chi on si.Store_No = chi.Store_No
	WHERE  
		(i.Remove_Item = 0 AND i.deleted_Item = 0)
		AND (si.Authorized = 1)
		AND (
				((inserted.GlutenFree = 1 AND deleted.GlutenFree = 0)
					OR (inserted.GlutenFree = 0 AND deleted.GlutenFree = 1)
					OR (inserted.GlutenFree = 1 AND deleted.GlutenFree IS NULL)
					OR (inserted.GlutenFree IS NULL AND deleted.GlutenFree = 1))
				OR (((inserted.UomRegulationChicagoBaby <> deleted.UomRegulationChicagoBaby)
					OR (inserted.UomRegulationChicagoBaby IS NOT NULL AND deleted.UomRegulationChicagoBaby IS NULL)
					OR (inserted.UomRegulationChicagoBaby IS NULL AND deleted.UomRegulationChicagoBaby IS NOT NULL))
					AND chi.FlagValue = 1)
				OR (((inserted.UomRegulationTagUom <> deleted.UomRegulationTagUom)
					OR (inserted.UomRegulationTagUom IS NOT NULL AND deleted.UomRegulationTagUom IS NULL)
					OR (inserted.UomRegulationTagUom IS NULL AND deleted.UomRegulationTagUom IS NOT NULL))
					AND tag.FlagValue = 1)
			)

		AND (
				dbo.fn_HasPendingItemChangePriceBatchDetailRecord(inserted.Item_Key, si.Store_No) = 0
			)
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
		si.Store_No,
		inserted.Item_Key,
		2,
		'ItemSignAttributeInsert'
	FROM   
		inserted
		INNER JOIN Item (NOLOCK) i ON inserted.Item_Key = i.Item_Key
		INNER JOIN StoreItem (NOLOCK) si ON i.Item_Key = si.Item_Key
		LEFT JOIN fn_GetInstanceDataFlagStoreValues ('BatchTagUomChanges') tag on si.Store_No = tag.Store_No
		LEFT JOIN fn_GetInstanceDataFlagStoreValues ('BatchChicagoBabyChanges') chi on si.Store_No = chi.Store_No
	WHERE  
		(i.Remove_Item = 0 AND i.deleted_Item = 0)
		AND (si.Authorized = 1)
		AND (
				inserted.GlutenFree = 1
				OR (inserted.UomRegulationChicagoBaby IS NOT NULL AND chi.FlagValue = 1)
				OR (inserted.UomRegulationTagUom IS NOT NULL AND tag.FlagValue = 1)
			)

		AND (
				dbo.fn_HasPendingItemChangePriceBatchDetailRecord(inserted.Item_Key, si.Store_No) = 0
			)
END

GO
GRANT ALTER
    ON OBJECT::[dbo].[ItemSignAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemSignAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemSignAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemSignAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemSignAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT ALTER
    ON OBJECT::[dbo].[ItemSignAttribute] TO [IRSUser]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemSignAttribute] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemSignAttribute] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemSignAttribute] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemSignAttribute] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemSignAttribute] TO [iCONReportingRole]
    AS [dbo];

