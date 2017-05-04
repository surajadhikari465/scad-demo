<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_AuditReportJobController
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
        Me.Label_ExceptionText = New System.Windows.Forms.Label
        Me.Label_JobStatus = New System.Windows.Forms.Label
        Me.Label_Instructions = New System.Windows.Forms.Label
        Me.Button_StartJob = New System.Windows.Forms.Button
        Me.ComboBox_StoreList = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.chkDeleteFiles = New System.Windows.Forms.CheckBox
        Me.TextBox_SSHStatus = New System.Windows.Forms.RichTextBox
        Me.CheckBox_POSSuccess = New System.Windows.Forms.CheckBox
        Me.CheckBox_POSRemoteJobs = New System.Windows.Forms.CheckBox
        Me.TextBox_POSFTPStatus = New System.Windows.Forms.RichTextBox
        Me.CheckBox_POSFileTransfer = New System.Windows.Forms.CheckBox
        Me.CheckBox_POSControlFiles = New System.Windows.Forms.CheckBox
        Me.Label_POSStoreCount = New System.Windows.Forms.Label
        Me.CheckBox_POSReadStore = New System.Windows.Forms.CheckBox
        Me.CheckBox_POSItemIdAdd = New System.Windows.Forms.CheckBox
        Me.CheckBox_POSVendorAdd = New System.Windows.Forms.CheckBox
        Me.CheckBox_POSStarted = New System.Windows.Forms.CheckBox
        Me.Text_SSHlabel = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Label_ExceptionText
        '
        Me.Label_ExceptionText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label_ExceptionText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label_ExceptionText.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ExceptionText.Location = New System.Drawing.Point(12, 415)
        Me.Label_ExceptionText.Name = "Label_ExceptionText"
        Me.Label_ExceptionText.Size = New System.Drawing.Size(363, 40)
        Me.Label_ExceptionText.TabIndex = 7
        Me.Label_ExceptionText.Text = "Exception stack trace"
        Me.Label_ExceptionText.Visible = False
        '
        'Label_JobStatus
        '
        Me.Label_JobStatus.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_JobStatus.Location = New System.Drawing.Point(9, 118)
        Me.Label_JobStatus.Name = "Label_JobStatus"
        Me.Label_JobStatus.Size = New System.Drawing.Size(364, 16)
        Me.Label_JobStatus.TabIndex = 6
        Me.Label_JobStatus.Text = "Current status is unavailable.  Click ""Build Store POS File"" to begin."
        '
        'Label_Instructions
        '
        Me.Label_Instructions.AutoSize = True
        Me.Label_Instructions.Location = New System.Drawing.Point(9, 57)
        Me.Label_Instructions.Name = "Label_Instructions"
        Me.Label_Instructions.Size = New System.Drawing.Size(314, 13)
        Me.Label_Instructions.TabIndex = 5
        Me.Label_Instructions.Text = "Click the button below to start the Build Store POS File Job."
        '
        'Button_StartJob
        '
        Me.Button_StartJob.Location = New System.Drawing.Point(12, 77)
        Me.Button_StartJob.Name = "Button_StartJob"
        Me.Button_StartJob.Size = New System.Drawing.Size(152, 23)
        Me.Button_StartJob.TabIndex = 4
        Me.Button_StartJob.Text = "Build Store POS File"
        Me.Button_StartJob.UseVisualStyleBackColor = True
        '
        'ComboBox_StoreList
        '
        Me.ComboBox_StoreList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_StoreList.FormattingEnabled = True
        Me.ComboBox_StoreList.Location = New System.Drawing.Point(12, 25)
        Me.ComboBox_StoreList.Name = "ComboBox_StoreList"
        Me.ComboBox_StoreList.Size = New System.Drawing.Size(227, 21)
        Me.ComboBox_StoreList.TabIndex = 8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(252, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Select the store you wish to build a POS file for:"
        '
        'chkDeleteFiles
        '
        Me.chkDeleteFiles.AutoSize = True
        Me.chkDeleteFiles.Location = New System.Drawing.Point(188, 81)
        Me.chkDeleteFiles.Name = "chkDeleteFiles"
        Me.chkDeleteFiles.Size = New System.Drawing.Size(133, 17)
        Me.chkDeleteFiles.TabIndex = 10
        Me.chkDeleteFiles.Tag = "Upon failure, allows generated files to be kept until the next run"
        Me.chkDeleteFiles.Text = "Purge files on failure"
        Me.chkDeleteFiles.UseVisualStyleBackColor = True
        '
        'TextBox_SSHStatus
        '
        Me.TextBox_SSHStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_SSHStatus.Location = New System.Drawing.Point(218, 338)
        Me.TextBox_SSHStatus.Name = "TextBox_SSHStatus"
        Me.TextBox_SSHStatus.ReadOnly = True
        Me.TextBox_SSHStatus.Size = New System.Drawing.Size(157, 47)
        Me.TextBox_SSHStatus.TabIndex = 51
        Me.TextBox_SSHStatus.Text = ""
        '
        'CheckBox_POSSuccess
        '
        Me.CheckBox_POSSuccess.AutoSize = True
        Me.CheckBox_POSSuccess.Enabled = False
        Me.CheckBox_POSSuccess.Location = New System.Drawing.Point(33, 391)
        Me.CheckBox_POSSuccess.Name = "CheckBox_POSSuccess"
        Me.CheckBox_POSSuccess.Size = New System.Drawing.Size(198, 17)
        Me.CheckBox_POSSuccess.TabIndex = 50
        Me.CheckBox_POSSuccess.Text = "POS Push Completed Successfully"
        Me.CheckBox_POSSuccess.UseVisualStyleBackColor = True
        '
        'CheckBox_POSRemoteJobs
        '
        Me.CheckBox_POSRemoteJobs.AutoSize = True
        Me.CheckBox_POSRemoteJobs.Enabled = False
        Me.CheckBox_POSRemoteJobs.Location = New System.Drawing.Point(33, 315)
        Me.CheckBox_POSRemoteJobs.Name = "CheckBox_POSRemoteJobs"
        Me.CheckBox_POSRemoteJobs.Size = New System.Drawing.Size(241, 17)
        Me.CheckBox_POSRemoteJobs.TabIndex = 49
        Me.CheckBox_POSRemoteJobs.Text = "Remote Jobs Started on Store POS Servers"
        Me.CheckBox_POSRemoteJobs.UseVisualStyleBackColor = True
        '
        'TextBox_POSFTPStatus
        '
        Me.TextBox_POSFTPStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_POSFTPStatus.Location = New System.Drawing.Point(163, 261)
        Me.TextBox_POSFTPStatus.Name = "TextBox_POSFTPStatus"
        Me.TextBox_POSFTPStatus.ReadOnly = True
        Me.TextBox_POSFTPStatus.Size = New System.Drawing.Size(212, 48)
        Me.TextBox_POSFTPStatus.TabIndex = 47
        Me.TextBox_POSFTPStatus.Text = ""
        '
        'CheckBox_POSFileTransfer
        '
        Me.CheckBox_POSFileTransfer.AutoSize = True
        Me.CheckBox_POSFileTransfer.Enabled = False
        Me.CheckBox_POSFileTransfer.Location = New System.Drawing.Point(33, 263)
        Me.CheckBox_POSFileTransfer.Name = "CheckBox_POSFileTransfer"
        Me.CheckBox_POSFileTransfer.Size = New System.Drawing.Size(126, 17)
        Me.CheckBox_POSFileTransfer.TabIndex = 46
        Me.CheckBox_POSFileTransfer.Text = "POS FTP Completed"
        Me.CheckBox_POSFileTransfer.UseVisualStyleBackColor = True
        '
        'CheckBox_POSControlFiles
        '
        Me.CheckBox_POSControlFiles.AutoSize = True
        Me.CheckBox_POSControlFiles.Enabled = False
        Me.CheckBox_POSControlFiles.Location = New System.Drawing.Point(33, 239)
        Me.CheckBox_POSControlFiles.Name = "CheckBox_POSControlFiles"
        Me.CheckBox_POSControlFiles.Size = New System.Drawing.Size(178, 17)
        Me.CheckBox_POSControlFiles.TabIndex = 45
        Me.CheckBox_POSControlFiles.Text = "Generated POS Control File(s)"
        Me.CheckBox_POSControlFiles.UseVisualStyleBackColor = True
        '
        'Label_POSStoreCount
        '
        Me.Label_POSStoreCount.AutoSize = True
        Me.Label_POSStoreCount.Enabled = False
        Me.Label_POSStoreCount.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_POSStoreCount.Location = New System.Drawing.Point(267, 171)
        Me.Label_POSStoreCount.Name = "Label_POSStoreCount"
        Me.Label_POSStoreCount.Size = New System.Drawing.Size(82, 13)
        Me.Label_POSStoreCount.TabIndex = 44
        Me.Label_POSStoreCount.Text = "# Stores Read?  "
        '
        'CheckBox_POSReadStore
        '
        Me.CheckBox_POSReadStore.AutoSize = True
        Me.CheckBox_POSReadStore.Enabled = False
        Me.CheckBox_POSReadStore.Location = New System.Drawing.Point(33, 170)
        Me.CheckBox_POSReadStore.Name = "CheckBox_POSReadStore"
        Me.CheckBox_POSReadStore.Size = New System.Drawing.Size(209, 17)
        Me.CheckBox_POSReadStore.TabIndex = 43
        Me.CheckBox_POSReadStore.Text = "Read POS Store Configuration Data"
        Me.CheckBox_POSReadStore.UseVisualStyleBackColor = True
        '
        'CheckBox_POSItemIdAdd
        '
        Me.CheckBox_POSItemIdAdd.AutoSize = True
        Me.CheckBox_POSItemIdAdd.Enabled = False
        Me.CheckBox_POSItemIdAdd.Location = New System.Drawing.Point(33, 216)
        Me.CheckBox_POSItemIdAdd.Name = "CheckBox_POSItemIdAdd"
        Me.CheckBox_POSItemIdAdd.Size = New System.Drawing.Size(142, 17)
        Me.CheckBox_POSItemIdAdd.TabIndex = 41
        Me.CheckBox_POSItemIdAdd.Text = "Read Item ID Add Data"
        Me.CheckBox_POSItemIdAdd.UseVisualStyleBackColor = True
        '
        'CheckBox_POSVendorAdd
        '
        Me.CheckBox_POSVendorAdd.AutoSize = True
        Me.CheckBox_POSVendorAdd.Enabled = False
        Me.CheckBox_POSVendorAdd.Location = New System.Drawing.Point(33, 193)
        Me.CheckBox_POSVendorAdd.Name = "CheckBox_POSVendorAdd"
        Me.CheckBox_POSVendorAdd.Size = New System.Drawing.Size(163, 17)
        Me.CheckBox_POSVendorAdd.TabIndex = 37
        Me.CheckBox_POSVendorAdd.Text = "Read Vendor ID Adds Data"
        Me.CheckBox_POSVendorAdd.UseVisualStyleBackColor = True
        '
        'CheckBox_POSStarted
        '
        Me.CheckBox_POSStarted.AutoSize = True
        Me.CheckBox_POSStarted.Enabled = False
        Me.CheckBox_POSStarted.Location = New System.Drawing.Point(12, 147)
        Me.CheckBox_POSStarted.Name = "CheckBox_POSStarted"
        Me.CheckBox_POSStarted.Size = New System.Drawing.Size(115, 17)
        Me.CheckBox_POSStarted.TabIndex = 36
        Me.CheckBox_POSStarted.Text = "POS Push Started"
        Me.CheckBox_POSStarted.UseVisualStyleBackColor = True
        '
        'Text_SSHlabel
        '
        Me.Text_SSHlabel.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text_SSHlabel.Enabled = False
        Me.Text_SSHlabel.Location = New System.Drawing.Point(51, 342)
        Me.Text_SSHlabel.Name = "Text_SSHlabel"
        Me.Text_SSHlabel.ReadOnly = True
        Me.Text_SSHlabel.Size = New System.Drawing.Size(164, 15)
        Me.Text_SSHlabel.TabIndex = 54
        Me.Text_SSHlabel.TabStop = False
        Me.Text_SSHlabel.Text = "SSH Remote Execution Status -->"
        Me.Text_SSHlabel.WordWrap = False
        '
        'Form_AuditReportJobController
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 464)
        Me.Controls.Add(Me.Text_SSHlabel)
        Me.Controls.Add(Me.TextBox_SSHStatus)
        Me.Controls.Add(Me.CheckBox_POSSuccess)
        Me.Controls.Add(Me.CheckBox_POSRemoteJobs)
        Me.Controls.Add(Me.TextBox_POSFTPStatus)
        Me.Controls.Add(Me.CheckBox_POSFileTransfer)
        Me.Controls.Add(Me.CheckBox_POSControlFiles)
        Me.Controls.Add(Me.Label_POSStoreCount)
        Me.Controls.Add(Me.CheckBox_POSReadStore)
        Me.Controls.Add(Me.CheckBox_POSItemIdAdd)
        Me.Controls.Add(Me.CheckBox_POSVendorAdd)
        Me.Controls.Add(Me.CheckBox_POSStarted)
        Me.Controls.Add(Me.chkDeleteFiles)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboBox_StoreList)
        Me.Controls.Add(Me.Label_ExceptionText)
        Me.Controls.Add(Me.Label_JobStatus)
        Me.Controls.Add(Me.Label_Instructions)
        Me.Controls.Add(Me.Button_StartJob)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(400, 500)
        Me.Name = "Form_AuditReportJobController"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Build Store File Job Controller"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label_ExceptionText As System.Windows.Forms.Label
    Friend WithEvents Label_JobStatus As System.Windows.Forms.Label
    Friend WithEvents Label_Instructions As System.Windows.Forms.Label
    Friend WithEvents Button_StartJob As System.Windows.Forms.Button
    Friend WithEvents ComboBox_StoreList As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkDeleteFiles As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox_SSHStatus As System.Windows.Forms.RichTextBox
    Friend WithEvents CheckBox_POSSuccess As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_POSRemoteJobs As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox_POSFTPStatus As System.Windows.Forms.RichTextBox
    Friend WithEvents CheckBox_POSFileTransfer As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_POSControlFiles As System.Windows.Forms.CheckBox
    Friend WithEvents Label_POSStoreCount As System.Windows.Forms.Label
    Friend WithEvents CheckBox_POSReadStore As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_POSItemIdAdd As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_POSVendorAdd As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_POSStarted As System.Windows.Forms.CheckBox
    Friend WithEvents Text_SSHlabel As System.Windows.Forms.TextBox
End Class