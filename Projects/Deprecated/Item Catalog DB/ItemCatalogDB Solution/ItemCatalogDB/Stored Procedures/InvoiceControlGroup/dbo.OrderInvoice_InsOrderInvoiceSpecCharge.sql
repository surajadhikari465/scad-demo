/****** Object:  StoredProcedure [dbo].[OrderInvoice_InsOrderInvoiceSpecCharge]    Script Date: 08/07/2008 12:54:26 ******/
IF EXISTS (
       SELECT *
       FROM   sys.objects
       WHERE  OBJECT_ID = OBJECT_ID(N'[dbo].[OrderInvoice_InsOrderInvoiceSpecCharge]')
              AND TYPE IN (N'P', N'PC')
   )
    DROP PROCEDURE [dbo].[OrderInvoice_InsOrderInvoiceSpecCharge]

GO

CREATE PROCEDURE [dbo].[OrderInvoice_InsOrderInvoiceSpecCharge]
	@OrderHeader_Id INT,
	@SACType_ID INT,
	@Description VARCHAR(100),
	@SubTeam_No INT,
	@isAllowance BIT,
	@Value SMALLMONEY
AS
/*
#################################################################################################################################
Revision History
=================================================================================================================================
Bug:     TFS 2311
Date:    6/29/2011
By:      Min Zhao
Added input parameter @isAllowance to the stored proc so that the isAllowance flag on the OrderInvoiceCharges table will be set
for the Special Allowance and Charge (SAC) for a PO.

Date		DEV		TFS			Comment
05/24/2013	DN		12007		Added ISNULL() function when comparing NULL values in Subteam_No
#################################################################################################################################
*/
BEGIN
	IF @SubTeam_No = -1
		set @SubTeam_No = null
	
	IF @Description = '' 
		SET @Description = NULL
		
		
	DECLARE @SpecChargeExists BIT    
	SELECT @SpecChargeExists = CASE 
	                                WHEN (
	                                         SELECT COUNT(1)
	                                         FROM   dbo.OrderInvoiceCharges OIC
	                                         WHERE  OIC.OrderHeader_ID = @OrderHeader_ID
	                                                AND OIC.SACType_ID = @SACType_ID
	                                                AND ISNULL(OIC.SubTeam_No,0) = @SubTeam_No
	                                                AND OIC.[Description] = @Description
	                                     ) > 0 THEN 1
	                                ELSE 0
	                           END
	
	IF @SpecChargeExists = 0
	    INSERT dbo.OrderInvoiceCharges
	      (
	        OrderHeader_Id,
	        SACType_ID,
	        [Description],
	        SubTeam_No,
	        IsAllowance,
	        VALUE
	      )
	    SELECT @OrderHeader_Id,
	           @SACType_ID,
	           @Description,
	           @SubTeam_No,
	           @isAllowance,
	           @Value
	ELSE
	    UPDATE dbo.OrderInvoiceCharges
	    SET    VALUE = VALUE + @Value,
			   IsAllowance = @isAllowance
	    WHERE  OrderHeader_ID = @OrderHeader_ID
	           AND SACType_ID = @SACType_ID
	           AND SubTeam_No = @SubTeam_No
	           AND [Description] = @Description
END          
GO
