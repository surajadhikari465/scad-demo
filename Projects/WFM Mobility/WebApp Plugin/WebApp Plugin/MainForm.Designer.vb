<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.mnuExit = New System.Windows.Forms.MenuItem
        Me.mnuAbort = New System.Windows.Forms.MenuItem
        Me.cmdLaunch = New System.Windows.Forms.Button
        Me.lblPluginName = New System.Windows.Forms.Label
        Me.txtURI = New System.Windows.Forms.TextBox
        Me.lblHttp = New System.Windows.Forms.Label
        Me.timerLaunch = New System.Windows.Forms.Timer
        Me.lblCountdown = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.mnuExit)
        Me.mainMenu1.MenuItems.Add(Me.mnuAbort)
        '
        'mnuExit
        '
        Me.mnuExit.Text = "Exit"
        '
        'mnuAbort
        '
        Me.mnuAbort.Text = "Abort"
        '
        'cmdLaunch
        '
        Me.cmdLaunch.Location = New System.Drawing.Point(53, 167)
        Me.cmdLaunch.Name = "cmdLaunch"
        Me.cmdLaunch.Size = New System.Drawing.Size(135, 35)
        Me.cmdLaunch.TabIndex = 0
        Me.cmdLaunch.Text = "Launch Now"
        '
        'lblPluginName
        '
        Me.lblPluginName.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblPluginName.Location = New System.Drawing.Point(59, 23)
        Me.lblPluginName.Name = "lblPluginName"
        Me.lblPluginName.Size = New System.Drawing.Size(177, 19)
        Me.lblPluginName.Text = "{0}"
        Me.lblPluginName.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtURI
        '
        Me.txtURI.Location = New System.Drawing.Point(53, 110)
        Me.txtURI.Name = "txtURI"
        Me.txtURI.ReadOnly = True
        Me.txtURI.Size = New System.Drawing.Size(183, 21)
        Me.txtURI.TabIndex = 2
        '
        'lblHttp
        '
        Me.lblHttp.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblHttp.Location = New System.Drawing.Point(3, 113)
        Me.lblHttp.Name = "lblHttp"
        Me.lblHttp.Size = New System.Drawing.Size(57, 20)
        Me.lblHttp.Text = "http://"
        '
        'timerLaunch
        '
        Me.timerLaunch.Interval = 1500
        '
        'lblCountdown
        '
        Me.lblCountdown.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblCountdown.Location = New System.Drawing.Point(53, 134)
        Me.lblCountdown.Name = "lblCountdown"
        Me.lblCountdown.Size = New System.Drawing.Size(183, 20)
        Me.lblCountdown.Text = "launching in: 5"
        Me.lblCountdown.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(60, 60)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.ControlBox = False
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lblCountdown)
        Me.Controls.Add(Me.txtURI)
        Me.Controls.Add(Me.lblPluginName)
        Me.Controls.Add(Me.cmdLaunch)
        Me.Controls.Add(Me.lblHttp)
        Me.Menu = Me.mainMenu1
        Me.Name = "MainForm"
        Me.Text = "WFM Mobile"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmdLaunch As System.Windows.Forms.Button
    Friend WithEvents lblPluginName As System.Windows.Forms.Label
    Friend WithEvents txtURI As System.Windows.Forms.TextBox
    Friend WithEvents lblHttp As System.Windows.Forms.Label
    Friend WithEvents mnuExit As System.Windows.Forms.MenuItem
    Friend WithEvents timerLaunch As System.Windows.Forms.Timer
    Friend WithEvents mnuAbort As System.Windows.Forms.MenuItem
    Friend WithEvents lblCountdown As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class
