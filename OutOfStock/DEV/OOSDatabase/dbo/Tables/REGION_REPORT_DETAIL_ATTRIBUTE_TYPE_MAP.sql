CREATE TABLE [dbo].[REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP] (
    [ID]                              INT          IDENTITY (1, 1) NOT NULL,
    [REGION_ID]                       INT          NOT NULL,
    [REPORT_DETAIL_ATTRIBUTE_TYPE_ID] INT          NOT NULL,
    [CREATED_BY]                      VARCHAR (50) NULL,
    [CREATED_DATE]                    DATETIME     NOT NULL,
    [LAST_UPDATED_BY]                 VARCHAR (50) NULL,
    [LAST_UPDATED_DATE]               DATETIME     NULL,
    CONSTRAINT [PK_REG_REPT_DET_ATTRBT_TYPE_MAP] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP_REGION_FK] FOREIGN KEY ([REGION_ID]) REFERENCES [dbo].[REGION] ([ID]),
    CONSTRAINT [REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP_REPORT_DETAIL_ATTRIBUTE_TYPE_FK] FOREIGN KEY ([REPORT_DETAIL_ATTRIBUTE_TYPE_ID]) REFERENCES [dbo].[REPORT_DETAIL_ATTRIBUTE_TYPE] ([ID])
);


GO


-- =============================================
-- Description:	Populate audit columns on insert
-- =============================================
CREATE TRIGGER [dbo].[REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP_INS] 
   ON  [dbo].[REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP set CREATED_DATE = (getdate()) where ID = (SELECT ID from Inserted); 
    UPDATE dbo.REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP set CREATED_BY = (suser_sname())where ID = (SELECT ID from Inserted);
    
END



GO

-- =============================================
-- Description:	Populate audit columns on update
-- =============================================
CREATE TRIGGER [dbo].[REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP_UPD] 
   ON  [dbo].[REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP set LAST_UPDATED_DATE = (getdate()) where ID = (SELECT ID from Deleted); 
    UPDATE dbo.REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP set LAST_UPDATED_BY = (suser_sname())where ID = (SELECT ID from Deleted);
    
END


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mapping to indicate which regions want which report detail attribute types.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'REGION_REPORT_DETAIL_ATTRIBUTE_TYPE_MAP';

