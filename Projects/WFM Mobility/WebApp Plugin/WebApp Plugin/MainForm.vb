Imports System.Windows.Forms
Imports Microsoft.Win32

Public Class MainForm

    Private _serviceURI As String
    Private _regionCode As String
    Private _userName As String
    Private _userEmail As String
    Private _userAuthenticted As Boolean
    Private _userID As Integer
    Private _pluginName As String
    Private _lastTick As Integer = 5

    Private _regKeyPath As String = "Software\WFM Mobile\WebPlugins\" & RegionCode & "\" & PluginName

#Region " Public Properties"

    Public Property ServiceURI() As String
        Get
            Return _serviceURI
        End Get
        Set(ByVal value As String)
            _serviceURI = value
        End Set
    End Property

    Public Property RegionCode() As String
        Get
            Return _regionCode
        End Get
        Set(ByVal value As String)
            _regionCode = value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return _userName
        End Get
        Set(ByVal value As String)
            _userName = value
        End Set
    End Property

    Public Property UserEmail() As String
        Get
            Return _userEmail
        End Get
        Set(ByVal value As String)
            _userEmail = value
        End Set
    End Property

    Public Property UserAuthenticated() As Boolean
        Get
            Return _userAuthenticted
        End Get
        Set(ByVal value As Boolean)
            _userAuthenticted = value
        End Set
    End Property

    Public Property PluginName() As String
        Get
            Return _pluginName
        End Get
        Set(ByVal value As String)
            _pluginName = value
        End Set
    End Property

#End Region

#Region " Constructors"

    Public Shared Sub Main()

        Application.Run(New MainForm())

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.ServiceURI = ""
        Me.RegionCode = "NC"

    End Sub

#End Region

#Region " Form Events"

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not String.IsNullOrEmpty(Me.PluginName) Then
            SetMessage(lblPluginName, Me.PluginName)
        Else
            SetMessage(lblPluginName, "Unknown Web App")
        End If

        ' first, did we get a URI passed to us from WFM Mobile client?
        If Not String.IsNullOrEmpty(Me.ServiceURI) Then

            txtURI.Text = Me.ServiceURI
            mnuAbort.Enabled = False

        Else

            ' no URI was passed, so now we look for one stored for this app
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey(_regKeyPath, True)

            If regKey Is Nothing Then

                ' this plugin is new, so create the URI key and set the value to empty for now
                regKey = Registry.CurrentUser.CreateSubKey(_regKeyPath)
                regKey.SetValue("URI", "")

            ElseIf Not String.IsNullOrEmpty(regKey.GetValue("URI")) Then

                ' we have a URI set
                txtURI.Text = regKey.GetValue("URI")

            End If

            ' close the registry
            Registry.CurrentUser.Close()

        End If

        If Not String.IsNullOrEmpty(txtURI.Text) Then

            lblCountdown.Visible = True
            timerLaunch.Enabled = True

        Else

            txtURI.ReadOnly = False

        End If

        txtURI.Focus()

    End Sub

#End Region

#Region " Control Events"


    Private Sub txtURI_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtURI.TextChanged

        If String.IsNullOrEmpty(txtURI.Text) Then
            cmdLaunch.Enabled = False
        Else
            cmdLaunch.Enabled = True
        End If

    End Sub

    Private Sub cmdLaunch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLaunch.Click

        If txtURI.Text.Length = 0 Then

            MessageBox.Show("A valid web address is required.", "Enter Address", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)

            txtURI.Focus()
            txtURI.SelectAll()

        Else

            ' save the entered URI
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey(_regKeyPath, True)
            regKey.SetValue("URI", txtURI.Text)

            ' close the registry
            Registry.CurrentUser.Close()

            ' launch IE
            LaunchWebSite()

        End If

    End Sub

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click

        DialogResult = Windows.Forms.DialogResult.OK

    End Sub

    Private Sub mnuAbort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAbort.Click

        ' stop the launch timer
        timerLaunch.Enabled = False

        SetMessage(lblCountdown, "Aborted")

        ' allow the user to edit the URI
        txtURI.ReadOnly = False
        txtURI.Focus()
        txtURI.SelectAll()

    End Sub

    Private Sub timerLaunch_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles timerLaunch.Tick

        _lastTick = _lastTick - 1

        If _lastTick = 0 Then

            SetMessage(lblCountdown, "launching...")

            ' launch IE
            LaunchWebSite()

        Else

            SetMessage(lblCountdown, "launching in: " & (_lastTick).ToString)

        End If

    End Sub

#End Region

#Region " Private Methods"

    Private Sub LaunchWebSite()

        cmdLaunch.Enabled = False

        Application.DoEvents()

        ' stop the launch timer
        timerLaunch.Enabled = False

        Dim p As New Process

        p.StartInfo.FileName = "IExplore.exe"
        p.StartInfo.Arguments = txtURI.Text
        p.Start()
        p.WaitForExit()

        SetMessage(lblCountdown, "launched")

        cmdLaunch.Enabled = True

        DialogResult = Windows.Forms.DialogResult.OK

    End Sub

    Private Sub SetMessage(ByVal Control As Control, ByVal Text As String)

        Control.Text = Text
        Control.Refresh()

    End Sub

#End Region

End Class