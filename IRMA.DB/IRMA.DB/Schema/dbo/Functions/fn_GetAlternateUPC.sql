-- =============================================
-- Author:		VC
-- Create date: 4/11/2007
-- Description:	Returns a String of Additional UPC (Non default,non deleted)
-- =============================================
CREATE function [dbo].[fn_GetAlternateUPC]( @ItemKey int )
returns varchar(200)
as
begin
	declare curUPC cursor FAST_FORWARD for
	select Identifier from ItemIdentifier
		WHERE default_identifier <> 1 
			AND deleted_identifier <>1
			AND Item_key = @ItemKey

	declare 
		@value varchar(20),
		@return varchar(200)

	set @return=''

	open curUPC
	fetch curUPC into @value
	while @@FETCH_STATUS = 0 
	begin
		set @return = @return + @value + ', ' --char(13) + char(10) 
		fetch curUPC into @value
	end

	If len(@return) > 0 
		Set @return =left(@return,len(@return)-1)

	close curUPC
	deallocate curUPC
	return @return
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetAlternateUPC] TO [IRMAReportsRole]
    AS [dbo];

