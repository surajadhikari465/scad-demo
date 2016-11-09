create PROCEDURE [app].[GetPLURequestBySearchParams]
@requestStatus varchar(15) = null

As

BEGIN

if @requestStatus is not null
begin
	if LOWER (@requestStatus) = 'all'		
		set @requestStatus = null
	else
		set @requestStatus = LOWER (@requestStatus)
end


select p.* from app.[PLURequest] p
where  ((lower(p.[requestStatus]) = @requestStatus)
             OR @requestStatus is null)


end;


GO