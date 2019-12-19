CREATE TABLE [dbo].[REPORT_DETAIL] (
    [ID]                INT             IDENTITY (1, 1) NOT NULL,
    [REPORT_HEADER_ID]  INT             NOT NULL,
    [REASON_ID]         INT             NULL,
    [LOCATION_ID]       INT             NULL,
    [VENDOR_KEY]        VARCHAR (10)    NULL,
    [VIN]               VARCHAR (25)    NULL,
    [PS_TEAM]           VARCHAR (50)    NULL,
    [PS_SUBTEAM]        VARCHAR (50)    NULL,
    [UPC]               VARCHAR (25)    NULL,
    [NAT_UPC]           VARCHAR (25)    NULL,
    [SOURCE_ID]         INT             NULL,
    [MOVEMENT]          DECIMAL (9, 3)  NULL,
    [EFF_COST]          DECIMAL (28, 3) NULL,
    [EFF_PRICE]         DECIMAL (28, 3) NULL,
    [EFF_PRICETYPE]     VARCHAR (5)     NULL,
    [NOTES]             VARCHAR (2000)  NULL,
    [CASE_SIZE]         DECIMAL (28, 4) NULL,
    [CREATED_BY]        VARCHAR (50)    NULL,
    [CREATED_DATE]      DATETIME        NOT NULL,
    [LAST_UPDATED_BY]   VARCHAR (50)    NULL,
    [LAST_UPDATED_DATE] DATETIME        NULL,
    [SCAN_DATE]         DATETIME        NULL,
    [BRAND]             VARCHAR (50)    NULL,
    [LONG_DESCRIPTION]  VARCHAR (50)    NULL,
    [ITEM_SIZE]         VARCHAR (10)    NULL,
    [ITEM_UOM]          VARCHAR (5)     NULL,
    [CATEGORY_NAME]     VARCHAR (50)    NULL,
    [CLASS_NAME]        VARCHAR (50)    NULL,
    [BRAND_NAME]        VARCHAR (50)    NULL,
    CONSTRAINT [PK_REPORT_DETAIL] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [REPORT_DETAIL_REASON_FK] FOREIGN KEY ([REASON_ID]) REFERENCES [dbo].[REASON] ([ID]),
    CONSTRAINT [REPORT_DETAIL_REPORT_HEADER_FK] FOREIGN KEY ([REPORT_HEADER_ID]) REFERENCES [dbo].[REPORT_HEADER] ([ID]),
    CONSTRAINT [REPORT_DETAIL_SOURCE_FK] FOREIGN KEY ([SOURCE_ID]) REFERENCES [dbo].[SOURCE] ([ID])
);




GO
-- =============================================
-- Description:	Populate audit columns on update
-- =============================================
CREATE TRIGGER [dbo].[REPORT_DETAIL_TRG_UPD] 
   ON  dbo.REPORT_DETAIL
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.REPORT_DETAIL set LAST_UPDATED_DATE = (getdate()) where ID = (SELECT ID from Deleted); 
    UPDATE dbo.REPORT_DETAIL set LAST_UPDATED_BY = (suser_sname())where ID = (SELECT ID from Deleted);
    
END

GO
DISABLE TRIGGER [dbo].[REPORT_DETAIL_TRG_UPD]
    ON [dbo].[REPORT_DETAIL];


GO
-- =============================================
-- Description:	Populate audit columns on insert
-- =============================================
CREATE TRIGGER [dbo].[REPORT_DETAIL_INS] 
   ON  dbo.REPORT_DETAIL
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.REPORT_DETAIL set CREATED_DATE = (getdate()) where ID = (SELECT ID from Inserted); 
    UPDATE dbo.REPORT_DETAIL set CREATED_BY = (suser_sname())where ID = (SELECT ID from Inserted);
    
END

GO
DISABLE TRIGGER [dbo].[REPORT_DETAIL_INS]
    ON [dbo].[REPORT_DETAIL];


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Out of Stock information for a particular UPC in a particular store.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'REPORT_DETAIL';


GO
CREATE NONCLUSTERED INDEX [ix_reportdetail_rhid]
    ON [dbo].[REPORT_DETAIL]([REPORT_HEADER_ID] ASC)
    INCLUDE([PS_TEAM], [UPC]);

