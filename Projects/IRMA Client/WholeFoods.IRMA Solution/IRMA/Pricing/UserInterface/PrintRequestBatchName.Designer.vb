<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PrintRequestBatchName
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.LabelChooseName = New System.Windows.Forms.Label()
        Me.TextBoxBatchName = New System.Windows.Forms.TextBox()
        Me.ButtonOK = New System.Windows.Forms.Button()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.LabelNote1 = New System.Windows.Forms.Label()
        Me.LabelNote2 = New System.Windows.Forms.Label()
        Me.CheckBoxSendToAllStores = New System.Windows.Forms.CheckBox()
        Me.CheckBoxApplyNoTagLogic = New System.Windows.Forms.CheckBox()
        Me.LabelNote3 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'LabelChooseName
        '
        Me.LabelChooseName.AutoSize = True
        Me.LabelChooseName.Location = New System.Drawing.Point(8, 139)
        Me.LabelChooseName.Name = "LabelChooseName"
        Me.LabelChooseName.Size = New System.Drawing.Size(243, 13)
        Me.LabelChooseName.TabIndex = 0
        Me.LabelChooseName.Text = "Please choose a batch name for this print request:"
        '
        'TextBoxBatchName
        '
        Me.TextBoxBatchName.Location = New System.Drawing.Point(11, 155)
        Me.TextBoxBatchName.Name = "TextBoxBatchName"
        Me.TextBoxBatchName.Size = New System.Drawing.Size(240, 20)
        Me.TextBoxBatchName.TabIndex = 1
        '
        'ButtonOK
        '
        Me.ButtonOK.Location = New System.Drawing.Point(95, 200)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOK.TabIndex = 2
        Me.ButtonOK.Text = "OK"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Location = New System.Drawing.Point(176, 200)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancel.TabIndex = 3
        Me.ButtonCancel.Text = "Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'LabelNote1
        '
        Me.LabelNote1.AutoSize = True
        Me.LabelNote1.Location = New System.Drawing.Point(12, 18)
        Me.LabelNote1.Name = "LabelNote1"
        Me.LabelNote1.Size = New System.Drawing.Size(227, 13)
        Me.LabelNote1.TabIndex = 4
        Me.LabelNote1.Text = "Items may be excluded from the request due to"
        '
        'LabelNote2
        '
        Me.LabelNote2.AutoSize = True
        Me.LabelNote2.Location = New System.Drawing.Point(12, 31)
        Me.LabelNote2.Name = "LabelNote2"
        Me.LabelNote2.Size = New System.Drawing.Size(205, 13)
        Me.LabelNote2.TabIndex = 6
        Me.LabelNote2.Text = "authorization, validation, deleted status, or"
        '
        'CheckBoxSendToAllStores
        '
        Me.CheckBoxSendToAllStores.AutoSize = True
        Me.CheckBoxSendToAllStores.Enabled = False
        Me.CheckBoxSendToAllStores.Location = New System.Drawing.Point(15, 72)
        Me.CheckBoxSendToAllStores.Name = "CheckBoxSendToAllStores"
        Me.CheckBoxSendToAllStores.Size = New System.Drawing.Size(110, 17)
        Me.CheckBoxSendToAllStores.TabIndex = 7
        Me.CheckBoxSendToAllStores.Text = "Send to All Stores"
        Me.CheckBoxSendToAllStores.UseVisualStyleBackColor = True
        '
        'CheckBoxApplyNoTagLogic
        '
        Me.CheckBoxApplyNoTagLogic.AutoSize = True
        Me.CheckBoxApplyNoTagLogic.Enabled = False
        Me.CheckBoxApplyNoTagLogic.Location = New System.Drawing.Point(15, 95)
        Me.CheckBoxApplyNoTagLogic.Name = "CheckBoxApplyNoTagLogic"
        Me.CheckBoxApplyNoTagLogic.Size = New System.Drawing.Size(120, 17)
        Me.CheckBoxApplyNoTagLogic.TabIndex = 8
        Me.CheckBoxApplyNoTagLogic.Text = "Apply No-Tag Logic"
        Me.CheckBoxApplyNoTagLogic.UseVisualStyleBackColor = True
        '
        'LabelNote3
        '
        Me.LabelNote3.AutoSize = True
        Me.LabelNote3.Location = New System.Drawing.Point(13, 44)
        Me.LabelNote3.Name = "LabelNote3"
        Me.LabelNote3.Size = New System.Drawing.Size(122, 13)
        Me.LabelNote3.TabIndex = 9
        Me.LabelNote3.Text = "no-tag logic (if selected)."
        '
        'PrintRequestBatchName
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(263, 242)
        Me.Controls.Add(Me.LabelNote3)
        Me.Controls.Add(Me.CheckBoxApplyNoTagLogic)
        Me.Controls.Add(Me.CheckBoxSendToAllStores)
        Me.Controls.Add(Me.LabelNote2)
        Me.Controls.Add(Me.LabelNote1)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonOK)
        Me.Controls.Add(Me.TextBoxBatchName)
        Me.Controls.Add(Me.LabelChooseName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PrintRequestBatchName"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Print Request Batch Name"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LabelChooseName As Label
    Friend WithEvents TextBoxBatchName As TextBox
    Friend WithEvents ButtonOK As Button
    Friend WithEvents ButtonCancel As Button
    Friend WithEvents LabelNote1 As Label
    Friend WithEvents LabelNote2 As Label
    Friend WithEvents CheckBoxSendToAllStores As CheckBox
    Friend WithEvents CheckBoxApplyNoTagLogic As CheckBox
    Friend WithEvents LabelNote3 As Label
End Class
