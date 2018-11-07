<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrdersSearch
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
	Public WithEvents cmdSearch As System.Windows.Forms.Button
	Public WithEvents chkFromQueue As System.Windows.Forms.CheckBox
	Public WithEvents _txtField_9 As System.Windows.Forms.TextBox
    Public WithEvents _optCredit_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optCredit_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optCredit_0 As System.Windows.Forms.RadioButton
	Public WithEvents Frame4 As System.Windows.Forms.GroupBox
	Public WithEvents _txtField_0 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_3 As System.Windows.Forms.TextBox
    Public WithEvents _cmbField_4 As System.Windows.Forms.ComboBox
	Public WithEvents _cmbField_3 As System.Windows.Forms.ComboBox
	Public WithEvents _cmbField_2 As System.Windows.Forms.ComboBox
	Public WithEvents _txtField_6 As System.Windows.Forms.TextBox
	Public WithEvents _cmbField_1 As System.Windows.Forms.ComboBox
	Public WithEvents _optType_3 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_1 As System.Windows.Forms.RadioButton
	Public WithEvents Frame3 As System.Windows.Forms.GroupBox
	Public WithEvents _cmbField_0 As System.Windows.Forms.ComboBox
	Public WithEvents _txtField_5 As System.Windows.Forms.TextBox
	Public WithEvents _txtField_4 As System.Windows.Forms.TextBox
	Public WithEvents _optOpen_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optOpen_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optOpen_0 As System.Windows.Forms.RadioButton
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents _optSend_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optSend_0 As System.Windows.Forms.RadioButton
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdSelect As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents lblLotNum As System.Windows.Forms.Label
	Public WithEvents _lblLabel_12 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_11 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_10 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_9 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_8 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_7 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_6 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents cmbField As Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents optCredit As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents optOpen As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents optSend As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents optType As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrdersSearch))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Return_Order")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderHeader_ID", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyName")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderDate")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Expected_Date")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SentDate")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CloseDate")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeam", 0)
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderedCost", 1)
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("eInvoice", 2)
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Source", 3)
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("DeleteDate", 4)
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Jurisdiction", 5)
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
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.cmdSelect = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.chkFromQueue = New System.Windows.Forms.CheckBox()
        Me._txtField_9 = New System.Windows.Forms.TextBox()
        Me.Frame4 = New System.Windows.Forms.GroupBox()
        Me._optCredit_1 = New System.Windows.Forms.RadioButton()
        Me._optCredit_2 = New System.Windows.Forms.RadioButton()
        Me._optCredit_0 = New System.Windows.Forms.RadioButton()
        Me._txtField_0 = New System.Windows.Forms.TextBox()
        Me._txtField_3 = New System.Windows.Forms.TextBox()
        Me._cmbField_3 = New System.Windows.Forms.ComboBox()
        Me._cmbField_2 = New System.Windows.Forms.ComboBox()
        Me._txtField_6 = New System.Windows.Forms.TextBox()
        Me._cmbField_1 = New System.Windows.Forms.ComboBox()
        Me.Frame3 = New System.Windows.Forms.GroupBox()
        Me._optType_3 = New System.Windows.Forms.RadioButton()
        Me._optType_0 = New System.Windows.Forms.RadioButton()
        Me._optType_2 = New System.Windows.Forms.RadioButton()
        Me._optType_1 = New System.Windows.Forms.RadioButton()
        Me._cmbField_0 = New System.Windows.Forms.ComboBox()
        Me._txtField_5 = New System.Windows.Forms.TextBox()
        Me._txtField_4 = New System.Windows.Forms.TextBox()
        Me.Frame2 = New System.Windows.Forms.GroupBox()
        Me._optOpen_3 = New System.Windows.Forms.RadioButton()
        Me._optOpen_2 = New System.Windows.Forms.RadioButton()
        Me._optOpen_1 = New System.Windows.Forms.RadioButton()
        Me._optOpen_0 = New System.Windows.Forms.RadioButton()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me._optSend_1 = New System.Windows.Forms.RadioButton()
        Me._optSend_0 = New System.Windows.Forms.RadioButton()
        Me.lblLotNum = New System.Windows.Forms.Label()
        Me._lblLabel_12 = New System.Windows.Forms.Label()
        Me._lblLabel_11 = New System.Windows.Forms.Label()
        Me._lblLabel_10 = New System.Windows.Forms.Label()
        Me._lblLabel_9 = New System.Windows.Forms.Label()
        Me._lblLabel_8 = New System.Windows.Forms.Label()
        Me._lblLabel_7 = New System.Windows.Forms.Label()
        Me._lblLabel_6 = New System.Windows.Forms.Label()
        Me._lblLabel_5 = New System.Windows.Forms.Label()
        Me._lblLabel_4 = New System.Windows.Forms.Label()
        Me._lblLabel_3 = New System.Windows.Forms.Label()
        Me._lblLabel_2 = New System.Windows.Forms.Label()
        Me._lblLabel_1 = New System.Windows.Forms.Label()
        Me._lblLabel_0 = New System.Windows.Forms.Label()
        Me.cmbField = New Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray(Me.components)
        Me._cmbField_4 = New System.Windows.Forms.ComboBox()
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optCredit = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optOpen = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optSend = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optType = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.ugrdSearchResults = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.dtpWarehouseSentDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.dtpOrderDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.dtpExpectedDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.dtpSentDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me._lblLabel_13 = New System.Windows.Forms.Label()
        Me.chkEInvoice = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkRefusedPO = New System.Windows.Forms.CheckBox()
        Me.chkPartialShipment = New System.Windows.Forms.CheckBox()
        Me.Frame4.SuspendLayout()
        Me.Frame3.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me.Frame1.SuspendLayout()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optCredit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optOpen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optSend, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdSearchResults, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpWarehouseSentDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpOrderDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpExpectedDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpSentDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Image = CType(resources.GetObject("cmdSearch.Image"), System.Drawing.Image)
        Me.cmdSearch.Location = New System.Drawing.Point(676, 212)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(41, 41)
        Me.cmdSearch.TabIndex = 31
        Me.cmdSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSearch, "Search")
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'cmdSelect
        '
        Me.cmdSelect.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelect.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Image = CType(resources.GetObject("cmdSelect.Image"), System.Drawing.Image)
        Me.cmdSelect.Location = New System.Drawing.Point(1118, 508)
        Me.cmdSelect.Name = "cmdSelect"
        Me.cmdSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelect.Size = New System.Drawing.Size(41, 41)
        Me.cmdSelect.TabIndex = 32
        Me.cmdSelect.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSelect, "Select Order")
        Me.cmdSelect.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(1166, 508)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 33
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'btnClear
        '
        Me.btnClear.BackColor = System.Drawing.SystemColors.Control
        Me.btnClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnClear.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.btnClear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnClear.Image = CType(resources.GetObject("btnClear.Image"), System.Drawing.Image)
        Me.btnClear.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnClear.Location = New System.Drawing.Point(723, 212)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnClear.Size = New System.Drawing.Size(41, 41)
        Me.btnClear.TabIndex = 100
        Me.btnClear.TabStop = False
        Me.btnClear.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.btnClear, "Reset Search Criteria")
        Me.btnClear.UseVisualStyleBackColor = False
        '
        'chkFromQueue
        '
        Me.chkFromQueue.BackColor = System.Drawing.SystemColors.Control
        Me.chkFromQueue.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkFromQueue.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkFromQueue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkFromQueue.Location = New System.Drawing.Point(308, 70)
        Me.chkFromQueue.Name = "chkFromQueue"
        Me.chkFromQueue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkFromQueue.Size = New System.Drawing.Size(97, 17)
        Me.chkFromQueue.TabIndex = 17
        Me.chkFromQueue.Text = "From Queue"
        Me.chkFromQueue.UseVisualStyleBackColor = False
        '
        '_txtField_9
        '
        Me._txtField_9.AcceptsReturn = True
        Me._txtField_9.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_9.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_9.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_9.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_9, CType(9, Short))
        Me._txtField_9.Location = New System.Drawing.Point(450, 118)
        Me._txtField_9.MaxLength = 12
        Me._txtField_9.Name = "_txtField_9"
        Me._txtField_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_9.Size = New System.Drawing.Size(150, 20)
        Me._txtField_9.TabIndex = 5
        Me._txtField_9.Tag = "STRING"
        '
        'Frame4
        '
        Me.Frame4.BackColor = System.Drawing.SystemColors.Control
        Me.Frame4.Controls.Add(Me._optCredit_1)
        Me.Frame4.Controls.Add(Me._optCredit_2)
        Me.Frame4.Controls.Add(Me._optCredit_0)
        Me.Frame4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame4.Location = New System.Drawing.Point(623, 7)
        Me.Frame4.Name = "Frame4"
        Me.Frame4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame4.Size = New System.Drawing.Size(100, 80)
        Me.Frame4.TabIndex = 27
        Me.Frame4.TabStop = False
        Me.Frame4.Text = "Order Type"
        '
        '_optCredit_1
        '
        Me._optCredit_1.BackColor = System.Drawing.SystemColors.Control
        Me._optCredit_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optCredit_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optCredit_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCredit.SetIndex(Me._optCredit_1, CType(1, Short))
        Me._optCredit_1.Location = New System.Drawing.Point(8, 32)
        Me._optCredit_1.Name = "_optCredit_1"
        Me._optCredit_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optCredit_1.Size = New System.Drawing.Size(90, 17)
        Me._optCredit_1.TabIndex = 29
        Me._optCredit_1.TabStop = True
        Me._optCredit_1.Text = "Regular"
        Me._optCredit_1.UseVisualStyleBackColor = False
        '
        '_optCredit_2
        '
        Me._optCredit_2.BackColor = System.Drawing.SystemColors.Control
        Me._optCredit_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optCredit_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optCredit_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCredit.SetIndex(Me._optCredit_2, CType(2, Short))
        Me._optCredit_2.Location = New System.Drawing.Point(8, 48)
        Me._optCredit_2.Name = "_optCredit_2"
        Me._optCredit_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optCredit_2.Size = New System.Drawing.Size(90, 17)
        Me._optCredit_2.TabIndex = 30
        Me._optCredit_2.TabStop = True
        Me._optCredit_2.Text = "Credit"
        Me._optCredit_2.UseVisualStyleBackColor = False
        '
        '_optCredit_0
        '
        Me._optCredit_0.BackColor = System.Drawing.SystemColors.Control
        Me._optCredit_0.Checked = True
        Me._optCredit_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optCredit_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optCredit_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCredit.SetIndex(Me._optCredit_0, CType(0, Short))
        Me._optCredit_0.Location = New System.Drawing.Point(8, 16)
        Me._optCredit_0.Name = "_optCredit_0"
        Me._optCredit_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optCredit_0.Size = New System.Drawing.Size(90, 17)
        Me._optCredit_0.TabIndex = 28
        Me._optCredit_0.TabStop = True
        Me._optCredit_0.Text = "Ignore Field"
        Me._optCredit_0.UseVisualStyleBackColor = False
        '
        '_txtField_0
        '
        Me._txtField_0.AcceptsReturn = True
        Me._txtField_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_0, CType(0, Short))
        Me._txtField_0.Location = New System.Drawing.Point(108, 31)
        Me._txtField_0.MaxLength = 10
        Me._txtField_0.Name = "_txtField_0"
        Me._txtField_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_0.Size = New System.Drawing.Size(121, 20)
        Me._txtField_0.TabIndex = 0
        Me._txtField_0.Tag = "Number"
        '
        '_txtField_3
        '
        Me._txtField_3.AcceptsReturn = True
        Me._txtField_3.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_3, CType(3, Short))
        Me._txtField_3.Location = New System.Drawing.Point(108, 55)
        Me._txtField_3.MaxLength = 16
        Me._txtField_3.Name = "_txtField_3"
        Me._txtField_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_3.Size = New System.Drawing.Size(121, 20)
        Me._txtField_3.TabIndex = 3
        Me._txtField_3.Tag = "String"
        '
        '_cmbField_3
        '
        Me._cmbField_3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_3.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_3, CType(3, Short))
        Me._cmbField_3.Location = New System.Drawing.Point(450, 225)
        Me._cmbField_3.Name = "_cmbField_3"
        Me._cmbField_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_3.Size = New System.Drawing.Size(209, 22)
        Me._cmbField_3.Sorted = True
        Me._cmbField_3.TabIndex = 13
        '
        '_cmbField_2
        '
        Me._cmbField_2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_2.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_2, CType(2, Short))
        Me._cmbField_2.Location = New System.Drawing.Point(450, 197)
        Me._cmbField_2.Name = "_cmbField_2"
        Me._cmbField_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_2.Size = New System.Drawing.Size(209, 22)
        Me._cmbField_2.Sorted = True
        Me._cmbField_2.TabIndex = 11
        '
        '_txtField_6
        '
        Me._txtField_6.AcceptsReturn = True
        Me._txtField_6.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_6.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_6.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_6, CType(6, Short))
        Me._txtField_6.Location = New System.Drawing.Point(450, 171)
        Me._txtField_6.MaxLength = 50
        Me._txtField_6.Name = "_txtField_6"
        Me._txtField_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_6.Size = New System.Drawing.Size(209, 20)
        Me._txtField_6.TabIndex = 9
        Me._txtField_6.Tag = "String"
        '
        '_cmbField_1
        '
        Me._cmbField_1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_1.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_1, CType(1, Short))
        Me._cmbField_1.Location = New System.Drawing.Point(108, 227)
        Me._cmbField_1.Name = "_cmbField_1"
        Me._cmbField_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_1.Size = New System.Drawing.Size(201, 22)
        Me._cmbField_1.Sorted = True
        Me._cmbField_1.TabIndex = 12
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me._optType_3)
        Me.Frame3.Controls.Add(Me._optType_0)
        Me.Frame3.Controls.Add(Me._optType_2)
        Me.Frame3.Controls.Add(Me._optType_1)
        Me.Frame3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(517, 7)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(100, 89)
        Me.Frame3.TabIndex = 22
        Me.Frame3.TabStop = False
        Me.Frame3.Text = "Order Type"
        '
        '_optType_3
        '
        Me._optType_3.BackColor = System.Drawing.SystemColors.Control
        Me._optType_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optType_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optType_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_3, CType(3, Short))
        Me._optType_3.Location = New System.Drawing.Point(8, 64)
        Me._optType_3.Name = "_optType_3"
        Me._optType_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optType_3.Size = New System.Drawing.Size(90, 17)
        Me._optType_3.TabIndex = 26
        Me._optType_3.TabStop = True
        Me._optType_3.Text = "Transfer"
        Me._optType_3.UseVisualStyleBackColor = False
        '
        '_optType_0
        '
        Me._optType_0.BackColor = System.Drawing.SystemColors.Control
        Me._optType_0.Checked = True
        Me._optType_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optType_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_0, CType(0, Short))
        Me._optType_0.Location = New System.Drawing.Point(8, 16)
        Me._optType_0.Name = "_optType_0"
        Me._optType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optType_0.Size = New System.Drawing.Size(90, 17)
        Me._optType_0.TabIndex = 23
        Me._optType_0.TabStop = True
        Me._optType_0.Text = "Ignore Field"
        Me._optType_0.UseVisualStyleBackColor = False
        '
        '_optType_2
        '
        Me._optType_2.BackColor = System.Drawing.SystemColors.Control
        Me._optType_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optType_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optType_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_2, CType(2, Short))
        Me._optType_2.Location = New System.Drawing.Point(8, 48)
        Me._optType_2.Name = "_optType_2"
        Me._optType_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optType_2.Size = New System.Drawing.Size(90, 17)
        Me._optType_2.TabIndex = 25
        Me._optType_2.TabStop = True
        Me._optType_2.Text = "Distribution"
        Me._optType_2.UseVisualStyleBackColor = False
        '
        '_optType_1
        '
        Me._optType_1.BackColor = System.Drawing.SystemColors.Control
        Me._optType_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optType_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_1, CType(1, Short))
        Me._optType_1.Location = New System.Drawing.Point(8, 32)
        Me._optType_1.Name = "_optType_1"
        Me._optType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optType_1.Size = New System.Drawing.Size(90, 17)
        Me._optType_1.TabIndex = 24
        Me._optType_1.TabStop = True
        Me._optType_1.Text = "Purchase"
        Me._optType_1.UseVisualStyleBackColor = False
        '
        '_cmbField_0
        '
        Me._cmbField_0.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_0.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_0.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_0, CType(0, Short))
        Me._cmbField_0.Location = New System.Drawing.Point(108, 202)
        Me._cmbField_0.Name = "_cmbField_0"
        Me._cmbField_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_0.Size = New System.Drawing.Size(201, 22)
        Me._cmbField_0.Sorted = True
        Me._cmbField_0.TabIndex = 10
        '
        '_txtField_5
        '
        Me._txtField_5.AcceptsReturn = True
        Me._txtField_5.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_5, CType(5, Short))
        Me._txtField_5.Location = New System.Drawing.Point(108, 178)
        Me._txtField_5.MaxLength = 50
        Me._txtField_5.Name = "_txtField_5"
        Me._txtField_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_5.Size = New System.Drawing.Size(201, 20)
        Me._txtField_5.TabIndex = 8
        Me._txtField_5.Tag = "Number"
        '
        '_txtField_4
        '
        Me._txtField_4.AcceptsReturn = True
        Me._txtField_4.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_4, CType(4, Short))
        Me._txtField_4.Location = New System.Drawing.Point(108, 7)
        Me._txtField_4.MaxLength = 50
        Me._txtField_4.Name = "_txtField_4"
        Me._txtField_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_4.Size = New System.Drawing.Size(185, 20)
        Me._txtField_4.TabIndex = 1
        Me._txtField_4.Tag = "String"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me._optOpen_3)
        Me.Frame2.Controls.Add(Me._optOpen_2)
        Me.Frame2.Controls.Add(Me._optOpen_1)
        Me.Frame2.Controls.Add(Me._optOpen_0)
        Me.Frame2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(411, 7)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(100, 89)
        Me.Frame2.TabIndex = 18
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Open Status"
        '
        '_optOpen_3
        '
        Me._optOpen_3.BackColor = System.Drawing.SystemColors.Control
        Me._optOpen_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optOpen_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optOpen_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optOpen.SetIndex(Me._optOpen_3, CType(3, Short))
        Me._optOpen_3.Location = New System.Drawing.Point(8, 65)
        Me._optOpen_3.Name = "_optOpen_3"
        Me._optOpen_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optOpen_3.Size = New System.Drawing.Size(90, 17)
        Me._optOpen_3.TabIndex = 23
        Me._optOpen_3.TabStop = True
        Me._optOpen_3.Text = "Deleted"
        Me._optOpen_3.UseVisualStyleBackColor = False
        '
        '_optOpen_2
        '
        Me._optOpen_2.BackColor = System.Drawing.SystemColors.Control
        Me._optOpen_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optOpen_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optOpen_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optOpen.SetIndex(Me._optOpen_2, CType(2, Short))
        Me._optOpen_2.Location = New System.Drawing.Point(8, 48)
        Me._optOpen_2.Name = "_optOpen_2"
        Me._optOpen_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optOpen_2.Size = New System.Drawing.Size(90, 17)
        Me._optOpen_2.TabIndex = 21
        Me._optOpen_2.TabStop = True
        Me._optOpen_2.Text = "Closed"
        Me._optOpen_2.UseVisualStyleBackColor = False
        '
        '_optOpen_1
        '
        Me._optOpen_1.BackColor = System.Drawing.SystemColors.Control
        Me._optOpen_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optOpen_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optOpen_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optOpen.SetIndex(Me._optOpen_1, CType(1, Short))
        Me._optOpen_1.Location = New System.Drawing.Point(8, 32)
        Me._optOpen_1.Name = "_optOpen_1"
        Me._optOpen_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optOpen_1.Size = New System.Drawing.Size(90, 17)
        Me._optOpen_1.TabIndex = 20
        Me._optOpen_1.TabStop = True
        Me._optOpen_1.Text = "Open"
        Me._optOpen_1.UseVisualStyleBackColor = False
        '
        '_optOpen_0
        '
        Me._optOpen_0.BackColor = System.Drawing.SystemColors.Control
        Me._optOpen_0.Checked = True
        Me._optOpen_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optOpen_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optOpen_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optOpen.SetIndex(Me._optOpen_0, CType(0, Short))
        Me._optOpen_0.Location = New System.Drawing.Point(8, 16)
        Me._optOpen_0.Name = "_optOpen_0"
        Me._optOpen_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optOpen_0.Size = New System.Drawing.Size(90, 17)
        Me._optOpen_0.TabIndex = 19
        Me._optOpen_0.TabStop = True
        Me._optOpen_0.Text = "Ignore Field"
        Me._optOpen_0.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me._optSend_1)
        Me.Frame1.Controls.Add(Me._optSend_0)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(305, 7)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(100, 57)
        Me.Frame1.TabIndex = 14
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Order Status"
        '
        '_optSend_1
        '
        Me._optSend_1.BackColor = System.Drawing.SystemColors.Control
        Me._optSend_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSend_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSend_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSend.SetIndex(Me._optSend_1, CType(1, Short))
        Me._optSend_1.Location = New System.Drawing.Point(8, 32)
        Me._optSend_1.Name = "_optSend_1"
        Me._optSend_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSend_1.Size = New System.Drawing.Size(90, 17)
        Me._optSend_1.TabIndex = 16
        Me._optSend_1.TabStop = True
        Me._optSend_1.Text = "Sent"
        Me._optSend_1.UseVisualStyleBackColor = False
        '
        '_optSend_0
        '
        Me._optSend_0.BackColor = System.Drawing.SystemColors.Control
        Me._optSend_0.Checked = True
        Me._optSend_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSend_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSend_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSend.SetIndex(Me._optSend_0, CType(0, Short))
        Me._optSend_0.Location = New System.Drawing.Point(8, 16)
        Me._optSend_0.Name = "_optSend_0"
        Me._optSend_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSend_0.Size = New System.Drawing.Size(90, 17)
        Me._optSend_0.TabIndex = 15
        Me._optSend_0.TabStop = True
        Me._optSend_0.Text = "Ignore Field"
        Me._optSend_0.UseVisualStyleBackColor = False
        '
        'lblLotNum
        '
        Me.lblLotNum.BackColor = System.Drawing.SystemColors.Control
        Me.lblLotNum.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLotNum.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLotNum.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLotNum.Location = New System.Drawing.Point(243, 121)
        Me.lblLotNum.Name = "lblLotNum"
        Me.lblLotNum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLotNum.Size = New System.Drawing.Size(201, 17)
        Me.lblLotNum.TabIndex = 48
        Me.lblLotNum.Text = "Orders Containing Items in Lot No :"
        '
        '_lblLabel_12
        '
        Me._lblLabel_12.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_12.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_12, CType(12, Short))
        Me._lblLabel_12.Location = New System.Drawing.Point(0, 103)
        Me._lblLabel_12.Name = "_lblLabel_12"
        Me._lblLabel_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_12.Size = New System.Drawing.Size(101, 17)
        Me._lblLabel_12.TabIndex = 47
        Me._lblLabel_12.Text = "Expected Date :"
        Me._lblLabel_12.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_11
        '
        Me._lblLabel_11.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_11.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_11, CType(11, Short))
        Me._lblLabel_11.Location = New System.Drawing.Point(307, 148)
        Me._lblLabel_11.Name = "_lblLabel_11"
        Me._lblLabel_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_11.Size = New System.Drawing.Size(137, 17)
        Me._lblLabel_11.TabIndex = 46
        Me._lblLabel_11.Text = "Warehouse Sent Date :"
        Me._lblLabel_11.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_10
        '
        Me._lblLabel_10.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_10.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_10, CType(10, Short))
        Me._lblLabel_10.Location = New System.Drawing.Point(355, 230)
        Me._lblLabel_10.Name = "_lblLabel_10"
        Me._lblLabel_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_10.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_10.TabIndex = 45
        Me._lblLabel_10.Text = "To Subteam :"
        Me._lblLabel_10.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_9
        '
        Me._lblLabel_9.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_9.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_9, CType(9, Short))
        Me._lblLabel_9.Location = New System.Drawing.Point(355, 200)
        Me._lblLabel_9.Name = "_lblLabel_9"
        Me._lblLabel_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_9.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_9.TabIndex = 44
        Me._lblLabel_9.Text = "Ship To :"
        Me._lblLabel_9.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_8
        '
        Me._lblLabel_8.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_8.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_8, CType(8, Short))
        Me._lblLabel_8.Location = New System.Drawing.Point(363, 174)
        Me._lblLabel_8.Name = "_lblLabel_8"
        Me._lblLabel_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_8.Size = New System.Drawing.Size(81, 17)
        Me._lblLabel_8.TabIndex = 42
        Me._lblLabel_8.Text = "Item Desc :"
        Me._lblLabel_8.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_7
        '
        Me._lblLabel_7.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_7.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_7, CType(7, Short))
        Me._lblLabel_7.Location = New System.Drawing.Point(5, 230)
        Me._lblLabel_7.Name = "_lblLabel_7"
        Me._lblLabel_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_7.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_7.TabIndex = 36
        Me._lblLabel_7.Text = "From Subteam :"
        Me._lblLabel_7.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_6
        '
        Me._lblLabel_6.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_6.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_6, CType(6, Short))
        Me._lblLabel_6.Location = New System.Drawing.Point(21, 205)
        Me._lblLabel_6.Name = "_lblLabel_6"
        Me._lblLabel_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_6.Size = New System.Drawing.Size(81, 17)
        Me._lblLabel_6.TabIndex = 43
        Me._lblLabel_6.Text = "Created By :"
        Me._lblLabel_6.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Location = New System.Drawing.Point(21, 181)
        Me._lblLabel_5.Name = "_lblLabel_5"
        Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_5.Size = New System.Drawing.Size(81, 17)
        Me._lblLabel_5.TabIndex = 41
        Me._lblLabel_5.Text = "Identifier :"
        Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_4, CType(4, Short))
        Me._lblLabel_4.Location = New System.Drawing.Point(20, 55)
        Me._lblLabel_4.Name = "_lblLabel_4"
        Me._lblLabel_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_4.Size = New System.Drawing.Size(81, 17)
        Me._lblLabel_4.TabIndex = 38
        Me._lblLabel_4.Text = "Invoice No :"
        Me._lblLabel_4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_3
        '
        Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_3, CType(3, Short))
        Me._lblLabel_3.Location = New System.Drawing.Point(29, 131)
        Me._lblLabel_3.Name = "_lblLabel_3"
        Me._lblLabel_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_3.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_3.TabIndex = 40
        Me._lblLabel_3.Text = "Sent Date :"
        Me._lblLabel_3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(28, 79)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_2.TabIndex = 39
        Me._lblLabel_2.Text = "Order Date :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(23, 31)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(78, 17)
        Me._lblLabel_1.TabIndex = 37
        Me._lblLabel_1.Text = "PO Number :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(28, 7)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_0.TabIndex = 35
        Me._lblLabel_0.Text = "Vendor :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbField
        '
        '
        '_cmbField_4
        '
        Me._cmbField_4.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_4, CType(4, Short))
        Me._cmbField_4.Location = New System.Drawing.Point(108, 153)
        Me._cmbField_4.Name = "_cmbField_4"
        Me._cmbField_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_4.Size = New System.Drawing.Size(121, 22)
        Me._cmbField_4.TabIndex = 102
        '
        'txtField
        '
        '
        'ugrdSearchResults
        '
        Me.ugrdSearchResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdSearchResults.DisplayLayout.Appearance = Appearance1
        Me.ugrdSearchResults.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        UltraGridColumn1.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn1.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn1.Header.Caption = "Credit"
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.RowLayoutColumnInfo.OriginX = 0
        UltraGridColumn1.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn1.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(41, 0)
        UltraGridColumn1.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn1.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn1.Width = 60
        UltraGridColumn2.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn2.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.Caption = "PO#"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.RowLayoutColumnInfo.OriginX = 3
        UltraGridColumn2.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn2.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn2.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn2.Width = 60
        UltraGridColumn3.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn3.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn3.Header.Caption = "Vendor"
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.RowLayoutColumnInfo.OriginX = 5
        UltraGridColumn3.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(201, 0)
        UltraGridColumn3.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn3.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn4.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn4.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn4.Header.Caption = "Ordered"
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.RowLayoutColumnInfo.OriginX = 7
        UltraGridColumn4.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn4.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn4.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn4.Width = 65
        UltraGridColumn5.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn5.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn5.Header.Caption = "Expected"
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.RowLayoutColumnInfo.OriginX = 9
        UltraGridColumn5.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn5.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn5.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn5.Width = 65
        UltraGridColumn6.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn6.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn6.Header.Caption = "Sent"
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.RowLayoutColumnInfo.OriginX = 11
        UltraGridColumn6.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn6.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn6.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn6.Width = 65
        UltraGridColumn7.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn7.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn7.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn7.Header.Caption = "Closed"
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.RowLayoutColumnInfo.OriginX = 16
        UltraGridColumn7.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn7.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(94, 0)
        UltraGridColumn7.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn7.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn7.Width = 65
        UltraGridColumn8.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn8.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn8.Header.Caption = "Subteam"
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.RowLayoutColumnInfo.OriginX = 13
        UltraGridColumn8.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn8.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(121, 0)
        UltraGridColumn8.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn8.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn9.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn9.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn9.Format = "##,##0.00##"
        UltraGridColumn9.Header.Caption = "Ordered Cost"
        UltraGridColumn9.Header.VisiblePosition = 8
        UltraGridColumn9.RowLayoutColumnInfo.OriginX = 15
        UltraGridColumn9.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn9.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(91, 0)
        UltraGridColumn9.RowLayoutColumnInfo.SpanX = 1
        UltraGridColumn9.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn10.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn10.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn10.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn10.Header.VisiblePosition = 9
        UltraGridColumn10.RowLayoutColumnInfo.OriginX = 2
        UltraGridColumn10.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn10.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(60, 0)
        UltraGridColumn10.RowLayoutColumnInfo.SpanX = 1
        UltraGridColumn10.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn10.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
        UltraGridColumn11.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn11.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn11.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn11.Header.VisiblePosition = 10
        UltraGridColumn11.RowLayoutColumnInfo.OriginX = 19
        UltraGridColumn11.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn11.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(96, 0)
        UltraGridColumn11.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn11.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn12.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn12.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn12.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn12.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn12.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn12.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn12.Header.Caption = "Deleted"
        UltraGridColumn12.Header.VisiblePosition = 11
        UltraGridColumn12.RowLayoutColumnInfo.OriginX = 18
        UltraGridColumn12.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn12.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(117, 0)
        UltraGridColumn12.RowLayoutColumnInfo.SpanX = 1
        UltraGridColumn12.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn13.Header.VisiblePosition = 12
        UltraGridColumn13.RowLayoutColumnInfo.OriginX = 21
        UltraGridColumn13.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn13.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(99, 0)
        UltraGridColumn13.RowLayoutColumnInfo.SpanX = 1
        UltraGridColumn13.RowLayoutColumnInfo.SpanY = 2
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout
        Me.ugrdSearchResults.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdSearchResults.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = "True"
        Me.ugrdSearchResults.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdSearchResults.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdSearchResults.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdSearchResults.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdSearchResults.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Appearance7.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdSearchResults.DisplayLayout.Override.ActiveRowAppearance = Appearance7
        Me.ugrdSearchResults.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdSearchResults.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance8.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdSearchResults.DisplayLayout.Override.CardAreaAppearance = Appearance8
        Appearance9.BorderColor = System.Drawing.Color.Silver
        Appearance9.FontData.BoldAsString = "True"
        Appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdSearchResults.DisplayLayout.Override.CellAppearance = Appearance9
        Me.ugrdSearchResults.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdSearchResults.DisplayLayout.Override.CellPadding = 0
        Appearance10.FontData.BoldAsString = "True"
        Me.ugrdSearchResults.DisplayLayout.Override.FixedHeaderAppearance = Appearance10
        Appearance11.BackColor = System.Drawing.SystemColors.Control
        Appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance11.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance11.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdSearchResults.DisplayLayout.Override.GroupByRowAppearance = Appearance11
        Appearance12.FontData.BoldAsString = "True"
        Appearance12.TextHAlignAsString = "Left"
        Me.ugrdSearchResults.DisplayLayout.Override.HeaderAppearance = Appearance12
        Me.ugrdSearchResults.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdSearchResults.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance13.BackColor = System.Drawing.SystemColors.Control
        Me.ugrdSearchResults.DisplayLayout.Override.RowAlternateAppearance = Appearance13
        Appearance14.BackColor = System.Drawing.SystemColors.Window
        Appearance14.BorderColor = System.Drawing.Color.Silver
        Me.ugrdSearchResults.DisplayLayout.Override.RowAppearance = Appearance14
        Me.ugrdSearchResults.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdSearchResults.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdSearchResults.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdSearchResults.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdSearchResults.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdSearchResults.DisplayLayout.Override.TemplateAddRowAppearance = Appearance15
        Me.ugrdSearchResults.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdSearchResults.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdSearchResults.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdSearchResults.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdSearchResults.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugrdSearchResults.Location = New System.Drawing.Point(10, 259)
        Me.ugrdSearchResults.Name = "ugrdSearchResults"
        Me.ugrdSearchResults.Size = New System.Drawing.Size(1198, 242)
        Me.ugrdSearchResults.TabIndex = 49
        Me.ugrdSearchResults.Text = "Search Results"
        '
        'dtpWarehouseSentDate
        '
        Me.dtpWarehouseSentDate.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpWarehouseSentDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpWarehouseSentDate.Location = New System.Drawing.Point(450, 144)
        Me.dtpWarehouseSentDate.MaskInput = ""
        Me.dtpWarehouseSentDate.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me.dtpWarehouseSentDate.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpWarehouseSentDate.Name = "dtpWarehouseSentDate"
        Me.dtpWarehouseSentDate.Size = New System.Drawing.Size(85, 21)
        Me.dtpWarehouseSentDate.TabIndex = 95
        Me.dtpWarehouseSentDate.Value = Nothing
        '
        'dtpOrderDate
        '
        Me.dtpOrderDate.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpOrderDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpOrderDate.Location = New System.Drawing.Point(108, 79)
        Me.dtpOrderDate.MaskInput = ""
        Me.dtpOrderDate.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me.dtpOrderDate.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpOrderDate.Name = "dtpOrderDate"
        Me.dtpOrderDate.Size = New System.Drawing.Size(85, 21)
        Me.dtpOrderDate.TabIndex = 96
        Me.dtpOrderDate.Value = Nothing
        '
        'dtpExpectedDate
        '
        Me.dtpExpectedDate.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpExpectedDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpExpectedDate.Location = New System.Drawing.Point(108, 103)
        Me.dtpExpectedDate.MaskInput = ""
        Me.dtpExpectedDate.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me.dtpExpectedDate.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpExpectedDate.Name = "dtpExpectedDate"
        Me.dtpExpectedDate.Size = New System.Drawing.Size(85, 21)
        Me.dtpExpectedDate.TabIndex = 97
        Me.dtpExpectedDate.Value = Nothing
        '
        'dtpSentDate
        '
        Me.dtpSentDate.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpSentDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpSentDate.Location = New System.Drawing.Point(108, 127)
        Me.dtpSentDate.MaskInput = ""
        Me.dtpSentDate.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me.dtpSentDate.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpSentDate.Name = "dtpSentDate"
        Me.dtpSentDate.Size = New System.Drawing.Size(85, 21)
        Me.dtpSentDate.TabIndex = 98
        Me.dtpSentDate.Value = Nothing
        '
        '_lblLabel_13
        '
        Me._lblLabel_13.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_13.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_13.Location = New System.Drawing.Point(29, 156)
        Me._lblLabel_13.Name = "_lblLabel_13"
        Me._lblLabel_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_13.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_13.TabIndex = 101
        Me._lblLabel_13.Text = "Source :"
        Me._lblLabel_13.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkEInvoice
        '
        Me.chkEInvoice.BackColor = System.Drawing.SystemColors.Control
        Me.chkEInvoice.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkEInvoice.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEInvoice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEInvoice.Location = New System.Drawing.Point(308, 87)
        Me.chkEInvoice.Name = "chkEInvoice"
        Me.chkEInvoice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkEInvoice.Size = New System.Drawing.Size(97, 17)
        Me.chkEInvoice.TabIndex = 103
        Me.chkEInvoice.Text = "EInvoice"
        Me.chkEInvoice.UseVisualStyleBackColor = False
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox1.Controls.Add(Me.chkRefusedPO)
        Me.GroupBox1.Controls.Add(Me.chkPartialShipment)
        Me.GroupBox1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox1.Location = New System.Drawing.Point(729, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox1.Size = New System.Drawing.Size(138, 66)
        Me.GroupBox1.TabIndex = 104
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Status"
        '
        'chkRefusedPO
        '
        Me.chkRefusedPO.BackColor = System.Drawing.SystemColors.Control
        Me.chkRefusedPO.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRefusedPO.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRefusedPO.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRefusedPO.Location = New System.Drawing.Point(9, 34)
        Me.chkRefusedPO.Name = "chkRefusedPO"
        Me.chkRefusedPO.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRefusedPO.Size = New System.Drawing.Size(112, 17)
        Me.chkRefusedPO.TabIndex = 105
        Me.chkRefusedPO.TabStop = False
        Me.chkRefusedPO.Text = "Refused PO"
        Me.chkRefusedPO.UseVisualStyleBackColor = False
        '
        'chkPartialShipment
        '
        Me.chkPartialShipment.BackColor = System.Drawing.SystemColors.Control
        Me.chkPartialShipment.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPartialShipment.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPartialShipment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPartialShipment.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkPartialShipment.Location = New System.Drawing.Point(9, 19)
        Me.chkPartialShipment.Name = "chkPartialShipment"
        Me.chkPartialShipment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPartialShipment.Size = New System.Drawing.Size(121, 17)
        Me.chkPartialShipment.TabIndex = 18
        Me.chkPartialShipment.Text = "Partial Shipment"
        Me.chkPartialShipment.UseVisualStyleBackColor = False
        '
        'frmOrdersSearch
        '
        Me.AcceptButton = Me.cmdSearch
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(1216, 554)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.chkEInvoice)
        Me.Controls.Add(Me._cmbField_4)
        Me.Controls.Add(Me._lblLabel_13)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.dtpSentDate)
        Me.Controls.Add(Me.dtpExpectedDate)
        Me.Controls.Add(Me.dtpOrderDate)
        Me.Controls.Add(Me.dtpWarehouseSentDate)
        Me.Controls.Add(Me.ugrdSearchResults)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.chkFromQueue)
        Me.Controls.Add(Me._txtField_9)
        Me.Controls.Add(Me.Frame4)
        Me.Controls.Add(Me._txtField_0)
        Me.Controls.Add(Me._txtField_3)
        Me.Controls.Add(Me._cmbField_3)
        Me.Controls.Add(Me._cmbField_2)
        Me.Controls.Add(Me._txtField_6)
        Me.Controls.Add(Me._cmbField_1)
        Me.Controls.Add(Me.Frame3)
        Me.Controls.Add(Me._cmbField_0)
        Me.Controls.Add(Me._txtField_5)
        Me.Controls.Add(Me._txtField_4)
        Me.Controls.Add(Me.Frame2)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.lblLotNum)
        Me.Controls.Add(Me._lblLabel_12)
        Me.Controls.Add(Me._lblLabel_11)
        Me.Controls.Add(Me._lblLabel_10)
        Me.Controls.Add(Me._lblLabel_9)
        Me.Controls.Add(Me._lblLabel_8)
        Me.Controls.Add(Me._lblLabel_7)
        Me.Controls.Add(Me._lblLabel_6)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me._lblLabel_4)
        Me.Controls.Add(Me._lblLabel_3)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(6, 109)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOrdersSearch"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Order Search"
        Me.Frame4.ResumeLayout(False)
        Me.Frame3.ResumeLayout(False)
        Me.Frame2.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optCredit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optOpen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optSend, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdSearchResults, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpWarehouseSentDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpOrderDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpExpectedDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpSentDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ugrdSearchResults As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents dtpWarehouseSentDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents dtpOrderDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents dtpExpectedDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents dtpSentDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Public WithEvents btnClear As System.Windows.Forms.Button
    Public WithEvents _lblLabel_13 As System.Windows.Forms.Label
    Public WithEvents chkEInvoice As System.Windows.Forms.CheckBox
    Public WithEvents _optOpen_3 As System.Windows.Forms.RadioButton
    Public WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents chkRefusedPO As System.Windows.Forms.CheckBox
    Public WithEvents chkPartialShipment As System.Windows.Forms.CheckBox
#End Region
End Class
