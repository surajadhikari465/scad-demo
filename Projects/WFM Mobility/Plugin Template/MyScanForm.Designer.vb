<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class MyScanForm
    Inherits HandheldHardware.ScanForm

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
        Me.Label1 = New System.Windows.Forms.Label
        Me.UPCTextBox = New System.Windows.Forms.TextBox
        Me.StatusBar1 = New System.Windows.Forms.StatusBar
        Me.SuccessLabel = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(5, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 20)
        Me.Label1.Text = "UPC"
        '
        'UPCTextBox
        '
        Me.UPCTextBox.Location = New System.Drawing.Point(59, 36)
        Me.UPCTextBox.Name = "UPCTextBox"
        Me.UPCTextBox.Size = New System.Drawing.Size(164, 21)
        Me.UPCTextBox.TabIndex = 1
        '
        'StatusBar1
        '
        Me.StatusBar1.Location = New System.Drawing.Point(0, 246)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Size = New System.Drawing.Size(240, 22)
        Me.StatusBar1.Text = "StatusBar1"
        '
        'SuccessLabel
        '
        Me.SuccessLabel.Location = New System.Drawing.Point(40, 103)
        Me.SuccessLabel.Name = "SuccessLabel"
        Me.SuccessLabel.Size = New System.Drawing.Size(153, 60)
        '
        'MyScanForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.SuccessLabel)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.UPCTextBox)
        Me.Controls.Add(Me.Label1)
        Me.KeyPreview = True
        Me.Menu = Me.mainMenu1
        Me.Name = "MyScanForm"
        Me.Text = "My Scan Form"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents UPCTextBox As System.Windows.Forms.TextBox
    Friend WithEvents StatusBar1 As System.Windows.Forms.StatusBar
    Friend WithEvents SuccessLabel As System.Windows.Forms.Label

End Class
