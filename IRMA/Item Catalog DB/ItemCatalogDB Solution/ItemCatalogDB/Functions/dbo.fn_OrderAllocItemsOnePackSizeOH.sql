IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_OrderAllocItemsOnePackSizeOH') 
    DROP FUNCTION dbo.fn_OrderAllocItemsOnePackSizeOH
GO

create function dbo.fn_OrderAllocItemsOnePackSizeOH ()
/*

    grant select on dbo.fn_OrderAllocItemsOnePackSizeOH to IRMAAdminRole
    grant select on dbo.fn_OrderAllocItemsOnePackSizeOH to IRMAClientRole
    grant select on dbo.fn_OrderAllocItemsOnePackSizeOH to IRMASchedJobsRole
    grant select on dbo.fn_OrderAllocItemsOnePackSizeOH to public

	select * from dbo.fn_OrderAllocItemsOnePackSizeOH()

*/
RETURNS @Table TABLE 
(
	Item_Key int, 
    PackSize Decimal
 )
AS
begin

	INSERT INTO @Table
    SELECT 
		tmpOrdersAllocateItems.Item_Key, 
		tmpOrdersAllocateItems.PackSize
	FROM tmpOrdersAllocateItems 
	INNER JOIN 
		(SELECT Item_Key
		FROM tmpOrdersAllocateItems
		WHERE BOH > 0
		GROUP BY Item_Key
		HAVING COUNT(*) = 1
		)  AS OOH 
	ON OOH.Item_Key = tmpOrdersAllocateItems.Item_Key
	WHERE BOH > 0

	return 
end
go

    grant select on dbo.fn_OrderAllocItemsOnePackSizeOH to IRMAAdminRole
    grant select on dbo.fn_OrderAllocItemsOnePackSizeOH to IRMAClientRole
    grant select on dbo.fn_OrderAllocItemsOnePackSizeOH to IRMASchedJobsRole
    grant select on dbo.fn_OrderAllocItemsOnePackSizeOH to public
