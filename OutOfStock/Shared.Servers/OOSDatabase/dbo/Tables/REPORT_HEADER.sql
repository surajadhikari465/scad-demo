CREATE TABLE [dbo].[REPORT_HEADER] (
    [ID]                        INT          IDENTITY (1, 1) NOT NULL,
    [STORE_ID]                  INT          NOT NULL,
    [CREATED_DATE]              DATETIME     NOT NULL,
    [CREATED_BY]                VARCHAR (50) NULL,
    [LAST_UPDATED_DATE]         DATETIME     NULL,
    [LAST_UPDATED_BY]           VARCHAR (50) NULL,
    [PROCESSED_FLAG]            VARCHAR (10) NULL,
    [EXCLUDE_FLAG]              INT          NULL,
    [OffsetCorrectedCreateDate] DATETIME     NULL,
    [TimeOffsetFromCentral]     SMALLINT     NULL,
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


GO


CREATE TRIGGER [dbo].[tr_ReportHeaderOffsetCorrection]
ON [dbo].[REPORT_HEADER]	
AFTER INSERT, UPDATE
AS 

	DECLARE @Action as char(1);
		SET @Action = (CASE WHEN EXISTS(SELECT * FROM INSERTED)
							 AND EXISTS(SELECT * FROM DELETED)
							THEN 'U'  -- Set Action to Updated.
							WHEN EXISTS(SELECT * FROM INSERTED)
							THEN 'I'  -- Set Action to Insert.
							WHEN EXISTS(SELECT * FROM DELETED)
							THEN 'D'  -- Set Action to Deleted.
							ELSE NULL -- Skip. It may have been a "failed delete".   
						END)

	DECLARE @msg  CHAR(17)
	SET @msg =  'trigger action: ' + @action;

	RAISERROR (@msg,0,1) WITH nowait 
	IF @Action = 'I' OR @Action = 'U'
		BEGIN
		
            UPDATE  dbo.REPORT_HEADER
            SET     report_header.OffsetCorrectedCreateDate = DATEADD(hour, r.TimeOffsetFromCentral, i.created_date) ,
                    report_header.TimeOffsetFromCentral = r.TimeOffsetFromCentral
            FROM    INSERTED i
                    INNER JOIN store s ON i.store_id = s.ID
                    INNER JOIN region r ON s.REGION_ID = r.ID
            WHERE   dbo.REPORT_HEADER.id = i.ID

		END