Imports System.Threading

Public Class AuthenticateForm

    Public _sender As PluginButton
    Public WriteOnly Property PluginCaller() As PluginButton
        Set(ByVal value As PluginButton)
            _sender = value
        End Set
    End Property

    Private result As String
    Public ReadOnly Property UserEmail() As String
        Get
            Return result
        End Get
    End Property

#Region " Control Events"

    Private Sub AuthenticateForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        UnlockControls()

        txtPassword.Text = String.Empty

        ' if the username is populated, the user may have already
        ' authenticated once and is reentering the same plugin again in the
        ' same seeion - help them out by focusing on the password to reduce taps
        If txtUserName.Text.Length > 0 Then
            txtPassword.Focus()
        Else
            txtUserName.Focus()
        End If

        Me.Text = My.Resources.Application_Title

    End Sub

    Private Sub AuthenticateForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        ' react to the enter kep being pressed by trying to sign in
        If e.KeyCode = Keys.Enter Then
            Authenticate()
        End If

    End Sub

    Private Sub btnAuthenticate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthenticate.Click

        ' try authenticating
        Authenticate()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        ' clear both fields
        txtUserName.Text = String.Empty
        txtPassword.Text = String.Empty

        Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Me.Close()

    End Sub

    Private Sub mnuClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuClear.Click

        txtUserName.Text = String.Empty
        txtPassword.Text = String.Empty
        txtUserName.Focus()

    End Sub

#End Region

#Region " Private Methods"

    Private Sub Authenticate()

        ' make sure the user entered a username and password
        If txtUserName.Text.Length = 0 Or txtPassword.Text.Length = 0 Then

            MessageBox.Show("Both a username and password are required.")

            Exit Sub

        End If

        Cursor.Current = Cursors.WaitCursor

        LockControls()

        ' Attempt to authenticated the user using the universal handheld service.
        result = Client.AuthenticateUser(txtUserName.Text, txtPassword.Text, ConfigurationManager.AppSettings("Region"), _sender.PluginName)

        ' If the result contains an email address, then authentication was successful.
        If result.ToLower().Contains("@wholefoods.com") Then
            Me.DialogResult = Windows.Forms.DialogResult.Yes
            Me.Close()
        Else

            'otherwise, show the user what is wrong
            MessageBox.Show(result)

            UnlockControls()

            If result.Contains("account") Then

                ' if the result message was due to a bad user account name, clear the username and password txtbox for them
                txtUserName.Text = String.Empty
                txtPassword.Text = String.Empty
                txtUserName.Focus()

            ElseIf result.Contains("password") Then

                ' if the result message was due to a bad password, clear the password txt box for them
                txtPassword.Text = String.Empty
                txtPassword.Focus()

            End If

        End If

        Cursor.Current = Cursors.Default

    End Sub

    Private Sub LockControls()
        txtUserName.Enabled = False
        txtPassword.Enabled = False
        btnAuthenticate.Enabled = False
        btnCancel.Enabled = False
        InputPanel1.Enabled = False
        Application.DoEvents()
    End Sub

    Private Sub UnlockControls()
        txtUserName.Enabled = True
        txtPassword.Enabled = True
        btnAuthenticate.Enabled = True
        btnCancel.Enabled = True
        Application.DoEvents()
    End Sub

#End Region

End Class