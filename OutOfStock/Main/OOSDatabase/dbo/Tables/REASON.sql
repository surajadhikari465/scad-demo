CREATE TABLE [dbo].[REASON] (
    [ID]                 INT           IDENTITY (1, 1) NOT NULL,
    [REASON_DESCRIPTION] VARCHAR (500) NULL,
    [CREATED_BY]         VARCHAR (50)  NULL,
    [CREATED_DATE]       DATETIME      NULL,
    [LAST_UPDATED_BY]    VARCHAR (50)  NULL,
    [LAST_UPDATED_DATE]  DATETIME      NULL,
    CONSTRAINT [PK_REASON] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
-- =============================================
-- Description:	Populate audit columns on update
-- =============================================
CREATE TRIGGER [dbo].[REASON_UPD] 
   ON  dbo.REASON
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.REASON set LAST_UPDATED_DATE = (getdate()) where ID = (SELECT ID from Deleted); 
    UPDATE dbo.REASON set LAST_UPDATED_BY = (suser_sname())where ID = (SELECT ID from Deleted);
    
END

GO

-- =============================================
-- Description:	Populate audit columns on insert
-- =============================================
CREATE TRIGGER [dbo].[REASON_INS] 
   ON  [dbo].[REASON]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.REASON set CREATED_DATE = (getdate()) where ID = (SELECT ID from Inserted); 
    UPDATE dbo.REASON set CREATED_BY = (suser_sname())where ID = (SELECT ID from Inserted);
    
END

