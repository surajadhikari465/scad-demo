
CREATE PROCEDURE [dbo].[IconItemUpdateItem]
	-- Add the parameters for the stored procedure here
	@ValidatedItemList dbo.IconUpdateItemType READONLY,
	@UserName varchar(25)
AS
BEGIN
	SET NOCOUNT ON;

/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK	Description
----------------------------------------------------------------------------------------------
MZ      2015-06-23  16200 (9290)    Updated the canonical colums on the ItemOverride table
                                    for alternate jurisdiction update.
CM		2016-01-13					Added Retail Size and Retail UOM
CM		2016-02-08					Added logic to include InstanceDataFlag to check whether or 
									not to update Retail Size and Retail UOM 
***********************************************************************************************/

	-- =====================================================
	-- Declare Variables
	-- =====================================================
	DECLARE @userId int;
	DECLARE @now datetime;
	DECLARe @distinctList dbo.IconUpdateItemType;
	DECLARE @subTeamAligned varchar(25);
	DECLARE @defaultNonAlignedPosDepNo int;
	DECLARE @uomIdLb int;
	DECLARE @uomIdEa int;
	DECLARE @updateRetailUomSize bit;
	
	SET @userId = (SELECT u.User_ID FROM Users u WHERE u.UserName = @UserName);
	SET @now = (SELECT GETDATE());
	set @defaultNonAlignedPosDepNo = 294;
	CREATE TABLE #tempCategory (Category_ID int, Category_Name varchar(35), SubTeam_No int, UserID int,  SubTeam_Type_ID int );
	set @subTeamAligned = 'SubTeam Aligned';
	SET @uomIdLb = (SELECT uom.Unit_ID FROM ItemUnit uom WHERE uom.Unit_Abbreviation = 'LB');
	SET @uomIdEa = (SELECT uom.Unit_ID FROM ItemUnit uom WHERE uom.Unit_Abbreviation = 'EA');
	SET @updateRetailUomSize = (SELECT FlagValue FROM InstanceDataFlags WHERE FlagKey = 'EnableIconRetailUomSizeUpdates');

	INSERT INTO @distinctList
	SELECT DISTINCT *
	FROM @ValidatedItemList vi
	-- =====================================================
	-- Update Item
	-- =====================================================
	BEGIN TRY
	SELECT DISTINCT vi.*, i.SubTeam_No as IrmaSubTeamNo, i.Category_ID as CategoryId INTO #subTeamDistinctList
		
		FROM @ValidatedItemList vi
		INNER JOIN ItemIdentifier ii on vi.ScanCode = ii.Identifier 
									AND ii.Deleted_Identifier = 0
									AND ii.Default_Identifier = 1
		INNER JOIN Item  i     on i.Item_Key = ii.Item_Key 
		                      and i.Retail_Sale = 1
		INNER JOIN SubTeam ist on ist.SubTeam_No = i.SubTeam_No
		 LEFT JOIN SubTeam st  on st.Dept_No = vi.DeptNo
		WHERE (st.SubTeam_No is NULL or ist.SubTeam_No  <> st.SubTeam_No) 


		--Add category if new sub team association does not exists..
		insert into ItemCategory ([Category_Name], [SubTeam_No], [User_ID])
		select distinct @subTeamAligned, st.SubTeam_No, @userId 
		from  #subTeamDistinctList		di 
		join SubTeam st on	di.DeptNo = st.Dept_No and  di.SubTeamNotAligned = 0
		where 
		NOT EXISTS (SELECT 1 
					FROM ItemCategory ic
					WHERE ic.[Category_Name] = @subTeamAligned
					AND ic.[SubTeam_No] = st.SubTeam_No)

		--Get item sub team updates
		UPDATE di
		SET
			di.IrmaSubTeamNo = st.SubTeam_No,
			di.CategoryId = ic.Category_ID
		FROM
			#subTeamDistinctList		di 	
			JOIN SubTeam		    st on	di.DeptNo = st.Dept_No
									   and  di.SubTeamNotAligned = 0
			LEFT JOIN ItemCategory  ic on	st.SubTeam_No = ic.SubTeam_No			
									   and  ic.Category_Name = @subTeamAligned

		UPDATE di
		SET
			di.IrmaSubTeamNo = st.SubTeam_No,
			di.CategoryId = ic.Category_ID
		FROM
			#subTeamDistinctList		di
			JOIN ItemIdentifier		ii on	ii.Identifier = di.ScanCode and di.SubTeamNotAligned = 1
			JOIN Item				i  on	i.Item_Key = ii.Item_Key
			JOIN SubTeam		oldSub on   i.SubTeam_No = oldSub.SubTeam_No and oldSub.AlignedSubTeam = 1
			JOIN SubTeam		    st on	st.Dept_No = @defaultNonAlignedPosDepNo
			LEFT JOIN ItemCategory  ic on	st.SubTeam_No = ic.SubTeam_No			
									and ic.Category_Name = @subTeamAligned

					

					
		-- =====================================================

		UPDATE i
		SET
			i.Item_Description	= vi.ProductDescription,
			i.POS_Description	= UPPER(vi.PosDescription),
			i.Package_Desc1		= CAST(vi.PackageUnit as decimal(9,4)),
			i.Food_Stamps		= CAST(vi.FoodStampEligible as bit),
			i.Brand_ID			= ib.Brand_ID,
			i.TaxClassID		= tc.TaxClassID,
			i.ClassID			= ni.ClassID,
			i.SubTeam_No = case when di.IrmaSubTeamNo is NULL then i.SubTeam_No ELSE di.IrmaSubTeamNo END,
			i.Category_ID = case when di.CategoryId is NULL then i.Category_ID ELSE di.CategoryId END,
			i.LastModifiedUser_ID = @userId,
			i.LastModifiedDate = @now,
			i.Package_Desc2 = case @updateRetailUomSize when 1 then vi.RetailSize else i.Package_Desc2 end,
			i.Package_Unit_ID = case @updateRetailUomSize when 1 then iu.Unit_ID else i.Package_Unit_ID end,
			i.Retail_Unit_ID = case @updateRetailUomSize when 1 then (case iu.Unit_ID when @uomIdLb then @uomIdLb ELSE @uomIdEa END) else i.Retail_Unit_ID end
		FROM
			Item i
			JOIN ItemIdentifier		ii on	i.Item_Key = ii.Item_Key
											AND ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1
			JOIN @distinctList		vi on	ii.Identifier = vi.ScanCode
			JOIN ValidatedBrand		vb on	vi.BrandId = vb.IconBrandId
			JOIN ItemBrand			ib on	vb.IrmaBrandId = ib.Brand_ID
			JOIN TaxClass			tc on	vi.TaxClassName = tc.TaxClassDesc or substring(vi.TaxClassName, 1, 7) = substring(tc.TaxClassDesc, 1, 7)
			JOIN NatItemClass		ni on   vi.NationalClassCode = ni.ClassID
			JOIN ItemUnit 			iu on   vi.RetailUom  = iu.Unit_Abbreviation
			LEFT JOIN #subTeamDistinctList di on vi.ScanCode = di.ScanCode

	-- =====================================================
	-- Update ItemOverride if exists for regions use alternate jurisdiction
	-- =====================================================
		UPDATE i
		SET
			i.Item_Description	  = vi.ProductDescription,
			i.POS_Description	  = UPPER(vi.PosDescription),
			i.Package_Desc1		  = CAST(vi.PackageUnit as decimal(9,4)),
			i.Food_Stamps		  = CAST(vi.FoodStampEligible as bit),
			i.Brand_ID			  = ib.Brand_ID,
			i.LastModifiedUser_ID = @userId
		FROM
			ItemOverride i
			JOIN ItemIdentifier		ii on	i.Item_Key = ii.Item_Key
											AND ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1
			JOIN @distinctList		vi on	ii.Identifier = vi.ScanCode
			JOIN ValidatedBrand		vb on	vi.BrandId = vb.IconBrandId
			JOIN ItemBrand			ib on	vb.IrmaBrandId = ib.Brand_ID 
			
		-- Update Price.PosTare if there is a value from Icon but there is no value already in the Price table
		-- Get Price rows that need an update
		SELECT
			p.Item_Key,
			p.Store_No,
			vi.Tare
		INTO #priceNeedsTare
		FROM 
			Price					p
			JOIN Item				i	on p.Item_Key = i.Item_Key
			JOIN ItemIdentifier		ii	on i.Item_Key = ii.Item_Key
										   AND ii.Deleted_Identifier = 0
										   AND ii.Default_Identifier = 1
			JOIN @ValidatedItemList	vi	on ii.Identifier = vi.ScanCode
		WHERE
			(p.PosTare IS NULL OR p.PosTare = 0)
			AND vi.Tare IS NOT NULL
			AND CAST(ROUND(CAST(vi.Tare as decimal(9,4)),0) as int) > 0

		

		-- Update Price rows
		UPDATE p
		SET
			p.PosTare = CAST(ROUND(CAST(pnt.Tare as decimal(9,4)),0) as int),
			p.LastScannedUserId_NonDTS = @userId
		FROM
			Price p
			JOIN #priceNeedsTare	pnt on p.Item_Key = pnt.Item_Key
										   AND p.Store_No = pnt.Store_No

	END TRY
	BEGIN CATCH
		DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
		RAISERROR ('IconItemUpdateItem failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
	END CATCH

END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IconItemUpdateItem] TO [IConInterface]
    AS [dbo];

