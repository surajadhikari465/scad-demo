<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_EditDataArchive
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_EditDataArchive))
        Me.Button_Save = New System.Windows.Forms.Button
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.GroupBox_ArchiveTableProperties = New System.Windows.Forms.GroupBox
        Me.Label_ProcessStatus = New System.Windows.Forms.Label
        Me.TextBox_ProcessStatus = New System.Windows.Forms.TextBox
        Me.TextBox_FilterSQL = New System.Windows.Forms.TextBox
        Me.Label_UpdatedBy = New System.Windows.Forms.Label
        Me.TextBox_UpdatedBy = New System.Windows.Forms.TextBox
        Me.Label_LastUpdate = New System.Windows.Forms.Label
        Me.TextBox_LastUpdate = New System.Windows.Forms.TextBox
        Me.ComboBox_JobFrequency = New System.Windows.Forms.ComboBox
        Me.Label_JobFrequency = New System.Windows.Forms.Label
        Me.ComboBox_ChangeType = New System.Windows.Forms.ComboBox
        Me.Label_RetentionDays = New System.Windows.Forms.Label
        Me.ComboBox_ArchiveTable = New System.Windows.Forms.ComboBox
        Me.Label_ChangeType = New System.Windows.Forms.Label
        Me.Label_TableName = New System.Windows.Forms.Label
        Me.TextBox_RetentionDays = New System.Windows.Forms.TextBox
        Me.Form_ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.Button_DisableArchive = New System.Windows.Forms.Button
        Me.CheckBox_ArchiveEnabled = New System.Windows.Forms.CheckBox
        Me.GroupBox_ArchiveTableProperties.SuspendLayout()
        CType(Me.Form_ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Save
        '
        Me.Button_Save.Enabled = False
        Me.Button_Save.Image = CType(resources.GetObject("Button_Save.Image"), System.Drawing.Image)
        Me.Button_Save.Location = New System.Drawing.Point(560, 595)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(87, 31)
        Me.Button_Save.TabIndex = 8
        Me.Button_Save.Text = "Save"
        Me.Button_Save.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(470, 595)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(82, 31)
        Me.Button_Cancel.TabIndex = 7
        Me.Button_Cancel.Text = "Close"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'GroupBox_ArchiveTableProperties
        '
        Me.GroupBox_ArchiveTableProperties.Controls.Add(Me.Label_ProcessStatus)
        Me.GroupBox_ArchiveTableProperties.Controls.Add(Me.TextBox_ProcessStatus)
        Me.GroupBox_ArchiveTableProperties.Controls.Add(Me.TextBox_FilterSQL)
        Me.GroupBox_ArchiveTableProperties.Controls.Add(Me.Label_UpdatedBy)
        Me.GroupBox_ArchiveTableProperties.Controls.Add(Me.TextBox_UpdatedBy)
        Me.GroupBox_ArchiveTableProperties.Controls.Add(Me.Label_LastUpdate)
        Me.GroupBox_ArchiveTableProperties.Controls.Add(Me.TextBox_LastUpdate)
        Me.GroupBox_ArchiveTableProperties.Controls.Add(Me.ComboBox_JobFrequency)
        Me.GroupBox_ArchiveTableProperties.Controls.Add(Me.Label_JobFrequency)
        Me.GroupBox_ArchiveTableProperties.Controls.Add(Me.ComboBox_ChangeType)
        Me.GroupBox_ArchiveTableProperties.Controls.Add(Me.Label_RetentionDays)
        Me.GroupBox_ArchiveTableProperties.Controls.Add(Me.ComboBox_ArchiveTable)
        Me.GroupBox_ArchiveTableProperties.Controls.Add(Me.Label_ChangeType)
        Me.GroupBox_ArchiveTableProperties.Controls.Add(Me.Label_TableName)
        Me.GroupBox_ArchiveTableProperties.Controls.Add(Me.TextBox_RetentionDays)
        Me.GroupBox_ArchiveTableProperties.Location = New System.Drawing.Point(32, 54)
        Me.GroupBox_ArchiveTableProperties.Name = "GroupBox_ArchiveTableProperties"
        Me.GroupBox_ArchiveTableProperties.Size = New System.Drawing.Size(615, 535)
        Me.GroupBox_ArchiveTableProperties.TabIndex = 6
        Me.GroupBox_ArchiveTableProperties.TabStop = False
        Me.GroupBox_ArchiveTableProperties.Text = "Archive Table Properties"
        '
        'Label_ProcessStatus
        '
        Me.Label_ProcessStatus.AutoSize = True
        Me.Label_ProcessStatus.Location = New System.Drawing.Point(358, 64)
        Me.Label_ProcessStatus.Name = "Label_ProcessStatus"
        Me.Label_ProcessStatus.Size = New System.Drawing.Size(65, 13)
        Me.Label_ProcessStatus.TabIndex = 30
        Me.Label_ProcessStatus.Text = "Last Status:"
        '
        'TextBox_ProcessStatus
        '
        Me.TextBox_ProcessStatus.Location = New System.Drawing.Point(429, 60)
        Me.TextBox_ProcessStatus.MaxLength = 50
        Me.TextBox_ProcessStatus.Name = "TextBox_ProcessStatus"
        Me.TextBox_ProcessStatus.Size = New System.Drawing.Size(133, 22)
        Me.TextBox_ProcessStatus.TabIndex = 29
        '
        'TextBox_FilterSQL
        '
        Me.TextBox_FilterSQL.Location = New System.Drawing.Point(12, 157)
        Me.TextBox_FilterSQL.MaxLength = 65535
        Me.TextBox_FilterSQL.Multiline = True
        Me.TextBox_FilterSQL.Name = "TextBox_FilterSQL"
        Me.TextBox_FilterSQL.Size = New System.Drawing.Size(584, 354)
        Me.TextBox_FilterSQL.TabIndex = 28
        '
        'Label_UpdatedBy
        '
        Me.Label_UpdatedBy.AutoSize = True
        Me.Label_UpdatedBy.Location = New System.Drawing.Point(360, 118)
        Me.Label_UpdatedBy.Name = "Label_UpdatedBy"
        Me.Label_UpdatedBy.Size = New System.Drawing.Size(63, 13)
        Me.Label_UpdatedBy.TabIndex = 27
        Me.Label_UpdatedBy.Text = "Update By:"
        '
        'TextBox_UpdatedBy
        '
        Me.TextBox_UpdatedBy.Location = New System.Drawing.Point(429, 115)
        Me.TextBox_UpdatedBy.MaxLength = 50
        Me.TextBox_UpdatedBy.Name = "TextBox_UpdatedBy"
        Me.TextBox_UpdatedBy.Size = New System.Drawing.Size(133, 22)
        Me.TextBox_UpdatedBy.TabIndex = 26
        '
        'Label_LastUpdate
        '
        Me.Label_LastUpdate.AutoSize = True
        Me.Label_LastUpdate.Location = New System.Drawing.Point(352, 91)
        Me.Label_LastUpdate.Name = "Label_LastUpdate"
        Me.Label_LastUpdate.Size = New System.Drawing.Size(71, 13)
        Me.Label_LastUpdate.TabIndex = 25
        Me.Label_LastUpdate.Text = "Last Update:"
        '
        'TextBox_LastUpdate
        '
        Me.TextBox_LastUpdate.Location = New System.Drawing.Point(429, 88)
        Me.TextBox_LastUpdate.MaxLength = 50
        Me.TextBox_LastUpdate.Name = "TextBox_LastUpdate"
        Me.TextBox_LastUpdate.Size = New System.Drawing.Size(133, 22)
        Me.TextBox_LastUpdate.TabIndex = 24
        '
        'ComboBox_JobFrequency
        '
        Me.ComboBox_JobFrequency.FormattingEnabled = True
        Me.ComboBox_JobFrequency.Location = New System.Drawing.Point(135, 88)
        Me.ComboBox_JobFrequency.Name = "ComboBox_JobFrequency"
        Me.ComboBox_JobFrequency.Size = New System.Drawing.Size(167, 21)
        Me.ComboBox_JobFrequency.TabIndex = 23
        '
        'Label_JobFrequency
        '
        Me.Label_JobFrequency.AutoSize = True
        Me.Label_JobFrequency.Location = New System.Drawing.Point(41, 91)
        Me.Label_JobFrequency.Name = "Label_JobFrequency"
        Me.Label_JobFrequency.Size = New System.Drawing.Size(84, 13)
        Me.Label_JobFrequency.TabIndex = 22
        Me.Label_JobFrequency.Text = "Job Frequency:"
        '
        'ComboBox_ChangeType
        '
        Me.ComboBox_ChangeType.FormattingEnabled = True
        Me.ComboBox_ChangeType.Location = New System.Drawing.Point(135, 61)
        Me.ComboBox_ChangeType.Name = "ComboBox_ChangeType"
        Me.ComboBox_ChangeType.Size = New System.Drawing.Size(167, 21)
        Me.ComboBox_ChangeType.TabIndex = 21
        '
        'Label_RetentionDays
        '
        Me.Label_RetentionDays.AutoSize = True
        Me.Label_RetentionDays.Location = New System.Drawing.Point(37, 119)
        Me.Label_RetentionDays.Name = "Label_RetentionDays"
        Me.Label_RetentionDays.Size = New System.Drawing.Size(88, 13)
        Me.Label_RetentionDays.TabIndex = 20
        Me.Label_RetentionDays.Text = "Retention Days:"
        '
        'ComboBox_ArchiveTable
        '
        Me.ComboBox_ArchiveTable.FormattingEnabled = True
        Me.ComboBox_ArchiveTable.Location = New System.Drawing.Point(135, 30)
        Me.ComboBox_ArchiveTable.Name = "ComboBox_ArchiveTable"
        Me.ComboBox_ArchiveTable.Size = New System.Drawing.Size(427, 21)
        Me.ComboBox_ArchiveTable.TabIndex = 5
        '
        'Label_ChangeType
        '
        Me.Label_ChangeType.AutoSize = True
        Me.Label_ChangeType.Location = New System.Drawing.Point(49, 64)
        Me.Label_ChangeType.Name = "Label_ChangeType"
        Me.Label_ChangeType.Size = New System.Drawing.Size(76, 13)
        Me.Label_ChangeType.TabIndex = 2
        Me.Label_ChangeType.Text = "Change Type:"
        '
        'Label_TableName
        '
        Me.Label_TableName.AutoSize = True
        Me.Label_TableName.Location = New System.Drawing.Point(56, 33)
        Me.Label_TableName.Name = "Label_TableName"
        Me.Label_TableName.Size = New System.Drawing.Size(69, 13)
        Me.Label_TableName.TabIndex = 1
        Me.Label_TableName.Text = "Table Name:"
        '
        'TextBox_RetentionDays
        '
        Me.TextBox_RetentionDays.Location = New System.Drawing.Point(135, 115)
        Me.TextBox_RetentionDays.MaxLength = 50
        Me.TextBox_RetentionDays.Name = "TextBox_RetentionDays"
        Me.TextBox_RetentionDays.Size = New System.Drawing.Size(60, 22)
        Me.TextBox_RetentionDays.TabIndex = 4
        '
        'Form_ErrorProvider
        '
        Me.Form_ErrorProvider.ContainerControl = Me
        '
        'Button_DisableArchive
        '
        Me.Button_DisableArchive.Image = CType(resources.GetObject("Button_DisableArchive.Image"), System.Drawing.Image)
        Me.Button_DisableArchive.Location = New System.Drawing.Point(532, 21)
        Me.Button_DisableArchive.Name = "Button_DisableArchive"
        Me.Button_DisableArchive.Size = New System.Drawing.Size(115, 27)
        Me.Button_DisableArchive.TabIndex = 11
        Me.Button_DisableArchive.Text = "Disable Archive"
        Me.Button_DisableArchive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_DisableArchive.UseVisualStyleBackColor = True
        '
        'CheckBox_ArchiveEnabled
        '
        Me.CheckBox_ArchiveEnabled.AutoSize = True
        Me.CheckBox_ArchiveEnabled.Enabled = False
        Me.CheckBox_ArchiveEnabled.Location = New System.Drawing.Point(32, 27)
        Me.CheckBox_ArchiveEnabled.Name = "CheckBox_ArchiveEnabled"
        Me.CheckBox_ArchiveEnabled.Size = New System.Drawing.Size(119, 17)
        Me.CheckBox_ArchiveEnabled.TabIndex = 0
        Me.CheckBox_ArchiveEnabled.TabStop = False
        Me.CheckBox_ArchiveEnabled.Text = "Archiving Enabled"
        Me.CheckBox_ArchiveEnabled.UseVisualStyleBackColor = True
        '
        'Form_EditDataArchive
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(681, 638)
        Me.Controls.Add(Me.Button_DisableArchive)
        Me.Controls.Add(Me.Button_Save)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.GroupBox_ArchiveTableProperties)
        Me.Controls.Add(Me.CheckBox_ArchiveEnabled)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form_EditDataArchive"
        Me.Text = "Edit Data Archive Record"
        Me.GroupBox_ArchiveTableProperties.ResumeLayout(False)
        Me.GroupBox_ArchiveTableProperties.PerformLayout()
        CType(Me.Form_ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button_Save As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents GroupBox_ArchiveTableProperties As System.Windows.Forms.GroupBox
    Friend WithEvents Label_ChangeType As System.Windows.Forms.Label
    Friend WithEvents Label_TableName As System.Windows.Forms.Label
    Friend WithEvents TextBox_RetentionDays As System.Windows.Forms.TextBox
    Friend WithEvents Label_RetentionDays As System.Windows.Forms.Label
    Friend WithEvents ComboBox_ArchiveTable As System.Windows.Forms.ComboBox
    Friend WithEvents Form_ErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents Button_DisableArchive As System.Windows.Forms.Button
    Friend WithEvents CheckBox_ArchiveEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBox_JobFrequency As System.Windows.Forms.ComboBox
    Friend WithEvents Label_JobFrequency As System.Windows.Forms.Label
    Friend WithEvents ComboBox_ChangeType As System.Windows.Forms.ComboBox
    Friend WithEvents Label_UpdatedBy As System.Windows.Forms.Label
    Friend WithEvents TextBox_UpdatedBy As System.Windows.Forms.TextBox
    Friend WithEvents Label_LastUpdate As System.Windows.Forms.Label
    Friend WithEvents TextBox_LastUpdate As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_FilterSQL As System.Windows.Forms.TextBox
    Friend WithEvents Label_ProcessStatus As System.Windows.Forms.Label
    Friend WithEvents TextBox_ProcessStatus As System.Windows.Forms.TextBox

End Class
