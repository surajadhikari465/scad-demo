Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic.CompetitorStoreDataSet

Namespace WholeFoods.IRMA.CompetitorStore.BusinessLogic
    Public Interface ICompetitorPriceRow
        Property FiscalWeekRowParent() As FiscalWeekRow
        ReadOnly Property IsExistingRow() As Boolean
        ReadOnly Property RowState() As Data.DataRowState
        Function IsItem_KeyNull() As Boolean
        Function IsCompetitorIDNull() As Boolean
        Function IsCompetitorNull() As Boolean
        Function IsCompetitorLocationIDNull() As Boolean
        Function IsCompetitorLocationNull() As Boolean
        Function IsCompetitorStoreIDNull() As Boolean
        Function IsCompetitorStoreNull() As Boolean
        Function IsFiscalYearNull() As Boolean
        Function IsFiscalPeriodNull() As Boolean
        Function IsPeriodWeekNull() As Boolean
        Function IsPriceMultipleNull() As Boolean
        Function IsPriceNull() As Boolean
        Function IsSizeNull() As Boolean
        Sub SetColumnError(ByVal columnName As String, ByVal errorText As String)
    End Interface
End Namespace
