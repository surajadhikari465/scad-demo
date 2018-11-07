CREATE TABLE [dbo].[MaxPurgeReference] (
    [TableName]            VARCHAR (50) NOT NULL,
    [MaxKeyID]             INT          DEFAULT ((0)) NOT NULL,
    [MaxPurgedKeyID]       INT          DEFAULT ((0)) NOT NULL,
    [LastModifiedDateTime] DATETIME     DEFAULT (getdate()) NOT NULL
);

