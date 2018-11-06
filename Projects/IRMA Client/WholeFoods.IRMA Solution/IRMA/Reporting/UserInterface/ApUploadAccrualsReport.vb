Public Class frmApUploadAccrualsReport

    Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click

        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################

        MsgBox("APUploadAccrualReport.vb  cmdReport_Click(): The Crystal Report APUpClosed.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)

        ' Parameters declaration to call Reporting Services.
        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String

        '--------------------------
        ' Setup Report URL
        '--------------------------

        filename = ConfigurationManager.AppSettings("Region")
        filename = filename + "_APUPAccrualsClosed"
        sReportURL.Append(filename)

        ' This chooses the region and based on the results points to the correct report.

        ' Report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")


        If cmbStore.SelectedIndex = -1 Or cmbStore.Text = "ALL" Or cmbStore.Text = "" Then
            sReportURL.Append("&sStore_No:isnull=true")
        Else
            sReportURL.Append("&sStore_No=" & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex))
        End If

        Call ReportingServicesReport(sReportURL.ToString)

        ' Case 6
        '    gRSRecordset = SQLOpenRecordSet("EXEC GetAPUpAccruals " & sStore_No, dao.RecordsetTypeEnum.dbOpenSnapshot, dao.RecordsetOptionEnum.dbSQLPassThrough + dao.RecordsetOptionEnum.dbForwardOnly)

        'If lReportIndex = 6 Then 'Accruals

        '	gDBReport.CommitTrans() 'Actually nothing to commit - could rollback but no error so this is cleaner

        '	If Not (gRSRecordset.EOF) Then
        '		'Export to CSV file
        '              cdbSave.InitialDirectory = My.Application.Info.DirectoryPath
        '              cdbSave.Filter = "Comma Separated Values (*.csv)|*.CSV"
        '              cdbSave.ShowDialog()
        '              If Len(cdbSave.FileName) > 0 Then
        '                  FileOpen(1, Trim(cdbSave.FileName), OpenMode.Output)
        '                  On Error GoTo me_f_err
        '                  'Output headers as the first row
        '                  PrintLine(1, "Unit,Ledger,Account,Team,Dept,Proj,Aff,Curr,Amount,N/R, RateType, Rate,BaseAmount, Stat, StatAmount, Description,PONumber,RecvLogNo")
        '                  Do
        '                      sOut = """" & IIf(gRSRecordset.Fields("Unit").Value > 0, gRSRecordset.Fields("Unit").Value, "") & """," & """ACTUALS""," & """" & IIf(gRSRecordset.Fields("Account").Value > 0, gRSRecordset.Fields("Account").Value, "") & """," & """" & IIf(gRSRecordset.Fields("Team").Value > 0, gRSRecordset.Fields("Team").Value, "") & """," & """" & IIf(gRSRecordset.Fields("Dept").Value > 0, gRSRecordset.Fields("Dept").Value, "") & """," & """""," & """""," & """USD""," & gRSRecordset.Fields("Amount").Value & "," & """""," & """""," & """""," & """""," & """""," & """""," & """" & gRSRecordset.Fields("Description").Value & """," & gRSRecordset.Fields("PONumber").Value & "," & gRSRecordset.Fields("RecvLogNo").Value

        '                      PrintLine(1, sOut)

        '                      gRSRecordset.MoveNext()
        '                  Loop Until gRSRecordset.EOF
        '                  FileClose(1)
        '                  On Error GoTo 0
        '                  MsgBox("Export to " & cdbSave.FileName & " completed", MsgBoxStyle.Information, Me.Text)
        '              End If
        '		gRSRecordset.Close()
        '	Else
        '		MsgBox("No Accruals to export", MsgBoxStyle.Critical, Me.Text)
        '	End If





    End Sub

    Private Sub _txtDate_0_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ApUploadAccrualsReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadStore(cmbStore, bInclude_Dist:=True)
    End Sub
End Class