Option Strict Off
Option Explicit On
Friend Class frmInventoryAdjustment
	Inherits System.Windows.Forms.Form
	
    Private IsInitializing As Boolean
    Private IsLoading As Boolean

    ' used for loading current on-hand inventory amounts
    Private mds As DataSet
    Private mdtOnHandSummary As DataTable
    Private mdtOnHandDetail As DataTable
    Private mdv As DataView

    Private _selectedPackSize As Double
    Private _selectedPackUOM As String
    Private _catchWeight As Boolean
    Private _currentUnits As Decimal
    Private _currentWeight As Decimal
    Private _costedByWeight As Boolean
    Private _retailUOM As String
    Private _distributionUOM As String
    Private _reloading As Boolean = False
    Private _isPackageUnit As Boolean
    Private _isDistributionCenter As Boolean
    Private _isStore As Boolean

    Private _isSoldAsEachCostedByWeightItem As Boolean = False
    Private _itemIdentifier As String = ""

    Dim dtShrinkSubtypes As New System.Data.DataTable
    Const col_ShrinkSubtype_ID As String = "ShrinkSubtype_ID"
    Const col_ShrinkReasonCode As String = "ReasonCodeDescription"
    Const col_InvAdjCode_Id As String = "InventoryAdjustmentCode_ID"

    Private Sub frmInventoryAdjustment_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        glItemID = -1

        IsLoading = False

        'Load the facilities that can do adjustments
        cmbStore.Items.Clear()

        Try
            'Load all Unit Abbr into combo box.
            gRSRecordset = SQLOpenRecordSet("EXEC GetStoresAndDistAdjustments", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            Do While Not gRSRecordset.EOF
                cmbStore.Items.Add(New VB6.ListBoxItem(gRSRecordset.Fields("Store_Name").Value, gRSRecordset.Fields("Store_No").Value))
                gRSRecordset.MoveNext()
            Loop

            cmbStore.SelectedIndex = 0
        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

        LoadAllSubTeams(cmbSubTeam)
        cmbSubTeam.SelectedIndex = -1

        LoadShrinkSubtypesComboBox(cmbShrinkSubtype)

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = 0
        End If
    End Sub

    Private Sub cmdItemSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdItemSearch.Click

        If Not cmbSubTeam.SelectedIndex = -1 Then

            Me.ResetPanel()

            '-- Set glItemid to none found
            frmItemSearch.InitForm()
            frmItemSearch.LimitSubTeam_No = VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex)
            frmItemSearch.ShowDialog()
            If glItemID <> 0 Then
                '-- if its not zero, then something was found
                LoadItemInformation()
                ' set the form title to show the item selected
                Me.Text = "Inventory Adjustment - " & Trim(Me.txtItemDesc.Text)
                ' enable all the various containers and grids
            End If
            frmItemSearch.Close()
            frmItemSearch.Dispose()

        Else
            MessageBox.Show("Please provide a Sub Team selection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Sub LoadItemInformation()

        Dim sSQL As String

        IsLoading = True
        Try
            If glItemID = -1 Then
                gRSRecordset = SQLOpenRecordSet("EXEC GetAdjustmentInfoFirst " & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex).ToString, DAO.RecordsetTypeEnum.dbOpenSnapshot, _
                                        DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Else
                sSQL = "EXEC GetAdjustmentInfo " & glItemID.ToString & ", " & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex).ToString
                gRSRecordset = SQLOpenRecordSet(sSQL, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            End If

            glItemID = gRSRecordset.Fields("Item_Key").Value
            SetCombo(cmbSubTeam, gRSRecordset.Fields("SubTeam_No").Value)

            Me.txtItemDesc.Text = gRSRecordset.Fields("Item_Description").Value
            Me.txtIdentifier.Text = gRSRecordset.Fields("Identifier").Value
            Me._costedByWeight = gRSRecordset.Fields("CostedByWeight").Value
            Me._catchWeight = gRSRecordset.Fields("CatchWeightRequired").Value
            Me._retailUOM = gRSRecordset.Fields("RetailUOM").Value
            Me._distributionUOM = gRSRecordset.Fields("DCUOM").Value
            Me._isPackageUnit = gRSRecordset.Fields("IsPackageUnit").Value
            Me._isDistributionCenter = gRSRecordset.Fields("Distribution_Center").Value
            Me._isStore = gRSRecordset.Fields("WFM_Store").Value

            Me._isSoldAsEachCostedByWeightItem = WholeFoods.IRMA.ItemHosting.DataAccess.ItemDAO.IfSoldAsEachInRetail(Me.txtIdentifier.Text)

            LoadInvAdjReason(cmbReason, _isDistributionCenter, _isStore)
            SetActive(cmbReason, True)
            cmbShrinkSubtype.SelectedIndex = -1

            'optReset.Checked = False
            optSubtract.Checked = False
            optAdd.Checked = False
            'optReset.Enabled = False
            optSubtract.Enabled = False
            optAdd.Enabled = False

            If Me._costedByWeight Then
                Me.optPounds.Enabled = True
                Me.optPounds.Checked = True
                Me.optCases.Enabled = True
                Me.optUnits.Enabled = False
            Else
                Me.optPounds.Enabled = False
                Me.optUnits.Enabled = True
                Me.optUnits.Checked = True
                Me.optCases.Enabled = True
            End If

            ' clear out adjustable pack sizes
            Me.cmbPack.Items.Clear()
            Me.cmbPack.Text = String.Empty

            ' load the current inventory on-hand information
            Me.SetupDataTable()
            Me.LoadDataTable()

            ' don't allow an adjustment unless there is current inventory
            Select Case cmbPack.Items.Count
                Case 1
                    Me.cmbPack.SelectedIndex = 0
                    Me.cmbPack.Enabled = False
                    Me.AllowAdjustment()
                Case Is > 1
                    Me.cmbPack.Items.Insert(0, "--Select Pack--")
                    Me.cmbPack.SelectedIndex = 0
                    Me.cmbPack.Enabled = True
                    Me.DisallowAdjustment()
                Case Else
                    Me.cmbPack.Enabled = False
            End Select

            ' Disable Amount value to be entered when the Item Info is loaded
            Me.txtQuantity.Enabled = False
        Finally

            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If

        End Try

        IsLoading = False

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
        Dim sSubTeam_No As String

        If ((optReset.Checked = True) And (SystemDateTime(True) = GetEndOfPeriodDate())) Then
            MsgBox("Today is End of Period - Reset by closing the Master Cycle Count.", MsgBoxStyle.Critical, "Reset not allowed...")
            Exit Sub
        End If

        If glItemID = -1 Then
            MsgBox("Select an item.", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        '-- Make sure they entered quantity
        If Trim(txtQuantity.Text) = "" Then
            MsgBox("A Quantity must be entered.", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        '-- Make sure they entered reason
        If cmbReason.SelectedIndex = -1 Then
            MsgBox("A Reason must be selected.", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        '-- Make sure they selected Adjustment Type
        'Bug 13786 MD: This is needed so that when reset option is selected we ignore adjustment type selection
        If (optSubtract.Enabled And optAdd.Enabled) Then
            If (optSubtract.Checked = False And optAdd.Checked = False) Then
                MsgBox("Adjustment Type must be selected.", MsgBoxStyle.Information, Me.Text)
                Exit Sub
            End If
        End If

        sSubTeam_No = ComboValue(cmbSubTeam)

        '-- Make sure its what they want to do
        If MsgBox("Really adjust this quantity?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Notice") = MsgBoxResult.No Then Exit Sub

        Dim _quantity As Decimal = 0
        Dim _weight As Decimal = 0

        If Me._costedByWeight Then
            If Me.optPounds.Checked Then
                _weight = CDec(Me.txtQuantity.Text)
            ElseIf Me.optCases.Checked Then
                _weight = CDec(Me.txtQuantity.Text) * Me._selectedPackSize
            End If
        Else
            If Me.optCases.Checked Then
                _quantity = CDec(Me.txtQuantity.Text) * Me._selectedPackSize
            ElseIf Me.optUnits.Checked Then
                _quantity = CDec(Me.txtQuantity.Text)
            End If
        End If

        'per bug 13591 - the three spoilage adjustment codes (specified below) have associated code (see bug 13534) 
        'such that the quantity values in ItemHistory should always be stored as a positive number, and then the associated 
        'Adjustment_Type is applied when those values are read out.  The other inventory adjustment codes aren't set up to
        'behave this way, so we're handling the +/- here based on the user input.

        Dim _reason As String = cmbReason.SelectedItem.ToString()
        Dim _allowSubtract As Boolean = True

        If (_reason = "SM-Samples" Or _reason = "SP-Spoilage" Or _reason = "FB-Food Bank") Then _allowSubtract = False
        If (_allowSubtract = True And optSubtract.Checked = True) Then
            _quantity = _quantity * -1
            _weight = _weight * -1
        End If

        Dim shrinkSubtypeId As Integer = cmbShrinkSubtype.SelectedValue

        Dim _sql As String = "EXEC InsertItemHistory3 " & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex) & ", " & glItemID & ", '" & VB6.Format(SystemDateTime, "MM/DD/YYYY HH:MM:SS") & "','" & _quantity & "','" & _weight & "'," & VB6.GetItemData(cmbReason, cmbReason.SelectedIndex) & "," & giUserID & ", " & sSubTeam_No & "," & Me._selectedPackSize & "," & shrinkSubtypeId.ToString

        SQLExecute(_sql, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        Me._reloading = True

        '-- Make sure they enter a real quantity each time
        txtQuantity.Text = ""

        SetupDataTable()
        LoadDataTable()

        Me._reloading = False
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub SetupDataTable()

        ' Create a data table to store the summary On Hand information
        mdtOnHandSummary = New DataTable("GetStoreOnHand")

        mdtOnHandSummary.Columns.Add(New DataColumn("Item Key", GetType(String)))
        mdtOnHandSummary.Columns.Add(New DataColumn("TTL Units", GetType(String)))
        mdtOnHandSummary.Columns.Add(New DataColumn("TTL Weight", GetType(String)))

        ' setup the primary key
        Dim mdtOnHandSummaryKeys(0) As DataColumn
        mdtOnHandSummaryKeys(0) = mdtOnHandSummary.Columns("Item Key")

        mdtOnHandSummary.PrimaryKey = mdtOnHandSummaryKeys

        ' Create a data table to store the detail On Hand information
        mdtOnHandDetail = New DataTable("GetStoreOnHandDetail")

        mdtOnHandDetail.Columns.Add(New DataColumn("Item Key", GetType(String)))
        mdtOnHandDetail.Columns.Add(New DataColumn("Units", GetType(String)))
        mdtOnHandDetail.Columns.Add(New DataColumn("Pack", GetType(String)))

    End Sub

    Private Sub LoadDataTable()

        Dim rsOnHandSummary As DAO.Recordset = Nothing
        Dim rsOnHandDetail As DAO.Recordset = Nothing
        Dim row As DataRow
        Dim CannotAdjust As Boolean = False

        Try

            ' clear out adjustable pack sizes
            Me.cmbPack.Items.Clear()

            ' clear everything before loading the dataset
            Me.gridOnHand.DataSource = Nothing
            mdtOnHandDetail.Rows.Clear()
            mdtOnHandSummary.Rows.Clear()
            mds = Nothing

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            ' get the store/item/pack details
            rsOnHandSummary = SQLOpenRecordSet("EXEC dbo.GetStoreOnHand " & glItemID & "," & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex) & "," & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            ' loop through and setup the store/item quantities

            If Not rsOnHandSummary.EOF Then

                row = mdtOnHandSummary.NewRow

                row("Item Key") = glItemID

                If Not rsOnHandSummary.Fields("Weight").Value > 0 Then
                    row("TTL Weight") = 0.0
                    Me._currentWeight = 0.0
                Else
                    Me._currentWeight = VB6.Format(rsOnHandSummary.Fields("Weight").Value, "####0.0###")
                    row("TTL Weight") = Me._currentWeight & " " & Me._retailUOM
                End If

                If Not rsOnHandSummary.Fields("OnHand").Value > 0 Then
                    row("TTL Units") = 0.0
                    Me._currentUnits = 0.0
                Else
                    Me._currentUnits = VB6.Format(rsOnHandSummary.Fields("OnHand").Value, "####0.0###")
                    row("TTL Units") = Me._currentUnits & " " & Me._retailUOM
                End If

                mdtOnHandSummary.Rows.Add(row)
                mdtOnHandSummary.AcceptChanges()
                ' End While
            End If


            ' close the recordset
            If rsOnHandSummary IsNot Nothing Then
                rsOnHandSummary.Close()
                rsOnHandSummary = Nothing
            End If

            ' get the store/item/pack details
            rsOnHandDetail = SQLOpenRecordSet("EXEC dbo.GetStoreOnHandDetail " & glItemID & "," & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex) & "," & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            ' loop through and setup the store/item/pack details
            While (Not rsOnHandDetail.EOF)
                row = mdtOnHandDetail.NewRow
                row("Item Key") = glItemID
                row("Units") = VB6.Format(Math.Round(CDec(rsOnHandDetail.Fields("OnHand").Value), 2), "####0.0###") & " " & Me._distributionUOM
                row("Pack") = CInt(rsOnHandDetail.Fields("PackSize").Value) & " " & Me._retailUOM
                mdtOnHandDetail.Rows.Add(row)
                AddAdjustablePack(rsOnHandDetail.Fields("PackSize").Value, Me._retailUOM)
                rsOnHandDetail.MoveNext()
            End While

            'Removed the if logic per bug 13787
            'If mdtOnHandDetail.Rows.Count > 0 Then

            ' the item can be adjusted to enable all the controls
            Me.groupAdjustment.Enabled = True
            Me.groupResults.Enabled = True
            Me.gridOnHand.Enabled = True

            '' set the ability to use differnt adjustment types based on user's role
            '' RESET is removed from the screen.  
            '                optReset.Enabled = gbCoordinator Or gbAccountant Or gbInventory_Administrator

            'If Not optAdjust.Enabled Then
            '    optWaste.Checked = True
            'End If

            If Not Me._reloading Then

                Me.lblAdjustmentUOM.Text = Me._retailUOM
                Me.lblAdjustmentUOM.Visible = True

            End If

            'Else

            'CannotAdjust = True

            'End If

            mdtOnHandDetail.AcceptChanges()

            ' close the recordset
            If rsOnHandDetail IsNot Nothing Then
                rsOnHandDetail.Close()
                rsOnHandDetail = Nothing
            End If

            ' setup the dataset we're binding to the grid
            mds = New DataSet
            mds.Tables.Add(mdtOnHandSummary)
            mds.Tables.Add(mdtOnHandDetail)

            ' create the relationship between the two dataset tables on the Store_No columns
            Dim relStore As New DataRelation("Key", mds.Tables("GetStoreOnHand").Columns("Item Key"), mds.Tables("GetStoreOnHandDetail").Columns("Item Key"))
            mds.Relations.Add(relStore)

            ' bind the dataset to the grid
            Me.gridOnHand.DataSource = mds

            ' hide the store_no column
            Me.gridOnHand.DisplayLayout.Bands(0).Columns(0).Hidden = True
            Me.gridOnHand.DisplayLayout.Bands(1).Columns(0).Hidden = True

            'Not needed per bug 13787
            'If CannotAdjust Then
            '    MessageBox.Show("Inventory cannot be adjusted." & Chr(13) & "Receive the item on a PO first.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            'End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            ' just in case, be sure the recordsets are closed
            If rsOnHandSummary IsNot Nothing Then
                rsOnHandSummary.Close()
                rsOnHandSummary = Nothing
            End If

            If rsOnHandDetail IsNot Nothing Then
                rsOnHandDetail.Close()
                rsOnHandDetail = Nothing
            End If

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    Private Sub AddAdjustablePack(ByVal PackSize As Integer, ByVal PackUOM As String)

        Dim _item As New VB6.ListBoxItem(PackSize & "|" & PackUOM, PackSize)
        cmbPack.Items.Add(_item)
        cmbPack.Sorted = True

    End Sub

    Private Sub cmbPack_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPack.SelectedIndexChanged

        If Not Me.IsLoading And cmbPack.SelectedIndex > 0 Then

            AllowAdjustment()

        ElseIf Not Me.IsLoading And cmbPack.SelectedIndex = 0 Then

            DisallowAdjustment()

        End If

    End Sub

    Private Sub cmbReason_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReason.SelectedIndexChanged

        Dim sSQL As String

        Dim bAdd As Boolean
        Dim bSubtract As Boolean
        Dim bReset As Boolean

        'For some reason, this Sub doesn't get called when you hit backspace in there.  
        If cmbReason.SelectedIndex = -1 Then
            optSubtract.Checked = False
            optAdd.Checked = False
            optSubtract.Enabled = False
            optAdd.Enabled = False
        End If

        sSQL = "EXEC GetInventoryAdjustmentAllows " & VB6.GetItemData(cmbReason, cmbReason.SelectedIndex).ToString
        gRSRecordset = SQLOpenRecordSet(sSQL, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        bAdd = gRSRecordset.Fields("AllowsInventoryAdd").Value
        bSubtract = gRSRecordset.Fields("AllowsInventoryDelete").Value
        bReset = gRSRecordset.Fields("AllowsInventoryReset").Value
        Me.optAdd.Enabled = bAdd
        Me.optSubtract.Enabled = bSubtract
        'Bug 13786 MD: This is needed so that users can enter a value for the reset
        Me.txtQuantity.Enabled = bReset


        If (optAdd.Checked = False And optSubtract.Checked = False) Then
            Label1.Enabled = False
            txtQuantity.Enabled = False
            lblAdjustmentUOM.Enabled = False
            ' remove previous values in Amount
            Me.txtQuantity.Text = String.Empty
        Else
            If (optAdd.Checked = True Or optSubtract.Checked = True) Then
                Label1.Enabled = True
                txtQuantity.Enabled = True
                lblAdjustmentUOM.Enabled = True
                ' remove previous values in Amount
                Me.txtQuantity.Text = String.Empty
            End If

        End If

        If (optAdd.Enabled = False And optSubtract.Enabled = False) Then
            Label1.Enabled = False

            'Bug 13786 MD: This is needed so that users can enter a value for the reset
            txtQuantity.Enabled = bReset
            optAdd.Checked = False
            optSubtract.Checked = False

            lblAdjustmentUOM.Enabled = False
            ' remove previous values in Amount
            Me.txtQuantity.Text = String.Empty

        End If

        If Not optAdd.Enabled And optSubtract.Enabled Then
            optSubtract.Checked = True
        End If

        If Not optSubtract.Enabled And optAdd.Enabled Then
            optAdd.Checked = True
        End If


        'TFS 2010 Task# 762, 01/24/2010 - Faisal Ahmed 
        '4.2 feature to dissassociate retail UOM with costed-by-weight flag
        'Enabled Units when a costed by weight item is sold in retail as each
        Dim _reason As String = cmbReason.SelectedItem.ToString()
        If Me._costedByWeight Then
            If (_reason = "SM-Samples" Or _reason = "SP-Spoilage" Or _reason = "FB-Food Bank") And Me._isSoldAsEachCostedByWeightItem Then
                Me.optUnits.Enabled = True
            Else
                Me.optUnits.Enabled = False
                Me.optPounds.Checked = True
            End If
        End If

        ' en/dis-able the shrink subtype combo box based on the main adjustment type
        If (_reason = "SM-Samples" Or _reason = "SP-Spoilage" Or _reason = "FB-Food Bank") Then
            Me.cmbShrinkSubtype.Enabled = True
        Else
            Me.cmbShrinkSubtype.SelectedIndex = -1
            Me.cmbShrinkSubtype.Enabled = False
        End If
    End Sub

    Private Sub DisallowAdjustment()

        Me.txtQuantity.Enabled = False
        Me.txtQuantity.Text = String.Empty

    End Sub

    Private Sub AllowAdjustment()

        Dim _selectedPack() As String = VB6.GetItemString(Me.cmbPack, Me.cmbPack.SelectedIndex).Split("|")
        Me._selectedPackSize = CInt(_selectedPack.GetValue(0))
        Me._selectedPackUOM = _selectedPack.GetValue(1)
        Me.txtQuantity.Enabled = True

    End Sub

    Private Sub cmbStore_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbStore.SelectedIndexChanged
        If IsInitializing Or IsLoading Or glItemID = -1 Then Exit Sub
        Me.ResetPanel()
        Me.LoadItemInformation()
    End Sub

    Private Sub ResetPanel()
        Me.optReset.Visible = False
        Me.IsLoading = True
        Me.cmbPack.Text = String.Empty
        Me.cmbPack.Items.Clear()
        Me.groupAdjustment.Enabled = False
        Me.groupResults.Enabled = False
        Me.gridOnHand.Enabled = False
        Me.lblAdjustmentUOM.Text = String.Empty
        Me.lblAdjustmentUOM.Visible = False
        Me.lblResultUnits.Visible = True
        Me.lblResultUnits.Text = String.Empty
        Me.txtQuantity.Text = String.Empty
        Me.txtResultUnits.Text = String.Empty
        Me._currentUnits = 0.0
        Me._currentWeight = 0.0
        Me.IsLoading = False
    End Sub

    Private Sub txtQuantity_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQuantity.TextChanged
        If IsInitializing Or IsLoading Or glItemID = -1 Then Exit Sub
       
        Me.RecalculateAdjustment()


    End Sub

    Private Sub optCases_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optCases.CheckedChanged
        If Me.optCases.Checked Then
            Me.lblAdjustmentUOM.Text = Me._distributionUOM
            Me.lblResultUnits.Text = Me._distributionUOM
            Me.RecalculateAdjustment()
            Me.txtQuantity.Focus()
        End If
    End Sub

    Private Sub optPounds_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optPounds.CheckedChanged
        If Me.optPounds.Checked Then
            Me.lblAdjustmentUOM.Text = "LBS"
            Me.lblResultUnits.Text = "LBS"
            Me.RecalculateAdjustment()
            Me.txtQuantity.Focus()
        End If
    End Sub

    Private Sub optUnits_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optUnits.CheckedChanged
        If Me.optUnits.Checked Then
            Me.lblAdjustmentUOM.Text = Me._retailUOM
            Me.lblResultUnits.Text = Me._retailUOM
            Me.RecalculateAdjustment()
            Me.txtQuantity.Focus()
        End If
    End Sub

    Private Sub RecalculateAdjustment()

        If Me.txtQuantity.Text = String.Empty Or txtQuantity.Text.Trim = "." Then
            Me.txtResultUnits.Text = String.Empty
            Me.lblResultUnits.Visible = False
            Exit Sub
        Else
            Me.lblResultUnits.Visible = True
        End If

        '' Russell - need to limit this to Numeric anyway.  

        If IsNumeric(Me.txtQuantity.Text) Then

            Dim _adjustmentQty As Decimal = CDec(Me.txtQuantity.Text)

            'Clears After Adjustmetn fields if amount field value is not greater than 0.0
            If Not (_adjustmentQty > "0.0") Then
                Me.txtResultUnits.Text = String.Empty
                Me.lblResultUnits.Visible = False
                Exit Sub
            End If

            If Me.optSubtract.Checked And _adjustmentQty > 0 Then _adjustmentQty = _adjustmentQty * -1

            'Bug 13786 MD: This is needed so that users can enter a value for the reset, and that new value
            'is used to set the inventory value.
            If (optAdd.Enabled = False And optSubtract.Enabled = False) Then
                Me.txtResultUnits.Text = _adjustmentQty
                Exit Sub
            End If


            ' to fix bug TFS #8353, always assume RESET is exactly what they are entering
            'If Me.optReset.Checked Then

            '    Me.txtResultUnits.Text = _adjustmentQty

            'ElseIf 

            If Not Me._costedByWeight Then

                If Me.optCases.Checked Then

                    If Me._isPackageUnit Then
                        Me.txtResultUnits.Text = Me._currentUnits + _adjustmentQty
                    Else

                        'Validates the input Amount for calculating the quantity in CAS. Does not allow decimal inputs

                        Dim _getDecimalValue As String() = txtQuantity.Text.Trim.Split(".")


                        If (_getDecimalValue(1).Length > 0) Then
                            If (Convert.ToInt32(_getDecimalValue(1).ToString) > 0) Then
                                MsgBox("Invalid Input for Amount in CAS", MsgBoxStyle.Information, Me.Text)
                                txtQuantity.Text = String.Empty
                                Exit Sub
                            Else
                                Me.txtResultUnits.Text = ((_adjustmentQty * Me._selectedPackSize) + Me._currentUnits) / Me._selectedPackSize
                                Me.lblResultUnits.Text = Me._distributionUOM

                            End If

                        Else
                            Me.txtResultUnits.Text = ((_adjustmentQty * Me._selectedPackSize) + Me._currentUnits) / Me._selectedPackSize
                            Me.lblResultUnits.Text = Me._distributionUOM

                        End If


                    End If

                ElseIf Me.optUnits.Checked Then

                    Me.txtResultUnits.Text = Me._currentUnits + _adjustmentQty

                End If

            ElseIf Me._costedByWeight Then

                If Me.optCases.Checked Then

                    Me.txtResultUnits.Text = ((_adjustmentQty * Me._selectedPackSize) + Me._currentWeight) / Me._selectedPackSize

                ElseIf Me.optUnits.Checked Or Me.optPounds.Checked Then

                    Me.txtResultUnits.Text = Me._currentWeight + _adjustmentQty

                    'TFS 2010 Task# 762, 01/24/2010 - Faisal Ahmed 
                    '4.2 feature to dissassociate retail UOM with costed-by-weight flag
                    'Multiply the UNITS by Average_Unit_Weight for coste-by-weight items that are sold as each 
                    If Me._isSoldAsEachCostedByWeightItem And Me._costedByWeight And Me.optUnits.Checked Then
                        Dim averageUnitWeight = WholeFoods.IRMA.ItemHosting.DataAccess.ItemDAO.GetAverageUnitCost(Me._txtIdentifier.Text)
                        Me.txtResultUnits.Text = Me._catchWeight + _adjustmentQty * averageUnitWeight
                    End If

                End If

            End If

        End If


    End Sub

    Private Sub optAdd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optAdd.CheckedChanged
        If IsInitializing Or IsLoading Or glItemID = -1 Then Exit Sub
        ' enable amount to be entered
        Label1.Enabled = True
        txtQuantity.Enabled = True
        lblAdjustmentUOM.Enabled = True
        Me.RecalculateAdjustment()
    End Sub

    Private Sub optReset_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optReset.CheckedChanged
        If IsInitializing Or IsLoading Or glItemID = -1 Then Exit Sub
        Me.RecalculateAdjustment()
    End Sub

    Private Sub optSubtract_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optSubtract.CheckedChanged
        If IsInitializing Or IsLoading Or glItemID = -1 Then Exit Sub
        ' enable amount to be entered
        Label1.Enabled = True
        txtQuantity.Enabled = True
        lblAdjustmentUOM.Enabled = True
        Me.RecalculateAdjustment()
    End Sub

    Private Sub LoadShrinkSubtypesComboBox(ByRef comboBox As ComboBox)
        ' read the shrink subtypes from the database
        dtShrinkSubtypes = ShrinkCorrectionsDAO.GetShrinkSubTypesOnly()

        ' set the combo box data source & member properties
        comboBox.DataSource = dtShrinkSubtypes
        comboBox.ValueMember = col_ShrinkSubtype_ID
        comboBox.DisplayMember = col_ShrinkReasonCode
        comboBox.SelectedIndex = -1

    End Sub

    ' if a shrink subtype has been selected, make sure the main shrink type (inventory adjustment code) fits
    Private Sub cmbShrinkSubtype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbShrinkSubtype.SelectedIndexChanged
        If IsInitializing Or IsLoading Or glItemID = -1 Or cmbShrinkSubtype.SelectedIndex < 0 Then Exit Sub

        ' find the indices in the target control
        Dim idxFoodBank As Integer = cmbReason.FindString("FB-Food Bank")
        Dim idxSamples As Integer = cmbReason.FindString("SM-Samples")
        Dim idxSpoilage As Integer = cmbReason.FindString("SP-Spoilage")


        ' the subtype control was bound with a data table having the same schema as the dbo.ShrinkSubtype table
        ' so read the subtype description (ShrinkSubtype_ID & InventoryAdjustmentCode_ID are the other fields) 
        Dim selectedSubtypeRow As DataRowView = cmbShrinkSubtype.SelectedItem
        Dim shrinkSubtype_Desc As String = selectedSubtypeRow.Item("ReasonCodeDescription").Trim()

        ' switch the reason combo box (shrink type) to a type which corresponds with the chosen shrink subtype
        If (shrinkSubtype_Desc).StartsWith("Donation", StringComparison.CurrentCultureIgnoreCase) Then
            cmbReason.SelectedIndex = idxFoodBank
        ElseIf (shrinkSubtype_Desc).StartsWith("Sampling", StringComparison.CurrentCultureIgnoreCase) Then
            cmbReason.SelectedIndex = idxSamples
        Else
            cmbReason.SelectedIndex = idxSpoilage
        End If
    End Sub

End Class
