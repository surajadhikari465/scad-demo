<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_AverageCostUpdateController
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
        Me.cmbStores = New System.Windows.Forms.ComboBox
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.Label_MessageExecutionText = New System.Windows.Forms.Label
        Me.cmbSubteam = New System.Windows.Forms.ComboBox
        Me.lbl_SubTeam = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me.Label_JobStatus = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmbStores
        '
        Me.cmbStores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStores.FormattingEnabled = True
        Me.cmbStores.Location = New System.Drawing.Point(80, 17)
        Me.cmbStores.Name = "cmbStores"
        Me.cmbStores.Size = New System.Drawing.Size(180, 21)
        Me.cmbStores.TabIndex = 1
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(80, 125)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(75, 23)
        Me.btnUpdate.TabIndex = 1
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(185, 125)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'Label_MessageExecutionText
        '
        Me.Label_MessageExecutionText.AutoSize = True
        Me.Label_MessageExecutionText.Location = New System.Drawing.Point(13, 220)
        Me.Label_MessageExecutionText.Name = "Label_MessageExecutionText"
        Me.Label_MessageExecutionText.Size = New System.Drawing.Size(77, 13)
        Me.Label_MessageExecutionText.TabIndex = 3
        Me.Label_MessageExecutionText.Text = "ExecutionText"
        Me.Label_MessageExecutionText.Visible = False
        '
        'cmbSubteam
        '
        Me.cmbSubteam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubteam.FormattingEnabled = True
        Me.cmbSubteam.Location = New System.Drawing.Point(80, 66)
        Me.cmbSubteam.Name = "cmbSubteam"
        Me.cmbSubteam.Size = New System.Drawing.Size(180, 21)
        Me.cmbSubteam.TabIndex = 2
        '
        'lbl_SubTeam
        '
        Me.lbl_SubTeam.BackColor = System.Drawing.Color.Transparent
        Me.lbl_SubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbl_SubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_SubTeam.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbl_SubTeam.Location = New System.Drawing.Point(12, 70)
        Me.lbl_SubTeam.Name = "lbl_SubTeam"
        Me.lbl_SubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbl_SubTeam.Size = New System.Drawing.Size(62, 17)
        Me.lbl_SubTeam.TabIndex = 7
        Me.lbl_SubTeam.Text = "Subteam :"
        Me.lbl_SubTeam.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_1.Location = New System.Drawing.Point(12, 21)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(58, 17)
        Me._lblLabel_1.TabIndex = 6
        Me._lblLabel_1.Text = "Store :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_JobStatus
        '
        Me.Label_JobStatus.AutoSize = True
        Me.Label_JobStatus.Location = New System.Drawing.Point(13, 174)
        Me.Label_JobStatus.Name = "Label_JobStatus"
        Me.Label_JobStatus.Size = New System.Drawing.Size(57, 13)
        Me.Label_JobStatus.TabIndex = 8
        Me.Label_JobStatus.Text = "JobStatus"
        '
        'Form_AverageCostUpdateController
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(577, 267)
        Me.Controls.Add(Me.Label_JobStatus)
        Me.Controls.Add(Me.lbl_SubTeam)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Controls.Add(Me.cmbSubteam)
        Me.Controls.Add(Me.Label_MessageExecutionText)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.cmbStores)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(308, 202)
        Me.Name = "Form_AverageCostUpdateController"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Average Cost Update Controller"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbStores As System.Windows.Forms.ComboBox
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label_MessageExecutionText As System.Windows.Forms.Label
    Friend WithEvents cmbSubteam As System.Windows.Forms.ComboBox
    Public WithEvents lbl_SubTeam As System.Windows.Forms.Label
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
    Friend WithEvents Label_JobStatus As System.Windows.Forms.Label
End Class
