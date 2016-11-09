create PROCEDURE [app].[IconItemSubTeamExceptions]	
	@DbServerName varchar(255),
	@DbName varchar(255),
	@ReportName varchar(255)
As

Begin

	if(@ReportName <> 'SubTeam')
	begin
		return;
	end


	create table #IconTable 
	 (
		[ScanCode] [nvarchar](13) NOT NULL,
		[ProductDescription] [nvarchar] (255) NULL,
		[PosDeptNo] [int] NOT NULL,
		[SubTeamName] [nvarchar](255) NULL,
		[SubTeamNotAligned] [bit] NOT NULL
	 );
	
	create table #IrmaTable
	(
		ScanCode nvarchar(13),
		ProductDescription [nvarchar] (255) NULL,
		Default_Identifier [bit] NOT NULL,
		SubTeamName nvarchar(255),
		Dept_No nvarchar(10),
		AlignedSubTeam bit
	);

	
	--=======================================================
	-- Declare Variables
	--=======================================================
	declare @posDept int;
	declare @finId int;
	declare @merchFin int;	
	declare @merchId int;
	declare @productDescriptionTraitID int;
	declare @wholeFoodsLocale int;
	declare @nonAlignedSubTeam int;
	DECLARE @SQLString nvarchar(MAX);
	DECLARE @SQLStringDB nvarchar(MAX);
	DECLARE @SQLStringItem nvarchar(MAX);
	DECLARE @SQLStringWhere nvarchar(MAX);
	DECLARE @SQLStringIdentifier nvarchar(MAX);
	DECLARE @SQLStringSubteam nvarchar(MAX);
	
	set @DbServerName = '['+ @DbServerName + ']'
	set @DbName = '[' + @DbName + ']'
	set @SQLStringDB = @DbServerName + '.' + @DbName; 
	
	-- traits
	set @posDept = (select traitID from Trait where traitCode = 'PDN');
	set @merchFin = (select traitID from Trait where traitCode = 'MFM');
	set @nonAlignedSubTeam = (select traitID from Trait where traitCode = 'NAS');
	SET @productDescriptionTraitID	= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'PRD');
	
	-- Locale
	SET @wholeFoodsLocale = (SELECT l.localeID FROM Locale l WHERE l.localeName = 'Whole Foods');
	
	-- hierarchies
	set @finId = (select hierarchyID from Hierarchy where hierarchyName = 'Financial');
	set @merchId = (select hierarchyID from Hierarchy where hierarchyName = 'Merchandise');
	
	--=======================================================
	-- Setup CTEs
	--=======================================================
	WITH 
	ItemTrait_CTE (traitID, traitValue, itemID)
	AS
	(
		SELECT it.traitID, it.traitValue, it.itemID
		FROM ItemTrait it
		WHERE it.localeID = 1
	),
	
	MerchSubTeam_CTE (itemID, hierarchyClassID, hierarchyClassName, hierarchyID, subTeamName)
	AS
	(
		SELECT ihc.itemID, hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID, hct.traitValue
		FROM 
			ItemHierarchyClass			ihc
			JOIN HierarchyClass			hc	on	ihc.hierarchyClassID = hc.hierarchyClassID
			JOIN HierarchyClassTrait	hct on	hc.hierarchyClassID = hct.hierarchyClassID
											and hct.traitID = @merchFin
		WHERE
			hc.hierarchyID = @merchId
	),
	Fin_CTE (hierarchyClassID, hierarchyClassName, hierarchyID, posDeptNo)
	AS
	(
		SELECT hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID, hct.traitValue
		FROM 
			
			 HierarchyClass			hc	
			JOIN HierarchyClassTrait	hct on	hc.hierarchyClassID = hct.hierarchyClassID
											and hct.traitID = @posDept
		WHERE
			hc.hierarchyID = @finId
	),
	
	Fin_Event_CTE (hierarchyClassID, hierarchyClassName, hierarchyID, disableSubTeamEvents)
	AS
	(
		SELECT hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID, hct.traitValue
		FROM 
			
			 HierarchyClass			hc	
			JOIN HierarchyClassTrait	hct on	hc.hierarchyClassID = hct.hierarchyClassID
											and hct.traitID = @nonAlignedSubTeam
		WHERE
			hc.hierarchyID = @finId
	)
	
	--=======================================================
	-- Main
	--=======================================================
	insert into #IconTable
	SELECT
		distinct sc.scanCode				as ScanCode,
		pd.traitValue				as ProductDescription,
		cast(coalesce(nullif(fin.posDeptNo,''), '-1') AS int) as PosDeptNo,
		hc.hierarchyClassName as SubTeamName,	
		cast(coalesce(nullif(finEvent.disableSubTeamEvents,'0'), '0') AS bit) as SubTeamNotAligned
	FROM

		
		ScanCode		sc
		LEFT JOIN ItemTrait	pd		on	sc.itemID = pd.itemID
									and pd.traitID = @productDescriptionTraitID
									and pd.localeID = @wholeFoodsLocale
		JOIN MerchSubTeam_CTE	merchFin		on	sc.itemID = merchFin.itemID
		JOIN HierarchyClass hc on merchFin.subTeamName = hc.hierarchyClassName
		JOIN Fin_CTE fin on hc.hierarchyClassID = fin.hierarchyClassID
		LEFT JOIN Fin_Event_CTE finEvent on hc.hierarchyClassID = finEvent.hierarchyClassID


	--=============================================================
	-- Get IRMA Items
	--=============================================================
	
	SET @SQLString = 'Select  ii.Identifier as ScanCode, i.Item_Description, ii.Default_Identifier, st.SubTeam_Name SubTeamName, st.Dept_No as Dept_No, st.AlignedSubTeam as SubTeamAligned
	from ';
	Set @SQLStringWhere = ' 
	where i.Retail_Sale = 1 
	and i.Deleted_Item = 0 
	and i.Remove_Item = 0
	and ii.Deleted_Identifier = 0 
	and ii.Remove_Identifier = 0
	order by ii.Identifier'
	set @SQLStringItem = @SQLStringDB + '.dbo.Item i 
	JOIN '
	set @SQLStringIdentifier = @SQLStringDB + '.dbo.ItemIdentifier	ii on	i.Item_Key = ii.Item_Key
	join '
	set @SQLStringSubteam = @SQLStringDB + '.dbo.SubTeam st on	i.SubTeam_No = st.SubTeam_No '
	
	Set @SQLString = @SQLString + @SQLStringItem +   @SQLStringIdentifier + @SQLStringSubteam  + @SQLStringWhere
	
	Insert into #IrmaTable 		EXECUTE sp_executesql @SQLString
		
	--=============================================================
	-- Get Non-Alinged Items
	--=============================================================

	Select
		it.ScanCode as ScanCode, 
		it.ProductDescription as 'ICON Description',
		irt.Default_Identifier as DefaultIdentifier,
		it.subteamname as 'Icon SubTeam',
		case when it.SubTeamNotAligned = 0 then 'Yes'
			 else 'No'
		end as 'Icon SubTeam Aligned',
		irt.SubTeamName as 'IRMA SubTeam'
	from  #IconTable it 
	join #IrmaTable irt on it.ScanCode = irt.ScanCode 
	where (irt.Dept_No <> it.PosDeptNo and it.[SubTeamNotAligned] = 0)
		OR
		(it.[SubTeamNotAligned] = 1 and irt.AlignedSubTeam = 1)

	drop table #IconTable
	drop table #IrmaTable

END