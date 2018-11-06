Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemChaining.DataAccess
    Public Class Settings
        Inherits WholeFoods.IRMA.ItemChaining.BusinessLogic.Settings

        Public Overrides Function ListZones() As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Return factory.GetStoredProcedureDataSet("GetZones")
        End Function
        Public Overrides Function ListStates() As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Return factory.GetStoredProcedureDataSet("GetStatesWithStores")
        End Function
        Public Overrides Function ListPriceTypes(ByVal IncludeReg As Boolean) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New DBParamList

            paramList.Add(New DBParam("IncludeReg", DBParamType.Bit, IncludeReg))

            Return factory.GetStoredProcedureDataSet("GetPriceTypes", paramList)
        End Function
    End Class
End Namespace