﻿CREATE TABLE [dbo].[store] (
    [ID]                 INT          IDENTITY (1, 1) NOT NULL,
    [PS_BU]              VARCHAR (10) NOT NULL,
    [STORE_ABBREVIATION] VARCHAR (10) NULL,
    [STORE_NAME]         VARCHAR (50) NULL,
    [REGION_ID]          INT          NOT NULL,
    [STATUS_ID]          INT          NULL,
    [LAST_UPDATED_DATE]  DATETIME     NULL,
    [LAST_UPDATED_BY]    VARCHAR (50) NULL,
    [CREATED_BY]         VARCHAR (50) NULL,
    [CREATED_DATE]       DATETIME     NOT NULL,
    [Hidden]             BIT          CONSTRAINT [df_store_hidden] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Store] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [STORE_REGION_FK] FOREIGN KEY ([REGION_ID]) REFERENCES [dbo].[REGION] ([ID]),
    CONSTRAINT [STORE_STATUS_FK] FOREIGN KEY ([STATUS_ID]) REFERENCES [dbo].[STATUS] ([ID])
);






GO



GO




GO



GO
