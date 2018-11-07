<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class SettingsForm
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

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SettingsForm))
        Me.cmbRegion = New System.Windows.Forms.ComboBox
        Me.lblTitle = New System.Windows.Forms.Label
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.mnuMainMenu = New System.Windows.Forms.MenuItem
        Me.cmdSave = New System.Windows.Forms.MenuItem
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.lblRegion = New System.Windows.Forms.Label
        Me.lblServiceURI = New System.Windows.Forms.Label
        Me.txtServiceURI = New System.Windows.Forms.TextBox
        Me.pnlServiceLocation = New System.Windows.Forms.Panel
        Me.pnlServiceLocation.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmbRegion
        '
        Me.cmbRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
        Me.cmbRegion.Location = New System.Drawing.Point(3, 98)
        Me.cmbRegion.Name = "cmbRegion"
        Me.cmbRegion.Size = New System.Drawing.Size(109, 22)
        Me.cmbRegion.TabIndex = 0
        Me.cmbRegion.Text = "-- Set Region --"
        '
        'lblTitle
        '
        Me.lblTitle.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.Location = New System.Drawing.Point(90, 30)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(100, 20)
        Me.lblTitle.Text = "Settings"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.Add(Me.mnuMainMenu)
        Me.MainMenu1.MenuItems.Add(Me.cmdSave)
        '
        'mnuMainMenu
        '
        Me.mnuMainMenu.Text = "Main Menu"
        '
        'cmdSave
        '
        Me.cmdSave.Text = "Save"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(60, 60)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        '
        'lblRegion
        '
        Me.lblRegion.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblRegion.Location = New System.Drawing.Point(3, 82)
        Me.lblRegion.Name = "lblRegion"
        Me.lblRegion.Size = New System.Drawing.Size(100, 13)
        Me.lblRegion.Text = "Region:"
        '
        'lblServiceURI
        '
        Me.lblServiceURI.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblServiceURI.Location = New System.Drawing.Point(0, 7)
        Me.lblServiceURI.Name = "lblServiceURI"
        Me.lblServiceURI.Size = New System.Drawing.Size(100, 13)
        Me.lblServiceURI.Text = "Service Location:"
        '
        'txtServiceURI
        '
        Me.txtServiceURI.Location = New System.Drawing.Point(0, 24)
        Me.txtServiceURI.Name = "txtServiceURI"
        Me.txtServiceURI.ReadOnly = True
        Me.txtServiceURI.Size = New System.Drawing.Size(234, 21)
        Me.txtServiceURI.TabIndex = 1
        '
        'pnlServiceLocation
        '
        Me.pnlServiceLocation.Controls.Add(Me.txtServiceURI)
        Me.pnlServiceLocation.Controls.Add(Me.lblServiceURI)
        Me.pnlServiceLocation.Location = New System.Drawing.Point(3, 126)
        Me.pnlServiceLocation.Name = "pnlServiceLocation"
        Me.pnlServiceLocation.Size = New System.Drawing.Size(234, 58)
        '
        'SettingsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlServiceLocation)
        Me.Controls.Add(Me.lblRegion)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.cmbRegion)
        Me.Controls.Add(Me.lblTitle)
        Me.Menu = Me.MainMenu1
        Me.Name = "SettingsForm"
        Me.pnlServiceLocation.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmbRegion As System.Windows.Forms.ComboBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents mnuMainMenu As System.Windows.Forms.MenuItem
    Friend WithEvents cmdSave As System.Windows.Forms.MenuItem
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblRegion As System.Windows.Forms.Label
    Friend WithEvents lblServiceURI As System.Windows.Forms.Label
    Friend WithEvents txtServiceURI As System.Windows.Forms.TextBox
    Friend WithEvents pnlServiceLocation As System.Windows.Forms.Panel
End Class
