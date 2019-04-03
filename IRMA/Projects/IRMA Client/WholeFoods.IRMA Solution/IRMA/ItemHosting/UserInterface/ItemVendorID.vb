Option Strict Off
Option Explicit On

Imports System.Text
Imports VB = Microsoft.VisualBasic
Imports log4net
Imports WholeFoods.IRMA.Mammoth.DataAccess
Imports WholeFoods.IRMA.Mammoth.BusinessLogic
Imports System.Linq

Friend Class frmItemVendorID
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean

    Private mdt As DataTable
    Private mdv As DataView

    Private m_sOrigVItem_ID As String
    Private m_lOrigSubTeamNo As Integer
    Private mbNoClick As Boolean
    Private mbFilling As Boolean

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Const INVALID_CHARACTERS As String = "|"

    Private Enum geStoreCol
        StoreNo = 0
        StoreName = 1
        PrimaryVendor = 2
        ZoneID = 3
        State = 4
        WFMStore = 5
        MegaStore = 6
        CustomerType = 7
    End Enum

    Private Sub frmItemVendorID_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmItemVendorID_Load Entry")

        '-- Center the form
        CenterForm(Me)
        'Me.StartPosition = FormStartPosition.CenterScreen
        Call SetupDataTable()
        Try
            '-- Load the data onto the screen
            gRSRecordset = SQLOpenRecordSet("EXEC GetItemIDInfo " & glItemID & ", " & glVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough + DAO.RecordsetOptionEnum.dbForwardOnly)
            txtVendor.Text = gRSRecordset.Fields("CompanyName").Value & ""
            txtItem_Description.Text = gRSRecordset.Fields("Item_Description").Value & ""
            txtIdentifier.Text = gRSRecordset.Fields("Identifier").Value & ""
            txtItemID.Text = Trim(gRSRecordset.Fields("Item_ID").Value & "")
            m_sOrigVItem_ID = Trim(gRSRecordset.Fields("Item_ID").Value & "")

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

        Call LoadZone(cmbZones)

        '-- Set up the grid
        Call LoadDataTable("EXEC GetItemVendorStores " & glItemID & ", " & glVendorID)

        Call LoadStates()

        Call SetCombos()

        If Not CheckAllStoreSelectionEnabled() Then
            _optSelection_1.Text = "All 365"
        End If

        logger.Debug("frmItemVendorID_Load Exit")
    End Sub

    Private Sub SetupDataTable()
        logger.Debug("SetupDataTable Entry")

        ' Create a data table
        mdt = New DataTable("StorePrimVend")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("Store_name", GetType(String)))
        mdt.Columns.Add(New DataColumn("PrimaryVendor", GetType(Boolean)))
        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("Store_no", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Zone_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("State", GetType(String)))
        mdt.Columns.Add(New DataColumn("WFM_Store", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("Mega_Store", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("CustomerType", GetType(Int16)))

        logger.Debug("SetupDataTable Exit")
    End Sub

    Private Sub LoadDataTable(ByVal sSearchSQL As String)
        logger.Debug("LoadDataTable Entry")

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
                row("Store_no") = rsSearch.Fields("Store_no").Value
                row("Store_name") = rsSearch.Fields("Store_name").Value
                row("PrimaryVendor") = rsSearch.Fields("PrimaryVendor").Value
                row("Zone_ID") = rsSearch.Fields("Zone_ID").Value
                If IsDBNull(rsSearch.Fields("State").Value) Then
                    row("State") = ""
                Else
                    row("State") = rsSearch.Fields("State").Value
                End If
                row("WFM_Store") = rsSearch.Fields("WFM_Store").Value
                row("Mega_Store") = rsSearch.Fields("Mega_Store").Value
                row("CustomerType") = rsSearch.Fields("CustomerType").Value
                mdt.Rows.Add(row)

                rsSearch.MoveNext()
            End While

            'Setup a column that you would like to sort on initially.
            mdt.AcceptChanges()
            mdv = New System.Data.DataView(mdt)
            mdv.Sort = "Store_Name"
            ugrdSIV.DataSource = mdv

        Finally
            If rsSearch IsNot Nothing Then
                rsSearch.Close()
                rsSearch = Nothing
            End If
        End Try

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

        logger.Debug("LoadDataTable Exit")
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
        logger.Debug("cmdAdd_Click Entry")

        Dim fItemVendorStore As New frmItemVendorStore
        fItemVendorStore.ShowDialog()
        fItemVendorStore.Dispose()
        Call LoadDataTable("EXEC GetItemVendorStores " & glItemID & ", " & glVendorID)

        logger.Debug("cmdAdd_Click Exit")
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
        logger.Debug("cmdDelete_Click Entry")

        Dim iTotSelRows As Short
        Dim iCnt As Short
        Dim sReturnDate As String
        Dim msg As New StringBuilder
        Dim msgStoreList As New StringBuilder
        Dim dt As New DataTable
        Dim row As DataRow
        dt.Columns.Add(New DataColumn("Store_Name", GetType(String)))

        'roll through the grid and delete each highlighted item from StoreItemVendor
        iTotSelRows = ugrdSIV.Selected.Rows.Count
        Dim fSelectDate As frmSelectDate
        Dim fPrimVendSelect As frmNewPrimVend
        Dim rsChkPrimVend As DAO.Recordset = Nothing
        If iTotSelRows = 0 Then
            MsgBox(ResourcesItemHosting.GetString("SelectStoreRemove"), MsgBoxStyle.Information, Me.Text)
            logger.Info(ResourcesItemHosting.GetString("SelectStoreRemove"))
        Else
            '--Get the remove date
            fSelectDate = New frmSelectDate(ResourcesItemHosting.GetString("EnterDateStoreRemove"), SystemDateTime(True))
            fSelectDate.ShowDialog()
            sReturnDate = fSelectDate.ReturnDate
            If fSelectDate.ReturnDate <> "" Then
                For iCnt = ugrdSIV.Selected.Rows.Count - 1 To 0 Step -1
                    Try
                        Dim storeNo As Integer = CInt(ugrdSIV.Selected.Rows(iCnt).Cells("Store_no").Value)
                        rsChkPrimVend = SQLOpenRecordSet("EXEC CheckIfPrimVendCanSwap " & glVendorID & ", " & glItemID & ", " & storeNo, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbForwardOnly + DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        If rsChkPrimVend.Fields("IsPrimVend").Value = 1 Then
                            fPrimVendSelect = New frmNewPrimVend(CStr(Me.txtVendor.Text), glVendorID, glItemID, storeNo)

                            fPrimVendSelect.ShowDialog()
                            If fPrimVendSelect.UnassignedItems = 0 Then
                                '-- Delete StoreItemVendor
                                SQLExecute("EXEC DeleteStoreItemVendor " & glVendorID & ", " & storeNo & ", " & glItemID & ", '" & VB6.Format(fSelectDate.ReturnDate, "YYYY-MM-DD") & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)

                                MammothEventDAO.CreateItemDeleteEvent(New MammothEventBO With {.ItemKey = glItemID, .StoreNo = storeNo})

                                If sReturnDate = VB6.Format(SystemDateTime(True), ResourcesIRMA.GetString("DateStringFormat")) Then ugrdSIV.Selected.Rows(iCnt).Delete(False)
                            Else
                                Call MsgBox(ResourcesItemHosting.GetString("SelectPrimaryVendor"), MsgBoxStyle.Critical, Me.Text)
                                logger.Info(ResourcesItemHosting.GetString("SelectPrimaryVendor"))
                            End If
                            fPrimVendSelect.Close()
                            fPrimVendSelect.Dispose()
                        Else
                            rsChkPrimVend.Close()
                            rsChkPrimVend = Nothing
                            '-- Delete StoreItemVendor
                            SQLExecute("EXEC DeleteStoreItemVendor " & glVendorID & ", " & storeNo & ", " & glItemID & ", '" & VB6.Format(fSelectDate.ReturnDate, "YYYY-MM-DD") & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)

                            MammothEventDAO.CreateItemDeleteEvent(New MammothEventBO With {.ItemKey = glItemID, .StoreNo = storeNo})

                            'track this store-name as a store that is being de-authorized
                            row = dt.NewRow
                            Dim STR As String

                            STR = "     * "
                            STR += ugrdSIV.Selected.Rows(iCnt).Cells("Store_Name").Value


                            row("Store_Name") = STR.ToString
                            dt.Rows.Add(row)

                            If sReturnDate = VB6.Format(SystemDateTime(True), ResourcesIRMA.GetString("DateStringFormat")) Then ugrdSIV.Selected.Rows(iCnt).Delete(False)
                        End If
                    Finally
                        If rsChkPrimVend IsNot Nothing Then
                            rsChkPrimVend.Close()
                            rsChkPrimVend = Nothing
                        End If
                    End Try
                Next
                fSelectDate.Close()
                fSelectDate.Dispose()

                'code to display list of store which were deleted and to show the message in a different window
                DeletedStoresList.LoadForm(dt, ResourcesItemHosting.GetString("msg_warning_DeleteVendor_DeAuthMsg1_Alt"), ResourcesItemHosting.GetString("msg_warning_DeleteVendor_DeAuthMsg2"))

                'display warning to user
                MsgBox(msg.ToString, MsgBoxStyle.Exclamation, Me.Text)
            End If
        End If

        '-- Reload the grid.
        Call LoadDataTable("EXEC GetItemVendorStores " & glItemID & ", " & glVendorID)

        logger.Debug("cmdDelete_Click Exit")
    End Sub

    ''' <summary>
    ''' Save any changes to the Vendor's ID to the database.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function SaveVendorId() As Boolean
        logger.Debug("SaveVendorId Entry")

        Dim saveSuccess As Boolean = True
        ' The vendor id is a required field.  However, if the user blanked it out and they select not to save their changes,
        ' they are not prompted to correct the blank value because it is not written to the database.

        'save the vendor item id if it's different than it's original value
        If m_sOrigVItem_ID <> Trim(txtItemID.Text) Then
            ' prompt the user to see if they want to save the changes
            If MsgBox(String.Format(ResourcesIRMA.GetString("SaveChanges1"), ResourcesItemHosting.GetString("VendorIdChangedForSave")), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                ' Make sure the new value is not blank
                If txtItemID.Text.Trim.Equals("") Then
                    'alert user to provide a value
                    MsgBox(String.Format(ResourcesIRMA.GetString("Required"), _lblLabel_3.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
                    txtItemID.Focus()
                    logger.Info(String.Format(ResourcesIRMA.GetString("Required"), _lblLabel_3.Text.Replace(":", "")))
                    saveSuccess = False
                ElseIf txtItemID.Text.Trim.ToCharArray().Any(Function(c) INVALID_CHARACTERS.Contains(c)) Then
                    ' Make sure the new value does not contain illegal characters
                    MsgBox(String.Format(ResourcesIRMA.GetString("InvalidCharacters"), _lblLabel_3.Text.Replace(":", ""), INVALID_CHARACTERS), MsgBoxStyle.Critical, Me.Text)
                    txtItemID.Focus()
                    logger.Info(String.Format(ResourcesIRMA.GetString("InvalidCharacters"), _lblLabel_3.Text.Replace(":", ""), INVALID_CHARACTERS))
                    saveSuccess = False
                Else
                    SQLExecute("EXEC UpdateItemVendor " & glItemID & ", " & glVendorID & ", '" & Trim(txtItemID.Text) & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    ' Refresh the original value so the user is not prompted to save again on form closing
                    m_sOrigVItem_ID = txtItemID.Text
                End If
            Else
                ' Refresh the value with the original since the user did not save
                txtItemID.Text = m_sOrigVItem_ID
            End If
        End If

        ' make sure the original value was not blank - this is a newly required field so old data
        ' might need to be updated
        If m_sOrigVItem_ID.Equals("") Then
            'alert user to provide a value
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), _lblLabel_3.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            txtItemID.Focus()
            logger.Info(String.Format(ResourcesIRMA.GetString("Required"), _lblLabel_3.Text.Replace(":", "")))
            saveSuccess = False
        End If

        logger.Debug("SaveVendorId Exit with " + saveSuccess.ToString)
        Return saveSuccess
    End Function

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        ' Save the vendor id - this is a required field for the form
        logger.Debug("cmdExit_Click Entry")

        If SaveVendorId() Then
            Me.Close()
        End If

        logger.Debug("cmdExit_Click Exit")
    End Sub

    Private Sub cmdSetPrimaryVend_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSetPrimaryVend.Click
        logger.Debug("cmdSetPrimaryVend_Click Entry")

        Dim iTotSelRows As Short
        Dim iLoop As Short
        Dim StoreLst As String
        StoreLst = String.Empty

        iTotSelRows = ugrdSIV.Selected.Rows.Count
        If iTotSelRows = 0 Then
            Call MsgBox(ResourcesItemHosting.GetString("SelectStore"), MsgBoxStyle.Critical, Me.Text)
            logger.Info(ResourcesItemHosting.GetString("SelectStore"))
            logger.Debug("cmdSetPrimaryVend_Click Exit")
            Exit Sub
        End If

        'roll through the grid and delete each highlighted item from StoreItemVendor
        For iLoop = 0 To iTotSelRows - 1
            If ugrdSIV.Selected.Rows(iLoop).Cells("PrimaryVendor").Value = False Then
                StoreLst = StoreLst & "|" & ugrdSIV.Selected.Rows(iLoop).Cells("Store_no").Value
                ugrdSIV.Selected.Rows(iLoop).Cells("PrimaryVendor").Value = True
            End If
        Next iLoop
        If Len(StoreLst) > 0 Then
            StoreLst = VB.Right(StoreLst, Len(StoreLst) - 1)
            SQLExecute("EXEC SetPrimaryVendor '" & StoreLst & "', " & glItemID & ", " & glVendorID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        Else
            If iTotSelRows > 1 Then
                ' plural stores
                Call MsgBox(ResourcesItemHosting.GetString("StoresPrimaryAlready"), MsgBoxStyle.Critical, Me.Text)
                logger.Info(ResourcesItemHosting.GetString("StoresPrimaryAlready"))
            Else
                ' one store
                Call MsgBox(ResourcesItemHosting.GetString("StorePrimaryAlready"), MsgBoxStyle.Critical, Me.Text)
                logger.Info(ResourcesItemHosting.GetString("StorePrimaryAlready"))
            End If
        End If

        '-- Reload the grid.
        Call LoadDataTable("EXEC GetItemVendorStores " & glItemID & ", " & glVendorID)

        logger.Debug("cmdSetPrimaryVend_Click Exit")
    End Sub

    Private Sub frmItemVendorID_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("frmItemVendorID_FormClosed Entry")

        ' Save the vendor id - this is a required field for the form
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason

        If UnloadMode = System.Windows.Forms.CloseReason.UserClosing Then
            If SaveVendorId() Then
                Cancel = False
            Else
                Cancel = True
            End If
        End If

        eventArgs.Cancel = Cancel

        logger.Debug("frmItemVendorID_FormClosed Exit")
    End Sub

    Private Sub txtItemID_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtItemID.Enter
        txtItemID.SelectAll()
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

    Private Sub ClearSelections()
        logger.Debug("ClearSelections Entry")

        Dim iCnt As Int16
        For iCnt = 0 To ugrdSIV.Rows.Count - 1
            ugrdSIV.Rows(iCnt).Selected = False
        Next

        logger.Debug("ClearSelections Exit")
    End Sub

    Private Sub OptSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSelection.CheckedChanged
        If Me.IsInitializing Then Exit Sub

        logger.Debug("OptSelection_CheckedChanged Entry")

        If eventSender.Checked Then
            Dim Index As Short = optSelection.GetIndex(eventSender)
            Dim iLoop As Short
            Dim iFirstStore As Short

            Call SetCombos()

            If mbNoClick = True Then Exit Sub
            iFirstStore = -1

            Select Case Index
                Case 0
                    '-- Manual.
                    ugrdSIV.Selected.Rows.Clear()
                    cmbZones.SelectedIndex = -1
                    cmbStates.SelectedIndex = -1
                Case 1
                    '-- All Stores or All 365 for RM
                    ClearSelections()
                    If CheckAllStoreSelectionEnabled() Then
                        For iLoop = 0 To ugrdSIV.Rows.Count - 1
                            iFirstStore = 0
                            ugrdSIV.Rows(iLoop).Selected = True
                        Next iLoop
                    Else
                        For iLoop = 0 To ugrdSIV.Rows.Count - 1
                            If ugrdSIV.Rows(iLoop).Cells("Mega_Store").Value = True Then
                                If iFirstStore = -1 Then
                                    iFirstStore = iLoop
                                End If
                                ugrdSIV.Rows(iLoop).Selected = True
                            End If
                        Next iLoop
                    End If
                Case 2
                    '-- By Zones.
                    ClearSelections()
                    If cmbZones.SelectedIndex > -1 Then
                        For iLoop = 0 To ugrdSIV.Rows.Count - 1
                            If CDbl(ugrdSIV.Rows(iLoop).Cells("Zone_ID").Value) = VB6.GetItemData(cmbZones, cmbZones.SelectedIndex) Then
                                If iFirstStore = -1 Then
                                    iFirstStore = iLoop
                                End If
                                ugrdSIV.Rows(iLoop).Selected = True
                            End If
                        Next iLoop
                    End If

                Case 3
                    '-- By State.
                    ClearSelections()
                    Call SelectStates(iFirstStore)

                Case 4
                    '-- All WFM.
                    ClearSelections()
                    For iLoop = 0 To ugrdSIV.Rows.Count - 1
                        If ugrdSIV.Rows(iLoop).Cells("WFM_Store").Value = True Then
                            If iFirstStore = -1 Then
                                iFirstStore = iLoop
                            End If
                            ugrdSIV.Rows(iLoop).Selected = True
                        End If
                    Next iLoop

                Case 5
                    '-- 5 = All Region.
                    ClearSelections()
                    For iLoop = 0 To ugrdSIV.Rows.Count - 1
                        If ugrdSIV.Rows(iLoop).Cells("CustomerType").Value = "3" Then
                            If iFirstStore = -1 Then
                                iFirstStore = iLoop
                            End If
                            ugrdSIV.Rows(iLoop).Selected = True
                        End If
                    Next iLoop

                Case 6
                    '-- 6 = All Region - Retail Only.
                    ClearSelections()
                    For iLoop = 0 To ugrdSIV.Rows.Count - 1
                        If ugrdSIV.Rows(iLoop).Cells("CustomerType").Value = "3" _
                                    And (ugrdSIV.Rows(iLoop).Cells("Mega_store").Value = True _
                                    Or ugrdSIV.Rows(iLoop).Cells("WFM_Store").Value = True) Then
                            If iFirstStore = -1 Then
                                iFirstStore = iLoop
                            End If
                            ugrdSIV.Rows(iLoop).Selected = True
                        End If
                    Next iLoop
            End Select

            ugrdSIV.ActiveRow = Nothing

        End If

        logger.Debug("OptSelection_CheckedChanged Exit")
    End Sub

    Private Sub SelectStates(ByRef iFirstStore As Short)
        logger.Debug("SelectStates Entry")
        Dim iLoop As Short

        ClearSelections()
        For iLoop = 0 To ugrdSIV.Rows.Count - 1
            If cmbStates.Text = "" Then Exit For
            Debug.Print(ugrdSIV.Rows(iLoop).Cells("Store_name").Value)
            If ugrdSIV.Rows(iLoop).Cells("State").Value = cmbStates.Text Then
                If iFirstStore = -1 Then
                    iFirstStore = iLoop
                End If
                ugrdSIV.Rows(iLoop).Selected = True
            End If
        Next iLoop

        logger.Debug("SelectStates Exit")
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

    Private Sub cmbStates_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStates.SelectedIndexChanged
        logger.Debug("cmbStates_SelectedIndexChanged Entry")
        Dim iFirstStore As Short
        If Me.IsInitializing Then Exit Sub
        If mbFilling Then Exit Sub

        iFirstStore = -1

        ugrdSIV.ActiveRow = Nothing

        Call SelectStates(iFirstStore)
        logger.Debug("cmbStates_SelectedIndexChanged Exit")
    End Sub

    Private Sub cmbZones_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbZones.SelectedIndexChanged
        If Me.IsInitializing Then Exit Sub

        logger.Debug("cmbZones_SelectedIndexChanged Entry")

        If mbFilling Then Exit Sub

        optSelection(2).Checked = True
        OptSelection_CheckedChanged(optSelection.Item(2), New System.EventArgs())

        logger.Debug("cmbZones_SelectedIndexChanged Exit")
    End Sub

    Private Sub LoadStates()
        logger.Debug("LoadStates Entry")

        Dim iLoop As Short

        For iLoop = 0 To ugrdSIV.Rows.Count - 1
            If Not IsDBNull(ugrdSIV.Rows(iLoop).Cells("State").Value) Then
                If Not StateInList(ugrdSIV.Rows(iLoop).Cells("State").Value) And Trim(ugrdSIV.Rows(iLoop).Cells("State").Value) <> "" Then
                    cmbStates.Items.Add((ugrdSIV.Rows(iLoop).Cells("State").Value))
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

    Private Sub ugrdSIV_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdSIV.Click
        logger.Debug("ugrdSIV_Click Entry")

        mbNoClick = True
        Me.optSelection(0).Checked = True
        mbNoClick = False

        logger.Debug("ugrdSIV_Click Exit")
    End Sub

    ' for bug 5442: not being able to tab to the "By State" option button (_optSelection_3) from cboZones
    ' or tabbing from cmbStates to ugrdSIV
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
            ugrdSIV.Focus()
            CurrentKey = Nothing
        End If
    End Sub
    ' for bug 5442: end

#Region "Tabbing code"
    ' Commented out by Rick Kelleher 2/27/08 while fixing bug 5442

    'Private Sub cmbStates_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStates.Leave
    '    ugrdSIV.Focus()
    'End Sub

    'Private Sub cmbStates_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStates.LostFocus
    '    ugrdSIV.Focus()
    'End Sub

    'Private Sub cmbZones_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbZones.Leave
    '    _optSelection_3.Focus()
    'End Sub

    'Private Sub cmbZones_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbZones.LostFocus
    '    _optSelection_3.Focus()
    'End Sub

    'Private Sub txtItemID_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemID.Leave
    '    _optSelection_0.Focus()
    'End Sub

    'Private Sub txtItemID_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemID.LostFocus
    '    _optSelection_0.Focus()
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

    'Private Sub ugrdSIV_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdSIV.Leave
    '    cmdAdd.Focus()
    'End Sub

    'Private Sub ugrdSIV_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdSIV.LostFocus
    '    cmdAdd.Focus()
    'End Sub

    'Private Sub _optSelection_1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_1.Leave
    '    _optSelection_6.Focus()
    'End Sub

    'Private Sub _optSelection_1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_1.LostFocus
    '    _optSelection_6.Focus()
    'End Sub

#End Region

End Class
