SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [dbo].[GetItemScanData]    Script Date: 07/07/2011 17:03:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetItemScanData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetItemScanData]
GO

/****** Object:  StoredProcedure [dbo].[GetItemScanData]    Script Date: 07/07/2011 17:03:15 ******/

-- =============================================
-- Author:		Alex Z
-- Create date: 05/01/2011 - May Day
-- Description:	SP returns data for one item used by scan audit tool
-- =============================================
CREATE PROCEDURE [dbo].[GetItemScanData] 
	-- Add the parameters for the stored procedure here
	@Identifier varchar(25),
	@Store_no int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT top 1 i.Item_Key, ii.Identifier, i.POS_Description, s.SubTeam_Name, b.Brand_Name, 
	i.Package_Desc2 as ItemSize, iu.Unit_Abbreviation as UOM, i.Package_Desc1 as PackSize, 
	 pct.PriceChgTypeDesc, 
	 p.POSSale_Price as SalePrice,
	p.POSPrice as Price,
	p.Multiple,
	 Case pct.On_Sale 
	when 1 then
	p.Sale_Start_Date else NULL end as SaleStart, 
	Case pct.On_Sale
	when 1 then
	p.Sale_End_Date else NULL end as SaleEnd, 
	 si.Authorized from Item i (NOLOCK)
	inner join itemidentifier ii (NOLOCK)
	on ii.Item_Key = i.Item_Key and ii.default_identifier = 1
	inner join price p (NOLOCK)
	on p.Item_Key = i.Item_key and p.store_no = @Store_no
	inner join storeitem si (NOLOCK)
	on si.item_key = i.item_key and si.store_no = p.store_no
	inner join SubTeam s (NOLOCK)
	on s.subteam_no = i.subteam_no
	inner join ItemBrand b (NOLOCK)
	on b.Brand_ID = i.Brand_ID
	inner join PriceChgType pct (NOLOCK)
	on pct.PriceChgTypeId = p.PriceChgTypeID
	inner join ItemUnit iu (NOLOCK)
	on iu.Unit_ID = i.Package_Unit_ID
	where ii.identifier = @identifier and i.Deleted_Item = 0 and i.Remove_Item = 0
	and ii.Deleted_Identifier = 0
	
	 SET NOCOUNT OFF;
END

 

GO


