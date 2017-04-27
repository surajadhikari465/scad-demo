Option Strict Off
Option Explicit On
Friend Class frmActiveItemReport
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
		Dim sFileName As String
		
		If cmbSubTeam.SelectedIndex = -1 Then
			MsgBox("Sub Team must be selected.", MsgBoxStyle.Exclamation, "Error!")
			Exit Sub
		End If
		
        Try
            ' - Do Export
            If chkExport.CheckState = 1 Then

                gRSRecordset = SQLOpenRecordSet("EXEC ActiveItemList " & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex) & ", " & chkWFMItems.CheckState, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                cdbFileOpen.InitialDirectory = My.Application.Info.DirectoryPath
                cdbFileSave.InitialDirectory = My.Application.Info.DirectoryPath
                cdbFileOpen.CheckFileExists = True
                cdbFileOpen.CheckPathExists = True
                cdbFileSave.CheckPathExists = True
                cdbFileOpen.ShowReadOnly = False
                cdbFileOpen.Filter = "Comma Separated Values (*.csv)|*.csv"
                cdbFileSave.Filter = "Comma Separated Values (*.csv)|*.csv"
                cdbFileSave.ShowDialog()
                cdbFileOpen.FileName = cdbFileSave.FileName

                sFileName = Trim(cdbFileOpen.FileName)

                If sFileName <> "" Then

                    FileOpen(1, sFileName, OpenMode.Output)

                    PrintLine(1, "'Item_Key','Identfiier','Description','CompanyName','Pkg.1','Pkg.2','Unit Name'")

                    While Not gRSRecordset.EOF

                        PrintLine(1, "'" & gRSRecordset.Fields("Item_Key").Value & "'," & "'" & gRSRecordset.Fields("Identifier").Value & "'," & "'" & gRSRecordset.Fields("Item_Description").Value & "'," & "'" & gRSRecordset.Fields("CompanyName").Value & "'," & gRSRecordset.Fields("Package_Desc1").Value & "," & gRSRecordset.Fields("Package_Desc2").Value & ", " & "'" & gRSRecordset.Fields("Unit_Name").Value)

                        gRSRecordset.MoveNext()

                    End While

                    FileClose(1)
                    MsgBox("Export complete.")
                End If

                gRSRecordset.Close()
            Else
                ' To fix the bug 6317. Calling Reporting services report.

                '--------------------------
                ' Setup Report URL
                ' for Reporting Services
                '--------------------------

                Dim sReportURL As New System.Text.StringBuilder
                sReportURL.Append("ActiveItemsListRpt")

                'report display
                sReportURL.Append("&rs:Command=Render")
                sReportURL.Append("&rc:Parameters=False")

                '--------------------------
                ' Add Report Parameters
                '--------------------------


                If cmbSubTeam.Text = "ALL" Or cmbSubTeam.Text = "" Then
                    sReportURL.Append("&SubTeam_No:isnull=true")
                    sReportURL.Append("&SubTeam_Name:isnull=true")
                Else
                    sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
                    sReportURL.Append("&SubTeam_Name=" & cmbSubTeam.Text)
                End If


                sReportURL.Append("&WFM_Item=" & chkWFMItems.CheckState)

                Call ReportingServicesReport(sReportURL.ToString)

            End If
        Finally
        End Try


        ' Commnented the previous Crystal Report Calling Code.

        'Dim sFileName As String

        'If cmbSubTeam.SelectedIndex = -1 Then
        '    MsgBox("Sub Team must be selected.", MsgBoxStyle.Exclamation, "Error!")
        '    Exit Sub
        'End If

        'Dim rsReport As ADODB.Recordset = Nothing
        'Try
        '    gRSRecordset = SQLOpenRecordSet("EXEC ActiveItemList " & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex) & ", " & chkWFMItems.CheckState, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        '    ' - Do Export
        '    If chkExport.CheckState = 1 Then

        '        cdbFileOpen.InitialDirectory = My.Application.Info.DirectoryPath
        '        cdbFileSave.InitialDirectory = My.Application.Info.DirectoryPath
        '        cdbFileOpen.CheckFileExists = True
        '        cdbFileOpen.CheckPathExists = True
        '        cdbFileSave.CheckPathExists = True
        '        cdbFileOpen.ShowReadOnly = False
        '        cdbFileOpen.Filter = "Comma Separated Values (*.csv)|*.csv"
        '        cdbFileSave.Filter = "Comma Separated Values (*.csv)|*.csv"
        '        cdbFileSave.ShowDialog()
        '        cdbFileOpen.FileName = cdbFileSave.FileName

        '        sFileName = Trim(cdbFileOpen.FileName)

        '        If sFileName <> "" Then

        '            FileOpen(1, sFileName, OpenMode.Output)

        '            PrintLine(1, "'Item_Key','Identfiier','Description','CompanyName','Pkg.1','Pkg.2','Unit Name'")

        '            While Not gRSRecordset.EOF

        '                PrintLine(1, "'" & gRSRecordset.Fields("Item_Key").Value & "'," & "'" & gRSRecordset.Fields("Identifier").Value & "'," & "'" & gRSRecordset.Fields("Item_Description").Value & "'," & "'" & gRSRecordset.Fields("CompanyName").Value & "'," & gRSRecordset.Fields("Package_Desc1").Value & "," & gRSRecordset.Fields("Package_Desc2").Value & ", " & "'" & gRSRecordset.Fields("Unit_Name").Value)

        '                gRSRecordset.MoveNext()

        '            End While

        '            FileClose(1)
        '            MsgBox("Export complete.")
        '        End If

        '        gRSRecordset.Close()
        '    Else

        '        gDBReport.BeginTrans()

        '        gDBReport.Execute("DELETE * FROM ActiveItemList")

        '        rsReport = New ADODB.Recordset
        '        rsReport.Open("ActiveItemList", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdTable)

        '        While Not gRSRecordset.EOF

        '            rsReport.AddNew()
        '            rsReport.Fields("Item_Key").Value = gRSRecordset.Fields("Item_Key").Value
        '            If Len(gRSRecordset.Fields("Identifier").Value) >= 12 Or Len(gRSRecordset.Fields("Identifier").Value) < 10 Then
        '                rsReport.Fields("UPC_Text").Value = System.DBNull.Value
        '            Else
        '                rsReport.Fields("UPC_Text").Value = BC_UPCA(VB6.Format(CDbl(gRSRecordset.Fields("Identifier").Value), "00000000000"))
        '            End If
        '            rsReport.Fields("Identifier").Value = gRSRecordset.Fields("Identifier").Value
        '            rsReport.Fields("Item_Description").Value = gRSRecordset.Fields("Item_Description").Value & ""
        '            rsReport.Fields("CompanyName").Value = gRSRecordset.Fields("CompanyName").Value & ""
        '            rsReport.Fields("Package_Desc").Value = gRSRecordset.Fields("Package_Desc1").Value & "/" & gRSRecordset.Fields("Package_Desc2").Value & " " & gRSRecordset.Fields("Unit_Name").Value
        '            rsReport.Update()

        '            gRSRecordset.MoveNext()

        '        End While

        '        gRSRecordset.Close()

        '        rsReport.Close()
        '        rsReport = Nothing

        '        gDBReport.CommitTrans()
        '        If gJetFlush IsNot Nothing Then
        '            gJetFlush.RefreshCache(gDBReport)
        '        End If
        '        ' ###########################################################################
        '        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        '        ' ###########################################################################
        '        MsgBox("ActiveItemReport.vb  cmdReport_Click(): The Crystal Report ActiveItemList.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)
        '        'crwReport.ReportTitle = "Active " & IIf(chkWFMItems.CheckState = 1, "WFM ", "") & "Item List (" & cmbSubTeam.Text & ")"
        '        'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)
        '        '         crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "ActiveItemList.rpt"
        '        'PrintReport(crwReport)

        '    End If
        'Finally
        '    If gRSRecordset IsNot Nothing Then
        '        gRSRecordset.Close()
        '        gRSRecordset = Nothing
        '    End If
        '    If rsReport IsNot Nothing Then
        '        rsReport.Close()
        '        rsReport = Nothing
        '    End If
        'End Try
    End Sub
	
	Private Sub frmActiveItemReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
		'-- Load the combos
		LoadAllSubTeams(cmbSubTeam)
		
		'-- Set it to the first sub team
        If cmbSubTeam.Items.Count > 0 Then cmbSubTeam.SelectedIndex = -1
		
	End Sub
End Class