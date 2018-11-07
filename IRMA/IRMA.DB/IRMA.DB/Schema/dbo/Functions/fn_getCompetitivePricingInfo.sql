/****** Object:  UserDefinedFunction [dbo].[fn_getCompetitivePricingInfo]    Script Date: 06/22/2007 11:03:31 ******/
CREATE FUNCTION [dbo].[fn_getCompetitivePricingInfo] 
 (@Item_Key INT, @Store_No INT)

RETURNS @TblComp TABLE
(Item_Key int PRIMARY KEY CLUSTERED, CompetitorID int, CompetitorStore varchar(50), CompetitorSaleMultiple tinyint, CompetitorSalePrice  smallmoney, CompetitorPriceCheckDate DateTime)

AS

BEGIN

INSERT @TblComp
select top 1 @Item_Key, C.CompetitorID, C.Name, CP.SaleMultiple, CP.Sale, CP.CheckDate
from competitorprice cp
JOIN competitorstore cs on cs.CompetitorStoreID = cp.competitorstoreid
JOIN competitor c on c.CompetitorID = Cs.CompetitorID
JOIN competitorlocation cl on cl.CompetitorLocationID = CS.CompetitorLocationID
JOIN store s on s.store_name = cl.Name
WHERE CP.Item_Key = @Item_Key and s.store_no = @Store_No
ORDER BY CP.CheckDate Desc

IF NOT EXISTS (SELECT * FROM @TblComp) 
INSERT @TblComp (Item_Key)
select @Item_Key


RETURN
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_getCompetitivePricingInfo] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_getCompetitivePricingInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_getCompetitivePricingInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_getCompetitivePricingInfo] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_getCompetitivePricingInfo] TO [IRMAPromoRole]
    AS [dbo];

