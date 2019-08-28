<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmItemList
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents _txtField_5 As System.Windows.Forms.TextBox
	Public WithEvents chkWFM As System.Windows.Forms.CheckBox
	Public WithEvents chkDiscontinued As System.Windows.Forms.CheckBox
    Public WithEvents _txtField_0 As System.Windows.Forms.TextBox
	Public WithEvents cmbSubTeam As SubteamComboBox
	Public WithEvents cmbStore As System.Windows.Forms.ComboBox
	Public WithEvents _txtField_1 As System.Windows.Forms.TextBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents _lblLabel_7 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_6 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItemList))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdReport = New System.Windows.Forms.Button()
		Me.cmdExit = New System.Windows.Forms.Button()
		Me.cmdCompanySearch = New System.Windows.Forms.Button()
		Me.chkPrintOnly = New System.Windows.Forms.CheckBox()
		Me._txtField_5 = New System.Windows.Forms.TextBox()
		Me.chkWFM = New System.Windows.Forms.CheckBox()
		Me.chkDiscontinued = New System.Windows.Forms.CheckBox()
		Me._txtField_0 = New System.Windows.Forms.TextBox()
		Me.cmbSubTeam = New SubteamComboBox()
		Me.cmbStore = New System.Windows.Forms.ComboBox()
		Me._txtField_1 = New System.Windows.Forms.TextBox()
		Me._lblLabel_7 = New System.Windows.Forms.Label()
		Me._lblLabel_4 = New System.Windows.Forms.Label()
		Me._lblLabel_3 = New System.Windows.Forms.Label()
		Me._lblLabel_2 = New System.Windows.Forms.Label()
		Me._lblLabel_5 = New System.Windows.Forms.Label()
		Me._lblLabel_6 = New System.Windows.Forms.Label()
		Me._lblLabel_1 = New System.Windows.Forms.Label()
		Me._lblLabel_0 = New System.Windows.Forms.Label()
		Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
		Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
		Me.chkNatItems = New System.Windows.Forms.CheckBox()
		Me.lblNatItems = New System.Windows.Forms.Label()
		Me.txtVendorName = New System.Windows.Forms.TextBox()
		CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
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
		'cmdCompanySearch
		'
		Me.cmdCompanySearch.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCompanySearch.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me.cmdCompanySearch, "cmdCompanySearch")
		Me.cmdCompanySearch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCompanySearch.Name = "cmdCompanySearch"
		Me.ToolTip1.SetToolTip(Me.cmdCompanySearch, resources.GetString("cmdCompanySearch.ToolTip"))
		Me.cmdCompanySearch.UseVisualStyleBackColor = False
		'
		'chkPrintOnly
		'
		Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
		Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me.chkPrintOnly, "chkPrintOnly")
		Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkPrintOnly.Name = "chkPrintOnly"
		Me.chkPrintOnly.UseVisualStyleBackColor = False
		'
		'_txtField_5
		'
		Me._txtField_5.AcceptsReturn = True
		Me._txtField_5.BackColor = System.Drawing.SystemColors.Window
		Me._txtField_5.Cursor = System.Windows.Forms.Cursors.IBeam
		resources.ApplyResources(Me._txtField_5, "_txtField_5")
		Me._txtField_5.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtField.SetIndex(Me._txtField_5, CType(5, Short))
		Me._txtField_5.Name = "_txtField_5"
		Me._txtField_5.Tag = "Integer"
		'
		'chkWFM
		'
		Me.chkWFM.BackColor = System.Drawing.SystemColors.Control
		Me.chkWFM.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me.chkWFM, "chkWFM")
		Me.chkWFM.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkWFM.Name = "chkWFM"
		Me.chkWFM.UseVisualStyleBackColor = False
		'
		'chkDiscontinued
		'
		Me.chkDiscontinued.BackColor = System.Drawing.SystemColors.Control
		Me.chkDiscontinued.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me.chkDiscontinued, "chkDiscontinued")
		Me.chkDiscontinued.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkDiscontinued.Name = "chkDiscontinued"
		Me.chkDiscontinued.UseVisualStyleBackColor = False
		'
		'_txtField_0
		'
		Me._txtField_0.AcceptsReturn = True
		Me._txtField_0.BackColor = System.Drawing.SystemColors.Window
		Me._txtField_0.Cursor = System.Windows.Forms.Cursors.IBeam
		resources.ApplyResources(Me._txtField_0, "_txtField_0")
		Me._txtField_0.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtField.SetIndex(Me._txtField_0, CType(0, Short))
		Me._txtField_0.Name = "_txtField_0"
		Me._txtField_0.Tag = "String"
		'
		'cmbSubTeam
		'
		resources.ApplyResources(Me.cmbSubTeam, "cmbSubTeam")
		Me.cmbSubTeam.CheckboxFont = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
		Me.cmbSubTeam.CheckboxForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbSubTeam.CheckboxText = "Show All"
		Me.cmbSubTeam.DataSource = Nothing
		Me.cmbSubTeam.DisplayMember = Nothing
		Me.cmbSubTeam.DropDownWidth = 201
		Me.cmbSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbSubTeam.HeaderFont = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
		Me.cmbSubTeam.HeaderForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbSubTeam.HeaderText = "Caption"
		Me.cmbSubTeam.HeaderVisible = False
		Me.cmbSubTeam.IsShowAll = True
		Me.cmbSubTeam.Name = "cmbSubTeam"
		Me.cmbSubTeam.SelectedIndex = -1
		Me.cmbSubTeam.SelectedItem = Nothing
		Me.cmbSubTeam.SelectedText = ""
		Me.cmbSubTeam.SelectedValue = Nothing
		Me.cmbSubTeam.ValueMember = Nothing
		'
		'cmbStore
		'
		Me.cmbStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
		Me.cmbStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cmbStore.BackColor = System.Drawing.SystemColors.Window
		Me.cmbStore.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		resources.ApplyResources(Me.cmbStore, "cmbStore")
		Me.cmbStore.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbStore.Name = "cmbStore"
		Me.cmbStore.Sorted = True
		'
		'_txtField_1
		'
		Me._txtField_1.AcceptsReturn = True
		Me._txtField_1.BackColor = System.Drawing.SystemColors.Window
		Me._txtField_1.Cursor = System.Windows.Forms.Cursors.IBeam
		resources.ApplyResources(Me._txtField_1, "_txtField_1")
		Me._txtField_1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtField.SetIndex(Me._txtField_1, CType(1, Short))
		Me._txtField_1.Name = "_txtField_1"
		Me._txtField_1.Tag = "String"
		'
		'_lblLabel_7
		'
		Me._lblLabel_7.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_7.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._lblLabel_7, "_lblLabel_7")
		Me._lblLabel_7.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.SetIndex(Me._lblLabel_7, CType(7, Short))
		Me._lblLabel_7.Name = "_lblLabel_7"
		'
		'_lblLabel_4
		'
		Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._lblLabel_4, "_lblLabel_4")
		Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.SetIndex(Me._lblLabel_4, CType(4, Short))
		Me._lblLabel_4.Name = "_lblLabel_4"
		'
		'_lblLabel_3
		'
		Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._lblLabel_3, "_lblLabel_3")
		Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.SetIndex(Me._lblLabel_3, CType(3, Short))
		Me._lblLabel_3.Name = "_lblLabel_3"
		'
		'_lblLabel_2
		'
		Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._lblLabel_2, "_lblLabel_2")
		Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
		Me._lblLabel_2.Name = "_lblLabel_2"
		'
		'_lblLabel_5
		'
		Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._lblLabel_5, "_lblLabel_5")
		Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
		Me._lblLabel_5.Name = "_lblLabel_5"
		'
		'_lblLabel_6
		'
		Me._lblLabel_6.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_6.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._lblLabel_6, "_lblLabel_6")
		Me._lblLabel_6.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.SetIndex(Me._lblLabel_6, CType(6, Short))
		Me._lblLabel_6.Name = "_lblLabel_6"
		'
		'_lblLabel_1
		'
		Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._lblLabel_1, "_lblLabel_1")
		Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
		Me._lblLabel_1.Name = "_lblLabel_1"
		'
		'_lblLabel_0
		'
		Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._lblLabel_0, "_lblLabel_0")
		Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
		Me._lblLabel_0.Name = "_lblLabel_0"
		'
		'txtField
		'
		'
		'chkNatItems
		'
		Me.chkNatItems.BackColor = System.Drawing.SystemColors.Control
		Me.chkNatItems.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me.chkNatItems, "chkNatItems")
		Me.chkNatItems.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkNatItems.Name = "chkNatItems"
		Me.chkNatItems.UseVisualStyleBackColor = False
		'
		'lblNatItems
		'
		Me.lblNatItems.BackColor = System.Drawing.Color.Transparent
		Me.lblNatItems.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me.lblNatItems, "lblNatItems")
		Me.lblNatItems.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblNatItems.Name = "lblNatItems"
		'
		'txtVendorName
		'
		Me.txtVendorName.AcceptsReturn = True
		Me.txtVendorName.BackColor = System.Drawing.SystemColors.Window
		Me.txtVendorName.Cursor = System.Windows.Forms.Cursors.IBeam
		resources.ApplyResources(Me.txtVendorName, "txtVendorName")
		Me.txtVendorName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtVendorName.Name = "txtVendorName"
		Me.txtVendorName.Tag = "String"
		'
		'frmItemList
		'
		Me.AcceptButton = Me.cmdReport
		resources.ApplyResources(Me, "$this")
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdExit
		Me.Controls.Add(Me.cmdCompanySearch)
		Me.Controls.Add(Me.lblNatItems)
		Me.Controls.Add(Me.chkNatItems)
		Me.Controls.Add(Me.chkPrintOnly)
		Me.Controls.Add(Me.cmdReport)
		Me.Controls.Add(Me._txtField_5)
		Me.Controls.Add(Me.chkWFM)
		Me.Controls.Add(Me.chkDiscontinued)
		Me.Controls.Add(Me.txtVendorName)
		Me.Controls.Add(Me._txtField_0)
		Me.Controls.Add(Me.cmbSubTeam)
		Me.Controls.Add(Me.cmbStore)
		Me.Controls.Add(Me._txtField_1)
		Me.Controls.Add(Me.cmdExit)
		Me.Controls.Add(Me._lblLabel_7)
		Me.Controls.Add(Me._lblLabel_4)
		Me.Controls.Add(Me._lblLabel_3)
		Me.Controls.Add(Me._lblLabel_2)
		Me.Controls.Add(Me._lblLabel_5)
		Me.Controls.Add(Me._lblLabel_6)
		Me.Controls.Add(Me._lblLabel_1)
		Me.Controls.Add(Me._lblLabel_0)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.KeyPreview = True
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmItemList"
		Me.ShowInTaskbar = False
		CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Public WithEvents chkNatItems As System.Windows.Forms.CheckBox
    Public WithEvents lblNatItems As System.Windows.Forms.Label
    Public WithEvents txtVendorName As System.Windows.Forms.TextBox
    Public WithEvents cmdCompanySearch As System.Windows.Forms.Button
#End Region
End Class