<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageStores
    Inherits Form_IRMABase

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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.GroupBox_Stores = New System.Windows.Forms.GroupBox
        Me.Button_Delete = New System.Windows.Forms.Button
        Me.Button_Edit = New System.Windows.Forms.Button
        Me.Button_Add = New System.Windows.Forms.Button
        Me.DataGridView_ConfigItems = New System.Windows.Forms.DataGridView
        Me.Button_Close = New System.Windows.Forms.Button
        Me.GroupBox_RegionalSettings = New System.Windows.Forms.GroupBox
        Me.Label_RegionalStoreError = New System.Windows.Forms.Label
        Me.Button_FTPInfo = New System.Windows.Forms.Button
        Me.Label_ZoneWriterVal = New System.Windows.Forms.Label
        Me.Label_CorpWriterVal = New System.Windows.Forms.Label
        Me.Button_EditRegionalSettings = New System.Windows.Forms.Button
        Me.Label_ZoneWriter = New System.Windows.Forms.Label
        Me.Label_CorpWriter = New System.Windows.Forms.Label
        Me.CheckBox_RegionalScale = New System.Windows.Forms.CheckBox
        Me.GroupBox_Stores.SuspendLayout()
        CType(Me.DataGridView_ConfigItems, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_RegionalSettings.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_Stores
        '
        Me.GroupBox_Stores.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox_Stores.Controls.Add(Me.Button_Delete)
        Me.GroupBox_Stores.Controls.Add(Me.Button_Edit)
        Me.GroupBox_Stores.Controls.Add(Me.Button_Add)
        Me.GroupBox_Stores.Controls.Add(Me.DataGridView_ConfigItems)
        Me.GroupBox_Stores.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_Stores.Name = "GroupBox_Stores"
        Me.GroupBox_Stores.Size = New System.Drawing.Size(629, 305)
        Me.GroupBox_Stores.TabIndex = 14
        Me.GroupBox_Stores.TabStop = False
        Me.GroupBox_Stores.Text = "Stores"
        '
        'Button_Delete
        '
        Me.Button_Delete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Delete.Enabled = False
        Me.Button_Delete.Location = New System.Drawing.Point(538, 77)
        Me.Button_Delete.Name = "Button_Delete"
        Me.Button_Delete.Size = New System.Drawing.Size(75, 23)
        Me.Button_Delete.TabIndex = 12
        Me.Button_Delete.Text = "Delete"
        Me.Button_Delete.UseVisualStyleBackColor = True
        '
        'Button_Edit
        '
        Me.Button_Edit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Edit.Location = New System.Drawing.Point(538, 48)
        Me.Button_Edit.Name = "Button_Edit"
        Me.Button_Edit.Size = New System.Drawing.Size(75, 23)
        Me.Button_Edit.TabIndex = 11
        Me.Button_Edit.Text = "Edit"
        Me.Button_Edit.UseVisualStyleBackColor = True
        '
        'Button_Add
        '
        Me.Button_Add.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Add.Enabled = False
        Me.Button_Add.Location = New System.Drawing.Point(538, 19)
        Me.Button_Add.Name = "Button_Add"
        Me.Button_Add.Size = New System.Drawing.Size(75, 23)
        Me.Button_Add.TabIndex = 10
        Me.Button_Add.Text = "Add"
        Me.Button_Add.UseVisualStyleBackColor = True
        '
        'DataGridView_ConfigItems
        '
        Me.DataGridView_ConfigItems.AllowUserToAddRows = False
        Me.DataGridView_ConfigItems.AllowUserToDeleteRows = False
        Me.DataGridView_ConfigItems.AllowUserToOrderColumns = True
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.DataGridView_ConfigItems.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView_ConfigItems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView_ConfigItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView_ConfigItems.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView_ConfigItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView_ConfigItems.Location = New System.Drawing.Point(18, 19)
        Me.DataGridView_ConfigItems.MultiSelect = False
        Me.DataGridView_ConfigItems.Name = "DataGridView_ConfigItems"
        Me.DataGridView_ConfigItems.ReadOnly = True
        Me.DataGridView_ConfigItems.Size = New System.Drawing.Size(514, 259)
        Me.DataGridView_ConfigItems.TabIndex = 6
        '
        'Button_Close
        '
        Me.Button_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Close.Location = New System.Drawing.Point(550, 459)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(75, 23)
        Me.Button_Close.TabIndex = 15
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'GroupBox_RegionalSettings
        '
        Me.GroupBox_RegionalSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GroupBox_RegionalSettings.Controls.Add(Me.Label_RegionalStoreError)
        Me.GroupBox_RegionalSettings.Controls.Add(Me.Button_FTPInfo)
        Me.GroupBox_RegionalSettings.Controls.Add(Me.Label_ZoneWriterVal)
        Me.GroupBox_RegionalSettings.Controls.Add(Me.Label_CorpWriterVal)
        Me.GroupBox_RegionalSettings.Controls.Add(Me.Button_EditRegionalSettings)
        Me.GroupBox_RegionalSettings.Controls.Add(Me.Label_ZoneWriter)
        Me.GroupBox_RegionalSettings.Controls.Add(Me.Label_CorpWriter)
        Me.GroupBox_RegionalSettings.Controls.Add(Me.CheckBox_RegionalScale)
        Me.GroupBox_RegionalSettings.Location = New System.Drawing.Point(12, 323)
        Me.GroupBox_RegionalSettings.Name = "GroupBox_RegionalSettings"
        Me.GroupBox_RegionalSettings.Size = New System.Drawing.Size(532, 127)
        Me.GroupBox_RegionalSettings.TabIndex = 16
        Me.GroupBox_RegionalSettings.TabStop = False
        Me.GroupBox_RegionalSettings.Text = "Regional Settings"
        '
        'Label_RegionalStoreError
        '
        Me.Label_RegionalStoreError.AutoSize = True
        Me.Label_RegionalStoreError.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_RegionalStoreError.ForeColor = System.Drawing.Color.DarkRed
        Me.Label_RegionalStoreError.Location = New System.Drawing.Point(22, 101)
        Me.Label_RegionalStoreError.Name = "Label_RegionalStoreError"
        Me.Label_RegionalStoreError.Size = New System.Drawing.Size(488, 13)
        Me.Label_RegionalStoreError.TabIndex = 16
        Me.Label_RegionalStoreError.Text = "WARNING: No regional entry in the STORE table.  Unable to set 'Regional Settings'" & _
            "."
        '
        'Button_FTPInfo
        '
        Me.Button_FTPInfo.Location = New System.Drawing.Point(363, 50)
        Me.Button_FTPInfo.Name = "Button_FTPInfo"
        Me.Button_FTPInfo.Size = New System.Drawing.Size(153, 23)
        Me.Button_FTPInfo.TabIndex = 15
        Me.Button_FTPInfo.Text = "Edit Regional FTP Info"
        Me.Button_FTPInfo.UseVisualStyleBackColor = True
        '
        'Label_ZoneWriterVal
        '
        Me.Label_ZoneWriterVal.AutoSize = True
        Me.Label_ZoneWriterVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ZoneWriterVal.Location = New System.Drawing.Point(145, 77)
        Me.Label_ZoneWriterVal.Name = "Label_ZoneWriterVal"
        Me.Label_ZoneWriterVal.Size = New System.Drawing.Size(45, 13)
        Me.Label_ZoneWriterVal.TabIndex = 14
        Me.Label_ZoneWriterVal.Text = "Label2"
        '
        'Label_CorpWriterVal
        '
        Me.Label_CorpWriterVal.AutoSize = True
        Me.Label_CorpWriterVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_CorpWriterVal.Location = New System.Drawing.Point(145, 50)
        Me.Label_CorpWriterVal.Name = "Label_CorpWriterVal"
        Me.Label_CorpWriterVal.Size = New System.Drawing.Size(45, 13)
        Me.Label_CorpWriterVal.TabIndex = 13
        Me.Label_CorpWriterVal.Text = "Label1"
        '
        'Button_EditRegionalSettings
        '
        Me.Button_EditRegionalSettings.Location = New System.Drawing.Point(363, 21)
        Me.Button_EditRegionalSettings.Name = "Button_EditRegionalSettings"
        Me.Button_EditRegionalSettings.Size = New System.Drawing.Size(153, 23)
        Me.Button_EditRegionalSettings.TabIndex = 12
        Me.Button_EditRegionalSettings.Text = "Edit Regional Scale Settings"
        Me.Button_EditRegionalSettings.UseVisualStyleBackColor = True
        '
        'Label_ZoneWriter
        '
        Me.Label_ZoneWriter.AutoSize = True
        Me.Label_ZoneWriter.Location = New System.Drawing.Point(22, 77)
        Me.Label_ZoneWriter.Name = "Label_ZoneWriter"
        Me.Label_ZoneWriter.Size = New System.Drawing.Size(100, 13)
        Me.Label_ZoneWriter.TabIndex = 4
        Me.Label_ZoneWriter.Text = "Zone Scale Writer:"
        '
        'Label_CorpWriter
        '
        Me.Label_CorpWriter.AutoSize = True
        Me.Label_CorpWriter.Location = New System.Drawing.Point(22, 50)
        Me.Label_CorpWriter.Name = "Label_CorpWriter"
        Me.Label_CorpWriter.Size = New System.Drawing.Size(126, 13)
        Me.Label_CorpWriter.TabIndex = 3
        Me.Label_CorpWriter.Text = "Corporate Scale Writer:"
        '
        'CheckBox_RegionalScale
        '
        Me.CheckBox_RegionalScale.AutoSize = True
        Me.CheckBox_RegionalScale.Enabled = False
        Me.CheckBox_RegionalScale.Location = New System.Drawing.Point(25, 24)
        Me.CheckBox_RegionalScale.Name = "CheckBox_RegionalScale"
        Me.CheckBox_RegionalScale.Size = New System.Drawing.Size(193, 17)
        Me.CheckBox_RegionalScale.TabIndex = 0
        Me.CheckBox_RegionalScale.Text = "Use Regional Scale Hosting Files"
        Me.CheckBox_RegionalScale.UseVisualStyleBackColor = True
        '
        'Form_ManageStores
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(655, 494)
        Me.Controls.Add(Me.GroupBox_RegionalSettings)
        Me.Controls.Add(Me.Button_Close)
        Me.Controls.Add(Me.GroupBox_Stores)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(671, 530)
        Me.Name = "Form_ManageStores"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Manage Stores"
        Me.GroupBox_Stores.ResumeLayout(False)
        CType(Me.DataGridView_ConfigItems, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_RegionalSettings.ResumeLayout(False)
        Me.GroupBox_RegionalSettings.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox_Stores As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Delete As System.Windows.Forms.Button
    Friend WithEvents Button_Edit As System.Windows.Forms.Button
    Friend WithEvents Button_Add As System.Windows.Forms.Button
    Public WithEvents DataGridView_ConfigItems As System.Windows.Forms.DataGridView
    Friend WithEvents Button_Close As System.Windows.Forms.Button
    Friend WithEvents GroupBox_RegionalSettings As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox_RegionalScale As System.Windows.Forms.CheckBox
    Friend WithEvents Label_ZoneWriter As System.Windows.Forms.Label
    Friend WithEvents Label_CorpWriter As System.Windows.Forms.Label
    Friend WithEvents Button_EditRegionalSettings As System.Windows.Forms.Button
    Friend WithEvents Label_ZoneWriterVal As System.Windows.Forms.Label
    Friend WithEvents Label_CorpWriterVal As System.Windows.Forms.Label
    Friend WithEvents Button_FTPInfo As System.Windows.Forms.Button
    Friend WithEvents Label_RegionalStoreError As System.Windows.Forms.Label
End Class
