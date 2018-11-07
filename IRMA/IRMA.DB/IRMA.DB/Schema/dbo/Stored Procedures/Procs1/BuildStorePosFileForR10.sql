
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2014-11-12
-- Description:	Populates the IConPOSPushPublish
--				table as part of the store build
--				process for bringing new stores
--				onboard to R10.
-- =============================================

CREATE PROCEDURE [dbo].[BuildStorePosFileForR10]
	@StoreNumber int
AS
BEGIN
	
	set nocount on;

	declare
		@RegionCode varchar(4) = (select RegionCode from Region),
		@UseRegionalScaleFile bit = (select FlagValue from InstanceDataFlags (nolock) where FlagKey = 'UseRegionalScaleFile'),
		@UseStoreJurisdictions bit = (select FlagValue from InstanceDataFlags (nolock) where FlagKey = 'UseStoreJurisdictions')
    
	create table #Publish
	(
		PriceBatchHeaderId		int,
		RegionCode				varchar(4),
		StoreNumber				int,
		ItemKey					int,
		Identifier				varchar(13),
		ChangeType				varchar(30),
		InsertDate				datetime,
		BusinessUnitId			int,
		RetailSize				decimal(9,4),
		RetailPackageUom		varchar(5),
		TmDiscountEligible		bit,
		CaseDiscount			bit,
		AgeCode					int,
		Recall					bit,
		RestrictedHours			bit,
		SoldByWeight			bit,
		ScaleForcedTare			bit,
		QuantityRequired		bit,
		PriceRequired			bit,
		QuantityProhibit		bit,
		VisualVerify			bit,
		RestrictSale			bit,
		Price					money,
		RetailUom				varchar(5),
		SalePrice				money,
		Multiple				int,
		SaleMultiple			int,
		SaleStartDate			smalldatetime,
		SaleEndDate				smalldatetime,
		InProcessBy				varchar(32),
		ProcessedDate			datetime2(7),
		ProcessingFailedDate	datetime2(7),
		LinkedIdentifier		varchar(13),
		PosTare					int
	)

	create table #ValidatedIdentifier
	(
		Identifier_ID			int,
		Item_Key				int,
		Identifier				varchar(13),
		Default_Identifier		tinyint,
		Deleted_Identifier		tinyint,
		Add_Identifier			tinyint,
		Remove_Identifier		tinyint,
		National_Identifier		tinyint,
		CheckDigit				char(1),
		IdentifierType			char(1),
		NumPluDigitsSentToScale	int,
		Scale_Identifier		bit
	)

	insert into
		#ValidatedIdentifier
	select
		Identifier_ID,
		Item_Key,
		Identifier,
		Default_Identifier,
		Deleted_Identifier,
		Add_Identifier,
		Remove_Identifier,
		National_Identifier,
		CheckDigit,
		IdentifierType,
		NumPluDigitsSentToScale,
		Scale_Identifier
	from 
		ItemIdentifier					ii	(nolock) 
		inner join ValidatedScanCode	vsc (nolock)	on ii.Identifier = vsc.ScanCode
														

	insert into
		#Publish
	select
		PriceBatchHeaderId		= 0,
		RegionCode				= @RegionCode,
		StoreNumber				= @StoreNumber,
		ItemKey					= i.Item_Key,
		Identifier				= vi.Identifier,
		ChangeType				= 'ScanCodeAdd',
		InsertDate				= getdate(),
		BusinessUnitId			= s.BusinessUnit_ID,
		RetailSize				= isnull(iov.Package_Desc2, i.Package_Desc2),
		RetailPackageUom		=	case 
										when isnull(isnull(iuv.Weight_Unit, isnull(ruo.Weight_Unit, ru.Weight_Unit)), 0) = 1 then 'LB'
										else 'EA'
									end,
		TmDiscountEligible		= p.Discountable,
		CaseDiscount			= p.IBM_Discount,
		AgeCode					= nullif(p.AgeCode, 0),
		Recall					= isnull(isnull(iov.Recall_Flag, i.Recall_Flag), 0),
		RestrictedHours			= p.Restricted_Hours,
		SoldByWeight			=	case 
										when isnull(isnull(iuv.Weight_Unit, isnull(ruo.Weight_Unit, ru.Weight_Unit)), 0) = 1 then 1
										else 0 
									end,
		ScaleForcedTare			= isnull(isnull(iso.ForceTare, isc.ForceTare), 0),
		QuantityRequired		= isnull(iov.Quantity_Required, i.Quantity_Required),
		PriceRequired			= isnull(iov.Price_Required, i.Price_Required),
		QuantityProhibit		= isnull(isnull(iov.QtyProhibit, i.QtyProhibit), 0),
		VisualVerify			= isnull(p.VisualVerify, 0),
		RestrictSale			= isnull(p.NotAuthorizedForSale, 0),
		Price					= round(p.Price, 2),
		RetailUom				= isnull(isnull(puo.Unit_Abbreviation, pu.Unit_Abbreviation), ''),
		SalePrice				= nullif(round(p.Sale_Price, 2), 0),
		Multiple				= p.Multiple,
		SaleMultiple			= p.Sale_Multiple,
		SaleStartDate			= p.Sale_Start_Date,
		SaleEndDate				= p.Sale_End_Date,
		InProcessBy				= null,
		ProcessedDate			= null,
		ProcessingFailedDate	= null,
		LinkedIdentifier		= li.Identifier,
		PosTare					= p.PosTare
		
	from
		#ValidatedIdentifier			vi

		join Price						p	(nolock)	on	vi.Item_Key = p.Item_Key
															and p.Store_No = @StoreNumber

		join Item						i	(nolock)	on	p.Item_Key = i.Item_Key

		join Store						s	(nolock)	on	p.Store_No = s.Store_No
															and s.Store_No = @StoreNumber

		join StoreItem					si	(nolock)	on	s.Store_No = si.Store_No
															and p.Item_Key = si.Item_Key
															and si.Authorized = 1

		join StoreItemVendor			siv	(nolock)	on	p.Store_No = siv.Store_No
															and i.Item_Key = siv.Item_Key
															and siv.PrimaryVendor = 1

		left join #ValidatedIdentifier	li				on	p.LinkedItem = li.Item_Key
															and li.Default_Identifier = 1

		left join ItemOverride			iov	(nolock)	on	p.Item_Key = iov.Item_Key
															and s.StoreJurisdictionID = iov.StoreJurisdictionID
															and @UseRegionalScaleFile = 0
															and @UseStoreJurisdictions = 1

		left join ItemScale				isc	(nolock)	on	i.Item_Key = isc.Item_Key

		left join ItemScaleOverride		iso (nolock)	on	p.Item_Key = iso.Item_Key
															and s.StoreJurisdictionID = iso.StoreJurisdictionID
															and @UseRegionalScaleFile = 0
															and @UseStoreJurisdictions = 1

		left join ItemUomOverride		iuo (nolock)	on	i.Item_Key = iuo.Item_Key
															and iuo.Store_No = @StoreNumber

		left join ItemUnit				ru	(nolock)	on	ru.Unit_ID = i.Retail_Unit_ID

		left join ItemUnit				ruo (nolock)	on  ruo.Unit_ID = iov.Retail_Unit_ID

		left join ItemUnit				pu	(nolock)	on  pu.Unit_ID = i.Package_Unit_ID

		left join ItemUnit				puo	(nolock)	on	puo.Unit_ID = iov.Package_Unit_ID

		left join ItemUnit				iuv (nolock)	on	iuv.Unit_ID = iuo.Retail_Unit_ID

	where
		i.Retail_Sale = 1
		and i.Remove_Item = 0
		and i.Deleted_Item = 0

	
	-- Clean up any mismatched sale information.  Sale fields should either all be populated or they should all be null.
	update 
		#Publish
	set	
		SalePrice = null,
		SaleStartDate = null,
		SaleEndDate = null,
		SaleMultiple = null
	where	
		SaleStartDate is null or
		SaleEndDate is null or
		SalePrice is null or
		SaleMultiple is null


	-- Check the price multiple configuration and perform the conversion if necessary.
	declare @ConvertMultiple bit = 
	(
		select 
			acv.Value
		from 
			AppConfigValue			acv (nolock)
			inner join AppConfigEnv ace (nolock)	on acv.EnvironmentID = ace.EnvironmentID 
			inner join AppConfigApp aca (nolock)	on acv.ApplicationID = aca.ApplicationID 
			inner join AppConfigKey ack (nolock)	on acv.KeyID = ack.KeyID 
		where 
			aca.Name = 'POS PUSH JOB' 
			and ack.Name = 'ConvertMultiple'
			and substring(ace.Name,1,1) = substring((select Environment from Version where ApplicationName = 'IRMA CLIENT'),1,1)
	)

	if (@ConvertMultiple = 1)
		begin
			
			update
				#Publish
			set
				Price			= case when Multiple > 1 then round((Price / Multiple),2,1) else Price end,
				Multiple		= case when Multiple > 1 then 1 else Multiple end,
				SaleMultiple	= case when SaleMultiple > 1 then 1 else SaleMultiple end,
				SalePrice		= case when SaleMultiple > 1 then round((SalePrice / SaleMultiple),2,1) else SalePrice end

		end


	-- Update duplicate ItemScale entries with the most current ForceTare value.
	declare @DuplicateRows int = 
	(
		select count(*) from 
		(
			select 
				StoreNumber, 
				ItemKey, 
				Identifier, 
				ChangeType, 
				InsertDate 
			from 
				#Publish
			group by 
				StoreNumber, ItemKey, Identifier, ChangeType, InsertDate
			having 
				count(ItemKey) > 1
		) DuplicateItems
	)

	if @DuplicateRows > 0
		begin
		
			merge 
				#Publish p2	
			using
				(
					select 
						MaxItemScale.*, 
						isc2.ForceTare 
					from 
						ItemScale isc2 (nolock) 
						join (
								select 
									max(isc.ItemScale_ID) as ItemScale_ID,
									DuplicateItems.Identifier, 
									isc.Item_Key
								from 
									ItemScale isc (nolock)
									join ( 
											select 
												StoreNumber, 
												ItemKey, 
												Identifier, 
												ChangeType, 
												InsertDate 
											from 
												#Publish p (nolock)
											group by 
												StoreNumber, ItemKey, Identifier, ChangeType, InsertDate
											having count(ItemKey) > 1
										) DuplicateItems
			
										on isc.Item_Key = DuplicateItems.ItemKey
								
								group by 
									isc.item_key, DuplicateItems.Identifier

							) MaxItemScale on isc2.ItemScale_ID = MaxItemScale.ItemScale_ID

				) CurrentForceTare 
				
			on p2.ItemKey = CurrentForceTare.Item_Key and p2.Identifier = CurrentForceTare.Identifier
			
			when matched then
				update set ScaleForcedTare = CurrentForceTare.ForceTare;
		end


	insert into IConPOSPushPublish select distinct * from #Publish

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BuildStorePosFileForR10] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BuildStorePosFileForR10] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BuildStorePosFileForR10] TO [IRSUser]
    AS [dbo];

