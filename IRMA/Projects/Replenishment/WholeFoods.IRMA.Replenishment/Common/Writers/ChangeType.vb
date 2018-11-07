Namespace WholeFoods.IRMA.Replenishment.Common.Writers

    ''' <summary>
    ''' Set of change types that are handled by the POS push process.  Each change type is handled by it's own retrieve and apply
    ''' stored procedure.
    ''' </summary>
    ''' <remarks>This Enum should correspond to the POSChangeType.POSChangeTypeKey values from the database.</remarks>
    Public Enum ChangeType
        ItemIdDelete = 1
        ItemIdAdd = 2
        ItemDataDelete = 3
        ItemDataChange = 4
        PromoOffer = 5
        VendorIDAdd = 6
        CorpScaleItemIdAdd = 7
        CorpScaleItemIdDelete = 8
        CorpScaleItemChange = 9
        ZoneScaleItemDelete = 10
        ZoneScalePriceChange = 11
        shelfTagChange = 12
        NutriFact = 13
        ExtraText = 14
        ZoneScaleSmartXPriceChange = 15
        CorpScalePriceExceptions = 16           ' note: this is used as a value in the application, but it does not correspond to a db key
        ItemDataDeAuth = 17                     ' note: this is used as a value in the application, but it does not correspond to a db key
        ZoneScaleItemAuthPriceChange = 18       ' note: this is used as a value in the application, but it does not correspond to a db key
        ZoneScaleItemDeAuthPriceChange = 19     ' note: this is used as a value in the application, but it does not correspond to a db key
        ItemRefresh = 20
        ElectronicShelfTagChange = 21
    End Enum

    Public Enum IconChangeType
        ItemLocaleAttributeChange
        ScanCodeAdd
        ScanCodeDelete
        ScanCodeAuthorization
        ScanCodeDeauthorization
        RegularPriceChange
        NonRegularPriceChange
        CancelAllSales
    End Enum

End Namespace