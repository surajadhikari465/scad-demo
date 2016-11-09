BEGIN TRY
BEGIN TRAN
	SELECT
		ii.regionCode,
		ii.identifier,
		ii.defaultIdentifier,
		ii.brandName,
		ii.itemDescription,
		ii.posDescription,
		ii.packageUnit,
		ii.foodStamp,
		ii.posScaleTare,
		ii.departmentSale,
		ii.giftCard,
		ii.taxClassID,
		ii.merchandiseClassID,
		ii.insertDate
	INTO #irmaItemBackup
	FROM app.IRMAItem ii

	ALTER TABLE [app].[IRMAItem] DROP CONSTRAINT [DF_defaultIdentifier]
	DROP TABLE [app].[IRMAItem]

	CREATE TABLE [app].[IRMAItem](
	[irmaItemID] [int] IDENTITY(1,1) NOT NULL,
	[regioncode] [varchar](2) NOT NULL,
	[identifier] [varchar](13) NOT NULL,
	[defaultIdentifier] [bit] NOT NULL CONSTRAINT [DF_defaultIdentifier]  DEFAULT ((1)),
	[brandName] [varchar](25) NOT NULL,
	[itemDescription] [varchar](60) NOT NULL,
	[posDescription] [varchar](60) NOT NULL,
	[packageUnit] [int] NOT NULL,
	[retailSize] [decimal](9, 4) NULL,
	[retailUom] [varchar](100) NULL,
	[foodStamp] [bit] NOT NULL,
	[posScaleTare] [decimal](18, 0) NOT NULL,
	[departmentSale] [bit] NOT NULL,
	[giftCard] [bit] NULL,
	[taxClassID] [int] NULL,
	[merchandiseClassID] [int] NULL,
	[insertDate] [datetime2](7) NOT NULL,	
	CONSTRAINT [PK_IRMAItem] PRIMARY KEY CLUSTERED 
	(
		[irmaItemID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
	) ON [PRIMARY]

	INSERT INTO app.IRMAItem
	(
		[regioncode],
		[identifier],
		[defaultIdentifier],
		[brandName],
		[itemDescription],
		[posDescription],
		[packageUnit],
		[retailSize],
		[retailUom],
		[foodStamp],
		[posScaleTare],
		[departmentSale],
		[giftCard],
		[taxClassID],
		[merchandiseClassID],
		[insertDate]
	)
	SELECT
		ii.regionCode,
		ii.identifier,
		ii.defaultIdentifier,
		ii.brandName,
		ii.itemDescription,
		ii.posDescription,
		ii.packageUnit,
		NULL	as retailSize,
		NULL	as retailUom,
		ii.foodStamp,
		ii.posScaleTare,
		ii.departmentSale,
		ii.giftCard,
		ii.taxClassID,
		ii.merchandiseClassID,
		ii.insertDate
	FROM #irmaItemBackup ii

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
	THROW;
END CATCH