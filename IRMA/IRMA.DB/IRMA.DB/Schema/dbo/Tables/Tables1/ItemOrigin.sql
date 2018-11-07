CREATE TABLE [dbo].[ItemOrigin] (
    [Origin_ID]   INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Origin_Name] VARCHAR (25) NOT NULL,
    [User_ID]     INT          NULL,
    CONSTRAINT [PK_ItemOrigin_Origin_ID] PRIMARY KEY CLUSTERED ([Origin_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ItemOrigin_1__13] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
ALTER TABLE [dbo].[ItemOrigin] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idxItemOriginName]
    ON [dbo].[ItemOrigin]([Origin_Name] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemOriginUserID]
    ON [dbo].[ItemOrigin]([User_ID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOrigin] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOrigin] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemOrigin] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemOrigin] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOrigin] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemOrigin] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOrigin] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOrigin] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemOrigin] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOrigin] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOrigin] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOrigin] TO [iCONReportingRole]
    AS [dbo];

