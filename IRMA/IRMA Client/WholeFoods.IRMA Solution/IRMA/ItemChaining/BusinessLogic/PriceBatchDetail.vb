Namespace WholeFoods.IRMA.ItemChaining.BusinessLogic
    Public MustInherit Class PriceBatchDetail
        Inherits BusinessObject
        Public MustOverride Function IsBatchedPriceChange(ByVal ItemIDs As String, ByVal StoreIDs As String) As Boolean
        Public MustOverride Function IsPendingRegularPriceChange(ByVal ItemIDs As String, ByVal StoreIDs As String, ByVal [Date] As Date) As Boolean
        Public Function CalcSomthing() As Boolean

        End Function
    End Class
End Namespace