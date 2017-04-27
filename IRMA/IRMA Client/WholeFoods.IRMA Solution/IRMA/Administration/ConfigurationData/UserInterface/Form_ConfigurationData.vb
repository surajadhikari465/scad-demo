Imports WholeFoods.Utility
Imports WholeFoods.IRMA.Administration.ConfigurationData.DataAccess

Public Class Form_ConfigurationData

#Region " Private Fields"

    ' Instantiate the data access class
    Private daoConfig As New ConfigurationDataDAO

    Private bIsLoading As Boolean

    ''' <summary>
    ''' Notes that the user grid list has a filter applied to it.
    ''' </summary>
    ''' <remarks>Used to decide whether to refresh the user user list in the grid.</remarks>
    Private _filterApplied As Boolean

#End Region

#Region " Filter events"

    Private Sub RemoveFilter()

        Me._filterApplied = False

        Me._formErrorProvider.Clear()

        Me.Cursor = Cursors.WaitCursor

        Dim source As New BindingSource()
        source.DataSource = Me._gridConfigList.DataSource

        ' Set the data source for the DataGridView.
        Me._gridConfigList.DataSource = source

        source.RemoveFilter()

        Me._textFilterValue.Text = String.Empty

        Me._comboFilterApp.SelectedIndex = -1
        Me._comboFilterEnv.SelectedIndex = -1
        Me._comboFilterType.SelectedIndex = -1
        Me._comboFilterKey.SelectedIndex = -1

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub ApplyFilter()

        Me.Cursor = Cursors.WaitCursor

        Me._formErrorProvider.Clear()

        Dim filterCollection As New Specialized.NameValueCollection()

        If Me._textFilterValue.Text.Length > 0 Then
            filterCollection.Add("Value", " LIKE '%" & Me._textFilterValue.Text & "%'")
        End If

        If Me._comboFilterKey.SelectedIndex > -1 Then
            filterCollection.Add("KeyName", " = '" & Me._comboFilterKey.Text & "'")
        End If

        If Me._comboFilterEnv.SelectedIndex > -1 Then
            filterCollection.Add("EnvName", " = '" & Me._comboFilterEnv.Text & "'")
        End If

        If Me._comboFilterType.SelectedIndex > -1 Then
            filterCollection.Add("TypeName", " = '" & Me._comboFilterType.Text & "'")
        End If

        If Me._comboFilterApp.SelectedIndex > -1 Then
            filterCollection.Add("AppName", " = '" & Me._comboFilterApp.Text & "'")
        End If

        If Me._checkHideDeleted.Checked Then
            filterCollection.Add("Deleted", " = 0")
            Me._gridConfigList.DisplayLayout.Bands(0).Columns("Deleted").Hidden = True
        Else
            Me._gridConfigList.DisplayLayout.Bands(0).Columns("Deleted").Hidden = False
        End If

        Dim source As New BindingSource()
        source.DataSource = Me._gridConfigList.DataSource

        ' Set the data source for the DataGridView.
        Me._gridConfigList.DataSource = source

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

        If Not Me.bIsLoading Then

            If source.Count = 0 Then
                MessageBox.Show("No configuration settings match that criteria.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        End If

        Me._filterApplied = True

    End Sub

    Private Sub _buttonFilterApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonFilterApply.Click

        Me.ApplyFilter()

    End Sub

    Private Sub _buttonFilterClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonFilterClear.Click

        Me.RemoveFilter()

    End Sub

#End Region

#Region " UpdateCallingForm Event Handlers"

    Dim WithEvents AddKeyForm As Form_ConfigurationData_KeyAdd
    Dim WithEvents AddAppForm As Form_ConfigurationData_ApplicationAdd
    Dim WithEvents AddEnvForm As Form_ConfigurationData_EnvironmentAdd
    Dim WithEvents UpdateKeyValueForm As Form_ConfigurationData_KeyValue

    Private Sub AddKey_UpdateCallingForm() Handles AddKeyForm.RefreshList

        If Not AddKeyForm.AppConfigKey Is Nothing Then
            Me.RefreshConfigList()
            Me.RefreshKeys()
            Me._comboFilterKey.SelectedValue = AddKeyForm.AppConfigKey.KeyID
            Me._comboFilterKey.Focus()
            If Me._filterApplied Then Me.ApplyFilter()
        End If

    End Sub

    Private Sub AddApp_UpdateCallingForm() Handles AddAppForm.RefreshList

        If Not AddAppForm.AppConfigApp Is Nothing Then
            Me.RefreshApplications()
            Me._comboFilterApp.SelectedValue = AddAppForm.AppConfigApp.ApplicationID
            Me._comboFilterApp.Focus()
            If Me._filterApplied Then Me.ApplyFilter()
        End If

    End Sub

    Private Sub AddEnv_UpdateCallingForm() Handles AddEnvForm.RefreshList

        If Not AddEnvForm.AppConfigEnv Is Nothing Then
            Me.RefreshEnvironments()
            Me._comboFilterEnv.SelectedValue = AddEnvForm.AppConfigEnv.EnvironmentID
            Me._comboFilterEnv.Focus()
            If Me._filterApplied Then Me.ApplyFilter()
        End If

    End Sub

    Private Sub UpdateKeyVal_UpdateCallingForm() Handles UpdateKeyValueForm.RefreshList

        If UpdateKeyValueForm.KeyValueUpdated Then
            Me.RefreshConfigList()
        End If

    End Sub

#End Region

#Region " Form Event Handlers"

    Private Sub Form_ConfigurationData_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.bIsLoading = True

        Try

            Dim dtAppConfigEnv As DataTable = daoConfig.GetEnvironmentList()
            Me._comboFilterEnv.DataSource = dtAppConfigEnv
            Me._comboFilterEnv.ValueMember = "EnvironmentID"
            Me._comboFilterEnv.DisplayMember = "Name"
            Me._comboFilterEnv.SelectedIndex = -1

            Dim dtAppConfigType As DataTable = daoConfig.GetApplicationTypeList()
            Me._comboFilterType.DataSource = dtAppConfigType
            Me._comboFilterType.ValueMember = "TypeID"
            Me._comboFilterType.DisplayMember = "Name"
            Me._comboFilterType.SelectedIndex = -1

            Dim dtAppConfigKey As DataTable = daoConfig.GetApplicationKeyList()
            Me._comboFilterKey.DataSource = dtAppConfigKey
            Me._comboFilterKey.ValueMember = "KeyID"
            Me._comboFilterKey.DisplayMember = "Name"
            Me._comboFilterKey.SelectedIndex = -1

            Me.RefreshConfigList()

            Me._checkHideDeleted.Checked = True

        Catch ex As Exception

            Logger.LogError("Exception: ", Me.GetType(), ex)
            MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
            Dim args(1) As String
            args(0) = Me.Name & " form event: Form_ConfigurationData_Load"
            ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

        End Try

        bIsLoading = False

    End Sub

#Region " Buttons"

    Private Sub _buttonShowAppGUID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonShowAppGUID.Click

        Dim form As New Form_ConfigurationData_GUID
        form.IdentificationLabel = "Application: " & Me._comboFilterApp.Text
        form.IdentificationGUID = Me._comboFilterApp.SelectedValue
        form.ShowDialog()
        form.Dispose()

    End Sub

    Private Sub _buttonShowEnvGUID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonShowEnvGUID.Click

        Dim form As New Form_ConfigurationData_GUID
        form.IdentificationLabel = "Environment: " & Me._comboFilterEnv.Text
        form.IdentificationGUID = Me._comboFilterEnv.SelectedValue
        form.ShowDialog()
        form.Dispose()

    End Sub

    Private Sub _buttonAddEnv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonAddEnv.Click

        Try

            AddEnvForm = New Form_ConfigurationData_EnvironmentAdd()
            AddEnvForm.ShowDialog()
            AddEnvForm.Dispose()

        Catch ex As Exception

            Logger.LogError("Exception: ", Me.GetType(), ex)
            MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
            Dim args(1) As String
            args(0) = Me.Name & " form event: _buttonAddEnv_Click"
            ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

        End Try

    End Sub

    Private Sub _buttonAddApp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonAddApp.Click

        If Me._comboFilterEnv.SelectedIndex > -1 Then

            Try

                AddAppForm = New Form_ConfigurationData_ApplicationAdd()
                AddAppForm.EnvironmentID = Me._comboFilterEnv.SelectedValue
                AddAppForm.EnvironmentName = Me._comboFilterEnv.Text
                AddAppForm.ShowDialog()
                AddAppForm.Dispose()

            Catch ex As Exception

                Logger.LogError("Exception: ", Me.GetType(), ex)
                MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
                Dim args(1) As String
                args(0) = Me.Name & " form event: _buttonAddApp_Click"
                ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

            End Try
        Else
            Me._formErrorProvider.SetError(Me._comboFilterEnv, "Required to add an application.")
        End If

    End Sub

    Private Sub _buttonAddKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonAddKey.Click

        Try

            AddKeyForm = New Form_ConfigurationData_KeyAdd()
            AddKeyForm.ShowDialog()
            AddKeyForm.Dispose()

        Catch ex As Exception

            Logger.LogError("Exception: ", Me.GetType(), ex)
            MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
            Dim args(1) As String
            args(0) = Me.Name & " form event: _buttonAddKey_Click"
            ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

        End Try

    End Sub

    Private Sub _buttonViewConfiguration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonViewConfiguration.Click

        Try

            Dim _doc As New Xml.XmlDocument
            _doc.LoadXml(daoConfig.GetConfigDocument(Me._comboFilterApp.SelectedValue, Me._comboFilterEnv.SelectedValue))

            Dim form As New Form_ConfigurationData_View

            form.Title = "ACTIVE CONFIG: " & Me._comboFilterEnv.Text & " - " & Me._comboFilterApp.Text
            form.DisplayDocument = _doc
            form.ShowDialog()
            form.Dispose()

        Catch ex As Exception

            Logger.LogError("Exception: ", Me.GetType(), ex)
            MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
            Dim args(1) As String
            args(0) = Me.Name & " form event: _buttonAddKey_Click"
            ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

        End Try

    End Sub

    Private Sub _buttonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Me.Close()

    End Sub

    Private Sub _buttonAppKeyAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonAppKeyAdd.Click

        Dim _appConfigValue As AppConfigValueBO

        Try

            If Me.ValidateInput() Then

                If MessageBox.Show("Add the following Key/Value Pair?" & vbCrLf & vbCrLf & _
                                    "Environment: " & Me._comboFilterEnv.Text & vbCrLf & _
                                    "Application: " & Me._comboFilterApp.Text & vbCrLf & _
                                    "Key Name: " & Me._comboFilterKey.Text & vbCrLf & _
                                    "Value: " & Me._textFilterValue.Text _
                                    , Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

                    _appConfigValue = New AppConfigValueBO
                    _appConfigValue.KeyID = Me._comboFilterKey.SelectedValue
                    _appConfigValue.ApplicationID = Me._comboFilterApp.SelectedValue
                    _appConfigValue.EnvironmentID = Me._comboFilterEnv.SelectedValue
                    _appConfigValue.Value = Me._textFilterValue.Text
                    _appConfigValue.Add(_appConfigValue, False)

                    MessageBox.Show("Key/Value pair added.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Me._comboFilterKey.SelectedIndex = -1

                    Me._textFilterValue.Clear()

                    Me.RefreshConfigList()

                End If

            End If

        Catch ex As Exception

            Logger.LogError("Exception: ", Me.GetType(), ex)
            MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
            Dim args(1) As String
            args(0) = Me.Name & " form event: _buttonAppKeyAdd_Click"
            ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

        End Try

    End Sub

    Private Sub _buttonRemoveEnv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonRemoveEnv.Click

        If Me._comboFilterEnv.SelectedIndex > -1 Then

            Try


                If MessageBox.Show("WARNING: This will remove all configuration settings for all applications associated with this environment!" & vbCrLf & vbCrLf & _
                                    "Are you sure you want to do this?", "CONFIRM ACTION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then

                    Dim _appConfigEnv As New AppConfigEnvBO

                    _appConfigEnv.EnvironmentID = Me._comboFilterEnv.SelectedValue
                    _appConfigEnv.UserID = My.Application.CurrentUserID

                    If _appConfigEnv.Remove(_appConfigEnv) Then
                        Me.RemoveFilter()
                        Me.RefreshEnvironments()
                        Me.RefreshConfigList()
                        MessageBox.Show("Environment was removed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("An error ocurred. Environment was not removed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If

                End If

            Catch ex As Exception

                Logger.LogError("Exception: ", Me.GetType(), ex)
                MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
                Dim args(1) As String
                args(0) = Me.Name & " form event: _buttonRemoveEnv_Click"
                ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

            End Try

        End If

    End Sub

    Private Sub _buttonRemoveApp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonRemoveApp.Click

        If Me._comboFilterApp.SelectedIndex > -1 And Me._comboFilterEnv.SelectedIndex > -1 Then

            Try


                If MessageBox.Show("WARNING!!!" & vbCrLf & vbCrLf & _
                                    "Your are about to remove this application and all associated configuration settings from this environment!" & vbCrLf & vbCrLf & _
                                    "The application will no longer function after it is removed." & vbCrLf & vbCrLf & _
                                    "Are you sure you want to do this?", "CONFIRM ACTION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then

                    Dim _appConfigApp As New AppConfigAppBO

                    _appConfigApp.ApplicationID = Me._comboFilterApp.SelectedValue
                    _appConfigApp.EnvironmentID = Me._comboFilterEnv.SelectedValue

                    If _appConfigApp.Remove(_appConfigApp) Then
                        Me.RemoveFilter()
                        Me.RefreshApplications()
                        Me.RefreshConfigList()
                        MessageBox.Show("Application was removed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("An error ocurred. Application was not removed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If

                End If

            Catch ex As Exception

                Logger.LogError("Exception: ", Me.GetType(), ex)
                MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
                Dim args(1) As String
                args(0) = Me.Name & " form event: _buttonRemoveApp_Click"
                ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

            End Try

        End If

    End Sub

    Private Sub _buttonRemoveKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonRemoveKey.Click

        If Me._comboFilterKey.SelectedIndex > -1 Then

            Try

                Dim _message As String = String.Empty

                If Me._comboFilterApp.SelectedIndex > -1 Then
                    _message = "This will remove this Key/Value pair from this environment and application!"
                Else
                    _message = "This will remove this Key/Value pair from ALL applications in this environment!"
                End If

                If MessageBox.Show("WARNING: " & _message & vbCrLf & vbCrLf & _
                                    "Are you sure you want to do this?", "CONFIRM ACTION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then

                    Dim _appConfigValue As New AppConfigValueBO

                    If Me._comboFilterApp.SelectedIndex > -1 Then
                        _appConfigValue.ApplicationID = Me._comboFilterApp.SelectedValue
                    Else
                        _appConfigValue.ApplicationID = Nothing
                    End If

                    _appConfigValue.EnvironmentID = Me._comboFilterEnv.SelectedValue
                    _appConfigValue.KeyID = Me._comboFilterKey.SelectedValue
                    _appConfigValue.UserID = My.Application.CurrentUserID

                    If _appConfigValue.Remove(_appConfigValue) Then
                        Me.RemoveFilter()
                        Me.RefreshKeys()
                        Me.RefreshConfigList()
                        MessageBox.Show("Key was removed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("An error ocurred. Key was not removed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If

                End If

            Catch ex As Exception

                Logger.LogError("Exception: ", Me.GetType(), ex)
                MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
                Dim args(1) As String
                args(0) = Me.Name & " form event: _buttonRemoveKey_Click"
                ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

            End Try

        End If

    End Sub

    Private Sub _buttonRemoveKeyAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonRemoveKeyAll.Click

        If Me._comboFilterKey.SelectedIndex > -1 Then

            Try

                If MessageBox.Show("WARNING: This will remove this Key/Value pair from ALL Applications in ALL environments!" & vbCrLf & vbCrLf & _
                                    "Are you sure you want to do this?", "CONFIRM ACTION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then

                    Dim _appConfigValue As New AppConfigValueBO

                    _appConfigValue.ApplicationID = Nothing
                    _appConfigValue.EnvironmentID = Nothing
                    _appConfigValue.KeyID = Me._comboFilterKey.SelectedValue
                    _appConfigValue.UserID = My.Application.CurrentUserID

                    If _appConfigValue.Remove(_appConfigValue) Then
                        Me.RemoveFilter()
                        Me.RefreshKeys()
                        Me.RefreshConfigList()
                        MessageBox.Show("Key was removed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("An error ocurred. Key was not removed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If

                End If

            Catch ex As Exception

                Logger.LogError("Exception: ", Me.GetType(), ex)
                MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
                Dim args(1) As String
                args(0) = Me.Name & " form event: _buttonRemoveKeyAll_Click"
                ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

            End Try

        End If

    End Sub

    Private Sub _buttonImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonImport.Click

        Try

            If Me._formOpenFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then

                Dim _doc As New Xml.XmlDocument
                _doc.Load(Me._formOpenFileDialog.FileName)

                Dim nodeList As Xml.XmlNodeList = _doc.SelectNodes("/configuration/appSettings/add")

                If nodeList.Count > 0 Then

                    Me._formProgressBar.Maximum = nodeList.Count()
                    Me._formProgressBar.Step = 1
                    Me._formStatusStrip.Visible = True

                    Dim _appConfigValue As New AppConfigValueBO

                    _appConfigValue.ApplicationID = Me._comboFilterApp.SelectedValue
                    _appConfigValue.EnvironmentID = Me._comboFilterEnv.SelectedValue
                    _appConfigValue.UserID = My.Application.CurrentUserID

                    Me._formProgressBar.Maximum = nodeList.Count()
                    Me._formProgressBar.Step = 1
                    Me._formStatusStrip.Visible = True

                    Me._formImportWorker.RunWorkerAsync(_appConfigValue)

                Else
                    MessageBox.Show("The document does not contain any valid application configuration settings.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End If

            End If

        Catch ex As Exception

            Logger.LogError("Exception: ", Me.GetType(), ex)
            MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
            Dim args(1) As String
            args(0) = Me.Name & " form event: _buttonImport_Click"
            ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

        End Try

    End Sub

#End Region

#Region " CheckBoxes"

    Private Sub _checkHideDeleted_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _checkHideDeleted.CheckedChanged

        Me.ApplyFilter()

    End Sub

#End Region

#Region " TextBoxes"

    'Private Sub _textFilterValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _textFilterValue.TextChanged

    '    If Me.bIsLoading Then Exit Sub

    '    If Me._textFilterValue.Text.Length > 0 Then
    '        Me._buttonAppKeyAdd.Enabled = True
    '    Else
    '        Me._buttonAppKeyAdd.Enabled = False
    '    End If

    'End Sub

#End Region

#Region " ComboBoxes"

    Private Sub _comboFilterEnv_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _comboFilterEnv.SelectedIndexChanged

        If Me.bIsLoading Then Exit Sub

        If Me._comboFilterEnv.SelectedIndex > -1 Then

            Me.RefreshApplications()

            Me._groupApplicationFilter.Enabled = True
            Me._buttonRemoveEnv.Enabled = True
            Me._buttonAddKey.Enabled = True
            Me._buttonShowEnvGUID.Enabled = True

        Else

            Me._groupApplicationFilter.Enabled = False
            Me._buttonRemoveEnv.Enabled = False
            Me._buttonAddKey.Enabled = False
            Me._buttonShowEnvGUID.Enabled = False

        End If

    End Sub

    Private Sub _comboFilterApp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _comboFilterApp.SelectedIndexChanged

        If Me.bIsLoading Then Exit Sub

        If Me._comboFilterApp.SelectedIndex > -1 Then
            Me._buttonViewConfiguration.Enabled = True
            Me._buttonRemoveApp.Enabled = True
            Me._buttonShowAppGUID.Enabled = True
            Me._buttonImport.Enabled = True
        Else
            Me._buttonViewConfiguration.Enabled = False
            Me._buttonRemoveApp.Enabled = False
            Me._buttonShowAppGUID.Enabled = False
            Me._buttonImport.Enabled = False
        End If

    End Sub

    Private Sub _comboFilterType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _comboFilterType.SelectedIndexChanged

        If Me.bIsLoading Then Exit Sub

        If Me._comboFilterType.SelectedIndex > -1 Then
            Me._buttonRemoveType.Enabled = True
        Else
            Me._buttonRemoveType.Enabled = False
        End If

    End Sub

    Private Sub _comboFilterKey_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _comboFilterKey.SelectedIndexChanged

        If Me.bIsLoading Then Exit Sub

        If Me._comboFilterKey.SelectedIndex > -1 Then
            Me._buttonRemoveKey.Enabled = True
            Me._buttonRemoveKeyAll.Enabled = True
            Me._buttonAppKeyAdd.Enabled = True
        Else
            Me._buttonRemoveKey.Enabled = False
            Me._buttonRemoveKeyAll.Enabled = False
            Me._buttonAppKeyAdd.Enabled = False
        End If

    End Sub

#End Region

#Region " Grid"

    Private Sub _gridConfigList_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _gridConfigList.MouseDown

        If e.Button = Windows.Forms.MouseButtons.Right Then
            Me._gridConfigList.Selected.Rows.Clear()
            Me._gridConfigList.ActiveRow = Nothing
            Dim mousePoint As Point = New Point(e.X, e.Y)
            'get the user interface element from the location of the mouse click
            Dim element As Infragistics.Win.UIElement = _gridConfigList.DisplayLayout.UIElement.ElementFromPoint(mousePoint)
            Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow = Nothing
            'see if that element is an ultragridrow
            row = CType(element.GetContext(GetType(Infragistics.Win.UltraWinGrid.UltraGridRow)), Infragistics.Win.UltraWinGrid.UltraGridRow)
            If row IsNot Nothing Then
                'select the row first
                Me._gridConfigList.ActiveRow = row
                row.Selected = True
            End If
        End If

    End Sub

    Private Sub _gridConfigList_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles _gridConfigList.DoubleClickRow

        If e.Row.Index > -1 Then

            Dim _deleted As Boolean = Me._gridConfigList.ActiveRow.Cells("Deleted").Value

            If Not _deleted Then

                Dim _appConfigValue As New AppConfigValueBO

                _appConfigValue.EnvironmentID = Me._gridConfigList.ActiveRow.Cells("EnvironmentID").Value
                _appConfigValue.ApplicationID = Me._gridConfigList.ActiveRow.Cells("ApplicationID").Value
                _appConfigValue.KeyID = Me._gridConfigList.ActiveRow.Cells("KeyID").Value
                _appConfigValue.Value = Me._gridConfigList.ActiveRow.Cells("Value").Value

                UpdateKeyValueForm = New Form_ConfigurationData_KeyValue
                UpdateKeyValueForm.AppConfigValue = _appConfigValue
                UpdateKeyValueForm.ApplicationName = Me._gridConfigList.ActiveRow.Cells("AppName").Value
                UpdateKeyValueForm.EnvironmentName = Me._gridConfigList.ActiveRow.Cells("EnvName").Value
                UpdateKeyValueForm.KeyName = Me._gridConfigList.ActiveRow.Cells("KeyName").Value

                UpdateKeyValueForm.ShowDialog()
                UpdateKeyValueForm.Dispose()
                Me._gridConfigList.ActiveRow = e.Row
                e.Row.Selected = True

            End If
        End If

    End Sub

#End Region

#Region " ContextMenu"

    Private Sub _contextMenuGrid_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _contextMenuGrid.Opening

        Dim _deleted As Boolean = False

        If Not Me._gridConfigList.ActiveRow Is Nothing Then
            _deleted = Me._gridConfigList.ActiveRow.Cells("Deleted").Value
            If _deleted Then
                ' if the key/value pair has been deleted, don't show the context menu
                e.Cancel = True
            End If
        Else
            e.Cancel = True
        End If

    End Sub

    Private Sub EditToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditToolStripMenuItem.Click

        If Me._gridConfigList.ActiveRow Is Nothing Then Exit Sub

        Dim _appConfigValue As New AppConfigValueBO

        _appConfigValue.ApplicationID = Me._gridConfigList.ActiveRow.Cells("ApplicationID").Value
        _appConfigValue.EnvironmentID = Me._gridConfigList.ActiveRow.Cells("EnvironmentID").Value
        _appConfigValue.KeyID = Me._gridConfigList.ActiveRow.Cells("KeyID").Value
        _appConfigValue.Value = Me._gridConfigList.ActiveRow.Cells("Value").Value

        UpdateKeyValueForm = New Form_ConfigurationData_KeyValue
        UpdateKeyValueForm.AppConfigValue = _appConfigValue
        UpdateKeyValueForm.ApplicationName = Me._gridConfigList.ActiveRow.Cells("AppName").Value
        UpdateKeyValueForm.EnvironmentName = Me._gridConfigList.ActiveRow.Cells("EnvName").Value
        UpdateKeyValueForm.KeyName = Me._gridConfigList.ActiveRow.Cells("KeyName").Value

        UpdateKeyValueForm.ShowDialog()
        UpdateKeyValueForm.Dispose()

    End Sub

    Private Sub RemoveToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RemoveToolStripMenuItem.Click

        If Me._gridConfigList.ActiveRow Is Nothing Then Exit Sub

        If MessageBox.Show("Remove the " & Me._gridConfigList.ActiveRow.Cells("KeyName").Value.ToString & _
                            " key from " & Me._gridConfigList.ActiveRow.Cells("EnvName").Value.ToString & _
                            " - " & Me._gridConfigList.ActiveRow.Cells("AppName").Value.ToString & "?" _
                            , "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then

            Dim _appConfigValue As New AppConfigValueBO

            _appConfigValue.ApplicationID = Me._gridConfigList.ActiveRow.Cells("ApplicationID").Value
            _appConfigValue.EnvironmentID = Me._gridConfigList.ActiveRow.Cells("EnvironmentID").Value
            _appConfigValue.KeyID = Me._gridConfigList.ActiveRow.Cells("KeyID").Value
            _appConfigValue.Value = Me._gridConfigList.ActiveRow.Cells("Value").Value
            _appConfigValue.UserID = My.Application.CurrentUserID

            _appConfigValue.Remove(_appConfigValue)

            Me.RefreshConfigList()

            MessageBox.Show("Key/Value pair removed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If

    End Sub

#End Region

#End Region

#Region " Input Validation"

    Private Function ValidateInput() As Boolean

        ValidateInput = True

        Me._formErrorProvider.Clear()

        If Me._textFilterValue.Text.Length = 0 Then
            If MessageBox.Show("A value for this Key has not been entered." & vbCrLf & "Would you like to save an empty value for this Key?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                Me._formErrorProvider.SetError(Me._textFilterValue, "Required")
                ValidateInput = False
            End If
        End If

        If Me._comboFilterKey.SelectedIndex = -1 Then
            Me._formErrorProvider.SetError(Me._comboFilterKey, "Required")
            ValidateInput = False
        End If

        If Me._comboFilterEnv.SelectedIndex = -1 Then
            Me._formErrorProvider.SetError(Me._comboFilterEnv, "Required")
            ValidateInput = False
        End If

        If Me._comboFilterApp.SelectedIndex = -1 Then
            Me._formErrorProvider.SetError(Me._comboFilterApp, "Required")
            ValidateInput = False
        End If

        Dim dtAppConfigList As DataTable = daoConfig.GetConfigList()

        Dim _app As Guid = Me._comboFilterApp.SelectedValue
        Dim _env As Guid = Me._comboFilterEnv.SelectedValue

        Dim _expression As String = "ApplicationID = '" & _app.ToString("D") & "' AND EnvironmentID = '" & _env.ToString("D") & "' AND KeyID = " & CInt(Me._comboFilterKey.SelectedValue)

        Dim _rows() As DataRow

        _rows = dtAppConfigList.Select(_expression)

        If _rows.Length > 0 Then

            ValidateInput = False

            MessageBox.Show("That Key/Value pair already exists for this Application and Environment.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If

        Return ValidateInput

    End Function

#End Region

#Region " Refresh Data Sources"

    Public Sub RefreshConfigList()

        Try

            Dim dtAppConfigList As DataTable = daoConfig.GetConfigList()

            Me._gridConfigList.DataSource = Nothing
            Me._gridConfigList.DataSource = dtAppConfigList

            If (Me._gridConfigList.Rows.Count > 0) Then

                Me._gridConfigList.DisplayLayout.Bands(0).Columns("EnvironmentID").Hidden = True
                Me._gridConfigList.DisplayLayout.Bands(0).Columns("EnvName").Header.Caption = "Environment"

                Me._gridConfigList.DisplayLayout.Bands(0).Columns("ApplicationID").Hidden = True
                Me._gridConfigList.DisplayLayout.Bands(0).Columns("AppName").Header.Caption = "Application"

                Me._gridConfigList.DisplayLayout.Bands(0).Columns("TypeID").Hidden = True
                Me._gridConfigList.DisplayLayout.Bands(0).Columns("TypeName").Header.Caption = "Type"

                Me._gridConfigList.DisplayLayout.Bands(0).Columns("KeyID").Hidden = True
                Me._gridConfigList.DisplayLayout.Bands(0).Columns("KeyName").Header.Caption = "Key"

            End If

        Catch ex As Exception

            Logger.LogError("Exception: ", Me.GetType(), ex)
            MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
            Dim args(1) As String
            args(0) = Me.Name & " form sub: RefreshConfigList()"
            ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

        End Try

        If Me._filterApplied Then Me.ApplyFilter()

    End Sub

    Private Sub RefreshApplications()

        If Me.bIsLoading Then Exit Sub

        Try

            Me._comboFilterApp.BeginUpdate()

            If Not Me._comboFilterEnv.SelectedIndex = -1 Then
                Dim dtAppConfigAppList As DataTable = daoConfig.GetApplicationList(CType(Me._comboFilterEnv.SelectedValue, Guid))
                Me._comboFilterApp.DataSource = dtAppConfigAppList
                Me._comboFilterApp.ValueMember = "ApplicationID"
                Me._comboFilterApp.DisplayMember = "Name"
                Me._comboFilterApp.SelectedIndex = -1
                Me._comboFilterApp.Text = String.Empty
            End If

            Me._comboFilterApp.EndUpdate()

        Catch ex As Exception

            Logger.LogError("Exception: ", Me.GetType(), ex)
            MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
            Dim args(1) As String
            args(0) = Me.Name & " form sub: RefreshApplications()"
            ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

        End Try

    End Sub

    Private Sub RefreshKeys()

        If Me.bIsLoading Then Exit Sub

        Try

            Dim dtAppConfigKey As DataTable = daoConfig.GetApplicationKeyList()

            Me._comboFilterKey.BeginUpdate()
            Me._comboFilterKey.DataSource = dtAppConfigKey
            Me._comboFilterKey.ValueMember = "KeyID"
            Me._comboFilterKey.DisplayMember = "Name"
            Me._comboFilterKey.SelectedIndex = -1
            Me._comboFilterKey.EndUpdate()

        Catch ex As Exception

            Logger.LogError("Exception: ", Me.GetType(), ex)
            MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
            Dim args(1) As String
            args(0) = Me.Name & " form sub: RefreshKeys()"
            ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

        End Try

    End Sub

    Private Sub RefreshEnvironments()

        If Me.bIsLoading Then Exit Sub

        Try

            Dim dtAppConfigEnvList As DataTable = daoConfig.GetEnvironmentList()

            Me._comboFilterEnv.BeginUpdate()
            Me._comboFilterEnv.DataSource = dtAppConfigEnvList
            Me._comboFilterEnv.ValueMember = "EnvironmentID"
            Me._comboFilterEnv.DisplayMember = "Name"
            Me._comboFilterEnv.SelectedIndex = -1
            Me._comboFilterEnv.EndUpdate()

        Catch ex As Exception

            Logger.LogError("Exception: ", Me.GetType(), ex)
            MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
            Dim args(1) As String
            args(0) = Me.Name & " form sub: RefreshEnvironments()"
            ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

        End Try

    End Sub

#End Region

#Region " BackgroundWorker Events"

    Private Sub _formImportWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles _formImportWorker.DoWork

        Dim node As Xml.XmlNode = Nothing

        Dim _appConfigValue As AppConfigValueBO = e.Argument

        Try

            Dim _doc As New Xml.XmlDocument
            _doc.Load(Me._formOpenFileDialog.FileName)

            Dim nodeList As Xml.XmlNodeList = _doc.SelectNodes("/configuration/appSettings/add")

            For Each node In nodeList

                _appConfigValue.ApplicationID = _appConfigValue.ApplicationID
                _appConfigValue.EnvironmentID = _appConfigValue.EnvironmentID
                _appConfigValue.UserID = _appConfigValue.UserID
                _appConfigValue.KeyID = daoConfig.ImportKey(New AppConfigKeyBO(node.Attributes("key").Value, 0, _appConfigValue.UserID)).KeyID
                _appConfigValue.Value = node.Attributes("value").Value

                _appConfigValue.Add(_appConfigValue, True)

                _formImportWorker.ReportProgress(0)

            Next

            e.Result = "Success"

        Catch ex As Exception

            e.Result = ex

            Logger.LogError("Exception: ", Me.GetType(), ex)
            Dim args(1) As String
            args(0) = Me.Name & " form event: _formImportWorker_DoWork()"
            ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

        End Try

    End Sub

    Private Sub _formImportWorker_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles _formImportWorker.ProgressChanged
        Me._formProgressBar.PerformStep()
    End Sub

    Private Sub _formImportWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles _formImportWorker.RunWorkerCompleted

        Me._formStatusStrip.Visible = False
        Me._formProgressBar.Value = 0

        If e.Result.ToString = "Success" Then
            MessageBox.Show("Import complete.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.RefreshKeys()
            Me.RefreshConfigList()
        Else
            MessageBox.Show(e.Error.Message & vbCrLf & vbCrLf & e.Error.StackTrace, "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

#End Region

End Class