CREATE FUNCTION dbo.fn_CheckUniquePLUMStoreNo( @StoreNo int, @PLUMStoreNo int )
RETURNS int
AS
BEGIN
    declare @PLUMStoreNoExists int
    set @PLUMStoreNoExists = 0

    if exists( select 'TRUE' from Store where Store.PLUMStoreNo = @PLUMStoreNo and Store.Store_No != @StoreNo )
    begin
        set @PLUMStoreNoExists = 1
    end
    
    return @PLUMStoreNoExists
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CheckUniquePLUMStoreNo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CheckUniquePLUMStoreNo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CheckUniquePLUMStoreNo] TO [IRMAReportsRole]
    AS [dbo];

