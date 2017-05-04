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

