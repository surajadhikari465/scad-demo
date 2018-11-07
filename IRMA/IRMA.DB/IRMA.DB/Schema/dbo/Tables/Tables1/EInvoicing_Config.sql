CREATE TABLE [dbo].[EInvoicing_Config] (
    [ElementName]             VARCHAR (255) NOT NULL,
    [Label]                   VARCHAR (255) NULL,
    [IsSacCode]               BIT           DEFAULT ((0)) NOT NULL,
    [IsHeaderElement]         BIT           DEFAULT ((0)) NOT NULL,
    [IsItemElement]           BIT           DEFAULT ((0)) NOT NULL,
    [SubTeam_No]              INT           NULL,
    [SacCodeType]             VARCHAR (255) NULL,
    [NeedsConfig]             BIT           DEFAULT ((1)) NOT NULL,
    [Disabled]                BIT           CONSTRAINT [EInvConfig_DisabledDefault] DEFAULT ((0)) NOT NULL,
    [ChargeOrAllowance]       INT           DEFAULT ((1)) NOT NULL,
    [ExcludeFromCalculations] BIT           DEFAULT ((0)) NULL,
    CONSTRAINT [EInvoicing_Config_PK] PRIMARY KEY CLUSTERED ([ElementName] ASC),
    CONSTRAINT [chkChargeOrAllowance] CHECK ([ChargeOrAllowance]=(-1) OR [ChargeOrAllowance]=(1))
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[EInvoicing_Config] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[EInvoicing_Config] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_Config] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EInvoicing_Config] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[EInvoicing_Config] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[EInvoicing_Config] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_Config] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EInvoicing_Config] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_Config] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_Config] TO [IRMAReportsRole]
    AS [dbo];

