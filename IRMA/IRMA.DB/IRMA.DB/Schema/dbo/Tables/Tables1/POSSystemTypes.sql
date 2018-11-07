CREATE TABLE [dbo].[POSSystemTypes] (
    [POSSystemId]   INT          IDENTITY (1, 1) NOT NULL,
    [POSSystemType] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_POSSystemTypes_POSSystemId] PRIMARY KEY CLUSTERED ([POSSystemId] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSSystemTypes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSSystemTypes] TO [IRMAReportsRole]
    AS [dbo];

