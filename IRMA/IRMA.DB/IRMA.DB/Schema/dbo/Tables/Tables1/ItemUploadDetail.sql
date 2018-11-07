CREATE TABLE [dbo].[ItemUploadDetail] (
    [ItemUploadDetail_ID]  INT           IDENTITY (1, 1) NOT NULL,
    [ItemUploadHeader_ID]  INT           NOT NULL,
    [ItemIdentifier]       VARCHAR (200) NULL,
    [POSDescription]       VARCHAR (200) NULL,
    [Description]          VARCHAR (200) NULL,
    [TaxClassID]           VARCHAR (200) NULL,
    [FoodStamps]           VARCHAR (200) NULL,
    [RestrictedHours]      VARCHAR (200) NULL,
    [EmployeeDiscountable] VARCHAR (200) NULL,
    [Discontinued]         VARCHAR (200) NULL,
    [NationalClassID]      VARCHAR (200) NULL,
    [Uploaded]             BIT           NULL,
    [Item_Key]             INT           NULL,
    [SubTeam_No]           INT           NULL,
    [ItemIdentifierValid]  BIT           CONSTRAINT [DF_ItemUploadDetail_ItemIdentifierValid] DEFAULT ((0)) NOT NULL,
    [SubTeamAllowed]       BIT           CONSTRAINT [DF_ItemUploadDetail_SubTeamAllowed] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ItemUploadDetail] PRIMARY KEY CLUSTERED ([ItemUploadDetail_ID] ASC),
    CONSTRAINT [FK_ItemUploadDetail_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_ItemUploadDetail_ItemUploadHeader] FOREIGN KEY ([ItemUploadHeader_ID]) REFERENCES [dbo].[ItemUploadHeader] ([ItemUploadHeader_ID]),
    CONSTRAINT [FK_ItemUploadDetail_SubTeam] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);

