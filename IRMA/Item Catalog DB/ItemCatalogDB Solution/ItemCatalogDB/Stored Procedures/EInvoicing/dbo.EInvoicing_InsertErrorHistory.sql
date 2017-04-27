
IF EXISTS (
       SELECT *
       FROM   sysobjects
       WHERE  NAME = 'EInvoicing_InsertErrorHistory'
              AND xtype = 'P'
   )
    DROP PROCEDURE dbo.EInvoicing_InsertErrorHistory
GO

CREATE PROCEDURE dbo.EInvoicing_InsertErrorHistory
	@EInvoiceId INT,
	@ErrorInformation VARCHAR(MAX)
AS
BEGIN
	-- insert new errorhistory entry.
	INSERT INTO EInvoicing_ErrorHistory
	  (
	    EInvoiceId,
	    ErrorInformation
	  )
	VALUES
	  (
	    @EInvoiceId,
	    @ErrorInformation
	  );
	
	-- delete old errorhistory entries. keep only the latest 10 for each einvoice.
	WITH ErrorHistoryRN AS (
	    SELECT *,
	           ROW_NUMBER() OVER(ORDER BY [timestamp] DESC) AS RowNum
	    FROM   EInvoicing_ErrorHistory eeh
	    WHERE  eeh.EInvoiceId = @EInvoiceId
	)
	DELETE 
	FROM   ErrorHistoryRN
	WHERE  RowNum > 10;
END
GO 




