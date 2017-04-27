if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetPriceBatchDetailForItemKeys') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dbo].[GetPriceBatchDetailForItemKeys] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [dbo].[GetPriceBatchDetailForItemKeys]
	@PriceBatchHeaderID int,
	@ItemList varchar(max),
	@ItemListSeparator char(1)
AS

BEGIN
    SET NOCOUNT ON

	declare @RegularPriceChangeTypeId int = (select PriceChgTypeID from PriceChgType where PriceChgTypeDesc = 'REG')

	SELECT
		[PriceBatchHeaderID] = PBH.PriceBatchHeaderID,				
		[PriceBatchDetailID] = PriceBatchDetailID,
		[Item_Key] = PBD.Item_Key, 
		[Identifier] = ISNULL(PBD.Identifier, II.Identifier),
		[Store_No] = PBD.Store_No,	
		[Business_Unit] = S.BusinessUnit_ID,
		[StartDate] = PBH.StartDate,
		[SaleStartDate] = PBD.StartDate,
		[ItemChgTypeID] = PBH.ItemChgTypeID, 
		[ItemChgTypeDesc] = ISNULL(RTRIM(ICT.ItemChgTypeDesc), 'Price'),
		[BatchDescription] = RTRIM(PBH.BatchDescription),
		[TprBatchHasPriceChange] =	case	
										when PBD.PriceChgTypeID is null then 0
										when PBD.PriceChgTypeID = @RegularPriceChangeTypeId then 0
										when p.Price <> PBD.Price then 1
										when p.Multiple <> PBD.Multiple then 1
										else 0
									end
	FROM
		PriceBatchDetail				PBD (nolock)
		INNER JOIN	PriceBatchHeader	PBH (nolock)	ON	PBD.PriceBatchHeaderID	= PBH.PriceBatchHeaderID
		INNER JOIN	Store S					(nolock)	ON	S.Store_No				= PBD.Store_No
		INNER JOIN	Item				i	(nolock)	ON	i.Item_Key				= PBD.Item_Key
		INNER JOIN	ItemIdentifier		II	(nolock)	ON	II.Item_Key				= PBD.Item_Key 
														AND II.Default_Identifier	= 1		
		INNER JOIN dbo.fn_Parse_List(@ItemList, @ItemListSeparator) IL
														ON IL.Key_Value				= II.Item_Key 
		LEFT JOIN ItemChgType ICT			(nolock)	ON ICT.ItemChgTypeID		= PBH.ItemChgTypeID		
		LEFT JOIN Price					P	(nolock)	ON P.Item_Key		=	 PBD.Item_Key	AND P.Store_No = PBD.Store_No
	WHERE 
		PBD.PriceBatchHeaderID = @PriceBatchHeaderID
END
GO
