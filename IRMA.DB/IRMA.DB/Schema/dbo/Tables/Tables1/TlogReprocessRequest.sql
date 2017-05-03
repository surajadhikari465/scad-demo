CREATE TABLE [dbo].[TlogReprocessRequest] (
    [Date_Key]        SMALLDATETIME NOT NULL,
    [Store_No]        INT           NOT NULL,
    [BusinessUnit_ID] INT           NOT NULL,
    CONSTRAINT [PK_TlogReprocessRequest] PRIMARY KEY CLUSTERED ([Date_Key] ASC, [Store_No] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_TlogReprocessRequest_Date] FOREIGN KEY ([Date_Key]) REFERENCES [dbo].[Date] ([Date_Key]),
    CONSTRAINT [FK_TlogReprocessRequest_StoreNo] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[TlogReprocessRequest] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[TlogReprocessRequest] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TlogReprocessRequest] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[TlogReprocessRequest] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[TlogReprocessRequest] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[TlogReprocessRequest] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TlogReprocessRequest] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[TlogReprocessRequest] TO [IRSUser]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[TlogReprocessRequest] TO [IConInterface]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[TlogReprocessRequest] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TlogReprocessRequest] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[TlogReprocessRequest] TO [IConInterface]
    AS [dbo];

