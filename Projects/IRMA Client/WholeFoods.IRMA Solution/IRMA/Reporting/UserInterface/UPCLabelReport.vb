Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Friend Class frmUpcLabelReport
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Sub LoadItemInformation()
        Try
            If glItemID = -1 Then
                gRSRecordset = SQLOpenRecordSet("EXEC GetAdjustmentInfoFirst NULL", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Else
                gRSRecordset = SQLOpenRecordSet("EXEC GetAdjustmentInfo " & glItemID & " , NULL", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            End If
            If gRSRecordset.EOF = False And gRSRecordset.BOF = False Then
                glItemID = gRSRecordset.Fields("Item_Key").Value
                txtItemDesc.Text = gRSRecordset.Fields("Item_Description").Value & ""
                txtIdentifier.Text = gRSRecordset.Fields("Identifier").Value & ""
                txtPackageDesc.Text = gRSRecordset.Fields("Package_Desc1").Value & " / " & gRSRecordset.Fields("Package_Desc2").Value & " " & gRSRecordset.Fields("Unit_Name").Value
            End If
        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try
    End Sub
	Private Sub cmdInventoryScan_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdInventoryScan.Click
		
		Dim iLoop As Short
        Dim rsReport As ADODB.Recordset
        Dim sBarCode As String = String.Empty
		
		If Len(txtIdentifier.Text) > 12 Then
			MsgBox("Must be a standard UPCA.", MsgBoxStyle.Exclamation, "Error!")
			Exit Sub
		End If
		
		gDBReport.BeginTrans()
		
		gDBReport.Execute("DELETE FROM UPCLabel")
		
		rsReport = New ADODB.Recordset
		rsReport.Open("UPCLabel", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdTable)
		
		For iLoop = 1 To IIf(opt5260.Checked, 30, 80)
			
			rsReport.AddNew()
			rsReport.Fields("Item_Description").Value = txtItemDesc.Text
			rsReport.Fields("Identifier").Value = txtIdentifier.Text
			On Error Resume Next
            sBarCode = VB.Left(BC_UPCA(VB6.Format(CDec(txtIdentifier.Text), "00000000000")), 20)
            ' prevent empty string error
            If sBarCode.Length = 0 Then sBarCode = " "
            rsReport.Fields("UPCText").Value = sBarCode

            On Error GoTo 0
			rsReport.Update()
			
		Next iLoop
		
		rsReport.Close()
        rsReport = Nothing
		
		gDBReport.CommitTrans()
        If gJetFlush IsNot Nothing Then
            gJetFlush.RefreshCache(gDBReport)
        End If
        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################
        MsgBox("UPCLabelreport.vb  cmdInventoryScan_Click(): The Crystal Report Avery5260.RPT or Avery5267.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)

        'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)
        '      crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "Avery" & IIf(opt5260.Checked, "5260", "5267") & ".RPT"
        'PrintReport(crwReport)
		
	End Sub
	
	Private Sub cmdItemSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdItemSearch.Click
		
        frmItemSearch.ShowDialog()
		
		'-- if its not zero, then something was found
		LoadItemInformation()
		
        frmItemSearch.Close()

	End Sub
	
	Private Sub frmUpcLabelReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		
		CenterForm(Me)
		
		'-- Load default information
		glItemID = -1
		LoadItemInformation()
		
	End Sub
End Class