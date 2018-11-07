CREATE TABLE [dbo].[ItemRequest_Status] (
    [ItemStatus_ID]    SMALLINT     IDENTITY (1, 1) NOT NULL,
    [ItemStatus_Level] VARCHAR (50) NULL,
    CONSTRAINT [PK_ItemRequest_Status] PRIMARY KEY CLUSTERED ([ItemStatus_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemRequest_Status] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemRequest_Status] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemRequest_Status] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemRequest_Status] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemRequest_Status] TO [IRMASLIMRole]
    AS [dbo];

