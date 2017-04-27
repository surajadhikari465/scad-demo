<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class selInventoryAdjustmentCode
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(selInventoryAdjustmentCode))
        Me.frameInvAdjCode = New System.Windows.Forms.GroupBox
        Me.cmbInvAdjCode = New System.Windows.Forms.ComboBox
        Me.lblInvAdjCode = New System.Windows.Forms.Label
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.frameInvAdjCode.SuspendLayout()
        Me.SuspendLayout()
        '
        'frameInvAdjCode
        '
        Me.frameInvAdjCode.BackColor = System.Drawing.SystemColors.Control
        Me.frameInvAdjCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.frameInvAdjCode.Controls.Add(Me.cmbInvAdjCode)
        Me.frameInvAdjCode.Controls.Add(Me.lblInvAdjCode)
        Me.frameInvAdjCode.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frameInvAdjCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frameInvAdjCode.Location = New System.Drawing.Point(26, 30)
        Me.frameInvAdjCode.Name = "frameInvAdjCode"
        Me.frameInvAdjCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frameInvAdjCode.Size = New System.Drawing.Size(472, 108)
        Me.frameInvAdjCode.TabIndex = 22
        Me.frameInvAdjCode.TabStop = False
        Me.frameInvAdjCode.Text = "Inventory Adjustment Code"
        '
        'cmbInvAdjCode
        '
        Me.cmbInvAdjCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbInvAdjCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbInvAdjCode.BackColor = System.Drawing.SystemColors.Window
        Me.cmbInvAdjCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbInvAdjCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbInvAdjCode.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbInvAdjCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbInvAdjCode.Location = New System.Drawing.Point(86, 34)
        Me.cmbInvAdjCode.Name = "cmbInvAdjCode"
        Me.cmbInvAdjCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbInvAdjCode.Size = New System.Drawing.Size(352, 22)
        Me.cmbInvAdjCode.Sorted = True
        Me.cmbInvAdjCode.TabIndex = 14
        '
        'lblInvAdjCode
        '
        Me.lblInvAdjCode.BackColor = System.Drawing.Color.Transparent
        Me.lblInvAdjCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInvAdjCode.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInvAdjCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInvAdjCode.Location = New System.Drawing.Point(15, 34)
        Me.lblInvAdjCode.Name = "lblInvAdjCode"
        Me.lblInvAdjCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInvAdjCode.Size = New System.Drawing.Size(65, 22)
        Me.lblInvAdjCode.TabIndex = 15
        Me.lblInvAdjCode.Text = "Code :"
        Me.lblInvAdjCode.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(457, 144)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 24
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Image = CType(resources.GetObject("cmdAdd.Image"), System.Drawing.Image)
        Me.cmdAdd.Location = New System.Drawing.Point(363, 144)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(41, 41)
        Me.cmdAdd.TabIndex = 23
        Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Image = CType(resources.GetObject("cmdEdit.Image"), System.Drawing.Image)
        Me.cmdEdit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdEdit.Location = New System.Drawing.Point(410, 144)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(41, 41)
        Me.cmdEdit.TabIndex = 25
        Me.cmdEdit.Tag = "B"
        Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'selInventoryAdjustmentCode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(531, 204)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.frameInvAdjCode)
        Me.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.Name = "selInventoryAdjustmentCode"
        Me.Text = "Manage Inventory Adjustment Code"
        Me.frameInvAdjCode.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents frameInvAdjCode As System.Windows.Forms.GroupBox
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents cmdEdit As System.Windows.Forms.Button
    Public WithEvents cmbInvAdjCode As System.Windows.Forms.ComboBox
    Public WithEvents lblInvAdjCode As System.Windows.Forms.Label
End Class
