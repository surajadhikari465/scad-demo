Imports System.Text
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Public Class Form_ManageRoleConflicts
    Dim m_blnIsReadOnly As Boolean

    Public Property IsReadOnly() As Boolean
        Get
            IsReadOnly = m_blnIsReadOnly
        End Get
        Set(ByVal value As Boolean)
            m_blnIsReadOnly = value
        End Set
    End Property

    Private Sub Form_ManageTitles_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PopulateForm()
    End Sub

    Private Sub PopulateForm()
        grdConflicts.Columns.Clear()

        If Me.IsReadOnly Then
            Label1.Visible = False
            Label2.Visible = False
            DropDownList_Role1.Visible = False
            DropDownList_Role2.Visible = False
            Button_AddConflict.Visible = False
            grdConflicts.Top = DropDownList_Role1.Top
            Button_Close.Top = (grdConflicts.Top + grdConflicts.Height) + 20
            Me.Height = (Button_Close.Top + 70)
        End If

        PopulatePrimaryConflictComboBox()
        PopulateConflictComboBox()
        PopulateGrid()
    End Sub

    Private Sub PopulatePrimaryConflictComboBox()
        Dim frm As New Form_EditUser
        Dim ctrl As Control
        Dim ctrl2 As Control

        DropDownList_Role1.Items.Clear()

        For Each ctrl In frm.TabControl_SecuritySettings.TabPages(0).Controls
            If TypeOf ctrl Is GroupBox Then
                For Each ctrl2 In ctrl.Controls
                    If TypeOf ctrl2 Is CheckBox Then
                        DropDownList_Role1.Items.Add(ctrl2.Text)
                    End If
                Next
            End If
        Next

        DropDownList_Role1.Sorted = True
    End Sub

    Private Sub PopulateConflictComboBox()
        Dim itm As Object

        DropDownList_Role2.Items.Clear()

        For Each itm In DropDownList_Role1.Items
            If itm.ToString <> DropDownList_Role1.Text And Not ConflictExists(DropDownList_Role1.Text, itm.ToString) Then
                DropDownList_Role2.Items.Add(itm.ToString)
            End If
        Next

        DropDownList_Role2.Sorted = True
    End Sub

    Private Sub PopulateGrid()
        Dim results As DataTable = Nothing

        Try
            grdConflicts.DataSource = TitleDAO.GetRoleConflicts
            grdConflicts.MultiSelect = False
        Catch ex As Exception
            Throw ex
        End Try

        FormatGrid()
    End Sub

    Private Sub FormatGrid()
        Dim col As New DataGridViewLinkColumn

        ' Format the view
        ' Make sure at least one entry was returned before configuring the columns
        If (grdConflicts.Columns.Count > 0) Then
            If Not m_blnIsReadOnly Then
                col.HeaderText = "Delete"
                col.Text = "Delete"
                col.UseColumnTextForLinkValue = True
                grdConflicts.Columns.Add(col)
                grdConflicts.Columns(1).DisplayIndex = 3
                grdConflicts.Columns(1).ReadOnly = True
                grdConflicts.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
                grdConflicts.Columns(1).FillWeight = 50
            End If

            grdConflicts.Columns("ConflictId").Visible = False
            grdConflicts.Columns("ConflictId").DisplayIndex = 0

            grdConflicts.Columns("Role1").DisplayIndex = 1
            grdConflicts.Columns("Role1").HeaderText = "Role 1"
            grdConflicts.Columns("Role1").ReadOnly = True
            grdConflicts.Columns("Role1").FillWeight = 250
            grdConflicts.Columns("Role1").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            grdConflicts.Columns("Role2").DisplayIndex = 2
            grdConflicts.Columns("Role2").HeaderText = "Role 2"
            grdConflicts.Columns("Role2").ReadOnly = True
            grdConflicts.Columns("Role2").FillWeight = 250
            grdConflicts.Columns("Role2").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
    End Sub

    Private Sub Button_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Close.Click
        Me.Close()
    End Sub

    Private Sub Button_AddConflict_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddConflict.Click
        If DropDownList_Role1.Text = "" Or DropDownList_Role2.Text = "" Then
            MsgBox("A Primary Role and Conflict Role must be selected.", MsgBoxStyle.Critical, "Manage Role Conflicts")
            Exit Sub
        End If

        TitleDAO.AddRoleConflict(DropDownList_Role1.Text, DropDownList_Role2.Text)
        PopulateForm()
    End Sub

    Private Sub grdConflicts_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdConflicts.CellContentClick
        If e.ColumnIndex = 3 Then
            TitleDAO.DeleteRoleConflict(grdConflicts.Rows(e.RowIndex).Cells(0).Value)
            PopulateForm()
        End If
    End Sub

    Private Sub DropDownList_Role1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList_Role1.SelectedIndexChanged
        PopulateConflictComboBox()
        DropDownList_Role2.Focus()
    End Sub

    Private Function ConflictExists(ByVal sRole1 As String, ByVal sRole2 As String) As Boolean
        Dim row As DataGridViewRow

        For Each row In grdConflicts.Rows
            If row.Cells("Role1").Value.ToString = sRole1 And row.Cells("Role2").Value.ToString = sRole2 Then
                ConflictExists = ConflictExists Or True
            Else
                ConflictExists = ConflictExists Or False
            End If
        Next
    End Function
End Class
