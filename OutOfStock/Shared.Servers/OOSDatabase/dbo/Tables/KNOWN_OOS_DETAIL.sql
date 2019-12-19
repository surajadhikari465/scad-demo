CREATE TABLE [dbo].[KNOWN_OOS_DETAIL] (
    [ID]                  INT            IDENTITY (1, 1) NOT NULL,
    [REASON_ID]           INT            NULL,
    [SOURCE_ID]           INT            NULL,
    [STORE_ID]            INT            NULL,
    [VIN]                 VARCHAR (25)   NULL,
    [UPC]                 VARCHAR (25)   NULL,
    [NAT_UPC]             VARCHAR (25)   NULL,
    [START_DATE]          DATETIME       NULL,
    [END_DATE]            DATETIME       NULL,
    [CREATED_BY]          VARCHAR (50)   NULL,
    [CREATED_DATE]        DATETIME       NULL,
    [LAST_UPDATED_BY]     VARCHAR (50)   NULL,
    [LAST_UPDATED_DATE]   DATETIME       NULL,
    [PS_TEAM]             VARCHAR (25)   NULL,
    [PS_SUBTEAM]          VARCHAR (25)   NULL,
    [KNOWN_OOS_HEADER_ID] INT            NULL,
    [NOTES]               VARCHAR (2000) NULL,
    [ProductStatus]       VARCHAR (200)  NULL,
    [ExpirationDate]      DATETIME       NULL,
    [isDeleted]           BIT            CONSTRAINT [DF_KNOWN_OOS_DETAIL_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_KNOWN_OOS] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [KNOWN_OOS_DETAIL_KNOWN_OOS_HEADER_FK] FOREIGN KEY ([KNOWN_OOS_HEADER_ID]) REFERENCES [dbo].[KNOWN_OOS_HEADER] ([ID]),
    CONSTRAINT [KNOWN_OOS_REASON_FK] FOREIGN KEY ([REASON_ID]) REFERENCES [dbo].[REASON] ([ID]),
    CONSTRAINT [KNOWN_OOS_SOURCE_FK] FOREIGN KEY ([SOURCE_ID]) REFERENCES [dbo].[SOURCE] ([ID]),
    CONSTRAINT [KNOWN_OOS_STORE_FK] FOREIGN KEY ([STORE_ID]) REFERENCES [dbo].[STORE] ([ID])
);


GO
-- =============================================
-- Description:	Populate audit columns on insert
-- =============================================
CREATE TRIGGER [dbo].[KNOWN_OOS_DETAIL_INS] 
   ON  dbo.KNOWN_OOS_DETAIL
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.KNOWN_OOS_DETAIL set CREATED_DATE = (getdate()) where ID = (SELECT ID from Inserted); 
    UPDATE dbo.KNOWN_OOS_DETAIL set CREATED_BY = (suser_sname())where ID = (SELECT ID from Inserted);
    
END

GO
DISABLE TRIGGER [dbo].[KNOWN_OOS_DETAIL_INS]
    ON [dbo].[KNOWN_OOS_DETAIL];


GO
-- =============================================
-- Description:	Populate audit columns on update
-- =============================================
CREATE TRIGGER [dbo].[KNOWN_OOS_DETAIL_TRG_UPD] 
   ON  dbo.KNOWN_OOS_DETAIL
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.KNOWN_OOS_DETAIL set LAST_UPDATED_DATE = (getdate()) where ID = (SELECT ID from Deleted); 
    UPDATE dbo.KNOWN_OOS_DETAIL set LAST_UPDATED_BY = (suser_sname())where ID = (SELECT ID from Deleted);
    
END

GO
DISABLE TRIGGER [dbo].[KNOWN_OOS_DETAIL_TRG_UPD]
    ON [dbo].[KNOWN_OOS_DETAIL];

