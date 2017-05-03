<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class UpdateShrink
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

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblDescription = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.lblDesc = New System.Windows.Forms.Label
        Me.lblUOM = New System.Windows.Forms.Label
        Me.cmdUpdate = New System.Windows.Forms.Button
        Me.txtQty = New System.Windows.Forms.TextBox
        Me.lblQty = New System.Windows.Forms.Label
        Me.lblUPC = New System.Windows.Forms.Label
        Me.lblUOMValue = New System.Windows.Forms.Label
        Me.lblOldQty = New System.Windows.Forms.Label
        Me.lblOldQtyValue = New System.Windows.Forms.Label
        Me.lblUPCVal = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblDescription
        '
        Me.lblDescription.Location = New System.Drawing.Point(58, 60)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(174, 43)
        '
        'cmdCancel
        '
        Me.cmdCancel.Location = New System.Drawing.Point(3, 213)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(109, 27)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "Cancel"
        '
        'lblDesc
        '
        Me.lblDesc.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblDesc.Location = New System.Drawing.Point(7, 60)
        Me.lblDesc.Name = "lblDesc"
        Me.lblDesc.Size = New System.Drawing.Size(45, 20)
        Me.lblDesc.Text = "Desc:"
        '
        'lblUOM
        '
        Me.lblUOM.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblUOM.Location = New System.Drawing.Point(7, 174)
        Me.lblUOM.Name = "lblUOM"
        Me.lblUOM.Size = New System.Drawing.Size(56, 20)
        Me.lblUOM.Text = "UOM:"
        '
        'cmdUpdate
        '
        Me.cmdUpdate.Location = New System.Drawing.Point(123, 213)
        Me.cmdUpdate.Name = "cmdUpdate"
        Me.cmdUpdate.Size = New System.Drawing.Size(109, 27)
        Me.cmdUpdate.TabIndex = 3
        Me.cmdUpdate.Text = "Update"
        '
        'txtQty
        '
        Me.txtQty.Location = New System.Drawing.Point(139, 137)
        Me.txtQty.Name = "txtQty"
        Me.txtQty.Size = New System.Drawing.Size(51, 21)
        Me.txtQty.TabIndex = 1
        Me.txtQty.Text = "1"
        '
        'lblQty
        '
        Me.lblQty.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblQty.Location = New System.Drawing.Point(7, 138)
        Me.lblQty.Name = "lblQty"
        Me.lblQty.Size = New System.Drawing.Size(126, 20)
        Me.lblQty.Text = "New Quantity:"
        '
        'lblUPC
        '
        Me.lblUPC.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblUPC.Location = New System.Drawing.Point(7, 35)
        Me.lblUPC.Name = "lblUPC"
        Me.lblUPC.Size = New System.Drawing.Size(45, 20)
        Me.lblUPC.Text = "UPC:"
        '
        'lblUOMValue
        '
        Me.lblUOMValue.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblUOMValue.Location = New System.Drawing.Point(69, 174)
        Me.lblUOMValue.Name = "lblUOMValue"
        Me.lblUOMValue.Size = New System.Drawing.Size(148, 20)
        '
        'lblOldQty
        '
        Me.lblOldQty.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblOldQty.Location = New System.Drawing.Point(7, 114)
        Me.lblOldQty.Name = "lblOldQty"
        Me.lblOldQty.Size = New System.Drawing.Size(126, 20)
        Me.lblOldQty.Text = "Quantity Recorded:"
        '
        'lblOldQtyValue
        '
        Me.lblOldQtyValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblOldQtyValue.Location = New System.Drawing.Point(139, 114)
        Me.lblOldQtyValue.Name = "lblOldQtyValue"
        Me.lblOldQtyValue.Size = New System.Drawing.Size(51, 20)
        '
        'lblUPCVal
        '
        Me.lblUPCVal.Location = New System.Drawing.Point(58, 34)
        Me.lblUPCVal.Name = "lblUPCVal"
        Me.lblUPCVal.Size = New System.Drawing.Size(174, 20)
        '
        'lblName
        '
        Me.lblName.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblName.Location = New System.Drawing.Point(47, 0)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(141, 34)
        '
        'UpdateShrink
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 294)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.lblUPCVal)
        Me.Controls.Add(Me.lblOldQtyValue)
        Me.Controls.Add(Me.lblOldQty)
        Me.Controls.Add(Me.lblUOMValue)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.lblDesc)
        Me.Controls.Add(Me.lblUOM)
        Me.Controls.Add(Me.cmdUpdate)
        Me.Controls.Add(Me.txtQty)
        Me.Controls.Add(Me.lblQty)
        Me.Controls.Add(Me.lblUPC)
        Me.Name = "UpdateShrink"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents lblDesc As System.Windows.Forms.Label
    Friend WithEvents lblUOM As System.Windows.Forms.Label
    Friend WithEvents cmdUpdate As System.Windows.Forms.Button
    Friend WithEvents txtQty As System.Windows.Forms.TextBox
    Friend WithEvents lblQty As System.Windows.Forms.Label
    Friend WithEvents lblUPC As System.Windows.Forms.Label
    Friend WithEvents lblUOMValue As System.Windows.Forms.Label
    Friend WithEvents lblOldQty As System.Windows.Forms.Label
    Friend WithEvents lblOldQtyValue As System.Windows.Forms.Label
    Friend WithEvents lblUPCVal As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
End Class
