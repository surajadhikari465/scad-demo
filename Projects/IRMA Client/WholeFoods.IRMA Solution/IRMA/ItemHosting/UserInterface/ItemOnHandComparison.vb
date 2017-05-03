Option Strict Off
Option Explicit On
Friend Class frmItemOnHandComparison
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        Dim rsReport As ADODB.Recordset = Nothing

		'-- Create the GAP Report
		'-- Please note if data is ran based on retail, if no retail values exist then use ExtCost
		
		gDBReport.BeginTrans()
		gDBReport.Execute("DELETE * FROM ItemOnHandComparison")
        Try
            rsReport = New ADODB.Recordset
            rsReport.Open("ItemOnHandComparison", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdTable)

            gRSRecordset = SQLOpenRecordSet("EXEC ItemOnHandComparisonBetweenLocation " & ComboValue(cmbStore1) & ", " & ComboValue(cmbStore2) & ", " & ComboValue(cmbSubTeam), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            While Not gRSRecordset.EOF
                rsReport.AddNew()
                rsReport.Fields("Identifier").Value = gRSRecordset.Fields("Identifier").Value
                If Len(gRSRecordset.Fields("Identifier").Value) >= 12 Then
                    rsReport.Fields("UPC_Text").Value = " "
                Else
                    rsReport.Fields("UPC_Text").Value = BC_UPCA(VB6.Format(CDbl(gRSRecordset.Fields("Identifier").Value), "00000000000"))
                End If
                rsReport.Fields("Item_Description").Value = gRSRecordset.Fields("Item_Description").Value
                rsReport.Fields("Package_Desc").Value = CStr(gRSRecordset.Fields("Package_Desc1").Value & "/" & gRSRecordset.Fields("Package_Desc2").Value & " " & gRSRecordset.Fields("Unit_Name").Value)
                rsReport.Fields("Package_Desc1").Value = gRSRecordset.Fields("Package_Desc1").Value
                rsReport.Fields("Location1_OH").Value = gRSRecordset.Fields("On_Hand_With").Value
                rsReport.Update()
                gRSRecordset.MoveNext()
            End While

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
            If rsReport IsNot Nothing Then
                rsReport.Close()
                rsReport = Nothing
            End If
        End Try

        gDBReport.CommitTrans()
        If gJetFlush IsNot Nothing Then
            gJetFlush.RefreshCache(gDBReport)
        End If

        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################
        MsgBox("ItemOnHandComparison.vb  cmdReport_Click(): The Crystal Report ItemOnHandComparison.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)

        ''-- Print the report
        '      crwReport.ReportTitle = String.Format(ResourcesItemHosting.GetString("ItemComparison"), cmbStore1.Text, cmbStore2.Text, vbCrLf, cmbSubTeam.Text)
        '      crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "ItemOnHandComparison.rpt"
        'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)
        'PrintReport(crwReport)

    End Sub
	
	Private Sub frmItemOnHandComparison_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		Call CenterForm(Me)
		
		LoadInventoryStore(cmbStore1)
        cmbStore1.SelectedIndex = -1

		LoadInventoryStore(cmbStore2)
        cmbStore2.SelectedIndex = -1
		
		LoadAllSubTeams(cmbSubTeam)
        cmbSubTeam.SelectedIndex = -1
		
	End Sub
End Class