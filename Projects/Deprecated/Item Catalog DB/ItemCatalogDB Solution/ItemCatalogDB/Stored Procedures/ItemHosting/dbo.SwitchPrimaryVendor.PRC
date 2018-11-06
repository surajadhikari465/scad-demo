SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SwitchPrimaryVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SwitchPrimaryVendor]
GO


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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


