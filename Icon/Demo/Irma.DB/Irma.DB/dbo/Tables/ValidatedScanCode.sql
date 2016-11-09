CREATE TABLE [dbo].[ValidatedScanCode] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [ScanCode]   VARCHAR (13) NOT NULL,
    [InsertDate] DATETIME     NOT NULL,
    CONSTRAINT [PK_ValidatedScanCode] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [ScanCodeExistsConstraint] CHECK ([dbo].[fn_DoesScanCodeExist]([ScanCode])<>(0)),
    CONSTRAINT [UniqueScanCodeConstraint] UNIQUE NONCLUSTERED ([ScanCode] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT ALTER
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT ALTER
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRSUser]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRSUser]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IConInterface]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IConInterface]
    AS [dbo];

