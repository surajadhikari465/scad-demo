<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_DeletePOSWriterFileConfig
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
        Me.Label_ColumnNoVal = New System.Windows.Forms.Label
        Me.Label_POSChangeTypeVal = New System.Windows.Forms.Label
        Me.Label_POSWriterVal = New System.Windows.Forms.Label
        Me.Label_ColumnNo = New System.Windows.Forms.Label
        Me.Label_POSChangeType = New System.Windows.Forms.Label
        Me.Label_POSWriter = New System.Windows.Forms.Label
        Me.Label_DataElement = New System.Windows.Forms.Label
        Me.Label_DataElementVal = New System.Windows.Forms.Label
        Me.Label_FieldID = New System.Windows.Forms.Label
        Me.Label_FieldIDVal = New System.Windows.Forms.Label
        Me.Label_RowNoVal = New System.Windows.Forms.Label
        Me.Label_RowNo = New System.Windows.Forms.Label
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
        Me.Label_Warning.Location = New System.Drawing.Point(61, 9)
        Me.Label_Warning.Size = New System.Drawing.Size(385, 17)
        Me.Label_Warning.Text = "Warning!  You are about to delete a POS writer detail record."
        '
        'GroupBox_DeleteData
        '
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_RowNoVal)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_RowNo)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_FieldIDVal)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_FieldID)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_DataElementVal)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_DataElement)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_ColumnNoVal)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_POSChangeTypeVal)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_POSWriterVal)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_ColumnNo)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_POSChangeType)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_POSWriter)
        Me.GroupBox_DeleteData.Size = New System.Drawing.Size(550, 162)
        '
        'Label_ColumnNoVal
        '
        Me.Label_ColumnNoVal.AutoSize = True
        Me.Label_ColumnNoVal.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ColumnNoVal.Location = New System.Drawing.Point(170, 85)
        Me.Label_ColumnNoVal.Name = "Label_ColumnNoVal"
        Me.Label_ColumnNoVal.Size = New System.Drawing.Size(41, 13)
        Me.Label_ColumnNoVal.TabIndex = 38
        Me.Label_ColumnNoVal.Text = "Label1"
        '
        'Label_POSChangeTypeVal
        '
        Me.Label_POSChangeTypeVal.AutoSize = True
        Me.Label_POSChangeTypeVal.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_POSChangeTypeVal.Location = New System.Drawing.Point(170, 41)
        Me.Label_POSChangeTypeVal.Name = "Label_POSChangeTypeVal"
        Me.Label_POSChangeTypeVal.Size = New System.Drawing.Size(41, 13)
        Me.Label_POSChangeTypeVal.TabIndex = 37
        Me.Label_POSChangeTypeVal.Text = "Label1"
        '
        'Label_POSWriterVal
        '
        Me.Label_POSWriterVal.AutoSize = True
        Me.Label_POSWriterVal.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_POSWriterVal.Location = New System.Drawing.Point(170, 19)
        Me.Label_POSWriterVal.Name = "Label_POSWriterVal"
        Me.Label_POSWriterVal.Size = New System.Drawing.Size(41, 13)
        Me.Label_POSWriterVal.TabIndex = 36
        Me.Label_POSWriterVal.Text = "Label1"
        '
        'Label_ColumnNo
        '
        Me.Label_ColumnNo.AutoSize = True
        Me.Label_ColumnNo.Location = New System.Drawing.Point(49, 85)
        Me.Label_ColumnNo.Name = "Label_ColumnNo"
        Me.Label_ColumnNo.Size = New System.Drawing.Size(68, 13)
        Me.Label_ColumnNo.TabIndex = 35
        Me.Label_ColumnNo.Text = "Column No:"
        '
        'Label_POSChangeType
        '
        Me.Label_POSChangeType.AutoSize = True
        Me.Label_POSChangeType.Location = New System.Drawing.Point(49, 41)
        Me.Label_POSChangeType.Name = "Label_POSChangeType"
        Me.Label_POSChangeType.Size = New System.Drawing.Size(100, 13)
        Me.Label_POSChangeType.TabIndex = 34
        Me.Label_POSChangeType.Text = "POS Change Type:"
        '
        'Label_POSWriter
        '
        Me.Label_POSWriter.AutoSize = True
        Me.Label_POSWriter.Location = New System.Drawing.Point(49, 19)
        Me.Label_POSWriter.Name = "Label_POSWriter"
        Me.Label_POSWriter.Size = New System.Drawing.Size(66, 13)
        Me.Label_POSWriter.TabIndex = 33
        Me.Label_POSWriter.Text = "POS Writer:"
        '
        'Label_DataElement
        '
        Me.Label_DataElement.AutoSize = True
        Me.Label_DataElement.Location = New System.Drawing.Point(49, 107)
        Me.Label_DataElement.Name = "Label_DataElement"
        Me.Label_DataElement.Size = New System.Drawing.Size(78, 13)
        Me.Label_DataElement.TabIndex = 39
        Me.Label_DataElement.Text = "Data Element:"
        '
        'Label_DataElementVal
        '
        Me.Label_DataElementVal.AutoSize = True
        Me.Label_DataElementVal.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_DataElementVal.Location = New System.Drawing.Point(170, 107)
        Me.Label_DataElementVal.Name = "Label_DataElementVal"
        Me.Label_DataElementVal.Size = New System.Drawing.Size(41, 13)
        Me.Label_DataElementVal.TabIndex = 40
        Me.Label_DataElementVal.Text = "Label2"
        '
        'Label_FieldID
        '
        Me.Label_FieldID.AutoSize = True
        Me.Label_FieldID.Location = New System.Drawing.Point(49, 129)
        Me.Label_FieldID.Name = "Label_FieldID"
        Me.Label_FieldID.Size = New System.Drawing.Size(49, 13)
        Me.Label_FieldID.TabIndex = 41
        Me.Label_FieldID.Text = "Field ID:"
        '
        'Label_FieldIDVal
        '
        Me.Label_FieldIDVal.AutoSize = True
        Me.Label_FieldIDVal.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_FieldIDVal.Location = New System.Drawing.Point(170, 129)
        Me.Label_FieldIDVal.Name = "Label_FieldIDVal"
        Me.Label_FieldIDVal.Size = New System.Drawing.Size(41, 13)
        Me.Label_FieldIDVal.TabIndex = 42
        Me.Label_FieldIDVal.Text = "Label4"
        '
        'Label_RowNoVal
        '
        Me.Label_RowNoVal.AutoSize = True
        Me.Label_RowNoVal.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_RowNoVal.Location = New System.Drawing.Point(170, 63)
        Me.Label_RowNoVal.Name = "Label_RowNoVal"
        Me.Label_RowNoVal.Size = New System.Drawing.Size(41, 13)
        Me.Label_RowNoVal.TabIndex = 44
        Me.Label_RowNoVal.Text = "Label2"
        '
        'Label_RowNo
        '
        Me.Label_RowNo.AutoSize = True
        Me.Label_RowNo.Location = New System.Drawing.Point(49, 63)
        Me.Label_RowNo.Name = "Label_RowNo"
        Me.Label_RowNo.Size = New System.Drawing.Size(51, 13)
        Me.Label_RowNo.TabIndex = 43
        Me.Label_RowNo.Text = "Row No:"
        '
        'Form_DeletePOSWriterFileConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(576, 266)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_DeletePOSWriterFileConfig"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Delete Writer Detail"
        Me.Panel_StandardButtons.ResumeLayout(False)
        Me.Panel_Instructions.ResumeLayout(False)
        Me.Panel_Instructions.PerformLayout()
        Me.GroupBox_DeleteData.ResumeLayout(False)
        Me.GroupBox_DeleteData.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label_FieldIDVal As System.Windows.Forms.Label
    Friend WithEvents Label_FieldID As System.Windows.Forms.Label
    Friend WithEvents Label_DataElementVal As System.Windows.Forms.Label
    Friend WithEvents Label_DataElement As System.Windows.Forms.Label
    Friend WithEvents Label_ColumnNoVal As System.Windows.Forms.Label
    Friend WithEvents Label_POSChangeTypeVal As System.Windows.Forms.Label
    Friend WithEvents Label_POSWriterVal As System.Windows.Forms.Label
    Friend WithEvents Label_ColumnNo As System.Windows.Forms.Label
    Friend WithEvents Label_POSChangeType As System.Windows.Forms.Label
    Friend WithEvents Label_POSWriter As System.Windows.Forms.Label
    Friend WithEvents Label_RowNoVal As System.Windows.Forms.Label
    Friend WithEvents Label_RowNo As System.Windows.Forms.Label

End Class
