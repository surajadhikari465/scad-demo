create table [dbo].[ItemColumnDisplayOrder]
(
	ColumnType nvarchar(15) not null,  
	ReferenceId int not null,
	ReferenceName nvarchar(150) null,
	DisplayOrder int not null
)


GO

CREATE  INDEX [IX_ItemColumnDisplayOrder_ColumnAndId] ON [dbo].[ItemColumnDisplayOrder] (ColumnType, ReferenceId)
