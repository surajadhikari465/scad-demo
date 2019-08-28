<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SubteamComboBox
	Inherits System.Windows.Forms.UserControl

	'UserControl overrides dispose to clean up the component list.
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
		Me.components = New System.ComponentModel.Container()
		Me.lbl = New System.Windows.Forms.Label()
		Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
		Me.pic = New System.Windows.Forms.PictureBox()
		Me.chk = New System.Windows.Forms.CheckBox()
		Me.cmb = New System.Windows.Forms.ComboBox()
		Me.pnl = New System.Windows.Forms.Panel()
		CType(Me.pic, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnl.SuspendLayout()
		Me.SuspendLayout()
		'
		'lbl
		'
		Me.lbl.AutoSize = True
		Me.lbl.Dock = System.Windows.Forms.DockStyle.Top
		Me.lbl.Location = New System.Drawing.Point(0, 0)
		Me.lbl.Name = "lbl"
		Me.lbl.Padding = New System.Windows.Forms.Padding(0, 0, 0, 3)
		Me.lbl.Size = New System.Drawing.Size(43, 16)
		Me.lbl.TabIndex = 0
		Me.lbl.Text = "Caption"
		Me.lbl.Visible = False
		'
		'pic
		'
		Me.pic.Dock = System.Windows.Forms.DockStyle.Left
		Me.pic.Image = Global.My.Resources.Resources.clear
		Me.pic.Location = New System.Drawing.Point(0, 0)
		Me.pic.Name = "pic"
		Me.pic.Padding = New System.Windows.Forms.Padding(0, 0, 5, 0)
		Me.pic.Size = New System.Drawing.Size(17, 24)
		Me.pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
		Me.pic.TabIndex = 2
		Me.pic.TabStop = False
		Me.ToolTip.SetToolTip(Me.pic, "Clear Selection")
		Me.pic.Visible = False
		'
		'chk
		'
		Me.chk.AutoSize = True
		Me.chk.Dock = System.Windows.Forms.DockStyle.Right
		Me.chk.Location = New System.Drawing.Point(178, 0)
		Me.chk.Name = "chk"
		Me.chk.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
		Me.chk.Size = New System.Drawing.Size(72, 24)
		Me.chk.TabIndex = 3
		Me.chk.Text = "Show All"
		Me.ToolTip.SetToolTip(Me.chk, "Show All Subteams including inactive")
		Me.chk.UseVisualStyleBackColor = True
		'
		'cmb
		'
		Me.cmb.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
		Me.cmb.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cmb.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmb.FormattingEnabled = True
		Me.cmb.Location = New System.Drawing.Point(17, 0)
		Me.cmb.Name = "cmb"
		Me.cmb.Size = New System.Drawing.Size(161, 21)
		Me.cmb.TabIndex = 3
		'
		'pnl
		'
		Me.pnl.Controls.Add(Me.cmb)
		Me.pnl.Controls.Add(Me.chk)
		Me.pnl.Controls.Add(Me.pic)
		Me.pnl.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pnl.Location = New System.Drawing.Point(0, 16)
		Me.pnl.Name = "pnl"
		Me.pnl.Size = New System.Drawing.Size(250, 24)
		Me.pnl.TabIndex = 4
		'
		'SubteamComboBox
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.AutoSize = True
		Me.Controls.Add(Me.pnl)
		Me.Controls.Add(Me.lbl)
		Me.MinimumSize = New System.Drawing.Size(25, 0)
		Me.Name = "SubteamComboBox"
		Me.Size = New System.Drawing.Size(250, 40)
		CType(Me.pic, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnl.ResumeLayout(False)
		Me.pnl.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents lbl As Label
	Friend WithEvents ToolTip As ToolTip
	Friend WithEvents cmb As ComboBox
	Friend WithEvents pnl As Panel
	Friend WithEvents chk As CheckBox
	Friend WithEvents pic As PictureBox
End Class
