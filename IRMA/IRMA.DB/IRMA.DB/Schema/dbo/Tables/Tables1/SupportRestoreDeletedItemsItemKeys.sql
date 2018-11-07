CREATE TABLE dbo.SupportRestoreDeletedItemsItemKeys
(
	Item_Key INT,
    CONSTRAINT [PK_SupportRestoreDeletedItemsItemKeys_Item_Key] PRIMARY KEY CLUSTERED ([Item_Key] ASC) WITH (FILLFACTOR = 80),
)

GO
GRANT SELECT, INSERT, DELETE
    ON OBJECT::[dbo].[SupportRestoreDeletedItemsItemKeys] TO [IRMAClientRole]
    AS [dbo];

GO
GRANT SELECT, INSERT, DELETE
    ON OBJECT::[dbo].[SupportRestoreDeletedItemsItemKeys] TO [IRMAAdminRole]
    AS [dbo];

GO
GRANT SELECT, INSERT, DELETE
    ON OBJECT::[dbo].[SupportRestoreDeletedItemsItemKeys] TO [IRMASupportRole]
    AS [dbo];

GO
GRANT SELECT, INSERT, DELETE
    ON OBJECT::[dbo].[SupportRestoreDeletedItemsItemKeys] TO [IConInterface]
    AS [dbo];

GO
GRANT SELECT, INSERT, DELETE
    ON OBJECT::[dbo].[SupportRestoreDeletedItemsItemKeys] TO [TibcoDataWriter]
    AS [dbo];

GO
GRANT SELECT, INSERT, DELETE
    ON OBJECT::[dbo].[SupportRestoreDeletedItemsItemKeys] TO [IRMASchedJobsRole]
    AS [dbo];

GO
GRANT SELECT, INSERT, DELETE
    ON OBJECT::[dbo].[SupportRestoreDeletedItemsItemKeys] TO [IRMAExcelRole]
    AS [dbo];

GO
GRANT SELECT, INSERT, DELETE
    ON OBJECT::[dbo].[SupportRestoreDeletedItemsItemKeys] TO [IRMAAVCIRole]
    AS [dbo];

GO
GRANT SELECT, INSERT, DELETE
    ON OBJECT::[dbo].[SupportRestoreDeletedItemsItemKeys] TO [IMHARole]
    AS [dbo];

GO
GRANT SELECT, INSERT, DELETE
    ON OBJECT::[dbo].[SupportRestoreDeletedItemsItemKeys] TO [IRMASLIMRole]
    AS [dbo];