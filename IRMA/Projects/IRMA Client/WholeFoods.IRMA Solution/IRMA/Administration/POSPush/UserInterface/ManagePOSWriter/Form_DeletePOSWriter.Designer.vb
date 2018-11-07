<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_DeletePOSWriter
    Inherits Form_IRMADelete

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
        Me.Label_WriterClassVal = New System.Windows.Forms.Label
        Me.Label_WriterCodeVal = New System.Windows.Forms.Label
        Me.Label_WriterClass = New System.Windows.Forms.Label
        Me.Label_WriterCode = New System.Windows.Forms.Label
        Me.Label_StoreWarnings = New System.Windows.Forms.Label
        Me.Panel_StandardButtons.SuspendLayout()
        Me.Panel_Instructions.SuspendLayout()
        Me.GroupBox_DeleteData.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Delete
        '
        '
        'Label_Warning
        '
        Me.Label_Warning.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Warning.Location = New System.Drawing.Point(43, 8)
        Me.Label_Warning.Size = New System.Drawing.Size(425, 17)
        Me.Label_Warning.Text = "Warning!  You are about to delete the following POS Writer record."
        '
        'GroupBox_DeleteData
        '
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_StoreWarnings)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_WriterClassVal)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_WriterCodeVal)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_WriterClass)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_WriterCode)
        '
        'Label_WriterClassVal
        '
        Me.Label_WriterClassVal.AutoSize = True
        Me.Label_WriterClassVal.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_WriterClassVal.Location = New System.Drawing.Point(180, 61)
        Me.Label_WriterClassVal.Name = "Label_WriterClassVal"
        Me.Label_WriterClassVal.Size = New System.Drawing.Size(41, 13)
        Me.Label_WriterClassVal.TabIndex = 23
        Me.Label_WriterClassVal.Text = "Label1"
        '
        'Label_WriterCodeVal
        '
        Me.Label_WriterCodeVal.AutoSize = True
        Me.Label_WriterCodeVal.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_WriterCodeVal.Location = New System.Drawing.Point(180, 30)
        Me.Label_WriterCodeVal.Name = "Label_WriterCodeVal"
        Me.Label_WriterCodeVal.Size = New System.Drawing.Size(41, 13)
        Me.Label_WriterCodeVal.TabIndex = 22
        Me.Label_WriterCodeVal.Text = "Label1"
        '
        'Label_WriterClass
        '
        Me.Label_WriterClass.AutoSize = True
        Me.Label_WriterClass.Location = New System.Drawing.Point(45, 61)
        Me.Label_WriterClass.Name = "Label_WriterClass"
        Me.Label_WriterClass.Size = New System.Drawing.Size(109, 13)
        Me.Label_WriterClass.TabIndex = 21
        Me.Label_WriterClass.Text = "Writer VB.Net Class:"
        '
        'Label_WriterCode
        '
        Me.Label_WriterCode.AutoSize = True
        Me.Label_WriterCode.Location = New System.Drawing.Point(45, 30)
        Me.Label_WriterCode.Name = "Label_WriterCode"
        Me.Label_WriterCode.Size = New System.Drawing.Size(72, 13)
        Me.Label_WriterCode.TabIndex = 19
        Me.Label_WriterCode.Text = "Writer Code:"
        '
        'Label_StoreWarnings
        '
        Me.Label_StoreWarnings.AutoSize = True
        Me.Label_StoreWarnings.Location = New System.Drawing.Point(6, 93)
        Me.Label_StoreWarnings.Name = "Label_StoreWarnings"
        Me.Label_StoreWarnings.Size = New System.Drawing.Size(13, 13)
        Me.Label_StoreWarnings.TabIndex = 24
        Me.Label_StoreWarnings.Text = "  "
        '
        'Form_DeletePOSWriter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(576, 266)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_DeletePOSWriter"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Delete File Writer"
        Me.Panel_StandardButtons.ResumeLayout(False)
        Me.Panel_Instructions.ResumeLayout(False)
        Me.Panel_Instructions.PerformLayout()
        Me.GroupBox_DeleteData.ResumeLayout(False)
        Me.GroupBox_DeleteData.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label_WriterClassVal As System.Windows.Forms.Label
    Friend WithEvents Label_WriterCodeVal As System.Windows.Forms.Label
    Friend WithEvents Label_WriterClass As System.Windows.Forms.Label
    Friend WithEvents Label_WriterCode As System.Windows.Forms.Label
    Friend WithEvents Label_StoreWarnings As System.Windows.Forms.Label

End Class
