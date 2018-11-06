Imports WholeFoods.IRMA.Administration.ConfigurationData.DataAccess

Public Class Form_ConfigurationData_EnvironmentAdd

    Public Event RefreshList()

    Public AppConfigEnv As AppConfigEnvBO

    ' Instantiate the data access class
    Private daoConfig As New ConfigurationDataDAO

    Private Sub Form_ConfigurationData_EnvironmentAdd_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me._textName.Focus()

    End Sub

    Private Sub _buttonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonAdd.Click

        Dim _appConfigEnv As AppConfigEnvBO

        Try

            If ValidateInput() Then

                Me._formErrorProvider.Clear()

                _appConfigEnv = New AppConfigEnvBO
                _appConfigEnv.Shortname = Me._textShortname.Text
                _appConfigEnv.Name = Me._textName.Text
                _appConfigEnv.UserID = My.Application.CurrentUserID

                AppConfigEnv = _appConfigEnv.Add(_appConfigEnv)

                RaiseEvent RefreshList()

                MessageBox.Show("The environment was added.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Me.Close()

            End If

        Catch ex As Exception

            Me.DialogResult = Windows.Forms.DialogResult.Abort
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()

        End Try

    End Sub

    Private Function ValidateInput() As Boolean

        ValidateInput = True

        If Me._textName.Text.Length = 0 Then
            Me._formErrorProvider.SetError(Me._textName, "Required")
            ValidateInput = False
        End If

        If Me._textShortname.Text.Length = 0 Then
            Me._formErrorProvider.SetError(Me._textShortname, "Required")
            ValidateInput = False
        End If

        Return ValidateInput

    End Function

End Class