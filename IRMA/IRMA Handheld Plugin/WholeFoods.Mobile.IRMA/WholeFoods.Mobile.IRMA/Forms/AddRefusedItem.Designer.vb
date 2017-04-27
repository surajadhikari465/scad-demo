<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class AddRefusedItem
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
        Me.lblUnit = New System.Windows.Forms.Label
        Me.lblInvCost = New System.Windows.Forms.Label
        Me.lblIdentifier = New System.Windows.Forms.Label
        Me.lblDesc = New System.Windows.Forms.Label
        Me.txtIdentifier = New System.Windows.Forms.TextBox
        Me.txtVIN = New System.Windows.Forms.TextBox
        Me.lblVIN = New System.Windows.Forms.Label
        Me.txtDesc = New System.Windows.Forms.TextBox
        Me.txtInvoiceCost = New System.Windows.Forms.TextBox
        Me.txtInvoiceQuantity = New System.Windows.Forms.TextBox
        Me.lblInvQty = New System.Windows.Forms.Label
        Me.LinkLabelReasonCode = New System.Windows.Forms.LinkLabel
        Me.cmbUnit = New System.Windows.Forms.ComboBox
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
        Me.cmbReasonCode.Enabled = False
        Me.cmbReasonCode.Location = New System.Drawing.Point(81, 220)
        Me.cmbReasonCode.Name = "cmbReasonCode"
        Me.cmbReasonCode.Size = New System.Drawing.Size(46, 22)
        Me.cmbReasonCode.TabIndex = 3
        '
        'lblUnit
        '
        Me.lblUnit.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblUnit.Location = New System.Drawing.Point(7, 140)
        Me.lblUnit.Name = "lblUnit"
        Me.lblUnit.Size = New System.Drawing.Size(68, 20)
        Me.lblUnit.Text = "UOM:"
        '
        'lblInvCost
        '
        Me.lblInvCost.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblInvCost.Location = New System.Drawing.Point(7, 167)
        Me.lblInvCost.Name = "lblInvCost"
        Me.lblInvCost.Size = New System.Drawing.Size(68, 20)
        Me.lblInvCost.Text = "Inv Cost:"
        '
        'lblIdentifier
        '
        Me.lblIdentifier.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblIdentifier.Location = New System.Drawing.Point(7, 9)
        Me.lblIdentifier.Name = "lblIdentifier"
        Me.lblIdentifier.Size = New System.Drawing.Size(68, 20)
        Me.lblIdentifier.Text = "Identifier:"
        '
        'lblDesc
        '
        Me.lblDesc.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblDesc.Location = New System.Drawing.Point(7, 63)
        Me.lblDesc.Name = "lblDesc"
        Me.lblDesc.Size = New System.Drawing.Size(68, 20)
        Me.lblDesc.Text = "Desc:"
        '
        'txtIdentifier
        '
        Me.txtIdentifier.Location = New System.Drawing.Point(81, 9)
        Me.txtIdentifier.Name = "txtIdentifier"
        Me.txtIdentifier.Size = New System.Drawing.Size(119, 21)
        Me.txtIdentifier.TabIndex = 18
        '
        'txtVIN
        '
        Me.txtVIN.Location = New System.Drawing.Point(81, 36)
        Me.txtVIN.Name = "txtVIN"
        Me.txtVIN.Size = New System.Drawing.Size(119, 21)
        Me.txtVIN.TabIndex = 20
        '
        'lblVIN
        '
        Me.lblVIN.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblVIN.Location = New System.Drawing.Point(7, 36)
        Me.lblVIN.Name = "lblVIN"
        Me.lblVIN.Size = New System.Drawing.Size(68, 20)
        Me.lblVIN.Text = "VIN:"
        '
        'txtDesc
        '
        Me.txtDesc.Location = New System.Drawing.Point(81, 63)
        Me.txtDesc.Multiline = True
        Me.txtDesc.Name = "txtDesc"
        Me.txtDesc.Size = New System.Drawing.Size(156, 71)
        Me.txtDesc.TabIndex = 22
        '
        'txtInvoiceCost
        '
        Me.txtInvoiceCost.Location = New System.Drawing.Point(81, 166)
        Me.txtInvoiceCost.Name = "txtInvoiceCost"
        Me.txtInvoiceCost.Size = New System.Drawing.Size(89, 21)
        Me.txtInvoiceCost.TabIndex = 23
        '
        'txtInvoiceQuantity
        '
        Me.txtInvoiceQuantity.Location = New System.Drawing.Point(81, 193)
        Me.txtInvoiceQuantity.Name = "txtInvoiceQuantity"
        Me.txtInvoiceQuantity.Size = New System.Drawing.Size(89, 21)
        Me.txtInvoiceQuantity.TabIndex = 25
        '
        'lblInvQty
        '
        Me.lblInvQty.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblInvQty.Location = New System.Drawing.Point(8, 194)
        Me.lblInvQty.Name = "lblInvQty"
        Me.lblInvQty.Size = New System.Drawing.Size(67, 20)
        Me.lblInvQty.Text = "Inv Qty:"
        '
        'LinkLabelReasonCode
        '
        Me.LinkLabelReasonCode.Font = New System.Drawing.Font("Tahoma", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.LinkLabelReasonCode.Location = New System.Drawing.Point(7, 222)
        Me.LinkLabelReasonCode.Name = "LinkLabelReasonCode"
        Me.LinkLabelReasonCode.Size = New System.Drawing.Size(68, 20)
        Me.LinkLabelReasonCode.TabIndex = 198
        Me.LinkLabelReasonCode.Text = "Code:"
        '
        'cmbUnit
        '
        Me.cmbUnit.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.cmbUnit.Items.Add("EACH")
        Me.cmbUnit.Items.Add("CASE")
        Me.cmbUnit.Items.Add("POUND")
        Me.cmbUnit.Items.Add("KILOGRAM")
        Me.cmbUnit.Items.Add("MILILITER")
        Me.cmbUnit.Items.Add("LITER")
        Me.cmbUnit.Location = New System.Drawing.Point(81, 140)
        Me.cmbUnit.Name = "cmbUnit"
        Me.cmbUnit.Size = New System.Drawing.Size(89, 20)
        Me.cmbUnit.TabIndex = 220
        '
        'AddRefusedItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.cmbUnit)
        Me.Controls.Add(Me.LinkLabelReasonCode)
        Me.Controls.Add(Me.txtInvoiceQuantity)
        Me.Controls.Add(Me.lblInvQty)
        Me.Controls.Add(Me.txtInvoiceCost)
        Me.Controls.Add(Me.txtDesc)
        Me.Controls.Add(Me.txtVIN)
        Me.Controls.Add(Me.lblVIN)
        Me.Controls.Add(Me.txtIdentifier)
        Me.Controls.Add(Me.lblDesc)
        Me.Controls.Add(Me.lblIdentifier)
        Me.Controls.Add(Me.lblInvCost)
        Me.Controls.Add(Me.lblUnit)
        Me.Controls.Add(Me.cmbReasonCode)
        Me.Menu = Me.mainMenu1
        Me.Name = "AddRefusedItem"
        Me.Text = "Add Refused Item"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmbReasonCode As System.Windows.Forms.ComboBox
    Friend WithEvents mmuCancel As System.Windows.Forms.MenuItem
    Friend WithEvents mmuAccept As System.Windows.Forms.MenuItem
    Friend WithEvents lblUnit As System.Windows.Forms.Label
    Friend WithEvents lblInvCost As System.Windows.Forms.Label
    Friend WithEvents lblIdentifier As System.Windows.Forms.Label
    Friend WithEvents lblDesc As System.Windows.Forms.Label
    Friend WithEvents txtIdentifier As System.Windows.Forms.TextBox
    Friend WithEvents txtVIN As System.Windows.Forms.TextBox
    Friend WithEvents lblVIN As System.Windows.Forms.Label
    Friend WithEvents txtDesc As System.Windows.Forms.TextBox
    Friend WithEvents txtInvoiceCost As System.Windows.Forms.TextBox
    Friend WithEvents txtInvoiceQuantity As System.Windows.Forms.TextBox
    Friend WithEvents lblInvQty As System.Windows.Forms.Label
    Friend WithEvents LinkLabelReasonCode As System.Windows.Forms.LinkLabel
    Friend WithEvents cmbUnit As System.Windows.Forms.ComboBox
End Class
