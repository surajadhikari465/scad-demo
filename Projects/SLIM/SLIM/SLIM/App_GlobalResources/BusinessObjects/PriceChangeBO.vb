Imports SLIM.WholeFoods.IRMA.Common.DataAccess

Public Enum PriceChangeStatus
    Valid
    Error_RegMultipleGreaterZero
    Error_RegPriceGreaterEqualZero
    Error_RegPriceGreaterZero
    Error_RegStartDateInPast
    Error_RegPriceChgTypeIDRequired
    Error_MissingPrimaryVendor
    Error_SaleMultipleGreaterZero
    Error_SalePriceGreaterEqualZero
    Error_SalePriceGreaterZero
    Error_SalePriceMustEqualZero
    Error_SaleStartAndEndDatesRequired
    Error_SaleStartDateInPast
    Error_SaleEndDateAfterSaleStartDate
    Error_SaleEndDateGreaterMaxDBDate
    Error_MSRPMultipleGreaterZero
    Error_MSRPPriceGreaterZero
    Error_PriceQuantityGreaterZero
    Error_SalePriceLimitGreaterZero
    Error_SaleWithPriceChangeInBatch
    Error_ExistingPromoWithSameStartDate
    Error_SalePriceChgTypeIDRequired
    Error_SalePricingMethodRequired
    Error_Unknown
    Warning_RegConflictsWithRegPriceChange
    Warning_RegConflictsWithSalePriceChange
    Warning_RegWithSaleCurrentlyOngoing
    Warning_RegWithPriceChangeInBatch
    Warning_SaleConflictsWithRegPriceChange
    Warning_SaleConflictsWithSalePriceChange
    Warning_SaleCurrentlyOngoing
End Enum


