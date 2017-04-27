SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetRegPriceExists') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetRegPriceExists
GO

CREATE PROCEDURE dbo.GetRegPriceExists
    @Item_Key int,
    @Store_No int
AS

BEGIN
-- This returns true if there is an unfinished batch containing this store/item combo
-- Fully Processed batches and unbatched records don't count
    SET NOCOUNT ON

DECLARE @RegPriceExists bit,
	@RegPriceChgTypeId int

select @RegPriceChgTypeId = PriceChgTypeId
from	PriceChgType
where	On_Sale = 0

SELECT @RegPriceExists = case when exists (
	SELECT 1 
	FROM PriceBatchDetail PBD
	WHERE PBD.Item_Key = @Item_Key
	  AND PBD.Store_No = @Store_No 
	  AND PBD.PriceChgTypeId = @RegPriceChgTypeId) then 1 
	when exists (
	SELECT 1
	FROM Price P
	WHERE P.Item_Key = @Item_Key
	  AND P.Store_No = @Store_No 
	  AND P.Price > 0.0) then 1
	  else 0 end

SELECT RegPriceExists = @RegPriceExists

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



