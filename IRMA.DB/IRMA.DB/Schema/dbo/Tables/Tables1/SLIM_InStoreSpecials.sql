CREATE TABLE [dbo].[SLIM_InStoreSpecials] (
    [RequestId]        INT           IDENTITY (1, 1) NOT NULL,
    [Item_Key]         INT           NOT NULL,
    [Store_no]         INT           NOT NULL,
    [Price]            MONEY         NOT NULL,
    [Multiple]         INT           NOT NULL,
    [SalePrice]        MONEY         NOT NULL,
    [SaleMultiple]     INT           NOT NULL,
    [POSPrice]         MONEY         NOT NULL,
    [POSSalePrice]     MONEY         NULL,
    [StartDate]        SMALLDATETIME NOT NULL,
    [EndDate]          SMALLDATETIME NOT NULL,
    [Status]           INT           NULL,
    [RequestedBy]      VARCHAR (25)  NULL,
    [ProcessedBy]      VARCHAR (25)  NULL,
    [Comments]         VARCHAR (255) NULL,
    [Identifier]       VARCHAR (13)  NULL,
    [Subteam_Name]     VARCHAR (100) NULL,
    [Item_Description] VARCHAR (60)  NULL,
    CONSTRAINT [PK_SLIM_InStoreSpecials] PRIMARY KEY CLUSTERED ([RequestId] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_InStoreSpecials] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[SLIM_InStoreSpecials] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[SLIM_InStoreSpecials] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_InStoreSpecials] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[SLIM_InStoreSpecials] TO [IRMASLIMRole]
    AS [dbo];

