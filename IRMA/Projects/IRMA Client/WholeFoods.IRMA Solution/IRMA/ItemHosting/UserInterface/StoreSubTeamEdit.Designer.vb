<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StoreSubTeamEdit
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
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(StoreSubTeamEdit))
		Me.Label_Store = New System.Windows.Forms.Label()
		Me.Label_SubTeam = New System.Windows.Forms.Label()
		Me.ComboBox_Team = New System.Windows.Forms.ComboBox()
		Me.Label_Team = New System.Windows.Forms.Label()
		Me.lblStoreName = New System.Windows.Forms.Label()
		Me.Button_Save = New System.Windows.Forms.Button()
		Me.Button_Cancel = New System.Windows.Forms.Button()
		Me.Label_PSSubTeam = New System.Windows.Forms.Label()
		Me.Label_PSTeam = New System.Windows.Forms.Label()
		Me.TextBox_PSSubTeamNo = New System.Windows.Forms.TextBox()
		Me.TextBox_PSTeamNo = New System.Windows.Forms.TextBox()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.Label_CostFactor = New System.Windows.Forms.Label()
		Me.Label_ICVID = New System.Windows.Forms.Label()
		Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
		Me.TextBox_CostFactor = New System.Windows.Forms.TextBox()
		Me.RichTextBox2 = New System.Windows.Forms.RichTextBox()
		Me.ComboBox_ICVID = New System.Windows.Forms.ComboBox()
		Me.cmbSubTeam = New SubteamComboBox()
		Me.SuspendLayout()
		'
		'Label_Store
		'
		Me.Label_Store.AutoSize = True
		Me.Label_Store.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label_Store.Location = New System.Drawing.Point(50, 9)
		Me.Label_Store.Name = "Label_Store"
		Me.Label_Store.Size = New System.Drawing.Size(41, 13)
		Me.Label_Store.TabIndex = 0
		Me.Label_Store.Text = "Store:"
		'
		'Label_SubTeam
		'
		Me.Label_SubTeam.AutoSize = True
		Me.Label_SubTeam.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label_SubTeam.Location = New System.Drawing.Point(28, 34)
		Me.Label_SubTeam.Name = "Label_SubTeam"
		Me.Label_SubTeam.Size = New System.Drawing.Size(64, 13)
		Me.Label_SubTeam.TabIndex = 1
		Me.Label_SubTeam.Text = "SubTeam:"
		'
		'ComboBox_Team
		'
		Me.ComboBox_Team.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.ComboBox_Team.FormattingEnabled = True
		Me.ComboBox_Team.Location = New System.Drawing.Point(98, 56)
		Me.ComboBox_Team.Name = "ComboBox_Team"
		Me.ComboBox_Team.Size = New System.Drawing.Size(210, 21)
		Me.ComboBox_Team.TabIndex = 2
		'
		'Label_Team
		'
		Me.Label_Team.AutoSize = True
		Me.Label_Team.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label_Team.Location = New System.Drawing.Point(50, 59)
		Me.Label_Team.Name = "Label_Team"
		Me.Label_Team.Size = New System.Drawing.Size(42, 13)
		Me.Label_Team.TabIndex = 3
		Me.Label_Team.Text = "Team:"
		'
		'lblStoreName
		'
		Me.lblStoreName.AutoSize = True
		Me.lblStoreName.Location = New System.Drawing.Point(97, 9)
		Me.lblStoreName.Name = "lblStoreName"
		Me.lblStoreName.Size = New System.Drawing.Size(0, 13)
		Me.lblStoreName.TabIndex = 4
		'
		'Button_Save
		'
		Me.Button_Save.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Button_Save.AutoSize = True
		Me.Button_Save.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Button_Save.Location = New System.Drawing.Point(575, 266)
		Me.Button_Save.Name = "Button_Save"
		Me.Button_Save.Size = New System.Drawing.Size(42, 23)
		Me.Button_Save.TabIndex = 6
		Me.Button_Save.Text = "&Save"
		Me.Button_Save.UseVisualStyleBackColor = True
		'
		'Button_Cancel
		'
		Me.Button_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Button_Cancel.AutoSize = True
		Me.Button_Cancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.Button_Cancel.Location = New System.Drawing.Point(623, 266)
		Me.Button_Cancel.Name = "Button_Cancel"
		Me.Button_Cancel.Size = New System.Drawing.Size(50, 23)
		Me.Button_Cancel.TabIndex = 7
		Me.Button_Cancel.Text = "&Cancel"
		Me.Button_Cancel.UseVisualStyleBackColor = True
		'
		'Label_PSSubTeam
		'
		Me.Label_PSSubTeam.AutoSize = True
		Me.Label_PSSubTeam.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label_PSSubTeam.Location = New System.Drawing.Point(39, 98)
		Me.Label_PSSubTeam.Name = "Label_PSSubTeam"
		Me.Label_PSSubTeam.Size = New System.Drawing.Size(108, 13)
		Me.Label_PSSubTeam.TabIndex = 9
		Me.Label_PSSubTeam.Text = "PS Sub Team No:"
		Me.ToolTip1.SetToolTip(Me.Label_PSSubTeam, "People Soft Sub Team (Product)")
		'
		'Label_PSTeam
		'
		Me.Label_PSTeam.AutoSize = True
		Me.Label_PSTeam.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label_PSTeam.Location = New System.Drawing.Point(65, 123)
		Me.Label_PSTeam.Name = "Label_PSTeam"
		Me.Label_PSTeam.Size = New System.Drawing.Size(82, 13)
		Me.Label_PSTeam.TabIndex = 10
		Me.Label_PSTeam.Text = "PS Team No:"
		Me.ToolTip1.SetToolTip(Me.Label_PSTeam, "People Soft Team (Department)")
		'
		'TextBox_PSSubTeamNo
		'
		Me.TextBox_PSSubTeamNo.Location = New System.Drawing.Point(153, 95)
		Me.TextBox_PSSubTeamNo.Name = "TextBox_PSSubTeamNo"
		Me.TextBox_PSSubTeamNo.Size = New System.Drawing.Size(100, 20)
		Me.TextBox_PSSubTeamNo.TabIndex = 11
		'
		'TextBox_PSTeamNo
		'
		Me.TextBox_PSTeamNo.Location = New System.Drawing.Point(153, 120)
		Me.TextBox_PSTeamNo.Name = "TextBox_PSTeamNo"
		Me.TextBox_PSTeamNo.Size = New System.Drawing.Size(100, 20)
		Me.TextBox_PSTeamNo.TabIndex = 12
		'
		'Label_CostFactor
		'
		Me.Label_CostFactor.AutoSize = True
		Me.Label_CostFactor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label_CostFactor.Location = New System.Drawing.Point(71, 183)
		Me.Label_CostFactor.Name = "Label_CostFactor"
		Me.Label_CostFactor.Size = New System.Drawing.Size(76, 13)
		Me.Label_CostFactor.TabIndex = 15
		Me.Label_CostFactor.Text = "Cost Factor:"
		Me.ToolTip1.SetToolTip(Me.Label_CostFactor, "People Soft Sub Team (Product)")
		'
		'Label_ICVID
		'
		Me.Label_ICVID.AutoSize = True
		Me.Label_ICVID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label_ICVID.Location = New System.Drawing.Point(2, 243)
		Me.Label_ICVID.Name = "Label_ICVID"
		Me.Label_ICVID.Size = New System.Drawing.Size(145, 13)
		Me.Label_ICVID.TabIndex = 17
		Me.Label_ICVID.Text = "Inventory Count Vendor:"
		Me.ToolTip1.SetToolTip(Me.Label_ICVID, "People Soft Sub Team (Product)")
		'
		'RichTextBox1
		'
		Me.RichTextBox1.BackColor = System.Drawing.SystemColors.Control
		Me.RichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.RichTextBox1.Location = New System.Drawing.Point(273, 98)
		Me.RichTextBox1.Name = "RichTextBox1"
		Me.RichTextBox1.ReadOnly = True
		Me.RichTextBox1.Size = New System.Drawing.Size(330, 61)
		Me.RichTextBox1.TabIndex = 14
		Me.RichTextBox1.TabStop = False
		Me.RichTextBox1.Text = resources.GetString("RichTextBox1.Text")
		'
		'TextBox_CostFactor
		'
		Me.TextBox_CostFactor.Location = New System.Drawing.Point(153, 180)
		Me.TextBox_CostFactor.Name = "TextBox_CostFactor"
		Me.TextBox_CostFactor.Size = New System.Drawing.Size(100, 20)
		Me.TextBox_CostFactor.TabIndex = 16
		'
		'RichTextBox2
		'
		Me.RichTextBox2.BackColor = System.Drawing.SystemColors.Control
		Me.RichTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.RichTextBox2.Location = New System.Drawing.Point(273, 180)
		Me.RichTextBox2.Name = "RichTextBox2"
		Me.RichTextBox2.ReadOnly = True
		Me.RichTextBox2.Size = New System.Drawing.Size(377, 45)
		Me.RichTextBox2.TabIndex = 19
		Me.RichTextBox2.TabStop = False
		Me.RichTextBox2.Text = "To find the cost factor for a subteam, use the following formula:" & Global.Microsoft.VisualBasic.ChrW(10) & "     Cost Facto" &
	"r = 1 – Margin (Margins are a decimal value; a 35% margin is .35)"
		'
		'ComboBox_ICVID
		'
		Me.ComboBox_ICVID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.ComboBox_ICVID.FormattingEnabled = True
		Me.ComboBox_ICVID.Location = New System.Drawing.Point(153, 240)
		Me.ComboBox_ICVID.Name = "ComboBox_ICVID"
		Me.ComboBox_ICVID.Size = New System.Drawing.Size(210, 21)
		Me.ComboBox_ICVID.TabIndex = 20
		'
		'cmbSubTeam
		'
		Me.cmbSubTeam.AutoSize = True
		Me.cmbSubTeam.IsShowAll = True
		Me.cmbSubTeam.CheckboxFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmbSubTeam.CheckboxForeColor = System.Drawing.SystemColors.ControlText
		Me.cmbSubTeam.CheckboxText = "Show All"
		Me.cmbSubTeam.ClearSelectionVisisble = False
		Me.cmbSubTeam.DataSource = Nothing
		Me.cmbSubTeam.DisplayMember = "SubTeamName"
		Me.cmbSubTeam.DropDownWidth = 211
		Me.cmbSubTeam.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmbSubTeam.HeaderForeColor = System.Drawing.SystemColors.ControlText
		Me.cmbSubTeam.HeaderText = "Caption"
		Me.cmbSubTeam.HeaderVisible = False
		Me.cmbSubTeam.Location = New System.Drawing.Point(98, 31)
		Me.cmbSubTeam.MinimumSize = New System.Drawing.Size(225, 0)
		Me.cmbSubTeam.Name = "cmbSubTeam"
		Me.cmbSubTeam.SelectedIndex = -1
		Me.cmbSubTeam.SelectedItem = Nothing
		Me.cmbSubTeam.SelectedText = ""
		Me.cmbSubTeam.SelectedValue = Nothing
		Me.cmbSubTeam.Size = New System.Drawing.Size(283, 21)
		Me.cmbSubTeam.TabIndex = 8
		Me.cmbSubTeam.ValueMember = "SubTeamNo"
		'
		'StoreSubTeamEdit
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(681, 298)
		Me.Controls.Add(Me.ComboBox_ICVID)
		Me.Controls.Add(Me.RichTextBox2)
		Me.Controls.Add(Me.Label_ICVID)
		Me.Controls.Add(Me.TextBox_CostFactor)
		Me.Controls.Add(Me.Label_CostFactor)
		Me.Controls.Add(Me.RichTextBox1)
		Me.Controls.Add(Me.TextBox_PSTeamNo)
		Me.Controls.Add(Me.TextBox_PSSubTeamNo)
		Me.Controls.Add(Me.Label_PSTeam)
		Me.Controls.Add(Me.Label_PSSubTeam)
		Me.Controls.Add(Me.cmbSubTeam)
		Me.Controls.Add(Me.Button_Cancel)
		Me.Controls.Add(Me.Button_Save)
		Me.Controls.Add(Me.lblStoreName)
		Me.Controls.Add(Me.Label_Team)
		Me.Controls.Add(Me.ComboBox_Team)
		Me.Controls.Add(Me.Label_SubTeam)
		Me.Controls.Add(Me.Label_Store)
		Me.Name = "StoreSubTeamEdit"
		Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Store Sub Team Edit"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents Label_Store As System.Windows.Forms.Label
    Friend WithEvents Label_SubTeam As System.Windows.Forms.Label
    Friend WithEvents ComboBox_Team As System.Windows.Forms.ComboBox
    Friend WithEvents Label_Team As System.Windows.Forms.Label
    Friend WithEvents lblStoreName As System.Windows.Forms.Label
    Friend WithEvents Button_Save As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
	Friend WithEvents cmbSubTeam As SubteamComboBox
	Friend WithEvents Label_PSSubTeam As System.Windows.Forms.Label
    Friend WithEvents Label_PSTeam As System.Windows.Forms.Label
    Friend WithEvents TextBox_PSSubTeamNo As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_PSTeamNo As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents TextBox_CostFactor As System.Windows.Forms.TextBox
    Friend WithEvents Label_CostFactor As System.Windows.Forms.Label
    Friend WithEvents Label_ICVID As System.Windows.Forms.Label
    Friend WithEvents RichTextBox2 As System.Windows.Forms.RichTextBox
    Friend WithEvents ComboBox_ICVID As System.Windows.Forms.ComboBox
End Class
