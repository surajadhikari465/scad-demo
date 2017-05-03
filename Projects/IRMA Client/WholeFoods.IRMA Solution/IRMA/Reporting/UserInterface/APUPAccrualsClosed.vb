Imports WholeFoods.Utility

Public Class frmAPUPAccrualsClosedReport

    Private Sub APUPAccrualsClosed_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CenterForm(Me)
        LoadStore(cmbStore, bInclude_Dist:=True)

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = -1
        End If
    End Sub

    Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click

        ' This report is part of ApUpload report, In legacy code it has been exported to csv file.
        ' Now its directly calling the report created using reporting services and user can save to csv format from there.

        ' Parameters declaration to call Reporting Services.
        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String

        '--------------------------
        ' Setup Report URL
        '--------------------------

        filename = ConfigurationServices.AppSettings("Region")

        ' No region name is attached to the report as it is not region specific.
        filename = "APUPAccrualsClosed"
        sReportURL.Append(filename)

        ' This chooses the region and based on the results points to the correct report.

        ' Report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")


        If cmbStore.SelectedIndex = -1 Or cmbStore.Text = "ALL" Or cmbStore.Text = "" Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex))
        End If


        Call ReportingServicesReport(sReportURL.ToString)

    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub
End Class