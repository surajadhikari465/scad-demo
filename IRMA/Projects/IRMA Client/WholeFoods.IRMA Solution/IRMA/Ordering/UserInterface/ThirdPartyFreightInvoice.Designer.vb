<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmThirdPartyFreightInvoice
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmThirdPartyFreightInvoice))
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Me.TextBox_InvoiceNum = New System.Windows.Forms.TextBox
        Me.Button_Exit = New System.Windows.Forms.Button
        Me.Label_InvFrghtTot = New System.Windows.Forms.Label
        Me.Label_InvoiceNum = New System.Windows.Forms.Label
        Me.Label_Date = New System.Windows.Forms.Label
        Me.UltraDateTime_InvoiceDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.Label_PONum = New System.Windows.Forms.Label
        Me.TextBox_VendorKey = New System.Windows.Forms.TextBox
        Me.Label_VendorKey = New System.Windows.Forms.Label
        Me.Button_SearchVendor = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.UltraNumericEditor_PONum = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.UltraNumericEditor_InvFrghtTot = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.Label_VendorID = New System.Windows.Forms.Label
        Me.UltraNumericEditor_VendorID = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.lblUploaded = New System.Windows.Forms.Label
        Me.txtUploadedDate = New System.Windows.Forms.TextBox
        CType(Me.UltraDateTime_InvoiceDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraNumericEditor_PONum, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraNumericEditor_InvFrghtTot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraNumericEditor_VendorID, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TextBox_InvoiceNum
        '
        Me.TextBox_InvoiceNum.AcceptsReturn = True
        Me.TextBox_InvoiceNum.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_InvoiceNum.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox_InvoiceNum.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_InvoiceNum.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox_InvoiceNum.Location = New System.Drawing.Point(141, 128)
        Me.TextBox_InvoiceNum.MaxLength = 16
        Me.TextBox_InvoiceNum.Name = "TextBox_InvoiceNum"
        Me.TextBox_InvoiceNum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox_InvoiceNum.Size = New System.Drawing.Size(121, 20)
        Me.TextBox_InvoiceNum.TabIndex = 3
        Me.TextBox_InvoiceNum.Tag = "String"
        '
        'Button_Exit
        '
        Me.Button_Exit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Exit.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Exit.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Exit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Exit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_Exit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Exit.Image = CType(resources.GetObject("Button_Exit.Image"), System.Drawing.Image)
        Me.Button_Exit.Location = New System.Drawing.Point(261, 222)
        Me.Button_Exit.Name = "Button_Exit"
        Me.Button_Exit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Exit.Size = New System.Drawing.Size(41, 41)
        Me.Button_Exit.TabIndex = 6
        Me.Button_Exit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button_Exit.UseVisualStyleBackColor = False
        '
        'Label_InvFrghtTot
        '
        Me.Label_InvFrghtTot.BackColor = System.Drawing.Color.Transparent
        Me.Label_InvFrghtTot.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_InvFrghtTot.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_InvFrghtTot.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_InvFrghtTot.Location = New System.Drawing.Point(45, 183)
        Me.Label_InvFrghtTot.Name = "Label_InvFrghtTot"
        Me.Label_InvFrghtTot.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_InvFrghtTot.Size = New System.Drawing.Size(89, 17)
        Me.Label_InvFrghtTot.TabIndex = 109
        Me.Label_InvFrghtTot.Text = "Invoice Total :"
        Me.Label_InvFrghtTot.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.ToolTip1.SetToolTip(Me.Label_InvFrghtTot, "Sum of Freight Total & Cost Total")
        '
        'Label_InvoiceNum
        '
        Me.Label_InvoiceNum.BackColor = System.Drawing.Color.Transparent
        Me.Label_InvoiceNum.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_InvoiceNum.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_InvoiceNum.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_InvoiceNum.Location = New System.Drawing.Point(45, 131)
        Me.Label_InvoiceNum.Name = "Label_InvoiceNum"
        Me.Label_InvoiceNum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_InvoiceNum.Size = New System.Drawing.Size(89, 17)
        Me.Label_InvoiceNum.TabIndex = 106
        Me.Label_InvoiceNum.Text = "Invoice # :"
        Me.Label_InvoiceNum.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_Date
        '
        Me.Label_Date.BackColor = System.Drawing.Color.Transparent
        Me.Label_Date.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_Date.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Date.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Date.Location = New System.Drawing.Point(45, 156)
        Me.Label_Date.Name = "Label_Date"
        Me.Label_Date.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_Date.Size = New System.Drawing.Size(89, 17)
        Me.Label_Date.TabIndex = 105
        Me.Label_Date.Text = " Date :"
        Me.Label_Date.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'UltraDateTime_InvoiceDate
        '
        Me.UltraDateTime_InvoiceDate.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.UltraDateTime_InvoiceDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraDateTime_InvoiceDate.Location = New System.Drawing.Point(141, 152)
        Me.UltraDateTime_InvoiceDate.MaskInput = ""
        Me.UltraDateTime_InvoiceDate.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me.UltraDateTime_InvoiceDate.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.UltraDateTime_InvoiceDate.Name = "UltraDateTime_InvoiceDate"
        Me.UltraDateTime_InvoiceDate.Size = New System.Drawing.Size(85, 21)
        Me.UltraDateTime_InvoiceDate.TabIndex = 4
        Me.UltraDateTime_InvoiceDate.Value = Nothing
        '
        'Label_PONum
        '
        Me.Label_PONum.BackColor = System.Drawing.Color.Transparent
        Me.Label_PONum.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_PONum.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_PONum.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_PONum.Location = New System.Drawing.Point(20, 17)
        Me.Label_PONum.Name = "Label_PONum"
        Me.Label_PONum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_PONum.Size = New System.Drawing.Size(114, 20)
        Me.Label_PONum.TabIndex = 114
        Me.Label_PONum.Text = "Purchase Order # :"
        Me.Label_PONum.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'TextBox_VendorKey
        '
        Me.TextBox_VendorKey.AcceptsReturn = True
        Me.TextBox_VendorKey.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_VendorKey.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox_VendorKey.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_VendorKey.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox_VendorKey.Location = New System.Drawing.Point(140, 78)
        Me.TextBox_VendorKey.MaxLength = 10
        Me.TextBox_VendorKey.Name = "TextBox_VendorKey"
        Me.TextBox_VendorKey.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox_VendorKey.Size = New System.Drawing.Size(121, 20)
        Me.TextBox_VendorKey.TabIndex = 0
        Me.TextBox_VendorKey.Tag = "String"
        '
        'Label_VendorKey
        '
        Me.Label_VendorKey.BackColor = System.Drawing.Color.Transparent
        Me.Label_VendorKey.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_VendorKey.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_VendorKey.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_VendorKey.Location = New System.Drawing.Point(20, 81)
        Me.Label_VendorKey.Name = "Label_VendorKey"
        Me.Label_VendorKey.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_VendorKey.Size = New System.Drawing.Size(114, 20)
        Me.Label_VendorKey.TabIndex = 116
        Me.Label_VendorKey.Text = "Vendor Key :"
        Me.Label_VendorKey.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Button_SearchVendor
        '
        Me.Button_SearchVendor.BackColor = System.Drawing.Color.Silver
        Me.Button_SearchVendor.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_SearchVendor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_SearchVendor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_SearchVendor.Image = CType(resources.GetObject("Button_SearchVendor.Image"), System.Drawing.Image)
        Me.Button_SearchVendor.Location = New System.Drawing.Point(267, 69)
        Me.Button_SearchVendor.Name = "Button_SearchVendor"
        Me.Button_SearchVendor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_SearchVendor.Size = New System.Drawing.Size(36, 32)
        Me.Button_SearchVendor.TabIndex = 1
        Me.Button_SearchVendor.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.Button_SearchVendor, "Search For Vendor")
        Me.Button_SearchVendor.UseVisualStyleBackColor = False
        '
        'UltraNumericEditor_PONum
        '
        Me.UltraNumericEditor_PONum.AlwaysInEditMode = True
        Appearance2.TextHAlign = Infragistics.Win.HAlign.Center
        Me.UltraNumericEditor_PONum.Appearance = Appearance2
        Me.UltraNumericEditor_PONum.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraNumericEditor_PONum.Location = New System.Drawing.Point(140, 14)
        Me.UltraNumericEditor_PONum.MaxValue = 1999999999
        Me.UltraNumericEditor_PONum.MinValue = 1
        Me.UltraNumericEditor_PONum.Name = "UltraNumericEditor_PONum"
        Me.UltraNumericEditor_PONum.Nullable = True
        Me.UltraNumericEditor_PONum.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.UltraNumericEditor_PONum.ReadOnly = True
        Me.UltraNumericEditor_PONum.Size = New System.Drawing.Size(85, 21)
        Me.UltraNumericEditor_PONum.TabIndex = 5
        Me.UltraNumericEditor_PONum.TabStop = False
        Me.UltraNumericEditor_PONum.Value = Nothing
        '
        'UltraNumericEditor_InvFrghtTot
        '
        Me.UltraNumericEditor_InvFrghtTot.AlwaysInEditMode = True
        Me.UltraNumericEditor_InvFrghtTot.Location = New System.Drawing.Point(140, 179)
        Me.UltraNumericEditor_InvFrghtTot.MaskInput = "{LOC}nnnnnnn.nn"
        Me.UltraNumericEditor_InvFrghtTot.MaxValue = 922337203685477.62
        Me.UltraNumericEditor_InvFrghtTot.MinValue = 0
        Me.UltraNumericEditor_InvFrghtTot.Name = "UltraNumericEditor_InvFrghtTot"
        Me.UltraNumericEditor_InvFrghtTot.Nullable = True
        Me.UltraNumericEditor_InvFrghtTot.NullText = "0"
        Me.UltraNumericEditor_InvFrghtTot.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.UltraNumericEditor_InvFrghtTot.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.UltraNumericEditor_InvFrghtTot.Size = New System.Drawing.Size(86, 21)
        Me.UltraNumericEditor_InvFrghtTot.TabIndex = 5
        '
        'Label_VendorID
        '
        Me.Label_VendorID.AutoSize = True
        Me.Label_VendorID.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_VendorID.Location = New System.Drawing.Point(67, 105)
        Me.Label_VendorID.Name = "Label_VendorID"
        Me.Label_VendorID.Size = New System.Drawing.Size(67, 14)
        Me.Label_VendorID.TabIndex = 125
        Me.Label_VendorID.Text = "Vendor ID :"
        '
        'UltraNumericEditor_VendorID
        '
        Me.UltraNumericEditor_VendorID.Location = New System.Drawing.Point(140, 101)
        Me.UltraNumericEditor_VendorID.MaskInput = "nnnnnnnnn"
        Me.UltraNumericEditor_VendorID.MinValue = 0
        Me.UltraNumericEditor_VendorID.Name = "UltraNumericEditor_VendorID"
        Me.UltraNumericEditor_VendorID.Nullable = True
        Me.UltraNumericEditor_VendorID.Size = New System.Drawing.Size(121, 21)
        Me.UltraNumericEditor_VendorID.TabIndex = 2
        Me.UltraNumericEditor_VendorID.Value = Nothing
        '
        'lblUploaded
        '
        Me.lblUploaded.BackColor = System.Drawing.Color.Transparent
        Me.lblUploaded.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUploaded.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUploaded.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUploaded.Location = New System.Drawing.Point(20, 44)
        Me.lblUploaded.Name = "lblUploaded"
        Me.lblUploaded.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUploaded.Size = New System.Drawing.Size(114, 20)
        Me.lblUploaded.TabIndex = 127
        Me.lblUploaded.Text = "Uploaded"
        Me.lblUploaded.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtUploadedDate
        '
        Me.txtUploadedDate.AcceptsReturn = True
        Me.txtUploadedDate.BackColor = System.Drawing.SystemColors.Control
        Me.txtUploadedDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtUploadedDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUploadedDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtUploadedDate.Location = New System.Drawing.Point(140, 41)
        Me.txtUploadedDate.MaxLength = 10
        Me.txtUploadedDate.Name = "txtUploadedDate"
        Me.txtUploadedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtUploadedDate.Size = New System.Drawing.Size(86, 20)
        Me.txtUploadedDate.TabIndex = 128
        Me.txtUploadedDate.Tag = "String"
        '
        'frmThirdPartyFreightInvoice
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(314, 275)
        Me.Controls.Add(Me.txtUploadedDate)
        Me.Controls.Add(Me.lblUploaded)
        Me.Controls.Add(Me.UltraNumericEditor_VendorID)
        Me.Controls.Add(Me.Label_VendorID)
        Me.Controls.Add(Me.UltraNumericEditor_InvFrghtTot)
        Me.Controls.Add(Me.UltraNumericEditor_PONum)
        Me.Controls.Add(Me.Button_SearchVendor)
        Me.Controls.Add(Me.TextBox_VendorKey)
        Me.Controls.Add(Me.Label_VendorKey)
        Me.Controls.Add(Me.Label_PONum)
        Me.Controls.Add(Me.TextBox_InvoiceNum)
        Me.Controls.Add(Me.Button_Exit)
        Me.Controls.Add(Me.Label_InvFrghtTot)
        Me.Controls.Add(Me.Label_InvoiceNum)
        Me.Controls.Add(Me.Label_Date)
        Me.Controls.Add(Me.UltraDateTime_InvoiceDate)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmThirdPartyFreightInvoice"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "3rd Party Freight Invoice Data"
        CType(Me.UltraDateTime_InvoiceDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraNumericEditor_PONum, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraNumericEditor_InvFrghtTot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraNumericEditor_VendorID, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents TextBox_InvoiceNum As System.Windows.Forms.TextBox
    Public WithEvents Button_Exit As System.Windows.Forms.Button
    Public WithEvents Label_InvFrghtTot As System.Windows.Forms.Label
    Public WithEvents Label_InvoiceNum As System.Windows.Forms.Label
    Public WithEvents Label_Date As System.Windows.Forms.Label
    Friend WithEvents UltraDateTime_InvoiceDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Public WithEvents Label_PONum As System.Windows.Forms.Label
    Public WithEvents TextBox_VendorKey As System.Windows.Forms.TextBox
    Public WithEvents Label_VendorKey As System.Windows.Forms.Label
    Public WithEvents Button_SearchVendor As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents UltraNumericEditor_PONum As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents UltraNumericEditor_InvFrghtTot As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents Label_VendorID As System.Windows.Forms.Label
    Friend WithEvents UltraNumericEditor_VendorID As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Public WithEvents lblUploaded As System.Windows.Forms.Label
    Public WithEvents txtUploadedDate As System.Windows.Forms.TextBox
End Class
