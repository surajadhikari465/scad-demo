CREATE TABLE [dbo].[Item] (
	itemID INT IDENTITY     constraint Item_PK primary key clustered,
	itemTypeID INT NOT NULL constraint ItemType_Item_FK1 foreign key references dbo.ItemType(itemTypeID),
	productKey INT NOT NULL constraint DF_Item_productKey DEFAULT app.fn_GetNextProductKey(),
  HospitalityItem bit not null default(0),
  KitchenItem bit not null default(0),
  KitchenDescription varchar(15),
  ImageURL varchar(255),
)
GO

CREATE NONCLUSTERED INDEX IX_Item_productKey on [dbo].[Item] (productKey)
GO