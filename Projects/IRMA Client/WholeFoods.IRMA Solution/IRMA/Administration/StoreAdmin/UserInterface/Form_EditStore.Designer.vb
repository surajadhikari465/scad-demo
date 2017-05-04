<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_EditStore
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
        Me.Button_FTPInfo = New System.Windows.Forms.Button
        Me.Button_ConfigureWriters = New System.Windows.Forms.Button
        Me.Label_StoreNo = New System.Windows.Forms.Label
        Me.Label_StoreName = New System.Windows.Forms.Label
        Me.GroupBox_ConfigStoreData = New System.Windows.Forms.GroupBox
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.GroupBox_StoreInfo = New System.Windows.Forms.GroupBox
        Me.ComboBox_POSSystem = New System.Windows.Forms.ComboBox
        Me.Label_POSSystemType = New System.Windows.Forms.Label
        Me.Label_StoreNameValue = New System.Windows.Forms.Label
        Me.Label_StoreNoValue = New System.Windows.Forms.Label
        Me.Button_Save = New System.Windows.Forms.Button
        Me.GroupBox_ConfigStoreData.SuspendLayout()
        Me.GroupBox_StoreInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_FTPInfo
        '
        Me.Button_FTPInfo.Location = New System.Drawing.Point(35, 57)
        Me.Button_FTPInfo.Name = "Button_FTPInfo"
        Me.Button_FTPInfo.Size = New System.Drawing.Size(127, 23)
        Me.Button_FTPInfo.TabIndex = 0
        Me.Button_FTPInfo.Text = "FTP Info"
        Me.Button_FTPInfo.UseVisualStyleBackColor = True
        '
        'Button_ConfigureWriters
        '
        Me.Button_ConfigureWriters.Location = New System.Drawing.Point(35, 28)
        Me.Button_ConfigureWriters.Name = "Button_ConfigureWriters"
        Me.Button_ConfigureWriters.Size = New System.Drawing.Size(127, 23)
        Me.Button_ConfigureWriters.TabIndex = 1
        Me.Button_ConfigureWriters.Text = "Configure Writers"
        Me.Button_ConfigureWriters.UseVisualStyleBackColor = True
        '
        'Label_StoreNo
        '
        Me.Label_StoreNo.AutoSize = True
        Me.Label_StoreNo.Location = New System.Drawing.Point(23, 29)
        Me.Label_StoreNo.Name = "Label_StoreNo"
        Me.Label_StoreNo.Size = New System.Drawing.Size(55, 13)
        Me.Label_StoreNo.TabIndex = 2
        Me.Label_StoreNo.Text = "Store No:"
        '
        'Label_StoreName
        '
        Me.Label_StoreName.AutoSize = True
        Me.Label_StoreName.Location = New System.Drawing.Point(23, 55)
        Me.Label_StoreName.Name = "Label_StoreName"
        Me.Label_StoreName.Size = New System.Drawing.Size(69, 13)
        Me.Label_StoreName.TabIndex = 4
        Me.Label_StoreName.Text = "Store Name:"
        '
        'GroupBox_ConfigStoreData
        '
        Me.GroupBox_ConfigStoreData.Controls.Add(Me.Button_ConfigureWriters)
        Me.GroupBox_ConfigStoreData.Controls.Add(Me.Button_FTPInfo)
        Me.GroupBox_ConfigStoreData.Location = New System.Drawing.Point(26, 116)
        Me.GroupBox_ConfigStoreData.Name = "GroupBox_ConfigStoreData"
        Me.GroupBox_ConfigStoreData.Size = New System.Drawing.Size(200, 100)
        Me.GroupBox_ConfigStoreData.TabIndex = 6
        Me.GroupBox_ConfigStoreData.TabStop = False
        Me.GroupBox_ConfigStoreData.Text = "Configure Store Data"
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(248, 245)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 9
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'GroupBox_StoreInfo
        '
        Me.GroupBox_StoreInfo.Controls.Add(Me.ComboBox_POSSystem)
        Me.GroupBox_StoreInfo.Controls.Add(Me.Label_POSSystemType)
        Me.GroupBox_StoreInfo.Controls.Add(Me.Label_StoreNameValue)
        Me.GroupBox_StoreInfo.Controls.Add(Me.Label_StoreNoValue)
        Me.GroupBox_StoreInfo.Controls.Add(Me.Label_StoreNo)
        Me.GroupBox_StoreInfo.Controls.Add(Me.GroupBox_ConfigStoreData)
        Me.GroupBox_StoreInfo.Controls.Add(Me.Label_StoreName)
        Me.GroupBox_StoreInfo.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_StoreInfo.Name = "GroupBox_StoreInfo"
        Me.GroupBox_StoreInfo.Size = New System.Drawing.Size(392, 227)
        Me.GroupBox_StoreInfo.TabIndex = 10
        Me.GroupBox_StoreInfo.TabStop = False
        Me.GroupBox_StoreInfo.Text = "Store Info"
        '
        'ComboBox_POSSystem
        '
        Me.ComboBox_POSSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_POSSystem.FormattingEnabled = True
        Me.ComboBox_POSSystem.Location = New System.Drawing.Point(117, 80)
        Me.ComboBox_POSSystem.Name = "ComboBox_POSSystem"
        Me.ComboBox_POSSystem.Size = New System.Drawing.Size(137, 21)
        Me.ComboBox_POSSystem.TabIndex = 10
        '
        'Label_POSSystemType
        '
        Me.Label_POSSystemType.AutoSize = True
        Me.Label_POSSystemType.Location = New System.Drawing.Point(23, 83)
        Me.Label_POSSystemType.Name = "Label_POSSystemType"
        Me.Label_POSSystemType.Size = New System.Drawing.Size(69, 13)
        Me.Label_POSSystemType.TabIndex = 9
        Me.Label_POSSystemType.Text = "POS System:"
        '
        'Label_StoreNameValue
        '
        Me.Label_StoreNameValue.AutoSize = True
        Me.Label_StoreNameValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_StoreNameValue.Location = New System.Drawing.Point(114, 55)
        Me.Label_StoreNameValue.Name = "Label_StoreNameValue"
        Me.Label_StoreNameValue.Size = New System.Drawing.Size(45, 13)
        Me.Label_StoreNameValue.TabIndex = 8
        Me.Label_StoreNameValue.Text = "Label1"
        '
        'Label_StoreNoValue
        '
        Me.Label_StoreNoValue.AutoSize = True
        Me.Label_StoreNoValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_StoreNoValue.Location = New System.Drawing.Point(114, 29)
        Me.Label_StoreNoValue.Name = "Label_StoreNoValue"
        Me.Label_StoreNoValue.Size = New System.Drawing.Size(45, 13)
        Me.Label_StoreNoValue.TabIndex = 7
        Me.Label_StoreNoValue.Text = "Label1"
        '
        'Button_Save
        '
        Me.Button_Save.Location = New System.Drawing.Point(329, 245)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(75, 23)
        Me.Button_Save.TabIndex = 11
        Me.Button_Save.Text = "Save"
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Form_EditStore
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(418, 278)
        Me.Controls.Add(Me.Button_Save)
        Me.Controls.Add(Me.GroupBox_StoreInfo)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_EditStore"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Edit Store"
        Me.GroupBox_ConfigStoreData.ResumeLayout(False)
        Me.GroupBox_StoreInfo.ResumeLayout(False)
        Me.GroupBox_StoreInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_FTPInfo As System.Windows.Forms.Button
    Friend WithEvents Button_ConfigureWriters As System.Windows.Forms.Button
    Friend WithEvents Label_StoreNo As System.Windows.Forms.Label
    Friend WithEvents Label_StoreName As System.Windows.Forms.Label
    Friend WithEvents GroupBox_ConfigStoreData As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents GroupBox_StoreInfo As System.Windows.Forms.GroupBox
    Friend WithEvents Label_StoreNameValue As System.Windows.Forms.Label
    Friend WithEvents Label_StoreNoValue As System.Windows.Forms.Label
    Friend WithEvents ComboBox_POSSystem As System.Windows.Forms.ComboBox
    Friend WithEvents Label_POSSystemType As System.Windows.Forms.Label
    Friend WithEvents Button_Save As System.Windows.Forms.Button
End Class
