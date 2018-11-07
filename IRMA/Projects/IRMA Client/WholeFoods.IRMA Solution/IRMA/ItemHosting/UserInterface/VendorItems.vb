Option Strict Off
Option Explicit On

Imports VB = Microsoft.VisualBasic
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports log4net

Friend Class frmVendorItems
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean
    Private plVendor_ID As Integer
    Private psVendorName As String
    Private psSQLSearch As String

    Private mdtItemList As DataTable
    Private mdvItemList As DataView

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Public Sub New(ByVal VendorID As Integer, ByVal VendorName As String)
        Me.New()
        logger.Debug("New Entry with VendorID=" + VendorID.ToString + ", VendorName = " + VendorName)

        plVendor_ID = VendorID
        psVendorName = VendorName
        psSQLSearch = ""

        logger.Debug("New Exit")

    End Sub
    Public Sub New(ByVal VendorID As Integer, ByVal VendorName As String, ByVal SQLSearch As String) 

        Me.New(VendorID, VendorName)
        logger.Debug("New Entry with VendorID=" + VendorID.ToString + ", VendorName=" + VendorName + ", SQLSearch=" + SQLSearch)


        psSQLSearch = SQLSearch

        Me.cmdExit.Visible = True
        Me.cmdEditItem.Visible = True
        Me.cmdSetPrimaryVend.Visible = True
        Me.cmdReports.Visible = False
        Me.cmdItemEdit.Visible = False
        Me.cmdCost.Visible = False
        Me.cmdDelete.Visible = False
        Me.cmdAdd.Visible = False

        logger.Debug("New Exit")

    End Sub
    Public Sub AdjustControls()
        logger.Debug("AdjustControls Entry")

        Call RightJustControls(Me, Me.cmdExit, Me.cmdReports, Me.cmdItemEdit, Me.cmdCost, Me.cmdEditItem, Me.cmdDelete, Me.cmdAdd, Me.cmdSetPrimaryVend)

        logger.Debug("AdjustControls Exit")
    End Sub

    Private Sub cmbBrand_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbBrand.KeyPress
        logger.Debug("cmbBrand_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        If KeyAscii = 8 Then cmbBrand.SelectedIndex = -1
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If

        logger.Debug("cmbBrand_KeyPress Exit")
    End Sub

    Private Sub cmbStore_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStore.SelectedIndexChanged
        logger.Debug("cmbStore_SelectedIndexChanged Entry")

        If Me.IsInitializing Then Exit Sub
        If cmbStore.SelectedIndex = -1 Then
            Call SetActive(cmdAdd, True)
            Call SetActive(cmdDelete, True)
        Else
            Call SetActive(cmdAdd, False)
            Call SetActive(cmdDelete, False)
        End If

        logger.Debug("cmbStore_SelectedIndexChanged Exit")
    End Sub

    Private Sub cmbStore_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbStore.KeyPress
        logger.Debug("cmbStore_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        If KeyAscii = 8 Then cmbStore.SelectedIndex = -1
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If

        logger.Debug("cmbStore_KeyPress Exit")
    End Sub


    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        logger.Debug("cmdAdd_Click Entry")
        Dim sInsertedItems As String
        Dim rsItemVendor As DAO.Recordset = Nothing
        Dim bDeleted_Item As Boolean
        Dim bDuplicate As Boolean
        Dim iCnt As Short
        sInsertedItems = String.Empty

        '-- Open the search form
        frmItemSearch.MultiSelect = True
        frmItemSearch.ShowDialog()

        '-- if its not zero, then something was found
        If frmItemSearch.SelectedItems.Count > 0 Then
            For iCnt = 0 To frmItemSearch.SelectedItems.Count - 1
                '-- Make sure it isn't already deleted
                Try
                    rsItemVendor = SQLOpenRecordSet("EXEC GetItemData " & frmItemSearch.SelectedItems.Item(iCnt).Item_Key, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    bDeleted_Item = rsItemVendor.Fields("Deleted_Item").Value
                Finally
                    If rsItemVendor IsNot Nothing Then
                        rsItemVendor.Close()
                    End If
                End Try
                If bDeleted_Item Then
                    MsgBox(String.Format(ResourcesIRMA.GetString("ItemDeleted"), frmItemSearch.SelectedItems.Item(iCnt).ItemDescription), MsgBoxStyle.Exclamation, Me.Text)
                    logger.Info(String.Format(ResourcesIRMA.GetString("ItemDeleted"), frmItemSearch.SelectedItems.Item(iCnt).ItemDescription))
                Else
                    Try
                        '-- Check to see if the name already exists
                        SQLOpenRS(rsItemVendor, "EXEC CheckForDuplicateItemVendors " & plVendor_ID & ", " & frmItemSearch.SelectedItems.Item(iCnt).Item_Key, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        bDuplicate = (rsItemVendor.Fields("ItemVendorCount").Value > 0)
                    Finally
                        If rsItemVendor IsNot Nothing Then
                            rsItemVendor.Close()
                        End If
                    End Try
                    If bDuplicate Then
                        MsgBox("Item " & frmItemSearch.SelectedItems.Item(iCnt).ItemDescription & " is already in the list.", MsgBoxStyle.Exclamation, Me.Text)
                        logger.Info("Item " & frmItemSearch.SelectedItems.Item(iCnt).ItemDescription & " is already in the list.")
                    Else
                        'Determine if user has access to this item's subteam
                        If ItemAdminSubTeam((frmItemSearch.SelectedItems.Item(iCnt).Item_Key)) Then

                            '-- Add the new record
                            SQLExecute("EXEC InsertItemVendor " & plVendor_ID & ", " & frmItemSearch.SelectedItems.Item(iCnt).Item_Key, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                            sInsertedItems = sInsertedItems & frmItemSearch.SelectedItems.Item(iCnt).Item_Key & "|"

                        Else
                            MsgBox(String.Format(ResourcesIRMA.GetString("NotYourSubTeam"), frmItemSearch.SelectedItems.Item(iCnt).ItemDescription), MsgBoxStyle.Critical, Me.Text)
                            logger.Info(String.Format(ResourcesIRMA.GetString("NotYourSubTeam"), frmItemSearch.SelectedItems.Item(iCnt).ItemDescription))
                        End If
                    End If
                End If
            Next iCnt

            'remove last Delimiter
            If Len(sInsertedItems) > 1 Then
                sInsertedItems = "'" & VB.Left(sInsertedItems, Len(sInsertedItems) - 1) & "'"
                'add items to local DB
                Call RefreshDataSource(sInsertedItems)
                'refresh grid
                Call RefreshGrid()
            End If

        End If
        logger.Debug("cmdAdd_Click Exit")

me_exit:
        rsItemVendor = Nothing
        frmItemSearch.Close()
        frmItemSearch.Dispose()
        logger.Debug("cmdAdd_Click me_exit:(From Handler)")
    End Sub

    Private Sub cmdCost_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCost.Click

        logger.Debug("cmdCost_Click Entry")

        If ugrdItemList.Selected.Rows.Count = 1 Then
            Dim vendorInfo As New VendorBO
            vendorInfo.VendorID = plVendor_ID
            vendorInfo.VendorName = psVendorName

            frmVendorCost.poVendorInfo = vendorInfo
            frmVendorCost.plItem_Key = ugrdItemList.Selected.Rows(0).Cells("Item_Key").Value

            frmVendorCost.ShowDialog()

            ugrdItemList.Selected.Rows(0).Cells("Item_ID").Value = frmVendorCost.txtItemID.Text
            ugrdItemList.UpdateData()

            frmVendorCost.Close()
            frmVendorCost.Dispose()
        Else
            MsgBox(ResourcesItemHosting.GetString("SelectItem"), MsgBoxStyle.Exclamation, Me.Text)
            logger.Info(ResourcesItemHosting.GetString("SelectItem"))
        End If

        logger.Debug("cmdCost_Click Exit")
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click

        logger.Debug("cmdDelete_Click Entry")

        Dim iLoop As Short
        Dim sDeleteDate As String

        Dim fSelectDate As frmSelectDate
        Dim fPrimVendSelect As frmNewPrimVend
        Dim rsChkPrimVend As DAO.Recordset = Nothing
        If ugrdItemList.Selected.Rows.Count > 0 Then

            If MsgBox(ResourcesItemHosting.GetString("msg_wanring_RemoveVendorItem"), MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                fSelectDate = New frmSelectDate(ResourcesItemHosting.GetString("EnterDateItemRemove"), SystemDateTime(True))
                fSelectDate.ShowDialog()
                sDeleteDate = VB6.Format(fSelectDate.ReturnDate, "YYYY-MM-DD")
                fSelectDate.Close()

                For iLoop = ugrdItemList.Selected.Rows.Count - 1 To 0 Step -1
                    glItemID = ugrdItemList.Selected.Rows(iLoop).Cells("Item_Key").Value
                    If ItemAdminSubTeam(glItemID) Then
                        If sDeleteDate <> "" Then
                            '-- Delete the Brand from the database
                            Try
                                rsChkPrimVend = SQLOpenRecordSet("EXEC CheckIfPrimVendCanSwap " & plVendor_ID & ", " & glItemID & ", null", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbForwardOnly + DAO.RecordsetOptionEnum.dbSQLPassThrough)
                                If rsChkPrimVend.Fields("IsPrimVend").Value = 1 Then
                                    fPrimVendSelect = New frmNewPrimVend(psVendorName, plVendor_ID, glItemID)
                                    fPrimVendSelect.ShowDialog()
                                    If fPrimVendSelect.UnassignedItems = 0 Then
                                        SQLExecute("EXEC DeleteItemVendor " & plVendor_ID & ", " & glItemID & ", '" & sDeleteDate & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
                                        ugrdItemList.Selected.Rows(iLoop).Delete()
                                    Else
                                        Call MsgBox(ResourcesItemHosting.GetString("SelectPrimaryVendor"), MsgBoxStyle.Critical, Me.Text)
                                        logger.Info(ResourcesItemHosting.GetString("SelectPrimaryVendor"))
                                    End If
                                    fPrimVendSelect.Close()
                                    fPrimVendSelect.Dispose()

                                Else
                                    rsChkPrimVend.Close()
                                    rsChkPrimVend = Nothing
                                    SQLExecute("EXEC DeleteItemVendor " & plVendor_ID & ", " & glItemID & ", '" & sDeleteDate & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
                                    ugrdItemList.Selected.Rows(iLoop).Delete()
                                End If
                            Finally
                                If rsChkPrimVend IsNot Nothing Then
                                    rsChkPrimVend.Close()
                                    rsChkPrimVend = Nothing
                                End If
                            End Try
                        End If

                    Else
                        MsgBox(String.Format(ResourcesItemHosting.GetString("NotYourSubTeam"), ugrdItemList.Selected.Rows(iLoop).Cells("Identifier").Value.ToString), MsgBoxStyle.Critical, Me.Text)
                        logger.Info(String.Format(ResourcesItemHosting.GetString("NotYourSubTeam"), ugrdItemList.Selected.Rows(iLoop).Cells("Identifier").Value.ToString))
                    End If
                Next iLoop
                mdtItemList.AcceptChanges()

                Call UpdateStatusLabel((ugrdItemList.Rows.Count))

            End If

        Else
            '-- No vendor was selected
            MsgBox(ResourcesItemHosting.GetString("HighlightItemDelete"), MsgBoxStyle.Exclamation, Me.Text)
            logger.Info(ResourcesItemHosting.GetString("HighlightItemDelete"))

        End If

        logger.Debug("cmdDelete_Click Exit")

    End Sub

    Private Sub cmdEditItem_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEditItem.Click

        logger.Debug("cmdEditItem_Click Entry")
        If ugrdItemList.Selected.Rows.Count = 1 Then

            glItemID = ugrdItemList.Selected.Rows(0).Cells("Item_Key").Value

            If ItemAdminSubTeam(glItemID) Then
                '-- Let them change the item id for the vendor
                frmItemVendorID.ShowDialog()

                ugrdItemList.Selected.Rows(0).Cells("Item_ID").Value = frmItemVendorID.txtItemID.Text

                ugrdItemList.UpdateData()
            Else
                MsgBox(ResourcesItemHosting.GetString("NotYourSubTeam"), MsgBoxStyle.Critical, Me.Text)
                logger.Info(ResourcesItemHosting.GetString("NotYourSubTeam"))
            End If
        Else
            '-- No vendor was selected
            MsgBox(ResourcesIRMA.GetString("SelectSingleRow"), MsgBoxStyle.Exclamation, Me.Text)
            logger.Info(ResourcesIRMA.GetString("SelectSingleRow"))
        End If
        logger.Debug("cmdEditItem_Click Exit")

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        logger.Debug("cmdExit_Click Entry")
        '-- Close the form
        Me.Close()
        logger.Debug("cmdExit_Click Exit")

    End Sub


    Private Sub cmdFilter_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFilter.Click

        logger.Debug("cmdFilter_Click Entry")

        Me.Enabled = False

        Dim lStore_No As Integer
        If cmbStore.SelectedIndex = -1 Then
            lStore_No = -1
        Else
            lStore_No = VB6.GetItemData(cmbStore, cmbStore.SelectedIndex)
        End If

        If mdtItemList.Select("Store_No = " & lStore_No).Length = 0 Then Call RefreshDataSource("", lStore_No)

        Call RefreshGrid()
        Me.Enabled = True

        logger.Debug("cmdFilter_Click Exit")

    End Sub

    Private Sub cmdItemEdit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdItemEdit.Click

        logger.Debug("cmdItemEdit_Click Entry")

        If ugrdItemList.Selected.Rows.Count = 1 Then

            Me.Enabled = False

            glItemID = ugrdItemList.Selected.Rows(0).Cells("Item_Key").Value

            frmItem.ShowDialog()
            frmItem.Close()

            RefreshGrid()

            Me.Enabled = True
        Else
            MsgBox(ResourcesIRMA.GetString("MustSelect"), MsgBoxStyle.Critical, Me.Text)
            logger.Info(ResourcesIRMA.GetString("MustSelect"))
        End If
        logger.Debug("cmdItemEdit_Click Exit")

    End Sub

    Private Sub cmdReports_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReports.Click

        logger.Debug("cmdReports_Click Entry")
        '-- Set glvendorid to none found
        glVendorID = plVendor_ID

        '-- Open the search form
        frmVendorReports.ShowDialog()
        frmVendorReports.Close()
        frmVendorReports.Dispose()
        logger.Debug("cmdReports_Click Exit")

    End Sub

    Private Sub cmdSetPrimaryVend_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSetPrimaryVend.Click

        logger.Debug("cmdSetPrimaryVend_Click Entry")

        Dim sStores As String
        Dim iItemKey As Integer
        Dim iTotSelRows As Integer

        sStores = String.Empty

        'verify that user has selected a row from the grid
        iTotSelRows = ugrdItemList.Selected.Rows.Count
        If iTotSelRows = 0 Then
            MsgBox(ResourcesCommon.GetString("msg_selectEditRow"))
        Else
            'set item key
            iItemKey = ugrdItemList.Selected.Rows(0).Cells("Item_Key").Value

            Call frmSelectList.LoadForm(glVendorID, sStores, iItemKey)
            frmSelectList.Close()
            frmSelectList.Dispose()
            SQLExecute("EXEC SetPrimaryVendor '" & sStores & "'," & iItemKey & ", " & glVendorID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End If

        logger.Debug("cmdSetPrimaryVend_Click Exit")
    End Sub

    Private Sub frmVendorItems_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmVendorItems_Load Entry")

        CenterForm(Me)

        If (DetermineVendorInternal() = True) Then
            ' Distributors/Manufacturers
            Call LoadSubTeamByType(enumSubTeamType.Supplier, cmbSubTeam, glStoreNo)
        Else
            ' External Vendors
            Call LoadSubTeamByType(enumSubTeamType.Retail, cmbSubTeam)
        End If

        'load all search combo boxes
        Call LoadStores((Me.cmbStore))
        Call LoadBrand((Me.cmbBrand))

        'set default store.
        Call SetCombo(cmbStore, glStore_Limit)

        If Not (mdtItemList Is Nothing) Then mdtItemList.Clear()


        cmdExit.Visible = True
        cmdReports.Visible = True
        cmdItemEdit.Visible = True
        cmdCost.Visible = True
        cmdSetPrimaryVend.Visible = True

        'set Visible controls based on gbItemAdministrator
        If gbItemAdministrator = True Then
            cmdAdd.Visible = True
            Call SetActive(cmdAdd, True)
            cmdDelete.Visible = True
            Call SetActive(cmdDelete, True)
            cmdEditItem.Visible = True
            Call SetActive(cmdEditItem, cmdAdd.Enabled)
        Else
            cmdAdd.Visible = False
            cmdDelete.Visible = False
            cmdEditItem.Visible = False
        End If

        RefreshDataSource()
        UpdateStatusLabel((ugrdItemList.Rows.Count))

        logger.Debug("frmVendorItems_Load Exit")
    End Sub
    Private Sub RefreshDataSource(Optional ByRef sItemList As String = "", Optional ByRef iStore_No As Integer = -1)

        logger.Debug("RefreshDataSource Entry")

        Dim rsVendorItemList As DAO.Recordset = Nothing
        Dim row As DataRow

        Try
            If psSQLSearch = "" Then
                If sItemList <> "" Then
                    rsVendorItemList = SQLOpenRecordSet("EXEC GetItemsInfo " & sItemList & ", " & plVendor_ID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                Else
                    mdtItemList.Clear()
                    rsVendorItemList = SQLOpenRecordSet("EXEC GetVendorItems " & plVendor_ID & ", " & IIf(iStore_No = -1, " NULL", CStr(iStore_No)) & ", 1", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                End If
            Else
                mdtItemList.Clear()
                rsVendorItemList = SQLOpenRecordSet(psSQLSearch, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            End If

            While Not rsVendorItemList.EOF

                row = mdtItemList.NewRow
                row("Item_Key") = rsVendorItemList.Fields("Item_Key").Value
                row("Item_Description") = rsVendorItemList.Fields("Item_Description").Value
                row("Identifier") = rsVendorItemList.Fields("Identifier").Value
                row("Item_ID") = rsVendorItemList.Fields("Item_ID").Value
                row("SubTeam_No") = rsVendorItemList.Fields("SubTeam_No").Value
                row("Category_Name") = rsVendorItemList.Fields("Category_Name").Value
                row("Category_ID") = rsVendorItemList.Fields("Category_ID").Value
                row("Brand_ID") = rsVendorItemList.Fields("Brand_ID").Value
                row("Store_No") = iStore_No

                mdtItemList.Rows.Add(row)

                rsVendorItemList.MoveNext()
            End While

        Finally
            If rsVendorItemList IsNot Nothing Then
                rsVendorItemList.Close()
                rsVendorItemList = Nothing
            End If
        End Try

        mdtItemList.AcceptChanges()
        mdvItemList = New System.Data.DataView(mdtItemList)

        Me.ugrdItemList.DataSource = mdvItemList
        logger.Debug("RefreshDataSource Exit")

    End Sub
    Public Sub RefreshGrid(Optional ByVal sSort As String = "")

        logger.Debug("RefreshGrid Entry")

        Dim sSQL As New System.Text.StringBuilder(String.Empty)

        If Trim(Me.txtItemDescription.Text) <> "" Then sSQL.Append("Item_Description like '%" & ConvertQuotes(Trim(Me.txtItemDescription.Text)) & "%'")
        If Trim(Me.txtIdentifier.Text) <> "" Then sSQL.Append(IIf(sSQL.Length = 0, "", " AND ") & "Identifier like '%" & ConvertQuotes(Trim(Me.txtIdentifier.Text)) & "%'")
        If Trim(Me.txtVendorItemID.Text) <> "" Then sSQL.Append(IIf(sSQL.Length = 0, "", " AND ") & "Item_id like '%" & ConvertQuotes(Trim(Me.txtVendorItemID.Text)) & "%'")
        If cmbSubTeam.SelectedIndex <> -1 Then sSQL.Append(IIf(sSQL.Length = 0, "", " AND ") & "SubTeam_No = " & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        If cmbCategory.SelectedIndex <> -1 Then sSQL.Append(IIf(sSQL.Length = 0, "", " AND ") & "Category_ID = " & VB6.GetItemData(cmbCategory, cmbCategory.SelectedIndex))
        If Me.cmbBrand.SelectedIndex <> -1 Then sSQL.Append(IIf(sSQL.Length = 0, "", " AND ") & "Brand_ID = " & VB6.GetItemData(Me.cmbBrand, Me.cmbBrand.SelectedIndex))
        If Me.cmbStore.SelectedIndex <> -1 Then
            sSQL.Append(IIf(sSQL.Length = 0, "", " AND ") & "Store_No = " & VB6.GetItemData(Me.cmbStore, Me.cmbStore.SelectedIndex))
        Else
            sSQL.Append(IIf(sSQL.Length = 0, "", " AND ") & "Store_No = -1")
        End If

        If sSort.Length = 0 Then sSort = "Item_Description"

        '*****************************************************
        mdvItemList.RowFilter = sSQL.ToString
        mdvItemList.Sort = sSort
        Me.ugrdItemList.DisplayLayout.Bands(0).SortedColumns.Clear()
        Me.ugrdItemList.DataSource = mdvItemList
        '*****************************************************

        ugrdItemList.DisplayLayout.Bands(0).Columns("Category_Name").PerformAutoResize()
        ugrdItemList.DisplayLayout.Bands(0).Columns("Item_Description").PerformAutoResize()

        If ugrdItemList.Rows.Count > 0 Then
            If ugrdItemList.Selected.Rows.Count = 0 Then ugrdItemList.Rows(0).Selected = True
        End If

        Call UpdateStatusLabel((ugrdItemList.Rows.Count))

        logger.Debug("RefreshGrid Exit")
    End Sub
    Private Sub UpdateStatusLabel(ByRef lRowCount As Integer)
        logger.Debug("UpdateStatusLabel Entry")

        If (lRowCount = 1) Then
            lblStatus.Text = VB6.Format(lRowCount, "###,###,###,##0") & " Vendor Item"
        Else
            lblStatus.Text = VB6.Format(lRowCount, "###,###,###,##0") & " Vendor Items"
        End If

        logger.Debug("UpdateStatusLabel Exit")
    End Sub

    Private Sub cmbSubTeam_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.SelectedIndexChanged

        logger.Debug("cmbSubTeam_SelectedIndexChanged Entry")

        If IsInitializing Then Exit Sub

        If cmbSubTeam.SelectedIndex = -1 Then
            cmbCategory.Items.Clear()
        Else
            Call LoadCategory(cmbCategory, VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
            SetActive(cmbCategory, True)
        End If

        logger.Debug("cmbSubTeam_SelectedIndexChanged Exit")

    End Sub

    Private Sub cmbSubTeam_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.TextChanged

        logger.Debug("cmbSubTeam_TextChanged Entry")

        If cmbSubTeam.Text = "" Then cmbSubTeam.SelectedIndex = -1

        logger.Debug("cmbSubTeam_TextChanged Exit")

    End Sub

    Private Sub cmbSubTeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress

        logger.Debug("cmbSubTeam_KeyPress Entry")

        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbSubTeam.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

        logger.Debug("cmbSubTeam_KeyPress Exit")

    End Sub


    Private Sub cmbCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCategory.KeyPress

        logger.Debug("cmbCategory_KeyPress Entry")

        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbCategory.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

        logger.Debug("cmbCategory_KeyPress Exit")

    End Sub

    '*************************************
    'End SubTeam / Catagory Code
    '*************************************
    Private Sub SetupDataTable()

        logger.Debug("SetupDataTable Entry")

        mdtItemList = New DataTable("ItemList")

        mdtItemList.Columns.Add(New DataColumn("Item_Key", GetType(Integer)))
        mdtItemList.Columns.Add(New DataColumn("Identifier", GetType(String)))
        mdtItemList.Columns.Add(New DataColumn("Category_Name", GetType(String)))
        mdtItemList.Columns.Add(New DataColumn("Item_Description", GetType(String)))
        mdtItemList.Columns.Add(New DataColumn("Item_ID", GetType(String)))
        mdtItemList.Columns.Add(New DataColumn("SubTeam_No", GetType(Integer)))
        mdtItemList.Columns.Add(New DataColumn("Category_ID", GetType(Integer)))
        mdtItemList.Columns.Add(New DataColumn("Brand_ID", GetType(Integer)))
        mdtItemList.Columns.Add(New DataColumn("Store_No", GetType(Integer)))

        logger.Debug("SetupDataTable Exit")
    End Sub
    Private Sub ugrdItemList_BeforeRowsDeleted(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs) Handles ugrdItemList.BeforeRowsDeleted
        e.DisplayPromptMsg = False
    End Sub

    Private Sub ugrdItemList_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdItemList.DoubleClickRow
        logger.Debug("ugrdItemList_DoubleClickRow Entry")
        cmdCost_Click(cmdCost, New System.EventArgs())
        logger.Debug("ugrdItemList_DoubleClickRow Exit")

    End Sub

    
    
End Class
