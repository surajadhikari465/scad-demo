CREATE TABLE [dbo].[Scale_ProductClaims] (
    [Scale_ProductClaims_ID] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Description]         VARCHAR (50)   NOT NULL,
    [ProductClaims]       VARCHAR (120) NOT NULL,
    PRIMARY KEY CLUSTERED ([Scale_ProductClaims_ID] ASC) WITH (FILLFACTOR = 80)
);

GO