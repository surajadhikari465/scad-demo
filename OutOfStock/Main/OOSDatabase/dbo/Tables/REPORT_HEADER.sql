CREATE TABLE [dbo].[REPORT_HEADER] (
    [ID]                INT          IDENTITY (1, 1) NOT NULL,
    [STORE_ID]          INT          NOT NULL,
    [CREATED_DATE]      DATETIME     NOT NULL,
    [CREATED_BY]        VARCHAR (50) NULL,
    [LAST_UPDATED_DATE] DATETIME     NULL,
    [LAST_UPDATED_BY]   VARCHAR (50) NULL,
    [PROCESSED_FLAG]    VARCHAR (10) NULL,
    CONSTRAINT [PK_REPORT_HEADER] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [REPORT_HEADER_STORE_FK] FOREIGN KEY ([STORE_ID]) REFERENCES [dbo].[STORE] ([ID])
);


GO


-- =============================================
-- Description:	Populate audit columns on insert
-- =============================================
CREATE TRIGGER [dbo].[REPORT_HEADER_INS] 
   ON  [dbo].[REPORT_HEADER]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.REPORT_HEADER set CREATED_DATE = (getdate()) where ID = (SELECT ID from Inserted); 
    UPDATE dbo.REPORT_HEADER set CREATED_BY = (suser_sname())where ID = (SELECT ID from Inserted);
    
END



GO
DISABLE TRIGGER [dbo].[REPORT_HEADER_INS]
    ON [dbo].[REPORT_HEADER];


GO
-- =============================================
-- Description:	Populate audit columns on update
-- =============================================
CREATE TRIGGER [dbo].[REPORT_HEADER_UPD] 
   ON  dbo.REPORT_HEADER
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.REPORT_HEADER set LAST_UPDATED_DATE = (getdate()) where ID = (SELECT ID from Deleted); 
    UPDATE dbo.REPORT_HEADER set LAST_UPDATED_BY = (suser_sname())where ID = (SELECT ID from Deleted);
    
END

GO
DISABLE TRIGGER [dbo].[REPORT_HEADER_UPD]
    ON [dbo].[REPORT_HEADER];


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The record that ties report detail records together (per date, per store).', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'REPORT_HEADER';

