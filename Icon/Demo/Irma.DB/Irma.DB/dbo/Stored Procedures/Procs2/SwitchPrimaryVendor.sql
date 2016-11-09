CREATE PROCEDURE dbo.SwitchPrimaryVendor
@SourceVendorID int,
@TargetVendorID int,
@Item_Key int,
@Store_No int
AS
BEGIN
    SET NOCOUNT ON

    UPDATE StoreItemVendor
    SET PrimaryVendor = 1
    WHERE StoreItemVendorID in
		(SELECT target.StoreItemVendorID
			FROM 
				StoreItemVendor Source
			INNER JOIN
                StoreItemVendor Target
                    ON Target.Item_Key = Source.Item_Key and Target.Store_no = Source.Store_no
            WHERE 
				Source.Vendor_id = @SourceVendorID 
				and 
				Source.PrimaryVendor = 1
				and 
				(Source.DeleteDate IS null or Source.DeleteDate > getdate()) 
				and 
				Target.Vendor_Id = @TargetVendorID 
				and 
				Target.PrimaryVendor = 0
				and 
				Target.Item_key = isnull(@Item_Key, Target.Item_Key) 
				and 
				Target.Store_no = isnull(@Store_No, Target.Store_No))   
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SwitchPrimaryVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SwitchPrimaryVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SwitchPrimaryVendor] TO [IRMAReportsRole]
    AS [dbo];

