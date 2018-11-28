Option Strict Off
Option Explicit On
Friend Class frmOutOfPeriodInvoice
	Inherits System.Windows.Forms.Form
	Private Sub cmbField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbField.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Dim Index As Short = cmbField.GetIndex(eventSender)

        If Index = 2 Then
            If KeyAscii = 8 And Not cmbField(2).Enabled Then
                cmbField(2).SelectedIndex = -1
            End If
        End If
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		Me.Close()
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
		Dim sYear As Short
		Dim sPeriod As Short
        Dim strText As String

        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String = "OutOfPeriodInv"

        'filename = ConfigurationServices.AppSettings("Region")
        'filename = filename + "_PurchaseAccrualReport"

        sReportURL.Append(filename)
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        strText = mskStartDate.Text.Replace(" ", "0")

        If strText.Length = 6 Then strText = String.Format("{0}{1}", strText, "0000000")

        sPeriod = CShort(Mid(strText, 1, 2))

        If sPeriod < 1 Or sPeriod > 13 Then
            MsgBox("Invalid Period", MsgBoxStyle.Critical, Me.Text)
            mskStartDate.Focus()
            Exit Sub
        End If

        If Mid(strText, 4, 4) = "" Then
            MsgBox("Invalid Year", MsgBoxStyle.Critical, Me.Text)
            mskStartDate.Focus()
            Exit Sub
        Else
            sYear = CShort(Mid(strText, 4, 4))
        End If

        If sYear < Year(SystemDateTime) - 2 Then
            MsgBox("Year must be at least " & Year(SystemDateTime) - 2, MsgBoxStyle.Critical, Me.Text)
            mskStartDate.Focus()
            Exit Sub
        End If

        If chkAllStores.Checked = False And cmbField(1).Text = "" Then
            MsgBox("A store must be selected if All Stores option is not checked!")
            _cmbField_1.Focus()
            Exit Sub
        End If

        If chkAllSubTeams.Checked = False And cmbField(2).Text = "" Then
            MsgBox("A sub-team must be selected if All SubTeams option is not checked!")
            _cmbField_2.Focus()
            Exit Sub
        End If

        If chkAllStores.Checked Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & VB6.GetItemData(cmbField(1), cmbField(1).SelectedIndex))
        End If

        If chkAllSubTeams.Checked Then
            sReportURL.Append("&Subteam_No:isnull=true")
        Else
            sReportURL.Append("&Subteam_No=" & VB6.GetItemData(cmbField(2), cmbField(2).SelectedIndex))
        End If

        sReportURL.Append("&Year=" & sYear.ToString)
        sReportURL.Append("&Period=" & sPeriod.ToString)

        Call ReportingServicesReport(sReportURL.ToString)

    End Sub
	
	Private Sub frmOutOfPeriodInvoice_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        LoadStores(cmbField(1))
        LoadAllSubTeams(cmbField(2))

        If glStore_Limit > 0 Then
            SetActive(cmbField(1), False)
            SetCombo(cmbField(1), glStore_Limit)
        Else
            cmbField(1).SelectedIndex = -1
        End If
        ' Dave Stacey 20100421 - TFS 12567 - Fixed mask so it doesn't drop trailing 0 in year 2010

        With mskStartDate
            .Mask = "##/####"
        End With

        ShowStores()
        ShowSubTeams()
        CenterForm(Me)
    End Sub

  Private Sub mskStartDate_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mskStartDate.Enter
    mskStartDate.SelectAll()
  End Sub

  Private Sub ShowStores()
        If chkAllStores.Checked Then
            _cmbField_1.Enabled = False
        Else
            _cmbField_1.Enabled = True
        End If
    End Sub
    Private Sub ShowSubTeams()
        If chkAllSubTeams.Checked Then
            _cmbField_2.Enabled = False
        Else
            _cmbField_2.Enabled = True
        End If
    End Sub

    Private Sub chkAllSubTeams_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAllSubTeams.Click
        ShowSubTeams()
    End Sub

    Private Sub chkAllStores_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAllStores.Click
        ShowStores()
    End Sub
End Class