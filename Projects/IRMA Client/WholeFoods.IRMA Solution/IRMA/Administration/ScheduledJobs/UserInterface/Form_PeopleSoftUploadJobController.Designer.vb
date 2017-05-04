<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_PeopleSoftUploadJobController
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
        Me.Button_StartJob = New System.Windows.Forms.Button
        Me.Label_Instructions = New System.Windows.Forms.Label
        Me.Label_MessageExecutionText = New System.Windows.Forms.Label
        Me.Label_JobStatus = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Button_StartJob
        '
        Me.Button_StartJob.Location = New System.Drawing.Point(22, 38)
        Me.Button_StartJob.Name = "Button_StartJob"
        Me.Button_StartJob.Size = New System.Drawing.Size(152, 23)
        Me.Button_StartJob.TabIndex = 0
        Me.Button_StartJob.Text = "Start AP Upload"
        Me.Button_StartJob.UseVisualStyleBackColor = True
        '
        'Label_Instructions
        '
        Me.Label_Instructions.AutoSize = True
        Me.Label_Instructions.Location = New System.Drawing.Point(19, 13)
        Me.Label_Instructions.Name = "Label_Instructions"
        Me.Label_Instructions.Size = New System.Drawing.Size(265, 13)
        Me.Label_Instructions.TabIndex = 1
        Me.Label_Instructions.Text = "Click the button below to start the AP Upload job."
        '
        'Label_MessageExecutionText
        '
        Me.Label_MessageExecutionText.AutoSize = True
        Me.Label_MessageExecutionText.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_MessageExecutionText.Location = New System.Drawing.Point(21, 117)
        Me.Label_MessageExecutionText.Name = "Label_MessageExecutionText"
        Me.Label_MessageExecutionText.Size = New System.Drawing.Size(94, 13)
        Me.Label_MessageExecutionText.TabIndex = 3
        Me.Label_MessageExecutionText.Text = "Execution Message"
        Me.Label_MessageExecutionText.Visible = False
        '
        'Label_JobStatus
        '
        Me.Label_JobStatus.AutoSize = True
        Me.Label_JobStatus.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_JobStatus.Location = New System.Drawing.Point(21, 76)
        Me.Label_JobStatus.Name = "Label_JobStatus"
        Me.Label_JobStatus.Size = New System.Drawing.Size(320, 13)
        Me.Label_JobStatus.TabIndex = 33
        Me.Label_JobStatus.Text = "AP Upload status is unavailable.  Click ""Start AP Upload"" to begin."
        '
        'Form_PeopleSoftUploadJobController
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(609, 254)
        Me.Controls.Add(Me.Label_JobStatus)
        Me.Controls.Add(Me.Label_MessageExecutionText)
        Me.Controls.Add(Me.Label_Instructions)
        Me.Controls.Add(Me.Button_StartJob)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimizeBox = False
        Me.Name = "Form_PeopleSoftUploadJobController"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "AP Upload Controller"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button_StartJob As System.Windows.Forms.Button
    Friend WithEvents Label_Instructions As System.Windows.Forms.Label
    Friend WithEvents Label_MessageExecutionText As System.Windows.Forms.Label
    Friend WithEvents Label_JobStatus As System.Windows.Forms.Label
End Class
