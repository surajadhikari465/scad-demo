<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_AuditExceptionsJobController
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
        Me.Button_Close = New System.Windows.Forms.Button
        Me.btnUploadFiles = New System.Windows.Forms.Button
        Me.btnImportFiles = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label_ExceptionText
        '
        Me.Label_ExceptionText.AutoSize = True
        Me.Label_ExceptionText.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ExceptionText.Location = New System.Drawing.Point(75, 112)
        Me.Label_ExceptionText.Name = "Label_ExceptionText"
        Me.Label_ExceptionText.Size = New System.Drawing.Size(106, 13)
        Me.Label_ExceptionText.TabIndex = 7
        Me.Label_ExceptionText.Text = "Exception stack trace"
        Me.Label_ExceptionText.Visible = False
        '
        'Label_JobStatus
        '
        Me.Label_JobStatus.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_JobStatus.Location = New System.Drawing.Point(20, 85)
        Me.Label_JobStatus.Name = "Label_JobStatus"
        Me.Label_JobStatus.Size = New System.Drawing.Size(448, 19)
        Me.Label_JobStatus.TabIndex = 6
        Me.Label_JobStatus.Text = "Audit Report status is unavailable.  Click ""Start Audit Report"" to begin."
        '
        'Label_Instructions
        '
        Me.Label_Instructions.AutoSize = True
        Me.Label_Instructions.Location = New System.Drawing.Point(20, 19)
        Me.Label_Instructions.Name = "Label_Instructions"
        Me.Label_Instructions.Size = New System.Drawing.Size(376, 13)
        Me.Label_Instructions.TabIndex = 5
        Me.Label_Instructions.Text = "Click a button below to import or view the Audit Exceptions Report Job."
        '
        'Button_StartJob
        '
        Me.Button_StartJob.Location = New System.Drawing.Point(128, 48)
        Me.Button_StartJob.Name = "Button_StartJob"
        Me.Button_StartJob.Size = New System.Drawing.Size(80, 23)
        Me.Button_StartJob.TabIndex = 4
        Me.Button_StartJob.Text = "View Report"
        Me.Button_StartJob.UseVisualStyleBackColor = True
        '
        'Button_Close
        '
        Me.Button_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Close.Location = New System.Drawing.Point(456, 176)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(75, 23)
        Me.Button_Close.TabIndex = 10
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'btnUploadFiles
        '
        Me.btnUploadFiles.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUploadFiles.Location = New System.Drawing.Point(466, -3)
        Me.btnUploadFiles.Name = "btnUploadFiles"
        Me.btnUploadFiles.Size = New System.Drawing.Size(80, 23)
        Me.btnUploadFiles.TabIndex = 11
        Me.btnUploadFiles.Text = "Upload File(s)"
        Me.btnUploadFiles.UseVisualStyleBackColor = True
        Me.btnUploadFiles.Visible = False
        '
        'btnImportFiles
        '
        Me.btnImportFiles.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImportFiles.Location = New System.Drawing.Point(42, 48)
        Me.btnImportFiles.Name = "btnImportFiles"
        Me.btnImportFiles.Size = New System.Drawing.Size(80, 23)
        Me.btnImportFiles.TabIndex = 12
        Me.btnImportFiles.Text = "Import File(s)"
        Me.btnImportFiles.UseVisualStyleBackColor = True
        '
        'Form_AuditExceptionsJobController
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(543, 211)
        Me.Controls.Add(Me.btnImportFiles)
        Me.Controls.Add(Me.btnUploadFiles)
        Me.Controls.Add(Me.Button_Close)
        Me.Controls.Add(Me.Label_ExceptionText)
        Me.Controls.Add(Me.Label_JobStatus)
        Me.Controls.Add(Me.Label_Instructions)
        Me.Controls.Add(Me.Button_StartJob)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimizeBox = False
        Me.Name = "Form_AuditExceptionsJobController"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Audit Exceptions Report Job Controller"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label_ExceptionText As System.Windows.Forms.Label
    Friend WithEvents Label_JobStatus As System.Windows.Forms.Label
    Friend WithEvents Label_Instructions As System.Windows.Forms.Label
    Friend WithEvents Button_StartJob As System.Windows.Forms.Button
    Friend WithEvents Button_Close As System.Windows.Forms.Button
    Friend WithEvents btnUploadFiles As System.Windows.Forms.Button
    Friend WithEvents btnImportFiles As System.Windows.Forms.Button
End Class
