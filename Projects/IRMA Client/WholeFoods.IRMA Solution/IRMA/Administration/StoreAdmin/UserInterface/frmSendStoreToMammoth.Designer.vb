<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSendStoreToMammoth
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ComboBox_StoreList = New System.Windows.Forms.ComboBox()
        Me.Label_JobStatus = New System.Windows.Forms.Label()
        Me.Label_Instructions = New System.Windows.Forms.Label()
        Me.Button_StartJob = New System.Windows.Forms.Button()
        Me.Label_ExceptionText = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(183, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Select the store to send to Mammoth:"
        '
        'ComboBox_StoreList
        '
        Me.ComboBox_StoreList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_StoreList.FormattingEnabled = True
        Me.ComboBox_StoreList.Location = New System.Drawing.Point(15, 44)
        Me.ComboBox_StoreList.Name = "ComboBox_StoreList"
        Me.ComboBox_StoreList.Size = New System.Drawing.Size(227, 21)
        Me.ComboBox_StoreList.TabIndex = 13
        '
        'Label_JobStatus
        '
        Me.Label_JobStatus.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_JobStatus.Location = New System.Drawing.Point(12, 137)
        Me.Label_JobStatus.Name = "Label_JobStatus"
        Me.Label_JobStatus.Size = New System.Drawing.Size(364, 70)
        Me.Label_JobStatus.TabIndex = 12
        Me.Label_JobStatus.Text = "Current status is unavailable.  Click ""Send Store To Mammoth"" to begin."
        '
        'Label_Instructions
        '
        Me.Label_Instructions.AutoSize = True
        Me.Label_Instructions.Location = New System.Drawing.Point(12, 76)
        Me.Label_Instructions.Name = "Label_Instructions"
        Me.Label_Instructions.Size = New System.Drawing.Size(258, 13)
        Me.Label_Instructions.TabIndex = 11
        Me.Label_Instructions.Text = "Click the button below to send the store to Mammoth."
        '
        'Button_StartJob
        '
        Me.Button_StartJob.Location = New System.Drawing.Point(15, 96)
        Me.Button_StartJob.Name = "Button_StartJob"
        Me.Button_StartJob.Size = New System.Drawing.Size(152, 23)
        Me.Button_StartJob.TabIndex = 10
        Me.Button_StartJob.Text = "Send Store To Mammoth"
        Me.Button_StartJob.UseVisualStyleBackColor = True
        '
        'Label_ExceptionText
        '
        Me.Label_ExceptionText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label_ExceptionText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label_ExceptionText.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ExceptionText.Location = New System.Drawing.Point(12, 238)
        Me.Label_ExceptionText.Name = "Label_ExceptionText"
        Me.Label_ExceptionText.Size = New System.Drawing.Size(363, 115)
        Me.Label_ExceptionText.TabIndex = 15
        Me.Label_ExceptionText.Text = "Exception stack trace"
        Me.Label_ExceptionText.Visible = False
        '
        'frmSendStoreToMammoth
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 362)
        Me.Controls.Add(Me.Label_ExceptionText)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboBox_StoreList)
        Me.Controls.Add(Me.Label_JobStatus)
        Me.Controls.Add(Me.Label_Instructions)
        Me.Controls.Add(Me.Button_StartJob)
        Me.Name = "frmSendStoreToMammoth"
        Me.Text = "Send Store To Mammoth"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboBox_StoreList As System.Windows.Forms.ComboBox
    Friend WithEvents Label_JobStatus As System.Windows.Forms.Label
    Friend WithEvents Label_Instructions As System.Windows.Forms.Label
    Friend WithEvents Button_StartJob As System.Windows.Forms.Button
    Friend WithEvents Label_ExceptionText As System.Windows.Forms.Label
End Class
