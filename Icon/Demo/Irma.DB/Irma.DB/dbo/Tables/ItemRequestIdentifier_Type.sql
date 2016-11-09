CREATE TABLE [dbo].[ItemRequestIdentifier_Type] (
    [ItemType_ID]   SMALLINT     IDENTITY (1, 1) NOT NULL,
    [ItemType_Name] VARCHAR (50) NULL,
    CONSTRAINT [PK_ItemRequestIdentifier_Type] PRIMARY KEY CLUSTERED ([ItemType_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemRequestIdentifier_Type] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemRequestIdentifier_Type] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemRequestIdentifier_Type] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemRequestIdentifier_Type] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemRequestIdentifier_Type] TO [IRMASLIMRole]
    AS [dbo];

