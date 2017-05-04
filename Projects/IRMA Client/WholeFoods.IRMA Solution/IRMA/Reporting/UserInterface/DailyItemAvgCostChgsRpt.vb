Imports WholeFoods.IRMA.Common.DataAccess
Public Class DailyItemAvgCostChgsRpt

    Private Sub DailyItemAvgCostChgsRpt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim StoreList As ArrayList = StoreListDAO.GetStoresListByStoreName()
        With cmbStore
            .DataSource = StoreList
            .DisplayMember = "StoreName"
            .ValueMember = "StoreNo"
            .SelectedIndex = 0
        End With

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = -1
        End If

        LoadAllSubTeams(cmbSubTeam)
        cmbSubTeam.Items.Insert(0, "ALL")

        If cmbSubTeam.Items.Count > 0 Then
            cmbSubTeam.SelectedIndex = 0
        End If

        'LoadAllSubTeams(cmbSubTeam)
        'cmbSubTeam.Items.Add("ALL")
        'cmbSubTeam.SelectedIndex = 0
    End Sub

    Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click

        '  Validation

        If cmbStore.SelectedIndex = -1 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblFacility.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            cmbStore.Focus()
            Exit Sub
        End If

        If cmbSubTeam.SelectedIndex = -1 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblSubTeam.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            cmbStore.Focus()
            Exit Sub
        End If


        '--------------------------
        ' Setup Report URL
        ' for Reporting Services
        '--------------------------

        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append("DailyItemAvgCostChgsRpt")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        '--------------------------
        ' Add Report Parameters
        '--------------------------


        If cmbStore.Text = "ALL" Or cmbStore.Text = "" Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & cmbStore.SelectedValue.ToString)
        End If


        If cmbSubTeam.Text = "ALL" Or cmbSubTeam.Text = "" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        End If

        If txtSKU.Text = "ALL" Or Trim(txtSKU.Text) = "" Then
            sReportURL.Append("&SKU:isnull=true")
        Else
            sReportURL.Append("&SKU=" & Trim(txtSKU.Text))
        End If

        '--------------------------
        ' Display Report
        '--------------------------
        'Dim s As String = sReportURL.ToString()
        Call ReportingServicesReport(sReportURL.ToString)
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub txtSKU_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSKU.TextChanged

    End Sub
End Class