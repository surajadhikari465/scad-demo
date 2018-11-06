SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[xl_OffSaleSubteams]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[xl_OffSaleSubteams]
GO


CREATE PROCEDURE dbo.xl_OffSaleSubteams
    @Store_No INT,
    @Subteam_No INT,
    @TargetDate smalldatetime
AS 
BEGIN
    SET NOCOUNT ON


    SELECT distinct item.Subteam_No as SubTeamNo, SubTeam.SubTeam_Name as SubTeamName
    FROM price (nolock)
        INNER JOIN
            Item            
            ON item.Item_key = Price.Item_key
        Inner join 
            SubTeam
            on Item.SubTeam_no = SubTeam.Subteam_no
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


