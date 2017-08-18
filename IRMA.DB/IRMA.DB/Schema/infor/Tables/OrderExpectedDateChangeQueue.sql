CREATE TABLE [infor].[OrderExpectedDateChangeQueue]
(
	[OrderHeader_ID] INT NOT NULL , 
    [InsertDate] DATETIME NOT NULL, 
    PRIMARY KEY ([InsertDate], [OrderHeader_ID]), 
    CONSTRAINT [FK_OrderExpectedDateChangeQueue_OrderHeader] FOREIGN KEY ([OrderHeader_ID]) REFERENCES [dbo].[OrderHeader]([OrderHeader_ID]) 
)
