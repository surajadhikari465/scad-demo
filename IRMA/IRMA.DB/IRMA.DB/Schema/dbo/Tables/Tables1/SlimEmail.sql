CREATE TABLE [dbo].[SlimEmail] (
    [EmailID]          INT           IDENTITY (1, 1) NOT NULL,
    [Store_No]         INT           NOT NULL,
    [Team_No]          INT           NOT NULL,
    [TeamLeader_email] VARCHAR (250) NULL,
    [BA_email]         VARCHAR (250) NULL,
    [Other_email]      VARCHAR (250) NULL,
    [Insert_Date]      SMALLDATETIME NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[SlimEmail] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[SlimEmail] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[SlimEmail] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SlimEmail] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[SlimEmail] TO [IRMASLIMRole]
    AS [dbo];

