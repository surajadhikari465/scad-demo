<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_EditBatchIdValue
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
        Me.Label_FileWriterCode = New System.Windows.Forms.Label
        Me.Label_ScaleWriterType = New System.Windows.Forms.Label
        Me.Label_ScaleWriterTypeHeader = New System.Windows.Forms.Label
        Me.Label_WriterType = New System.Windows.Forms.Label
        Me.Label_WriterTypeHeader = New System.Windows.Forms.Label
        Me.Label_FileWriterCodeHeader = New System.Windows.Forms.Label
        Me.Label_ChangeTypeHeader = New System.Windows.Forms.Label
        Me.Label_ChangeType = New System.Windows.Forms.Label
        Me.TextBox_DefaultBatchId = New System.Windows.Forms.TextBox
        Me.Label_DefaultBatchIdHeader = New System.Windows.Forms.Label
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.Button_OK = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label_FileWriterCode
        '
        Me.Label_FileWriterCode.AutoSize = True
        Me.Label_FileWriterCode.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_FileWriterCode.Location = New System.Drawing.Point(126, 9)
        Me.Label_FileWriterCode.Name = "Label_FileWriterCode"
        Me.Label_FileWriterCode.Size = New System.Drawing.Size(41, 13)
        Me.Label_FileWriterCode.TabIndex = 30
        Me.Label_FileWriterCode.Text = "Label1"
        '
        'Label_ScaleWriterType
        '
        Me.Label_ScaleWriterType.AutoSize = True
        Me.Label_ScaleWriterType.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ScaleWriterType.Location = New System.Drawing.Point(126, 55)
        Me.Label_ScaleWriterType.Name = "Label_ScaleWriterType"
        Me.Label_ScaleWriterType.Size = New System.Drawing.Size(41, 13)
        Me.Label_ScaleWriterType.TabIndex = 29
        Me.Label_ScaleWriterType.Text = "Label2"
        '
        'Label_ScaleWriterTypeHeader
        '
        Me.Label_ScaleWriterTypeHeader.AutoSize = True
        Me.Label_ScaleWriterTypeHeader.Location = New System.Drawing.Point(12, 55)
        Me.Label_ScaleWriterTypeHeader.Name = "Label_ScaleWriterTypeHeader"
        Me.Label_ScaleWriterTypeHeader.Size = New System.Drawing.Size(97, 13)
        Me.Label_ScaleWriterTypeHeader.TabIndex = 28
        Me.Label_ScaleWriterTypeHeader.Text = "Scale Writer Type:"
        '
        'Label_WriterType
        '
        Me.Label_WriterType.AutoSize = True
        Me.Label_WriterType.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_WriterType.Location = New System.Drawing.Point(126, 32)
        Me.Label_WriterType.Name = "Label_WriterType"
        Me.Label_WriterType.Size = New System.Drawing.Size(41, 13)
        Me.Label_WriterType.TabIndex = 27
        Me.Label_WriterType.Text = "Label1"
        '
        'Label_WriterTypeHeader
        '
        Me.Label_WriterTypeHeader.AutoSize = True
        Me.Label_WriterTypeHeader.Location = New System.Drawing.Point(12, 32)
        Me.Label_WriterTypeHeader.Name = "Label_WriterTypeHeader"
        Me.Label_WriterTypeHeader.Size = New System.Drawing.Size(89, 13)
        Me.Label_WriterTypeHeader.TabIndex = 26
        Me.Label_WriterTypeHeader.Text = "File Writer Type:"
        '
        'Label_FileWriterCodeHeader
        '
        Me.Label_FileWriterCodeHeader.AutoSize = True
        Me.Label_FileWriterCodeHeader.Location = New System.Drawing.Point(12, 9)
        Me.Label_FileWriterCodeHeader.Name = "Label_FileWriterCodeHeader"
        Me.Label_FileWriterCodeHeader.Size = New System.Drawing.Size(93, 13)
        Me.Label_FileWriterCodeHeader.TabIndex = 25
        Me.Label_FileWriterCodeHeader.Text = "File Writer Code:"
        '
        'Label_ChangeTypeHeader
        '
        Me.Label_ChangeTypeHeader.AutoSize = True
        Me.Label_ChangeTypeHeader.Location = New System.Drawing.Point(6, 16)
        Me.Label_ChangeTypeHeader.Name = "Label_ChangeTypeHeader"
        Me.Label_ChangeTypeHeader.Size = New System.Drawing.Size(76, 13)
        Me.Label_ChangeTypeHeader.TabIndex = 31
        Me.Label_ChangeTypeHeader.Text = "Change Type:"
        '
        'Label_ChangeType
        '
        Me.Label_ChangeType.AutoSize = True
        Me.Label_ChangeType.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ChangeType.Location = New System.Drawing.Point(114, 16)
        Me.Label_ChangeType.Name = "Label_ChangeType"
        Me.Label_ChangeType.Size = New System.Drawing.Size(75, 13)
        Me.Label_ChangeType.TabIndex = 32
        Me.Label_ChangeType.Text = "Change Type"
        '
        'TextBox_DefaultBatchId
        '
        Me.TextBox_DefaultBatchId.Location = New System.Drawing.Point(117, 36)
        Me.TextBox_DefaultBatchId.MaxLength = 4
        Me.TextBox_DefaultBatchId.Name = "TextBox_DefaultBatchId"
        Me.TextBox_DefaultBatchId.Size = New System.Drawing.Size(53, 22)
        Me.TextBox_DefaultBatchId.TabIndex = 33
        '
        'Label_DefaultBatchIdHeader
        '
        Me.Label_DefaultBatchIdHeader.AutoSize = True
        Me.Label_DefaultBatchIdHeader.Location = New System.Drawing.Point(6, 39)
        Me.Label_DefaultBatchIdHeader.Name = "Label_DefaultBatchIdHeader"
        Me.Label_DefaultBatchIdHeader.Size = New System.Drawing.Size(80, 13)
        Me.Label_DefaultBatchIdHeader.TabIndex = 34
        Me.Label_DefaultBatchIdHeader.Text = "Default Value:"
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Cancel.Location = New System.Drawing.Point(151, 155)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 35
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Button_OK
        '
        Me.Button_OK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_OK.Location = New System.Drawing.Point(232, 155)
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.Size = New System.Drawing.Size(75, 23)
        Me.Button_OK.TabIndex = 36
        Me.Button_OK.Text = "OK"
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label_ChangeTypeHeader)
        Me.GroupBox1.Controls.Add(Me.Label_ChangeType)
        Me.GroupBox1.Controls.Add(Me.TextBox_DefaultBatchId)
        Me.GroupBox1.Controls.Add(Me.Label_DefaultBatchIdHeader)
        Me.GroupBox1.Location = New System.Drawing.Point(15, 81)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(292, 64)
        Me.GroupBox1.TabIndex = 37
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Default Batch Id"
        '
        'Form_EditBatchIdValue
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(320, 191)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_OK)
        Me.Controls.Add(Me.Label_FileWriterCode)
        Me.Controls.Add(Me.Label_ScaleWriterType)
        Me.Controls.Add(Me.Label_ScaleWriterTypeHeader)
        Me.Controls.Add(Me.Label_WriterType)
        Me.Controls.Add(Me.Label_WriterTypeHeader)
        Me.Controls.Add(Me.Label_FileWriterCodeHeader)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_EditBatchIdValue"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Edit Batch Id Value"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label_FileWriterCode As System.Windows.Forms.Label
    Friend WithEvents Label_ScaleWriterType As System.Windows.Forms.Label
    Friend WithEvents Label_ScaleWriterTypeHeader As System.Windows.Forms.Label
    Friend WithEvents Label_WriterType As System.Windows.Forms.Label
    Friend WithEvents Label_WriterTypeHeader As System.Windows.Forms.Label
    Friend WithEvents Label_FileWriterCodeHeader As System.Windows.Forms.Label
    Friend WithEvents Label_ChangeTypeHeader As System.Windows.Forms.Label
    Friend WithEvents Label_ChangeType As System.Windows.Forms.Label
    Friend WithEvents TextBox_DefaultBatchId As System.Windows.Forms.TextBox
    Friend WithEvents Label_DefaultBatchIdHeader As System.Windows.Forms.Label
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents Button_OK As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
End Class
