<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InventoryAdjustmentCode
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InventoryAdjustmentCode))
        Me.txtCodeAbbrev = New System.Windows.Forms.TextBox
        Me.lblCodeAbbrev = New System.Windows.Forms.Label
        Me.lblCodeDesc = New System.Windows.Forms.Label
        Me.txtCodeDesc = New System.Windows.Forms.TextBox
        Me.lblInventoryEffect = New System.Windows.Forms.Label
        Me.lblGLCode = New System.Windows.Forms.Label
        Me.lblEXEWarehouse = New System.Windows.Forms.Label
        Me.lblStore = New System.Windows.Forms.Label
        Me.cmdSubmit = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmbInventoryEffect = New System.Windows.Forms.ComboBox
        Me.txtGLCode = New System.Windows.Forms.TextBox
        Me.chkEXEWarehouse = New System.Windows.Forms.CheckBox
        Me.chkStore = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'txtCodeAbbrev
        '
        Me.txtCodeAbbrev.AcceptsReturn = True
        Me.txtCodeAbbrev.BackColor = System.Drawing.SystemColors.Window
        Me.txtCodeAbbrev.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCodeAbbrev.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.txtCodeAbbrev.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCodeAbbrev.Location = New System.Drawing.Point(159, 36)
        Me.txtCodeAbbrev.MaxLength = 2
        Me.txtCodeAbbrev.Name = "txtCodeAbbrev"
        Me.txtCodeAbbrev.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCodeAbbrev.Size = New System.Drawing.Size(30, 20)
        Me.txtCodeAbbrev.TabIndex = 3
        Me.txtCodeAbbrev.Tag = "String"
        '
        'lblCodeAbbrev
        '
        Me.lblCodeAbbrev.BackColor = System.Drawing.SystemColors.Control
        Me.lblCodeAbbrev.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCodeAbbrev.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblCodeAbbrev.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCodeAbbrev.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblCodeAbbrev.Location = New System.Drawing.Point(21, 39)
        Me.lblCodeAbbrev.Name = "lblCodeAbbrev"
        Me.lblCodeAbbrev.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCodeAbbrev.Size = New System.Drawing.Size(132, 17)
        Me.lblCodeAbbrev.TabIndex = 2
        Me.lblCodeAbbrev.Text = "Code Abbreviation:"
        Me.lblCodeAbbrev.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'lblCodeDesc
        '
        Me.lblCodeDesc.BackColor = System.Drawing.SystemColors.Control
        Me.lblCodeDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCodeDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblCodeDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCodeDesc.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblCodeDesc.Location = New System.Drawing.Point(30, 67)
        Me.lblCodeDesc.Name = "lblCodeDesc"
        Me.lblCodeDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCodeDesc.Size = New System.Drawing.Size(123, 17)
        Me.lblCodeDesc.TabIndex = 4
        Me.lblCodeDesc.Text = "Code Description:"
        Me.lblCodeDesc.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'txtCodeDesc
        '
        Me.txtCodeDesc.AcceptsReturn = True
        Me.txtCodeDesc.BackColor = System.Drawing.SystemColors.Window
        Me.txtCodeDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCodeDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.txtCodeDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCodeDesc.Location = New System.Drawing.Point(159, 67)
        Me.txtCodeDesc.MaxLength = 50
        Me.txtCodeDesc.Name = "txtCodeDesc"
        Me.txtCodeDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCodeDesc.Size = New System.Drawing.Size(370, 20)
        Me.txtCodeDesc.TabIndex = 5
        Me.txtCodeDesc.Tag = "String"
        '
        'lblInventoryEffect
        '
        Me.lblInventoryEffect.BackColor = System.Drawing.SystemColors.Control
        Me.lblInventoryEffect.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInventoryEffect.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblInventoryEffect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInventoryEffect.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblInventoryEffect.Location = New System.Drawing.Point(30, 97)
        Me.lblInventoryEffect.Name = "lblInventoryEffect"
        Me.lblInventoryEffect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInventoryEffect.Size = New System.Drawing.Size(123, 17)
        Me.lblInventoryEffect.TabIndex = 6
        Me.lblInventoryEffect.Text = "Effect On inventory:"
        Me.lblInventoryEffect.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'lblGLCode
        '
        Me.lblGLCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblGLCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGLCode.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblGLCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGLCode.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblGLCode.Location = New System.Drawing.Point(30, 125)
        Me.lblGLCode.Name = "lblGLCode"
        Me.lblGLCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGLCode.Size = New System.Drawing.Size(123, 17)
        Me.lblGLCode.TabIndex = 7
        Me.lblGLCode.Text = "GL Code:"
        Me.lblGLCode.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'lblEXEWarehouse
        '
        Me.lblEXEWarehouse.BackColor = System.Drawing.SystemColors.Control
        Me.lblEXEWarehouse.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEXEWarehouse.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblEXEWarehouse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEXEWarehouse.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblEXEWarehouse.Location = New System.Drawing.Point(30, 151)
        Me.lblEXEWarehouse.Name = "lblEXEWarehouse"
        Me.lblEXEWarehouse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEXEWarehouse.Size = New System.Drawing.Size(123, 17)
        Me.lblEXEWarehouse.TabIndex = 8
        Me.lblEXEWarehouse.Text = "EXE Warehouse:"
        Me.lblEXEWarehouse.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'lblStore
        '
        Me.lblStore.BackColor = System.Drawing.SystemColors.Control
        Me.lblStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblStore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStore.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblStore.Location = New System.Drawing.Point(30, 180)
        Me.lblStore.Name = "lblStore"
        Me.lblStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStore.Size = New System.Drawing.Size(123, 17)
        Me.lblStore.TabIndex = 9
        Me.lblStore.Text = "Store:"
        Me.lblStore.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'cmdSubmit
        '
        Me.cmdSubmit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSubmit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSubmit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSubmit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSubmit.Image = CType(resources.GetObject("cmdSubmit.Image"), System.Drawing.Image)
        Me.cmdSubmit.Location = New System.Drawing.Point(441, 207)
        Me.cmdSubmit.Name = "cmdSubmit"
        Me.cmdSubmit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSubmit.Size = New System.Drawing.Size(41, 41)
        Me.cmdSubmit.TabIndex = 10
        Me.cmdSubmit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdSubmit.UseVisualStyleBackColor = False
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
        Me.cmdExit.Location = New System.Drawing.Point(488, 207)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 25
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmbInventoryEffect
        '
        Me.cmbInventoryEffect.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbInventoryEffect.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbInventoryEffect.BackColor = System.Drawing.SystemColors.Window
        Me.cmbInventoryEffect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbInventoryEffect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbInventoryEffect.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbInventoryEffect.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbInventoryEffect.Location = New System.Drawing.Point(159, 96)
        Me.cmbInventoryEffect.Name = "cmbInventoryEffect"
        Me.cmbInventoryEffect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbInventoryEffect.Size = New System.Drawing.Size(208, 22)
        Me.cmbInventoryEffect.Sorted = True
        Me.cmbInventoryEffect.TabIndex = 26
        '
        'txtGLCode
        '
        Me.txtGLCode.AcceptsReturn = True
        Me.txtGLCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtGLCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtGLCode.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.txtGLCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtGLCode.Location = New System.Drawing.Point(159, 124)
        Me.txtGLCode.MaxLength = 50
        Me.txtGLCode.Name = "txtGLCode"
        Me.txtGLCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtGLCode.Size = New System.Drawing.Size(83, 20)
        Me.txtGLCode.TabIndex = 27
        Me.txtGLCode.Tag = "String"
        '
        'chkEXEWarehouse
        '
        Me.chkEXEWarehouse.AutoSize = True
        Me.chkEXEWarehouse.Location = New System.Drawing.Point(159, 154)
        Me.chkEXEWarehouse.Name = "chkEXEWarehouse"
        Me.chkEXEWarehouse.Size = New System.Drawing.Size(15, 14)
        Me.chkEXEWarehouse.TabIndex = 28
        Me.chkEXEWarehouse.UseVisualStyleBackColor = True
        '
        'chkStore
        '
        Me.chkStore.AutoSize = True
        Me.chkStore.Location = New System.Drawing.Point(159, 183)
        Me.chkStore.Name = "chkStore"
        Me.chkStore.Size = New System.Drawing.Size(15, 14)
        Me.chkStore.TabIndex = 29
        Me.chkStore.UseVisualStyleBackColor = True
        '
        'InventoryAdjustmentCode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(571, 269)
        Me.Controls.Add(Me.chkStore)
        Me.Controls.Add(Me.chkEXEWarehouse)
        Me.Controls.Add(Me.txtGLCode)
        Me.Controls.Add(Me.cmbInventoryEffect)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdSubmit)
        Me.Controls.Add(Me.lblStore)
        Me.Controls.Add(Me.lblEXEWarehouse)
        Me.Controls.Add(Me.lblGLCode)
        Me.Controls.Add(Me.lblInventoryEffect)
        Me.Controls.Add(Me.txtCodeDesc)
        Me.Controls.Add(Me.lblCodeDesc)
        Me.Controls.Add(Me.txtCodeAbbrev)
        Me.Controls.Add(Me.lblCodeAbbrev)
        Me.Name = "InventoryAdjustmentCode"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents txtCodeAbbrev As System.Windows.Forms.TextBox
    Public WithEvents lblCodeAbbrev As System.Windows.Forms.Label
    Public WithEvents lblCodeDesc As System.Windows.Forms.Label
    Public WithEvents txtCodeDesc As System.Windows.Forms.TextBox
    Public WithEvents lblInventoryEffect As System.Windows.Forms.Label
    Public WithEvents lblGLCode As System.Windows.Forms.Label
    Public WithEvents lblEXEWarehouse As System.Windows.Forms.Label
    Public WithEvents lblStore As System.Windows.Forms.Label
    Public WithEvents cmdSubmit As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmbInventoryEffect As System.Windows.Forms.ComboBox
    Public WithEvents txtGLCode As System.Windows.Forms.TextBox
    Friend WithEvents chkEXEWarehouse As System.Windows.Forms.CheckBox
    Friend WithEvents chkStore As System.Windows.Forms.CheckBox
End Class
