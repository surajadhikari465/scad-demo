Imports System.Text
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_EditStoreFTPInfo

    Private _isEdit As Boolean
    Private _storeFTPConfig As StoreFTPConfigBO
    Private _availableWriterTypes As ArrayList

#Region "property definitions"

    Public Property IsEdit() As Boolean
        Get
            Return _isEdit
        End Get
        Set(ByVal value As Boolean)
            _isEdit = value
        End Set
    End Property

    Public Property StoreFTPConfig() As StoreFTPConfigBO
        Get
            Return _storeFTPConfig
        End Get
        Set(ByVal value As StoreFTPConfigBO)
            _storeFTPConfig = value
        End Set
    End Property

    Public Property AvailableWriterTypes() As ArrayList
        Get
            Return _availableWriterTypes
        End Get
        Set(ByVal value As ArrayList)
            _availableWriterTypes = value
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

#Region "form events"

    Private Sub Form_EditStoreFTPInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.CenterToScreen()

        'enable file writer type drop down if not Edit
        If IsEdit Then
            Me.ComboBox_FileWriterType.Visible = False
            Me.Label_FileWriterTypeValue.Visible = True
        Else
            BindData()
            Me.ComboBox_FileWriterType.Visible = True
            Me.Label_FileWriterTypeValue.Visible = False
        End If

        If _storeFTPConfig IsNot Nothing Then
            InitializeData()
        End If
    End Sub

#End Region

    Private Sub BindData()
        Me.ComboBox_FileWriterType.DataSource = _availableWriterTypes
    End Sub

    Private Sub InitializeData()
        'fill in text boxes w/ existing data
        Me.Label_FileWriterTypeValue.Text = _storeFTPConfig.FileWriterType
        Me.TextBox_IPAddress.Text = _storeFTPConfig.IPAddress
        Me.TextBox_FTPUser.Text = _storeFTPConfig.FTPUser
        Me.TextBox_FTPPassword.Text = _storeFTPConfig.FTPPassword
        Me.TextBox_ChangeDirectory.Text = _storeFTPConfig.ChangeDirectory
        Me.TextBox_Port.Text = _storeFTPConfig.Port
        Me.CheckBox_IsSecureTransfer.Checked = _storeFTPConfig.IsSecureTransfer
    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

    Private Sub Button_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Save.Click
        'save changes
        Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = Windows.Forms.DialogResult.Yes Then
            If ApplyChanges() Then
                ' Raise event - allows the data on the parent form to be refreshed
                RaiseEvent UpdateCallingForm()

                'close form
                Me.Close()
            End If
        End If
    End Sub

    Private Function ApplyChanges() As Boolean
        Dim success As Boolean = False
        Dim statusList As ArrayList
        Dim statusEnum As IEnumerator
        Dim message As New StringBuilder
        Dim currentStatus As StoreFTPConfigStatus
        Dim storeFtpConfigDAO As New StoreFTPConfigDAO

        'get values from form
        UpdateData()

        'validate required data
        statusList = _storeFTPConfig.ValidateFTPData()
        statusEnum = statusList.GetEnumerator

        'loop through possible validation erorrs and build message string containing all errors
        statusEnum = statusList.GetEnumerator
        While statusEnum.MoveNext
            currentStatus = CType(statusEnum.Current, StoreFTPConfigStatus)

            Select Case currentStatus
                Case StoreFTPConfigStatus.Error_Required_FileWriterType
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_FileWriterType.Text))
                    message.Append(Environment.NewLine)
                Case StoreFTPConfigStatus.Error_Required_IPAddress
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_IPAddress.Text))
                    message.Append(Environment.NewLine)
                Case StoreFTPConfigStatus.Error_Required_FTPUser
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_FTPUser.Text))
                    message.Append(Environment.NewLine)
                Case StoreFTPConfigStatus.Error_Required_FTPPassword
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_FTPPassword.Text))
                    message.Append(Environment.NewLine)
                Case StoreFTPConfigStatus.Error_Port_Numeric
                    message.Append(String.Format(ResourcesAdministration.GetString("msg_validation_notNumeric"), Me.Label_Port.Text))
                    message.Append(Environment.NewLine)
            End Select
        End While

        If message.Length <= 0 Then
            'data is valid - perform insert or update
            Try
                If Me.IsEdit Then
                    storeFtpConfigDAO.UpdateFTPInfo(_storeFTPConfig)
                Else
                    storeFtpConfigDAO.InsertFTPInfo(_storeFTPConfig)
                End If

                success = True
            Catch ex As DataFactoryException
                Logger.LogError("Exception: ", Me.GetType(), ex)
                'display a message to the user
                DisplayErrorMessage(ERROR_DB)
                'send message about exception
                Dim args(1) As String
                args(0) = "Form_EditStoreFTPInfo form: ApplyChanges sub"
                ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
            End Try
        Else
            'display error msg
            MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        Return success
    End Function

    Private Sub UpdateData()
        'updates _storeFTPConfig property with values filled in/updated in form elements
        If Not Me.IsEdit Then
            _storeFTPConfig.FileWriterType = Me.ComboBox_FileWriterType.SelectedValue.ToString
        End If

        _storeFTPConfig.IPAddress = Me.TextBox_IPAddress.Text
        _storeFTPConfig.FTPUser = Me.TextBox_FTPUser.Text
        _storeFTPConfig.FTPPassword = Me.TextBox_FTPPassword.Text
        _storeFTPConfig.ChangeDirectory = Me.TextBox_ChangeDirectory.Text
        _storeFTPConfig.Port = Me.TextBox_Port.Text
        _storeFTPConfig.IsSecureTransfer = Me.CheckBox_IsSecureTransfer.Checked
    End Sub
End Class