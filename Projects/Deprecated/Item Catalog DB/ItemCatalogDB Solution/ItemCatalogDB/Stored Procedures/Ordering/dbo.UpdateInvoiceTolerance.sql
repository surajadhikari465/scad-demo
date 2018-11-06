if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateInvoiceTolerance]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateInvoiceTolerance]
GO
CREATE PROCEDURE dbo.UpdateInvoiceTolerance

        (
        @Vendor_Tolerance decimal(5,2),
        @Vendor_Tolerance_Amount smallmoney,
        @User_ID int
        )

AS
BEGIN
        SET NOCOUNT ON

        /* -- Description:
   -- This procedure updates the Regional Tolerance Setting
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 11/11/2009  AZ	Creation Date
   -- 08/12/2010  RE	If InvoiceMatchingTolerance record does not exist, create a NULL one so new values can be 'updated' from the UI.
   -- 01/17/2012  BJL	TFS 4314 - Use DEFAULT value for UpdateDate.
   */
   
   	-- if a record doesnt exist create it. on 1 should exist and always be updated afterwards.
	IF NOT EXISTS  (SELECT * FROM  InvoiceMatchingTolerance)
		INSERT INTO InvoiceMatchingTolerance 
			(	
				Vendor_Tolerance, 
				Vendor_Tolerance_Amount, 
				User_ID, 
				UpdateDate
			)
		VALUES 
			(
				null,
				null,
				null,
				null
			)
	
    
    -- update the existing record.
    UPDATE    InvoiceMatchingTolerance
              SET	[Vendor_Tolerance] = @Vendor_Tolerance,
					[Vendor_Tolerance_Amount] = @Vendor_Tolerance_Amount,
                    [User_ID] = @User_ID,
					[UpdateDate] = DEFAULT
                                                                                        
                                                                                        
        SET NOCOUNT Off
END

GO
