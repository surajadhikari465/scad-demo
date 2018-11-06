Imports WholeFoods.IRMA.Administration.ConfigurationData.DataAccess

Public Class Form_ConfigurationData_KeyAdd

    ' Instantiate the data access class
    Private daoConfig As New ConfigurationDataDAO

    Public Event RefreshList()

    Public AppConfigKey As AppConfigKeyBO

    Private Sub Form_ConfigurationData_EnvironmentAdd_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me._textName.Focus()

    End Sub

    Private Sub _buttonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonAdd.Click

        Dim _appConfigKey As AppConfigKeyBO

        Try

            If ValidateInput() Then

                Me._formErrorProvider.Clear()

                _appConfigKey = New AppConfigKeyBO
                _appConfigKey.Name = Me._textName.Text
                _appConfigKey.UserID = My.Application.CurrentUserID

                AppConfigKey = _appConfigKey.Add(_appConfigKey)

                RaiseEvent RefreshList()

                If Len(AppConfigKey.KeyID) > 0 Then
                    ' the key was added so inform the user, otherwise close silently because it either existed already
                    ' or was inactive and the user chose not to reactivate it.
                    MessageBox.Show("The key was added.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                Me.DialogResult = Windows.Forms.DialogResult.OK

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

        Return ValidateInput

    End Function

End Class