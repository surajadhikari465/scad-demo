<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NoTagLogicConfiguration
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Me.NumericUpDownMovementHistory = New System.Windows.Forms.NumericUpDown()
		Me.NumericUpDownReceivingHistory = New System.Windows.Forms.NumericUpDown()
		Me.NumericUpDownOrderingHistory = New System.Windows.Forms.NumericUpDown()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.GroupBoxGlobalThreshold = New System.Windows.Forms.GroupBox()
		Me.ButtonOK = New System.Windows.Forms.Button()
		Me.ButtonCancel = New System.Windows.Forms.Button()
		Me.GroupBoxSubteamOverride = New System.Windows.Forms.GroupBox()
		Me.NumericUpDownSubteamOverride = New System.Windows.Forms.NumericUpDown()
		Me.LabelSubteamOverrideThreshold = New System.Windows.Forms.Label()
		Me.LabelSubteams = New System.Windows.Forms.Label()
		Me.cmbSubTeam = New SubteamComboBox()
		Me.ButtonReset = New System.Windows.Forms.Button()
		CType(Me.NumericUpDownMovementHistory, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.NumericUpDownReceivingHistory, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.NumericUpDownOrderingHistory, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBoxGlobalThreshold.SuspendLayout()
		Me.GroupBoxSubteamOverride.SuspendLayout()
		CType(Me.NumericUpDownSubteamOverride, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'NumericUpDownMovementHistory
		'
		Me.NumericUpDownMovementHistory.Location = New System.Drawing.Point(114, 41)
		Me.NumericUpDownMovementHistory.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
		Me.NumericUpDownMovementHistory.Name = "NumericUpDownMovementHistory"
		Me.NumericUpDownMovementHistory.Size = New System.Drawing.Size(56, 20)
		Me.NumericUpDownMovementHistory.TabIndex = 0
		'
		'NumericUpDownReceivingHistory
		'
		Me.NumericUpDownReceivingHistory.Location = New System.Drawing.Point(457, 41)
		Me.NumericUpDownReceivingHistory.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
		Me.NumericUpDownReceivingHistory.Name = "NumericUpDownReceivingHistory"
		Me.NumericUpDownReceivingHistory.Size = New System.Drawing.Size(56, 20)
		Me.NumericUpDownReceivingHistory.TabIndex = 2
		'
		'NumericUpDownOrderingHistory
		'
		Me.NumericUpDownOrderingHistory.Location = New System.Drawing.Point(282, 41)
		Me.NumericUpDownOrderingHistory.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
		Me.NumericUpDownOrderingHistory.Name = "NumericUpDownOrderingHistory"
		Me.NumericUpDownOrderingHistory.Size = New System.Drawing.Size(56, 20)
		Me.NumericUpDownOrderingHistory.TabIndex = 1
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(16, 43)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(92, 13)
		Me.Label1.TabIndex = 4
		Me.Label1.Text = "Movement History"
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(361, 43)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(90, 13)
		Me.Label2.TabIndex = 5
		Me.Label2.Text = "Receiving History"
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(194, 43)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(82, 13)
		Me.Label3.TabIndex = 6
		Me.Label3.Text = "Ordering History"
		'
		'GroupBoxGlobalThreshold
		'
		Me.GroupBoxGlobalThreshold.Controls.Add(Me.Label1)
		Me.GroupBoxGlobalThreshold.Controls.Add(Me.Label3)
		Me.GroupBoxGlobalThreshold.Controls.Add(Me.NumericUpDownMovementHistory)
		Me.GroupBoxGlobalThreshold.Controls.Add(Me.Label2)
		Me.GroupBoxGlobalThreshold.Controls.Add(Me.NumericUpDownReceivingHistory)
		Me.GroupBoxGlobalThreshold.Controls.Add(Me.NumericUpDownOrderingHistory)
		Me.GroupBoxGlobalThreshold.Location = New System.Drawing.Point(36, 29)
		Me.GroupBoxGlobalThreshold.Name = "GroupBoxGlobalThreshold"
		Me.GroupBoxGlobalThreshold.Size = New System.Drawing.Size(532, 103)
		Me.GroupBoxGlobalThreshold.TabIndex = 0
		Me.GroupBoxGlobalThreshold.TabStop = False
		Me.GroupBoxGlobalThreshold.Text = "No-Tag History Threshold Values (Days)"
		'
		'ButtonOK
		'
		Me.ButtonOK.Location = New System.Drawing.Point(412, 294)
		Me.ButtonOK.Name = "ButtonOK"
		Me.ButtonOK.Size = New System.Drawing.Size(75, 23)
		Me.ButtonOK.TabIndex = 2
		Me.ButtonOK.Text = "OK"
		Me.ButtonOK.UseVisualStyleBackColor = True
		'
		'ButtonCancel
		'
		Me.ButtonCancel.Location = New System.Drawing.Point(493, 294)
		Me.ButtonCancel.Name = "ButtonCancel"
		Me.ButtonCancel.Size = New System.Drawing.Size(75, 23)
		Me.ButtonCancel.TabIndex = 3
		Me.ButtonCancel.Text = "Cancel"
		Me.ButtonCancel.UseVisualStyleBackColor = True
		'
		'GroupBoxSubteamOverride
		'
		Me.GroupBoxSubteamOverride.Controls.Add(Me.NumericUpDownSubteamOverride)
		Me.GroupBoxSubteamOverride.Controls.Add(Me.LabelSubteamOverrideThreshold)
		Me.GroupBoxSubteamOverride.Controls.Add(Me.LabelSubteams)
		Me.GroupBoxSubteamOverride.Controls.Add(Me.cmbSubTeam)
		Me.GroupBoxSubteamOverride.Location = New System.Drawing.Point(36, 149)
		Me.GroupBoxSubteamOverride.Name = "GroupBoxSubteamOverride"
		Me.GroupBoxSubteamOverride.Size = New System.Drawing.Size(532, 116)
		Me.GroupBoxSubteamOverride.TabIndex = 1
		Me.GroupBoxSubteamOverride.TabStop = False
		Me.GroupBoxSubteamOverride.Text = "Subteam Override Values (Days)"
		'
		'NumericUpDownSubteamOverride
		'
		Me.NumericUpDownSubteamOverride.Location = New System.Drawing.Point(450, 48)
		Me.NumericUpDownSubteamOverride.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
		Me.NumericUpDownSubteamOverride.Name = "NumericUpDownSubteamOverride"
		Me.NumericUpDownSubteamOverride.Size = New System.Drawing.Size(56, 20)
		Me.NumericUpDownSubteamOverride.TabIndex = 1
		'
		'LabelSubteamOverrideThreshold
		'
		Me.LabelSubteamOverrideThreshold.AutoSize = True
		Me.LabelSubteamOverrideThreshold.Location = New System.Drawing.Point(390, 50)
		Me.LabelSubteamOverrideThreshold.Name = "LabelSubteamOverrideThreshold"
		Me.LabelSubteamOverrideThreshold.Size = New System.Drawing.Size(54, 13)
		Me.LabelSubteamOverrideThreshold.TabIndex = 2
		Me.LabelSubteamOverrideThreshold.Text = "Threshold"
		'
		'LabelSubteams
		'
		Me.LabelSubteams.AutoSize = True
		Me.LabelSubteams.Location = New System.Drawing.Point(21, 50)
		Me.LabelSubteams.Name = "LabelSubteams"
		Me.LabelSubteams.Size = New System.Drawing.Size(49, 13)
		Me.LabelSubteams.TabIndex = 1
		Me.LabelSubteams.Text = "Subteam"
		'
		'cmbSubTeam
		'
		Me.cmbSubTeam.AutoSize = True
		Me.cmbSubTeam.CheckboxFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmbSubTeam.CheckboxForeColor = System.Drawing.SystemColors.ControlText
		Me.cmbSubTeam.CheckboxText = "Show All"
		Me.cmbSubTeam.ClearSelectionVisisble = False
		Me.cmbSubTeam.DataSource = Nothing
		Me.cmbSubTeam.DisplayMember = Nothing
		Me.cmbSubTeam.DropDownWidth = 212
		Me.cmbSubTeam.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmbSubTeam.HeaderForeColor = System.Drawing.SystemColors.ControlText
		Me.cmbSubTeam.HeaderText = "Caption"
		Me.cmbSubTeam.HeaderVisible = False
		Me.cmbSubTeam.IsShowAll = False
		Me.cmbSubTeam.Location = New System.Drawing.Point(76, 47)
		Me.cmbSubTeam.MinimumSize = New System.Drawing.Size(25, 0)
		Me.cmbSubTeam.Name = "cmbSubTeam"
		Me.cmbSubTeam.SelectedIndex = -1
		Me.cmbSubTeam.SelectedItem = Nothing
		Me.cmbSubTeam.SelectedText = ""
		Me.cmbSubTeam.SelectedValue = Nothing
		Me.cmbSubTeam.Size = New System.Drawing.Size(284, 21)
		Me.cmbSubTeam.TabIndex = 0
		Me.cmbSubTeam.ValueMember = Nothing
		'
		'ButtonReset
		'
		Me.ButtonReset.Location = New System.Drawing.Point(36, 294)
		Me.ButtonReset.Name = "ButtonReset"
		Me.ButtonReset.Size = New System.Drawing.Size(100, 23)
		Me.ButtonReset.TabIndex = 4
		Me.ButtonReset.Text = "Reset to Default"
		Me.ButtonReset.UseVisualStyleBackColor = True
		'
		'NoTagLogicConfiguration
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(605, 346)
		Me.Controls.Add(Me.ButtonReset)
		Me.Controls.Add(Me.GroupBoxSubteamOverride)
		Me.Controls.Add(Me.ButtonCancel)
		Me.Controls.Add(Me.ButtonOK)
		Me.Controls.Add(Me.GroupBoxGlobalThreshold)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "NoTagLogicConfiguration"
		Me.ShowIcon = False
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "No-Tag Logic Configuration"
		CType(Me.NumericUpDownMovementHistory, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.NumericUpDownReceivingHistory, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.NumericUpDownOrderingHistory, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBoxGlobalThreshold.ResumeLayout(False)
		Me.GroupBoxGlobalThreshold.PerformLayout()
		Me.GroupBoxSubteamOverride.ResumeLayout(False)
		Me.GroupBoxSubteamOverride.PerformLayout()
		CType(Me.NumericUpDownSubteamOverride, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents NumericUpDownMovementHistory As NumericUpDown
	Friend WithEvents NumericUpDownReceivingHistory As NumericUpDown
	Friend WithEvents NumericUpDownOrderingHistory As NumericUpDown
	Friend WithEvents Label1 As Label
	Friend WithEvents Label2 As Label
	Friend WithEvents Label3 As Label
	Friend WithEvents GroupBoxGlobalThreshold As GroupBox
	Friend WithEvents ButtonOK As Button
	Friend WithEvents ButtonCancel As Button
	Friend WithEvents GroupBoxSubteamOverride As GroupBox
	Friend WithEvents LabelSubteams As Label
	Friend WithEvents cmbSubTeam As SubteamComboBox
	Friend WithEvents LabelSubteamOverrideThreshold As Label
    Friend WithEvents NumericUpDownSubteamOverride As NumericUpDown
    Friend WithEvents ButtonReset As Button
End Class
