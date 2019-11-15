Option Strict Off
Option Explicit On

Imports log4net
Imports WholeFoods.IRMA.Ordering.BusinessLogic
Imports WholeFoods.IRMA.Ordering.BusinessLogic.FairShareAllocationBO
Imports WholeFoods.Utility.DataAccess

Friend Class frmOrdersAllocate
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean
    Private mbResetBOH As Boolean
    Private mlStoreIndex As Integer
    Private mlSubTeamIndex As Integer
    Private miPreOrderIndex As Short
    Private miBOHIndex As Short

	' Define the log4net logger for this class.
	Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private mdtAlloc As DataTable       ' tmpOrdersAllocateOrderItems
    Private mdtItem As DataTable        ' tmpOrdersAllocateItems
    Private mrsItems As DataTable
    Private miCurrentRec As Integer = -1
    Private mFactory As New DataFactory(DataFactory.ItemCatalog)
    Private miSubTeamIndex As Integer = -1

    Private bEnableWarhseSend As Boolean
    Private bEnableAutoAlloc As Boolean
    Private bEnableOrders As Boolean
    Private bEnableSearch As Boolean
    Private bEnableNext As Boolean
    Private bEnablePrevious As Boolean

    Private ProgressComplete As Boolean
    Private AnErrorHasOccurred As Boolean

    Private iSelectedStore As Integer
    Private iSelectedSubteam As Integer
    Private bDoCasePackMoves As Boolean

    Private CurrentSession As FairShareAllocationBO.AllocationSession

#Region " Private Properties"

    Public ReadOnly Property OrderSubteamTypeOption() As OrderSubteamType
        Get
            If Me.optAllOrders.Checked Then
                Return OrderSubteamType.All
            ElseIf Me.optNonRetail.Checked Then
                Return OrderSubteamType.NonRetail
            ElseIf Me.optRetail.Checked Then
                Return OrderSubteamType.Retail
            End If
        End Get
    End Property

    Public ReadOnly Property PreOrderOption() As FairShareAllocationBO.PreOrder
        Get
            If Me.optPreOrder(0).Checked Then
                Return FairShareAllocationBO.PreOrder.All
            ElseIf Me.optPreOrder(1).Checked Then
                Return FairShareAllocationBO.PreOrder.PreOrder
            ElseIf Me.optPreOrder(2).Checked Then
                Return FairShareAllocationBO.PreOrder.NonPreOrder
            End If
        End Get
    End Property

    Public ReadOnly Property BOHOption() As BOH
        Get
            If Me.optBOH(0).Checked Then
                Return PreOrder.All
            ElseIf Me.optBOH(1).Checked Then
                Return PreOrder.PreOrder
            ElseIf Me.optBOH(2).Checked Then
                Return PreOrder.NonPreOrder
            ElseIf Me.optBOH(3).Checked Then
                Return BOH.LessThanZero
            ElseIf Me.optBOH(4).Checked Then
                Return BOH.GreaterThanEqualZero
            End If
        End Get
    End Property

    Public ReadOnly Property IncludeExpectedDate() As Boolean
        Get
            Return Me.checkIncludeWOO.Checked
        End Get
    End Property

    Public ReadOnly Property ExpectedStartDate() As Date
        Get
            Return Me.dtWOOStart.Value
        End Get
    End Property

    Public ReadOnly Property ExpectedEndDate() As Date
        Get
            Return Me.dtWOOEnd.Value
        End Get
    End Property

    Public ReadOnly Property WarehouseNo() As Integer
        Get
            Return ComboVal(cmbStore)
        End Get
    End Property

    Public ReadOnly Property WarehouseName() As String
        Get
            Return cmbStore.Text
        End Get
    End Property

    Public ReadOnly Property SubteamNo() As Integer
        Get
            Return ComboVal(cmbSubTeam)
        End Get
    End Property

#End Region

    Private Sub frmOrdersAllocate_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmOrdersAllocate_Load Entry")

        ' load cmbStore combo box
        LoadDistMfg(cmbStore)
        If cmbStore.Items.Count > 0 Then
            cmbStore.SelectedIndex = 0
        End If

        mlStoreIndex = cmbStore.SelectedIndex

        Select Case True
            Case optPreOrder(0).Checked : miPreOrderIndex = 0
            Case optPreOrder(1).Checked : miPreOrderIndex = 1
            Case optPreOrder(2).Checked : miPreOrderIndex = 2
        End Select

        Select Case True
            Case optBOH(0).Checked : miBOHIndex = 0
            Case optBOH(1).Checked : miBOHIndex = 1
            Case optBOH(2).Checked : miBOHIndex = 2
        End Select

        ' set up System.Data.DataTables
        SetupAllocDataTable()
        SetupItemDataTable()

        Me.dtWOOStart.Value = Date.Today.ToShortDateString
        Me.dtWOOStart.Value = Date.Today.ToShortDateString

        Global.SetUltraGridSelectionStyle(ugrdAlloc)
        Global.SetUltraGridSelectionStyle(ugrdItem)
        logger.Debug("frmOrdersAllocate_Load Exit")

    End Sub

    Private Sub cmdAutoAllocate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAutoAllocate.Click

        Dim frmOrdersAutoAllocate As New OrdersAutoAllocate

        frmOrdersAutoAllocate.ShowDialog()

        Dim enumDialogResult As DialogResult = frmOrdersAutoAllocate.DialogResult

        bDoCasePackMoves = OrdersAutoAllocate.DoCasePackMoves()

        frmOrdersAutoAllocate.Close()
        frmOrdersAutoAllocate.Dispose()

        If enumDialogResult = Windows.Forms.DialogResult.OK Then

            Cursor = Cursors.WaitCursor
            DisableButtons()
            RefreshData(False, True, False)
            Cursor = Cursors.WaitCursor

            iSelectedStore = ComboVal(cmbStore)
            iSelectedSubteam = ComboVal(cmbSubTeam)

            Dim theAllocationThread As New Thread(AddressOf Me.DoFSAAutoAllocateThread)
            theAllocationThread.Start()

            Thread.Sleep(500)

            If Not Me.ProgressComplete And Not Me.AnErrorHasOccurred Then
                Dim theProgressDialog As ProgressDialog = ProgressDialog.OpenProgressDialog(Me, "Performing Auto-allocation.", _
                "This may take a while. Please Stand By...", "rows", ProgressBarStyle.Marquee, 1, 0)

                ' poll for the collection's progress
                While Not Me.ProgressComplete
                    Thread.Sleep(100)
                    If Not IsNothing(theProgressDialog) Then
                        theProgressDialog.UpdateProgressDialogValue(1)
                    End If
                End While

                If Not IsNothing(theProgressDialog) Then
                    RefreshData(False, True, False)
                    theProgressDialog.CloseProgressDialog()
                End If

            End If

            If Not Me.AnErrorHasOccurred Then
                MessageBox.Show("Auto-allocation complete.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            grpSubstitute.Enabled = True

            PopItemDisplay()

            EnableButtons(sender)
            Cursor = Cursors.Default

        End If

    End Sub

    Private Sub cmbStore_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbStore.KeyPress
        logger.Debug("cmbStore_KeyPress Entry")

        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then cmbStore.SelectedIndex = -1

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        logger.Debug("cmbStore_KeyPress Exit")
    End Sub

    Private Sub cmbStore_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStore.SelectedIndexChanged
        logger.Debug("cmbStore_SelectedIndexChanged Entry")

        If Me.IsInitializing = True Then Exit Sub

		If cmbStore.SelectedIndex = -1 Then
			cmbSubTeam.DataSource = Nothing
			cmbSubTeam.Items.Clear()
			Exit Sub
		End If

		Static bCancel As Boolean

        If bCancel Then Exit Sub

        If Not SaveData() Then
            bCancel = True
            cmbStore.SelectedIndex = mlStoreIndex
            bCancel = False
        Else
            ClearDisplay()
			mlStoreIndex = cmbStore.SelectedIndex

			' load cmbSubTeam combo box
			LoadSubTeamByType(enumSubTeamType.Supplier, cmbSubTeam, Nothing, VB6.GetItemData(cmbStore, cmbStore.SelectedIndex), isIncludeAllTeams:=chkSubteam.Checked)
			If cmbSubTeam.Items.Count = 1 Then
                cmbSubTeam.SelectedIndex = 0
            Else
                cmbSubTeam.SelectedIndex = -1
                cmdReport.Enabled = False
                cmdRefresh.Enabled = False
                cmdCloseOrderingWindow.Enabled = False
            End If

            mlSubTeamIndex = cmbSubTeam.SelectedIndex
        End If

        logger.Debug("cmbStore_SelectedIndexChanged Exit")
    End Sub

    Private Sub cmbSubTeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress
        logger.Debug("cmbSubTeam_KeyPress Entry")
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then cmbSubTeam.SelectedIndex = -1

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        logger.Debug("cmbSubTeam_KeyPress Exit")
    End Sub

    Private Sub cmbSubTeam_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbSubTeam.SelectedIndexChanged

        logger.Debug("cmbSubTeam_SelectedIndexChanged Entry")
        If Me.IsInitializing = True Then Exit Sub

        Static bCancel As Boolean

        If bCancel Then Exit Sub

        If miSubTeamIndex > -1 And Not CurrentSession Is Nothing Then DeleteTempRecords(ComboVal(cmbStore).ToString, VB6.GetItemData(cmbSubTeam, miSubTeamIndex).ToString, True, True, CurrentSession.PreOrderOption)

        miSubTeamIndex = cmbSubTeam.SelectedIndex

        If Not SaveData() Then
            bCancel = True
            cmbSubTeam.SelectedIndex = mlSubTeamIndex
            bCancel = False
        Else
            ClearDisplay()

            cmdReport.Enabled = True
            cmdRefresh.Enabled = True
            cmdCloseOrderingWindow.Enabled = True
        End If

        logger.Debug("cmbSubTeam_SelectedIndexChanged Exit")
    End Sub

    Private Sub cmdCloseOrderingWindow_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCloseOrderingWindow.Click
        logger.Debug("cmdCloseOrderingWindow_Click Entry")
        Cursor = Cursors.WaitCursor
        If Not CheckOrderWindows() Then
            ' #########################################################################################################################
            '  Bug. 6529 - RE
            '  Order windows are checked based on TIME only, not Date and time. The Close Order Window button closes the window by adjusting the DATE portion of the order window.
            '  Therefore this check will always tell you order windows are not closed even if you manually close the window.
            '
            ' Order Window logic on this screen does not work like it should.
            '
            '  Per Lawrence, The error message displayed as been changed and the window will close instead of allowing the users to manually close a window. 
            '
            ' #########################################################################################################################
            Exit Sub
        End If

        SQLExecute("AutoSendDistOrders", DAO.RecordsetOptionEnum.dbSQLPassThrough)
        Cursor = Cursors.Default
        MsgBox("Close Ordering Window completed", MsgBoxStyle.Information, Me.Text)

        logger.Debug("cmdCloseOrderingWindow_Click Exit")
    End Sub

    Private Sub cmdOrders_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOrders.Click
        logger.Debug("cmdOrders_Click Entry")

        Cursor = Cursors.WaitCursor

        Dim sCurrIdentifier As String
        Dim bOrderWindowDisplayed As Boolean

        If Not SaveData() Then Exit Sub
        '20090531 - Dave Stacey - Added OrderItem_ID off ultragrid to where clause
        Dim frmOrderList As New frmOrdersAllocateOrderList
        frmOrderList.WhereClause = "Where Store_no = " & ComboVal(cmbStore).ToString & _
                                        " and SubTeam_No = " & ComboVal(cmbSubTeam).ToString & _
                                        " and OrderItem_ID = " & Me.ugrdAlloc.ActiveRow.Cells("OrderItem_ID").Value.ToString

        frmOrderList.ShowDialog()
        bOrderWindowDisplayed = frmOrderList.mbOrderWindowDisplayed
        frmOrderList.Close()
        frmOrderList.Dispose()

        If bOrderWindowDisplayed Then
            sCurrIdentifier = mrsItems.Rows(miCurrentRec).Item("Identifier").ToString
            RefreshData(True, True, False)
            ItemSearch(sCurrIdentifier)
        End If

        Cursor = Cursors.Default

        logger.Debug("cmdOrders_Click Exit")
    End Sub

    Private Sub cmdNext_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNext.Click
        logger.Debug("cmdNext_Click Entry")
        If Not SaveData() Then Exit Sub
        Cursor = Cursors.WaitCursor
        If miCurrentRec < mrsItems.Rows.Count - 1 Then
            miCurrentRec += 1
        End If
        EnableNavigationCmds()
        EnableButtons(eventSender)
        PopItemDisplay()
        Cursor = Cursors.Default
        logger.Debug("cmdNext_Click Exit")
    End Sub

    Private Sub cmdPrevious_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdPrevious.Click
        logger.Debug("cmdPrevious_Click Entry")
        If Not SaveData() Then Exit Sub
        Cursor = Cursors.WaitCursor
        If miCurrentRec > 0 Then
            miCurrentRec -= 1
        End If
        EnableNavigationCmds()
        EnableButtons(eventSender)
        PopItemDisplay()
        Cursor = Cursors.Default
        logger.Debug("cmdPrevious_Click Exit")
    End Sub

    Private Sub cmdRefresh_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRefresh.Click
        logger.Debug("cmdRefresh_Click Entry")

        RefreshScreen(eventSender)

        logger.Debug("cmdRefresh_Click Exit")
    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        logger.Debug("cmdReport_Click Entry")

        If Not SaveData() Then Exit Sub

        Cursor = Cursors.WaitCursor

        Dim rptAllocationReport As New frmAllocationReport

        If optPreOrder(0).Checked Then
            rptAllocationReport.PreOrderOption = FairShareAllocationBO.PreOrder.All
        ElseIf optPreOrder(1).Checked Then
            rptAllocationReport.PreOrderOption = FairShareAllocationBO.PreOrder.PreOrder
        ElseIf optPreOrder(2).Checked Then
            rptAllocationReport.PreOrderOption = FairShareAllocationBO.PreOrder.NonPreOrder
        End If

        If optBOH(0).Checked Then
            rptAllocationReport.BOHOption = FairShareAllocationBO.BOH.All
        ElseIf optBOH(1).Checked Then
            rptAllocationReport.BOHOption = FairShareAllocationBO.BOH.GreaterThanZero
        ElseIf optBOH(2).Checked Then
            rptAllocationReport.BOHOption = FairShareAllocationBO.BOH.LessThanEqualZero
        ElseIf optBOH(3).Checked Then
            rptAllocationReport.BOHOption = FairShareAllocationBO.BOH.LessThanZero
        ElseIf optBOH(4).Checked Then
            rptAllocationReport.BOHOption = FairShareAllocationBO.BOH.GreaterThanEqualZero
        End If

        If optAllOrders.Checked Then
            rptAllocationReport.OrderSubteamTypeOption = FairShareAllocationBO.OrderSubteamType.All
        ElseIf optNonRetail.Checked Then
            rptAllocationReport.OrderSubteamTypeOption = FairShareAllocationBO.OrderSubteamType.NonRetail
        ElseIf optRetail.Checked Then
            rptAllocationReport.OrderSubteamTypeOption = FairShareAllocationBO.OrderSubteamType.Retail
        End If

        rptAllocationReport.IncludeExpectedDate = Me.checkIncludeWOO.Checked
        rptAllocationReport.ExpectedStartDate = Me.dtWOOStart.Value
        rptAllocationReport.ExpectedEndDate = Me.dtWOOEnd.Value

        rptAllocationReport.StoreNo = VB6.GetItemData(cmbStore, cmbStore.SelectedIndex)
        rptAllocationReport.StoreName = cmbStore.Text
        rptAllocationReport.SubteamNo = VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex)

        Cursor = Cursors.Default

        rptAllocationReport.ShowDialog()

        frmAllocationReport.Close()
        frmAllocationReport.Dispose()

        logger.Debug("cmdReport_Click Exit")

    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        logger.Debug("cmdSearch_Click Entry")
        If Not SaveData() Then Exit Sub

        Dim sInput As String
        sInput = InputBox("Enter Identifier", Me.Text & " - Item Search", "")

        If Len(sInput) > 0 Then ItemSearch(sInput)
        logger.Debug("cmdSearch_Click Exit")
    End Sub

    Private Sub cmdWarehouseSend_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdWarehouseSend.Click
        logger.Debug("cmdWarehouseSend_Click Entry")
        'Save local changes to IRMA
        If Not SaveData() Then Exit Sub

        Cursor = Cursors.WaitCursor

        If CurrentSession IsNot Nothing Then
            If MsgBox("Send ALL currently selected orders to the warehouse now?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                FairShareAllocationDAO.DoFSAWarehouseSend(CurrentSession)
            End If
        Else
            MessageBox.Show("You must establish an FSA session before you can send orders to the warehouse.", "No FSA Session Exists", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        RefreshScreen(eventSender)

        logger.Debug("cmdWarehouseSend_Click Exit")

        Cursor = Cursors.Default

    End Sub

    Private Sub optBOH_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optBOH.CheckedChanged
        logger.Debug("optBOH_CheckedChanged Entry")

        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then
            Dim Index As Short = optBOH.GetIndex(eventSender)

            Static bCancel As Boolean

            If mbResetBOH Then Exit Sub

            If bCancel Then Exit Sub

            If Not SaveData() Then
                bCancel = True
                optBOH(miBOHIndex).Checked = True
                bCancel = False
            Else
                miBOHIndex = Index
                RefreshItemSelection(False, False)
                EnableNavigationCmds()
                EnableButtons(Nothing)
            End If

        End If
        logger.Debug("optBOH_CheckedChanged Exit")
    End Sub

    Private Sub optPreOrder_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optPreOrder.CheckedChanged

        logger.Debug("optPreOrder_CheckedChanged Entry")
        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then
            Dim Index As Short = optPreOrder.GetIndex(eventSender)

            Static bCancel As Boolean

            If bCancel Then Exit Sub

            If Not SaveData() Then
                bCancel = True
                optPreOrder(miPreOrderIndex).Checked = True
                bCancel = False
            Else
                miPreOrderIndex = Index
                mbResetBOH = True
                optBOH(miBOHIndex).Checked = False
                optBOH(0).Checked = True
                mbResetBOH = False
                RefreshItemSelection(True, False)
                EnableNavigationCmds()
                EnableButtons(Nothing)
            End If

        End If
        logger.Debug("optPreOrder_CheckedChanged Exit")
    End Sub

    Private Sub optNonRetail_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optNonRetail.CheckedChanged
        logger.Debug("optNonRetail_CheckedChanged Entry")
        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then
            Static bCancel As Boolean

            If bCancel Then Exit Sub

            If Not SaveData() Then
                bCancel = True
                optNonRetail.Checked = Not optNonRetail.Checked
                bCancel = False
            Else
                ClearDisplay()
                If Not Me.CurrentSession Is Nothing Then
                    Me.PopItemDisplay()
                End If
            End If

        End If
        logger.Debug("optNonRetail_CheckedChanged Exit")
    End Sub

    Private Sub optRetail_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optRetail.CheckedChanged
        logger.Debug("optRetail_CheckedChanged Entry")
        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then
            Static bCancel As Boolean

            If bCancel Then Exit Sub

            If Not SaveData() Then
                bCancel = True
                optRetail.Checked = Not optRetail.Checked
                bCancel = False
            Else
                ClearDisplay()
                If Not Me.CurrentSession Is Nothing Then
                    Me.PopItemDisplay()
                End If
            End If

        End If
        logger.Debug("optRetail_CheckedChanged Exit")
    End Sub

    Private Sub ugrdAlloc_AfterCellActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdAlloc.AfterCellActivate
        logger.Debug("ugrdAlloc_AfterCellActivate Entry")

        If ugrdAlloc.ActiveCell.Column.CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit Then
            ugrdAlloc.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
        End If

        logger.Debug("ugrdAlloc_AfterCellActivate Exit")
    End Sub

    Private Sub ugrdAlloc_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles ugrdAlloc.AfterRowUpdate
        logger.Debug("ugrdAlloc_AfterRowUpdate Entry")

        If Not (mdtAlloc Is Nothing) Then
            ugrdAlloc.UpdateData()

            Dim cdt As DataTable = mdtAlloc.GetChanges(DataRowState.Modified)
            If cdt IsNot Nothing Then
                Dim sSQL As String
                Dim row As DataRow
                For Each row In cdt.Rows

                    If Not IsDBNull(row("QuantityAllocated")) Then
                        If IsDBNull(row("Package_Desc1")) Then
                            sSQL = "UPDATE tmpOrdersAllocateOrderItems SET QuantityAllocated = " & _
                                    row("QuantityAllocated") & " WHERE OrderItem_ID = " & row("OrderItem_ID")
                            mFactory.ExecuteNonQuery(sSQL)
                        Else
                            sSQL = "UPDATE tmpOrdersAllocateOrderItems SET QuantityAllocated = " & _
                                            row("QuantityAllocated") & ", Package_Desc1 = " & row("Package_Desc1") & _
                                            " WHERE OrderItem_ID = " & row("OrderItem_ID")
                            mFactory.ExecuteNonQuery(sSQL)
                        End If
                    Else
                        sSQL = "UPDATE tmpOrdersAllocateOrderItems SET QuantityAllocated = NULL WHERE OrderItem_ID = " & _
                                row("OrderItem_ID")
                        mFactory.ExecuteNonQuery(sSQL)
                    End If

                Next
                cdt.AcceptChanges()
                cdt.Dispose()

                PopItemGrid()
            End If
        End If

        logger.Debug("ugrdAlloc_AfterRowUpdate Exit")
    End Sub

    Private Sub ugrdAlloc_BeforeCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs) Handles ugrdAlloc.BeforeCellUpdate
        logger.Debug("ugrdAlloc_BeforeCellUpdate Entry")

        If e.Cell.Column.Key = "QuantityAllocated" And Not IsDBNull(e.NewValue) Then
            Try
                'Get the value in the local database to determine the change
                Dim sSQL As String
                sSQL = "SELECT BOH, WOO, (SELECT ISNULL(SUM(QuantityAllocated), 0) " & _
                        "FROM tmpOrdersAllocateOrderItems WHERE OrderItem_ID <> " & e.Cell.Row.Cells("OrderItem_ID").Value & _
                        " AND Item_Key = tmpOrdersAllocateItems.Item_Key AND Package_Desc1 = tmpOrdersAllocateItems.PackSize) As Alloc " & _
                        "FROM tmpOrdersAllocateItems WHERE Item_Key = " & mrsItems.Rows(miCurrentRec).Item("Item_Key").ToString & _
                        " AND PackSize = " & e.Cell.Row.Cells("Package_Desc1").Value

                ' the old code assumes always only one record will return from this query, so I will assume the same
                Dim dt As DataTable = mFactory.GetDataTable(sSQL)

                Dim isValNull As Boolean = IsDBNull(e.Cell.Value)
                Dim cellVal As Decimal

                If isValNull Then
                    cellVal = 0
                Else
                    cellVal = CDec(e.Cell.Value)
                End If

                If CDec(e.NewValue) >= cellVal Then
                    If dt.Rows(0).Item("Alloc") + cellVal > dt.Rows(0).Item("BOH") + dt.Rows(0).Item("WOO") Then
                        e.Cancel = True
                        MsgBox("Allocation total for this item exceeds BOH + WOO for the selected Pack Size", MsgBoxStyle.Critical, Me.Text)
                    End If
                End If

            Finally
            End Try
        End If

        logger.Debug("ugrdAlloc_BeforeCellUpdate Exit")
    End Sub

    Private Sub ugrdAlloc_BeforeRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CancelableRowEventArgs) Handles ugrdAlloc.BeforeRowUpdate
        logger.Debug("ugrdAlloc_BeforeRowUpdate Entry")

        If Not IsDBNull(e.Row.Cells("QuantityAllocated").Value) Then
            If e.Row.Cells("QuantityAllocated").Value > 0 And IsDBNull(e.Row.Cells("Package_Desc1").Value) Then
                MsgBox("Pack Size selection is required", MsgBoxStyle.Critical, Me.Text)
                e.Cancel = True
            End If
        End If

        logger.Debug("ugrdAlloc_BeforeRowUpdate Exit")
    End Sub

    Private Sub ugrdAlloc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ugrdAlloc.KeyDown
        logger.Debug("ugrdAlloc_KeyDown Entry")

        Select Case e.KeyValue
            Case Keys.Up
                ugrdAlloc.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                ugrdAlloc.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.AboveCell, False, False)
                e.Handled = True
                ugrdAlloc.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Down
                ugrdAlloc.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                ugrdAlloc.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.BelowCell, False, False)
                e.Handled = True
                ugrdAlloc.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Right
                ugrdAlloc.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                ugrdAlloc.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.NextCellByTab, False, False)
                e.Handled = True
                ugrdAlloc.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Left
                ugrdAlloc.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                ugrdAlloc.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.PrevCellByTab, False, False)
                e.Handled = True
                ugrdAlloc.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
        End Select

        logger.Debug("ugrdAlloc_KeyDown Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
    End Sub

    Private Sub frmOrdersAllocate_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("frmOrdersAllocate_FormClosing Entry")
        If Not SaveData() Then
            e.Cancel = True
            Exit Sub
        End If
        If CurrentSession IsNot Nothing Then
            DeleteTempRecords(ComboVal(cmbStore).ToString, ComboVal(cmbSubTeam).ToString, True, True, CurrentSession.PreOrderOption)
        End If
        logger.Debug("frmOrdersAllocate_FormClosing Exit")
    End Sub

    Private Sub cmdSubItemSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubItemSearch.Click

        Dim itemValues() As String
        Dim item As Object
        Dim sKey As String
        Dim sDesc As String
        Dim iCount As Integer

        logger.Debug("cmdSubItemSearch_Click Entry")

        cmdSubItemSearch.Enabled = False

        Cursor = Cursors.WaitCursor

        frmErrorProvider.SetError(cmdSubItemSearch, "")

        If txtSubstituteIdentifier.Text.Trim.Length = 0 Then

            SetValidationMessage("Please enter an identifier to search for.")

        ElseIf Me.txtSubstituteIdentifier.Text.Equals(lblIdentifier.Text.Trim) Then

            ' the key that came back cannot = the key of the item we're trying to substitute for
            SetValidationMessage("That identifier is the same one you are trying to substitute for. Enter a different identifier.")

        Else

            item = mFactory.ExecuteScalar("SELECT dbo.fn_GetItemDescription(" & txtSubstituteIdentifier.Text.Trim & ")")

            If IsDBNull(item) Then

                ' if the return is null, the identifier entered was not found
                SetValidationMessage("Item not found.")

            Else

                ' something came back - split it into the key and the description
                itemValues = item.ToString.Split("|")

                sKey = itemValues.GetValue(0).ToString
                sDesc = itemValues.GetValue(1).ToString

                ' Get count of orders this item is on
                iCount = mFactory.ExecuteScalar("SELECT COUNT(*) FROM tmpOrdersAllocateOrderItems WHERE Item_Key = " & sKey)

                If iCount = 0 Then
                    ' if the item does not appear on any orders, it is invalid as a substitution item
                    SetValidationMessage("That item does not appear on any orders included in this FSA session.")
                Else

                    ' show the description of the item to the user
                    SetSubstitutionCommands(True)
                    Me.lblSubIdentifierDesc.Text = sDesc

                End If

            End If
        End If

        Cursor = Cursors.Default

        cmdSubItemSearch.Enabled = True

        logger.Debug("cmdSubItemSearch_Click Exit")

    End Sub

    Private Sub checkIncludeWOO_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkIncludeWOO.CheckedChanged
        Me.grpPOExpectedDate.Enabled = Me.checkIncludeWOO.Checked
    End Sub

    Private Sub cmdSubstitute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubstitute.Click

        logger.Debug("cmdSubstitute_Click Entry")

        Cursor = Cursors.WaitCursor

        Dim sOrigItem As String = lblIdentifier.Text.Trim

        Dim msg As String = String.Empty
        msg &= "Substitute " & lblSubIdentifierDesc.Text.Trim & " for " & lblItemDesc.Text.Trim
        msg &= vbNewLine
        msg &= vbNewLine
        msg &= "Perform substitution?"

        If MessageBox.Show(msg, "Confirm Substitution Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Try
                FairShareAllocationDAO.DoFSASubstitution(lblIdentifier.Text.Trim, txtSubstituteIdentifier.Text.Trim, optNonRetail.Checked)
                MessageBox.Show("Substitution Complete.", "Substitution Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Unable to perform substitution: " & ex.InnerException.Message.ToString, "Substitution Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                RefreshData(False, True, False)
                ItemSearch(sOrigItem)
            End Try

        End If

        Cursor = Cursors.Default

        logger.Debug("cmdSubstitute_Click Exit")

    End Sub

    Private Sub cmdShowNotAvailable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdShowNotAvailable.Click

        logger.Debug("cmdShowNotAvailable_Click Entry")

        If Me.txtSubstituteIdentifier.Text.Length = 0 Then
            SetValidationMessage("Please enter an identifier to search for.")
        Else

            Dim frmOrdersAllocateOrderSubstitute As New OrdersAllocateOrderSubstitute

            frmOrdersAllocateOrderSubstitute.Identifier = lblIdentifier.Text.Trim
            frmOrdersAllocateOrderSubstitute.SubIdentifier = txtSubstituteIdentifier.Text.Trim
            frmOrdersAllocateOrderSubstitute.NonRetail = optNonRetail.Checked
            frmOrdersAllocateOrderSubstitute.SubTeam = cmbSubTeam.Text.Trim
            frmOrdersAllocateOrderSubstitute.ShowDialog()
            frmOrdersAllocateOrderSubstitute.Dispose()

        End If

        logger.Debug("cmdShowNotAvailable_Click Exit")

    End Sub

    Private Sub SetupAllocDataTable()

        logger.Debug("SetupAllocDataTable Entry")

        mdtAlloc = New DataTable("OrdersAllocateOrderItems")
        Dim Keys(0) As DataColumn
        Dim dc As DataColumn

        mdtAlloc.PrimaryKey = Keys

        dc = New DataColumn("OrderItem_ID", GetType(Integer))
        mdtAlloc.Columns.Add(dc)
        Keys(0) = dc

        mdtAlloc.Columns.Add(New DataColumn("Item_Key", GetType(Integer)))
        mdtAlloc.Columns.Add(New DataColumn("CompanyName", GetType(String)))
        mdtAlloc.Columns.Add(New DataColumn("OrderHeader_ID", GetType(Integer)))
        mdtAlloc.Columns.Add(New DataColumn("SubTeam_Name", GetType(String)))
        mdtAlloc.Columns.Add(New DataColumn("QuantityOrdered", GetType(Decimal)))
        mdtAlloc.Columns.Add(New DataColumn("Package_Desc1", GetType(Decimal)))
        mdtAlloc.Columns.Add(New DataColumn("QuantityAllocated", GetType(Decimal)))

        logger.Debug("SetupAllocDataTable Exit")
    End Sub

    Private Sub SetupItemDataTable()
        logger.Debug("SetupItemDataTable Entry")

        mdtItem = New DataTable("OrdersAllocateItems")
        mdtItem.Columns.Add(New DataColumn("PackSize", GetType(Decimal)))
        mdtItem.Columns.Add(New DataColumn("BOH", GetType(Decimal)))
        mdtItem.Columns.Add(New DataColumn("WOO", GetType(Decimal)))
        mdtItem.Columns.Add(New DataColumn("SOO", GetType(Decimal)))
        mdtItem.Columns.Add(New DataColumn("Alloc", GetType(Decimal)))
        mdtItem.Columns.Add(New DataColumn("EOH", GetType(Decimal)))

        logger.Debug("SetupItemDataTable Exit")
    End Sub

    Private Sub ItemSearch(ByRef sIdentifier As String)
        logger.Debug("ItemSearch Entry")

        'Dim view As DataView = mrsItems.DefaultView
        'view.Sort = "Identifier ASC"
        'Dim i As Integer = view.Find(sIdentifier)
        Dim i As Integer = 0
        Dim bRecFound As Boolean = False

        For i = 0 To mrsItems.Rows.Count - 1
            If sIdentifier = mrsItems.Rows(i).Item("Identifier").ToString Then
                miCurrentRec = i
                bRecFound = True
                Exit For
            End If
        Next

        If bRecFound Then
            'miCurrentRec = i
            EnableNavigationCmds()
            EnableButtons(Nothing)
            PopItemDisplay()
        Else
            MsgBox("Identifier '" & sIdentifier & "' not found", MsgBoxStyle.Exclamation, Me.Text)
        End If

        logger.Debug("ItemSearch Exit")
    End Sub

    Private Sub ClearDisplay()

        logger.Debug("ClearDisplay Entry")

        lblIdentifier.Text = ""
        lblItemDesc.Text = ""

        SetValidationMessage("")
        SetSubstitutionCommands(False)

        If Not (mdtItem Is Nothing) Then mdtItem.Clear()
        If Not (mdtAlloc Is Nothing) Then mdtAlloc.Clear()

        logger.Debug("ClearDisplay Exit")

    End Sub

    Private Sub EnableNavigationCmds()
        logger.Debug("EnableNavigationCmds Entry")

        If mrsItems.Rows.Count < 2 Then
            bEnablePrevious = False
            bEnableNext = False
            bEnableSearch = False
        Else
            bEnableSearch = True
            If miCurrentRec = 0 Then ' first row
                bEnablePrevious = False
                bEnableNext = True
            ElseIf miCurrentRec = (mrsItems.Rows.Count - 1) Then 'last row
                bEnablePrevious = True
                bEnableNext = False
            Else
                bEnablePrevious = True
                bEnableNext = True
            End If
        End If

        logger.Debug("EnableNavigationCmds Exit")
    End Sub

    Private Sub PopItemGrid()

        logger.Debug("PopItemGrid Entry")

        If mrsItems.Rows.Count > 0 Then
            mdtItem = FairShareAllocationDAO.GetOrderAllocateItemsQty(mrsItems.Rows(miCurrentRec).Item("Item_Key"))
            ugrdItem.DataSource = mdtItem
            If ugrdItem.Rows.Count > 1 Then
                ugrdItem.Rows(0).CellAppearance.BackColor = Color.MistyRose
            End If
        End If

        logger.Debug("PopItemGrid Exit")

    End Sub

    Private Sub PopItemDisplay()

        logger.Debug("PopItemDisplay Entry")

        Dim dt As DataTable
        Dim x As Integer

        If mrsItems.Rows.Count > 0 Then

            PopItemGrid()

            dt = FairShareAllocationDAO.GetAllocationItemPackSizes(mrsItems.Rows(miCurrentRec).Item("Item_Key"))

            ugrdAlloc.DisplayLayout.ValueLists("PackSize").ValueListItems.Clear()

            For x = 0 To dt.Rows.Count - 1
                ugrdAlloc.DisplayLayout.ValueLists("PackSize").ValueListItems.Add( _
                                            dt.Rows(x).Item("PackSize"), Format(dt.Rows(x).Item("PackSize"), "#####.##"))
            Next

            mdtAlloc = FairShareAllocationDAO.GetAllocationItems(mrsItems.Rows(miCurrentRec).Item("Item_Key"))
            ugrdAlloc.DataSource = mdtAlloc

            If mdtAlloc.Rows.Count > 0 Then
                lblIdentifier.Text = mdtAlloc.Rows(0).Item("Identifier")
                lblItemDesc.Text = mdtAlloc.Rows(0).Item("Item_Description")
            End If

        End If

        logger.Debug("PopItemDisplay Exit")

    End Sub

    Private Sub RefreshData(ByVal RefreshBOH As Boolean, ByVal RefreshItems As Boolean, ByVal AdjustBOH As Boolean)
        logger.Debug("RefreshData Entry")

        If Not SaveData() Then Exit Sub

        Cursor = Cursors.WaitCursor

        ClearDisplay()

        If Not CheckOrderWindows() Then
            ' order windows are not closed. This screen cannot be used.
            Exit Sub
        End If

        If WarehouseSubTeamInUse() Then
            Exit Sub
        End If

        If CurrentSession IsNot Nothing Then
            DeleteTempRecords(ComboVal(cmbStore).ToString, ComboVal(cmbSubTeam).ToString, RefreshBOH, RefreshItems, CurrentSession.PreOrderOption)
        End If

        If RefreshBOH Then
            If Me.checkIncludeWOO.Checked Then
                FairShareAllocationDAO.RefreshBOH(ComboVal(cmbStore), ComboVal(cmbSubTeam), Me.OrderSubteamTypeOption, AdjustBOH, Me.checkIncludeWOO.Checked, Me.PreOrderOption, Me.dtWOOStart.Value.ToShortDateString, Me.dtWOOEnd.Value.ToShortDateString)
            Else
                FairShareAllocationDAO.RefreshBOH(ComboVal(cmbStore), ComboVal(cmbSubTeam), Me.OrderSubteamTypeOption, AdjustBOH, Me.checkIncludeWOO.Checked, Me.PreOrderOption)
            End If
        End If

        If FairShareAllocationDAO.CountAllocationItems > 0 Then
            FairShareAllocationDAO.GetOrderAllocationOrderItems(ComboVal(cmbStore), ComboVal(cmbSubTeam), Me.OrderSubteamTypeOption)
            FairShareAllocationDAO.UpdateAllocationItemPackSize()

            Me.bEnableOrders = True
            Me.bEnableAutoAlloc = True
        Else
            Me.bEnableOrders = False
            Me.bEnableAutoAlloc = False
        End If

        mbResetBOH = True
        optBOH(miBOHIndex).Checked = False
        optBOH(0).Checked = True
        mbResetBOH = False

        RefreshItemSelection(True, True)

        Cursor = Cursors.Default

        logger.Debug("RefreshData Exit")
    End Sub

    Private Sub RefreshItemSelection(ByRef bEnableWarehouseSend As Boolean, ByVal bShowWarning As Boolean)
        logger.Debug("RefreshItemSelection Entry")
        Dim iPreOrder As Integer = -1
        Dim iGroupBy As Integer = -1

        Cursor = Cursors.WaitCursor

        ClearDisplay()

        If Me.optPreOrder(0).Checked Then
            iPreOrder = -1
        ElseIf Me.optPreOrder(1).Checked Then
            iPreOrder = 1
        Else
            iPreOrder = 0
        End If

        If Me.optBOH(0).Checked Then
            iGroupBy = 1
        ElseIf Me.optBOH(1).Checked Then
            iGroupBy = 2
        ElseIf Me.optBOH(2).Checked Then
            iGroupBy = 3
        ElseIf Me.optBOH(3).Checked Then
            iGroupBy = 4
        ElseIf Me.optBOH(3).Checked Then
            iGroupBy = 5
        End If

        If CurrentSession Is Nothing Then
            CreateSession()
        End If

        mrsItems = FairShareAllocationDAO.GetOrderAllocationItems(ComboVal(cmbStore), ComboVal(cmbSubTeam), iPreOrder, iGroupBy, Me.checkMultiPackOnly.Checked)

        If mrsItems.Rows.Count = 0 Then
            If bShowWarning Then MsgBox("No Orders meet the criteria entered.", MsgBoxStyle.Information, Me.Text)
        Else
            If bEnableWarehouseSend Then
                Me.bEnableWarhseSend = True
                Me.bEnableOrders = True
                Me.grpSubstitute.Enabled = True
            End If

            miCurrentRec = -1
            cmdNext_Click(cmdSearch, New System.EventArgs())
        End If

        Cursor = Cursors.Default

        logger.Debug("RefreshItemSelection Exit")
    End Sub

    Private Sub SetValidationMessage(ByVal Msg As String)

        logger.Debug("SetValidationMessage Entry")

        ' if there is a messgage, set the error provider and play the beep
        If Msg.Length > 0 Then
            My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
            frmErrorProvider.SetError(cmdSubItemSearch, Msg)
            txtSubstituteIdentifier.Focus()
        Else
            ' otherwise clear the error
            frmErrorProvider.SetError(cmdSubItemSearch, "")

        End If

        SetSubstitutionCommands(False)

        logger.Debug("SetValidationMessage Exit")

    End Sub

    Private Sub SetSubstitutionCommands(ByVal Enable As Boolean)

        logger.Debug("SetSubstitutionCommands Entry")

        cmdSubstitute.Enabled = Enable
        cmdShowNotAvailable.Enabled = Enable

        If Not Enable Then
            Me.txtSubstituteIdentifier.Text = String.Empty
            Me.lblSubIdentifierDesc.Text = String.Empty
        End If

        logger.Debug("SetSubstitutionCommands Exit")

    End Sub

    Private Sub DisableButtons()

        cmdAutoAllocate.Enabled = False
        cmdOrders.Enabled = False
        cmdSearch.Enabled = False
        cmdPrevious.Enabled = False
        cmdNext.Enabled = False
        cmdWarehouseSend.Enabled = False

    End Sub

    Private Sub EnableButtons(ByVal sender As System.Object)

        If sender Is cmdSearch Then Exit Sub

        cmdAutoAllocate.Enabled = bEnableAutoAlloc
        cmdOrders.Enabled = bEnableOrders
        cmdSearch.Enabled = bEnableSearch
        cmdPrevious.Enabled = bEnablePrevious
        cmdNext.Enabled = bEnableNext
        cmdWarehouseSend.Enabled = bEnableWarhseSend

    End Sub

    Private Sub DoFSAAutoAllocateThread()

        Try
            Dim _result As Boolean = FairShareAllocationDAO.DoFSAAutoAllocate(iSelectedStore, iSelectedSubteam, gsUserName, CurrentSession.PreOrderOption, bDoCasePackMoves)
            FairShareAllocationDAO.UpdateFSAStoreOnOrder(iSelectedStore, iSelectedSubteam, gsUserName, CurrentSession.PreOrderOption)
            Me.ProgressComplete = _result
        Catch ex As Exception
            Me.AnErrorHasOccurred = True
            MessageBox.Show("An error occurred during the auto-allocation process. " + _
            Environment.NewLine + Environment.NewLine + ex.Message, "Auto-allocation Error", _
            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Function SaveData() As Boolean
        logger.Debug("SaveData Entry")

        SetSubstitutionCommands(False)

        Try
            If CurrentSession IsNot Nothing Then
                FairShareAllocationDAO.UpdateFSAStoreOnOrder(ComboVal(cmbStore), ComboVal(cmbSubTeam), gsUserName, CurrentSession.PreOrderOption)
            End If
            Return True
        Catch ex As Exception
            MessageBox.Show("An error occurred saving the panel data." & Environment.NewLine + Environment.NewLine & ex.Message, "Save Error", MessageBoxButtons.OK)
            logger.Debug(ex.Message & Environment.NewLine & ex.InnerException.Message & Environment.NewLine & ex.StackTrace)
        End Try

        logger.Debug("SaveData Exit")

    End Function

    Private Function WarehouseSubTeamInUse() As Boolean
        Dim sReturnMsg As String = ""

        sReturnMsg = FairShareAllocationDAO.GetSubTeamUserName(ComboVal(cmbStore).ToString, ComboVal(cmbSubTeam).ToString, PreOrderOption, gsUserName)

        If sReturnMsg.Length > 0 Then
            MessageBox.Show(sReturnMsg, "Session Collision", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return True
        Else
            Return False
        End If

    End Function

    Private Function DeleteTempRecords(ByVal WarehouseNo As Integer, ByVal SubTeamNo As Integer, ByVal DeleteBOH As Boolean, ByVal DeleteItems As Boolean, ByVal PreOrderOption As PreOrder) As Boolean
        Try
            If CurrentSession IsNot Nothing Then
                FairShareAllocationDAO.DeleteTempFSARecords(WarehouseNo, SubTeamNo, gsUserName, DeleteItems, DeleteBOH, CurrentSession.PreOrderOption)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Function CheckOrderWindows() As Boolean

        Dim RecCount As Integer
        Dim retval As Boolean = False

        RecCount = mFactory.ExecuteScalar("EXEC GetDistOrderWindowsClosed " & ComboVal(cmbStore) & "," & ComboVal(cmbSubTeam) _
                       & "," & IIf(Me.optNonRetail.Checked, 1, 0))
        If RecCount > 0 Then
        
            MsgBox("Ordering window is not closed. You cannot close the order window at this time. Please try again during the order window downtime.", MsgBoxStyle.Critical, Me.Text)

            retval = False
        Else
            retval = True
        End If
        Return retval
    End Function

    Private Sub CreateSession()

        CurrentSession = New FairShareAllocationBO.AllocationSession(gsUserName, Me.WarehouseNo, Me.OrderSubteamTypeOption, Me.PreOrderOption, Me.SubteamNo)

    End Sub

    Private Sub RefreshScreen(ByVal sender As System.Object)
        Cursor = Cursors.WaitCursor

        DisableButtons()

        If CurrentSession Is Nothing Then
            CreateSession()
            RefreshData(True, True, True)
        Else
            RefreshData(True, True, False)
            CreateSession()
        End If

        EnableButtons(sender)

        Cursor = Cursors.Default
    End Sub

	Private Sub chkSubteam_Click(sender As Object, e As EventArgs) Handles chkSubteam.Click
		RefreshSubteamCombo(cmbSubTeam, Nothing, chkSubteam.Checked)
	End Sub
End Class