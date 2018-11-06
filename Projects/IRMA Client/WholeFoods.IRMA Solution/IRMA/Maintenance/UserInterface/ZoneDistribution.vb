Option Strict Off
Option Explicit On
Imports log4net

Friend Class frmZoneDistribution
    Inherits System.Windows.Forms.Form

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
	Private Enum geToZoneCol
		ZoneID = 0
		ZoneName = 1
		SubTeamID = 2
		SubTeamName = 3
		SupplierID = 4
		supplierName = 5
	End Enum
	
	Private Enum geSubTeamSupplier
		SubTeamID = 0
		SubTeamName = 1
		SupplierID = 2
		supplierName = 3
	End Enum

    Private msToZoneName As String
    Private mrsStoreList As ADODB.Recordset
    Private mvSubTeams(1, 0) As String
    Private mvSuppliers(1, 0) As String
    Private mvCurrentSelections(1, 0) As String

    Private mdtToZone As DataTable
    Private mdtSubTeamSupplier As DataTable

	Private Sub frmZoneDistribution_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmZoneDistribution_Load Entry")

        SetupDataTables()
		
        LoadAllData()
        PopulateSubTeamSupplier()
		
        LoadSupplierList()

        logger.Debug("frmZoneDistribution_Load Exit")

		
	End Sub

    Private Sub SetupDataTables()
        ' Create a data table

        logger.Debug("SetupDataTables Entry")

        mdtToZone = New DataTable("ToZone")

        'Visible on grid.
        '--------------------
        mdtToZone.Columns.Add(New DataColumn("ZoneName", GetType(String)))

        'Hidden.
        '--------------------
        mdtToZone.Columns.Add(New DataColumn("ZoneID", GetType(Integer)))
        mdtToZone.Columns.Add(New DataColumn("SubTeamID", GetType(String)))
        mdtToZone.Columns.Add(New DataColumn("SubTeamName", GetType(String)))
        mdtToZone.Columns.Add(New DataColumn("SupplierID", GetType(String)))
        mdtToZone.Columns.Add(New DataColumn("SupplierName", GetType(String)))

        ' Create a data table
        mdtSubTeamSupplier = New DataTable("SubTeamSupplier")

        'Visible on grid.
        '--------------------
        mdtSubTeamSupplier.Columns.Add(New DataColumn("SubTeamName", GetType(String)))
        mdtSubTeamSupplier.Columns.Add(New DataColumn("SupplierName", GetType(String)))

        'Hidden.
        '--------------------
        mdtSubTeamSupplier.Columns.Add(New DataColumn("SubTeamID", GetType(Integer)))
        mdtSubTeamSupplier.Columns.Add(New DataColumn("SupplierID", GetType(Integer)))

        logger.Debug("SetupDataTables Exit")


    End Sub


	Private Sub LoadAllData()

        logger.Debug("LoadAllData Entry")

        Dim rsStoreList As DAO.Recordset = Nothing
        Dim sZoneName As String = String.Empty
        Dim sZonePrevName As String = String.Empty
        sZonePrevName = String.Empty
        Dim row As DataRow

        mdtToZone.Clear()

        Try
            rsStoreList = SQLOpenRecordSet("EXEC GetZoneSubTeams", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            While Not rsStoreList.EOF
                sZoneName = rsStoreList.Fields("Zone_Name").Value

                If sZoneName <> sZonePrevName Then
                    row = mdtToZone.NewRow
                    row("ZoneID") = rsStoreList.Fields("Zone_ID").Value
                    row("ZoneName") = rsStoreList.Fields("Zone_Name").Value
                    row("SubTeamID") = rsStoreList.Fields("SubTeam_No").Value
                    row("SubTeamName") = rsStoreList.Fields("SubTeam_Name").Value
                    row("SupplierID") = rsStoreList.Fields("Supplier_Store_No").Value
                    row("SupplierName") = rsStoreList.Fields("Store_Name").Value

                    mdtToZone.Rows.Add(row)

                    sZonePrevName = sZoneName

                End If

                rsStoreList.MoveNext()
            End While

            'Copy the DAO recordset to an ADO recordset so we can keep the data around.
            rsStoreList.MoveFirst()
            Call CreateEmptyADORS_FromDAO(rsStoreList, mrsStoreList, False)
            mrsStoreList.Open()

            Do While Not rsStoreList.EOF
                Call CopyDAORecordToADORecord(rsStoreList, mrsStoreList, False)
                rsStoreList.MoveNext()
            Loop

            rsStoreList.Close()
            rsStoreList = Nothing

            mdtToZone.AcceptChanges()
            ugrdToZones.DataSource = mdtToZone

            If ugrdToZones.Rows.Count > 0 Then
                'Set the first item to selected.
                ugrdToZones.Rows(0).Selected = True
            End If
        Finally
            If rsStoreList IsNot Nothing Then
                rsStoreList.Close()
            End If
        End Try

        logger.Debug("LoadAllData Exit")
    End Sub
	
    Private Sub PopulateSubTeamSupplier()

        logger.Debug("PopulateSubTeamSupplier Entry")

        Dim lCurrZoneID As Integer
        Dim row As DataRow
        msToZoneName = String.Empty

        GetSelectedZoneInfo(lCurrZoneID, msToZoneName)

        mdtSubTeamSupplier.Clear()

        'Walk the ado recordset (used to store data) and populate the SubTeam / Supplier grid.
        mrsStoreList.MoveFirst()
        Do While Not mrsStoreList.EOF
            If mrsStoreList.Fields("Zone_ID").Value = lCurrZoneID And mrsStoreList.Fields("Store_Name").Value <> "" Then

                row = mdtSubTeamSupplier.NewRow
                row("SubTeamID") = mrsStoreList.Fields("SubTeam_No").Value
                row("SubTeamName") = mrsStoreList.Fields("SubTeam_Name").Value
                row("SupplierID") = mrsStoreList.Fields("Supplier_Store_No").Value
                row("SupplierName") = mrsStoreList.Fields("Store_Name").Value

                mdtSubTeamSupplier.Rows.Add(row)

            End If
            mrsStoreList.MoveNext()
        Loop

        mdtSubTeamSupplier.AcceptChanges()
        ugrdSubTeamSupplier.DataSource = mdtSubTeamSupplier

        If ugrdSubTeamSupplier.Rows.Count > 0 Then
            'Set the first item to selected.
            ugrdSubTeamSupplier.Rows(0).Selected = True
        End If

        SetDeleteKey()

        logger.Debug("PopulateSubTeamSupplier Exit")


    End Sub
	
    Private Sub ugrdSubTeamSupplier_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdSubTeamSupplier.Click
        SetDeleteKey()
    End Sub

    Private Sub ugrdSubTeamSupplier_CellChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles ugrdSubTeamSupplier.CellChange
        SetDeleteKey()
    End Sub

    Private Sub SetDeleteKey()
        logger.Debug("SetDeleteKey Entry")

        If ugrdSubTeamSupplier.Selected.Rows.Count > 0 Then
            cmdDelete.Enabled = True
        Else
            cmdDelete.Enabled = False
        End If

        logger.Debug("SetDeleteKey Exit")


    End Sub
	
    Private Sub ugrdToZones_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdToZones.Click
        PopulateSubTeamSupplier()
    End Sub

    Private Sub ugrdToZones_CellChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles ugrdToZones.CellChange
        PopulateSubTeamSupplier()
    End Sub
	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        logger.Debug("cmdAdd_Click Entry")


		Dim lToZoneID As Integer
		
		Call LoadSubTeamList()
		Call LoadCurrentSelectionList()
		Call GetSelectedZoneInfo(lToZoneID)
		
        If (UBound(mvSuppliers, 2) = 0 And mvSuppliers(0, 0) = "") Then
            MsgBox("There are no valid suppliers (distribution centers or manufacturers) for this zone.", MsgBoxStyle.Exclamation)
            logger.Info("There are no valid suppliers (distribution centers or manufacturers) for this zone.")
        Else
            If frmZoneDistributionEdit.LoadForm((msToZoneName), lToZoneID, mvSubTeams, mvSuppliers, mvCurrentSelections) Then
                'Change was made, update the recordset used for storage.
                Call UpdateSubTeamSupplierList()
            Else
                Erase mvSubTeams
                Erase mvCurrentSelections
            End If
        End If

        logger.Debug("cmdAdd_Click Exit")
		
	End Sub
	
    Private Sub LoadSupplierList()

        logger.Debug("LoadSupplierList Entry")

        '-------------------------------------
        ' Purpose: Get list of suppliers from DB.
        '-------------------------------------
        Dim i As Short
        Dim rsStoreList As DAO.Recordset = Nothing

        Try
            rsStoreList = SQLOpenRecordSet("EXEC GetDistAndMfg", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            i = 0
            While Not rsStoreList.EOF

                ReDim Preserve mvSuppliers(1, i)
                mvSuppliers(0, i) = rsStoreList.Fields("Store_No").Value
                mvSuppliers(1, i) = rsStoreList.Fields("Store_Name").Value

                rsStoreList.MoveNext()
                i = i + 1
            End While
            rsStoreList.Close()
            rsStoreList = Nothing

        Finally
            If rsStoreList IsNot Nothing Then
                rsStoreList.Close()
            End If
        End Try

        logger.Debug("LoadSupplierList Exit")


    End Sub
	
    Private Sub LoadSubTeamList()

        logger.Debug("LoadSubTeamList Entry")

        '---------------------------------------------------------------
        ' Purpose: To provide a list of subteams for the selectd "To Zone".
        '---------------------------------------------------------------
        Dim lCurrZoneID As Integer
        Dim i As Short

        i = 0

        Call GetSelectedZoneInfo(lCurrZoneID)

        'Walk the ado recordset and find the list of appropriate sub teams.
        mrsStoreList.MoveFirst()
        Do While Not mrsStoreList.EOF

            'Include all subteams for this zone.
            If mrsStoreList.Fields("Zone_ID").Value = lCurrZoneID Then

                ReDim Preserve mvSubTeams(1, i)
                mvSubTeams(0, i) = mrsStoreList.Fields("SubTeam_No").Value
                mvSubTeams(1, i) = mrsStoreList.Fields("SubTeam_Name").Value
                i = i + 1

            End If
            mrsStoreList.MoveNext()
        Loop

        logger.Debug("LoadSubTeamList Exit")



    End Sub
	
    Private Sub LoadCurrentSelectionList()

        logger.Debug("LoadCurrentSelectionList Entry")

        '------------------------------------------------------------------
        ' Purpose:  To provide a list of currently selected subteam / suppliers.
        '               This is sent to the edit form and used when entering new
        '               or editing  subteam / suppliers. It allows the form to
        '               notify the user if their selection is already in the list.
        '------------------------------------------------------------------
        Dim i As Short
        Dim lCurrZoneID As Integer

        i = 0

        Call GetSelectedZoneInfo(lCurrZoneID)

        'Walk the ado recordset and find the list of appropriate sub teams.
        mrsStoreList.MoveFirst()
        Do While Not mrsStoreList.EOF

            'Include all subteams for this zone.
            If mrsStoreList.Fields("Zone_ID").Value = lCurrZoneID And mrsStoreList.Fields("Store_Name").Value <> "" Then

                ReDim Preserve mvCurrentSelections(1, i)
                mvCurrentSelections(0, i) = mrsStoreList.Fields("SubTeam_No").Value
                mvCurrentSelections(1, i) = mrsStoreList.Fields("Supplier_Store_No").Value
                i = i + 1

            End If
            mrsStoreList.MoveNext()
        Loop

        logger.Debug("LoadCurrentSelectionList Exit")

    End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click

        logger.Debug("cmdDelete_Click Entry")

		Dim iDelCnt As Short
		Dim intAns As Short
		Dim lZoneID As Integer
		Dim lSubTeamID As Integer
		Dim lSupplierID As Integer

		intAns = MsgBox("Delete the selected SubTeam / Supplier(s)?", MsgBoxStyle.OKCancel, "Delete SubTeam / Supplier")
		
        If intAns = MsgBoxResult.Ok Then

            logger.Info("Delete the selected SubTeam / Supplier(s)?-OK")

            Call GetSelectedZoneInfo(lZoneID, "")

            For iDelCnt = 0 To ugrdSubTeamSupplier.Selected.Rows.Count - 1
                GetSelectedSubTeamSupplierInfo(iDelCnt, lSubTeamID, lSupplierID)
                SQLExecute("EXEC DeleteZoneSubTeam " & lZoneID & "," & lSubTeamID & "," & lSupplierID, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            Next iDelCnt

        End If
		
        UpdateSubTeamSupplierList()

        logger.Debug("cmdDelete_Click Exit")

		
	End Sub
	
	Private Sub GetSelectedZoneInfo(ByRef lZoneID As Integer, Optional ByRef sZoneName As String = "")
		'-----------------------------------------------------------------------------------
		' Purpose:  Determines the selected zone info (ID and name) and returns by reference.
        '-----------------------------------------------------------------------------------

        logger.Debug("GetSelectedZoneInfo Entry")

        'Set the selected ZoneID and name.
        If ugrdToZones.Selected.Rows.Count > 0 Then
            lZoneID = ugrdToZones.Selected.Rows(0).Cells("ZoneID").Value
            sZoneName = ugrdToZones.Selected.Rows(0).Cells("ZoneName").Value
        End If

        logger.Debug("GetSelectedZoneInfo Exit")



	End Sub
	
    Private Sub GetSelectedSubTeamSupplierInfo(ByVal selectedRowNum As Integer, ByRef lSubTeamID As Integer, ByRef lSupplierID As Object)
        '------------------------------------------------------------------------------------------
        ' Purpose:  Determines the selected SubTeamID and SupplierID and returns them by reference.
        '------------------------------------------------------------------------------------------	

        logger.Debug("GetSelectedSubTeamSupplierInfo Entry")

        lSubTeamID = ugrdSubTeamSupplier.Selected.Rows(selectedRowNum).Cells("SubTeamID").Value
        If IsNumeric(ugrdSubTeamSupplier.Selected.Rows(selectedRowNum).Cells("SupplierID").Value) Then
            lSupplierID = ugrdSubTeamSupplier.Selected.Rows(selectedRowNum).Cells("SupplierID").Value
        Else
            lSupplierID = 0
        End If

        logger.Debug("GetSelectedSubTeamSupplierInfo Exit")


    End Sub
	
    Private Sub UpdateSubTeamSupplierList()

        logger.Debug("UpdateSubTeamSupplierList Entry")


        Call LoadAllData()

        Call PopulateSubTeamSupplier()

        logger.Debug("UpdateSubTeamSupplierList Exit")


    End Sub
	
	Private Sub frmZoneDistribution_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        logger.Debug("frmZoneDistribution_FormClosed Entry")

		mrsStoreList = Nothing
		Erase mvSubTeams
		Erase mvSuppliers
        Erase mvCurrentSelections

        logger.Debug("frmZoneDistribution_FormClosed Exit")

		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        logger.Debug("cmdExit_Click Entry")


        Me.Close()

        logger.Debug("cmdExit_Click Exit")
		
	End Sub
End Class