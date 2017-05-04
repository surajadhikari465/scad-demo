Imports System.Text
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_ManageMenuAccess

    Private Sub Form_ManageMenuAccess_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PopulateGrid(-1)
    End Sub

    Private Sub PopulateGrid(ByVal iRowIndex As Integer)
        LoadDataGrid()
        FormatDataGrid()

        If iRowIndex > -1 Then grdMenus.CurrentCell = grdMenus.Rows(iRowIndex).Cells("Visible")
    End Sub

    Private Sub LoadDataGrid()
        grdMenus.Columns.Clear()
        Me.grdMenus.DataSource = MenuAccessDAO.GetMenuAccessRecords.Tables(0)
    End Sub

    Private Sub FormatDataGrid()
        Dim col As New DataGridViewLinkColumn

        grdMenus.Columns("MenuAccessId").Visible = False
        grdMenus.Columns("MenuAccessId").DisplayIndex = 0

        grdMenus.Columns("MenuName").DisplayIndex = 1
        grdMenus.Columns("MenuName").HeaderText = "Menu Name"
        grdMenus.Columns("MenuName").ReadOnly = True
        grdMenus.Columns("MenuName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        grdMenus.Columns("MenuName").FillWeight = 250

        grdMenus.Columns("Visible").DisplayIndex = 2
        grdMenus.Columns("Visible").HeaderText = "Visible"
        grdMenus.Columns("Visible").ReadOnly = False
        grdMenus.Columns("Visible").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        grdMenus.Columns("Visible").FillWeight = 50

        col.Name = "Delete"
        col.HeaderText = "Delete"
        col.Text = "Delete"
        col.UseColumnTextForLinkValue = True
        grdMenus.Columns.Add(col)
        grdMenus.Columns("Delete").DisplayIndex = 3
        grdMenus.Columns("Delete").ReadOnly = True
        grdMenus.Columns("Delete").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        grdMenus.Columns("Delete").FillWeight = 50

        grdMenus.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub grdMenus_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdMenus.CellContentClick
        If e.ColumnIndex = 2 Then
            UpdateVisibility(grdMenus.Rows(e.RowIndex).Cells("MenuAccessId").Value, False, False)
        ElseIf e.ColumnIndex = 3 Then
            MenuAccessDAO.DeleteMenuItem(grdMenus.Rows(e.RowIndex).Cells("MenuAccessId").Value)
        End If

        PopulateGrid(e.RowIndex)
    End Sub

    Private Sub CheckBox_All_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_All.CheckStateChanged
        Dim row As DataGridViewRow

        For Each row In grdMenus.Rows
            row.Cells("Visible").Value = CheckBox_All.Checked
        Next

        UpdateVisibility(0, True, CheckBox_All.Checked)
    End Sub

    Private Sub UpdateVisibility(ByVal iMenuAccessId As Integer, ByVal blnAll As Boolean, ByVal blnAllValue As Boolean)
        Dim menuAccessBO As New MenuAccessBO
        Dim menuaccessdao As New MenuAccessDAO

        menuAccessBO.MenuAccessKey = iMenuAccessId
        menuaccessdao.UpdateMenuItem(menuAccessBO, blnAll, blnAllValue)
    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub Button_Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        Dim frm As New Form_AddMenu

        frm.ShowDialog()

        If frm.MenuName.ToString <> "" Then
            MenuAccessDAO.AddMenuItem(frm.MenuName, frm.IsMenuVisible)
            PopulateGrid(-1)
        End If
    End Sub
End Class