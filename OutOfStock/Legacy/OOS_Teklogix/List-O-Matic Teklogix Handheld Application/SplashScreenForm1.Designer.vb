<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class SplashScreenForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SplashScreenForm))
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label_Version = New System.Windows.Forms.Label
        Me.Label_Status = New System.Windows.Forms.Label
        Me.ProgressBar_SplashScreen = New System.Windows.Forms.ProgressBar
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Black
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 63)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(237, 170)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 18.0!, System.Drawing.FontStyle.Regular)
        Me.Label1.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.Label1.Location = New System.Drawing.Point(91, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 28)
        Me.Label1.Text = "OOS"
        '
        'Label_Version
        '
        Me.Label_Version.Font = New System.Drawing.Font("Courier New", 8.0!, System.Drawing.FontStyle.Regular)
        Me.Label_Version.ForeColor = System.Drawing.Color.White
        Me.Label_Version.Location = New System.Drawing.Point(77, 274)
        Me.Label_Version.Name = "Label_Version"
        Me.Label_Version.Size = New System.Drawing.Size(86, 20)
        Me.Label_Version.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label_Status
        '
        Me.Label_Status.ForeColor = System.Drawing.Color.White
        Me.Label_Status.Location = New System.Drawing.Point(3, 239)
        Me.Label_Status.Name = "Label_Status"
        Me.Label_Status.Size = New System.Drawing.Size(234, 18)
        Me.Label_Status.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ProgressBar_SplashScreen
        '
        Me.ProgressBar_SplashScreen.Location = New System.Drawing.Point(53, 219)
        Me.ProgressBar_SplashScreen.Name = "ProgressBar_SplashScreen"
        Me.ProgressBar_SplashScreen.Size = New System.Drawing.Size(133, 7)
        '
        'SplashScreenForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.WindowFrame
        Me.ClientSize = New System.Drawing.Size(240, 294)
        Me.ControlBox = False
        Me.Controls.Add(Me.ProgressBar_SplashScreen)
        Me.Controls.Add(Me.Label_Status)
        Me.Controls.Add(Me.Label_Version)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureBox1)
        Me.ForeColor = System.Drawing.Color.Black
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Menu = Me.mainMenu1
        Me.MinimizeBox = False
        Me.Name = "SplashScreenForm"
        Me.Text = "SplashScreenForm"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label_Version As System.Windows.Forms.Label
    Friend WithEvents Label_Status As System.Windows.Forms.Label
    Friend WithEvents ProgressBar_SplashScreen As System.Windows.Forms.ProgressBar
End Class
