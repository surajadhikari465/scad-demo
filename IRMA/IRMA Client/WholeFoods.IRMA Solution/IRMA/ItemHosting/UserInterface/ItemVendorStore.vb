Option Strict Off
Option Explicit On

Imports System.Text
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Imports log4net

Friend Class frmItemVendorStore
	Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean

	Private m_colGrdItms As New Collection
	Private mbNoClick As Boolean
    Private mbFilling As Boolean

    Private mdt As DataTable
    Private mdv As DataView

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	Private Enum geStoreCol
		StoreNo = 0
		StoreName = 1
		ZoneID = 2
		State = 3
		WFMStore = 4
		MegaStore = 5
		CustomerType = 6
	End Enum
	
	Private Sub frmItemVendorStore_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmItemVendorStore_Load Entry")

        CenterForm(Me)
        Call LoadZone(cmbZones)
        Call SetupDataTable()
        Call LoadDataTable()
        Call LoadStates()
        Call SetCombos()

        If Not CheckAllStoreSelectionEnabled() Then
            _optSelection_1.Text = "All 365"
        End If

        logger.Debug("frmItemVendorStore_Load Exit")
		
    End Sub

    Private Sub cmbStates_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStates.SelectedIndexChanged
        logger.Debug("cmbStates_SelectedIndexChanged Entry")
        If mbFilling Or IsInitializing Then Exit Sub
        If cmbStates.SelectedIndex > -1 Then
            Call StoreListGridSelectByState(ugrdStoreList, cmbStates.Items(cmbStates.SelectedIndex))
        End If
        logger.Debug("cmbStates_SelectedIndexChanged Exit")
    End Sub

	Private Sub cmbZones_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbZones.SelectedIndexChanged
        logger.Debug("cmbZones_SelectedIndexChanged Entry")
        If mbFilling Or IsInitializing Then Exit Sub
        optSelection(geStoreCol.ZoneID).Checked = True
        OptSelection_CheckedChanged(optSelection.Item(2), New System.EventArgs())
        logger.Debug("cmbZones_SelectedIndexChanged Exit")
    End Sub
	
	Private Sub cmdApply_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdApply.Click
        logger.Debug("cmdApply_Click Entry")

        Dim iTotSelRows As Short
        Dim iCnt As Integer
        Dim isPerformSave As Boolean = False
        Dim newlyAuthedStores As New ArrayList
        Dim isPerformAuth As Boolean = False


        'roll through the grid and delete each highlighted item from StoreItemVendor
        iTotSelRows = ugrdStoreList.Selected.Rows.Count
        If iTotSelRows = 0 Then
            MsgBox(ResourcesItemHosting.GetString("SelectStoreAllow"))
            logger.Info(ResourcesItemHosting.GetString("SelectStoreAllow"))
        Else
            'loop through the selected stores to determine if any store/item combo is not already authorized 
            For iCnt = ugrdStoreList.Selected.Rows.Count - 1 To 0 Step -1
                'if store is not authorized, capture it so it will be displayed to user
                If Not StoreDAO.IsItemAuthorized(ugrdStoreList.Selected.Rows(iCnt).Cells("Store_no").Value, glItemID) Then
                    newlyAuthedStores.Add(ugrdStoreList.Selected.Rows(iCnt).Cells("Store_Name").Value)
                End If
            Next

            'if any exist, prompt user to confirm they wish to authorize this item/store combo
            If newlyAuthedStores.Count > 0 Then
                Dim msg As New StringBuilder
                msg.Append(ResourcesItemHosting.GetString("msg_confirm_AddVendorStore_AuthMsg1"))
                msg.Append(Environment.NewLine)
                msg.Append(Environment.NewLine)

                'append list of store/item values that are being authed
                Dim iterStore As IEnumerator = newlyAuthedStores.GetEnumerator()
                While iterStore.MoveNext
                    msg.Append("     * ")
                    msg.Append(iterStore.Current.ToString)
                    msg.Append(Environment.NewLine)
                End While

                msg.Append(Environment.NewLine)
                msg.Append(ResourcesItemHosting.GetString("msg_confirm_AddVendorStore_AuthMsg2"))

                'prompt user to confirm
                Dim result As DialogResult = MessageBox.Show(msg.ToString, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = Windows.Forms.DialogResult.Yes Then
                    ' Save the StoreItemVendor relationship and authorize the item for sale at the store
                    isPerformSave = True
                    isPerformAuth = True
                Else
                    ' Save the StoreItemVendor relationship, but do not authorize the item for sale at the store
                    isPerformSave = True
                    isPerformAuth = False
                End If
            Else
                ' Save the StoreItemVendor relationship, but do not authorize the item for sale at the store
                isPerformSave = True
                isPerformAuth = False
            End If

            If isPerformSave Then
                For iCnt = ugrdStoreList.Selected.Rows.Count - 1 To 0 Step -1
                    'activate store/item/vendor 
                    SQLExecute("EXEC InsertStoreItemVendor " & glVendorID & "," & ugrdStoreList.Selected.Rows(iCnt).Cells("Store_no").Value & "," & glItemID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    If Me.chkPrimary.CheckState = 1 Then
                        SQLExecute("EXEC SetPrimaryVendor " & ugrdStoreList.Selected.Rows(iCnt).Cells("Store_no").Value & ", " & glItemID & ", " & glVendorID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    End If

                    'if the user selected to automatically authorize this item, set the authorized flag for the store/item
                    If isPerformAuth Then
                        Dim itemStore As New ItemStoreBO
                        itemStore.ItemKey = glItemID
                        itemStore.StoreId = ugrdStoreList.Selected.Rows(iCnt).Cells("Store_no").Value
                        itemStore.Authorized = True
                        itemStore.RefreshPOSInfo = False
                        ItemDAO.UpdateItemStoreAuthorization(itemStore)
                    End If

                    ugrdStoreList.Selected.Rows(iCnt).Delete(False)
                Next iCnt
            End If
        End If

        Me.Close()
        logger.Debug("cmdApply_Click Exit")
    End Sub
	
    Private Sub LoadStates()
        logger.Debug("LoadStates Entry")
        Dim iLoop As Integer
        For iLoop = 1 To ugrdStoreList.Rows.Count - 1
            If Not (ugrdStoreList.Rows(iLoop).Cells("State").Value Is System.DBNull.Value) Then
                If Not StateInList(ugrdStoreList.Rows(iLoop).Cells("State").Value) And Trim(ugrdStoreList.Rows(iLoop).Cells("State").Value) <> "" Then
                    cmbStates.Items.Add((ugrdStoreList.Rows(iLoop).Cells("State").Value))
                End If
            End If
        Next iLoop
        logger.Debug("LoadStates Exit")
    End Sub
	
	Private Function StateInList(ByRef strState As String) As Boolean
        logger.Debug("StateInList Entry")
		Dim i As Short
		
		StateInList = False
		
		For i = 0 To cmbStates.Items.Count - 1
			If VB6.GetItemString(cmbStates, i) = strState Then
				StateInList = True
				Exit For
			End If
        Next i
        logger.Debug("StateInList Exit")
    End Function
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		Me.Close()
	End Sub
	
    Private Sub OptSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSelection.CheckedChanged

        logger.Debug("OptSelection_CheckedChanged Entry")

        If IsInitializing Then
            logger.Debug("OptSelection_CheckedChanged IsInitializing - Exit")
            Exit Sub
        End If

        If eventSender.Checked Then
            Dim Index As Short = optSelection.GetIndex(eventSender)
            Dim iFirstStore As Short

            Call SetCombos()

            If mbNoClick = True Then Exit Sub
            iFirstStore = -1

            Select Case Index
                Case 0
                    '-- Manual.
                    ugrdStoreList.Selected.Rows.Clear()
                    cmbZones.SelectedIndex = -1
                    cmbStates.SelectedIndex = -1

                    If ugrdStoreList.Rows.Count > 0 Then ugrdStoreList.ActiveRow = ugrdStoreList.Rows(0)
                Case 1
                    '-- All Stores or All 365 for RM
                    If CheckAllStoreSelectionEnabled() Then
                        Call StoreListGridSelectAll(ugrdStoreList, True)
                    Else
                        Call StoreListGridSelectAll365(ugrdStoreList)
                    End If
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

            ugrdStoreList.ActiveRow = Nothing

        End If

        logger.Debug("OptSelection_CheckedChanged Exit")
    End Sub
	
    Private Sub SetCombos()

        logger.Debug("SetCombos Entry")

        mbFilling = True

        'Zones.
        If optSelection(2).Checked = True Then
            cmbZones.Enabled = True
            cmbZones.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbZones.SelectedIndex = -1
            cmbZones.Enabled = False
            cmbZones.BackColor = System.Drawing.SystemColors.Control
        End If

        'States.
        If optSelection(3).Checked = True Then
            cmbStates.Enabled = True
            cmbStates.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbStates.SelectedIndex = -1
            cmbStates.Enabled = False
            cmbStates.BackColor = System.Drawing.SystemColors.Control
        End If

        mbFilling = False

        logger.Debug("SetCombos Exit")

    End Sub

    Private Sub SetupDataTable()

        logger.Debug("SetupDataTable Entry")

        mdt = New DataTable("Stores")
        'visible on grid
        mdt.Columns.Add(New DataColumn("Store_Name", GetType(String)))
        'hidden on grid
        mdt.Columns.Add(New DataColumn("Store_No", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Zone_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("State", GetType(String)))
        mdt.Columns.Add(New DataColumn("WFM_Store", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("Mega_Store", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("CustomerType", GetType(Integer)))

        logger.Debug("SetupDataTable Exit")
    End Sub

    Private Sub LoadDataTable()

        logger.Debug("LoadDataTable Entry")

        Dim rsStoreList As DAO.Recordset = Nothing
        Dim row As DataRow

        Try
            rsStoreList = SQLOpenRecordSet("EXEC GetAvailItemVendorStores " & glItemID & ", " & glVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough + DAO.RecordsetOptionEnum.dbForwardOnly)
            mdt.Rows.Clear()

            While Not rsStoreList.EOF
                row = mdt.NewRow
                row("Store_No") = rsStoreList.Fields("Store_No").Value
                row("Store_Name") = rsStoreList.Fields("Store_Name").Value
                row("Zone_ID") = rsStoreList.Fields("Zone_ID").Value
                row("State") = rsStoreList.Fields("State").Value
                row("WFM_Store") = rsStoreList.Fields("WFM_Store").Value
                row("Mega_Store") = rsStoreList.Fields("Mega_Store").Value
                row("CustomerType") = rsStoreList.Fields("CustomerType").Value
                mdt.Rows.Add(row)
                rsStoreList.MoveNext()
            End While
        Finally
            If rsStoreList IsNot Nothing Then
                rsStoreList.Close()
                rsStoreList = Nothing
            End If
        End Try
        mdt.AcceptChanges()
        mdv = New System.Data.DataView(mdt)

        ugrdStoreList.DataSource = mdv

        logger.Debug("LoadDataTable Exit")
    End Sub

    Private Sub ugrdStoreList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdStoreList.Click

        logger.Debug("ugrdStoreList_Click Entry")
        mbNoClick = True
        Me.optSelection(0).Checked = True
        mbNoClick = False
        logger.Debug("ugrdStoreList_Click Exit")
    End Sub

    ' for bug 5442: not being able to tab to the "By State" option button (_optSelection_3) from cboZones
    ' or tabbing from cmbStates to ugrdStoreList
    ' Rick Kelleher 3/4/08
    ' start
    Private CurrentKey As Integer

    Private Sub cmbZones_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles cmbZones.PreviewKeyDown
        CurrentKey = e.KeyValue
    End Sub

    Private Sub cmbZones_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbZones.Leave
        If CurrentKey = 9 Then
            _optSelection_3.Focus()
            CurrentKey = Nothing
        End If
    End Sub

    Private Sub cmbStates_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles cmbStates.PreviewKeyDown
        CurrentKey = e.KeyValue
    End Sub

    Private Sub cmbStates_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStates.Leave
        If CurrentKey = 9 Then
            ugrdStoreList.Focus()
            CurrentKey = Nothing
        End If
    End Sub
    ' for bug 5442: end

#Region "Tabbing code"
    ' Commented out by Rick Kelleher 2/27/08 while fixing bug 5442

    'Private Sub cmbStates_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStates.Leave
    '    ugrdStoreList.Focus()
    'End Sub

    'Private Sub cmbStates_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStates.LostFocus
    '    ugrdStoreList.Focus()
    'End Sub

    'Private Sub cmbZones_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbZones.Leave
    '    _optSelection_3.Focus()
    'End Sub

    'Private Sub cmbZones_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbZones.LostFocus
    '    _optSelection_3.Focus()
    'End Sub

    'Private Sub _optSelection_0_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_0.Leave
    '    _optSelection_5.Focus()
    'End Sub

    'Private Sub _optSelection_0_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_0.LostFocus
    '    _optSelection_5.Focus()
    'End Sub

    'Private Sub _optSelection_5_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_5.Leave
    '    _optSelection_4.Focus()
    'End Sub

    'Private Sub _optSelection_5_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_5.LostFocus
    '    _optSelection_4.Focus()
    'End Sub

    'Private Sub _optSelection_4_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_4.Leave
    '    _optSelection_1.Focus()
    'End Sub

    'Private Sub _optSelection_4_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_4.LostFocus
    '    _optSelection_1.Focus()
    'End Sub

    'Private Sub _optSelection_1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_1.Leave
    '    _optSelection_6.Focus()
    'End Sub

    'Private Sub _optSelection_1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_1.LostFocus
    '    _optSelection_6.Focus()
    'End Sub

    'Private Sub _optSelection_6_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_6.Leave
    '    _optSelection_2.Focus()
    'End Sub

    'Private Sub _optSelection_6_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_6.LostFocus
    '    _optSelection_2.Focus()
    'End Sub

    'Private Sub _optSelection_2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_2.Leave
    '    cmbZones.Enabled = True
    '    cmbZones.Focus()
    'End Sub

    'Private Sub _optSelection_2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_2.LostFocus
    '    cmbZones.Enabled = True
    '    cmbZones.Focus()
    'End Sub

    'Private Sub _optSelection_3_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_3.Leave
    '    cmbStates.Enabled = True
    '    cmbStates.Focus()
    'End Sub

    'Private Sub _optSelection_3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_3.LostFocus
    '    cmbStates.Enabled = True
    '    cmbStates.Focus()
    'End Sub

    'Private Sub ugrdStoreList_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdStoreList.Leave
    '    chkPrimary.Focus()
    'End Sub

    'Private Sub ugrdStoreList_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdStoreList.LostFocus
    '    chkPrimary.Focus()
    'End Sub

    'Private Sub chkPrimary_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPrimary.Leave
    '    cmdApply.Focus()
    'End Sub

    'Private Sub chkPrimary_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPrimary.LostFocus
    '    cmdApply.Focus()
    'End Sub

    'Private Sub cmdApply_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdApply.Leave
    '    cmdExit.Focus()
    'End Sub

    'Private Sub cmdApply_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdApply.LostFocus
    '    cmdExit.Focus()
    'End Sub

    'Private Sub cmdExit_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdExit.Leave
    '    _optSelection_0.Focus()
    'End Sub

    'Private Sub cmdExit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdExit.LostFocus
    '    _optSelection_0.Focus()
    'End Sub

#End Region

End Class