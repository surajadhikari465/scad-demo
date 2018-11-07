<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmItemPrice
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
	Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdStore As System.Windows.Forms.Button
    Public WithEvents cmdPrice As System.Windows.Forms.Button
    Public WithEvents cmdSearch As System.Windows.Forms.Button
    Public WithEvents cmdFunction As Microsoft.VisualBasic.Compatibility.VB6.ButtonArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItemPrice))
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store Name", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("POS Price")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Price")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Case")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sale Start")
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sale End")
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("POS Sale Price")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sale Price")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Tax")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Restricted")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StoreNo")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsRetailStore")
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Stop Sale")
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Line Discount")
        Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceChgTypeDesc")
        Dim UltraGridColumn16 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("AvgCost")
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn17 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("MarginPercentAvgCost")
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn18 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("RegUnitCost")
        Dim UltraGridColumn19 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("NetUnitCost")
        Dim UltraGridColumn20 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("MarginPercentCurrCost")
        Dim UltraGridColumn21 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Competitive Item", 0)
        Dim UltraGridColumn22 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sale_EDLP", 1)
        Dim UltraGridColumn23 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeam", 2)
        Dim UltraGridColumn24 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeam No", 3)
        Dim UltraGridColumn25 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Currency", 4)
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraDataColumn1 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Store Name")
        Dim UltraDataColumn2 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("POS Price")
        Dim UltraDataColumn3 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Price")
        Dim UltraDataColumn4 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Case")
        Dim UltraDataColumn5 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Sale Start")
        Dim UltraDataColumn6 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Sale End")
        Dim UltraDataColumn7 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("POS Sale Price")
        Dim UltraDataColumn8 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Sale Price")
        Dim UltraDataColumn9 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Tax")
        Dim UltraDataColumn10 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Restricted")
        Dim UltraDataColumn11 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("StoreNo")
        Dim UltraDataColumn12 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("IsRetailStore")
        Dim UltraDataColumn13 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Stop Sale")
        Dim UltraDataColumn14 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Line Discount")
        Dim UltraDataColumn15 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("PriceChgTypeDesc")
        Dim UltraDataColumn16 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("AvgCost")
        Dim UltraDataColumn17 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("MarginPercentAvgCost")
        Dim UltraDataColumn18 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("RegUnitCost")
        Dim UltraDataColumn19 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("NetUnitCost")
        Dim UltraDataColumn20 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("MarginPercentCurrCost")
        Dim UltraDataColumn21 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Store Name")
        Dim UltraDataColumn22 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("POS Price")
        Dim UltraDataColumn23 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Price")
        Dim UltraDataColumn24 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Case")
        Dim UltraDataColumn25 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Sale Start")
        Dim UltraDataColumn26 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Sale End")
        Dim UltraDataColumn27 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("POS Sale Price")
        Dim UltraDataColumn28 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Sale Price")
        Dim UltraDataColumn29 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Avg Cost")
        Dim UltraDataColumn30 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Margin")
        Dim UltraDataColumn31 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Tax")
        Dim UltraDataColumn32 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Restricted")
        Dim UltraDataColumn33 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("StoreNo")
        Dim UltraDataColumn34 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("IsRetailStore")
        Dim UltraDataColumn35 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Stop Sale")
        Dim UltraDataColumn36 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Line Discount")
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdStore = New System.Windows.Forms.Button()
        Me.cmdPrice = New System.Windows.Forms.Button()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.Button_MarginInfo = New System.Windows.Forms.Button()
        Me.Button_CancelAllSales = New System.Windows.Forms.Button()
        Me.Button_EndSaleEarly = New System.Windows.Forms.Button()
        Me.cmdFunction = New Microsoft.VisualBasic.Compatibility.VB6.ButtonArray(Me.components)
        Me.ugrdPriceList = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.UltraDataSource2 = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.UltraDataSource1 = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.Label_PromotionExistence = New System.Windows.Forms.Label()
        Me.Label_UseAverageCostforCostandMargin = New System.Windows.Forms.Label()
        CType(Me.cmdFunction, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdPriceList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDataSource2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        'cmdStore
        '
        resources.ApplyResources(Me.cmdStore, "cmdStore")
        Me.cmdStore.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFunction.SetIndex(Me.cmdStore, CType(1, Short))
        Me.cmdStore.Name = "cmdStore"
        Me.ToolTip1.SetToolTip(Me.cmdStore, resources.GetString("cmdStore.ToolTip"))
        Me.cmdStore.UseVisualStyleBackColor = False
        '
        'cmdPrice
        '
        resources.ApplyResources(Me.cmdPrice, "cmdPrice")
        Me.cmdPrice.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrice.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFunction.SetIndex(Me.cmdPrice, CType(0, Short))
        Me.cmdPrice.Name = "cmdPrice"
        Me.ToolTip1.SetToolTip(Me.cmdPrice, resources.GetString("cmdPrice.ToolTip"))
        Me.cmdPrice.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        resources.ApplyResources(Me.cmdSearch, "cmdSearch")
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Name = "cmdSearch"
        Me.ToolTip1.SetToolTip(Me.cmdSearch, resources.GetString("cmdSearch.ToolTip"))
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'Button_MarginInfo
        '
        resources.ApplyResources(Me.Button_MarginInfo, "Button_MarginInfo")
        Me.Button_MarginInfo.BackColor = System.Drawing.SystemColors.Control
        Me.Button_MarginInfo.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_MarginInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_MarginInfo.Name = "Button_MarginInfo"
        Me.ToolTip1.SetToolTip(Me.Button_MarginInfo, resources.GetString("Button_MarginInfo.ToolTip"))
        Me.Button_MarginInfo.UseVisualStyleBackColor = False
        '
        'Button_CancelAllSales
        '
        resources.ApplyResources(Me.Button_CancelAllSales, "Button_CancelAllSales")
        Me.Button_CancelAllSales.BackColor = System.Drawing.SystemColors.Control
        Me.Button_CancelAllSales.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_CancelAllSales.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_CancelAllSales.Name = "Button_CancelAllSales"
        Me.ToolTip1.SetToolTip(Me.Button_CancelAllSales, resources.GetString("Button_CancelAllSales.ToolTip"))
        Me.Button_CancelAllSales.UseVisualStyleBackColor = False
        '
        'Button_EndSaleEarly
        '
        resources.ApplyResources(Me.Button_EndSaleEarly, "Button_EndSaleEarly")
        Me.Button_EndSaleEarly.BackColor = System.Drawing.SystemColors.Control
        Me.Button_EndSaleEarly.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_EndSaleEarly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_EndSaleEarly.Name = "Button_EndSaleEarly"
        Me.ToolTip1.SetToolTip(Me.Button_EndSaleEarly, resources.GetString("Button_EndSaleEarly.ToolTip"))
        Me.Button_EndSaleEarly.UseVisualStyleBackColor = False
        '
        'ugrdPriceList
        '
        resources.ApplyResources(Me.ugrdPriceList, "ugrdPriceList")
        Me.ugrdPriceList.DataSource = Me.UltraDataSource2
        Appearance6.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance6.BorderColor = System.Drawing.SystemColors.InactiveCaption
        resources.ApplyResources(Appearance6.FontData, "Appearance6.FontData")
        resources.ApplyResources(Appearance6, "Appearance6")
        Appearance6.ForceApplyResources = "FontData|"
        Me.ugrdPriceList.DisplayLayout.Appearance = Appearance6
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.RowLayoutColumnInfo.OriginX = 0
        UltraGridColumn1.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn1.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(91, 0)
        UltraGridColumn1.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn1.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn1.Width = 35
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.RowLayoutColumnInfo.OriginX = 2
        UltraGridColumn2.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(75, 0)
        UltraGridColumn2.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn2.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn2.Width = 64
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Hidden = True
        UltraGridColumn3.RowLayoutColumnInfo.OriginX = 4
        UltraGridColumn3.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn3.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn3.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn3.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.RowLayoutColumnInfo.OriginX = 6
        UltraGridColumn4.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(44, 0)
        UltraGridColumn4.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn4.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn4.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn4.Width = 58
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        resources.ApplyResources(Appearance3, "Appearance3")
        Appearance3.ForceApplyResources = ""
        UltraGridColumn5.CellAppearance = Appearance3
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.RowLayoutColumnInfo.OriginX = 8
        UltraGridColumn5.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn5.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(73, 0)
        UltraGridColumn5.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn5.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn5.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn5.Width = 49
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        resources.ApplyResources(Appearance10, "Appearance10")
        Appearance10.ForceApplyResources = ""
        UltraGridColumn6.CellAppearance = Appearance10
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.RowLayoutColumnInfo.OriginX = 10
        UltraGridColumn6.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn6.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(72, 0)
        UltraGridColumn6.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn6.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn6.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn6.Width = 49
        UltraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn7.Header.VisiblePosition = 7
        UltraGridColumn7.RowLayoutColumnInfo.OriginX = 12
        UltraGridColumn7.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn7.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(92, 0)
        UltraGridColumn7.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn7.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn7.Width = 58
        UltraGridColumn8.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn8.Header.VisiblePosition = 6
        UltraGridColumn8.Hidden = True
        UltraGridColumn8.RowLayoutColumnInfo.OriginX = 12
        UltraGridColumn8.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn8.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn8.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn8.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn9.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn9.Header.VisiblePosition = 13
        UltraGridColumn9.RowLayoutColumnInfo.OriginX = 24
        UltraGridColumn9.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn9.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(66, 0)
        UltraGridColumn9.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn9.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn9.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn9.Width = 58
        UltraGridColumn10.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn10.Header.VisiblePosition = 14
        UltraGridColumn10.RowLayoutColumnInfo.OriginX = 30
        UltraGridColumn10.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn10.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(25, 0)
        UltraGridColumn10.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn10.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn10.Width = 58
        UltraGridColumn11.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn11.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn11.Header.VisiblePosition = 15
        UltraGridColumn11.Hidden = True
        UltraGridColumn11.RowLayoutColumnInfo.OriginX = 30
        UltraGridColumn11.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn11.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn11.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn12.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn12.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn12.Header.VisiblePosition = 16
        UltraGridColumn12.Hidden = True
        UltraGridColumn12.MinWidth = 8
        UltraGridColumn12.RowLayoutColumnInfo.OriginX = 32
        UltraGridColumn12.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn12.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn12.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn13.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn13.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn13.Header.VisiblePosition = 18
        UltraGridColumn13.MinWidth = 4
        UltraGridColumn13.RowLayoutColumnInfo.OriginX = 32
        UltraGridColumn13.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn13.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(25, 0)
        UltraGridColumn13.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn13.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn13.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn13.Width = 58
        UltraGridColumn14.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn14.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn14.Header.VisiblePosition = 19
        UltraGridColumn14.MinWidth = 4
        UltraGridColumn14.RowLayoutColumnInfo.OriginX = 34
        UltraGridColumn14.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn14.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(13, 0)
        UltraGridColumn14.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn14.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn14.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn14.Width = 58
        UltraGridColumn15.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn15.Header.Caption = resources.GetString("resource.Caption")
        UltraGridColumn15.Header.VisiblePosition = 20
        UltraGridColumn15.RowLayoutColumnInfo.OriginX = 28
        UltraGridColumn15.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn15.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(60, 0)
        UltraGridColumn15.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn15.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn15.Width = 98
        resources.ApplyResources(Appearance11, "Appearance11")
        Appearance11.ForceApplyResources = ""
        UltraGridColumn16.CellAppearance = Appearance11
        resources.ApplyResources(UltraGridColumn16, "UltraGridColumn16")
        UltraGridColumn16.Header.Caption = resources.GetString("resource.Caption1")
        UltraGridColumn16.Header.ToolTipText = resources.GetString("resource.ToolTipText")
        UltraGridColumn16.Header.VisiblePosition = 11
        UltraGridColumn16.RowLayoutColumnInfo.OriginX = 20
        UltraGridColumn16.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn16.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(48, 0)
        UltraGridColumn16.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn16.RowLayoutColumnInfo.SpanY = 2
        resources.ApplyResources(Appearance25, "Appearance25")
        Appearance25.ForceApplyResources = ""
        UltraGridColumn17.CellAppearance = Appearance25
        resources.ApplyResources(UltraGridColumn17, "UltraGridColumn17")
        UltraGridColumn17.Header.Caption = resources.GetString("resource.Caption2")
        UltraGridColumn17.Header.ToolTipText = "Margin% (AvgCost) =100*(Current Reg Retail - Current Average Cost)/Current Reg Re" & _
    "tail"
        UltraGridColumn17.Header.VisiblePosition = 12
        UltraGridColumn17.RowLayoutColumnInfo.OriginX = 22
        UltraGridColumn17.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn17.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(59, 0)
        UltraGridColumn17.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn17.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn18.Header.Caption = resources.GetString("resource.Caption3")
        UltraGridColumn18.Header.ToolTipText = "Reg Cost = Current Reg Cost from Primary Vendor/Current Vendor Pack"
        UltraGridColumn18.Header.VisiblePosition = 8
        UltraGridColumn18.RowLayoutColumnInfo.OriginX = 14
        UltraGridColumn18.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn18.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(46, 0)
        UltraGridColumn18.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn18.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn19.Header.Caption = resources.GetString("resource.Caption4")
        UltraGridColumn19.Header.ToolTipText = "Net Cost = Current Net Cost from Primary Vendor/Current Vendor Pack"
        UltraGridColumn19.Header.VisiblePosition = 9
        UltraGridColumn19.RowLayoutColumnInfo.OriginX = 16
        UltraGridColumn19.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn19.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(44, 0)
        UltraGridColumn19.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn19.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn20.Header.Caption = resources.GetString("resource.Caption5")
        UltraGridColumn20.Header.ToolTipText = "Margin% (CurrCost) = 100*(Reg Retail - Current Net Cost from Primary Vendor)/Reg " & _
    "Retail"
        UltraGridColumn20.Header.VisiblePosition = 10
        UltraGridColumn20.RowLayoutColumnInfo.OriginX = 18
        UltraGridColumn20.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn20.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(59, 0)
        UltraGridColumn20.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn20.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn21.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn21.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn21.Header.VisiblePosition = 21
        UltraGridColumn21.MinWidth = 4
        UltraGridColumn21.RowLayoutColumnInfo.OriginX = 36
        UltraGridColumn21.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn21.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(24, 0)
        UltraGridColumn21.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn21.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn21.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn21.Width = 58
        UltraGridColumn22.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn22.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn22.Header.Caption = resources.GetString("resource.Caption6")
        UltraGridColumn22.Header.VisiblePosition = 17
        UltraGridColumn22.Hidden = True
        UltraGridColumn22.MinWidth = 4
        UltraGridColumn22.RowLayoutColumnInfo.OriginX = 34
        UltraGridColumn22.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn22.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn22.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn23.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn23.Header.VisiblePosition = 22
        UltraGridColumn23.RowLayoutColumnInfo.OriginX = 26
        UltraGridColumn23.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn23.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(91, 0)
        UltraGridColumn23.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn23.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn23.Width = 58
        UltraGridColumn24.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn24.Header.VisiblePosition = 23
        UltraGridColumn24.Hidden = True
        UltraGridColumn24.RowLayoutColumnInfo.OriginX = 46
        UltraGridColumn24.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn24.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn24.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn25.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn25.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn25.Header.VisiblePosition = 24
        UltraGridColumn25.RowLayoutColumnInfo.OriginX = 4
        UltraGridColumn25.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn25.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(76, 0)
        UltraGridColumn25.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn25.RowLayoutColumnInfo.SpanY = 2
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15, UltraGridColumn16, UltraGridColumn17, UltraGridColumn18, UltraGridColumn19, UltraGridColumn20, UltraGridColumn21, UltraGridColumn22, UltraGridColumn23, UltraGridColumn24, UltraGridColumn25})
        resources.ApplyResources(Appearance26.FontData, "Appearance26.FontData")
        resources.ApplyResources(Appearance26, "Appearance26")
        Appearance26.ForceApplyResources = "FontData|"
        UltraGridBand1.Override.CellAppearance = Appearance26
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout
        Me.ugrdPriceList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdPriceList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        resources.ApplyResources(Appearance12.FontData, "Appearance12.FontData")
        resources.ApplyResources(Appearance12, "Appearance12")
        Appearance12.ForceApplyResources = "FontData|"
        Me.ugrdPriceList.DisplayLayout.CaptionAppearance = Appearance12
        Me.ugrdPriceList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance13.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance13.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance13.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance13.FontData, "Appearance13.FontData")
        resources.ApplyResources(Appearance13, "Appearance13")
        Appearance13.ForceApplyResources = "FontData|"
        Me.ugrdPriceList.DisplayLayout.GroupByBox.Appearance = Appearance13
        Appearance14.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance14.FontData, "Appearance14.FontData")
        resources.ApplyResources(Appearance14, "Appearance14")
        Appearance14.ForceApplyResources = "FontData|"
        Me.ugrdPriceList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance14
        Me.ugrdPriceList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdPriceList.DisplayLayout.GroupByBox.Hidden = True
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance15.BackColor2 = System.Drawing.SystemColors.Control
        Appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance15.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance15.FontData, "Appearance15.FontData")
        resources.ApplyResources(Appearance15, "Appearance15")
        Appearance15.ForceApplyResources = "FontData|"
        Me.ugrdPriceList.DisplayLayout.GroupByBox.PromptAppearance = Appearance15
        Me.ugrdPriceList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdPriceList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance16.BackColor = System.Drawing.SystemColors.Window
        Appearance16.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Appearance16.FontData, "Appearance16.FontData")
        resources.ApplyResources(Appearance16, "Appearance16")
        Appearance16.ForceApplyResources = "FontData|"
        Me.ugrdPriceList.DisplayLayout.Override.ActiveCellAppearance = Appearance16
        Me.ugrdPriceList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdPriceList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance17.BackColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance17.FontData, "Appearance17.FontData")
        resources.ApplyResources(Appearance17, "Appearance17")
        Appearance17.ForceApplyResources = "FontData|"
        Me.ugrdPriceList.DisplayLayout.Override.CardAreaAppearance = Appearance17
        Appearance18.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance18.FontData, "Appearance18.FontData")
        Appearance18.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        resources.ApplyResources(Appearance18, "Appearance18")
        Appearance18.ForceApplyResources = "FontData|"
        Me.ugrdPriceList.DisplayLayout.Override.CellAppearance = Appearance18
        Me.ugrdPriceList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.ugrdPriceList.DisplayLayout.Override.CellPadding = 0
        resources.ApplyResources(Appearance19.FontData, "Appearance19.FontData")
        resources.ApplyResources(Appearance19, "Appearance19")
        Appearance19.ForceApplyResources = "FontData|"
        Me.ugrdPriceList.DisplayLayout.Override.FixedHeaderAppearance = Appearance19
        Appearance20.BackColor = System.Drawing.SystemColors.Control
        Appearance20.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance20.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance20.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance20.FontData, "Appearance20.FontData")
        resources.ApplyResources(Appearance20, "Appearance20")
        Appearance20.ForceApplyResources = "FontData|"
        Me.ugrdPriceList.DisplayLayout.Override.GroupByRowAppearance = Appearance20
        resources.ApplyResources(Appearance21.FontData, "Appearance21.FontData")
        resources.ApplyResources(Appearance21, "Appearance21")
        Appearance21.ForceApplyResources = "FontData|"
        Me.ugrdPriceList.DisplayLayout.Override.HeaderAppearance = Appearance21
        Me.ugrdPriceList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdPriceList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance22.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance22.FontData, "Appearance22.FontData")
        resources.ApplyResources(Appearance22, "Appearance22")
        Appearance22.ForceApplyResources = "FontData|"
        Me.ugrdPriceList.DisplayLayout.Override.RowAlternateAppearance = Appearance22
        Appearance23.BackColor = System.Drawing.SystemColors.Window
        Appearance23.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance23.FontData, "Appearance23.FontData")
        resources.ApplyResources(Appearance23, "Appearance23")
        Appearance23.ForceApplyResources = "FontData|"
        Me.ugrdPriceList.DisplayLayout.Override.RowAppearance = Appearance23
        Me.ugrdPriceList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdPriceList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdPriceList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdPriceList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdPriceList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance24.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance24.FontData, "Appearance24.FontData")
        resources.ApplyResources(Appearance24, "Appearance24")
        Appearance24.ForceApplyResources = "FontData|"
        Me.ugrdPriceList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance24
        Me.ugrdPriceList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdPriceList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdPriceList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdPriceList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdPriceList.Name = "ugrdPriceList"
        '
        'UltraDataSource2
        '
        UltraDataColumn5.DataType = GetType(Date)
        UltraDataColumn6.DataType = GetType(Date)
        Me.UltraDataSource2.Band.Columns.AddRange(New Object() {UltraDataColumn1, UltraDataColumn2, UltraDataColumn3, UltraDataColumn4, UltraDataColumn5, UltraDataColumn6, UltraDataColumn7, UltraDataColumn8, UltraDataColumn9, UltraDataColumn10, UltraDataColumn11, UltraDataColumn12, UltraDataColumn13, UltraDataColumn14, UltraDataColumn15, UltraDataColumn16, UltraDataColumn17, UltraDataColumn18, UltraDataColumn19, UltraDataColumn20})
        '
        'UltraDataSource1
        '
        Me.UltraDataSource1.Band.Columns.AddRange(New Object() {UltraDataColumn21, UltraDataColumn22, UltraDataColumn23, UltraDataColumn24, UltraDataColumn25, UltraDataColumn26, UltraDataColumn27, UltraDataColumn28, UltraDataColumn29, UltraDataColumn30, UltraDataColumn31, UltraDataColumn32, UltraDataColumn33, UltraDataColumn34, UltraDataColumn35, UltraDataColumn36})
        '
        'Label_PromotionExistence
        '
        resources.ApplyResources(Me.Label_PromotionExistence, "Label_PromotionExistence")
        Me.Label_PromotionExistence.Name = "Label_PromotionExistence"
        '
        'Label_UseAverageCostforCostandMargin
        '
        resources.ApplyResources(Me.Label_UseAverageCostforCostandMargin, "Label_UseAverageCostforCostandMargin")
        Me.Label_UseAverageCostforCostandMargin.ForeColor = System.Drawing.Color.Red
        Me.Label_UseAverageCostforCostandMargin.Name = "Label_UseAverageCostforCostandMargin"
        '
        'frmItemPrice
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.Label_UseAverageCostforCostandMargin)
        Me.Controls.Add(Me.Button_EndSaleEarly)
        Me.Controls.Add(Me.Button_CancelAllSales)
        Me.Controls.Add(Me.Button_MarginInfo)
        Me.Controls.Add(Me.Label_PromotionExistence)
        Me.Controls.Add(Me.ugrdPriceList)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdStore)
        Me.Controls.Add(Me.cmdPrice)
        Me.Controls.Add(Me.cmdSearch)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.MinimizeBox = False
        Me.Name = "frmItemPrice"
        Me.ShowInTaskbar = False
        CType(Me.cmdFunction, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdPriceList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDataSource2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ugrdPriceList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents Label_PromotionExistence As System.Windows.Forms.Label
    Friend WithEvents UltraDataSource1 As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Friend WithEvents UltraDataSource2 As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Public WithEvents Button_MarginInfo As System.Windows.Forms.Button
    Public WithEvents Button_CancelAllSales As System.Windows.Forms.Button
    Public WithEvents Button_EndSaleEarly As System.Windows.Forms.Button
    Friend WithEvents Label_UseAverageCostforCostandMargin As System.Windows.Forms.Label
#End Region 
End Class