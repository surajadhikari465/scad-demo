﻿CREATE TYPE app.ItemLocaleMessageType AS TABLE 
(
    [MessageTypeId]      INT            NOT NULL,
    [MessageStatusId]    INT            NOT NULL,
    [MessageHistoryId]   INT            NULL,
    [MessageActionId]    INT            NOT NULL,
    [IRMAPushID]         INT            NOT NULL,
    [InsertDate]         DATETIME2 (7)  DEFAULT (sysdatetime()) NOT NULL,
    [RegionCode]         VARCHAR (4)    NOT NULL,
    [BusinessUnit_ID]    INT            NOT NULL,
    [ItemId]             INT            NOT NULL,
    [ItemTypeCode]       NVARCHAR (3)   NOT NULL,
    [ItemTypeDesc]       NVARCHAR (255) NOT NULL,
    [LocaleId]           INT            NOT NULL,
    [LocaleName]         VARCHAR (255)  NOT NULL,
    [ScanCodeId]         INT            NOT NULL,
    [ScanCode]           VARCHAR (13)   NOT NULL,
    [ScanCodeTypeId]     INT            NOT NULL,
    [ScanCodeTypeDesc]   NVARCHAR (255) NOT NULL,
    [ChangeType]         VARCHAR (32)   NOT NULL,
    [LockedForSale]      BIT            NOT NULL,
    [Recall]             BIT            NOT NULL,
    [TMDiscountEligible] BIT            NOT NULL,
    [Case_Discount]      BIT            NOT NULL,
    [AgeCode]            INT            NULL,
    [Restricted_Hours]   BIT            NOT NULL,
    [Sold_By_Weight]     BIT            NOT NULL,
    [ScaleForcedTare]    BIT            NOT NULL,
    [Quantity_Required]  BIT            NOT NULL,
    [Price_Required]     BIT            NOT NULL,
    [QtyProhibit]        BIT            NOT NULL,
    [VisualVerify]       BIT            NOT NULL,
    [LinkedItemScanCode] NVARCHAR (13)  NULL,
    [PreviousLinkedItemScanCode] VARCHAR (13)   NULL,
    [PosScaleTare]       INT            NULL,
    [InProcessBy]        INT            NULL,
    [ProcessedDate]      DATETIME2 (7)  NULL
)
GO
