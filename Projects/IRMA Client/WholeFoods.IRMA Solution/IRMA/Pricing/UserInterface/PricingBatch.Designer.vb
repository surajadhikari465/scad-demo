<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmPricingBatch
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()

        IsInitializing = True

		'This call is required by the Windows Form Designer.
        InitializeComponent()

        isinitializing = False

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
	Public WithEvents _cmdMaintain_7 As System.Windows.Forms.Button
	Public WithEvents _cmdMaintain_6 As System.Windows.Forms.Button
	Public WithEvents _cmdMaintain_5 As System.Windows.Forms.Button
	Public WithEvents _cmdMaintain_0 As System.Windows.Forms.Button
	Public WithEvents _cmdMaintain_1 As System.Windows.Forms.Button
	Public WithEvents _cmdMaintain_2 As System.Windows.Forms.Button
	Public WithEvents _cmdMaintain_4 As System.Windows.Forms.Button
	Public WithEvents _cmdMaintain_3 As System.Windows.Forms.Button
	Public WithEvents fraMaintain As System.Windows.Forms.Panel
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtIdentifier As System.Windows.Forms.TextBox
	Public WithEvents cmbState As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_0 As System.Windows.Forms.RadioButton
    Public WithEvents _optSelection_5 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_1 As System.Windows.Forms.RadioButton
	Public WithEvents cmbZones As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_3 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_4 As System.Windows.Forms.RadioButton
	Public WithEvents fraStores As System.Windows.Forms.GroupBox
	Public WithEvents _optType_4 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_3 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_0 As System.Windows.Forms.RadioButton
	Public WithEvents Frame3 As System.Windows.Forms.GroupBox
    Public WithEvents fraPriceType As System.Windows.Forms.GroupBox
	Public WithEvents cmbStatus As System.Windows.Forms.ComboBox
	Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents cmdSearch As System.Windows.Forms.Button
    Public WithEvents lblDash As System.Windows.Forms.Label
    Public WithEvents lblDesc As System.Windows.Forms.Label
    Public WithEvents lblIdentifier As System.Windows.Forms.Label
    Public WithEvents lblStatus As System.Windows.Forms.Label
    Public WithEvents lblSubTeam As System.Windows.Forms.Label
    Public WithEvents lblDates As System.Windows.Forms.Label
    Public WithEvents fraSelection As System.Windows.Forms.GroupBox
    Public WithEvents _cmdProcess_1 As System.Windows.Forms.Button
    Public WithEvents _cmdProcess_0 As System.Windows.Forms.Button
    Public WithEvents fraProcess As System.Windows.Forms.Panel
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdMaintain As Microsoft.VisualBasic.Compatibility.VB6.ButtonArray
    Public WithEvents cmdProcess As Microsoft.VisualBasic.Compatibility.VB6.ButtonArray
    Public WithEvents optSelection As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Public WithEvents optTagType As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Public WithEvents optType As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Public WithEvents txtDate As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPricingBatch))
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceBatchHeaderID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_No")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeam_Name")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeam_No")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StartDate")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceBatchStatusDesc")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceBatchStatusID")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemChgTypeDesc")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemChgTypeID")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceChgTypeDesc")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceChgTypeID")
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("TotalItems")
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("BatchDescription")
        Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Auto")
        Dim UltraGridColumn16 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ApplyDate")
        Dim UltraGridColumn17 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("POSBatchId")
        Dim UltraGridColumn18 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PrintShelfTags", 0)
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn19 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ApplyBatches", 1)
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraDataColumn1 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("PriceBatchHeaderID")
        Dim UltraDataColumn2 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Store_No")
        Dim UltraDataColumn3 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Store_Name")
        Dim UltraDataColumn4 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("SubTeam_Name")
        Dim UltraDataColumn5 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("SubTeam_No")
        Dim UltraDataColumn6 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("StartDate")
        Dim UltraDataColumn7 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("PriceBatchStatusDesc")
        Dim UltraDataColumn8 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("PriceBatchStatusID")
        Dim UltraDataColumn9 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("ItemChgTypeDesc")
        Dim UltraDataColumn10 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("ItemChgTypeID")
        Dim UltraDataColumn11 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("PriceChgTypeDesc")
        Dim UltraDataColumn12 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("PriceChgTypeID")
        Dim UltraDataColumn13 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("TotalItems")
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._cmdMaintain_7 = New System.Windows.Forms.Button()
        Me._cmdMaintain_6 = New System.Windows.Forms.Button()
        Me._cmdMaintain_5 = New System.Windows.Forms.Button()
        Me._cmdMaintain_0 = New System.Windows.Forms.Button()
        Me._cmdMaintain_1 = New System.Windows.Forms.Button()
        Me._cmdMaintain_2 = New System.Windows.Forms.Button()
        Me._cmdMaintain_4 = New System.Windows.Forms.Button()
        Me._cmdMaintain_3 = New System.Windows.Forms.Button()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me._cmdProcess_1 = New System.Windows.Forms.Button()
        Me._cmdProcess_0 = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me._cmdProcess_2 = New System.Windows.Forms.Button()
        Me.fraMaintain = New System.Windows.Forms.Panel()
        Me.fraSelection = New System.Windows.Forms.GroupBox()
        Me.chkIgnoreNoTagLogic = New System.Windows.Forms.CheckBox()
        Me.BatchDescriptionTextBox = New System.Windows.Forms.TextBox()
        Me.BatchDescLabel = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.AutoAllRadioButton = New System.Windows.Forms.RadioButton()
        Me.AutoNoRadioButton = New System.Windows.Forms.RadioButton()
        Me.AutoYesRadioButton = New System.Windows.Forms.RadioButton()
        Me.AutoApplyDateUDTE = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.ApplyDateLabel = New System.Windows.Forms.Label()
        Me.dtpEndDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.dtpStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtIdentifier = New System.Windows.Forms.TextBox()
        Me.fraStores = New System.Windows.Forms.GroupBox()
        Me.ucmbStoreList = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.cmbState = New System.Windows.Forms.ComboBox()
        Me._optSelection_2 = New System.Windows.Forms.RadioButton()
        Me._optSelection_0 = New System.Windows.Forms.RadioButton()
        Me._optSelection_5 = New System.Windows.Forms.RadioButton()
        Me._optSelection_1 = New System.Windows.Forms.RadioButton()
        Me.cmbZones = New System.Windows.Forms.ComboBox()
        Me._optSelection_3 = New System.Windows.Forms.RadioButton()
        Me._optSelection_4 = New System.Windows.Forms.RadioButton()
        Me.Frame3 = New System.Windows.Forms.GroupBox()
        Me._optType_5 = New System.Windows.Forms.RadioButton()
        Me._optType_4 = New System.Windows.Forms.RadioButton()
        Me._optType_3 = New System.Windows.Forms.RadioButton()
        Me._optType_2 = New System.Windows.Forms.RadioButton()
        Me._optType_1 = New System.Windows.Forms.RadioButton()
        Me._optType_0 = New System.Windows.Forms.RadioButton()
        Me.fraPriceType = New System.Windows.Forms.GroupBox()
        Me.cmbPriceType = New System.Windows.Forms.ComboBox()
        Me.cmbStatus = New System.Windows.Forms.ComboBox()
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox()
        Me.lblDesc = New System.Windows.Forms.Label()
        Me.lblIdentifier = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblSubTeam = New System.Windows.Forms.Label()
        Me.lblDates = New System.Windows.Forms.Label()
        Me.lblDash = New System.Windows.Forms.Label()
        Me.fraProcess = New System.Windows.Forms.Panel()
        Me.cmdMaintain = New Microsoft.VisualBasic.Compatibility.VB6.ButtonArray(Me.components)
        Me.cmdProcess = New Microsoft.VisualBasic.Compatibility.VB6.ButtonArray(Me.components)
        Me.optSelection = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optTagType = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optType = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtDate = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.ugrdList = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.UltraDataSource1 = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.fraMaintain.SuspendLayout()
        Me.fraSelection.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.AutoApplyDateUDTE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraStores.SuspendLayout()
        CType(Me.ucmbStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame3.SuspendLayout()
        Me.fraPriceType.SuspendLayout()
        Me.fraProcess.SuspendLayout()
        CType(Me.cmdMaintain, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmdProcess, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optTagType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_cmdMaintain_7
        '
        Me._cmdMaintain_7.BackColor = System.Drawing.SystemColors.Control
        Me._cmdMaintain_7.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._cmdMaintain_7, "_cmdMaintain_7")
        Me._cmdMaintain_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMaintain.SetIndex(Me._cmdMaintain_7, CType(7, Short))
        Me._cmdMaintain_7.Name = "_cmdMaintain_7"
        Me.ToolTip1.SetToolTip(Me._cmdMaintain_7, resources.GetString("_cmdMaintain_7.ToolTip"))
        Me._cmdMaintain_7.UseVisualStyleBackColor = False
        '
        '_cmdMaintain_6
        '
        Me._cmdMaintain_6.BackColor = System.Drawing.SystemColors.Control
        Me._cmdMaintain_6.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._cmdMaintain_6, "_cmdMaintain_6")
        Me._cmdMaintain_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMaintain.SetIndex(Me._cmdMaintain_6, CType(6, Short))
        Me._cmdMaintain_6.Name = "_cmdMaintain_6"
        Me.ToolTip1.SetToolTip(Me._cmdMaintain_6, resources.GetString("_cmdMaintain_6.ToolTip"))
        Me._cmdMaintain_6.UseVisualStyleBackColor = False
        '
        '_cmdMaintain_5
        '
        Me._cmdMaintain_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdMaintain_5.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._cmdMaintain_5, "_cmdMaintain_5")
        Me._cmdMaintain_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMaintain.SetIndex(Me._cmdMaintain_5, CType(5, Short))
        Me._cmdMaintain_5.Name = "_cmdMaintain_5"
        Me.ToolTip1.SetToolTip(Me._cmdMaintain_5, resources.GetString("_cmdMaintain_5.ToolTip"))
        Me._cmdMaintain_5.UseVisualStyleBackColor = False
        '
        '_cmdMaintain_0
        '
        Me._cmdMaintain_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdMaintain_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._cmdMaintain_0, "_cmdMaintain_0")
        Me._cmdMaintain_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMaintain.SetIndex(Me._cmdMaintain_0, CType(0, Short))
        Me._cmdMaintain_0.Name = "_cmdMaintain_0"
        Me.ToolTip1.SetToolTip(Me._cmdMaintain_0, resources.GetString("_cmdMaintain_0.ToolTip"))
        Me._cmdMaintain_0.UseVisualStyleBackColor = False
        '
        '_cmdMaintain_1
        '
        Me._cmdMaintain_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdMaintain_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._cmdMaintain_1, "_cmdMaintain_1")
        Me._cmdMaintain_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMaintain.SetIndex(Me._cmdMaintain_1, CType(1, Short))
        Me._cmdMaintain_1.Name = "_cmdMaintain_1"
        Me._cmdMaintain_1.Tag = "B"
        Me.ToolTip1.SetToolTip(Me._cmdMaintain_1, resources.GetString("_cmdMaintain_1.ToolTip"))
        Me._cmdMaintain_1.UseVisualStyleBackColor = False
        '
        '_cmdMaintain_2
        '
        Me._cmdMaintain_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdMaintain_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._cmdMaintain_2, "_cmdMaintain_2")
        Me._cmdMaintain_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMaintain.SetIndex(Me._cmdMaintain_2, CType(2, Short))
        Me._cmdMaintain_2.Name = "_cmdMaintain_2"
        Me.ToolTip1.SetToolTip(Me._cmdMaintain_2, resources.GetString("_cmdMaintain_2.ToolTip"))
        Me._cmdMaintain_2.UseVisualStyleBackColor = False
        '
        '_cmdMaintain_4
        '
        Me._cmdMaintain_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdMaintain_4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._cmdMaintain_4, "_cmdMaintain_4")
        Me._cmdMaintain_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMaintain.SetIndex(Me._cmdMaintain_4, CType(4, Short))
        Me._cmdMaintain_4.Name = "_cmdMaintain_4"
        Me.ToolTip1.SetToolTip(Me._cmdMaintain_4, resources.GetString("_cmdMaintain_4.ToolTip"))
        Me._cmdMaintain_4.UseVisualStyleBackColor = False
        '
        '_cmdMaintain_3
        '
        Me._cmdMaintain_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdMaintain_3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._cmdMaintain_3, "_cmdMaintain_3")
        Me._cmdMaintain_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMaintain.SetIndex(Me._cmdMaintain_3, CType(3, Short))
        Me._cmdMaintain_3.Name = "_cmdMaintain_3"
        Me.ToolTip1.SetToolTip(Me._cmdMaintain_3, resources.GetString("_cmdMaintain_3.ToolTip"))
        Me._cmdMaintain_3.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdSearch, "cmdSearch")
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Name = "cmdSearch"
        Me.ToolTip1.SetToolTip(Me.cmdSearch, resources.GetString("cmdSearch.ToolTip"))
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        '_cmdProcess_1
        '
        Me._cmdProcess_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdProcess_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._cmdProcess_1, "_cmdProcess_1")
        Me._cmdProcess_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdProcess.SetIndex(Me._cmdProcess_1, CType(1, Short))
        Me._cmdProcess_1.Name = "_cmdProcess_1"
        Me.ToolTip1.SetToolTip(Me._cmdProcess_1, resources.GetString("_cmdProcess_1.ToolTip"))
        Me._cmdProcess_1.UseVisualStyleBackColor = False
        '
        '_cmdProcess_0
        '
        Me._cmdProcess_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdProcess_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._cmdProcess_0, "_cmdProcess_0")
        Me._cmdProcess_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdProcess.SetIndex(Me._cmdProcess_0, CType(0, Short))
        Me._cmdProcess_0.Name = "_cmdProcess_0"
        Me.ToolTip1.SetToolTip(Me._cmdProcess_0, resources.GetString("_cmdProcess_0.ToolTip"))
        Me._cmdProcess_0.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        '_cmdProcess_2
        '
        Me._cmdProcess_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdProcess_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._cmdProcess_2, "_cmdProcess_2")
        Me._cmdProcess_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdProcess.SetIndex(Me._cmdProcess_2, CType(2, Short))
        Me._cmdProcess_2.Name = "_cmdProcess_2"
        Me.ToolTip1.SetToolTip(Me._cmdProcess_2, resources.GetString("_cmdProcess_2.ToolTip"))
        Me._cmdProcess_2.UseVisualStyleBackColor = False
        '
        'fraMaintain
        '
        Me.fraMaintain.BackColor = System.Drawing.SystemColors.Control
        Me.fraMaintain.Controls.Add(Me._cmdMaintain_7)
        Me.fraMaintain.Controls.Add(Me._cmdMaintain_6)
        Me.fraMaintain.Controls.Add(Me._cmdMaintain_5)
        Me.fraMaintain.Controls.Add(Me._cmdMaintain_0)
        Me.fraMaintain.Controls.Add(Me._cmdMaintain_1)
        Me.fraMaintain.Controls.Add(Me._cmdMaintain_2)
        Me.fraMaintain.Controls.Add(Me._cmdMaintain_4)
        Me.fraMaintain.Controls.Add(Me._cmdMaintain_3)
        Me.fraMaintain.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.fraMaintain, "fraMaintain")
        Me.fraMaintain.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMaintain.Name = "fraMaintain"
        '
        'fraSelection
        '
        Me.fraSelection.BackColor = System.Drawing.SystemColors.Control
        Me.fraSelection.Controls.Add(Me.chkIgnoreNoTagLogic)
        Me.fraSelection.Controls.Add(Me.BatchDescriptionTextBox)
        Me.fraSelection.Controls.Add(Me.BatchDescLabel)
        Me.fraSelection.Controls.Add(Me.GroupBox1)
        Me.fraSelection.Controls.Add(Me.AutoApplyDateUDTE)
        Me.fraSelection.Controls.Add(Me.ApplyDateLabel)
        Me.fraSelection.Controls.Add(Me.dtpEndDate)
        Me.fraSelection.Controls.Add(Me.dtpStartDate)
        Me.fraSelection.Controls.Add(Me.txtDescription)
        Me.fraSelection.Controls.Add(Me.txtIdentifier)
        Me.fraSelection.Controls.Add(Me.fraStores)
        Me.fraSelection.Controls.Add(Me.Frame3)
        Me.fraSelection.Controls.Add(Me.fraPriceType)
        Me.fraSelection.Controls.Add(Me.cmbStatus)
        Me.fraSelection.Controls.Add(Me.cmbSubTeam)
        Me.fraSelection.Controls.Add(Me.cmdSearch)
        Me.fraSelection.Controls.Add(Me.lblDesc)
        Me.fraSelection.Controls.Add(Me.lblIdentifier)
        Me.fraSelection.Controls.Add(Me.lblStatus)
        Me.fraSelection.Controls.Add(Me.lblSubTeam)
        Me.fraSelection.Controls.Add(Me.lblDates)
        Me.fraSelection.Controls.Add(Me.lblDash)
        resources.ApplyResources(Me.fraSelection, "fraSelection")
        Me.fraSelection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSelection.Name = "fraSelection"
        Me.fraSelection.TabStop = False
        '
        'chkIgnoreNoTagLogic
        '
        resources.ApplyResources(Me.chkIgnoreNoTagLogic, "chkIgnoreNoTagLogic")
        Me.chkIgnoreNoTagLogic.Name = "chkIgnoreNoTagLogic"
        Me.chkIgnoreNoTagLogic.UseVisualStyleBackColor = True
        '
        'BatchDescriptionTextBox
        '
        Me.BatchDescriptionTextBox.AcceptsReturn = True
        Me.BatchDescriptionTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.BatchDescriptionTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.BatchDescriptionTextBox, "BatchDescriptionTextBox")
        Me.BatchDescriptionTextBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.BatchDescriptionTextBox.Name = "BatchDescriptionTextBox"
        Me.BatchDescriptionTextBox.Tag = "String"
        '
        'BatchDescLabel
        '
        Me.BatchDescLabel.BackColor = System.Drawing.Color.Transparent
        Me.BatchDescLabel.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.BatchDescLabel, "BatchDescLabel")
        Me.BatchDescLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BatchDescLabel.Name = "BatchDescLabel"
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox1.Controls.Add(Me.AutoAllRadioButton)
        Me.GroupBox1.Controls.Add(Me.AutoNoRadioButton)
        Me.GroupBox1.Controls.Add(Me.AutoYesRadioButton)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'AutoAllRadioButton
        '
        Me.AutoAllRadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.AutoAllRadioButton.Checked = True
        Me.AutoAllRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.AutoAllRadioButton, "AutoAllRadioButton")
        Me.AutoAllRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.AutoAllRadioButton.Name = "AutoAllRadioButton"
        Me.AutoAllRadioButton.TabStop = True
        Me.AutoAllRadioButton.UseVisualStyleBackColor = False
        '
        'AutoNoRadioButton
        '
        Me.AutoNoRadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.AutoNoRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.AutoNoRadioButton, "AutoNoRadioButton")
        Me.AutoNoRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.AutoNoRadioButton.Name = "AutoNoRadioButton"
        Me.AutoNoRadioButton.TabStop = True
        Me.AutoNoRadioButton.UseVisualStyleBackColor = False
        '
        'AutoYesRadioButton
        '
        Me.AutoYesRadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.AutoYesRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.AutoYesRadioButton, "AutoYesRadioButton")
        Me.AutoYesRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.AutoYesRadioButton.Name = "AutoYesRadioButton"
        Me.AutoYesRadioButton.TabStop = True
        Me.AutoYesRadioButton.UseVisualStyleBackColor = False
        '
        'AutoApplyDateUDTE
        '
        Me.AutoApplyDateUDTE.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        resources.ApplyResources(Me.AutoApplyDateUDTE, "AutoApplyDateUDTE")
        Me.AutoApplyDateUDTE.MaskInput = ""
        Me.AutoApplyDateUDTE.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me.AutoApplyDateUDTE.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.AutoApplyDateUDTE.Name = "AutoApplyDateUDTE"
        Me.AutoApplyDateUDTE.Value = Nothing
        '
        'ApplyDateLabel
        '
        Me.ApplyDateLabel.BackColor = System.Drawing.Color.Transparent
        Me.ApplyDateLabel.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.ApplyDateLabel, "ApplyDateLabel")
        Me.ApplyDateLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ApplyDateLabel.Name = "ApplyDateLabel"
        '
        'dtpEndDate
        '
        Me.dtpEndDate.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        resources.ApplyResources(Me.dtpEndDate, "dtpEndDate")
        Me.dtpEndDate.MaskInput = ""
        Me.dtpEndDate.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me.dtpEndDate.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Value = Nothing
        '
        'dtpStartDate
        '
        Me.dtpStartDate.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        resources.ApplyResources(Me.dtpStartDate, "dtpStartDate")
        Me.dtpStartDate.MaskInput = ""
        Me.dtpStartDate.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me.dtpStartDate.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Value = Nothing
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtDescription, "txtDescription")
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Tag = "String"
        '
        'txtIdentifier
        '
        Me.txtIdentifier.AcceptsReturn = True
        Me.txtIdentifier.BackColor = System.Drawing.SystemColors.Window
        Me.txtIdentifier.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtIdentifier, "txtIdentifier")
        Me.txtIdentifier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIdentifier.Name = "txtIdentifier"
        Me.txtIdentifier.Tag = "String"
        '
        'fraStores
        '
        Me.fraStores.BackColor = System.Drawing.SystemColors.Control
        Me.fraStores.Controls.Add(Me.ucmbStoreList)
        Me.fraStores.Controls.Add(Me.cmbState)
        Me.fraStores.Controls.Add(Me._optSelection_2)
        Me.fraStores.Controls.Add(Me._optSelection_0)
        Me.fraStores.Controls.Add(Me._optSelection_5)
        Me.fraStores.Controls.Add(Me._optSelection_1)
        Me.fraStores.Controls.Add(Me.cmbZones)
        Me.fraStores.Controls.Add(Me._optSelection_3)
        Me.fraStores.Controls.Add(Me._optSelection_4)
        resources.ApplyResources(Me.fraStores, "fraStores")
        Me.fraStores.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraStores.Name = "fraStores"
        Me.fraStores.TabStop = False
        '
        'ucmbStoreList
        '
        Me.ucmbStoreList.CheckedListSettings.CheckStateMember = ""
        Appearance16.BackColor = System.Drawing.SystemColors.Window
        Appearance16.BorderColor = System.Drawing.SystemColors.InactiveCaption
        resources.ApplyResources(Appearance16.FontData, "Appearance16.FontData")
        resources.ApplyResources(Appearance16, "Appearance16")
        Appearance16.ForceApplyResources = "FontData|"
        Me.ucmbStoreList.DisplayLayout.Appearance = Appearance16
        Me.ucmbStoreList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ucmbStoreList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance17.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance17.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance17.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance17.FontData, "Appearance17.FontData")
        resources.ApplyResources(Appearance17, "Appearance17")
        Appearance17.ForceApplyResources = "FontData|"
        Me.ucmbStoreList.DisplayLayout.GroupByBox.Appearance = Appearance17
        Appearance19.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance19.FontData, "Appearance19.FontData")
        resources.ApplyResources(Appearance19, "Appearance19")
        Appearance19.ForceApplyResources = "FontData|"
        Me.ucmbStoreList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance19
        Me.ucmbStoreList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance18.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance18.BackColor2 = System.Drawing.SystemColors.Control
        Appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance18.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance18.FontData, "Appearance18.FontData")
        resources.ApplyResources(Appearance18, "Appearance18")
        Appearance18.ForceApplyResources = "FontData|"
        Me.ucmbStoreList.DisplayLayout.GroupByBox.PromptAppearance = Appearance18
        Me.ucmbStoreList.DisplayLayout.MaxColScrollRegions = 1
        Me.ucmbStoreList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance22.BackColor = System.Drawing.SystemColors.Window
        Appearance22.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Appearance22.FontData, "Appearance22.FontData")
        resources.ApplyResources(Appearance22, "Appearance22")
        Appearance22.ForceApplyResources = "FontData|"
        Me.ucmbStoreList.DisplayLayout.Override.ActiveCellAppearance = Appearance22
        Appearance25.BackColor = System.Drawing.SystemColors.Highlight
        Appearance25.ForeColor = System.Drawing.SystemColors.HighlightText
        resources.ApplyResources(Appearance25.FontData, "Appearance25.FontData")
        resources.ApplyResources(Appearance25, "Appearance25")
        Appearance25.ForceApplyResources = "FontData|"
        Me.ucmbStoreList.DisplayLayout.Override.ActiveRowAppearance = Appearance25
        Me.ucmbStoreList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ucmbStoreList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance27.BackColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance27.FontData, "Appearance27.FontData")
        resources.ApplyResources(Appearance27, "Appearance27")
        Appearance27.ForceApplyResources = "FontData|"
        Me.ucmbStoreList.DisplayLayout.Override.CardAreaAppearance = Appearance27
        Appearance23.BorderColor = System.Drawing.Color.Silver
        Appearance23.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        resources.ApplyResources(Appearance23.FontData, "Appearance23.FontData")
        resources.ApplyResources(Appearance23, "Appearance23")
        Appearance23.ForceApplyResources = "FontData|"
        Me.ucmbStoreList.DisplayLayout.Override.CellAppearance = Appearance23
        Me.ucmbStoreList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ucmbStoreList.DisplayLayout.Override.CellPadding = 0
        Appearance21.BackColor = System.Drawing.SystemColors.Control
        Appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance21.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance21.FontData, "Appearance21.FontData")
        resources.ApplyResources(Appearance21, "Appearance21")
        Appearance21.ForceApplyResources = "FontData|"
        Me.ucmbStoreList.DisplayLayout.Override.GroupByRowAppearance = Appearance21
        resources.ApplyResources(Appearance20, "Appearance20")
        resources.ApplyResources(Appearance20.FontData, "Appearance20.FontData")
        Appearance20.ForceApplyResources = "FontData|"
        Me.ucmbStoreList.DisplayLayout.Override.HeaderAppearance = Appearance20
        Me.ucmbStoreList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ucmbStoreList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance26.BackColor = System.Drawing.SystemColors.Window
        Appearance26.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance26.FontData, "Appearance26.FontData")
        resources.ApplyResources(Appearance26, "Appearance26")
        Appearance26.ForceApplyResources = "FontData|"
        Me.ucmbStoreList.DisplayLayout.Override.RowAppearance = Appearance26
        Me.ucmbStoreList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance24.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance24.FontData, "Appearance24.FontData")
        resources.ApplyResources(Appearance24, "Appearance24")
        Appearance24.ForceApplyResources = "FontData|"
        Me.ucmbStoreList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance24
        Me.ucmbStoreList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ucmbStoreList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ucmbStoreList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ucmbStoreList, "ucmbStoreList")
        Me.ucmbStoreList.Name = "ucmbStoreList"
        '
        'cmbState
        '
        Me.cmbState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbState.BackColor = System.Drawing.SystemColors.Window
        Me.cmbState.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbState, "cmbState")
        Me.cmbState.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbState.Name = "cmbState"
        Me.cmbState.Sorted = True
        Me.cmbState.TabStop = False
        '
        '_optSelection_2
        '
        Me._optSelection_2.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_2, "_optSelection_2")
        Me._optSelection_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_2, CType(2, Short))
        Me._optSelection_2.Name = "_optSelection_2"
        Me._optSelection_2.UseVisualStyleBackColor = False
        '
        '_optSelection_0
        '
        Me._optSelection_0.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_0, "_optSelection_0")
        Me._optSelection_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_0, CType(0, Short))
        Me._optSelection_0.Name = "_optSelection_0"
        Me._optSelection_0.UseVisualStyleBackColor = False
        '
        '_optSelection_5
        '
        Me._optSelection_5.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_5.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_5, "_optSelection_5")
        Me._optSelection_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_5, CType(5, Short))
        Me._optSelection_5.Name = "_optSelection_5"
        Me._optSelection_5.UseVisualStyleBackColor = False
        '
        '_optSelection_1
        '
        Me._optSelection_1.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_1, "_optSelection_1")
        Me._optSelection_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_1, CType(1, Short))
        Me._optSelection_1.Name = "_optSelection_1"
        Me._optSelection_1.UseVisualStyleBackColor = False
        '
        'cmbZones
        '
        Me.cmbZones.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbZones.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbZones.BackColor = System.Drawing.SystemColors.Window
        Me.cmbZones.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbZones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbZones, "cmbZones")
        Me.cmbZones.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbZones.Name = "cmbZones"
        Me.cmbZones.Sorted = True
        Me.cmbZones.TabStop = False
        '
        '_optSelection_3
        '
        Me._optSelection_3.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_3, "_optSelection_3")
        Me._optSelection_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_3, CType(3, Short))
        Me._optSelection_3.Name = "_optSelection_3"
        Me._optSelection_3.UseVisualStyleBackColor = False
        '
        '_optSelection_4
        '
        Me._optSelection_4.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_4, "_optSelection_4")
        Me._optSelection_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_4, CType(4, Short))
        Me._optSelection_4.Name = "_optSelection_4"
        Me._optSelection_4.UseVisualStyleBackColor = False
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me._optType_5)
        Me.Frame3.Controls.Add(Me._optType_4)
        Me.Frame3.Controls.Add(Me._optType_3)
        Me.Frame3.Controls.Add(Me._optType_2)
        Me.Frame3.Controls.Add(Me._optType_1)
        Me.Frame3.Controls.Add(Me._optType_0)
        resources.ApplyResources(Me.Frame3, "Frame3")
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Name = "Frame3"
        Me.Frame3.TabStop = False
        '
        '_optType_5
        '
        Me._optType_5.BackColor = System.Drawing.SystemColors.Control
        Me._optType_5.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optType_5, "_optType_5")
        Me._optType_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_5, CType(5, Short))
        Me._optType_5.Name = "_optType_5"
        Me._optType_5.UseVisualStyleBackColor = False
        '
        '_optType_4
        '
        Me._optType_4.BackColor = System.Drawing.SystemColors.Control
        Me._optType_4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optType_4, "_optType_4")
        Me._optType_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_4, CType(4, Short))
        Me._optType_4.Name = "_optType_4"
        Me._optType_4.UseVisualStyleBackColor = False
        '
        '_optType_3
        '
        Me._optType_3.BackColor = System.Drawing.SystemColors.Control
        Me._optType_3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optType_3, "_optType_3")
        Me._optType_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_3, CType(3, Short))
        Me._optType_3.Name = "_optType_3"
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
        Me._optType_0.UseVisualStyleBackColor = False
        '
        'fraPriceType
        '
        Me.fraPriceType.BackColor = System.Drawing.SystemColors.Control
        Me.fraPriceType.Controls.Add(Me.cmbPriceType)
        resources.ApplyResources(Me.fraPriceType, "fraPriceType")
        Me.fraPriceType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPriceType.Name = "fraPriceType"
        Me.fraPriceType.TabStop = False
        '
        'cmbPriceType
        '
        Me.cmbPriceType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbPriceType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbPriceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbPriceType, "cmbPriceType")
        Me.cmbPriceType.Name = "cmbPriceType"
        '
        'cmbStatus
        '
        Me.cmbStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbStatus, "cmbStatus")
        Me.cmbStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStatus.Name = "cmbStatus"
        '
        'cmbSubTeam
        '
        Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbSubTeam, "cmbSubTeam")
        Me.cmbSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.Sorted = True
        '
        'lblDesc
        '
        Me.lblDesc.BackColor = System.Drawing.Color.Transparent
        Me.lblDesc.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDesc, "lblDesc")
        Me.lblDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDesc.Name = "lblDesc"
        '
        'lblIdentifier
        '
        Me.lblIdentifier.BackColor = System.Drawing.Color.Transparent
        Me.lblIdentifier.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblIdentifier, "lblIdentifier")
        Me.lblIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIdentifier.Name = "lblIdentifier"
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblStatus, "lblStatus")
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Name = "lblStatus"
        '
        'lblSubTeam
        '
        Me.lblSubTeam.BackColor = System.Drawing.Color.Transparent
        Me.lblSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblSubTeam, "lblSubTeam")
        Me.lblSubTeam.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubTeam.Name = "lblSubTeam"
        '
        'lblDates
        '
        Me.lblDates.BackColor = System.Drawing.Color.Transparent
        Me.lblDates.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDates, "lblDates")
        Me.lblDates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDates.Name = "lblDates"
        '
        'lblDash
        '
        Me.lblDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDash, "lblDash")
        Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDash.Name = "lblDash"
        '
        'fraProcess
        '
        Me.fraProcess.BackColor = System.Drawing.SystemColors.Control
        Me.fraProcess.Controls.Add(Me._cmdProcess_2)
        Me.fraProcess.Controls.Add(Me._cmdProcess_1)
        Me.fraProcess.Controls.Add(Me._cmdProcess_0)
        Me.fraProcess.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.fraProcess, "fraProcess")
        Me.fraProcess.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraProcess.Name = "fraProcess"
        '
        'cmdMaintain
        '
        '
        'cmdProcess
        '
        '
        'optSelection
        '
        '
        'optType
        '
        '
        'ugrdList
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        resources.ApplyResources(Appearance1.FontData, "Appearance1.FontData")
        resources.ApplyResources(Appearance1, "Appearance1")
        Appearance1.ForceApplyResources = "FontData|"
        Me.ugrdList.DisplayLayout.Appearance = Appearance1
        Me.ugrdList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn2.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Hidden = True
        UltraGridColumn3.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn3.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.Header.Caption = resources.GetString("resource.Caption")
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.MinWidth = 12
        UltraGridColumn3.RowLayoutColumnInfo.OriginX = 0
        UltraGridColumn3.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(96, 0)
        UltraGridColumn3.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn3.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn3.Width = 60
        UltraGridColumn4.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn4.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.Header.Caption = resources.GetString("resource.Caption1")
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.MinWidth = 10
        UltraGridColumn4.RowLayoutColumnInfo.OriginX = 4
        UltraGridColumn4.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(86, 0)
        UltraGridColumn4.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn4.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn4.Width = 67
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Hidden = True
        UltraGridColumn6.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn6.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.Header.Caption = resources.GetString("resource.Caption2")
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.MinWidth = 12
        UltraGridColumn6.RowLayoutColumnInfo.OriginX = 6
        UltraGridColumn6.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn6.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(74, 0)
        UltraGridColumn6.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn6.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn6.Width = 63
        UltraGridColumn7.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn7.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn7.Header.Caption = resources.GetString("resource.Caption3")
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.MinWidth = 10
        UltraGridColumn7.RowLayoutColumnInfo.OriginX = 8
        UltraGridColumn7.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn7.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(61, 0)
        UltraGridColumn7.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn7.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn7.Width = 63
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Hidden = True
        UltraGridColumn9.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn9.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn9.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn9.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn9.Header.Caption = resources.GetString("resource.Caption4")
        UltraGridColumn9.Header.VisiblePosition = 8
        UltraGridColumn9.MinWidth = 12
        UltraGridColumn9.RowLayoutColumnInfo.OriginX = 10
        UltraGridColumn9.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn9.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(59, 0)
        UltraGridColumn9.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn9.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn9.Width = 63
        UltraGridColumn10.Header.VisiblePosition = 9
        UltraGridColumn10.Hidden = True
        UltraGridColumn11.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn11.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn11.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn11.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn11.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn11.Header.Caption = resources.GetString("resource.Caption5")
        UltraGridColumn11.Header.VisiblePosition = 10
        UltraGridColumn11.MinWidth = 12
        UltraGridColumn11.RowLayoutColumnInfo.OriginX = 12
        UltraGridColumn11.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn11.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(66, 0)
        UltraGridColumn11.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn11.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn11.Width = 63
        UltraGridColumn12.Header.VisiblePosition = 11
        UltraGridColumn12.Hidden = True
        UltraGridColumn13.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn13.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn13.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn13.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn13.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn13.Header.Caption = resources.GetString("resource.Caption6")
        UltraGridColumn13.Header.VisiblePosition = 12
        UltraGridColumn13.MinWidth = 12
        UltraGridColumn13.RowLayoutColumnInfo.OriginX = 14
        UltraGridColumn13.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn13.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn13.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn13.Width = 63
        UltraGridColumn14.Header.Caption = resources.GetString("resource.Caption7")
        UltraGridColumn14.Header.VisiblePosition = 13
        UltraGridColumn14.RowLayoutColumnInfo.OriginX = 2
        UltraGridColumn14.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn14.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(85, 0)
        UltraGridColumn14.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn14.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn14.Width = 63
        UltraGridColumn15.Header.VisiblePosition = 14
        UltraGridColumn15.MinWidth = 4
        UltraGridColumn15.RowLayoutColumnInfo.OriginX = 16
        UltraGridColumn15.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn15.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(37, 0)
        UltraGridColumn15.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn15.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn15.Width = 63
        UltraGridColumn16.Header.VisiblePosition = 15
        UltraGridColumn16.RowLayoutColumnInfo.OriginX = 18
        UltraGridColumn16.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn16.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(48, 0)
        UltraGridColumn16.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn16.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn16.Width = 63
        UltraGridColumn17.Header.Caption = resources.GetString("resource.Caption8")
        UltraGridColumn17.Header.VisiblePosition = 16
        UltraGridColumn17.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(51, 0)
        UltraGridColumn17.Width = 63
        resources.ApplyResources(Appearance2, "Appearance2")
        resources.ApplyResources(Appearance2.FontData, "Appearance2.FontData")
        Appearance2.ForceApplyResources = "FontData|"
        UltraGridColumn18.CellAppearance = Appearance2
        UltraGridColumn18.Header.Caption = resources.GetString("resource.Caption9")
        UltraGridColumn18.Header.VisiblePosition = 17
        UltraGridColumn18.MinWidth = 4
        UltraGridColumn18.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(40, 0)
        UltraGridColumn18.Width = 63
        UltraGridColumn19.Header.VisiblePosition = 18
        UltraGridColumn19.Hidden = True
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15, UltraGridColumn16, UltraGridColumn17, UltraGridColumn18, UltraGridColumn19})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout
        Me.ugrdList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance3.FontData.BoldAsString = resources.GetString("resource.BoldAsString")
        resources.ApplyResources(Appearance3, "Appearance3")
        Appearance3.ForceApplyResources = ""
        Me.ugrdList.DisplayLayout.CaptionAppearance = Appearance3
        Me.ugrdList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance4.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance4.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance4.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance4.FontData, "Appearance4.FontData")
        resources.ApplyResources(Appearance4, "Appearance4")
        Appearance4.ForceApplyResources = "FontData|"
        Me.ugrdList.DisplayLayout.GroupByBox.Appearance = Appearance4
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance5.FontData, "Appearance5.FontData")
        resources.ApplyResources(Appearance5, "Appearance5")
        Appearance5.ForceApplyResources = "FontData|"
        Me.ugrdList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance5
        Me.ugrdList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdList.DisplayLayout.GroupByBox.Hidden = True
        Appearance6.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance6.BackColor2 = System.Drawing.SystemColors.Control
        Appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance6.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance6.FontData, "Appearance6.FontData")
        resources.ApplyResources(Appearance6, "Appearance6")
        Appearance6.ForceApplyResources = "FontData|"
        Me.ugrdList.DisplayLayout.GroupByBox.PromptAppearance = Appearance6
        Me.ugrdList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Appearance7.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Appearance7.FontData, "Appearance7.FontData")
        resources.ApplyResources(Appearance7, "Appearance7")
        Appearance7.ForceApplyResources = "FontData|"
        Me.ugrdList.DisplayLayout.Override.ActiveCellAppearance = Appearance7
        Me.ugrdList.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.ugrdList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance8.BackColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance8.FontData, "Appearance8.FontData")
        resources.ApplyResources(Appearance8, "Appearance8")
        Appearance8.ForceApplyResources = "FontData|"
        Me.ugrdList.DisplayLayout.Override.CardAreaAppearance = Appearance8
        Appearance9.BorderColor = System.Drawing.Color.Silver
        Appearance9.FontData.BoldAsString = resources.GetString("resource.BoldAsString1")
        Appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        resources.ApplyResources(Appearance9, "Appearance9")
        Appearance9.ForceApplyResources = ""
        Me.ugrdList.DisplayLayout.Override.CellAppearance = Appearance9
        Me.ugrdList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.ugrdList.DisplayLayout.Override.CellPadding = 0
        Appearance10.FontData.BoldAsString = resources.GetString("resource.BoldAsString2")
        resources.ApplyResources(Appearance10, "Appearance10")
        Appearance10.ForceApplyResources = ""
        Me.ugrdList.DisplayLayout.Override.FixedHeaderAppearance = Appearance10
        Appearance11.BackColor = System.Drawing.SystemColors.Control
        Appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance11.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance11.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance11.FontData, "Appearance11.FontData")
        resources.ApplyResources(Appearance11, "Appearance11")
        Appearance11.ForceApplyResources = "FontData|"
        Me.ugrdList.DisplayLayout.Override.GroupByRowAppearance = Appearance11
        Appearance12.FontData.BoldAsString = resources.GetString("resource.BoldAsString3")
        resources.ApplyResources(Appearance12, "Appearance12")
        Appearance12.ForceApplyResources = ""
        Me.ugrdList.DisplayLayout.Override.HeaderAppearance = Appearance12
        Me.ugrdList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance13.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance13.FontData, "Appearance13.FontData")
        resources.ApplyResources(Appearance13, "Appearance13")
        Appearance13.ForceApplyResources = "FontData|"
        Me.ugrdList.DisplayLayout.Override.RowAlternateAppearance = Appearance13
        Appearance14.BackColor = System.Drawing.SystemColors.Window
        Appearance14.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance14.FontData, "Appearance14.FontData")
        resources.ApplyResources(Appearance14, "Appearance14")
        Appearance14.ForceApplyResources = "FontData|"
        Me.ugrdList.DisplayLayout.Override.RowAppearance = Appearance14
        Me.ugrdList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance15.FontData, "Appearance15.FontData")
        resources.ApplyResources(Appearance15, "Appearance15")
        Appearance15.ForceApplyResources = "FontData|"
        Me.ugrdList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance15
        Me.ugrdList.DisplayLayout.RowSelectorImages.ActiveRowImage = CType(resources.GetObject("ugrdList.DisplayLayout.RowSelectorImages.ActiveRowImage"), System.Drawing.Image)
        Me.ugrdList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdList, "ugrdList")
        Me.ugrdList.Name = "ugrdList"
        '
        'UltraDataSource1
        '
        UltraDataColumn6.DataType = GetType(Date)
        Me.UltraDataSource1.Band.Columns.AddRange(New Object() {UltraDataColumn1, UltraDataColumn2, UltraDataColumn3, UltraDataColumn4, UltraDataColumn5, UltraDataColumn6, UltraDataColumn7, UltraDataColumn8, UltraDataColumn9, UltraDataColumn10, UltraDataColumn11, UltraDataColumn12, UltraDataColumn13})
        '
        'chkSelectAll
        '
        resources.ApplyResources(Me.chkSelectAll, "chkSelectAll")
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'frmPricingBatch
        '
        Me.AcceptButton = Me.cmdSearch
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.chkSelectAll)
        Me.Controls.Add(Me.ugrdList)
        Me.Controls.Add(Me.fraMaintain)
        Me.Controls.Add(Me.fraSelection)
        Me.Controls.Add(Me.fraProcess)
        Me.Controls.Add(Me.cmdExit)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPricingBatch"
        Me.ShowInTaskbar = False
        Me.fraMaintain.ResumeLayout(False)
        Me.fraSelection.ResumeLayout(False)
        Me.fraSelection.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.AutoApplyDateUDTE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraStores.ResumeLayout(False)
        Me.fraStores.PerformLayout()
        CType(Me.ucmbStoreList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame3.ResumeLayout(False)
        Me.fraPriceType.ResumeLayout(False)
        Me.fraProcess.ResumeLayout(False)
        CType(Me.cmdMaintain, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmdProcess, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optTagType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ugrdList As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents _optType_5 As System.Windows.Forms.RadioButton
    Friend WithEvents dtpStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents dtpEndDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraDataSource1 As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Public WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents AutoAllRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents AutoNoRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents AutoYesRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents AutoApplyDateUDTE As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Public WithEvents ApplyDateLabel As System.Windows.Forms.Label
    Public WithEvents BatchDescriptionTextBox As System.Windows.Forms.TextBox
    Public WithEvents BatchDescLabel As System.Windows.Forms.Label
    Public WithEvents cmbPriceType As System.Windows.Forms.ComboBox
    Public WithEvents _cmdProcess_2 As System.Windows.Forms.Button
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents ucmbStoreList As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents chkIgnoreNoTagLogic As CheckBox
#End Region
End Class