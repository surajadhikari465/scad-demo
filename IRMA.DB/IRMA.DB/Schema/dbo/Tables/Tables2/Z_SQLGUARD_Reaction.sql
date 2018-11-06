CREATE TABLE [dbo].[Z_SQLGUARD_Reaction] (
    [ID]     INT          NOT NULL,
    [Field1] VARCHAR (50) NULL,
    [Field2] VARCHAR (50) NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Z_SQLGUARD_Reaction] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Z_SQLGUARD_Reaction] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Z_SQLGUARD_Reaction] TO [IRMAReportsRole]
    AS [dbo];

