CREATE TABLE [dbo].[InstanceData] (
    [PrimaryRegionName]    VARCHAR (20) NOT NULL,
    [PrimaryRegionCode]    VARCHAR (2)  NOT NULL,
    [PluDigitsSentToScale] VARCHAR (20) CONSTRAINT [DF_InstanceData_PluDigitsSentToScale] DEFAULT ('VARIABLE BY ITEM') NOT NULL,
    [PS_SetID]             VARCHAR (10) NULL,
    [UG_Culture]           VARCHAR (5)  NULL,
    [UG_DateMask]          VARCHAR (10) NULL,
    [InstanceDataID]       INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    CONSTRAINT [PK_InstanceData] PRIMARY KEY CLUSTERED ([InstanceDataID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[InstanceData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InstanceData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InstanceData] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InstanceData] TO [IRMAExcelRole]
    AS [dbo];

