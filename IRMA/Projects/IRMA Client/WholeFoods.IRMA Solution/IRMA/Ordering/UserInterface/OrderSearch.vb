Option Strict Off
Option Explicit On
Imports log4net
Imports WholeFoods.IRMA.Ordering.DataAccess

Friend Class frmOrdersSearch
    Inherits System.Windows.Forms.Form

    Private mdt As DataTable
    Private mdv As DataView
    Private IsInitializing As Boolean

    Private IsMultipleJurisdiction As Boolean

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public AutoSearch_OrderHeaderId As Integer?

    Private Sub SetupDataTable()

        mdt = New DataTable("OrderSearch")

        'Visible on grid.
        mdt.Columns.Add(New DataColumn("Return_Order", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("OrderHeader_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("CompanyName", GetType(String)))
        mdt.Columns.Add(New DataColumn("OrderDate", GetType(Date)))
        mdt.Columns.Add(New DataColumn("Expected_Date", GetType(Date)))
        mdt.Columns.Add(New DataColumn("SentDate", GetType(Date)))
        mdt.Columns.Add(New DataColumn("SubTeam", GetType(String)))
        mdt.Columns.Add(New DataColumn("EInvoice", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("OrderedCost", GetType(Single)))
        mdt.Columns.Add(New DataColumn("CloseDate", GetType(Date)))
        mdt.Columns.Add(New DataColumn("DeleteDate", GetType(Date)))
        mdt.Columns.Add(New DataColumn("Source", GetType(String)))
        mdt.Columns.Add(New DataColumn("Jurisdiction", GetType(String)))

    End Sub

    Private Sub cmbField_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbField.KeyPress
        logger.Debug("cmbField_KeyPress Entry")

        Dim KeyAscii As Short = Asc(e.KeyChar)
        Dim Index As Short = cmbField.GetIndex(eventSender)

        If KeyAscii = 8 Then
            cmbField(Index).SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)

        If KeyAscii = 0 Then
            e.Handled = True
        End If

        logger.Debug("cmbField_KeyPress Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        logger.Debug("cmdSearch_Click Entry")
        RunSearch()
        logger.Debug("cmdSearch_Click exit")
    End Sub

    Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click
        logger.Debug("cmdSelect_Click Entry")
        ReturnSelection()
        logger.Debug("cmdSelect_Click Exit")
    End Sub

    Private Sub frmOrdersSearch_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmOrdersSearch_Load Entry")

        Dim iLoop As Short
        Dim iSend As Short

        IsMultipleJurisdiction = OrderSearchDAO.IsMultipleJurisdiction()

        ugrdSearchResults.DisplayLayout.Bands(0).Columns("Jurisdiction").Hidden = Not IsMultipleJurisdiction
 
        '-- Center the form and the buttons on the form
        CenterForm(Me)

        LoadUsers(cmbField(0))
        LoadAllSubTeams(cmbField(1))
        ReplicateCombo(cmbField(1), cmbField(3))
        LoadCustomer(cmbField(2))
        LoadSources()

        '-- Load it with defaults
        txtField(0).Text = GetSetting("IRMA", "OrderSearch", "PONumber")
        txtField(4).Text = GetSetting("IRMA", "OrderSearch", "VendorName")
        dtpOrderDate.Value = GetSetting("IRMA", "OrderSearch", "OrderDate")
        dtpSentDate.Value = GetSetting("IRMA", "OrderSearch", "SentDate")
        txtField(3).Text = GetSetting("IRMA", "OrderSearch", "VendorPO")
        txtField(5).Text = GetSetting("IRMA", "OrderSearch", "Identifier")
        txtField(6).Text = GetSetting("IRMA", "OrderSearch", "ItemDesc")
        dtpWarehouseSentDate.Value = GetSetting("IRMA", "OrderSearch", "WareHouseSentDate")
        dtpExpectedDate.Value = GetSetting("IRMA", "OrderSearch", "ExpectedDate")
        iSend = Val(GetSetting("IRMA", "OrderSearch", "Send"))
        If iSend <= optSend.UBound Then optSend(iSend).Checked = True
        optOpen(Val(GetSetting("IRMA", "OrderSearch", "Open"))).Checked = True
        optType(Val(GetSetting("IRMA", "OrderSearch", "Type"))).Checked = True
        optCredit(Val(GetSetting("IRMA", "OrderSearch", "Credit"))).Checked = True

        If Val(GetSetting("IRMA", "OrderSearch", "CreatedBy", "-1")) >= 0 Then
            For iLoop = 0 To cmbField(0).Items.Count - 1
                If VB6.GetItemData(cmbField(0), iLoop) = Val(GetSetting("IRMA", "OrderSearch", "CreatedBy")) Then
                    cmbField(0).SelectedIndex = iLoop
                    logger.Info("frmOrdersSearch_Load OrderSearch-CreatedBy")
                    Exit For
                End If
            Next iLoop
        End If

        If Val(GetSetting("IRMA", "OrderSearch", "Transfer_SubTeam", "-1")) >= 0 Then
            For iLoop = 0 To cmbField(1).Items.Count - 1
                If VB6.GetItemData(cmbField(1), iLoop) = Val(GetSetting("IRMA", "OrderSearch", "Transfer_SubTeam")) Then
                    cmbField(1).SelectedIndex = iLoop
                    logger.Info("frmOrdersSearch_Load OrderSearch-Transfer_SubTeam")
                    Exit For
                End If
            Next iLoop
        End If

        If Val(GetSetting("IRMA", "OrderSearch", "Transfer_To_SubTeam", "-1")) >= 0 Then
            For iLoop = 0 To cmbField(3).Items.Count - 1
                If VB6.GetItemData(cmbField(3), iLoop) = Val(GetSetting("IRMA", "OrderSearch", "Transfer_To_SubTeam")) Then
                    cmbField(3).SelectedIndex = iLoop
                    logger.Info("frmOrdersSearch_Load OrderSearch-Transfer_To_SubTeam")
                    Exit For
                End If
            Next iLoop
        End If

        If Val(GetSetting("IRMA", "OrderSearch", "ReceiveLocation", "-1")) >= 0 Then
            For iLoop = 0 To cmbField(2).Items.Count - 1
                If VB6.GetItemData(cmbField(2), iLoop) = Val(GetSetting("IRMA", "OrderSearch", "ReceiveLocation")) Then
                    cmbField(2).SelectedIndex = iLoop
                    logger.Info("frmOrdersSearch_Load OrderSearch-ReceiveLocation")
                    Exit For
                End If
            Next iLoop
        End If

        Call SetupDataTable()

        If Me.AutoSearch_OrderHeaderId IsNot Nothing Then
            _txtField_0.Text = Me.AutoSearch_OrderHeaderId
            RunSearch()
            Me.AutoSearch_OrderHeaderId = Nothing
        End If

        Global.SetUltraGridSelectionStyle(ugrdSearchResults)
        logger.Debug("frmOrdersSearch_Load Exit")
    End Sub

    Private Sub frmOrdersSearch_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        logger.Debug("frmOrdersSearch_FormClosed Entry")

        SaveSetting("IRMA", "OrderSearch", "PONumber", Trim(txtField(0).Text))
        SaveSetting("IRMA", "OrderSearch", "VendorName", Trim(txtField(4).Text))
        SaveSetting("IRMA", "OrderSearch", "OrderDate", FixNull(dtpOrderDate.Value))
        SaveSetting("IRMA", "OrderSearch", "SentDate", FixNull(dtpSentDate.Value))
        SaveSetting("IRMA", "OrderSearch", "VendorPO", Trim(txtField(3).Text))
        SaveSetting("IRMA", "OrderSearch", "Identifier", Trim(txtField(5).Text))
        SaveSetting("IRMA", "OrderSearch", "ItemDesc", Trim(txtField(6).Text))
        SaveSetting("IRMA", "OrderSearch", "WareHouseSentDate", FixNull(dtpWarehouseSentDate.Value))
        SaveSetting("IRMA", "OrderSearch", "ExpectedDate", FixNull(dtpExpectedDate.Value))

        '-- Set up order default
        Select Case True
            Case optSend(0).Checked : SaveSetting("IRMA", "OrderSearch", "Send", "0")
            Case optSend(1).Checked : SaveSetting("IRMA", "OrderSearch", "Send", "1")
        End Select
        Select Case True
            Case optOpen(0).Checked : SaveSetting("IRMA", "OrderSearch", "Open", "0")
            Case optOpen(1).Checked : SaveSetting("IRMA", "OrderSearch", "Open", "1")
            Case optOpen(2).Checked : SaveSetting("IRMA", "OrderSearch", "Open", "2")
            Case optOpen(3).Checked : SaveSetting("IRMA", "OrderSearch", "Open", "3")
        End Select
        Select Case True
            Case optType(0).Checked : SaveSetting("IRMA", "OrderSearch", "Type", "0")
            Case optType(1).Checked : SaveSetting("IRMA", "OrderSearch", "Type", "1")
            Case optType(2).Checked : SaveSetting("IRMA", "OrderSearch", "Type", "2")
            Case optType(3).Checked : SaveSetting("IRMA", "OrderSearch", "Type", "3")
        End Select
        Select Case True
            Case optCredit(0).Checked : SaveSetting("IRMA", "OrderSearch", "Credit", "0")
            Case optCredit(1).Checked : SaveSetting("IRMA", "OrderSearch", "Credit", "1")
            Case optCredit(2).Checked : SaveSetting("IRMA", "OrderSearch", "Credit", "2")
        End Select

        If cmbField(0).SelectedIndex = -1 Then
            SaveSetting("IRMA", "OrderSearch", "CreatedBy", "-1")
        Else
            SaveSetting("IRMA", "OrderSearch", "CreatedBy", Str(VB6.GetItemData(cmbField(0), cmbField(0).SelectedIndex)))
        End If

        If cmbField(1).SelectedIndex = -1 Then
            SaveSetting("IRMA", "OrderSearch", "Transfer_SubTeam", "-1")
        Else
            SaveSetting("IRMA", "OrderSearch", "Transfer_SubTeam", Str(VB6.GetItemData(cmbField(1), cmbField(1).SelectedIndex)))
        End If

        If cmbField(3).SelectedIndex = -1 Then
            SaveSetting("IRMA", "OrderSearch", "Transfer_To_SubTeam", "-1")
        Else
            SaveSetting("IRMA", "OrderSearch", "Transfer_To_SubTeam", Str(VB6.GetItemData(cmbField(3), cmbField(3).SelectedIndex)))
        End If

        If cmbField(2).SelectedIndex = -1 Then
            SaveSetting("IRMA", "OrderSearch", "ReceiveLocation", "-1")
        Else
            SaveSetting("IRMA", "OrderSearch", "ReceiveLocation", Str(VB6.GetItemData(cmbField(2), cmbField(2).SelectedIndex)))
        End If

        logger.Debug("frmOrdersSearch_FormClosed Exit")
    End Sub

  Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
    CType(eventSender, TextBox).SelectAll()
  End Sub

  Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
        logger.Debug("txtField_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        Dim Index As Short = txtField.GetIndex(eventSender)

        'allows copy function in the keyboard, especially for the PO# TextBox
        If KeyAscii <> 3 Then
            KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)
        End If

        eventArgs.KeyChar = Chr(KeyAscii)

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If

        logger.Debug("txtField_KeyPress Exit")
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        _cmbField_0.SelectedIndex = -1
        _cmbField_1.SelectedIndex = -1
        _cmbField_2.SelectedIndex = -1
        _cmbField_3.SelectedIndex = -1
        _cmbField_4.SelectedIndex = -1
        _txtField_0.Text = String.Empty
        _txtField_3.Text = String.Empty
        _txtField_4.Text = String.Empty
        _txtField_5.Text = String.Empty
        _txtField_6.Text = String.Empty
        _txtField_9.Text = String.Empty
        optSend(0).Checked = True
        optOpen(0).Checked = True
        optType(0).Checked = True
        optCredit(0).Checked = True
        dtpOrderDate.Value = String.Empty
        dtpSentDate.Value = String.Empty
        dtpWarehouseSentDate.Value = String.Empty
        dtpExpectedDate.Value = String.Empty
        _cmbField_4.SelectedIndex = -1
        chkEInvoice.Checked = False
        chkFromQueue.Checked = False
        chkRefusedPO.Checked = False
        chkPartialShipment.Checked = False
    End Sub

    Public Sub RunSearch()
        logger.Debug("RunSearch Entry")

        '-- Local variables declared
        Dim iNumOfRows As Short
        Dim lOrderHeader_ID As Integer
        Dim sInvoiceNumber As String
        Dim dOrderDate As Date
        Dim dExpectedDate As Date
        Dim dSentDate As Date
        Dim dWarehouseSentDate As Date
        Dim iSend As Short
        Dim OrderStatus As Short = 1
        Dim iOpen1 As Short
        Dim iOpen2 As Short
        Dim iOrderType_ID As Short
        Dim iType3 As Short
        Dim iType4 As Short
        Dim sVendor As String
        Dim iCreatedBy As Short
        Dim lTransfer_SubTeam As Integer
        Dim lTransfer_To_SubTeam As Integer
        Dim iReceiveLocation As Integer
        Dim sIdentifier As String
        Dim sItem_Description As String
        Dim sLotNo As String
        Dim iSourceID As Integer
        Dim dtResultSet As DataTable

        sInvoiceNumber = String.Empty
        sVendor = String.Empty
        sIdentifier = String.Empty
        sItem_Description = String.Empty
        sLotNo = String.Empty

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        '-- Check the orders status
        If Trim(txtField(0).Text) <> vbNullString Then
            lOrderHeader_ID = CInt(Trim(txtField(0).Text))
        Else
            '-- Do the date search
            If dtpOrderDate.IsDateValid And (Len(dtpOrderDate.Value) > 0) Then
                dOrderDate = dtpOrderDate.Value
            Else
                dOrderDate = Nothing
            End If

            If dtpExpectedDate.IsDateValid And (Len(dtpExpectedDate.Value) > 0) Then
                dExpectedDate = dtpExpectedDate.Value
            Else
                dExpectedDate = Nothing
            End If

            If dtpSentDate.IsDateValid And (Len(dtpSentDate.Value) > 0) Then
                dSentDate = dtpSentDate.Value
            Else
                dSentDate = Nothing
            End If

            If dtpWarehouseSentDate.IsDateValid And (Len(dtpWarehouseSentDate.Value) > 0) Then
                dWarehouseSentDate = dtpWarehouseSentDate.Value
            Else
                dWarehouseSentDate = Nothing
            End If

            sInvoiceNumber = Trim(txtField(3).Text)

            '-- Option buttons
            iSend = IIf(optSend(1).Checked, 1, 0)

            For i As Integer = 1 To optOpen.Count()
                If optOpen(i - 1).Checked Then OrderStatus = i
            Next

            iOpen1 = IIf(optOpen(1).Checked, 1, 0)
            iOpen2 = IIf(optOpen(2).Checked, 1, 0)

            Select Case True
                Case optType(1).Checked : iOrderType_ID = 1
                Case optType(2).Checked : iOrderType_ID = 2
                Case optType(3).Checked : iOrderType_ID = 3
                Case Else : iOrderType_ID = 0
            End Select

            iType3 = IIf(optCredit(2).Checked, 1, 0)
            iType4 = IIf(optCredit(1).Checked, 1, 0)

            sVendor = Trim(txtField(4).Text)

            If cmbField(0).SelectedIndex > -1 Then
                iCreatedBy = VB6.GetItemData(cmbField(0), cmbField(0).SelectedIndex)
            End If

            If cmbField(1).SelectedIndex > -1 Then
                lTransfer_SubTeam = VB6.GetItemData(cmbField(1), cmbField(1).SelectedIndex)
            End If

            If cmbField(3).SelectedIndex > -1 Then
                lTransfer_To_SubTeam = VB6.GetItemData(cmbField(3), cmbField(3).SelectedIndex)
            End If

            If cmbField(2).SelectedIndex > -1 Then
                iReceiveLocation = VB6.GetItemData(cmbField(2), cmbField(2).SelectedIndex)
            End If

            If cmbField(4).SelectedIndex > -1 Then
                iSourceID = cmbField(4).SelectedValue
            End If

            sIdentifier = Trim(txtField(5).Text)
            sItem_Description = Trim(txtField(6).Text)
            sLotNo = Trim(txtField(9).Text)

        End If

        'Removed the call to sub LoadDataTable. Instead, calling GetORderSearch in OrderingDAO.
        dtResultSet = OrderingDAO.GetOrderSearch(lOrderHeader_ID, dOrderDate, dSentDate, sInvoiceNumber, iSend, _
                                                 OrderStatus, iOrderType_ID, iType3, iType4, sVendor, _
                                                 iCreatedBy, lTransfer_SubTeam, lTransfer_To_SubTeam, iReceiveLocation, sIdentifier, _
                                                 sItem_Description, dWarehouseSentDate, dExpectedDate, sLotNo, Val(CStr(chkFromQueue.CheckState)), _
                                                 Val(CStr(chkEInvoice.CheckState)), iSourceID, chkPartialShipment.Checked, chkRefusedPO.Checked)

        Me.ugrdSearchResults.DataSource = dtResultSet

        ugrdSearchResults.DisplayLayout.Bands(0).SortedColumns.Clear()
        ugrdSearchResults.DisplayLayout.Bands(0).SortedColumns.Add("OrderDate", True, False)
        ugrdSearchResults.Refresh()

        iNumOfRows = dtResultSet.Rows.Count

        Select Case iNumOfRows
            Case 0
                MsgBox("No items found.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            Case 1
                ugrdSearchResults.Rows(0).Selected = True
                ReturnSelection()
            Case 2 To 1000
                ugrdSearchResults.Rows(0).Selected = True
            Case Is > 1000
                MsgBox("More data is available." & vbCrLf & "To refine the results, please limit the search criteria.", MsgBoxStyle.Information, Me.Text)
                logger.Info("RunSearch" & "More data is available." & vbCrLf & "For more data, please limit search criteria.")
            Case Else
                MsgBox("No items found.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
        End Select

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

        logger.Debug("RunSearch Exit")
    End Sub

    Private Sub LoadSources()
        logger.Debug("LoadSources Entry")

        cmbField(4).DataSource = OrderSearchDAO.GetAllOrderExternalSource()
        cmbField(4).DisplayMember = "Description"
        cmbField(4).ValueMember = "ID"
        cmbField(4).SelectedIndex = -1

        logger.Debug("LoadSources Exit")
    End Sub

    Private Sub ReturnSelection()
        logger.Debug("ReturnSelection Entry")

        '-- Make sure one item was selected
        If ugrdSearchResults.Selected.Rows.Count = 1 Then
            glOrderHeaderID = CInt(ugrdSearchResults.Selected.Rows(0).Cells(0).Text)
            Me.Close()
        ElseIf ugrdSearchResults.Selected.Rows.Count > 1 Then
            MsgBox("Only one item from the list can be selected.", MsgBoxStyle.Information, Me.Text)
            logger.Info("ReturnSelection- Only one item from the list can be selected.")
        Else
            MsgBox("Please select an item from the list.", MsgBoxStyle.Information, Me.Text)
            logger.Info("ReturnSelection -An item from the list must be selected.")
        End If

        logger.Debug("ReturnSelection Exit")
    End Sub

    Private Sub ugrdSearchResults_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdSearchResults.DoubleClickRow
        logger.Debug("ugrdSearchResults_DoubleClickRow Entry")
        ReturnSelection()
        logger.Debug("ugrdSearchResults_DoubleClickRow Exit")
    End Sub

    Private Sub _txtField_0_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _txtField_0.TextChanged
        Dim boolCheckIfNumber As Boolean

        '--Make sure the PO Number textbox has no numeric value input
        boolCheckIfNumber = IsNumeric(_txtField_0.Text)

        If (boolCheckIfNumber = False) Then
            _txtField_0.Text = ""
        End If
    End Sub

    Private Sub chkRefusedPO_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRefusedPO.CheckedChanged
        If chkRefusedPO.Checked Then
            optOpen(2).Checked = True
        End If
    End Sub

End Class