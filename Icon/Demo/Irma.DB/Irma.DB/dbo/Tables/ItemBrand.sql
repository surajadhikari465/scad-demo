CREATE TABLE [dbo].[ItemBrand] (
    [Brand_ID]            INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Brand_Name]          VARCHAR (25) NOT NULL,
    [User_ID]             INT          NULL,
    [LastUpdateTimestamp] DATETIME     NULL,
    CONSTRAINT [PK_ItemBrand_Brand_ID] PRIMARY KEY CLUSTERED ([Brand_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ItemBrand_1__13] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID])
);





GO
ALTER TABLE [dbo].[ItemBrand] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idxItemBrandName]
    ON [dbo].[ItemBrand]([Brand_Name] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemBrandUserID]
    ON [dbo].[ItemBrand]([User_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE TRIGGER [dbo].[ItemBrandAddUpdate] ON [dbo].[ItemBrand] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update ItemBrand 
		Set LastUpdateTimestamp = GetDate()
	from Inserted i
	where ItemBrand.Brand_Id = i.Brand_Id

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemBrandAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemBrand] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemBrand] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemBrand] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemBrand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemBrand] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemBrand] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemBrand] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemBrand] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemBrand] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemBrand] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemBrand] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemBrand] TO [IRSUser]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemBrand] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemBrand] TO [IMHARole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemBrand] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemBrand] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemBrand] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemBrand] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemBrand] TO [ExtractRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemBrand] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemBrand] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemBrand] TO [IConInterface]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemBrand] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemBrand] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemBrand] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemBrand] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemBrand] TO [iCONReportingRole]
    AS [dbo];

