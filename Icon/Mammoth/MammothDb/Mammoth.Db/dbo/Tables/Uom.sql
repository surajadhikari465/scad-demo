CREATE TABLE [dbo].[Uom](
	[UomId] [int] PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[UomCode] [nvarchar](4) NOT NULL,
	[UomName] [nvarchar](100) NOT NULL
)