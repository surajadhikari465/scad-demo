CREATE TABLE [dbo].[InstanceDataFlagsStoreOverride] (
    [Store_No]  INT          NOT NULL,
    [FlagKey]   VARCHAR (50) NOT NULL,
    [FlagValue] BIT          CONSTRAINT [DF_InstanceDataFlagsStoreOverride_FlagValue] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_InstanceDataFlagsStoreOverride] PRIMARY KEY CLUSTERED ([Store_No] ASC, [FlagKey] ASC),
    CONSTRAINT [FK_InstanceDataFlags_InstanceDataFlagsStoreOverride_FlagKey] FOREIGN KEY ([FlagKey]) REFERENCES [dbo].[InstanceDataFlags] ([FlagKey]),
    CONSTRAINT [FK_Store_InstanceDataFlagsStoreOverride_StoreNo] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[InstanceDataFlagsStoreOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InstanceDataFlagsStoreOverride] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InstanceDataFlagsStoreOverride] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InstanceDataFlagsStoreOverride] TO [IRMAExcelRole]
    AS [dbo];

