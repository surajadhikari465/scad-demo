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
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_InsertErrorHistory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_InsertErrorHistory] TO [IRMAClientRole]
    AS [dbo];

