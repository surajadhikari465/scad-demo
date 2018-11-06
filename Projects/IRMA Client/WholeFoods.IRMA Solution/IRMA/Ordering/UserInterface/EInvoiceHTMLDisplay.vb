Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports WholeFoods.IRMA.Ordering.BusinessLogic.OrderingFunctions
Imports WholeFoods.IRMA.Ordering.DataAccess
Imports WholeFoods.Utility.DataAccess
Imports log4net


Public Class EInvoiceHTMLDisplay

    Private _headerTemplate As String = String.Empty
    Private _detailTemplate As String = String.Empty
    Private _footerTemplate As String = String.Empty
    Private _chargeTemplate As String = String.Empty
    Private _dsHeaderInfo As DataSet = Nothing
    Private _isNavigating As Boolean = False

    Public EinvoiceId As Integer
    Public StoreNo As Integer

    Private Sub EInvoiceHTMLDisplay_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        WebViewer.Dispose()
        WebViewer = Nothing
    End Sub

    Private Sub EInvoiceHTMLDisplay_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        _headerTemplate = LoadFile(Application.StartupPath & "\EInvoicingHTMLHeader.html")
        _detailTemplate = LoadFile(Application.StartupPath & "\EInvoicingHTMLDetail.html")
        _footerTemplate = LoadFile(Application.StartupPath & "\EInvoicingHTMLFooter.html")
        _chargeTemplate = LoadFile(Application.StartupPath & "\EInvoicingHTMLCharge.html")

        If _headerTemplate = String.Empty Then _headerTemplate = "header template is missing <br/>"
        If _detailTemplate = String.Empty Then _detailTemplate = "detail template is missing <br/>"
        If _footerTemplate = String.Empty Then _footerTemplate = "footer template is missing <br/>"

        GenerateOutput()

    End Sub

    Private Sub GenerateOutput()
        Dim detaillines As DataTable
        Dim charges As DataTable
        Dim counter As Integer = 0
        Dim _output As StringBuilder = New StringBuilder()

        parseHeaderData(Me.EinvoiceId, Me.StoreNo, _headerTemplate, _footerTemplate)

        'create header 
        _output.Append(_headerTemplate)

        'get detail lines
        detaillines = GetDetailLines(Me.EinvoiceId)

        If detaillines.Rows.Count > 0 Then
            For Each dr As DataRow In detaillines.Rows
                _output.Append(parseDetailData(dr, _detailTemplate))

                charges = GetItemChargesInfo(Me.EinvoiceId, dr("line_num"))
                For Each dr2 As DataRow In charges.Rows
                    _output.Append(parseChargesdata(dr2, _chargeTemplate))
                Next

            Next

            charges = GetSummaryChargesInfo(Me.EinvoiceId)
            For Each dr As DataRow In charges.Rows
                _output.Append(parseChargesdata(dr, _chargeTemplate))
            Next

            'add footer
            _output.Append(_footerTemplate)
        Else
            _output.Append("No detail data is available. Check the logs from when this Invoice was imported into IRMA.</br>")
        End If

        'load a blank page.
        WebViewer.Navigate("about:blank")

        ' wait until the browser is ready.
        While ((WebViewer.ReadyState <> WebBrowserReadyState.Complete) And (counter < 5))
            Threading.Thread.Sleep(1000)
            counter = counter + 1
            Application.DoEvents()
        End While

        ' some werid error occurned in the browser control. Ive seen System Error &H80131c25& but dont really know how to handle that.
        WebViewer.Document.Write(_output.ToString())

        ' wait until the browser is ready.
        While ((WebViewer.ReadyState <> WebBrowserReadyState.Complete) And (counter < 5))
            Threading.Thread.Sleep(1000)
            counter = counter + 1
            Application.DoEvents()
        End While
    End Sub

    Private Function LoadFile(ByVal _filename As String) As String
        Dim retval As String
        Dim sr As StreamReader = New StreamReader(_filename)

        retval = sr.ReadToEnd

        sr.Dispose()

        Return retval
    End Function

    Private Sub WebViewer_Navigating(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserNavigatingEventArgs) Handles WebViewer.Navigating
        _isNavigating = True
        FileToolStripMenuItem.Enabled = False
    End Sub

    Private Sub WebViewer_DocumentCompleted(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles WebViewer.DocumentCompleted
        _isNavigating = False
        FileToolStripMenuItem.Enabled = True
    End Sub

    Private Sub WebViewer_Navigated(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserNavigatedEventArgs) Handles WebViewer.Navigated
        _isNavigating = False
        FileToolStripMenuItem.Enabled = True
    End Sub

    Private Function GetDetailLines(ByVal EInvoiceId As Integer) As DataTable
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As ArrayList = New ArrayList()
        Dim currentParam As DBParam
        Dim retval As Boolean = False

        currentParam = New DBParam
        currentParam.Name = "@EinvoiceId"
        currentParam.Value = EInvoiceId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        Return factory.GetStoredProcedureDataSet("EInvoicing_GetEInvoiceDisplay_Items", paramList).Tables(0)

    End Function

    Private Function GetSummaryChargesInfo(ByVal EInvoiceId As Integer) As DataTable
        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        Dim paramList As ArrayList = New ArrayList()
        Dim currentParam As DBParam
        Dim retval As Boolean = False

        currentParam = New DBParam
        currentParam.Name = "@EinvoiceId"
        currentParam.Value = EInvoiceId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        Return factory.GetStoredProcedureDataSet("EInvoicing_GetEInvoiceDisplay_SummaryCharges", paramList).Tables(0)
    End Function

    Private Function GetItemChargesInfo(ByVal EInvoiceId As Integer, ByVal ItemId As Integer) As DataTable
        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        Dim paramList As ArrayList = New ArrayList()
        Dim currentParam As DBParam
        Dim retval As Boolean = False

        currentParam = New DBParam
        currentParam.Name = "@EinvoiceId"
        currentParam.Value = EInvoiceId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@ItemId"
        currentParam.Value = ItemId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        Return factory.GetStoredProcedureDataSet("EInvoicing_GetEInvoiceDisplay_ItemCharges", paramList).Tables(0)
    End Function

    Private Function GetInvoiceMessage(ByVal einvoiceid As Integer) As String

        Dim retval As String = String.Empty
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim ds As DataSet = Nothing

        Dim paramList As ArrayList = New ArrayList()
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@EinvoiceID"
        currentParam.Value = einvoiceid
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)


        ds = factory.GetStoredProcedureDataSet("EInvoicing_GetEInvoiceDisplay_InvoiceMessage", paramList)

        If ds.Tables(0).Rows.Count > 0 Then
            retval = ds.Tables(0).Rows(0)("ElementValue").ToString()
        Else
            retval = String.Empty
        End If
        ds.Dispose()

        Return retval
    End Function

    Private Sub GetHeaderInfo(ByVal einvoiceid As Integer, ByVal storeno As Integer)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As ArrayList = New ArrayList()
        Dim currentParam As DBParam
        Dim retval As Boolean = False

        currentParam = New DBParam
        currentParam.Name = "@EinvoiceID"
        currentParam.Value = einvoiceid
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@StoreNo"
        currentParam.Value = storeno
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        _dsHeaderInfo = factory.GetStoredProcedureDataSet("EInvoicing_GetEInvoiceDisplay_HeaderInfo", paramList)
    End Sub

    Private Function parseDetailData(ByRef _dr As DataRow, ByVal detailTemplate As String) As String
        MapData(detailTemplate, _dr, "Description", "descrip")
        MapData(detailTemplate, _dr, "Brand", "brand")
        MapData(detailTemplate, _dr, "UnitCost", "unit_cost", "Currency")
        MapData(detailTemplate, _dr, "InvQty", "qty_shipped")
        MapData(detailTemplate, _dr, "Weight", "alt_ordering_qty")
        MapData(detailTemplate, _dr, "CaseSize", "item_uom")
        MapData(detailTemplate, _dr, "TotalEaches", "item_qtyper")
        MapData(detailTemplate, _dr, "InvTotal", "ext_cost", "Currency")
        MapData(detailTemplate, _dr, "UPC", "upc")
        MapData(detailTemplate, _dr, "VIN", "vendor_item_num")
        MapData(detailTemplate, _dr, "Size", "case_pack")
        MapData(detailTemplate, _dr, "Charge", "LineItemChargeOrAllowance", "Currency")

        Return detailTemplate
    End Function

    Private Function parseChargesdata(ByRef _dr As DataRow, ByVal chargesTemplate As String) As String
        MapData(chargesTemplate, _dr, "ChargeName", "ChargeName")
        MapData(chargesTemplate, _dr, "ChargeValue", "ChargeValue", "Currency")
        Return chargesTemplate
    End Function

    Private Sub parseHeaderData(ByVal einvoiceid As Integer, ByVal storeno As Integer, ByRef _headerTemplate As String, ByRef _footerTemplate As String)

        GetHeaderInfo(einvoiceid, storeno)

        If _dsHeaderInfo.Tables(0).Rows.Count > 0 Then
            If Not _dsHeaderInfo.Tables(0).Rows(0) Is Nothing Then
                With _dsHeaderInfo.Tables(0)
                    MapData(_headerTemplate, .Rows(0), "InvoiceNumber", "Invoice_Num")
                    MapData(_headerTemplate, .Rows(0), "InvoiceDate", "Invoice_Date", "Date")
                    MapData(_headerTemplate, .Rows(0), "Business_Unit", "Business_Unit")
                    MapData(_headerTemplate, .Rows(0), "CustomerID", "Customer_ID")
                    MapData(_headerTemplate, .Rows(0), "PurchaseOrder", "Purchase_Order")
                    MapData(_headerTemplate, .Rows(0), "Sent_On", "Order_Date", "Date")
                    MapData(_headerTemplate, .Rows(0), "Buyer", "Buyer")

                    ' Format Vendor Data
                    Dim VendorData As String = _
                    String.Format("<SPAN style=""font-weight: bold;"">{0}</SPAN></br>{1}</br>{2}</br>{3}</br>{4}", _
                    .Rows(0)("CompanyName"), _
                    .Rows(0)("V_Address1"), _
                    String.Format("{0}, {1} {2}", .Rows(0)("V_City"), .Rows(0)("V_State"), .Rows(0)("V_ZipCode")), _
                    String.Format("Phone: {0}", .Rows(0)("V_Phone")), _
                    String.Format("Fax: {0}", .Rows(0)("V_Fax")))

                    ' Format ShipTo Data
                    Dim ShipToData As String = _
                    String.Format("<SPAN style=""font-weight: bold;"">{0}</SPAN></br>{1}</br>{2}</br>{3}</br>{4}", _
                    .Rows(0)("Store"), _
                    .Rows(0)("S_Address1"), _
                    String.Format("{0}, {1} {2}", .Rows(0)("S_City"), .Rows(0)("S_State"), .Rows(0)("S_ZipCode")), _
                    String.Format("Phone: {0}", .Rows(0)("S_Phone")), _
                    String.Empty)

                    ' Format Bill To Data
                    Dim BillToData As String = ShipToData

                    MapData(_headerTemplate, "VendorData", VendorData)
                    MapData(_headerTemplate, "ShipToData", ShipToData)
                    MapData(_headerTemplate, "BillToData", BillToData)

                    ' do the footer also since it uses data from the Header record.
                    MapData(_footerTemplate, .Rows(0), "itemcount", "itemcount")
                    MapData(_footerTemplate, .Rows(0), "invoice_amt", "invoice_amt", "Currency")

                    ' if the invoices has message data. display it.
                    MapData(_footerTemplate, "invoicemessage", GetInvoiceMessage(einvoiceid))
                End With
            End If
        Else
            _headerTemplate = "No header data is available.  Check the logs from when this eInvoice was imported into IRMA.</br>"
            _footerTemplate = "No footer data is available.  Check the logs from when this eInvoice was imported into IRMA.</br>"
        End If
    End Sub

    Private Sub MapData(ByRef _template As String, ByVal _data As DataRow, ByVal key As String, ByVal value As String, Optional ByVal format As String = "")
        Dim outputvalue As String = _data(value).ToString()

        If outputvalue.Equals(String.Empty) Then
            _template = _template.Replace(String.Format("%{0}%", key), "&nbsp;")
        Else
            If Not format.Equals(String.Empty) Then
                Select Case format
                    Case "Currency"
                        outputvalue = String.Format("{0:c}", CDbl(outputvalue))
                    Case "Date"
                        outputvalue = DateTime.Parse(outputvalue).ToShortDateString()
                    Case Else
                        outputvalue = String.Format(format, outputvalue)
                End Select
            End If
            _template = _template.Replace(String.Format("%{0}%", key), outputvalue)
        End If
    End Sub

    Private Sub MapData(ByRef _template As String, ByVal key As String, ByVal value As String, Optional ByVal format As String = "")
        Dim outputvalue As String = String.Empty
        If value = String.Empty Then
            outputvalue = "nbsp;"
        Else

            If Not format.Equals(String.Empty) Then
                outputvalue = String.Format(format, value)
            Else
                outputvalue = value
            End If
        End If
        _template = _template.Replace(String.Format("%{0}%", key), value.ToString())
    End Sub

    Private Sub PrintToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem1.Click
        WebViewer.ShowPrintDialog()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
End Class