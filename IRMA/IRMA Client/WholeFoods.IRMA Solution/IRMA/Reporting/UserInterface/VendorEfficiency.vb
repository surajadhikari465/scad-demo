Option Strict Off
Option Explicit On
Friend Class frmVendorEfficiency
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        Dim rsReport As ADODB.Recordset = Nothing

        Try
            gDBReport.BeginTrans()
            gDBReport.Execute("DELETE * FROM VendorEfficiency")

            rsReport = New ADODB.Recordset
            rsReport.Open("VendorEfficiency", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdTable)

            gRSRecordset = SQLOpenRecordSet("EXEC VendorEfficiency " & ComboValue(cmbSubTeam), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            While Not gRSRecordset.EOF

                rsReport.AddNew()
                rsReport.Fields("CompanyName").Value = gRSRecordset.Fields("CompanyName").Value
                rsReport.Fields("Identifier").Value = gRSRecordset.Fields("Identifier").Value
                rsReport.Fields("Item_Description").Value = gRSRecordset.Fields("Item_Description").Value & ""
                rsReport.Fields("Average_Days").Value = gRSRecordset.Fields("Average_Days").Value
                rsReport.Fields("Average_Received").Value = gRSRecordset.Fields("Average_Received").Value
                rsReport.Fields("Unit_Name").Value = gRSRecordset.Fields("Unit_Name").Value
                rsReport.Fields("Percentage_Received").Value = gRSRecordset.Fields("Percentage_Received").Value
                rsReport.Fields("Number_Of_Orders").Value = gRSRecordset.Fields("Number_Of_Orders").Value
                rsReport.Update()

                gRSRecordset.MoveNext()
            End While

            gDBReport.CommitTrans()
            If gJetFlush IsNot Nothing Then
                gJetFlush.RefreshCache(gDBReport)
            End If
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

        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################
        MsgBox("VendorEfficiency.vb  cmdReport_Click(): The Crystal Report VendorEfficiency.RPT is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)


        '      crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "VendorEfficiency.rpt"
        'crwReport.ReportTitle = "Vendor Efficiency For " & VB6.GetItemString(cmbSubTeam, cmbSubTeam.SelectedIndex)
        'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)
        'PrintReport(crwReport)

    End Sub
	
	Private Sub frmVendorEfficiency_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		Call CenterForm(Me)
		LoadAllSubTeams(cmbSubTeam)
        cmbSubTeam.SelectedIndex = -1
		
	End Sub
End Class