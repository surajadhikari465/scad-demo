CREATE TABLE [infor].[HistoricalAttributes]
(
	AttributeHistoricalId int IDENTITY(1,1) CONSTRAINT PK_Attribute PRIMARY KEY,
	AttributeGuid uniqueidentifier,
	AttributeName nvarchar(255), 
    [AttributeType] NVARCHAR(255) NULL
)
