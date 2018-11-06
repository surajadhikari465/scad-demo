/****** Object:  StoredProcedure [dbo].[Reporting_SpecialsInProgress]    Script Date: 11/08/2006 15:30:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reporting_SpecialsInProgress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Reporting_SpecialsInProgress]
GO
/****** Object:  StoredProcedure [dbo].[Reporting_SpecialsInProgress]    Script Date: 11/08/2006 15:30:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.[Reporting_SpecialsInProgress]
	@Store_No int,
	@SubTeam_No int
AS

SET NOCOUNT ON

declare @NOW datetime
	
select @NOW = (getdate())

SELECT ItemIdentifier.Identifier, 
	Item.Item_Description, 
	Price.Multiple, 
	Price.POSPrice AS Price, 
	Price.Sale_Start_Date, 
	Price.Sale_End_Date, 
	SubTeam.SubTeam_Name,
	Price.POSSale_Price AS Sale_Price, 
	Price.Sale_Multiple,
	Price.Store_No,
	Store.Store_Name 
FROM Item (nolock) 
	INNER JOIN ItemIdentifier (nolock) 
        ON (ItemIdentifier.Item_Key = Item.Item_Key 
            AND ItemIdentifier.Default_Identifier = 1)
	INNER JOIN Price (nolock) 
        ON (Item.Item_Key = Price.Item_Key) 
	INNER JOIN SubTeam (nolock) 
        ON (Item.Subteam_No = SubTeam.SubTeam_No)
	INNER JOIN Store (nolock) 
        ON (Price.Store_No = Store.Store_No)
WHERE 
    Price.Sale_Start_Date <= @NOW 
    AND Price.Sale_End_Date >= @NOW
    AND Price.Store_No = @Store_No 
    AND Item.SubTeam_No = ISNULL(@SubTeam_No, Item.SubTeam_No) 
    
SET NOCOUNT OFF
GO
