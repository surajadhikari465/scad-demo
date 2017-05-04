Option Strict Off
Option Explicit On

Friend Class frmCreditReason
    Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean

	Dim bDistribution, bTransfer As Boolean
	Dim KeyAsciiTemp As Short
	
    Private Sub cmbVendor_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbVendor.SelectedIndexChanged
        If Me.IsInitializing = True Then Exit Sub

        If KeyAsciiTemp <> 8 Then
            '-- See if it is a distribution or transfer order
            glVendorID = VB6.GetItemData(cmbVendor, cmbVendor.SelectedIndex)
            If DetermineVendorInternal() Then
                If ((gbDistribution_Center) Or (gbManufacturer)) Then
                    Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Distribution
                    ' old way, remove one day
                    bDistribution = True
                Else
                    Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Transfer
                    ' old way, remove one day
                    bTransfer = True
                End If
            End If

            '-- Load combo boxes
            If Not (bDistribution Or bTransfer) Then 'Purchase order
                chkShowStoreCost.Enabled = False
                chkShowStoreCost.CheckState = System.Windows.Forms.CheckState.Unchecked
            Else
                chkShowStoreCost.Enabled = True
            End If

            bDistribution = False
            bTransfer = False
        Else
            KeyAsciiTemp = 0
        End If

    End Sub
	
    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub
	
	Private Sub frmCreditReason_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		CenterForm(Me)
		
		LoadInternalCustomer(cmbStore)
		LoadVendors(cmbVendor)
		LoadZone(cmbZone)

        dtpStartDate.Value = SystemDateTime()
        dtpEndDate.Value = SystemDateTime()
        cmbZone.Enabled = False

	End Sub

    Private Sub cmbStore_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs)
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then cmbStore.SelectedIndex = -1

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmbVendor_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbVendor.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then
            KeyAsciiTemp = 8
            cmbVendor.SelectedIndex = -1
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmbZone_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs)
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then cmbZone.SelectedIndex = -1

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        Dim sTitle1 As String = String.Empty
        Dim sTitle2 As String = String.Empty
        Dim sReportFile As String = String.Empty

        If cmbVendor.SelectedIndex = -1 Then
            MsgBox("You must select a vendor", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Select a Vendor")
            Exit Sub
        ElseIf dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If

        Select Case True
            Case optStore.Checked
                sTitle1 = "Credit Reason Store Report"
                sTitle2 = cmbStore.Text
                If cmbStore.SelectedIndex = -1 Then
                    MsgBox("You must select a store.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, Me.Text)
                    cmbStore.Focus()
                    Exit Sub
                ElseIf chkDetail.CheckState = CheckState.Checked Then
                    sReportFile = "CreditReasonZone"
                Else
                    sReportFile = "CreditReasonDetailSummary"
                End If

            Case optZone.Checked
                If cmbZone.SelectedIndex = -1 Then
                    MsgBox("You must select a zone.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, Me.Text)
                    cmbZone.Focus()
                    Exit Sub
                ElseIf chkDetail.CheckState = CheckState.Checked Then
                    sTitle1 = "Credit Reason Report"
                    sTitle2 = cmbZone.Text
                    sReportFile = "CreditReasonDetail"
                Else
                    sTitle1 = "Credit Reason Summary Report"
                    sTitle2 = cmbZone.Text
                    sReportFile = "CreditReasonDetailSummary"
                End If

            Case optRegion.Checked
                sTitle1 = "Credit Reason Region Report"
                sTitle2 = "Region"
                If chkDetail.CheckState = CheckState.Checked Then
                    sReportFile = "CreditReasonRegion"
                Else
                    sReportFile = "CreditReasonRegionSummary"
                End If

            Case Else
                Exit Sub
        End Select

        Me.Text = "Running the Credit Reason Report..."

        '--------------------------
        ' Setup Report URL
        ' for Reporting Services
        '--------------------------

        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append(sReportFile)

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

        If cmbVendor.Text = "ALL" Or cmbVendor.Text = "" Then
            sReportURL.Append("&Vendor_ID:isnull=true")
            sReportURL.Append("&Vendor_Name:isnull=true")
        Else
            sReportURL.Append("&Vendor_ID=" & VB6.GetItemData(cmbVendor, cmbVendor.SelectedIndex))
            sReportURL.Append("&Vendor_Name=" & Trim(cmbVendor.SelectedItem.ToString))
        End If

        If cmbStore.Text = "ALL" Or cmbStore.Text = "" Then
            sReportURL.Append("&ReceiveLocation_ID:isnull=true")
            sReportURL.Append("&ReceiveLocation_Name:isnull=true")
        Else
            sReportURL.Append("&ReceiveLocation_ID=" & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex))
            sReportURL.Append("&ReceiveLocation_Name=" & Trim(cmbStore.SelectedItem.ToString))
        End If

        If cmbZone.Text = "ALL" Or cmbZone.Text = "" Then
            sReportURL.Append("&Zone:isnull=true")
            sReportURL.Append("&Zone_Name:isnull=true")
        Else
            sReportURL.Append("&Zone=" & VB6.GetItemData(cmbZone, cmbZone.SelectedIndex))
            sReportURL.Append("&Zone_Name=" & Trim(cmbZone.SelectedItem.ToString))
        End If

        sReportURL.Append("&CostOption=" & chkShowStoreCost.CheckState)

        '*** FKN 2/26/08 Commneted out as the reports do not accept thse parameters
        'sReportURL.Append("&VendorText=" & cmbVendor.Text.Trim)
        'sReportURL.Append("&Title1=" & sTitle1.Trim)
        'sReportURL.Append("&Title2=" & sTitle2.Trim)

        '--------------------------
        ' Display Report
        '--------------------------
        Dim s As String = sReportURL.ToString()
        Call ReportingServicesReport(sReportURL.ToString)

    End Sub

    Private Sub optStore_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optStore.CheckedChanged
        cmbStore.Enabled = optStore.Checked
    End Sub

    Private Sub optZone_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optZone.CheckedChanged
        cmbZone.Enabled = optZone.Checked
    End Sub

    Private Sub cmbZone_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbZone.EnabledChanged

        ' Clear out selected value if disabled
        If Not cmbZone.Enabled Then
            cmbZone.SelectedIndex = -1
        End If

    End Sub

    Private Sub cmbStore_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.EnabledChanged

        ' Clear out selected value if disabled
        If Not cmbStore.Enabled Then
            cmbStore.SelectedIndex = -1
        End If

    End Sub
End Class