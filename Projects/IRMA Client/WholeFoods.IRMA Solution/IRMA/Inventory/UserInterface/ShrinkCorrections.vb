Option Strict Off
Option Explicit On

Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess
Imports Infragistics.Win.UltraWinGrid
Imports log4net
Imports System.Windows.Forms
Imports System.Linq

Friend Class frmShrinkCorrections

    Dim daItemCatalog As New System.Data.SqlClient.SqlDataAdapter
    Dim dtShrink As New System.Data.DataTable
    Dim dtShrinkDetails As New System.Data.DataSet
    Dim dtWasteTypes As New System.Data.DataTable
    Dim dtShrinkSubtypes As New System.Data.DataTable
    Dim dtSubTeams As New System.Data.DataTable
    Dim dtGridWasteTypes As New System.Data.DataTable
    Dim dtGridShrinkSubtypes As New System.Data.DataTable

    ' constants for the shrink types ('wType') combo box - from GetInventoryAdjustmentWasteCodes
    Const col_InvAdjCode_Id As String = "InventoryAdjustmentCode_ID"
    Const col_InvAdjCode_WasteType As String = "Waste_type"
    Const col_InvAdjCode_Description As String = "Description"
    ' constants for the shrink subtypes combo box - from GetShrinkSubtypesOnly
    Const col_ShrinkSubtypes_Id As String = "ShrinkSubType_ID"
    Const col_ShrinkSubtypes_Descr As String = "ReasonCodeDescription"
    Const col_ShrinkSubtypes_InvAdjCode_Id As String = "InventoryAdjustmentCode_ID"
    '  constants for the results grid  - from GetShrinkCorrections
    Const col_Grid_ShrinkType As String = "wType"
    Const col_Grid_ShrinkSubtype As String = "ShrinkSubtype_ID"
    Const col_Grid_InvAdjCode_Id As String = "InventoryAdjustment_ID"
    Const col_Grid_ShrinkSubtype_Desc As String = "ShrinkSubtype_Desc"
    ' constants for change-tracking columns - added programmatically
    Const col_GridOld_WasteType As String = "OLD_Waste_type"
    Const col_GridOld_InvAdjCodeId As String = "OLD_inventoryAdjustmentCode_ID"
    Const col_GridOld_ShrinkSubtypeId As String = "OLD_shrinkSubtype_ID"
    'keep track of shrink type/subtypes for validation of changes
    Dim validShrinkTypeIds As List(Of Integer)
    Dim validShrinkSubTypeIds As List(Of Integer)

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub frmShrinkCorrections_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmShrinkCorrections_Load Entry")

        ' Load Store and SubTeam combo boxes
        LoadStores(cmbStore)
        LoadSubTeam(cmbSubTeam)
        ' Set the Store combobox to the Store Limit for the user if there is one.
        If glStore_Limit > 0 Then
            SetCombo(cmbStore, glStore_Limit)
            SetActive(cmbStore, False)
        End If

        ' Load Shrink combo boxes
        LoadShrinkTypesComboBox(cmbShrinkType)
        LoadShrinkSubtypesComboBox(cmbShrinkSubtype)

        If InstanceDataDAO.IsFlagActive("SplitWasteCategory") = False Then
            cmbShrinkType.Enabled = False
        End If

        gridShrink.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center
        gridShrink.DisplayLayout.Bands(0).Override.ColumnAutoSizeMode = ColumnAutoSizeMode.AllRowsInBand

        logger.Debug("frmShrinkCorrections_Load Exit")


    End Sub

    Private Sub LoadShrinkTypesComboBox(ByRef comboBox As ComboBox)

        ' read the shrink types from the database
        dtWasteTypes = ShrinkCorrectionsDAO.GetInventoryAdjustmentWasteCodes()

        validShrinkTypeIds = dtWasteTypes.AsEnumerable _
                .Where(Function(x) x(col_InvAdjCode_Id) > 0) _
                .Select(Function(x) Int32.Parse(x(col_InvAdjCode_Id).ToString)).ToList()

        ' add row for the "All" options
        Dim allTypesRow As DataRow = dtWasteTypes.NewRow()
        allTypesRow(col_InvAdjCode_Id) = 0
        allTypesRow(col_InvAdjCode_WasteType) = "All"
        allTypesRow(col_InvAdjCode_Description) = "All Waste Types"
        dtWasteTypes.Rows.InsertAt(allTypesRow, 0)

        ' set the combo box data source & member properties
        comboBox.DataSource = dtWasteTypes
        comboBox.ValueMember = col_InvAdjCode_Id
        comboBox.DisplayMember = col_InvAdjCode_Description
        comboBox.SelectedIndex = -1

    End Sub

    Private Sub LoadShrinkSubtypesComboBox(ByRef comboBox As ComboBox)

        ' read the shrink types from the database
        dtShrinkSubtypes = ShrinkCorrectionsDAO.GetShrinkSubTypesOnly()

        validShrinkSubTypeIds = dtShrinkSubtypes.AsEnumerable _
                .Where(Function(x) x(col_ShrinkSubtypes_Id) > 0) _
                .Select(Function(x) Int32.Parse(x(col_ShrinkSubtypes_Id).ToString)).ToList()

        ' add row for the "All" options
        Dim allTypesRow As DataRow = dtShrinkSubtypes.NewRow()
        allTypesRow(col_ShrinkSubtypes_Id) = 0
        allTypesRow(col_ShrinkSubtypes_Descr) = "All Subtypes"
        allTypesRow(col_InvAdjCode_Id) = 0
        dtShrinkSubtypes.Rows.InsertAt(allTypesRow, 0)

        ' set the combo box data source & member properties
        comboBox.DataSource = dtShrinkSubtypes
        comboBox.ValueMember = col_ShrinkSubtypes_Id
        comboBox.DisplayMember = col_ShrinkSubtypes_Descr
        comboBox.SelectedIndex = -1

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click

        Dim StoreNo As Int32
        Dim SubTeamNo As Int32
        Dim StartDate As DateTime
        Dim EndDate As DateTime
        Dim InventoryAdjustmentCode As Int32
        Dim ShrinkSubtype As Int32

        Try
            gridShrink.DataSource = Nothing
            gridShrink.DisplayLayout.Reset()

            If Me.cmbStore.SelectedIndex = -1 Then
                MsgBox("Store selection is required", MsgBoxStyle.Critical, Me.Text)
                Me.cmbStore.Focus()
                Exit Sub
            Else
                StoreNo = VB6.GetItemData(cmbStore, cmbStore.SelectedIndex)
            End If

            If Me.cmbSubTeam.SelectedIndex = -1 Then
                MsgBox("SubTeam selection is required", MsgBoxStyle.Critical, Me.Text)
                Me.cmbSubTeam.Focus()
                Exit Sub
            Else
                SubTeamNo = VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex)
            End If

            If Me.mskStartDate.Text.Length = 0 Then
                MsgBox("Start Date is required", MsgBoxStyle.Critical, Me.Text)
                Me.mskStartDate.Focus()
                Exit Sub
            Else
                StartDate = Me.mskStartDate.Text
            End If

            If Me.mskEndDate.Text.Length = 0 Then
                MsgBox("End Date is required", MsgBoxStyle.Critical, Me.Text)
                Me.mskEndDate.Focus()
                Exit Sub
            Else
                EndDate = Me.mskEndDate.Text
                'bug 7401.  Include the entire day for end date
                EndDate = EndDate.AddHours(23)
                EndDate = EndDate.AddMinutes(59)
                EndDate = EndDate.AddSeconds(59)
            End If

            If cmbShrinkType.SelectedIndex >= 0 Then
                Dim wasteTypeSelection As DataRowView = TryCast(cmbShrinkType.SelectedItem, DataRowView)
                If (wasteTypeSelection IsNot Nothing) Then
                    InventoryAdjustmentCode = Convert.ToInt32(wasteTypeSelection.Item(col_InvAdjCode_Id))
                End If
            End If

            If cmbShrinkSubtype.SelectedIndex >= 0 Then
                Dim subtypeSelection As DataRowView = TryCast(cmbShrinkSubtype.SelectedItem, DataRowView)
                If (subtypeSelection IsNot Nothing) Then
                    ShrinkSubtype = Convert.ToInt32(subtypeSelection.Item(col_ShrinkSubtypes_Id))
                    'InventoryAdjustmentCode = Convert.ToInt32(subtypeSelection.Item(col_InvAdjCode_Id))
                End If
            End If

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            dtShrinkDetails = ShrinkCorrectionsDAO.GetShrinkCorrectionsDetails(StoreNo, SubTeamNo, StartDate, EndDate, ShrinkSubtype, InventoryAdjustmentCode)
            gridShrink.DataSource = dtShrinkDetails

            If gridShrink.Rows.Count > 0 Then
                gridShrink.Rows(0).Selected = True

                cmdExcelExport.Enabled = True
            End If

            gridShrink.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            gridShrink.DisplayLayout.Bands(0).Override.ColumnAutoSizeMode = ColumnAutoSizeMode.AllRowsInBand

        Catch ex As Exception
            MsgBox("Search failed with error " & ex.Message, MsgBoxStyle.Critical, Me.Text)
        Finally
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End Try

    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim msg As String = String.Empty

        Dim n As Integer = gridShrink.Rows.Count
        Dim changed As Boolean = False

        For i As Integer = 0 To n - 1
            If gridShrink.Rows(i).Cells("AllowEdit").Value = True Then
                Dim deleteQuantity As Decimal = 0
                Dim deleteWeight As Decimal = 0

                Dim OldQty As Decimal = CDbl(gridShrink.Rows(i).Cells("OldQty").Value)
                Dim NewQty As Decimal = CDbl(gridShrink.Rows(i).Cells("Qty").Value)

                Dim NewWeight As Decimal = 0
                Dim OldWeight As Decimal = 0

                changed = False

                If NewQty <> OldQty Then
                    changed = True
                    msg = msg + "Qty  =" + (NewQty - OldQty).ToString + " "
                End If

                Dim oldInventoryAdjustmentCodeId As Integer = 0
                Dim newInventoryAdjustmentCodeId As Integer = 0
                Dim oldShrinkSubtypeId As Integer = 0
                Dim newShrinkSubtypeId As Integer = 0
                If gridShrink.Rows(i).Cells(col_GridOld_InvAdjCodeId).Value IsNot Nothing Then
                    oldInventoryAdjustmentCodeId = gridShrink.Rows(i).Cells(col_GridOld_InvAdjCodeId).Value
                End If
                If gridShrink.Rows(i).Cells(col_Grid_InvAdjCode_Id).Value IsNot Nothing Then
                    newInventoryAdjustmentCodeId = gridShrink.Rows(i).Cells(col_Grid_InvAdjCode_Id).Value
                End If
                If gridShrink.Rows(i).Cells(col_GridOld_ShrinkSubtypeId).Value IsNot Nothing Then
                    oldShrinkSubtypeId = gridShrink.Rows(i).Cells(col_GridOld_ShrinkSubtypeId).Value
                End If
                If gridShrink.Rows(i).Cells(col_GridOld_ShrinkSubtypeId).Value IsNot Nothing Then
                    newShrinkSubtypeId = gridShrink.Rows(i).Cells(col_Grid_ShrinkSubtype).Value
                End If

                If oldInventoryAdjustmentCodeId <> newInventoryAdjustmentCodeId Then
                    changed = True
                    msg = msg + "Type  =" + newInventoryAdjustmentCodeId.ToString() + " "
                End If
                If oldShrinkSubtypeId <> newShrinkSubtypeId Then
                    changed = True
                    msg = msg + "SubType  =" + newShrinkSubtypeId.ToString() + " "
                End If

                Dim OldSubTeam As String = gridShrink.Rows(i).Cells("OldSubTeam_No").Value.ToString
                Dim NewSubTeam As String = gridShrink.Rows(i).Cells("SubTeam_No").Value.ToString

                If NewSubTeam <> OldSubTeam Then
                    changed = True
                    msg = msg + "SubTeam  =" + NewSubTeam + " "
                End If

                Dim identifier As String = gridShrink.Rows(i).Cells("Identifier").Value.ToString
                Dim username As String = gridShrink.Rows(i).Cells("UserName").Value.ToString
                Dim CostedByWeight As Integer = CInt(gridShrink.Rows(i).Cells("CostedbyWeight").Value)

                Dim IsSoldAsEachinRetail As Boolean = WholeFoods.IRMA.ItemHosting.DataAccess.ItemDAO.IfSoldAsEachInRetail(identifier)

                If CostedByWeight = 0 Then
                    Dim averageUnitWeight = WholeFoods.IRMA.ItemHosting.DataAccess.ItemDAO.GetAverageUnitCost(identifier)
                    deleteQuantity = OldQty * (-1) 'Bug 1406: Needed for corrections
                    deleteWeight = 0

                    If IsSoldAsEachinRetail = 1 Then
                        NewWeight = NewQty * averageUnitWeight
                        NewQty = 0
                        deleteQuantity = 0
                        deleteWeight = OldQty * averageUnitWeight
                    End If
                Else
                    NewWeight = NewQty
                    NewQty = 0
                    deleteQuantity = 0
                    deleteWeight = OldQty * (-1) 'Bug 1406: Needed for corrections
                End If

                If changed Then
                    Dim deleteSQLString As String = "EXEC InsertItemHistoryShrinkUpdate " _
                            & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex) & ", " _
                            & ShrinkCorrectionsDAO.GetItemKey(identifier) & ", '" _
                            & gridShrink.Rows(i).Cells("OriginalDateStamp").Text & "','" _
                            & CStr(deleteQuantity) & "','" _
                            & CStr(deleteWeight) & "'," _
                            & oldInventoryAdjustmentCodeId.ToString() & "," _
                            & ShrinkCorrectionsDAO.GetUserID(username) & ", " _
                            & OldSubTeam & "," _
                            & "0," _
                            & oldShrinkSubtypeId.ToString()

                    SQLExecute(deleteSQLString, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                    'Add New Entry
                    Dim insertSQLString As String = "EXEC InsertItemHistory3 " _
                            & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex) & ", " _
                            & ShrinkCorrectionsDAO.GetItemKey(identifier) & ", '" _
                            & gridShrink.Rows(i).Cells("OriginalDateStamp").Text & "','" _
                            & CStr(NewQty) & "','" _
                            & CStr(NewWeight) & "'," _
                            & newInventoryAdjustmentCodeId.ToString() & "," _
                            & ShrinkCorrectionsDAO.GetUserID(gsUserName) & ", " _
                            & NewSubTeam & "," _
                            & "0," _
                            & newShrinkSubtypeId.ToString()

                    SQLExecute(insertSQLString, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                End If
            End If
        Next i

        cmdSearch.PerformClick()

    End Sub

    Private Sub gridShrink_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles gridShrink.InitializeLayout

        gridShrink.DisplayLayout.GroupByBox.Hidden = True

        gridShrink.DisplayLayout.Override.CellAppearance.BorderColor = Color.Black

        'Disable column moving and swapping
        gridShrink.DisplayLayout.Bands(0).CardView = False
        gridShrink.DisplayLayout.Bands(0).Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False

        gridShrink.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed
        gridShrink.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed
        gridShrink.DisplayLayout.Override.AllowGroupMoving = AllowGroupMoving.NotAllowed
        gridShrink.DisplayLayout.Override.AllowGroupSwapping = AllowGroupSwapping.NotAllowed

        'Enable single sort
        e.Layout.Override.HeaderClickAction = HeaderClickAction.SortSingle
        gridShrink.DisplayLayout.Override.HeaderAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True

        'Hide a few columns
        gridShrink.DisplayLayout.Bands(0).Columns("CostedbyWeight").Hidden = True
        gridShrink.DisplayLayout.Bands(0).Columns("OriginalDateStamp").Format = "MM/dd/yyyy hh:mm:ss.fff tt"
        gridShrink.DisplayLayout.Bands(0).Columns("OriginalDateStamp").Hidden = True
        gridShrink.DisplayLayout.Bands(0).Columns("UnitCost").Hidden = True

        gridShrink.DisplayLayout.Bands(0).Columns(col_Grid_InvAdjCode_Id).Hidden = True
        gridShrink.DisplayLayout.Bands(0).Columns(col_Grid_ShrinkSubtype_Desc).Hidden = True

        gridShrink.DisplayLayout.Bands(0).Columns.Add("AllowEdit")
        gridShrink.DisplayLayout.Bands(0).Columns("AllowEdit").Hidden = True
        gridShrink.DisplayLayout.Bands(0).Columns("AllowEdit").DataType = GetType(Boolean)

        gridShrink.DisplayLayout.Bands(0).Columns.Add("OldQty")
        gridShrink.DisplayLayout.Bands(0).Columns("OldQty").Hidden = True
        gridShrink.DisplayLayout.Bands(0).Columns("OldQty").DataType = GetType(Double)
        gridShrink.DisplayLayout.Bands(0).Columns("OldQty").DefaultCellValue = 0

        gridShrink.DisplayLayout.Bands(0).Columns.Add(col_GridOld_WasteType)
        gridShrink.DisplayLayout.Bands(0).Columns(col_GridOld_WasteType).Hidden = True
        gridShrink.DisplayLayout.Bands(0).Columns(col_GridOld_WasteType).DataType = GetType(String)
        gridShrink.DisplayLayout.Bands(0).Columns(col_GridOld_WasteType).DefaultCellValue = String.Empty

        gridShrink.DisplayLayout.Bands(0).Columns.Add("OldSubTeam_No")
        gridShrink.DisplayLayout.Bands(0).Columns("OldSubTeam_No").Hidden = True
        gridShrink.DisplayLayout.Bands(0).Columns("OldSubTeam_No").DataType = GetType(String)
        gridShrink.DisplayLayout.Bands(0).Columns("OldSubTeam_No").DefaultCellValue = String.Empty

        gridShrink.DisplayLayout.Bands(0).Columns.Add(col_GridOld_InvAdjCodeId)
        gridShrink.DisplayLayout.Bands(0).Columns(col_GridOld_InvAdjCodeId).Hidden = True
        gridShrink.DisplayLayout.Bands(0).Columns(col_GridOld_InvAdjCodeId).DataType = GetType(Integer)
        gridShrink.DisplayLayout.Bands(0).Columns(col_GridOld_InvAdjCodeId).DefaultCellValue = 0

        gridShrink.DisplayLayout.Bands(0).Columns.Add(col_GridOld_ShrinkSubtypeId)
        gridShrink.DisplayLayout.Bands(0).Columns(col_GridOld_ShrinkSubtypeId).Hidden = True
        gridShrink.DisplayLayout.Bands(0).Columns(col_GridOld_ShrinkSubtypeId).DataType = GetType(Integer)
        gridShrink.DisplayLayout.Bands(0).Columns(col_GridOld_ShrinkSubtypeId).DefaultCellValue = 0

        gridShrink.DisplayLayout.Bands(0).Columns.Add("Cost")
        gridShrink.DisplayLayout.Bands(0).Columns("Cost").Hidden = False
        gridShrink.DisplayLayout.Bands(0).Columns("Cost").DataType = GetType(Double)
        gridShrink.DisplayLayout.Bands(0).Columns("Cost").DefaultCellValue = 0.0
        gridShrink.DisplayLayout.Bands(0).Columns("Cost").Header.VisiblePosition = 8
        gridShrink.DisplayLayout.Bands(0).Columns("Cost").CellActivation = Activation.NoEdit
        gridShrink.DisplayLayout.Bands(0).Columns("Cost").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
        gridShrink.DisplayLayout.Bands(0).Columns("Cost").Format = "$#,##0.00"

        gridShrink.DisplayLayout.Bands(0).Columns("DateStamp").Format = "MM/dd/yyyy hh:mm:ss.fff tt"

        ' per notes in bug 4328.  Relocated these dropdown inits here to hopefully solve the mysterious
        ' Big Red X(tm) error.  

        'Initilize dropdown for subteam column
        Dim subteamsDropDown As New UltraDropDown
        dtSubTeams = ShrinkCorrectionsDAO.GetSubTeams()
        subteamsDropDown.DataSource = dtSubTeams
        subteamsDropDown.ValueMember = "SubTeam_No"
        subteamsDropDown.DisplayMember = "SubTeam_No"
        gridShrink.DisplayLayout.Bands(0).Columns("SubTeam_No").ValueList = subteamsDropDown

        'Initilize dropdown for waste type column
        dtGridWasteTypes = ShrinkCorrectionsDAO.GetInventoryAdjustmentWasteCodes()
        Dim wasteTypesGridDropdown As New UltraDropDown
        wasteTypesGridDropdown.ValueMember = col_InvAdjCode_Id
        wasteTypesGridDropdown.DisplayMember = col_InvAdjCode_WasteType
        wasteTypesGridDropdown.DataSource = dtGridWasteTypes
        gridShrink.DisplayLayout.Bands(0).Columns(col_Grid_ShrinkType).ValueList = wasteTypesGridDropdown
        wasteTypesGridDropdown.DisplayLayout.Bands(0).Columns(col_InvAdjCode_Id).Hidden = True
        wasteTypesGridDropdown.DisplayLayout.Bands(0).Columns(col_InvAdjCode_WasteType).Hidden = True
        wasteTypesGridDropdown.DisplayLayout.Bands(0).Columns(col_InvAdjCode_Description).Header.Caption = "Shrink Type"
        gridShrink.DisplayLayout.Bands(0).Columns(col_Grid_ShrinkType).Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList

        'Initilize dropdown for shrink subtype column
        dtGridShrinkSubtypes = ShrinkCorrectionsDAO.GetShrinkSubTypesOnly()
        Dim shrinkSubtypesGridDropdown As New UltraDropDown
        shrinkSubtypesGridDropdown.ValueMember = col_ShrinkSubtypes_Id
        shrinkSubtypesGridDropdown.DisplayMember = col_ShrinkSubtypes_Descr
        shrinkSubtypesGridDropdown.DataSource = dtGridShrinkSubtypes
        gridShrink.DisplayLayout.Bands(0).Columns(col_Grid_ShrinkSubtype).ValueList = shrinkSubtypesGridDropdown
        shrinkSubtypesGridDropdown.DisplayLayout.Bands(0).Columns(col_ShrinkSubtypes_Id).Hidden = True
        shrinkSubtypesGridDropdown.DisplayLayout.Bands(0).Columns(col_ShrinkSubtypes_InvAdjCode_Id).Hidden = True
        shrinkSubtypesGridDropdown.DisplayLayout.Bands(0).Columns(col_ShrinkSubtypes_Descr).Header.Caption = "Shrink Subtype"
        gridShrink.DisplayLayout.Bands(0).Columns(col_Grid_ShrinkSubtype).Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList

        If InstanceDataDAO.IsFlagActive("SplitWasteCategory") = False Then
            gridShrink.DisplayLayout.Bands(0).Columns(col_Grid_ShrinkType).Hidden = True
            gridShrink.DisplayLayout.Bands(0).Columns(col_Grid_ShrinkSubtype).Hidden = True
        End If

        'Enable/Disable the columns
        Dim i As Integer
        For i = 0 To gridShrink.DisplayLayout.Bands(0).Columns.Count - 1
            gridShrink.DisplayLayout.Bands(0).Columns(i).CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Next i
        gridShrink.DisplayLayout.Bands(0).Columns("wType").CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit
        gridShrink.DisplayLayout.Bands(0).Columns("Qty").CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit
        gridShrink.DisplayLayout.Bands(0).Columns("SubTeam_No").CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit
        gridShrink.DisplayLayout.Bands(0).Columns(col_Grid_ShrinkSubtype).CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit

        gridShrink.DisplayLayout.Bands(0).Columns("Qty").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right

        'Column captions
        gridShrink.DisplayLayout.Bands(0).Columns("SubTeam_No").Header.Caption = "Shrink" & Environment.NewLine & "Subteam"
        gridShrink.DisplayLayout.Bands(0).Columns("wType").Header.Caption = "Shrink" & Environment.NewLine & "Type"
        gridShrink.DisplayLayout.Bands(0).Columns("UserName").Header.Caption = "Created" & Environment.NewLine & "By"
        gridShrink.DisplayLayout.Bands(0).Columns("Item_Description").Header.Caption = "Item" & Environment.NewLine & "Description"
        gridShrink.DisplayLayout.Bands(0).Columns("Qty").Header.Caption = "Qty"
        gridShrink.DisplayLayout.Bands(0).Columns("Category_Name").Header.Caption = "Category" & Environment.NewLine & "Name"
        gridShrink.DisplayLayout.Bands(0).Columns("Brand_Name").Header.Caption = "Brand" & Environment.NewLine & "Name"
        gridShrink.DisplayLayout.Bands(0).Columns("ItemSubTeam").Header.Caption = "Item" & Environment.NewLine & "SubTeam"
        gridShrink.DisplayLayout.Bands(0).Columns("DateStamp").Header.Caption = "Date" & Environment.NewLine & "Stamp"
        gridShrink.DisplayLayout.Bands(0).Columns(col_Grid_ShrinkSubtype).Header.Caption = "Shrink" & Environment.NewLine & "Subtype"

        're-weight the column widths for a nice display
        gridShrink.DisplayLayout.Bands(0).Columns("Identifier").Width = 60
        gridShrink.DisplayLayout.Bands(0).Columns("Item_Description").Width = 140
        gridShrink.DisplayLayout.Bands(0).Columns("ItemSubTeam").Width = 60
        gridShrink.DisplayLayout.Bands(0).Columns("Category_Name").Width = 80
        gridShrink.DisplayLayout.Bands(0).Columns("Brand_Name").Width = 80
        gridShrink.DisplayLayout.Bands(0).Columns("DateStamp").Width = 120
        gridShrink.DisplayLayout.Bands(0).Columns("SubTeam_No").Width = 60
        gridShrink.DisplayLayout.Bands(0).Columns("Cost").Width = 40
        gridShrink.DisplayLayout.Bands(0).Columns("Qty").Width = 40
        gridShrink.DisplayLayout.Bands(0).Columns(col_Grid_ShrinkType).Width = 40
        gridShrink.DisplayLayout.Bands(0).Columns(col_Grid_ShrinkSubtype).Width = 64
        gridShrink.DisplayLayout.Bands(0).Columns("UserName").Width = 90

        gridShrink.DisplayLayout.Bands(0).ColHeaderLines = 2
        gridShrink.DisplayLayout.Bands(0).Override.ColumnAutoSizeMode = ColumnAutoSizeMode.AllRowsInBand

        'Set various grid properties
        If (gridShrink.DisplayLayout.Bands.Count > 1) Then
            gridShrink.DisplayLayout.Bands(1).Override.CellClickAction = CellClickAction.RowSelect
            gridShrink.DisplayLayout.Bands(1).Override.RowAppearance.BackColor = Color.LightSteelBlue
            gridShrink.DisplayLayout.Bands(1).Override.RowAppearance.ForeColor = Color.Black

            gridShrink.DisplayLayout.Bands(1).Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False
            gridShrink.DisplayLayout.Bands(1).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False

            gridShrink.DisplayLayout.Bands(1).Columns("DateStamp").Hidden = True
            gridShrink.DisplayLayout.Bands(1).Columns("Identifier").Hidden = True
            gridShrink.DisplayLayout.Bands(1).Columns("Item_Description").Hidden = True
            gridShrink.DisplayLayout.Bands(1).Columns("WasteType").Hidden = True

            gridShrink.DisplayLayout.Bands(1).Columns("Insert_Date").CellActivation = Activation.NoEdit
            gridShrink.DisplayLayout.Bands(1).Columns("Insert_Date").Header.Caption = "Insert Date"
            gridShrink.DisplayLayout.Bands(1).Columns("Insert_Date").Format = "MM/dd/yyyy hh:mm:ff tt"
            gridShrink.DisplayLayout.Bands(1).Columns("Insert_Date").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right

            gridShrink.DisplayLayout.Bands(1).Columns("Quantity").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center

            gridShrink.DisplayLayout.Bands(1).CardView = False
            gridShrink.DisplayLayout.Bands(1).Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False

            gridShrink.DisplayLayout.Bands(1).Override.ColumnAutoSizeMode = ColumnAutoSizeMode.AllRowsInBand

        Else
            MsgBox("No shrink records found...", MsgBoxStyle.Information, "Shrink Corrections")
        End If

        gridShrink.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns
    End Sub

    Private Sub gridShrink_InitializeRow(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles gridShrink.InitializeRow

        If e.Row.Band.Index = 1 Then Exit Sub

        ' TFS 23485:: VSTS 20059 Shrink Corrections Bug - Data conversion error for blank quantity field
        '  if a null value somehow exists for the Quantity, do not put the row in the grid
        If IsDBNull(e.Row.Cells("Qty")) Then Exit Sub

        Dim enabledFlag As Boolean = False
        Dim spoilageUser As String = e.Row.Cells("UserName").Value

        If gbSuperUser Or gbUserShrinkAdmin Or gbCoordinator Then
            enabledFlag = True
        ElseIf gsUserName.ToLower = spoilageUser.ToLower Then
            enabledFlag = True
        End If

        ' Check if each row can be edited.
        If enabledFlag Then
            If CanEdit(CDate(e.Row.Cells("DateStamp").Value)) = True Then
                enabledFlag = True
            Else
                enabledFlag = False
            End If
        End If

        If Not enabledFlag Then
            e.Row.Cells("AllowEdit").Value = False
            e.Row.Appearance.BackColor = Color.LightGreen
            e.Row.Appearance.ForeColor = Color.Black
            e.Row.Appearance.AlphaLevel = 75
        Else
            e.Row.Cells("AllowEdit").Value = True
        End If

        e.Row.Cells(col_GridOld_InvAdjCodeId).Value = e.Row.Cells(col_Grid_InvAdjCode_Id).Value
        e.Row.Cells(col_GridOld_WasteType).Value = e.Row.Cells(col_Grid_ShrinkType).Value
        If Not e.Row.Cells(col_Grid_ShrinkSubtype).Value.Equals(DBNull.Value) Then
            e.Row.Cells(col_GridOld_ShrinkSubtypeId).Value = e.Row.Cells(col_Grid_ShrinkSubtype).Value
        End If

        e.Row.Cells("OldQty").Value = CDbl(e.Row.Cells("Qty").Value)
        e.Row.Cells("OldSubTeam_No").Value = e.Row.Cells("SubTeam_No").Value

        e.Row.Cells("OldQty").Value = CDbl(e.Row.Cells("Qty").Value)
        e.Row.Cells("OldSubTeam_No").Value = e.Row.Cells("SubTeam_No").Value

        If IsDBNull(e.Row.Cells("UnitCost").Value) Then
            e.Row.Appearance.BackColor = Color.LightGoldenrodYellow
            e.Row.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True
            e.Row.Cells("Cost").Appearance.BackColor = Color.Red
            e.Row.ToolTipText = "ITEM HAS NO COST VALUE"
        Else
            e.Row.Cells("Cost").Value = e.Row.Cells("UnitCost").Value * e.Row.Cells("Qty").Value
        End If

    End Sub

    Private Sub gridShrink_BeforeCellActivate(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles gridShrink.BeforeCellActivate

        If e.Cell.Band.Index = 1 Then Exit Sub

        If e.Cell.Row.Cells("AllowEdit").Value = False Then
            e.Cancel = True
        End If

    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

        Dim msg As String = String.Empty

        If gridShrink.Selected.Rows.Count < 1 Then
            Exit Sub
        End If

        Dim i As Integer = gridShrink.Selected.Rows(0).Index
        Dim quantity As Decimal
        Dim weight As Decimal
        Dim oldInventoryAdjustmentCodeId As Integer = 0
        Dim oldShrinkSubtypeId As Integer = 0

        If gridShrink.Rows(i).Cells("AllowEdit").Value = True Then
            If (MsgBox("Do you want to delete?", MsgBoxStyle.YesNo, "Shrink Corrections") = MsgBoxResult.No) Then
                Exit Sub
            End If

            Dim OldQty As Decimal = CDbl(gridShrink.Rows(i).Cells("OldQty").Value)
            If gridShrink.Rows(i).Cells(col_GridOld_InvAdjCodeId).Value IsNot Nothing Then
                oldInventoryAdjustmentCodeId = gridShrink.Rows(i).Cells(col_GridOld_InvAdjCodeId).Value
            End If
            Dim OldSubTeam As String = gridShrink.Rows(i).Cells("OldSubTeam_No").Value.ToString
            If gridShrink.Rows(i).Cells(col_GridOld_ShrinkSubtypeId).Value IsNot Nothing Then
                oldShrinkSubtypeId = gridShrink.Rows(i).Cells(col_GridOld_ShrinkSubtypeId).Value
            End If
            Dim identifier As String = gridShrink.Rows(i).Cells("Identifier").Value.ToString
            Dim username As String = gridShrink.Rows(i).Cells("UserName").Value.ToString

            Dim CostedByWeight As Integer = CInt(gridShrink.Rows(i).Cells("CostedbyWeight").Value)

            'TFS 2406 -- Record the negative amount of the origianl quantity to the ItemHistory table so that in the [GetShrinkCorrections]
            'stored procedure, when the quantities is summed up for the shrink item, the qualities for the deleted shrink item will be zero. 
            If CostedByWeight = 0 Then
                quantity = OldQty * (-1)
                weight = 0
            Else
                quantity = 0
                weight = OldQty * (-1)
            End If

            Dim deleteSQLString As String = "EXEC InsertItemHistory3 " _
                    & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex) & ", " _
                    & ShrinkCorrectionsDAO.GetItemKey(identifier) & ", '" _
                    & gridShrink.Rows(i).Cells("OriginalDateStamp").Text & "','" _
                    & CStr(quantity) & "','" _
                    & CStr(weight) & "'," _
                    & oldInventoryAdjustmentCodeId.ToString() & "," _
                    & ShrinkCorrectionsDAO.GetUserID(username) & ", " _
                    & OldSubTeam & "," _
                    & "0," _
                    & oldShrinkSubtypeId.ToString()

            SQLExecute(deleteSQLString, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End If

        cmdSearch.PerformClick()

    End Sub

    Private Function CanEdit(ByVal day As DateTime) As Boolean

        Dim today As DateTime

        ' Fiscal Information for Shrink Date
        Dim shrinkFP As Integer
        Dim shrinkFW As Integer
        Dim shrinkFY As Integer

        ' Fiscal Information for Today's Date
        Dim todayFP As Integer
        Dim todayFW As Integer
        Dim todayFY As Integer
        Dim todayDayOfYear As Integer
        Dim todayDM As Integer

        Dim dtToday As DataTable
        Dim dtShrink As DataTable

        Dim editRow As Boolean = False

        ' Get today's server date instead of local date.  The server date is adjusted for timezone already.
        today = SystemDateTime(True)

        ' Get Fiscal Period Information from Date table in DB for both the Shrink Date and Today's Date
        Try
            dtShrink = ShrinkCorrectionsDAO.GetFiscalCalendarInfo(day.Date)
            dtToday = ShrinkCorrectionsDAO.GetFiscalCalendarInfo(today)

            ' Set Fiscal Period Information for each date for comparison
            If dtShrink.Rows.Count > 0 Then
                Dim drShrink As DataRow
                drShrink = dtShrink.Rows(0)
                shrinkFP = drShrink("FiscalPeriod")
                shrinkFW = drShrink("FiscalWeek")
                shrinkFY = drShrink("FiscalYear")
            End If

            If dtToday.Rows.Count > 0 Then
                Dim drToday As DataRow
                drToday = dtToday.Rows(0)
                todayFP = drToday("FiscalPeriod")
                todayFW = drToday("FiscalWeek")
                todayFY = drToday("FiscalYear")
                todayDayOfYear = drToday("Day_Of_Year")
                todayDM = drToday("Day_Of_Month")
            End If

        Catch ex As Exception
            logger.Info(String.Format("There was an exception for ShrinkCorrectionsDAO.GetFiscalCalendarInfo() funtion: {0}", ex.InnerException))
        End Try

        ' If Today's Date and the Shrink Date are in the same Fiscal Period, then Shrink can be edited
        ' If Today's Date is first day of the month/period, Shrink in the previous period can be edited
        ' Account for situations where fiscal data did not return valid values from DB and make sure it can't be edited (e.g. 0 or Nothing)
        ' Also handle first day of the fiscal year so that shrink from the previous FP can be edited.
        If (todayFY - shrinkFY = 1) And todayDayOfYear = 1 And shrinkFP = 12 Then
            editRow = True

        ElseIf (todayFY = shrinkFY) And (todayFY <> 0 Or todayFY <> Nothing Or shrinkFY <> 0 Or shrinkFY <> Nothing) Then
            If (todayFP = shrinkFP) Then
                editRow = True

            ElseIf (todayFP - shrinkFP) = 1 Then
                If todayDM = 1 Then
                    editRow = True
                End If

            End If
        End If

        Return editRow

    End Function

    Private Sub gridShrink_AfterSelectChange(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles gridShrink.AfterSelectChange

        If (e.Type.Name <> "UltraGridRow") Then Exit Sub

        If gridShrink.Selected.Rows.Count < 1 Then
            Exit Sub
        End If

        If gridShrink.Selected.Rows(0).Band.Index = 1 Then Exit Sub

        If gridShrink.Selected.Rows(0).Cells("AllowEdit").Value = False Then
            cmdDelete.Enabled = False
        Else
            cmdDelete.Enabled = True
        End If

    End Sub
    Public Function IsValidSubTeam(ByRef lSubTeam_No As Long) As Boolean
        Dim i As Integer

        For i = 0 To cmbSubTeam.Items.Count - 1
            If VB6.GetItemData(cmbSubTeam, i) = lSubTeam_No Then
                Return True
            End If
        Next i

        Return False
    End Function

    Private Sub gridShrink_BeforeCellUpdate(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs) Handles gridShrink.BeforeCellUpdate

        If e.Cell.Band.Index = 1 Then Exit Sub

        If e.Cell.Column.Key = "Qty" Then
            ' TFS 23485:: VSTS 20059 Shrink Corrections Bug - Data conversion error for blank quantity field
            '  do not allow user to enter a blank (null) or non-numeric (text) value for the quantity
            If IsDBNull(e.NewValue) Or Not IsNumeric(e.NewValue) Then
                MsgBox("Quantity cannot be blank (must be a number greater than 0)", MsgBoxStyle.Exclamation, "Shrink Corrections")
                e.Cancel = True
            Else
                Dim quantity As Decimal = CDbl(e.NewValue)
                If quantity < 0 Then
                    MsgBox("Quantity must be greater than 0", MsgBoxStyle.Exclamation, "Shrink Corrections")
                    e.Cancel = True
                End If
            End If
        ElseIf e.Cell.Column.Key = "SubTeam_No" Then
            Dim subteam_no As Long = CLng(e.NewValue)

            If Not IsValidSubTeam(subteam_no) Then
                MsgBox("Invalid SubTeam No [" + CStr(subteam_no) + "]", MsgBoxStyle.Exclamation, "Shrink Corrections")
                e.Cancel = True
            End If
        ElseIf e.Cell.Column.Key = col_Grid_ShrinkType Then
            Dim shrink_type_id As Integer = e.NewValue

            'If shrink_type <> "SP" And shrink_type <> "FB" And shrink_type <> "SM" Then
            If Not validShrinkTypeIds.Contains(shrink_type_id) Then
                MsgBox("Invalid Shrink Type", MsgBoxStyle.Exclamation, "Shrink Corrections")
                e.Cancel = True
            End If
        ElseIf e.Cell.Column.Key = col_Grid_ShrinkSubtype Then
            Dim shrink_subtype_id As Integer = e.NewValue

            If Not validShrinkSubTypeIds.Contains(shrink_subtype_id) Then
                MsgBox("Invalid Shrink SubType", MsgBoxStyle.Exclamation, "Shrink Corrections")
                e.Cancel = True
            End If
        End If

    End Sub
    '******************************************************************************************************
    ' Function: cmdExcelExport_Click()
    '   Author: Ron Savage
    '     Date: 07/05/2011
    ' 
    ' Description:
    ' This function handles the export to excel button on the bottom of the form, with a default filename
    ' built from the input parameters.
    '******************************************************************************************************
    Private Sub cmdExcelExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExcelExport.Click
        Dim exportFile As String = ""
        Dim fileDlg As New SaveFileDialog
        Dim typeText As String = Me.cmbShrinkType.SelectedItem.ToString.Trim()
        Dim storeText As String = Me.cmbStore.SelectedItem.ToString.Trim()
        Dim subTeamText As String = Me.cmbSubTeam.SelectedItem.ToString.Trim()
        Dim startDate As String = Me.mskStartDate.Text.Replace("/", "_")
        Dim endDate As String = Me.mskEndDate.Text.Replace("/", "_")

        exportFile = storeText + "_" + subTeamText + "_" + typeText + "_" + startDate + "_to_" + endDate + ".xls"
        exportFile = exportFile.Replace(" ", "_").Trim()

        fileDlg.CheckPathExists = True
        fileDlg.FileName = exportFile

        If (fileDlg.ShowDialog = Windows.Forms.DialogResult.OK) Then
            exportFile = fileDlg.FileName

            Me.UltraGridExcelExporter1.Export(gridShrink, exportFile)
        End If

    End Sub

    Private Sub gridShrink_Resize(sender As Object, e As EventArgs) Handles gridShrink.Resize
        gridShrink.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns
        'gridShrink.DisplayLayout.PerformAutoResizeColumns(False, False)
    End Sub
End Class
