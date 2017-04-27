Namespace WholeFoods.IRMA.ItemChaining.BusinessLogic
    Public MustInherit Class ItemChain
        Inherits BusinessObject
        Public ChainName As String = ""
        Public ChainedItems As ArrayList = New ArrayList
        Public MustOverride Function CreateChain() As Boolean
        Public MustOverride Function GetChain(ByVal withItems As Boolean) As Boolean
        Public MustOverride Function ListChains() As DataSet
        Public MustOverride Function ListItemChains(ByVal ItemID As String) As DataSet
        Public MustOverride Function DeleteChain() As Boolean
        Public MustOverride Function IsREGPriceDifference(ByVal ItemIDs As String, ByVal StoreIDs As String) As Boolean
        Public MustOverride Function ItemPriceListByItemAndStore(ByVal ItemIDs As String, ByVal StoreIDs As String) As DataSet
        Public Function CalcSomthing() As Boolean

        End Function
    End Class
End Namespace