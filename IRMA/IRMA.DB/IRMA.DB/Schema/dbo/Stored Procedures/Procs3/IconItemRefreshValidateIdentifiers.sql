
CREATE procedure [dbo].[IconItemRefreshValidateIdentifiers]
	@Identifiers varchar(max),
	@RefreshType varchar(50)
as
begin
	declare @itemRefreshResults table
	(
		Identifier nvarchar(13) not null,
		RefreshError nvarchar(40) null
	)

	insert into @itemRefreshResults(Identifier)
	select list.Key_Value
	from fn_ParseStringList(@Identifiers, '|') list

	set @RefreshType = lower(@RefreshType)

	-- All refresh types will check Identifier existence and deleted item/identifier status
	update @itemRefreshResults
	set RefreshError = 'Identifier does not exist.'
	where Identifier not in
		(select ii.Identifier
		 from ItemIdentifier ii)

	update @itemRefreshResults
		set RefreshError = 'Identifer or Item is deleted.'
		where RefreshError is null
			and Identifier not in
			(select ii.Identifier
			 from ItemIdentifier ii
			 join Item i on ii.Item_Key = i.Item_Key
			 where ii.Identifier = Identifier
				and (ii.Remove_Identifier = 0
					and ii.Deleted_Identifier = 0
					and i.Remove_Item = 0
					and i.Deleted_Item = 0))

	if (@RefreshType = 'pos' OR @RefreshType = 'icon')
	begin

		update @itemRefreshResults
		set RefreshError = 'Item is not Retail Sale.'
		where RefreshError is null
			and Identifier not in
			(select ii.Identifier
			 from ItemIdentifier ii
			 join Item i on ii.Item_Key = i.Item_Key
			 where ii.Identifier = Identifier
				and ((i.Retail_Sale = 1)
				 or ((CONVERT(FLOAT, Identifier) >= 46000000000 And CONVERT(FLOAT, Identifier)  <= 46999999999) Or (CONVERT(FLOAT, Identifier) >= 48000000000 And CONVERT(FLOAT, Identifier) <= 48999999999))))
	end

	if (@RefreshType = 'slaw')
	begin
		update ir
		set RefreshError = 'Item is not Validated.'
		from @itemRefreshResults ir
		where ir.RefreshError is null 
			and not exists (select 1 from ValidatedScanCode v where v.ScanCode = ir.Identifier)
	end

	select * from @itemRefreshResults where RefreshError is not null
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IconItemRefreshValidateIdentifiers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IconItemRefreshValidateIdentifiers] TO [IRSUser]
    AS [dbo];

