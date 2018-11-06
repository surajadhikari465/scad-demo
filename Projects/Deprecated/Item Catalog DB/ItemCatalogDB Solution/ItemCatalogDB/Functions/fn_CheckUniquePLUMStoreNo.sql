if exists( select 'TRUE' from sys.objects o where o.object_id = OBJECT_ID(N'fn_CheckUniquePLUMStoreNo') )
begin
    drop function dbo.fn_CheckUniquePLUMStoreNo
end
go

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_CheckUniquePLUMStoreNo]') )
drop function [dbo].[fn_CheckUniquePLUMStoreNo]
GO

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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



