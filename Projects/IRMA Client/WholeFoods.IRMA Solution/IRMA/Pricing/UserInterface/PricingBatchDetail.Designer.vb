<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmPricingBatchDetail
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()

        isinitializing = True

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
            cdbFileOpen.Dispose()
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents _optPrintSign_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optPrintSign_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optPrintSign_2 As System.Windows.Forms.RadioButton
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents _cmdMaintain_0 As System.Windows.Forms.Button
	Public WithEvents _cmdMaintain_1 As System.Windows.Forms.Button
	Public WithEvents _cmdMaintain_2 As System.Windows.Forms.Button
	Public WithEvents fraMaintain As System.Windows.Forms.Panel
	Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
	Public WithEvents cmbTagType As System.Windows.Forms.ComboBox
	Public WithEvents cmdPrint As System.Windows.Forms.Button
	Public WithEvents _txtField_4 As System.Windows.Forms.TextBox
    Public WithEvents chkExport As System.Windows.Forms.CheckBox
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_8 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents fraProcess As System.Windows.Forms.GroupBox
	Public WithEvents _StatusBar1_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents _StatusBar1_Panel2 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents StatusBar1 As System.Windows.Forms.StatusStrip
	Public WithEvents cmdExit As System.Windows.Forms.Button
    'Public WithEvents crwReport As AxCrystal.AxCrystalReport
	Public cdbFileOpen As System.Windows.Forms.OpenFileDialog
	Public cdbFileSave As System.Windows.Forms.SaveFileDialog
	Public cdbFileFont As System.Windows.Forms.FontDialog
	Public cdbFileColor As System.Windows.Forms.ColorDialog
	Public cdbFilePrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdMaintain As Microsoft.VisualBasic.Compatibility.VB6.ButtonArray
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents optPrintSign As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPricingBatchDetail))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Description")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("POSPrice")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StartDate")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sale_End_Date")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceBatchDetailID")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PrintSign")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("POSPriceWithMultiple", 0)
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
        Dim UltraDataColumn1 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Identifier")
        Dim UltraDataColumn2 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Item_Description")
        Dim UltraDataColumn3 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("POSPrice")
        Dim UltraDataColumn4 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("StartDate")
        Dim UltraDataColumn5 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Sale_End_Date")
        Dim UltraDataColumn6 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Item_Key")
        Dim UltraDataColumn7 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("PriceBatchDetailID")
        Dim UltraDataColumn8 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("PrintSign")
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._cmdMaintain_0 = New System.Windows.Forms.Button()
        Me._cmdMaintain_1 = New System.Windows.Forms.Button()
        Me._cmdMaintain_2 = New System.Windows.Forms.Button()
        Me.cmdPrint = New System.Windows.Forms.Button()
        Me._txtField_4 = New System.Windows.Forms.TextBox()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdSubmit = New System.Windows.Forms.Button()
        Me.cmdMarkAsPrinted = New System.Windows.Forms.Button()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me._optPrintSign_0 = New System.Windows.Forms.RadioButton()
        Me._optPrintSign_1 = New System.Windows.Forms.RadioButton()
        Me._optPrintSign_2 = New System.Windows.Forms.RadioButton()
        Me.fraMaintain = New System.Windows.Forms.Panel()
        Me.fraProcess = New System.Windows.Forms.GroupBox()
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox()
        Me.cmbTagType = New System.Windows.Forms.ComboBox()
        Me.chkExport = New System.Windows.Forms.CheckBox()
        Me._lblLabel_2 = New System.Windows.Forms.Label()
        Me._lblLabel_8 = New System.Windows.Forms.Label()
        Me._lblLabel_1 = New System.Windows.Forms.Label()
        Me._lblLabel_0 = New System.Windows.Forms.Label()
        Me.StatusBar1 = New System.Windows.Forms.StatusStrip()
        Me._StatusBar1_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me._StatusBar1_Panel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.cdbFileOpen = New System.Windows.Forms.OpenFileDialog()
        Me.cdbFileSave = New System.Windows.Forms.SaveFileDialog()
        Me.cdbFileFont = New System.Windows.Forms.FontDialog()
        Me.cdbFileColor = New System.Windows.Forms.ColorDialog()
        Me.cdbFilePrint = New System.Windows.Forms.PrintDialog()
        Me.cmdMaintain = New Microsoft.VisualBasic.Compatibility.VB6.ButtonArray(Me.components)
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optPrintSign = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.ugrdList = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.UltraDataSource1 = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.HeaderFrame = New System.Windows.Forms.GroupBox()
        Me.chkIgnoreNoTagLogic = New System.Windows.Forms.CheckBox()
        Me.PosBatchIdTextBox = New System.Windows.Forms.TextBox()
        Me.PosBatchIdLabel = New System.Windows.Forms.Label()
        Me.AutoApplyDateUDTE = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.BatchDescriptionTextBox = New System.Windows.Forms.TextBox()
        Me.BatchDescLabel = New System.Windows.Forms.Label()
        Me.ApplyDateLabel = New System.Windows.Forms.Label()
        Me.AutoApplyCheckBox = New System.Windows.Forms.CheckBox()
        Me.Frame1.SuspendLayout()
        Me.fraMaintain.SuspendLayout()
        Me.fraProcess.SuspendLayout()
        Me.StatusBar1.SuspendLayout()
        CType(Me.cmdMaintain, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optPrintSign, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.HeaderFrame.SuspendLayout()
        CType(Me.AutoApplyDateUDTE, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_cmdMaintain_0
        '
        Me._cmdMaintain_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdMaintain_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdMaintain_0.Font = New System.Drawing.Font("Arial", 8.0!)
        Me._cmdMaintain_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdMaintain_0.Image = CType(resources.GetObject("_cmdMaintain_0.Image"), System.Drawing.Image)
        Me.cmdMaintain.SetIndex(Me._cmdMaintain_0, CType(0, Short))
        Me._cmdMaintain_0.Location = New System.Drawing.Point(0, 0)
        Me._cmdMaintain_0.Name = "_cmdMaintain_0"
        Me._cmdMaintain_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdMaintain_0.Size = New System.Drawing.Size(41, 41)
        Me._cmdMaintain_0.TabIndex = 1
        Me._cmdMaintain_0.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me._cmdMaintain_0, "Add Detail")
        Me._cmdMaintain_0.UseVisualStyleBackColor = False
        '
        '_cmdMaintain_1
        '
        Me._cmdMaintain_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdMaintain_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdMaintain_1.Font = New System.Drawing.Font("Arial", 8.0!)
        Me._cmdMaintain_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdMaintain_1.Image = CType(resources.GetObject("_cmdMaintain_1.Image"), System.Drawing.Image)
        Me.cmdMaintain.SetIndex(Me._cmdMaintain_1, CType(1, Short))
        Me._cmdMaintain_1.Location = New System.Drawing.Point(40, 0)
        Me._cmdMaintain_1.Name = "_cmdMaintain_1"
        Me._cmdMaintain_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdMaintain_1.Size = New System.Drawing.Size(41, 41)
        Me._cmdMaintain_1.TabIndex = 2
        Me._cmdMaintain_1.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me._cmdMaintain_1, "Delete Detail")
        Me._cmdMaintain_1.UseVisualStyleBackColor = False
        '
        '_cmdMaintain_2
        '
        Me._cmdMaintain_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdMaintain_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdMaintain_2.Font = New System.Drawing.Font("Arial", 8.0!)
        Me._cmdMaintain_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdMaintain_2.Image = CType(resources.GetObject("_cmdMaintain_2.Image"), System.Drawing.Image)
        Me.cmdMaintain.SetIndex(Me._cmdMaintain_2, CType(2, Short))
        Me._cmdMaintain_2.Location = New System.Drawing.Point(80, 0)
        Me._cmdMaintain_2.Name = "_cmdMaintain_2"
        Me._cmdMaintain_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdMaintain_2.Size = New System.Drawing.Size(41, 41)
        Me._cmdMaintain_2.TabIndex = 3
        Me._cmdMaintain_2.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me._cmdMaintain_2, "Remove Detail From Batch")
        Me._cmdMaintain_2.UseVisualStyleBackColor = False
        '
        'cmdPrint
        '
        Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrint.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrint.Image = CType(resources.GetObject("cmdPrint.Image"), System.Drawing.Image)
        Me.cmdPrint.Location = New System.Drawing.Point(371, 18)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrint.Size = New System.Drawing.Size(41, 41)
        Me.cmdPrint.TabIndex = 9
        Me.cmdPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdPrint, "Print Tags")
        Me.cmdPrint.UseVisualStyleBackColor = False
        '
        '_txtField_4
        '
        Me._txtField_4.AcceptsReturn = True
        Me._txtField_4.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_4.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me._txtField_4.Enabled = False
        Me._txtField_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me._txtField_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_4, CType(4, Short))
        Me._txtField_4.Location = New System.Drawing.Point(89, 39)
        Me._txtField_4.MaxLength = 2
        Me._txtField_4.Name = "_txtField_4"
        Me._txtField_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_4.Size = New System.Drawing.Size(33, 20)
        Me._txtField_4.TabIndex = 6
        Me._txtField_4.Tag = "String"
        Me._txtField_4.Text = "1"
        Me._txtField_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me._txtField_4, "Enter Number of Labels to Print")
        Me._txtField_4.UseWaitCursor = True
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(735, 475)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 10
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Close")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdSubmit
        '
        Me.cmdSubmit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSubmit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSubmit.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdSubmit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSubmit.Image = CType(resources.GetObject("cmdSubmit.Image"), System.Drawing.Image)
        Me.cmdSubmit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdSubmit.Location = New System.Drawing.Point(556, 19)
        Me.cmdSubmit.Name = "cmdSubmit"
        Me.cmdSubmit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSubmit.Size = New System.Drawing.Size(41, 41)
        Me.cmdSubmit.TabIndex = 113
        Me.cmdSubmit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSubmit, "Update Batch")
        Me.cmdSubmit.UseVisualStyleBackColor = False
        '
        'cmdMarkAsPrinted
        '
        Me.cmdMarkAsPrinted.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMarkAsPrinted.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMarkAsPrinted.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmdMarkAsPrinted.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMarkAsPrinted.Image = CType(resources.GetObject("cmdMarkAsPrinted.Image"), System.Drawing.Image)
        Me.cmdMarkAsPrinted.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdMarkAsPrinted.Location = New System.Drawing.Point(425, 17)
        Me.cmdMarkAsPrinted.Name = "cmdMarkAsPrinted"
        Me.cmdMarkAsPrinted.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMarkAsPrinted.Size = New System.Drawing.Size(41, 41)
        Me.cmdMarkAsPrinted.TabIndex = 35
        Me.cmdMarkAsPrinted.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdMarkAsPrinted, "Mark As Printed")
        Me.cmdMarkAsPrinted.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me._optPrintSign_0)
        Me.Frame1.Controls.Add(Me._optPrintSign_1)
        Me.Frame1.Controls.Add(Me._optPrintSign_2)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(10, 1)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(766, 49)
        Me.Frame1.TabIndex = 21
        Me.Frame1.TabStop = False
        '
        '_optPrintSign_0
        '
        Me._optPrintSign_0.BackColor = System.Drawing.SystemColors.Control
        Me._optPrintSign_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optPrintSign_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me._optPrintSign_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPrintSign.SetIndex(Me._optPrintSign_0, CType(0, Short))
        Me._optPrintSign_0.Location = New System.Drawing.Point(16, 16)
        Me._optPrintSign_0.Name = "_optPrintSign_0"
        Me._optPrintSign_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optPrintSign_0.Size = New System.Drawing.Size(105, 17)
        Me._optPrintSign_0.TabIndex = 11
        Me._optPrintSign_0.TabStop = True
        Me._optPrintSign_0.Text = "Tag Required"
        Me._optPrintSign_0.UseVisualStyleBackColor = False
        '
        '_optPrintSign_1
        '
        Me._optPrintSign_1.BackColor = System.Drawing.SystemColors.Control
        Me._optPrintSign_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optPrintSign_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me._optPrintSign_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPrintSign.SetIndex(Me._optPrintSign_1, CType(1, Short))
        Me._optPrintSign_1.Location = New System.Drawing.Point(136, 16)
        Me._optPrintSign_1.Name = "_optPrintSign_1"
        Me._optPrintSign_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optPrintSign_1.Size = New System.Drawing.Size(145, 17)
        Me._optPrintSign_1.TabIndex = 12
        Me._optPrintSign_1.TabStop = True
        Me._optPrintSign_1.Text = "No-Tag Required"
        Me._optPrintSign_1.UseVisualStyleBackColor = False
        '
        '_optPrintSign_2
        '
        Me._optPrintSign_2.BackColor = System.Drawing.SystemColors.Control
        Me._optPrintSign_2.Checked = True
        Me._optPrintSign_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optPrintSign_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me._optPrintSign_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPrintSign.SetIndex(Me._optPrintSign_2, CType(2, Short))
        Me._optPrintSign_2.Location = New System.Drawing.Point(296, 16)
        Me._optPrintSign_2.Name = "_optPrintSign_2"
        Me._optPrintSign_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optPrintSign_2.Size = New System.Drawing.Size(105, 17)
        Me._optPrintSign_2.TabIndex = 13
        Me._optPrintSign_2.TabStop = True
        Me._optPrintSign_2.Text = "All"
        Me._optPrintSign_2.UseVisualStyleBackColor = False
        '
        'fraMaintain
        '
        Me.fraMaintain.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.fraMaintain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.fraMaintain.BackColor = System.Drawing.SystemColors.Control
        Me.fraMaintain.Controls.Add(Me._cmdMaintain_0)
        Me.fraMaintain.Controls.Add(Me._cmdMaintain_1)
        Me.fraMaintain.Controls.Add(Me._cmdMaintain_2)
        Me.fraMaintain.Cursor = System.Windows.Forms.Cursors.Default
        Me.fraMaintain.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.fraMaintain.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMaintain.Location = New System.Drawing.Point(12, 403)
        Me.fraMaintain.Name = "fraMaintain"
        Me.fraMaintain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMaintain.Size = New System.Drawing.Size(129, 49)
        Me.fraMaintain.TabIndex = 19
        '
        'fraProcess
        '
        Me.fraProcess.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraProcess.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.fraProcess.BackColor = System.Drawing.SystemColors.Control
        Me.fraProcess.Controls.Add(Me.cmdMarkAsPrinted)
        Me.fraProcess.Controls.Add(Me.chkPrintOnly)
        Me.fraProcess.Controls.Add(Me.cmbTagType)
        Me.fraProcess.Controls.Add(Me.cmdPrint)
        Me.fraProcess.Controls.Add(Me._txtField_4)
        Me.fraProcess.Controls.Add(Me.chkExport)
        Me.fraProcess.Controls.Add(Me._lblLabel_2)
        Me.fraProcess.Controls.Add(Me._lblLabel_8)
        Me.fraProcess.Controls.Add(Me._lblLabel_1)
        Me.fraProcess.Controls.Add(Me._lblLabel_0)
        Me.fraProcess.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.fraProcess.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraProcess.Location = New System.Drawing.Point(173, 458)
        Me.fraProcess.Name = "fraProcess"
        Me.fraProcess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraProcess.Size = New System.Drawing.Size(472, 62)
        Me.fraProcess.TabIndex = 15
        Me.fraProcess.TabStop = False
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(348, 41)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(17, 17)
        Me.chkPrintOnly.TabIndex = 8
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        Me.chkPrintOnly.Visible = False
        '
        'cmbTagType
        '
        Me.cmbTagType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbTagType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbTagType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbTagType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbTagType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTagType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbTagType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbTagType.Items.AddRange(New Object() {"Grocery", "Nutrition"})
        Me.cmbTagType.Location = New System.Drawing.Point(89, 14)
        Me.cmbTagType.Name = "cmbTagType"
        Me.cmbTagType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbTagType.Size = New System.Drawing.Size(129, 22)
        Me.cmbTagType.Sorted = True
        Me.cmbTagType.TabIndex = 4
        '
        'chkExport
        '
        Me.chkExport.BackColor = System.Drawing.SystemColors.Control
        Me.chkExport.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkExport.Enabled = False
        Me.chkExport.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.chkExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkExport.Location = New System.Drawing.Point(202, 41)
        Me.chkExport.Name = "chkExport"
        Me.chkExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkExport.Size = New System.Drawing.Size(17, 17)
        Me.chkExport.TabIndex = 7
        Me.chkExport.UseVisualStyleBackColor = False
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(269, 40)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_2.TabIndex = 22
        Me._lblLabel_2.Text = "Print Only :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblLabel_2.Visible = False
        '
        '_lblLabel_8
        '
        Me._lblLabel_8.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_8.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me._lblLabel_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_8, CType(8, Short))
        Me._lblLabel_8.Location = New System.Drawing.Point(9, 16)
        Me._lblLabel_8.Name = "_lblLabel_8"
        Me._lblLabel_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_8.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_8.TabIndex = 20
        Me._lblLabel_8.Text = "Tag Type :"
        Me._lblLabel_8.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(147, 40)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(49, 17)
        Me._lblLabel_1.TabIndex = 18
        Me._lblLabel_1.Text = "Export :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(30, 39)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(59, 17)
        Me._lblLabel_0.TabIndex = 16
        Me._lblLabel_0.Text = "Copies :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'StatusBar1
        '
        Me.StatusBar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.StatusBar1.AutoSize = False
        Me.StatusBar1.Dock = System.Windows.Forms.DockStyle.None
        Me.StatusBar1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.StatusBar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusBar1_Panel1, Me._StatusBar1_Panel2})
        Me.StatusBar1.Location = New System.Drawing.Point(0, 522)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Size = New System.Drawing.Size(782, 25)
        Me.StatusBar1.TabIndex = 14
        '
        '_StatusBar1_Panel1
        '
        Me._StatusBar1_Panel1.AutoSize = False
        Me._StatusBar1_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._StatusBar1_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._StatusBar1_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusBar1_Panel1.Name = "_StatusBar1_Panel1"
        Me._StatusBar1_Panel1.Size = New System.Drawing.Size(96, 25)
        Me._StatusBar1_Panel1.Text = "0 Selected"
        Me._StatusBar1_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_StatusBar1_Panel2
        '
        Me._StatusBar1_Panel2.AutoSize = False
        Me._StatusBar1_Panel2.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._StatusBar1_Panel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._StatusBar1_Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusBar1_Panel2.Name = "_StatusBar1_Panel2"
        Me._StatusBar1_Panel2.Size = New System.Drawing.Size(96, 25)
        Me._StatusBar1_Panel2.Text = "0 Total"
        Me._StatusBar1_Panel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cdbFileOpen
        '
        Me.cdbFileOpen.DefaultExt = "DAT"
        '
        'cmdMaintain
        '
        '
        'optPrintSign
        '
        '
        'ugrdList
        '
        Me.ugrdList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdList.DisplayLayout.Appearance = Appearance1
        Me.ugrdList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 124
        UltraGridColumn2.Header.Caption = "Description"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 250
        UltraGridColumn3.Formula = "#####0.00"
        UltraGridColumn3.Header.Caption = "POS Price"
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Hidden = True
        UltraGridColumn3.Width = 135
        UltraGridColumn4.Header.Caption = "Start Date"
        UltraGridColumn4.Header.VisiblePosition = 4
        UltraGridColumn4.Width = 129
        UltraGridColumn5.Header.Caption = "End Date"
        UltraGridColumn5.Header.VisiblePosition = 5
        UltraGridColumn5.Width = 117
        UltraGridColumn6.Header.VisiblePosition = 6
        UltraGridColumn6.Hidden = True
        UltraGridColumn7.Header.VisiblePosition = 7
        UltraGridColumn7.Hidden = True
        UltraGridColumn8.Header.VisiblePosition = 8
        UltraGridColumn8.Hidden = True
        UltraGridColumn9.Header.Caption = "POS Price"
        UltraGridColumn9.Header.VisiblePosition = 3
        UltraGridColumn9.Width = 125
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9})
        Me.ugrdList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = "True"
        Me.ugrdList.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdList.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdList.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdList.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdList.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Me.ugrdList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdList.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.FontData.BoldAsString = "True"
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdList.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugrdList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdList.DisplayLayout.Override.CellPadding = 0
        Appearance9.FontData.BoldAsString = "True"
        Me.ugrdList.DisplayLayout.Override.FixedHeaderAppearance = Appearance9
        Appearance10.BackColor = System.Drawing.SystemColors.Control
        Appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance10.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdList.DisplayLayout.Override.GroupByRowAppearance = Appearance10
        Appearance11.FontData.BoldAsString = "True"
        Appearance11.TextHAlignAsString = "Left"
        Me.ugrdList.DisplayLayout.Override.HeaderAppearance = Appearance11
        Me.ugrdList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdList.DisplayLayout.Override.RowAlternateAppearance = Appearance12
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.Color.Silver
        Me.ugrdList.DisplayLayout.Override.RowAppearance = Appearance13
        Me.ugrdList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance14
        Me.ugrdList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdList.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.ugrdList.Location = New System.Drawing.Point(10, 56)
        Me.ugrdList.Name = "ugrdList"
        Me.ugrdList.Size = New System.Drawing.Size(766, 334)
        Me.ugrdList.TabIndex = 26
        Me.ugrdList.Text = "Search Results"
        '
        'UltraDataSource1
        '
        UltraDataColumn4.DataType = GetType(Date)
        UltraDataColumn5.DataType = GetType(Date)
        Me.UltraDataSource1.Band.Columns.AddRange(New Object() {UltraDataColumn1, UltraDataColumn2, UltraDataColumn3, UltraDataColumn4, UltraDataColumn5, UltraDataColumn6, UltraDataColumn7, UltraDataColumn8})
        '
        'HeaderFrame
        '
        Me.HeaderFrame.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HeaderFrame.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.HeaderFrame.Controls.Add(Me.chkIgnoreNoTagLogic)
        Me.HeaderFrame.Controls.Add(Me.PosBatchIdTextBox)
        Me.HeaderFrame.Controls.Add(Me.PosBatchIdLabel)
        Me.HeaderFrame.Controls.Add(Me.cmdSubmit)
        Me.HeaderFrame.Controls.Add(Me.AutoApplyDateUDTE)
        Me.HeaderFrame.Controls.Add(Me.BatchDescriptionTextBox)
        Me.HeaderFrame.Controls.Add(Me.BatchDescLabel)
        Me.HeaderFrame.Controls.Add(Me.ApplyDateLabel)
        Me.HeaderFrame.Controls.Add(Me.AutoApplyCheckBox)
        Me.HeaderFrame.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold)
        Me.HeaderFrame.ForeColor = System.Drawing.SystemColors.ControlText
        Me.HeaderFrame.Location = New System.Drawing.Point(173, 396)
        Me.HeaderFrame.Name = "HeaderFrame"
        Me.HeaderFrame.Size = New System.Drawing.Size(603, 66)
        Me.HeaderFrame.TabIndex = 107
        Me.HeaderFrame.TabStop = False
        Me.HeaderFrame.Text = "Batch Info"
        '
        'chkIgnoreNoTagLogic
        '
        Me.chkIgnoreNoTagLogic.AutoSize = True
        Me.chkIgnoreNoTagLogic.Enabled = False
        Me.chkIgnoreNoTagLogic.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.chkIgnoreNoTagLogic.Location = New System.Drawing.Point(415, 41)
        Me.chkIgnoreNoTagLogic.Name = "chkIgnoreNoTagLogic"
        Me.chkIgnoreNoTagLogic.Size = New System.Drawing.Size(135, 18)
        Me.chkIgnoreNoTagLogic.TabIndex = 116
        Me.chkIgnoreNoTagLogic.Text = "Ignore No-Tag Logic"
        Me.chkIgnoreNoTagLogic.UseVisualStyleBackColor = True
        '
        'PosBatchIdTextBox
        '
        Me.PosBatchIdTextBox.Location = New System.Drawing.Point(262, 39)
        Me.PosBatchIdTextBox.MaxLength = 3
        Me.PosBatchIdTextBox.Name = "PosBatchIdTextBox"
        Me.PosBatchIdTextBox.Size = New System.Drawing.Size(54, 20)
        Me.PosBatchIdTextBox.TabIndex = 115
        '
        'PosBatchIdLabel
        '
        Me.PosBatchIdLabel.AutoSize = True
        Me.PosBatchIdLabel.Location = New System.Drawing.Point(183, 42)
        Me.PosBatchIdLabel.Name = "PosBatchIdLabel"
        Me.PosBatchIdLabel.Size = New System.Drawing.Size(78, 14)
        Me.PosBatchIdLabel.TabIndex = 114
        Me.PosBatchIdLabel.Text = "POS Batch ID:"
        '
        'AutoApplyDateUDTE
        '
        Me.AutoApplyDateUDTE.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.AutoApplyDateUDTE.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.AutoApplyDateUDTE.Location = New System.Drawing.Point(89, 37)
        Me.AutoApplyDateUDTE.MaskInput = ""
        Me.AutoApplyDateUDTE.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me.AutoApplyDateUDTE.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.AutoApplyDateUDTE.Name = "AutoApplyDateUDTE"
        Me.AutoApplyDateUDTE.Size = New System.Drawing.Size(85, 21)
        Me.AutoApplyDateUDTE.TabIndex = 110
        Me.AutoApplyDateUDTE.Value = Nothing
        '
        'BatchDescriptionTextBox
        '
        Me.BatchDescriptionTextBox.AcceptsReturn = True
        Me.BatchDescriptionTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.BatchDescriptionTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.BatchDescriptionTextBox.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.BatchDescriptionTextBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.BatchDescriptionTextBox.Location = New System.Drawing.Point(262, 14)
        Me.BatchDescriptionTextBox.MaxLength = 30
        Me.BatchDescriptionTextBox.Multiline = True
        Me.BatchDescriptionTextBox.Name = "BatchDescriptionTextBox"
        Me.BatchDescriptionTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.BatchDescriptionTextBox.Size = New System.Drawing.Size(288, 21)
        Me.BatchDescriptionTextBox.TabIndex = 111
        Me.BatchDescriptionTextBox.Tag = "String"
        '
        'BatchDescLabel
        '
        Me.BatchDescLabel.BackColor = System.Drawing.Color.Transparent
        Me.BatchDescLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.BatchDescLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.BatchDescLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BatchDescLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.BatchDescLabel.Location = New System.Drawing.Point(183, 16)
        Me.BatchDescLabel.Name = "BatchDescLabel"
        Me.BatchDescLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.BatchDescLabel.Size = New System.Drawing.Size(85, 17)
        Me.BatchDescLabel.TabIndex = 112
        Me.BatchDescLabel.Text = "Batch  Desc :"
        Me.BatchDescLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ApplyDateLabel
        '
        Me.ApplyDateLabel.BackColor = System.Drawing.Color.Transparent
        Me.ApplyDateLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.ApplyDateLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ApplyDateLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ApplyDateLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ApplyDateLabel.Location = New System.Drawing.Point(6, 37)
        Me.ApplyDateLabel.Name = "ApplyDateLabel"
        Me.ApplyDateLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ApplyDateLabel.Size = New System.Drawing.Size(77, 16)
        Me.ApplyDateLabel.TabIndex = 109
        Me.ApplyDateLabel.Text = "Apply Date :"
        Me.ApplyDateLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'AutoApplyCheckBox
        '
        Me.AutoApplyCheckBox.AutoSize = True
        Me.AutoApplyCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AutoApplyCheckBox.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold)
        Me.AutoApplyCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.AutoApplyCheckBox.Location = New System.Drawing.Point(9, 16)
        Me.AutoApplyCheckBox.Name = "AutoApplyCheckBox"
        Me.AutoApplyCheckBox.Size = New System.Drawing.Size(95, 18)
        Me.AutoApplyCheckBox.TabIndex = 108
        Me.AutoApplyCheckBox.Text = "Auto Apply : "
        Me.AutoApplyCheckBox.UseVisualStyleBackColor = True
        '
        'frmPricingBatchDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(782, 547)
        Me.Controls.Add(Me.HeaderFrame)
        Me.Controls.Add(Me.ugrdList)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.fraMaintain)
        Me.Controls.Add(Me.fraProcess)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.cmdExit)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPricingBatchDetail"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Pricing Batch Detail"
        Me.Frame1.ResumeLayout(False)
        Me.fraMaintain.ResumeLayout(False)
        Me.fraProcess.ResumeLayout(False)
        Me.fraProcess.PerformLayout()
        Me.StatusBar1.ResumeLayout(False)
        Me.StatusBar1.PerformLayout()
        CType(Me.cmdMaintain, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optPrintSign, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.HeaderFrame.ResumeLayout(False)
        Me.HeaderFrame.PerformLayout()
        CType(Me.AutoApplyDateUDTE, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Protected Overrides Sub Finalize()
        cdbFileOpen.Dispose()
        MyBase.Finalize()
    End Sub
    Friend WithEvents ugrdList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents UltraDataSource1 As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Friend WithEvents HeaderFrame As System.Windows.Forms.GroupBox
    Public WithEvents BatchDescriptionTextBox As System.Windows.Forms.TextBox
    Public WithEvents BatchDescLabel As System.Windows.Forms.Label
    Friend WithEvents AutoApplyDateUDTE As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Public WithEvents ApplyDateLabel As System.Windows.Forms.Label
    Friend WithEvents AutoApplyCheckBox As System.Windows.Forms.CheckBox
    Public WithEvents cmdSubmit As System.Windows.Forms.Button
    Public WithEvents cmdMarkAsPrinted As System.Windows.Forms.Button
    Friend WithEvents PosBatchIdTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PosBatchIdLabel As System.Windows.Forms.Label
    Friend WithEvents chkIgnoreNoTagLogic As CheckBox
End Class
