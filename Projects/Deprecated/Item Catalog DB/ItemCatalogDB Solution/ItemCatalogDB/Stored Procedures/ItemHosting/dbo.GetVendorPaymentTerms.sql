SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVendorPaymentTerms]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
    drop procedure [dbo].[GetVendorPaymentTerms]
GO
CREATE PROCEDURE [dbo].[GetVendorPaymentTerms] 
AS

BEGIN

	SET NOCOUNT ON

	SELECT PaymentTermID, [Description], DateLoaded, [Default]
	FROM VendorPaymentTerms

	SET NOCOUNT OFF

END
GO