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

        ' Populate the data grid
        PopulateUsersConfig()
        GetTitles()

        Me.DataGridView_ConfigItems.ClearSelection()
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

        PopulateUsersConfig()

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
        PopulateUsersConfig()

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
            Dim selectedRow As DataGridViewRow = getSelectedRow(DataGridView_ConfigItems)
            If selectedRow IsNot Nothing Then
                ' Populate the edit form with the values from the selected row

                ' NO LONGER do this using the row in the grid. Pull the user ID off the selected row instead.
                ' this allows a more limited subset of data in the grid and speeds it up by not creating UserBO
                ' until you actually decide to edit one.
                'editUserForm.UserConfig = New UserBO(selectedRow)

                editUserForm.UserConfig = New UserBO(CType(selectedRow.Cells("User_ID").Value, Integer))

                ' Show the form
                editUserForm.ShowDialog(Me)
                editUserForm.Dispose()

                If selectedRow IsNot Nothing Then
                    If CType(selectedRow.Cells(3).Value, Boolean) = True Then

                        Me.Button_Delete.Enabled = True

                    Else

                        Me.Button_Delete.Enabled = False

                    End If
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
    Private Sub DataGridView_ConfigItems_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView_ConfigItems.CellDoubleClick
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
            Dim selectedRow As DataGridViewRow = getSelectedRow(DataGridView_ConfigItems)
            If selectedRow IsNot Nothing Then
                '' Populate the delete form with the values from the selected row
                'deleteUserForm = New Form_DeleteUser()
                'deleteUserForm.UserConfig = New UserBO(selectedRow)
                '' Show the form
                'deleteUserForm.ShowDialog(Me)
                'deleteUserForm.Dispose()

                ' just disable the account - why do all this extra work?
                Me.Cursor = Cursors.WaitCursor
                UserDAO.DeleteUserRecord(CType(selectedRow.Cells("User_ID").Value, Integer))
                'refresh the grid after disabling the account
                PopulateUsersConfig()
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

        Dim source As New BindingSource()
        source.DataSource = Me.DataGridView_ConfigItems.DataSource

        ' Set the data source for the DataGridView.
        Me.DataGridView_ConfigItems.DataSource = source

        source.RemoveFilter()

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

        Dim source As New BindingSource()
        source.DataSource = Me.DataGridView_ConfigItems.DataSource

        ' Set the data source for the DataGridView.
        Me.DataGridView_ConfigItems.DataSource = source

        Dim filter As String = String.Empty

        Dim i As Integer
        For i = 0 To filterCollection.Count - 1
            filter = filter & String.Format("{0} {1}", filterCollection.GetKey(i), filterCollection.Get(i))
            If i < filterCollection.Count - 1 Then
                filter = filter & " AND "
            End If
        Next i

        source.Filter = filter

        Me.Cursor = Cursors.Default

        If source.Count = 0 Then
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
    ''' Populate the DataGridView_ConfigItems with the current data from the database.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateUsersConfig()
        Try

            ' Read the users data
            Dim _dataSet As DataSet = UserDAO.GetUsers()

            DataGridView_ConfigItems.DataSource = _dataSet.Tables(0)
            DataGridView_ConfigItems.MultiSelect = False

            ' Format the view
            ' Make sure at least one entry was returned before configuring the columns
            If (DataGridView_ConfigItems.Columns.Count > 0) Then

                DataGridView_ConfigItems.Columns("User_ID").Visible = False

                ' WAY to much processing going on here. Limit to only the fields we are actually displaying
                ' DO NOT create objects for every user - only create an object for a user if we are going to use it
                ' otherwise we are slowing this panel way down.

                'DataGridView_ConfigItems.Columns("SuperUser").Visible = False
                'DataGridView_ConfigItems.Columns("Search_1").Visible = False
                'DataGridView_ConfigItems.Columns("Search_2").Visible = False
                'DataGridView_ConfigItems.Columns("Search_3").Visible = False
                'DataGridView_ConfigItems.Columns("Search_4").Visible = False
                'DataGridView_ConfigItems.Columns("Search_5").Visible = False
                'DataGridView_ConfigItems.Columns("Search_6").Visible = False
                'DataGridView_ConfigItems.Columns("Search_7").Visible = False
                'DataGridView_ConfigItems.Columns("Search_8").Visible = False
                'DataGridView_ConfigItems.Columns("Search_9").Visible = False
                'DataGridView_ConfigItems.Columns("Search_10").Visible = False
                'DataGridView_ConfigItems.Columns("Telxon_Store_Limit").Visible = False
                'DataGridView_ConfigItems.Columns("Telxon_Enabled").Visible = False
                'DataGridView_ConfigItems.Columns("Telxon_Waste").Visible = False
                'DataGridView_ConfigItems.Columns("Telxon_Cycle_Count").Visible = False
                'DataGridView_ConfigItems.Columns("Telxon_Distribution").Visible = False
                'DataGridView_ConfigItems.Columns("Telxon_Transfers").Visible = False
                'DataGridView_ConfigItems.Columns("Telxon_Price_Check").Visible = False
                'DataGridView_ConfigItems.Columns("Telxon_Superuser").Visible = False
                'DataGridView_ConfigItems.Columns("Telxon_Orders").Visible = False
                'DataGridView_ConfigItems.Columns("Telxon_Receiving").Visible = False
                'DataGridView_ConfigItems.Columns("Telxon_Reserved_2").Visible = False
                'DataGridView_ConfigItems.Columns("Telxon_Reserved_3").Visible = False
                'DataGridView_ConfigItems.Columns("Support_Password").Visible = False
                'DataGridView_ConfigItems.Columns("Support_Administrator").Visible = False
                'DataGridView_ConfigItems.Columns("Support_Worker").Visible = False
                'DataGridView_ConfigItems.Columns("Maintenance_Password").Visible = False
                'DataGridView_ConfigItems.Columns("Maintenance_Administrator").Visible = False
                'DataGridView_ConfigItems.Columns("Maintenance_Worker").Visible = False
                'DataGridView_ConfigItems.Columns("RecvLog_Store_Limit").Visible = False
                'DataGridView_ConfigItems.Columns("Delete_Access").Visible = False
                'DataGridView_ConfigItems.Columns("Warehouse").Visible = False
                'DataGridView_ConfigItems.Columns("Non_PO_Administrator").Visible = False
                'DataGridView_ConfigItems.Columns("PriceBatchProcessor").Visible = False
                'DataGridView_ConfigItems.Columns("Inventory_Administrator").Visible = False

                DataGridView_ConfigItems.Columns("UserName").DisplayIndex = 0
                DataGridView_ConfigItems.Columns("UserName").HeaderText = ResourcesAdministration.GetString("label_UserName")
                DataGridView_ConfigItems.Columns("UserName").ReadOnly = True

                DataGridView_ConfigItems.Columns("FullName").DisplayIndex = 1
                DataGridView_ConfigItems.Columns("FullName").HeaderText = ResourcesAdministration.GetString("label_FullName")
                DataGridView_ConfigItems.Columns("FullName").ReadOnly = True

                'DataGridView_ConfigItems.Columns("EMail").DisplayIndex = 2
                'DataGridView_ConfigItems.Columns("EMail").HeaderText = ResourcesAdministration.GetString("label_EMail")
                'DataGridView_ConfigItems.Columns("EMail").ReadOnly = True

                'DataGridView_ConfigItems.Columns("Phone_Number").DisplayIndex = 3
                'DataGridView_ConfigItems.Columns("Phone_Number").HeaderText = ResourcesAdministration.GetString("label_Phone_Number")
                'DataGridView_ConfigItems.Columns("Phone_Number").ReadOnly = True

                DataGridView_ConfigItems.Columns("Title").DisplayIndex = 2
                DataGridView_ConfigItems.Columns("Title").HeaderText = ResourcesAdministration.GetString("label_Title")
                DataGridView_ConfigItems.Columns("Title").ReadOnly = True

                DataGridView_ConfigItems.Columns("AccountEnabled").DisplayIndex = 3
                DataGridView_ConfigItems.Columns("AccountEnabled").HeaderText = ResourcesAdministration.GetString("label_AccountEnabled")
                DataGridView_ConfigItems.Columns("AccountEnabled").ReadOnly = True

                DataGridView_ConfigItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            End If

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

    Private Sub DataGridView_ConfigItems_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView_ConfigItems.ColumnHeaderMouseClick
        Me.DataGridView_ConfigItems.ClearSelection()
        '       Me.Button_Delete.Enabled = False
    End Sub

    Private Sub DataGridView_ConfigItems_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView_ConfigItems.RowEnter

        If Not Me._initializing And Me.DataGridView_ConfigItems.SelectedRows.Count = 1 Then

            Logger.LogDebug("DataGridView_ConfigItems_RowEnter entry", Me.GetType())

            Try

                Me.Button_Delete.Enabled = False

                ' Get the selected row
                Dim selectedRow As DataGridViewRow = getSelectedRow(DataGridView_ConfigItems)
                If selectedRow IsNot Nothing Then
                    If CType(selectedRow.Cells("AccountEnabled").Value, Boolean) = True Then

                        Me.Button_Delete.Enabled = True

                    Else

                        Me.Button_Delete.Enabled = False

                    End If
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
