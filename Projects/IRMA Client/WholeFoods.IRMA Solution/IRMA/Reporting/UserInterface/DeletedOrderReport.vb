Option Strict Off
Option Explicit On
Friend Class frmDeletedOrderReport
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean

	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
        If txtPO.Text.Length = 0 Then
            If cmbStore.SelectedIndex = -1 Then
                MsgBox("You must select a store", MsgBoxStyle.Critical, "Select a Store")
                Exit Sub
            End If
        Else
            If Not (IsNumeric(txtPO.Text) And InStr(1, txtPO.Text, ".") = 0 And InStr(1, txtPO.Text, "$") = 0) Then
                MsgBox("You must enter a number for the PO number", MsgBoxStyle.Critical, "Select a Store")
                Exit Sub
            End If
        End If
		
        If dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If

        '--------------------------
        ' Setup Report URL
        ' for Reporting Services
        '--------------------------

        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append("Deleted Orders Report")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

       

        '-----------------------------------------------
        ' Add Report Parameters
        '-----------------------------------------------
        If Trim(txtPO.Text) <> "" Then

            sReportURL.Append("&Store_No:isnull=true")
            sReportURL.Append("&OrderHeader_ID=" & Trim(txtPO.Text))
            sReportURL.Append("&StartDate:isnull=true" & dtpStartDate.Text)
            sReportURL.Append("&EndDate:isnull=true" & dtpEndDate.Text)
        Else
            If cmbStore.Text = "ALL" Or Trim(cmbStore.Text) = "" Then
                sReportURL.Append("&Store_No:isnull=true")
            Else
                sReportURL.Append("&Store_No=" & CrystalValue(cmbStore))
            End If

            If txtPO.Text.Length = 0 Then
                sReportURL.Append("&OrderHeader_ID:isnull=true")
            Else
                sReportURL.Append("&OrderHeader_ID=" & Trim(txtPO.Text))
            End If

            sReportURL.Append("&StartDate=" & dtpStartDate.Text)
            sReportURL.Append("&EndDate=" & dtpEndDate.Text)
        End If

        If Me.optDateType(0).Checked Then
            sReportURL.Append("&SearchBy=" & 0)
        Else
            sReportURL.Append("&SearchBy=" & 1)
        End If

        Call ReportingServicesReport(sReportURL.ToString)

    End Sub
	
	Private Sub frmDeletedOrderReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
        LoadInternalCustomer(cmbStore)

        If glVendor_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glVendor_Limit)
        Else
            cmbStore.SelectedIndex = -1
        End If

        dtpStartDate.Value = DateAdd(DateInterval.Day, -1, SystemDateTime)
        dtpEndDate.Value = SystemDateTime()
		
	End Sub
	
    Private Sub txtPO_KeyUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles txtPO.KeyUp
        Dim KeyCode As Short = eventArgs.KeyCode
        Dim Shift As Short = eventArgs.KeyData \ &H10000
        If txtPO.Text.Length = 0 Then
            Call SetActive((Me.cmbStore), True)
        Else
            cmbStore.SelectedIndex = -1
            Call SetActive((Me.cmbStore), False)
        End If
    End Sub

    Private Sub cmbStore_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbStore.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbStore.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
    End Sub

End Class