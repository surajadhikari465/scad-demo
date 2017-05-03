<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ItemDiscount
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
        Me.mmuCancel = New System.Windows.Forms.MenuItem
        Me.mmuAccept = New System.Windows.Forms.MenuItem
        Me.cmbReasonCode = New System.Windows.Forms.ComboBox
        Me.lblReasonCode = New System.Windows.Forms.Label
        Me.cmbDiscountType = New System.Windows.Forms.ComboBox
        Me.lblDiscount = New System.Windows.Forms.Label
        Me.txtDiscount = New System.Windows.Forms.TextBox
        Me.lblDiscountType = New System.Windows.Forms.Label
        Me.lblUPC = New System.Windows.Forms.Label
        Me.lblDesc = New System.Windows.Forms.Label
        Me.lblDescValue = New System.Windows.Forms.Label
        Me.lblUPCValue = New System.Windows.Forms.Label
        Me.lblPercent = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.mmuCancel)
        Me.mainMenu1.MenuItems.Add(Me.mmuAccept)
        '
        'mmuCancel
        '
        Me.mmuCancel.Text = "Cancel"
        '
        'mmuAccept
        '
        Me.mmuAccept.Text = "Accept"
        '
        'cmbReasonCode
        '
        Me.cmbReasonCode.Location = New System.Drawing.Point(8, 171)
        Me.cmbReasonCode.Name = "cmbReasonCode"
        Me.cmbReasonCode.Size = New System.Drawing.Size(229, 22)
        Me.cmbReasonCode.TabIndex = 3
        '
        'lblReasonCode
        '
        Me.lblReasonCode.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblReasonCode.Location = New System.Drawing.Point(8, 151)
        Me.lblReasonCode.Name = "lblReasonCode"
        Me.lblReasonCode.Size = New System.Drawing.Size(93, 20)
        Me.lblReasonCode.Text = "Reason Code:"
        '
        'cmbDiscountType
        '
        Me.cmbDiscountType.Items.Add("No Discount")
        Me.cmbDiscountType.Items.Add("Cash Discount")
        Me.cmbDiscountType.Items.Add("Percent Discount")
        Me.cmbDiscountType.Location = New System.Drawing.Point(118, 84)
        Me.cmbDiscountType.Name = "cmbDiscountType"
        Me.cmbDiscountType.Size = New System.Drawing.Size(116, 22)
        Me.cmbDiscountType.TabIndex = 10
        '
        'lblDiscount
        '
        Me.lblDiscount.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblDiscount.Location = New System.Drawing.Point(8, 112)
        Me.lblDiscount.Name = "lblDiscount"
        Me.lblDiscount.Size = New System.Drawing.Size(104, 20)
        Me.lblDiscount.Text = "Discount:"
        '
        'txtDiscount
        '
        Me.txtDiscount.Location = New System.Drawing.Point(118, 112)
        Me.txtDiscount.Name = "txtDiscount"
        Me.txtDiscount.Size = New System.Drawing.Size(52, 21)
        Me.txtDiscount.TabIndex = 15
        '
        'lblDiscountType
        '
        Me.lblDiscountType.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblDiscountType.Location = New System.Drawing.Point(8, 86)
        Me.lblDiscountType.Name = "lblDiscountType"
        Me.lblDiscountType.Size = New System.Drawing.Size(104, 20)
        Me.lblDiscountType.Text = "Discount Type:"
        '
        'lblUPC
        '
        Me.lblUPC.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblUPC.Location = New System.Drawing.Point(7, 12)
        Me.lblUPC.Name = "lblUPC"
        Me.lblUPC.Size = New System.Drawing.Size(39, 20)
        Me.lblUPC.Text = "UPC:"
        '
        'lblDesc
        '
        Me.lblDesc.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblDesc.Location = New System.Drawing.Point(6, 32)
        Me.lblDesc.Name = "lblDesc"
        Me.lblDesc.Size = New System.Drawing.Size(40, 20)
        Me.lblDesc.Text = "Desc:"
        '
        'lblDescValue
        '
        Me.lblDescValue.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lblDescValue.Location = New System.Drawing.Point(52, 32)
        Me.lblDescValue.Name = "lblDescValue"
        Me.lblDescValue.Size = New System.Drawing.Size(182, 36)
        '
        'lblUPCValue
        '
        Me.lblUPCValue.Location = New System.Drawing.Point(52, 12)
        Me.lblUPCValue.Name = "lblUPCValue"
        Me.lblUPCValue.Size = New System.Drawing.Size(182, 20)
        '
        'lblPercent
        '
        Me.lblPercent.Location = New System.Drawing.Point(176, 112)
        Me.lblPercent.Name = "lblPercent"
        Me.lblPercent.Size = New System.Drawing.Size(18, 21)
        Me.lblPercent.Text = "%"
        Me.lblPercent.Visible = False
        '
        'ItemDiscount
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.lblPercent)
        Me.Controls.Add(Me.lblUPCValue)
        Me.Controls.Add(Me.lblDescValue)
        Me.Controls.Add(Me.lblDesc)
        Me.Controls.Add(Me.lblUPC)
        Me.Controls.Add(Me.lblDiscountType)
        Me.Controls.Add(Me.txtDiscount)
        Me.Controls.Add(Me.lblDiscount)
        Me.Controls.Add(Me.cmbDiscountType)
        Me.Controls.Add(Me.lblReasonCode)
        Me.Controls.Add(Me.cmbReasonCode)
        Me.Menu = Me.mainMenu1
        Me.Name = "ItemDiscount"
        Me.Text = "Item Discount"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmbReasonCode As System.Windows.Forms.ComboBox
    Friend WithEvents lblReasonCode As System.Windows.Forms.Label
    Friend WithEvents mmuCancel As System.Windows.Forms.MenuItem
    Friend WithEvents mmuAccept As System.Windows.Forms.MenuItem
    Friend WithEvents cmbDiscountType As System.Windows.Forms.ComboBox
    Friend WithEvents lblDiscount As System.Windows.Forms.Label
    Friend WithEvents txtDiscount As System.Windows.Forms.TextBox
    Friend WithEvents lblDiscountType As System.Windows.Forms.Label
    Friend WithEvents lblUPC As System.Windows.Forms.Label
    Friend WithEvents lblDesc As System.Windows.Forms.Label
    Friend WithEvents lblDescValue As System.Windows.Forms.Label
    Friend WithEvents lblUPCValue As System.Windows.Forms.Label
    Friend WithEvents lblPercent As System.Windows.Forms.Label
End Class
