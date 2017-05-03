CREATE PROCEDURE dbo.SetPrimaryVendor
@Store_No varchar(5000),
@Item_Key int,
@Vendor_Id int
AS 

	Declare @StoreLst table(Store_no int)

	insert into @StoreLst
	select Key_Value from dbo.FN_Parse_list(@Store_No,'|') StoreList

    -- Turn on the primary vendor for the selected vendor record
	-- Previous primary vendors will be turned off by trigger
	UPDATE StoreItemVendor
    SET PrimaryVendor = 1
    FROM StoreItemVendor
        INNER JOIN
            @StoreLst StoreList
            on StoreList.Store_No = StoreItemVendor.Store_no
    WHERE Vendor_ID = @Vendor_Id 
          AND Item_Key = @Item_Key 
		  AND PrimaryVendor = 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SetPrimaryVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SetPrimaryVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SetPrimaryVendor] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SetPrimaryVendor] TO [IRMASLIMRole]
    AS [dbo];

