-- =============================================
-- Author:		Benjamin Sims
-- Create date: 2015-01-14
-- Description:	Called from the POS Push Controller
--				to get a list of ItemPrice records.
--				This is ultimately for the sake of
--				determining if a UOM changed compared
--				to the IRMAPush item.
-- =============================================

CREATE PROCEDURE [app].[GetItemPrices]
	@IrmaPushRows [app].[IrmaPushType] readonly
AS
BEGIN

	DECLARE @businessUnitTraitId int = (SELECT traitID FROM Trait WHERE traitCode = 'BU')

	SELECT
		0						as IrmaPushId, -- defaulting to zero since we don't have the Id from the @IrmaPushRows table type
		sc.itemID				as ItemId,
		bu.localeID				as LocaleId,
		ip.itemPriceTypeID		as ItemPriceTypeId,
		ip.uomID				as UomId,
		ip.currencyTypeId		as CurrencyTypeId,
		ip.itemPriceAmt			as ItemPriceAmount,
		ip.breakPointStartQty	as BreakPointStartQuantity,
		ip.startDate			as StartDate,
		ip.endDate				as EndDate
	FROM
		@IrmaPushRows			pu
		JOIN ScanCode			sc	on	pu.Identifier		= sc.scanCode
		LEFT JOIN LocaleTrait	bu	on	pu.businessUnitId	= bu.traitValue
										AND bu.traitID		= @businessUnitTraitId
		INNER JOIN ItemPrice	ip	on	sc.itemID			= ip.itemID
										AND bu.localeID		= ip.localeID

END
