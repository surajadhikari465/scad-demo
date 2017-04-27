<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StoreAdd
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
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Properties_GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtGeoCode = New System.Windows.Forms.TextBox()
        Me.lblPlumStoreNo = New System.Windows.Forms.Label()
        Me.txtPlumStoreNo = New System.Windows.Forms.TextBox()
        Me.lblPSIStoreNo = New System.Windows.Forms.Label()
        Me.txtPSIStoreNo = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.grpOrderingReceiving = New System.Windows.Forms.GroupBox()
        Me.txtVendorState = New System.Windows.Forms.TextBox()
        Me.lblPeopleSoftID = New System.Windows.Forms.Label()
        Me.txtPeopleSoftID = New System.Windows.Forms.TextBox()
        Me.lblVendorState = New System.Windows.Forms.Label()
        Me.lblVendorAddress = New System.Windows.Forms.Label()
        Me.txtVendorAddress = New System.Windows.Forms.TextBox()
        Me.lblVendorZip = New System.Windows.Forms.Label()
        Me.txtVendorZip = New System.Windows.Forms.TextBox()
        Me.lblVendorCity = New System.Windows.Forms.Label()
        Me.txtVendorCity = New System.Windows.Forms.TextBox()
        Me.lblVendorName = New System.Windows.Forms.Label()
        Me.txtVendorName = New System.Windows.Forms.TextBox()
        Me.lblBusinessUnitID = New System.Windows.Forms.Label()
        Me.txtBusinessUnitID = New System.Windows.Forms.TextBox()
        Me.lblStoreAbbrev = New System.Windows.Forms.Label()
        Me.txtStoreAbbrev = New System.Windows.Forms.TextBox()
        Me.lblStoreNumber = New System.Windows.Forms.Label()
        Me.txtStoreNumber = New System.Windows.Forms.TextBox()
        Me.lblTaxJurisdiction = New System.Windows.Forms.Label()
        Me.cboTaxJurisdiction = New System.Windows.Forms.ComboBox()
        Me.cboStoreJurisdiction = New System.Windows.Forms.ComboBox()
        Me.lblStoreJurisdiction = New System.Windows.Forms.Label()
        Me.cboZone = New System.Windows.Forms.ComboBox()
        Me.lblStoreName = New System.Windows.Forms.Label()
        Me.grpPendingPriceOptions = New System.Windows.Forms.GroupBox()
        Me.chkIncPromoPlanner = New System.Windows.Forms.CheckBox()
        Me.chkIncSlim = New System.Windows.Forms.CheckBox()
        Me.chkIncFutureSale = New System.Windows.Forms.CheckBox()
        Me.txtStoreName = New System.Windows.Forms.TextBox()
        Me.GroupBox_DataSourceOptions = New System.Windows.Forms.GroupBox()
        Me.dgvSubStoreSubteam = New System.Windows.Forms.DataGridView()
        Me.btnRemoveStoreSub = New System.Windows.Forms.Button()
        Me.btnAddSubStore = New System.Windows.Forms.Button()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.lblSubSubTeam = New System.Windows.Forms.Label()
        Me.cboPOSSubTeam = New System.Windows.Forms.ComboBox()
        Me.lblSubStore = New System.Windows.Forms.Label()
        Me.cboSubStore = New System.Windows.Forms.ComboBox()
        Me.lblISSPriceChgType = New System.Windows.Forms.Label()
        Me.cboISSPriceChgType = New System.Windows.Forms.ComboBox()
        Me.lblSourceStore = New System.Windows.Forms.Label()
        Me.cboSourceStore = New System.Windows.Forms.ComboBox()
        Me.lblPOSWriter = New System.Windows.Forms.Label()
        Me.cboPOSWriter = New System.Windows.Forms.ComboBox()
        Me.lblZone = New System.Windows.Forms.Label()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.SubTeamSubstitutionListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Properties_GroupBox1.SuspendLayout()
        Me.grpOrderingReceiving.SuspendLayout()
        Me.grpPendingPriceOptions.SuspendLayout()
        Me.GroupBox_DataSourceOptions.SuspendLayout()
        CType(Me.dgvSubStoreSubteam, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SubTeamSubstitutionListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(486, 586)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 26
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(568, 586)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 27
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Properties_GroupBox1
        '
        Me.Properties_GroupBox1.Controls.Add(Me.Label1)
        Me.Properties_GroupBox1.Controls.Add(Me.txtGeoCode)
        Me.Properties_GroupBox1.Controls.Add(Me.lblPlumStoreNo)
        Me.Properties_GroupBox1.Controls.Add(Me.txtPlumStoreNo)
        Me.Properties_GroupBox1.Controls.Add(Me.lblPSIStoreNo)
        Me.Properties_GroupBox1.Controls.Add(Me.txtPSIStoreNo)
        Me.Properties_GroupBox1.Controls.Add(Me.Label16)
        Me.Properties_GroupBox1.Controls.Add(Me.grpOrderingReceiving)
        Me.Properties_GroupBox1.Controls.Add(Me.lblBusinessUnitID)
        Me.Properties_GroupBox1.Controls.Add(Me.txtBusinessUnitID)
        Me.Properties_GroupBox1.Controls.Add(Me.lblStoreAbbrev)
        Me.Properties_GroupBox1.Controls.Add(Me.txtStoreAbbrev)
        Me.Properties_GroupBox1.Controls.Add(Me.lblStoreNumber)
        Me.Properties_GroupBox1.Controls.Add(Me.txtStoreNumber)
        Me.Properties_GroupBox1.Controls.Add(Me.lblTaxJurisdiction)
        Me.Properties_GroupBox1.Controls.Add(Me.cboTaxJurisdiction)
        Me.Properties_GroupBox1.Controls.Add(Me.cboStoreJurisdiction)
        Me.Properties_GroupBox1.Controls.Add(Me.lblStoreJurisdiction)
        Me.Properties_GroupBox1.Controls.Add(Me.cboZone)
        Me.Properties_GroupBox1.Controls.Add(Me.lblStoreName)
        Me.Properties_GroupBox1.Controls.Add(Me.grpPendingPriceOptions)
        Me.Properties_GroupBox1.Controls.Add(Me.txtStoreName)
        Me.Properties_GroupBox1.Controls.Add(Me.GroupBox_DataSourceOptions)
        Me.Properties_GroupBox1.Controls.Add(Me.lblZone)
        Me.Properties_GroupBox1.Location = New System.Drawing.Point(12, 3)
        Me.Properties_GroupBox1.Name = "Properties_GroupBox1"
        Me.Properties_GroupBox1.Size = New System.Drawing.Size(627, 572)
        Me.Properties_GroupBox1.TabIndex = 0
        Me.Properties_GroupBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(323, 97)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 62
        Me.Label1.Text = "Geo Code"
        '
        'txtGeoCode
        '
        Me.txtGeoCode.Location = New System.Drawing.Point(453, 94)
        Me.txtGeoCode.MaxLength = 15
        Me.txtGeoCode.Name = "txtGeoCode"
        Me.txtGeoCode.Size = New System.Drawing.Size(100, 22)
        Me.txtGeoCode.TabIndex = 8
        '
        'lblPlumStoreNo
        '
        Me.lblPlumStoreNo.AutoSize = True
        Me.lblPlumStoreNo.Location = New System.Drawing.Point(323, 123)
        Me.lblPlumStoreNo.Name = "lblPlumStoreNo"
        Me.lblPlumStoreNo.Size = New System.Drawing.Size(80, 13)
        Me.lblPlumStoreNo.TabIndex = 60
        Me.lblPlumStoreNo.Text = "Plum Store No"
        '
        'txtPlumStoreNo
        '
        Me.txtPlumStoreNo.Location = New System.Drawing.Point(453, 120)
        Me.txtPlumStoreNo.MaxLength = 20
        Me.txtPlumStoreNo.Name = "txtPlumStoreNo"
        Me.txtPlumStoreNo.Size = New System.Drawing.Size(56, 22)
        Me.txtPlumStoreNo.TabIndex = 10
        '
        'lblPSIStoreNo
        '
        Me.lblPSIStoreNo.AutoSize = True
        Me.lblPSIStoreNo.Location = New System.Drawing.Point(17, 123)
        Me.lblPSIStoreNo.Name = "lblPSIStoreNo"
        Me.lblPSIStoreNo.Size = New System.Drawing.Size(70, 13)
        Me.lblPSIStoreNo.TabIndex = 58
        Me.lblPSIStoreNo.Text = "PSI Store No"
        '
        'txtPSIStoreNo
        '
        Me.txtPSIStoreNo.Location = New System.Drawing.Point(147, 120)
        Me.txtPSIStoreNo.MaxLength = 20
        Me.txtPSIStoreNo.Name = "txtPSIStoreNo"
        Me.txtPSIStoreNo.Size = New System.Drawing.Size(56, 22)
        Me.txtPSIStoreNo.TabIndex = 9
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.ForeColor = System.Drawing.Color.Black
        Me.Label16.Location = New System.Drawing.Point(17, 556)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(136, 13)
        Me.Label16.TabIndex = 56
        Me.Label16.Text = "* Denotes Required Field"
        '
        'grpOrderingReceiving
        '
        Me.grpOrderingReceiving.Controls.Add(Me.txtVendorState)
        Me.grpOrderingReceiving.Controls.Add(Me.lblPeopleSoftID)
        Me.grpOrderingReceiving.Controls.Add(Me.txtPeopleSoftID)
        Me.grpOrderingReceiving.Controls.Add(Me.lblVendorState)
        Me.grpOrderingReceiving.Controls.Add(Me.lblVendorAddress)
        Me.grpOrderingReceiving.Controls.Add(Me.txtVendorAddress)
        Me.grpOrderingReceiving.Controls.Add(Me.lblVendorZip)
        Me.grpOrderingReceiving.Controls.Add(Me.txtVendorZip)
        Me.grpOrderingReceiving.Controls.Add(Me.lblVendorCity)
        Me.grpOrderingReceiving.Controls.Add(Me.txtVendorCity)
        Me.grpOrderingReceiving.Controls.Add(Me.lblVendorName)
        Me.grpOrderingReceiving.Controls.Add(Me.txtVendorName)
        Me.grpOrderingReceiving.Location = New System.Drawing.Point(15, 456)
        Me.grpOrderingReceiving.Name = "grpOrderingReceiving"
        Me.grpOrderingReceiving.Size = New System.Drawing.Size(606, 100)
        Me.grpOrderingReceiving.TabIndex = 32
        Me.grpOrderingReceiving.TabStop = False
        Me.grpOrderingReceiving.Text = "Ordering and Receiving Information"
        '
        'txtVendorState
        '
        Me.ErrorProvider1.SetError(Me.txtVendorState, "True")
        Me.txtVendorState.Location = New System.Drawing.Point(438, 44)
        Me.txtVendorState.MaxLength = 2
        Me.txtVendorState.Name = "txtVendorState"
        Me.txtVendorState.Size = New System.Drawing.Size(31, 22)
        Me.txtVendorState.TabIndex = 23
        '
        'lblPeopleSoftID
        '
        Me.lblPeopleSoftID.AutoSize = True
        Me.lblPeopleSoftID.Location = New System.Drawing.Point(308, 74)
        Me.lblPeopleSoftID.Name = "lblPeopleSoftID"
        Me.lblPeopleSoftID.Size = New System.Drawing.Size(77, 13)
        Me.lblPeopleSoftID.TabIndex = 54
        Me.lblPeopleSoftID.Text = "PeopleSoft ID"
        '
        'txtPeopleSoftID
        '
        Me.ErrorProvider1.SetError(Me.txtPeopleSoftID, "True")
        Me.txtPeopleSoftID.Location = New System.Drawing.Point(438, 71)
        Me.txtPeopleSoftID.MaxLength = 20
        Me.txtPeopleSoftID.Name = "txtPeopleSoftID"
        Me.txtPeopleSoftID.Size = New System.Drawing.Size(143, 22)
        Me.txtPeopleSoftID.TabIndex = 25
        '
        'lblVendorState
        '
        Me.lblVendorState.AutoSize = True
        Me.lblVendorState.Location = New System.Drawing.Point(308, 48)
        Me.lblVendorState.Name = "lblVendorState"
        Me.lblVendorState.Size = New System.Drawing.Size(79, 13)
        Me.lblVendorState.TabIndex = 52
        Me.lblVendorState.Text = "Vendor State*"
        '
        'lblVendorAddress
        '
        Me.lblVendorAddress.AutoSize = True
        Me.lblVendorAddress.Location = New System.Drawing.Point(308, 22)
        Me.lblVendorAddress.Name = "lblVendorAddress"
        Me.lblVendorAddress.Size = New System.Drawing.Size(94, 13)
        Me.lblVendorAddress.TabIndex = 50
        Me.lblVendorAddress.Text = "Vendor Address*"
        '
        'txtVendorAddress
        '
        Me.ErrorProvider1.SetError(Me.txtVendorAddress, "True")
        Me.txtVendorAddress.Location = New System.Drawing.Point(438, 19)
        Me.txtVendorAddress.MaxLength = 20
        Me.txtVendorAddress.Name = "txtVendorAddress"
        Me.txtVendorAddress.Size = New System.Drawing.Size(143, 22)
        Me.txtVendorAddress.TabIndex = 21
        '
        'lblVendorZip
        '
        Me.lblVendorZip.AutoSize = True
        Me.lblVendorZip.Location = New System.Drawing.Point(2, 74)
        Me.lblVendorZip.Name = "lblVendorZip"
        Me.lblVendorZip.Size = New System.Drawing.Size(99, 13)
        Me.lblVendorZip.TabIndex = 48
        Me.lblVendorZip.Text = "Vendor Zip Code*"
        '
        'txtVendorZip
        '
        Me.ErrorProvider1.SetError(Me.txtVendorZip, "True")
        Me.txtVendorZip.Location = New System.Drawing.Point(132, 71)
        Me.txtVendorZip.MaxLength = 20
        Me.txtVendorZip.Name = "txtVendorZip"
        Me.txtVendorZip.Size = New System.Drawing.Size(143, 22)
        Me.txtVendorZip.TabIndex = 24
        '
        'lblVendorCity
        '
        Me.lblVendorCity.AutoSize = True
        Me.lblVendorCity.Location = New System.Drawing.Point(2, 48)
        Me.lblVendorCity.Name = "lblVendorCity"
        Me.lblVendorCity.Size = New System.Drawing.Size(72, 13)
        Me.lblVendorCity.TabIndex = 46
        Me.lblVendorCity.Text = "Vendor City*"
        '
        'txtVendorCity
        '
        Me.ErrorProvider1.SetError(Me.txtVendorCity, "True")
        Me.txtVendorCity.Location = New System.Drawing.Point(132, 44)
        Me.txtVendorCity.MaxLength = 20
        Me.txtVendorCity.Name = "txtVendorCity"
        Me.txtVendorCity.Size = New System.Drawing.Size(143, 22)
        Me.txtVendorCity.TabIndex = 22
        '
        'lblVendorName
        '
        Me.lblVendorName.AutoSize = True
        Me.lblVendorName.Location = New System.Drawing.Point(2, 22)
        Me.lblVendorName.Name = "lblVendorName"
        Me.lblVendorName.Size = New System.Drawing.Size(82, 13)
        Me.lblVendorName.TabIndex = 44
        Me.lblVendorName.Text = "Vendor Name*"
        '
        'txtVendorName
        '
        Me.ErrorProvider1.SetError(Me.txtVendorName, "True")
        Me.txtVendorName.Location = New System.Drawing.Point(132, 19)
        Me.txtVendorName.MaxLength = 20
        Me.txtVendorName.Name = "txtVendorName"
        Me.txtVendorName.Size = New System.Drawing.Size(143, 22)
        Me.txtVendorName.TabIndex = 20
        '
        'lblBusinessUnitID
        '
        Me.lblBusinessUnitID.AutoSize = True
        Me.lblBusinessUnitID.Location = New System.Drawing.Point(17, 97)
        Me.lblBusinessUnitID.Name = "lblBusinessUnitID"
        Me.lblBusinessUnitID.Size = New System.Drawing.Size(96, 13)
        Me.lblBusinessUnitID.TabIndex = 42
        Me.lblBusinessUnitID.Text = "Business Unit ID*"
        '
        'txtBusinessUnitID
        '
        Me.ErrorProvider1.SetError(Me.txtBusinessUnitID, "True")
        Me.txtBusinessUnitID.Location = New System.Drawing.Point(147, 94)
        Me.txtBusinessUnitID.MaxLength = 20
        Me.txtBusinessUnitID.Name = "txtBusinessUnitID"
        Me.txtBusinessUnitID.Size = New System.Drawing.Size(56, 22)
        Me.txtBusinessUnitID.TabIndex = 7
        '
        'lblStoreAbbrev
        '
        Me.lblStoreAbbrev.AutoSize = True
        Me.lblStoreAbbrev.Location = New System.Drawing.Point(323, 16)
        Me.lblStoreAbbrev.Name = "lblStoreAbbrev"
        Me.lblStoreAbbrev.Size = New System.Drawing.Size(108, 13)
        Me.lblStoreAbbrev.TabIndex = 36
        Me.lblStoreAbbrev.Text = "Store Abbreviation*"
        '
        'txtStoreAbbrev
        '
        Me.ErrorProvider1.SetError(Me.txtStoreAbbrev, "True")
        Me.txtStoreAbbrev.Location = New System.Drawing.Point(453, 13)
        Me.txtStoreAbbrev.MaxLength = 3
        Me.txtStoreAbbrev.Name = "txtStoreAbbrev"
        Me.txtStoreAbbrev.Size = New System.Drawing.Size(46, 22)
        Me.txtStoreAbbrev.TabIndex = 2
        '
        'lblStoreNumber
        '
        Me.lblStoreNumber.AutoSize = True
        Me.lblStoreNumber.Location = New System.Drawing.Point(17, 16)
        Me.lblStoreNumber.Name = "lblStoreNumber"
        Me.lblStoreNumber.Size = New System.Drawing.Size(83, 13)
        Me.lblStoreNumber.TabIndex = 34
        Me.lblStoreNumber.Text = "Store Number*"
        '
        'txtStoreNumber
        '
        Me.ErrorProvider1.SetError(Me.txtStoreNumber, "True")
        Me.txtStoreNumber.Location = New System.Drawing.Point(147, 13)
        Me.txtStoreNumber.MaxLength = 20
        Me.txtStoreNumber.Name = "txtStoreNumber"
        Me.txtStoreNumber.Size = New System.Drawing.Size(40, 22)
        Me.txtStoreNumber.TabIndex = 1
        '
        'lblTaxJurisdiction
        '
        Me.lblTaxJurisdiction.AutoSize = True
        Me.lblTaxJurisdiction.Location = New System.Drawing.Point(323, 66)
        Me.lblTaxJurisdiction.Name = "lblTaxJurisdiction"
        Me.lblTaxJurisdiction.Size = New System.Drawing.Size(90, 13)
        Me.lblTaxJurisdiction.TabIndex = 30
        Me.lblTaxJurisdiction.Text = "Tax Jurisdiction*"
        '
        'cboTaxJurisdiction
        '
        Me.cboTaxJurisdiction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTaxJurisdiction.FormattingEnabled = True
        Me.cboTaxJurisdiction.Location = New System.Drawing.Point(453, 63)
        Me.cboTaxJurisdiction.Name = "cboTaxJurisdiction"
        Me.cboTaxJurisdiction.Size = New System.Drawing.Size(143, 21)
        Me.cboTaxJurisdiction.TabIndex = 6
        '
        'cboStoreJurisdiction
        '
        Me.cboStoreJurisdiction.DisplayMember = "StoreJurisdictionID"
        Me.cboStoreJurisdiction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStoreJurisdiction.FormattingEnabled = True
        Me.cboStoreJurisdiction.Location = New System.Drawing.Point(453, 37)
        Me.cboStoreJurisdiction.Name = "cboStoreJurisdiction"
        Me.cboStoreJurisdiction.Size = New System.Drawing.Size(143, 21)
        Me.cboStoreJurisdiction.TabIndex = 4
        Me.cboStoreJurisdiction.ValueMember = "StoreJurisdictionID"
        '
        'lblStoreJurisdiction
        '
        Me.lblStoreJurisdiction.AutoSize = True
        Me.lblStoreJurisdiction.Location = New System.Drawing.Point(323, 40)
        Me.lblStoreJurisdiction.Name = "lblStoreJurisdiction"
        Me.lblStoreJurisdiction.Size = New System.Drawing.Size(101, 13)
        Me.lblStoreJurisdiction.TabIndex = 27
        Me.lblStoreJurisdiction.Text = "Store Jurisdiction*"
        '
        'cboZone
        '
        Me.cboZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboZone.FormattingEnabled = True
        Me.cboZone.Location = New System.Drawing.Point(147, 66)
        Me.cboZone.Name = "cboZone"
        Me.cboZone.Size = New System.Drawing.Size(143, 21)
        Me.cboZone.TabIndex = 5
        '
        'lblStoreName
        '
        Me.lblStoreName.AutoSize = True
        Me.lblStoreName.Location = New System.Drawing.Point(17, 43)
        Me.lblStoreName.Name = "lblStoreName"
        Me.lblStoreName.Size = New System.Drawing.Size(71, 13)
        Me.lblStoreName.TabIndex = 25
        Me.lblStoreName.Text = "Store Name*"
        '
        'grpPendingPriceOptions
        '
        Me.grpPendingPriceOptions.Controls.Add(Me.chkIncPromoPlanner)
        Me.grpPendingPriceOptions.Controls.Add(Me.chkIncSlim)
        Me.grpPendingPriceOptions.Controls.Add(Me.chkIncFutureSale)
        Me.grpPendingPriceOptions.Location = New System.Drawing.Point(15, 400)
        Me.grpPendingPriceOptions.Name = "grpPendingPriceOptions"
        Me.grpPendingPriceOptions.Size = New System.Drawing.Size(606, 50)
        Me.grpPendingPriceOptions.TabIndex = 31
        Me.grpPendingPriceOptions.TabStop = False
        Me.grpPendingPriceOptions.Text = "Pending Price Options"
        '
        'chkIncPromoPlanner
        '
        Me.chkIncPromoPlanner.AutoSize = True
        Me.chkIncPromoPlanner.Location = New System.Drawing.Point(336, 19)
        Me.chkIncPromoPlanner.Name = "chkIncPromoPlanner"
        Me.chkIncPromoPlanner.Size = New System.Drawing.Size(133, 17)
        Me.chkIncPromoPlanner.TabIndex = 19
        Me.chkIncPromoPlanner.Text = "Include Promo Planner"
        Me.chkIncPromoPlanner.UseVisualStyleBackColor = True
        '
        'chkIncSlim
        '
        Me.chkIncSlim.AutoSize = True
        Me.chkIncSlim.Location = New System.Drawing.Point(10, 19)
        Me.chkIncSlim.Name = "chkIncSlim"
        Me.chkIncSlim.Size = New System.Drawing.Size(124, 17)
        Me.chkIncSlim.TabIndex = 17
        Me.chkIncSlim.Text = "Include SLIM Entries"
        Me.chkIncSlim.UseVisualStyleBackColor = True
        '
        'chkIncFutureSale
        '
        Me.chkIncFutureSale.AutoSize = True
        Me.chkIncFutureSale.Location = New System.Drawing.Point(165, 19)
        Me.chkIncFutureSale.Name = "chkIncFutureSale"
        Me.chkIncFutureSale.Size = New System.Drawing.Size(146, 17)
        Me.chkIncFutureSale.TabIndex = 18
        Me.chkIncFutureSale.Text = "Include Future Sale Items"
        Me.chkIncFutureSale.UseVisualStyleBackColor = True
        '
        'txtStoreName
        '
        Me.ErrorProvider1.SetError(Me.txtStoreName, "True")
        Me.txtStoreName.Location = New System.Drawing.Point(147, 40)
        Me.txtStoreName.MaxLength = 20
        Me.txtStoreName.Name = "txtStoreName"
        Me.txtStoreName.Size = New System.Drawing.Size(143, 22)
        Me.txtStoreName.TabIndex = 3
        '
        'GroupBox_DataSourceOptions
        '
        Me.GroupBox_DataSourceOptions.Controls.Add(Me.dgvSubStoreSubteam)
        Me.GroupBox_DataSourceOptions.Controls.Add(Me.btnRemoveStoreSub)
        Me.GroupBox_DataSourceOptions.Controls.Add(Me.btnAddSubStore)
        Me.GroupBox_DataSourceOptions.Controls.Add(Me.Label15)
        Me.GroupBox_DataSourceOptions.Controls.Add(Me.lblSubSubTeam)
        Me.GroupBox_DataSourceOptions.Controls.Add(Me.cboPOSSubTeam)
        Me.GroupBox_DataSourceOptions.Controls.Add(Me.lblSubStore)
        Me.GroupBox_DataSourceOptions.Controls.Add(Me.cboSubStore)
        Me.GroupBox_DataSourceOptions.Controls.Add(Me.lblISSPriceChgType)
        Me.GroupBox_DataSourceOptions.Controls.Add(Me.cboISSPriceChgType)
        Me.GroupBox_DataSourceOptions.Controls.Add(Me.lblSourceStore)
        Me.GroupBox_DataSourceOptions.Controls.Add(Me.cboSourceStore)
        Me.GroupBox_DataSourceOptions.Controls.Add(Me.lblPOSWriter)
        Me.GroupBox_DataSourceOptions.Controls.Add(Me.cboPOSWriter)
        Me.GroupBox_DataSourceOptions.Location = New System.Drawing.Point(15, 163)
        Me.GroupBox_DataSourceOptions.Name = "GroupBox_DataSourceOptions"
        Me.GroupBox_DataSourceOptions.Size = New System.Drawing.Size(606, 231)
        Me.GroupBox_DataSourceOptions.TabIndex = 30
        Me.GroupBox_DataSourceOptions.TabStop = False
        Me.GroupBox_DataSourceOptions.Text = "Data Source Options"
        '
        'dgvSubStoreSubteam
        '
        Me.dgvSubStoreSubteam.AllowUserToAddRows = False
        Me.dgvSubStoreSubteam.AllowUserToDeleteRows = False
        Me.dgvSubStoreSubteam.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSubStoreSubteam.Location = New System.Drawing.Point(14, 124)
        Me.dgvSubStoreSubteam.Name = "dgvSubStoreSubteam"
        Me.dgvSubStoreSubteam.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvSubStoreSubteam.Size = New System.Drawing.Size(469, 92)
        Me.dgvSubStoreSubteam.TabIndex = 52
        '
        'btnRemoveStoreSub
        '
        Me.btnRemoveStoreSub.Location = New System.Drawing.Point(489, 134)
        Me.btnRemoveStoreSub.Name = "btnRemoveStoreSub"
        Me.btnRemoveStoreSub.Size = New System.Drawing.Size(70, 23)
        Me.btnRemoveStoreSub.TabIndex = 16
        Me.btnRemoveStoreSub.Text = "Remove"
        Me.btnRemoveStoreSub.UseVisualStyleBackColor = True
        '
        'btnAddSubStore
        '
        Me.btnAddSubStore.Location = New System.Drawing.Point(536, 97)
        Me.btnAddSubStore.Name = "btnAddSubStore"
        Me.btnAddSubStore.Size = New System.Drawing.Size(48, 23)
        Me.btnAddSubStore.TabIndex = 15
        Me.btnAddSubStore.Text = "Add"
        Me.btnAddSubStore.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label15.Location = New System.Drawing.Point(1, 66)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(195, 13)
        Me.Label15.TabIndex = 51
        Me.Label15.Text = "Include Subteams From Other Stores"
        '
        'lblSubSubTeam
        '
        Me.lblSubSubTeam.AutoSize = True
        Me.lblSubSubTeam.Location = New System.Drawing.Point(11, 100)
        Me.lblSubSubTeam.Name = "lblSubSubTeam"
        Me.lblSubSubTeam.Size = New System.Drawing.Size(108, 13)
        Me.lblSubSubTeam.TabIndex = 49
        Me.lblSubSubTeam.Text = "Substitute Subteam"
        '
        'cboPOSSubTeam
        '
        Me.cboPOSSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPOSSubTeam.FormattingEnabled = True
        Me.cboPOSSubTeam.Location = New System.Drawing.Point(132, 97)
        Me.cboPOSSubTeam.Name = "cboPOSSubTeam"
        Me.cboPOSSubTeam.Size = New System.Drawing.Size(143, 21)
        Me.cboPOSSubTeam.TabIndex = 13
        '
        'lblSubStore
        '
        Me.lblSubStore.AutoSize = True
        Me.lblSubStore.Location = New System.Drawing.Point(293, 100)
        Me.lblSubStore.Name = "lblSubStore"
        Me.lblSubStore.Size = New System.Drawing.Size(90, 13)
        Me.lblSubStore.TabIndex = 47
        Me.lblSubStore.Text = "Substitute Store"
        '
        'cboSubStore
        '
        Me.cboSubStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubStore.FormattingEnabled = True
        Me.cboSubStore.Location = New System.Drawing.Point(384, 98)
        Me.cboSubStore.Name = "cboSubStore"
        Me.cboSubStore.Size = New System.Drawing.Size(143, 21)
        Me.cboSubStore.TabIndex = 14
        '
        'lblISSPriceChgType
        '
        Me.lblISSPriceChgType.AutoSize = True
        Me.lblISSPriceChgType.Location = New System.Drawing.Point(302, 47)
        Me.lblISSPriceChgType.Name = "lblISSPriceChgType"
        Me.lblISSPriceChgType.Size = New System.Drawing.Size(118, 13)
        Me.lblISSPriceChgType.TabIndex = 45
        Me.lblISSPriceChgType.Text = "ISS Price Change Type"
        '
        'cboISSPriceChgType
        '
        Me.cboISSPriceChgType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboISSPriceChgType.FormattingEnabled = True
        Me.cboISSPriceChgType.Location = New System.Drawing.Point(441, 44)
        Me.cboISSPriceChgType.Name = "cboISSPriceChgType"
        Me.cboISSPriceChgType.Size = New System.Drawing.Size(143, 21)
        Me.cboISSPriceChgType.TabIndex = 12
        '
        'lblSourceStore
        '
        Me.lblSourceStore.AutoSize = True
        Me.lblSourceStore.Location = New System.Drawing.Point(302, 22)
        Me.lblSourceStore.Name = "lblSourceStore"
        Me.lblSourceStore.Size = New System.Drawing.Size(101, 13)
        Me.lblSourceStore.TabIndex = 42
        Me.lblSourceStore.Text = "Main Source Store"
        '
        'cboSourceStore
        '
        Me.cboSourceStore.CausesValidation = False
        Me.cboSourceStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSourceStore.FormattingEnabled = True
        Me.cboSourceStore.Location = New System.Drawing.Point(441, 19)
        Me.cboSourceStore.Name = "cboSourceStore"
        Me.cboSourceStore.Size = New System.Drawing.Size(143, 21)
        Me.cboSourceStore.TabIndex = 11
        '
        'lblPOSWriter
        '
        Me.lblPOSWriter.AutoSize = True
        Me.lblPOSWriter.Location = New System.Drawing.Point(2, 22)
        Me.lblPOSWriter.Name = "lblPOSWriter"
        Me.lblPOSWriter.Size = New System.Drawing.Size(137, 13)
        Me.lblPOSWriter.TabIndex = 40
        Me.lblPOSWriter.Text = "POS Writer for New Store"
        '
        'cboPOSWriter
        '
        Me.cboPOSWriter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPOSWriter.FormattingEnabled = True
        Me.cboPOSWriter.Location = New System.Drawing.Point(140, 19)
        Me.cboPOSWriter.Name = "cboPOSWriter"
        Me.cboPOSWriter.Size = New System.Drawing.Size(135, 21)
        Me.cboPOSWriter.TabIndex = 10
        '
        'lblZone
        '
        Me.lblZone.AutoSize = True
        Me.lblZone.Location = New System.Drawing.Point(17, 69)
        Me.lblZone.Name = "lblZone"
        Me.lblZone.Size = New System.Drawing.Size(38, 13)
        Me.lblZone.TabIndex = 14
        Me.lblZone.Text = "Zone*"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'StoreAdd
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(655, 621)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.Properties_GroupBox1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "StoreAdd"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Store"
        Me.Properties_GroupBox1.ResumeLayout(False)
        Me.Properties_GroupBox1.PerformLayout()
        Me.grpOrderingReceiving.ResumeLayout(False)
        Me.grpOrderingReceiving.PerformLayout()
        Me.grpPendingPriceOptions.ResumeLayout(False)
        Me.grpPendingPriceOptions.PerformLayout()
        Me.GroupBox_DataSourceOptions.ResumeLayout(False)
        Me.GroupBox_DataSourceOptions.PerformLayout()
        CType(Me.dgvSubStoreSubteam, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SubTeamSubstitutionListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Properties_GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents grpPendingPriceOptions As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox_DataSourceOptions As System.Windows.Forms.GroupBox
    Friend WithEvents chkIncSlim As System.Windows.Forms.CheckBox
    Friend WithEvents lblZone As System.Windows.Forms.Label
    Friend WithEvents lblStoreName As System.Windows.Forms.Label
    Friend WithEvents txtStoreName As System.Windows.Forms.TextBox
    Friend WithEvents cboZone As System.Windows.Forms.ComboBox
    Friend WithEvents cboStoreJurisdiction As System.Windows.Forms.ComboBox
    Friend WithEvents lblStoreJurisdiction As System.Windows.Forms.Label
    Friend WithEvents lblTaxJurisdiction As System.Windows.Forms.Label
    Friend WithEvents cboTaxJurisdiction As System.Windows.Forms.ComboBox
    Friend WithEvents chkIncFutureSale As System.Windows.Forms.CheckBox
    Friend WithEvents lblStoreNumber As System.Windows.Forms.Label
    Friend WithEvents txtStoreNumber As System.Windows.Forms.TextBox
    Friend WithEvents lblStoreAbbrev As System.Windows.Forms.Label
    Friend WithEvents txtStoreAbbrev As System.Windows.Forms.TextBox
    Friend WithEvents lblPOSWriter As System.Windows.Forms.Label
    Friend WithEvents cboPOSWriter As System.Windows.Forms.ComboBox
    Friend WithEvents lblBusinessUnitID As System.Windows.Forms.Label
    Friend WithEvents txtBusinessUnitID As System.Windows.Forms.TextBox
    Friend WithEvents lblSourceStore As System.Windows.Forms.Label
    Friend WithEvents cboSourceStore As System.Windows.Forms.ComboBox
    Friend WithEvents chkIncPromoPlanner As System.Windows.Forms.CheckBox
    Friend WithEvents grpOrderingReceiving As System.Windows.Forms.GroupBox
    Friend WithEvents lblPeopleSoftID As System.Windows.Forms.Label
    Friend WithEvents txtPeopleSoftID As System.Windows.Forms.TextBox
    Friend WithEvents lblVendorState As System.Windows.Forms.Label
    Friend WithEvents lblVendorAddress As System.Windows.Forms.Label
    Friend WithEvents txtVendorAddress As System.Windows.Forms.TextBox
    Friend WithEvents lblVendorZip As System.Windows.Forms.Label
    Friend WithEvents txtVendorZip As System.Windows.Forms.TextBox
    Friend WithEvents lblVendorCity As System.Windows.Forms.Label
    Friend WithEvents txtVendorCity As System.Windows.Forms.TextBox
    Friend WithEvents lblVendorName As System.Windows.Forms.Label
    Friend WithEvents txtVendorName As System.Windows.Forms.TextBox
    Friend WithEvents lblISSPriceChgType As System.Windows.Forms.Label
    Friend WithEvents cboISSPriceChgType As System.Windows.Forms.ComboBox
    Friend WithEvents lblSubSubTeam As System.Windows.Forms.Label
    Friend WithEvents cboPOSSubTeam As System.Windows.Forms.ComboBox
    Friend WithEvents lblSubStore As System.Windows.Forms.Label
    Friend WithEvents cboSubStore As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents btnRemoveStoreSub As System.Windows.Forms.Button
    Friend WithEvents btnAddSubStore As System.Windows.Forms.Button
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents lblPlumStoreNo As System.Windows.Forms.Label
    Friend WithEvents txtPlumStoreNo As System.Windows.Forms.TextBox
    Friend WithEvents lblPSIStoreNo As System.Windows.Forms.Label
    Friend WithEvents txtPSIStoreNo As System.Windows.Forms.TextBox
    Friend WithEvents txtVendorState As System.Windows.Forms.TextBox
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtGeoCode As System.Windows.Forms.TextBox
    Friend WithEvents SubTeamSubstitutionListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents dgvSubStoreSubteam As System.Windows.Forms.DataGridView
End Class
