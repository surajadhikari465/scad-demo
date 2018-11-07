<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_BuildFullESTFileForStore
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
        Me.Label_ExceptionText = New System.Windows.Forms.Label()
        Me.Label_Instructions = New System.Windows.Forms.Label()
        Me.Button_StartJob = New System.Windows.Forms.Button()
        Me.ComboBox_StoreList = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CheckBox_ESTSuccess = New System.Windows.Forms.CheckBox()
        Me.TextBox_ESTFTPStatus = New System.Windows.Forms.RichTextBox()
        Me.Label_ESTStoreCount = New System.Windows.Forms.Label()
        Me.CheckBox_ESTStores = New System.Windows.Forms.CheckBox()
        Me.CheckBox_FullESTData = New System.Windows.Forms.CheckBox()
        Me.CheckBox_ESTFTP = New System.Windows.Forms.CheckBox()
        Me.CheckBox_ESTStarted = New System.Windows.Forms.CheckBox()
        Me.Label_JobStatus = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label_ExceptionText
        '
        Me.Label_ExceptionText.AutoSize = True
        Me.Label_ExceptionText.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ExceptionText.Location = New System.Drawing.Point(19, 283)
        Me.Label_ExceptionText.Name = "Label_ExceptionText"
        Me.Label_ExceptionText.Size = New System.Drawing.Size(106, 13)
        Me.Label_ExceptionText.TabIndex = 7
        Me.Label_ExceptionText.Text = "Exception stack trace"
        Me.Label_ExceptionText.Visible = False
        '
        'Label_Instructions
        '
        Me.Label_Instructions.AutoSize = True
        Me.Label_Instructions.Location = New System.Drawing.Point(20, 61)
        Me.Label_Instructions.Name = "Label_Instructions"
        Me.Label_Instructions.Size = New System.Drawing.Size(393, 13)
        Me.Label_Instructions.TabIndex = 5
        Me.Label_Instructions.Text = "Click the button below to start the Build Store Electronic Shelf Tag File Job."
        '
        'Button_StartJob
        '
        Me.Button_StartJob.Location = New System.Drawing.Point(23, 77)
        Me.Button_StartJob.Name = "Button_StartJob"
        Me.Button_StartJob.Size = New System.Drawing.Size(205, 29)
        Me.Button_StartJob.TabIndex = 4
        Me.Button_StartJob.Text = "Build Store Electronic Shelf Tag File"
        Me.Button_StartJob.UseVisualStyleBackColor = True
        '
        'ComboBox_StoreList
        '
        Me.ComboBox_StoreList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_StoreList.FormattingEnabled = True
        Me.ComboBox_StoreList.Location = New System.Drawing.Point(23, 25)
        Me.ComboBox_StoreList.Name = "ComboBox_StoreList"
        Me.ComboBox_StoreList.Size = New System.Drawing.Size(227, 21)
        Me.ComboBox_StoreList.TabIndex = 8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(349, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Select the store you wish to build a full electronic shelf tag file for:"
        '
        'CheckBox_ESTSuccess
        '
        Me.CheckBox_ESTSuccess.AutoSize = True
        Me.CheckBox_ESTSuccess.Enabled = False
        Me.CheckBox_ESTSuccess.Location = New System.Drawing.Point(72, 233)
        Me.CheckBox_ESTSuccess.Name = "CheckBox_ESTSuccess"
        Me.CheckBox_ESTSuccess.Size = New System.Drawing.Size(292, 17)
        Me.CheckBox_ESTSuccess.TabIndex = 25
        Me.CheckBox_ESTSuccess.Text = "Full Electronic Shelf Tag File Completed Successfully"
        Me.CheckBox_ESTSuccess.UseVisualStyleBackColor = True
        '
        'TextBox_ESTFTPStatus
        '
        Me.TextBox_ESTFTPStatus.Location = New System.Drawing.Point(207, 183)
        Me.TextBox_ESTFTPStatus.Name = "TextBox_ESTFTPStatus"
        Me.TextBox_ESTFTPStatus.ReadOnly = True
        Me.TextBox_ESTFTPStatus.Size = New System.Drawing.Size(368, 44)
        Me.TextBox_ESTFTPStatus.TabIndex = 24
        Me.TextBox_ESTFTPStatus.Text = ""
        '
        'Label_ESTStoreCount
        '
        Me.Label_ESTStoreCount.AutoSize = True
        Me.Label_ESTStoreCount.Enabled = False
        Me.Label_ESTStoreCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ESTStoreCount.Location = New System.Drawing.Point(353, 140)
        Me.Label_ESTStoreCount.Name = "Label_ESTStoreCount"
        Me.Label_ESTStoreCount.Size = New System.Drawing.Size(88, 13)
        Me.Label_ESTStoreCount.TabIndex = 23
        Me.Label_ESTStoreCount.Text = "# Stores Read?  "
        '
        'CheckBox_ESTStores
        '
        Me.CheckBox_ESTStores.AutoSize = True
        Me.CheckBox_ESTStores.Enabled = False
        Me.CheckBox_ESTStores.Location = New System.Drawing.Point(72, 139)
        Me.CheckBox_ESTStores.Name = "CheckBox_ESTStores"
        Me.CheckBox_ESTStores.Size = New System.Drawing.Size(269, 17)
        Me.CheckBox_ESTStores.TabIndex = 22
        Me.CheckBox_ESTStores.Text = "Finished Reading Store EST Configuration Data"
        Me.CheckBox_ESTStores.UseVisualStyleBackColor = True
        '
        'CheckBox_FullESTData
        '
        Me.CheckBox_FullESTData.AutoSize = True
        Me.CheckBox_FullESTData.Enabled = False
        Me.CheckBox_FullESTData.Location = New System.Drawing.Point(72, 162)
        Me.CheckBox_FullESTData.Name = "CheckBox_FullESTData"
        Me.CheckBox_FullESTData.Size = New System.Drawing.Size(331, 17)
        Me.CheckBox_FullESTData.TabIndex = 21
        Me.CheckBox_FullESTData.Text = "Finished Reading List of Electronic Shelf Tag Items for Store"
        Me.CheckBox_FullESTData.UseVisualStyleBackColor = True
        '
        'CheckBox_ESTFTP
        '
        Me.CheckBox_ESTFTP.AutoSize = True
        Me.CheckBox_ESTFTP.Enabled = False
        Me.CheckBox_ESTFTP.Location = New System.Drawing.Point(72, 185)
        Me.CheckBox_ESTFTP.Name = "CheckBox_ESTFTP"
        Me.CheckBox_ESTFTP.Size = New System.Drawing.Size(122, 17)
        Me.CheckBox_ESTFTP.TabIndex = 20
        Me.CheckBox_ESTFTP.Text = "EST FTP Completed"
        Me.CheckBox_ESTFTP.UseVisualStyleBackColor = True
        '
        'CheckBox_ESTStarted
        '
        Me.CheckBox_ESTStarted.AutoSize = True
        Me.CheckBox_ESTStarted.Enabled = False
        Me.CheckBox_ESTStarted.Location = New System.Drawing.Point(22, 116)
        Me.CheckBox_ESTStarted.Name = "CheckBox_ESTStarted"
        Me.CheckBox_ESTStarted.Size = New System.Drawing.Size(217, 17)
        Me.CheckBox_ESTStarted.TabIndex = 19
        Me.CheckBox_ESTStarted.Text = "Build Electronic Shelf Tag File Started"
        Me.CheckBox_ESTStarted.UseVisualStyleBackColor = True
        '
        'Label_JobStatus
        '
        Me.Label_JobStatus.AutoSize = True
        Me.Label_JobStatus.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_JobStatus.Location = New System.Drawing.Point(250, 85)
        Me.Label_JobStatus.Name = "Label_JobStatus"
        Me.Label_JobStatus.Size = New System.Drawing.Size(325, 13)
        Me.Label_JobStatus.TabIndex = 27
        Me.Label_JobStatus.Text = "Current status is unavailable.  Click ""Build Store Scale File"" to begin."
        '
        'Form_BuildFullESTFileForStore
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(634, 384)
        Me.Controls.Add(Me.Label_JobStatus)
        Me.Controls.Add(Me.CheckBox_ESTSuccess)
        Me.Controls.Add(Me.TextBox_ESTFTPStatus)
        Me.Controls.Add(Me.Label_ESTStoreCount)
        Me.Controls.Add(Me.CheckBox_ESTStores)
        Me.Controls.Add(Me.CheckBox_FullESTData)
        Me.Controls.Add(Me.CheckBox_ESTFTP)
        Me.Controls.Add(Me.CheckBox_ESTStarted)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboBox_StoreList)
        Me.Controls.Add(Me.Label_ExceptionText)
        Me.Controls.Add(Me.Label_Instructions)
        Me.Controls.Add(Me.Button_StartJob)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(650, 420)
        Me.Name = "Form_BuildFullESTFileForStore"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Build Store Electronic Shelf Tag File Job Controller"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label_ExceptionText As System.Windows.Forms.Label
    Friend WithEvents Label_Instructions As System.Windows.Forms.Label
    Friend WithEvents Button_StartJob As System.Windows.Forms.Button
    Friend WithEvents ComboBox_StoreList As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CheckBox_ESTSuccess As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox_ESTFTPStatus As System.Windows.Forms.RichTextBox
    Friend WithEvents Label_ESTStoreCount As System.Windows.Forms.Label
    Friend WithEvents CheckBox_ESTStores As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_FullESTData As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_ESTFTP As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_ESTStarted As System.Windows.Forms.CheckBox
    Friend WithEvents Label_JobStatus As System.Windows.Forms.Label
End Class
