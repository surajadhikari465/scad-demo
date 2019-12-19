CREATE TABLE [dbo].[KNOWN_OOS_HEADER] (
    [ID]                INT          IDENTITY (1, 1) NOT NULL,
    [CREATED_BY]        VARCHAR (50) NULL,
    [CREATED_DATE]      DATETIME     NOT NULL,
    [LAST_UPDATED_BY]   VARCHAR (50) NULL,
    [LAST_UPDATED_DATE] DATETIME     NULL,
    CONSTRAINT [PK_KNOWN_OOS_HEADER] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO



-- =============================================
-- Description:	Populate audit columns on insert
-- =============================================
CREATE TRIGGER [dbo].[KNOWN_OOS_HEADER_INS] 
   ON  [dbo].[KNOWN_OOS_HEADER]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.KNOWN_OOS_HEADER set CREATED_DATE = (getdate()) where ID = (SELECT ID from Inserted); 
    UPDATE dbo.KNOWN_OOS_HEADER set CREATED_BY = (suser_sname())where ID = (SELECT ID from Inserted);
    
END




GO
-- =============================================
-- Description:	Populate audit columns on update
-- =============================================
CREATE TRIGGER [dbo].[KNOWN_OOS_HEADER_UPD] 
   ON  dbo.KNOWN_OOS_HEADER
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.KNOWN_OOS_HEADER set LAST_UPDATED_DATE = (getdate()) where ID = (SELECT ID from Deleted); 
    UPDATE dbo.KNOWN_OOS_HEADER set LAST_UPDATED_BY = (suser_sname())where ID = (SELECT ID from Deleted);
    
END
