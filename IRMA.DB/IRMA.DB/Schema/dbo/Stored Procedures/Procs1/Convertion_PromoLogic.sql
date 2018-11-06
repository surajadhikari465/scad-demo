CREATE   PROCEDURE [dbo].[Convertion_PromoLogic] 
@identifier varchar(13),
@vendorkey varchar(10),
@unitcost smallmoney,
@startdate smalldatetime,
@enddate smalldatetime
AS


begin

insert into item_cost_temp (identifier,vendor_key,unitcost,startdate,enddate)
	values(@identifier,@vendorkey,@unitcost,@startdate,@enddate)
	
end



--return 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Convertion_PromoLogic] TO [DataMigration]
    AS [dbo];

