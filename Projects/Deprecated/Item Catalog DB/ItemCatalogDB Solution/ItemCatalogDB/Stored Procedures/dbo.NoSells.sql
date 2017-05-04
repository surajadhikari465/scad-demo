SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[NoSells]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[NoSells]
GO


CREATE PROCEDURE dbo.NoSells
	@Team_No int,
	@SubTeam_No int,
	@Store_No int,
	@Zone_Id int,
	@StartDate datetime,
	@EndDate datetime
AS

-- **************************************************************************
-- Procedure: GetStoreItemSearch()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from ItemCatalogLib project within IRMA Client solution
--
-- Modification History:
-- Date        Init		TFS		Comment
-- 01/14/2013  BAS		8755	Update i.Discontinue_Item reference to dbo.fn_GetDiscontinueStatus
--								to account for schema change
-- **************************************************************************

SELECT
	Item.Item_Key,
	Item_Description,
	Identifier 
FROM 
	Item (NOLOCK)
	INNER JOIN SubTeam (NOLOCK) ON (Item.SubTeam_No = SubTeam.SubTeam_No)
	INNER JOIN ItemIdentifier (NOLOCK) ON (	ItemIdentifier.Item_key = Item.Item_Key
											AND ItemIdentifier.Default_Identifier = 1) 
WHERE
	dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, NULL)	= 0
	AND ISNULL(@Team_No, Subteam.Team_No)					= Subteam.Team_No
	AND ISNULL(@SubTeam_No, SubTeam.SubTeam_No)				= SubTeam.SubTeam_No AND Sales_Account IS NULL 
    AND Not (Item.Item_Key IN (SELECT DISTINCT
									Item_Key 
                                FROM
									Store (NOLOCK)INNER JOIN (Sales_SumByItem (NOLOCK) INNER JOIN SubTeam (NOLOCK) ON (Sales_SumByItem.SubTeam_No = SubTeam.SubTeam_No) 
									) ON Store.Store_No = Sales_SumByItem.Store_No                                
                                WHERE
									Sales_Quantity								> 0
									AND Date_Key								>= @StartDate 
									AND Date_Key								<= @EndDate 
									AND ISNULL(@Team_No, Subteam.Team_No)		= Subteam.Team_No
									AND ISNULL(@SubTeam_No, SubTeam.SubTeam_No) = SubTeam.SubTeam_No
									AND ISNULL(@Store_No, Store.Store_No)		= Store.Store_No
									AND ISNULL(@Zone_Id, Store.Zone_Id)			= Store.Zone_Id)
								)


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


