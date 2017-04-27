<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmDistributionCreditOrder
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
    Public WithEvents cmdSubmit As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDistributionCreditOrder))
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Recordset", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderItem_ID")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Description")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Units_Per_Pallet")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Quantity")
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Quantity_Previous")
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("QuantityUnit_Text")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("QuantityUnit")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Package_Desc1")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Package_Desc2")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Package_Unit_ID")
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("LandedCost")
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("MarkupPercent")
        Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("MarkupCost")
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn16 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Cost")
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn17 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitCost")
        Dim UltraGridColumn18 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitExtCost")
        Dim UltraGridColumn19 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CostUnit")
        Dim UltraGridColumn20 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("LineItemCost")
        Dim UltraGridColumn21 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("LineItemFreight")
        Dim UltraGridColumn22 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Freight")
        Dim UltraGridColumn23 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("FreightUnit")
        Dim UltraGridColumn24 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Handling")
        Dim UltraGridColumn25 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("HandlingUnit")
        Dim UltraGridColumn26 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("QuantityDiscount")
        Dim UltraGridColumn27 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("DiscountType")
        Dim UltraGridColumn28 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Total_Weight")
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn29 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Total_Weight_Previous")
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn30 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Retail_Unit_ID")
        Dim UltraGridColumn31 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CreditReason")
        Dim UltraGridColumn32 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CreditReason_ID", -1, 213704360)
        Dim UltraGridColumn33 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Total_ReceivedItemCost")
        Dim UltraGridColumn34 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("NetVendorItemDiscount")
        Dim UltraGridColumn35 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("HandlingCharge")
        Dim UltraGridColumn36 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("RetailUnit_Text")
        Dim UltraGridColumn37 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ReturnUnit_Id", -1, 435633797)
        Dim UltraGridColumn38 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("RetailWeight_Unit")
        Dim UltraGridColumn39 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PackageSize")
        Dim ColScrollRegion1 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(867)
        Dim ColScrollRegion2 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(638)
        Dim ColScrollRegion3 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(500)
        Dim ColScrollRegion4 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(-507)
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim ValueList1 As Infragistics.Win.ValueList = New Infragistics.Win.ValueList(213704360)
        Dim ValueList2 As Infragistics.Win.ValueList = New Infragistics.Win.ValueList(435633797)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdSubmit = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.ugrdReturn = New Infragistics.Win.UltraWinGrid.UltraGrid
        CType(Me.ugrdReturn, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdSubmit
        '
        Me.cmdSubmit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSubmit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSubmit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSubmit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSubmit.Image = CType(resources.GetObject("cmdSubmit.Image"), System.Drawing.Image)
        Me.cmdSubmit.Location = New System.Drawing.Point(788, 280)
        Me.cmdSubmit.Name = "cmdSubmit"
        Me.cmdSubmit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSubmit.Size = New System.Drawing.Size(41, 41)
        Me.cmdSubmit.TabIndex = 1
        Me.cmdSubmit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSubmit, "Commit Changes")
        Me.cmdSubmit.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(836, 280)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 2
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'ugrdReturn
        '
        Me.ugrdReturn.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn1.Width = 14
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Hidden = True
        UltraGridColumn2.TabStop = False
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.MinWidth = 97
        UltraGridColumn3.TabStop = False
        UltraGridColumn3.Width = 102
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.Header.Caption = "Description"
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.MinWidth = 179
        UltraGridColumn4.TabStop = False
        UltraGridColumn4.Width = 180
        UltraGridColumn5.Header.VisiblePosition = 7
        UltraGridColumn5.Hidden = True
        UltraGridColumn5.Width = 14
        Appearance1.TextHAlignAsString = "Right"
        UltraGridColumn6.CellAppearance = Appearance1
        UltraGridColumn6.Format = "##0.00"
        UltraGridColumn6.Header.Caption = "Return"
        UltraGridColumn6.Header.VisiblePosition = 6
        UltraGridColumn6.MinWidth = 50
        UltraGridColumn6.Width = 50
        Appearance2.TextHAlignAsString = "Right"
        UltraGridColumn7.CellAppearance = Appearance2
        UltraGridColumn7.Format = "##0.00"
        UltraGridColumn7.Header.Caption = "Rec'd"
        UltraGridColumn7.Header.VisiblePosition = 5
        UltraGridColumn7.MinWidth = 50
        UltraGridColumn7.TabStop = False
        UltraGridColumn7.Width = 50
        UltraGridColumn8.Header.VisiblePosition = 11
        UltraGridColumn8.Hidden = True
        UltraGridColumn8.Width = 103
        UltraGridColumn9.Header.VisiblePosition = 8
        UltraGridColumn9.Hidden = True
        UltraGridColumn9.TabStop = False
        UltraGridColumn9.Width = 26
        UltraGridColumn10.Header.VisiblePosition = 14
        UltraGridColumn10.Hidden = True
        UltraGridColumn10.Width = 55
        UltraGridColumn11.Header.VisiblePosition = 16
        UltraGridColumn11.Hidden = True
        UltraGridColumn11.Width = 14
        UltraGridColumn12.Header.VisiblePosition = 17
        UltraGridColumn12.Hidden = True
        UltraGridColumn12.Width = 14
        UltraGridColumn13.Header.VisiblePosition = 19
        UltraGridColumn13.Hidden = True
        UltraGridColumn13.Width = 14
        UltraGridColumn14.Header.VisiblePosition = 22
        UltraGridColumn14.Hidden = True
        UltraGridColumn14.Width = 14
        UltraGridColumn15.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance15.TextHAlignAsString = "Right"
        UltraGridColumn15.CellAppearance = Appearance15
        UltraGridColumn15.Format = "####0.00"
        UltraGridColumn15.Header.Caption = "Delivered Cost"
        UltraGridColumn15.Header.VisiblePosition = 13
        UltraGridColumn15.MinWidth = 90
        UltraGridColumn15.Width = 90
        UltraGridColumn16.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance3.TextHAlignAsString = "Right"
        UltraGridColumn16.CellAppearance = Appearance3
        UltraGridColumn16.Format = "####0.00"
        UltraGridColumn16.Header.VisiblePosition = 12
        UltraGridColumn16.MinWidth = 60
        UltraGridColumn16.TabStop = False
        UltraGridColumn16.Width = 61
        UltraGridColumn17.Header.VisiblePosition = 18
        UltraGridColumn17.Hidden = True
        UltraGridColumn17.Width = 14
        UltraGridColumn18.Header.VisiblePosition = 20
        UltraGridColumn18.Hidden = True
        UltraGridColumn18.Width = 39
        UltraGridColumn19.Header.VisiblePosition = 29
        UltraGridColumn19.Hidden = True
        UltraGridColumn19.Width = 14
        UltraGridColumn20.Header.VisiblePosition = 21
        UltraGridColumn20.Hidden = True
        UltraGridColumn20.Width = 14
        UltraGridColumn21.Header.VisiblePosition = 23
        UltraGridColumn21.Hidden = True
        UltraGridColumn21.Width = 14
        UltraGridColumn22.Header.VisiblePosition = 30
        UltraGridColumn22.Hidden = True
        UltraGridColumn22.Width = 14
        UltraGridColumn23.Header.VisiblePosition = 31
        UltraGridColumn23.Hidden = True
        UltraGridColumn23.Width = 14
        UltraGridColumn24.Header.VisiblePosition = 24
        UltraGridColumn24.Hidden = True
        UltraGridColumn24.Width = 14
        UltraGridColumn25.Header.VisiblePosition = 26
        UltraGridColumn25.Hidden = True
        UltraGridColumn25.Width = 14
        UltraGridColumn26.Header.VisiblePosition = 32
        UltraGridColumn26.Hidden = True
        UltraGridColumn26.Width = 14
        UltraGridColumn27.Header.VisiblePosition = 33
        UltraGridColumn27.Hidden = True
        UltraGridColumn27.Width = 14
        Appearance4.TextHAlignAsString = "Right"
        UltraGridColumn28.CellAppearance = Appearance4
        UltraGridColumn28.Format = "####0.00"
        UltraGridColumn28.Header.Caption = "Wgt Ret"
        UltraGridColumn28.Header.VisiblePosition = 10
        UltraGridColumn28.MaskInput = "{double:5.2}"
        UltraGridColumn28.MinWidth = 60
        UltraGridColumn28.Width = 61
        UltraGridColumn29.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance5.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance5.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance5.BorderColor = System.Drawing.SystemColors.Window
        Appearance5.TextHAlignAsString = "Right"
        UltraGridColumn29.CellAppearance = Appearance5
        UltraGridColumn29.Format = "####0.00"
        UltraGridColumn29.Header.Caption = "Wgt Rec"
        UltraGridColumn29.Header.VisiblePosition = 9
        UltraGridColumn29.MinWidth = 60
        UltraGridColumn29.TabStop = False
        UltraGridColumn29.Width = 61
        UltraGridColumn30.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn30.Header.VisiblePosition = 25
        UltraGridColumn30.Hidden = True
        UltraGridColumn30.Width = 17
        UltraGridColumn31.Header.VisiblePosition = 34
        UltraGridColumn31.Hidden = True
        UltraGridColumn31.Width = 14
        UltraGridColumn32.Header.Caption = "Credit Reason"
        UltraGridColumn32.Header.VisiblePosition = 15
        UltraGridColumn32.MinWidth = 116
        UltraGridColumn32.Width = 118
        UltraGridColumn33.Header.VisiblePosition = 35
        UltraGridColumn33.Hidden = True
        UltraGridColumn33.Width = 14
        UltraGridColumn34.Header.VisiblePosition = 36
        UltraGridColumn34.Hidden = True
        UltraGridColumn34.Width = 14
        UltraGridColumn35.Header.VisiblePosition = 37
        UltraGridColumn35.Hidden = True
        UltraGridColumn35.Width = 14
        UltraGridColumn36.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn36.Header.VisiblePosition = 27
        UltraGridColumn36.Hidden = True
        UltraGridColumn36.Width = 36
        UltraGridColumn37.Header.Caption = "Unit"
        UltraGridColumn37.Header.VisiblePosition = 4
        UltraGridColumn37.MinWidth = 74
        UltraGridColumn37.Width = 75
        UltraGridColumn38.Header.VisiblePosition = 38
        UltraGridColumn38.Hidden = True
        UltraGridColumn38.Width = 18
        UltraGridColumn39.Header.VisiblePosition = 28
        UltraGridColumn39.Hidden = True
        UltraGridColumn39.Width = 14
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15, UltraGridColumn16, UltraGridColumn17, UltraGridColumn18, UltraGridColumn19, UltraGridColumn20, UltraGridColumn21, UltraGridColumn22, UltraGridColumn23, UltraGridColumn24, UltraGridColumn25, UltraGridColumn26, UltraGridColumn27, UltraGridColumn28, UltraGridColumn29, UltraGridColumn30, UltraGridColumn31, UltraGridColumn32, UltraGridColumn33, UltraGridColumn34, UltraGridColumn35, UltraGridColumn36, UltraGridColumn37, UltraGridColumn38, UltraGridColumn39})
        UltraGridBand1.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.[True]
        UltraGridBand1.Expandable = False
        UltraGridBand1.GroupHeadersVisible = False
        UltraGridBand1.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdReturn.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdReturn.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdReturn.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdReturn.DisplayLayout.ColScrollRegions.Add(ColScrollRegion1)
        Me.ugrdReturn.DisplayLayout.ColScrollRegions.Add(ColScrollRegion2)
        Me.ugrdReturn.DisplayLayout.ColScrollRegions.Add(ColScrollRegion3)
        Me.ugrdReturn.DisplayLayout.ColScrollRegions.Add(ColScrollRegion4)
        Me.ugrdReturn.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.[False]
        Appearance6.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance6.BorderColor = System.Drawing.SystemColors.Window
        Appearance6.TextHAlignAsString = "Right"
        Me.ugrdReturn.DisplayLayout.GroupByBox.Appearance = Appearance6
        Appearance7.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdReturn.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance7
        Me.ugrdReturn.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdReturn.DisplayLayout.GroupByBox.Hidden = True
        Appearance8.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance8.BackColor2 = System.Drawing.SystemColors.Control
        Appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance8.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdReturn.DisplayLayout.GroupByBox.PromptAppearance = Appearance8
        Me.ugrdReturn.DisplayLayout.MaxBandDepth = 1
        Me.ugrdReturn.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdReturn.DisplayLayout.MaxRowScrollRegions = 1
        Me.ugrdReturn.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.ugrdReturn.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
        Me.ugrdReturn.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed
        Me.ugrdReturn.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdReturn.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdReturn.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None
        Me.ugrdReturn.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        Me.ugrdReturn.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdReturn.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdReturn.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance9.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdReturn.DisplayLayout.Override.CardAreaAppearance = Appearance9
        Appearance10.BorderColor = System.Drawing.Color.Silver
        Appearance10.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdReturn.DisplayLayout.Override.CellAppearance = Appearance10
        Me.ugrdReturn.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdReturn.DisplayLayout.Override.CellPadding = 0
        Appearance11.BackColor = System.Drawing.SystemColors.Control
        Appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance11.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance11.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdReturn.DisplayLayout.Override.GroupByRowAppearance = Appearance11
        Appearance12.TextHAlignAsString = "Left"
        Me.ugrdReturn.DisplayLayout.Override.HeaderAppearance = Appearance12
        Me.ugrdReturn.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ugrdReturn.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Me.ugrdReturn.DisplayLayout.Override.MaxSelectedCells = 1
        Appearance13.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdReturn.DisplayLayout.Override.RowAlternateAppearance = Appearance13
        Me.ugrdReturn.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdReturn.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdReturn.DisplayLayout.Override.TemplateAddRowAppearance = Appearance14
        Me.ugrdReturn.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdReturn.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        ValueList1.Key = "CreditReasons"
        ValueList2.Key = "Units"
        Me.ugrdReturn.DisplayLayout.ValueLists.AddRange(New Infragistics.Win.ValueList() {ValueList1, ValueList2})
        Me.ugrdReturn.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdReturn.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugrdReturn.Location = New System.Drawing.Point(8, 12)
        Me.ugrdReturn.Name = "ugrdReturn"
        Me.ugrdReturn.Size = New System.Drawing.Size(869, 262)
        Me.ugrdReturn.TabIndex = 0
        Me.ugrdReturn.Text = "Return Order"
        '
        'frmDistributionCreditOrder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(885, 327)
        Me.Controls.Add(Me.ugrdReturn)
        Me.Controls.Add(Me.cmdSubmit)
        Me.Controls.Add(Me.cmdExit)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDistributionCreditOrder"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Distribution Credit Order"
        CType(Me.ugrdReturn, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ugrdReturn As Infragistics.Win.UltraWinGrid.UltraGrid
#End Region
End Class

