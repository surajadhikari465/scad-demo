Imports System.Linq

Public Class SlawItemRefresh
  Private RefreshService As ItemIdentifierRefreshService
  Private RefreshType As ItemRefreshType
  Const MAX_IDENTIFIERS As Integer = 1000

  Public Enum ItemRefreshType
    ItemLocale
    Price
  End Enum

  Public Sub New(ByVal argRefreshType As ItemRefreshType)
    InitializeComponent()
    RefreshType = argRefreshType
    RefreshService = New ItemIdentifierRefreshService()
        Me.Text = String.Format("Mammoth {0} Refresh", If(RefreshType = ItemRefreshType.ItemLocale, "Item Locale", "Price"))
    End Sub

  Private Sub RefreshItemsBtn_Click(sender As Object, e As EventArgs) Handles RefreshItemsBtn.Click
    Dim identifiers As List(Of ItemRefreshModel) = ItemIdentifiersTextBox.Lines() _
                .Where(Function(i) Not String.IsNullOrWhiteSpace(i)) _
                .Select(Function(i) New ItemRefreshModel With {.Identifier = i.Trim()}).ToList()

    If Not identifiers.Any() OrElse identifiers.Count > MAX_IDENTIFIERS Then
      MessageBox.Show(String.Format("No Item Identifiers has been entered or number of Item Identifiers exceeds the maximum allowed of {0}.", MAX_IDENTIFIERS), "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
      Exit Sub
    End If

    Try
      Dim request As ItemIdentifierRefreshRequest = New ItemIdentifierRefreshRequest(identifiers, "Slaw")
      Dim response As ItemIdentifierRefreshResponse = Nothing

      Select Case RefreshType
        Case ItemRefreshType.ItemLocale : response = RefreshService.RefreshSlawItemLocale(request)
        Case ItemRefreshType.Price : response = RefreshService.RefreshSlawPrice(request)
        Case Else : Exit Sub
      End Select

      If response.ItemRefreshResults.Any(Function(r) r.RefreshFailed) Then
        Dim errorsWindow As IconItemRefreshErrors = New IconItemRefreshErrors(response.ItemRefreshResults.Where(Function(m) m.RefreshFailed).ToList(), "Slaw")
        errorsWindow.ShowDialog()
      Else
        MessageBox.Show("Refreshed all items successfully.", Me.Text)
      End If
    Catch ex As Exception
      MessageBox.Show(String.Format("Unexpected error occurred while refreshing items. Error Details: {0}", ex.ToString()), "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Try
  End Sub
End Class