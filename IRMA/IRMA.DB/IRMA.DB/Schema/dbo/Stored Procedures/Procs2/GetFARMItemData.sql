CREATE PROCEDURE [dbo].[GetFARMItemData]
AS
BEGIN
    SET NOCOUNT ON

	select
		substring('000000000000',1,12-len(substring(II.Identifier,1,12))) + II.Identifier as Upc12
		, I.SubTeam_No, I.Item_Description, I.Package_Desc2, IU.Unit_Abbreviation, SJ.StoreJurisdictionDesc
		, I.POS_Description
		, IB.Brand_Name
		, I.ClassID
		, TC.TaxClassDesc
		from Item I 
		inner join ItemIdentifier II on I.Item_Key = II.Item_Key
		inner join ItemUnit IU on I.Package_Unit_ID = IU.Unit_ID
		inner join StoreJurisdiction SJ on I.StoreJurisdictionID = SJ.StoreJurisdictionID
		left outer join ItemBrand IB on I.Brand_ID = IB.Brand_ID
		left outer join TaxClass TC on I.TaxClassID = TC.TaxClassID
		where I.Deleted_Item = 0
		union
		select 
		substring('000000000000',1,12-len(substring(II.Identifier,1,12))) + II.Identifier as Upc12
		, I.SubTeam_No, I.Item_Description, O.Package_Desc2, IU.Unit_Abbreviation, SJ.StoreJurisdictionDesc
		, O.POS_Description
		, IB.Brand_Name
		, I.ClassID
		, TC.TaxClassDesc
		from Item I
		inner join ItemOverride O on I.Item_Key = O.Item_Key
		inner join ItemIdentifier II on I.Item_Key = II.Item_Key
		inner join ItemUnit IU on O.Package_Unit_ID = IU.Unit_ID
		inner join StoreJurisdiction SJ on O.StoreJurisdictionID = SJ.StoreJurisdictionID
		left outer join ItemBrand IB on I.Brand_ID = IB.Brand_ID
		left outer join TaxClass TC on I.TaxClassID = TC.TaxClassID
		where I.Deleted_Item = 0
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFARMItemData] TO [IRMA_Farm_Role]
    AS [dbo];

