CREATE TABLE [dbo].[REPORT_DETAIL_ATTRIBUTE_TYPE] (
    [ID]                INT            IDENTITY (1, 1) NOT NULL,
    [DESCRIPTION]       VARCHAR (500)  NOT NULL,
    [QUERY]             VARCHAR (6000) NULL,
    [CREATED_BY]        VARCHAR (50)   NULL,
    [CREATED_DATE]      DATETIME       NOT NULL,
    [LAST_UPDATED_BY]   VARCHAR (50)   NULL,
    [LAST_UPDATED_DATE] DATETIME       NULL,
    CONSTRAINT [PK_REPORT_DETAIL_ATTRIBUTE_TYPE] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO


-- =============================================
-- Description:	Populate audit columns on insert
-- =============================================
CREATE TRIGGER [dbo].[REPORT_DETAIL_ATTRIBUTE_TYPE_INS] 
   ON  [dbo].[REPORT_DETAIL_ATTRIBUTE_TYPE]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.REPORT_DETAIL_ATTRIBUTE_TYPE set CREATED_DATE = (getdate()) where ID = (SELECT ID from Inserted); 
    UPDATE dbo.REPORT_DETAIL_ATTRIBUTE_TYPE set CREATED_BY = (suser_sname())where ID = (SELECT ID from Inserted);
    
END



GO

-- =============================================
-- Description:	Populate audit columns on update
-- =============================================
CREATE TRIGGER [dbo].[REPORT_DETAIL_ATTRIBUTE_TYPE_UPD] 
   ON  [dbo].[REPORT_DETAIL_ATTRIBUTE_TYPE]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.REPORT_DETAIL_ATTRIBUTE_TYPE set LAST_UPDATED_DATE = (getdate()) where ID = (SELECT ID from Deleted); 
    UPDATE dbo.REPORT_DETAIL_ATTRIBUTE_TYPE set LAST_UPDATED_BY = (suser_sname())where ID = (SELECT ID from Deleted);
    
END

