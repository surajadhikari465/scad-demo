CREATE TYPE [dbo].[PickListType] AS TABLE(
    [PickListId] [int] NOT NULL,
    [AttributeId] [int] NULL,
    [PickListValue] [nvarchar](50) NULL
)