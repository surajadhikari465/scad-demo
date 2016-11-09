create function dbo.fn_LineDriveType
    ()
returns int
as
begin

--	This returns the one PriceChgType that is authorized for line drives.  
--	If we allow more than one in the future, this should be changed to accept 
--	PriceChgTypeId and return the LineDrive flag instead.  

	declare @result int

	select @result = isnull((select PriceChgTypeID
					from	PriceChgType
					where	LineDrive = 1),0)

    return @result
end