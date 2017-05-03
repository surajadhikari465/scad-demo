<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_EditStorePOSConfig
    Inherits Form_IRMABase

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
        Me.Label_StoreVal = New System.Windows.Forms.Label()
        Me.ComboBox_ConfigTypeVal = New System.Windows.Forms.ComboBox()
        Me.Button_Save = New System.Windows.Forms.Button()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.ComboBox_FileWriterVal = New System.Windows.Forms.ComboBox()
        Me.ComboBox_StoreVal = New System.Windows.Forms.ComboBox()
        Me.GroupBox_EditStore = New System.Windows.Forms.GroupBox()
        Me.ComboBox_TagWriterVal = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ComboBox_ScaleWriterVal = New System.Windows.Forms.ComboBox()
        Me.Label_ScaleWriter = New System.Windows.Forms.Label()
        Me.Label_FileWriter = New System.Windows.Forms.Label()
        Me.Label_ConfigType = New System.Windows.Forms.Label()
        Me.Label_StoreNo = New System.Windows.Forms.Label()
        Me.ComboBox_ElectronicShelfTagVal = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox_EditStore.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label_StoreVal
        '
        Me.Label_StoreVal.AutoSize = True
        Me.Label_StoreVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_StoreVal.Location = New System.Drawing.Point(135, 22)
        Me.Label_StoreVal.Name = "Label_StoreVal"
        Me.Label_StoreVal.Size = New System.Drawing.Size(45, 13)
        Me.Label_StoreVal.TabIndex = 7
        Me.Label_StoreVal.Text = "Label1"
        '
        'ComboBox_ConfigTypeVal
        '
        Me.ComboBox_ConfigTypeVal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_ConfigTypeVal.FormattingEnabled = True
        Me.ComboBox_ConfigTypeVal.Location = New System.Drawing.Point(138, 160)
        Me.ComboBox_ConfigTypeVal.Name = "ComboBox_ConfigTypeVal"
        Me.ComboBox_ConfigTypeVal.Size = New System.Drawing.Size(133, 21)
        Me.ComboBox_ConfigTypeVal.TabIndex = 6
        '
        'Button_Save
        '
        Me.Button_Save.Location = New System.Drawing.Point(390, 247)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(75, 23)
        Me.Button_Save.TabIndex = 8
        Me.Button_Save.Text = "Save"
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(310, 247)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 7
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'ComboBox_FileWriterVal
        '
        Me.ComboBox_FileWriterVal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_FileWriterVal.FormattingEnabled = True
        Me.ComboBox_FileWriterVal.Location = New System.Drawing.Point(138, 52)
        Me.ComboBox_FileWriterVal.Name = "ComboBox_FileWriterVal"
        Me.ComboBox_FileWriterVal.Size = New System.Drawing.Size(133, 21)
        Me.ComboBox_FileWriterVal.TabIndex = 5
        '
        'ComboBox_StoreVal
        '
        Me.ComboBox_StoreVal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_StoreVal.FormattingEnabled = True
        Me.ComboBox_StoreVal.Location = New System.Drawing.Point(138, 19)
        Me.ComboBox_StoreVal.Name = "ComboBox_StoreVal"
        Me.ComboBox_StoreVal.Size = New System.Drawing.Size(265, 21)
        Me.ComboBox_StoreVal.TabIndex = 4
        '
        'GroupBox_EditStore
        '
        Me.GroupBox_EditStore.Controls.Add(Me.ComboBox_ElectronicShelfTagVal)
        Me.GroupBox_EditStore.Controls.Add(Me.Label2)
        Me.GroupBox_EditStore.Controls.Add(Me.ComboBox_TagWriterVal)
        Me.GroupBox_EditStore.Controls.Add(Me.Label1)
        Me.GroupBox_EditStore.Controls.Add(Me.ComboBox_ScaleWriterVal)
        Me.GroupBox_EditStore.Controls.Add(Me.Label_ScaleWriter)
        Me.GroupBox_EditStore.Controls.Add(Me.Label_StoreVal)
        Me.GroupBox_EditStore.Controls.Add(Me.ComboBox_ConfigTypeVal)
        Me.GroupBox_EditStore.Controls.Add(Me.ComboBox_FileWriterVal)
        Me.GroupBox_EditStore.Controls.Add(Me.ComboBox_StoreVal)
        Me.GroupBox_EditStore.Controls.Add(Me.Label_FileWriter)
        Me.GroupBox_EditStore.Controls.Add(Me.Label_ConfigType)
        Me.GroupBox_EditStore.Controls.Add(Me.Label_StoreNo)
        Me.GroupBox_EditStore.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_EditStore.Name = "GroupBox_EditStore"
        Me.GroupBox_EditStore.Size = New System.Drawing.Size(456, 229)
        Me.GroupBox_EditStore.TabIndex = 6
        Me.GroupBox_EditStore.TabStop = False
        '
        'ComboBox_TagWriterVal
        '
        Me.ComboBox_TagWriterVal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_TagWriterVal.FormattingEnabled = True
        Me.ComboBox_TagWriterVal.Location = New System.Drawing.Point(138, 106)
        Me.ComboBox_TagWriterVal.Name = "ComboBox_TagWriterVal"
        Me.ComboBox_TagWriterVal.Size = New System.Drawing.Size(133, 21)
        Me.ComboBox_TagWriterVal.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 109)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Tag Writer:"
        '
        'ComboBox_ScaleWriterVal
        '
        Me.ComboBox_ScaleWriterVal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_ScaleWriterVal.FormattingEnabled = True
        Me.ComboBox_ScaleWriterVal.Location = New System.Drawing.Point(138, 79)
        Me.ComboBox_ScaleWriterVal.Name = "ComboBox_ScaleWriterVal"
        Me.ComboBox_ScaleWriterVal.Size = New System.Drawing.Size(133, 21)
        Me.ComboBox_ScaleWriterVal.TabIndex = 9
        '
        'Label_ScaleWriter
        '
        Me.Label_ScaleWriter.AutoSize = True
        Me.Label_ScaleWriter.Location = New System.Drawing.Point(7, 82)
        Me.Label_ScaleWriter.Name = "Label_ScaleWriter"
        Me.Label_ScaleWriter.Size = New System.Drawing.Size(68, 13)
        Me.Label_ScaleWriter.TabIndex = 8
        Me.Label_ScaleWriter.Text = "Scale Writer:"
        '
        'Label_FileWriter
        '
        Me.Label_FileWriter.AutoSize = True
        Me.Label_FileWriter.Location = New System.Drawing.Point(7, 55)
        Me.Label_FileWriter.Name = "Label_FileWriter"
        Me.Label_FileWriter.Size = New System.Drawing.Size(57, 13)
        Me.Label_FileWriter.TabIndex = 3
        Me.Label_FileWriter.Text = "File Writer:"
        '
        'Label_ConfigType
        '
        Me.Label_ConfigType.AutoSize = True
        Me.Label_ConfigType.Location = New System.Drawing.Point(7, 163)
        Me.Label_ConfigType.Name = "Label_ConfigType"
        Me.Label_ConfigType.Size = New System.Drawing.Size(125, 13)
        Me.Label_ConfigType.TabIndex = 2
        Me.Label_ConfigType.Text = "Acknowledgement Type:"
        '
        'Label_StoreNo
        '
        Me.Label_StoreNo.AutoSize = True
        Me.Label_StoreNo.Location = New System.Drawing.Point(7, 22)
        Me.Label_StoreNo.Name = "Label_StoreNo"
        Me.Label_StoreNo.Size = New System.Drawing.Size(35, 13)
        Me.Label_StoreNo.TabIndex = 0
        Me.Label_StoreNo.Text = "Store:"
        '
        'ComboBox_ElectronicShelfTagVal
        '
        Me.ComboBox_ElectronicShelfTagVal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_ElectronicShelfTagVal.FormattingEnabled = True
        Me.ComboBox_ElectronicShelfTagVal.Location = New System.Drawing.Point(138, 133)
        Me.ComboBox_ElectronicShelfTagVal.Name = "ComboBox_ElectronicShelfTagVal"
        Me.ComboBox_ElectronicShelfTagVal.Size = New System.Drawing.Size(133, 21)
        Me.ComboBox_ElectronicShelfTagVal.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 136)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(106, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Electronic Shelf Tag:"
        '
        'Form_EditStorePOSConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(477, 276)
        Me.Controls.Add(Me.Button_Save)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.GroupBox_EditStore)
        Me.Name = "Form_EditStorePOSConfig"
        Me.Text = "Configure Store Writers"
        Me.GroupBox_EditStore.ResumeLayout(False)
        Me.GroupBox_EditStore.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label_StoreVal As System.Windows.Forms.Label
    Friend WithEvents ComboBox_ConfigTypeVal As System.Windows.Forms.ComboBox
    Friend WithEvents Button_Save As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents ComboBox_FileWriterVal As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox_StoreVal As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox_EditStore As System.Windows.Forms.GroupBox
    Friend WithEvents Label_FileWriter As System.Windows.Forms.Label
    Friend WithEvents Label_ConfigType As System.Windows.Forms.Label
    Friend WithEvents Label_StoreNo As System.Windows.Forms.Label
    Friend WithEvents ComboBox_ScaleWriterVal As System.Windows.Forms.ComboBox
    Friend WithEvents Label_ScaleWriter As System.Windows.Forms.Label
    Friend WithEvents ComboBox_TagWriterVal As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboBox_ElectronicShelfTagVal As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label

End Class
