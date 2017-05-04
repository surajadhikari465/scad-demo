Imports System.Text
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Public Class Form_EditDataArchive

#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    ''' The current action that is being performed: New or Edit
    ''' </summary>
    ''' <remarks></remarks>
    Private _currentAction As FormAction
    ''' <summary>
    ''' Value of the current archive configuration for edits
    ''' </summary>
    ''' <remarks></remarks>
    Private _archiveTableConfig As DataArchiveBO
    ''' <summary>
    ''' Flag to keep track of archive changes that have not been saved.
    ''' </summary>
    ''' <remarks></remarks>
    Private hasChanges As Boolean

    ''' <summary>
    ''' True when the form is being loaded.
    ''' </summary>
    ''' <remarks></remarks>
    Private _initializing As Boolean

    ''' <summary>
    ''' Stores the valid titles in the Titles table.
    ''' </summary>
    ''' <remarks></remarks>
    Private _titles As DataTable

    
#End Region

#Region "Events raised by this form"
    ''' <summary>
    ''' This event is raised when a child form makes a change that requires the
    ''' data on the calling form to be updated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UpdateCallingForm()

    ''' <summary>
    ''' This event is raised whenver form data is changed to indicate that it needs to be saved.
    ''' </summary>
    ''' <remarks></remarks>
    Private Event FormDataChanged()

#End Region

#Region "Events handled by this form"

#Region "Load Form"
    ''' <summary>
    ''' Load the form, pre-filling with the existing data for an edit or querying the database to populate 
    ''' the form for an add.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_EditDataArchive_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Logger.LogDebug("Form_EditDataArchive_Load entry", Me.GetType())

        Me._initializing = True

        'set up button and title bar labels
        LoadText()

        Try

            ' Populate drop-down selectors
            PopulateData()

            ' Show/Hide form controls and pre-fill data based on the form action
            Select Case _currentAction
                Case FormAction.Create
                    ' Initialize the Archive Table
                    _archiveTableConfig = New DataArchiveBO()

                    CheckBox_ArchiveEnabled.Checked = True
                    Me.Button_DisableArchive.Enabled = False

                Case FormAction.Edit
                    ' Default the changes flag 
                    hasChanges = False

                    CheckBox_ArchiveEnabled.Checked = _archiveTableConfig.IsEnabled
                    If _archiveTableConfig.IsEnabled = False Then
                        Me.Button_Save.Enabled = False
                        Me.Button_DisableArchive.Enabled = False
                    Else
                        CheckBox_ArchiveEnabled.Enabled = False
                    End If

            End Select

            ' Informational data about the Archived Data
            ComboBox_ArchiveTable.Text = _archiveTableConfig.TableName
            ComboBox_ChangeType.Text = _archiveTableConfig.ChangeTypeName
            ComboBox_JobFrequency.Text = _archiveTableConfig.JobFrequencyName
            TextBox_RetentionDays.Text = _archiveTableConfig.RetentionDays
            TextBox_LastUpdate.Text = _archiveTableConfig.LastUpdate
            TextBox_UpdatedBy.Text = _archiveTableConfig.FullName

            Select Case _archiveTableConfig.ProcessStatus
                Case 0
                    TextBox_ProcessStatus.Text = "New"
                Case 1
                    TextBox_ProcessStatus.Text = "Successful"
                Case Else
                    TextBox_ProcessStatus.Text = "Failed"
            End Select

        Catch ex As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), ex)
            ' Display a message to the user
            DisplayErrorMessage(ERROR_DB)
            ' Send message about exception
            Dim args(1) As String
            args(0) = "Form_EditDataArchive form: Form_EditDataArchive_Load sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        Finally
            _initializing = False
        End Try
        Logger.LogDebug("Form_EditDataArchive_Load exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' Sets localized string text for buttons and title bar.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadText()
        Select Case _currentAction
            Case FormAction.Create
                Me.Text = ResourcesAdministration.GetString("Add Data Archive Info")
            Case FormAction.Edit
                Me.Text = ResourcesAdministration.GetString("Edit Data Archive Info")
        End Select
    End Sub

    ''' <summary>
    ''' Pre-populates the various drop-downs on the form.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateData()

        ' Populate Tables
        Me.ComboBox_ArchiveTable.DataSource = (TableDAO.GetTables)
        Me.ComboBox_ArchiveTable.DisplayMember = "name"
        Me.ComboBox_ArchiveTable.ValueMember = "object_id"
        Me.ComboBox_ArchiveTable.SelectedIndex = -1

        ' Populate Change Types
        Me.ComboBox_ChangeType.DataSource = ChangeTypeDAO.GetChangeTypes()
        Me.ComboBox_ChangeType.DisplayMember = "ChangeTypeName"
        Me.ComboBox_ChangeType.ValueMember = "ChangeTypeID"
        Me.ComboBox_ChangeType.SelectedIndex = -1

        ' Populate Job Frequency
        Me.ComboBox_JobFrequency.DataSource = JobFrequencyDAO.GetJobFrequency()
        Me.ComboBox_JobFrequency.DisplayMember = "JobFrequencyName"
        Me.ComboBox_JobFrequency.ValueMember = "JobFrequencyID"
        Me.ComboBox_JobFrequency.SelectedIndex = -1

    End Sub
    
#End Region

#Region "Save Button"

    ''' <summary>
    ''' Save the changes to the database and update the parent form.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessSave()
        Logger.LogDebug("ProcessSave entry", Me.GetType())
        Try

            If ValidateData() Then

                Dim success As Boolean = False

                ' Populate the business object with the form data and save the change to the database
                _archiveTableConfig.IsEnabled = CheckBox_ArchiveEnabled.Checked
                _archiveTableConfig.FullName = TextBox_UpdatedBy.Text.Trim
                _archiveTableConfig.ChangeTypeID = ComboBox_ChangeType.SelectedValue

                ' Validate data
                Dim statusList As ArrayList = _archiveTableConfig.ValidateArchiveConfigData()
                Dim statusEnum As IEnumerator = statusList.GetEnumerator
                Dim message As New StringBuilder
                Dim currentStatus As ArchiveConfigStatus

                'loop through possible validation erorrs and build message string containing all errors
                While statusEnum.MoveNext
                    currentStatus = CType(statusEnum.Current, ArchiveConfigStatus)

                    Select Case currentStatus
                        Case ArchiveConfigStatus.Error_Required_TableName
                            message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Label_TableName.Text))
                            message.Append(Environment.NewLine)
                    End Select
                End While

                ' Save the data or display a validation error message
                If message.Length <= 0 Then
                    success = True

                    ' Data is valid - perform insert/update
                    Select Case _currentAction
                        Case FormAction.Create
                            DataArchiveDAO.AddDataArchiveRecord(_archiveTableConfig)
                        Case FormAction.Edit
                            DataArchiveDAO.UpdateDataArchiveRecord(_archiveTableConfig)
                    End Select

                    ' Set the changes flag to false because they've been saved
                    Me.hasChanges = False

                    ' Raise the save event - allows the data on the parent form to be refreshed
                    RaiseEvent UpdateCallingForm()


                    Me.Button_Save.Enabled = False
                    If _archiveTableConfig.IsEnabled Then
                        Me.Button_DisableArchive.Enabled = True
                        Me.CheckBox_ArchiveEnabled.Enabled = False
                    End If

                    Me._currentAction = FormAction.Edit

                Else
                    success = False
                    ' Display error msg
                    MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

            End If

        Catch ex As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), ex)
            ' Display a message to the user
            DisplayErrorMessage(ERROR_DB)
            ' Send message about exception
            Dim args(0) As String
            args(0) = "Form_EditDataArchive form: ProcessSave sub"
            ErrorHandler.ProcessError(ErrorType.DataFactoryException, args, SeverityLevel.Warning, ex)
            ' Close the child window
            Me.Close()
        End Try
        Logger.LogDebug("ProcessSave exit", Me.GetType())

        Me.Cursor = Cursors.Default

    End Sub

    ''' <summary>
    ''' The user selected the Save button - apply the changes.
    ''' Close the form and return focus to the View Stores form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Save.Click
        Logger.LogDebug("Button_Save_Click entry", Me.GetType())
        ' Save the updates to the database
        Me.Cursor = Cursors.WaitCursor
        ProcessSave()
        Me.Cursor = Cursors.Default
        Logger.LogDebug("Button_Save_Click exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' Validates the user form data. Invalid data is highlighted using the error provider control.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateData() As Boolean

        Logger.LogDebug("ValidateData entry", Me.GetType())

        Me.Form_ErrorProvider.Clear()

        Dim errCount As Integer = 0

        If Me.CheckBox_ArchiveEnabled.Checked Then

            If Me.ComboBox_ArchiveTable.Text.Length = 0 Then
                Me.Form_ErrorProvider.SetError(Me.ComboBox_ArchiveTable, "Required")
                errCount = errCount + 1
            End If

            If Me.ComboBox_ChangeType.Text.Length = 0 Then
                Me.Form_ErrorProvider.SetError(Me.ComboBox_ChangeType, "Required")
                errCount = errCount + 1
            End If

            If Me.ComboBox_JobFrequency.Text.Length = 0 Then
                Me.Form_ErrorProvider.SetError(Me.ComboBox_JobFrequency, "Required")
                errCount = errCount + 1
            End If

            If Me.TextBox_RetentionDays.Text.Length = 0 Then
                Me.Form_ErrorProvider.SetError(Me.TextBox_RetentionDays, "Required")
                errCount = errCount + 1
            End If

        Else
            errCount = 0
        End If


        If errCount = 0 Then
            Me.Form_ErrorProvider.Clear()
            Return True
        Else
            Return False
        End If

        Logger.LogDebug("ValidateData exit", Me.GetType())

    End Function

#End Region

#Region "Cancel Button"
    ''' <summary>
    ''' The user selected the Cancel button - do not save the changes.
    ''' Close the form and return focus to the View Stores form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Logger.LogDebug("Button_Cancel_Click entry", Me.GetType())
        ' Set the flag so they are not prompted to save
        hasChanges = False
        ' Close the child window
        Me.Close()
        Logger.LogDebug("Button_Cancel_Click exit", Me.GetType())
    End Sub
#End Region

#Region "Close Form"
    ''' <summary>
    ''' Confirm user wants to save any changed data when they are clicking 'X' button in top-right of window
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_EditUser_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If hasChanges Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), ResourcesCommon.GetString("msg_titleConfirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                ' Save the updates to the database
                If Me.ValidateData Then
                    ProcessSave()
                Else
                    e.Cancel = True
                End If
            End If
        End If
    End Sub
#End Region

#Region "Edit data"

    ''' <summary>
    ''' Sets the hasChanges form indicator to True and enables the Save button.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormHasChanges() Handles Me.FormDataChanged

        If Me.CheckBox_ArchiveEnabled.Checked Then
            Me.hasChanges = True
            Me.Button_Save.Enabled = True
        End If

    End Sub

    Private Sub ComboBox_ArchiveTable_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_ArchiveTable.TextChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub ComboBox_ChangeType_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_ChangeType.TextChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub ComboBox_JobFrequency_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_JobFrequency.TextChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_AcctEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_ArchiveEnabled.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub TextBox_UserName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub TextBox_Title_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub TextBox_FullName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    ''' <summary>
    ''' This event triggers UI changes based on the CheckState of the CheckBox_RolePromoStoreBuyer check box.
    ''' </summary>
    ''' <remarks>Certain conditions exist in PromoPlanner that require limiting the
    '''selection of certain fields to only the hard-coded vlues PromoPlanner accepts.</remarks>
   
    Private Sub ProcessDisable()

        Logger.LogDebug("ProcessDisable Enter", Me.GetType())
        ''Try
        ''    Me.Cursor = Cursors.WaitCursor
        ''    Me.Button_DisableArchive.Enabled = False
        ''    UserDAO.DeleteUserRecord(_userConfig.UserId)
        ''    hasChanges = False
        ''    ' Raise the save event - allows the data on the parent form to be refreshed
        ''    RaiseEvent UpdateCallingForm()
        ''    Me.Cursor = Cursors.Default
        ''    Me.Close()
        ''Catch ex As Exception
        ''    Logger.LogError("Exception: ", Me.GetType(), ex)
        ''    'display a message to the user
        ''    DisplayErrorMessage(ex.Message)
        ''    'send message about exception
        ''    Dim args(0) As String
        ''    args(0) = "Form_EditUser form: ProcessDisable sub"
        ''    ErrorHandler.ProcessError(ErrorType.Administration_UserDisableAccount, args, SeverityLevel.Warning, ex)
        ''    ' Close the child window
        ''    Me.Close()
        ''Finally
        ''    Me.Cursor = Cursors.Default
        ''End Try
        Logger.LogDebug("ProcessDisable exit", Me.GetType())

    End Sub

    ''' <summary>
    ''' Disables the user account immediately.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_DisableAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_DisableArchive.Click
        ProcessDisable()
    End Sub

#End Region

#End Region

#Region "Property Definitions"

    Public Property CurrentAction() As FormAction
        Get
            Return _currentAction
        End Get
        Set(ByVal value As FormAction)
            _currentAction = value
        End Set
    End Property

    Public Property DataArchiveConfig() As DataArchiveBO
        Get
            Return _archiveTableConfig
        End Get
        Set(ByVal value As DataArchiveBO)
            _archiveTableConfig = value
        End Set
    End Property
#End Region

    
End Class
