

/****** Object:  StoredProcedure [dbo].[Reporting_COOLShipping]    Script Date: 01/14/2009 06:28:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reporting_COOLShipping]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Reporting_COOLShipping]
GO
CREATE PROCEDURE [dbo].[Reporting_COOLShipping]
@BeginDate DateTime,
@EndDate DateTime,
@Store_No Int,
@BIO bit,
@COOL bit,
@DC Int,
@SubTeam_No int
As

-- =============================================
-- Author:		Dave Stacey
-- Create date: 01/15/2009
-- Description:	SP used for COOL Shipping Report
-- =============================================

BEGIN
    SET NOCOUNT ON

select POID = CAST(COOL.InvoiceID AS INT), S.Store_Name, ST.SubTeam_Name, DC = DC.Store_Name, COOL.ShipmentDate, 
COOL.ShipmentTime, COOL.Product, COOL.ProductDescription, COOL.ProductSize, ShippedQuantity = CAST(COOL.ShippedQuantity AS INT), 
COOL.ExclusivelyFrom, COOL.Processed, COOL.InboundCarrierName
from  dbo.OrderItemCOOL4010Detail COOL 
JOIN dbo.OrderHeader OH on CAST(COOL.InvoiceID AS INT) = OH.OrderHeader_ID
JOIN dbo.Store DC ON CAST(COOL.ShippingDC AS INT) =  ISNULL(DC.BusinessUnit_ID, 0) % 100
JOIN dbo.Subteam ST ON OH.Transfer_To_SubTeam = ST.SubTeam_No
JOIN dbo.OrderItem OI ON OI.OrderItem_ID = COOL.OrderItem_ID AND OI.OrderHeader_ID = OH.OrderHeader_ID
JOIN dbo.Vendor V ON OH.ReceiveLocation_ID = V.Vendor_ID
JOIN dbo.Store S on S.Store_No = V.Store_No
WHERE 
(((@BeginDate IS NULL and @EndDate IS NULL) OR CAST(COOL.ShipmentDate AS DateTime) BETWEEN @BeginDate and @EndDate))
AND  (@Store_No IS NULL OR V.Store_No = @Store_No)
AND  (@COOL IS NULL OR OI.OrderItemCOOL = @COOL)
AND  (@BIO IS NULL OR OI.OrderItemBIO = @BIO)
AND  (@DC IS NULL OR CAST(COOL.ShippingDC AS INT) = @DC)
AND  (@SubTeam_No IS NULL OR st.SubTeam_No = @SubTeam_No)
GROUP BY CAST(COOL.InvoiceID AS INT), S.Store_Name, ST.SubTeam_Name, DC.Store_Name, COOL.ShipmentDate, 
COOL.ShipmentTime, COOL.Product, COOL.ProductDescription, COOL.ProductSize, CAST(COOL.ShippedQuantity AS INT), 
COOL.ExclusivelyFrom, COOL.Processed, COOL.InboundCarrierName

    SET NOCOUNT OFF
END
GO