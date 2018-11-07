Option Strict Off
Option Explicit On
Imports System.Text
Imports WholeFoods.Utility
Friend Class frmInvoiceManifest
	Inherits System.Windows.Forms.Form
	
	Private Sub cmbField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbField.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Dim Index As Short = cmbField.GetIndex(eventSender)
		
		If KeyAscii = 8 Then cmbField(Index).SelectedIndex = -1
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        Dim reportUrlBuilder As StringBuilder

        If dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If

        reportUrlBuilder = New StringBuilder()

        reportUrlBuilder.AppendFormat("{0}_InvoiceManifestReport&rs:Command=Render&rc:Parameters=False", ConfigurationServices.AppSettings("Region"))

        If (cmbField(0).SelectedIndex > 0) Then
            reportUrlBuilder.AppendFormat("&FromVendor={0}", ComboValue(cmbField(0)))
        End If

        If (cmbField(1).SelectedIndex > 0) Then
            reportUrlBuilder.AppendFormat("&ToVendor={0}", ComboValue(cmbField(1)))
        End If
        reportUrlBuilder.AppendFormat("&StartDate={0:d}&EndDate={1:d}&Distribution_Only={2}", dtpStartDate.Value, dtpEndDate.Value.AddDays(1), chkDistribution_Only.Checked)

        If cmbSubTeam.Text = "ALL" Or cmbSubTeam.Text = "" Then
            reportUrlBuilder.AppendFormat("&SubTeam_No:isnull=true")
        Else
            reportUrlBuilder.AppendFormat("&SubTeam_No=" & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        End If

        ReportingServicesReport(reportUrlBuilder.ToString())
    End Sub
	
	Private Sub frmInvoiceManifest_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
		LoadInternalCustomer(cmbField(0))
		LoadInternalCustomer(cmbField(1))

        LoadAllSubTeams(cmbSubTeam)
        cmbSubTeam.Items.Insert(0, "ALL")

        If cmbSubTeam.Items.Count > 0 Then
            cmbSubTeam.SelectedIndex = 0
        End If
        dtpStartDate.Value = DateAdd(DateInterval.Day, -7, SystemDateTime)
        dtpEndDate.Value = DateAdd(DateInterval.Day, -1, SystemDateTime)

	End Sub
	
End Class