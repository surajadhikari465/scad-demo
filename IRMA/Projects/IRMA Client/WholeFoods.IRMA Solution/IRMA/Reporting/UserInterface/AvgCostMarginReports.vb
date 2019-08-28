Option Strict Off
Option Explicit On
Imports Infragistics.Win.UltraWinGrid
Friend Class frmAvgCostMarginReports
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean
	Private mbNoClick As Boolean
    Private mbFilling As Boolean
    Private mdtStores As DataTable
	
	Private Enum geStoreCol
		StoreNo = 0
		StoreName = 1
		ZoneID = 2
		State = 3
		WFMStore = 4
		MegaStore = 5
		CustomerType = 6
	End Enum
	
	Private Sub frmAvgCostMarginReports_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
        dtpStartDate.Value = SystemDateTime()
        dtpEndDate.Value = SystemDateTime()

        Call LoadAvgCostVendors()
        Call SetActive(fraLevel, True)
		Call SetStoreSelection(False)
		
	End Sub
	
	Private Sub LoadAvgCostVendors()
		
		cmbVendor.Items.Clear()

        Try
            gRSRecordset = SQLOpenRecordSet("EXEC GetAvgCostVendor", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            Do While Not gRSRecordset.EOF
                cmbVendor.Items.Add(New VB6.ListBoxItem(gRSRecordset.Fields("CompanyName").Value, gRSRecordset.Fields("Vendor_ID").Value))
                gRSRecordset.MoveNext()
            Loop

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

    End Sub

    Private Sub cmbVendor_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbVendor.SelectedIndexChanged
        If IsInitializing = True Then Exit Sub

        Dim rsStores As DAO.Recordset = Nothing
        Dim strVendorID As String

        If cmbVendor.SelectedIndex > -1 Then

			LoadSubTeamByType(enumSubTeamType.StoreByVendorID, cmbSubTeam, Nothing, VB6.GetItemData(cmbVendor, cmbVendor.SelectedIndex), -1, True)
			SetActive(cmbSubTeam, True)

            '-- Fill out the store list
            strVendorID = CStr(VB6.GetItemData(cmbVendor, cmbVendor.SelectedIndex))
            Try
                rsStores = SQLOpenRecordSet("EXEC GetStoreCustomer NULL, " & strVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                StoreListGridCreateLoad(rsStores, mdtStores, ugrdStoreList)
            Finally
                If rsStores IsNot Nothing Then
                    rsStores.Close()
                    rsStores = Nothing
                End If
            End Try

            Call LoadVendZones(cmbZones, VB6.GetItemData(cmbVendor, cmbVendor.SelectedIndex))

            Call StoreListGridLoadStatesCombo(mdtStores, cmbStates)

            Call SetStoreSelection(True)

        Else
            cmbSubTeam.Items.Clear()
            SetActive(cmbSubTeam, False)

            Call SetStoreSelection(False)

        End If

        Call SetCombos()

    End Sub

    Private Sub SetStoreSelection(ByRef bEnabled As Boolean)

        Dim ctrl As System.Windows.Forms.Control

        On Error Resume Next

        For Each ctrl In Me.Controls
            If ctrl.Parent.Name = "fraStores" Then
                ctrl.Enabled = bEnabled
            End If
        Next ctrl

        optSelection(0).Checked = True 'Set store selection back to manual.

        Call SetCombos()

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Me.Close()

    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        Dim strReportFile As String = String.Empty
        Dim strReportTitle As String
        Dim strStoreList As String
        Dim strStoreListSeparator As String
        Dim strTransferSubTeam As String
        Dim row As UltraGridRow

        ' Validate parameters
        If cmbVendor.SelectedIndex = -1 Then
            MsgBox("You must select a vendor", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Select a Vendor")
            Exit Sub
        ElseIf cmbSubTeam.SelectedIndex = -1 Then
            MsgBox("You must select a Transfer SubTeam", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Select a Transfer SubTeam")
            Exit Sub
        ElseIf dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If

        strStoreListSeparator = "|"

        ' Determine Report Name and Title
        If optReport(0).Checked Then
            'PO Selected
            If optLevel(0).Checked Then
                'Detail Selected.
                strReportFile = "AvgCostMrgPODet"
                strReportTitle = "Distribution PO Margin" & vbCrLf & cmbVendor.Text & vbCrLf & dtpStartDate.Text & " - " & dtpEndDate.Text
            Else
                'Summary Selected
                If optSelection(5).Checked Or optSelection(6).Checked Then
                    'Region Selected
                    strReportFile = "AvgCostMrgPORegion"
                    strReportTitle = "Distribution PO Margin Region Summary" & vbCrLf & cmbVendor.Text & vbCrLf & dtpStartDate.Text & " - " & dtpEndDate.Text
                Else
                    'Region NOT selected (Store Summary)
                    strReportFile = "AvgCostMrgPOSum"
                    strReportTitle = "Distribution PO Margin Summary" & vbCrLf & cmbVendor.Text & vbCrLf & dtpStartDate.Text & " - " & dtpEndDate.Text
                End If
            End If

        Else
            'Item Selected
            If optLevel(0).Checked Then
                'Detail Selected
                strReportFile = "AvgCostMrgItemDet"
                strReportTitle = "Distribution Item Margin" & vbCrLf & cmbVendor.Text & vbCrLf & dtpStartDate.Text & " - " & dtpEndDate.Text
            Else
                'Summary Selected
                strReportFile = "AvgCostMrgItemSum"
                strReportTitle = "Distribution Item Margin Summary" & vbCrLf & cmbVendor.Text & vbCrLf & dtpStartDate.Text & " - " & dtpEndDate.Text
            End If
        End If

        ' Setup Report URL for Reporting Services
        Me.Text = "Running the Distribution margin Report..."
        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append(strReportFile)

        ' Report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        ' Add Report Parameters
        ' Vendor ID
        If cmbVendor.Text = "ALL" Or cmbVendor.Text = "" Then
            sReportURL.Append("&Vendor_ID:isnull=true")
        Else
            sReportURL.Append("&Vendor_ID=" & VB6.GetItemData(cmbVendor, cmbVendor.SelectedIndex))
        End If

        ' Transfer SubTeam
        If cmbSubTeam.SelectedIndex = -1 Then
            strTransferSubTeam = "NULL"
        Else
            strTransferSubTeam = CStr(VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        End If

        sReportURL.Append("&Transfer_SubTeam=" & strTransferSubTeam)

        ' StoreList
        strStoreList = ""

        If Me.ugrdStoreList.Selected.Rows.Count = 0 Then
        Else
            For Each row In Me.ugrdStoreList.Selected.Rows
                strStoreList = strStoreList & (row.Cells("Vendor_ID").Value.ToString) & strStoreListSeparator
            Next
        End If

        If strStoreList.Length > 0 Then
            strStoreList = Mid(strStoreList, 1, strStoreList.Length - 1)
        End If

        sReportURL.Append("&StoreList=" & strStoreList)

        ' Store List Separator
        sReportURL.Append("&StoreListSeparator=" & strStoreListSeparator)

        ' Start Date
        If dtpStartDate.Text = "" Then
            sReportURL.Append("&StartRecvDate:isnull=true")
        Else
            sReportURL.Append("&StartRecvDate=" & dtpStartDate.Text)
        End If

        ' End Date
        If dtpEndDate.Text = "" Then
            sReportURL.Append("&EndRecvDate:isnull=true")
        Else
            sReportURL.Append("&EndRecvDate=" & dtpEndDate.Text)
        End If

        'Display(Report)

        Dim s As String = sReportURL.ToString()
        'MsgBox(s)
        Call ReportingServicesReport(sReportURL.ToString)

    End Sub
	
	Private Function OKtoPrint() As Boolean
		
		OKtoPrint = True
		
		'-- Required parms validation
		If cmbVendor.SelectedIndex = -1 Then
			MsgBox("Please select a vendor", MsgBoxStyle.Critical, Me.Text)
			GoTo NotOK
		End If
		
        If dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            GoTo NotOK
        End If

        If ugrdStoreList.Selected.Rows.Count = 0 Then
            MsgBox("No stores have been selected.", MsgBoxStyle.Critical, Me.Text)
            GoTo NotOK
        End If
		
		Exit Function
		
NotOK: 
		OKtoPrint = False
		
	End Function
	
    Private Sub optReport_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optReport.CheckedChanged
        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then
            Dim Index As Short = optReport.GetIndex(eventSender)

            Call SetLevelButtons()

        End If
    End Sub
	
    Private Sub OptSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSelection.CheckedChanged

        If mbFilling Or IsInitializing Then Exit Sub

        If eventSender.Checked Then
            Dim Index As Short = optSelection.GetIndex(eventSender)

            Call SetCombos()

            mbFilling = True

            ugrdStoreList.Selected.Rows.Clear()

            Select Case Index
                Case 0
                    '-- Manual.
                    cmbZones.SelectedIndex = -1
                    cmbStates.SelectedIndex = -1
                Case 1
                    '-- All Stores
                    Call StoreListGridSelectAll(ugrdStoreList, True)
                Case 2
                    '-- By Zone
                    If cmbZones.SelectedIndex > -1 Then Call StoreListGridSelectByZone(ugrdStoreList, VB6.GetItemData(cmbZones, cmbZones.SelectedIndex))
                Case 3
                    '-- By State.
                    If cmbStates.SelectedIndex > -1 Then Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemData(cmbStates, cmbStates.SelectedIndex))
                Case 4
                    '-- All WFM.
                    Call StoreListGridSelectAllWFM(ugrdStoreList)
                Case 5
                    '-- 5 = All Region.
                    StoreListGridSelectAllRegion(ugrdStoreList)
                Case 6
                    '-- 6 = All Region - Retail Only.
                    StoreListGridSelectRetailOnly(ugrdStoreList)
            End Select

            mbFilling = False

        End If

        Call SetLevelButtons()

    End Sub
	
	Private Sub SetLevelButtons()
		
		'Enable Detail report option.
		optLevel(0).Enabled = False
		optLevel(1).Enabled = False
		
		If optSelection(5).Checked = True Or optSelection(6).Checked = True Then
			'Region is selected...
			If optReport(0).Checked Then 'PO report
				optLevel(1).Enabled = True 'SUMMARY (Only for the region)...
				optLevel(1).Checked = True
			Else '-- Item report
				optLevel(0).Enabled = True 'DETAIL...
				optLevel(1).Enabled = True
			End If
		Else
			optLevel(0).Enabled = True 'DETAIL...
			optLevel(1).Enabled = True
		End If
		
	End Sub

	Private Sub SetCombos()
		
		mbFilling = True
		
		'Zones.
        If optSelection(2).Checked = True Then
            SetActive(cmbZones, True)
        Else
            cmbZones.SelectedIndex = -1
            SetActive(cmbZones, False)
        End If
		
		'States.
		If optSelection(3).Checked = True Then
            SetActive(cmbStates, True)
        Else
            cmbStates.SelectedIndex = -1
            SetActive(cmbStates, False)
        End If
		
		mbFilling = False
		
	End Sub

    Private Sub cmbStates_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStates.SelectedIndexChanged

        If mbFilling Or IsInitializing Then Exit Sub

        mbFilling = True

        Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemString(cmbStates, cmbStates.SelectedIndex))

        mbFilling = False

    End Sub
	
    Private Sub cmbZones_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbZones.SelectedIndexChanged

        If mbFilling Or IsInitializing Then Exit Sub

        OptSelection_CheckedChanged(optSelection.Item(2), New System.EventArgs())

    End Sub

    Private Sub ugrdStoreList_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles ugrdStoreList.AfterSelectChange

        If mbFilling Or IsInitializing Then Exit Sub

        mbFilling = True
        optSelection.Item(0).Checked = True
        mbFilling = False

    End Sub

    Private Sub StoreListGridCreateLoad(ByRef rsStores As DAO.Recordset, ByRef dtStores As DataTable, ByRef ugrdStoreList As Infragistics.Win.UltraWinGrid.UltraGrid)

        dtStores = New DataTable("StoreList")
        dtStores.Columns.Add(New DataColumn("Vendor_ID", GetType(Integer)))
        dtStores.Columns.Add(New DataColumn("CompanyName", GetType(String)))
        dtStores.Columns.Add(New DataColumn("Zone_ID", GetType(Integer)))
        dtStores.Columns.Add(New DataColumn("State", GetType(String)))
        dtStores.Columns.Add(New DataColumn("WFM_Store", GetType(Boolean)))
        dtStores.Columns.Add(New DataColumn("Mega_Store", GetType(Boolean)))
        dtStores.Columns.Add(New DataColumn("CustomerType", GetType(Integer)))

        '-- Fill out the store list
        Dim row As DataRow
        While Not rsStores.EOF
            row = dtStores.NewRow
            row("Vendor_ID") = rsStores.Fields("Vendor_ID").Value
            row("CompanyName") = rsStores.Fields("CompanyName").Value
            row("Zone_ID") = rsStores.Fields("Zone_ID").Value
            row("State") = rsStores.Fields("State").Value
            row("WFM_Store") = rsStores.Fields("WFM_Store").Value
            row("Mega_Store") = rsStores.Fields("Mega_Store").Value
            row("CustomerType") = rsStores.Fields("CustomerType").Value

            dtStores.Rows.Add(row)

            rsStores.MoveNext()
        End While

        dtStores.AcceptChanges()
        ugrdStoreList.DataSource = dtStores

    End Sub

    Private Function StoreListGridGetSelVendorList(ByRef ugrd As Infragistics.Win.UltraWinGrid.UltraGrid) As String

        Dim sStores As New System.Text.StringBuilder(String.Empty)
        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow

        For Each row In ugrd.Selected.Rows
            If sStores.Length > 0 Then
                sStores.Append("|" & row.Cells("Vendor_ID").Value.ToString)
            Else
                sStores.Append(row.Cells("Vendor_ID").Value.ToString)
            End If

        Next

        Return sStores.ToString

    End Function
End Class