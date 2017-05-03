SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetAPUploadsUploaded]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetAPUploadsUploaded]
GO

CREATE PROCEDURE dbo.SetAPUploadsUploaded
    @OrderHeader_ID			int,
    @Freight3Party			bit,
    @PurchaseAccountsTotal	money,
    @APUploadedCost			money
  
AS
-- ****************************************************************************************************************
-- Procedure: SetAPUploadsUploaded
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2009/08/10	MD				WI 10393, WI 10721 Added the PurchaseAccountsTotal (includes only 500000 account charges) and 
--								APUploadedCost (includes all charges - 500000, allowance and charges) field to be stored in OrderHeader table
-- 2010/04/12	TL		12198	Removed @Date as a parameter passed in, and set @Date within this proc so that DB-system time is used for OrderHeader.UploadedDate.
-- 2011/12/26	KM		3744	Added update history template; changed file extension; coding standards;
-- ****************************************************************************************************************
BEGIN
    SET NOCOUNT ON

    DECLARE 
		@Date					datetime,
		@CentralTimeZoneOffset	int
    
    SELECT @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region
    SELECT @Date = DATEADD(hour, @CentralTimeZoneOffset, GETDATE())

	IF @Freight3Party = 0
		BEGIN
		
			-- Update the OrderHeader table for vendor invoices
			UPDATE 
				OrderHeader 
			SET 
				UploadedDate			= @Date, 
				PurchaseAccountsTotal	= @PurchaseAccountsTotal, 
				APUploadedCost			= @APUploadedCost
			WHERE 
				OrderHeader_ID = @OrderHeader_ID
		END
	ELSE
	
	BEGIN
		
		-- Update the OrderHeader table for vendor invoices
		UPDATE 
			OrderInvoice_Freight3Party
		SET 
			UploadedDate = @Date 
		WHERE
			OrderHeader_ID = @OrderHeader_ID
	END
	          
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO