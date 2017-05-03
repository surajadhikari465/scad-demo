Option Strict Off
Option Explicit On

Imports WholeFoods.Utility

Friend Class frmDiscontinueItemsWithInventory
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################
        'MsgBox("DiscontinueItemsWithInventory.vb  cmdReport_Click(): The Crystal Report DiscontinueItemsWithInventory.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)

        'crwReport.set_StoredProcParam(0, VB6.GetItemData(cmbStore, cmbStore.SelectedIndex))
        'crwReport.set_StoredProcParam(1, VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))

        '      crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "DiscontinueItemsWithInventory.rpt"
        'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)
        'crwReport.ReportTitle = "Discontinued Items With Inventory " & vbCrLf & " For " & cmbStore.Text & " - " & cmbSubTeam.Text
        'crwReport.Connect = gsCrystal_Connect
        'PrintReport(crwReport)


        ' Code to call corresponding Sql server reporting service.

        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String


        '--------------------------
        ' Setup Report URL
        '--------------------------
        filename = ConfigurationServices.AppSettings("Region")

        ' Region name has been deleted from the fileName.
        filename = "DiscontinuedItemsWithInventory"
        sReportURL.Append(filename)

        ' This chooses the region and based on the results points to the correct report.

        ' Report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")


        If cmbStore.SelectedIndex = -1 Then
            MsgBox("Store must be selected.", MsgBoxStyle.Exclamation, "Invalid Store")
            cmbStore.Focus()
            Exit Sub
        End If

        If cmbSubTeam.SelectedIndex = -1 Then
            MsgBox("Subteam must be selected.", MsgBoxStyle.Exclamation, "Invalid Subteam")
            cmbSubTeam.Focus()
            Exit Sub
        End If


        If cmbStore.Text = "ALL" Or cmbStore.Text = "" Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex))
        End If


        If cmbSubTeam.Text = "ALL" Or cmbSubTeam.Text = "" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        End If

        Call ReportingServicesReport(sReportURL.ToString)

		
	End Sub
	
	Private Sub frmDiscontinueItemsWithInventory_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
        CenterForm(Me)

		'-- Load the combos
		LoadAllSubTeams(cmbSubTeam)
		LoadInventoryStore(cmbStore)

        If cmbSubTeam.Items.Count > 0 Then cmbSubTeam.SelectedIndex = -1

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = -1
        End If
    End Sub
End Class