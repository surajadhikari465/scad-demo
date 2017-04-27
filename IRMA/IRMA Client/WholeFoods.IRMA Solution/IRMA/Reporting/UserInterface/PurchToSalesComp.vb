Option Strict Off
Option Explicit On

Friend Class PurchToSalesComp
    Inherits System.Windows.Forms.Form

    Private Sub frmPurchToSalesComp_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        LoadStore(cmbStore)
        LoadAllSubTeams(cmbSubTeam)

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = -1
        End If

        cmbSubTeam.SelectedIndex = -1

        dtpStartDate.Value = DateAdd(DateInterval.Day, -1, SystemDateTime)
        dtpEndDate.Value = DateAdd(DateInterval.Day, -1, SystemDateTime)

    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        Dim sTitle As String = String.Empty

        '-------------------------------Validation
        If dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If

        If cmbStore.SelectedIndex = -1 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblLabel(1).Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            cmbStore.Focus()
            Exit Sub
        End If

        If cmbSubTeam.SelectedIndex = -1 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblLabel(5).Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            cmbStore.Focus()
            Exit Sub
        End If


        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String = "PurchToSalesComp"

        sReportURL.Append(filename)
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If cmbStore.Text = "ALL" Or cmbStore.Text = "" Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex))
        End If

        If cmbSubTeam.Text = "ALL" Or cmbSubTeam.Text = "" Then
            sReportURL.Append("&Subteam_No:isnull=true")
        Else
            sReportURL.Append("&Subteam_No=" & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        End If

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
        sReportURL.Append("&StoreName=" & VB6.GetItemString(cmbStore, cmbStore.SelectedIndex))
        sReportURL.Append("&SubTeamName=" & VB6.GetItemString(cmbSubTeam, cmbSubTeam.SelectedIndex))

        'sTitle = "Purchase To Sales Comp Report (" & dtpStartDate.Text & " - " & dtpEndDate.Text & ")"
        ' & vbCrLf & vbCrLf & "Store - " & VB6.GetItemString(cmbStore, cmbStore.SelectedIndex) & vbCrLf & vbCrLf & "Sub Team - " & VB6.GetItemString(cmbSubTeam, cmbSubTeam.SelectedIndex)
        'sReportURL.Append("&Title=" & sTitle)
        sReportURL.Append("&Title:isnull=true")

        Call ReportingServicesReport(sReportURL.ToString)


        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################
        'MsgBox("frmPurchToSalesComp.vb  cmdReport_Click(): The Crystal Report PurchToSalesComp.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)

        '------------------------------Initialize Report
        '@Store_No int,
        '@SubTeam_No int,
        '@BeginDate varchar(10),
        '@EndDate varchar(10),
        'crwReport.Reset()
        'crwReport.set_StoredProcParam(0, CrystalValue(cmbStore))
        'crwReport.set_StoredProcParam(1, CrystalValue(cmbSubTeam))
        'crwReport.set_StoredProcParam(2, dtpStartDate.Text)
        'crwReport.set_StoredProcParam(3, dtpEndDate.Text)

        'crwReport.SelectionFormula = ""
        'crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "PurchToSalesComp.rpt"
        'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)
        'crwReport.ReportTitle = sTitle
        'crwReport.Connect = gsCrystal_Connect
        'PrintReport(crwReport)

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        Me.Close()
    End Sub

End Class