Option Explicit On
Option Strict On

Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.DataAccess

Public Class Form_ManageUsers

#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    ''' Form to create or edit a single User.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents editUserForm As Form_EditUser

    ''' <summary>
    ''' Form to delete a single User.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents deleteUserForm As Form_DeleteUser

    ''' <summary>
    ''' Notes that the form is loading.
    ''' </summary>
    ''' <remarks>Use to suspend control events while loading form values.</remarks>
    Dim _initializing As Boolean = False

    ''' <summary>
    ''' Notes that the user grid list has a filter applied to it.
    ''' </summary>
    ''' <remarks>Used to decide whether to refresh the user user list in the grid.</remarks>
    Dim _filterApplied As Boolean = False

#End Region

#Region "Events handled by this form"
    ''' <summary>
    ''' This function reads the selected row from a data grid.
    ''' The row can be selected by highlighting the entire row or a single cell
    ''' within the row.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getSelectedRow(ByRef dataGrid As DataGridView) As DataGridViewRow
        ' Get the selected row
        Dim selectedRow As DataGridViewRow = Nothing
        If (dataGrid.SelectedRows.Count = 1) Then
            Dim rowEnum As IEnumerator = dataGrid.SelectedRows.GetEnumerator
            rowEnum.MoveNext()
            selectedRow = CType(rowEnum.Current, DataGridViewRow)
        ElseIf (dataGrid.SelectedCells.Count = 1) Then
            Dim cellEnum As IEnumerator = dataGrid.SelectedCells.GetEnumerator
            cellEnum.MoveNext()
            Dim selectedCell As DataGridViewCell = CType(cellEnum.Current, DataGridViewCell)
            selectedRow = selectedCell.OwningRow
        Else
            ' Error condition
            DisplayErrorMessage("A row must be selected to perform this action.")
        End If
        Return selectedRow
    End Function

#Region "Load Form"

    ''' <summary>
    ''' Load the form, querying the database to populate the list of configured users.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ManageUsers_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Cursor = Cursors.WaitCursor
        Me._initializing = True

        colUserName.HeaderText = ResourcesAdministration.GetString("label_UserName")
        colFullName.HeaderText = ResourcesAdministration.GetString("label_FullName")
        colTitle.HeaderText = ResourcesAdministration.GetString("label_Title")
        colAccountEnabled.HeaderText = ResourcesAdministration.GetString("label_AccountEnabled")

        GetTitles()

        Me.DataGridView_ManageUsers.ClearSelection()
        Me.TextBox_FilterUserName.Focus()
        Me._initializing = False
        Cursor = Cursors.Arrow
    End Sub

    Private Sub GetTitles()

        Me.ComboBox_FilterTitle.DataSource = TitleDAO.GetTitles()
        Me.ComboBox_FilterTitle.DisplayMember = "Title_Desc"
        Me.ComboBox_FilterTitle.ValueMember = "Title_ID"
        Me.ComboBox_FilterTitle.SelectedIndex = -1
        Me.ComboBox_FilterTitle.Text = "ALL TITLES"

    End Sub

#End Region

#Region "Updates made to child form"
    ''' <summary>
    ''' Changes were made to the user configurations.  Refresh the user table.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EditUserForm_UpdateCallingForm() Handles editUserForm.UpdateCallingForm
        ' Refresh the data grid

        PopulateGrid()

        ' if the user had a filter applied before opening the form, reapply it
        If Me._filterApplied Then

            ApplyFilter()

        End If

        Me.Cursor = Cursors.Default
    End Sub

    ''' <summary>
    ''' Changes were made to the user configurations.  Refresh the user table.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DeleteUserForm_UpdateCallingForm() Handles deleteUserForm.UpdateCallingForm
        ' Refresh the data grid
        PopulateGrid()

        ' if the user had a filter applied before opening the form, reapply it
        If Me._filterApplied Then

            ApplyFilter()

        End If

        Me.Cursor = Cursors.Default
    End Sub

#End Region

#Region "Add User events"
    ''' <summary>
    ''' Process the add user events
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessAddUser()
        Logger.LogDebug("ProcessAddUser entry", Me.GetType())
        Try
            ' Bring focus to the form
            editUserForm = New Form_EditUser()
            editUserForm.CurrentAction = FormAction.Create
            ' Show the form
            editUserForm.ShowDialog(Me)
            editUserForm.Dispose()
        Catch ex As Exception
            Logger.LogError("ProcessAddUser exception when getting type=Form_EditUser", Me.GetType(), ex)
            DisplayErrorMessage()
        End Try
        Logger.LogDebug("ProcessAddUser exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' User selected the Add user button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        Logger.LogDebug("Button_Add_Click entry", Me.GetType())
        Me.Cursor = Cursors.WaitCursor
        ProcessAddUser()
        Me.Cursor = Cursors.Default
        Logger.LogDebug("Button_Add_Click exit", Me.GetType())
    End Sub

#End Region

#Region "Edit User events"
    ''' <summary>
    ''' Process the edit user events.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessEditUser()
        Try
            ' Bring focus to the form
            editUserForm = New Form_EditUser()
            editUserForm.CurrentAction = FormAction.Edit
            ' Get the selected row
            Dim selectedRow As DataGridViewRow = getSelectedRow(DataGridView_ManageUsers)
            If selectedRow IsNot Nothing Then
                ' Populate the edit form with the values from the selected row

                ' NO LONGER do this using the row in the grid. Pull the user ID off the selected row instead.
                ' this allows a more limited subset of data in the grid and speeds it up by not creating UserBO
                ' until you actually decide to edit one.
                'editUserForm.UserConfig = New UserBO(selectedRow)

                editUserForm.UserConfig = New UserBO(CType(selectedRow.Cells(colUserID.Name).Value, Integer))

                ' Show the form
                editUserForm.ShowDialog(Me)
                editUserForm.Dispose()

                selectedRow = getSelectedRow(DataGridView_ManageUsers)
                If selectedRow IsNot Nothing Then
                    Me.Button_Delete.Enabled = (CType(selectedRow.Cells(colAccountEnabled.Name).Value, Boolean) = True)
                End If

            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            Logger.LogError("ProcessEditUser exception when getting type=Form_EditStorePOSConfig", Me.GetType(), ex)
            DisplayErrorMessage()
        End Try
    End Sub

    ''' <summary>
    ''' The user double clicked a row in the user table.
    ''' This is the same as selecting the User -> Edit Selected User menu option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DataGridView_ConfigItems_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView_ManageUsers.CellDoubleClick
        Logger.LogDebug("DataGridView_ConfigItems_CellDoubleClick", Me.GetType())
        Me.Cursor = Cursors.WaitCursor
        ProcessEditUser()
        Me.Cursor = Cursors.Default
        Logger.LogDebug("DataGridView_ConfigItems_CellDoubleClick", Me.GetType())
    End Sub

    ''' <summary>
    ''' The user selected the Edit user button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Edit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Edit.Click
        Logger.LogDebug("Button_Edit_Click", Me.GetType())
        Me.Cursor = Cursors.WaitCursor
        ProcessEditUser()
        Me.Cursor = Cursors.Default
        Logger.LogDebug("Button_Edit_Click", Me.GetType())
    End Sub
#End Region

#Region "Delete User events"

    ''' <summary>
    ''' The user selected the Delete user button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Delete.Click
        Logger.LogDebug("Button_Delete_Click entry", Me.GetType())
        ProcessDeleteUser()

        ' if the user had a filter applied before disabling the user, reapply it
        If Me._filterApplied Then

            ApplyFilter()

        End If

        Me.Button_Delete.Enabled = False
        Logger.LogDebug("Button_Delete_Click exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' Process the delete user events.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessDeleteUser()
        Logger.LogDebug("ProcessDeleteUser entry", Me.GetType())
        Try
            ' Get the selected row
            Dim selectedRow As DataGridViewRow = getSelectedRow(DataGridView_ManageUsers)
            If selectedRow IsNot Nothing Then
                '' Populate the delete form with the values from the selected row
                'deleteUserForm = New Form_DeleteUser()
                'deleteUserForm.UserConfig = New UserBO(selectedRow)
                '' Show the form
                'deleteUserForm.ShowDialog(Me)
                'deleteUserForm.Dispose()

                ' just disable the account - why do all this extra work?
                Me.Cursor = Cursors.WaitCursor
                UserDAO.DeleteUserRecord(CType(selectedRow.Cells(colUserID.Name).Value, Integer))
                'refresh the grid after disabling the account
                PopulateGrid()
                Me.Cursor = Cursors.Default
                MessageBox.Show("The user account has been disabled.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            Logger.LogError("ProcessDeleteUser exception when getting type=Form_DeleteStorePOSConfig", Me.GetType(), ex)
            DisplayErrorMessage()
        End Try
        Logger.LogDebug("ProcessDeleteUser exit", Me.GetType())
    End Sub

#End Region

#Region "Close button"
    ''' <summary>
    ''' The close button returns the user to the calling form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Close.Click
        Me.Cursor = Cursors.Default
        Me.Close()
    End Sub

    ''' <summary>
    ''' The user clicked the 'X' button in top-right of window to close the form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ManageUsers_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' No additional processing is needed because data can't be changed on this form
    End Sub

#End Region

#Region " Filter events"

    Private Sub RemoveFilter()

        Me._filterApplied = False

        Me.Cursor = Cursors.WaitCursor

        UserDAOBindingSource.RemoveFilter()

        Me.TextBox_FilterFullName.Text = String.Empty
        Me.TextBox_FilterUserName.Text = String.Empty
        Me.ComboBox_FilterTitle.SelectedIndex = -1
        Me.ComboBox_FilterTitle.Text = "ALL TITLES"
        Me.Radio_FiterStatusAll.Checked = True

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub ApplyFilter()
        Me.Cursor = Cursors.WaitCursor

        Dim filterCollection As New Specialized.NameValueCollection()

        If Me.TextBox_FilterUserName.Text.Length > 0 Then
            filterCollection.Add("Username", " LIKE '%" & Me.TextBox_FilterUserName.Text & "%'")
        End If

        If Me.TextBox_FilterFullName.Text.Length > 0 Then
            filterCollection.Add("FullName", " LIKE '%" & Me.TextBox_FilterFullName.Text & "%'")
        End If

        If Me.ComboBox_FilterTitle.SelectedIndex > -1 Then
            filterCollection.Add("Title", " = '" & Me.ComboBox_FilterTitle.Text & "'")
        End If

        If Me.Radio_FilterStatusEnabled.Checked Then
            filterCollection.Add("AccountEnabled", " = True")
        ElseIf Me.Radio_FilterStatusDisabled.Checked Then
            filterCollection.Add("AccountEnabled", " = False")
        End If

        Dim filter As String = String.Empty

        Dim i As Integer
        For i = 0 To filterCollection.Count - 1
            filter = filter & String.Format("{0} {1}", filterCollection.GetKey(i), filterCollection.Get(i))
            If i < filterCollection.Count - 1 Then
                filter = filter & " AND "
            End If
        Next i

        PopulateGrid()
        UserDAOBindingSource.Filter = filter

        Me.Cursor = Cursors.Default

        If UserDAOBindingSource.Count = 0 Then
            MessageBox.Show("No users match that criteria.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Me._filterApplied = True
    End Sub

    Private Sub Button_ApplyFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ApplyFilter.Click

        ApplyFilter()

    End Sub

    Private Sub Button_ClearFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ClearFilter.Click

        RemoveFilter()

    End Sub

#End Region

#End Region

#Region "Populate and format DataGridView objects for the form"

    ''' <summary>
    ''' Populate the DataGridView with the current data from the database.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateGrid()
        Try
            ' Read the users data
            Dim _dataSet As DataSet = UserDAO.GetUsers()

            DataGridView_ManageUsers.DataSource = Nothing
            DataGridView_ManageUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            UserDAOBindingSource.DataSource = _dataSet.Tables(0)

            DataGridView_ManageUsers.DataSource = UserDAOBindingSource
            DataGridView_ManageUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        Catch e As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), e)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_ManageUsers form: PopulateUsersConfig sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try
    End Sub

    Private Sub DataGridView_ConfigItems_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView_ManageUsers.ColumnHeaderMouseClick
        Me.DataGridView_ManageUsers.ClearSelection()
        '       Me.Button_Delete.Enabled = False
    End Sub

    Private Sub DataGridView_ConfigItems_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView_ManageUsers.RowEnter

        If Not Me._initializing And Me.DataGridView_ManageUsers.SelectedRows.Count = 1 Then

            Logger.LogDebug("DataGridView_ConfigItems_RowEnter entry", Me.GetType())

            Try

                Me.Button_Delete.Enabled = False

                ' Get the selected row
                Dim selectedRow As DataGridViewRow = getSelectedRow(DataGridView_ManageUsers)

                If selectedRow IsNot Nothing Then
                    Me.Button_Delete.Enabled = (CType(selectedRow.Cells(colAccountEnabled.Name).Value, Boolean) = True)
                End If

                Me.Button_Edit.Enabled = True

            Catch ex As Exception
                Me.Cursor = Cursors.Default
                Logger.LogError("DataGridView_ConfigItems_RowEnter exception.", Me.GetType(), ex)
                DisplayErrorMessage()
            End Try

            Logger.LogDebug("DataGridView_ConfigItems_RowEnter exit", Me.GetType())

        End If
    End Sub

#End Region

End Class
