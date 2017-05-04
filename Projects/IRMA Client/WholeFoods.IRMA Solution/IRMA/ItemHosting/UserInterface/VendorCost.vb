Option Strict Off
Option Explicit On

Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.Utility
Imports log4net
Imports WholeFoods.IRMA.Ordering.DataAccess

Friend Class frmVendorCost
    Inherits System.Windows.Forms.Form

    Private mdt As DataTable
    Private mdv As DataView

    Private IsInitializing As Boolean
    Public plItem_Key As Integer
    Private poItemInfo As ItemBO
    Public poVendorInfo As VendorBO
    Private _hasRunSearch As Boolean
    Private _ignoreCasePackUpdates As Boolean

    Dim sOrig_Item_ID As String
    Dim iVendorItemStatus As Integer
    Const END_DATE_COL As Short = 6
    Const PRIMARY_VENDOR_COL As Short = 7
    Const VENDOR_COST_HISTORY_KEY_COL As Short = 8
    Const STORE_NO_COL As Short = 9

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub SetupDataTable()
        logger.Debug("SetupDataTable Entry")

        ' Create a data table
        mdt = New DataTable("VendCost")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("Store_Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("FromVendor", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("Type", GetType(String)))
        mdt.Columns.Add(New DataColumn("UnitCost", GetType(String)))
        mdt.Columns.Add(New DataColumn("NetDiscount", GetType(String)))
        mdt.Columns.Add(New DataColumn("NetCost", GetType(String)))
        mdt.Columns.Add(New DataColumn("VendorPack", GetType(String)))
        mdt.Columns.Add(New DataColumn("UnitFreight", GetType(String)))
        mdt.Columns.Add(New DataColumn("StartDate", GetType(Date)))
        mdt.Columns.Add(New DataColumn("EndDate", GetType(Date)))
        mdt.Columns.Add(New DataColumn("PrimaryVendor", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("CostUnit_Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("FreightUnit_Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("Package_Desc1", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Currency", GetType(String)))
        mdt.Columns.Add(New DataColumn("Insert_Date", GetType(Date)))
        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("Ignore_Pack_Updates", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("VendorCostHistoryID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Store_No", GetType(Integer)))

        logger.Debug("SetupDataTable Exit")
    End Sub

    Private Sub LoadDataTable(ByVal sSearchSQL As String)

        logger.Debug("LoadDataTable Entry with sSearchSQL = " + sSearchSQL)

        Dim rsSearch As DAO.Recordset = Nothing
        Dim row As DataRow
        Dim iLoop As Integer
        Dim MaxLoop As Short = 1000

        Try
            rsSearch = SQLOpenRecordSet(sSearchSQL, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            'Load the data set.
            mdt.Rows.Clear()

            While (Not rsSearch.EOF) And (iLoop < MaxLoop)
                iLoop = iLoop + 1

                row = mdt.NewRow
                row("Store_Name") = rsSearch.Fields("Store_Name").Value
                row("FromVendor") = rsSearch.Fields("FromVendor").Value
                row("Type") = rsSearch.Fields("Type").Value
                row("UnitCost") = VB6.Format(rsSearch.Fields("UnitCost").Value, "###0.0000")
                row("NetDiscount") = VB6.Format(rsSearch.Fields("NetDiscount").Value, "###0.0000")
                row("NetCost") = VB6.Format(rsSearch.Fields("NetCost").Value, "###0.0000")
                row("VendorPack") = rsSearch.Fields("VendorPack").Value
                row("UnitFreight") = VB6.Format(rsSearch.Fields("UnitFreight").Value, "###0.0000")
                row("StartDate") = rsSearch.Fields("StartDate").Value
                row("EndDate") = rsSearch.Fields("EndDate").Value
                row("PrimaryVendor") = rsSearch.Fields("PrimaryVendor").Value
                row("CostUnit_Name") = rsSearch.Fields("CostUnit_Name").Value
                row("FreightUnit_Name") = rsSearch.Fields("FreightUnit_Name").Value
                row("Package_Desc1") = rsSearch.Fields("Package_Desc1").Value
                row("Ignore_Pack_Updates") = rsSearch.Fields("IgnoreCasePack").Value
                _ignoreCasePackUpdates = rsSearch.Fields("IgnoreCasePack").Value
                row("Currency") = If(IsDBNull(rsSearch.Fields("CurrencyCode").Value), ConfigurationServices.AppSettings("CurrencyDefault"), rsSearch.Fields("CurrencyCode").Value)
                row("Insert_Date") = rsSearch.Fields("InsertDate").Value

                'hidden cols
                row("VendorCostHistoryID") = rsSearch.Fields("VendorCostHistoryID").Value
                row("Store_No") = rsSearch.Fields("Store_No").Value

                mdt.Rows.Add(row)

                rsSearch.MoveNext()
            End While

            'Setup a column that you would like to sort on initially.
            mdt.AcceptChanges()
            mdv = New System.Data.DataView(mdt)
            'mdv.Sort = "Store_Name"
            ugrdCostHistory.DataSource = mdv
            ugrdCostHistory.Focus()

            'This may or may not be required.
            If rsSearch.RecordCount > 0 Then
                'Set the first item to selected.
                ugrdCostHistory.Rows(0).Selected = True
            End If

        Finally
            If rsSearch IsNot Nothing Then
                rsSearch.Close()
                rsSearch = Nothing
            End If
        End Try
        logger.Debug("LoadDataTable Exit")

ExitSub:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        logger.Debug("LoadDataTable Exit(Exitsub:)")

    End Sub

    Private Sub RunSearch(ByRef bValidationMsg As Boolean)

        logger.Debug("RunSearch Entry")

        Dim sStore_No As String
        Dim sZone_ID As String
        Dim sState As String
        Dim sWFM As String
        Dim sHFM As String
        Dim sStartDate As String
        Dim sEndDate As String
        Dim sVendItemStatus As String

        sStore_No = String.Empty
        sZone_ID = String.Empty
        sState = String.Empty
        sWFM = String.Empty
        sHFM = String.Empty
        sStartDate = String.Empty
        sEndDate = String.Empty
        sVendItemStatus = String.Empty

        If cmbVendItemStats.SelectedIndex <> -1 Then
            sVendItemStatus = CStr(VB6.GetItemData(cmbVendItemStats, cmbVendItemStats.SelectedIndex))
        End If

        'Validations
        Select Case True
            Case optSelection(0).Checked
                If cmbStore.SelectedIndex = -1 Then
                    If bValidationMsg Then
                        MsgBox(String.Format(ResourcesIRMA.GetString("Required"), optSelection(0).Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
                        logger.Info(String.Format(ResourcesIRMA.GetString("Required"), optSelection(0).Text.Replace(":", "")))
                        logger.Debug("RunSearch Exit")
                    End If

                    Exit Sub
                Else
                    sStore_No = CStr(VB6.GetItemData(cmbStore, cmbStore.SelectedIndex))
                End If
            Case optSelection(1).Checked
                If cmbZones.SelectedIndex = -1 Then
                    If bValidationMsg Then
                        MsgBox(String.Format(ResourcesIRMA.GetString("Required"), optSelection(1).Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
                        logger.Info(String.Format(ResourcesIRMA.GetString("Required"), optSelection(1).Text.Replace(":", "")))
                        logger.Debug("RunSearch Exit")
                    End If
                    Exit Sub
                Else
                    sZone_ID = CStr(VB6.GetItemData(cmbZones, cmbZones.SelectedIndex))
                End If
            Case optSelection(2).Checked
                If cmbState.SelectedIndex = -1 Then
                    If bValidationMsg Then
                        MsgBox(String.Format(ResourcesIRMA.GetString("Required"), optSelection(2).Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
                        logger.Info(String.Format(ResourcesIRMA.GetString("Required"), optSelection(2).Text.Replace(":", "")))
                        logger.Debug("RunSearch Exit")
                    End If

                    Exit Sub
                Else
                    sState = "'" & cmbState.Text & "'"
                End If
            Case optSelection(3).Checked
                sHFM = "1"
            Case optSelection(4).Checked
                sWFM = "1"
        End Select

        If Len(sStore_No) = 0 Then sStore_No = "NULL"
        If Len(sZone_ID) = 0 Then sZone_ID = "NULL"
        If Len(sState) = 0 Then sState = "NULL"
        If Len(sWFM) = 0 Then sWFM = "NULL"
        If Len(sHFM) = 0 Then sHFM = "NULL"
        If Len(sVendItemStatus) = 0 Then sVendItemStatus = "NULL"

        If chkCurrentCost.CheckState = CheckState.Unchecked Then
            If dtpEndDate.Value <> Nothing AndAlso (dtpEndDate.Value < dtpStartDate.Value) Then
                If bValidationMsg Then
                    MsgBox(ResourcesIRMA.GetString("EndDateGreater"), MsgBoxStyle.Critical, Me.Text)
                    logger.Info(ResourcesIRMA.GetString("EndDateGreater"))
                    logger.Debug("RunSearch Exit")
                End If
                Exit Sub
            Else
                sStartDate = If(CDate(dtpStartDate.Value) > System.DateTime.FromOADate(0), "'" & CDate(dtpStartDate.Value).ToString("yyyy-MM-dd") & "'", "NULL")
                sEndDate = If(CDate(dtpEndDate.Value) > System.DateTime.FromOADate(0), "'" & CDate(dtpEndDate.Value).ToString("yyyy-MM-dd") & "'", "NULL")
            End If
        End If

        'Run the search
        If chkCurrentCost.CheckState = CheckState.Checked Then
            Call LoadDataTable("ItemVendorCostCurrent " & plItem_Key & "," & poVendorInfo.VendorID & "," & sZone_ID & "," & sStore_No & "," & sWFM & "," & sHFM & "," & sState)
        Else
            Call LoadDataTable("ItemVendorCostSearch " & plItem_Key & "," & poVendorInfo.VendorID & "," & sZone_ID & "," & sStore_No & "," & sWFM & "," & sHFM & "," & sState & "," & sStartDate & "," & sEndDate)
        End If

        If ugrdCostHistory.Rows.Count = 0 Then
            If bValidationMsg Then
                MsgBox(ResourcesItemHosting.GetString("NoMatchingVendorCost"), MsgBoxStyle.Exclamation, Me.Text)
                logger.Info(ResourcesItemHosting.GetString("NoMatchingVendorCost"))
            End If
        Else
            _hasRunSearch = True
        End If
        logger.Debug("RunSearch Exit")
    End Sub

    Private Function SaveChanges() As Boolean

        logger.Debug("SaveChanges Entry")
        Dim success As Boolean = True

        ' The vendor id is a required field.  However, if the user blanked it out and they select not to save their changes,
        ' they are not prompted to correct the blank value because it is not written to the database.

        'save the vendor item id if it's different than it's original value
        If sOrig_Item_ID <> Trim(txtItemID.Text) Then
            ' prompt the user to see if they want to save the changes
            If MsgBox(String.Format(ResourcesIRMA.GetString("SaveChanges1"), ResourcesItemHosting.GetString("VendorIdChangedForSave")), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                ' Make sure the new value is not blank
                If txtItemID.Text.Trim.Equals("") Then
                    'alert user to provide a value
                    MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblVendorItemID.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
                    logger.Info(String.Format(ResourcesIRMA.GetString("Required"), lblVendorItemID.Text.Replace(":", "")))
                    txtItemID.Focus()
                    success = False
                Else
                    SQLExecute("EXEC UpdateItemID " & plItem_Key & ", " & poVendorInfo.VendorID & ", '" & Trim(txtItemID.Text) & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    ' Refresh the original value so the user is not prompted to save again on form closing
                    sOrig_Item_ID = txtItemID.Text
                End If
            Else
                ' Refresh the value with the original since the user did not save
                txtItemID.Text = sOrig_Item_ID
            End If
        End If

        ' make sure the original value was not blank - this is a newly required field so old data
        ' might need to be updated
        If sOrig_Item_ID.Equals("") Then
            'alert user to provide a value
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblVendorItemID.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            logger.Info(String.Format(ResourcesIRMA.GetString("Required"), lblVendorItemID.Text.Replace(":", "")))
            txtItemID.Focus()
            success = False
        End If

        logger.Debug("SaveChanges Exit with " & success)
        Return success
    End Function

    Private Sub chkCurrentCost_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkCurrentCost.CheckStateChanged
        logger.Debug("chkCurrentCost_CheckStateChanged Entry")
        If Me.IsInitializing Then Exit Sub
        If chkCurrentCost.CheckState = System.Windows.Forms.CheckState.Checked Then
            Call SetActive(dtpStartDate, False)
            Call SetActive(dtpEndDate, False)
        Else
            Call SetActive(dtpStartDate, True)
            Call SetActive(dtpEndDate, True)
        End If
        logger.Debug("chkCurrentCost_CheckStateChanged Exit")
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        logger.Debug("cmdAdd_Click Entry")
        'pass item info to child screen
        frmVendorCostDetail.poItemInfo = poItemInfo
        frmVendorCostDetail.poVendorInfo = poVendorInfo
        frmVendorCostDetail.IgnoreCasePackUpdates = _ignoreCasePackUpdates
        frmVendorCostDetail.ShowDialog()
        frmVendorCostDetail.Close()
        frmVendorCostDetail.Dispose()

        'Refresh the grid
        RunSearch(bValidationMsg:=False)
        logger.Debug("cmdAdd_Click Exit")

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click

        logger.Debug("cmdDelete_Click Entry")
        Dim i As Integer
        Dim bFromVendor As Boolean
        Dim bPast As Boolean
        Dim sMsg As String
        sMsg = String.Empty

        If Me.ugrdCostHistory.Selected.Rows.Count = 0 Then
            MsgBox(ResourcesItemHosting.GetString("MustSelectCost"))
            logger.Info(ResourcesItemHosting.GetString("MustSelectCost"))
            logger.Debug("cmdDelete_Click Exit")
            Exit Sub
        End If

        If MsgBox(ResourcesItemHosting.GetString("DeleteSelectedCosts"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then Exit Sub

        For i = ugrdCostHistory.Selected.Rows.Count - 1 To 0 Step -1

            If ugrdCostHistory.Selected.Rows(i).Cells("FromVendor").Value = False Then
                If DateDiff(Microsoft.VisualBasic.DateInterval.Day, SystemDateTime, CDate(ugrdCostHistory.Selected.Rows(i).Cells("EndDate").Value)) >= 0 Then

                    SQLExecute("EXEC DeleteVendorCostHistory " & ugrdCostHistory.Selected.Rows(i).Cells("VendorCostHistoryID").Value, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    ugrdCostHistory.Selected.Rows(i).Delete(False)
                Else
                    bPast = True
                End If
            Else
                bFromVendor = True
            End If
        Next

        If bFromVendor Or bPast Then
            If bFromVendor Then sMsg = sMsg & ResourcesItemHosting.GetString("CostFromVendor")
            If bPast Then sMsg = sMsg & IIf(Len(sMsg) > 0, vbCrLf & ResourcesItemHosting.GetString("PastCost"), ResourcesItemHosting.GetString("Past cost"))
            MsgBox(ResourcesItemHosting.GetString("CouldNotDeleteMessage") & vbCrLf & sMsg, MsgBoxStyle.Exclamation, Me.Text)

            logger.Info(ResourcesItemHosting.GetString("CouldNotDeleteMessage") & vbCrLf & sMsg)

        End If

        logger.Debug("cmdDelete_Click Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        If SaveChanges() Then
            Me.Hide()
        End If
        logger.Debug("cmdExit_Click Exit")
    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        logger.Debug("cmdSearch_Click Entry")
        RunSearch(bValidationMsg:=True)
        logger.Debug("cmdSearch_Click Exit")
    End Sub

    Private Sub frmVendorCost_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmVendorCost_Load Entry")

        Dim rs As DAO.Recordset = Nothing
        Dim bItemAdminSubTeam As Boolean
        Dim isCostedByWeight As Boolean
        Dim isCatchweight As Boolean

        CenterForm(Me)

        btnConversionCalculator.Visible = OrderSearchDAO.IsMultipleJurisdiction()

        Call SetupDataTable()

        Try
            '-- Load the data onto the screen
            rs = SQLOpenRecordSet("EXEC GetItemIDInfo " & plItem_Key & ", " & poVendorInfo.VendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough + DAO.RecordsetOptionEnum.dbForwardOnly)
            Me.Text = Me.Text & " [" & rs.Fields("CompanyName").Value & "]"
            txtItem_Description.Text = rs.Fields("Item_Description").Value & ""
            txtIdentifier.Text = rs.Fields("Identifier").Value & ""
            txtItemID.Text = Trim(rs.Fields("Item_ID").Value & "")
            sOrig_Item_ID = txtItemID.Text
            chkEDLP.CheckState = System.Math.Abs(CInt(rs.Fields("EDLP").Value))
            isCostedByWeight = rs.Fields("CostedByWeight").Value
            isCatchweight = rs.Fields("CatchweightRequired").Value
            iVendorItemStatus = IIf(IsDBNull(rs.Fields("VendorItemStatus").Value), -1, rs.Fields("VendorItemStatus").Value)
        Finally
            If rs IsNot Nothing Then
                rs.Close()
                rs = Nothing
            End If
        End Try

        'setup item business object to be passed to child forms
        poItemInfo = New ItemBO
        poItemInfo.Item_Key = Me.plItem_Key
        poItemInfo.ItemDescription = txtItem_Description.Text
        poItemInfo.CostedByWeight = isCostedByWeight
        poItemInfo.CatchweightRequired = isCatchweight
        'populate package desc info
        ItemUnitDAO.GetItemUnitInfo(poItemInfo)

        LoadStores(cmbStore)
        LoadZone(cmbZones)
        LoadStates(cmbState)
        LoadVendorItemStatuses(cmbVendItemStats)

        If iVendorItemStatus > -1 Then SetCombo(cmbVendItemStats, iVendorItemStatus)

        'Default to current for the search
        chkCurrentCost.CheckState = System.Windows.Forms.CheckState.Checked

        bItemAdminSubTeam = ItemAdminSubTeam(plItem_Key)

        SetActive(cmbZones, False)
        SetActive(cmbState, False)
        SetActive(txtItemID, gbItemAdministrator And bItemAdminSubTeam)
        SetActive(cmdAdd, gbItemAdministrator And bItemAdminSubTeam)
        SetActive(cmdDelete, cmdAdd.Enabled)

        dtpStartDate.Value = Nothing
        dtpEndDate.Value = Nothing

        Button_AddCostPromo.Enabled = (gbSuperUser Or (gbItemAdministrator And frmItem.pbUserSubTeam))

        If bSpecificStoreItemVendorCost = True Then
            SetCombo(cmbStore, glStoreID)
            RunSearch(bValidationMsg:=True)
        End If

        If Not CheckAllStoreSelectionEnabled() Then
            _optSelection_5.Visible = False
        End If

        logger.Debug("frmVendorCost_Load Exit")
    End Sub

    Private Sub frmVendorCost_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        logger.Debug("frmVendorCost_FormClosing Entry")

        Dim Cancel As Boolean = eventArgs.Cancel
        Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason

        If UnloadMode = System.Windows.Forms.CloseReason.UserClosing Then
            If SaveChanges() Then
                Cancel = False
            Else
                Cancel = True
            End If
        End If
        eventArgs.Cancel = Cancel

        logger.Debug("frmVendorCost_FormClosing Exit")
    End Sub

    Private Sub OptSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSelection.CheckedChanged

        logger.Debug("OptSelection_CheckedChanged Entry")

        If Me.IsInitializing Then Exit Sub
        If eventSender.Checked Then
            Dim Index As Short = optSelection.GetIndex(eventSender)
            Select Case Index
                Case 0
                    SetActive(cmbStore, True)
                    SetActive(cmbZones, False)
                    SetActive(cmbState, False)
                Case 1
                    SetActive(cmbStore, False)
                    SetActive(cmbZones, True)
                    SetActive(cmbState, False)
                Case 2
                    SetActive(cmbStore, False)
                    SetActive(cmbZones, False)
                    SetActive(cmbState, True)
                Case Else
                    SetActive(cmbStore, False)
                    SetActive(cmbZones, False)
                    SetActive(cmbState, False)
            End Select
        End If
        logger.Debug("OptSelection_CheckedChanged Exit")
    End Sub

    Private Sub txtItemID_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtItemID.Enter

        logger.Debug("txtItemID_Enter Entry")
        HighlightText(txtItemID)
        logger.Debug("txtItemID_Enter Exit")

    End Sub

    Private Sub txtItemID_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtItemID.KeyPress

        logger.Debug("txtItemID_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, "String", txtItemID, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        logger.Debug("txtItemID_KeyPress Exit")
    End Sub

    Private Sub cmbStore_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbStore.KeyPress

        logger.Debug("cmbStore_KeyPress Entry")
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbStore.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        logger.Debug("cmbStore_KeyPress Exit")
    End Sub

    Private Sub cmbZones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbZones.KeyPress
        logger.Debug("cmbZones_KeyPress Entry")

        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbZones.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        logger.Debug("cmbZones_KeyPress Exit")
    End Sub

    Private Sub cmbState_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbState.KeyPress

        logger.Debug("cmbState_KeyPress Entry")
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbState.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        logger.Debug("cmbState_KeyPress Exit")
    End Sub

    Private Sub cmdNetDiscountDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNetDiscountDetails.Click

        logger.Debug("cmdNetDiscountDetails_Click Entry")
        Dim form As New CostPromotion
        Dim storeNo As Integer
        Dim storeName As String

        'get selected store record - user must select a store to view cost promos for
        If Me.ugrdCostHistory.Selected.Rows.Count = 1 Then
            storeNo = ugrdCostHistory.Selected.Rows(0).Cells("Store_No").Value
            storeName = ugrdCostHistory.Selected.Rows(0).Cells("Store_Name").Value
        ElseIf ugrdCostHistory.Rows.Count = 1 Then
            storeNo = ugrdCostHistory.Rows(0).Cells("Store_No").Value
            storeName = ugrdCostHistory.Rows(0).Cells("Store_Name").Value
        Else
            MsgBox(ResourcesIRMA.GetString("SelectSingleRow"), MsgBoxStyle.Critical, Me.Text)
            logger.Info(ResourcesIRMA.GetString("SelectSingleRow"))
            logger.Debug("cmdNetDiscountDetails_Click Exit")
            Exit Sub
        End If

        'pass item info to child screen
        Dim storeInfo As New StoreBO
        storeInfo.StoreNo = storeNo
        storeInfo.StoreName = storeName

        'TFS 9251, 12/14/2012, Faisal Ahmed - Load the item info again based on selected store jurisdiction
        ItemUnitDAO.GetItemUnitInfo(poItemInfo, storeNo)

        form.ItemBO = poItemInfo
        form.VendorBO = poVendorInfo
        form.StoreBO = storeInfo

        form.ShowDialog()
        form.Dispose()
        logger.Debug("cmdNetDiscountDetails_Click Exit")
    End Sub

    Private Sub Button_AddCostPromo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddCostPromo.Click

        logger.Debug("Button_AddCostPromo_Click Entry")

        Dim costPromoForm As New CostPromotionDetail

        costPromoForm.IsAdd = True
        costPromoForm.ItemBO = poItemInfo
        costPromoForm.VendorBO = poVendorInfo
        costPromoForm.ShowDialog()
        costPromoForm.Dispose()

        'run cost search again
        If _hasRunSearch Then
            RunSearch(bValidationMsg:=True)
        End If

        logger.Debug("Button_AddCostPromo_Click Exit")
    End Sub

    Private Sub MarginInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_MarginInfo.Click

        logger.Debug("MarginInfo_Click Entry")

        Dim marginInfoForm As New MarginInfo

        marginInfoForm.ItemBO = poItemInfo
        marginInfoForm.ShowDialog()
        marginInfoForm.Dispose()

        logger.Debug("MarginInfo_Click Exit")
    End Sub

    Private Sub cmbVendItemStats_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbVendItemStats.SelectedIndexChanged
        If Not cmbVendItemStats.SelectedItem Is Nothing Then
            'Dim VDAO As VendorDAO = New VendorDAO()
            'VDAO.UpdateVendorItemStatus(plItem_Key, poVendorInfo.VendorID, VB6.GetItemData(cmbVendItemStats, cmbVendItemStats.SelectedIndex))
            VendorDAO.UpdateVendorItemStatus(plItem_Key, poVendorInfo.VendorID, VB6.GetItemData(cmbVendItemStats, cmbVendItemStats.SelectedIndex))
        End If
    End Sub

    Private Sub btnConversionCalculator_Click(sender As System.Object, e As System.EventArgs) Handles btnConversionCalculator.Click
        logger.Debug("btnConversionCalculator_Click Entry")
        Dim ccForm As New FrmConvertMeasures

        ccForm.ShowDialog(Me)
        ccForm.Dispose()

        logger.Debug("btnConversionCalculator_Click Exit")
    End Sub
End Class