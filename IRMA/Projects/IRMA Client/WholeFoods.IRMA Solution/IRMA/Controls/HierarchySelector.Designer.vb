<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HierarchySelector
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
		Me.cmbCategory = New System.Windows.Forms.ComboBox()
		Me.cmbSubTeam = New System.Windows.Forms.ComboBox()
		Me.lblCategory = New System.Windows.Forms.Label()
		Me.lblSubTeam = New System.Windows.Forms.Label()
		Me.panelFourLevel = New System.Windows.Forms.Panel()
		Me.cmdAddLevel4 = New System.Windows.Forms.Button()
		Me.cmdAddLevel3 = New System.Windows.Forms.Button()
		Me.cmbLevel3 = New System.Windows.Forms.ComboBox()
		Me.cmbLevel4 = New System.Windows.Forms.ComboBox()
		Me.lblLevel4 = New System.Windows.Forms.Label()
		Me.lblLevel3 = New System.Windows.Forms.Label()
		Me.cmdAddCat = New System.Windows.Forms.Button()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.pnlSubTeam = New System.Windows.Forms.Panel()
		Me.chkSubTeam = New System.Windows.Forms.CheckBox()
		Me.panelFourLevel.SuspendLayout()
		Me.GroupBox1.SuspendLayout()
		Me.pnlSubTeam.SuspendLayout()
		Me.SuspendLayout()
		'
		'cmbCategory
		'
		Me.cmbCategory.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmbCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
		Me.cmbCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbCategory.Font = New System.Drawing.Font("Arial", 8.25!)
		Me.cmbCategory.FormattingEnabled = True
		Me.cmbCategory.Location = New System.Drawing.Point(81, 37)
		Me.cmbCategory.Name = "cmbCategory"
		Me.cmbCategory.Size = New System.Drawing.Size(345, 22)
		Me.cmbCategory.Sorted = True
		Me.cmbCategory.TabIndex = 1
		'
		'cmbSubTeam
		'
		Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
		Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cmbSubTeam.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbSubTeam.Font = New System.Drawing.Font("Arial", 8.25!)
		Me.cmbSubTeam.FormattingEnabled = True
		Me.cmbSubTeam.Location = New System.Drawing.Point(0, 0)
		Me.cmbSubTeam.Name = "cmbSubTeam"
		Me.cmbSubTeam.Size = New System.Drawing.Size(271, 22)
		Me.cmbSubTeam.Sorted = True
		Me.cmbSubTeam.TabIndex = 0
		'
		'lblCategory
		'
		Me.lblCategory.AutoSize = True
		Me.lblCategory.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblCategory.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCategory.Location = New System.Drawing.Point(31, 40)
		Me.lblCategory.Name = "lblCategory"
		Me.lblCategory.Size = New System.Drawing.Size(44, 14)
		Me.lblCategory.TabIndex = 44
		Me.lblCategory.Text = "Class :"
		'
		'lblSubTeam
		'
		Me.lblSubTeam.AutoSize = True
		Me.lblSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblSubTeam.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblSubTeam.Location = New System.Drawing.Point(6, 16)
		Me.lblSubTeam.Name = "lblSubTeam"
		Me.lblSubTeam.Size = New System.Drawing.Size(68, 14)
		Me.lblSubTeam.TabIndex = 42
		Me.lblSubTeam.Text = "Sub-Team :"
		'
		'panelFourLevel
		'
		Me.panelFourLevel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.panelFourLevel.Controls.Add(Me.cmdAddLevel4)
		Me.panelFourLevel.Controls.Add(Me.cmdAddLevel3)
		Me.panelFourLevel.Controls.Add(Me.cmbLevel3)
		Me.panelFourLevel.Controls.Add(Me.cmbLevel4)
		Me.panelFourLevel.Controls.Add(Me.lblLevel4)
		Me.panelFourLevel.Controls.Add(Me.lblLevel3)
		Me.panelFourLevel.Location = New System.Drawing.Point(7, 59)
		Me.panelFourLevel.Name = "panelFourLevel"
		Me.panelFourLevel.Size = New System.Drawing.Size(453, 52)
		Me.panelFourLevel.TabIndex = 46
		'
		'cmdAddLevel4
		'
		Me.cmdAddLevel4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdAddLevel4.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAddLevel4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
		Me.cmdAddLevel4.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.cmdAddLevel4.Location = New System.Drawing.Point(422, 25)
		Me.cmdAddLevel4.Name = "cmdAddLevel4"
		Me.cmdAddLevel4.Size = New System.Drawing.Size(24, 22)
		Me.cmdAddLevel4.TabIndex = 6
		Me.cmdAddLevel4.Text = "+"
		Me.cmdAddLevel4.UseVisualStyleBackColor = False
		'
		'cmdAddLevel3
		'
		Me.cmdAddLevel3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdAddLevel3.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAddLevel3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
		Me.cmdAddLevel3.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.cmdAddLevel3.Location = New System.Drawing.Point(422, 2)
		Me.cmdAddLevel3.Name = "cmdAddLevel3"
		Me.cmdAddLevel3.Size = New System.Drawing.Size(24, 22)
		Me.cmdAddLevel3.TabIndex = 4
		Me.cmdAddLevel3.Text = "+"
		Me.cmdAddLevel3.UseVisualStyleBackColor = False
		'
		'cmbLevel3
		'
		Me.cmbLevel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmbLevel3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
		Me.cmbLevel3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cmbLevel3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbLevel3.Enabled = False
		Me.cmbLevel3.Font = New System.Drawing.Font("Arial", 8.25!)
		Me.cmbLevel3.FormattingEnabled = True
		Me.cmbLevel3.Location = New System.Drawing.Point(74, 2)
		Me.cmbLevel3.Name = "cmbLevel3"
		Me.cmbLevel3.Size = New System.Drawing.Size(345, 22)
		Me.cmbLevel3.TabIndex = 3
		'
		'cmbLevel4
		'
		Me.cmbLevel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmbLevel4.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
		Me.cmbLevel4.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cmbLevel4.BackColor = System.Drawing.SystemColors.Window
		Me.cmbLevel4.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbLevel4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbLevel4.Enabled = False
		Me.cmbLevel4.Font = New System.Drawing.Font("Arial", 8.0!)
		Me.cmbLevel4.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbLevel4.Location = New System.Drawing.Point(74, 26)
		Me.cmbLevel4.Name = "cmbLevel4"
		Me.cmbLevel4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbLevel4.Size = New System.Drawing.Size(345, 22)
		Me.cmbLevel4.Sorted = True
		Me.cmbLevel4.TabIndex = 5
		'
		'lblLevel4
		'
		Me.lblLevel4.BackColor = System.Drawing.Color.Transparent
		Me.lblLevel4.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLevel4.Enabled = False
		Me.lblLevel4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblLevel4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLevel4.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblLevel4.Location = New System.Drawing.Point(-1, 30)
		Me.lblLevel4.Name = "lblLevel4"
		Me.lblLevel4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLevel4.Size = New System.Drawing.Size(69, 18)
		Me.lblLevel4.TabIndex = 39
		Me.lblLevel4.Text = "Level 4 :"
		Me.lblLevel4.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblLevel3
		'
		Me.lblLevel3.BackColor = System.Drawing.Color.Transparent
		Me.lblLevel3.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLevel3.Enabled = False
		Me.lblLevel3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblLevel3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLevel3.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblLevel3.Location = New System.Drawing.Point(-1, 5)
		Me.lblLevel3.Name = "lblLevel3"
		Me.lblLevel3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLevel3.Size = New System.Drawing.Size(69, 18)
		Me.lblLevel3.TabIndex = 37
		Me.lblLevel3.Text = "Level 3:"
		Me.lblLevel3.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'cmdAddCat
		'
		Me.cmdAddCat.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdAddCat.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAddCat.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
		Me.cmdAddCat.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.cmdAddCat.Location = New System.Drawing.Point(429, 37)
		Me.cmdAddCat.Name = "cmdAddCat"
		Me.cmdAddCat.Size = New System.Drawing.Size(24, 22)
		Me.cmdAddCat.TabIndex = 2
		Me.cmdAddCat.Text = "+"
		Me.cmdAddCat.UseVisualStyleBackColor = False
		'
		'GroupBox1
		'
		Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupBox1.Controls.Add(Me.pnlSubTeam)
		Me.GroupBox1.Controls.Add(Me.lblSubTeam)
		Me.GroupBox1.Controls.Add(Me.cmdAddCat)
		Me.GroupBox1.Controls.Add(Me.panelFourLevel)
		Me.GroupBox1.Controls.Add(Me.cmbCategory)
		Me.GroupBox1.Controls.Add(Me.lblCategory)
		Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(466, 117)
		Me.GroupBox1.TabIndex = 48
		Me.GroupBox1.TabStop = False
		Me.GroupBox1.Text = "Hierarchy"
		'
		'pnlSubTeam
		'
		Me.pnlSubTeam.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.pnlSubTeam.Controls.Add(Me.cmbSubTeam)
		Me.pnlSubTeam.Controls.Add(Me.chkSubTeam)
		Me.pnlSubTeam.Location = New System.Drawing.Point(81, 13)
		Me.pnlSubTeam.Name = "pnlSubTeam"
		Me.pnlSubTeam.Size = New System.Drawing.Size(345, 22)
		Me.pnlSubTeam.TabIndex = 48
		'
		'chkSubTeam
		'
		Me.chkSubTeam.AutoSize = True
		Me.chkSubTeam.Dock = System.Windows.Forms.DockStyle.Right
		Me.chkSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.chkSubTeam.Location = New System.Drawing.Point(271, 0)
		Me.chkSubTeam.Name = "chkSubTeam"
		Me.chkSubTeam.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
		Me.chkSubTeam.Size = New System.Drawing.Size(74, 22)
		Me.chkSubTeam.TabIndex = 47
		Me.chkSubTeam.Text = "Show All"
		Me.chkSubTeam.UseVisualStyleBackColor = True
		'
		'HierarchySelector
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Controls.Add(Me.GroupBox1)
		Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
		Me.Name = "HierarchySelector"
		Me.Size = New System.Drawing.Size(474, 120)
		Me.panelFourLevel.ResumeLayout(False)
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		Me.pnlSubTeam.ResumeLayout(False)
		Me.pnlSubTeam.PerformLayout()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents lblCategory As System.Windows.Forms.Label
    Friend WithEvents lblSubTeam As System.Windows.Forms.Label
    Friend WithEvents panelFourLevel As System.Windows.Forms.Panel
    Public WithEvents cmbLevel4 As System.Windows.Forms.ComboBox
    Public WithEvents lblLevel4 As System.Windows.Forms.Label
    Public WithEvents lblLevel3 As System.Windows.Forms.Label
    Friend WithEvents cmdAddCat As System.Windows.Forms.Button
    Public WithEvents cmbCategory As System.Windows.Forms.ComboBox
    Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
    Public WithEvents cmbLevel3 As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdAddLevel4 As System.Windows.Forms.Button
    Friend WithEvents cmdAddLevel3 As System.Windows.Forms.Button
	Friend WithEvents chkSubTeam As CheckBox
	Friend WithEvents pnlSubTeam As Panel
End Class
