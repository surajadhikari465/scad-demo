<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class NoNetwork
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
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItem_Exit = New System.Windows.Forms.MenuItem
        Me.MenuItem_Continue = New System.Windows.Forms.MenuItem
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label_NetworkStatus = New System.Windows.Forms.Label
        Me.Timer1 = New System.Windows.Forms.Timer
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.MenuItem_Exit)
        Me.mainMenu1.MenuItems.Add(Me.MenuItem_Continue)
        '
        'MenuItem_Exit
        '
        Me.MenuItem_Exit.Text = "Exit"
        '
        'MenuItem_Continue
        '
        Me.MenuItem_Continue.Text = "Continue"
        '
        'Label1
        '
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(3, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(234, 44)
        Me.Label1.Text = "An active network connection is required to use this application."
        '
        'Label_NetworkStatus
        '
        Me.Label_NetworkStatus.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label_NetworkStatus.Location = New System.Drawing.Point(3, 67)
        Me.Label_NetworkStatus.Name = "Label_NetworkStatus"
        Me.Label_NetworkStatus.Size = New System.Drawing.Size(234, 21)
        Me.Label_NetworkStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 2000
        '
        'Label2
        '
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(3, 188)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(234, 44)
        Me.Label2.Text = "When the network becomes available you can continue from this screen. "
        '
        'NoNetwork
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label_NetworkStatus)
        Me.Controls.Add(Me.Label1)
        Me.Menu = Me.mainMenu1
        Me.Name = "NoNetwork"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label_NetworkStatus As System.Windows.Forms.Label
    Friend WithEvents MenuItem_Exit As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem_Continue As System.Windows.Forms.MenuItem
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
