CREATE TABLE [app].[IRMAPush] (
    [IRMAPushID] [int] IDENTITY(1,1) NOT NULL,
	[RegionCode] [varchar](4) NOT NULL,
	[BusinessUnit_ID] [int] NOT NULL,
	[Identifier] [varchar](13) NOT NULL,
	[ChangeType] [varchar](30) NOT NULL,
	[InsertDate] [datetime2](7) NOT NULL CONSTRAINT [DF_IRMAPush_InsertDate]  DEFAULT (sysdatetime()),
	[RetailSize] [decimal](9,4) NOT NULL,
	[RetailPackageUom] [varchar](5) NOT NULL,
	[TMDiscountEligible] [bit] NOT NULL,
	[Case_Discount] [bit] NOT NULL,
	[AgeCode] [int] NULL,
	[Recall_Flag] [bit] NOT NULL,
	[Restricted_Hours] [bit] NOT NULL,
	[Sold_By_Weight] [bit] NOT NULL,
	[ScaleForcedTare] [bit] NOT NULL,
	[Quantity_Required] [bit] NOT NULL,
	[Price_Required] [bit] NOT NULL,
	[QtyProhibit] [bit] NOT NULL,
	[VisualVerify] [bit] NOT NULL,
	[RestrictSale] [bit] NOT NULL,
	[PosScaleTare] [int] NULL,
	[LinkedIdentifier] [varchar](13) NULL,
	[Price] [money] NULL,
	[RetailUom] [varchar](5) NOT NULL,
	[Multiple] [int] NULL,
	[SaleMultiple] [int] NULL,
	[Sale_Price] [money] NULL,
	[Sale_Start_Date] [smalldatetime] NULL,
	[Sale_End_Date] [smalldatetime] NULL,
	[InProcessBy] int NULL,
	[InUdmDate] [datetime2](7) NULL,
	[EsbReadyDate] [datetime2](7) NULL,
	[UdmFailedDate] [datetime2](7) NULL,
	[EsbReadyFailedDate] [datetime2](7) NULL,
    CONSTRAINT [PK_IrmaPush_IrmaPushID] PRIMARY KEY CLUSTERED ([IRMAPushID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [UQ_IrmaPush_Keys] UNIQUE NONCLUSTERED ([BusinessUnit_ID] ASC, [Identifier] ASC, [ChangeType] ASC, [InsertDate] ASC) WITH (FILLFACTOR = 80)
);
GO

CREATE NONCLUSTERED INDEX [IX_ConcurrencyColumnsForEsb] ON [app].[IRMAPush] ([InProcessBy], [EsbReadyDate], [EsbReadyFailedDate])
INCLUDE ([IRMAPushID])
GO

CREATE NONCLUSTERED INDEX [IX_ConcurrencyColumnsForUdm] ON [app].[IRMAPush] ([InProcessBy],	[InUdmDate], [UdmFailedDate], [EsbReadyDate])
INCLUDE ([IRMAPushID])
GO
