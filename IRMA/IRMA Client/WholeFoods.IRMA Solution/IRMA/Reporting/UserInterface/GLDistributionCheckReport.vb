Option Strict Off
Option Explicit On
Imports System.Text
Imports WholeFoods.Utility
Friend Class frmGLDistributionCheckReport
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

        ' ***************************************************************************************
        Dim reportUrlBuilder As StringBuilder
        Dim reportName As String
        Dim aztest As String
        ' ******************** Combo Validation *********************
        Dim i As Integer = 0
        For i = 0 To 3
            If cmbField(i).SelectedIndex = -1 Then
                MsgBox("Missing Selection", MsgBoxStyle.Exclamation, "Error")
                cmbField(i).Focus()
                Exit Sub
            End If
        Next
        ' ************************************************************

        If dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If

        If chkShowDetail.Checked Then
            reportName = "GLDistributionCheckDetailsReport"
        Else
            reportName = "GLDistributionCheckReport"
        End If
        reportUrlBuilder = New StringBuilder()

        ' Changes as part of Fix for 5312

        'If cboBusUnit.Text = "ALL" Or cboBusUnit.Text = "" Then
        '    sReportURL.Append("&BusUnit:isnull=true")
        'Else
        '    sReportURL.Append("&BusUnit=" & VB6.GetItemData(cboBusUnit, cboBusUnit.SelectedIndex))
        'End If
        aztest = VB6.GetItemData(cmbField(0), cmbField(0).SelectedIndex).ToString

        ' Date:         09/29/2011
        ' Updated By:   Denis Ng
        ' Bug #:        1011
        ' Description:  When launching the report using IRMA Client the URL includes the parameters and 
        '               something is getting turned off preventing the report to display the summary totals.
        ' Solution:     1.  Check to see if the SelectedIndex of cmbField(0), cmbField(1), cmbField(2), cmbField(3) are equal to 0
        '               2.  If step 1 is true, then set DistributionStoreNo:IsNull=True, ReceiveNo:IsNull=True, 
        '                   Transfer_SubTeam:IsNull=True, Transfer_ToSubTeam:IsNull=True
        '               3.  Otherwise,
        '                   DistributonStore_No = VB6.GetItemData(cmbField(0), cmbField(0).SelectedIndex).ToString
        '                   ReceiveStore_No = VB6.GetItemData(cmbField(1), cmbField(1).SelectedIndex).ToString
        '                   Transfer_SubTeam = VB6.GetItemData(cmbField(2), cmbField(2).SelectedIndex).ToString
        '                   and, Transfer_To_SubTeam = VB6.GetItemData(cmbField(3), cmbField(3).SelectedIndex).ToString
        '               4.  Four new string variables will be declared: DistributionStoreNo, ReceiveStoreNo, TransferSubTeam, TransferToSubTeam
        '
        ' Begin Fix: Bug 1011
        Dim DistributionStoreNo As String = ""
        Dim ReceiveStoreNo As String = ""
        Dim TransferSubTeam As String = ""
        Dim TransferToSubTeam As String = ""

        If VB6.GetItemData(cmbField(0), cmbField(0).SelectedIndex).ToString = "0" Then
            DistributionStoreNo = "DistributionStore_No:ISNull=True"
        Else
            DistributionStoreNo = "DistributionStore_No=" & VB6.GetItemData(cmbField(0), cmbField(0).SelectedIndex).ToString
        End If

        If VB6.GetItemData(cmbField(1), cmbField(1).SelectedIndex).ToString = "0" Then
            ReceiveStoreNo = "ReceiveStore_No:ISNull=True"
        Else
            ReceiveStoreNo = "ReceiveStore_No=" & VB6.GetItemData(cmbField(1), cmbField(1).SelectedIndex).ToString
        End If

        If VB6.GetItemData(cmbField(2), cmbField(2).SelectedIndex).ToString = "0" Then
            TransferSubTeam = "Transfer_SubTeam:ISNull=True"
        Else
            TransferSubTeam = "Transfer_SubTeam=" & VB6.GetItemData(cmbField(2), cmbField(2).SelectedIndex).ToString
        End If

        If VB6.GetItemData(cmbField(3), cmbField(3).SelectedIndex).ToString = "0" Then
            TransferToSubTeam = "Transfer_To_SubTeam:ISNull=True"
        Else
            TransferToSubTeam = "Transfer_To_SubTeam=" & VB6.GetItemData(cmbField(3), cmbField(3).SelectedIndex).ToString
        End If

        reportUrlBuilder.AppendFormat("{3}&rs:Command=Render&rc:Parameters=False&StartDate={1:d}&EndDate={2:d}" & _
        "&" & DistributionStoreNo & "&" & ReceiveStoreNo & "&" & TransferSubTeam & "&" & TransferToSubTeam, _
            ConfigurationServices.AppSettings("Region"), _
            dtpStartDate.Value, _
            dtpEndDate.Value, _
            reportName)

        ' End Fix: Bug 1011
        ReportingServicesReport(reportUrlBuilder.ToString())
        ' ***************************************************************************************
    End Sub
	
	Private Sub frmGLDistributionCheckReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
        dtpStartDate.Value = DateAdd(DateInterval.Day, -7, SystemDateTime)
        dtpEndDate.Value = DateAdd(DateInterval.Day, -1, SystemDateTime)
        '20090616 - Dave Stacey - Added "ALL" option TFS 9852
        LoadStores(cmbField(0))
        ReplicateCombo(cmbField(0), cmbField(1))
        cmbField(0).Items.Insert(0, "ALL")
        cmbField(0).SelectedItem = "ALL"
        cmbField(1).Items.Insert(0, "ALL")
        cmbField(1).SelectedItem = "ALL"
        LoadAllSubTeams(cmbField(2))
        ReplicateCombo(cmbField(2), cmbField(3))
        cmbField(2).Items.Insert(0, "ALL")
        cmbField(2).SelectedItem = "ALL"
        cmbField(3).Items.Insert(0, "ALL")
        cmbField(3).SelectedItem = "ALL"
		
	End Sub
	
End Class