/****** Object:  StoredProcedure [dbo].[GetVendorDealTypes]    Script Date: 1/25/2007 11:06:56 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetVendorDealTypes]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetVendorDealTypes]
GO

/****** Object:  StoredProcedure [dbo].[GetVendorDealTypes]    Script Date: 1/25/2007 11:06:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE dbo.GetVendorDealTypes
AS
BEGIN
    SET NOCOUNT ON

	SELECT VendorDealTypeID, Code, Description, CaseAmtType
    FROM VendorDealType
    ORDER BY Description
    
    SET NOCOUNT OFF
END
GO
 