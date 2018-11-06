<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ViewError
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
        Me.MenuItemClose = New System.Windows.Forms.MenuItem
        Me.TextBox_ErrorInfo = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.MenuItemClose)
        '
        'MenuItemClose
        '
        Me.MenuItemClose.Text = "Close"
        '
        'TextBox_ErrorInfo
        '
        Me.TextBox_ErrorInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox_ErrorInfo.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.TextBox_ErrorInfo.Location = New System.Drawing.Point(0, 0)
        Me.TextBox_ErrorInfo.Multiline = True
        Me.TextBox_ErrorInfo.Name = "TextBox_ErrorInfo"
        Me.TextBox_ErrorInfo.Size = New System.Drawing.Size(240, 268)
        Me.TextBox_ErrorInfo.TabIndex = 0
        '
        'ViewError
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.TextBox_ErrorInfo)
        Me.Menu = Me.mainMenu1
        Me.Name = "ViewError"
        Me.Text = "View Error Info"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MenuItemClose As System.Windows.Forms.MenuItem
    Friend WithEvents TextBox_ErrorInfo As System.Windows.Forms.TextBox
End Class
