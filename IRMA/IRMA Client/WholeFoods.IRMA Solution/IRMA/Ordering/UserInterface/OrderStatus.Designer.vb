<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrderStatus
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        Me.IsInitializing = True
        'This call is required by the Windows Form Designer.
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
    Public WithEvents _optType_1 As System.Windows.Forms.RadioButton
    Public WithEvents _optType_0 As System.Windows.Forms.RadioButton
    Public WithEvents fraType As System.Windows.Forms.GroupBox
    Public WithEvents txtVendorPO As System.Windows.Forms.TextBox
    Public WithEvents txtCostDifference As System.Windows.Forms.TextBox
    Public WithEvents _txtInvoiceCost_0 As System.Windows.Forms.TextBox
    Public WithEvents txtTotalInvoiceCost As System.Windows.Forms.TextBox
    Public WithEvents cmdCloseOrder As System.Windows.Forms.Button
    Public WithEvents cmdApprove As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents _lblSubTeam_0 As System.Windows.Forms.Label
    Public WithEvents lblDocNo As System.Windows.Forms.Label
    Public WithEvents _lblLabel_16 As System.Windows.Forms.Label
    Public WithEvents lblDifference As System.Windows.Forms.Label
    Public WithEvents lblTotal As System.Windows.Forms.Label
    Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    Public WithEvents lblSubTeam As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    Public WithEvents optType As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Public WithEvents txtInvoiceCost As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrderStatus))
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance28 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance33 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance29 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance36 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance32 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance30 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance31 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance34 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance35 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Type")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("GLAccount")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, False)
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Value")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsAllowance")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ElementName")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Charge_Id", 0)
        Dim ColScrollRegion1 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(439)
        Dim ColScrollRegion2 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(-7)
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
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
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCloseOrder = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdDisplayEInvoice = New System.Windows.Forms.Button()
        Me.Button_AddCharges = New System.Windows.Forms.Button()
        Me.Button_RemoveCharges = New System.Windows.Forms.Button()
        Me.btnCurrency = New System.Windows.Forms.Button()
        Me.btnRefuseReceiving = New System.Windows.Forms.Button()
        Me.ucReasonCode = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me._lblSubTeam_0 = New System.Windows.Forms.Label()
        Me.lblDifference = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmdItemsRefused = New System.Windows.Forms.Button()
        Me.lblRefusalTotal = New System.Windows.Forms.Label()
        Me.txtCostDifference = New System.Windows.Forms.TextBox()
        Me._txtInvoiceCost_0 = New System.Windows.Forms.TextBox()
        Me.txtSACTotal = New System.Windows.Forms.TextBox()
        Me.txtInvTot = New System.Windows.Forms.TextBox()
        Me.fraType = New System.Windows.Forms.GroupBox()
        Me._optType_2 = New System.Windows.Forms.RadioButton()
        Me._optType_1 = New System.Windows.Forms.RadioButton()
        Me._optType_0 = New System.Windows.Forms.RadioButton()
        Me.txtVendorPO = New System.Windows.Forms.TextBox()
        Me.txtTotalInvoiceCost = New System.Windows.Forms.TextBox()
        Me.lblDocNo = New System.Windows.Forms.Label()
        Me._lblLabel_16 = New System.Windows.Forms.Label()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblSubTeam = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optType = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtInvoiceCost = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.dtpSearchDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.UltraNumericEditor_SpecCharges = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.btnRemoveSubteamGLAcct = New System.Windows.Forms.Button()
        Me.btnAddSubteamGLAcct = New System.Windows.Forms.Button()
        Me.CheckedListBox_SpecCharges = New System.Windows.Forms.CheckedListBox()
        Me.cboSubteamGLAcct = New System.Windows.Forms.ComboBox()
        Me.UltraGrid_Charges = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label_NonAllocCharges = New System.Windows.Forms.Label()
        Me.Label_AllocatedCharges = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label_LineItemCharges = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbCurrency = New System.Windows.Forms.ComboBox()
        Me.FormValidator = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.lblReasonCode = New System.Windows.Forms.Label()
        Me.txtRefusalTotal = New System.Windows.Forms.TextBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        CType(Me.ucReasonCode, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraType.SuspendLayout()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSubTeam, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtInvoiceCost, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpSearchDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.UltraNumericEditor_SpecCharges, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraGrid_Charges, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        CType(Me.FormValidator, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCloseOrder
        '
        resources.ApplyResources(Me.cmdCloseOrder, "cmdCloseOrder")
        Me.cmdCloseOrder.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCloseOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCloseOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCloseOrder.Name = "cmdCloseOrder"
        Me.ToolTip1.SetToolTip(Me.cmdCloseOrder, resources.GetString("cmdCloseOrder.ToolTip"))
        Me.cmdCloseOrder.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdDisplayEInvoice
        '
        resources.ApplyResources(Me.cmdDisplayEInvoice, "cmdDisplayEInvoice")
        Me.cmdDisplayEInvoice.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDisplayEInvoice.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDisplayEInvoice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDisplayEInvoice.Name = "cmdDisplayEInvoice"
        Me.cmdDisplayEInvoice.Tag = ""
        Me.ToolTip1.SetToolTip(Me.cmdDisplayEInvoice, resources.GetString("cmdDisplayEInvoice.ToolTip"))
        Me.cmdDisplayEInvoice.UseVisualStyleBackColor = False
        '
        'Button_AddCharges
        '
        resources.ApplyResources(Me.Button_AddCharges, "Button_AddCharges")
        Me.Button_AddCharges.Name = "Button_AddCharges"
        Me.ToolTip1.SetToolTip(Me.Button_AddCharges, resources.GetString("Button_AddCharges.ToolTip"))
        Me.Button_AddCharges.UseVisualStyleBackColor = True
        '
        'Button_RemoveCharges
        '
        resources.ApplyResources(Me.Button_RemoveCharges, "Button_RemoveCharges")
        Me.Button_RemoveCharges.Name = "Button_RemoveCharges"
        Me.ToolTip1.SetToolTip(Me.Button_RemoveCharges, resources.GetString("Button_RemoveCharges.ToolTip"))
        Me.Button_RemoveCharges.UseVisualStyleBackColor = True
        '
        'btnCurrency
        '
        resources.ApplyResources(Me.btnCurrency, "btnCurrency")
        Me.btnCurrency.Image = Global.My.Resources.Resources.ac0001_32
        Me.btnCurrency.Name = "btnCurrency"
        Me.ToolTip1.SetToolTip(Me.btnCurrency, resources.GetString("btnCurrency.ToolTip"))
        Me.btnCurrency.UseVisualStyleBackColor = False
        '
        'btnRefuseReceiving
        '
        resources.ApplyResources(Me.btnRefuseReceiving, "btnRefuseReceiving")
        Me.btnRefuseReceiving.Name = "btnRefuseReceiving"
        Me.ToolTip1.SetToolTip(Me.btnRefuseReceiving, resources.GetString("btnRefuseReceiving.ToolTip"))
        Me.btnRefuseReceiving.UseVisualStyleBackColor = True
        '
        'ucReasonCode
        '
        Me.ucReasonCode.CheckedListSettings.CheckStateMember = ""
        Appearance25.BackColor = System.Drawing.SystemColors.Window
        Appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption
        resources.ApplyResources(Appearance25.FontData, "Appearance25.FontData")
        resources.ApplyResources(Appearance25, "Appearance25")
        Appearance25.ForceApplyResources = "FontData|"
        Me.ucReasonCode.DisplayLayout.Appearance = Appearance25
        Me.ucReasonCode.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ucReasonCode.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance26.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance26.FontData, "Appearance26.FontData")
        resources.ApplyResources(Appearance26, "Appearance26")
        Appearance26.ForceApplyResources = "FontData|"
        Me.ucReasonCode.DisplayLayout.GroupByBox.Appearance = Appearance26
        Appearance28.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance28.FontData, "Appearance28.FontData")
        resources.ApplyResources(Appearance28, "Appearance28")
        Appearance28.ForceApplyResources = "FontData|"
        Me.ucReasonCode.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance28
        Me.ucReasonCode.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance27.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance27.BackColor2 = System.Drawing.SystemColors.Control
        Appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance27.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance27.FontData, "Appearance27.FontData")
        resources.ApplyResources(Appearance27, "Appearance27")
        Appearance27.ForceApplyResources = "FontData|"
        Me.ucReasonCode.DisplayLayout.GroupByBox.PromptAppearance = Appearance27
        Me.ucReasonCode.DisplayLayout.MaxColScrollRegions = 1
        Me.ucReasonCode.DisplayLayout.MaxRowScrollRegions = 1
        Appearance33.BackColor = System.Drawing.SystemColors.Window
        Appearance33.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Appearance33.FontData, "Appearance33.FontData")
        resources.ApplyResources(Appearance33, "Appearance33")
        Appearance33.ForceApplyResources = "FontData|"
        Me.ucReasonCode.DisplayLayout.Override.ActiveCellAppearance = Appearance33
        Appearance29.BackColor = System.Drawing.SystemColors.Highlight
        Appearance29.ForeColor = System.Drawing.SystemColors.HighlightText
        resources.ApplyResources(Appearance29.FontData, "Appearance29.FontData")
        resources.ApplyResources(Appearance29, "Appearance29")
        Appearance29.ForceApplyResources = "FontData|"
        Me.ucReasonCode.DisplayLayout.Override.ActiveRowAppearance = Appearance29
        Me.ucReasonCode.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ucReasonCode.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance36.BackColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance36.FontData, "Appearance36.FontData")
        resources.ApplyResources(Appearance36, "Appearance36")
        Appearance36.ForceApplyResources = "FontData|"
        Me.ucReasonCode.DisplayLayout.Override.CardAreaAppearance = Appearance36
        Appearance32.BorderColor = System.Drawing.Color.Silver
        Appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        resources.ApplyResources(Appearance32.FontData, "Appearance32.FontData")
        resources.ApplyResources(Appearance32, "Appearance32")
        Appearance32.ForceApplyResources = "FontData|"
        Me.ucReasonCode.DisplayLayout.Override.CellAppearance = Appearance32
        Me.ucReasonCode.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ucReasonCode.DisplayLayout.Override.CellPadding = 0
        Appearance30.BackColor = System.Drawing.SystemColors.Control
        Appearance30.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance30.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance30.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance30.FontData, "Appearance30.FontData")
        resources.ApplyResources(Appearance30, "Appearance30")
        Appearance30.ForceApplyResources = "FontData|"
        Me.ucReasonCode.DisplayLayout.Override.GroupByRowAppearance = Appearance30
        resources.ApplyResources(Appearance31, "Appearance31")
        resources.ApplyResources(Appearance31.FontData, "Appearance31.FontData")
        Appearance31.ForceApplyResources = "FontData|"
        Me.ucReasonCode.DisplayLayout.Override.HeaderAppearance = Appearance31
        Me.ucReasonCode.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ucReasonCode.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance34.BackColor = System.Drawing.SystemColors.Window
        Appearance34.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance34.FontData, "Appearance34.FontData")
        resources.ApplyResources(Appearance34, "Appearance34")
        Appearance34.ForceApplyResources = "FontData|"
        Me.ucReasonCode.DisplayLayout.Override.RowAppearance = Appearance34
        Me.ucReasonCode.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance35.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance35.FontData, "Appearance35.FontData")
        resources.ApplyResources(Appearance35, "Appearance35")
        Appearance35.ForceApplyResources = "FontData|"
        Me.ucReasonCode.DisplayLayout.Override.TemplateAddRowAppearance = Appearance35
        Me.ucReasonCode.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ucReasonCode.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ucReasonCode.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ucReasonCode.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList
        resources.ApplyResources(Me.ucReasonCode, "ucReasonCode")
        Me.ucReasonCode.Name = "ucReasonCode"
        Me.ToolTip1.SetToolTip(Me.ucReasonCode, resources.GetString("ucReasonCode.ToolTip"))
        '
        '_lblSubTeam_0
        '
        Me._lblSubTeam_0.BackColor = System.Drawing.Color.Transparent
        Me._lblSubTeam_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblSubTeam_0, "_lblSubTeam_0")
        Me._lblSubTeam_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubTeam.SetIndex(Me._lblSubTeam_0, CType(0, Short))
        Me._lblSubTeam_0.Name = "_lblSubTeam_0"
        Me.ToolTip1.SetToolTip(Me._lblSubTeam_0, resources.GetString("_lblSubTeam_0.ToolTip"))
        '
        'lblDifference
        '
        Me.lblDifference.BackColor = System.Drawing.Color.Transparent
        Me.lblDifference.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDifference, "lblDifference")
        Me.lblDifference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDifference.Name = "lblDifference"
        Me.ToolTip1.SetToolTip(Me.lblDifference, resources.GetString("lblDifference.ToolTip"))
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        Me.ToolTip1.SetToolTip(Me.Label1, resources.GetString("Label1.ToolTip"))
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Name = "Label2"
        Me.ToolTip1.SetToolTip(Me.Label2, resources.GetString("Label2.ToolTip"))
        '
        'cmdItemsRefused
        '
        resources.ApplyResources(Me.cmdItemsRefused, "cmdItemsRefused")
        Me.cmdItemsRefused.BackColor = System.Drawing.SystemColors.Control
        Me.cmdItemsRefused.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdItemsRefused.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdItemsRefused.Image = Global.My.Resources.Resources.Refusal32
        Me.cmdItemsRefused.Name = "cmdItemsRefused"
        Me.ToolTip1.SetToolTip(Me.cmdItemsRefused, resources.GetString("cmdItemsRefused.ToolTip"))
        Me.cmdItemsRefused.UseVisualStyleBackColor = False
        '
        'lblRefusalTotal
        '
        Me.lblRefusalTotal.BackColor = System.Drawing.Color.Transparent
        Me.lblRefusalTotal.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblRefusalTotal, "lblRefusalTotal")
        Me.lblRefusalTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRefusalTotal.Name = "lblRefusalTotal"
        Me.ToolTip1.SetToolTip(Me.lblRefusalTotal, resources.GetString("lblRefusalTotal.ToolTip"))
        '
        'txtCostDifference
        '
        Me.txtCostDifference.AcceptsReturn = True
        Me.txtCostDifference.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtCostDifference.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtCostDifference, "txtCostDifference")
        Me.txtCostDifference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCostDifference.Name = "txtCostDifference"
        Me.txtCostDifference.ReadOnly = True
        Me.txtCostDifference.TabStop = False
        '
        '_txtInvoiceCost_0
        '
        Me._txtInvoiceCost_0.AcceptsReturn = True
        Me._txtInvoiceCost_0.BackColor = System.Drawing.SystemColors.ControlLight
        Me._txtInvoiceCost_0.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtInvoiceCost_0, "_txtInvoiceCost_0")
        Me._txtInvoiceCost_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInvoiceCost.SetIndex(Me._txtInvoiceCost_0, CType(0, Short))
        Me._txtInvoiceCost_0.Name = "_txtInvoiceCost_0"
        Me._txtInvoiceCost_0.ReadOnly = True
        Me._txtInvoiceCost_0.Tag = "Date"
        '
        'txtSACTotal
        '
        Me.txtSACTotal.AcceptsReturn = True
        Me.txtSACTotal.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtSACTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtSACTotal, "txtSACTotal")
        Me.txtSACTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSACTotal.Name = "txtSACTotal"
        Me.txtSACTotal.ReadOnly = True
        Me.txtSACTotal.TabStop = False
        '
        'txtInvTot
        '
        Me.txtInvTot.AcceptsReturn = True
        Me.txtInvTot.BackColor = System.Drawing.SystemColors.Window
        Me.txtInvTot.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtInvTot, "txtInvTot")
        Me.txtInvTot.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInvTot.Name = "txtInvTot"
        '
        'fraType
        '
        Me.fraType.BackColor = System.Drawing.SystemColors.Control
        Me.fraType.Controls.Add(Me._optType_2)
        Me.fraType.Controls.Add(Me._optType_1)
        Me.fraType.Controls.Add(Me._optType_0)
        resources.ApplyResources(Me.fraType, "fraType")
        Me.fraType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraType.Name = "fraType"
        Me.fraType.TabStop = False
        '
        '_optType_2
        '
        Me._optType_2.BackColor = System.Drawing.SystemColors.Control
        Me._optType_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optType_2, "_optType_2")
        Me._optType_2.ForeColor = System.Drawing.SystemColors.ControlText
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
        'txtVendorPO
        '
        Me.txtVendorPO.AcceptsReturn = True
        Me.txtVendorPO.BackColor = System.Drawing.SystemColors.Window
        Me.txtVendorPO.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtVendorPO, "txtVendorPO")
        Me.txtVendorPO.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVendorPO.Name = "txtVendorPO"
        Me.txtVendorPO.Tag = "String"
        '
        'txtTotalInvoiceCost
        '
        Me.txtTotalInvoiceCost.AcceptsReturn = True
        Me.txtTotalInvoiceCost.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtTotalInvoiceCost.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtTotalInvoiceCost, "txtTotalInvoiceCost")
        Me.txtTotalInvoiceCost.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalInvoiceCost.Name = "txtTotalInvoiceCost"
        Me.txtTotalInvoiceCost.ReadOnly = True
        Me.txtTotalInvoiceCost.TabStop = False
        '
        'lblDocNo
        '
        Me.lblDocNo.BackColor = System.Drawing.Color.Transparent
        Me.lblDocNo.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDocNo, "lblDocNo")
        Me.lblDocNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocNo.Name = "lblDocNo"
        '
        '_lblLabel_16
        '
        Me._lblLabel_16.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_16.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_16, "_lblLabel_16")
        Me._lblLabel_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_16, CType(16, Short))
        Me._lblLabel_16.Name = "_lblLabel_16"
        '
        'lblTotal
        '
        Me.lblTotal.BackColor = System.Drawing.Color.Transparent
        Me.lblTotal.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblTotal, "lblTotal")
        Me.lblTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotal.Name = "lblTotal"
        '
        'optType
        '
        '
        'dtpSearchDate
        '
        Me.dtpSearchDate.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        resources.ApplyResources(Me.dtpSearchDate, "dtpSearchDate")
        Me.dtpSearchDate.MaskInput = ""
        Me.dtpSearchDate.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me.dtpSearchDate.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpSearchDate.Name = "dtpSearchDate"
        Me.dtpSearchDate.Value = Nothing
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.UltraNumericEditor_SpecCharges)
        Me.GroupBox1.Controls.Add(Me.btnRemoveSubteamGLAcct)
        Me.GroupBox1.Controls.Add(Me.btnAddSubteamGLAcct)
        Me.GroupBox1.Controls.Add(Me.CheckedListBox_SpecCharges)
        Me.GroupBox1.Controls.Add(Me.cboSubteamGLAcct)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'UltraNumericEditor_SpecCharges
        '
        Me.UltraNumericEditor_SpecCharges.AlwaysInEditMode = True
        resources.ApplyResources(Me.UltraNumericEditor_SpecCharges, "UltraNumericEditor_SpecCharges")
        Me.UltraNumericEditor_SpecCharges.MaskInput = "{LOC}-nnnnnnn.nn"
        Me.UltraNumericEditor_SpecCharges.MaxValue = 922337203685477.62R
        Me.UltraNumericEditor_SpecCharges.MinValue = CType(-922337203685478, Long)
        Me.UltraNumericEditor_SpecCharges.Name = "UltraNumericEditor_SpecCharges"
        Me.UltraNumericEditor_SpecCharges.NullText = "0"
        Me.UltraNumericEditor_SpecCharges.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.UltraNumericEditor_SpecCharges.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        '
        'btnRemoveSubteamGLAcct
        '
        resources.ApplyResources(Me.btnRemoveSubteamGLAcct, "btnRemoveSubteamGLAcct")
        Me.btnRemoveSubteamGLAcct.Name = "btnRemoveSubteamGLAcct"
        Me.btnRemoveSubteamGLAcct.UseVisualStyleBackColor = True
        '
        'btnAddSubteamGLAcct
        '
        resources.ApplyResources(Me.btnAddSubteamGLAcct, "btnAddSubteamGLAcct")
        Me.btnAddSubteamGLAcct.Name = "btnAddSubteamGLAcct"
        Me.btnAddSubteamGLAcct.UseVisualStyleBackColor = True
        '
        'CheckedListBox_SpecCharges
        '
        Me.CheckedListBox_SpecCharges.FormattingEnabled = True
        resources.ApplyResources(Me.CheckedListBox_SpecCharges, "CheckedListBox_SpecCharges")
        Me.CheckedListBox_SpecCharges.Name = "CheckedListBox_SpecCharges"
        '
        'cboSubteamGLAcct
        '
        Me.cboSubteamGLAcct.FormattingEnabled = True
        resources.ApplyResources(Me.cboSubteamGLAcct, "cboSubteamGLAcct")
        Me.cboSubteamGLAcct.Name = "cboSubteamGLAcct"
        '
        'UltraGrid_Charges
        '
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        resources.ApplyResources(Appearance1.FontData, "Appearance1.FontData")
        resources.ApplyResources(Appearance1, "Appearance1")
        Appearance1.ForceApplyResources = "FontData|"
        Me.UltraGrid_Charges.DisplayLayout.Appearance = Appearance1
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 69
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 70
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 216
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Width = 65
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Hidden = True
        UltraGridColumn6.Header.VisiblePosition = 6
        UltraGridColumn6.Hidden = True
        UltraGridColumn7.Header.VisiblePosition = 5
        UltraGridColumn7.Hidden = True
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7})
        Me.UltraGrid_Charges.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid_Charges.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_Charges.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_Charges.DisplayLayout.ColScrollRegions.Add(ColScrollRegion1)
        Me.UltraGrid_Charges.DisplayLayout.ColScrollRegions.Add(ColScrollRegion2)
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance2.FontData, "Appearance2.FontData")
        resources.ApplyResources(Appearance2, "Appearance2")
        Appearance2.ForceApplyResources = "FontData|"
        Me.UltraGrid_Charges.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance3.FontData, "Appearance3.FontData")
        resources.ApplyResources(Appearance3, "Appearance3")
        Appearance3.ForceApplyResources = "FontData|"
        Me.UltraGrid_Charges.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.UltraGrid_Charges.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_Charges.DisplayLayout.GroupByBox.Hidden = True
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance4.FontData, "Appearance4.FontData")
        resources.ApplyResources(Appearance4, "Appearance4")
        Appearance4.ForceApplyResources = "FontData|"
        Me.UltraGrid_Charges.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.UltraGrid_Charges.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_Charges.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Appearance5.FontData, "Appearance5.FontData")
        resources.ApplyResources(Appearance5, "Appearance5")
        Appearance5.ForceApplyResources = "FontData|"
        Me.UltraGrid_Charges.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.Highlight
        Appearance6.ForeColor = System.Drawing.SystemColors.HighlightText
        resources.ApplyResources(Appearance6.FontData, "Appearance6.FontData")
        resources.ApplyResources(Appearance6, "Appearance6")
        Appearance6.ForceApplyResources = "FontData|"
        Me.UltraGrid_Charges.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.UltraGrid_Charges.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.UltraGrid_Charges.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_Charges.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_Charges.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_Charges.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance7.FontData, "Appearance7.FontData")
        resources.ApplyResources(Appearance7, "Appearance7")
        Appearance7.ForceApplyResources = "FontData|"
        Me.UltraGrid_Charges.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        resources.ApplyResources(Appearance8.FontData, "Appearance8.FontData")
        resources.ApplyResources(Appearance8, "Appearance8")
        Appearance8.ForceApplyResources = "FontData|"
        Me.UltraGrid_Charges.DisplayLayout.Override.CellAppearance = Appearance8
        Me.UltraGrid_Charges.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_Charges.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance9.FontData, "Appearance9.FontData")
        resources.ApplyResources(Appearance9, "Appearance9")
        Appearance9.ForceApplyResources = "FontData|"
        Me.UltraGrid_Charges.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        resources.ApplyResources(Appearance10, "Appearance10")
        resources.ApplyResources(Appearance10.FontData, "Appearance10.FontData")
        Appearance10.ForceApplyResources = "FontData|"
        Me.UltraGrid_Charges.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.UltraGrid_Charges.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_Charges.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Appearance11.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance11.FontData, "Appearance11.FontData")
        resources.ApplyResources(Appearance11, "Appearance11")
        Appearance11.ForceApplyResources = "FontData|"
        Me.UltraGrid_Charges.DisplayLayout.Override.RowAppearance = Appearance11
        Me.UltraGrid_Charges.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_Charges.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_Charges.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_Charges.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance12.FontData, "Appearance12.FontData")
        resources.ApplyResources(Appearance12, "Appearance12")
        Appearance12.ForceApplyResources = "FontData|"
        Me.UltraGrid_Charges.DisplayLayout.Override.TemplateAddRowAppearance = Appearance12
        Me.UltraGrid_Charges.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_Charges.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        resources.ApplyResources(Me.UltraGrid_Charges, "UltraGrid_Charges")
        Me.UltraGrid_Charges.Name = "UltraGrid_Charges"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.UltraGrid_Charges)
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'Label_NonAllocCharges
        '
        resources.ApplyResources(Me.Label_NonAllocCharges, "Label_NonAllocCharges")
        Me.Label_NonAllocCharges.Name = "Label_NonAllocCharges"
        '
        'Label_AllocatedCharges
        '
        resources.ApplyResources(Me.Label_AllocatedCharges, "Label_AllocatedCharges")
        Me.Label_AllocatedCharges.Name = "Label_AllocatedCharges"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'Label_LineItemCharges
        '
        resources.ApplyResources(Me.Label_LineItemCharges, "Label_LineItemCharges")
        Me.Label_LineItemCharges.Name = "Label_LineItemCharges"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Name = "Label6"
        '
        'cmbCurrency
        '
        resources.ApplyResources(Me.cmbCurrency, "cmbCurrency")
        Me.cmbCurrency.FormattingEnabled = True
        Me.cmbCurrency.Name = "cmbCurrency"
        '
        'FormValidator
        '
        Me.FormValidator.ContainerControl = Me
        '
        'lblReasonCode
        '
        resources.ApplyResources(Me.lblReasonCode, "lblReasonCode")
        Me.lblReasonCode.Name = "lblReasonCode"
        '
        'txtRefusalTotal
        '
        Me.txtRefusalTotal.AcceptsReturn = True
        Me.txtRefusalTotal.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtRefusalTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtRefusalTotal, "txtRefusalTotal")
        Me.txtRefusalTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRefusalTotal.Name = "txtRefusalTotal"
        Me.txtRefusalTotal.ReadOnly = True
        Me.txtRefusalTotal.TabStop = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.lblReasonCode)
        Me.GroupBox3.Controls.Add(Me.btnRefuseReceiving)
        Me.GroupBox3.Controls.Add(Me.ucReasonCode)
        resources.ApplyResources(Me.GroupBox3, "GroupBox3")
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.TabStop = False
        '
        'frmOrderStatus
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.txtRefusalTotal)
        Me.Controls.Add(Me.lblRefusalTotal)
        Me.Controls.Add(Me.cmdItemsRefused)
        Me.Controls.Add(Me.cmbCurrency)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.btnCurrency)
        Me.Controls.Add(Me.Label_LineItemCharges)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label_NonAllocCharges)
        Me.Controls.Add(Me.Label_AllocatedCharges)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Button_RemoveCharges)
        Me.Controls.Add(Me.Button_AddCharges)
        Me.Controls.Add(Me.txtInvTot)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtSACTotal)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdDisplayEInvoice)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.fraType)
        Me.Controls.Add(Me.txtVendorPO)
        Me.Controls.Add(Me.txtCostDifference)
        Me.Controls.Add(Me._txtInvoiceCost_0)
        Me.Controls.Add(Me.txtTotalInvoiceCost)
        Me.Controls.Add(Me.cmdCloseOrder)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me._lblSubTeam_0)
        Me.Controls.Add(Me.lblDocNo)
        Me.Controls.Add(Me._lblLabel_16)
        Me.Controls.Add(Me.lblDifference)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.dtpSearchDate)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOrderStatus"
        Me.ShowInTaskbar = False
        CType(Me.ucReasonCode, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraType.ResumeLayout(False)
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSubTeam, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtInvoiceCost, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpSearchDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.UltraNumericEditor_SpecCharges, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraGrid_Charges, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.FormValidator, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dtpSearchDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents UltraNumericEditor_SpecCharges As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents btnRemoveSubteamGLAcct As System.Windows.Forms.Button
    Friend WithEvents btnAddSubteamGLAcct As System.Windows.Forms.Button
    Friend WithEvents CheckedListBox_SpecCharges As System.Windows.Forms.CheckedListBox
    Public WithEvents cboSubteamGLAcct As System.Windows.Forms.ComboBox
    Public WithEvents cmdDisplayEInvoice As System.Windows.Forms.Button
    Public WithEvents txtSACTotal As System.Windows.Forms.TextBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents txtInvTot As System.Windows.Forms.TextBox
    Public WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents UltraGrid_Charges As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents Button_AddCharges As System.Windows.Forms.Button
    Friend WithEvents Button_RemoveCharges As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label_NonAllocCharges As System.Windows.Forms.Label
    Friend WithEvents Label_AllocatedCharges As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label_LineItemCharges As System.Windows.Forms.Label
    Public WithEvents _optType_2 As System.Windows.Forms.RadioButton
    Friend WithEvents btnCurrency As System.Windows.Forms.Button
    Public WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbCurrency As System.Windows.Forms.ComboBox
    Friend WithEvents btnRefuseReceiving As System.Windows.Forms.Button
    Friend WithEvents FormValidator As System.Windows.Forms.ErrorProvider
    Friend WithEvents ucReasonCode As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents lblReasonCode As System.Windows.Forms.Label
    Public WithEvents cmdItemsRefused As System.Windows.Forms.Button
    Public WithEvents txtRefusalTotal As System.Windows.Forms.TextBox
    Public WithEvents lblRefusalTotal As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox

#End Region
End Class

