CREATE TABLE [dbo].[Customer] (
    [CustomerID] INT          IDENTITY (1, 1) NOT NULL,
    [FirstName]  VARCHAR (50) NOT NULL,
    [LastName]   VARCHAR (50) NOT NULL,
    [Phone]      VARCHAR (20) NOT NULL,
    [Address1]   VARCHAR (50) NULL,
    [Address2]   VARCHAR (50) NULL,
    [City]       VARCHAR (30) NULL,
    [State]      VARCHAR (2)  NULL,
    [ZipCode]    VARCHAR (10) NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([CustomerID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Customer] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Customer] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Customer] TO [IRMAReportsRole]
    AS [dbo];

