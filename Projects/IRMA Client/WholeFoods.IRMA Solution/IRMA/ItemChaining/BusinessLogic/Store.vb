Namespace WholeFoods.IRMA.ItemChaining.BusinessLogic
    Public MustInherit Class Store
        Inherits BusinessObject
        Public StoreName As String = ""
        Public MustOverride Function ListStores() As DataSet
        Public MustOverride Function ListStores(ByVal Zone As String) As DataSet
        Public MustOverride Function ListStoresByState(ByVal State As String) As DataSet
        Public Function CalcSomthing() As Boolean

        End Function
    End Class
End Namespace