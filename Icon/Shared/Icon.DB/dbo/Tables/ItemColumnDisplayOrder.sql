create table [dbo].[ItemColumnDisplayOrder]
(
	ColumnType nvarchar(15) not null,  
	ReferenceId int not null,
	DisplayOrder int not null
)


GO

CREATE  INDEX [IX_ItemColumnDisplayOrder_ColumnAndId] ON [dbo].[ItemColumnDisplayOrder] (ColumnType, ReferenceId)
