Option Strict Off
Option Explicit On

Public Class InvoiceDiscrepanciesReport
    Private lVendor_ID As Integer = 0

    Private Sub cmdCompanySearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCompanySearch.Click
        glVendorID = 0

        '-- Set the search type
        giSearchType = iSearchAllVendors

        '-- Open the search form
        frmSearch.Text = "Search for Vendor by Company Name"
        frmSearch.ShowDialog()
        frmSearch.Close()
        frmSearch.Dispose()

        '-- if its not zero, then something was found
        If glVendorID <> 0 Then
            Call GetVendorName(glVendorID)
            lVendor_ID = glVendorID
        End If
    End Sub

    Private Sub GetVendorName(ByRef lVendorID As Integer)
        Dim rsVendor As DAO.Recordset = Nothing

        rsVendor = SQLOpenRecordSet("EXEC GetVendorName " & glVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        Me.txtVendorName.Text = rsVendor.Fields("CompanyName").Value.ToString

        rsVendor.Close()
        rsVendor = Nothing
    End Sub

    Private Sub InvoiceDiscrepanciesReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CenterForm(Me)
        LoadInventoryStore(cmbStore)

        ' Disable the Specific Discrepancies checkboxes
        chkPackDiscrepancies.Enabled = False
        chkQuantityDiscrepancies.Enabled = False
        chkCostDiscrepancies.Enabled = False
        chkNoIDDiscrepancies.Enabled = False

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = -1
        End If
    End Sub

    Private Sub cmbStore_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbStore.KeyPress
        Dim KeyAscii As Integer = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbStore.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click
        Dim sVendor_ID As String = ":isnull=true"
        Dim sStore_No As String = ":isnull=true"
        Dim sStartDate As String = ":isnull=true"
        Dim sEndDate As String = ":isnull=true"
        Dim sOptPaymentDiscrepancies As String = "=false"
        Dim sOptPackDiscrepancies As String = "=false"
        Dim sOptQuantityDiscrepancies As String = "=false"
        Dim sOptCostDiscrepancies As String = "=false"
        Dim sOptNoIDDiscrepancies As String = "=false"
        Dim sReportURL As String = String.Empty
        Dim sReportTitle As String = "=Invoice Discrepancies"


        If Me.txtVendorName.Text.Length > 0 Then sVendor_ID = "=" & lVendor_ID.ToString
        If cmbStore.SelectedIndex > -1 Then sStore_No = "=" & ComboValue(cmbStore)
        sStartDate = "=" & DateTimePickerStartDate.Value.ToString("yyyy-MM-dd")
        sEndDate = "=" & DateTimePickerEndDate.Value.ToString("yyyy-MM-dd")

        If optPaymentDiscrepancies.Checked Then
            sReportTitle = "=Invoice Payment Discrepancies"
            sOptPaymentDiscrepancies = "=true"
        End If

        If optSpecificDiscrepancies.Checked Then
            ' Call a procedure to return a report title based on the selected items
            sReportTitle = BuildReportTitle()

            ' Use the values in the checkboxes
            If chkPackDiscrepancies.Checked Then sOptPackDiscrepancies = "=true"
            If chkQuantityDiscrepancies.Checked Then sOptQuantityDiscrepancies = "=true"
            If chkCostDiscrepancies.Checked Then sOptCostDiscrepancies = "=true"
            If chkNoIDDiscrepancies.Checked Then sOptNoIDDiscrepancies = "=true"
        End If

        Debug.Print(sReportURL = "InvoiceDiscrepanciesReport&rs:Command=Render&rc:Parameters=false&Vendor_ID" & sVendor_ID & "&Store_No" & sStore_No & "&StartDate" & sStartDate & "&EndDate" & sEndDate & "&INVDiscrepancysent=-1&AutomatedReport=false" & "&ReportTitle" & sReportTitle & "&PaymentDiscrep" & sOptPaymentDiscrepancies & "&PackDiscrep" & sOptPackDiscrepancies & "&QtyDiscrep" & sOptQuantityDiscrepancies & "&CostDiscrep" & sOptCostDiscrepancies & "&NoIdDiscrep" & sOptNoIDDiscrepancies)

        sReportURL = "InvoiceDiscrepanciesReport&rs:Command=Render&rc:Parameters=false&Vendor_ID" & sVendor_ID & "&Store_No" & sStore_No & "&StartDate" & sStartDate & "&EndDate" & sEndDate & "&INVDiscrepancySent=-1&AutomatedReport=false" & "&ReportTitle" & sReportTitle & "&PaymentDiscrep" & sOptPaymentDiscrepancies & "&PackDiscrep" & sOptPackDiscrepancies & "&QtyDiscrep" & sOptQuantityDiscrepancies & "&CostDiscrep" & sOptCostDiscrepancies & "&NoIdDiscrep" & sOptNoIDDiscrepancies

        Call ReportingServicesReport(sReportURL)
    End Sub

    Private Sub optVendorInvoiceComplete_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optVendorInvoiceComplete.CheckedChanged
        ' Disable the Specific Discrepancies checkboxes
        chkPackDiscrepancies.Enabled = False
        chkQuantityDiscrepancies.Enabled = False
        chkCostDiscrepancies.Enabled = False
        chkNoIDDiscrepancies.Enabled = False
    End Sub

    Private Sub optPaymentDiscrepancies_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optPaymentDiscrepancies.CheckedChanged
        ' Disable the Specific Discrepancies checkboxes
        chkPackDiscrepancies.Enabled = False
        chkQuantityDiscrepancies.Enabled = False
        chkCostDiscrepancies.Enabled = False
        chkNoIDDiscrepancies.Enabled = False

    End Sub

    Private Sub optSpecificDiscrepancies_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optSpecificDiscrepancies.CheckedChanged
        ' Enable the Specific Discrepancies checkboxes
        chkPackDiscrepancies.Enabled = True
        chkQuantityDiscrepancies.Enabled = True
        chkCostDiscrepancies.Enabled = True
        chkNoIDDiscrepancies.Enabled = True
    End Sub

    Private Function BuildReportTitle() As String
        Dim sReportTitle As String = "=Invoice Discrepancies ("
        Dim intItemCount As Integer = 0

        ' Use the values in the checkboxes
        If chkPackDiscrepancies.Checked Then
            Select Case intItemCount
                Case 0
                    ' This is the first item
                    sReportTitle += "Pack"
                Case Else
                    ' This is a subsequent item, so prefix it with a comma
                    sReportTitle += ", Pack"
            End Select
            intItemCount += 1
        End If

        If chkQuantityDiscrepancies.Checked Then
            Select Case intItemCount
                Case 0
                    ' This is the first item
                    sReportTitle += "Qty"
                Case Else
                    ' This is a subsequent item, so prefix it with a comma
                    sReportTitle += ", Qty"
            End Select
            intItemCount += 1
        End If

        If chkCostDiscrepancies.Checked Then
            Select Case intItemCount
                Case 0
                    ' This is the first item
                    sReportTitle += "Cost"
                Case Else
                    ' This is a subsequent item, so prefix it with a comma
                    sReportTitle += ", Cost"
            End Select
            intItemCount += 1
        End If

        If chkNoIDDiscrepancies.Checked Then
            Select Case intItemCount
                Case 0
                    ' This is the first item
                    sReportTitle += "Not Found/Ordered"
                Case Else
                    ' This is a subsequent item, so prefix it with a comma
                    sReportTitle += ", Not Found/Ordered"
            End Select
            intItemCount += 1
        End If

        ' Close out the grouping
        sReportTitle += ")"

        Return sReportTitle

    End Function

End Class