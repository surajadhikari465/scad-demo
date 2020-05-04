CREATE TYPE [dbo].[ItemColumnDisplayOrderType] AS TABLE(
	[ColumnType] nvarchar(15) NOT NULL,
	[ReferenceId] int not null, 
	OrderId int
)