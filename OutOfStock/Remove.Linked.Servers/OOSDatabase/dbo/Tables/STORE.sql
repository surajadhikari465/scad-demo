CREATE TABLE [dbo].[STORE] (
    [ID]                 INT          IDENTITY (1, 1) NOT NULL,
    [PS_BU]              VARCHAR (10) NOT NULL,
    [STORE_ABBREVIATION] VARCHAR (10) NULL,
    [STORE_NAME]         VARCHAR (50) NULL,
    [REGION_ID]          INT          NOT NULL,
    [STATUS_ID]          INT          NULL,
    [LAST_UPDATED_DATE]  DATETIME     NULL,
    [LAST_UPDATED_BY]    VARCHAR (50) NULL,
    [CREATED_BY]         VARCHAR (50) NULL,
    [CREATED_DATE]       DATETIME     NOT NULL,
    CONSTRAINT [PK_STORE] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [STORE_REGION_FK] FOREIGN KEY ([REGION_ID]) REFERENCES [dbo].[REGION] ([ID]),
    CONSTRAINT [STORE_STATUS_FK] FOREIGN KEY ([STATUS_ID]) REFERENCES [dbo].[STATUS] ([ID])
);




GO
CREATE NONCLUSTERED INDEX [IX_STORE_PS_BU]
    ON [dbo].[STORE]([PS_BU] ASC);


GO


-- =============================================
-- Description:	Populate audit columns on insert
-- =============================================
CREATE TRIGGER [dbo].[STORE_INS] 
   ON  [dbo].[STORE]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.STORE set CREATED_DATE = (getdate()) where ID = (SELECT ID from Inserted); 
    UPDATE dbo.STORE set CREATED_BY = (suser_sname())where ID = (SELECT ID from Inserted);
    
END



GO
DISABLE TRIGGER [dbo].[STORE_INS]
    ON [dbo].[STORE];


GO
-- =============================================
-- Description:	Populate audit columns on update
-- =============================================
CREATE TRIGGER [dbo].[STORE_TRG_UPD] 
   ON  dbo.STORE
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.STORE set LAST_UPDATED_DATE = (getdate()) where ID = (SELECT ID from Deleted); 
    UPDATE dbo.STORE set LAST_UPDATED_BY = (suser_sname())where ID = (SELECT ID from Deleted);
    
END