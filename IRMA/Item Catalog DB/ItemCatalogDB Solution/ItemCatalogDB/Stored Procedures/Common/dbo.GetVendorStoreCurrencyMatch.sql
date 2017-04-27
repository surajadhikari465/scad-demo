SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVendorStoreCurrencyMatch]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVendorStoreCurrencyMatch]
GO

CREATE PROCEDURE [dbo].[GetVendorStoreCurrencyMatch]
    @StoreID	int,
    @VendorID	int,
    @Direction	varchar(25)
AS 
   -- **************************************************************************
   -- Procedure: GetVendorStoreCurrencyMatch()
   --    Author: Billy Blackerby
   --      Date: 09.03.09
   --
   -- Description:
   -- This procedure is called from IRMA Client code in various calls to deteremine
   -- vendor to store currency match
   --
   -- Modification History:
   -- Date        Init	Comment
   -- **************************************************************************
BEGIN    
	DECLARE @VendorCurrency	varchar(10)
	DECLARE @StoreCurrency	varchar(10)

    SET NOCOUNT ON    
        IF @Direction = 'USD-CAD'
			BEGIN
				SET @VendorCurrency = ISNULL((SELECT CurrencyCode FROM Vendor (nolock) v JOIN Currency (nolock) c ON v.CurrencyID = c.CurrencyID WHERE v.Vendor_ID = @VendorID), 'USD')

				SET @StoreCurrency  = ISNULL((SELECT CurrencyCode FROM Vendor (nolock) v JOIN Currency (nolock) c ON v.CurrencyID = c.CurrencyID WHERE v.Vendor_ID = @StoreID), 'USD')
				
				IF @StoreCurrency = 'USD' AND @VendorCurrency = 'CAD'
					SELECT 'False' AS 'Match'
				ELSE
					SELECT 'True' AS 'Match'
			END

    SET NOCOUNT OFF    
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
