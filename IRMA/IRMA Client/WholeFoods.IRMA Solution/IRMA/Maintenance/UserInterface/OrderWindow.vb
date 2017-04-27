Imports log4net

Public Class OrderWindow
    Private mdtZone As DataTable
    Private mdtStore As DataTable
    Private mdtSubTeam As DataTable
    Private mdtOrderWindow As DataTable
    Private m_blnCheckChanged As Boolean
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub OrderWindow_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call SetupDataTables()
        Call RefreshGrid_Zone()
        Call RefreshGrid_Store()
        Call RefreshGrid_SubTeam()
        Call RefreshGrid_OrderWindow()

        Call SetPermissions()

        m_blnCheckChanged = False
    End Sub

    Private Sub SetupDataTables()

        logger.Debug("SetupDataTables Entry")

        mdtZone = New DataTable("Zones")
        mdtZone.Columns.Add(New DataColumn("Zone_Id", GetType(Integer)))
        mdtZone.Columns.Add(New DataColumn("Zone_Name", GetType(String)))

        mdtStore = New DataTable("Stores")
        mdtStore.Columns.Add(New DataColumn("Store_No", GetType(Integer)))
        mdtStore.Columns.Add(New DataColumn("Store_Name", GetType(String)))
        mdtStore.Columns.Add(New DataColumn("Zone_Id", GetType(Integer)))

        mdtSubTeam = New DataTable("SubTeams")
        mdtSubTeam.Columns.Add(New DataColumn("SubTeam_No", GetType(Integer)))
        mdtSubTeam.Columns.Add(New DataColumn("SubTeam_Name", GetType(String)))

        mdtOrderWindow = New DataTable("OrderWindow")
        mdtOrderWindow.Columns.Add(New DataColumn("Zone_Id", GetType(Integer)))
        mdtOrderWindow.Columns.Add(New DataColumn("Zone_Name", GetType(String)))
        mdtOrderWindow.Columns.Add(New DataColumn("SubTeam_No", GetType(Integer)))
        mdtOrderWindow.Columns.Add(New DataColumn("SubTeam_Name", GetType(String)))
        mdtOrderWindow.Columns.Add(New DataColumn("Store_No", GetType(Integer)))
        mdtOrderWindow.Columns.Add(New DataColumn("Store_Name", GetType(String)))
        mdtOrderWindow.Columns.Add(New DataColumn("OrderStart", GetType(String)))
        mdtOrderWindow.Columns.Add(New DataColumn("OrderEnd", GetType(String)))

        logger.Debug("SetupDataTables Exit")
    End Sub

    Public Sub RefreshGrid_Zone()
        logger.Debug("RefreshGrid_Zone Entry")

        Dim rsZone As DAO.Recordset = Nothing
        Dim row As DataRow

        mdtZone.Clear()

        Try
            '-- Set up the databound stuff
            rsZone = SQLOpenRecordSet("EXEC GetZones", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            While Not rsZone.EOF
                row = mdtZone.NewRow
                row("Zone_Id") = rsZone.Fields("Zone_Id").Value
                row("Zone_Name") = rsZone.Fields("Zone_Name").Value
                mdtZone.Rows.Add(row)
                rsZone.MoveNext()
            End While

        Finally
            If rsZone IsNot Nothing Then
                rsZone.Close()
                rsZone = Nothing
            End If
        End Try

        mdtZone.AcceptChanges()
        ugrdZone.DataSource = mdtZone

        If ugrdZone.Rows.Count > 0 Then
            ugrdZone.Rows(0).Selected = True
        End If

        ugrdZone.DisplayLayout.Bands(0).Columns("Zone_Id").Hidden = True
        ugrdZone.DisplayLayout.Bands(0).Columns("Zone_Name").Header.Caption = "Zone Name"

        logger.Debug("RefreshGrid_Zone Exit")
    End Sub

    Public Sub RefreshGrid_Store()
        logger.Debug("RefreshGrid_Store Entry")

        Dim rsStore As DAO.Recordset = Nothing
        Dim row As DataRow

        mdtStore.Clear()

        Try
            '-- Set up the databound stuff
            rsStore = SQLOpenRecordSet("EXEC GetDistAndMfg", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            While Not rsStore.EOF
                row = mdtStore.NewRow
                row("Store_No") = rsStore.Fields("Store_No").Value
                row("Store_Name") = rsStore.Fields("Store_Name").Value
                row("Zone_Id") = rsStore.Fields("Zone_Id").Value
                mdtStore.Rows.Add(row)
                rsStore.MoveNext()
            End While

        Finally
            If rsStore IsNot Nothing Then
                rsStore.Close()
                rsStore = Nothing
            End If
        End Try

        mdtStore.AcceptChanges()
        ugrdStore.DataSource = mdtStore

        If mdtStore.Rows.Count > 0 Then
            ugrdStore.DisplayLayout.Bands(0).Columns("Store_No").Hidden = True
            ugrdStore.DisplayLayout.Bands(0).Columns("Zone_Id").Hidden = True
            ugrdStore.DisplayLayout.Bands(0).Columns("Store_Name").Header.Caption = "Store Name"
            ugrdStore.DisplayLayout.Bands(0).SortedColumns.Add("Store_Name", False, False)
            ugrdStore.ActiveRow = ugrdStore.Rows(0)
            ugrdStore.Rows(0).Selected = True
        End If

        logger.Debug("RefreshGrid_Store Exit")
    End Sub

    Public Sub RefreshGrid_SubTeam()
        logger.Debug("RefreshGrid_SubTeam Entry")

        Dim rsSubTeam As DAO.Recordset = Nothing
        Dim row As DataRow

        mdtSubTeam.Clear()

        Try
            '-- Set up the databound stuff
            rsSubTeam = SQLOpenRecordSet("EXEC GetEXEDistSubTeams", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            While Not rsSubTeam.EOF
                row = mdtSubTeam.NewRow
                row("SubTeam_No") = rsSubTeam.Fields("SubTeam_No").Value
                row("SubTeam_Name") = rsSubTeam.Fields("SubTeam_Name").Value
                mdtSubTeam.Rows.Add(row)
                rsSubTeam.MoveNext()
            End While

        Finally
            If rsSubTeam IsNot Nothing Then
                rsSubTeam.Close()
                rsSubTeam = Nothing
            End If
        End Try

        mdtSubTeam.AcceptChanges()
        ugrdSubTeam.DataSource = mdtSubTeam

        If ugrdSubTeam.Rows.Count > 0 Then
            ugrdSubTeam.Rows(0).Selected = True
        End If

        ugrdSubTeam.DisplayLayout.Bands(0).Columns("SubTeam_No").Hidden = True
        ugrdSubTeam.DisplayLayout.Bands(0).Columns("SubTeam_Name").Header.Caption = "SubTeam Name"

        logger.Debug("RefreshGrid_SubTeam Exit")
    End Sub

    Public Sub RefreshGrid_OrderWindow()
        logger.Debug("RefreshGrid_OrderWindow Entry")

        Dim rsOrderWindow As DAO.Recordset = Nothing
        Dim row As DataRow

        mdtOrderWindow.Clear()

        Try
            '-- Set up the databound stuff
            rsOrderWindow = SQLOpenRecordSet("EXEC GetOrderWindow", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            While Not rsOrderWindow.EOF
                row = mdtOrderWindow.NewRow
                row("Zone_Id") = rsOrderWindow.Fields("Zone_Id").Value
                row("Zone_Name") = rsOrderWindow.Fields("Zone_Name").Value
                row("SubTeam_No") = rsOrderWindow.Fields("SubTeam_No").Value
                row("SubTeam_Name") = rsOrderWindow.Fields("SubTeam_Name").Value
                row("Store_No") = rsOrderWindow.Fields("Store_No").Value
                row("Store_Name") = rsOrderWindow.Fields("Store_Name").Value
                row("OrderStart") = VB6.Format(rsOrderWindow.Fields("OrderStart").Value, "hh:mm AMPM")
                row("OrderEnd") = VB6.Format(rsOrderWindow.Fields("OrderEnd").Value, "hh:mm AMPM")

                mdtOrderWindow.Rows.Add(row)
                rsOrderWindow.MoveNext()
            End While

        Finally
            If rsOrderWindow IsNot Nothing Then
                rsOrderWindow.Close()
                rsOrderWindow = Nothing
            End If
        End Try

        mdtOrderWindow.AcceptChanges()
        ugrdOrderWindow.DataSource = mdtOrderWindow

        If ugrdOrderWindow.Rows.Count > 0 Then
            ugrdOrderWindow.Rows(0).Selected = True
        End If

        ugrdOrderWindow.DisplayLayout.Bands(0).Columns("Zone_Id").Hidden = True
        ugrdOrderWindow.DisplayLayout.Bands(0).Columns("Store_No").Hidden = True
        ugrdOrderWindow.DisplayLayout.Bands(0).Columns("SubTeam_No").Hidden = True

        ugrdOrderWindow.DisplayLayout.Bands(0).Columns("Zone_Name").Header.Caption = "Zone Name"
        ugrdOrderWindow.DisplayLayout.Bands(0).Columns("SubTeam_Name").Header.Caption = "SubTeam Name"
        ugrdOrderWindow.DisplayLayout.Bands(0).Columns("Store_Name").Header.Caption = "Store Name"
        ugrdOrderWindow.DisplayLayout.Bands(0).Columns("OrderStart").Header.Caption = "Order Window Open"
        ugrdOrderWindow.DisplayLayout.Bands(0).Columns("OrderEnd").Header.Caption = "Order Window Close"

        logger.Debug("RefreshGrid_SubTeam Exit")
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Call FilterGrid()
    End Sub

    Private Sub FilterGrid()
        Dim mdv As DataView
        Dim sWhere As String

        If Not m_blnCheckChanged Then
            Call RefreshGrid_OrderWindow()
        End If

        mdv = New System.Data.DataView(mdtOrderWindow)

        sWhere = ""

        If ugrdZone.Selected.Rows.Count > 0 Then
            sWhere = "Zone_Id IN (" & GetFilterString("ZONE") & ")"
        End If

        If ugrdSubTeam.Selected.Rows.Count > 0 Then
            If sWhere <> "" Then
                sWhere = sWhere & " AND SubTeam_No IN (" & GetFilterString("SUBTEAM") & ")"
            Else
                sWhere = "SubTeam_No IN (" & GetFilterString("SUBTEAM") & ")"
            End If
        End If

        If ugrdStore.Selected.Rows.Count > 0 Then
            If sWhere <> "" Then
                sWhere = sWhere & " AND Store_No IN (" & GetFilterString("STORE") & ")"
            Else
                sWhere = "Store_No IN (" & GetFilterString("STORE") & ")"
            End If
        End If

        mdv.RowFilter = sWhere
        Me.ugrdOrderWindow.DisplayLayout.Bands(0).SortedColumns.Clear()
        Me.ugrdOrderWindow.DataSource = mdv
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim rowZone As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim rowStore As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim rowSubTeam As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim sZoneList As String
        Dim sSubTeamList As String
        Dim sStoreList As String
        Dim sResults As String

        sZoneList = ""
        sSubTeamList = ""
        sStoreList = ""

        For Each rowZone In ugrdZone.Selected.Rows
            If sZoneList = "" Then
                sZoneList = rowZone.Cells(0).Value.ToString
            Else
                sZoneList = sZoneList & "|" & rowZone.Cells(0).Value.ToString
            End If
        Next

        For Each rowStore In ugrdStore.Selected.Rows
            If sStoreList = "" Then
                sStoreList = rowStore.Cells(0).Value.ToString
            Else
                sStoreList = sStoreList & "|" & rowStore.Cells(0).Value.ToString
            End If
        Next

        For Each rowSubTeam In ugrdSubTeam.Selected.Rows
            If sSubTeamList = "" Then
                sSubTeamList = rowSubTeam.Cells(0).Value.ToString
            Else
                sSubTeamList = sSubTeamList & "|" & rowSubTeam.Cells(0).Value.ToString
            End If
        Next

        If sZoneList = "" Then
            MsgBox("At least one Zone must be selected", MsgBoxStyle.Critical, "Order Window Maintenance")
            Exit Sub
        End If


        If sStoreList = "" Then
            MsgBox("At least one Store must be selected", MsgBoxStyle.Critical, "Order Window Maintenance")
            Exit Sub
        End If

        If sSubTeamList = "" Then
            MsgBox("At least one SubTeam must be selected", MsgBoxStyle.Critical, "Order Window Maintenance")
            Exit Sub
        End If


        sResults = IsOrderWindowOpen(sZoneList, sSubTeamList, sStoreList)
        If sResults <> "" Then
            MsgBox("Your changes were not applied because some Zone/Store/SubTeam combinations have the order window open.  Please remove them from the list and try again.", MsgBoxStyle.Critical, "Order Window Maintenance")
        Else
            SQLExecute("EXEC InsertUpdateOrderWindow '" & sZoneList & "', '" & sStoreList & "', '" & sSubTeamList & "', '" & dtpOrderStart.Value.ToShortTimeString & "', '" & dtpOrderClose.Value.ToShortTimeString & "', '" & dtpOrderClose.Value.ToShortTimeString & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
            RefreshGrid_OrderWindow()
            FilterGrid()
        End If
    End Sub

    Private Sub chkAllZone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAllZone.CheckedChanged
        m_blnCheckChanged = True

        ugrdZone.Selected.Rows.Clear()
        Call StoreListGridSelectAll(ugrdZone, chkAllZone.Checked)

        m_blnCheckChanged = False
        FilterGrid()
    End Sub

    Private Sub chkAllStore_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAllStore.CheckedChanged
        m_blnCheckChanged = True

        ugrdStore.Selected.Rows.Clear()
        Call StoreListGridSelectAll(ugrdStore, chkAllStore.Checked)

        m_blnCheckChanged = False
        FilterGrid()
    End Sub

    Private Sub chkAllSubTeam_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAllSubTeam.CheckedChanged
        m_blnCheckChanged = True

        ugrdSubTeam.Selected.Rows.Clear()
        Call StoreListGridSelectAll(ugrdSubTeam, chkAllSubTeam.Checked)

        m_blnCheckChanged = False
        FilterGrid()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim rowOrderWindow As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim sZoneList As String
        Dim sSubTeamList As String
        Dim sStoreList As String
        Dim sresults As String

        sZoneList = ""
        sSubTeamList = ""
        sStoreList = ""

        For Each rowOrderWindow In ugrdOrderWindow.Selected.Rows
            If sZoneList = "" Then
                sZoneList = rowOrderWindow.Cells(0).Value.ToString
            Else
                sZoneList = sZoneList & "|" & rowOrderWindow.Cells(0).Value.ToString
            End If

            If sSubTeamList = "" Then
                sSubTeamList = rowOrderWindow.Cells(2).Value.ToString
            Else
                sSubTeamList = sSubTeamList & "|" & rowOrderWindow.Cells(2).Value.ToString
            End If

            If sStoreList = "" Then
                sStoreList = rowOrderWindow.Cells(4).Value.ToString
            Else
                sStoreList = sStoreList & "|" & rowOrderWindow.Cells(4).Value.ToString
            End If
        Next

        If ugrdOrderWindow.Selected.Rows.Count = 0 Then
            MsgBox("At least one row must be selected to be deleted.", MsgBoxStyle.Critical, "Order Window Maintenance")
            Exit Sub
        End If

        sresults = IsOrderWindowOpen(sZoneList, sSubTeamList, sStoreList)
        If sresults <> "" Then
            MsgBox("The items cannot be deleted because some Zone/Store/SubTeam combinations have the order window open.  Please remove them from the list and try again.", MsgBoxStyle.Critical, "Order Window Maintenance")
        Else
            SQLExecute("EXEC DeleteOrderWindowEntry '" & sZoneList & "', '" & sStoreList & "', '" & sSubTeamList & "', " & chkAllOrderWindow.Checked, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            RefreshGrid_OrderWindow()
            FilterGrid()
        End If
    End Sub

    Private Function IsOrderWindowOpen(ByVal sZoneList As String, ByVal sSubteamList As String, ByVal sStoreList As String) As String
        Dim rs As DAO.Recordset = Nothing

        rs = SQLOpenRecordSet("SELECT dbo.fn_IsOrderWindowOpen('" & sZoneList & "', '" & sSubteamList & "', '" & sStoreList & "')", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        IsOrderWindowOpen = rs.Fields(0).Value.ToString & ""
    End Function

    Private Function ParseMessage(ByVal sResults As String) As String
        Dim aryResult() As String
        Dim aryData() As String
        Dim sMsg As String
        Dim x As Integer

        aryResult = Split(sResults, ",")
        sMsg = ""

        For x = 0 To UBound(aryResult)
            aryData = Split(aryResult(x), "|")
            If aryData(0) <> "" Then
                If sMsg = "" Then
                    sMsg = "Zone: " & Trim(aryData(0)) & ", SubTeam: " & Trim(aryData(1)) & ", Store: " & Trim(aryData(2))
                Else
                    sMsg = sMsg & vbCrLf & "Zone: " & Trim(aryData(0)) & ", SubTeam: " & Trim(aryData(1)) & ", Store: " & Trim(aryData(2))
                End If
            End If
        Next

        ParseMessage = sMsg
    End Function

    Private Sub chkAllOrderWindow_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAllOrderWindow.CheckedChanged
        ugrdOrderWindow.Selected.Rows.Clear()
        Call StoreListGridSelectAll(ugrdOrderWindow, chkAllOrderWindow.Checked)
    End Sub

    Private Sub SetPermissions()
        SetActive(dtpOrderStart, (gbDCAdmin Or gbSuperUser))
        SetActive(dtpOrderClose, (gbDCAdmin Or gbSuperUser))
        SetActive(cmdSave, (gbDCAdmin Or gbSuperUser))
        SetActive(cmdDelete, (gbDCAdmin Or gbSuperUser))
    End Sub

    Private Function GetFilterString(ByVal sType As String) As String
        Dim sFilter As String
        Dim rowZone As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim rowStore As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim rowSubTeam As Infragistics.Win.UltraWinGrid.UltraGridRow

        sFilter = ""

        Select Case sType
            Case "ZONE"
                For Each rowZone In ugrdZone.Selected.Rows
                    If sFilter = "" Then
                        sFilter = rowZone.Cells(0).Value.ToString
                    Else
                        sFilter = sFilter & "," & rowZone.Cells(0).Value.ToString
                    End If
                Next

            Case "STORE"
                For Each rowStore In ugrdStore.Selected.Rows
                    If sFilter = "" Then
                        sFilter = rowStore.Cells(0).Value.ToString
                    Else
                        sFilter = sFilter & "," & rowStore.Cells(0).Value.ToString
                    End If
                Next

            Case "SUBTEAM"
                For Each rowSubTeam In ugrdSubTeam.Selected.Rows
                    If sFilter = "" Then
                        sFilter = rowSubTeam.Cells(0).Value.ToString
                    Else
                        sFilter = sFilter & "," & rowSubTeam.Cells(0).Value.ToString
                    End If
                Next
        End Select

        GetFilterString = sFilter
    End Function

    Private Sub ugrdStore_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles ugrdStore.AfterSelectChange
        FilterGrid()
    End Sub

    Private Sub ugrdSubTeam_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles ugrdSubTeam.AfterSelectChange
        FilterGrid()
    End Sub

    Private Sub ugrdZone_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles ugrdZone.AfterSelectChange
        UpdateStoresGrid()
        FilterGrid()
    End Sub

    Private Sub UpdateStoresGrid()
        Dim rowZone As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim rowStore As Infragistics.Win.UltraWinGrid.UltraGridRow

        For Each rowZone In ugrdZone.Selected.Rows
            For Each rowStore In ugrdStore.Rows
                If rowZone.Cells(0).Text = rowStore.Cells(2).Text Then
                    rowStore.Hidden = False
                    rowStore.Selected = False
                Else
                    rowStore.Hidden = True
                    rowStore.Selected = False
                End If
            Next
        Next

        For Each rowStore In ugrdStore.Rows
            If rowStore.Hidden = False Then
                rowStore.Selected = True
                Exit For
            End If
        Next
    End Sub

    Private Sub ugrdStore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdStore.Click
        chkAllStore.Checked = False
    End Sub

    Private Sub ugrdSubTeam_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdSubTeam.Click
        chkAllSubTeam.Checked = False
    End Sub

    Private Sub ugrdZone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdZone.Click
        chkAllZone.Checked = False
    End Sub
End Class