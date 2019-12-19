CREATE TABLE [dbo].[REGION_MOVEMENT_QUERY] (
    [ID]                INT            IDENTITY (1, 1) NOT NULL,
    [REGION_ID]         INT            NULL,
    [QUERY]             VARCHAR (5000) NULL,
    [CREATED_BY]        VARCHAR (50)   NULL,
    [CREATED_DATE]      DATETIME       NOT NULL,
    [LAST_UPDATED_BY]   VARCHAR (50)   NULL,
    [LAST_UPDATED_DATE] DATETIME       NULL,
    CONSTRAINT [PK_REGION_MOVEMENT_QUERY] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [REGION_MOVEMENT_QUERY_FK] FOREIGN KEY ([REGION_ID]) REFERENCES [dbo].[REGION] ([ID])
);


GO


-- =============================================
-- Description:	Populate audit columns on insert
-- =============================================
CREATE TRIGGER [dbo].[REGION_MOVEMENT_QUERY_INS] 
   ON  [dbo].[REGION_MOVEMENT_QUERY]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.REGION_MOVEMENT_QUERY set CREATED_DATE = (getdate()) where ID = (SELECT ID from Inserted); 
    UPDATE dbo.REGION_MOVEMENT_QUERY set CREATED_BY = (suser_sname())where ID = (SELECT ID from Inserted);
    
END



GO

-- =============================================
-- Description:	Populate audit columns on update
-- =============================================
CREATE TRIGGER [dbo].[REGION_MOVEMENT_QUERY_UPD] 
   ON  [dbo].[REGION_MOVEMENT_QUERY]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.REGION_MOVEMENT_QUERY set LAST_UPDATED_DATE = (getdate()) where ID = (SELECT ID from Deleted); 
    UPDATE dbo.REGION_MOVEMENT_QUERY set LAST_UPDATED_BY = (suser_sname())where ID = (SELECT ID from Deleted);
    
END

