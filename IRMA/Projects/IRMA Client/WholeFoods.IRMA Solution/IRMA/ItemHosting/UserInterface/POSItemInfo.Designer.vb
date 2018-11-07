<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class POSItemInfo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(POSItemInfo))
        Me.Label_IdentifierValue = New System.Windows.Forms.Label
        Me.Label_Identifier = New System.Windows.Forms.Label
        Me.cmdExit = New System.Windows.Forms.Button
        Me.GroupBox_Common = New System.Windows.Forms.GroupBox
        Me.TextBox_ProductCode = New System.Windows.Forms.TextBox
        Me.TextBox_UnitPriceCategory = New System.Windows.Forms.TextBox
        Me.Label_UnitPriceCategory = New System.Windows.Forms.Label
        Me.Label_ProductCode = New System.Windows.Forms.Label
        Me.TextBox_MiscTransSale = New System.Windows.Forms.TextBox
        Me.TextBox_MiscTransRefund = New System.Windows.Forms.TextBox
        Me.Label_IceTare = New System.Windows.Forms.Label
        Me.TextBox_IceTare = New System.Windows.Forms.TextBox
        Me.Label_MiscTransRefund = New System.Windows.Forms.Label
        Me.Label_MiscTransSale = New System.Windows.Forms.Label
        Me.CheckBox_CouponMultiplier = New System.Windows.Forms.CheckBox
        Me.CheckBox_CaseDiscount = New System.Windows.Forms.CheckBox
        Me.Label_GroupList = New System.Windows.Forms.Label
        Me.TextBox_GroupList = New System.Windows.Forms.TextBox
        Me.CheckBox_PriceRequired = New System.Windows.Forms.CheckBox
        Me.CheckBox_QuantityProhibit = New System.Windows.Forms.CheckBox
        Me.CheckBox_QuantityRequired = New System.Windows.Forms.CheckBox
        Me.CheckBox_FoodStamps = New System.Windows.Forms.CheckBox
        Me.CheckBox_FSAEligible = New System.Windows.Forms.CheckBox
        Me.GroupBox_Common.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label_IdentifierValue
        '
        Me.Label_IdentifierValue.AutoSize = True
        Me.Label_IdentifierValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_IdentifierValue.Location = New System.Drawing.Point(76, 9)
        Me.Label_IdentifierValue.Name = "Label_IdentifierValue"
        Me.Label_IdentifierValue.Size = New System.Drawing.Size(45, 13)
        Me.Label_IdentifierValue.TabIndex = 99
        Me.Label_IdentifierValue.Text = "Label1"
        '
        'Label_Identifier
        '
        Me.Label_Identifier.BackColor = System.Drawing.Color.Transparent
        Me.Label_Identifier.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_Identifier.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Identifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Identifier.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_Identifier.Location = New System.Drawing.Point(14, 9)
        Me.Label_Identifier.Name = "Label_Identifier"
        Me.Label_Identifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_Identifier.Size = New System.Drawing.Size(56, 14)
        Me.Label_Identifier.TabIndex = 98
        Me.Label_Identifier.Text = "Identifier :"
        Me.Label_Identifier.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdExit.Location = New System.Drawing.Point(309, 228)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 100
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'GroupBox_Common
        '
        Me.GroupBox_Common.Controls.Add(Me.CheckBox_FSAEligible)
        Me.GroupBox_Common.Controls.Add(Me.TextBox_ProductCode)
        Me.GroupBox_Common.Controls.Add(Me.TextBox_UnitPriceCategory)
        Me.GroupBox_Common.Controls.Add(Me.Label_UnitPriceCategory)
        Me.GroupBox_Common.Controls.Add(Me.Label_ProductCode)
        Me.GroupBox_Common.Controls.Add(Me.TextBox_MiscTransSale)
        Me.GroupBox_Common.Controls.Add(Me.TextBox_MiscTransRefund)
        Me.GroupBox_Common.Controls.Add(Me.Label_IceTare)
        Me.GroupBox_Common.Controls.Add(Me.TextBox_IceTare)
        Me.GroupBox_Common.Controls.Add(Me.Label_MiscTransRefund)
        Me.GroupBox_Common.Controls.Add(Me.Label_MiscTransSale)
        Me.GroupBox_Common.Controls.Add(Me.CheckBox_CouponMultiplier)
        Me.GroupBox_Common.Controls.Add(Me.CheckBox_CaseDiscount)
        Me.GroupBox_Common.Controls.Add(Me.Label_GroupList)
        Me.GroupBox_Common.Controls.Add(Me.TextBox_GroupList)
        Me.GroupBox_Common.Controls.Add(Me.CheckBox_PriceRequired)
        Me.GroupBox_Common.Controls.Add(Me.CheckBox_QuantityProhibit)
        Me.GroupBox_Common.Controls.Add(Me.CheckBox_QuantityRequired)
        Me.GroupBox_Common.Controls.Add(Me.CheckBox_FoodStamps)
        Me.GroupBox_Common.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox_Common.Location = New System.Drawing.Point(12, 36)
        Me.GroupBox_Common.Name = "GroupBox_Common"
        Me.GroupBox_Common.Size = New System.Drawing.Size(338, 186)
        Me.GroupBox_Common.TabIndex = 101
        Me.GroupBox_Common.TabStop = False
        Me.GroupBox_Common.Text = "POS Settings"
        '
        'TextBox_ProductCode
        '
        Me.TextBox_ProductCode.Location = New System.Drawing.Point(290, 110)
        Me.TextBox_ProductCode.Name = "TextBox_ProductCode"
        Me.TextBox_ProductCode.Size = New System.Drawing.Size(42, 20)
        Me.TextBox_ProductCode.TabIndex = 30
        '
        'TextBox_UnitPriceCategory
        '
        Me.TextBox_UnitPriceCategory.Location = New System.Drawing.Point(290, 133)
        Me.TextBox_UnitPriceCategory.Name = "TextBox_UnitPriceCategory"
        Me.TextBox_UnitPriceCategory.Size = New System.Drawing.Size(42, 20)
        Me.TextBox_UnitPriceCategory.TabIndex = 29
        '
        'Label_UnitPriceCategory
        '
        Me.Label_UnitPriceCategory.AutoSize = True
        Me.Label_UnitPriceCategory.Location = New System.Drawing.Point(186, 136)
        Me.Label_UnitPriceCategory.Name = "Label_UnitPriceCategory"
        Me.Label_UnitPriceCategory.Size = New System.Drawing.Size(98, 13)
        Me.Label_UnitPriceCategory.TabIndex = 28
        Me.Label_UnitPriceCategory.Text = "Unit Price Category"
        '
        'Label_ProductCode
        '
        Me.Label_ProductCode.AutoSize = True
        Me.Label_ProductCode.Location = New System.Drawing.Point(212, 113)
        Me.Label_ProductCode.Name = "Label_ProductCode"
        Me.Label_ProductCode.Size = New System.Drawing.Size(72, 13)
        Me.Label_ProductCode.TabIndex = 27
        Me.Label_ProductCode.Text = "Product Code"
        '
        'TextBox_MiscTransSale
        '
        Me.TextBox_MiscTransSale.Location = New System.Drawing.Point(290, 64)
        Me.TextBox_MiscTransSale.Name = "TextBox_MiscTransSale"
        Me.TextBox_MiscTransSale.Size = New System.Drawing.Size(42, 20)
        Me.TextBox_MiscTransSale.TabIndex = 26
        '
        'TextBox_MiscTransRefund
        '
        Me.TextBox_MiscTransRefund.Location = New System.Drawing.Point(290, 87)
        Me.TextBox_MiscTransRefund.Name = "TextBox_MiscTransRefund"
        Me.TextBox_MiscTransRefund.Size = New System.Drawing.Size(42, 20)
        Me.TextBox_MiscTransRefund.TabIndex = 25
        '
        'Label_IceTare
        '
        Me.Label_IceTare.AutoSize = True
        Me.Label_IceTare.Location = New System.Drawing.Point(237, 44)
        Me.Label_IceTare.Name = "Label_IceTare"
        Me.Label_IceTare.Size = New System.Drawing.Size(47, 13)
        Me.Label_IceTare.TabIndex = 24
        Me.Label_IceTare.Text = "Ice Tare"
        '
        'TextBox_IceTare
        '
        Me.TextBox_IceTare.Location = New System.Drawing.Point(290, 41)
        Me.TextBox_IceTare.Name = "TextBox_IceTare"
        Me.TextBox_IceTare.Size = New System.Drawing.Size(42, 20)
        Me.TextBox_IceTare.TabIndex = 23
        '
        'Label_MiscTransRefund
        '
        Me.Label_MiscTransRefund.AutoSize = True
        Me.Label_MiscTransRefund.Location = New System.Drawing.Point(152, 89)
        Me.Label_MiscTransRefund.Name = "Label_MiscTransRefund"
        Me.Label_MiscTransRefund.Size = New System.Drawing.Size(132, 13)
        Me.Label_MiscTransRefund.TabIndex = 21
        Me.Label_MiscTransRefund.Text = "Misc Transaction (Refund)"
        '
        'Label_MiscTransSale
        '
        Me.Label_MiscTransSale.AutoSize = True
        Me.Label_MiscTransSale.Location = New System.Drawing.Point(166, 67)
        Me.Label_MiscTransSale.Name = "Label_MiscTransSale"
        Me.Label_MiscTransSale.Size = New System.Drawing.Size(118, 13)
        Me.Label_MiscTransSale.TabIndex = 20
        Me.Label_MiscTransSale.Text = "Misc Transaction (Sale)"
        '
        'CheckBox_CouponMultiplier
        '
        Me.CheckBox_CouponMultiplier.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_CouponMultiplier.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox_CouponMultiplier.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.CheckBox_CouponMultiplier.Location = New System.Drawing.Point(5, 133)
        Me.CheckBox_CouponMultiplier.Name = "CheckBox_CouponMultiplier"
        Me.CheckBox_CouponMultiplier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_CouponMultiplier.Size = New System.Drawing.Size(137, 20)
        Me.CheckBox_CouponMultiplier.TabIndex = 16
        Me.CheckBox_CouponMultiplier.Text = "Coupon Multiplier"
        Me.CheckBox_CouponMultiplier.UseVisualStyleBackColor = False
        '
        'CheckBox_CaseDiscount
        '
        Me.CheckBox_CaseDiscount.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_CaseDiscount.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox_CaseDiscount.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.CheckBox_CaseDiscount.Location = New System.Drawing.Point(5, 110)
        Me.CheckBox_CaseDiscount.Name = "CheckBox_CaseDiscount"
        Me.CheckBox_CaseDiscount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_CaseDiscount.Size = New System.Drawing.Size(137, 20)
        Me.CheckBox_CaseDiscount.TabIndex = 14
        Me.CheckBox_CaseDiscount.Text = "Case Discount"
        Me.CheckBox_CaseDiscount.UseVisualStyleBackColor = False
        '
        'Label_GroupList
        '
        Me.Label_GroupList.AutoSize = True
        Me.Label_GroupList.Location = New System.Drawing.Point(229, 22)
        Me.Label_GroupList.Name = "Label_GroupList"
        Me.Label_GroupList.Size = New System.Drawing.Size(55, 13)
        Me.Label_GroupList.TabIndex = 1
        Me.Label_GroupList.Text = "Group List"
        '
        'TextBox_GroupList
        '
        Me.TextBox_GroupList.Location = New System.Drawing.Point(290, 18)
        Me.TextBox_GroupList.MaxLength = 2
        Me.TextBox_GroupList.Name = "TextBox_GroupList"
        Me.TextBox_GroupList.Size = New System.Drawing.Size(42, 20)
        Me.TextBox_GroupList.TabIndex = 3
        '
        'CheckBox_PriceRequired
        '
        Me.CheckBox_PriceRequired.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_PriceRequired.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox_PriceRequired.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.CheckBox_PriceRequired.Location = New System.Drawing.Point(5, 87)
        Me.CheckBox_PriceRequired.Name = "CheckBox_PriceRequired"
        Me.CheckBox_PriceRequired.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_PriceRequired.Size = New System.Drawing.Size(137, 20)
        Me.CheckBox_PriceRequired.TabIndex = 11
        Me.CheckBox_PriceRequired.Text = "Price Required"
        Me.CheckBox_PriceRequired.UseVisualStyleBackColor = False
        '
        'CheckBox_QuantityProhibit
        '
        Me.CheckBox_QuantityProhibit.AutoSize = True
        Me.CheckBox_QuantityProhibit.Location = New System.Drawing.Point(5, 43)
        Me.CheckBox_QuantityProhibit.Name = "CheckBox_QuantityProhibit"
        Me.CheckBox_QuantityProhibit.Size = New System.Drawing.Size(103, 17)
        Me.CheckBox_QuantityProhibit.TabIndex = 0
        Me.CheckBox_QuantityProhibit.Text = "Quantity Prohibit"
        Me.CheckBox_QuantityProhibit.UseVisualStyleBackColor = True
        '
        'CheckBox_QuantityRequired
        '
        Me.CheckBox_QuantityRequired.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_QuantityRequired.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox_QuantityRequired.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.CheckBox_QuantityRequired.Location = New System.Drawing.Point(5, 64)
        Me.CheckBox_QuantityRequired.Name = "CheckBox_QuantityRequired"
        Me.CheckBox_QuantityRequired.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_QuantityRequired.Size = New System.Drawing.Size(137, 20)
        Me.CheckBox_QuantityRequired.TabIndex = 12
        Me.CheckBox_QuantityRequired.Text = "Quantity Required"
        Me.CheckBox_QuantityRequired.UseVisualStyleBackColor = False
        '
        'CheckBox_FoodStamps
        '
        Me.CheckBox_FoodStamps.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_FoodStamps.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox_FoodStamps.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.CheckBox_FoodStamps.Location = New System.Drawing.Point(5, 19)
        Me.CheckBox_FoodStamps.Name = "CheckBox_FoodStamps"
        Me.CheckBox_FoodStamps.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_FoodStamps.Size = New System.Drawing.Size(137, 20)
        Me.CheckBox_FoodStamps.TabIndex = 13
        Me.CheckBox_FoodStamps.Text = "Food Stamps"
        Me.CheckBox_FoodStamps.UseVisualStyleBackColor = False
        '
        'CheckBox_FSAEligible
        '
        Me.CheckBox_FSAEligible.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_FSAEligible.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox_FSAEligible.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.CheckBox_FSAEligible.Location = New System.Drawing.Point(5, 159)
        Me.CheckBox_FSAEligible.Name = "CheckBox_FSAEligible"
        Me.CheckBox_FSAEligible.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_FSAEligible.Size = New System.Drawing.Size(137, 20)
        Me.CheckBox_FSAEligible.TabIndex = 31
        Me.CheckBox_FSAEligible.Text = "FSA Eligible"
        Me.CheckBox_FSAEligible.UseVisualStyleBackColor = False
        '
        'POSItemInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(364, 281)
        Me.Controls.Add(Me.GroupBox_Common)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.Label_IdentifierValue)
        Me.Controls.Add(Me.Label_Identifier)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "POSItemInfo"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "POS Info"
        Me.GroupBox_Common.ResumeLayout(False)
        Me.GroupBox_Common.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label_IdentifierValue As System.Windows.Forms.Label
    Public WithEvents Label_Identifier As System.Windows.Forms.Label
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Friend WithEvents GroupBox_Common As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox_QuantityProhibit As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox_PriceRequired As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox_QuantityRequired As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox_FoodStamps As System.Windows.Forms.CheckBox
    Friend WithEvents Label_GroupList As System.Windows.Forms.Label
    Friend WithEvents TextBox_GroupList As System.Windows.Forms.TextBox
    Public WithEvents CheckBox_CaseDiscount As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox_CouponMultiplier As System.Windows.Forms.CheckBox
    Friend WithEvents Label_MiscTransRefund As System.Windows.Forms.Label
    Friend WithEvents Label_MiscTransSale As System.Windows.Forms.Label
    Friend WithEvents Label_IceTare As System.Windows.Forms.Label
    Friend WithEvents TextBox_IceTare As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_MiscTransSale As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_MiscTransRefund As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_ProductCode As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_UnitPriceCategory As System.Windows.Forms.TextBox
    Friend WithEvents Label_UnitPriceCategory As System.Windows.Forms.Label
    Friend WithEvents Label_ProductCode As System.Windows.Forms.Label
    Public WithEvents CheckBox_FSAEligible As System.Windows.Forms.CheckBox
End Class
