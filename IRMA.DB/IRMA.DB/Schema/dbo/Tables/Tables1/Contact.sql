CREATE TABLE [dbo].[Contact] (
    [Contact_ID]   INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Vendor_ID]    INT          NOT NULL,
    [Contact_Name] VARCHAR (50) NOT NULL,
    [Phone]        VARCHAR (20) NULL,
    [Fax]          VARCHAR (20) NULL,
    [Phone_Ext]    VARCHAR (5)  NULL,
    [Email]        VARCHAR (50) NULL,
    CONSTRAINT [PK_Contact_Contact_ID] PRIMARY KEY CLUSTERED ([Contact_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Contact_1__16] FOREIGN KEY ([Vendor_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID])
);


GO
CREATE NONCLUSTERED INDEX [idxContactName]
    ON [dbo].[Contact]([Contact_Name] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxContactVendorID]
    ON [dbo].[Contact]([Vendor_ID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Contact] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Contact] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Contact] TO [IRMAReportsRole]
    AS [dbo];

