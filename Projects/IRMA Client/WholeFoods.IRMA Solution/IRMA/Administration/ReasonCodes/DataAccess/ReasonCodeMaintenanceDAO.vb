Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Administration.ReasonCodes.BusinessLogic

Namespace WholeFoods.IRMA.Administration.ReasonCodes.DataAccess

    Public Class ReasonCodeMaintenanceDAO

        Public Function getReasonCodeType() As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("ReasonCodes_GetTypes")

                ' Process the result record
            Catch e As DataFactoryException
                Logger.LogError("Exception: ", Nothing, e)
                'send message about exception
                ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Warning, e)
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Dispose()
                End If
            End Try

            Return results
        End Function

        Public Function getReasonCodeDetail() As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("ReasonCodes_GetDetails")

                ' Process the result record
            Catch e As DataFactoryException
                Logger.LogError("Exception: ", Nothing, e)
                'send message about exception
                ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Warning, e)
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Dispose()
                End If
            End Try

            Return results
        End Function

        Public Function getReasonCodeDetailsForType(ByVal ReasonCodeTypeAbbr As String) As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing

            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@ReasonCodeTypeAbbr"
            currentParam.Value = ReasonCodeTypeAbbr
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("ReasonCodes_GetDetailsForType", paramList)

            Catch e As DataFactoryException
                Logger.LogError("Exception: ", Nothing, e)
                'send message about exception
                ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Warning, e)
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Dispose()
                End If
            End Try

            Return results
        End Function

        Public Function checkIfReasonCodeDetailExists(ByVal ReasonCode As String) As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing

            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@ReasonCode"
            currentParam.Value = ReasonCode
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("ReasonCodes_CheckIfDetailExists", paramList)

            Catch e As DataFactoryException
                Logger.LogError("Exception: ", Nothing, e)
                'send message about exception
                ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Warning, e)
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Dispose()
                End If
            End Try

            Return results
        End Function

        Public Function checkIfReasonCodeTypeExists(ByVal ReasonCodeTypeAbbr As String) As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing

            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@ReasonCodeTypeAbbr"
            currentParam.Value = ReasonCodeTypeAbbr
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("ReasonCodes_CheckIfTypeExists", paramList)

            Catch e As DataFactoryException
                Logger.LogError("Exception: ", Nothing, e)
                'send message about exception
                ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Warning, e)
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Dispose()
                End If
            End Try

            Return results
        End Function

        Public Sub createReasonCodeType(ByVal ReasonCodeTypeAbbr As String, ByVal ReasonCodeTypeDesc As String)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@ReasonCodeTypeAbbr"
            currentParam.Value = ReasonCodeTypeAbbr
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ReasonCodeTypeDesc"
            currentParam.Value = ReasonCodeTypeDesc
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("ReasonCodes_CreateType", paramList)

        End Sub

        Public Sub createReasonCodeDetail(ByVal ReasonCode As String, ByVal ReasonCodeDesc As String, ByVal ReasonCodeExtDesc As String)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@ReasonCode"
            currentParam.Value = ReasonCode
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ReasonCodeDesc"
            currentParam.Value = ReasonCodeDesc
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ReasonCodeExtDesc"
            currentParam.Value = ReasonCodeExtDesc
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("ReasonCodes_CreateDetail", paramList)

        End Sub

        Public Sub updateReasonCodeDetail(ByVal ReasonCode As String, ByVal ReasonCodeDesc As String, ByVal ReasonCodeExtDesc As String)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@ReasonCode"
            currentParam.Value = ReasonCode
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ReasonCodeDesc"
            currentParam.Value = ReasonCodeDesc
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ReasonCodeExtDesc"
            currentParam.Value = ReasonCodeExtDesc
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("ReasonCodes_UpdateDetails", paramList)

        End Sub

        Public Sub addReasonCodeMapping(ByVal ReasonCodeTypeAbbr As String, ByVal ReasonCode As String)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@ReasonCodeTypeAbbr"
            currentParam.Value = ReasonCodeTypeAbbr
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ReasonCode"
            currentParam.Value = ReasonCode
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("ReasonCodes_AddMapping", paramList)

        End Sub

        Public Sub disableReasonCodeMapping(ByVal ReasonCodeTypeAbbr As String, ByVal ReasonCode As String)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@ReasonCodeTypeAbbr"
            currentParam.Value = ReasonCodeTypeAbbr
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ReasonCode"
            currentParam.Value = ReasonCode
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("ReasonCodes_DisableMapping", paramList)

        End Sub
    End Class

End Namespace

