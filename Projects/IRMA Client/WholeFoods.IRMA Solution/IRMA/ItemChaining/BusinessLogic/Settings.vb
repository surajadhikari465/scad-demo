Namespace WholeFoods.IRMA.ItemChaining.BusinessLogic
    Public MustInherit Class Settings
        Inherits BusinessObject
        Public MustOverride Function ListZones() As DataSet
        Public MustOverride Function ListStates() As DataSet
        Public MustOverride Function ListPriceTypes(ByVal IncludeReg As Boolean) As DataSet
        Public Function CalcSomthing() As Boolean

        End Function
    End Class
End Namespace