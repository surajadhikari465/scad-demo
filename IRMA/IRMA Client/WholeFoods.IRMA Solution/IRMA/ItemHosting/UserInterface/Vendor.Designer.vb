<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmVendor
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
        'This call is required by the Windows Form Designer.
        Me.IsInitializing = True
        InitializeComponent()
        Me.IsInitializing = False
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
	Public WithEvents cmdCompanySearch As System.Windows.Forms.Button
	Public WithEvents cmdUnlock As System.Windows.Forms.Button
    Public WithEvents lblZip As System.Windows.Forms.Label
    Public WithEvents lblState As System.Windows.Forms.Label
    Public WithEvents lblCity As System.Windows.Forms.Label
    Public WithEvents lblCountry As System.Windows.Forms.Label
    Public WithEvents lblPhone As System.Windows.Forms.Label
    Public WithEvents lblAddress As System.Windows.Forms.Label
    Public WithEvents lblCompany As System.Windows.Forms.Label
    Public WithEvents lblKey As System.Windows.Forms.Label
    Public WithEvents lblExt As System.Windows.Forms.Label
    Public WithEvents lblVendorID As System.Windows.Forms.Label
    Public WithEvents _txtField_8 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_7 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_6 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_4 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_3 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_2 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_1 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_11 As System.Windows.Forms.TextBox
    Public WithEvents _chkField_0 As System.Windows.Forms.CheckBox
    Public WithEvents _chkField_1 As System.Windows.Forms.CheckBox
    Public WithEvents _cmbField_0 As System.Windows.Forms.ComboBox
    Public WithEvents _txtField_25 As System.Windows.Forms.TextBox
    Public WithEvents _chkField_2 As System.Windows.Forms.CheckBox
    Public WithEvents _txtField_0 As System.Windows.Forms.TextBox
    Public WithEvents _tabVendor_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents _lblLabel_11 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_12 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_13 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_14 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_15 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_16 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_17 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_18 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_19 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_29 As System.Windows.Forms.Label
    Public WithEvents _txtField_12 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_13 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_14 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_15 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_17 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_18 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_19 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_20 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_21 As System.Windows.Forms.TextBox
    Public WithEvents cmdSameData As System.Windows.Forms.Button
    Public WithEvents _txtField_26 As System.Windows.Forms.TextBox
    Public WithEvents chkNonProductVendor As System.Windows.Forms.CheckBox
    Public WithEvents _chkField_3 As System.Windows.Forms.CheckBox
    Public WithEvents _tabVendor_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents _lblLabel_22 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_24 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_20 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_21 As System.Windows.Forms.Label
    Public WithEvents _txtField_22 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_23 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_24 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_27 As System.Windows.Forms.TextBox
    Public WithEvents _tabVendor_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents tabVendor As System.Windows.Forms.TabControl
    Public WithEvents cmdItems As System.Windows.Forms.Button
    Public WithEvents cmdContacts As System.Windows.Forms.Button
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents lblReadOnly As System.Windows.Forms.Label
    Public WithEvents chkField As Microsoft.VisualBasic.Compatibility.VB6.CheckBoxArray
    Public WithEvents cmbField As Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray
    Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    Public WithEvents optPOTrans As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVendor))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCompanySearch = New System.Windows.Forms.Button()
        Me.cmdUnlock = New System.Windows.Forms.Button()
        Me.cmdSameData = New System.Windows.Forms.Button()
        Me.cmdItems = New System.Windows.Forms.Button()
        Me.cmdContacts = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.txtLeadTimeDays = New System.Windows.Forms.MaskedTextBox()
        Me.CheckBoxAllowReceiveAll = New System.Windows.Forms.CheckBox()
        Me.lblLeadTimeDayOfWeek = New System.Windows.Forms.Label()
        Me.tabVendor = New System.Windows.Forms.TabControl()
        Me._tabVendor_TabPage0 = New System.Windows.Forms.TabPage()
        Me.cbxActive = New System.Windows.Forms.CheckBox()
        Me.CheckBoxAllowBarcodePOReport = New System.Windows.Forms.CheckBox()
        Me.cbxShortpayProhibited = New System.Windows.Forms.CheckBox()
        Me.DSDVendor_StoreSetup = New System.Windows.Forms.Button()
        Me.CheckBox_EinvoiceReqd = New System.Windows.Forms.CheckBox()
        Me.txtAccountingContactEmail = New System.Windows.Forms.TextBox()
        Me.lblAccountingContact = New System.Windows.Forms.Label()
        Me.CheckBox_EInvoicing = New System.Windows.Forms.CheckBox()
        Me._txtField_32 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me._txtField_29 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me._txtField_5 = New System.Windows.Forms.TextBox()
        Me._txtField_25 = New System.Windows.Forms.TextBox()
        Me._chkField_2 = New System.Windows.Forms.CheckBox()
        Me._chkField_1 = New System.Windows.Forms.CheckBox()
        Me._chkField_0 = New System.Windows.Forms.CheckBox()
        Me._txtField_8 = New System.Windows.Forms.TextBox()
        Me.lblZip = New System.Windows.Forms.Label()
        Me.txtCounty = New System.Windows.Forms.TextBox()
        Me.lblState = New System.Windows.Forms.Label()
        Me.lblCity = New System.Windows.Forms.Label()
        Me.lblCountry = New System.Windows.Forms.Label()
        Me.lblPhone = New System.Windows.Forms.Label()
        Me.lblAddress = New System.Windows.Forms.Label()
        Me.lblCompany = New System.Windows.Forms.Label()
        Me.lblKey = New System.Windows.Forms.Label()
        Me.lblExt = New System.Windows.Forms.Label()
        Me.lblVendorID = New System.Windows.Forms.Label()
        Me._txtField_7 = New System.Windows.Forms.TextBox()
        Me._txtField_6 = New System.Windows.Forms.TextBox()
        Me._txtField_4 = New System.Windows.Forms.TextBox()
        Me._txtField_3 = New System.Windows.Forms.TextBox()
        Me._txtField_2 = New System.Windows.Forms.TextBox()
        Me._txtField_1 = New System.Windows.Forms.TextBox()
        Me._txtField_11 = New System.Windows.Forms.TextBox()
        Me._txtField_0 = New System.Windows.Forms.TextBox()
        Me._tabVendor_TabPage1 = New System.Windows.Forms.TabPage()
        Me._txtField_16 = New System.Windows.Forms.TextBox()
        Me._lblLabel_11 = New System.Windows.Forms.Label()
        Me._lblLabel_12 = New System.Windows.Forms.Label()
        Me._lblLabel_13 = New System.Windows.Forms.Label()
        Me.txtPayToCounty = New System.Windows.Forms.TextBox()
        Me._lblLabel_14 = New System.Windows.Forms.Label()
        Me._lblLabel_15 = New System.Windows.Forms.Label()
        Me._lblLabel_16 = New System.Windows.Forms.Label()
        Me._lblLabel_17 = New System.Windows.Forms.Label()
        Me._lblLabel_18 = New System.Windows.Forms.Label()
        Me._lblLabel_19 = New System.Windows.Forms.Label()
        Me._lblLabel_29 = New System.Windows.Forms.Label()
        Me._txtField_12 = New System.Windows.Forms.TextBox()
        Me._txtField_13 = New System.Windows.Forms.TextBox()
        Me._txtField_14 = New System.Windows.Forms.TextBox()
        Me._txtField_15 = New System.Windows.Forms.TextBox()
        Me._txtField_17 = New System.Windows.Forms.TextBox()
        Me._txtField_18 = New System.Windows.Forms.TextBox()
        Me._txtField_19 = New System.Windows.Forms.TextBox()
        Me._txtField_20 = New System.Windows.Forms.TextBox()
        Me._txtField_21 = New System.Windows.Forms.TextBox()
        Me._txtField_26 = New System.Windows.Forms.TextBox()
        Me.chkNonProductVendor = New System.Windows.Forms.CheckBox()
        Me._chkField_3 = New System.Windows.Forms.CheckBox()
        Me._tabVendor_TabPage2 = New System.Windows.Forms.TabPage()
        Me.ComboBox_PaymentTerms = New System.Windows.Forms.ComboBox()
        Me.lblPaymentTerms = New System.Windows.Forms.Label()
        Me.cmbCurrency = New System.Windows.Forms.ComboBox()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.StoreLevelOverridesButton = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox_PSVendorExport = New System.Windows.Forms.TextBox()
        Me.Label_PSVendorExport = New System.Windows.Forms.Label()
        Me._lblLabel_22 = New System.Windows.Forms.Label()
        Me._lblLabel_24 = New System.Windows.Forms.Label()
        Me._lblLabel_20 = New System.Windows.Forms.Label()
        Me._lblLabel_21 = New System.Windows.Forms.Label()
        Me._txtField_22 = New System.Windows.Forms.TextBox()
        Me._txtField_23 = New System.Windows.Forms.TextBox()
        Me._txtField_24 = New System.Windows.Forms.TextBox()
        Me._txtField_27 = New System.Windows.Forms.TextBox()
        Me._tabVendor_TabPage3 = New System.Windows.Forms.TabPage()
        Me._lblLabel_6 = New System.Windows.Forms.Label()
        Me._txtField_10 = New System.Windows.Forms.TextBox()
        Me._lblLabel_27 = New System.Windows.Forms.Label()
        Me._txtField_31 = New System.Windows.Forms.TextBox()
        Me._lblLabel_26 = New System.Windows.Forms.Label()
        Me._txtField_30 = New System.Windows.Forms.TextBox()
        Me._tabVendor_TabPage4 = New System.Windows.Forms.TabPage()
        Me._optPOTrans_3 = New System.Windows.Forms.RadioButton()
        Me._optPOTrans_2 = New System.Windows.Forms.RadioButton()
        Me._optPOTrans_1 = New System.Windows.Forms.RadioButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me._optPOTrans_0 = New System.Windows.Forms.RadioButton()
        Me._txtField_9 = New System.Windows.Forms.TextBox()
        Me._txtField_28 = New System.Windows.Forms.TextBox()
        Me._tabVendor_POCostLeadTimeTab = New System.Windows.Forms.TabPage()
        Me.gbxDayOfWeek = New System.Windows.Forms.GroupBox()
        Me.cmbLeadTimeDayOfWeek = New System.Windows.Forms.ComboBox()
        Me.chkUseLeadTimeDayOfWeek = New System.Windows.Forms.CheckBox()
        Me.lblUseLeadTimeDayOfWeek = New System.Windows.Forms.Label()
        Me.txtAuthorziedDate = New System.Windows.Forms.TextBox()
        Me.lblAuthorizedDate = New System.Windows.Forms.Label()
        Me.txtAuthorizedBy = New System.Windows.Forms.TextBox()
        Me.lblAuthorizedBy = New System.Windows.Forms.Label()
        Me.lblLeadTimeDays = New System.Windows.Forms.Label()
        Me.chkEnableLeadTime = New System.Windows.Forms.CheckBox()
        Me.lblEnableLeadTimeCost = New System.Windows.Forms.Label()
        Me._cmbField_0 = New System.Windows.Forms.ComboBox()
        Me.lblReadOnly = New System.Windows.Forms.Label()
        Me.chkField = New Microsoft.VisualBasic.Compatibility.VB6.CheckBoxArray(Me.components)
        Me.cmbField = New Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.optPOTrans = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me._label_ConfiguredAs = New System.Windows.Forms.Label()
        Me.UltraDataSource1 = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tabVendor.SuspendLayout()
        Me._tabVendor_TabPage0.SuspendLayout()
        Me._tabVendor_TabPage1.SuspendLayout()
        Me._tabVendor_TabPage2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabVendor_TabPage3.SuspendLayout()
        Me._tabVendor_TabPage4.SuspendLayout()
        Me._tabVendor_POCostLeadTimeTab.SuspendLayout()
        Me.gbxDayOfWeek.SuspendLayout()
        CType(Me.chkField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optPOTrans, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCompanySearch
        '
        resources.ApplyResources(Me.cmdCompanySearch, "cmdCompanySearch")
        Me.cmdCompanySearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCompanySearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCompanySearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCompanySearch.Name = "cmdCompanySearch"
        Me.ToolTip1.SetToolTip(Me.cmdCompanySearch, resources.GetString("cmdCompanySearch.ToolTip"))
        Me.cmdCompanySearch.UseVisualStyleBackColor = False
        '
        'cmdUnlock
        '
        resources.ApplyResources(Me.cmdUnlock, "cmdUnlock")
        Me.cmdUnlock.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUnlock.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUnlock.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUnlock.Name = "cmdUnlock"
        Me.ToolTip1.SetToolTip(Me.cmdUnlock, resources.GetString("cmdUnlock.ToolTip"))
        Me.cmdUnlock.UseVisualStyleBackColor = False
        '
        'cmdSameData
        '
        Me.cmdSameData.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSameData.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdSameData, "cmdSameData")
        Me.cmdSameData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSameData.Name = "cmdSameData"
        Me.ToolTip1.SetToolTip(Me.cmdSameData, resources.GetString("cmdSameData.ToolTip"))
        Me.cmdSameData.UseVisualStyleBackColor = False
        '
        'cmdItems
        '
        resources.ApplyResources(Me.cmdItems, "cmdItems")
        Me.cmdItems.BackColor = System.Drawing.SystemColors.Control
        Me.cmdItems.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdItems.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdItems.Name = "cmdItems"
        Me.ToolTip1.SetToolTip(Me.cmdItems, resources.GetString("cmdItems.ToolTip"))
        Me.cmdItems.UseVisualStyleBackColor = False
        '
        'cmdContacts
        '
        resources.ApplyResources(Me.cmdContacts, "cmdContacts")
        Me.cmdContacts.BackColor = System.Drawing.SystemColors.Control
        Me.cmdContacts.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdContacts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdContacts.Name = "cmdContacts"
        Me.ToolTip1.SetToolTip(Me.cmdContacts, resources.GetString("cmdContacts.ToolTip"))
        Me.cmdContacts.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        resources.ApplyResources(Me.cmdDelete, "cmdDelete")
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Name = "cmdDelete"
        Me.ToolTip1.SetToolTip(Me.cmdDelete, resources.GetString("cmdDelete.ToolTip"))
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        resources.ApplyResources(Me.cmdAdd, "cmdAdd")
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Name = "cmdAdd"
        Me.ToolTip1.SetToolTip(Me.cmdAdd, resources.GetString("cmdAdd.ToolTip"))
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'txtLeadTimeDays
        '
        resources.ApplyResources(Me.txtLeadTimeDays, "txtLeadTimeDays")
        Me.txtLeadTimeDays.Name = "txtLeadTimeDays"
        Me.ToolTip1.SetToolTip(Me.txtLeadTimeDays, resources.GetString("txtLeadTimeDays.ToolTip"))
        '
        'CheckBoxAllowReceiveAll
        '
        resources.ApplyResources(Me.CheckBoxAllowReceiveAll, "CheckBoxAllowReceiveAll")
        Me.CheckBoxAllowReceiveAll.Name = "CheckBoxAllowReceiveAll"
        Me.ToolTip1.SetToolTip(Me.CheckBoxAllowReceiveAll, resources.GetString("CheckBoxAllowReceiveAll.ToolTip"))
        Me.CheckBoxAllowReceiveAll.UseVisualStyleBackColor = True
        '
        'lblLeadTimeDayOfWeek
        '
        resources.ApplyResources(Me.lblLeadTimeDayOfWeek, "lblLeadTimeDayOfWeek")
        Me.lblLeadTimeDayOfWeek.Name = "lblLeadTimeDayOfWeek"
        '
        'tabVendor
        '
        Me.tabVendor.Controls.Add(Me._tabVendor_TabPage0)
        Me.tabVendor.Controls.Add(Me._tabVendor_TabPage1)
        Me.tabVendor.Controls.Add(Me._tabVendor_TabPage2)
        Me.tabVendor.Controls.Add(Me._tabVendor_TabPage3)
        Me.tabVendor.Controls.Add(Me._tabVendor_TabPage4)
        Me.tabVendor.Controls.Add(Me._tabVendor_POCostLeadTimeTab)
        resources.ApplyResources(Me.tabVendor, "tabVendor")
        Me.tabVendor.Name = "tabVendor"
        Me.tabVendor.SelectedIndex = 0
        Me.tabVendor.TabStop = False
        '
        '_tabVendor_TabPage0
        '
        Me._tabVendor_TabPage0.BackColor = System.Drawing.Color.Transparent
        Me._tabVendor_TabPage0.Controls.Add(Me.cbxActive)
        Me._tabVendor_TabPage0.Controls.Add(Me.CheckBoxAllowBarcodePOReport)
        Me._tabVendor_TabPage0.Controls.Add(Me.cbxShortpayProhibited)
        Me._tabVendor_TabPage0.Controls.Add(Me.CheckBoxAllowReceiveAll)
        Me._tabVendor_TabPage0.Controls.Add(Me.DSDVendor_StoreSetup)
        Me._tabVendor_TabPage0.Controls.Add(Me.CheckBox_EinvoiceReqd)
        Me._tabVendor_TabPage0.Controls.Add(Me.txtAccountingContactEmail)
        Me._tabVendor_TabPage0.Controls.Add(Me.lblAccountingContact)
        Me._tabVendor_TabPage0.Controls.Add(Me.CheckBox_EInvoicing)
        Me._tabVendor_TabPage0.Controls.Add(Me._txtField_32)
        Me._tabVendor_TabPage0.Controls.Add(Me.Label3)
        Me._tabVendor_TabPage0.Controls.Add(Me._txtField_29)
        Me._tabVendor_TabPage0.Controls.Add(Me.Label1)
        Me._tabVendor_TabPage0.Controls.Add(Me._txtField_5)
        Me._tabVendor_TabPage0.Controls.Add(Me._txtField_25)
        Me._tabVendor_TabPage0.Controls.Add(Me._chkField_2)
        Me._tabVendor_TabPage0.Controls.Add(Me._chkField_1)
        Me._tabVendor_TabPage0.Controls.Add(Me._chkField_0)
        Me._tabVendor_TabPage0.Controls.Add(Me._txtField_8)
        Me._tabVendor_TabPage0.Controls.Add(Me.lblZip)
        Me._tabVendor_TabPage0.Controls.Add(Me.txtCounty)
        Me._tabVendor_TabPage0.Controls.Add(Me.lblState)
        Me._tabVendor_TabPage0.Controls.Add(Me.lblCity)
        Me._tabVendor_TabPage0.Controls.Add(Me.lblCountry)
        Me._tabVendor_TabPage0.Controls.Add(Me.lblPhone)
        Me._tabVendor_TabPage0.Controls.Add(Me.lblAddress)
        Me._tabVendor_TabPage0.Controls.Add(Me.lblCompany)
        Me._tabVendor_TabPage0.Controls.Add(Me.lblKey)
        Me._tabVendor_TabPage0.Controls.Add(Me.lblExt)
        Me._tabVendor_TabPage0.Controls.Add(Me.lblVendorID)
        Me._tabVendor_TabPage0.Controls.Add(Me._txtField_7)
        Me._tabVendor_TabPage0.Controls.Add(Me._txtField_6)
        Me._tabVendor_TabPage0.Controls.Add(Me._txtField_4)
        Me._tabVendor_TabPage0.Controls.Add(Me._txtField_3)
        Me._tabVendor_TabPage0.Controls.Add(Me._txtField_2)
        Me._tabVendor_TabPage0.Controls.Add(Me._txtField_1)
        Me._tabVendor_TabPage0.Controls.Add(Me._txtField_11)
        Me._tabVendor_TabPage0.Controls.Add(Me._txtField_0)
        resources.ApplyResources(Me._tabVendor_TabPage0, "_tabVendor_TabPage0")
        Me._tabVendor_TabPage0.Name = "_tabVendor_TabPage0"
        Me._tabVendor_TabPage0.UseVisualStyleBackColor = True
        '
        'cbxActive
        '
        resources.ApplyResources(Me.cbxActive, "cbxActive")
        Me.cbxActive.Name = "cbxActive"
        Me.cbxActive.UseVisualStyleBackColor = True
        '
        'CheckBoxAllowBarcodePOReport
        '
        resources.ApplyResources(Me.CheckBoxAllowBarcodePOReport, "CheckBoxAllowBarcodePOReport")
        Me.CheckBoxAllowBarcodePOReport.Name = "CheckBoxAllowBarcodePOReport"
        Me.CheckBoxAllowBarcodePOReport.UseVisualStyleBackColor = True
        '
        'cbxShortpayProhibited
        '
        resources.ApplyResources(Me.cbxShortpayProhibited, "cbxShortpayProhibited")
        Me.cbxShortpayProhibited.Name = "cbxShortpayProhibited"
        Me.cbxShortpayProhibited.UseVisualStyleBackColor = True
        '
        'DSDVendor_StoreSetup
        '
        resources.ApplyResources(Me.DSDVendor_StoreSetup, "DSDVendor_StoreSetup")
        Me.DSDVendor_StoreSetup.Name = "DSDVendor_StoreSetup"
        Me.DSDVendor_StoreSetup.UseVisualStyleBackColor = True
        '
        'CheckBox_EinvoiceReqd
        '
        resources.ApplyResources(Me.CheckBox_EinvoiceReqd, "CheckBox_EinvoiceReqd")
        Me.CheckBox_EinvoiceReqd.Name = "CheckBox_EinvoiceReqd"
        Me.CheckBox_EinvoiceReqd.UseVisualStyleBackColor = True
        '
        'txtAccountingContactEmail
        '
        Me.txtAccountingContactEmail.AcceptsReturn = True
        Me.txtAccountingContactEmail.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountingContactEmail.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtAccountingContactEmail, "txtAccountingContactEmail")
        Me.txtAccountingContactEmail.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountingContactEmail.Name = "txtAccountingContactEmail"
        Me.txtAccountingContactEmail.Tag = "String"
        '
        'lblAccountingContact
        '
        Me.lblAccountingContact.BackColor = System.Drawing.Color.Transparent
        Me.lblAccountingContact.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblAccountingContact, "lblAccountingContact")
        Me.lblAccountingContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountingContact.Name = "lblAccountingContact"
        '
        'CheckBox_EInvoicing
        '
        Me.CheckBox_EInvoicing.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.CheckBox_EInvoicing, "CheckBox_EInvoicing")
        Me.CheckBox_EInvoicing.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox_EInvoicing.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox_EInvoicing.Name = "CheckBox_EInvoicing"
        Me.CheckBox_EInvoicing.UseVisualStyleBackColor = False
        '
        '_txtField_32
        '
        Me._txtField_32.AcceptsReturn = True
        Me._txtField_32.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_32.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_32, "_txtField_32")
        Me._txtField_32.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_32, CType(32, Short))
        Me._txtField_32.Name = "_txtField_32"
        Me._txtField_32.Tag = "ExtCurrency"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Name = "Label3"
        '
        '_txtField_29
        '
        Me._txtField_29.AcceptsReturn = True
        Me._txtField_29.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_29.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_29, "_txtField_29")
        Me._txtField_29.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_29, CType(29, Short))
        Me._txtField_29.Name = "_txtField_29"
        Me._txtField_29.Tag = "String"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
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
        Me._txtField_5.Tag = "String"
        '
        '_txtField_25
        '
        Me._txtField_25.AcceptsReturn = True
        Me._txtField_25.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_25.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_25, "_txtField_25")
        Me._txtField_25.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_25, CType(25, Short))
        Me._txtField_25.Name = "_txtField_25"
        Me._txtField_25.Tag = "String"
        '
        '_chkField_2
        '
        Me._chkField_2.BackColor = System.Drawing.Color.Transparent
        Me._chkField_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._chkField_2, "_chkField_2")
        Me._chkField_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_2, CType(2, Short))
        Me._chkField_2.Name = "_chkField_2"
        Me._chkField_2.UseVisualStyleBackColor = False
        '
        '_chkField_1
        '
        Me._chkField_1.BackColor = System.Drawing.Color.Transparent
        Me._chkField_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._chkField_1, "_chkField_1")
        Me._chkField_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_1, CType(1, Short))
        Me._chkField_1.Name = "_chkField_1"
        Me._chkField_1.UseVisualStyleBackColor = False
        '
        '_chkField_0
        '
        Me._chkField_0.BackColor = System.Drawing.Color.Transparent
        Me._chkField_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._chkField_0, "_chkField_0")
        Me._chkField_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_0, CType(0, Short))
        Me._chkField_0.Name = "_chkField_0"
        Me._chkField_0.UseVisualStyleBackColor = False
        '
        '_txtField_8
        '
        Me._txtField_8.AcceptsReturn = True
        Me._txtField_8.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_8.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_8, "_txtField_8")
        Me._txtField_8.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_8, CType(8, Short))
        Me._txtField_8.Name = "_txtField_8"
        Me._txtField_8.Tag = "String"
        '
        'lblZip
        '
        Me.lblZip.BackColor = System.Drawing.Color.Transparent
        Me.lblZip.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblZip, "lblZip")
        Me.lblZip.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblZip.Name = "lblZip"
        '
        'txtCounty
        '
        Me.txtCounty.AcceptsReturn = True
        Me.txtCounty.BackColor = System.Drawing.SystemColors.Window
        Me.txtCounty.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCounty.ForeColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.txtCounty, "txtCounty")
        Me.txtCounty.Name = "txtCounty"
        Me.txtCounty.Tag = "String"
        '
        'lblState
        '
        Me.lblState.BackColor = System.Drawing.Color.Transparent
        Me.lblState.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblState, "lblState")
        Me.lblState.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblState.Name = "lblState"
        '
        'lblCity
        '
        Me.lblCity.BackColor = System.Drawing.Color.Transparent
        Me.lblCity.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblCity, "lblCity")
        Me.lblCity.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCity.Name = "lblCity"
        '
        'lblCountry
        '
        Me.lblCountry.BackColor = System.Drawing.Color.Transparent
        Me.lblCountry.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblCountry, "lblCountry")
        Me.lblCountry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCountry.Name = "lblCountry"
        '
        'lblPhone
        '
        Me.lblPhone.BackColor = System.Drawing.Color.Transparent
        Me.lblPhone.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPhone, "lblPhone")
        Me.lblPhone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPhone.Name = "lblPhone"
        '
        'lblAddress
        '
        Me.lblAddress.BackColor = System.Drawing.Color.Transparent
        Me.lblAddress.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblAddress, "lblAddress")
        Me.lblAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress.Name = "lblAddress"
        '
        'lblCompany
        '
        Me.lblCompany.BackColor = System.Drawing.Color.Transparent
        Me.lblCompany.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblCompany, "lblCompany")
        Me.lblCompany.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCompany.Name = "lblCompany"
        '
        'lblKey
        '
        Me.lblKey.BackColor = System.Drawing.Color.Transparent
        Me.lblKey.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblKey, "lblKey")
        Me.lblKey.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblKey.Name = "lblKey"
        '
        'lblExt
        '
        Me.lblExt.BackColor = System.Drawing.Color.Transparent
        Me.lblExt.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblExt, "lblExt")
        Me.lblExt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExt.Name = "lblExt"
        '
        'lblVendorID
        '
        Me.lblVendorID.BackColor = System.Drawing.Color.Transparent
        Me.lblVendorID.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblVendorID, "lblVendorID")
        Me.lblVendorID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVendorID.Name = "lblVendorID"
        '
        '_txtField_7
        '
        Me._txtField_7.AcceptsReturn = True
        Me._txtField_7.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_7.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_7, "_txtField_7")
        Me._txtField_7.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_7, CType(7, Short))
        Me._txtField_7.Name = "_txtField_7"
        Me._txtField_7.Tag = "String"
        '
        '_txtField_6
        '
        Me._txtField_6.AcceptsReturn = True
        Me._txtField_6.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_6.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_6, "_txtField_6")
        Me._txtField_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_6, CType(6, Short))
        Me._txtField_6.Name = "_txtField_6"
        Me._txtField_6.Tag = "String"
        '
        '_txtField_4
        '
        Me._txtField_4.AcceptsReturn = True
        Me._txtField_4.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_4.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_4, "_txtField_4")
        Me._txtField_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_4, CType(4, Short))
        Me._txtField_4.Name = "_txtField_4"
        Me._txtField_4.Tag = "String"
        '
        '_txtField_3
        '
        Me._txtField_3.AcceptsReturn = True
        Me._txtField_3.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_3.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_3, "_txtField_3")
        Me._txtField_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_3, CType(3, Short))
        Me._txtField_3.Name = "_txtField_3"
        Me._txtField_3.Tag = "String"
        '
        '_txtField_2
        '
        Me._txtField_2.AcceptsReturn = True
        Me._txtField_2.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_2.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_2, "_txtField_2")
        Me._txtField_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_2, CType(2, Short))
        Me._txtField_2.Name = "_txtField_2"
        Me._txtField_2.Tag = "String"
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
        '_txtField_11
        '
        Me._txtField_11.AcceptsReturn = True
        Me._txtField_11.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_11.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_11, "_txtField_11")
        Me._txtField_11.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_11, CType(11, Short))
        Me._txtField_11.Name = "_txtField_11"
        Me._txtField_11.Tag = "String"
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
        Me._txtField_0.ReadOnly = True
        Me._txtField_0.Tag = "String"
        '
        '_tabVendor_TabPage1
        '
        Me._tabVendor_TabPage1.BackColor = System.Drawing.Color.Transparent
        Me._tabVendor_TabPage1.Controls.Add(Me._txtField_16)
        Me._tabVendor_TabPage1.Controls.Add(Me._lblLabel_11)
        Me._tabVendor_TabPage1.Controls.Add(Me._lblLabel_12)
        Me._tabVendor_TabPage1.Controls.Add(Me._lblLabel_13)
        Me._tabVendor_TabPage1.Controls.Add(Me.txtPayToCounty)
        Me._tabVendor_TabPage1.Controls.Add(Me._lblLabel_14)
        Me._tabVendor_TabPage1.Controls.Add(Me._lblLabel_15)
        Me._tabVendor_TabPage1.Controls.Add(Me._lblLabel_16)
        Me._tabVendor_TabPage1.Controls.Add(Me._lblLabel_17)
        Me._tabVendor_TabPage1.Controls.Add(Me._lblLabel_18)
        Me._tabVendor_TabPage1.Controls.Add(Me._lblLabel_19)
        Me._tabVendor_TabPage1.Controls.Add(Me._lblLabel_29)
        Me._tabVendor_TabPage1.Controls.Add(Me._txtField_12)
        Me._tabVendor_TabPage1.Controls.Add(Me._txtField_13)
        Me._tabVendor_TabPage1.Controls.Add(Me._txtField_14)
        Me._tabVendor_TabPage1.Controls.Add(Me._txtField_15)
        Me._tabVendor_TabPage1.Controls.Add(Me._txtField_17)
        Me._tabVendor_TabPage1.Controls.Add(Me._txtField_18)
        Me._tabVendor_TabPage1.Controls.Add(Me._txtField_19)
        Me._tabVendor_TabPage1.Controls.Add(Me._txtField_20)
        Me._tabVendor_TabPage1.Controls.Add(Me._txtField_21)
        Me._tabVendor_TabPage1.Controls.Add(Me.cmdSameData)
        Me._tabVendor_TabPage1.Controls.Add(Me._txtField_26)
        Me._tabVendor_TabPage1.Controls.Add(Me.chkNonProductVendor)
        Me._tabVendor_TabPage1.Controls.Add(Me._chkField_3)
        resources.ApplyResources(Me._tabVendor_TabPage1, "_tabVendor_TabPage1")
        Me._tabVendor_TabPage1.Name = "_tabVendor_TabPage1"
        Me._tabVendor_TabPage1.UseVisualStyleBackColor = True
        '
        '_txtField_16
        '
        Me._txtField_16.AcceptsReturn = True
        Me._txtField_16.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_16.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_16, "_txtField_16")
        Me._txtField_16.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_16, CType(16, Short))
        Me._txtField_16.Name = "_txtField_16"
        Me._txtField_16.Tag = "String"
        '
        '_lblLabel_11
        '
        Me._lblLabel_11.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_11.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_11, "_lblLabel_11")
        Me._lblLabel_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_11.Name = "_lblLabel_11"
        '
        '_lblLabel_12
        '
        Me._lblLabel_12.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_12.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_12, "_lblLabel_12")
        Me._lblLabel_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_12.Name = "_lblLabel_12"
        '
        '_lblLabel_13
        '
        Me._lblLabel_13.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_13.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_13, "_lblLabel_13")
        Me._lblLabel_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_13.Name = "_lblLabel_13"
        '
        'txtPayToCounty
        '
        Me.txtPayToCounty.AcceptsReturn = True
        Me.txtPayToCounty.BackColor = System.Drawing.SystemColors.Window
        Me.txtPayToCounty.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPayToCounty.ForeColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.txtPayToCounty, "txtPayToCounty")
        Me.txtPayToCounty.Name = "txtPayToCounty"
        Me.txtPayToCounty.Tag = "String"
        '
        '_lblLabel_14
        '
        Me._lblLabel_14.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_14.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_14, "_lblLabel_14")
        Me._lblLabel_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_14.Name = "_lblLabel_14"
        '
        '_lblLabel_15
        '
        Me._lblLabel_15.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_15.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_15, "_lblLabel_15")
        Me._lblLabel_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_15.Name = "_lblLabel_15"
        '
        '_lblLabel_16
        '
        Me._lblLabel_16.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_16.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_16, "_lblLabel_16")
        Me._lblLabel_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_16.Name = "_lblLabel_16"
        '
        '_lblLabel_17
        '
        Me._lblLabel_17.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_17.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_17, "_lblLabel_17")
        Me._lblLabel_17.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_17.Name = "_lblLabel_17"
        '
        '_lblLabel_18
        '
        Me._lblLabel_18.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_18.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_18, "_lblLabel_18")
        Me._lblLabel_18.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_18.Name = "_lblLabel_18"
        '
        '_lblLabel_19
        '
        Me._lblLabel_19.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_19.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_19, "_lblLabel_19")
        Me._lblLabel_19.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_19.Name = "_lblLabel_19"
        '
        '_lblLabel_29
        '
        Me._lblLabel_29.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_29.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_29, "_lblLabel_29")
        Me._lblLabel_29.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_29.Name = "_lblLabel_29"
        '
        '_txtField_12
        '
        Me._txtField_12.AcceptsReturn = True
        Me._txtField_12.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_12.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_12, "_txtField_12")
        Me._txtField_12.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_12, CType(12, Short))
        Me._txtField_12.Name = "_txtField_12"
        Me._txtField_12.Tag = "String"
        '
        '_txtField_13
        '
        Me._txtField_13.AcceptsReturn = True
        Me._txtField_13.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_13.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_13, "_txtField_13")
        Me._txtField_13.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_13, CType(13, Short))
        Me._txtField_13.Name = "_txtField_13"
        Me._txtField_13.Tag = "String"
        '
        '_txtField_14
        '
        Me._txtField_14.AcceptsReturn = True
        Me._txtField_14.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_14.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_14, "_txtField_14")
        Me._txtField_14.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_14, CType(14, Short))
        Me._txtField_14.Name = "_txtField_14"
        Me._txtField_14.Tag = "String"
        '
        '_txtField_15
        '
        Me._txtField_15.AcceptsReturn = True
        Me._txtField_15.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_15.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_15, "_txtField_15")
        Me._txtField_15.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_15, CType(15, Short))
        Me._txtField_15.Name = "_txtField_15"
        Me._txtField_15.Tag = "String"
        '
        '_txtField_17
        '
        Me._txtField_17.AcceptsReturn = True
        Me._txtField_17.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_17.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_17, "_txtField_17")
        Me._txtField_17.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_17, CType(17, Short))
        Me._txtField_17.Name = "_txtField_17"
        Me._txtField_17.Tag = "String"
        '
        '_txtField_18
        '
        Me._txtField_18.AcceptsReturn = True
        Me._txtField_18.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_18.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_18, "_txtField_18")
        Me._txtField_18.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_18, CType(18, Short))
        Me._txtField_18.Name = "_txtField_18"
        Me._txtField_18.Tag = "String"
        '
        '_txtField_19
        '
        Me._txtField_19.AcceptsReturn = True
        Me._txtField_19.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_19.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_19, "_txtField_19")
        Me._txtField_19.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_19, CType(19, Short))
        Me._txtField_19.Name = "_txtField_19"
        Me._txtField_19.Tag = "String"
        '
        '_txtField_20
        '
        Me._txtField_20.AcceptsReturn = True
        Me._txtField_20.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_20.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_20, "_txtField_20")
        Me._txtField_20.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_20, CType(20, Short))
        Me._txtField_20.Name = "_txtField_20"
        Me._txtField_20.Tag = "String"
        '
        '_txtField_21
        '
        Me._txtField_21.AcceptsReturn = True
        Me._txtField_21.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_21.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_21, "_txtField_21")
        Me._txtField_21.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_21, CType(21, Short))
        Me._txtField_21.Name = "_txtField_21"
        Me._txtField_21.Tag = "String"
        '
        '_txtField_26
        '
        Me._txtField_26.AcceptsReturn = True
        Me._txtField_26.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_26.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_26, "_txtField_26")
        Me._txtField_26.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_26, CType(26, Short))
        Me._txtField_26.Name = "_txtField_26"
        Me._txtField_26.Tag = "String"
        '
        'chkNonProductVendor
        '
        Me.chkNonProductVendor.BackColor = System.Drawing.Color.Transparent
        Me.chkNonProductVendor.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkNonProductVendor, "chkNonProductVendor")
        Me.chkNonProductVendor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkNonProductVendor.Name = "chkNonProductVendor"
        Me.chkNonProductVendor.UseVisualStyleBackColor = False
        '
        '_chkField_3
        '
        Me._chkField_3.BackColor = System.Drawing.Color.Transparent
        Me._chkField_3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._chkField_3, "_chkField_3")
        Me._chkField_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_3, CType(3, Short))
        Me._chkField_3.Name = "_chkField_3"
        Me._chkField_3.UseVisualStyleBackColor = False
        '
        '_tabVendor_TabPage2
        '
        Me._tabVendor_TabPage2.BackColor = System.Drawing.Color.Transparent
        Me._tabVendor_TabPage2.Controls.Add(Me.ComboBox_PaymentTerms)
        Me._tabVendor_TabPage2.Controls.Add(Me.lblPaymentTerms)
        Me._tabVendor_TabPage2.Controls.Add(Me.cmbCurrency)
        Me._tabVendor_TabPage2.Controls.Add(Me.lblCurrency)
        Me._tabVendor_TabPage2.Controls.Add(Me.GroupBox1)
        Me._tabVendor_TabPage2.Controls.Add(Me.PictureBox1)
        Me._tabVendor_TabPage2.Controls.Add(Me.Label2)
        Me._tabVendor_TabPage2.Controls.Add(Me.TextBox_PSVendorExport)
        Me._tabVendor_TabPage2.Controls.Add(Me.Label_PSVendorExport)
        Me._tabVendor_TabPage2.Controls.Add(Me._lblLabel_22)
        Me._tabVendor_TabPage2.Controls.Add(Me._lblLabel_24)
        Me._tabVendor_TabPage2.Controls.Add(Me._lblLabel_20)
        Me._tabVendor_TabPage2.Controls.Add(Me._lblLabel_21)
        Me._tabVendor_TabPage2.Controls.Add(Me._txtField_22)
        Me._tabVendor_TabPage2.Controls.Add(Me._txtField_23)
        Me._tabVendor_TabPage2.Controls.Add(Me._txtField_24)
        Me._tabVendor_TabPage2.Controls.Add(Me._txtField_27)
        resources.ApplyResources(Me._tabVendor_TabPage2, "_tabVendor_TabPage2")
        Me._tabVendor_TabPage2.Name = "_tabVendor_TabPage2"
        Me._tabVendor_TabPage2.UseVisualStyleBackColor = True
        '
        'ComboBox_PaymentTerms
        '
        Me.ComboBox_PaymentTerms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_PaymentTerms.FormattingEnabled = True
        Me.ComboBox_PaymentTerms.Items.AddRange(New Object() {resources.GetString("ComboBox_PaymentTerms.Items"), resources.GetString("ComboBox_PaymentTerms.Items1"), resources.GetString("ComboBox_PaymentTerms.Items2")})
        resources.ApplyResources(Me.ComboBox_PaymentTerms, "ComboBox_PaymentTerms")
        Me.ComboBox_PaymentTerms.Name = "ComboBox_PaymentTerms"
        '
        'lblPaymentTerms
        '
        Me.lblPaymentTerms.BackColor = System.Drawing.Color.Transparent
        Me.lblPaymentTerms.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPaymentTerms, "lblPaymentTerms")
        Me.lblPaymentTerms.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentTerms.Name = "lblPaymentTerms"
        '
        'cmbCurrency
        '
        resources.ApplyResources(Me.cmbCurrency, "cmbCurrency")
        Me.cmbCurrency.FormattingEnabled = True
        Me.cmbCurrency.Name = "cmbCurrency"
        '
        'lblCurrency
        '
        resources.ApplyResources(Me.lblCurrency, "lblCurrency")
        Me.lblCurrency.BackColor = System.Drawing.Color.Transparent
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Name = "lblCurrency"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.StoreLevelOverridesButton)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'StoreLevelOverridesButton
        '
        resources.ApplyResources(Me.StoreLevelOverridesButton, "StoreLevelOverridesButton")
        Me.StoreLevelOverridesButton.Name = "StoreLevelOverridesButton"
        Me.StoreLevelOverridesButton.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        resources.ApplyResources(Me.PictureBox1, "PictureBox1")
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.TabStop = False
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'TextBox_PSVendorExport
        '
        Me.TextBox_PSVendorExport.AcceptsReturn = True
        Me.TextBox_PSVendorExport.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_PSVendorExport.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.TextBox_PSVendorExport, "TextBox_PSVendorExport")
        Me.TextBox_PSVendorExport.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox_PSVendorExport.Name = "TextBox_PSVendorExport"
        Me.TextBox_PSVendorExport.Tag = "String"
        '
        'Label_PSVendorExport
        '
        Me.Label_PSVendorExport.BackColor = System.Drawing.Color.Transparent
        Me.Label_PSVendorExport.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label_PSVendorExport, "Label_PSVendorExport")
        Me.Label_PSVendorExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_PSVendorExport.Name = "Label_PSVendorExport"
        '
        '_lblLabel_22
        '
        Me._lblLabel_22.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_22.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_22, "_lblLabel_22")
        Me._lblLabel_22.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_22.Name = "_lblLabel_22"
        '
        '_lblLabel_24
        '
        Me._lblLabel_24.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_24.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_24, "_lblLabel_24")
        Me._lblLabel_24.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_24.Name = "_lblLabel_24"
        '
        '_lblLabel_20
        '
        Me._lblLabel_20.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_20.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_20, "_lblLabel_20")
        Me._lblLabel_20.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_20.Name = "_lblLabel_20"
        '
        '_lblLabel_21
        '
        Me._lblLabel_21.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_21.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_21, "_lblLabel_21")
        Me._lblLabel_21.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_21.Name = "_lblLabel_21"
        '
        '_txtField_22
        '
        Me._txtField_22.AcceptsReturn = True
        Me._txtField_22.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_22.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_22, "_txtField_22")
        Me._txtField_22.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_22, CType(22, Short))
        Me._txtField_22.Name = "_txtField_22"
        Me._txtField_22.Tag = "String"
        '
        '_txtField_23
        '
        Me._txtField_23.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_23.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_23, "_txtField_23")
        Me._txtField_23.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_23, CType(23, Short))
        Me._txtField_23.Name = "_txtField_23"
        Me._txtField_23.Tag = "String"
        '
        '_txtField_24
        '
        Me._txtField_24.AcceptsReturn = True
        Me._txtField_24.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_24.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_24, "_txtField_24")
        Me._txtField_24.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_24, CType(24, Short))
        Me._txtField_24.Name = "_txtField_24"
        Me._txtField_24.Tag = "Number"
        '
        '_txtField_27
        '
        Me._txtField_27.AcceptsReturn = True
        Me._txtField_27.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_27.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_27, "_txtField_27")
        Me._txtField_27.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_27, CType(27, Short))
        Me._txtField_27.Name = "_txtField_27"
        Me._txtField_27.Tag = "String"
        '
        '_tabVendor_TabPage3
        '
        Me._tabVendor_TabPage3.BackColor = System.Drawing.Color.Transparent
        Me._tabVendor_TabPage3.Controls.Add(Me._lblLabel_6)
        Me._tabVendor_TabPage3.Controls.Add(Me._txtField_10)
        Me._tabVendor_TabPage3.Controls.Add(Me._lblLabel_27)
        Me._tabVendor_TabPage3.Controls.Add(Me._txtField_31)
        Me._tabVendor_TabPage3.Controls.Add(Me._lblLabel_26)
        Me._tabVendor_TabPage3.Controls.Add(Me._txtField_30)
        resources.ApplyResources(Me._tabVendor_TabPage3, "_tabVendor_TabPage3")
        Me._tabVendor_TabPage3.Name = "_tabVendor_TabPage3"
        Me._tabVendor_TabPage3.UseVisualStyleBackColor = True
        '
        '_lblLabel_6
        '
        Me._lblLabel_6.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_6.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_6, "_lblLabel_6")
        Me._lblLabel_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_6.Name = "_lblLabel_6"
        '
        '_txtField_10
        '
        Me._txtField_10.AcceptsReturn = True
        Me._txtField_10.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_10.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_10, "_txtField_10")
        Me._txtField_10.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_10, CType(10, Short))
        Me._txtField_10.Name = "_txtField_10"
        Me._txtField_10.Tag = "String"
        '
        '_lblLabel_27
        '
        Me._lblLabel_27.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_27.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_27, "_lblLabel_27")
        Me._lblLabel_27.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_27.Name = "_lblLabel_27"
        '
        '_txtField_31
        '
        Me._txtField_31.AcceptsReturn = True
        Me._txtField_31.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_31.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_31, "_txtField_31")
        Me._txtField_31.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_31, CType(31, Short))
        Me._txtField_31.Name = "_txtField_31"
        Me._txtField_31.Tag = "String"
        '
        '_lblLabel_26
        '
        Me._lblLabel_26.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_26.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_26, "_lblLabel_26")
        Me._lblLabel_26.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_26.Name = "_lblLabel_26"
        '
        '_txtField_30
        '
        Me._txtField_30.AcceptsReturn = True
        Me._txtField_30.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_30.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_30, "_txtField_30")
        Me._txtField_30.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_30, CType(30, Short))
        Me._txtField_30.Name = "_txtField_30"
        Me._txtField_30.Tag = "String"
        '
        '_tabVendor_TabPage4
        '
        Me._tabVendor_TabPage4.Controls.Add(Me._optPOTrans_3)
        Me._tabVendor_TabPage4.Controls.Add(Me._optPOTrans_2)
        Me._tabVendor_TabPage4.Controls.Add(Me._optPOTrans_1)
        Me._tabVendor_TabPage4.Controls.Add(Me.Label4)
        Me._tabVendor_TabPage4.Controls.Add(Me._optPOTrans_0)
        Me._tabVendor_TabPage4.Controls.Add(Me._txtField_9)
        Me._tabVendor_TabPage4.Controls.Add(Me._txtField_28)
        resources.ApplyResources(Me._tabVendor_TabPage4, "_tabVendor_TabPage4")
        Me._tabVendor_TabPage4.Name = "_tabVendor_TabPage4"
        Me._tabVendor_TabPage4.UseVisualStyleBackColor = True
        '
        '_optPOTrans_3
        '
        resources.ApplyResources(Me._optPOTrans_3, "_optPOTrans_3")
        Me.optPOTrans.SetIndex(Me._optPOTrans_3, CType(3, Short))
        Me._optPOTrans_3.Name = "_optPOTrans_3"
        Me._optPOTrans_3.TabStop = True
        Me._optPOTrans_3.UseVisualStyleBackColor = True
        '
        '_optPOTrans_2
        '
        resources.ApplyResources(Me._optPOTrans_2, "_optPOTrans_2")
        Me.optPOTrans.SetIndex(Me._optPOTrans_2, CType(2, Short))
        Me._optPOTrans_2.Name = "_optPOTrans_2"
        Me._optPOTrans_2.TabStop = True
        Me._optPOTrans_2.UseVisualStyleBackColor = True
        '
        '_optPOTrans_1
        '
        resources.ApplyResources(Me._optPOTrans_1, "_optPOTrans_1")
        Me.optPOTrans.SetIndex(Me._optPOTrans_1, CType(1, Short))
        Me._optPOTrans_1.Name = "_optPOTrans_1"
        Me._optPOTrans_1.TabStop = True
        Me._optPOTrans_1.UseVisualStyleBackColor = True
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        '_optPOTrans_0
        '
        resources.ApplyResources(Me._optPOTrans_0, "_optPOTrans_0")
        Me.optPOTrans.SetIndex(Me._optPOTrans_0, CType(0, Short))
        Me._optPOTrans_0.Name = "_optPOTrans_0"
        Me._optPOTrans_0.TabStop = True
        Me._optPOTrans_0.UseVisualStyleBackColor = True
        '
        '_txtField_9
        '
        Me._txtField_9.AcceptsReturn = True
        Me._txtField_9.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_9.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_9, "_txtField_9")
        Me._txtField_9.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_9, CType(9, Short))
        Me._txtField_9.Name = "_txtField_9"
        Me._txtField_9.Tag = "String"
        '
        '_txtField_28
        '
        Me._txtField_28.AcceptsReturn = True
        Me._txtField_28.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_28.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_28, "_txtField_28")
        Me._txtField_28.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_28, CType(28, Short))
        Me._txtField_28.Name = "_txtField_28"
        Me._txtField_28.Tag = "String"
        '
        '_tabVendor_POCostLeadTimeTab
        '
        Me._tabVendor_POCostLeadTimeTab.Controls.Add(Me.txtLeadTimeDays)
        Me._tabVendor_POCostLeadTimeTab.Controls.Add(Me.gbxDayOfWeek)
        Me._tabVendor_POCostLeadTimeTab.Controls.Add(Me.txtAuthorziedDate)
        Me._tabVendor_POCostLeadTimeTab.Controls.Add(Me.lblAuthorizedDate)
        Me._tabVendor_POCostLeadTimeTab.Controls.Add(Me.txtAuthorizedBy)
        Me._tabVendor_POCostLeadTimeTab.Controls.Add(Me.lblAuthorizedBy)
        Me._tabVendor_POCostLeadTimeTab.Controls.Add(Me.lblLeadTimeDays)
        Me._tabVendor_POCostLeadTimeTab.Controls.Add(Me.chkEnableLeadTime)
        Me._tabVendor_POCostLeadTimeTab.Controls.Add(Me.lblEnableLeadTimeCost)
        resources.ApplyResources(Me._tabVendor_POCostLeadTimeTab, "_tabVendor_POCostLeadTimeTab")
        Me._tabVendor_POCostLeadTimeTab.Name = "_tabVendor_POCostLeadTimeTab"
        Me._tabVendor_POCostLeadTimeTab.UseVisualStyleBackColor = True
        '
        'gbxDayOfWeek
        '
        Me.gbxDayOfWeek.Controls.Add(Me.cmbLeadTimeDayOfWeek)
        Me.gbxDayOfWeek.Controls.Add(Me.lblLeadTimeDayOfWeek)
        Me.gbxDayOfWeek.Controls.Add(Me.chkUseLeadTimeDayOfWeek)
        Me.gbxDayOfWeek.Controls.Add(Me.lblUseLeadTimeDayOfWeek)
        resources.ApplyResources(Me.gbxDayOfWeek, "gbxDayOfWeek")
        Me.gbxDayOfWeek.Name = "gbxDayOfWeek"
        Me.gbxDayOfWeek.TabStop = False
        '
        'cmbLeadTimeDayOfWeek
        '
        resources.ApplyResources(Me.cmbLeadTimeDayOfWeek, "cmbLeadTimeDayOfWeek")
        Me.cmbLeadTimeDayOfWeek.FormattingEnabled = True
        Me.cmbLeadTimeDayOfWeek.Items.AddRange(New Object() {resources.GetString("cmbLeadTimeDayOfWeek.Items"), resources.GetString("cmbLeadTimeDayOfWeek.Items1"), resources.GetString("cmbLeadTimeDayOfWeek.Items2"), resources.GetString("cmbLeadTimeDayOfWeek.Items3"), resources.GetString("cmbLeadTimeDayOfWeek.Items4"), resources.GetString("cmbLeadTimeDayOfWeek.Items5"), resources.GetString("cmbLeadTimeDayOfWeek.Items6")})
        Me.cmbLeadTimeDayOfWeek.Name = "cmbLeadTimeDayOfWeek"
        '
        'chkUseLeadTimeDayOfWeek
        '
        resources.ApplyResources(Me.chkUseLeadTimeDayOfWeek, "chkUseLeadTimeDayOfWeek")
        Me.chkUseLeadTimeDayOfWeek.Name = "chkUseLeadTimeDayOfWeek"
        Me.chkUseLeadTimeDayOfWeek.UseVisualStyleBackColor = True
        '
        'lblUseLeadTimeDayOfWeek
        '
        resources.ApplyResources(Me.lblUseLeadTimeDayOfWeek, "lblUseLeadTimeDayOfWeek")
        Me.lblUseLeadTimeDayOfWeek.Name = "lblUseLeadTimeDayOfWeek"
        '
        'txtAuthorziedDate
        '
        resources.ApplyResources(Me.txtAuthorziedDate, "txtAuthorziedDate")
        Me.txtAuthorziedDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAuthorziedDate.Name = "txtAuthorziedDate"
        Me.txtAuthorziedDate.ReadOnly = True
        '
        'lblAuthorizedDate
        '
        resources.ApplyResources(Me.lblAuthorizedDate, "lblAuthorizedDate")
        Me.lblAuthorizedDate.Name = "lblAuthorizedDate"
        '
        'txtAuthorizedBy
        '
        resources.ApplyResources(Me.txtAuthorizedBy, "txtAuthorizedBy")
        Me.txtAuthorizedBy.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAuthorizedBy.Name = "txtAuthorizedBy"
        Me.txtAuthorizedBy.ReadOnly = True
        '
        'lblAuthorizedBy
        '
        resources.ApplyResources(Me.lblAuthorizedBy, "lblAuthorizedBy")
        Me.lblAuthorizedBy.Name = "lblAuthorizedBy"
        '
        'lblLeadTimeDays
        '
        resources.ApplyResources(Me.lblLeadTimeDays, "lblLeadTimeDays")
        Me.lblLeadTimeDays.Name = "lblLeadTimeDays"
        '
        'chkEnableLeadTime
        '
        resources.ApplyResources(Me.chkEnableLeadTime, "chkEnableLeadTime")
        Me.chkEnableLeadTime.Name = "chkEnableLeadTime"
        Me.chkEnableLeadTime.UseVisualStyleBackColor = True
        '
        'lblEnableLeadTimeCost
        '
        resources.ApplyResources(Me.lblEnableLeadTimeCost, "lblEnableLeadTimeCost")
        Me.lblEnableLeadTimeCost.Name = "lblEnableLeadTimeCost"
        '
        '_cmbField_0
        '
        Me._cmbField_0.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_0.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_0.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me._cmbField_0, "_cmbField_0")
        Me._cmbField_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_0, CType(0, Short))
        Me._cmbField_0.Name = "_cmbField_0"
        Me._cmbField_0.Sorted = True
        '
        'lblReadOnly
        '
        resources.ApplyResources(Me.lblReadOnly, "lblReadOnly")
        Me.lblReadOnly.BackColor = System.Drawing.Color.Transparent
        Me.lblReadOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReadOnly.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblReadOnly.Name = "lblReadOnly"
        '
        'chkField
        '
        '
        'cmbField
        '
        '
        'txtField
        '
        '
        '_label_ConfiguredAs
        '
        resources.ApplyResources(Me._label_ConfiguredAs, "_label_ConfiguredAs")
        Me._label_ConfiguredAs.Name = "_label_ConfiguredAs"
        '
        'BackgroundWorker1
        '
        Me.BackgroundWorker1.WorkerReportsProgress = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripDropDownButton1})
        resources.ApplyResources(Me.StatusStrip1, "StatusStrip1")
        Me.StatusStrip1.Name = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        resources.ApplyResources(Me.ToolStripStatusLabel1, "ToolStripStatusLabel1")
        Me.ToolStripStatusLabel1.Spring = True
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        resources.ApplyResources(Me.ToolStripDropDownButton1, "ToolStripDropDownButton1")
        Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        '
        'frmVendor
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me._label_ConfiguredAs)
        Me.Controls.Add(Me.cmdCompanySearch)
        Me.Controls.Add(Me.cmdUnlock)
        Me.Controls.Add(Me.tabVendor)
        Me.Controls.Add(Me.cmdItems)
        Me.Controls.Add(Me.cmdContacts)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.lblReadOnly)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmVendor"
        Me.ShowInTaskbar = False
        Me.tabVendor.ResumeLayout(False)
        Me._tabVendor_TabPage0.ResumeLayout(False)
        Me._tabVendor_TabPage0.PerformLayout()
        Me._tabVendor_TabPage1.ResumeLayout(False)
        Me._tabVendor_TabPage1.PerformLayout()
        Me._tabVendor_TabPage2.ResumeLayout(False)
        Me._tabVendor_TabPage2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabVendor_TabPage3.ResumeLayout(False)
        Me._tabVendor_TabPage3.PerformLayout()
        Me._tabVendor_TabPage4.ResumeLayout(False)
        Me._tabVendor_TabPage4.PerformLayout()
        Me._tabVendor_POCostLeadTimeTab.ResumeLayout(False)
        Me._tabVendor_POCostLeadTimeTab.PerformLayout()
        Me.gbxDayOfWeek.ResumeLayout(False)
        Me.gbxDayOfWeek.PerformLayout()
        CType(Me.chkField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optPOTrans, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents txtCounty As System.Windows.Forms.TextBox
    Public WithEvents txtPayToCounty As System.Windows.Forms.TextBox
    Public WithEvents _txtField_5 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_16 As System.Windows.Forms.TextBox
    Friend WithEvents _tabVendor_TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents _lblLabel_26 As System.Windows.Forms.Label
    Public WithEvents _txtField_30 As System.Windows.Forms.TextBox
    Public WithEvents _lblLabel_27 As System.Windows.Forms.Label
    Public WithEvents _txtField_31 As System.Windows.Forms.TextBox
    Public WithEvents _lblLabel_6 As System.Windows.Forms.Label
    Public WithEvents _txtField_10 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_29 As System.Windows.Forms.TextBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents TextBox_PSVendorExport As System.Windows.Forms.TextBox
    Public WithEvents Label_PSVendorExport As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents _label_ConfiguredAs As System.Windows.Forms.Label
    Public WithEvents _txtField_32 As System.Windows.Forms.TextBox
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents CheckBox_EInvoicing As System.Windows.Forms.CheckBox
    Friend WithEvents _tabVendor_TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents _optPOTrans_2 As System.Windows.Forms.RadioButton
    Friend WithEvents _optPOTrans_1 As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents _optPOTrans_0 As System.Windows.Forms.RadioButton
    Public WithEvents _txtField_9 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_28 As System.Windows.Forms.TextBox
    Friend WithEvents _optPOTrans_3 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents StoreLevelOverridesButton As System.Windows.Forms.Button
    Friend WithEvents cmbCurrency As System.Windows.Forms.ComboBox
    Public WithEvents lblCurrency As System.Windows.Forms.Label
    Public WithEvents txtAccountingContactEmail As System.Windows.Forms.TextBox
    Public WithEvents lblAccountingContact As System.Windows.Forms.Label
    Friend WithEvents UltraDataSource1 As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Public WithEvents lblPaymentTerms As System.Windows.Forms.Label
    Friend WithEvents ComboBox_PaymentTerms As System.Windows.Forms.ComboBox
    Friend WithEvents _tabVendor_POCostLeadTimeTab As System.Windows.Forms.TabPage
    Friend WithEvents lblLeadTimeDayOfWeek As System.Windows.Forms.Label
    Friend WithEvents chkUseLeadTimeDayOfWeek As System.Windows.Forms.CheckBox
    Friend WithEvents lblUseLeadTimeDayOfWeek As System.Windows.Forms.Label
    Friend WithEvents lblLeadTimeDays As System.Windows.Forms.Label
    Friend WithEvents chkEnableLeadTime As System.Windows.Forms.CheckBox
    Friend WithEvents lblEnableLeadTimeCost As System.Windows.Forms.Label
    Friend WithEvents txtAuthorziedDate As System.Windows.Forms.TextBox
    Friend WithEvents lblAuthorizedDate As System.Windows.Forms.Label
    Friend WithEvents txtAuthorizedBy As System.Windows.Forms.TextBox
    Friend WithEvents lblAuthorizedBy As System.Windows.Forms.Label
    Friend WithEvents cmbLeadTimeDayOfWeek As System.Windows.Forms.ComboBox
    Friend WithEvents gbxDayOfWeek As System.Windows.Forms.GroupBox
    Friend WithEvents txtLeadTimeDays As System.Windows.Forms.MaskedTextBox
    Friend WithEvents CheckBox_EinvoiceReqd As System.Windows.Forms.CheckBox
    Friend WithEvents DSDVendor_StoreSetup As System.Windows.Forms.Button
    Friend WithEvents CheckBoxAllowReceiveAll As System.Windows.Forms.CheckBox
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripDropDownButton1 As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents cbxShortpayProhibited As System.Windows.Forms.CheckBox
    Friend WithEvents cbxActive As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxAllowBarcodePOReport As System.Windows.Forms.CheckBox
#End Region
End Class
