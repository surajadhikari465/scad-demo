<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EInvoicing_ViewInvoices
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Invoice")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("InvoiceDate")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PurchaseOrder", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_No")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorId")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Vendor")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Einvoice_Id")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ImportDate")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Status")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ErrorCode_id")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ErrorMessage")
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Archived", 0)
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ArchivedDate", 1)
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance37 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance38 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EInvoicing_ViewInvoices))
        Dim Appearance54 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance53 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance36 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance35 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance49 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance50 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance51 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance39 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand2 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("StoreBO", -1)
        Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StoreNo")
        Dim UltraGridColumn16 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StoreName")
        Dim UltraGridColumn17 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PhoneNo")
        Dim UltraGridColumn18 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("RegionalOffice")
        Dim UltraGridColumn19 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("POSSystem")
        Dim UltraGridColumn20 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ZoneId")
        Dim UltraGridColumn21 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("State")
        Dim UltraGridColumn22 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("BusinessUnitId", 0)
        Dim Appearance40 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance41 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance42 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance43 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance44 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance45 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance46 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance47 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance48 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance59 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance60 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand3 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("", -1)
        Dim UltraGridColumn23 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ErrorCodeId", 0)
        Dim UltraGridColumn24 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ErrorMessage", 1)
        Dim UltraGridColumn25 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ErrorDescription", 2)
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance28 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance29 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance30 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance31 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance32 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance33 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance34 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance52 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand4 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("", -1)
        Dim UltraGridColumn26 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorID", 0)
        Dim UltraGridColumn27 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorName", 1)
        Dim UltraGridColumn28 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PSVendorId", 2)
        Me.UltraGrid_EInvoices = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.InfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ReparseMenu_MenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewErrorInformationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangePOInformationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ArchiveSelectedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ForceLoadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ReportsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SuspendedEInvoicesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UnmatchedPOsForEInvoicingVendorsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.EInvoicing_ViewInvoices_Fill_Panel = New System.Windows.Forms.Panel()
        Me.Panel_Reparse = New System.Windows.Forms.Panel()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel_Info = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_Count = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnCancelReparse = New System.Windows.Forms.Button()
        Me.ListView_Reparse = New System.Windows.Forms.ListView()
        Me.column_EinvoiceID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.column_PO = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.column_Invoice = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.column_Store = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.column_Vendor = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.column_Status = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraDateTimeEditor_InvoiceStart = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraDateTimeEditor_InvoiceEnd = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraDateTimeEditor_ImportEnd = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraDateTimeEditor_ImportStart = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraGroupBox1 = New Infragistics.Win.Misc.UltraGroupBox()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTextEditor_InvoiceNumber = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.UltraTextEditor_PONumber = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel6 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel8 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraCombo_Store = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.UltraCombo_ErrorMessage = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.UltraCombo_Status = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.UltraLabel7 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraButton_Search = New Infragistics.Win.Misc.UltraButton()
        Me.UltraToolTipManager1 = New Infragistics.Win.UltraWinToolTip.UltraToolTipManager(Me.components)
        Me.UltraCombo_Vendors = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.CheckBox_Archived = New System.Windows.Forms.CheckBox()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        CType(Me.UltraGrid_EInvoices, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.EInvoicing_ViewInvoices_Fill_Panel.SuspendLayout()
        Me.Panel_Reparse.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.UltraDateTimeEditor_InvoiceStart, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDateTimeEditor_InvoiceEnd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDateTimeEditor_ImportEnd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDateTimeEditor_ImportStart, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraGroupBox1.SuspendLayout()
        CType(Me.UltraTextEditor_InvoiceNumber, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraTextEditor_PONumber, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraCombo_Store, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraCombo_ErrorMessage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraCombo_Status, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraCombo_Vendors, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'UltraGrid_EInvoices
        '
        Me.UltraGrid_EInvoices.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UltraGrid_EInvoices.ContextMenuStrip = Me.ContextMenuStrip1
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_EInvoices.DisplayLayout.Appearance = Appearance1
        Me.UltraGrid_EInvoices.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 65
        UltraGridColumn2.FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.GreaterThanOrEqualTo
        UltraGridColumn2.FilterOperatorDropDownItems = CType((((Infragistics.Win.UltraWinGrid.FilterOperatorDropDownItems.LessThan Or Infragistics.Win.UltraWinGrid.FilterOperatorDropDownItems.LessThanOrEqualTo) _
            Or Infragistics.Win.UltraWinGrid.FilterOperatorDropDownItems.GreaterThan) _
            Or Infragistics.Win.UltraWinGrid.FilterOperatorDropDownItems.GreaterThanOrEqualTo), Infragistics.Win.UltraWinGrid.FilterOperatorDropDownItems)
        UltraGridColumn2.Header.Caption = "Invoice Date"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 154
        UltraGridColumn3.Header.Caption = "PO"
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 91
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Hidden = True
        UltraGridColumn4.Width = 15
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Width = 122
        UltraGridColumn6.Header.Caption = "Vendor Id"
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Width = 96
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Width = 177
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Hidden = True
        UltraGridColumn8.Width = 21
        UltraGridColumn9.FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.GreaterThanOrEqualTo
        UltraGridColumn9.FilterOperatorDropDownItems = CType((((Infragistics.Win.UltraWinGrid.FilterOperatorDropDownItems.LessThan Or Infragistics.Win.UltraWinGrid.FilterOperatorDropDownItems.LessThanOrEqualTo) _
            Or Infragistics.Win.UltraWinGrid.FilterOperatorDropDownItems.GreaterThan) _
            Or Infragistics.Win.UltraWinGrid.FilterOperatorDropDownItems.GreaterThanOrEqualTo), Infragistics.Win.UltraWinGrid.FilterOperatorDropDownItems)
        UltraGridColumn9.Header.Caption = "Import Date"
        UltraGridColumn9.Header.VisiblePosition = 8
        UltraGridColumn9.Width = 65
        UltraGridColumn10.FilterClearButtonVisible = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn10.FilterEvaluationTrigger = Infragistics.Win.UltraWinGrid.FilterEvaluationTrigger.OnCellValueChange
        UltraGridColumn10.FilterOperandStyle = Infragistics.Win.UltraWinGrid.FilterOperandStyle.DropDownList
        UltraGridColumn10.FilterOperatorLocation = Infragistics.Win.UltraWinGrid.FilterOperatorLocation.Hidden
        UltraGridColumn10.Header.VisiblePosition = 9
        UltraGridColumn10.Width = 83
        UltraGridColumn11.Header.VisiblePosition = 10
        UltraGridColumn11.Hidden = True
        UltraGridColumn11.Width = 100
        UltraGridColumn12.Header.Caption = "Error Message"
        UltraGridColumn12.Header.VisiblePosition = 11
        UltraGridColumn12.Width = 157
        UltraGridColumn13.Header.VisiblePosition = 12
        UltraGridColumn13.Hidden = True
        UltraGridColumn13.Width = 75
        UltraGridColumn14.Header.VisiblePosition = 13
        UltraGridColumn14.Hidden = True
        UltraGridColumn14.Width = 75
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14})
        Me.UltraGrid_EInvoices.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid_EInvoices.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_EInvoices.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_EInvoices.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_EInvoices.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.UltraGrid_EInvoices.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_EInvoices.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.UltraGrid_EInvoices.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_EInvoices.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_EInvoices.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.Highlight
        Appearance6.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_EInvoices.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.UltraGrid_EInvoices.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.UltraGrid_EInvoices.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_EInvoices.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_EInvoices.DisplayLayout.Override.AllowGroupMoving = Infragistics.Win.UltraWinGrid.AllowGroupMoving.NotAllowed
        Me.UltraGrid_EInvoices.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_EInvoices.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_EInvoices.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_EInvoices.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_EInvoices.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_EInvoices.DisplayLayout.Override.CellAppearance = Appearance8
        Me.UltraGrid_EInvoices.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_EInvoices.DisplayLayout.Override.CellPadding = 0
        Me.UltraGrid_EInvoices.DisplayLayout.Override.FilterClearButtonLocation = Infragistics.Win.UltraWinGrid.FilterClearButtonLocation.RowAndCell
        Me.UltraGrid_EInvoices.DisplayLayout.Override.FilterEvaluationTrigger = Infragistics.Win.UltraWinGrid.FilterEvaluationTrigger.OnEnterKeyOrLeaveRow
        Me.UltraGrid_EInvoices.DisplayLayout.Override.FilterOperandStyle = Infragistics.Win.UltraWinGrid.FilterOperandStyle.UseColumnEditor
        Me.UltraGrid_EInvoices.DisplayLayout.Override.FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.Equals
        Me.UltraGrid_EInvoices.DisplayLayout.Override.FilterOperatorLocation = Infragistics.Win.UltraWinGrid.FilterOperatorLocation.WithOperand
        Me.UltraGrid_EInvoices.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_EInvoices.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        Appearance10.TextHAlignAsString = "Left"
        Me.UltraGrid_EInvoices.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.UltraGrid_EInvoices.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_EInvoices.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance37.BackColor = System.Drawing.SystemColors.Window
        Appearance37.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_EInvoices.DisplayLayout.Override.RowAppearance = Appearance37
        Me.UltraGrid_EInvoices.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.HideFilteredOutRows
        Me.UltraGrid_EInvoices.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance38.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_EInvoices.DisplayLayout.Override.TemplateAddRowAppearance = Appearance38
        Me.UltraGrid_EInvoices.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_EInvoices.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_EInvoices.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_EInvoices.Location = New System.Drawing.Point(0, 28)
        Me.UltraGrid_EInvoices.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.UltraGrid_EInvoices.Name = "UltraGrid_EInvoices"
        Me.UltraGrid_EInvoices.Size = New System.Drawing.Size(1012, 478)
        Me.UltraGrid_EInvoices.TabIndex = 0
        Me.UltraGrid_EInvoices.Text = "EInvoices"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InfoToolStripMenuItem, Me.ToolStripSeparator1, Me.ReparseMenu_MenuItem, Me.ViewErrorInformationToolStripMenuItem, Me.ChangePOInformationToolStripMenuItem, Me.ArchiveSelectedToolStripMenuItem, Me.ForceLoadToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(203, 142)
        '
        'InfoToolStripMenuItem
        '
        Me.InfoToolStripMenuItem.Enabled = False
        Me.InfoToolStripMenuItem.Name = "InfoToolStripMenuItem"
        Me.InfoToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.InfoToolStripMenuItem.Text = "Info"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(199, 6)
        '
        'ReparseMenu_MenuItem
        '
        Me.ReparseMenu_MenuItem.Image = CType(resources.GetObject("ReparseMenu_MenuItem.Image"), System.Drawing.Image)
        Me.ReparseMenu_MenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ReparseMenu_MenuItem.Name = "ReparseMenu_MenuItem"
        Me.ReparseMenu_MenuItem.Size = New System.Drawing.Size(202, 22)
        Me.ReparseMenu_MenuItem.Text = "Reparse Invoice"
        '
        'ViewErrorInformationToolStripMenuItem
        '
        Me.ViewErrorInformationToolStripMenuItem.Image = CType(resources.GetObject("ViewErrorInformationToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ViewErrorInformationToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ViewErrorInformationToolStripMenuItem.Name = "ViewErrorInformationToolStripMenuItem"
        Me.ViewErrorInformationToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.ViewErrorInformationToolStripMenuItem.Text = "View Error Information"
        '
        'ChangePOInformationToolStripMenuItem
        '
        Me.ChangePOInformationToolStripMenuItem.Image = CType(resources.GetObject("ChangePOInformationToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ChangePOInformationToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ChangePOInformationToolStripMenuItem.Name = "ChangePOInformationToolStripMenuItem"
        Me.ChangePOInformationToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.ChangePOInformationToolStripMenuItem.Text = "Change PO Information"
        '
        'ArchiveSelectedToolStripMenuItem
        '
        Me.ArchiveSelectedToolStripMenuItem.Image = CType(resources.GetObject("ArchiveSelectedToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ArchiveSelectedToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ArchiveSelectedToolStripMenuItem.Name = "ArchiveSelectedToolStripMenuItem"
        Me.ArchiveSelectedToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.ArchiveSelectedToolStripMenuItem.Text = "Archive Selected"
        '
        'ForceLoadToolStripMenuItem
        '
        Me.ForceLoadToolStripMenuItem.Image = CType(resources.GetObject("ForceLoadToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ForceLoadToolStripMenuItem.Name = "ForceLoadToolStripMenuItem"
        Me.ForceLoadToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.ForceLoadToolStripMenuItem.Text = "Force Load"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReportsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1012, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ReportsToolStripMenuItem
        '
        Me.ReportsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SuspendedEInvoicesToolStripMenuItem, Me.UnmatchedPOsForEInvoicingVendorsToolStripMenuItem})
        Me.ReportsToolStripMenuItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ReportsToolStripMenuItem.Name = "ReportsToolStripMenuItem"
        Me.ReportsToolStripMenuItem.Size = New System.Drawing.Size(62, 20)
        Me.ReportsToolStripMenuItem.Text = "Reports"
        '
        'SuspendedEInvoicesToolStripMenuItem
        '
        Me.SuspendedEInvoicesToolStripMenuItem.Name = "SuspendedEInvoicesToolStripMenuItem"
        Me.SuspendedEInvoicesToolStripMenuItem.Size = New System.Drawing.Size(290, 22)
        Me.SuspendedEInvoicesToolStripMenuItem.Text = "Suspended eInvoices"
        '
        'UnmatchedPOsForEInvoicingVendorsToolStripMenuItem
        '
        Me.UnmatchedPOsForEInvoicingVendorsToolStripMenuItem.Name = "UnmatchedPOsForEInvoicingVendorsToolStripMenuItem"
        Me.UnmatchedPOsForEInvoicingVendorsToolStripMenuItem.Size = New System.Drawing.Size(290, 22)
        Me.UnmatchedPOsForEInvoicingVendorsToolStripMenuItem.Text = "Unmatched PO's for eInvoicing Vendors"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(3, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(288, 15)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Right click on a suspended eInvoice to view options."
        '
        'EInvoicing_ViewInvoices_Fill_Panel
        '
        Me.EInvoicing_ViewInvoices_Fill_Panel.Controls.Add(Me.Panel_Reparse)
        Me.EInvoicing_ViewInvoices_Fill_Panel.Controls.Add(Me.UltraGrid_EInvoices)
        Me.EInvoicing_ViewInvoices_Fill_Panel.Controls.Add(Me.Label2)
        Me.EInvoicing_ViewInvoices_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default
        Me.EInvoicing_ViewInvoices_Fill_Panel.Location = New System.Drawing.Point(0, 128)
        Me.EInvoicing_ViewInvoices_Fill_Panel.Name = "EInvoicing_ViewInvoices_Fill_Panel"
        Me.EInvoicing_ViewInvoices_Fill_Panel.Size = New System.Drawing.Size(1012, 527)
        Me.EInvoicing_ViewInvoices_Fill_Panel.TabIndex = 2
        '
        'Panel_Reparse
        '
        Me.Panel_Reparse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel_Reparse.Controls.Add(Me.StatusStrip1)
        Me.Panel_Reparse.Controls.Add(Me.Label1)
        Me.Panel_Reparse.Controls.Add(Me.btnCancelReparse)
        Me.Panel_Reparse.Controls.Add(Me.ListView_Reparse)
        Me.Panel_Reparse.Location = New System.Drawing.Point(166, 37)
        Me.Panel_Reparse.Name = "Panel_Reparse"
        Me.Panel_Reparse.Size = New System.Drawing.Size(681, 373)
        Me.Panel_Reparse.TabIndex = 5
        Me.Panel_Reparse.Visible = False
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel_Info, Me.ToolStripStatusLabel_Count, Me.ToolStripProgressBar1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 349)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(679, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel_Info
        '
        Me.ToolStripStatusLabel_Info.Name = "ToolStripStatusLabel_Info"
        Me.ToolStripStatusLabel_Info.Size = New System.Drawing.Size(562, 17)
        Me.ToolStripStatusLabel_Info.Spring = True
        Me.ToolStripStatusLabel_Info.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripStatusLabel_Count
        '
        Me.ToolStripStatusLabel_Count.Name = "ToolStripStatusLabel_Count"
        Me.ToolStripStatusLabel_Count.Size = New System.Drawing.Size(0, 17)
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(100, 16)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(4, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(335, 25)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Reparsing Selected E-Invoices"
        '
        'btnCancelReparse
        '
        Me.btnCancelReparse.Location = New System.Drawing.Point(555, 318)
        Me.btnCancelReparse.Name = "btnCancelReparse"
        Me.btnCancelReparse.Size = New System.Drawing.Size(115, 28)
        Me.btnCancelReparse.TabIndex = 2
        Me.btnCancelReparse.Text = "Cancel"
        Me.btnCancelReparse.UseVisualStyleBackColor = True
        '
        'ListView_Reparse
        '
        Me.ListView_Reparse.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.ListView_Reparse.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.column_EinvoiceID, Me.column_PO, Me.column_Invoice, Me.column_Store, Me.column_Vendor, Me.column_Status})
        Me.ListView_Reparse.GridLines = True
        Me.ListView_Reparse.Location = New System.Drawing.Point(9, 61)
        Me.ListView_Reparse.Name = "ListView_Reparse"
        Me.ListView_Reparse.Size = New System.Drawing.Size(661, 251)
        Me.ListView_Reparse.TabIndex = 0
        Me.ListView_Reparse.UseCompatibleStateImageBehavior = False
        Me.ListView_Reparse.View = System.Windows.Forms.View.Details
        '
        'column_EinvoiceID
        '
        Me.column_EinvoiceID.Text = "EinvoiceId"
        Me.column_EinvoiceID.Width = 70
        '
        'column_PO
        '
        Me.column_PO.Text = "PO"
        Me.column_PO.Width = 100
        '
        'column_Invoice
        '
        Me.column_Invoice.Text = "Invoice"
        Me.column_Invoice.Width = 100
        '
        'column_Store
        '
        Me.column_Store.Text = "Store"
        Me.column_Store.Width = 150
        '
        'column_Vendor
        '
        Me.column_Vendor.Text = "Vendor"
        Me.column_Vendor.Width = 150
        '
        'column_Status
        '
        Me.column_Status.Text = "Status"
        Me.column_Status.Width = 80
        '
        'UltraLabel1
        '
        Appearance54.TextHAlignAsString = "Right"
        Appearance54.TextVAlignAsString = "Middle"
        Me.UltraLabel1.Appearance = Appearance54
        Me.UltraLabel1.Location = New System.Drawing.Point(6, 21)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(58, 13)
        Me.UltraLabel1.TabIndex = 3
        Me.UltraLabel1.Text = "Invoice"
        '
        'UltraLabel2
        '
        Appearance53.TextHAlignAsString = "Right"
        Appearance53.TextVAlignAsString = "Middle"
        Me.UltraLabel2.Appearance = Appearance53
        Me.UltraLabel2.Location = New System.Drawing.Point(6, 47)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(58, 20)
        Me.UltraLabel2.TabIndex = 4
        Me.UltraLabel2.Text = "Import"
        '
        'UltraDateTimeEditor_InvoiceStart
        '
        Me.UltraDateTimeEditor_InvoiceStart.DropDownCalendarAlignment = Infragistics.Win.DropDownListAlignment.Center
        Me.UltraDateTimeEditor_InvoiceStart.Location = New System.Drawing.Point(70, 17)
        Me.UltraDateTimeEditor_InvoiceStart.Name = "UltraDateTimeEditor_InvoiceStart"
        Me.UltraDateTimeEditor_InvoiceStart.Size = New System.Drawing.Size(98, 23)
        Me.UltraDateTimeEditor_InvoiceStart.TabIndex = 5
        '
        'UltraDateTimeEditor_InvoiceEnd
        '
        Me.UltraDateTimeEditor_InvoiceEnd.DropDownCalendarAlignment = Infragistics.Win.DropDownListAlignment.Center
        Me.UltraDateTimeEditor_InvoiceEnd.Location = New System.Drawing.Point(192, 16)
        Me.UltraDateTimeEditor_InvoiceEnd.Name = "UltraDateTimeEditor_InvoiceEnd"
        Me.UltraDateTimeEditor_InvoiceEnd.Size = New System.Drawing.Size(98, 23)
        Me.UltraDateTimeEditor_InvoiceEnd.TabIndex = 6
        '
        'UltraDateTimeEditor_ImportEnd
        '
        Me.UltraDateTimeEditor_ImportEnd.DropDownCalendarAlignment = Infragistics.Win.DropDownListAlignment.Center
        Me.UltraDateTimeEditor_ImportEnd.Location = New System.Drawing.Point(192, 47)
        Me.UltraDateTimeEditor_ImportEnd.Name = "UltraDateTimeEditor_ImportEnd"
        Me.UltraDateTimeEditor_ImportEnd.Size = New System.Drawing.Size(98, 23)
        Me.UltraDateTimeEditor_ImportEnd.TabIndex = 8
        Me.UltraDateTimeEditor_ImportEnd.Value = Nothing
        '
        'UltraDateTimeEditor_ImportStart
        '
        Me.UltraDateTimeEditor_ImportStart.DropDownCalendarAlignment = Infragistics.Win.DropDownListAlignment.Center
        Me.UltraDateTimeEditor_ImportStart.Location = New System.Drawing.Point(70, 46)
        Me.UltraDateTimeEditor_ImportStart.Name = "UltraDateTimeEditor_ImportStart"
        Me.UltraDateTimeEditor_ImportStart.Size = New System.Drawing.Size(98, 23)
        Me.UltraDateTimeEditor_ImportStart.TabIndex = 7
        Me.UltraDateTimeEditor_ImportStart.Value = Nothing
        '
        'UltraGroupBox1
        '
        Me.UltraGroupBox1.Controls.Add(Me.UltraDateTimeEditor_InvoiceEnd)
        Me.UltraGroupBox1.Controls.Add(Me.UltraDateTimeEditor_ImportEnd)
        Me.UltraGroupBox1.Controls.Add(Me.UltraLabel1)
        Me.UltraGroupBox1.Controls.Add(Me.UltraDateTimeEditor_ImportStart)
        Me.UltraGroupBox1.Controls.Add(Me.UltraLabel2)
        Me.UltraGroupBox1.Controls.Add(Me.UltraDateTimeEditor_InvoiceStart)
        Me.UltraGroupBox1.Location = New System.Drawing.Point(12, 27)
        Me.UltraGroupBox1.Name = "UltraGroupBox1"
        Me.UltraGroupBox1.Size = New System.Drawing.Size(301, 79)
        Me.UltraGroupBox1.TabIndex = 9
        Me.UltraGroupBox1.Text = "Dates:"
        '
        'UltraLabel3
        '
        Appearance36.TextHAlignAsString = "Right"
        Appearance36.TextVAlignAsString = "Middle"
        Me.UltraLabel3.Appearance = Appearance36
        Me.UltraLabel3.Location = New System.Drawing.Point(313, 49)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(49, 13)
        Me.UltraLabel3.TabIndex = 10
        Me.UltraLabel3.Text = "Invoice"
        '
        'UltraLabel4
        '
        Appearance35.TextHAlignAsString = "Right"
        Appearance35.TextVAlignAsString = "Middle"
        Me.UltraLabel4.Appearance = Appearance35
        Me.UltraLabel4.Location = New System.Drawing.Point(335, 78)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(27, 13)
        Me.UltraLabel4.TabIndex = 11
        Me.UltraLabel4.Text = "PO"
        '
        'UltraTextEditor_InvoiceNumber
        '
        Me.UltraTextEditor_InvoiceNumber.Location = New System.Drawing.Point(368, 43)
        Me.UltraTextEditor_InvoiceNumber.Name = "UltraTextEditor_InvoiceNumber"
        Me.UltraTextEditor_InvoiceNumber.Size = New System.Drawing.Size(102, 23)
        Me.UltraTextEditor_InvoiceNumber.TabIndex = 12
        '
        'UltraTextEditor_PONumber
        '
        Me.UltraTextEditor_PONumber.Location = New System.Drawing.Point(368, 74)
        Me.UltraTextEditor_PONumber.Name = "UltraTextEditor_PONumber"
        Me.UltraTextEditor_PONumber.Size = New System.Drawing.Size(102, 23)
        Me.UltraTextEditor_PONumber.TabIndex = 13
        '
        'UltraLabel5
        '
        Appearance49.TextHAlignAsString = "Right"
        Appearance49.TextVAlignAsString = "Middle"
        Me.UltraLabel5.Appearance = Appearance49
        Me.UltraLabel5.Location = New System.Drawing.Point(474, 44)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(53, 19)
        Me.UltraLabel5.TabIndex = 15
        Me.UltraLabel5.Text = "Vendor"
        '
        'UltraLabel6
        '
        Appearance50.TextHAlignAsString = "Right"
        Appearance50.TextVAlignAsString = "Middle"
        Me.UltraLabel6.Appearance = Appearance50
        Me.UltraLabel6.Location = New System.Drawing.Point(484, 74)
        Me.UltraLabel6.Name = "UltraLabel6"
        Me.UltraLabel6.Size = New System.Drawing.Size(43, 19)
        Me.UltraLabel6.TabIndex = 16
        Me.UltraLabel6.Text = "Store"
        '
        'UltraLabel8
        '
        Appearance51.TextHAlignAsString = "Right"
        Appearance51.TextVAlignAsString = "Middle"
        Me.UltraLabel8.Appearance = Appearance51
        Me.UltraLabel8.Location = New System.Drawing.Point(679, 44)
        Me.UltraLabel8.Name = "UltraLabel8"
        Me.UltraLabel8.Size = New System.Drawing.Size(37, 19)
        Me.UltraLabel8.TabIndex = 18
        Me.UltraLabel8.Text = "Error"
        '
        'UltraCombo_Store
        '
        Me.UltraCombo_Store.AllowNull = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraCombo_Store.CheckedListSettings.CheckStateMember = ""
        Appearance39.BackColor = System.Drawing.SystemColors.Window
        Appearance39.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraCombo_Store.DisplayLayout.Appearance = Appearance39
        UltraGridColumn15.Header.VisiblePosition = 1
        UltraGridColumn15.Hidden = True
        UltraGridColumn16.Header.Caption = "Store Name"
        UltraGridColumn16.Header.VisiblePosition = 0
        UltraGridColumn17.Header.VisiblePosition = 2
        UltraGridColumn17.Hidden = True
        UltraGridColumn18.Header.VisiblePosition = 3
        UltraGridColumn18.Hidden = True
        UltraGridColumn19.Header.VisiblePosition = 4
        UltraGridColumn19.Hidden = True
        UltraGridColumn20.Header.VisiblePosition = 5
        UltraGridColumn20.Hidden = True
        UltraGridColumn21.Header.VisiblePosition = 6
        UltraGridColumn21.Hidden = True
        UltraGridColumn22.Header.VisiblePosition = 7
        UltraGridColumn22.Hidden = True
        UltraGridBand2.Columns.AddRange(New Object() {UltraGridColumn15, UltraGridColumn16, UltraGridColumn17, UltraGridColumn18, UltraGridColumn19, UltraGridColumn20, UltraGridColumn21, UltraGridColumn22})
        Me.UltraCombo_Store.DisplayLayout.BandsSerializer.Add(UltraGridBand2)
        Me.UltraCombo_Store.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraCombo_Store.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance40.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance40.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance40.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraCombo_Store.DisplayLayout.GroupByBox.Appearance = Appearance40
        Appearance41.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraCombo_Store.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance41
        Me.UltraCombo_Store.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance42.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance42.BackColor2 = System.Drawing.SystemColors.Control
        Appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance42.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraCombo_Store.DisplayLayout.GroupByBox.PromptAppearance = Appearance42
        Me.UltraCombo_Store.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraCombo_Store.DisplayLayout.MaxRowScrollRegions = 1
        Appearance43.BackColor = System.Drawing.SystemColors.Window
        Appearance43.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraCombo_Store.DisplayLayout.Override.ActiveCellAppearance = Appearance43
        Appearance44.BackColor = System.Drawing.SystemColors.Highlight
        Appearance44.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraCombo_Store.DisplayLayout.Override.ActiveRowAppearance = Appearance44
        Me.UltraCombo_Store.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraCombo_Store.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance45.BackColor = System.Drawing.SystemColors.Window
        Me.UltraCombo_Store.DisplayLayout.Override.CardAreaAppearance = Appearance45
        Appearance46.BorderColor = System.Drawing.Color.Silver
        Appearance46.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraCombo_Store.DisplayLayout.Override.CellAppearance = Appearance46
        Me.UltraCombo_Store.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraCombo_Store.DisplayLayout.Override.CellPadding = 0
        Appearance47.BackColor = System.Drawing.SystemColors.Control
        Appearance47.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance47.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance47.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance47.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraCombo_Store.DisplayLayout.Override.GroupByRowAppearance = Appearance47
        Appearance48.TextHAlignAsString = "Left"
        Me.UltraCombo_Store.DisplayLayout.Override.HeaderAppearance = Appearance48
        Me.UltraCombo_Store.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraCombo_Store.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance59.BackColor = System.Drawing.SystemColors.Window
        Appearance59.BorderColor = System.Drawing.Color.Silver
        Me.UltraCombo_Store.DisplayLayout.Override.RowAppearance = Appearance59
        Me.UltraCombo_Store.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance60.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraCombo_Store.DisplayLayout.Override.TemplateAddRowAppearance = Appearance60
        Me.UltraCombo_Store.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraCombo_Store.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraCombo_Store.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraCombo_Store.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList
        Me.UltraCombo_Store.Location = New System.Drawing.Point(533, 74)
        Me.UltraCombo_Store.Name = "UltraCombo_Store"
        Me.UltraCombo_Store.Size = New System.Drawing.Size(139, 24)
        Me.UltraCombo_Store.TabIndex = 19
        '
        'UltraCombo_ErrorMessage
        '
        Me.UltraCombo_ErrorMessage.CheckedListSettings.CheckStateMember = ""
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Appearance11.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraCombo_ErrorMessage.DisplayLayout.Appearance = Appearance11
        UltraGridColumn23.Header.VisiblePosition = 0
        UltraGridColumn23.Hidden = True
        UltraGridColumn24.Header.Caption = "Error Message"
        UltraGridColumn24.Header.VisiblePosition = 1
        UltraGridColumn25.Header.VisiblePosition = 2
        UltraGridColumn25.Hidden = True
        UltraGridBand3.Columns.AddRange(New Object() {UltraGridColumn23, UltraGridColumn24, UltraGridColumn25})
        Me.UltraCombo_ErrorMessage.DisplayLayout.BandsSerializer.Add(UltraGridBand3)
        Me.UltraCombo_ErrorMessage.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraCombo_ErrorMessage.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance12.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance12.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance12.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraCombo_ErrorMessage.DisplayLayout.GroupByBox.Appearance = Appearance12
        Appearance14.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraCombo_ErrorMessage.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance14
        Me.UltraCombo_ErrorMessage.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance13.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance13.BackColor2 = System.Drawing.SystemColors.Control
        Appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance13.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraCombo_ErrorMessage.DisplayLayout.GroupByBox.PromptAppearance = Appearance13
        Me.UltraCombo_ErrorMessage.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraCombo_ErrorMessage.DisplayLayout.MaxRowScrollRegions = 1
        Appearance19.BackColor = System.Drawing.SystemColors.Window
        Appearance19.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraCombo_ErrorMessage.DisplayLayout.Override.ActiveCellAppearance = Appearance19
        Appearance15.BackColor = System.Drawing.SystemColors.Highlight
        Appearance15.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraCombo_ErrorMessage.DisplayLayout.Override.ActiveRowAppearance = Appearance15
        Me.UltraCombo_ErrorMessage.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraCombo_ErrorMessage.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance22.BackColor = System.Drawing.SystemColors.Window
        Me.UltraCombo_ErrorMessage.DisplayLayout.Override.CardAreaAppearance = Appearance22
        Appearance18.BorderColor = System.Drawing.Color.Silver
        Appearance18.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraCombo_ErrorMessage.DisplayLayout.Override.CellAppearance = Appearance18
        Me.UltraCombo_ErrorMessage.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraCombo_ErrorMessage.DisplayLayout.Override.CellPadding = 0
        Appearance16.BackColor = System.Drawing.SystemColors.Control
        Appearance16.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance16.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance16.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraCombo_ErrorMessage.DisplayLayout.Override.GroupByRowAppearance = Appearance16
        Appearance17.TextHAlignAsString = "Left"
        Me.UltraCombo_ErrorMessage.DisplayLayout.Override.HeaderAppearance = Appearance17
        Me.UltraCombo_ErrorMessage.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraCombo_ErrorMessage.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance20.BackColor = System.Drawing.SystemColors.Window
        Appearance20.BorderColor = System.Drawing.Color.Silver
        Me.UltraCombo_ErrorMessage.DisplayLayout.Override.RowAppearance = Appearance20
        Me.UltraCombo_ErrorMessage.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance21.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraCombo_ErrorMessage.DisplayLayout.Override.TemplateAddRowAppearance = Appearance21
        Me.UltraCombo_ErrorMessage.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraCombo_ErrorMessage.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraCombo_ErrorMessage.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraCombo_ErrorMessage.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList
        Me.UltraCombo_ErrorMessage.Location = New System.Drawing.Point(722, 43)
        Me.UltraCombo_ErrorMessage.Name = "UltraCombo_ErrorMessage"
        Me.UltraCombo_ErrorMessage.Size = New System.Drawing.Size(278, 24)
        Me.UltraCombo_ErrorMessage.TabIndex = 21
        '
        'UltraCombo_Status
        '
        Me.UltraCombo_Status.CheckedListSettings.CheckStateMember = ""
        Appearance23.BackColor = System.Drawing.SystemColors.Window
        Appearance23.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraCombo_Status.DisplayLayout.Appearance = Appearance23
        Me.UltraCombo_Status.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraCombo_Status.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance24.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance24.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance24.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraCombo_Status.DisplayLayout.GroupByBox.Appearance = Appearance24
        Appearance25.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraCombo_Status.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance25
        Me.UltraCombo_Status.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance26.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance26.BackColor2 = System.Drawing.SystemColors.Control
        Appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance26.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraCombo_Status.DisplayLayout.GroupByBox.PromptAppearance = Appearance26
        Me.UltraCombo_Status.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraCombo_Status.DisplayLayout.MaxRowScrollRegions = 1
        Appearance27.BackColor = System.Drawing.SystemColors.Window
        Appearance27.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraCombo_Status.DisplayLayout.Override.ActiveCellAppearance = Appearance27
        Appearance28.BackColor = System.Drawing.SystemColors.Highlight
        Appearance28.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraCombo_Status.DisplayLayout.Override.ActiveRowAppearance = Appearance28
        Me.UltraCombo_Status.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraCombo_Status.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance29.BackColor = System.Drawing.SystemColors.Window
        Me.UltraCombo_Status.DisplayLayout.Override.CardAreaAppearance = Appearance29
        Appearance30.BorderColor = System.Drawing.Color.Silver
        Appearance30.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraCombo_Status.DisplayLayout.Override.CellAppearance = Appearance30
        Me.UltraCombo_Status.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraCombo_Status.DisplayLayout.Override.CellPadding = 0
        Appearance31.BackColor = System.Drawing.SystemColors.Control
        Appearance31.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance31.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance31.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraCombo_Status.DisplayLayout.Override.GroupByRowAppearance = Appearance31
        Appearance32.TextHAlignAsString = "Left"
        Me.UltraCombo_Status.DisplayLayout.Override.HeaderAppearance = Appearance32
        Me.UltraCombo_Status.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraCombo_Status.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance33.BackColor = System.Drawing.SystemColors.Window
        Appearance33.BorderColor = System.Drawing.Color.Silver
        Me.UltraCombo_Status.DisplayLayout.Override.RowAppearance = Appearance33
        Me.UltraCombo_Status.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance34.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraCombo_Status.DisplayLayout.Override.TemplateAddRowAppearance = Appearance34
        Me.UltraCombo_Status.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraCombo_Status.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraCombo_Status.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraCombo_Status.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList
        Me.UltraCombo_Status.Location = New System.Drawing.Point(722, 74)
        Me.UltraCombo_Status.Name = "UltraCombo_Status"
        Me.UltraCombo_Status.Size = New System.Drawing.Size(130, 24)
        Me.UltraCombo_Status.TabIndex = 23
        '
        'UltraLabel7
        '
        Appearance52.TextHAlignAsString = "Right"
        Appearance52.TextVAlignAsString = "Middle"
        Me.UltraLabel7.Appearance = Appearance52
        Me.UltraLabel7.Location = New System.Drawing.Point(671, 75)
        Me.UltraLabel7.Name = "UltraLabel7"
        Me.UltraLabel7.Size = New System.Drawing.Size(45, 19)
        Me.UltraLabel7.TabIndex = 22
        Me.UltraLabel7.Text = "Status"
        '
        'UltraButton_Search
        '
        Me.UltraButton_Search.Location = New System.Drawing.Point(865, 75)
        Me.UltraButton_Search.Name = "UltraButton_Search"
        Me.UltraButton_Search.Size = New System.Drawing.Size(135, 23)
        Me.UltraButton_Search.TabIndex = 24
        Me.UltraButton_Search.Text = "Search"
        '
        'UltraToolTipManager1
        '
        Me.UltraToolTipManager1.ContainingControl = Me
        Me.UltraToolTipManager1.DisplayStyle = Infragistics.Win.ToolTipDisplayStyle.Office2007
        '
        'UltraCombo_Vendors
        '
        Me.UltraCombo_Vendors.CheckedListSettings.CheckStateMember = ""
        UltraGridColumn26.Header.VisiblePosition = 0
        UltraGridColumn26.Hidden = True
        UltraGridColumn27.Header.Caption = "Vendor Name"
        UltraGridColumn27.Header.VisiblePosition = 1
        UltraGridColumn28.Header.VisiblePosition = 2
        UltraGridColumn28.Hidden = True
        UltraGridBand4.Columns.AddRange(New Object() {UltraGridColumn26, UltraGridColumn27, UltraGridColumn28})
        Me.UltraCombo_Vendors.DisplayLayout.BandsSerializer.Add(UltraGridBand4)
        Me.UltraCombo_Vendors.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList
        Me.UltraCombo_Vendors.Location = New System.Drawing.Point(533, 43)
        Me.UltraCombo_Vendors.Name = "UltraCombo_Vendors"
        Me.UltraCombo_Vendors.Size = New System.Drawing.Size(138, 24)
        Me.UltraCombo_Vendors.TabIndex = 25
        '
        'CheckBox_Archived
        '
        Me.CheckBox_Archived.AutoSize = True
        Me.CheckBox_Archived.Location = New System.Drawing.Point(865, 105)
        Me.CheckBox_Archived.Name = "CheckBox_Archived"
        Me.CheckBox_Archived.Size = New System.Drawing.Size(133, 19)
        Me.CheckBox_Archived.TabIndex = 26
        Me.CheckBox_Archived.Text = "Show Archived Only"
        Me.CheckBox_Archived.UseVisualStyleBackColor = True
        '
        'BackgroundWorker1
        '
        Me.BackgroundWorker1.WorkerReportsProgress = True
        Me.BackgroundWorker1.WorkerSupportsCancellation = True
        '
        'EInvoicing_ViewInvoices
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1012, 643)
        Me.Controls.Add(Me.CheckBox_Archived)
        Me.Controls.Add(Me.UltraTextEditor_PONumber)
        Me.Controls.Add(Me.UltraCombo_Vendors)
        Me.Controls.Add(Me.UltraButton_Search)
        Me.Controls.Add(Me.UltraCombo_Store)
        Me.Controls.Add(Me.UltraCombo_Status)
        Me.Controls.Add(Me.UltraLabel6)
        Me.Controls.Add(Me.UltraLabel7)
        Me.Controls.Add(Me.UltraCombo_ErrorMessage)
        Me.Controls.Add(Me.UltraLabel8)
        Me.Controls.Add(Me.UltraLabel4)
        Me.Controls.Add(Me.UltraTextEditor_InvoiceNumber)
        Me.Controls.Add(Me.UltraLabel3)
        Me.Controls.Add(Me.UltraLabel5)
        Me.Controls.Add(Me.UltraGroupBox1)
        Me.Controls.Add(Me.EInvoicing_ViewInvoices_Fill_Panel)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.Name = "EInvoicing_ViewInvoices"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "View Imported E-Invoices"
        CType(Me.UltraGrid_EInvoices, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.EInvoicing_ViewInvoices_Fill_Panel.ResumeLayout(False)
        Me.EInvoicing_ViewInvoices_Fill_Panel.PerformLayout()
        Me.Panel_Reparse.ResumeLayout(False)
        Me.Panel_Reparse.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.UltraDateTimeEditor_InvoiceStart, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDateTimeEditor_InvoiceEnd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDateTimeEditor_ImportEnd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDateTimeEditor_ImportStart, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraGroupBox1.ResumeLayout(False)
        Me.UltraGroupBox1.PerformLayout()
        CType(Me.UltraTextEditor_InvoiceNumber, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraTextEditor_PONumber, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraCombo_Store, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraCombo_ErrorMessage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraCombo_Status, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraCombo_Vendors, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UltraGrid_EInvoices As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ReparseMenu_MenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewErrorInformationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReportsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SuspendedEInvoicesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UnmatchedPOsForEInvoicingVendorsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InfoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ChangePOInformationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ArchiveSelectedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ForceLoadToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EInvoicing_ViewInvoices_Fill_Panel As System.Windows.Forms.Panel
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraDateTimeEditor_InvoiceStart As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraDateTimeEditor_InvoiceEnd As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraDateTimeEditor_ImportEnd As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraDateTimeEditor_ImportStart As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraGroupBox1 As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraTextEditor_InvoiceNumber As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents UltraTextEditor_PONumber As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel6 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel8 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraCombo_Store As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents UltraCombo_ErrorMessage As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents UltraCombo_Status As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents UltraLabel7 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraButton_Search As Infragistics.Win.Misc.UltraButton
    Friend WithEvents UltraToolTipManager1 As Infragistics.Win.UltraWinToolTip.UltraToolTipManager
    Friend WithEvents UltraCombo_Vendors As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents CheckBox_Archived As System.Windows.Forms.CheckBox
    Friend WithEvents Panel_Reparse As System.Windows.Forms.Panel
    Friend WithEvents btnCancelReparse As System.Windows.Forms.Button
    Friend WithEvents ListView_Reparse As System.Windows.Forms.ListView
    Friend WithEvents column_Invoice As System.Windows.Forms.ColumnHeader
    Friend WithEvents column_PO As System.Windows.Forms.ColumnHeader
    Friend WithEvents column_Store As System.Windows.Forms.ColumnHeader
    Friend WithEvents column_Vendor As System.Windows.Forms.ColumnHeader
    Friend WithEvents column_Status As System.Windows.Forms.ColumnHeader
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents column_EinvoiceID As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel_Info As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel_Count As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
End Class
