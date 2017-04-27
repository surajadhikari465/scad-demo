<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReceivingDocumentSetting
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
        Me.btnSetAllDocumentSetting = New Infragistics.Win.Misc.UltraButton()
        Me.btnSaveAndExit = New Infragistics.Win.Misc.UltraButton()
        Me.btnExitWithoutSaving = New Infragistics.Win.Misc.UltraButton()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Store_Number = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Store_Name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.IsReceivingDocument = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.deSelectAllStores = New Infragistics.Win.Misc.UltraButton()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnSetAllDocumentSetting
        '
        Me.btnSetAllDocumentSetting.AutoSize = True
        Me.btnSetAllDocumentSetting.Location = New System.Drawing.Point(454, 12)
        Me.btnSetAllDocumentSetting.Name = "btnSetAllDocumentSetting"
        Me.btnSetAllDocumentSetting.Size = New System.Drawing.Size(26, 24)
        Me.btnSetAllDocumentSetting.TabIndex = 87
        Me.btnSetAllDocumentSetting.Text = "&All"
        Me.btnSetAllDocumentSetting.WrapText = False
        '
        'btnSaveAndExit
        '
        Me.btnSaveAndExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveAndExit.Location = New System.Drawing.Point(454, 330)
        Me.btnSaveAndExit.Name = "btnSaveAndExit"
        Me.btnSaveAndExit.Size = New System.Drawing.Size(72, 23)
        Me.btnSaveAndExit.TabIndex = 91
        Me.btnSaveAndExit.Text = "&Save"
        '
        'btnExitWithoutSaving
        '
        Me.btnExitWithoutSaving.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExitWithoutSaving.Location = New System.Drawing.Point(12, 330)
        Me.btnExitWithoutSaving.Name = "btnExitWithoutSaving"
        Me.btnExitWithoutSaving.Size = New System.Drawing.Size(72, 23)
        Me.btnExitWithoutSaving.TabIndex = 92
        Me.btnExitWithoutSaving.Text = "E&xit"
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Store_Number, Me.Store_Name, Me.IsReceivingDocument})
        Me.DataGridView1.Location = New System.Drawing.Point(12, 42)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(514, 280)
        Me.DataGridView1.TabIndex = 93
        '
        'Store_Number
        '
        Me.Store_Number.DataPropertyName = "Store_Number"
        Me.Store_Number.HeaderText = "Store Number"
        Me.Store_Number.Name = "Store_Number"
        '
        'Store_Name
        '
        Me.Store_Name.DataPropertyName = "Store_Name"
        Me.Store_Name.HeaderText = "Store Name"
        Me.Store_Name.Name = "Store_Name"
        Me.Store_Name.Width = 140
        '
        'IsReceivingDocument
        '
        Me.IsReceivingDocument.DataPropertyName = "IsReceivingDocument"
        Me.IsReceivingDocument.FalseValue = "0"
        Me.IsReceivingDocument.HeaderText = "Receiving Document"
        Me.IsReceivingDocument.Name = "IsReceivingDocument"
        Me.IsReceivingDocument.TrueValue = "1"
        Me.IsReceivingDocument.Width = 120
        '
        'deSelectAllStores
        '
        Me.deSelectAllStores.AutoSize = True
        Me.deSelectAllStores.Location = New System.Drawing.Point(486, 12)
        Me.deSelectAllStores.Name = "deSelectAllStores"
        Me.deSelectAllStores.Size = New System.Drawing.Size(40, 24)
        Me.deSelectAllStores.TabIndex = 96
        Me.deSelectAllStores.Text = "&None"
        Me.deSelectAllStores.WrapText = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(408, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 14)
        Me.Label1.TabIndex = 98
        Me.Label1.Text = "Select:"
        '
        'frmReceivingDocumentSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(538, 365)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.deSelectAllStores)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.btnExitWithoutSaving)
        Me.Controls.Add(Me.btnSaveAndExit)
        Me.Controls.Add(Me.btnSetAllDocumentSetting)
        Me.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.Location = New System.Drawing.Point(445, 206)
        Me.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmReceivingDocumentSetting"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Receiving Document Vendor Setting"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnSetAllDocumentSetting As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnSaveAndExit As Infragistics.Win.Misc.UltraButton
    Friend WithEvents UltraDataSource1 As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Friend WithEvents btnExitWithoutSaving As Infragistics.Win.Misc.UltraButton
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents deSelectAllStores As Infragistics.Win.Misc.UltraButton
    Friend WithEvents Store_Number As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Store_Name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents IsReceivingDocument As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
