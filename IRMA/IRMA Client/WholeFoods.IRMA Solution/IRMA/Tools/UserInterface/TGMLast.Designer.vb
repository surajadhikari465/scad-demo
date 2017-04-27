<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmTGMLast
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
	Public cdbFileOpen As System.Windows.Forms.OpenFileDialog
	Public cdbFileSave As System.Windows.Forms.SaveFileDialog
	Public WithEvents cmdRefresh As System.Windows.Forms.Button
	Public WithEvents cmbCategory As System.Windows.Forms.ComboBox
	Public WithEvents _txtField_0 As System.Windows.Forms.TextBox
	Public WithEvents _txtField_1 As System.Windows.Forms.TextBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents _lblActual_2 As System.Windows.Forms.Label
	Public WithEvents _lblActual_1 As System.Windows.Forms.Label
	Public WithEvents _lblActual_0 As System.Windows.Forms.Label
	Public WithEvents _lblLine_2 As System.Windows.Forms.Label
	Public WithEvents _lblLine_0 As System.Windows.Forms.Label
	Public WithEvents _lblAverage_2 As System.Windows.Forms.Label
	Public WithEvents _lblAverage_1 As System.Windows.Forms.Label
	Public WithEvents _lblAverage_0 As System.Windows.Forms.Label
	Public WithEvents _lblLine_1 As System.Windows.Forms.Label
	Public WithEvents lblTarget As System.Windows.Forms.Label
	Public WithEvents lblMarginCaption As System.Windows.Forms.Label
	Public WithEvents _lblNew_2 As System.Windows.Forms.Label
	Public WithEvents _lblNew_1 As System.Windows.Forms.Label
	Public WithEvents _lblCurrent_2 As System.Windows.Forms.Label
	Public WithEvents _lblCurrent_1 As System.Windows.Forms.Label
    Public WithEvents _lblLine_4 As System.Windows.Forms.Label
	Public WithEvents _lblLine_3 As System.Windows.Forms.Label
	Public WithEvents _lblTitle_1 As System.Windows.Forms.Label
	Public WithEvents _lblTitle_0 As System.Windows.Forms.Label
	Public WithEvents _lblTitle_2 As System.Windows.Forms.Label
	Public WithEvents _lblNew_0 As System.Windows.Forms.Label
	Public WithEvents _lblCurrent_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents lblActual As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lblAverage As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lblCurrent As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lblLine As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lblNew As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lblTitle As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents mnuMultipleStoreList As Microsoft.VisualBasic.Compatibility.VB6.ToolStripMenuItemArray
	Public WithEvents mnuSalesMixList As Microsoft.VisualBasic.Compatibility.VB6.ToolStripMenuItemArray
	Public WithEvents mnuSortBy As Microsoft.VisualBasic.Compatibility.VB6.ToolStripMenuItemArray
	Public WithEvents mnuStoreList As Microsoft.VisualBasic.Compatibility.VB6.ToolStripMenuItemArray
	Public WithEvents mnuViews As Microsoft.VisualBasic.Compatibility.VB6.ToolStripMenuItemArray
	Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	Public WithEvents mnuSave As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSaveAs As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuCommitPrices As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuItemInformation As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuItemHistory As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuInfo As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_0 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_1 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_2 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_3 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_4 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_5 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_6 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_7 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_8 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_9 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_10 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_11 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_12 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_13 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_14 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_15 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuViews_16 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuView As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuSalesMixList_0 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuSalesMixList_1 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuSalesMixList_2 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSalesMix As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuSortBy_0 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuSortBy_1 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents _mnuSortBy_2 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSort As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuCalculator As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuUtilities As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuExportView As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuPriceChanges As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuPrintView As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuPrintRows As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuReports As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTGMLast))
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Description")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Wt")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("New Price")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("5")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("6")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("7")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("8", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, False)
        Dim ColScrollRegion1 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(732)
        Dim ColScrollRegion2 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(-413)
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraDataColumn1 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("ID")
        Dim UltraDataColumn2 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Identifier")
        Dim UltraDataColumn3 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Description")
        Dim UltraDataColumn4 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Wt")
        Dim UltraDataColumn5 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("New Price")
        Dim UltraDataColumn6 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("5")
        Dim UltraDataColumn7 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("6")
        Dim UltraDataColumn8 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("7")
        Dim UltraDataColumn9 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("8")
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraDataColumn10 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("ID")
        Dim UltraDataColumn11 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Identifier")
        Dim UltraDataColumn12 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Description")
        Dim UltraDataColumn13 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Wt")
        Dim UltraDataColumn14 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("New Price")
        Dim UltraDataColumn15 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("5")
        Dim UltraDataColumn16 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("6")
        Dim UltraDataColumn17 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("7")
        Dim UltraDataColumn18 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("8")
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdRefresh = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmbMultiStores = New System.Windows.Forms.ComboBox
        Me.cdbFileOpen = New System.Windows.Forms.OpenFileDialog
        Me.cdbFileSave = New System.Windows.Forms.SaveFileDialog
        Me.cmbCategory = New System.Windows.Forms.ComboBox
        Me._txtField_0 = New System.Windows.Forms.TextBox
        Me._txtField_1 = New System.Windows.Forms.TextBox
        Me._lblActual_2 = New System.Windows.Forms.Label
        Me._lblActual_1 = New System.Windows.Forms.Label
        Me._lblActual_0 = New System.Windows.Forms.Label
        Me._lblLine_2 = New System.Windows.Forms.Label
        Me._lblLine_0 = New System.Windows.Forms.Label
        Me._lblAverage_2 = New System.Windows.Forms.Label
        Me._lblAverage_1 = New System.Windows.Forms.Label
        Me._lblAverage_0 = New System.Windows.Forms.Label
        Me._lblLine_1 = New System.Windows.Forms.Label
        Me.lblTarget = New System.Windows.Forms.Label
        Me.lblMarginCaption = New System.Windows.Forms.Label
        Me._lblNew_2 = New System.Windows.Forms.Label
        Me._lblNew_1 = New System.Windows.Forms.Label
        Me._lblCurrent_2 = New System.Windows.Forms.Label
        Me._lblCurrent_1 = New System.Windows.Forms.Label
        Me._lblLine_4 = New System.Windows.Forms.Label
        Me._lblLine_3 = New System.Windows.Forms.Label
        Me._lblTitle_1 = New System.Windows.Forms.Label
        Me._lblTitle_0 = New System.Windows.Forms.Label
        Me._lblTitle_2 = New System.Windows.Forms.Label
        Me._lblNew_0 = New System.Windows.Forms.Label
        Me._lblCurrent_0 = New System.Windows.Forms.Label
        Me._lblLabel_2 = New System.Windows.Forms.Label
        Me._lblLabel_5 = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me.lblActual = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblAverage = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblCurrent = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblLine = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblNew = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblTitle = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.mnuMultipleStoreList = New Microsoft.VisualBasic.Compatibility.VB6.ToolStripMenuItemArray(Me.components)
        Me.mnuSalesMixList = New Microsoft.VisualBasic.Compatibility.VB6.ToolStripMenuItemArray(Me.components)
        Me._mnuSalesMixList_0 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuSalesMixList_1 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuSalesMixList_2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSortBy = New Microsoft.VisualBasic.Compatibility.VB6.ToolStripMenuItemArray(Me.components)
        Me._mnuSortBy_0 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuSortBy_1 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuSortBy_2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuStoreList = New Microsoft.VisualBasic.Compatibility.VB6.ToolStripMenuItemArray(Me.components)
        Me.mnuViews = New Microsoft.VisualBasic.Compatibility.VB6.ToolStripMenuItemArray(Me.components)
        Me._mnuViews_0 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_1 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_2 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_3 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_4 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_5 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_6 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_7 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_8 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_9 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_10 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_11 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_12 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_13 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_14 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_15 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuViews_16 = New System.Windows.Forms.ToolStripMenuItem
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSave = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSaveAs = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuCommitPrices = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuInfo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuItemInformation = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuItemHistory = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuView = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSalesMix = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSort = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuUtilities = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuCalculator = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReports = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExportView = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPriceChanges = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPrintView = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPrintRows = New System.Windows.Forms.ToolStripMenuItem
        Me.UltraGrid1 = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.UltraDataSource1 = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.UltraGridPrintDocument1 = New Infragistics.Win.UltraWinGrid.UltraGridPrintDocument(Me.components)
        Me.UltraPrintPreviewDialog1 = New Infragistics.Win.Printing.UltraPrintPreviewDialog(Me.components)
        Me.cmbStore = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.UltraDataSource2 = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        CType(Me.lblActual, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblAverage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblCurrent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLine, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblNew, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTitle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.mnuMultipleStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.mnuSalesMixList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.mnuSortBy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.mnuStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.mnuViews, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainMenu1.SuspendLayout()
        CType(Me.UltraGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDataSource2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdRefresh
        '
        Me.cmdRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRefresh.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRefresh.Image = CType(resources.GetObject("cmdRefresh.Image"), System.Drawing.Image)
        Me.cmdRefresh.Location = New System.Drawing.Point(653, 447)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRefresh.Size = New System.Drawing.Size(41, 43)
        Me.cmdRefresh.TabIndex = 6
        Me.cmdRefresh.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdRefresh, "Refresh Grid")
        Me.cmdRefresh.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(701, 447)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 7
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmbMultiStores
        '
        Me.cmbMultiStores.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbMultiStores.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbMultiStores.BackColor = System.Drawing.SystemColors.Window
        Me.cmbMultiStores.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbMultiStores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMultiStores.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMultiStores.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbMultiStores.Items.AddRange(New Object() {"Corporate", "HFM-GA", "WFM-GA", "WFM-NC", "WFM Stores"})
        Me.cmbMultiStores.Location = New System.Drawing.Point(565, 32)
        Me.cmbMultiStores.Name = "cmbMultiStores"
        Me.cmbMultiStores.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbMultiStores.Size = New System.Drawing.Size(177, 22)
        Me.cmbMultiStores.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.cmbMultiStores, "List of Multi Stores")
        '
        'cmbCategory
        '
        Me.cmbCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbCategory.BackColor = System.Drawing.SystemColors.Window
        Me.cmbCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCategory.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCategory.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbCategory.Location = New System.Drawing.Point(87, 77)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbCategory.Size = New System.Drawing.Size(145, 22)
        Me.cmbCategory.Sorted = True
        Me.cmbCategory.TabIndex = 2
        '
        '_txtField_0
        '
        Me._txtField_0.AcceptsReturn = True
        Me._txtField_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_0, CType(0, Short))
        Me._txtField_0.Location = New System.Drawing.Point(87, 51)
        Me._txtField_0.MaxLength = 60
        Me._txtField_0.Name = "_txtField_0"
        Me._txtField_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_0.Size = New System.Drawing.Size(265, 20)
        Me._txtField_0.TabIndex = 1
        Me._txtField_0.Tag = "String"
        '
        '_txtField_1
        '
        Me._txtField_1.AcceptsReturn = True
        Me._txtField_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_1, CType(1, Short))
        Me._txtField_1.Location = New System.Drawing.Point(87, 27)
        Me._txtField_1.MaxLength = 18
        Me._txtField_1.Name = "_txtField_1"
        Me._txtField_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_1.Size = New System.Drawing.Size(145, 20)
        Me._txtField_1.TabIndex = 0
        Me._txtField_1.Tag = "String"
        '
        '_lblActual_2
        '
        Me._lblActual_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblActual_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblActual_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblActual_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblActual_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblActual.SetIndex(Me._lblActual_2, CType(2, Short))
        Me._lblActual_2.Location = New System.Drawing.Point(565, 423)
        Me._lblActual_2.Name = "_lblActual_2"
        Me._lblActual_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblActual_2.Size = New System.Drawing.Size(65, 17)
        Me._lblActual_2.TabIndex = 29
        Me._lblActual_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblActual_1
        '
        Me._lblActual_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblActual_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblActual_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblActual_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblActual_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblActual.SetIndex(Me._lblActual_1, CType(1, Short))
        Me._lblActual_1.Location = New System.Drawing.Point(493, 423)
        Me._lblActual_1.Name = "_lblActual_1"
        Me._lblActual_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblActual_1.Size = New System.Drawing.Size(73, 17)
        Me._lblActual_1.TabIndex = 30
        Me._lblActual_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblActual_0
        '
        Me._lblActual_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblActual_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblActual_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblActual_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblActual_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblActual.SetIndex(Me._lblActual_0, CType(0, Short))
        Me._lblActual_0.Location = New System.Drawing.Point(421, 423)
        Me._lblActual_0.Name = "_lblActual_0"
        Me._lblActual_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblActual_0.Size = New System.Drawing.Size(73, 17)
        Me._lblActual_0.TabIndex = 31
        Me._lblActual_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblLine_2
        '
        Me._lblLine_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLine_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLine_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLine_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLine.SetIndex(Me._lblLine_2, CType(2, Short))
        Me._lblLine_2.Location = New System.Drawing.Point(349, 423)
        Me._lblLine_2.Name = "_lblLine_2"
        Me._lblLine_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLine_2.Size = New System.Drawing.Size(65, 17)
        Me._lblLine_2.TabIndex = 28
        Me._lblLine_2.Text = "Actual :"
        Me._lblLine_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLine_0
        '
        Me._lblLine_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLine_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLine_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLine_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLine.SetIndex(Me._lblLine_0, CType(0, Short))
        Me._lblLine_0.Location = New System.Drawing.Point(293, 383)
        Me._lblLine_0.Name = "_lblLine_0"
        Me._lblLine_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLine_0.Size = New System.Drawing.Size(65, 17)
        Me._lblLine_0.TabIndex = 27
        Me._lblLine_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblAverage_2
        '
        Me._lblAverage_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblAverage_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblAverage_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblAverage_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblAverage_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAverage.SetIndex(Me._lblAverage_2, CType(2, Short))
        Me._lblAverage_2.Location = New System.Drawing.Point(565, 399)
        Me._lblAverage_2.Name = "_lblAverage_2"
        Me._lblAverage_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblAverage_2.Size = New System.Drawing.Size(65, 17)
        Me._lblAverage_2.TabIndex = 26
        Me._lblAverage_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblAverage_1
        '
        Me._lblAverage_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblAverage_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblAverage_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblAverage_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblAverage_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAverage.SetIndex(Me._lblAverage_1, CType(1, Short))
        Me._lblAverage_1.Location = New System.Drawing.Point(493, 399)
        Me._lblAverage_1.Name = "_lblAverage_1"
        Me._lblAverage_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblAverage_1.Size = New System.Drawing.Size(73, 17)
        Me._lblAverage_1.TabIndex = 25
        Me._lblAverage_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblAverage_0
        '
        Me._lblAverage_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblAverage_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblAverage_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblAverage_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblAverage_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAverage.SetIndex(Me._lblAverage_0, CType(0, Short))
        Me._lblAverage_0.Location = New System.Drawing.Point(421, 399)
        Me._lblAverage_0.Name = "_lblAverage_0"
        Me._lblAverage_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblAverage_0.Size = New System.Drawing.Size(73, 17)
        Me._lblAverage_0.TabIndex = 24
        Me._lblAverage_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblLine_1
        '
        Me._lblLine_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLine_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLine_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLine_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLine.SetIndex(Me._lblLine_1, CType(1, Short))
        Me._lblLine_1.Location = New System.Drawing.Point(349, 399)
        Me._lblLine_1.Name = "_lblLine_1"
        Me._lblLine_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLine_1.Size = New System.Drawing.Size(65, 17)
        Me._lblLine_1.TabIndex = 23
        Me._lblLine_1.Text = "Average :"
        Me._lblLine_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblTarget
        '
        Me.lblTarget.BackColor = System.Drawing.SystemColors.Control
        Me.lblTarget.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTarget.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTarget.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTarget.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTarget.Location = New System.Drawing.Point(109, 399)
        Me.lblTarget.Name = "lblTarget"
        Me.lblTarget.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTarget.Size = New System.Drawing.Size(73, 17)
        Me.lblTarget.TabIndex = 22
        Me.lblTarget.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblMarginCaption
        '
        Me.lblMarginCaption.BackColor = System.Drawing.Color.Transparent
        Me.lblMarginCaption.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMarginCaption.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMarginCaption.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMarginCaption.Location = New System.Drawing.Point(13, 399)
        Me.lblMarginCaption.Name = "lblMarginCaption"
        Me.lblMarginCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMarginCaption.Size = New System.Drawing.Size(89, 17)
        Me.lblMarginCaption.TabIndex = 21
        Me.lblMarginCaption.Text = "Target Margin :"
        Me.lblMarginCaption.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblNew_2
        '
        Me._lblNew_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblNew_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblNew_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblNew_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblNew_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNew.SetIndex(Me._lblNew_2, CType(2, Short))
        Me._lblNew_2.Location = New System.Drawing.Point(565, 471)
        Me._lblNew_2.Name = "_lblNew_2"
        Me._lblNew_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblNew_2.Size = New System.Drawing.Size(65, 17)
        Me._lblNew_2.TabIndex = 20
        Me._lblNew_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblNew_1
        '
        Me._lblNew_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblNew_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblNew_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblNew_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblNew_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNew.SetIndex(Me._lblNew_1, CType(1, Short))
        Me._lblNew_1.Location = New System.Drawing.Point(493, 471)
        Me._lblNew_1.Name = "_lblNew_1"
        Me._lblNew_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblNew_1.Size = New System.Drawing.Size(73, 17)
        Me._lblNew_1.TabIndex = 19
        Me._lblNew_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblCurrent_2
        '
        Me._lblCurrent_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblCurrent_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblCurrent_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCurrent_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCurrent_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrent.SetIndex(Me._lblCurrent_2, CType(2, Short))
        Me._lblCurrent_2.Location = New System.Drawing.Point(565, 447)
        Me._lblCurrent_2.Name = "_lblCurrent_2"
        Me._lblCurrent_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCurrent_2.Size = New System.Drawing.Size(65, 17)
        Me._lblCurrent_2.TabIndex = 18
        Me._lblCurrent_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblCurrent_1
        '
        Me._lblCurrent_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblCurrent_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblCurrent_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCurrent_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCurrent_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrent.SetIndex(Me._lblCurrent_1, CType(1, Short))
        Me._lblCurrent_1.Location = New System.Drawing.Point(493, 447)
        Me._lblCurrent_1.Name = "_lblCurrent_1"
        Me._lblCurrent_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCurrent_1.Size = New System.Drawing.Size(73, 17)
        Me._lblCurrent_1.TabIndex = 17
        Me._lblCurrent_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblLine_4
        '
        Me._lblLine_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLine_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLine_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLine_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLine.SetIndex(Me._lblLine_4, CType(4, Short))
        Me._lblLine_4.Location = New System.Drawing.Point(349, 471)
        Me._lblLine_4.Name = "_lblLine_4"
        Me._lblLine_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLine_4.Size = New System.Drawing.Size(65, 17)
        Me._lblLine_4.TabIndex = 14
        Me._lblLine_4.Text = "New :"
        Me._lblLine_4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLine_3
        '
        Me._lblLine_3.BackColor = System.Drawing.Color.Transparent
        Me._lblLine_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLine_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLine_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLine.SetIndex(Me._lblLine_3, CType(3, Short))
        Me._lblLine_3.Location = New System.Drawing.Point(349, 447)
        Me._lblLine_3.Name = "_lblLine_3"
        Me._lblLine_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLine_3.Size = New System.Drawing.Size(65, 17)
        Me._lblLine_3.TabIndex = 13
        Me._lblLine_3.Text = "Current :"
        Me._lblLine_3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblTitle_1
        '
        Me._lblTitle_1.BackColor = System.Drawing.Color.Transparent
        Me._lblTitle_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTitle_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTitle_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTitle.SetIndex(Me._lblTitle_1, CType(1, Short))
        Me._lblTitle_1.Location = New System.Drawing.Point(493, 383)
        Me._lblTitle_1.Name = "_lblTitle_1"
        Me._lblTitle_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTitle_1.Size = New System.Drawing.Size(73, 17)
        Me._lblTitle_1.TabIndex = 12
        Me._lblTitle_1.Text = "Cost $"
        Me._lblTitle_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblTitle_0
        '
        Me._lblTitle_0.BackColor = System.Drawing.Color.Transparent
        Me._lblTitle_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTitle_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTitle_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTitle.SetIndex(Me._lblTitle_0, CType(0, Short))
        Me._lblTitle_0.Location = New System.Drawing.Point(421, 383)
        Me._lblTitle_0.Name = "_lblTitle_0"
        Me._lblTitle_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTitle_0.Size = New System.Drawing.Size(73, 17)
        Me._lblTitle_0.TabIndex = 11
        Me._lblTitle_0.Text = "Retail $"
        Me._lblTitle_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblTitle_2
        '
        Me._lblTitle_2.BackColor = System.Drawing.Color.Transparent
        Me._lblTitle_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTitle_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTitle_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTitle.SetIndex(Me._lblTitle_2, CType(2, Short))
        Me._lblTitle_2.Location = New System.Drawing.Point(565, 383)
        Me._lblTitle_2.Name = "_lblTitle_2"
        Me._lblTitle_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTitle_2.Size = New System.Drawing.Size(65, 17)
        Me._lblTitle_2.TabIndex = 10
        Me._lblTitle_2.Text = "% TGM"
        Me._lblTitle_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblNew_0
        '
        Me._lblNew_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblNew_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblNew_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblNew_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblNew_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNew.SetIndex(Me._lblNew_0, CType(0, Short))
        Me._lblNew_0.Location = New System.Drawing.Point(421, 471)
        Me._lblNew_0.Name = "_lblNew_0"
        Me._lblNew_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblNew_0.Size = New System.Drawing.Size(73, 17)
        Me._lblNew_0.TabIndex = 9
        Me._lblNew_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblCurrent_0
        '
        Me._lblCurrent_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblCurrent_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._lblCurrent_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCurrent_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCurrent_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrent.SetIndex(Me._lblCurrent_0, CType(0, Short))
        Me._lblCurrent_0.Location = New System.Drawing.Point(421, 447)
        Me._lblCurrent_0.Name = "_lblCurrent_0"
        Me._lblCurrent_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCurrent_0.Size = New System.Drawing.Size(73, 17)
        Me._lblCurrent_0.TabIndex = 8
        Me._lblCurrent_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(23, 51)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(57, 17)
        Me._lblLabel_2.TabIndex = 5
        Me._lblLabel_2.Text = "Desc :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Location = New System.Drawing.Point(16, 77)
        Me._lblLabel_5.Name = "_lblLabel_5"
        Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_5.Size = New System.Drawing.Size(65, 22)
        Me._lblLabel_5.TabIndex = 2
        Me._lblLabel_5.Text = "Class :"
        Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(16, 27)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(65, 17)
        Me._lblLabel_1.TabIndex = 0
        Me._lblLabel_1.Text = "Identifier :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'mnuSalesMixList
        '
        '
        '_mnuSalesMixList_0
        '
        Me._mnuSalesMixList_0.Checked = True
        Me._mnuSalesMixList_0.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuSalesMixList.SetIndex(Me._mnuSalesMixList_0, CType(0, Short))
        Me._mnuSalesMixList_0.Name = "_mnuSalesMixList_0"
        Me._mnuSalesMixList_0.Size = New System.Drawing.Size(189, 22)
        Me._mnuSalesMixList_0.Text = "Mix By &Current View"
        '
        '_mnuSalesMixList_1
        '
        Me.mnuSalesMixList.SetIndex(Me._mnuSalesMixList_1, CType(1, Short))
        Me._mnuSalesMixList_1.Name = "_mnuSalesMixList_1"
        Me._mnuSalesMixList_1.Size = New System.Drawing.Size(189, 22)
        Me._mnuSalesMixList_1.Text = "Mix By Complete &View"
        '
        '_mnuSalesMixList_2
        '
        Me._mnuSalesMixList_2.Enabled = False
        Me.mnuSalesMixList.SetIndex(Me._mnuSalesMixList_2, CType(2, Short))
        Me._mnuSalesMixList_2.Name = "_mnuSalesMixList_2"
        Me._mnuSalesMixList_2.Size = New System.Drawing.Size(189, 22)
        Me._mnuSalesMixList_2.Text = "Mix By &Acct Dept"
        Me._mnuSalesMixList_2.Visible = False
        '
        'mnuSortBy
        '
        '
        '_mnuSortBy_0
        '
        Me._mnuSortBy_0.Checked = True
        Me._mnuSortBy_0.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuSortBy.SetIndex(Me._mnuSortBy_0, CType(0, Short))
        Me._mnuSortBy_0.Name = "_mnuSortBy_0"
        Me._mnuSortBy_0.Size = New System.Drawing.Size(167, 22)
        Me._mnuSortBy_0.Text = "Sort by &ID"
        '
        '_mnuSortBy_1
        '
        Me.mnuSortBy.SetIndex(Me._mnuSortBy_1, CType(1, Short))
        Me._mnuSortBy_1.Name = "_mnuSortBy_1"
        Me._mnuSortBy_1.Size = New System.Drawing.Size(167, 22)
        Me._mnuSortBy_1.Text = "Sort by &TGM"
        '
        '_mnuSortBy_2
        '
        Me.mnuSortBy.SetIndex(Me._mnuSortBy_2, CType(2, Short))
        Me._mnuSortBy_2.Name = "_mnuSortBy_2"
        Me._mnuSortBy_2.Size = New System.Drawing.Size(167, 22)
        Me._mnuSortBy_2.Text = "Sort By Sales &Mix"
        '
        'mnuViews
        '
        '
        '_mnuViews_0
        '
        Me.mnuViews.SetIndex(Me._mnuViews_0, CType(0, Short))
        Me._mnuViews_0.Name = "_mnuViews_0"
        Me._mnuViews_0.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_0.Text = "&Average TGM"
        '
        '_mnuViews_1
        '
        Me.mnuViews.SetIndex(Me._mnuViews_1, CType(1, Short))
        Me._mnuViews_1.Name = "_mnuViews_1"
        Me._mnuViews_1.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_1.Text = "Average TGM 2"
        '
        '_mnuViews_2
        '
        Me.mnuViews.SetIndex(Me._mnuViews_2, CType(2, Short))
        Me._mnuViews_2.Name = "_mnuViews_2"
        Me._mnuViews_2.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_2.Text = "A&verage Sales Mix"
        '
        '_mnuViews_3
        '
        Me.mnuViews.SetIndex(Me._mnuViews_3, CType(3, Short))
        Me._mnuViews_3.Name = "_mnuViews_3"
        Me._mnuViews_3.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_3.Text = "Avera&ge TGM/Mix"
        '
        '_mnuViews_4
        '
        Me.mnuViews.SetIndex(Me._mnuViews_4, CType(4, Short))
        Me._mnuViews_4.Name = "_mnuViews_4"
        Me._mnuViews_4.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_4.Text = "Act&ual TGM"
        '
        '_mnuViews_5
        '
        Me.mnuViews.SetIndex(Me._mnuViews_5, CType(5, Short))
        Me._mnuViews_5.Name = "_mnuViews_5"
        Me._mnuViews_5.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_5.Text = "Actual TGM 2"
        '
        '_mnuViews_6
        '
        Me.mnuViews.SetIndex(Me._mnuViews_6, CType(6, Short))
        Me._mnuViews_6.Name = "_mnuViews_6"
        Me._mnuViews_6.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_6.Text = "Actual Sales Mix"
        '
        '_mnuViews_7
        '
        Me.mnuViews.SetIndex(Me._mnuViews_7, CType(7, Short))
        Me._mnuViews_7.Name = "_mnuViews_7"
        Me._mnuViews_7.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_7.Text = "Actual TGM/Mix"
        '
        '_mnuViews_8
        '
        Me.mnuViews.SetIndex(Me._mnuViews_8, CType(8, Short))
        Me._mnuViews_8.Name = "_mnuViews_8"
        Me._mnuViews_8.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_8.Text = "&Current TGM"
        '
        '_mnuViews_9
        '
        Me.mnuViews.SetIndex(Me._mnuViews_9, CType(9, Short))
        Me._mnuViews_9.Name = "_mnuViews_9"
        Me._mnuViews_9.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_9.Text = "Current TGM 2"
        '
        '_mnuViews_10
        '
        Me.mnuViews.SetIndex(Me._mnuViews_10, CType(10, Short))
        Me._mnuViews_10.Name = "_mnuViews_10"
        Me._mnuViews_10.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_10.Text = "C&urrent Sales Mix"
        '
        '_mnuViews_11
        '
        Me.mnuViews.SetIndex(Me._mnuViews_11, CType(11, Short))
        Me._mnuViews_11.Name = "_mnuViews_11"
        Me._mnuViews_11.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_11.Text = "Cu&rrent TGM/Mix"
        '
        '_mnuViews_12
        '
        Me.mnuViews.SetIndex(Me._mnuViews_12, CType(12, Short))
        Me._mnuViews_12.Name = "_mnuViews_12"
        Me._mnuViews_12.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_12.Text = "&New TGM"
        '
        '_mnuViews_13
        '
        Me.mnuViews.SetIndex(Me._mnuViews_13, CType(13, Short))
        Me._mnuViews_13.Name = "_mnuViews_13"
        Me._mnuViews_13.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_13.Text = "New TGM 2"
        '
        '_mnuViews_14
        '
        Me.mnuViews.SetIndex(Me._mnuViews_14, CType(14, Short))
        Me._mnuViews_14.Name = "_mnuViews_14"
        Me._mnuViews_14.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_14.Text = "N&ew Sales Mix"
        '
        '_mnuViews_15
        '
        Me.mnuViews.SetIndex(Me._mnuViews_15, CType(15, Short))
        Me._mnuViews_15.Name = "_mnuViews_15"
        Me._mnuViews_15.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_15.Text = "Ne&w TGM/Mix"
        '
        '_mnuViews_16
        '
        Me._mnuViews_16.Checked = True
        Me._mnuViews_16.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuViews.SetIndex(Me._mnuViews_16, CType(16, Short))
        Me._mnuViews_16.Name = "_mnuViews_16"
        Me._mnuViews_16.Size = New System.Drawing.Size(173, 22)
        Me._mnuViews_16.Text = "&Information"
        '
        'txtField
        '
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuInfo, Me.mnuView, Me.mnuSalesMix, Me.mnuSort, Me.mnuUtilities, Me.mnuReports})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(754, 24)
        Me.MainMenu1.TabIndex = 8
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSave, Me.mnuSaveAs, Me.mnuCommitPrices, Me.mnuExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(35, 20)
        Me.mnuFile.Text = "&File"
        '
        'mnuSave
        '
        Me.mnuSave.Name = "mnuSave"
        Me.mnuSave.Size = New System.Drawing.Size(151, 22)
        Me.mnuSave.Text = "&Save"
        '
        'mnuSaveAs
        '
        Me.mnuSaveAs.Name = "mnuSaveAs"
        Me.mnuSaveAs.Size = New System.Drawing.Size(151, 22)
        Me.mnuSaveAs.Text = "Save &As"
        '
        'mnuCommitPrices
        '
        Me.mnuCommitPrices.Name = "mnuCommitPrices"
        Me.mnuCommitPrices.Size = New System.Drawing.Size(151, 22)
        Me.mnuCommitPrices.Text = "&Commit Prices"
        Me.mnuCommitPrices.Visible = False
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(151, 22)
        Me.mnuExit.Text = "E&xit"
        '
        'mnuInfo
        '
        Me.mnuInfo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuItemInformation, Me.mnuItemHistory})
        Me.mnuInfo.Name = "mnuInfo"
        Me.mnuInfo.Size = New System.Drawing.Size(75, 20)
        Me.mnuInfo.Text = "&Information"
        '
        'mnuItemInformation
        '
        Me.mnuItemInformation.Name = "mnuItemInformation"
        Me.mnuItemInformation.Size = New System.Drawing.Size(166, 22)
        Me.mnuItemInformation.Text = "I&tem Information"
        '
        'mnuItemHistory
        '
        Me.mnuItemHistory.Name = "mnuItemHistory"
        Me.mnuItemHistory.Size = New System.Drawing.Size(166, 22)
        Me.mnuItemHistory.Text = "Item &History"
        '
        'mnuView
        '
        Me.mnuView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._mnuViews_0, Me._mnuViews_1, Me._mnuViews_2, Me._mnuViews_3, Me._mnuViews_4, Me._mnuViews_5, Me._mnuViews_6, Me._mnuViews_7, Me._mnuViews_8, Me._mnuViews_9, Me._mnuViews_10, Me._mnuViews_11, Me._mnuViews_12, Me._mnuViews_13, Me._mnuViews_14, Me._mnuViews_15, Me._mnuViews_16})
        Me.mnuView.Name = "mnuView"
        Me.mnuView.Size = New System.Drawing.Size(41, 20)
        Me.mnuView.Text = "&View"
        '
        'mnuSalesMix
        '
        Me.mnuSalesMix.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._mnuSalesMixList_0, Me._mnuSalesMixList_1, Me._mnuSalesMixList_2})
        Me.mnuSalesMix.Name = "mnuSalesMix"
        Me.mnuSalesMix.Size = New System.Drawing.Size(63, 20)
        Me.mnuSalesMix.Text = "Sales &Mix"
        '
        'mnuSort
        '
        Me.mnuSort.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._mnuSortBy_0, Me._mnuSortBy_1, Me._mnuSortBy_2})
        Me.mnuSort.Name = "mnuSort"
        Me.mnuSort.Size = New System.Drawing.Size(39, 20)
        Me.mnuSort.Text = "S&ort"
        '
        'mnuUtilities
        '
        Me.mnuUtilities.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCalculator})
        Me.mnuUtilities.Name = "mnuUtilities"
        Me.mnuUtilities.Size = New System.Drawing.Size(53, 20)
        Me.mnuUtilities.Text = "&Utilities"
        '
        'mnuCalculator
        '
        Me.mnuCalculator.Name = "mnuCalculator"
        Me.mnuCalculator.Size = New System.Drawing.Size(133, 22)
        Me.mnuCalculator.Text = "&Calculator"
        '
        'mnuReports
        '
        Me.mnuReports.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuExportView, Me.mnuPriceChanges, Me.mnuPrintView, Me.mnuPrintRows})
        Me.mnuReports.Name = "mnuReports"
        Me.mnuReports.Size = New System.Drawing.Size(57, 20)
        Me.mnuReports.Text = "&Reports"
        '
        'mnuExportView
        '
        Me.mnuExportView.Name = "mnuExportView"
        Me.mnuExportView.Size = New System.Drawing.Size(157, 22)
        Me.mnuExportView.Text = "&Export View"
        '
        'mnuPriceChanges
        '
        Me.mnuPriceChanges.Enabled = False
        Me.mnuPriceChanges.Name = "mnuPriceChanges"
        Me.mnuPriceChanges.Size = New System.Drawing.Size(157, 22)
        Me.mnuPriceChanges.Text = "&Price Changes"
        '
        'mnuPrintView
        '
        Me.mnuPrintView.Name = "mnuPrintView"
        Me.mnuPrintView.Size = New System.Drawing.Size(157, 22)
        Me.mnuPrintView.Text = "Print &View"
        '
        'mnuPrintRows
        '
        Me.mnuPrintRows.Name = "mnuPrintRows"
        Me.mnuPrintRows.Size = New System.Drawing.Size(157, 22)
        Me.mnuPrintRows.Text = "Print &Top Rows"
        '
        'UltraGrid1
        '
        Me.UltraGrid1.DataSource = Me.UltraDataSource1
        Me.UltraGrid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 8
        UltraGridColumn1.Width = 15
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.Header.VisiblePosition = 0
        UltraGridColumn2.MaxWidth = 80
        UltraGridColumn2.MinWidth = 70
        UltraGridColumn2.Width = 70
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.MaxWidth = 300
        UltraGridColumn3.MinWidth = 100
        UltraGridColumn3.Width = 118
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.Header.VisiblePosition = 1
        UltraGridColumn4.MaxWidth = 30
        UltraGridColumn4.MinWidth = 30
        UltraGridColumn4.Width = 30
        UltraGridColumn5.Header.VisiblePosition = 3
        UltraGridColumn5.MaxWidth = 70
        UltraGridColumn5.MinWidth = 70
        UltraGridColumn5.Width = 70
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.Header.VisiblePosition = 4
        UltraGridColumn6.MaxWidth = 90
        UltraGridColumn6.MinWidth = 70
        UltraGridColumn6.Width = 86
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn7.Header.VisiblePosition = 5
        UltraGridColumn7.MaxWidth = 120
        UltraGridColumn7.MinWidth = 80
        UltraGridColumn7.Width = 88
        UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn8.Header.VisiblePosition = 6
        UltraGridColumn8.MaxWidth = 120
        UltraGridColumn8.MinWidth = 100
        UltraGridColumn8.Width = 110
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn9.Header.VisiblePosition = 7
        UltraGridColumn9.MaxWidth = 120
        UltraGridColumn9.MinWidth = 100
        UltraGridColumn9.Width = 110
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9})
        UltraGridBand1.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.[True]
        UltraGridBand1.Expandable = False
        UltraGridBand1.GroupHeadersVisible = False
        Me.UltraGrid1.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid1.DisplayLayout.ColScrollRegions.Add(ColScrollRegion1)
        Me.UltraGrid1.DisplayLayout.ColScrollRegions.Add(ColScrollRegion2)
        Me.UltraGrid1.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.[False]
        Appearance1.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance1.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance1.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid1.DisplayLayout.GroupByBox.Appearance = Appearance1
        Appearance2.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid1.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance2
        Me.UltraGrid1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid1.DisplayLayout.GroupByBox.Hidden = True
        Appearance3.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance3.BackColor2 = System.Drawing.SystemColors.Control
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid1.DisplayLayout.GroupByBox.PromptAppearance = Appearance3
        Me.UltraGrid1.DisplayLayout.MaxBandDepth = 1
        Me.UltraGrid1.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid1.DisplayLayout.MaxRowScrollRegions = 1
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid1.DisplayLayout.Override.ActiveRowAppearance = Appearance4
        Me.UltraGrid1.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.UltraGrid1.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
        Me.UltraGrid1.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed
        Me.UltraGrid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid1.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid1.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None
        Me.UltraGrid1.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        Me.UltraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid1.DisplayLayout.Override.CardAreaAppearance = Appearance5
        Appearance6.BorderColor = System.Drawing.Color.Silver
        Appearance6.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid1.DisplayLayout.Override.CellAppearance = Appearance6
        Me.UltraGrid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid1.DisplayLayout.Override.CellPadding = 0
        Appearance7.BackColor = System.Drawing.SystemColors.Control
        Appearance7.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance7.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance7.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid1.DisplayLayout.Override.GroupByRowAppearance = Appearance7
        Appearance8.TextHAlign = Infragistics.Win.HAlign.Left
        Me.UltraGrid1.DisplayLayout.Override.HeaderAppearance = Appearance8
        Me.UltraGrid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Me.UltraGrid1.DisplayLayout.Override.MaxSelectedCells = 1
        Me.UltraGrid1.DisplayLayout.Override.MaxSelectedRows = 1
        Appearance9.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid1.DisplayLayout.Override.RowAlternateAppearance = Appearance9
        Me.UltraGrid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid1.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance10.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid1.DisplayLayout.Override.TemplateAddRowAppearance = Appearance10
        Me.UltraGrid1.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.Vertical
        Me.UltraGrid1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid1.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.UltraGrid1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid1.Location = New System.Drawing.Point(8, 105)
        Me.UltraGrid1.Name = "UltraGrid1"
        Me.UltraGrid1.Size = New System.Drawing.Size(734, 275)
        Me.UltraGrid1.TabIndex = 5
        Me.UltraGrid1.Text = "TGM Tool List"
        '
        'UltraDataSource1
        '
        UltraDataColumn1.DataType = GetType(Integer)
        UltraDataColumn1.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn2.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn3.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn4.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn5.AllowDBNull = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn5.DataType = GetType(Decimal)
        UltraDataColumn5.ReadOnly = Infragistics.Win.DefaultableBoolean.[False]
        UltraDataColumn6.DataType = GetType(Decimal)
        UltraDataColumn6.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn7.DataType = GetType(Decimal)
        UltraDataColumn7.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn8.DataType = GetType(Decimal)
        UltraDataColumn8.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn9.DataType = GetType(Decimal)
        UltraDataColumn9.ReadOnly = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraDataSource1.Band.Columns.AddRange(New Object() {UltraDataColumn1, UltraDataColumn2, UltraDataColumn3, UltraDataColumn4, UltraDataColumn5, UltraDataColumn6, UltraDataColumn7, UltraDataColumn8, UltraDataColumn9})
        '
        'UltraGridPrintDocument1
        '
        Me.UltraGridPrintDocument1.DocumentName = "TGM"
        Me.UltraGridPrintDocument1.FitWidthToPages = 1
        Me.UltraGridPrintDocument1.Grid = Me.UltraGrid1
        Appearance11.FontData.BoldAsString = "True"
        Appearance11.FontData.SizeInPoints = 14.0!
        Appearance11.TextHAlign = Infragistics.Win.HAlign.Center
        Me.UltraGridPrintDocument1.Header.Appearance = Appearance11
        Me.UltraGridPrintDocument1.Header.TextCenter = ""
        Me.UltraGridPrintDocument1.Header.TextLeft = ""
        Me.UltraGridPrintDocument1.Header.TextRight = ""
        '
        'UltraPrintPreviewDialog1
        '
        Me.UltraPrintPreviewDialog1.Document = Me.UltraGridPrintDocument1
        Me.UltraPrintPreviewDialog1.Name = "UltraPrintPreviewDialog1"
        Me.UltraPrintPreviewDialog1.PreviewSettings.PageNumberDisplayStyle = Infragistics.Win.Printing.PageNumberDisplayStyle.LeftOfRow
        '
        'cmbStore
        '
        Me.cmbStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStore.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStore.Location = New System.Drawing.Point(565, 60)
        Me.cmbStore.Name = "cmbStore"
        Me.cmbStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStore.Size = New System.Drawing.Size(177, 22)
        Me.cmbStore.Sorted = True
        Me.cmbStore.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(490, 63)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(73, 19)
        Me.Label1.TabIndex = 34
        Me.Label1.Text = "Store :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(479, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(84, 20)
        Me.Label2.TabIndex = 36
        Me.Label2.Text = "Store(s) :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'UltraDataSource2
        '
        UltraDataColumn10.DataType = GetType(Integer)
        UltraDataColumn10.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn11.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn12.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn13.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn14.AllowDBNull = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn14.DataType = GetType(Decimal)
        UltraDataColumn14.ReadOnly = Infragistics.Win.DefaultableBoolean.[False]
        UltraDataColumn15.DataType = GetType(Decimal)
        UltraDataColumn15.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn16.DataType = GetType(Decimal)
        UltraDataColumn16.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn17.DataType = GetType(Decimal)
        UltraDataColumn17.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn18.DataType = GetType(Decimal)
        UltraDataColumn18.ReadOnly = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraDataSource2.Band.Columns.AddRange(New Object() {UltraDataColumn10, UltraDataColumn11, UltraDataColumn12, UltraDataColumn13, UltraDataColumn14, UltraDataColumn15, UltraDataColumn16, UltraDataColumn17, UltraDataColumn18})
        '
        'frmTGMLast
        '
        Me.AcceptButton = Me.cmdRefresh
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(754, 495)
        Me.Controls.Add(Me.cmbMultiStores)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbStore)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.UltraGrid1)
        Me.Controls.Add(Me.cmdRefresh)
        Me.Controls.Add(Me.cmbCategory)
        Me.Controls.Add(Me._txtField_0)
        Me.Controls.Add(Me._txtField_1)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me._lblActual_2)
        Me.Controls.Add(Me._lblActual_1)
        Me.Controls.Add(Me._lblActual_0)
        Me.Controls.Add(Me._lblLine_2)
        Me.Controls.Add(Me._lblLine_0)
        Me.Controls.Add(Me._lblAverage_2)
        Me.Controls.Add(Me._lblAverage_1)
        Me.Controls.Add(Me._lblAverage_0)
        Me.Controls.Add(Me._lblLine_1)
        Me.Controls.Add(Me.lblTarget)
        Me.Controls.Add(Me.lblMarginCaption)
        Me.Controls.Add(Me._lblNew_2)
        Me.Controls.Add(Me._lblNew_1)
        Me.Controls.Add(Me._lblCurrent_2)
        Me.Controls.Add(Me._lblCurrent_1)
        Me.Controls.Add(Me._lblLine_4)
        Me.Controls.Add(Me._lblLine_3)
        Me.Controls.Add(Me._lblTitle_1)
        Me.Controls.Add(Me._lblTitle_0)
        Me.Controls.Add(Me._lblTitle_2)
        Me.Controls.Add(Me._lblNew_0)
        Me.Controls.Add(Me._lblCurrent_0)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(57, 129)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTGMLast"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "TGM Tool"
        CType(Me.lblActual, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblAverage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblCurrent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLine, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblNew, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTitle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.mnuMultipleStoreList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.mnuSalesMixList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.mnuSortBy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.mnuStoreList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.mnuViews, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        CType(Me.UltraGrid1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDataSource2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UltraGrid1 As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents UltraDataSource1 As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Friend WithEvents UltraGridPrintDocument1 As Infragistics.Win.UltraWinGrid.UltraGridPrintDocument
    Friend WithEvents UltraPrintPreviewDialog1 As Infragistics.Win.Printing.UltraPrintPreviewDialog
    Public WithEvents cmbStore As System.Windows.Forms.ComboBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents cmbMultiStores As System.Windows.Forms.ComboBox
    Public WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents UltraDataSource2 As Infragistics.Win.UltraWinDataSource.UltraDataSource
#End Region
End Class