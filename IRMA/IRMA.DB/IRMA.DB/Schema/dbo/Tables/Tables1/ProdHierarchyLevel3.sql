CREATE TABLE [dbo].[ProdHierarchyLevel3] (
    [ProdHierarchyLevel3_ID] INT          IDENTITY (1, 1) NOT NULL,
    [Category_ID]            INT          NOT NULL,
    [Description]            VARCHAR (50) NOT NULL,
    [LastUpdateTimestamp]    DATETIME     NULL,
    CONSTRAINT [PK_ProdHierarchyLevel3] PRIMARY KEY CLUSTERED ([ProdHierarchyLevel3_ID] ASC),
    CONSTRAINT [FK_ProdHierarchyLevel3_ItemCategory] FOREIGN KEY ([Category_ID]) REFERENCES [dbo].[ItemCategory] ([Category_ID])
);


GO
ALTER TABLE [dbo].[ProdHierarchyLevel3] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE TRIGGER [dbo].[ProdHierarchyLevel3AddUpdate] ON [dbo].[ProdHierarchyLevel3] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update ProdHierarchyLevel3
		Set LastUpdateTimestamp = GetDate()
	from Inserted i
	where ProdHierarchyLevel3.ProdHierarchyLevel3_Id = i.ProdHierarchyLevel3_Id

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ProdHierarchyLevel3AddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ProdHierarchyLevel3] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ProdHierarchyLevel3] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ProdHierarchyLevel3] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ProdHierarchyLevel3] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ProdHierarchyLevel3] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ProdHierarchyLevel3] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ProdHierarchyLevel3] TO [IRMA_Teradata]
    AS [dbo];





GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ProdHierarchyLevel3] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ProdHierarchyLevel3] TO [iCONReportingRole]
    AS [dbo];

