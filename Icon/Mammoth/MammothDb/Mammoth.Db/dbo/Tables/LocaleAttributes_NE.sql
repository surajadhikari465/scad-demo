﻿CREATE TABLE [dbo].[LocaleAttributes_NE] (
    [Region]            NCHAR (2)      DEFAULT ('NE') NOT NULL,
    [LocaleAttributeID] INT            IDENTITY (1, 1) NOT NULL,
    [AttributeID]       INT            NULL,
    [AttributeValue]    NVARCHAR (255) NULL,
    [AddedDate]         DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]      DATETIME       NULL,
    CONSTRAINT [PK_LocaleAttributes_NE] PRIMARY KEY CLUSTERED ([Region] ASC, [LocaleAttributeID] ASC) WITH (FILLFACTOR = 100) ON [FG_NE]
);
