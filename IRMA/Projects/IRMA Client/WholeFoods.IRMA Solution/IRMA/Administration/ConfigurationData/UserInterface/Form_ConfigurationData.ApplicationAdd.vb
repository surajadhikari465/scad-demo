Imports WholeFoods.IRMA.Administration.ConfigurationData.DataAccess

Public Class Form_ConfigurationData_ApplicationAdd

    Public Event RefreshList()

    Public AppConfigApp As AppConfigAppBO

    Private _envID As Guid
    Public Property EnvironmentID() As Guid
        Get
            Return Me._envID
        End Get
        Set(ByVal value As Guid)
            Me._envID = value
        End Set
    End Property

    Public WriteOnly Property EnvironmentName() As String
        Set(ByVal value As String)
            Me._labelEnv.Text = String.Format(Me._labelEnv.Text, value)
        End Set
    End Property

    ' Instantiate the data access class
    Private daoConfig As New ConfigurationDataDAO

    Private Sub Form_ConfigurationData_ApplicationAdd_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dtAppConfigType As DataTable = daoConfig.GetApplicationTypeList()
        Me._comboFilterType.DataSource = dtAppConfigType
        Me._comboFilterType.ValueMember = "TypeID"
        Me._comboFilterType.DisplayMember = "Name"
        Me._comboFilterType.SelectedIndex = -1

        Me._textName.Focus()

    End Sub

    Private Sub _buttonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonAdd.Click

        Dim _appConfigApp As AppConfigAppBO

        Try

            If ValidateInput() Then

                Me._formErrorProvider.Clear()

                _appConfigApp = New AppConfigAppBO
                _appConfigApp.EnvironmentID = Me._envID
                _appConfigApp.Type = Me._comboFilterType.SelectedValue
                _appConfigApp.Name = Me._textName.Text
                _appConfigApp.UserID = My.Application.CurrentUserID

                AppConfigApp = _appConfigApp.Add(_appConfigApp)

                RaiseEvent RefreshList()

                MessageBox.Show("The application was added.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Me.Close()

            End If

        Catch ex As Exception

            Me.DialogResult = Windows.Forms.DialogResult.Abort
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()

        End Try

    End Sub

    Private Function ValidateInput() As Boolean

        Dim _errCount As Integer = 0

        ValidateInput = True

        If Me._textName.Text.Length = 0 Then
            Me._formErrorProvider.SetError(Me._textName, "Required")
            ValidateInput = False
        End If

        If Me._comboFilterType.SelectedIndex = -1 Then
            Me._formErrorProvider.SetError(Me._comboFilterType, "Required")
            ValidateInput = False
        End If

        If Me._envID = Nothing Then
            MessageBox.Show("No Environment was selected on the previous screen.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Me.Close()
        End If

        Return ValidateInput

    End Function


End Class