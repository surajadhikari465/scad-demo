﻿CREATE TABLE gpm.MessageArchivePrice
(
	MessageArchivePriceID BIGINT IDENTITY(1, 1) NOT NULL,
	EsbMessageID UNIQUEIDENTIFIER NOT NULL,
	GpmID UNIQUEIDENTIFIER NOT NULL,
	ItemID INT NOT NULL,
	BusinessUnitID INT NOT NULL,
	Region NVARCHAR(2) NOT NULL,
	PriceType NVARCHAR(3) NOT NULL,
	StartDate DATETIME2(7) NOT NULL,
	MessageAction NVARCHAR(25) NOT NULL,
	JsonObject NVARCHAR(MAX) NOT NULL,
	ErrorCode NVARCHAR(50) NULL,
	ErrorDetails NVARCHAR(MAX) NULL,
	[InsertDate] DATETIME2(7) CONSTRAINT [DF_MessageArchivePrice_InsertDate] DEFAULT (SYSDATETIME()) NOT NULL,
    CONSTRAINT [PK_MessageArchivePriceID] PRIMARY KEY CLUSTERED ([MessageArchivePriceID] ASC) WITH (FILLFACTOR = 100)
)