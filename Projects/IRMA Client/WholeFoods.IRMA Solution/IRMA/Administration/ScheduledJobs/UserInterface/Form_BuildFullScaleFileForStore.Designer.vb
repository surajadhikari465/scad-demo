<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_BuildFullScaleFileForStore
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
        Me.Label_ScaleRegional = New System.Windows.Forms.Label
        Me.CheckBox_ScaleSuccess = New System.Windows.Forms.CheckBox
        Me.TextBox_ScaleFTPStatus = New System.Windows.Forms.RichTextBox
        Me.Label_ScalesStoreCount = New System.Windows.Forms.Label
        Me.CheckBox_ScaleStores = New System.Windows.Forms.CheckBox
        Me.CheckBox_FullScaleData = New System.Windows.Forms.CheckBox
        Me.CheckBox_ScaleFTP = New System.Windows.Forms.CheckBox
        Me.CheckBox_ScaleStarted = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'Label_ExceptionText
        '
        Me.Label_ExceptionText.AutoSize = True
        Me.Label_ExceptionText.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ExceptionText.Location = New System.Drawing.Point(20, 315)
        Me.Label_ExceptionText.Name = "Label_ExceptionText"
        Me.Label_ExceptionText.Size = New System.Drawing.Size(106, 13)
        Me.Label_ExceptionText.TabIndex = 7
        Me.Label_ExceptionText.Text = "Exception stack trace"
        Me.Label_ExceptionText.Visible = False
        '
        'Label_JobStatus
        '
        Me.Label_JobStatus.AutoSize = True
        Me.Label_JobStatus.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_JobStatus.Location = New System.Drawing.Point(206, 82)
        Me.Label_JobStatus.Name = "Label_JobStatus"
        Me.Label_JobStatus.Size = New System.Drawing.Size(325, 13)
        Me.Label_JobStatus.TabIndex = 6
        Me.Label_JobStatus.Text = "Current status is unavailable.  Click ""Build Store Scale File"" to begin."
        '
        'Label_Instructions
        '
        Me.Label_Instructions.AutoSize = True
        Me.Label_Instructions.Location = New System.Drawing.Point(20, 61)
        Me.Label_Instructions.Name = "Label_Instructions"
        Me.Label_Instructions.Size = New System.Drawing.Size(319, 13)
        Me.Label_Instructions.TabIndex = 5
        Me.Label_Instructions.Text = "Click the button below to start the Build Store Scale File Job."
        '
        'Button_StartJob
        '
        Me.Button_StartJob.Location = New System.Drawing.Point(23, 77)
        Me.Button_StartJob.Name = "Button_StartJob"
        Me.Button_StartJob.Size = New System.Drawing.Size(152, 23)
        Me.Button_StartJob.TabIndex = 4
        Me.Button_StartJob.Text = "Build Store Scale File"
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
        Me.Label1.Size = New System.Drawing.Size(276, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Select the store you wish to build a full scale file for:"
        '
        'Label_ScaleRegional
        '
        Me.Label_ScaleRegional.AutoSize = True
        Me.Label_ScaleRegional.Enabled = False
        Me.Label_ScaleRegional.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ScaleRegional.Location = New System.Drawing.Point(336, 149)
        Me.Label_ScaleRegional.Name = "Label_ScaleRegional"
        Me.Label_ScaleRegional.Size = New System.Drawing.Size(155, 13)
        Me.Label_ScaleRegional.TabIndex = 26
        Me.Label_ScaleRegional.Text = "Regional Scale Configuration?  "
        '
        'CheckBox_ScaleSuccess
        '
        Me.CheckBox_ScaleSuccess.AutoSize = True
        Me.CheckBox_ScaleSuccess.Enabled = False
        Me.CheckBox_ScaleSuccess.Location = New System.Drawing.Point(73, 265)
        Me.CheckBox_ScaleSuccess.Name = "CheckBox_ScaleSuccess"
        Me.CheckBox_ScaleSuccess.Size = New System.Drawing.Size(218, 17)
        Me.CheckBox_ScaleSuccess.TabIndex = 25
        Me.CheckBox_ScaleSuccess.Text = "Full Scale File Completed Successfully"
        Me.CheckBox_ScaleSuccess.UseVisualStyleBackColor = True
        '
        'TextBox_ScaleFTPStatus
        '
        Me.TextBox_ScaleFTPStatus.Location = New System.Drawing.Point(208, 215)
        Me.TextBox_ScaleFTPStatus.Name = "TextBox_ScaleFTPStatus"
        Me.TextBox_ScaleFTPStatus.ReadOnly = True
        Me.TextBox_ScaleFTPStatus.Size = New System.Drawing.Size(368, 44)
        Me.TextBox_ScaleFTPStatus.TabIndex = 24
        Me.TextBox_ScaleFTPStatus.Text = ""
        '
        'Label_ScalesStoreCount
        '
        Me.Label_ScalesStoreCount.AutoSize = True
        Me.Label_ScalesStoreCount.Enabled = False
        Me.Label_ScalesStoreCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ScalesStoreCount.Location = New System.Drawing.Point(336, 172)
        Me.Label_ScalesStoreCount.Name = "Label_ScalesStoreCount"
        Me.Label_ScalesStoreCount.Size = New System.Drawing.Size(88, 13)
        Me.Label_ScalesStoreCount.TabIndex = 23
        Me.Label_ScalesStoreCount.Text = "# Stores Read?  "
        '
        'CheckBox_ScaleStores
        '
        Me.CheckBox_ScaleStores.AutoSize = True
        Me.CheckBox_ScaleStores.Enabled = False
        Me.CheckBox_ScaleStores.Location = New System.Drawing.Point(73, 171)
        Me.CheckBox_ScaleStores.Name = "CheckBox_ScaleStores"
        Me.CheckBox_ScaleStores.Size = New System.Drawing.Size(278, 17)
        Me.CheckBox_ScaleStores.TabIndex = 22
        Me.CheckBox_ScaleStores.Text = "Finished Reading Scale Store Configuration Data"
        Me.CheckBox_ScaleStores.UseVisualStyleBackColor = True
        '
        'CheckBox_FullScaleData
        '
        Me.CheckBox_FullScaleData.AutoSize = True
        Me.CheckBox_FullScaleData.Enabled = False
        Me.CheckBox_FullScaleData.Location = New System.Drawing.Point(73, 194)
        Me.CheckBox_FullScaleData.Name = "CheckBox_FullScaleData"
        Me.CheckBox_FullScaleData.Size = New System.Drawing.Size(257, 17)
        Me.CheckBox_FullScaleData.TabIndex = 21
        Me.CheckBox_FullScaleData.Text = "Finished Reading List of Scale Items for Store"
        Me.CheckBox_FullScaleData.UseVisualStyleBackColor = True
        '
        'CheckBox_ScaleFTP
        '
        Me.CheckBox_ScaleFTP.AutoSize = True
        Me.CheckBox_ScaleFTP.Enabled = False
        Me.CheckBox_ScaleFTP.Location = New System.Drawing.Point(73, 217)
        Me.CheckBox_ScaleFTP.Name = "CheckBox_ScaleFTP"
        Me.CheckBox_ScaleFTP.Size = New System.Drawing.Size(131, 17)
        Me.CheckBox_ScaleFTP.TabIndex = 20
        Me.CheckBox_ScaleFTP.Text = "Scale FTP Completed"
        Me.CheckBox_ScaleFTP.UseVisualStyleBackColor = True
        '
        'CheckBox_ScaleStarted
        '
        Me.CheckBox_ScaleStarted.AutoSize = True
        Me.CheckBox_ScaleStarted.Enabled = False
        Me.CheckBox_ScaleStarted.Location = New System.Drawing.Point(23, 148)
        Me.CheckBox_ScaleStarted.Name = "CheckBox_ScaleStarted"
        Me.CheckBox_ScaleStarted.Size = New System.Drawing.Size(143, 17)
        Me.CheckBox_ScaleStarted.TabIndex = 19
        Me.CheckBox_ScaleStarted.Text = "Build Scale File Started"
        Me.CheckBox_ScaleStarted.UseVisualStyleBackColor = True
        '
        'Form_BuildFullScaleFileForStore
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(634, 384)
        Me.Controls.Add(Me.Label_ScaleRegional)
        Me.Controls.Add(Me.CheckBox_ScaleSuccess)
        Me.Controls.Add(Me.TextBox_ScaleFTPStatus)
        Me.Controls.Add(Me.Label_ScalesStoreCount)
        Me.Controls.Add(Me.CheckBox_ScaleStores)
        Me.Controls.Add(Me.CheckBox_FullScaleData)
        Me.Controls.Add(Me.CheckBox_ScaleFTP)
        Me.Controls.Add(Me.CheckBox_ScaleStarted)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboBox_StoreList)
        Me.Controls.Add(Me.Label_ExceptionText)
        Me.Controls.Add(Me.Label_JobStatus)
        Me.Controls.Add(Me.Label_Instructions)
        Me.Controls.Add(Me.Button_StartJob)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(650, 420)
        Me.Name = "Form_BuildFullScaleFileForStore"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Build Store Scale File Job Controller"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label_ExceptionText As System.Windows.Forms.Label
    Friend WithEvents Label_JobStatus As System.Windows.Forms.Label
    Friend WithEvents Label_Instructions As System.Windows.Forms.Label
    Friend WithEvents Button_StartJob As System.Windows.Forms.Button
    Friend WithEvents ComboBox_StoreList As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label_ScaleRegional As System.Windows.Forms.Label
    Friend WithEvents CheckBox_ScaleSuccess As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox_ScaleFTPStatus As System.Windows.Forms.RichTextBox
    Friend WithEvents Label_ScalesStoreCount As System.Windows.Forms.Label
    Friend WithEvents CheckBox_ScaleStores As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_FullScaleData As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_ScaleFTP As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_ScaleStarted As System.Windows.Forms.CheckBox
End Class
