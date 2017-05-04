SELECT 
	ii.Identifier [ScanCode],
	'"' + i.Item_Description + '"' [Product Description],
	'"' +i.POS_Description + '"' [POS Description],
	convert(int, i.Package_Desc1) [Item Pack],		
	case when i.Food_Stamps = 1 then 'true' else 'false' end [Food Stamp Eligible],
	'"Brand Name' + '|' + ib.Brand_Name + '"' [Brand],		
	SUBSTRING(tc.TaxClassDesc, 1, 7) [Tax],
	ni.ClassID [National-Association],	
	st.SubTeam_Name [Subteam],		
	i.Package_Desc2 [Retail Size],		
	iu.Unit_Abbreviation [UOM]
FROM
	Item i
	JOIN ItemIdentifier			ii	ON i.Item_Key = ii.Item_Key
											AND ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1
	JOIN ValidatedScanCode		vsc ON ii.Identifier = vsc.ScanCode
	JOIN ItemBrand				ib	ON i.Brand_ID = ib.Brand_ID
	JOIN ValidatedBrand			vb	ON ib.Brand_ID = vb.IrmaBrandId
	JOIN TaxClass				tc	ON i.TaxClassID = tc.TaxClassID
	JOIN NatItemClass			ni	ON i.ClassID = ni.ClassID
	JOIN SubTeam				st	ON i.SubTeam_No = st.SubTeam_No
	JOIN ItemUnit 				iu	ON i.Package_Unit_ID  = iu.Unit_ID
	JOIN ItemUnit				ru	ON i.Retail_Unit_ID = ru.Unit_ID
