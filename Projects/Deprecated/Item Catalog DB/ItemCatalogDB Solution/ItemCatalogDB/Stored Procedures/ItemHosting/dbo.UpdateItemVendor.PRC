SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateItemVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateItemVendor]
GO


CREATE PROCEDURE dbo.UpdateItemVendor 
    @Item_Key int,
    @Vendor_ID int,
    @Item_ID varchar(255) 
AS
BEGIN
    SET NOCOUNT ON
    
    UPDATE ItemVendor
    SET Item_ID = @Item_ID 
    FROM ItemVendor (rowlock)
    WHERE Vendor_ID = @Vendor_Id AND Item_Key = @Item_Key 
        
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



