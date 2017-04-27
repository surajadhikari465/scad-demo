

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].IdentifyItem') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].IdentifyItem
GO

CREATE PROCEDURE dbo.IdentifyItem
    @Identifier varchar(255) OUTPUT,
    @Item_ID varchar(255),
    @PS_Vendor_ID int,
    @Vendor_ID int,
    @Item_Key int OUTPUT,
    @SubTeam_Name varchar(255) OUTPUT,
    @SubTeam_No int OUTPUT
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @Item TABLE (Item_Key int, Identifier varchar(13))
    DECLARE @CurrDate datetime


    if @Vendor_ID is null
      begin
        SELECT @Vendor_ID = Vendor_ID from vendor where CONVERT(bigint,PS_Vendor_ID) = @PS_Vendor_ID
      end

    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))    

    INSERT INTO @Item
    SELECT II.Item_Key, II.Identifier
    FROM ItemIdentifier II (NOLOCK)
    INNER JOIN Item (NOLOCK) ON Item.Item_Key = II.Item_Key
    INNER JOIN ItemVendor (NOLOCK) ON ItemVendor.Item_Key = Item.Item_Key AND Vendor_ID = @Vendor_ID
    WHERE Deleted_Item = 0 AND (Identifier = @Identifier or itemVendor.Item_ID = @Item_ID) and 1 = case when @Identifier is null 
                                                                                                          then case when II.Default_Identifier = 1
                                                                                                                       then 1
                                                                                                                       else 0
                                                                                                                    end
                                                                                                          else 1
                                                                                                         end 
                                                                                                                        
    AND @CurrDate < ISNULL(DeleteDate, DATEADD(day, 1, @CurrDate))
    

    SELECT @Identifier = I.Identifier, @Item_Key = Item.Item_Key, @SubTeam_Name = SubTeam.SubTeam_Name, @SubTeam_No = Item.SubTeam_No
    FROM Item (NOLOCK)
    INNER JOIN @Item I ON I.Item_Key = Item.Item_Key
    INNER JOIN SubTeam (NOLOCK) ON SubTeam.SubTeam_No = Item.SubTeam_No

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


