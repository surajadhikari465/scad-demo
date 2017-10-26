CREATE TABLE [dbo].[InstanceDataFlags] (
    [FlagKey]          VARCHAR (50) NOT NULL,
    [FlagValue]        BIT          CONSTRAINT [DF_InstanceDataFlags_FlagValue] DEFAULT ((0)) NOT NULL,
    [CanStoreOverride] BIT          CONSTRAINT [DF_InstanceDataFlags_CanStoreOverride] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_InstanceDataFlags_FlagKey] PRIMARY KEY CLUSTERED ([FlagKey] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[InstanceDataFlags] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InstanceDataFlags] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InstanceDataFlags] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InstanceDataFlags] TO [IRMAExcelRole]
    AS [dbo];

GO
GRANT SELECT
    ON OBJECT::[dbo].[InstanceDataFlags] TO [IConInterface]
    AS [dbo];