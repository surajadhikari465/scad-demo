create function dbo.fn_IsDISPriceChangeType
    (@PriceChgTypeID int)
returns bit
as
begin
	declare @result bit

	select @result = (select case when count(*) = 0 then 0 else 1 end
					from	PriceChgType
					where	PriceChgTypeID = @PriceChgTypeID and Lower(PriceChgTypeDesc) = 'dis')

    return @result
end