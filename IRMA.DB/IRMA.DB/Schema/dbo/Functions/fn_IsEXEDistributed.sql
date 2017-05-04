﻿CREATE FUNCTION dbo.fn_IsEXEDistributed 
	(@Item_Key int)
RETURNS bit
AS
BEGIN
    DECLARE @Cnt int, @Result bit

	SELECT @Cnt = COUNT(*)
    FROM Item
    INNER JOIN
        ZoneSubTeam 
        ON ZoneSubTeam.SubTeam_No = Item.SubTeam_No
    INNER JOIN
        Store 
        ON Store.Store_No = ZoneSubTeam.Supplier_Store_No
    LEFT JOIN
        WarehouseItemChange
        ON WarehouseItemChange.Item_Key = Item.Item_Key 
        AND WarehouseItemChange.Store_No = Store.Store_No
        AND ChangeType = 'A'
    WHERE Item.Item_Key = @Item_Key
        AND EXEDistributed = 1
        AND Store.Distribution_Center = 1
        AND Store.EXEWarehouse IS NOT NULL
        AND WarehouseItemChangeID IS NULL

    SELECT @Result = CASE WHEN @Cnt > 0 THEN 1 ELSE 0 END

    RETURN @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsEXEDistributed] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsEXEDistributed] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsEXEDistributed] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsEXEDistributed] TO [IRMAReportsRole]
    AS [dbo];

