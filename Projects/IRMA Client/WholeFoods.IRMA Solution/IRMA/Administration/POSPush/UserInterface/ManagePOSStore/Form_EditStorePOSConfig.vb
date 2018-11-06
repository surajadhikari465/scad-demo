Imports System.Text
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_EditStorePOSConfig
#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    ''' The current action that is being performed: New or Edit
    ''' </summary>
    ''' <remarks></remarks>
    Private _currentAction As FormAction
    ''' <summary>
    ''' Flag that tracks the value of the regional scale hosting option
    ''' </summary>
    ''' <remarks></remarks>
    Private _regionalScaleHosting As Boolean
    ''' <summary>
    ''' Value of the current store POS configuration for edits
    ''' </summary>
    ''' <remarks></remarks>
    Private _storePOSConfig As StorePOSConfigBO
    ''' <summary>
    ''' Value of the current store POS configuration for edits
    ''' </summary>
    ''' <remarks></remarks>
    Private _storeTagConfig As StorePOSConfigBO
    ''' <summary>
    ''' Value of the current store Scale configuration for edits
    ''' </summary>
    ''' <remarks></remarks>
    Private _storeElectronicShelfTagConfig As StorePOSConfigBO
    ''' <summary>
    ''' Value of the current store Scale configuration for edits
    ''' </summary>
    ''' <remarks></remarks>
    Private _storeScaleConfig As StoreScaleConfigBO
    ''' <summary>
    ''' Flag to keep track of user changes that have not been saved.
    ''' </summary>
    ''' <remarks></remarks>
    Private hasChanges As Boolean
    Private deleteFileWriter As Boolean = False
    Private deleteScaleWriter As Boolean = False
    Private deleteTagWriter As Boolean = False
#End Region

#Region "Events raised by this form"
    ''' <summary>
    ''' This event is raised when a child form makes a change that requires the
    ''' data on the calling form to be updated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UpdateCallingForm()
#End Region

#Region "Events handled by this form"
#Region "Load Form"
    ''' <summary>
    ''' Load the form, pre-filling with the existing data for an edit or querying the database to populate 
    ''' the available stores for an add.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_EditStorePOSConfig_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Logger.LogDebug("Form_EditStorePOSConfig_Load entry", Me.GetType())
        Me.CenterToScreen()

        'set up button and title bar labels
        LoadText()

        Try
            ' Populate the selections for File Writer 
            Me.ComboBox_FileWriterVal.DataSource = POSWriterDAO.GetFileWriters(POSWriterBO.WRITER_TYPE_POS).Tables(0)
            Me.ComboBox_FileWriterVal.DisplayMember = "POSFileWriterCode"
            Me.ComboBox_FileWriterVal.ValueMember = "POSFileWriterKey"
            Me.ComboBox_FileWriterVal.SelectedIndex = -1

            ' Populate the selections for Scale Writer 
            Me.ComboBox_ScaleWriterVal.DataSource = POSWriterDAO.GetFileWriters(POSWriterBO.WRITER_TYPE_SCALE, POSWriterBO.SCALE_WRITER_TYPE_STORE).Tables(0)
            Me.ComboBox_ScaleWriterVal.DisplayMember = "POSFileWriterCode"
            Me.ComboBox_ScaleWriterVal.ValueMember = "POSFileWriterKey"
            Me.ComboBox_ScaleWriterVal.SelectedIndex = -1

            ' Populate the selections for Tag Writer 
            Me.ComboBox_TagWriterVal.DataSource = POSWriterDAO.GetFileWriters(POSWriterBO.WRITER_TYPE_TAG).Tables(0)
            Me.ComboBox_TagWriterVal.DisplayMember = "POSFileWriterCode"
            Me.ComboBox_TagWriterVal.ValueMember = "POSFileWriterKey"
            Me.ComboBox_TagWriterVal.SelectedIndex = -1

            ' Populate the selections for Electronic Shelf Tag Writer 
            Me.ComboBox_ElectronicShelfTagVal.DataSource = POSWriterDAO.GetFileWriters(POSWriterBO.WRITER_TYPE_ELECTRONICSHELFTAG).Tables(0)
            Me.ComboBox_ElectronicShelfTagVal.DisplayMember = "POSFileWriterCode"
            Me.ComboBox_ElectronicShelfTagVal.ValueMember = "POSFileWriterKey"
            Me.ComboBox_ElectronicShelfTagVal.SelectedIndex = -1

            ' Populate the selections for Config Type
            Me.ComboBox_ConfigTypeVal.DataSource = POSConfigTypeDAO.GetConfigTypes()
            Me.ComboBox_ConfigTypeVal.DisplayMember = "Description"
            Me.ComboBox_ConfigTypeVal.ValueMember = "Value"
            Me.ComboBox_ConfigTypeVal.SelectedIndex = -1


            ' Show/Hide form controls and pre-fill data based on the form action
            If _regionalScaleHosting Then
                Me.ComboBox_ScaleWriterVal.Enabled = False
                Me.ComboBox_ScaleWriterVal.SelectedValue = ""
            Else
                Me.ComboBox_ScaleWriterVal.Enabled = True
            End If
            Select Case _currentAction
                Case FormAction.Create
                    ' Populate the list of available stores to add
                    Me.ComboBox_StoreVal.DataSource = StoreWriterConfigDAO.GetStoresNotConfigured().Tables(0)
                    Me.ComboBox_StoreVal.DisplayMember = "Store_Name"
                    Me.ComboBox_StoreVal.ValueMember = "Store_No"
                    Me.ComboBox_StoreVal.SelectedIndex = -1

                    'select store being added
                    Me.ComboBox_StoreVal.SelectedValue = _storePOSConfig.StoreConfig.StoreNo
                    'don't allow user to change selected store
                    Me.ComboBox_StoreVal.Enabled = False
                    Me.ComboBox_StoreVal.Visible = True
                    Me.Label_StoreVal.Visible = False

                    '' initialize the store configurations 
                    '_storePOSConfig = New StorePOSConfigBO()
                    '_storeScaleConfig = New StoreScaleConfigBO()
                    ' Default the changes flag 
                    hasChanges = True
                Case FormAction.Edit
                    Me.ComboBox_StoreVal.Visible = False
                    Me.Label_StoreVal.Visible = True
                    ' Pre-fill the existing values for edits
                    Me.Label_StoreVal.Text = String.Format(ResourcesAdministration.GetString("label_storeDesc"), _storePOSConfig.StoreConfig.StoreNo, _storePOSConfig.StoreConfig.StoreName)
                    Me.ComboBox_ConfigTypeVal.SelectedValue = _storePOSConfig.ConfigType
                    Me.ComboBox_FileWriterVal.SelectedValue = _storePOSConfig.POSFileWriterKey

                    If _storeScaleConfig.ScaleFileWriterKey IsNot Nothing Then
                        Me.ComboBox_ScaleWriterVal.SelectedValue = _storeScaleConfig.ScaleFileWriterKey
                    Else
                        Me.ComboBox_ScaleWriterVal.SelectedValue = 0
                    End If

                    If _storeTagConfig.POSFileWriterKey <> 0 Then
                        Me.ComboBox_TagWriterVal.SelectedValue = _storeTagConfig.POSFileWriterKey
                    Else
                        Me.ComboBox_TagWriterVal.SelectedValue = 0
                    End If

                    If _storeElectronicShelfTagConfig.POSFileWriterKey <> 0 Then
                        Me.ComboBox_ElectronicShelfTagVal.SelectedValue = _storeElectronicShelfTagConfig.POSFileWriterKey
                    Else
                        Me.ComboBox_ElectronicShelfTagVal.SelectedValue = 0
                    End If

                    ' Default the changes flag 
                    hasChanges = False
            End Select
        Catch ex As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), ex)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_POSWriterConfig form: PopulatePOSWriterData sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try
        Logger.LogDebug("Form_EditStorePOSConfig_Load exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' set localized string text for buttons and title bar
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadText()
        Select Case _currentAction
            Case FormAction.Create
                Me.Text = ResourcesAdministration.GetString("label_titleBar_EditStorePOSConfig_Add")
            Case FormAction.Edit
                Me.Text = ResourcesAdministration.GetString("label_titleBar_EditStorePOSConfig_Edit")
        End Select
    End Sub
#End Region

#Region "Save Button"
    ''' <summary>
    ''' Save the changes to the database and update the parent form.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ProcessSave() As Boolean
        Logger.LogDebug("ProcessSave entry", Me.GetType())

        Dim success As Boolean = False
        Dim statusList As ArrayList
        Dim statusEnum As IEnumerator
        Dim message As New StringBuilder
        Dim currentStatus As StorePOSConfigStatus
        Dim tagWriterNew As Boolean = False
        Dim electronicshelftagWriterNew As Boolean = False
        Dim scaleWriterNew As Boolean = False

        Try
            ' Populate the business object with the form data and save the change to the database
            If _currentAction = FormAction.Create Then
                If Me.ComboBox_StoreVal.SelectedValue Is Nothing Then
                    _storePOSConfig.StoreConfig.StoreNo = -1
                Else
                    _storePOSConfig.StoreConfig.StoreNo = CType(Me.ComboBox_StoreVal.SelectedValue, Integer)
                End If
            End If

            If Me.ComboBox_FileWriterVal.SelectedValue Is Nothing Then
                _storePOSConfig.POSFileWriterKey = -1
            Else
                _storePOSConfig.ConfigType = CType(Me.ComboBox_ConfigTypeVal.SelectedValue, String)
                _storePOSConfig.POSFileWriterKey = CType(Me.ComboBox_FileWriterVal.SelectedValue, Integer)
            End If

            If Me.ComboBox_TagWriterVal.SelectedValue Is Nothing Then
                _storeTagConfig.POSFileWriterKey = 0
            Else
                _storeTagConfig.StoreConfig.StoreNo = _storePOSConfig.StoreConfig.StoreNo
                _storeTagConfig.ConfigType = CType(Me.ComboBox_ConfigTypeVal.SelectedValue, String)
                If _storeTagConfig.POSFileWriterKey = 0 Then
                    tagWriterNew = True
                End If
                _storeTagConfig.POSFileWriterKey = CType(Me.ComboBox_TagWriterVal.SelectedValue, Integer)
            End If

            If Me.ComboBox_ElectronicShelfTagVal.SelectedValue Is Nothing Then
                _storeElectronicShelfTagConfig.POSFileWriterKey = 0
            Else
                _storeElectronicShelfTagConfig.StoreConfig.StoreNo = _storePOSConfig.StoreConfig.StoreNo
                _storeElectronicShelfTagConfig.ConfigType = CType(Me.ComboBox_ConfigTypeVal.SelectedValue, String)
                If _storeElectronicShelfTagConfig.POSFileWriterKey = 0 Then
                    electronicshelftagWriterNew = True
                End If
                _storeElectronicShelfTagConfig.POSFileWriterKey = CType(Me.ComboBox_ElectronicShelfTagVal.SelectedValue, Integer)
            End If

            If Me.ComboBox_ScaleWriterVal.SelectedValue Is Nothing Then
                _storeScaleConfig.ScaleFileWriterKey = Nothing
            Else
                If (_storeScaleConfig.ScaleFileWriterKey Is Nothing) Then
                    scaleWriterNew = True                    
                End If
                _storeScaleConfig.ScaleFileWriterKey = CType(Me.ComboBox_ScaleWriterVal.SelectedValue, String)
            End If

            'validate data
            statusList = _storePOSConfig.ValidateStorePOSConfigData

            'loop through possible validation erorrs and build message string containing all errors
            statusEnum = statusList.GetEnumerator
            While statusEnum.MoveNext
                currentStatus = CType(statusEnum.Current, StorePOSConfigStatus)

                Select Case currentStatus
                    Case StorePOSConfigStatus.Error_Required_Store
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_StoreNo.Text))
                        message.Append(Environment.NewLine)
                    Case StorePOSConfigStatus.Error_Required_FileWriter
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_FileWriter.Text))
                        message.Append(Environment.NewLine)
                    Case StorePOSConfigStatus.Error_Required_AcknowledgementType
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_ConfigType.Text))
                        message.Append(Environment.NewLine)
                End Select
            End While

            If message.Length <= 0 Then
                'process save
                Select Case _currentAction
                    Case FormAction.Create
                        'add store pos config data
                        StoreWriterConfigDAO.AddStorePOSConfigRecord(_storePOSConfig)

                        'scale info
                        If Me.ComboBox_ScaleWriterVal.SelectedValue IsNot Nothing Then
                            StoreWriterConfigDAO.AddStoreScaleConfigRecord(_storeScaleConfig)
                        End If

                        'tag info
                        If Me.ComboBox_TagWriterVal.SelectedValue IsNot Nothing Then
                            StoreWriterConfigDAO.AddStoreShelfTagConfigRecord(_storeTagConfig)
                        End If

                        'electronic shelf tag info
                        If Me.ComboBox_ElectronicShelfTagVal.SelectedValue IsNot Nothing Then
                            StoreWriterConfigDAO.AddStoreElectronicShelfTagConfigRecord(_storeElectronicShelfTagConfig)
                        End If
                    Case FormAction.Edit
                        If deleteFileWriter Then
                            StoreWriterConfigDAO.DeleteStorePOSConfigRecord(_storePOSConfig)
                        Else
                            'edit store pos config data
                            StoreWriterConfigDAO.UpdateStorePOSConfigRecord(_storePOSConfig)
                        End If
                        'tag info
                        If tagWriterNew Then
                            StoreWriterConfigDAO.AddStoreShelfTagConfigRecord(_storeTagConfig)
                        ElseIf deleteTagWriter Then
                            StoreWriterConfigDAO.DeleteStoreShelfTagConfigRecord(_storeTagConfig)
                        ElseIf _storeTagConfig.POSFileWriterKey <> 0 Then
                            StoreWriterConfigDAO.UpdateStoreShelfTagConfigRecord(_storeTagConfig)
                        End If

                        'electronic shelf tag info
                        If electronicshelftagWriterNew Then
                            StoreWriterConfigDAO.AddStoreElectronicShelfTagConfigRecord(_storeElectronicShelfTagConfig)
                        ElseIf deleteTagWriter Then
                            StoreWriterConfigDAO.DeleteStoreElectronicShelfTagConfigRecord(_storeElectronicShelfTagConfig)
                        ElseIf _storeElectronicShelfTagConfig.POSFileWriterKey <> 0 Then
                            StoreWriterConfigDAO.UpdateStoreElectronicShelfTagConfigRecord(_storeElectronicShelfTagConfig)
                        End If

                        'scale info
                        If scaleWriterNew Then
                            StoreWriterConfigDAO.AddStoreScaleConfigRecord(_storeScaleConfig)
                        ElseIf deleteScaleWriter Then
                            StoreWriterConfigDAO.DeleteStoreScaleConfigRecord(_storeScaleConfig)
                        ElseIf _storeScaleConfig.ScaleFileWriterKey IsNot Nothing Then
                            StoreWriterConfigDAO.UpdateStoreScaleConfigRecord(_storeScaleConfig)
                        End If
                End Select

                ' Raise the save event - allows the data on the parent form to be refreshed
                RaiseEvent UpdateCallingForm()

                success = True
            Else
                'display error msg
                MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                success = False
            End If

            ' All changes have been saved - reset the flags
            hasChanges = False
        Catch ex As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), ex)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_EditStorePOSConfig form: ProcessSave sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try

        Logger.LogDebug("ProcessSave exit", Me.GetType())

        Return success
    End Function

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
        If ProcessSave() Then
            ' Close the child window
            Me.Close()
        End If
        Logger.LogDebug("Button_Save_Click exit", Me.GetType())
    End Sub
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
    Private Sub Form_EditStorePOSConfig_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If hasChanges Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), ResourcesCommon.GetString("msg_titleConfirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                ' Save the updates to the database
                ProcessSave()
            End If
        End If
    End Sub
#End Region

#Region "Edit data"
    ''' <summary>
    ''' No validation is necessary for a combo box - just set the changed data flag.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ComboBox_StoreVal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_StoreVal.SelectedIndexChanged
        hasChanges = True
    End Sub

    Private Sub ComboBox_FileWriterVal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ComboBox_FileWriterVal.KeyPress
        Dim KeyAscii As Integer = AscW(e.KeyChar)
        If KeyAscii = Keys.Back Then ComboBox_FileWriterVal.SelectedIndex = -1
        deleteFileWriter = True
    End Sub

    ''' <summary>
    ''' No validation is necessary for a combo box - just set the changed data flag.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ComboBox_FileWriterVal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_FileWriterVal.SelectedIndexChanged
        hasChanges = True
    End Sub

    ''' <summary>
    ''' No validation is necessary for a combo box - just set the changed data flag.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ComboBox_ConfigTypeVal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_ConfigTypeVal.SelectedIndexChanged
        hasChanges = True
    End Sub

    Private Sub ComboBox_TagWriterVal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ComboBox_TagWriterVal.KeyPress
        Dim KeyAscii As Integer = AscW(e.KeyChar)
        If KeyAscii = Keys.Back Then
            ComboBox_TagWriterVal.SelectedIndex = -1
            deleteTagWriter = True
        End If
    End Sub

    ''' <summary>
    ''' No validation is necessary for a combo box - just set the changed data flag. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ComboBox_TagWriterVal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_TagWriterVal.SelectedIndexChanged
        hasChanges = True
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

    Public Property StorePOSConfig() As StorePOSConfigBO
        Get
            Return _storePOSConfig
        End Get
        Set(ByVal value As StorePOSConfigBO)
            _storePOSConfig = value
        End Set
    End Property
    Public Property StoreTagConfig() As StorePOSConfigBO
        Get
            Return _storeTagConfig
        End Get
        Set(ByVal value As StorePOSConfigBO)
            _storeTagConfig = value
        End Set
    End Property
    Public Property StoreElectronicShelfTagConfig() As StorePOSConfigBO
        Get
            Return _storeElectronicShelfTagConfig
        End Get
        Set(ByVal value As StorePOSConfigBO)
            _storeElectronicShelfTagConfig = value
        End Set
    End Property
    Public Property RegionalScaleHosting() As Boolean
        Get
            Return _regionalScaleHosting
        End Get
        Set(ByVal value As Boolean)
            _regionalScaleHosting = value
        End Set
    End Property

    Public Property StoreScaleConfig() As StoreScaleConfigBO
        Get
            Return _storeScaleConfig
        End Get
        Set(ByVal value As StoreScaleConfigBO)
            _storeScaleConfig = value
        End Set
    End Property
#End Region

    Private Sub ComboBox_ScaleWriterVal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ComboBox_ScaleWriterVal.KeyPress
        Dim KeyAscii As Integer = AscW(e.KeyChar)
        If KeyAscii = Keys.Back Then ComboBox_ScaleWriterVal.SelectedIndex = -1
        deleteScaleWriter = True
    End Sub

    Private Sub ComboBox_ElectronicShelfTagVal_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles ComboBox_ElectronicShelfTagVal.KeyPress
        Dim KeyAscii As Integer = AscW(e.KeyChar)
        If KeyAscii = Keys.Back Then
            ComboBox_ElectronicShelfTagVal.SelectedIndex = -1
            deleteTagWriter = True
        End If
    End Sub
End Class
