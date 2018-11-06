Option Strict Off
Option Explicit On

Friend Class frmInventoryValueReport
    Inherits System.Windows.Forms.Form

    Enum ReportTypes
        BU
        Team
        SubTeam
        Detail
    End Enum

    Private RptType As ReportTypes

    Private Sub frmInventoryValueReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        '-- Center the form
        ' Rick Kelleher 9/26/07: replaced the old VB6 method of centering the form with the .NET method
        Me.StartPosition = FormStartPosition.CenterParent

        '-- Load the combo boxes
        LoadInventoryStore(cboBusUnit)   ' <- changed this call to include all stores. Not just warehouse. tfs 13521
        If cboBusUnit.Items.Count > 0 Then
            cboBusUnit.SelectedIndex = 0
        End If

        LoadTeam(cboTeam)

        cboTeam.Items.Insert(0, "ALL")
        If cboTeam.Items.Count > 0 Then
            cboTeam.SelectedIndex = 0
        End If

        ' hide the '+' buttons
        HierarchySelector1.cmdAddCat.Visible = False
        HierarchySelector1.cmdAddLevel3.Visible = False
        HierarchySelector1.cmdAddLevel4.Visible = False

        If gsRegionCode.Equals("MA") Then
            radioTeam.Enabled = False
            _lblLabel_0.Enabled = False
            cboTeam.Enabled = False
        Else
            HierarchySelector1.cmdAddCat.Visible = False
            HierarchySelector1.cmdAddLevel3.Visible = False
            HierarchySelector1.cmdAddLevel4.Visible = False
        End If

        cboTeam.Items.Insert(0, "ALL")
        If cboTeam.Items.Count > 0 Then
            cboTeam.SelectedIndex = 0
        End If

        HierarchySelector1.cmbSubTeam.Items.Insert(0, "ALL")
        HierarchySelector1.cmbSubTeam.SelectedItem = "ALL"

        ' (Taken from frmItemList)
        'hide the "Print Only" checkbox; unable to print directly in SQL Server Reporting Services
        chkPrintOnly.Visible = False

    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        If Me.radioBU.Checked Then
            RptType = ReportTypes.BU
        ElseIf Me.radioTeam.Checked Then
            RptType = ReportTypes.Team
        ElseIf Me.radioSubTeam.Checked Then
            RptType = ReportTypes.SubTeam
        ElseIf Me.radioFullDetail.Checked Then
            RptType = ReportTypes.Detail
        Else
            MsgBox("Report type must be selected.", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        If cboBusUnit.SelectedIndex = -1 Then
            MsgBox("Business Unit must be selected.", MsgBoxStyle.Critical, Me.Text)
            Me.cboBusUnit.Focus()
            Exit Sub
        End If

        If RptType = ReportTypes.BU Then
        Else
            ' Must be Team, SubTeam or Detail
            If cboTeam.SelectedIndex = -1 Then
                MsgBox("Team must be selected.", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If

            If RptType = ReportTypes.Team Then
            Else
                ' Must be SubTeam or Detail
                If HierarchySelector1.cmbSubTeam.SelectedIndex = -1 Then
                    MsgBox("SubTeam must be selected.", MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
            End If
        End If


        '--------------------------
        ' Setup Report URL
        '--------------------------
        sReportURL.Append(gsRegionCode)

        If RptType = ReportTypes.Detail Then
            sReportURL.Append("_InventoryValueDetail")
        Else
            sReportURL.Append("_InventoryValueSummary")
        End If

        'This chooses the region and based on the results points to the correct report.

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        '--------------------------
        ' Add Report Parameters
        '--------------------------
        If cboBusUnit.Text = "ALL" Or cboBusUnit.Text = "" Then
            sReportURL.Append("&BusUnit:isnull=true")
        Else
            sReportURL.Append("&BusUnit=" & VB6.GetItemData(cboBusUnit, cboBusUnit.SelectedIndex))
        End If

        If cboTeam.Text = "ALL" Or cboTeam.Text = "" Then
            sReportURL.Append("&TeamNo:isnull=true")
        Else
            sReportURL.Append("&TeamNo=" & VB6.GetItemData(cboTeam, cboTeam.SelectedIndex))
        End If

        ' Billy Blackerby 9/10/08 Bug 6623: Changed report Parameter to match report, undoing what Matt Potok did on 9/9/08
        If HierarchySelector1.cmbSubTeam.Text = "ALL" Or HierarchySelector1.cmbSubTeam.Text = "" Then
            sReportURL.Append("&SubTeamNo:isnull=true")
        Else
            sReportURL.Append("&SubTeamNo=" & VB6.GetItemData(HierarchySelector1.cmbSubTeam, HierarchySelector1.cmbSubTeam.SelectedIndex))
        End If

        If HierarchySelector1.cmbCategory.Text = "ALL" Or HierarchySelector1.cmbCategory.Text = "" Then
            sReportURL.Append("&Class:isnull=true")
        Else
            sReportURL.Append("&Class=" & VB6.GetItemData(HierarchySelector1.cmbCategory, HierarchySelector1.cmbCategory.SelectedIndex))
        End If

        If HierarchySelector1.cmbLevel3.Text = "ALL" Or HierarchySelector1.cmbLevel3.Text = "" Then
            sReportURL.Append("&Level3:isnull=true")
        Else
            sReportURL.Append("&Level3=" & VB6.GetItemData(HierarchySelector1.cmbLevel3, HierarchySelector1.cmbLevel3.SelectedIndex))
        End If

        If HierarchySelector1.cmbLevel4.Text = "ALL" Or HierarchySelector1.cmbLevel4.Text = "" Then
            sReportURL.Append("&Level4:isnull=true")
        Else
            sReportURL.Append("&Level4=" & VB6.GetItemData(HierarchySelector1.cmbLevel4, HierarchySelector1.cmbLevel4.SelectedIndex))
        End If

        If txtSKUNumber.Text = "ALL" Or txtSKUNumber.Text = "" Then
            sReportURL.Append("&Identifier:isnull=true")
        Else
            sReportURL.Append("&Identifier=" & txtSKUNumber.Text)
        End If

        '--------------------------
        ' Display Report
        '--------------------------
        'MsgBox("Testing: URL = " & vbCrLf & vbCrLf & sReportURL.ToString)
        Call ReportingServicesReport(sReportURL.ToString)
 
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

End Class