Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemChaining.DataAccess
    Public Class Store
        Inherits WholeFoods.IRMA.ItemChaining.BusinessLogic.Store

        Public Overrides Function ListStores() As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Return factory.GetStoredProcedureDataSet("GetStores")
        End Function
        Public Overrides Function ListStores(ByVal Zone As String) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New DBParamList

            paramList.Add(New DBParam("Zone_Name", DBParamType.String, Zone))

            Return factory.GetStoredProcedureDataSet("GetStoresByZoneName", paramList)
        End Function
        Public Overrides Function ListStoresByState(ByVal State As String) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New DBParamList

            paramList.Add(New DBParam("State", DBParamType.String, State))

            Return factory.GetStoredProcedureDataSet("GetStoresByState", paramList)
        End Function
    End Class
End Namespace