Option Strict Off
Option Explicit On
Imports WholeFoods.Utility
Friend Class frmStoreOrdersTotBySKUReport
    Inherits System.Windows.Forms.Form

    Private _isInitializing As Boolean

    Private Sub frmStoreOrdersTotBySKUReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Me._isInitializing = True

        '-- Center the form
        Me.StartPosition = FormStartPosition.CenterScreen

        '-- Load the combo boxes
        LoadDistributionCenters(cboWarehouse)
        If cboWarehouse.Items.Count > 0 Then
            cboWarehouse.SelectedIndex = 0
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

        HierarchySelector1.cmbSubTeam.Items.Insert(0, "ALL")

        If gsRegionCode.Equals("MA") Then
            _lblLabel_0.Visible = False
            cboTeam.Visible = False
        Else
            HierarchySelector1.cmdAddCat.Visible = False
            HierarchySelector1.cmdAddLevel3.Visible = False
            HierarchySelector1.cmdAddLevel4.Visible = False
        End If

        Me._dateFrom.Value = Today.Date.ToShortDateString
        Me._dateTo.Value = Today.Date.ToShortDateString

        ' TODO: Load Identifiers here
        ' cboIdentifier

        ' (Taken from frmItemList)
        'hide the "Print Only" checkbox; unable to print directly in SQL Server Reporting Services
        chkPrintOnly.Visible = False

        Me._isInitializing = False

    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        If cboWarehouse.SelectedIndex = -1 Or cboWarehouse.Text = "" Then
            MsgBox("Business Unit must be selected.", MsgBoxStyle.Exclamation, "Error")
            Exit Sub
        End If

        If Me._dateTo.Value < Me._dateFrom.Value Then
            Me._formErrorProvider.SetError(Me._dateTo, ResourcesItemHosting.GetString("msg_validation_startDateGreaterThanEndDate"))
            Exit Sub
        End If

        '--------------------------
        ' Setup Report URL
        '--------------------------
        'sReportURL.Append(gsRegionCode)
        'sReportURL.Append("MA")
        sReportURL.Append(ConfigurationServices.AppSettings("Region"))
        sReportURL.Append("_StoreOrdersTotBySKU")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        '--------------------------
        ' Add Report Parameters
        '--------------------------
        sReportURL.Append("&Warehouse=" & VB6.GetItemData(cboWarehouse, cboWarehouse.SelectedIndex))

        If cboTeam.Text = "ALL" Or cboTeam.Text = "" Then
            sReportURL.Append("&Team:isnull=true")
        Else
            sReportURL.Append("&Team=" & VB6.GetItemData(cboTeam, cboTeam.SelectedIndex))
        End If

        If HierarchySelector1.cmbSubTeam.Text = "ALL" Or HierarchySelector1.cmbSubTeam.Text = "" Then
            sReportURL.Append("&SubTeam:isnull=true")
        Else
            sReportURL.Append("&SubTeam=" & VB6.GetItemData(HierarchySelector1.cmbSubTeam, HierarchySelector1.cmbSubTeam.SelectedIndex))
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

        sReportURL.Append("&Start=" & Me._dateFrom.Value)
        sReportURL.Append("&End=" & Me._dateTo.Value)

        '--------------------------
        ' Display Report
        '--------------------------
        'MsgBox("Test: URL = " & sReportURL.ToString)
        Call ReportingServicesReport(sReportURL.ToString)

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub _dateTo_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _dateTo.ValueChanged
        If Not Me._isInitializing Then
            If Me._dateTo.Value > Me._dateFrom.Value Then
                Me._formErrorProvider.Clear()
            ElseIf Me._dateTo.Value < Me._dateFrom.Value Then
                Me._formErrorProvider.SetError(Me._dateTo, ResourcesItemHosting.GetString("msg_validation_startDateGreaterThanEndDate"))
            End If
        End If
    End Sub

    Private Sub _dateFrom_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _dateFrom.ValueChanged
        If Not Me._isInitializing Then
            If Me._dateTo.Value > Me._dateFrom.Value Then
                Me._formErrorProvider.Clear()
            ElseIf Me._dateTo.Value < Me._dateFrom.Value Then
                Me._formErrorProvider.SetError(Me._dateTo, ResourcesItemHosting.GetString("msg_validation_startDateGreaterThanEndDate"))
            End If
        End If
    End Sub
End Class