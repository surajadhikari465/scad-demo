<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmStore
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
	Public WithEvents cmdSelStore As System.Windows.Forms.Button
	Public WithEvents cmdItems As System.Windows.Forms.Button
	Public WithEvents txtAbbr As System.Windows.Forms.TextBox
	Public WithEvents txtEXEWarehouse As System.Windows.Forms.TextBox
	Public WithEvents txtUNFI As System.Windows.Forms.TextBox
	Public WithEvents txtBusinessUnit As System.Windows.Forms.TextBox
	Public WithEvents cmbZone As System.Windows.Forms.ComboBox
	Public WithEvents chkRegional As System.Windows.Forms.CheckBox
	Public WithEvents chkInternal As System.Windows.Forms.CheckBox
	Public WithEvents _optType_3 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_0 As System.Windows.Forms.RadioButton
	Public WithEvents fraType As System.Windows.Forms.GroupBox
	Public WithEvents txtPhone As System.Windows.Forms.TextBox
	Public WithEvents txtName As System.Windows.Forms.TextBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents _lblLabel_6 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents optType As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStore))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdSelStore = New System.Windows.Forms.Button()
        Me.cmdItems = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdSelect = New System.Windows.Forms.Button()
        Me.txtGeoCode = New System.Windows.Forms.TextBox()
        Me.lblGeoCode = New System.Windows.Forms.Label()
        Me.txtAbbr = New System.Windows.Forms.TextBox()
        Me.txtEXEWarehouse = New System.Windows.Forms.TextBox()
        Me.txtUNFI = New System.Windows.Forms.TextBox()
        Me.txtBusinessUnit = New System.Windows.Forms.TextBox()
        Me.cmbZone = New System.Windows.Forms.ComboBox()
        Me.chkRegional = New System.Windows.Forms.CheckBox()
        Me.chkInternal = New System.Windows.Forms.CheckBox()
        Me.fraType = New System.Windows.Forms.GroupBox()
        Me._optType_3 = New System.Windows.Forms.RadioButton()
        Me._optType_2 = New System.Windows.Forms.RadioButton()
        Me._optType_1 = New System.Windows.Forms.RadioButton()
        Me._optType_0 = New System.Windows.Forms.RadioButton()
        Me.txtPhone = New System.Windows.Forms.TextBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me._lblLabel_6 = New System.Windows.Forms.Label()
        Me._lblLabel_5 = New System.Windows.Forms.Label()
        Me._lblLabel_4 = New System.Windows.Forms.Label()
        Me._lblLabel_2 = New System.Windows.Forms.Label()
        Me._lblLabel_3 = New System.Windows.Forms.Label()
        Me._lblLabel_1 = New System.Windows.Forms.Label()
        Me._lblLabel_0 = New System.Windows.Forms.Label()
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optType = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.ComboBox_TaxJurisdiction = New System.Windows.Forms.ComboBox()
        Me.Label_TaxJurisdiction = New System.Windows.Forms.Label()
        Me.Button_StoreSubTeam = New System.Windows.Forms.Button()
        Me.lblPSIStore = New System.Windows.Forms.Label()
        Me.txtPSIStoreNo = New System.Windows.Forms.TextBox()
        Me.Label_StoreJurisdiction = New System.Windows.Forms.Label()
        Me.ComboBox_StoreJurisdiction = New System.Windows.Forms.ComboBox()
        Me.TextBox_ScaleStoreNo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.fraType.SuspendLayout()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optType, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdSelStore
        '
        resources.ApplyResources(Me.cmdSelStore, "cmdSelStore")
        Me.cmdSelStore.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelStore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelStore.Name = "cmdSelStore"
        Me.cmdSelStore.TabStop = False
        Me.ToolTip1.SetToolTip(Me.cmdSelStore, resources.GetString("cmdSelStore.ToolTip"))
        Me.cmdSelStore.UseVisualStyleBackColor = False
        '
        'cmdItems
        '
        resources.ApplyResources(Me.cmdItems, "cmdItems")
        Me.cmdItems.BackColor = System.Drawing.SystemColors.Control
        Me.cmdItems.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdItems.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdItems.Name = "cmdItems"
        Me.cmdItems.TabStop = False
        Me.ToolTip1.SetToolTip(Me.cmdItems, resources.GetString("cmdItems.ToolTip"))
        Me.cmdItems.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.TabStop = False
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdSelect
        '
        resources.ApplyResources(Me.cmdSelect, "cmdSelect")
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Name = "cmdSelect"
        Me.cmdSelect.TabStop = False
        Me.ToolTip1.SetToolTip(Me.cmdSelect, resources.GetString("cmdSelect.ToolTip"))
        Me.cmdSelect.UseVisualStyleBackColor = False
        '
        'txtGeoCode
        '
        Me.txtGeoCode.AcceptsReturn = True
        Me.txtGeoCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtGeoCode.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtGeoCode, "txtGeoCode")
        Me.txtGeoCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtGeoCode.Name = "txtGeoCode"
        Me.txtGeoCode.Tag = "Geo"
        Me.ToolTip1.SetToolTip(Me.txtGeoCode, resources.GetString("txtGeoCode.ToolTip"))
        '
        'lblGeoCode
        '
        Me.lblGeoCode.BackColor = System.Drawing.Color.Transparent
        Me.lblGeoCode.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblGeoCode, "lblGeoCode")
        Me.lblGeoCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGeoCode.Name = "lblGeoCode"
        Me.ToolTip1.SetToolTip(Me.lblGeoCode, resources.GetString("lblGeoCode.ToolTip"))
        '
        'txtAbbr
        '
        Me.txtAbbr.AcceptsReturn = True
        Me.txtAbbr.BackColor = System.Drawing.SystemColors.Window
        Me.txtAbbr.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtAbbr, "txtAbbr")
        Me.txtAbbr.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAbbr.Name = "txtAbbr"
        Me.txtAbbr.Tag = "String"
        '
        'txtEXEWarehouse
        '
        Me.txtEXEWarehouse.AcceptsReturn = True
        Me.txtEXEWarehouse.BackColor = System.Drawing.SystemColors.Window
        Me.txtEXEWarehouse.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtEXEWarehouse, "txtEXEWarehouse")
        Me.txtEXEWarehouse.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEXEWarehouse.Name = "txtEXEWarehouse"
        Me.txtEXEWarehouse.Tag = "Number"
        '
        'txtUNFI
        '
        Me.txtUNFI.AcceptsReturn = True
        Me.txtUNFI.BackColor = System.Drawing.SystemColors.Window
        Me.txtUNFI.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtUNFI, "txtUNFI")
        Me.txtUNFI.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtUNFI.Name = "txtUNFI"
        Me.txtUNFI.Tag = "Number"
        '
        'txtBusinessUnit
        '
        Me.txtBusinessUnit.AcceptsReturn = True
        Me.txtBusinessUnit.BackColor = System.Drawing.SystemColors.Window
        Me.txtBusinessUnit.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtBusinessUnit, "txtBusinessUnit")
        Me.txtBusinessUnit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBusinessUnit.Name = "txtBusinessUnit"
        Me.txtBusinessUnit.Tag = "Number"
        '
        'cmbZone
        '
        Me.cmbZone.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbZone.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbZone.BackColor = System.Drawing.SystemColors.Window
        Me.cmbZone.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbZone, "cmbZone")
        Me.cmbZone.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbZone.Name = "cmbZone"
        Me.cmbZone.Sorted = True
        '
        'chkRegional
        '
        Me.chkRegional.BackColor = System.Drawing.SystemColors.Control
        Me.chkRegional.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkRegional, "chkRegional")
        Me.chkRegional.ForeColor = System.Drawing.SystemColors.WindowText
        Me.chkRegional.Name = "chkRegional"
        Me.chkRegional.TabStop = False
        Me.chkRegional.UseVisualStyleBackColor = False
        '
        'chkInternal
        '
        Me.chkInternal.BackColor = System.Drawing.SystemColors.Control
        Me.chkInternal.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkInternal, "chkInternal")
        Me.chkInternal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkInternal.Name = "chkInternal"
        Me.chkInternal.TabStop = False
        Me.chkInternal.UseVisualStyleBackColor = False
        '
        'fraType
        '
        Me.fraType.BackColor = System.Drawing.SystemColors.Control
        Me.fraType.Controls.Add(Me._optType_3)
        Me.fraType.Controls.Add(Me._optType_2)
        Me.fraType.Controls.Add(Me._optType_1)
        Me.fraType.Controls.Add(Me._optType_0)
        resources.ApplyResources(Me.fraType, "fraType")
        Me.fraType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraType.Name = "fraType"
        Me.fraType.TabStop = False
        '
        '_optType_3
        '
        Me._optType_3.BackColor = System.Drawing.SystemColors.Control
        Me._optType_3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optType_3, "_optType_3")
        Me._optType_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_3, CType(3, Short))
        Me._optType_3.Name = "_optType_3"
        Me._optType_3.TabStop = True
        Me._optType_3.UseVisualStyleBackColor = False
        '
        '_optType_2
        '
        Me._optType_2.BackColor = System.Drawing.SystemColors.Control
        Me._optType_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optType_2, "_optType_2")
        Me._optType_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_2, CType(2, Short))
        Me._optType_2.Name = "_optType_2"
        Me._optType_2.TabStop = True
        Me._optType_2.UseVisualStyleBackColor = False
        '
        '_optType_1
        '
        Me._optType_1.BackColor = System.Drawing.SystemColors.Control
        Me._optType_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optType_1, "_optType_1")
        Me._optType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_1, CType(1, Short))
        Me._optType_1.Name = "_optType_1"
        Me._optType_1.TabStop = True
        Me._optType_1.UseVisualStyleBackColor = False
        '
        '_optType_0
        '
        Me._optType_0.BackColor = System.Drawing.SystemColors.Control
        Me._optType_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optType_0, "_optType_0")
        Me._optType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_0, CType(0, Short))
        Me._optType_0.Name = "_optType_0"
        Me._optType_0.TabStop = True
        Me._optType_0.UseVisualStyleBackColor = False
        '
        'txtPhone
        '
        Me.txtPhone.AcceptsReturn = True
        Me.txtPhone.BackColor = System.Drawing.SystemColors.Window
        Me.txtPhone.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtPhone, "txtPhone")
        Me.txtPhone.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPhone.Name = "txtPhone"
        Me.txtPhone.Tag = "PHONENUMBER"
        '
        'txtName
        '
        Me.txtName.AcceptsReturn = True
        Me.txtName.BackColor = System.Drawing.SystemColors.Window
        Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtName, "txtName")
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Name = "txtName"
        Me.txtName.Tag = "String"
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
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_5, "_lblLabel_5")
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Name = "_lblLabel_5"
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
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_2, "_lblLabel_2")
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Name = "_lblLabel_2"
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
        'ComboBox_TaxJurisdiction
        '
        Me.ComboBox_TaxJurisdiction.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ComboBox_TaxJurisdiction.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_TaxJurisdiction.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox_TaxJurisdiction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.ComboBox_TaxJurisdiction, "ComboBox_TaxJurisdiction")
        Me.ComboBox_TaxJurisdiction.FormattingEnabled = True
        Me.ComboBox_TaxJurisdiction.Name = "ComboBox_TaxJurisdiction"
        '
        'Label_TaxJurisdiction
        '
        resources.ApplyResources(Me.Label_TaxJurisdiction, "Label_TaxJurisdiction")
        Me.Label_TaxJurisdiction.Name = "Label_TaxJurisdiction"
        '
        'Button_StoreSubTeam
        '
        resources.ApplyResources(Me.Button_StoreSubTeam, "Button_StoreSubTeam")
        Me.Button_StoreSubTeam.Name = "Button_StoreSubTeam"
        Me.Button_StoreSubTeam.UseVisualStyleBackColor = True
        '
        'lblPSIStore
        '
        resources.ApplyResources(Me.lblPSIStore, "lblPSIStore")
        Me.lblPSIStore.Name = "lblPSIStore"
        '
        'txtPSIStoreNo
        '
        resources.ApplyResources(Me.txtPSIStoreNo, "txtPSIStoreNo")
        Me.txtPSIStoreNo.Name = "txtPSIStoreNo"
        '
        'Label_StoreJurisdiction
        '
        resources.ApplyResources(Me.Label_StoreJurisdiction, "Label_StoreJurisdiction")
        Me.Label_StoreJurisdiction.Name = "Label_StoreJurisdiction"
        '
        'ComboBox_StoreJurisdiction
        '
        Me.ComboBox_StoreJurisdiction.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ComboBox_StoreJurisdiction.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_StoreJurisdiction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_StoreJurisdiction.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBox_StoreJurisdiction, "ComboBox_StoreJurisdiction")
        Me.ComboBox_StoreJurisdiction.Name = "ComboBox_StoreJurisdiction"
        '
        'TextBox_ScaleStoreNo
        '
        Me.TextBox_ScaleStoreNo.AcceptsReturn = True
        Me.TextBox_ScaleStoreNo.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_ScaleStoreNo.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.TextBox_ScaleStoreNo, "TextBox_ScaleStoreNo")
        Me.TextBox_ScaleStoreNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox_ScaleStoreNo.Name = "TextBox_ScaleStoreNo"
        Me.TextBox_ScaleStoreNo.Tag = "PHONENUMBER"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        '
        'frmStore
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.TextBox_ScaleStoreNo)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtGeoCode)
        Me.Controls.Add(Me.lblGeoCode)
        Me.Controls.Add(Me.ComboBox_StoreJurisdiction)
        Me.Controls.Add(Me.Label_StoreJurisdiction)
        Me.Controls.Add(Me.txtPSIStoreNo)
        Me.Controls.Add(Me.lblPSIStore)
        Me.Controls.Add(Me.Button_StoreSubTeam)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.Label_TaxJurisdiction)
        Me.Controls.Add(Me.ComboBox_TaxJurisdiction)
        Me.Controls.Add(Me.cmdSelStore)
        Me.Controls.Add(Me.cmdItems)
        Me.Controls.Add(Me.txtAbbr)
        Me.Controls.Add(Me.txtEXEWarehouse)
        Me.Controls.Add(Me.txtUNFI)
        Me.Controls.Add(Me.txtBusinessUnit)
        Me.Controls.Add(Me.cmbZone)
        Me.Controls.Add(Me.chkRegional)
        Me.Controls.Add(Me.chkInternal)
        Me.Controls.Add(Me.fraType)
        Me.Controls.Add(Me.txtPhone)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me._lblLabel_6)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me._lblLabel_4)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Controls.Add(Me._lblLabel_3)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmStore"
        Me.ShowInTaskbar = False
        Me.fraType.ResumeLayout(False)
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optType, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboBox_TaxJurisdiction As System.Windows.Forms.ComboBox
    Friend WithEvents Label_TaxJurisdiction As System.Windows.Forms.Label
    Public WithEvents cmdSelect As System.Windows.Forms.Button
    Friend WithEvents Button_StoreSubTeam As System.Windows.Forms.Button
    Friend WithEvents lblPSIStore As System.Windows.Forms.Label
    Friend WithEvents txtPSIStoreNo As System.Windows.Forms.TextBox
    Friend WithEvents Label_StoreJurisdiction As System.Windows.Forms.Label
    Friend WithEvents ComboBox_StoreJurisdiction As System.Windows.Forms.ComboBox
    Public WithEvents txtGeoCode As System.Windows.Forms.TextBox
    Public WithEvents lblGeoCode As System.Windows.Forms.Label
    Public WithEvents TextBox_ScaleStoreNo As System.Windows.Forms.TextBox
    Public WithEvents Label1 As System.Windows.Forms.Label
#End Region
End Class