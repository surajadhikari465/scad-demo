Option Strict Off
Option Explicit On
Friend Class frmInventoryGuideReport
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
		Dim sFileName As String
		
		' GET THE STORE SELECTED
		If cboStore.SelectedIndex = -1 Then
			MsgBox("Store must be selected.", MsgBoxStyle.Exclamation, "Error")
			Exit Sub
		End If
		
		' GET THE WAREHOUSE SELECTED
		If cboWarehouse.SelectedIndex = -1 Then
			MsgBox("Warehouse must be selected.", MsgBoxStyle.Exclamation, "Error")
			Exit Sub
		End If
		
		' GET THE SUBTEAM SELECTED
		If cboSubTeam.SelectedIndex = -1 Then
			MsgBox("Sub-Team must be selected.", MsgBoxStyle.Exclamation, "Error")
			Exit Sub
		End If
		
		'-- Do Export
		If chkExport.CheckState = 1 Then
			
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
                Try
                    gRSRecordset = SQLOpenRecordSet("EXEC ProductInventoryGuide " & VB6.GetItemData(cboStore, cboStore.SelectedIndex) & ", " & VB6.GetItemData(cboWarehouse, cboWarehouse.SelectedIndex) & ", " & VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex) & ", " & System.Math.Abs(chkWFMItems.CheckState) & ", " & System.Math.Abs(chkDiscontinued.CheckState), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    FileOpen(1, sFileName, OpenMode.Output)

                    PrintLine(1, "Origin,Identifier,Description,Pack1,Unit Name,Retail,Current Store Cost,Current Cost From Warehouse,Back Stock, Front Stock, Total, Total Cost, Total Retail")

                    While Not gRSRecordset.EOF

                        PrintLine(1, """" & gRSRecordset.Fields("Origin_Name").Value & """," & """" & gRSRecordset.Fields("Identifier").Value & """," & """" & gRSRecordset.Fields("Item_Description").Value & """," & gRSRecordset.Fields("Package_Desc1").Value & "," & gRSRecordset.Fields("Unit_Name").Value & "," & """" & IIf(gRSRecordset.Fields("RetailMultiple").Value > 1, gRSRecordset.Fields("RetailMultiple").Value & "@", "") & VB6.Format(gRSRecordset.Fields("RetailPrice").Value, "###0.00") & """," & VB6.Format(gRSRecordset.Fields("Store_Cost").Value, "###0.0000") & "," & VB6.Format(gRSRecordset.Fields("Warehouse_Cost").Value, "###0.0000") & ",,,,")

                        gRSRecordset.MoveNext()

                    End While

                Finally
                    If gRSRecordset IsNot Nothing Then
                        gRSRecordset.Close()
                        gRSRecordset = Nothing
                    End If
                End Try

                FileClose(1)
                MsgBox("Export complete.")
            End If
        Else
            Dim sReportURL As New System.Text.StringBuilder
            Dim filename As String = "InventoryGuideReport"

            sReportURL.Append(filename)
            sReportURL.Append("&rs:Command=Render")
            sReportURL.Append("&rc:Parameters=False")

            sReportURL.Append("&Store_No=" & VB6.GetItemData(cboStore, cboStore.SelectedIndex))
            sReportURL.Append("&Warehouse_ID=" & VB6.GetItemData(cboWarehouse, cboWarehouse.SelectedIndex))
            sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex))

            sReportURL.Append("&WFM_Item=" & System.Math.Abs(chkWFMItems.CheckState))
            sReportURL.Append("&Include_Discontinued=" & System.Math.Abs(chkDiscontinued.CheckState))

            Call ReportingServicesReport(sReportURL.ToString)
        End If
		
	End Sub
	
	Private Sub frmInventoryGuideReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		'-- Center the form
		CenterForm(Me)
		
		'-- Load the combo boxes
		LoadInventoryStore(cboStore)
		LoadInventoryStore(cboWarehouse)
        LoadAllSubTeams(cboSubTeam)

        If glStore_Limit > 0 Then
            SetActive(cboStore, False)
            SetCombo(cboStore, glStore_Limit)
        Else
            cboStore.SelectedIndex = -1
        End If
	End Sub
End Class