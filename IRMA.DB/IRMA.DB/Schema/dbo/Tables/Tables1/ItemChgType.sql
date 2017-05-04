CREATE TABLE [dbo].[ItemChgType] (
    [ItemChgTypeID]   TINYINT      NOT NULL,
    [ItemChgTypeDesc] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_ItemChgType] PRIMARY KEY CLUSTERED ([ItemChgTypeID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemChgType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemChgType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemChgType] TO [IRMAReportsRole]
    AS [dbo];

