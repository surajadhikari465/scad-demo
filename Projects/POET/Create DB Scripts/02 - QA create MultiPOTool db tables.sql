Use MultiPOTool_QA

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ErrorLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ErrorLog]
GO

CREATE TABLE [dbo].[ErrorLog](
     [ErrorLogID] [int] IDENTITY(1,1)  NOT NULL ,
     [Timestamp] [datetime] NULL ,
     [ErrorMessage] [varchar](max) NULL 
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ErrorLog] WITH NOCHECK
    ADD CONSTRAINT [PK_ErrorLog] 
    PRIMARY KEY (ErrorLogID)
    ON [PRIMARY]

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Exception]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Exception]
GO

CREATE TABLE [dbo].[Exception](
     [ExceptionID] [int] NOT NULL ,
     [ExceptionDescription] [varchar](max) NOT NULL 
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Exception] WITH NOCHECK
    ADD CONSTRAINT [PK_Exceptions] 
    PRIMARY KEY (ExceptionID)
    ON [PRIMARY]

GO



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HelpLinks]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HelpLinks]
GO

CREATE TABLE [dbo].[HelpLinks](
     [HelpLinksID] [int] IDENTITY(1,1)  NOT NULL ,
     [LinkDescription] [varchar](max) NULL ,
     [LinkURL] varchar(max) NULL ,
     [UpdatedDate] [datetime] NULL ,
     [UpdatedUserID] [int] NULL ,
     [OrderOfAppearance] [int] NULL 
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[HelpLinks] WITH NOCHECK
    ADD CONSTRAINT [PK_HelpLinks] 
    PRIMARY KEY (HelpLinksID)
    ON [PRIMARY]

GO



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[POHeader]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[POHeader]
GO

CREATE TABLE [dbo].[POHeader](
     [POHeaderID] [int] IDENTITY(1,1)  NOT NULL ,
     [PONumberID] [int] NOT NULL ,
     [UploadSessionHistoryID] [int] NOT NULL ,
     [CreatedDate] [datetime] NOT NULL ,
     [VendorPSNumber] [varchar](10) NOT NULL ,
     [VendorName] [varchar](50) NULL ,
     [IRMAVendor_ID] [int] NULL ,
     [RegionID] [int] NOT NULL ,
     [BusinessUnit] [int] NOT NULL ,
     [Subteam] [int] NOT NULL ,
     [ExpectedDate] [datetime] NOT NULL ,
     [TotalPOCost] [money] NULL ,
     [Notes] [varchar](245)  NULL ,
     [DeletedDate] [datetime] NULL ,
     [OrderItemCount] [int] NULL ,
     [ExceptionItemCount] [int] NULL ,
     [ValidationAttemptDate] [datetime] NULL ,
     [PushedToIRMADate] [datetime] NULL ,
     [ConfirmedInIRMADate] [datetime] NULL ,
     [Expired] [bit] NULL ,
     [StoreAbbr] [varchar](5) NULL 
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[POHeader] WITH NOCHECK
    ADD CONSTRAINT [PK_POHeader] 
    PRIMARY KEY (POHeaderID)
    ON [PRIMARY]

GO



CREATE INDEX [IX_PONumberID_UploadSessionHistoryID]
    ON [dbo].[POHeader] ([PONumberID], [UploadSessionHistoryID]) ON [PRIMARY]

GO

CREATE INDEX [IX_UploadSessionHistoryID]
    ON [dbo].[POHeader] ([UploadSessionHistoryID]) ON [PRIMARY]

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[POItem]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[POItem]
GO

CREATE TABLE [dbo].[POItem](
     [POItemID] [int] IDENTITY(1,1)  NOT NULL ,
     [POHeaderID] [int] NOT NULL ,
     [Identifier] [varchar](13) NOT NULL ,
     [Item_Key] [int] NULL ,
     [VendorItemNumber] [varchar](20) NOT NULL ,
     [ItemBrand] [varchar](25) NOT NULL ,
     [ItemDescription] [varchar](60) NOT NULL ,
     [FreeQuantity] [int] NULL ,
     [OrderQuantity] [int] NULL ,
     [UnitCost] [money] NULL ,
     [UnitCostUOM] [int] NULL 
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[POItem] WITH NOCHECK
    ADD CONSTRAINT [PK_POItem] 
    PRIMARY KEY (POItemID)
    ON [PRIMARY]

GO


CREATE INDEX [IX_POHeaderID]
    ON [dbo].[POItem] ([POHeaderID]) ON [PRIMARY]

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[POItemException]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[POItemException]
GO

CREATE TABLE [dbo].[POItemException](
     [POItemExceptionID] [int] IDENTITY(1,1)  NOT NULL ,
     [POItemID] [int] NOT NULL ,
     [ExceptionID] [int] NOT NULL 
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[POItemException] WITH NOCHECK
    ADD CONSTRAINT [PK_POItemException] 
    PRIMARY KEY (POItemExceptionID)
    ON [PRIMARY]

GO



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PONumber]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PONumber]
GO

CREATE TABLE [dbo].[PONumber](
     [PONumberID] [int] IDENTITY(1,1)  NOT NULL ,
     [POTypeID] [int] NOT NULL ,
     [RegionID] [int] NOT NULL ,
     [POIncrement] [int] NOT NULL ,
     [PONumber] [int] NOT NULL ,
     [DateAssigned] [datetime] NOT NULL ,
     [PushedToIRMA] [bit] NOT NULL ,
     [AssignedByUserID] [int] NOT NULL 
) ON [PRIMARY]

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[POType]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[POType]
GO

CREATE TABLE [dbo].[POType](
     [POTypeID] [int] IDENTITY(1,1)  NOT NULL ,
     [POTypeCode] [varchar](5) NOT NULL ,
     [POTypeDescription] [varchar](50) NULL 
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[POType] WITH NOCHECK
    ADD CONSTRAINT [PK_POType] 
    PRIMARY KEY (POTypeID)
    ON [PRIMARY]

GO



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PushToIRMAQueue]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PushToIRMAQueue]
GO

CREATE TABLE [dbo].[PushToIRMAQueue](
     [PushToIRMAQueueID] [int] IDENTITY(1,1)  NOT NULL ,
     [POHeaderID] [int] NOT NULL ,
     [ProcessingFlag] [bit] NOT NULL 
) ON [PRIMARY]

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Regions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Regions]
GO

CREATE TABLE [dbo].[Regions](
     [RegionID] [int] NOT NULL ,
     [RegionCode] [varchar](2) NOT NULL ,
     [RegionName] [varchar](25) NOT NULL ,
     [IRMAServer] [varchar](6) NULL ,
     [IRMADatabase] [varchar](50) NULL 
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Regions] WITH NOCHECK
    ADD CONSTRAINT [PK_Regions] 
    PRIMARY KEY (RegionID)
    ON [PRIMARY]

GO



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UploadSessionHistory]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[UploadSessionHistory]
GO

CREATE TABLE [dbo].[UploadSessionHistory](
     [UploadSessionHistoryID] [int] IDENTITY(1,1)  NOT NULL ,
     [UploadUserID] [int] NOT NULL ,
     [FileName] [varchar](100) NULL ,
     [UploadedDate] [datetime] NOT NULL ,
     [ValidationSuccessful] [bit] NULL 
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UploadSessionHistory] WITH NOCHECK
    ADD CONSTRAINT [PK_UploadSessionHistory] 
    PRIMARY KEY (UploadSessionHistoryID)
    ON [PRIMARY]

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Users]
GO

CREATE TABLE [dbo].[Users](
     [UserID] [int] IDENTITY(1,1)  NOT NULL ,
     [UserName] [varchar](50) NOT NULL ,
     [RegionID] [int] NOT NULL ,
     [GlobalBuyer] [bit] DEFAULT ((0))  NOT NULL ,
     [Administrator] [bit] DEFAULT ((0))  NOT NULL ,
     [Active] [bit] DEFAULT ((0))  NOT NULL ,
     [InsertDate] [datetime] DEFAULT (getdate())  NULL ,
     [Email] [varchar](100) NULL ,
     [CCEmail] [varchar](100) NULL 
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Users] WITH NOCHECK
    ADD CONSTRAINT [PK_Users] 
    PRIMARY KEY (UserID)
    ON [PRIMARY]

GO



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ValidationQueue]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ValidationQueue]
GO

CREATE TABLE [dbo].[ValidationQueue](
     [ValidationQueueID] [int] IDENTITY(1,1)  NOT NULL ,
     [UploadSessionHistoryID] [int] NOT NULL ,
     [ProcessingFlag] [bit] NOT NULL 
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ValidationQueue] WITH NOCHECK
    ADD CONSTRAINT [PK_ValidationQueue] 
    PRIMARY KEY (ValidationQueueID)
    ON [PRIMARY]

GO

ALTER TABLE [dbo].[ValidationQueue]
    ADD CONSTRAINT [FK_ValidationQueue_UploadSessionHistory] 
    FOREIGN KEY (UploadSessionHistoryID)
    REFERENCES [dbo].[UploadSessionHistory] (UploadSessionHistoryID)

GO

ALTER TABLE [dbo].[PONumber] WITH NOCHECK
    ADD CONSTRAINT [PK_PONumber] 
    PRIMARY KEY (PONumberID)
    ON [PRIMARY]

GO

ALTER TABLE [dbo].[PONumber]
    ADD CONSTRAINT [FK_PONumber_Users] 
    FOREIGN KEY (AssignedByUserID)
    REFERENCES [dbo].[Users] (UserID)

GO

ALTER TABLE [dbo].[PONumber]
    ADD CONSTRAINT [FK_PONumber_POType] 
    FOREIGN KEY (POTypeID)
    REFERENCES [dbo].[POType] (POTypeID)

GO

ALTER TABLE [dbo].[PONumber]
    ADD CONSTRAINT [FK_PONumber_Regions] 
    FOREIGN KEY (RegionID)
    REFERENCES [dbo].[Regions] (RegionID)

GO

ALTER TABLE [dbo].[PushToIRMAQueue] WITH NOCHECK
    ADD CONSTRAINT [PK_PushToIRMAQueue] 
    PRIMARY KEY (PushToIRMAQueueID)
    ON [PRIMARY]

GO

ALTER TABLE [dbo].[PushToIRMAQueue]
    ADD CONSTRAINT [FK_PushToIRMAQueue_POHeader] 
    FOREIGN KEY (POHeaderID)
    REFERENCES [dbo].[POHeader] (POHeaderID)

GO

ALTER TABLE [dbo].[POItem]
    ADD CONSTRAINT [FK_POItem_POHeader] 
    FOREIGN KEY (POHeaderID)
    REFERENCES [dbo].[POHeader] (POHeaderID)

GO


ALTER TABLE [dbo].[POItemException]
    ADD CONSTRAINT [FK_POItemException_POItem] 
    FOREIGN KEY (POItemID)
    REFERENCES [dbo].[POItem] (POItemID)

GO

ALTER TABLE [dbo].[POItemException]
    ADD CONSTRAINT [FK_POItemException_Exceptions] 
    FOREIGN KEY (ExceptionID)
    REFERENCES [dbo].[Exception] (ExceptionID)

GO


ALTER TABLE [dbo].[UploadSessionHistory]
    ADD CONSTRAINT [FK_UploadSessionHistory_Users] 
    FOREIGN KEY (UploadUserID)
    REFERENCES [dbo].[Users] (UserID)

GO


ALTER TABLE [dbo].[Users]
    ADD CONSTRAINT [FK_Users_Regions] 
    FOREIGN KEY (RegionID)
    REFERENCES [dbo].[Regions] (RegionID)

GO