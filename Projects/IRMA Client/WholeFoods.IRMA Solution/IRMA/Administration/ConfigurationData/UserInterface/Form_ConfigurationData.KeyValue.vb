Imports WholeFoods.Utility.Encryption

Public Class Form_ConfigurationData_KeyValue

    Public Event RefreshList()

#Region " Fields"

    Public KeyValueUpdated As Boolean
    Private _appConfigValue As AppConfigValueBO
    Private bIsLoading As Boolean

#End Region

#Region " Properties"

    Public WriteOnly Property AppConfigValue() As AppConfigValueBO
        Set(ByVal value As AppConfigValueBO)
            Me._appConfigValue = value
        End Set
    End Property

    Public WriteOnly Property ApplicationName() As String
        Set(ByVal value As String)
            Me._labelApplicationName.Text = String.Format(Me._labelApplicationName.Text, value)
        End Set
    End Property

    Public WriteOnly Property EnvironmentName() As String
        Set(ByVal value As String)
            Me._labelEnvironmentName.Text = String.Format(Me._labelEnvironmentName.Text, value)
        End Set
    End Property

    Public WriteOnly Property KeyName() As String
        Set(ByVal value As String)
            Me._labelKeyName.Text = String.Format(Me._labelKeyName.Text, value)
        End Set
    End Property

#End Region

#Region " Methods"

    ''' <summary>
    ''' This event is raised whenver form data is changed to indicate that it needs to be saved.
    ''' </summary>
    ''' <remarks></remarks>
    Private Event FormDataChanged()

    ''' <summary>
    ''' Sets the hasChanges form indicator to True and enables the Save button.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormHasChanges() Handles Me.FormDataChanged

        Me.KeyValueUpdated = True
        Me._appConfigValue.Value = Me._textValue.Text
        Me._appConfigValue.UserID = My.Application.CurrentUserID
        Me._buttonSave.Enabled = True

    End Sub

    Private Sub _checkEncrypt_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _checkEncrypt.CheckedChanged

        If Me.bIsLoading Then Exit Sub
        RaiseEvent FormDataChanged()

    End Sub

    Private Sub _textValue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _textValue.TextChanged

        If Me.bIsLoading Then Exit Sub
        RaiseEvent FormDataChanged()

    End Sub

    Private Sub Form_ConfigurationData_KeyValue_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.bIsLoading = True
        Me._textValue.Text = Me._appConfigValue.Value
        Me._textValue.SelectAll()
        Me._textValue.Focus()
        Me.bIsLoading = False

    End Sub

    Private Sub _buttonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonSave.Click

        If Me.KeyValueUpdated And ValidateInput() Then

            If Me._checkEncrypt.Checked Then

                MessageBox.Show("You have chosen to encrypt a key value." & vbCrLf & vbCrLf & _
                                "The app.config file for this application must have the key 'encryptedConnectionStrings' set to TRUE in order for this value to be read properly by the application.", "Encryption Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)

                Dim _encryptor As New Encryptor

                _appConfigValue.Value = _encryptor.Encrypt(_appConfigValue.Value)

            End If

            Me._formErrorProvider.Clear()

            If _appConfigValue.Update(_appConfigValue) Then
                RaiseEvent RefreshList()
                MessageBox.Show("The value was updated.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
            End If

        End If

    End Sub

    Private Function ValidateInput() As Boolean

        ValidateInput = True

        If Me._textValue.Text.Length = 0 Then

            If MessageBox.Show("A value for this Key has not been entered." & vbCrLf & "Would you like to save an empty value for this Key?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                Me._formErrorProvider.SetError(Me._textValue, "Required")
                ValidateInput = False
            End If

        End If

        Return ValidateInput

    End Function

#End Region

End Class