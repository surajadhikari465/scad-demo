SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetDistributionMarkup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetDistributionMarkup]
GO


CREATE PROCEDURE dbo.GetDistributionMarkup
@Vendor_ID int, 
@ReceiveLocation_ID int,
@SubTeam_No int
AS

BEGIN
    DECLARE @DiscountQuantity int

    SELECT @DiscountQuantity = Distribution_Markup
    FROM ZoneSupply
    WHERE SubTeam_No = @SubTeam_No AND
          FromZone_ID = (SELECT Zone_ID 
                         FROM Store INNER JOIN Vendor ON (Vendor.Store_No = Store.Store_No)
                         WHERE Vendor_ID = @Vendor_ID) AND
          ToZone_ID = (SELECT Zone_ID 
                       FROM Store INNER JOIN Vendor ON (Vendor.Store_No = Store.Store_No)
                       WHERE Vendor_ID = @ReceiveLocation_ID)

    IF (@DiscountQuantity IS NULL)
        SELECT 0 AS DiscountQuantity, 0 AS DiscountType
    ELSE
        SELECT @DiscountQuantity * -1 AS DiscountQuantity, 4 AS DiscountType
END 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


