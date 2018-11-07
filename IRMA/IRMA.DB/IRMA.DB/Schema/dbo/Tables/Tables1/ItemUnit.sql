CREATE TABLE [dbo].[ItemUnit] (
    [Unit_ID]             INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Unit_Name]           VARCHAR (25) NOT NULL,
    [Weight_Unit]         BIT          CONSTRAINT [DF__ItemUnit__Weight__430CD787] DEFAULT ((0)) NOT NULL,
    [User_ID]             INT          NULL,
    [Unit_Abbreviation]   VARCHAR (5)  NULL,
    [UnitSysCode]         VARCHAR (5)  NULL,
    [IsPackageUnit]       BIT          CONSTRAINT [DF_ItemUnit_IsPackageUnit] DEFAULT ((0)) NOT NULL,
    [PlumUnitAbbr]        VARCHAR (5)  NULL,
    [EDISysCode]          CHAR (2)     NULL,
    [LastUpdateTimestamp] DATETIME     NULL,
    CONSTRAINT [PK_ItemUnit_Unit_ID] PRIMARY KEY CLUSTERED ([Unit_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [Chk_Constraint_ItemUnit_PlumUnitAbbr] CHECK ([PlumUnitAbbr]='KG' OR [PlumUnitAbbr]='HG' OR [PlumUnitAbbr]='BC' OR [PlumUnitAbbr]='LB' OR [PlumUnitAbbr]='HB' OR [PlumUnitAbbr]='QB' OR [PlumUnitAbbr]='FW' OR [PlumUnitAbbr]='FP' OR [PlumUnitAbbr]='OK' OR [PlumUnitAbbr]='OG' OR [PlumUnitAbbr]='OP' OR [PlumUnitAbbr]='OH' OR [PlumUnitAbbr]='OQ' OR [PlumUnitAbbr]='OB' OR [PlumUnitAbbr]='0' OR [PlumUnitAbbr]='1' OR [PlumUnitAbbr]='3'),
    CONSTRAINT [FK_ItemUnit_1__13] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
ALTER TABLE [dbo].[ItemUnit] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idxItemUnitName]
    ON [dbo].[ItemUnit]([Unit_Name] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemUnitUserID]
    ON [dbo].[ItemUnit]([User_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE TRIGGER [dbo].[ItemUnitAddUpdate] ON [dbo].[ItemUnit] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update ItemUnit 
		Set LastUpdateTimestamp = GetDate()
	from Inserted i
	where ItemUnit.Unit_Id = i.Unit_id

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemUnitAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemUnit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemUnit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUnit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemUnit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemUnit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemUnit] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUnit] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUnit] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemUnit] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUnit] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemUnit] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemUnit] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUnit] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUnit] TO [ExtractRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUnit] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUnit] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemUnit] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemUnit] TO [BizTalk]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUnit] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemUnit] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemUnit] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUnit] TO [IRMAPDXExtractRole]
    AS [dbo];

GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUnit] TO [TibcoDataWriter]
    AS [dbo];
