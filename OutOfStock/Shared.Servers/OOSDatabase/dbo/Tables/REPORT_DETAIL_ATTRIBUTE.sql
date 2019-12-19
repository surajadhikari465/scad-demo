CREATE TABLE [dbo].[REPORT_DETAIL_ATTRIBUTE] (
    [ID]                              INT           IDENTITY (1, 1) NOT NULL,
    [REPORT_DETAIL_ID]                INT           NOT NULL,
    [REPORT_DETAIL_ATTRIBUTE_TYPE_ID] INT           NOT NULL,
    [VALUE]                           VARCHAR (300) NULL,
    [CREATED_BY]                      VARCHAR (50)  NULL,
    [CREATED_DATE]                    DATETIME      NOT NULL,
    [LAST_UPDATED_BY]                 VARCHAR (50)  NULL,
    [LAST_UPDATED_DATE]               VARCHAR (50)  NULL,
    CONSTRAINT [PK_REPORT_DETAIL_ATTRIBUTE] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [REPORT_DETAIL_ATTRIBUTE_REPORT_DETAIL_ATTRIBUTE_TYPE_FK] FOREIGN KEY ([REPORT_DETAIL_ATTRIBUTE_TYPE_ID]) REFERENCES [dbo].[REPORT_DETAIL_ATTRIBUTE_TYPE] ([ID]),
    CONSTRAINT [REPORT_DETAIL_ATTRIBUTE_REPORT_DETAIL_FK] FOREIGN KEY ([REPORT_DETAIL_ID]) REFERENCES [dbo].[REPORT_DETAIL] ([ID])
);


GO


-- =============================================
-- Description:	Populate audit columns on insert
-- =============================================
CREATE TRIGGER [dbo].[REPORT_DETAIL_ATTRIBUTE_INS] 
   ON  [dbo].[REPORT_DETAIL_ATTRIBUTE]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.REPORT_DETAIL_ATTRIBUTE set CREATED_DATE = (getdate()) where ID = (SELECT ID from Inserted); 
    UPDATE dbo.REPORT_DETAIL_ATTRIBUTE set CREATED_BY = (suser_sname())where ID = (SELECT ID from Inserted);
    
END



GO

-- =============================================
-- Description:	Populate audit columns on update
-- =============================================
CREATE TRIGGER [dbo].[REPORT_DETAIL_ATTRIBUTE_UPD] 
   ON  [dbo].[REPORT_DETAIL_ATTRIBUTE]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.REPORT_DETAIL_ATTRIBUTE set LAST_UPDATED_DATE = (getdate()) where ID = (SELECT ID from Deleted); 
    UPDATE dbo.REPORT_DETAIL_ATTRIBUTE set LAST_UPDATED_BY = (suser_sname())where ID = (SELECT ID from Deleted);
    
END

