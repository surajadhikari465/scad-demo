Option Strict Off
Option Explicit On
Imports log4net
Friend Class frmOrderItemQueueView
    Inherits System.Windows.Forms.Form

    Private mdt As DataTable
    Private mdv As DataView

    Private IsInitializing As Boolean
	Private m_lOrderHeader_ID As Integer
	Private m_iLastColumnSelected As Short
    Private m_bIsLastColumnAscending As Boolean

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Private Sub SetupDataTable()

        logger.Debug("SetupDataTable Entry")

        ' Create a data table
        mdt = New DataTable("OrderQueuView")
        'Visible on grid.
        '--------------------        
        mdt.Columns.Add(New DataColumn("Identifier", GetType(String)))
        mdt.Columns.Add(New DataColumn("Description", GetType(String)))
        mdt.Columns.Add(New DataColumn("Quantity", GetType(Double)))
        mdt.Columns.Add(New DataColumn("Unit", GetType(String)))
        mdt.Columns.Add(New DataColumn("Primary_Vendor", GetType(String)))
        mdt.Columns.Add(New DataColumn("User", GetType(String)))
        mdt.Columns.Add(New DataColumn("Insert_Date", GetType(Date)))

        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("Order_Item_Queu_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Item_Key", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Unit_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Discontinued", GetType(Boolean)))


        logger.Debug("SetupDataTable Exit")
    End Sub
    Private Sub LoadDataTable(ByVal sSearchSQL As String)

        logger.Debug("LoadDataTable Entry")
        Dim rsSearch As DAO.Recordset = Nothing
        Dim row As DataRow

        Try
            rsSearch = SQLOpenRecordSet(sSearchSQL, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            'Load the data set.
            mdt.Rows.Clear()

            While Not rsSearch.EOF
                row = mdt.NewRow

                row("Order_Item_Queu_ID") = rsSearch.Fields("OrderItemQueue_ID").Value
                row("Item_Key") = rsSearch.Fields("Item_Key").Value
                row("Identifier") = rsSearch.Fields("Identifier").Value
                row("Description") = rsSearch.Fields("Item_Description").Value
                row("Quantity") = rsSearch.Fields("Quantity").Value
                row("Unit") = rsSearch.Fields("QuantityUnitName").Value
                row("Primary_Vendor") = rsSearch.Fields("PrimaryVendor").Value
                row("User") = rsSearch.Fields("UserName").Value
                row("Insert_Date") = rsSearch.Fields("Insert_Date").Value
                row("Unit_ID") = rsSearch.Fields("Unit_ID").Value
                row("Discontinued") = rsSearch.Fields("Discontinue_Item").Value
                mdt.Rows.Add(row)
                rsSearch.MoveNext()
            End While

            If rsSearch.RecordCount > 0 Then

                mdt.AcceptChanges()
                mdv = New System.Data.DataView(mdt)

                ugrdOrderList.DataSource = mdv
                Me.ugrdOrderList.Rows(0).Band.Columns.Item(7).Hidden = True
                Me.ugrdOrderList.Rows(0).Band.Columns.Item(8).Hidden = True
                Me.ugrdOrderList.Rows(0).Band.Columns.Item(1).Width = 204

                'Set the first item to selected.
                ugrdOrderList.Rows(0).Selected = True
                ugrdOrderList.ActiveRow = ugrdOrderList.Rows(0)
            End If

        Finally
            If rsSearch IsNot Nothing Then
                rsSearch.Close()
                rsSearch = Nothing
            End If
        End Try

        logger.Debug("LoadDataTable Exit")
    End Sub

    Private Sub cmbPurchasing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPurchasing.KeyPress

        logger.Debug("cmbPurchasing_KeyPress Entry")
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbPurchasing.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        logger.Debug("cmbPurchasing_KeyPress Exit")
    End Sub

    Private Sub cmbPurchasing_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbPurchasing.SelectedIndexChanged

        logger.Debug("cmbPurchasing_SelectedIndexChanged Entry")
        If Me.IsInitializing = True Then Exit Sub

        Dim lPurchasingID As Integer

        ' Load the list of Transfer To subteams

        If (cmbPurchasing.SelectedIndex > -1) Then
            lPurchasingID = VB6.GetItemData(cmbPurchasing, cmbPurchasing.SelectedIndex)
            Call SetActive(cmbTransferToSubteam, True)
        Else
            lPurchasingID = 0
            cmbTransferToSubteam.Items.Clear()
        End If

        'Load the purchasing store's subteam list from the StoreSubteam table
        Call LoadVendStoreSubteam(cmbTransferToSubteam, lPurchasingID)

        Call SetCombo(cmbTransferToSubteam, GetSetting("IRMA", "OrderItemQueue", "TransferToSubTeam"))

        logger.Debug("cmbPurchasing_SelectedIndexChanged Exit")
    End Sub
	
	Private Sub cmbUser_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbUser.KeyPress

        logger.Debug("cmbUser_KeyPress Entry")
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = 8 Then cmbUser.SelectedIndex = -1
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If
        logger.Debug("cmbUser_KeyPress Exit")
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click

        logger.Debug("cmdDelete_Click Entry")
        Dim nTotalSelRows As Short
        Dim iLoop As Short
		Dim lOrderItemQueue_ID As Integer
		
		On Error GoTo me_err
		
        If (ugrdOrderList.Rows.Count = 0) Then
            MsgBox("Search for items.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
            cmdSearch.Focus()
            Exit Sub
        End If
		
		'-- Get the number of selected items
        nTotalSelRows = ugrdOrderList.Selected.Rows.Count
		If (nTotalSelRows = 0) Then
			MsgBox("Please highlight item(s) to delete from the Order Item Queue.", MsgBoxStyle.OKOnly + MsgBoxStyle.Information, Me.Text)
            logger.Debug("cmdDelete_Click Exit")
            Exit Sub
		End If
		
		If MsgBox("Delete these items from the Queue?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
            logger.Debug("cmdDelete_Click Exit")
            Exit Sub
		End If
		
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
		
        For iLoop = 0 To ugrdOrderList.Selected.Rows.Count - 1
            If ugrdOrderList.Selected.Rows(iLoop).Selected = True Then
                lOrderItemQueue_ID = CInt(ugrdOrderList.Selected.Rows(iLoop).Cells("Order_Item_Queu_ID").Value)
                If (lOrderItemQueue_ID > 0) Then
                    ' Delete this row from the OrderItemQueue table
                    frmOrderItemQueue.DeleteItem(lOrderItemQueue_ID)
                End If
            End If
        Next iLoop
		
		Call RefreshDataSource()
		
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        logger.Debug("cmdDelete_Click Exit")
		Exit Sub
		
me_err: 
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

        logger.Error("cmdDelete_Click " & "Deleting failed with error: " & Err.Number & " - " & Err.Description)

        MsgBox("Deleting failed with error: " & Err.Number & " - " & Err.Description, MsgBoxStyle.Critical, Me.Text)

        logger.Debug("cmdDelete_Click Exit")

		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Call Me.Close()
        logger.Debug("cmdExit_Click Exit")
		
	End Sub
	
	Private Sub cmdItemEdit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdItemEdit.Click

        logger.Debug("cmdItemEdit_Click Entry")
        Dim nTotalSelRows As Short
		Dim iLoop As Short
		Dim vBook As Object
		
		On Error GoTo me_err
		
        If (ugrdOrderList.Rows.Count = 0) Then
            MsgBox("Search for items.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
            logger.Info("cmdItemEdit_Click " & " Search for items.")
            cmdSearch.Focus()
            logger.Debug("cmdItemEdit_Click Exit")
            Exit Sub
        End If

        '-- Get the number of selected items
        nTotalSelRows = ugrdOrderList.Selected.Rows.Count
		If (nTotalSelRows = 0) Then
            MsgBox("Please highlight one item.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            logger.Info("cmdItemEdit_Click " & " Please highlight one item.")
            logger.Debug("cmdItemEdit_Click Exit")
			Exit Sub
		Else
			If (nTotalSelRows > 1) Then
				MsgBox("Please highlight only one item.", MsgBoxStyle.OKOnly + MsgBoxStyle.Information, Me.Text)
                logger.Info("cmdItemEdit_Click " & "Please highlight only one item.")
                logger.Debug("cmdItemEdit_Click Exit")
                Exit Sub
			End If
		End If
		
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
		
		Me.Enabled = False
		
        For iLoop = 0 To ugrdOrderList.Rows.Count
            If ugrdOrderList.Rows(iLoop).Selected = True Then
                glItemID = CInt(ugrdOrderList.Rows(iLoop).Cells("Item_Key").Value)
                Exit For
            End If
        Next iLoop
		
		frmItem.ShowDialog()
        frmItem.Dispose()
		
		Me.Enabled = True
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        logger.Debug("cmdItemEdit_Click Exit")
        Exit Sub
me_err: 
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        logger.Error("cmdItemEdit_Click " & " Viewing item failed with error: " & Err.Number & " - " & Err.Description)
        MsgBox("Viewing item failed with error: " & Err.Number & " - " & Err.Description, MsgBoxStyle.Critical, Me.Text)
        logger.Debug("cmdItemEdit_Click Exit")
    End Sub
	
    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        logger.Debug("cmdSearch_Click Entry")

        If cmbPurchasing.SelectedIndex = -1 Then
            MsgBox("You must select a Purchasing store.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, Me.Text)
            cmbPurchasing.Focus()
            Exit Sub
        End If

        '-- Make sure a transfer to subteam is selected, always
        If cmbTransferToSubteam.SelectedIndex = -1 Then
            MsgBox("Transfer To is required", MsgBoxStyle.Critical, Me.Text)
            cmbTransferToSubteam.Focus()
            Exit Sub
        End If

        Call RefreshDataSource()

        logger.Debug("cmdSearch_Click Exit")

    End Sub
	
	Private Sub cmdVendorClear_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdVendorClear.Click
        txtVendor.Text = ""
        glVendorID = 0
	End Sub
	
	Private Sub cmdVendorSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdVendorSearch.Click
        logger.Debug("cmdVendorSearch_Click Entry")

        '-- Set search criteria
		glVendorID = 0
		giSearchType = iSearchVendorCompany
		glOrderHeaderID = 0
		
		'-- Open the search form
		frmSearch.Text = "Vendor Search"
		frmSearch.ShowDialog()
        frmSearch.Dispose()
		
		' If they chose a vendor, then populate the vendor textbox
		If glVendorID > 0 Then
            txtVendor.Text = ReturnVendorName(glVendorID)
		Else
			' otherwise clear it
			txtVendor.Text = ""
		End If

        logger.Debug("cmdVendorSearch_Click Exit")
	End Sub
	
	Private Sub frmOrderItemQueueView_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        logger.Debug("frmOrderItemQueueView_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If cmdSearch.Enabled Then
			If KeyAscii = 13 Then Call cmdSearch_Click(cmdSearch, New System.EventArgs())
		End If
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If
        logger.Debug("frmOrderItemQueueView_KeyPress Exit")
	End Sub
	
	Private Sub frmOrderItemQueueView_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmOrderItemQueueView_Load Entry")

        Call CenterForm(Me)

        Call SetupDataTable()

		Call LoadRegionCustomer(cmbPurchasing)
		Call LoadUsers(cmbUser)
		
		'-- Limit them to one store
        Dim i As Short
        If (gbBuyer Or gbDistributor) And (Not gbCoordinator) And (Not IsDBNull(glVendor_Limit)) And (Not glVendor_Limit = 0) Then
            For i = 0 To cmbPurchasing.Items.Count - 1
                If VB6.GetItemData(cmbPurchasing, i) = glVendor_Limit Then
                    cmbPurchasing.SelectedIndex = i
                    Exit For
                End If
            Next i
            SetActive(cmbPurchasing, False)
        Else
            Call SetCombo(cmbPurchasing, GetSetting("IRMA", "OrderItemQueue", "Purchaser"))
        End If
		
		If (cmbPurchasing.SelectedIndex > -1) Then
			Call SetActive(cmbTransferToSubteam, True)
		Else
			Call SetActive(cmbTransferToSubteam, False)
		End If
		
        Call EnableControls(False)

        logger.Debug("frmOrderItemQueueView_Load Exit")
		
	End Sub
	
    Private Sub EnableControls(ByRef bValue As Boolean)

        logger.Debug("EnableControls Entry")
        Call SetActive(cmdDelete, bValue)
        Call SetActive(cmdItemEdit, bValue)
        logger.Debug("EnableControls Exit")

    End Sub
	
	Private Sub frmOrderItemQueueView_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        logger.Debug("frmOrderItemQueueView_FormClosed Entry")

        ' save these off for convenience
		If (cmbPurchasing.SelectedIndex > -1) Then
            SaveSetting("IRMA", "OrderItemQueue", "Purchaser", CStr(VB6.GetItemData(cmbPurchasing, cmbPurchasing.SelectedIndex)))
		End If
		If (cmbTransferToSubteam.SelectedIndex > -1) Then
            SaveSetting("IRMA", "OrderItemQueue", "TransferToSubTeam", CStr(VB6.GetItemData(cmbTransferToSubteam, cmbTransferToSubteam.SelectedIndex)))
        End If

        logger.Debug("frmOrderItemQueueView_FormClosed Exit")
		
	End Sub

    Private Sub RefreshDataSource()

        logger.Debug("RefreshDataSource Entry")
        Dim sSQLText As String
        Dim sItem_Description As String
        Dim sIdentifier As String
        Dim lVendor_ID As Integer
        Dim lUser_ID As Integer

        sItem_Description = ConvertQuotes(Trim(txtDescription.Text))
        sIdentifier = Trim(txtIdentifier.Text)

        lVendor_ID = IIf(Len(txtVendor.Text) > 0, glVendorID, 0)
        If (cmbUser.SelectedIndex > -1) Then lUser_ID = VB6.GetItemData(cmbUser, cmbUser.SelectedIndex)

        ' Search for the items
        sSQLText = "EXEC GetOrderItemQueueView " & VB6.GetItemData(cmbPurchasing, cmbPurchasing.SelectedIndex) & ", " & VB6.GetItemData(cmbTransferToSubteam, cmbTransferToSubteam.SelectedIndex) & ", '" & sItem_Description & "'" & ", '" & sIdentifier & "'" & ", " & IIf(optTransfer.Checked = True, 1, 0) & ", " & IIf(optCredit.Checked = True, 1, 0) & ", " & lVendor_ID & ", " & lUser_ID

        Call LoadDataTable(sSQLText)

        If ugrdOrderList.Rows.Count > 0 Then

            Call EnableControls(True)
            lblRowCount.Text = "Number of Items : " & CStr(ugrdOrderList.Rows.Count)
        Else
            lblRowCount.Text = "Number of Items : 0"
            Call EnableControls(False)
            MsgBox("No matching items found." & vbNewLine & "Try changing the search criteria.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
        End If

        logger.Debug("RefreshDataSource Exit")

    End Sub

    Private Sub cmbTransferToSubteam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbTransferToSubteam.KeyPress

        logger.Debug("cmbTransferToSubteam_KeyPress Entry")
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbTransferToSubteam.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        logger.Debug("cmbTransferToSubteam_KeyPress Exit")
    End Sub

    Private Sub cmdReports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReports.Click

        Dim frmOrderItemQueueReport As New frmOrderItemQueueReport()
        Dim iOrderType As Integer

        ' Store validation
        If cmbPurchasing.SelectedIndex = -1 Then
            MsgBox("Store must be selected.", MsgBoxStyle.Exclamation, "Error")
            Exit Sub
        End If

        ' SubTeam validation
        If cmbTransferToSubteam.SelectedIndex = -1 Then
            MsgBox("SubTeam must be selected.", MsgBoxStyle.Exclamation, "Error")
            Exit Sub
        End If

        ' Set Order Type
        Select Case True
            Case optOrder.Checked
                iOrderType = 0
            Case optTransfer.Checked
                iOrderType = 1
            Case optCredit.Checked
                iOrderType = 2
            Case Else
                iOrderType = 0
        End Select

        ' Populate input to SQL report
        frmOrderItemQueueReport.PurchasingVendor_ID = VB6.GetItemData(cmbPurchasing, cmbPurchasing.SelectedIndex)
        frmOrderItemQueueReport.TransferToSubTeam_No = VB6.GetItemData(cmbTransferToSubteam, cmbTransferToSubteam.SelectedIndex)
        frmOrderItemQueueReport.Item_Description = Trim(txtDescription.Text)
        frmOrderItemQueueReport.OrderType = iOrderType
        frmOrderItemQueueReport.Identifier = Trim(txtIdentifier.Text)
        If glVendorID > 0 Then
            frmOrderItemQueueReport.Vendor_ID = glVendorID
        End If
        If cmbUser.SelectedIndex > -1 Then
            frmOrderItemQueueReport.User_ID = VB6.GetItemData(cmbUser, cmbUser.SelectedIndex)
        End If

        frmOrderItemQueueReport.ShowDialog()
        frmOrderItemQueueReport.Close()

    End Sub
End Class