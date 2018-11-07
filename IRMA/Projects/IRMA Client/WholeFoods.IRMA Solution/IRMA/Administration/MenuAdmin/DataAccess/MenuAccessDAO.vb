Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Public Class MenuAccessDAO
    Public Shared Function GetMenuAccessRecords() As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList

        ' Execute the stored procedure 
        Return factory.GetStoredProcedureDataSet("Administration_MenuAccess_GetMenuAccessRecords", paramList)
    End Function
    Public Shared Function UpdateMenuAccessRecord() As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList

        ' Execute the stored procedure 
        Return factory.GetStoredProcedureDataSet("Administration_MenuAccess_UpdateMenuAccessRecord", paramList)
    End Function
    Public Shared Function GetMenuNamesList() As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList

        ' Execute the stored procedure 
        Return factory.GetStoredProcedureDataSet("Administration_MenuAccess_GetMenuNamesList", paramList)
    End Function

    Public Shared Sub UpdateMenuItem(ByVal menuAccessMethod As MenuAccessBO, ByVal blnAll As Boolean, ByVal blnAllValue As Boolean)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "MenuAccessID"
            currentParam.Value = menuAccessMethod.MenuAccessKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UpdateAll"
            currentParam.Value = blnAll
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "AllValue"
            currentParam.Value = blnAllValue
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("Administration_MenuAccess_UpdateMenuAccessRecord", paramList)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub DeleteMenuItem(ByVal iMenuAccessId As Integer)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "MenuAccessID"
            currentParam.Value = iMenuAccessId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("Administration_MenuAccess_DeleteMenuAccessRecord", paramList)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub AddMenuItem(ByVal sMenuName As String, ByVal blnIsVisible As Boolean)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "MenuName"
            currentParam.Value = sMenuName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IsVisible"
            currentParam.Value = blnIsVisible
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("Administration_MenuAccess_AddMenuAccessRecord", paramList)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
