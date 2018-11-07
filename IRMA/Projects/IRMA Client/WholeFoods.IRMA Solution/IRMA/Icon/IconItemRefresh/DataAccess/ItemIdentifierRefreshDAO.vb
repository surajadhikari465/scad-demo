Imports WholeFoods.Utility.DataAccess
Imports System.Linq
Imports System.Data.SqlClient
Imports log4net

Public Class ItemIdentifierRefreshDAO

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Function GetInvalidIdentifiers(ByVal request As ItemIdentifierRefreshRequest) As List(Of Tuple(Of String, String))
        logger.Debug("GetNonValidatedItems Entry")

        Dim identifiers As String = String.Join("|", request.ItemRefreshModels _
                                                .Where(Function(m) Not m.RefreshFailed) _
                                                .Select(Function(m) m.Identifier))
        If String.IsNullOrWhiteSpace(identifiers) Then
            Return New List(Of Tuple(Of String, String))
        End If

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim sqlReader As SqlDataReader = Nothing

        Try
            currentParam = New DBParam
            currentParam.Name = "Identifiers"
            currentParam.Value = identifiers
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "RefreshType"
            currentParam.Value = request.ItemRefreshType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            sqlReader = factory.GetStoredProcedureDataReader("IconItemRefreshValidateIdentifiers", paramList)

            Dim results = New List(Of Tuple(Of String, String))
            Dim identifier As String = Nothing
            Dim refreshError As String = Nothing

            While sqlReader.Read()
                identifier = sqlReader.GetString(sqlReader.GetOrdinal("Identifier"))
                refreshError = sqlReader.GetString(sqlReader.GetOrdinal("RefreshError"))

                results.Add(New Tuple(Of String, String)(identifier, refreshError))
            End While

            Return results
        Catch ex As Exception
            Throw ex
        Finally
            If sqlReader IsNot Nothing Then
                sqlReader.Close()
            End If

            logger.Debug("GetNonValidatedItems Exit")
        End Try
    End Function

    Public Sub RefreshItems(ByVal request As ItemIdentifierRefreshRequest, storedProcedureRefresh As String)
        logger.Debug("RefreshItems Entry")

        Dim identifiers As String = String.Join("|", request.ItemRefreshModels _
                                                .Where(Function(m) Not m.RefreshFailed) _
                                                .Select(Function(m) m.Identifier))
        If String.IsNullOrWhiteSpace(identifiers) Then
            Return
        End If

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim sqlReader As SqlDataReader = Nothing

        Try
            currentParam = New DBParam
            currentParam.Name = "Identifiers"
            currentParam.Value = identifiers
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure(storedProcedureRefresh, paramList)
        Catch ex As Exception
            Throw ex
        Finally
            If sqlReader IsNot Nothing Then
                sqlReader.Close()
            End If

            logger.Debug("RefreshItems Exit")
        End Try
    End Sub
End Class
