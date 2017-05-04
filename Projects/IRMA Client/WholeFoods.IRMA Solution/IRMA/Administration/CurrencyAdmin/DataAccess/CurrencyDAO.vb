Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Public Class CurrencyDAO
    Public Shared Sub AddNewCurrency(ByVal newCurr As Currency)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        ' setup parameters for stored proc
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "CurrencyCode"
        currentParam.Value = newCurr.Code
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "CurrencyName"
        currentParam.Value = newCurr.Description
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "CurrencyID"
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        factory.ExecuteStoredProcedure("InsertCurrency", paramList)

    End Sub
    Public Shared Sub UpdateCurrency(ByVal upCurr As Currency)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        ' setup parameters for stored proc
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "CurrencyCode"
        currentParam.Value = upCurr.Code
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "CurrencyName"
        currentParam.Value = upCurr.Description
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "CurrencyID"
        currentParam.Value = upCurr.ID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        factory.ExecuteStoredProcedure("UpdateCurrency", paramList)

    End Sub
    Public Shared Function GetCurrencyList() As DataTable
        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        Return factory.GetStoredProcedureDataTable("GetCurrencies")
    End Function

    Public Shared Sub DeleteCurrency(ByVal delCurr As Currency)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        ' setup parameters for stored proc
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "CurrencyID"
        currentParam.Value = delCurr.ID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        factory.ExecuteStoredProcedure("DeleteCurrency", paramList)

    End Sub
End Class
