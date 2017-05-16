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
        Me.mnuOptions = New System.Windows.Forms.MenuItem
        Me.mnuSettings = New System.Windows.Forms.MenuItem
        Me.mnuAbout = New System.Windows.Forms.MenuItem
        Me.mnuExit = New System.Windows.Forms.MenuItem
        Me.lblTitle = New System.Windows.Forms.Label
        Me.imgLogo = New System.Windows.Forms.PictureBox
        Me.lblEnvironment = New System.Windows.Forms.Label
        Me.ucContainer = New System.Windows.Forms.Panel
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.mnuOptions)
        Me.mainMenu1.MenuItems.Add(Me.mnuExit)
        '
        'mnuOptions
        '
        Me.mnuOptions.MenuItems.Add(Me.mnuSettings)
        Me.mnuOptions.MenuItems.Add(Me.mnuAbout)
        Me.mnuOptions.Text = "Options"
        '
        'mnuSettings
        '
        Me.mnuSettings.Text = "Settings"
        '
        'mnuAbout
        '
        Me.mnuAbout.Text = "About"
        '
        'mnuExit
        '
        Me.mnuExit.Text = "Exit"
        '
        'lblTitle
        '
        Me.lblTitle.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.Location = New System.Drawing.Point(77, 28)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(151, 20)
        Me.lblTitle.Text = "What do you want to use?"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'imgLogo
        '
        Me.imgLogo.Image = CType(resources.GetObject("imgLogo.Image"), System.Drawing.Image)
        Me.imgLogo.Location = New System.Drawing.Point(3, 3)
        Me.imgLogo.Name = "imgLogo"
        Me.imgLogo.Size = New System.Drawing.Size(60, 60)
        Me.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        '
        'lblEnvironment
        '
        Me.lblEnvironment.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblEnvironment.Location = New System.Drawing.Point(3, 254)
        Me.lblEnvironment.Name = "lblEnvironment"
        Me.lblEnvironment.Size = New System.Drawing.Size(230, 10)
        Me.lblEnvironment.Text = "[Environment]"
        '
        'ucContainer
        '
        Me.ucContainer.Location = New System.Drawing.Point(3, 70)
        Me.ucContainer.Name = "ucContainer"
        Me.ucContainer.Size = New System.Drawing.Size(234, 181)
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.ControlBox = False
        Me.Controls.Add(Me.ucContainer)
        Me.Controls.Add(Me.lblEnvironment)
        Me.Controls.Add(Me.imgLogo)
        Me.Controls.Add(Me.lblTitle)
        Me.KeyPreview = True
        Me.Menu = Me.mainMenu1
        Me.Name = "MainForm"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents imgLogo As System.Windows.Forms.PictureBox
    Friend WithEvents mnuOptions As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSettings As System.Windows.Forms.MenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAbout As System.Windows.Forms.MenuItem
    Friend WithEvents lblEnvironment As System.Windows.Forms.Label
    Friend WithEvents ucContainer As System.Windows.Forms.Panel

End Class
