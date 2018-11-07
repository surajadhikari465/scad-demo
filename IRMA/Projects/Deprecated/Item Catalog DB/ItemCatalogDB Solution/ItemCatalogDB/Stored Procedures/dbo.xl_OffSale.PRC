SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[xl_OffSale]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[xl_OffSale]
GO


CREATE PROCEDURE dbo.xl_OffSale
    @Store_No INT,
    @Subteam_No INT,
    @TargetDate smalldatetime
AS 
BEGIN
    SET NOCOUNT ON


    SELECT ItemIdentifier.Identifier, Item.Item_Description 
    FROM price (nolock)
        INNER JOIN
            Item            
            ON item.Item_key = Price.Item_key
        INNER JOIN 
            ItemIdentifier
            ON ItemIdentifier.Item_key = Item.Item_key
    WHERE dbo.fn_OnSale(PriceChgTypeId) = 1 and Sale_end_date = @TargetDate
          and Item.SubTeam_No = isnull(@SubTeam_No, Item.SubTeam_No)
          and Price.Store_No = isnull(@Store_no, Price.Store_No)



    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



