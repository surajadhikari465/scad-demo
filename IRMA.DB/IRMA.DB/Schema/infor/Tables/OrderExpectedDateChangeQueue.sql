CREATE TABLE [infor].[OrderExpectedDateChangeQueue]
(
	[OrderHeader_ID] INT NOT NULL , 
    [InsertDate] DATETIME NOT NULL, 
    PRIMARY KEY ([InsertDate], [OrderHeader_ID])
)

GO

GRANT SELECT
    ON OBJECT::[infor].[OrderExpectedDateChangeQueue] TO [IRMAPDXExtractRole]
    AS [dbo];

GO
GRANT SELECT
    ON OBJECT::[infor].[OrderExpectedDateChangeQueue] TO [IRMAClientRole]
    AS [dbo];

GO
GRANT INSERT
    ON OBJECT::[infor].[OrderExpectedDateChangeQueue] TO [IRMAClientRole]
    AS [dbo];

GO