Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Administration.EInvoicing.BusinessLogic

Namespace WholeFoods.IRMA.Administration.EInvoicing.DataAccess
    Public Class EinvoicingConfigDAO


        Public Function getsacTypesDataTable() As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("EInvoicing_getSACTypes")

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

        Public Function loadEInvoicingErrorCodes() As ArrayList

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing
            Dim list As ArrayList = New ArrayList
            Dim errorCode As EinvoicingErrorCodeBO = Nothing

            results = factory.GetStoredProcedureDataTable("EInvoicing_GetErrorCodes")

            For Each dr As DataRow In results.Rows
                errorCode = New EinvoicingErrorCodeBO(dr("errorcode_id"), dr("errormessage").ToString(), dr("description").ToString())
                list.Add(errorCode)
                errorCode.Dispose()
            Next

            results.Dispose()
            Return list

        End Function
        Public Sub UpdateConfigValue(ByVal _configvalue As EInvoicingConfigBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@ElementName"
            currentParam.Value = _configvalue.ElementName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@SACType"
            currentParam.Value = IIf(_configvalue.SACType = -1, DBNull.Value, _configvalue.SACType)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "@IsSAC"
            currentParam.Value = IIf(_configvalue.IsSAC, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@SubTeam_No"
            currentParam.Value = IIf(_configvalue.SubTeam_NO = -1, DBNull.Value, _configvalue.SubTeam_NO)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@IsAllowance"
            currentParam.Value = IIf(_configvalue.IsAllowance, -1, 1)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "@IsDisabled"
            If _configvalue.Disabled Then
                currentParam.Value = 1
            Else
                currentParam.Value = 0
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "@Label"
            currentParam.Value = _configvalue.Label
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@IsHeaderElement"
            If _configvalue.IsHeaderElement Then
                currentParam.Value = 1
            Else
                currentParam.Value = 0
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@IsItemElement"
            If _configvalue.IsItemElement Then
                currentParam.Value = 1
            Else
                currentParam.Value = 0
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("EInvoicing_UpdateConfigValue", paramList)


        End Sub

        Public Sub RemoveConfigvalue(ByVal ElementName As String)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)


            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@ElementName"
            currentParam.Value = ElementName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("EInvoicing_RemoveConfigValue", paramList)


        End Sub
        Public Sub InsertConfigValue(ByVal _configvalue As EInvoicingConfigBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@ElementName"
            currentParam.Value = _configvalue.ElementName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@SACType"
            currentParam.Value = _configvalue.SACType
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@IsSAC"
            currentParam.Value = IIf(_configvalue.IsSAC, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@SubTeam_No"
            currentParam.Value = IIf(_configvalue.SubTeam_NO = -1, DBNull.Value, _configvalue.SubTeam_NO)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "@IsAllowance"
            currentParam.Value = IIf(_configvalue.IsAllowance, -1, 1)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Label"
            currentParam.Value = _configvalue.Label
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@IsDisabled"
            If _configvalue.Disabled Then
                currentParam.Value = 1
            Else
                currentParam.Value = 0
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@IsHeaderElement"
            If _configvalue.IsHeaderElement Then
                currentParam.Value = 1
            Else
                currentParam.Value = 0
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@IsItemElement"
            If _configvalue.IsItemElement Then
                currentParam.Value = 1
            Else
                currentParam.Value = 0
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("EInvoicing_InsertConfigValue", paramList)


        End Sub

    



        Public Function getSubTeamsDataTable() As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("EInvoicing_getSubTeams")


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

        Public Function getConfigInfoDataTable() As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("EInvoicing_GetConfigInfo")

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


    End Class
End Namespace

