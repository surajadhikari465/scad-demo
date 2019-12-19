CREATE TABLE [dbo].[CONFIG_TEXT] (
    [ID]                  INT            IDENTITY (1, 1) NOT NULL,
    [CONFIG_TEXT_RAW]     VARCHAR (2000) NULL,
    [CONFIG_TEXT_DISPLAY] VARCHAR (2000) NULL,
    [CREATED_BY]          VARCHAR (50)   NULL,
    [CREATED_DATE]        DATETIME       NOT NULL,
    [LAST_UPDATED_BY]     VARCHAR (50)   NULL,
    [LAST_UPDATED_DATE]   DATETIME       NULL,
    [START_DATE]          DATETIME       NULL,
    [END_DATE]            DATETIME       NULL,
    [PAGE_NAME]           VARCHAR (50)   NULL,
    [FIELD_NAME]          VARCHAR (50)   NULL,
    CONSTRAINT [PK_CONFIG_TEXT] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO


-- =============================================
-- Description:	Populate audit columns on insert
-- =============================================
CREATE TRIGGER [dbo].[CONFIG_TEXT_INS] 
   ON  [dbo].[CONFIG_TEXT]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.CONFIG_TEXT set CREATED_DATE = (getdate()) where ID = (SELECT ID from Inserted); 
    UPDATE dbo.CONFIG_TEXT set CREATED_BY = (suser_sname())where ID = (SELECT ID from Inserted);
    
END



GO

-- =============================================
-- Description:	Audit column for last updated date
-- =============================================
CREATE TRIGGER [dbo].[CONFIG_TEXT_UPD] 
   ON  [dbo].[CONFIG_TEXT]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.CONFIG_TEXT set LAST_UPDATED_DATE = (getdate()) where ID = (SELECT ID from Deleted); 
    UPDATE dbo.CONFIG_TEXT set LAST_UPDATED_BY = (suser_sname())where ID = (SELECT ID from Deleted);
    
END

