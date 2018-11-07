<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmCopyPO_ExpectedDate
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dtpExpectedDate = New System.Windows.Forms.DateTimePicker()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.LabelExpectedDate = New System.Windows.Forms.Label()
        Me.LabelCopyToStore = New System.Windows.Forms.Label()
        Me.cmbCopyToStore = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'dtpExpectedDate
        '
        Me.dtpExpectedDate.CustomFormat = ""
        Me.dtpExpectedDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.dtpExpectedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpExpectedDate.Location = New System.Drawing.Point(172, 40)
        Me.dtpExpectedDate.Name = "dtpExpectedDate"
        Me.dtpExpectedDate.Size = New System.Drawing.Size(92, 20)
        Me.dtpExpectedDate.TabIndex = 0
        '
        'cmdOK
        '
        Me.cmdOK.Location = New System.Drawing.Point(211, 83)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(75, 23)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(292, 83)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'LabelExpectedDate
        '
        Me.LabelExpectedDate.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelExpectedDate.Location = New System.Drawing.Point(8, 42)
        Me.LabelExpectedDate.Name = "LabelExpectedDate"
        Me.LabelExpectedDate.Size = New System.Drawing.Size(158, 31)
        Me.LabelExpectedDate.TabIndex = 3
        Me.LabelExpectedDate.Text = "Expected Date of New PO:"
        Me.LabelExpectedDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LabelCopyToStore
        '
        Me.LabelCopyToStore.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCopyToStore.Location = New System.Drawing.Point(59, 14)
        Me.LabelCopyToStore.Name = "LabelCopyToStore"
        Me.LabelCopyToStore.Size = New System.Drawing.Size(107, 18)
        Me.LabelCopyToStore.TabIndex = 4
        Me.LabelCopyToStore.Text = "Copy to Store:"
        Me.LabelCopyToStore.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbCopyToStore
        '
        Me.cmbCopyToStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCopyToStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbCopyToStore.FormattingEnabled = True
        Me.cmbCopyToStore.Location = New System.Drawing.Point(172, 10)
        Me.cmbCopyToStore.Name = "cmbCopyToStore"
        Me.cmbCopyToStore.Size = New System.Drawing.Size(185, 22)
        Me.cmbCopyToStore.TabIndex = 5
        '
        'frmCopyPO_ExpectedDate
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(379, 118)
        Me.Controls.Add(Me.cmbCopyToStore)
        Me.Controls.Add(Me.LabelCopyToStore)
        Me.Controls.Add(Me.LabelExpectedDate)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.dtpExpectedDate)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(445, 206)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCopyPO_ExpectedDate"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "IRMA Copy PO"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dtpExpectedDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents LabelExpectedDate As System.Windows.Forms.Label
    Friend WithEvents LabelCopyToStore As System.Windows.Forms.Label
    Friend WithEvents cmbCopyToStore As System.Windows.Forms.ComboBox
#End Region
End Class