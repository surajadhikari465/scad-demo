﻿CREATE TABLE [dbo].[ITEM_MASTER] (
    [ID]                INT          IDENTITY (1, 1) NOT NULL,
    [NAT_UPC]           VARCHAR (15) NULL,
    [LONG_DESCRIPTION]  VARCHAR (50) NULL,
    [ITEM_SIZE]         VARCHAR (10) NULL,
    [ITEM_UOM]          VARCHAR (5)  NULL,
    [BRAND]             VARCHAR (10) NULL,
    [TIMESTAMP]         DATETIME     NULL,
    [FAMILY_NAME]       VARCHAR (50) NULL,
    [CATEGORY_NAME]     VARCHAR (50) NULL,
    [SUB_CATEGORY_NAME] VARCHAR (50) NULL,
    [CLASS_NAME]        VARCHAR (50) NULL,
    [CREATED_BY]        VARCHAR (50) NULL,
    [CREATED_DATE]      DATETIME     NULL,
    [LAST_UPDATED_BY]   VARCHAR (50) NULL,
    [LAST_UPDATED_DATE] DATETIME     NULL
);

