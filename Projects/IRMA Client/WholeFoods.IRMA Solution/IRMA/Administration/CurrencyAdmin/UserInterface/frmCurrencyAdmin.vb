Public Class CurrencyAdmin


#Region "Properties"
    Private mCurrencies As New Currency
    Private bChanged As Boolean
#End Region

#Region "Subs and Functions"
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Call InitButtons()
        Call SetButtons()

    End Sub

    Private Sub InitButtons()

        Me.Text = "Manage Currencies"
        Me.btnDelete.Visible = True
        Me.ugrdCurrencyList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single

    End Sub

    Private Sub SetButtons()

        If Me.ugrdCurrencyList.Selected.Rows.Count > 0 Then
            Me.btnDelete.Enabled = True
        Else
            Me.btnDelete.Enabled = False
        End If

    End Sub
    Private Sub BindGrid()
        Me.ugrdCurrencyList.DataSource = mCurrencies.GetList
        Me.ugrdCurrencyList.DisplayLayout.Bands(0).Columns(0).Hidden = True
    End Sub
    Private Function Save(ByVal rebind As Boolean) As Boolean
        If bChanged Then
            Try
                For Each item As Infragistics.Win.UltraWinGrid.UltraGridRow In Me.ugrdCurrencyList.Rows
                    If item.DataChanged Then
                        Dim currItem As New Currency
                        currItem.ID = item.Cells(0).Text
                        currItem.Code = item.Cells(1).Text
                        currItem.Description = item.Cells(2).Text
                        currItem.Update()
                    End If
                Next

                If rebind Then
                    BindGrid()
                End If

                bChanged = False
            Catch ex As Exception
                MessageBox.Show(ex.ToString,
                      "Error saving", MessageBoxButtons.OK,
                          MessageBoxIcon.Exclamation)
            End Try
        End If

        Return True
    End Function
#End Region

#Region "Event Handlers"
    Private Sub frmCurrencyAdmin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BindGrid()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        Using dlg As New CurrencyAdd()
            dlg.Enabled = True
            Dim dlgResult As DialogResult = dlg.ShowDialog
            If dlgResult = Windows.Forms.DialogResult.OK Then
                BindGrid()
            End If
        End Using

        SetButtons()

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Me.ugrdCurrencyList.ActiveRow.Selected = True

        If MsgBox("Remove the selected Currency?", MsgBoxStyle.OkCancel, "Remove Currency") = MsgBoxResult.Ok Then
            For Each item As Infragistics.Win.UltraWinGrid.UltraGridRow In Me.ugrdCurrencyList.Selected.Rows
                Dim currItem As New Currency
                currItem.ID = item.Cells(0).Text
                currItem.Remove()
            Next
        End If

        Call SetButtons()
        BindGrid()

    End Sub

    Private Sub ugrdCurrencyList_AfterCellActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdCurrencyList.AfterCellActivate
        Me.ugrdCurrencyList.ActiveRow.Selected = True
        Call SetButtons()
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Save(False)
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Save(True)

        btnApply.Enabled = False
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If bChanged Then
            If MessageBox.Show("You have unapplied changes to Currencies.  Save Changes before closing?", "Save Changes?",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Save(False)
            End If
        End If
        Me.Close()
    End Sub

    Private Sub ugrdCurrencyList_AfterCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles ugrdCurrencyList.AfterCellUpdate
        btnApply.Enabled = True
        bChanged = True
    End Sub
#End Region

End Class