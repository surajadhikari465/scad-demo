CREATE PROCEDURE dbo.GetWIMPExtract_ITEMDATA 
AS
BEGIN
    SET NOCOUNT ON
    
		SELECT 
			II.Identifier,
			I.SubTeam_No,
			IB.Brand_Name,
			I.Item_Description,
			I.Package_Desc2,
			IU.Unit_Abbreviation,	
			NIC.NatFamilyID,
			NIC.NatCatID,
			I.ClassID,
			'' AS miscalpha2,
			case 				
				when I.Deleted_Item = 0 Then (REPLACE(I.Deleted_Item, 0, 'N'))
				when I.Deleted_Item = 1 Then (REPLACE(I.Deleted_Item, 1,'Y'))
				end	as Deleted_Item,
			case 				
				when I.Discontinue_Item = 0 Then (REPLACE(I.Discontinue_Item, 0, 'N'))
				when I.Discontinue_Item = 1 Then (REPLACE(I.Discontinue_Item, 1,'Y'))
				end	as Discontinue_Item,
			case 				
				when I.Not_Available = 0 Then (REPLACE(I.Not_Available, 0, 'N'))
				when I.Not_Available = 1 Then (REPLACE(I.Not_Available, 1,'Y'))
				end	as Not_Available,
			I.Not_AvailableNote,
			case 				
				when I.Remove_Item = 0 Then (REPLACE(I.Remove_Item, 0, 'N'))
				when I.Remove_Item = 1 Then (REPLACE(I.Remove_Item, 1,'Y'))
				end	as Remove_Item,
			case 				
				when I.Retail_Sale = 0 Then (REPLACE(I.Retail_Sale, 0, 'N'))
				when I.Retail_Sale = 1 Then (REPLACE(I.Retail_Sale, 1,'Y'))
				end	as Retail_Sale,
			I.Product_Code
		FROM 
			Item I (NOLOCK)
		INNER JOIN
			ItemIdentifier II (NOLOCK) ON II.Item_Key = I.Item_Key 
		INNER JOIN
			ItemBrand IB (NOLOCK) ON I.Brand_ID = IB.Brand_ID
		INNER JOIN
			ItemUnit IU (NOLOCK) ON IU.Unit_ID = I.Package_Unit_ID
		INNER JOIN
			NatItemClass NICL (NOLOCK) ON NICL.ClassID = I.ClassID
		INNER JOIN
			NatItemCat NIC (NOLOCK) ON NIC.NatCatID = NICL.NatCatID
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWIMPExtract_ITEMDATA] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWIMPExtract_ITEMDATA] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWIMPExtract_ITEMDATA] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWIMPExtract_ITEMDATA] TO [IRMAReportsRole]
    AS [dbo];

