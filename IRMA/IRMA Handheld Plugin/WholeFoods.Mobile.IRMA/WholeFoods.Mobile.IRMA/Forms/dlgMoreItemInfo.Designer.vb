<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class dlgMoreItemInfo
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblVendorPack = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblWeek1 = New System.Windows.Forms.Label
        Me.lblWeek2 = New System.Windows.Forms.Label
        Me.lblWeek3 = New System.Windows.Forms.Label
        Me.lblWeek4 = New System.Windows.Forms.Label
        Me.lblWeek5 = New System.Windows.Forms.Label
        Me.lblWeek6 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblInv1 = New System.Windows.Forms.Label
        Me.lblInv2 = New System.Windows.Forms.Label
        Me.btnClose = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(23, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(199, 20)
        Me.Label1.Text = "Display Item Information"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(23, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 20)
        Me.Label2.Text = "Vendor Package:"
        '
        'lblVendorPack
        '
        Me.lblVendorPack.Location = New System.Drawing.Point(129, 51)
        Me.lblVendorPack.Name = "lblVendorPack"
        Me.lblVendorPack.Size = New System.Drawing.Size(76, 20)
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.Location = New System.Drawing.Point(23, 71)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(182, 20)
        Me.Label3.Text = "Movement Information:"
        '
        'lblWeek1
        '
        Me.lblWeek1.Location = New System.Drawing.Point(23, 104)
        Me.lblWeek1.Name = "lblWeek1"
        Me.lblWeek1.Size = New System.Drawing.Size(81, 20)
        '
        'lblWeek2
        '
        Me.lblWeek2.Location = New System.Drawing.Point(129, 104)
        Me.lblWeek2.Name = "lblWeek2"
        Me.lblWeek2.Size = New System.Drawing.Size(100, 20)
        '
        'lblWeek3
        '
        Me.lblWeek3.Location = New System.Drawing.Point(23, 124)
        Me.lblWeek3.Name = "lblWeek3"
        Me.lblWeek3.Size = New System.Drawing.Size(81, 20)
        '
        'lblWeek4
        '
        Me.lblWeek4.Location = New System.Drawing.Point(129, 124)
        Me.lblWeek4.Name = "lblWeek4"
        Me.lblWeek4.Size = New System.Drawing.Size(81, 20)
        '
        'lblWeek5
        '
        Me.lblWeek5.Location = New System.Drawing.Point(23, 144)
        Me.lblWeek5.Name = "lblWeek5"
        Me.lblWeek5.Size = New System.Drawing.Size(81, 20)
        '
        'lblWeek6
        '
        Me.lblWeek6.Location = New System.Drawing.Point(129, 144)
        Me.lblWeek6.Name = "lblWeek6"
        Me.lblWeek6.Size = New System.Drawing.Size(81, 20)
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.Location = New System.Drawing.Point(23, 186)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(144, 20)
        Me.Label4.Text = "Invoice Quantity:"
        '
        'lblInv1
        '
        Me.lblInv1.Location = New System.Drawing.Point(23, 217)
        Me.lblInv1.Name = "lblInv1"
        Me.lblInv1.Size = New System.Drawing.Size(100, 20)
        '
        'lblInv2
        '
        Me.lblInv2.Location = New System.Drawing.Point(23, 248)
        Me.lblInv2.Name = "lblInv2"
        Me.lblInv2.Size = New System.Drawing.Size(100, 20)
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.YellowGreen
        Me.btnClose.Location = New System.Drawing.Point(173, 245)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(64, 20)
        Me.btnClose.TabIndex = 18
        Me.btnClose.Text = "Close"
        '
        'dlgMoreItemInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblInv2)
        Me.Controls.Add(Me.lblInv1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblWeek6)
        Me.Controls.Add(Me.lblWeek5)
        Me.Controls.Add(Me.lblWeek4)
        Me.Controls.Add(Me.lblWeek3)
        Me.Controls.Add(Me.lblWeek2)
        Me.Controls.Add(Me.lblWeek1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblVendorPack)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Menu = Me.mainMenu1
        Me.Name = "dlgMoreItemInfo"
        Me.Text = "dlgMoreItemInfo"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblVendorPack As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblWeek1 As System.Windows.Forms.Label
    Friend WithEvents lblWeek2 As System.Windows.Forms.Label
    Friend WithEvents lblWeek3 As System.Windows.Forms.Label
    Friend WithEvents lblWeek4 As System.Windows.Forms.Label
    Friend WithEvents lblWeek5 As System.Windows.Forms.Label
    Friend WithEvents lblWeek6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblInv1 As System.Windows.Forms.Label
    Friend WithEvents lblInv2 As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
