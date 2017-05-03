Imports System.Linq
Public Class ItemIdentifierRefreshService
    Private Property DAO As ItemIdentifierRefreshDAO

    Sub New()
        DAO = New ItemIdentifierRefreshDAO()
    End Sub

    Public Function RefreshIconItems(ByVal request As ItemIdentifierRefreshRequest) As ItemIdentifierRefreshResponse
        Return RefreshItems(request, "IconItemRefresh")
    End Function

    Public Function RefreshPosItems(ByVal request As ItemIdentifierRefreshRequest) As ItemIdentifierRefreshResponse
        Return RefreshItems(request, "IconPosItemRefresh")
    End Function

    Public Function RefreshSlawItemLocale(ByVal request As ItemIdentifierRefreshRequest) As ItemIdentifierRefreshResponse
        Return RefreshItems(request, "mammoth.SlawItemLocaleRefresh")
    End Function
    Public Function RefreshSlawPrice(ByVal request As ItemIdentifierRefreshRequest) As ItemIdentifierRefreshResponse
        Return RefreshItems(request, "mammoth.SlawPriceRefresh")
    End Function

    Private Function RefreshItems(ByVal request As ItemIdentifierRefreshRequest, storedProcedureRefresh As String) As ItemIdentifierRefreshResponse
        ValidateItemRefreshModels(request)
        Dim response = New ItemIdentifierRefreshResponse() With {.ItemRefreshResults = request.ItemRefreshModels}

        Try
            DAO.RefreshItems(request, storedProcedureRefresh)
        Catch ex As Exception
            For Each model As ItemRefreshModel In response.ItemRefreshResults.Where(Function(m) Not m.RefreshFailed)
                model.RefreshFailed = True
                model.RefreshError = "An unexpected error occurred while refreshing items. Error details: " + ex.Message
            Next
        End Try

        Return response
    End Function

    Private Sub ValidateItemRefreshModels(ByVal request As ItemIdentifierRefreshRequest)
        For Each model As ItemRefreshModel In request.ItemRefreshModels
            ValidateItemRefreshModel(model)
        Next

        Dim invalidIdentifiers As List(Of Tuple(Of String, String)) = DAO.GetInvalidIdentifiers(request)
        For Each invalidIdentifier As Tuple(Of String, String) In invalidIdentifiers
            Dim model As ItemRefreshModel = request.ItemRefreshModels.First(Function(m) m.Identifier.Equals(invalidIdentifier.Item1))
            model.RefreshFailed = True
            model.RefreshError = invalidIdentifier.Item2
        Next
    End Sub

    Private Sub ValidateItemRefreshModel(ByVal model As ItemRefreshModel)
        If (Not IsNumeric(model.Identifier) Or model.Identifier.Length > 13) Then
            model.RefreshFailed = True
            model.RefreshError = "Identifier must be 13 or fewer numbers."
        End If
    End Sub
End Class
