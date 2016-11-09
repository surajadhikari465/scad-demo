CREATE TABLE [dbo].[ExternalOrderInformation] (
    [OrderHeader_Id]    INT NOT NULL,
    [ExternalSource_Id] INT NOT NULL,
    [ExternalOrder_Id]  INT NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_ExternalOrderInformation_ExternalSource]
    ON [dbo].[ExternalOrderInformation]([ExternalSource_Id] ASC)
    INCLUDE([OrderHeader_Id], [ExternalOrder_Id]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_ExternalOrderInformation_OrderHeaderId]
    ON [dbo].[ExternalOrderInformation]([OrderHeader_Id] ASC)
    INCLUDE([ExternalSource_Id], [ExternalOrder_Id]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_ExternalORderInformation_ExSourceExOrder]
    ON [dbo].[ExternalOrderInformation]([ExternalSource_Id] ASC, [ExternalOrder_Id] ASC) WITH (FILLFACTOR = 80);


GO
GRANT INSERT
    ON OBJECT::[dbo].[ExternalOrderInformation] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExternalOrderInformation] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ExternalOrderInformation] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExternalOrderInformation] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ExternalOrderInformation] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExternalOrderInformation] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ExternalOrderInformation] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExternalOrderInformation] TO [IRMAReportsRole]
    AS [dbo];

