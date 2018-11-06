Imports System.Linq
Imports WholeFoods.Utility

Public Class PosItemRefresh

    Private maximumNumberOfIdentifiers As Integer = 1000

    Private posItemRefreshService As ItemIdentifierRefreshService

    Public Sub New()
        InitializeComponent()
        posItemRefreshService = New ItemIdentifierRefreshService()
        maximumNumberOfIdentifiers = CInt(ConfigurationServices.AppSettings("R10ItemRefreshMaximum"))
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
            PosRefreshItems(identifiers)
        End If
    End Sub

    Private Sub PosRefreshItems(identifiers As List(Of ItemRefreshModel))
        Dim request As ItemIdentifierRefreshRequest = New ItemIdentifierRefreshRequest(identifiers, "POS")

        If (identifiers.Count > (maximumNumberOfIdentifiers / 10)) Then
            Dim dlgResult = MessageBox.Show(
                "Refreshing a large number of items can impact the time it takes for the POS Push to complete.  Are you sure you want to continue?",
                "Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning)

            If (dlgResult = DialogResult.No) Then
                Exit Sub
            End If
        End If

        Try
            Dim response As ItemIdentifierRefreshResponse = posItemRefreshService.RefreshPosItems(request)

            If response.ItemRefreshResults.Any(Function(r) r.RefreshFailed) Then
                Dim errorsWindow As IconItemRefreshErrors = New IconItemRefreshErrors(response.ItemRefreshResults.Where(Function(m) m.RefreshFailed).ToList(), "R10")
                errorsWindow.ShowDialog()
            Else
                MessageBox.Show("Refreshed all items successfully.", "R10 Item Refresh")
            End If
        Catch ex As Exception
            MessageBox.Show("Unexpected error occurred while refreshing items. Error Details:" + ex.ToString())
        End Try
    End Sub
End Class