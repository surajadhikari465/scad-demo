CREATE TABLE [dbo].[OrderHeaderExtended]
(
	[OrderHeader_ID] INT NOT NULL PRIMARY KEY, 
    [PastReceiptDate] DATE NULL, 
    [InsertDate] DATETIME2 NULL CONSTRAINT DF_OrderHeaderExtended_InsertDate DEFAULT (GetDate()),
    [LastUpdatedDate] DATETIME2 NULL, 
    CONSTRAINT [FK_OrderHeaderExtended_OrderHeader] FOREIGN KEY (OrderHeader_ID) REFERENCES [OrderHeader]([OrderHeader_ID])
)
