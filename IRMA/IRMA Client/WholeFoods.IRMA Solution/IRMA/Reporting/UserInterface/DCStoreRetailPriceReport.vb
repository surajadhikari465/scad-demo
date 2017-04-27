Option Strict Off
Option Explicit On
Imports Infragistics.Win.UltraWinGrid
Friend Class frmDCStoreRetailPriceReport
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

    Private Sub frmDCStoreRetailPriceReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        CenterForm(Me)

        '-- Load the combos
        LoadAllSubTeams(cmbSubTeam)

        '-- Set it to the first sub team
        If cmbSubTeam.Items.Count > 0 Then cmbSubTeam.SelectedIndex = 0

        'LoadInternalCustomer(cmbStore)
        Call LoadAvgCostVendors()
        Call SetStoreSelection(False)

    End Sub

    Private Sub LoadAvgCostVendors()

        cmbVendor.Items.Clear()

        Try
            gRSRecordset = SQLOpenRecordSet("EXEC GetAvgCostVendor", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            'cmbVendor.Items.Add(New VB6.ListBoxItem("Select a DC", 0))
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

            LoadVendStoreSubteam(cmbSubTeam, VB6.GetItemData(cmbVendor, cmbVendor.SelectedIndex))
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
        Dim strStoreList As String
        Dim strStoreListSeparator As String
        Dim row As UltraGridRow

        strStoreListSeparator = ","

        ' Setup Report URL for Reporting Services
        Me.Text = "Running the DC Store Retail Price Report..."
        Dim sReportURL As New System.Text.StringBuilder

        strReportFile = "DCStoreRetailPriceReport"
        sReportURL.Append(strReportFile)

        ' Report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If chkIsRegional.Checked Then
            sReportURL.Append("&IsRegional=True")
        Else
            sReportURL.Append("&IsRegional=False")
        End If
        ' StoreList
        strStoreList = ""

        If Me.ugrdStoreList.Selected.Rows.Count = 0 Then
        Else
            For Each row In Me.ugrdStoreList.Selected.Rows
                strStoreList = strStoreList & (row.Cells("Store_No").Value.ToString) & strStoreListSeparator
            Next
        End If

        If strStoreList.Length > 0 Then
            strStoreList = Mid(strStoreList, 1, strStoreList.Length - 1)
        End If

        sReportURL.Append("&Store_No_List=" & strStoreList)


        ' Store List Separator
        'sReportURL.Append("&StoreListSeparator=" & strStoreListSeparator)


        ' Subteam
        If cmbSubTeam.Text = "ALL" Or cmbSubTeam.Text = "" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
            'sReportURL.Append("&SubTeam_Name:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
            'sReportURL.Append("&SubTeam_Name=" & cmbSubTeam.Text)
        End If


        ' Vendor
        sReportURL.Append("&Vendor_ID=" & VB6.GetItemData(cmbVendor, cmbVendor.SelectedIndex))

        ' Identifieri
        If txtItemIdentifier.Text = "ALL" Or txtItemIdentifier.Text = "" Then
            sReportURL.Append("&Identifier:isnull=true")
        Else
            sReportURL.Append("&Identifier=" & Trim(txtItemIdentifier.Text.ToString))
        End If

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

        If ugrdStoreList.Selected.Rows.Count = 0 Then
            MsgBox("No stores have been selected.", MsgBoxStyle.Critical, Me.Text)
            GoTo NotOK
        End If

        Exit Function

NotOK:
        OKtoPrint = False

    End Function


    Private Sub SetCombos()

        SetActive(cmbZones, True)

    End Sub


    Private Sub StoreListGridCreateLoad(ByRef rsStores As DAO.Recordset, ByRef dtStores As DataTable, ByRef ugrdStoreList As Infragistics.Win.UltraWinGrid.UltraGrid)

        dtStores = New DataTable("StoreList")
        dtStores.Columns.Add(New DataColumn("Store_No", GetType(Integer)))
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
            row("Store_No") = rsStores.Fields("Store_No").Value
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