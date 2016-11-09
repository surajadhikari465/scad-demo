CREATE TABLE [dbo].[Store] (
    [Store_No]            INT           NOT NULL,
    [Store_Name]          VARCHAR (50)  NOT NULL,
    [Phone_Number]        VARCHAR (20)  NULL,
    [Mega_Store]          BIT           CONSTRAINT [DF__Store__Mega_Stor__70F39DC8] DEFAULT ((1)) NOT NULL,
    [Distribution_Center] BIT           CONSTRAINT [DF__Store__Distribut__72DBE63A] DEFAULT ((0)) NOT NULL,
    [Manufacturer]        BIT           CONSTRAINT [DF_Store_Manufacturer] DEFAULT ((0)) NOT NULL,
    [WFM_Store]           BIT           CONSTRAINT [DF_Store_WFM_Store] DEFAULT ((0)) NOT NULL,
    [Internal]            BIT           CONSTRAINT [DF_Store_Internal] DEFAULT ((0)) NOT NULL,
    [TelnetUser]          VARCHAR (25)  NULL,
    [TelnetPassword]      VARCHAR (25)  NULL,
    [BatchID]             INT           CONSTRAINT [DF_Store_BatchID] DEFAULT ((0)) NOT NULL,
    [BatchRecords]        INT           CONSTRAINT [DF_Store_BatchRecords] DEFAULT ((0)) NOT NULL,
    [BusinessUnit_ID]     INT           NULL,
    [Zone_ID]             INT           NULL,
    [UNFI_Store]          VARCHAR (12)  NULL,
    [LastRecvLogDate]     DATETIME      CONSTRAINT [DF_Store_LastRecvLogDate] DEFAULT (getdate()) NULL,
    [LastRecvLog_No]      INT           CONSTRAINT [DF__Store__LastRecvL__6BEB2AA0] DEFAULT ((0)) NULL,
    [RecvLogUser_ID]      INT           NULL,
    [EXEWarehouse]        SMALLINT      NULL,
    [Regional]            BIT           CONSTRAINT [DF_Store_Regional] DEFAULT ((0)) NOT NULL,
    [LastSalesUpdateDate] SMALLDATETIME CONSTRAINT [DF_Store_LastRecvLogDate1] DEFAULT (getdate()) NULL,
    [StoreAbbr]           VARCHAR (5)   NULL,
    [PLUMStoreNo]         INT           NULL,
    [TaxJurisdictionID]   INT           NULL,
    [POSSystemId]         INT           NULL,
    [PSI_Store_No]        INT           NULL,
    [StoreJurisdictionID] INT           NULL,
    [UseAvgCostHistory]   BIT           NULL,
    [GeoCode]             VARCHAR (15)  CONSTRAINT [DF_Store_GeoCode] DEFAULT (NULL) NULL,
    CONSTRAINT [PK_Store_Store_No] PRIMARY KEY CLUSTERED ([Store_No] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK__Store__RecvLogUs__6CDF4ED9] FOREIGN KEY ([RecvLogUser_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_Store_POSSystemId] FOREIGN KEY ([POSSystemId]) REFERENCES [dbo].[POSSystemTypes] ([POSSystemId]),
    CONSTRAINT [FK_Store_StoreJurisdictionID] FOREIGN KEY ([StoreJurisdictionID]) REFERENCES [dbo].[StoreJurisdiction] ([StoreJurisdictionID]),
    CONSTRAINT [FK_Store_TaxJurisdiction] FOREIGN KEY ([TaxJurisdictionID]) REFERENCES [dbo].[TaxJurisdiction] ([TaxJurisdictionID]),
    CONSTRAINT [FK_Store_Zone] FOREIGN KEY ([Zone_ID]) REFERENCES [dbo].[Zone] ([Zone_ID])
);





GO
ALTER TABLE [dbo].[Store] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idxStoreName]
    ON [dbo].[Store]([Store_Name] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxStoreZone]
    ON [dbo].[Store]([Zone_ID] ASC, [Store_No] ASC) WITH (FILLFACTOR = 80);


GO
CREATE TRIGGER StoreAddUpdate
ON Store
FOR INSERT, UPDATE 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	INSERT INTO PMOrganizationChg (HierLevel, ItemID, ItemDescription, ParentID, ParentDescription, ActionID)
    SELECT 'Store', Inserted.Store_No, Inserted.Store_Name, Zone.Zone_ID, Zone_Name,
           CASE WHEN (Deleted.Store_No IS NULL) OR ((ISNULL(Deleted.Mega_Store, 0) = 0 AND ISNULL(Deleted.WFM_Store, 0) = 0) AND (Inserted.Mega_Store = 1 OR Inserted.WFM_Store = 1))
                     THEN 'ADD'
                ELSE 'CHANGE' END
    FROM Inserted
    LEFT JOIN
        Deleted
        ON Inserted.Store_No = Deleted.Store_No
    LEFT JOIN
        Zone
        ON Inserted.Zone_ID = Zone.Zone_ID
    WHERE ((ISNULL(Deleted.Store_Name, '') <> ISNULL(Inserted.Store_Name, ''))
           OR (ISNULL(Deleted.Zone_ID, 0) <> ISNULL(Inserted.Zone_ID, 0)))
          AND (ISNULL(Deleted.Mega_Store, 0) = 1 OR ISNULL(Deleted.WFM_Store, 0) = 1 OR Inserted.Mega_Store = 1 OR Inserted.WFM_Store = 1)

    SELECT @Error_No = @@ERROR

    --IF @Error_No = 0
    --BEGIN
    --    INSERT INTO VendorExportQueue
    --    SELECT Vendor_ID
    --    FROM Vendor
    --	INNER JOIN
    --	    Inserted
    --        On Inserted.Store_No = Vendor.Store_No
    --	LEFT JOIN
    --	    Deleted
    --	    On Inserted.Store_No = Deleted.Store_No
    --    WHERE (Inserted.Store_Name <> ISNULL(Deleted.Store_Name, '') AND (Inserted.BusinessUnit_ID IS NOT NULL))
    --        OR (ISNULL(Inserted.BusinessUnit_ID, 0) <> ISNULL(Deleted.BusinessUnit_ID, 0))
    --
    --    SELECT @Error_No = @@ERROR
    --END 

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('StoreAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT DELETE
    ON OBJECT::[dbo].[Store] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Store] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Store] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Store] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Store] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Store] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Store] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Store] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Store] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Store] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Store] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Store] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Store] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Store] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Store] TO [ExtractRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Store] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Store] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Store] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Store] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Store] TO [MammothRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Store] TO [IConInterface]
    AS [dbo];

