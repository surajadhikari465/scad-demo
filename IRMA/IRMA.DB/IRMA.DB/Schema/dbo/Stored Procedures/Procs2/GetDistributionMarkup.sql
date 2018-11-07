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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistributionMarkup] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistributionMarkup] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistributionMarkup] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistributionMarkup] TO [IRMAReportsRole]
    AS [dbo];

