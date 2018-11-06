<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmLocationItemManualCountReport
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()

        IsInitializing = True

		'This call is required by the Windows Form Designer.
        InitializeComponent()

        IsInitializing = False

	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cboLocation As System.Windows.Forms.ComboBox
	Public WithEvents cboStore As System.Windows.Forms.ComboBox
	Public WithEvents cboSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents lblStore As System.Windows.Forms.Label
	Public WithEvents lblLocName As System.Windows.Forms.Label
	Public WithEvents _lblSubTeam_2 As System.Windows.Forms.Label
	Public WithEvents lblSubTeam As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLocationItemManualCountReport))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdReport = New System.Windows.Forms.Button()
        Me.cboLocation = New System.Windows.Forms.ComboBox()
        Me.cboStore = New System.Windows.Forms.ComboBox()
        Me.cboSubTeam = New System.Windows.Forms.ComboBox()
        Me.lblStore = New System.Windows.Forms.Label()
        Me.lblLocName = New System.Windows.Forms.Label()
        Me._lblSubTeam_2 = New System.Windows.Forms.Label()
        Me.lblSubTeam = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblSortBy = New System.Windows.Forms.Label()
        Me.cboSortBy = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cbBarCode = New System.Windows.Forms.CheckBox()
        CType(Me.lblSubTeam, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdReport, "cmdReport")
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Name = "cmdReport"
        Me.ToolTip1.SetToolTip(Me.cmdReport, resources.GetString("cmdReport.ToolTip"))
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'cboLocation
        '
        Me.cboLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboLocation.BackColor = System.Drawing.SystemColors.Window
        Me.cboLocation.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cboLocation, "cboLocation")
        Me.cboLocation.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboLocation.Name = "cboLocation"
        Me.cboLocation.Sorted = True
        '
        'cboStore
        '
        Me.cboStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboStore.BackColor = System.Drawing.SystemColors.Window
        Me.cboStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cboStore, "cboStore")
        Me.cboStore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStore.Name = "cboStore"
        Me.cboStore.Sorted = True
        '
        'cboSubTeam
        '
        Me.cboSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cboSubTeam, "cboSubTeam")
        Me.cboSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubTeam.Name = "cboSubTeam"
        Me.cboSubTeam.Sorted = True
        '
        'lblStore
        '
        Me.lblStore.BackColor = System.Drawing.Color.Transparent
        Me.lblStore.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblStore, "lblStore")
        Me.lblStore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStore.Name = "lblStore"
        '
        'lblLocName
        '
        Me.lblLocName.BackColor = System.Drawing.SystemColors.Control
        Me.lblLocName.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblLocName, "lblLocName")
        Me.lblLocName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLocName.Name = "lblLocName"
        '
        '_lblSubTeam_2
        '
        Me._lblSubTeam_2.BackColor = System.Drawing.Color.Transparent
        Me._lblSubTeam_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblSubTeam_2, "_lblSubTeam_2")
        Me._lblSubTeam_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubTeam.SetIndex(Me._lblSubTeam_2, CType(2, Short))
        Me._lblSubTeam_2.Name = "_lblSubTeam_2"
        '
        'lblSortBy
        '
        Me.lblSortBy.BackColor = System.Drawing.SystemColors.Control
        Me.lblSortBy.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblSortBy, "lblSortBy")
        Me.lblSortBy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSortBy.Name = "lblSortBy"
        '
        'cboSortBy
        '
        Me.cboSortBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboSortBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboSortBy.BackColor = System.Drawing.SystemColors.Window
        Me.cboSortBy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cboSortBy, "cboSortBy")
        Me.cboSortBy.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSortBy.Items.AddRange(New Object() {resources.GetString("cboSortBy.Items"), resources.GetString("cboSortBy.Items1")})
        Me.cboSortBy.Name = "cboSortBy"
        Me.cboSortBy.Sorted = True
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        '
        'cbBarCode
        '
        resources.ApplyResources(Me.cbBarCode, "cbBarCode")
        Me.cbBarCode.Checked = True
        Me.cbBarCode.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbBarCode.Name = "cbBarCode"
        Me.cbBarCode.UseVisualStyleBackColor = True
        '
        'frmLocationItemManualCountReport
        '
        Me.AcceptButton = Me.cmdReport
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.Controls.Add(Me.cbBarCode)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboSortBy)
        Me.Controls.Add(Me.lblSortBy)
        Me.Controls.Add(Me.cboLocation)
        Me.Controls.Add(Me.cboStore)
        Me.Controls.Add(Me.cboSubTeam)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.lblStore)
        Me.Controls.Add(Me.lblLocName)
        Me.Controls.Add(Me._lblSubTeam_2)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLocationItemManualCountReport"
        Me.ShowInTaskbar = False
        CType(Me.lblSubTeam, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents lblSortBy As System.Windows.Forms.Label
    Public WithEvents cboSortBy As System.Windows.Forms.ComboBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cbBarCode As System.Windows.Forms.CheckBox
#End Region
End Class