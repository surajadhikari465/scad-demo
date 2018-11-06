
Public Class TaxJurisdictionAdmin
#Region "Properties"
    Private mTaxJurisdictions As New TaxJurisdictionAdminBO
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

        Me.Text = "Manage Tax Jurisdictions"
        Me.btnDelete.Visible = True
        Me.ugrdTaxJurisdictionList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single

    End Sub

    Private Sub SetButtons()

        If Me.ugrdTaxJurisdictionList.Selected.Rows.Count > 0 AndAlso Me.ugrdTaxJurisdictionList.Selected.Rows(0).Cells(4).Text = 0 Then
            Me.btnDelete.Enabled = True
        Else
            Me.btnDelete.Enabled = False
        End If

    End Sub
    Private Sub BindGrid()
        ugrdTaxJurisdictionList.DataSource = TaxJurisdictionAdminBO.GetList()
        ugrdTaxJurisdictionList.DisplayLayout.Bands(0).Columns(0).Hidden = True
        ugrdTaxJurisdictionList.DisplayLayout.Bands(0).Columns(2).Hidden = True
        ugrdTaxJurisdictionList.DisplayLayout.Bands(0).Columns(3).Hidden = True
        ugrdTaxJurisdictionList.DisplayLayout.Bands(0).Columns(4).Hidden = True
        ugrdTaxJurisdictionList.DisplayLayout.Bands(0).Columns(5).Hidden = True
        ugrdTaxJurisdictionList.DisplayLayout.Bands(0).Columns(6).Hidden = True
    End Sub
    Private Function Save(ByVal rebind As Boolean) As Boolean

        If bChanged Then
            Try
                For Each item As Infragistics.Win.UltraWinGrid.UltraGridRow In Me.ugrdTaxJurisdictionList.Rows
                    Dim currItem As New TaxJurisdictionAdminBO
                    currItem.TaxJurisdictionId = item.Cells(0).Text
                    currItem.LastUpdateUserID = giUserID
                    currItem.TaxJurisdictionDesc = item.Cells(1).Text
                    currItem.LastUpdate = item.Cells(2).Text
                    currItem.Update()
                Next

                If rebind Then
                    BindGrid()
                End If

                bChanged = False
            Catch ex As Exception
                MessageBox.Show(ex.ToString, _
                      "Error saving", MessageBoxButtons.OK, _
                          MessageBoxIcon.Exclamation)
            End Try
        End If

        Return True

        Return True
    End Function
#End Region

#Region "Event Handlers"

    Private Sub frmTaxJurisdictionAdmin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BindGrid()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        Using dlg As New TaxJurisdictionAdd()
            dlg.Enabled = True
            Dim dlgResult As DialogResult = dlg.ShowDialog
            If dlgResult = Windows.Forms.DialogResult.OK Then
                BindGrid()
            End If
        End Using

        SetButtons()

    End Sub


    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Me.ugrdTaxJurisdictionList.ActiveRow.Selected = True
        If MsgBox("Remove the selected Tax Jurisdiction?", MsgBoxStyle.OkCancel, "Remove Tax Jurisdiction") = MsgBoxResult.Ok Then
            Dim SelectedItems As New List(Of TaxJurisdictionAdminBO)
            For Each item As Infragistics.Win.UltraWinGrid.UltraGridRow In Me.ugrdTaxJurisdictionList.Selected.Rows
                mTaxJurisdictions.TaxJurisdictionId = item.Cells(0).Text
                mTaxJurisdictions.Remove()
            Next
        End If

        Call SetButtons()
        BindGrid()
    End Sub

    Private Sub ugrdTaxJurisdictionList_AfterCellActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdTaxJurisdictionList.AfterCellActivate
        Me.ugrdTaxJurisdictionList.ActiveRow.Selected = True
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
            If MessageBox.Show("You have unapplied changes to Rooms and or Stations.  Save Changes before closing?", "Save Changes?", _
               MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Save(False)
            End If
        End If
        Me.Close()
    End Sub

    Private Sub ugrdTaxJurisdictionList_AfterCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles ugrdTaxJurisdictionList.AfterCellUpdate
        btnApply.Enabled = True
        bChanged = True
    End Sub

#End Region
End Class
