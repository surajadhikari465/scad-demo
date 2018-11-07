Imports System.Linq
Imports System.Threading.Tasks

Public Class SlawItemLocaleRefresh
    Private slawItemLocaleRefreshService As ItemIdentifierRefreshService
    Private maximumNumberOfIdentifiers As Integer

    Public Sub New()
        InitializeComponent()
        slawItemLocaleRefreshService = New ItemIdentifierRefreshService()
        maximumNumberOfIdentifiers = 1000
    End Sub

    Private Sub RefreshItemsBtn_Click(sender As Object, e As EventArgs) Handles RefreshItemsBtn.Click
        Dim identifiers As List(Of ItemRefreshModel) = ItemIdentifiersTextBox _
                .Lines() _
                .Where(Function(i) Not String.IsNullOrWhiteSpace(i)) _
                .Select(Function(i) New ItemRefreshModel With {.Identifier = i.Trim()}) _
                .ToList()
        If Not identifiers.Any() Then
            MessageBox.Show("No Item Identifiers entered.")
        ElseIf identifiers.Count > maximumNumberOfIdentifiers Then
            MessageBox.Show(String.Format("More than the maximum allowed {0} Item Identifiers entered.", maximumNumberOfIdentifiers))
        Else
            Dim request As ItemIdentifierRefreshRequest = New ItemIdentifierRefreshRequest(identifiers, "Slaw")

            Try
                Dim response As ItemIdentifierRefreshResponse = slawItemLocaleRefreshService.RefreshSlawItemLocale(request)

                If response.ItemRefreshResults.Any(Function(r) r.RefreshFailed) Then
                    Dim errorsWindow As IconItemRefreshErrors = New IconItemRefreshErrors(response.ItemRefreshResults.Where(Function(m) m.RefreshFailed).ToList(), "Slaw")
                    errorsWindow.ShowDialog()
                Else
                    MessageBox.Show("Refreshed all items successfully.", "Slaw Item Locale Refresh")
                End If
            Catch ex As Exception
                MessageBox.Show("Unexpected error occurred while refreshing items. Error Details:" + ex.ToString())
            End Try
        End If
    End Sub
End Class