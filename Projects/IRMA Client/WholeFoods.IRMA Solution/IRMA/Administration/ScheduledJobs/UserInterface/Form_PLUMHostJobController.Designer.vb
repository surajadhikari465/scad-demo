<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_PLUMHostJobController
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_PLUMHostJobController))
        Me.Button_StartJob = New System.Windows.Forms.Button
        Me.Label_Instructions = New System.Windows.Forms.Label
        Me.Label_JobStatus = New System.Windows.Forms.Label
        Me.Label_ExceptionText = New System.Windows.Forms.Label
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox
        Me.CheckBox_IncludeExport = New System.Windows.Forms.CheckBox
        Me.Label_PlumDir = New System.Windows.Forms.Label
        Me.TextBox_PlumDir = New System.Windows.Forms.TextBox
        Me.FolderBrowserDialog_PlumInstall = New System.Windows.Forms.FolderBrowserDialog
        Me.Button_PlumInstallDir = New System.Windows.Forms.Button
        Me.Button_StartStoreFTP = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.RichTextBox2 = New System.Windows.Forms.RichTextBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_StartJob
        '
        Me.Button_StartJob.Location = New System.Drawing.Point(15, 128)
        Me.Button_StartJob.Name = "Button_StartJob"
        Me.Button_StartJob.Size = New System.Drawing.Size(159, 23)
        Me.Button_StartJob.TabIndex = 0
        Me.Button_StartJob.Text = "Start PLUM Import/Export"
        Me.Button_StartJob.UseVisualStyleBackColor = True
        '
        'Label_Instructions
        '
        Me.Label_Instructions.AutoSize = True
        Me.Label_Instructions.Location = New System.Drawing.Point(19, 9)
        Me.Label_Instructions.Name = "Label_Instructions"
        Me.Label_Instructions.Size = New System.Drawing.Size(0, 13)
        Me.Label_Instructions.TabIndex = 1
        '
        'Label_JobStatus
        '
        Me.Label_JobStatus.AutoSize = True
        Me.Label_JobStatus.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_JobStatus.Location = New System.Drawing.Point(22, 301)
        Me.Label_JobStatus.Name = "Label_JobStatus"
        Me.Label_JobStatus.Size = New System.Drawing.Size(412, 13)
        Me.Label_JobStatus.TabIndex = 2
        Me.Label_JobStatus.Text = "PLUM Import/Export status is unavailable.  Click ""Start PLUM Import/Export"" to be" & _
            "gin."
        '
        'Label_ExceptionText
        '
        Me.Label_ExceptionText.AutoSize = True
        Me.Label_ExceptionText.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ExceptionText.Location = New System.Drawing.Point(77, 328)
        Me.Label_ExceptionText.Name = "Label_ExceptionText"
        Me.Label_ExceptionText.Size = New System.Drawing.Size(106, 13)
        Me.Label_ExceptionText.TabIndex = 3
        Me.Label_ExceptionText.Text = "Exception stack trace"
        Me.Label_ExceptionText.Visible = False
        '
        'RichTextBox1
        '
        Me.RichTextBox1.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.RichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox1.Location = New System.Drawing.Point(15, 19)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.ReadOnly = True
        Me.RichTextBox1.Size = New System.Drawing.Size(537, 50)
        Me.RichTextBox1.TabIndex = 4
        Me.RichTextBox1.Text = resources.GetString("RichTextBox1.Text")
        '
        'CheckBox_IncludeExport
        '
        Me.CheckBox_IncludeExport.AutoSize = True
        Me.CheckBox_IncludeExport.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox_IncludeExport.Checked = True
        Me.CheckBox_IncludeExport.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox_IncludeExport.Location = New System.Drawing.Point(15, 75)
        Me.CheckBox_IncludeExport.Name = "CheckBox_IncludeExport"
        Me.CheckBox_IncludeExport.Size = New System.Drawing.Size(135, 17)
        Me.CheckBox_IncludeExport.TabIndex = 5
        Me.CheckBox_IncludeExport.Text = "Export to PLUM Store"
        Me.CheckBox_IncludeExport.UseVisualStyleBackColor = True
        '
        'Label_PlumDir
        '
        Me.Label_PlumDir.AutoSize = True
        Me.Label_PlumDir.Location = New System.Drawing.Point(12, 101)
        Me.Label_PlumDir.Name = "Label_PlumDir"
        Me.Label_PlumDir.Size = New System.Drawing.Size(119, 13)
        Me.Label_PlumDir.TabIndex = 6
        Me.Label_PlumDir.Text = "PLUM Install Directory"
        '
        'TextBox_PlumDir
        '
        Me.TextBox_PlumDir.Location = New System.Drawing.Point(130, 98)
        Me.TextBox_PlumDir.Name = "TextBox_PlumDir"
        Me.TextBox_PlumDir.ReadOnly = True
        Me.TextBox_PlumDir.Size = New System.Drawing.Size(295, 22)
        Me.TextBox_PlumDir.TabIndex = 7
        Me.TextBox_PlumDir.Text = "C:\Plum4"
        '
        'FolderBrowserDialog_PlumInstall
        '
        Me.FolderBrowserDialog_PlumInstall.Description = "Select the install directory for Plum Host.  The default directory is C:\Plum4."
        Me.FolderBrowserDialog_PlumInstall.RootFolder = System.Environment.SpecialFolder.MyComputer
        Me.FolderBrowserDialog_PlumInstall.ShowNewFolderButton = False
        Me.FolderBrowserDialog_PlumInstall.Tag = ""
        '
        'Button_PlumInstallDir
        '
        Me.Button_PlumInstallDir.Location = New System.Drawing.Point(431, 96)
        Me.Button_PlumInstallDir.Name = "Button_PlumInstallDir"
        Me.Button_PlumInstallDir.Size = New System.Drawing.Size(75, 23)
        Me.Button_PlumInstallDir.TabIndex = 8
        Me.Button_PlumInstallDir.Text = "Browse ..."
        Me.Button_PlumInstallDir.UseVisualStyleBackColor = True
        '
        'Button_StartStoreFTP
        '
        Me.Button_StartStoreFTP.Location = New System.Drawing.Point(15, 72)
        Me.Button_StartStoreFTP.Name = "Button_StartStoreFTP"
        Me.Button_StartStoreFTP.Size = New System.Drawing.Size(159, 23)
        Me.Button_StartStoreFTP.TabIndex = 9
        Me.Button_StartStoreFTP.Text = "FTP Export File to Stores"
        Me.Button_StartStoreFTP.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RichTextBox2)
        Me.GroupBox1.Controls.Add(Me.Button_StartStoreFTP)
        Me.GroupBox1.Location = New System.Drawing.Point(25, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(558, 113)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "FTP From PLUM Host to PLUM Store"
        '
        'RichTextBox2
        '
        Me.RichTextBox2.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.RichTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox2.Location = New System.Drawing.Point(15, 19)
        Me.RichTextBox2.Name = "RichTextBox2"
        Me.RichTextBox2.ReadOnly = True
        Me.RichTextBox2.Size = New System.Drawing.Size(537, 47)
        Me.RichTextBox2.TabIndex = 11
        Me.RichTextBox2.Text = "Click the ""FTP Existing PLUM Export to Stores"" button to FTP any pending PLUM Hos" & _
            "t export files to the stores.  " & Global.Microsoft.VisualBasic.ChrW(10) & "This option will not run the import or merge job" & _
            "s."
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.RichTextBox1)
        Me.GroupBox2.Controls.Add(Me.CheckBox_IncludeExport)
        Me.GroupBox2.Controls.Add(Me.Button_PlumInstallDir)
        Me.GroupBox2.Controls.Add(Me.Button_StartJob)
        Me.GroupBox2.Controls.Add(Me.TextBox_PlumDir)
        Me.GroupBox2.Controls.Add(Me.Label_PlumDir)
        Me.GroupBox2.Location = New System.Drawing.Point(25, 131)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(558, 157)
        Me.GroupBox2.TabIndex = 11
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Process PLUM Import and Export Jobs"
        '
        'Form_PLUMHostJobController
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(612, 392)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label_ExceptionText)
        Me.Controls.Add(Me.Label_JobStatus)
        Me.Controls.Add(Me.Label_Instructions)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(628, 428)
        Me.Name = "Form_PLUMHostJobController"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "PLUM Host Controller"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button_StartJob As System.Windows.Forms.Button
    Friend WithEvents Label_Instructions As System.Windows.Forms.Label
    Friend WithEvents Label_JobStatus As System.Windows.Forms.Label
    Friend WithEvents Label_ExceptionText As System.Windows.Forms.Label
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents CheckBox_IncludeExport As System.Windows.Forms.CheckBox
    Friend WithEvents Label_PlumDir As System.Windows.Forms.Label
    Friend WithEvents TextBox_PlumDir As System.Windows.Forms.TextBox
    Friend WithEvents FolderBrowserDialog_PlumInstall As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Button_PlumInstallDir As System.Windows.Forms.Button
    Friend WithEvents Button_StartStoreFTP As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents RichTextBox2 As System.Windows.Forms.RichTextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
End Class
