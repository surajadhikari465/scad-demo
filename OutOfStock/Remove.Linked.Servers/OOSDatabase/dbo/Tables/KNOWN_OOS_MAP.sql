CREATE TABLE [dbo].[KNOWN_OOS_MAP] (
    [ID]                  INT          IDENTITY (1, 1) NOT NULL,
    [REGION_ID]           INT          NOT NULL,
    [KNOWN_OOS_HEADER_ID] INT          NULL,
    [VENDOR_KEY]          VARCHAR (10) NULL,
    [CREATED_BY]          VARCHAR (50) NULL,
    [CREATED_DATE]        DATETIME     NOT NULL,
    [LAST_UPDATED_BY]     VARCHAR (50) NULL,
    [LAST_UPDATED_DATE]   DATETIME     NULL,
    CONSTRAINT [PK_KNOWN_OOS_MAP] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [KNOWN_OOS_MAP_KNOWN_OOS_HEADER_FK] FOREIGN KEY ([KNOWN_OOS_HEADER_ID]) REFERENCES [dbo].[KNOWN_OOS_HEADER] ([ID]),
    CONSTRAINT [KNOWN_OOS_MAP_REGION_ID_FK] FOREIGN KEY ([REGION_ID]) REFERENCES [dbo].[REGION] ([ID])
);


GO
-- =============================================
-- Description:	Populate audit columns on insert
-- =============================================
CREATE TRIGGER [dbo].[KNOWN_OOS_MAP_INS] 
   ON  dbo.KNOWN_OOS_MAP
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.KNOWN_OOS_MAP set CREATED_DATE = (getdate()) where ID = (SELECT ID from Inserted); 
    UPDATE dbo.KNOWN_OOS_MAP set CREATED_BY = (suser_sname())where ID = (SELECT ID from Inserted);
    
END

GO
DISABLE TRIGGER [dbo].[KNOWN_OOS_MAP_INS]
    ON [dbo].[KNOWN_OOS_MAP];


GO
-- =============================================
-- Description:	Populate audit columns on update
-- =============================================
CREATE TRIGGER [dbo].[KNOWN_OOS_HEADER_MAP_UPD] 
   ON  dbo.KNOWN_OOS_MAP
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Update statements for trigger here
    UPDATE dbo.KNOWN_OOS_MAP set LAST_UPDATED_DATE = (getdate()) where ID = (SELECT ID from Deleted); 
    UPDATE dbo.KNOWN_OOS_MAP set LAST_UPDATED_BY = (suser_sname())where ID = (SELECT ID from Deleted);
    
END

GO
DISABLE TRIGGER [dbo].[KNOWN_OOS_HEADER_MAP_UPD]
    ON [dbo].[KNOWN_OOS_MAP];


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'KNOWN_OOS_HEADER_ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KNOWN_OOS_MAP', @level2type = N'COLUMN', @level2name = N'KNOWN_OOS_HEADER_ID';

