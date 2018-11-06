Imports log4net
Public Class EInvoiceExceptionReport

    Private Sub EInvoiceExceptionReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Global.LoadVendors(Me.cboReceiveLocation)
        Global.LoadStores(Me.cboReceiveLocation)
        Global.LoadAllSubTeams(Me.cboSubTeam)
        Global.LoadVendors(Me.cboVendor)
    End Sub

    Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click
        Dim logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
        logger.debug("cmdReport_Click Entry")

        Dim ReportURL As New System.Text.StringBuilder
        Dim ReportFile As String = ""
        Dim StartDate As String = ""
        Dim EndDate As String = ""

        ReportFile = "eInvoiceExceptionReport"

        'Get Start Date
        StartDate = Date.Parse(dtpDateStart.Value).ToShortDateString()

        'Get End Date
        EndDate = Date.Parse(dtpDateEnd.Value).ToShortDateString()

        'Setup Report URL for Reporting Services
        ReportURL.Append(ReportFile)
        ReportURL.Append("&rs:Command=Render")
        ReportURL.Append("&rc:Parameters=False")

        'Adding report parameters
        If Me.cboReceiveLocation.SelectedIndex > -1 Then
            ReportURL.Append("&ReceiveLocation_ID=" & CStr(VB6.GetItemData(Me.cboReceiveLocation, Me.cboReceiveLocation.SelectedIndex)))
        Else
            ReportURL.Append("&ReceiveLocation_ID:isnull=true")
        End If

        If Me.cboSubTeam.SelectedIndex > -1 Then
            ReportURL.Append("&SubTeam_No=" & CStr(VB6.GetItemData(Me.cboSubTeam, Me.cboSubTeam.SelectedIndex)))
        Else
            ReportURL.Append("&SubTeam_No:isnull=true")
        End If

        If Me.cboVendor.SelectedIndex > -1 Then
            ReportURL.Append("&Vendor_ID=" & CStr(VB6.GetItemData(Me.cboVendor, Me.cboVendor.SelectedIndex)))
        Else
            ReportURL.Append("&Vendor_ID:isnull=true")
        End If

        ReportURL.Append("&OrderDateStart=" & StartDate & "&OrderDateEnd=" & EndDate)

        'Display Report
        Call ReportingServicesReport(ReportURL.ToString)
        logger.Debug("cmdReport_click Exit")


    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub
End Class