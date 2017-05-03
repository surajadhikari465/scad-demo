Option Strict Off
Option Explicit On

Imports log4net
Imports System.Web


Friend Class frmWarehouseMovementReport
    Inherits System.Windows.Forms.Form

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub frmWarehouseMovementReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        '-- Center the form
        'CenterForm(Me)

        ' Commented as part of the Fix for the Bug 5938.
        'LoadInventoryStore(cboWarehouse)

        ' Changed the following line to fix the bug 5938.
        LoadWarehouseStore(cboWarehouse)
        cboWarehouse.SelectedIndex = -1

        LoadAllSubTeams(cboSubTeam)
        cboSubTeam.SelectedIndex = -1

        'TFS Bug 6692:Clear out the vendor ID from the previous search to allow 
        'for no vendor selection on report.  The form does not maintain
        'the previously selected vendor name, only the vendor ID.
        glVendorID = 0


    End Sub

    Private Sub cmdCompanySearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCompanySearch.Click
        logger.Debug("cmdCompanySearch_Click Entry")

        glVendorID = 0
        '-- Set the search type
        giSearchType = iSearchAllVendors
        '-- Open the search form
        frmSearch.Text = "Search for Vendor by Company Name"
        frmSearch.ShowDialog()

        frmSearch.Close()
        frmSearch.Dispose()

        '-- if its not zero, then something was found
        If glVendorID <> 0 Then Call GetVendorName(glVendorID)

        logger.Debug("cmdCompanySearch_Click Entry")
    End Sub

    Private Sub GetVendorName(ByRef lVendorID As Integer)
        logger.Debug("GetVendorName Entry")

        Dim rsVendor As DAO.Recordset = Nothing
        Try
            rsVendor = SQLOpenRecordSet("EXEC GetVendorName " & glVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Me.txtVendorName.Text = rsVendor.Fields("CompanyName").Value
        Finally
            If rsVendor IsNot Nothing Then
                rsVendor.Close()
                rsVendor = Nothing
            End If
        End Try

        logger.Debug("GetVendorName Exit")
    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        ' VALIDATION
        If cboWarehouse.SelectedIndex = -1 Then
            MsgBox("Warehouse must be selected.", MsgBoxStyle.Exclamation, "Error")
            Exit Sub
        End If

        If cboSubTeam.SelectedIndex = -1 Then
            MsgBox("Sub-Team must be selected.", MsgBoxStyle.Exclamation, "Error")
            Exit Sub
        End If

        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String = "WarehouseMovement"

        sReportURL.Append(filename)
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")
        sReportURL.Append("&Store_No=" & VB6.GetItemData(cboWarehouse, cboWarehouse.SelectedIndex))
        sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex))
        sReportURL.Append("&Store_Name=" & cboWarehouse.Text)
        sReportURL.Append("&SubTeam_Name=" & cboSubTeam.Text)

        If glVendorID = 0 Then
            sReportURL.Append("&VendorID:isnull=true")
        Else
            sReportURL.Append("&VendorID=" & glVendorID)
        End If

        If txtVendorName.Text.Trim = "" Then
            sReportURL.Append("&VendorName:isnull=true")
        Else
            'TFS 6744 - Encode Vendor Name to protect against characters like & in the name
            Dim vendorNameEncoded As String = HttpUtility.UrlEncode(txtVendorName.Text.Trim)
            sReportURL.Append("&VendorName=" & vendorNameEncoded)
        End If

        If txtIdentifier.Text.Trim = "" Then
            sReportURL.Append("&Identifier:isnull=true")
        Else
            sReportURL.Append("&Identifier=" & txtIdentifier.Text)
        End If

        Dim sURL As String = sReportURL.ToString
        Call ReportingServicesReport(sURL)

        ' Old version:
        'ReportingServicesReport(String.Format("WarehouseMovement&rs:Command=Render&rc:Parameters=False&Store_No={1}&SubTeam_No={2}&Store_Name={3}&SubTeam_Name={4}", _
        '    ConfigurationServices.AppSettings("Region"), _
        '    VB6.GetItemData(cboWarehouse, cboWarehouse.SelectedIndex), _
        '    VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex), _
        '    cboWarehouse.Text, _
        '    cboSubTeam.Text))
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

End Class