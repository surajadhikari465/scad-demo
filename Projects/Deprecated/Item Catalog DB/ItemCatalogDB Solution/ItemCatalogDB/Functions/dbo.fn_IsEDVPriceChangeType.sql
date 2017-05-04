IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_IsEDVPriceChangeType') 
    DROP FUNCTION fn_IsEDVPriceChangeType
GO

create function dbo.fn_IsEDVPriceChangeType
    (@PriceChgTypeID int)
returns bit
as
begin
	declare @result bit

	select @result = isnull((select MSRP_Required
					from	PriceChgType
					where	PriceChgTypeID = @PriceChgTypeID),0)

    return @result
end    

GO