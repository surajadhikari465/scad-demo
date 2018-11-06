Imports WholeFoods.IRMA.Common.DataAccess
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess

Public Class InventoryWeeklyHistoryReport

    Private Sub radioTeam_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioPost.CheckedChanged

    End Sub

    Private Sub InventoryWeeklyHistoryReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim StoreList As ArrayList = StoreListDAO.GetFacilitiesListByVendorName()
        With cmbFacility
            .DataSource = StoreList
            .DisplayMember = "VendorName"
            .ValueMember = "VendorID"
            .SelectedIndex = 0
        End With

        If cmbFacility.Items.Count > 0 Then
            cmbFacility.SelectedIndex = 0
        End If

        LoadAllSubTeams(cmbSubTeam)
        cmbSubTeam.Items.Insert(0, "ALL")

        If cmbSubTeam.Items.Count > 0 Then
            cmbSubTeam.SelectedIndex = 0
        End If


    End Sub


    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub


    Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click
        '  Validation

        If cmbFacility.SelectedIndex = -1 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblFacility.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            cmbFacility.Focus()
            Exit Sub
        End If

        If cmbSubTeam.SelectedIndex = -1 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblSubTeam.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            cmbSubTeam.Focus()
            Exit Sub
        End If


        '--------------------------
        ' Setup Report URL
        ' for Reporting Services
        '--------------------------

        Dim sReportURL As New System.Text.StringBuilder

        ' Based on the report selection.
        If radioPre.Checked Then
            sReportURL.Append("PreAlocInventoryWeeklyHistoryRpt")
        ElseIf radioPost.Checked Then
            sReportURL.Append("PostAlocInventoryWeeklyHistoryRpt")
        End If

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        '--------------------------
        ' Add Report Parameters
        '--------------------------


        If cmbFacility.Text = "ALL" Or cmbFacility.Text = "" Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & cmbFacility.SelectedValue.ToString)
        End If



        If cmbFacility.Text = "ALL" Or cmbFacility.Text = "" Then
            sReportURL.Append("&Store_Name:isnull=true")
        Else
            sReportURL.Append("&Store_Name=" & cmbFacility.Text)
        End If


        If cmbSubTeam.Text = "ALL" Or cmbSubTeam.Text = "" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        End If

        If cmbSubTeam.Text = "ALL" Or cmbSubTeam.Text = "" Then
            sReportURL.Append("&SubTeam_Name:isnull=true")
        Else
            sReportURL.Append("&SubTeam_Name=" & cmbSubTeam.Text)
        End If

        Dim dateRange As String
        dateRange = GetDateRangeOfCurrentWeek()
        sReportURL.Append("&Date_Range=" & dateRange)

        Call ReportingServicesReport(sReportURL.ToString)
    End Sub

    Public Shared Function GetDateRangeOfCurrentWeek() As String

        Dim Str As String = String.Empty

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As SqlDataReader = Nothing

        Try
            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataReader("GetDateRangeForCurrentWeek")

            While results.Read
                Str = results.GetString(results.GetOrdinal("DateRange"))
            End While
        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try

        Return Str
    End Function
End Class