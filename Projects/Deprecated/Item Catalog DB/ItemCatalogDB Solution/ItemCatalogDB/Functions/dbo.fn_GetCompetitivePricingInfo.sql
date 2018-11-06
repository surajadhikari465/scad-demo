

/****** Object:  UserDefinedFunction [dbo].[fn_getCompetitivePricingInfo]    Script Date: 07/13/2010 22:39:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_getCompetitivePricingInfo]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_getCompetitivePricingInfo]


/****** Object:  UserDefinedFunction [dbo].[fn_getCompetitivePricingInfo]    Script Date: 07/13/2010 22:39:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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