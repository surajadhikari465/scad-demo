Imports System.Text
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.Utility
Imports IRMA.Library.Security

Public Class Form_Login
#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    ''' Value of the current login data
    ''' </summary>
    ''' <remarks></remarks>
    Private _loginData As LoginBO

    ''' <summary>
    ''' Status of the most recent login attempt
    ''' </summary>
    ''' <remarks></remarks>
    Private _loginStatus As Boolean = False

    ''' <summary>
    ''' Alerts the calling application if the user cancelled the login.
    ''' </summary>
    ''' <remarks></remarks>
    Private _loginCancelled As Boolean = False

    ''' <summary>
    ''' Flag set to true if the IRMA login should be validated.
    ''' Set to false if only windows authentication is performed (in case of a timeout).
    ''' </summary>
    ''' <remarks></remarks>
    Private _validateIRMALogin As Boolean = True
#End Region

#Region "Events handled by this form"
#Region "Load Form"
    ''' <summary>
    ''' Load the form, pre-filling the login id.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_Login_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Pre-fill the login user id
        If _validateIRMALogin AndAlso _loginData.UserName IsNot Nothing AndAlso _loginData.UserName IsNot "" Then
            ' If the IRMA login is not being validated (in case of a timeout), the user name is pre-filled with 
            ' the value from the previous successful login.
            ' If the user name changes, the IRMA validation must be performed.
            Me.txtUserName.Text = _loginData.UserName
        Else
            _loginData.SetUserName()
            Me.txtUserName.Text = _loginData.UserName
        End If

        ' Center the login form
        Me.CenterToScreen()
    End Sub
#End Region

#Region "Form Buttons"
    ''' <summary>
    ''' Validate the login on OK.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Logger.LogDebug("cmdOK_Click entry", Me.GetType())
        ' Populate the business object with the form data
        _loginData.UserName = Me.txtUserName.Text
        _loginData.Password = Me.txtPassword.Text

        ' Validate the data
        Dim statusList As ArrayList = _loginData.ValidateData()
        Dim statusEnum As IEnumerator = statusList.GetEnumerator
        Dim message As New StringBuilder
        Dim loginDataStatus As LoginDataStatus
        Dim loginVerStatus As LoginVerificationStatus

        'loop through possible validation erorrs and build message string containing all errors
        While statusEnum.MoveNext
            loginDataStatus = CType(statusEnum.Current, LoginDataStatus)
            Select Case loginDataStatus
                Case loginDataStatus.Error_Required_UserName
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), lbl_UserName.Text))
                    message.Append(Environment.NewLine)
                Case loginDataStatus.Error_Required_Password
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), lbl_Password.Text))
                    message.Append(Environment.NewLine)
                Case WholeFoods.IRMA.Administration.Common.BusinessLogic.LoginDataStatus.Valid

            End Select
        End While

        ' validate the login or display a validation error message
        If message.Length <= 0 Then
            'data is valid - validate the login
            Logger.LogInfo("cmdOK_Click: Validating the login for UserName=" + _loginData.UserName, Me.GetType())
            loginVerStatus = _loginData.AuthenticateUser(_validateIRMALogin)
            Select Case loginVerStatus
                Case LoginVerificationStatus.Error_WindowsAuthenticationFailure
                    MessageBox.Show(String.Format(ResourcesCommon.GetString("error_windowsAuthenticationFailure"), _loginData.ErrorMessage), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    _loginStatus = False
                    Me.Close()
                Case LoginVerificationStatus.Error_MissingIRMAAccount
                    MessageBox.Show(ResourcesCommon.GetString("error_missingIRMAAccount"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    _loginStatus = False
                    Me.Close()
                Case LoginVerificationStatus.Error_DisabledIRMAAccount
                    MessageBox.Show(ResourcesCommon.GetString("error_disableIRMAAccount"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    _loginStatus = False
                    Me.Close()
                Case WholeFoods.IRMA.Administration.Common.BusinessLogic.LoginVerificationStatus.Valid
                    _loginStatus = True
                    'TODO - Integrate Logon w/CSLA
                    'Use CSLA call to allow Principal auth for using application
                    IRMA.Library.Security.IrmaPrincipal.Login(_loginData.UserName, _loginData.Password, True, False)
                    My.Application.CurrentUserID = _loginData.UserConfig.UserId
                    Me.Close()
            End Select
        Else
            'display error msg
            MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        Logger.LogDebug("cmdOK_Click exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' Hide the form on Cancel.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        _loginCancelled = True
        _loginStatus = False
        Me.Close()
    End Sub

#End Region
#End Region

#Region "Property Definitions"
    Public Property LoginData() As LoginBO
        Get
            Return _loginData
        End Get
        Set(ByVal value As LoginBO)
            _loginData = value
        End Set
    End Property

    Public Property ValidateIRMALogin() As Boolean
        Get
            Return _validateIRMALogin
        End Get
        Set(ByVal value As Boolean)
            _validateIRMALogin = value
        End Set
    End Property

    Public Property LoginStatus() As Boolean
        Get
            Return _loginStatus
        End Get
        Set(ByVal value As Boolean)
            _loginStatus = value
        End Set
    End Property

    Public Property LoginCancelled() As Boolean
        Get
            Return _loginCancelled
        End Get
        Set(ByVal value As Boolean)
            _loginCancelled = value
        End Set
    End Property
#End Region

    ''' <summary>
    ''' IRMA validation must be performed if the user name changes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtUserName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUserName.TextChanged
        _validateIRMALogin = True
    End Sub
End Class