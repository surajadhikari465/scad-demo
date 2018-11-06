Imports WholeFoods.IRMA.Common.DataAccess

Public Class KitchenCaseXferRpt

    Private Sub KitchenCaseXferRpt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim FacilitiesList As ArrayList = StoreListDAO.GetFacilitiesListByVendorName()
        With cmbFacility
            .DataSource = FacilitiesList
            .DisplayMember = "VendorName"
            .ValueMember = "VendorID"
            .SelectedIndex = 0
        End With

        LoadAllSubTeams(cmbSubTeam)
        'cmbSubTeam.SelectedIndex = 0
        cmbSubTeam.Items.Insert(0, "ALL")

        dtpStartDate.Value = Today.AddMonths(-1)
        dtpEndDate.Value = Today

    End Sub

    Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click

        '  Validation
        If dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If

        If cmbFacility.SelectedIndex = -1 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblFacility.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            cmbFacility.Focus()
            Exit Sub
        End If

        If cmbSubTeam.SelectedIndex = -1 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblSubTeam.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            cmbFacility.Focus()
            Exit Sub
        End If

        Dim sTitle As String = String.Empty
        Dim sTitle1 As String = String.Empty
        Dim sTitle2 As String = String.Empty
        'Dim sReportFile As String = String.Empty

        Me.Text = "Querying Kitchen Case Transfer Report..."

        '--------------------------
        ' Setup Report URL
        ' for Reporting Services
        '--------------------------

        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append("KitchenCaseTransfer")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        '--------------------------
        ' Add Report Parameters
        ' (all Credit Reason reports share the same stored proc 
        ' and have the same parameters)
        '--------------------------

        If dtpStartDate.Text = "" Then
            sReportURL.Append("&StartDate:isnull=true")
        Else
            sReportURL.Append("&StartDate=" & dtpStartDate.Text)
        End If

        If dtpEndDate.Text = "" Then
            sReportURL.Append("&EndDate:isnull=true")
        Else
            sReportURL.Append("&EndDate=" & dtpEndDate.Text)
        End If

        If cmbFacility.Text = "ALL" Or cmbFacility.Text = "" Then
            sReportURL.Append("&Facility:isnull=true")
            sReportURL.Append("&FacilityName=ALL")
        Else
            sReportURL.Append("&Facility=" & cmbFacility.SelectedValue.ToString())
            sReportURL.Append("&FacilityName=" & cmbFacility.Text.Trim())
        End If

        If cmbSubTeam.Text = "ALL" Or cmbSubTeam.Text = "" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        End If
        'sReportURL.Append("&Title1=" & sTitle1.Trim)
        'sReportURL.Append("&Title2=" & sTitle2.Trim)

        '--------------------------
        ' Display Report
        '--------------------------
        Dim s As String = sReportURL.ToString()
        Call ReportingServicesReport(sReportURL.ToString)


    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub
End Class