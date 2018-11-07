Imports System.Text
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Public Class Form_TitleDeleteConflicts
    Dim m_iTitleId As Integer
    Dim m_blnConflictsResolved As Boolean

    Public Property ConflictsResolved() As Boolean
        Get
            ConflictsResolved = m_blnConflictsResolved
        End Get
        Set(ByVal value As Boolean)
            m_blnConflictsResolved = value
        End Set
    End Property

    Public Property TitleId() As Integer
        Get
            TitleId = m_iTitleId
        End Get
        Set(ByVal value As Integer)
            m_iTitleId = value
        End Set
    End Property

    Private Sub Form_ManageTitles_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ComboBox_Titles.DataSource = TitleDAO.GetTitles
        ComboBox_Titles.ValueMember = "Title_ID"
        ComboBox_Titles.DisplayMember = "Title_Desc"
        ComboBox_Titles.SelectedValue = -1

        PopulateGrid()
    End Sub

    Private Sub ComboBox_Titles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Titles.SelectedIndexChanged
        For x As Integer = 0 To grdUsers.Rows.Count - 1
            Dim selectedrow As DataGridViewRow = grdUsers.Rows(x)
            selectedrow.Cells("Title").Value = ComboBox_Titles.SelectedValue
        Next
    End Sub

    Private Sub PopulateGrid()
        Dim results As DataTable = Nothing

        Try
            grdUsers.DataSource = TitleDAO.GetTitleConflicts(Me.TitleId)
            grdUsers.MultiSelect = False
        Catch ex As Exception
            Throw ex
        End Try

        FormatGrid()
    End Sub

    Private Sub FormatGrid()
        Dim col As New DataGridViewComboBoxColumn

        ' Format the view
        ' Make sure at least one entry was returned before configuring the columns
        If (grdUsers.Columns.Count > 0) Then
            grdUsers.Columns("User_ID").Visible = False

            grdUsers.Columns("UserName").DisplayIndex = 0
            grdUsers.Columns("UserName").HeaderText = ResourcesAdministration.GetString("label_UserName")
            grdUsers.Columns("UserName").ReadOnly = True

            grdUsers.Columns("FullName").DisplayIndex = 1
            grdUsers.Columns("FullName").HeaderText = ResourcesAdministration.GetString("label_FullName")
            grdUsers.Columns("FullName").ReadOnly = True

            col.HeaderText = ResourcesAdministration.GetString("label_Title")
            col.DisplayMember = "Title_Desc"
            col.ValueMember = "Title_ID"
            col.DataSource = TitleDAO.GetTitles()
            col.Name = "Title"
            grdUsers.Columns.Add(col)
            grdUsers.Columns("Title").DisplayIndex = 2

            'default all rows to the title
            For x As Integer = 0 To grdUsers.Rows.Count - 1
                Dim selectedrow As DataGridViewRow = grdUsers.Rows(x)
                selectedrow.Cells("Title").Value = Me.TitleId
            Next

            grdUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        End If
    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Me.ConflictsResolved = False
        Me.Close()
    End Sub

    Private Sub CheckBox_ApplyAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_ApplyAll.CheckedChanged
        ComboBox_Titles.Enabled = CheckBox_ApplyAll.Checked
    End Sub

    Private Sub Button_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Save.Click
        Dim blnOriginalTitleExists As Boolean = False

        For x As Integer = 0 To grdUsers.Rows.Count - 1
            Dim selectedrow As DataGridViewRow = grdUsers.Rows(x)

            blnOriginalTitleExists = blnOriginalTitleExists Or CBool(CInt(selectedrow.Cells("Title").Value) = Me.TitleId)
            TitleDAO.UpdateTitleConflicts(selectedrow.Cells("Title").Value, selectedrow.Cells("User_Id").Value)
        Next

        If blnOriginalTitleExists Then
            MsgBox("There are still instances of the original title.  Please resolve the following conflicts.", MsgBoxStyle.Critical, "Title Delete Conflicts")
            Me.ConflictsResolved = False
            Me.grdUsers.Columns.Clear()
            PopulateGrid()
        Else
            Me.ConflictsResolved = True
        End If
    End Sub
End Class
