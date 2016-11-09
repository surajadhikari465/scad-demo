/****** Object:  UserDefinedFunction [dbo].[fn_GET_DRLUOM_for_item]    Script Date: 04/25/2007 09:37:55 ******/

CREATE FUNCTION [dbo].[fn_GET_DRLUOM_for_item] (
@Item_Key int,
@Store_No int)
RETURNS nvarchar(2)
AS
BEGIN
DECLARE @DRLUOM nvarchar(2)
--Text_6 is NA's version of where the DRLUOM lands after conversion

select @DRLUOM = upcj.UnitPriceCode
FROM dbo.ShelfTag_UnitPriceCodeJurisdiction AS upcj
JOIN dbo.ShelfTag_UnitPriceCode AS upc ON upc.UnitPriceCodeID = upcj.UnitPriceCodeID
JOIN dbo.TaxJurisdiction AS tj ON tj.TaxJurisdictionID = upcj.TaxJurisdictionID
JOIN dbo.Store as st ON st.TaxJurisdictionID = tj.TaxJurisdictionID
JOIN dbo.ItemAttribute AS ia ON ia.Text_6 = convert(varchar(50),upc.UnitPriceCodeID)
where ia.Item_Key = @Item_Key
AND st.Store_No = @Store_No


RETURN @DRLUOM
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GET_DRLUOM_for_item] TO [IRMAClientRole]
    AS [dbo];

