CREATE TABLE [dbo].[ProdHierarchyLevel4] (
    [ProdHierarchyLevel4_ID] INT          IDENTITY (1, 1) NOT NULL,
    [ProdHierarchyLevel3_ID] INT          NOT NULL,
    [Description]            VARCHAR (50) NOT NULL,
    [LastUpdateTimestamp]    DATETIME     NULL,
    CONSTRAINT [PK_ProdHierarchyLevel4] PRIMARY KEY CLUSTERED ([ProdHierarchyLevel4_ID] ASC),
    CONSTRAINT [FK_ProdHierarchyLevel4_ProdHierarchyLevel3] FOREIGN KEY ([ProdHierarchyLevel3_ID]) REFERENCES [dbo].[ProdHierarchyLevel3] ([ProdHierarchyLevel3_ID])
);


GO
ALTER TABLE [dbo].[ProdHierarchyLevel4] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE TRIGGER [dbo].[ProdHierarchyLevel4AddUpdate] ON [dbo].[ProdHierarchyLevel4] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update ProdHierarchyLevel4
		Set LastUpdateTimestamp = GetDate()
	from Inserted i
	where ProdHierarchyLevel4.ProdHierarchyLevel4_id = i.ProdHierarchyLevel4_Id

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ProdHierarchyLevel4AddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ProdHierarchyLevel4] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ProdHierarchyLevel4] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ProdHierarchyLevel4] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ProdHierarchyLevel4] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ProdHierarchyLevel4] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ProdHierarchyLevel4] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ProdHierarchyLevel4] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ProdHierarchyLevel4] TO [BizTalk]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ProdHierarchyLevel4] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ProdHierarchyLevel4] TO [iCONReportingRole]
    AS [dbo];

