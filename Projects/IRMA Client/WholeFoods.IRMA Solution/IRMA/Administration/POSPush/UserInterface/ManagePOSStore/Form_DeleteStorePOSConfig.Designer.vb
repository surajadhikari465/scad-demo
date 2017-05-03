<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_DeleteStorePOSConfig
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
        Me.Label_ConfigTypeVal = New System.Windows.Forms.Label
        Me.Label_FileWriterVal = New System.Windows.Forms.Label
        Me.Label_StoreVal = New System.Windows.Forms.Label
        Me.Label_FileWriter = New System.Windows.Forms.Label
        Me.Label_ConfigType = New System.Windows.Forms.Label
        Me.Label_Store = New System.Windows.Forms.Label
        Me.Panel_StandardButtons.SuspendLayout()
        Me.Panel_Instructions.SuspendLayout()
        Me.GroupBox_DeleteData.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Delete
        '
        '
        'Button_Cancel
        '
        '
        'Label_Warning
        '
        Me.Label_Warning.Location = New System.Drawing.Point(12, 9)
        Me.Label_Warning.Size = New System.Drawing.Size(547, 17)
        Me.Label_Warning.Text = "Warning!  You are about to delete a Store POS Push configuration record."
        '
        'GroupBox_DeleteData
        '
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_ConfigTypeVal)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_FileWriterVal)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_StoreVal)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_FileWriter)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_ConfigType)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_Store)
        '
        'Label_ConfigTypeVal
        '
        Me.Label_ConfigTypeVal.AutoSize = True
        Me.Label_ConfigTypeVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ConfigTypeVal.Location = New System.Drawing.Point(179, 97)
        Me.Label_ConfigTypeVal.Name = "Label_ConfigTypeVal"
        Me.Label_ConfigTypeVal.Size = New System.Drawing.Size(45, 13)
        Me.Label_ConfigTypeVal.TabIndex = 18
        Me.Label_ConfigTypeVal.Text = "Label1"
        '
        'Label_FileWriterVal
        '
        Me.Label_FileWriterVal.AutoSize = True
        Me.Label_FileWriterVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_FileWriterVal.Location = New System.Drawing.Point(179, 66)
        Me.Label_FileWriterVal.Name = "Label_FileWriterVal"
        Me.Label_FileWriterVal.Size = New System.Drawing.Size(45, 13)
        Me.Label_FileWriterVal.TabIndex = 17
        Me.Label_FileWriterVal.Text = "Label1"
        '
        'Label_StoreVal
        '
        Me.Label_StoreVal.AutoSize = True
        Me.Label_StoreVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_StoreVal.Location = New System.Drawing.Point(179, 35)
        Me.Label_StoreVal.Name = "Label_StoreVal"
        Me.Label_StoreVal.Size = New System.Drawing.Size(45, 13)
        Me.Label_StoreVal.TabIndex = 16
        Me.Label_StoreVal.Text = "Label1"
        '
        'Label_FileWriter
        '
        Me.Label_FileWriter.AutoSize = True
        Me.Label_FileWriter.Location = New System.Drawing.Point(44, 66)
        Me.Label_FileWriter.Name = "Label_FileWriter"
        Me.Label_FileWriter.Size = New System.Drawing.Size(57, 13)
        Me.Label_FileWriter.TabIndex = 15
        Me.Label_FileWriter.Text = "File Writer:"
        '
        'Label_ConfigType
        '
        Me.Label_ConfigType.AutoSize = True
        Me.Label_ConfigType.Location = New System.Drawing.Point(44, 97)
        Me.Label_ConfigType.Name = "Label_ConfigType"
        Me.Label_ConfigType.Size = New System.Drawing.Size(125, 13)
        Me.Label_ConfigType.TabIndex = 14
        Me.Label_ConfigType.Text = "Acknowledgement Type:"
        '
        'Label_Store
        '
        Me.Label_Store.AutoSize = True
        Me.Label_Store.Location = New System.Drawing.Point(44, 35)
        Me.Label_Store.Name = "Label_Store"
        Me.Label_Store.Size = New System.Drawing.Size(35, 13)
        Me.Label_Store.TabIndex = 13
        Me.Label_Store.Text = "Store:"
        '
        'Form_DeleteStorePOSConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(576, 266)
        Me.Name = "Form_DeleteStorePOSConfig"
        Me.Text = "Delete Store"
        Me.Panel_StandardButtons.ResumeLayout(False)
        Me.Panel_Instructions.ResumeLayout(False)
        Me.Panel_Instructions.PerformLayout()
        Me.GroupBox_DeleteData.ResumeLayout(False)
        Me.GroupBox_DeleteData.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label_ConfigTypeVal As System.Windows.Forms.Label
    Friend WithEvents Label_FileWriterVal As System.Windows.Forms.Label
    Friend WithEvents Label_StoreVal As System.Windows.Forms.Label
    Friend WithEvents Label_FileWriter As System.Windows.Forms.Label
    Friend WithEvents Label_ConfigType As System.Windows.Forms.Label
    Friend WithEvents Label_Store As System.Windows.Forms.Label

End Class
