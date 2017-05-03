<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmReceivingList
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
    Private IsInitializing As Boolean
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cmdSelectAll As System.Windows.Forms.Button
	Public WithEvents optAll As System.Windows.Forms.RadioButton
	Public WithEvents optReceived As System.Windows.Forms.RadioButton
	Public WithEvents optNotReceived As System.Windows.Forms.RadioButton
	Public WithEvents fraDisplay As System.Windows.Forms.GroupBox
	Public WithEvents cmdReceiveDelete As System.Windows.Forms.Button
	Public WithEvents cmdReceive As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents imgIcons As System.Windows.Forms.ImageList
    Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReceivingList))
        Dim UltraDataColumn1 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("OrderLineID")
        Dim UltraDataColumn2 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Line")
        Dim UltraDataColumn3 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Identifier")
        Dim UltraDataColumn4 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Description")
        Dim UltraDataColumn5 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Ordered")
        Dim UltraDataColumn6 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Unit")
        Dim UltraDataColumn7 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Received")
        Dim UltraDataColumn8 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Weight")
        Dim UltraDataColumn9 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("CostedByWeight")
        Dim UltraDataColumn10 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("isPackageUnit")
        Dim UltraDataColumn11 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("WeightPerPackage")
        Dim UltraDataColumn12 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("eInvoice")
        Dim UltraDataColumn13 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("VendorItemId")
        Dim UltraDataColumn14 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Brand")
        Dim UltraDataColumn15 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Code")
        Dim UltraDataColumn16 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Cost")
        Dim UltraDataColumn17 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("PkgDesc")
        Dim UltraDataColumn18 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("eInvoiceWeight")
        Dim UltraDataColumn19 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("eInvoiceCost")
        Dim UltraDataColumn20 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("eInvoice Unit")
        Dim UltraDataColumn21 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Shipped")
        Dim UltraDataColumn22 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Ship Weight")
        Dim UltraDataColumn23 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("VendorItemDescription")
        Dim UltraDataColumn24 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("CatchweightRequired")
        Dim UltraDataColumn25 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("PD2")
        Dim UltraDataColumn26 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Refused")
        Dim UltraDataColumn27 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Line")
        Dim UltraDataColumn28 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("eInvoice")
        Dim UltraDataColumn29 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Identifier")
        Dim UltraDataColumn30 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Brand")
        Dim UltraDataColumn31 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("VendorItemId")
        Dim UltraDataColumn32 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Description")
        Dim UltraDataColumn33 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Ordered")
        Dim UltraDataColumn34 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("eInvoiceQty")
        Dim UltraDataColumn35 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("eInvoice Unit")
        Dim UltraDataColumn36 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Weight")
        Dim UltraDataColumn37 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("eInvoiceWeight")
        Dim UltraDataColumn38 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Cost")
        Dim UltraDataColumn39 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("eInvoiceItemException")
        Dim UltraDataColumn40 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("CostedByWeight")
        Dim UltraDataColumn41 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Shipped")
        Dim UltraDataColumn42 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Ship Weight")
        Dim UltraDataColumn43 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("PkgDesc")
        Dim UltraDataColumn44 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Unit")
        Dim UltraDataColumn45 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("eInvoice Package")
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Recordset", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Line")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("eInvoice")
        Dim Appearance56 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance57 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Brand")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorItemId")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Description")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Ordered")
        Dim Appearance58 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance59 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("eInvoiceQty", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, False)
        Dim Appearance60 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance61 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("eInvoice Unit")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Weight")
        Dim Appearance62 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance63 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("eInvoiceWeight")
        Dim Appearance64 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance65 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Cost")
        Dim Appearance66 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance67 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("eInvoiceItemException")
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CostedByWeight")
        Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Shipped")
        Dim Appearance68 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance69 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn16 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Ship Weight")
        Dim Appearance70 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance71 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn17 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PkgDesc")
        Dim Appearance72 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance73 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn18 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Unit")
        Dim UltraGridColumn19 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("eInvoice Package")
        Dim Appearance74 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance75 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand2 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Recordset", -1)
        Dim UltraGridColumn20 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderLineID")
        Dim UltraGridColumn21 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Line")
        Dim UltraGridColumn22 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn23 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Description")
        Dim UltraGridColumn24 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Ordered")
        Dim Appearance76 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance77 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn25 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Unit")
        Dim UltraGridColumn26 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Received")
        Dim Appearance78 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance79 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance80 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn27 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Weight")
        Dim Appearance81 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance82 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn28 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CostedByWeight")
        Dim UltraGridColumn29 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("isPackageUnit")
        Dim UltraGridColumn30 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WeightPerPackage")
        Dim Appearance83 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance84 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn31 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("eInvoice", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim Appearance85 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance86 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn32 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorItemId")
        Dim UltraGridColumn33 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Brand")
        Dim UltraGridColumn34 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Code")
        Dim UltraGridColumn35 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Cost")
        Dim Appearance87 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance88 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn36 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PkgDesc")
        Dim Appearance89 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance90 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn37 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("eInvoiceWeight")
        Dim Appearance91 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance92 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn38 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("eInvoiceCost")
        Dim Appearance93 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance94 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn39 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("eInvoice Unit")
        Dim UltraGridColumn40 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Shipped")
        Dim Appearance95 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance96 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn41 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Ship Weight")
        Dim Appearance97 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance98 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn42 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorItemDescription")
        Dim UltraGridColumn43 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CatchweightRequired")
        Dim UltraGridColumn44 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PD2")
        Dim UltraGridColumn45 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Refused")
        Dim Appearance99 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance100 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance30 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance33 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance29 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance28 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance31 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance32 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdSelectAll = New System.Windows.Forms.Button()
        Me.cmdReceiveDelete = New System.Windows.Forms.Button()
        Me.cmdReceive = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdCloseOrder = New System.Windows.Forms.Button()
        Me.fraDisplay = New System.Windows.Forms.GroupBox()
        Me.cmdNOIDNORDReport = New System.Windows.Forms.Button()
        Me.optExceptions = New System.Windows.Forms.RadioButton()
        Me.optAll = New System.Windows.Forms.RadioButton()
        Me.optReceived = New System.Windows.Forms.RadioButton()
        Me.optNotReceived = New System.Windows.Forms.RadioButton()
        Me.imgIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.udsReceivingList = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBoxBrand = New System.Windows.Forms.TextBox()
        Me.TextBoxVIN = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me._txtField_1 = New System.Windows.Forms.TextBox()
        Me._lblLabel_1 = New System.Windows.Forms.Label()
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.filterGroup = New System.Windows.Forms.GroupBox()
        Me.cmdRemoveFilter = New System.Windows.Forms.Button()
        Me.cmdFilter = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtOrderHeader_ID = New System.Windows.Forms.TextBox()
        Me.txtSubteamName = New System.Windows.Forms.TextBox()
        Me.txtStoreName = New System.Windows.Forms.TextBox()
        Me.txtTotalOrderCost = New System.Windows.Forms.TextBox()
        Me.txtExpectedDate = New System.Windows.Forms.TextBox()
        Me.udsNOIDNORD = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.cmdReparseEInvoice = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.grdNOIDNORD = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.grdRL = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.grpReceiveCommands = New System.Windows.Forms.GroupBox()
        Me.pbrStatus = New System.Windows.Forms.ProgressBar()
        Me.lblReparseStatus = New System.Windows.Forms.Label()
        Me.ReparseWorker = New System.ComponentModel.BackgroundWorker()
        Me.chkShowVendorDescriptions = New System.Windows.Forms.CheckBox()
        Me.uddReasonCode = New Infragistics.Win.UltraWinGrid.UltraDropDown()
        Me.partialShippmentCheck = New System.Windows.Forms.CheckBox()
        Me.fraDisplay.SuspendLayout()
        CType(Me.udsReceivingList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.filterGroup.SuspendLayout()
        CType(Me.udsNOIDNORD, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.grdNOIDNORD, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grdRL, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpReceiveCommands.SuspendLayout()
        CType(Me.uddReasonCode, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdSelectAll
        '
        Me.cmdSelectAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSelectAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectAll.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelectAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectAll.Location = New System.Drawing.Point(7, 10)
        Me.cmdSelectAll.Name = "cmdSelectAll"
        Me.cmdSelectAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectAll.Size = New System.Drawing.Size(48, 41)
        Me.cmdSelectAll.TabIndex = 8
        Me.cmdSelectAll.Tag = "Select"
        Me.ToolTip1.SetToolTip(Me.cmdSelectAll, "Select All")
        Me.cmdSelectAll.UseVisualStyleBackColor = True
        '
        'cmdReceiveDelete
        '
        Me.cmdReceiveDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdReceiveDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReceiveDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReceiveDelete.Enabled = False
        Me.cmdReceiveDelete.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReceiveDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReceiveDelete.Image = CType(resources.GetObject("cmdReceiveDelete.Image"), System.Drawing.Image)
        Me.cmdReceiveDelete.Location = New System.Drawing.Point(171, 10)
        Me.cmdReceiveDelete.Name = "cmdReceiveDelete"
        Me.cmdReceiveDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReceiveDelete.Size = New System.Drawing.Size(48, 41)
        Me.cmdReceiveDelete.TabIndex = 2
        Me.cmdReceiveDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReceiveDelete, "Delete Receiving Info")
        Me.cmdReceiveDelete.UseVisualStyleBackColor = True
        '
        'cmdReceive
        '
        Me.cmdReceive.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdReceive.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReceive.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReceive.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReceive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReceive.Image = CType(resources.GetObject("cmdReceive.Image"), System.Drawing.Image)
        Me.cmdReceive.Location = New System.Drawing.Point(62, 10)
        Me.cmdReceive.Name = "cmdReceive"
        Me.cmdReceive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReceive.Size = New System.Drawing.Size(48, 41)
        Me.cmdReceive.TabIndex = 1
        Me.cmdReceive.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReceive, "Receive Order")
        Me.cmdReceive.UseVisualStyleBackColor = True
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
        Me.cmdExit.Location = New System.Drawing.Point(226, 10)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(48, 41)
        Me.cmdExit.TabIndex = 3
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = True
        '
        'cmdCloseOrder
        '
        Me.cmdCloseOrder.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCloseOrder.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCloseOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCloseOrder.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCloseOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCloseOrder.Image = CType(resources.GetObject("cmdCloseOrder.Image"), System.Drawing.Image)
        Me.cmdCloseOrder.Location = New System.Drawing.Point(117, 10)
        Me.cmdCloseOrder.Name = "cmdCloseOrder"
        Me.cmdCloseOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCloseOrder.Size = New System.Drawing.Size(48, 41)
        Me.cmdCloseOrder.TabIndex = 32
        Me.cmdCloseOrder.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdCloseOrder, "Close Order")
        Me.cmdCloseOrder.UseVisualStyleBackColor = True
        '
        'fraDisplay
        '
        Me.fraDisplay.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.fraDisplay.BackColor = System.Drawing.SystemColors.Control
        Me.fraDisplay.Controls.Add(Me.cmdNOIDNORDReport)
        Me.fraDisplay.Controls.Add(Me.optExceptions)
        Me.fraDisplay.Controls.Add(Me.optAll)
        Me.fraDisplay.Controls.Add(Me.optReceived)
        Me.fraDisplay.Controls.Add(Me.optNotReceived)
        Me.fraDisplay.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDisplay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDisplay.Location = New System.Drawing.Point(10, 627)
        Me.fraDisplay.Name = "fraDisplay"
        Me.fraDisplay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDisplay.Size = New System.Drawing.Size(631, 55)
        Me.fraDisplay.TabIndex = 4
        Me.fraDisplay.TabStop = False
        Me.fraDisplay.Text = "Display Options"
        '
        'cmdNOIDNORDReport
        '
        Me.cmdNOIDNORDReport.Image = CType(resources.GetObject("cmdNOIDNORDReport.Image"), System.Drawing.Image)
        Me.cmdNOIDNORDReport.Location = New System.Drawing.Point(412, 10)
        Me.cmdNOIDNORDReport.Name = "cmdNOIDNORDReport"
        Me.cmdNOIDNORDReport.Size = New System.Drawing.Size(213, 41)
        Me.cmdNOIDNORDReport.TabIndex = 30
        Me.cmdNOIDNORDReport.Text = "eInvoice Exception Report"
        Me.cmdNOIDNORDReport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdNOIDNORDReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.cmdNOIDNORDReport.UseVisualStyleBackColor = True
        '
        'optExceptions
        '
        Me.optExceptions.BackColor = System.Drawing.SystemColors.Control
        Me.optExceptions.Cursor = System.Windows.Forms.Cursors.Default
        Me.optExceptions.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optExceptions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optExceptions.Location = New System.Drawing.Point(269, 18)
        Me.optExceptions.Name = "optExceptions"
        Me.optExceptions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optExceptions.Size = New System.Drawing.Size(159, 28)
        Me.optExceptions.TabIndex = 8
        Me.optExceptions.TabStop = True
        Me.optExceptions.Text = "EInvoicing Exceptions"
        Me.optExceptions.UseVisualStyleBackColor = True
        '
        'optAll
        '
        Me.optAll.BackColor = System.Drawing.SystemColors.Control
        Me.optAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAll.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAll.Location = New System.Drawing.Point(215, 21)
        Me.optAll.Name = "optAll"
        Me.optAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAll.Size = New System.Drawing.Size(48, 23)
        Me.optAll.TabIndex = 7
        Me.optAll.TabStop = True
        Me.optAll.Text = "All"
        Me.optAll.UseVisualStyleBackColor = True
        '
        'optReceived
        '
        Me.optReceived.BackColor = System.Drawing.SystemColors.Control
        Me.optReceived.Cursor = System.Windows.Forms.Cursors.Default
        Me.optReceived.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optReceived.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optReceived.Location = New System.Drawing.Point(120, 21)
        Me.optReceived.Name = "optReceived"
        Me.optReceived.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optReceived.Size = New System.Drawing.Size(85, 23)
        Me.optReceived.TabIndex = 6
        Me.optReceived.TabStop = True
        Me.optReceived.Text = "Received"
        Me.optReceived.UseVisualStyleBackColor = True
        '
        'optNotReceived
        '
        Me.optNotReceived.BackColor = System.Drawing.SystemColors.Control
        Me.optNotReceived.Checked = True
        Me.optNotReceived.Cursor = System.Windows.Forms.Cursors.Default
        Me.optNotReceived.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optNotReceived.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optNotReceived.Location = New System.Drawing.Point(9, 21)
        Me.optNotReceived.Name = "optNotReceived"
        Me.optNotReceived.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optNotReceived.Size = New System.Drawing.Size(104, 23)
        Me.optNotReceived.TabIndex = 5
        Me.optNotReceived.TabStop = True
        Me.optNotReceived.Text = "Not Received"
        Me.optNotReceived.UseVisualStyleBackColor = True
        '
        'imgIcons
        '
        Me.imgIcons.ImageStream = CType(resources.GetObject("imgIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgIcons.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgIcons.Images.SetKeyName(0, "All")
        Me.imgIcons.Images.SetKeyName(1, "None")
        '
        'udsReceivingList
        '
        Me.udsReceivingList.Band.AllowAdd = Infragistics.Win.DefaultableBoolean.[False]
        Me.udsReceivingList.Band.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        UltraDataColumn1.DataType = GetType(Integer)
        UltraDataColumn1.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn2.DataType = GetType(Integer)
        UltraDataColumn2.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn3.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn4.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn5.DataType = GetType(Decimal)
        UltraDataColumn5.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn6.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn7.DataType = GetType(Decimal)
        UltraDataColumn7.ReadOnly = Infragistics.Win.DefaultableBoolean.[False]
        UltraDataColumn8.DataType = GetType(Decimal)
        UltraDataColumn8.ReadOnly = Infragistics.Win.DefaultableBoolean.[False]
        UltraDataColumn9.DataType = GetType(Boolean)
        UltraDataColumn9.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn10.DataType = GetType(Boolean)
        UltraDataColumn10.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn12.DataType = GetType(Decimal)
        UltraDataColumn16.DataType = GetType(Decimal)
        UltraDataColumn20.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn22.DataType = GetType(Decimal)
        UltraDataColumn22.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn23.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn24.DataType = GetType(Boolean)
        UltraDataColumn26.DataType = GetType(Decimal)
        Me.udsReceivingList.Band.Columns.AddRange(New Object() {UltraDataColumn1, UltraDataColumn2, UltraDataColumn3, UltraDataColumn4, UltraDataColumn5, UltraDataColumn6, UltraDataColumn7, UltraDataColumn8, UltraDataColumn9, UltraDataColumn10, UltraDataColumn11, UltraDataColumn12, UltraDataColumn13, UltraDataColumn14, UltraDataColumn15, UltraDataColumn16, UltraDataColumn17, UltraDataColumn18, UltraDataColumn19, UltraDataColumn20, UltraDataColumn21, UltraDataColumn22, UltraDataColumn23, UltraDataColumn24, UltraDataColumn25, UltraDataColumn26})
        Me.udsReceivingList.Band.Key = "Recordset"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(52, 83)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Brand :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextBoxBrand
        '
        Me.TextBoxBrand.AcceptsReturn = True
        Me.TextBoxBrand.BackColor = System.Drawing.SystemColors.Window
        Me.TextBoxBrand.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBoxBrand.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxBrand.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me.TextBoxBrand, CType(2, Short))
        Me.TextBoxBrand.Location = New System.Drawing.Point(102, 80)
        Me.TextBoxBrand.MaxLength = 60
        Me.TextBoxBrand.Name = "TextBoxBrand"
        Me.TextBoxBrand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBoxBrand.Size = New System.Drawing.Size(200, 20)
        Me.TextBoxBrand.TabIndex = 15
        Me.TextBoxBrand.Tag = "String"
        '
        'TextBoxVIN
        '
        Me.TextBoxVIN.AcceptsReturn = True
        Me.TextBoxVIN.BackColor = System.Drawing.SystemColors.Window
        Me.TextBoxVIN.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBoxVIN.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxVIN.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me.TextBoxVIN, CType(1, Short))
        Me.TextBoxVIN.Location = New System.Drawing.Point(102, 55)
        Me.TextBoxVIN.MaxLength = 13
        Me.TextBoxVIN.Name = "TextBoxVIN"
        Me.TextBoxVIN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBoxVIN.Size = New System.Drawing.Size(200, 20)
        Me.TextBoxVIN.TabIndex = 13
        Me.TextBoxVIN.Tag = "String"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(8, 58)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(96, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Vendor Item ID:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_txtField_1
        '
        Me._txtField_1.AcceptsReturn = True
        Me._txtField_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_1, CType(0, Short))
        Me._txtField_1.Location = New System.Drawing.Point(102, 31)
        Me._txtField_1.MaxLength = 13
        Me._txtField_1.Name = "_txtField_1"
        Me._txtField_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_1.Size = New System.Drawing.Size(200, 20)
        Me._txtField_1.TabIndex = 10
        Me._txtField_1.Tag = "String"
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.AutoSize = True
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_1.Location = New System.Drawing.Point(36, 34)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(65, 13)
        Me._lblLabel_1.TabIndex = 11
        Me._lblLabel_1.Text = "Identifier :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtField
        '
        '
        'filterGroup
        '
        Me.filterGroup.Controls.Add(Me.cmdRemoveFilter)
        Me.filterGroup.Controls.Add(Me.cmdFilter)
        Me.filterGroup.Controls.Add(Me._txtField_1)
        Me.filterGroup.Controls.Add(Me.Label2)
        Me.filterGroup.Controls.Add(Me._lblLabel_1)
        Me.filterGroup.Controls.Add(Me.TextBoxBrand)
        Me.filterGroup.Controls.Add(Me.Label1)
        Me.filterGroup.Controls.Add(Me.TextBoxVIN)
        Me.filterGroup.Location = New System.Drawing.Point(10, 68)
        Me.filterGroup.Name = "filterGroup"
        Me.filterGroup.Size = New System.Drawing.Size(452, 120)
        Me.filterGroup.TabIndex = 16
        Me.filterGroup.TabStop = False
        Me.filterGroup.Text = "Filter"
        '
        'cmdRemoveFilter
        '
        Me.cmdRemoveFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemoveFilter.Image = CType(resources.GetObject("cmdRemoveFilter.Image"), System.Drawing.Image)
        Me.cmdRemoveFilter.Location = New System.Drawing.Point(308, 64)
        Me.cmdRemoveFilter.Name = "cmdRemoveFilter"
        Me.cmdRemoveFilter.Size = New System.Drawing.Size(126, 32)
        Me.cmdRemoveFilter.TabIndex = 17
        Me.cmdRemoveFilter.Text = "Remove Filter"
        Me.cmdRemoveFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdRemoveFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.cmdRemoveFilter.UseVisualStyleBackColor = True
        '
        'cmdFilter
        '
        Me.cmdFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFilter.Image = CType(resources.GetObject("cmdFilter.Image"), System.Drawing.Image)
        Me.cmdFilter.Location = New System.Drawing.Point(308, 31)
        Me.cmdFilter.Name = "cmdFilter"
        Me.cmdFilter.Size = New System.Drawing.Size(126, 32)
        Me.cmdFilter.TabIndex = 16
        Me.cmdFilter.Text = "Apply Filter"
        Me.cmdFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.cmdFilter.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(23, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(74, 13)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "PO Number:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(225, 42)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 13)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "Subteam:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(246, 14)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(43, 13)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "Store:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(474, 14)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(96, 13)
        Me.Label6.TabIndex = 20
        Me.Label6.Text = "Total Order Cost:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(10, 42)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(87, 13)
        Me.Label7.TabIndex = 21
        Me.Label7.Text = "Expected Date:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtOrderHeader_ID
        '
        Me.txtOrderHeader_ID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrderHeader_ID.Location = New System.Drawing.Point(103, 11)
        Me.txtOrderHeader_ID.Name = "txtOrderHeader_ID"
        Me.txtOrderHeader_ID.ReadOnly = True
        Me.txtOrderHeader_ID.Size = New System.Drawing.Size(116, 20)
        Me.txtOrderHeader_ID.TabIndex = 22
        '
        'txtSubteamName
        '
        Me.txtSubteamName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubteamName.Location = New System.Drawing.Point(292, 39)
        Me.txtSubteamName.Name = "txtSubteamName"
        Me.txtSubteamName.ReadOnly = True
        Me.txtSubteamName.Size = New System.Drawing.Size(170, 20)
        Me.txtSubteamName.TabIndex = 23
        '
        'txtStoreName
        '
        Me.txtStoreName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStoreName.Location = New System.Drawing.Point(292, 11)
        Me.txtStoreName.Name = "txtStoreName"
        Me.txtStoreName.ReadOnly = True
        Me.txtStoreName.Size = New System.Drawing.Size(170, 20)
        Me.txtStoreName.TabIndex = 24
        '
        'txtTotalOrderCost
        '
        Me.txtTotalOrderCost.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalOrderCost.Location = New System.Drawing.Point(576, 11)
        Me.txtTotalOrderCost.Name = "txtTotalOrderCost"
        Me.txtTotalOrderCost.ReadOnly = True
        Me.txtTotalOrderCost.Size = New System.Drawing.Size(151, 20)
        Me.txtTotalOrderCost.TabIndex = 25
        '
        'txtExpectedDate
        '
        Me.txtExpectedDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExpectedDate.Location = New System.Drawing.Point(103, 39)
        Me.txtExpectedDate.Name = "txtExpectedDate"
        Me.txtExpectedDate.ReadOnly = True
        Me.txtExpectedDate.Size = New System.Drawing.Size(116, 20)
        Me.txtExpectedDate.TabIndex = 26
        '
        'udsNOIDNORD
        '
        Me.udsNOIDNORD.Band.AllowAdd = Infragistics.Win.DefaultableBoolean.[False]
        Me.udsNOIDNORD.Band.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        UltraDataColumn27.DataType = GetType(Integer)
        UltraDataColumn27.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn28.DataType = GetType(Decimal)
        UltraDataColumn28.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn29.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn30.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn31.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn32.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn33.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn34.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn35.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn36.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn37.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn38.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn39.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn40.DataType = GetType(Boolean)
        UltraDataColumn40.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn41.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn42.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        Me.udsNOIDNORD.Band.Columns.AddRange(New Object() {UltraDataColumn27, UltraDataColumn28, UltraDataColumn29, UltraDataColumn30, UltraDataColumn31, UltraDataColumn32, UltraDataColumn33, UltraDataColumn34, UltraDataColumn35, UltraDataColumn36, UltraDataColumn37, UltraDataColumn38, UltraDataColumn39, UltraDataColumn40, UltraDataColumn41, UltraDataColumn42, UltraDataColumn43, UltraDataColumn44, UltraDataColumn45})
        Me.udsNOIDNORD.Band.Key = "Recordset"
        '
        'cmdReparseEInvoice
        '
        Me.cmdReparseEInvoice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReparseEInvoice.Image = CType(resources.GetObject("cmdReparseEInvoice.Image"), System.Drawing.Image)
        Me.cmdReparseEInvoice.Location = New System.Drawing.Point(477, 99)
        Me.cmdReparseEInvoice.Name = "cmdReparseEInvoice"
        Me.cmdReparseEInvoice.Size = New System.Drawing.Size(152, 32)
        Me.cmdReparseEInvoice.TabIndex = 28
        Me.cmdReparseEInvoice.Text = "Reparse EInvoice"
        Me.cmdReparseEInvoice.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdReparseEInvoice.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.cmdReparseEInvoice.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.grdNOIDNORD)
        Me.Panel1.Controls.Add(Me.grdRL)
        Me.Panel1.Location = New System.Drawing.Point(10, 194)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(951, 432)
        Me.Panel1.TabIndex = 29
        '
        'grdNOIDNORD
        '
        Me.grdNOIDNORD.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdNOIDNORD.DataSource = Me.udsNOIDNORD
        Me.grdNOIDNORD.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 14
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Appearance56.TextHAlignAsString = "Right"
        UltraGridColumn2.CellAppearance = Appearance56
        Appearance57.TextHAlignAsString = "Right"
        UltraGridColumn2.Header.Appearance = Appearance57
        UltraGridColumn2.Header.VisiblePosition = 18
        UltraGridColumn2.Width = 38
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn3.Header.VisiblePosition = 1
        UltraGridColumn3.Width = 14
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn4.Header.VisiblePosition = 2
        UltraGridColumn4.Width = 14
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn5.Header.VisiblePosition = 3
        UltraGridColumn5.Width = 14
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn6.Header.VisiblePosition = 4
        UltraGridColumn6.Width = 14
        UltraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Appearance58.TextHAlignAsString = "Right"
        UltraGridColumn7.CellAppearance = Appearance58
        Appearance59.TextHAlignAsString = "Right"
        UltraGridColumn7.Header.Appearance = Appearance59
        UltraGridColumn7.Header.VisiblePosition = 5
        UltraGridColumn7.Width = 14
        UltraGridColumn8.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Appearance60.TextHAlignAsString = "Right"
        UltraGridColumn8.CellAppearance = Appearance60
        Appearance61.TextHAlignAsString = "Right"
        UltraGridColumn8.Header.Appearance = Appearance61
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Width = 221
        UltraGridColumn9.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn9.Header.VisiblePosition = 9
        UltraGridColumn9.Width = 24
        UltraGridColumn10.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Appearance62.TextHAlignAsString = "Right"
        UltraGridColumn10.CellAppearance = Appearance62
        Appearance63.TextHAlignAsString = "Right"
        UltraGridColumn10.Header.Appearance = Appearance63
        UltraGridColumn10.Header.VisiblePosition = 11
        UltraGridColumn10.Width = 77
        UltraGridColumn11.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Appearance64.TextHAlignAsString = "Right"
        UltraGridColumn11.CellAppearance = Appearance64
        Appearance65.TextHAlignAsString = "Right"
        UltraGridColumn11.Header.Appearance = Appearance65
        UltraGridColumn11.Header.VisiblePosition = 10
        UltraGridColumn11.Width = 48
        UltraGridColumn12.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Appearance66.TextHAlignAsString = "Right"
        UltraGridColumn12.CellAppearance = Appearance66
        Appearance67.TextHAlignAsString = "Right"
        UltraGridColumn12.Header.Appearance = Appearance67
        UltraGridColumn12.Header.VisiblePosition = 12
        UltraGridColumn12.Width = 35
        UltraGridColumn13.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn13.Header.VisiblePosition = 17
        UltraGridColumn13.Width = 45
        UltraGridColumn14.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn14.Header.VisiblePosition = 14
        UltraGridColumn14.Width = 48
        UltraGridColumn15.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Appearance68.TextHAlignAsString = "Right"
        UltraGridColumn15.CellAppearance = Appearance68
        Appearance69.TextHAlignAsString = "Right"
        UltraGridColumn15.Header.Appearance = Appearance69
        UltraGridColumn15.Header.VisiblePosition = 15
        UltraGridColumn15.Width = 36
        UltraGridColumn16.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Appearance70.TextHAlignAsString = "Right"
        UltraGridColumn16.CellAppearance = Appearance70
        Appearance71.TextHAlignAsString = "Right"
        UltraGridColumn16.Header.Appearance = Appearance71
        UltraGridColumn16.Header.VisiblePosition = 16
        UltraGridColumn16.Width = 30
        UltraGridColumn17.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Appearance72.TextHAlignAsString = "Right"
        UltraGridColumn17.CellAppearance = Appearance72
        Appearance73.TextHAlignAsString = "Right"
        UltraGridColumn17.Header.Appearance = Appearance73
        UltraGridColumn17.Header.VisiblePosition = 13
        UltraGridColumn17.Width = 73
        UltraGridColumn18.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn18.Header.VisiblePosition = 6
        UltraGridColumn18.Width = 14
        Appearance74.TextHAlignAsString = "Right"
        UltraGridColumn19.CellAppearance = Appearance74
        Appearance75.TextHAlignAsString = "Right"
        UltraGridColumn19.Header.Appearance = Appearance75
        UltraGridColumn19.Header.VisiblePosition = 8
        UltraGridColumn19.Width = 157
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15, UltraGridColumn16, UltraGridColumn17, UltraGridColumn18, UltraGridColumn19})
        Me.grdNOIDNORD.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.grdNOIDNORD.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.grdNOIDNORD.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.[False]
        Appearance7.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance7.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance7.BorderColor = System.Drawing.SystemColors.Window
        Me.grdNOIDNORD.DisplayLayout.GroupByBox.Appearance = Appearance7
        Appearance8.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grdNOIDNORD.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance8
        Me.grdNOIDNORD.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.grdNOIDNORD.DisplayLayout.GroupByBox.Hidden = True
        Appearance9.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance9.BackColor2 = System.Drawing.SystemColors.Control
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grdNOIDNORD.DisplayLayout.GroupByBox.PromptAppearance = Appearance9
        Me.grdNOIDNORD.DisplayLayout.MaxBandDepth = 1
        Me.grdNOIDNORD.DisplayLayout.MaxColScrollRegions = 1
        Me.grdNOIDNORD.DisplayLayout.MaxRowScrollRegions = 1
        Me.grdNOIDNORD.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
        Me.grdNOIDNORD.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed
        Me.grdNOIDNORD.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        Me.grdNOIDNORD.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None
        Me.grdNOIDNORD.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        Me.grdNOIDNORD.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.grdNOIDNORD.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance10.BackColor = System.Drawing.SystemColors.Window
        Me.grdNOIDNORD.DisplayLayout.Override.CardAreaAppearance = Appearance10
        Appearance11.BorderColor = System.Drawing.Color.Silver
        Appearance11.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.grdNOIDNORD.DisplayLayout.Override.CellAppearance = Appearance11
        Me.grdNOIDNORD.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.grdNOIDNORD.DisplayLayout.Override.CellPadding = 0
        Appearance12.BackColor = System.Drawing.SystemColors.Control
        Appearance12.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance12.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance12.BorderColor = System.Drawing.SystemColors.Window
        Me.grdNOIDNORD.DisplayLayout.Override.GroupByRowAppearance = Appearance12
        Appearance13.TextHAlignAsString = "Left"
        Me.grdNOIDNORD.DisplayLayout.Override.HeaderAppearance = Appearance13
        Me.grdNOIDNORD.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.grdNOIDNORD.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Me.grdNOIDNORD.DisplayLayout.Override.MaxSelectedCells = 1
        Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
        Me.grdNOIDNORD.DisplayLayout.Override.RowAlternateAppearance = Appearance14
        Me.grdNOIDNORD.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLight
        Me.grdNOIDNORD.DisplayLayout.Override.TemplateAddRowAppearance = Appearance15
        Me.grdNOIDNORD.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.grdNOIDNORD.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.grdNOIDNORD.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.grdNOIDNORD.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdNOIDNORD.Location = New System.Drawing.Point(0, 252)
        Me.grdNOIDNORD.Name = "grdNOIDNORD"
        Me.grdNOIDNORD.Size = New System.Drawing.Size(951, 179)
        Me.grdNOIDNORD.TabIndex = 36
        Me.grdNOIDNORD.Text = "eInvoice Item Exception"
        '
        'grdRL
        '
        Me.grdRL.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdRL.DataSource = Me.udsReceivingList
        Me.grdRL.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn20.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn20.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn20.Header.VisiblePosition = 0
        UltraGridColumn20.TabStop = False
        UltraGridColumn20.Width = 14
        UltraGridColumn21.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn21.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn21.Header.VisiblePosition = 1
        UltraGridColumn21.TabStop = False
        UltraGridColumn21.Width = 16
        UltraGridColumn22.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn22.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn22.Header.VisiblePosition = 2
        UltraGridColumn22.TabStop = False
        UltraGridColumn22.Width = 36
        UltraGridColumn23.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn23.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn23.Header.VisiblePosition = 6
        UltraGridColumn23.TabStop = False
        UltraGridColumn23.Width = 14
        UltraGridColumn24.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn24.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance76.TextHAlignAsString = "Right"
        UltraGridColumn24.CellAppearance = Appearance76
        UltraGridColumn24.Format = "###.##"
        Appearance77.TextHAlignAsString = "Right"
        UltraGridColumn24.Header.Appearance = Appearance77
        UltraGridColumn24.Header.VisiblePosition = 7
        UltraGridColumn24.MaskInput = ""
        UltraGridColumn24.TabStop = False
        UltraGridColumn24.Width = 14
        UltraGridColumn25.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn25.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn25.Header.VisiblePosition = 8
        UltraGridColumn25.TabStop = False
        UltraGridColumn25.Width = 18
        UltraGridColumn26.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Appearance78.TextHAlignAsString = "Right"
        UltraGridColumn26.CellAppearance = Appearance78
        Appearance79.TextHAlignAsString = "Right"
        UltraGridColumn26.CellButtonAppearance = Appearance79
        UltraGridColumn26.Format = "##0.##"
        Appearance80.TextHAlignAsString = "Right"
        UltraGridColumn26.Header.Appearance = Appearance80
        UltraGridColumn26.Header.VisiblePosition = 13
        UltraGridColumn26.MaskInput = "{double:4.2}"
        UltraGridColumn26.Width = 60
        UltraGridColumn27.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Appearance81.TextHAlignAsString = "Right"
        UltraGridColumn27.CellAppearance = Appearance81
        UltraGridColumn27.Format = "#####.##"
        Appearance82.TextHAlignAsString = "Right"
        UltraGridColumn27.Header.Appearance = Appearance82
        UltraGridColumn27.Header.VisiblePosition = 15
        UltraGridColumn27.MaskInput = "{double:5.2}"
        UltraGridColumn27.Width = 69
        UltraGridColumn28.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn28.Header.VisiblePosition = 18
        UltraGridColumn28.Hidden = True
        UltraGridColumn28.TabStop = False
        UltraGridColumn29.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn29.Header.VisiblePosition = 19
        UltraGridColumn29.Hidden = True
        UltraGridColumn29.TabStop = False
        UltraGridColumn30.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Appearance83.TextHAlignAsString = "Right"
        UltraGridColumn30.CellAppearance = Appearance83
        Appearance84.TextHAlignAsString = "Right"
        UltraGridColumn30.Header.Appearance = Appearance84
        UltraGridColumn30.Header.VisiblePosition = 20
        UltraGridColumn30.Hidden = True
        UltraGridColumn30.TabStop = False
        UltraGridColumn31.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn31.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance85.TextHAlignAsString = "Right"
        UltraGridColumn31.CellAppearance = Appearance85
        UltraGridColumn31.Format = "###.##"
        Appearance86.TextHAlignAsString = "Right"
        UltraGridColumn31.Header.Appearance = Appearance86
        UltraGridColumn31.Header.Caption = "eInvoice Qty"
        UltraGridColumn31.Header.VisiblePosition = 9
        UltraGridColumn31.Width = 92
        UltraGridColumn32.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn32.Header.VisiblePosition = 4
        UltraGridColumn32.Width = 14
        UltraGridColumn33.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn33.Header.VisiblePosition = 3
        UltraGridColumn33.Width = 14
        UltraGridColumn34.Header.VisiblePosition = 24
        UltraGridColumn34.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
        UltraGridColumn34.Width = 44
        UltraGridColumn35.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Appearance87.TextHAlignAsString = "Right"
        UltraGridColumn35.CellAppearance = Appearance87
        Appearance88.TextHAlignAsString = "Right"
        UltraGridColumn35.Header.Appearance = Appearance88
        UltraGridColumn35.Header.VisiblePosition = 16
        UltraGridColumn35.Width = 54
        UltraGridColumn36.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Appearance89.TextHAlignAsString = "Right"
        UltraGridColumn36.CellAppearance = Appearance89
        Appearance90.TextHAlignAsString = "Right"
        UltraGridColumn36.Header.Appearance = Appearance90
        UltraGridColumn36.Header.VisiblePosition = 17
        UltraGridColumn36.Width = 72
        UltraGridColumn37.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn37.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance91.TextHAlignAsString = "Right"
        UltraGridColumn37.CellAppearance = Appearance91
        Appearance92.TextHAlignAsString = "Right"
        UltraGridColumn37.Header.Appearance = Appearance92
        UltraGridColumn37.Header.VisiblePosition = 11
        UltraGridColumn37.Width = 96
        Appearance93.TextHAlignAsString = "Right"
        UltraGridColumn38.CellAppearance = Appearance93
        Appearance94.TextHAlignAsString = "Right"
        UltraGridColumn38.Header.Appearance = Appearance94
        UltraGridColumn38.Header.Caption = "eInvoice Cost"
        UltraGridColumn38.Header.VisiblePosition = 12
        UltraGridColumn38.Width = 85
        UltraGridColumn39.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn39.Header.VisiblePosition = 10
        UltraGridColumn39.Width = 82
        UltraGridColumn40.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn40.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance95.TextHAlignAsString = "Right"
        UltraGridColumn40.CellAppearance = Appearance95
        Appearance96.TextHAlignAsString = "Right"
        UltraGridColumn40.Header.Appearance = Appearance96
        UltraGridColumn40.Header.VisiblePosition = 21
        UltraGridColumn40.Width = 60
        UltraGridColumn41.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Appearance97.TextHAlignAsString = "Right"
        UltraGridColumn41.CellAppearance = Appearance97
        Appearance98.TextHAlignAsString = "Right"
        UltraGridColumn41.Header.Appearance = Appearance98
        UltraGridColumn41.Header.VisiblePosition = 22
        UltraGridColumn41.Width = 76
        UltraGridColumn42.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn42.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn42.Header.Caption = "Vendor Description"
        UltraGridColumn42.Header.VisiblePosition = 5
        UltraGridColumn42.Hidden = True
        UltraGridColumn42.Width = 94
        UltraGridColumn43.Header.VisiblePosition = 23
        UltraGridColumn43.Hidden = True
        UltraGridColumn43.Width = 94
        UltraGridColumn44.Header.VisiblePosition = 25
        UltraGridColumn44.Hidden = True
        UltraGridColumn44.Width = 94
        UltraGridColumn45.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn45.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance99.TextHAlignAsString = "Right"
        UltraGridColumn45.CellAppearance = Appearance99
        UltraGridColumn45.Format = "###.##"
        Appearance100.TextHAlignAsString = "Right"
        UltraGridColumn45.Header.Appearance = Appearance100
        UltraGridColumn45.Header.VisiblePosition = 14
        UltraGridColumn45.Hidden = True
        UltraGridColumn45.Width = 94
        UltraGridBand2.Columns.AddRange(New Object() {UltraGridColumn20, UltraGridColumn21, UltraGridColumn22, UltraGridColumn23, UltraGridColumn24, UltraGridColumn25, UltraGridColumn26, UltraGridColumn27, UltraGridColumn28, UltraGridColumn29, UltraGridColumn30, UltraGridColumn31, UltraGridColumn32, UltraGridColumn33, UltraGridColumn34, UltraGridColumn35, UltraGridColumn36, UltraGridColumn37, UltraGridColumn38, UltraGridColumn39, UltraGridColumn40, UltraGridColumn41, UltraGridColumn42, UltraGridColumn43, UltraGridColumn44, UltraGridColumn45})
        UltraGridBand2.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.[True]
        UltraGridBand2.Expandable = False
        UltraGridBand2.GroupHeadersVisible = False
        Me.grdRL.DisplayLayout.BandsSerializer.Add(UltraGridBand2)
        Me.grdRL.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.grdRL.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.[False]
        Appearance19.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance19.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance19.BorderColor = System.Drawing.SystemColors.Window
        Me.grdRL.DisplayLayout.GroupByBox.Appearance = Appearance19
        Appearance20.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grdRL.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance20
        Me.grdRL.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.grdRL.DisplayLayout.GroupByBox.Hidden = True
        Appearance21.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance21.BackColor2 = System.Drawing.SystemColors.Control
        Appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance21.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grdRL.DisplayLayout.GroupByBox.PromptAppearance = Appearance21
        Me.grdRL.DisplayLayout.MaxBandDepth = 1
        Me.grdRL.DisplayLayout.MaxColScrollRegions = 1
        Me.grdRL.DisplayLayout.MaxRowScrollRegions = 1
        Me.grdRL.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
        Me.grdRL.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed
        Me.grdRL.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        Me.grdRL.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None
        Me.grdRL.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        Me.grdRL.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.grdRL.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance22.BackColor = System.Drawing.SystemColors.Window
        Me.grdRL.DisplayLayout.Override.CardAreaAppearance = Appearance22
        Appearance23.BorderColor = System.Drawing.Color.Silver
        Appearance23.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.grdRL.DisplayLayout.Override.CellAppearance = Appearance23
        Me.grdRL.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.grdRL.DisplayLayout.Override.CellPadding = 0
        Appearance24.BackColor = System.Drawing.SystemColors.Control
        Appearance24.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance24.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance24.BorderColor = System.Drawing.SystemColors.Window
        Me.grdRL.DisplayLayout.Override.GroupByRowAppearance = Appearance24
        Appearance25.TextHAlignAsString = "Left"
        Me.grdRL.DisplayLayout.Override.HeaderAppearance = Appearance25
        Me.grdRL.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.grdRL.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Me.grdRL.DisplayLayout.Override.MaxSelectedCells = 1
        Appearance26.BackColor = System.Drawing.SystemColors.ControlLight
        Me.grdRL.DisplayLayout.Override.RowAlternateAppearance = Appearance26
        Me.grdRL.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Appearance27.BackColor = System.Drawing.SystemColors.ControlLight
        Me.grdRL.DisplayLayout.Override.TemplateAddRowAppearance = Appearance27
        Me.grdRL.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.grdRL.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.grdRL.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.grdRL.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdRL.Location = New System.Drawing.Point(0, 0)
        Me.grdRL.Name = "grdRL"
        Me.grdRL.Size = New System.Drawing.Size(951, 256)
        Me.grdRL.TabIndex = 35
        Me.grdRL.Text = "Receiving List"
        '
        'grpReceiveCommands
        '
        Me.grpReceiveCommands.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpReceiveCommands.Controls.Add(Me.cmdCloseOrder)
        Me.grpReceiveCommands.Controls.Add(Me.cmdSelectAll)
        Me.grpReceiveCommands.Controls.Add(Me.cmdExit)
        Me.grpReceiveCommands.Controls.Add(Me.cmdReceive)
        Me.grpReceiveCommands.Controls.Add(Me.cmdReceiveDelete)
        Me.grpReceiveCommands.Location = New System.Drawing.Point(680, 627)
        Me.grpReceiveCommands.Name = "grpReceiveCommands"
        Me.grpReceiveCommands.Size = New System.Drawing.Size(281, 55)
        Me.grpReceiveCommands.TabIndex = 33
        Me.grpReceiveCommands.TabStop = False
        '
        'pbrStatus
        '
        Me.pbrStatus.Location = New System.Drawing.Point(477, 160)
        Me.pbrStatus.Name = "pbrStatus"
        Me.pbrStatus.Size = New System.Drawing.Size(223, 23)
        Me.pbrStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.pbrStatus.TabIndex = 34
        Me.pbrStatus.Visible = False
        '
        'lblReparseStatus
        '
        Me.lblReparseStatus.AutoSize = True
        Me.lblReparseStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReparseStatus.Location = New System.Drawing.Point(478, 141)
        Me.lblReparseStatus.Name = "lblReparseStatus"
        Me.lblReparseStatus.Size = New System.Drawing.Size(108, 13)
        Me.lblReparseStatus.TabIndex = 35
        Me.lblReparseStatus.Text = "Reparsing eInvoice..."
        Me.lblReparseStatus.Visible = False
        '
        'ReparseWorker
        '
        '
        'chkShowVendorDescriptions
        '
        Me.chkShowVendorDescriptions.AutoSize = True
        Me.chkShowVendorDescriptions.Location = New System.Drawing.Point(477, 42)
        Me.chkShowVendorDescriptions.Name = "chkShowVendorDescriptions"
        Me.chkShowVendorDescriptions.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkShowVendorDescriptions.Size = New System.Drawing.Size(185, 17)
        Me.chkShowVendorDescriptions.TabIndex = 36
        Me.chkShowVendorDescriptions.Text = "Display Vendor Descriptions"
        Me.chkShowVendorDescriptions.UseVisualStyleBackColor = True
        '
        'uddReasonCode
        '
        Appearance4.BackColor = System.Drawing.SystemColors.Window
        Appearance4.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.uddReasonCode.DisplayLayout.Appearance = Appearance4
        Me.uddReasonCode.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.uddReasonCode.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance5.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance5.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance5.BorderColor = System.Drawing.SystemColors.Window
        Me.uddReasonCode.DisplayLayout.GroupByBox.Appearance = Appearance5
        Appearance16.ForeColor = System.Drawing.SystemColors.GrayText
        Me.uddReasonCode.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance16
        Me.uddReasonCode.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance6.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance6.BackColor2 = System.Drawing.SystemColors.Control
        Appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance6.ForeColor = System.Drawing.SystemColors.GrayText
        Me.uddReasonCode.DisplayLayout.GroupByBox.PromptAppearance = Appearance6
        Me.uddReasonCode.DisplayLayout.MaxColScrollRegions = 1
        Me.uddReasonCode.DisplayLayout.MaxRowScrollRegions = 1
        Appearance30.BackColor = System.Drawing.SystemColors.Window
        Appearance30.ForeColor = System.Drawing.SystemColors.ControlText
        Me.uddReasonCode.DisplayLayout.Override.ActiveCellAppearance = Appearance30
        Appearance17.BackColor = System.Drawing.SystemColors.Highlight
        Appearance17.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.uddReasonCode.DisplayLayout.Override.ActiveRowAppearance = Appearance17
        Me.uddReasonCode.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.uddReasonCode.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance33.BackColor = System.Drawing.SystemColors.Window
        Me.uddReasonCode.DisplayLayout.Override.CardAreaAppearance = Appearance33
        Appearance29.BorderColor = System.Drawing.Color.Silver
        Appearance29.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.uddReasonCode.DisplayLayout.Override.CellAppearance = Appearance29
        Me.uddReasonCode.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.uddReasonCode.DisplayLayout.Override.CellPadding = 0
        Appearance18.BackColor = System.Drawing.SystemColors.Control
        Appearance18.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance18.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance18.BorderColor = System.Drawing.SystemColors.Window
        Me.uddReasonCode.DisplayLayout.Override.GroupByRowAppearance = Appearance18
        Appearance28.TextHAlignAsString = "Left"
        Me.uddReasonCode.DisplayLayout.Override.HeaderAppearance = Appearance28
        Me.uddReasonCode.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.uddReasonCode.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance31.BackColor = System.Drawing.SystemColors.Window
        Appearance31.BorderColor = System.Drawing.Color.Silver
        Me.uddReasonCode.DisplayLayout.Override.RowAppearance = Appearance31
        Me.uddReasonCode.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance32.BackColor = System.Drawing.SystemColors.ControlLight
        Me.uddReasonCode.DisplayLayout.Override.TemplateAddRowAppearance = Appearance32
        Me.uddReasonCode.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.uddReasonCode.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.uddReasonCode.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.uddReasonCode.Location = New System.Drawing.Point(706, 149)
        Me.uddReasonCode.Name = "uddReasonCode"
        Me.uddReasonCode.Size = New System.Drawing.Size(255, 34)
        Me.uddReasonCode.TabIndex = 37
        Me.uddReasonCode.Visible = False
        '
        'partialShippmentCheck
        '
        Me.partialShippmentCheck.AutoSize = True
        Me.partialShippmentCheck.Location = New System.Drawing.Point(477, 68)
        Me.partialShippmentCheck.Name = "partialShippmentCheck"
        Me.partialShippmentCheck.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.partialShippmentCheck.Size = New System.Drawing.Size(118, 17)
        Me.partialShippmentCheck.TabIndex = 38
        Me.partialShippmentCheck.Text = "Partial Shipment"
        Me.partialShippmentCheck.UseVisualStyleBackColor = True
        '
        'frmReceivingList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(984, 698)
        Me.Controls.Add(Me.partialShippmentCheck)
        Me.Controls.Add(Me.uddReasonCode)
        Me.Controls.Add(Me.chkShowVendorDescriptions)
        Me.Controls.Add(Me.lblReparseStatus)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pbrStatus)
        Me.Controls.Add(Me.cmdReparseEInvoice)
        Me.Controls.Add(Me.txtExpectedDate)
        Me.Controls.Add(Me.txtTotalOrderCost)
        Me.Controls.Add(Me.txtStoreName)
        Me.Controls.Add(Me.txtSubteamName)
        Me.Controls.Add(Me.txtOrderHeader_ID)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.filterGroup)
        Me.Controls.Add(Me.fraDisplay)
        Me.Controls.Add(Me.grpReceiveCommands)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(297, 188)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(990, 725)
        Me.Name = "frmReceivingList"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Receiving List"
        Me.fraDisplay.ResumeLayout(False)
        CType(Me.udsReceivingList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        Me.filterGroup.ResumeLayout(False)
        Me.filterGroup.PerformLayout()
        CType(Me.udsNOIDNORD, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        CType(Me.grdNOIDNORD, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grdRL, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpReceiveCommands.ResumeLayout(False)
        CType(Me.uddReasonCode, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents optExceptions As System.Windows.Forms.RadioButton
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents TextBoxBrand As System.Windows.Forms.TextBox
    Public WithEvents TextBoxVIN As System.Windows.Forms.TextBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents _txtField_1 As System.Windows.Forms.TextBox
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
    Friend WithEvents filterGroup As System.Windows.Forms.GroupBox
    Friend WithEvents cmdFilter As System.Windows.Forms.Button
    Friend WithEvents cmdRemoveFilter As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtOrderHeader_ID As System.Windows.Forms.TextBox
    Friend WithEvents txtSubteamName As System.Windows.Forms.TextBox
    Friend WithEvents txtStoreName As System.Windows.Forms.TextBox
    Friend WithEvents txtTotalOrderCost As System.Windows.Forms.TextBox
    Friend WithEvents txtExpectedDate As System.Windows.Forms.TextBox
    Friend WithEvents cmdReparseEInvoice As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents grdRL As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents cmdNOIDNORDReport As System.Windows.Forms.Button
    Public WithEvents cmdCloseOrder As System.Windows.Forms.Button
    Friend WithEvents grpReceiveCommands As System.Windows.Forms.GroupBox
    Friend WithEvents pbrStatus As System.Windows.Forms.ProgressBar
    Friend WithEvents lblReparseStatus As System.Windows.Forms.Label
    Friend WithEvents ReparseWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents chkShowVendorDescriptions As System.Windows.Forms.CheckBox
    Private WithEvents udsReceivingList As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Private WithEvents udsNOIDNORD As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Private WithEvents grdNOIDNORD As Infragistics.Win.UltraWinGrid.UltraGrid
    Private WithEvents uddReasonCode As Infragistics.Win.UltraWinGrid.UltraDropDown
    Friend WithEvents partialShippmentCheck As System.Windows.Forms.CheckBox
#End Region
End Class