CREATE TABLE [dbo].[SubTeam] (
    [SubTeam_No]                       INT            NOT NULL,
    [Team_No]                          INT            NULL,
    [SubTeam_Name]                     VARCHAR (100)  NULL,
    [SubTeam_Abbreviation]             VARCHAR (10)   NULL,
    [Dept_No]                          INT            NULL,
    [SubDept_No]                       INT            NULL,
    [Buyer_User_ID]                    INT            NULL,
    [Target_Margin]                    DECIMAL (9, 4) CONSTRAINT [DF_SubTeam_Target_Margin] DEFAULT ((0)) NOT NULL,
    [JDA]                              INT            NULL,
    [GLPurchaseAcct]                   INT            NULL,
    [GLDistributionAcct]               INT            NULL,
    [GLTransferAcct]                   INT            NULL,
    [GLSalesAcct]                      INT            NULL,
    [Transfer_To_Markup]               DECIMAL (9, 4) NULL,
    [EXEWarehouseSent]                 BIT            CONSTRAINT [DF_SubTeam_EXEWarehouseSent] DEFAULT ((0)) NOT NULL,
    [ScaleDept]                        INT            NULL,
    [Retail]                           BIT            CONSTRAINT [DF_SubTeam_Retail] DEFAULT ((0)) NOT NULL,
    [EXEDistributed]                   BIT            CONSTRAINT [DF__SubTeam__EXEDist__0AD533D4] DEFAULT ((0)) NOT NULL,
    [SubTeamType_ID]                   TINYINT        NULL,
    [PurchaseThresholdCouponAvailable] BIT            NULL,
    [GLSuppliesAcct]                   INT            NULL,
    [GLPackagingAcct]                  INT            NULL,
    [FixedSpoilage]                    BIT            NULL,
    [InventoryCountByCase]             BIT            NULL,
    [Beverage]                         BIT            DEFAULT ((0)) NULL,
    [AlignedSubTeam]                   BIT            DEFAULT ((0)) NULL,
    CONSTRAINT [PK_SubTeam_SubTeam_No] PRIMARY KEY CLUSTERED ([SubTeam_No] ASC) WITH (FILLFACTOR = 80)
);





GO
ALTER TABLE [dbo].[SubTeam] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE TRIGGER SubTeamAddUpdate
ON SubTeam
FOR INSERT, UPDATE
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    INSERT INTO PMProductChg (HierLevel, ItemID, ItemDescription, ParentID, ParentDescription, ActionID)
    SELECT 'SubTeam', CONVERT(varchar(255), Inserted.SubTeam_No), Inserted.SubTeam_Name, CONVERT(varchar(255), Inserted.Team_No),
           Team.Team_Name, CASE WHEN Deleted.SubTeam_No IS NULL THEN 'ADD' ELSE 'CHANGE' END
    FROM Inserted
    INNER JOIN
        Team
        ON Team.Team_No = Inserted.Team_No
    LEFT JOIN
        Deleted
        ON Inserted.SubTeam_No = Deleted.SubTeam_No
    WHERE (ISNULL(Deleted.SubTeam_Name, '') <> ISNULL(Inserted.SubTeam_Name, ''))
          OR (ISNULL(Deleted.Team_No, 0) <> ISNULL(Inserted.Team_No, 0))

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('SubTeamAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT DELETE
    ON OBJECT::[dbo].[SubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[SubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[SubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[SubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[SubTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[SubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SubTeam] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[SubTeam] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[SubTeam] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SubTeam] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SubTeam] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SubTeam] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SubTeam] TO [ExtractRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SubTeam] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[SubTeam] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SubTeam] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[SubTeam] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[SubTeam] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SubTeam] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[SubTeam] TO [iCONReportingRole]
    AS [dbo];

