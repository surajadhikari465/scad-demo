Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.IRMA.Administration.StoreAdmin.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_EditStore

    Private _regionalScaleHosting As Boolean
    Private _storeBO As StoreBO
    Private _storePOSConfig As StorePOSConfigBO
    Private _storeScaleConfig As StoreScaleConfigBO
    Private _storeTagConfig As StorePOSConfigBO
    Private _storeElectronicShelfTagConfig As StorePOSConfigBO
    Private _hasChanges As Boolean

#Region "property access methods"

    Public Property RegionalScaleHosting() As Boolean
        Get
            Return _regionalScaleHosting
        End Get
        Set(ByVal value As Boolean)
            _regionalScaleHosting = value
        End Set
    End Property

    Public Property StoreBO() As StoreBO
        Get
            Return _storeBO
        End Get
        Set(ByVal value As StoreBO)
            _storeBO = value
        End Set
    End Property

#End Region

#Region "Events raised by this form"
    ''' <summary>
    ''' This event is raised when a child form makes a change that requires the
    ''' data on the calling form to be updated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UpdateCallingForm()
#End Region

#Region "form & button events"

    Private Sub Form_EditStore_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CenterToScreen()
        LoadData()
        SetupWriterConfigData()
    End Sub

    Private Sub Form_EditStore_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If _hasChanges Then
            'prompt the user to save changes
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                If ApplyChanges() Then
                    'close form
                    Me.Close()
                Else
                    e.Cancel = True
                End If
            End If
        End If
    End Sub

    Private Sub Button_FTPInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_FTPInfo.Click
        'open form
        Dim ftpConfigForm As New Form_ManageStoreFtpInfo
        ftpConfigForm.StoreConfig = _storeBO
        ftpConfigForm.ShowDialog(Me)
        ftpConfigForm.Dispose()
    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

    Private Sub Button_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Save.Click
        Me.Close()
    End Sub

    Private Sub Button_ConfigureWriters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ConfigureWriters.Click
        Dim editStorePOSConfigForm As New Form_EditStorePOSConfig()
        editStorePOSConfigForm.RegionalScaleHosting = Me.RegionalScaleHosting

        'if writers exist, send user to 'edit' screen; otherwise to 'add' screen
        If _storePOSConfig IsNot Nothing Then
            editStorePOSConfigForm.CurrentAction = FormAction.Edit
            editStorePOSConfigForm.StorePOSConfig = _storePOSConfig
        Else
            editStorePOSConfigForm.CurrentAction = FormAction.Create

            Dim storePOSConfig As New StorePOSConfigBO
            storePOSConfig.StoreConfig = _storeBO
            editStorePOSConfigForm.StorePOSConfig = storePOSConfig
        End If

        If _storeScaleConfig IsNot Nothing And _storeScaleConfig.ScaleFileWriterKey IsNot Nothing Then
            editStorePOSConfigForm.StoreScaleConfig = _storeScaleConfig
        Else
            _storeScaleConfig = New StoreScaleConfigBO
            _storeScaleConfig.StoreConfig = _storeBO
            editStorePOSConfigForm.StoreScaleConfig = _storeScaleConfig
        End If
        ' Check For TagConfig Object and create one if Nothing
        If _storeTagConfig IsNot Nothing AndAlso _storeTagConfig.POSFileWriterKey <> 0 Then
            editStorePOSConfigForm.StoreTagConfig = _storeTagConfig
        Else
            'Dim storeTagConfig As New StorePOSConfigBO
            'storeTagConfig.StoreConfig = _storeBO
            'editStorePOSConfigForm.StoreTagConfig = storeTagConfig
            _storeTagConfig = New StorePOSConfigBO
            _storeTagConfig.StoreConfig = _storeBO
            editStorePOSConfigForm.StoreTagConfig = _storeTagConfig
        End If

        If _storeElectronicShelfTagConfig IsNot Nothing AndAlso _storeElectronicShelfTagConfig.POSFileWriterKey <> 0 Then
            editStorePOSConfigForm.StoreElectronicShelfTagConfig = _storeElectronicShelfTagConfig
        Else
            _storeElectronicShelfTagConfig = New StorePOSConfigBO
            _storeElectronicShelfTagConfig.StoreConfig = _storeBO
            editStorePOSConfigForm.StoreElectronicShelfTagConfig = _storeElectronicShelfTagConfig
        End If

        'open form
        editStorePOSConfigForm.ShowDialog(Me)
        editStorePOSConfigForm.Dispose()
    End Sub

    Private Sub ComboBox_POSSystem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_POSSystem.SelectedIndexChanged
        _hasChanges = True
    End Sub

#End Region

    Private Sub LoadData()
        Me.Label_StoreNoValue.Text = _storeBO.StoreNo.ToString
        Me.Label_StoreNameValue.Text = _storeBO.StoreName

        'populate POS System drop down
        Dim systemList As ArrayList = POSSystemTypeDAO.GetPOSSystemTypes
        Me.ComboBox_POSSystem.DataSource = systemList
        If systemList.Count > 0 Then
            Me.ComboBox_POSSystem.ValueMember = "POSSystemID"
            Me.ComboBox_POSSystem.DisplayMember = "POSSystemType"

            'set pos system value if one exists, otherwise set to blank
            If _storeBO.POSSystem IsNot Nothing AndAlso _storeBO.POSSystem.POSSystemID > 0 Then
                Me.ComboBox_POSSystem.SelectedValue = _storeBO.POSSystem.POSSystemID
            Else
                Me.ComboBox_POSSystem.SelectedIndex = -1
            End If
        End If

        _hasChanges = False
    End Sub

    Private Sub SetupWriterConfigData()
        'determine if store has writer data configured
        _storePOSConfig = StoreWriterConfigDAO.GetStorePOSConfiguration(_storeBO.StoreNo)
        _storeScaleConfig = StoreWriterConfigDAO.GetStoreScaleConfiguration(_storeBO.StoreNo, POSWriterBO.SCALE_WRITER_TYPE_STORE)
        _storeTagConfig = StoreWriterConfigDAO.GetStoreShelfTagWriterConfiguration(_storeBO.StoreNo, POSWriterBO.WRITER_TYPE_TAG)
        _storeElectronicShelfTagConfig = StoreWriterConfigDAO.GetStoreElectronicShelfTagWriterConfiguration(_storeBO.StoreNo, POSWriterBO.WRITER_TYPE_ELECTRONICSHELFTAG)
    End Sub

    ''' <summary>
    ''' save data
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ApplyChanges() As Boolean
        Dim success As Boolean = False
        Dim storeDAO As New StoreDAO

        'set values from entry screen
        Dim posSystem As New POSSystemType
        posSystem.POSSystemID = CType(Me.ComboBox_POSSystem.SelectedValue, Integer)
        posSystem.POSSystemType = Me.ComboBox_POSSystem.SelectedText
        _storeBO.POSSystem = posSystem

        Try
            'save data
            storeDAO.UpdateStore(_storeBO)

            success = True

            'data has been saved - reset the edit flag
            _hasChanges = False

            ' Raise event - allows the data on the parent form to be refreshed
            RaiseEvent UpdateCallingForm()
        Catch ex As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), ex)
            'display a message to the user
            MessageBox.Show(Form_IRMABase.ERROR_DB, "IRMA Application Error", MessageBoxButtons.OK)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_EditStore form: ApplyChanges sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try

        Return success
    End Function

End Class