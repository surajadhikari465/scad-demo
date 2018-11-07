Option Strict Off
Option Explicit On

Imports WholeFoods.Utility

Imports log4net
Friend Class frmInvoiceReport
    Inherits System.Windows.Forms.Form

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        logger.Debug("cmdReport_Click Entry")
        ' Parameters declaration to call Reporting Services.
        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String

        '--------------------------
        ' Setup Report URL
        '--------------------------

        ' To pass the parameter for the ReceivingCheckList stored procedure.
        Dim optItemId As Integer
        If optIdentifier(0).Checked Then
            optItemId = 1
        Else
            optItemId = 0
        End If


        filename = ConfigurationServices.AppSettings("Region")
        filename = "OrderInvoice"
        sReportURL.Append(filename)

        ' This chooses the region and based on the results points to the correct report.

        ' Report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")


        ' Passing parameters to report.
        If glOrderHeaderID > 0 Then
            sReportURL.Append("&OrderHeader_ID=" & glOrderHeaderID)
        End If

        If optItemId >= 0 Then
            sReportURL.Append("&Item_ID=" & optItemId)
        Else
            sReportURL.Append("&Item_ID=" & optItemId)
        End If

        ' Passing Sorting Parameter as an input to the report.
        Select Case True
            Case optSort(0).Checked
                ' Soring on LineItem(0 as parameter value)
                sReportURL.Append("&OptSort=" & 1)
            Case optSort(1).Checked
                ' Sort on Identifier (1 as parameter Value)
                sReportURL.Append("&OptSort=" & 2)
            Case optSort(2).Checked
                ' Sort on Item_description(2 as parameter Value)
                sReportURL.Append("&OptSort=" & 3)
            Case optSort(3).Checked
                ' Sort on Item_description(2 as parameter Value)
                sReportURL.Append("&OptSort=" & 4)
        End Select

        If Me.GroupByCatCheckBox.Checked Then
            sReportURL.Append("&GroupByCategory=True")
        Else
            sReportURL.Append("&GroupByCategory=False")
        End If

        If glOrderHeaderID > 0 Then
            sReportURL.Append("&OrderHeader_ID_1=" & glOrderHeaderID)
        End If

        If POInvoiceRadioButton.Equals(False) Then
            sReportURL.Append("&POInvoiceOpt=False")
        Else
            sReportURL.Append("&POInvoiceOpt=True")
        End If

        'Debug.Print(POInvoiceRadioButton.ToString)
        'Debug.Print(sReportURL.ToString)
        Call ReportingServicesReport(sReportURL.ToString)

        logger.Debug("cmdReport_Click Exit")



        'Dim rsReport As ADODB.Recordset

        ''-- Get OrderHeader Info
        'GetPurchaseOrderHeader(glOrderHeaderID)

        'gDBReport.BeginTrans()

        'gDBReport.Execute("DELETE * FROM PURCHASEORDERLINE")

        ''-- Put the data in access
        ''    If optInvoice(2).Value Then
        ''        Select Case True
        ''            Case optIdentifier(0): SQLOpenRS gRSRecordset, "EXEC GetOrderItemReceivedListByRecipient " & glOrderHeaderID & ", 1", dbOpenSnapshot, dbSQLPassThrough
        ''            Case optIdentifier(1): SQLOpenRS gRSRecordset, "EXEC GetOrderItemReceivedListByRecipient " & glOrderHeaderID & ", 0", dbOpenSnapshot, dbSQLPassThrough
        ''        End Select
        ''    Else
        'Select Case True
        '          Case optIdentifier(0).Checked : gRSRecordset = SQLOpenRecordSet("EXEC GetOrderItemReceivedList " & glOrderHeaderID & ", 1", dao.RecordsetTypeEnum.dbOpenSnapshot, dao.RecordsetOptionEnum.dbSQLPassThrough)
        '          Case optIdentifier(1).Checked : gRSRecordset = SQLOpenRecordSet("EXEC GetOrderItemReceivedList " & glOrderHeaderID & ", 0", dao.RecordsetTypeEnum.dbOpenSnapshot, dao.RecordsetOptionEnum.dbSQLPassThrough)
        '      End Select
        ''    End If

        'rsReport = New ADODB.Recordset
        'rsReport.Open("PurchaseOrderLine", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdTable)

        'While Not gRSRecordset.EOF

        '	rsReport.AddNew()
        '	rsReport.Fields("LineItem").Value = gRSRecordset.Fields("OrderItem_ID").Value
        '	rsReport.Fields("Identifier").Value = gRSRecordset.Fields("Identifier").Value
        '	rsReport.Fields("Item_Description").Value = gRSRecordset.Fields("Item_Description").Value
        '	rsReport.Fields("Package_Desc").Value = gRSRecordset.Fields("Package_Desc1").Value & "/" & gRSRecordset.Fields("Package_Desc2").Value & " " & gRSRecordset.Fields("Package_Unit").Value
        '	rsReport.Fields("Package_Desc1").Value = gRSRecordset.Fields("Package_Desc1").Value
        '	rsReport.Fields("QuantityOrdered").Value = gRSRecordset.Fields("QuantityReceived").Value
        '	rsReport.Fields("QuantityUnit").Value = gRSRecordset.Fields("Unit_Name").Value
        '	If gRSRecordset.Fields("QuantityReceived").Value <> 0 Then rsReport.Fields("CasePrice").Value = gRSRecordset.Fields("ReceivedItemCost").Value / gRSRecordset.Fields("QuantityReceived").Value
        '	rsReport.Fields("TotalPrice").Value = gRSRecordset.Fields("ReceivedItemCost").Value
        '	rsReport.Fields("TotalHandling").Value = 0
        '	rsReport.Fields("TotalFreight").Value = gRSRecordset.Fields("EstItemFreight").Value
        '	rsReport.Fields("SubTeam_Name").Value = gRSRecordset.Fields("SubTeam_Name").Value
        '	rsReport.Fields("UnitFreight").Value = gRSRecordset.Fields("EstUnitFreight").Value
        '	rsReport.Fields("UnitsReceived").Value = gRSRecordset.Fields("UnitsReceived").Value
        '	rsReport.Fields("SubTeam_No").Value = gRSRecordset.Fields("SubTeam_No").Value
        '	rsReport.Fields("Team_No").Value = gRSRecordset.Fields("Team_No").Value
        '	rsReport.Fields("Category_Name").Value = gRSRecordset.Fields("Category_Name").Value
        '	rsReport.Fields("Brand_Name").Value = gRSRecordset.Fields("Brand_Name").Value
        '	rsReport.Fields("Origin_Name").Value = gRSRecordset.Fields("Origin_Name").Value
        '	rsReport.Fields("Proc_Name").Value = gRSRecordset.Fields("Proc_Name").Value
        '	rsReport.Fields("Lot_no").Value = gRSRecordset.Fields("Lot_no").Value
        '	rsReport.Update()

        '	If gRSRecordset.Fields("DiscountType").Value = 3 And gRSRecordset.Fields("QuantityDiscount").Value > 0 Then

        '		rsReport.AddNew()
        '		rsReport.Fields("LineItem").Value = gRSRecordset.Fields("OrderItem_ID").Value
        '		rsReport.Fields("Identifier").Value = gRSRecordset.Fields("Identifier").Value
        '		rsReport.Fields("Item_Description").Value = gRSRecordset.Fields("Item_Description").Value
        '		rsReport.Fields("Package_Desc").Value = gRSRecordset.Fields("Package_Desc1").Value & "/" & gRSRecordset.Fields("Package_Desc2").Value & " " & gRSRecordset.Fields("Package_Unit").Value
        '		rsReport.Fields("Package_Desc1").Value = gRSRecordset.Fields("Package_Desc1").Value
        '		rsReport.Fields("QuantityOrdered").Value = gRSRecordset.Fields("QuantityDiscount").Value
        '		rsReport.Fields("QuantityUnit").Value = gRSRecordset.Fields("Unit_Name").Value
        '		rsReport.Fields("CasePrice").Value = 0
        '		rsReport.Fields("TotalPrice").Value = 0
        '		rsReport.Fields("TotalHandling").Value = 0
        '		rsReport.Fields("TotalFreight").Value = 0
        '		rsReport.Fields("SubTeam_Name").Value = gRSRecordset.Fields("SubTeam_Name").Value
        '		rsReport.Fields("UnitFreight").Value = 0
        '		rsReport.Fields("UnitsReceived").Value = 0
        '		rsReport.Fields("SubTeam_No").Value = gRSRecordset.Fields("SubTeam_No").Value
        '		rsReport.Fields("Team_No").Value = gRSRecordset.Fields("Team_No").Value
        '		rsReport.Fields("Category_Name").Value = gRSRecordset.Fields("Category_Name").Value
        '		rsReport.Fields("Brand_Name").Value = gRSRecordset.Fields("Brand_Name").Value
        '		rsReport.Fields("Origin_Name").Value = gRSRecordset.Fields("Origin_Name").Value
        '		rsReport.Fields("Proc_Name").Value = gRSRecordset.Fields("Proc_Name").Value
        '		rsReport.Update()
        '	End If

        '	gRSRecordset.MoveNext()

        'End While

        'rsReport.Close()
        ''UPGRADE_NOTE: Object rsReport may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        'rsReport = Nothing

        'gDBReport.CommitTrans()
        'gRSRecordset.Close()
        'gJetFlush.RefreshCache(gDBReport)

        ''-- Sort the records
        'Select Case (True)
        '	Case optSort(0).Checked
        '   Case optSort(1).Checked : SortPurchaseOrder(2)
        '	Case optSort(2).Checked : SortPurchaseOrder(3)
        '	Case optSort(3).Checked : SortPurchaseOrder(4)
        'End Select

        '      ' ###########################################################################
        '      ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        '      ' ###########################################################################
        '      MsgBox("InvoiceReport.vb  cmdReport_Click(): The Crystal OrderInvoice.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)

        '      ''-- Display the report
        '      'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)
        '      '      crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "OrderInvoice.rpt"

        '      'PrintReport(crwReport)

    End Sub

    Private Sub frmInvoiceReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmInvoiceReport_Load Entry")
        CenterForm(Me)
        logger.Debug("frmInvoiceReport_Load Exit")
    End Sub

    Private Sub GroupByCatCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupByCatCheckBox.CheckedChanged
        If Me.GroupByCatCheckBox.Checked Then
            Me.SortByFrame.Enabled = False
        Else
            Me.SortByFrame.Enabled = True
        End If
    End Sub
End Class