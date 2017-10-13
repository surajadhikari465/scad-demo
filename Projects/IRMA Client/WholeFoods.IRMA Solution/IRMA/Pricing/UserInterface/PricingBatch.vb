Option Strict Off
Option Explicit On
Imports System.Linq
Imports Infragistics.Win.UltraWinGrid
Imports log4net
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports WholeFoods.IRMA.Pricing.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Friend Class frmPricingBatch
    Inherits System.Windows.Forms.Form

    Private mdt As DataTable
    Private mdv As DataView
    Private noTagItemsExcluded As Boolean

    Dim dtStores As DataTable = Nothing
    Dim mrsBatch As ADODB.Recordset
    Dim msStores As String
    Dim msState As String
    Dim msItemChgTypeID As String
    Dim msPriceChgTypeID As String
    Dim msPriceBatchStatusID As String
    Dim msSubTeam_No As String
    Dim msFromStartDate As String
    Dim msToStartDate As String
    Dim msIdentifier As String
    Dim msItem_Description As String
    Dim m_bGridCtrlKey As Boolean
    Dim msAutoApplyFlag As String
    Dim IsInitializing As Boolean
    Dim _operationSucceededWhileIgnoringNoTagLogic As Boolean

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public ReadOnly Property IgnoreNoTagLogic() As Boolean
        Get
            Return chkIgnoreNoTagLogic.Checked
        End Get
    End Property

    Private Function ConfirmThatUserWantsToIgnoreNoTagLogic() As Boolean
        'reset the flag
        _operationSucceededWhileIgnoringNoTagLogic = False
        'we are turning on the Ignore option - pop up a warning to confirm
        Dim confirmed As DialogResult = MsgBox(ResourcesIRMA.GetString("BypassNoTagLogicWarning"), MsgBoxStyle.YesNo, "Confirm Bypassing No-Tag Logic")
        Return (confirmed = DialogResult.Yes)
    End Function

    Private Sub NotifyIfNoTagLogicWasIgnored()
        'were we actively ignoring no-tag logic & were all batches that needed to be Sent and/or Printed?
        If (IgnoreNoTagLogic And _operationSucceededWhileIgnoringNoTagLogic) Then
            'notify user that batch(es) were processed while the option was active
            MsgBox(ResourcesIRMA.GetString("BypassNoTagLogicInformationMessage"), MsgBoxStyle.Information, Me.Text)
            'reset the flag
            _operationSucceededWhileIgnoringNoTagLogic = False
            ' make sure the checkbox is unchecked 
            If chkIgnoreNoTagLogic.Checked Then
                chkIgnoreNoTagLogic.Checked = False
            End If
        End If
    End Sub

    Private Sub chkIgnoreNoTagLogic_CheckedChanged(sender As Object, e As EventArgs) Handles chkIgnoreNoTagLogic.CheckedChanged
        If TypeOf sender Is CheckBox Then
            Dim chkBox As CheckBox = DirectCast(sender, CheckBox)
            ' make sure user confirms turning this option on
            If (chkBox.Checked) Then
                chkBox.Checked = ConfirmThatUserWantsToIgnoreNoTagLogic()
                'Else
                '    chkBox.Checked = StopIgnoringNoTagLogic()
            End If

            'If (chkBox.Checked) Then
            '    'pop up a warning to confirm
            '    Dim confirmed As DialogResult = MsgBox(ResourcesIRMA.GetString("BypassNoTagLogicWarning"), MsgBoxStyle.YesNo, "Confirm Bypassing No-Tag Logic")
            '    If (confirmed = DialogResult.No) Then
            '        chkBox.Checked = False
            '    End If
            'End If

            ' IgnoreNoTagLogic = chkBox.Checked
        End If
    End Sub

    Private Sub frmPricingBatch_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmPricingBatch_Load Entry")

        Dim sPrevZone As String = String.Empty
        Dim sPrevState As String = String.Empty
        Dim i As Short

        CenterForm(Me)

        dtpStartDate.Value = String.Empty
        dtpEndDate.Value = String.Empty

        logger.Debug("EXEC GetRetailStores")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        Try
            dtStores = factory.GetStoredProcedureDataTable("GetRetailStores")

            ucmbStoreList.DataSource = dtStores
            ucmbStoreList.DisplayMember = "StoreAbbr"
            ucmbStoreList.ValueMember = "Store_No"

            SetStoreListCombo(ucmbStoreList)
        Catch e As DataFactoryException
            ErrorHandler.ProcessError(WholeFoods.Utility.ErrorType.DataFactoryException, SeverityLevel.Warning, e)
        End Try

        logger.Debug("EXEC GetRetailStores Done")

        If dtStores.Rows.Count > 0 Then
            Dim distinctZoneTable As DataTable = dtStores.DefaultView.ToTable(True, "Zone_id", "Zone_Name")

            For Each r As DataRow In distinctZoneTable.Rows
                cmbZones.Items.Add(New VB6.ListBoxItem(r("Zone_Name").ToString(), r("Zone_ID").ToString()))
            Next

            cmbZones.SelectedIndex = -1

            Dim distinctStateTable As DataTable = dtStores.DefaultView.ToTable(True, "State")

            For Each r As DataRow In distinctStateTable.Rows
                cmbState.Items.Add(New VB6.ListBoxItem(r("State").ToString()))
            Next

            cmbState.SelectedIndex = -1
        End If

        LoadPriceTypeCombo()
        LoadSubTeam(cmbSubTeam)

        Try
            logger.Debug("EXEC GetPriceBatchStatusList")

            gRSRecordset = SQLOpenRecordSet("EXEC GetPriceBatchStatusList ", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            Do While Not gRSRecordset.EOF
                cmbStatus.Items.Add(New VB6.ListBoxItem(gRSRecordset.Fields("PriceBatchStatusDesc").Value, gRSRecordset.Fields("PriceBatchStatusID").Value))
                gRSRecordset.MoveNext()
            Loop

            logger.Debug("EXEC GetPriceBatchStatusList Done")
        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

        If glStore_Limit > 0 Then
            SetActive(ucmbStoreList, False)

            SetCombo(ucmbStoreList, glStore_Limit)

            For i = 1 To optSelection.Count - 1
                optSelection(i).Enabled = False
            Next

            optSelection.Item(0).Checked = True
        Else
            ucmbStoreList.SelectedRow = Nothing
        End If

        SetActive(cmbZones, False)
        SetActive(cmbState, False)
        SetActive(ucmbStoreList, False)

        If Not CheckAllStoreSelectionEnabled() Then
            _optSelection_5.Visible = True
            _optSelection_5.Checked = True
        End If

        _optType_5.Checked = True

        'default button loading display
        'hide button panel that contains all maintenance buttons, except for the 'Add Batch' and 'Reports' button
        fraProcess.Left = 0

        cmdMaintain(0).Visible = gbBatchBuildOnly Or gbItemAdministrator Or gbPriceBatchProcessor Or gbSuperUser 'plus icon
        cmdMaintain(1).Visible = False 'edit/view batch
        cmdMaintain(2).Visible = False 'delete batch
        cmdMaintain(3).Visible = False 'remove all batch details
        cmdMaintain(4).Visible = True  'reports
        cmdMaintain(5).Visible = False 'STATUS: ready for stores
        cmdMaintain(6).Visible = False 'STATUS: package
        cmdMaintain(7).Visible = False 'STATUS: print tags

        'hide button panel that contains Pring Shelf Tags & Apply Batches buttons
        fraProcess.Visible = False

        logger.Debug("frmPricingBatch_Load Exit")
    End Sub

    ''' <summary>
    ''' Sets the layout of the Store combo box
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetStoreListCombo(ByRef ultraCombo As Infragistics.Win.UltraWinGrid.UltraCombo)
        logger.Debug("SetStoreListCombo Entry")

        'Add an additional unbound column to WinCombo. 
        'This will be used for the Selection of each Item 
        Dim c As UltraGridColumn = ultraCombo.DisplayLayout.Bands(0).Columns.Add()

        c.Key = "Selected"
        c.Header.Caption = String.Empty

        'This allows end users to select / unselect ALL items 
        c.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always

        c.DataType = GetType(Boolean)

        'Move the checkbox column to the first position. 
        c.Header.VisiblePosition = 0
        ultraCombo.CheckedListSettings.CheckStateMember = "Selected"
        ultraCombo.CheckedListSettings.EditorValueSource = Infragistics.Win.EditorWithComboValueSource.CheckedItems

        ' Set up the control to use a custom list delimiter 
        ultraCombo.CheckedListSettings.ListSeparator = "; "

        ' Set ItemCheckArea to Item, so that clicking directly on an item also checks the item
        ultraCombo.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item

        ultraCombo.DisplayLayout.Bands(0).Columns("Zone_id").Hidden = True
        ultraCombo.DisplayLayout.Bands(0).Columns("Zone_Name").Hidden = True
        ultraCombo.DisplayLayout.Bands(0).Columns("Store_No").Hidden = True
        ultraCombo.DisplayLayout.Bands(0).Columns("Mega_Store").Hidden = True
        ultraCombo.DisplayLayout.Bands(0).Columns("WFM_Store").Hidden = True
        ultraCombo.DisplayLayout.Bands(0).Columns("State").Hidden = True
        ultraCombo.DisplayLayout.Bands(0).Columns("CustomerType").Hidden = True
        ultraCombo.DisplayLayout.Bands(0).Columns("BusinessUnit_ID").Hidden = True
        ultraCombo.DisplayLayout.Bands(0).Columns("BusinessUnit_ID").Hidden = True
        ultraCombo.DisplayLayout.Bands(0).Columns("StoreAbbr").Width = 60
        ultraCombo.DisplayLayout.Bands(0).Columns("Store_Name").Width = 200

        logger.Debug("SetStoreListCombo Exit")
    End Sub

    ''' <summary>
    ''' loads data into the price change type drop down
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadPriceTypeCombo()
        logger.Debug("LoadPriceTypeCombo Entry")

        Dim priceChgDAO As New PriceChgTypeDAO
        Dim priceChgList As ArrayList = PriceChgTypeDAO.GetPriceChgTypeList(True, True)

        With cmbPriceType
            .DisplayMember = "PriceChgTypeDesc"
            .ValueMember = "PriceChgTypeID"
            .DataSource = priceChgList
        End With

        logger.Debug("LoadPriceTypeCombo Exit")
    End Sub

    ''' <summary>
    ''' builds PriceBatchSearchBO object that sets property values for current batch search
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function BuildSearchItem() As PriceBatchSearchBO
        logger.Debug("BuildSearchItem Entry")

        Dim searchItem As New PriceBatchSearchBO
        searchItem.StoreList = msStores
        searchItem.StoreListSeparator = "|"

        If msItemChgTypeID IsNot Nothing AndAlso Not msItemChgTypeID.Equals("") Then
            searchItem.ItemChgTypeID = msItemChgTypeID
        End If
        If msPriceChgTypeID IsNot Nothing AndAlso Not msPriceChgTypeID.Equals("") Then
            searchItem.PriceChgTypeID = msPriceChgTypeID
        End If
        If msPriceBatchStatusID IsNot Nothing AndAlso Not msPriceBatchStatusID.Equals("") AndAlso Not msPriceBatchStatusID.Equals("NULL") Then
            searchItem.PriceBatchStatusID = msPriceBatchStatusID
        End If
        If msSubTeam_No IsNot Nothing AndAlso Not msSubTeam_No.Equals("") AndAlso Not msSubTeam_No.Equals("NULL") Then
            searchItem.SubTeamNo = msSubTeam_No
        End If
        If msFromStartDate IsNot Nothing AndAlso Not msFromStartDate.Equals("") AndAlso Not msFromStartDate.Equals("NULL") Then
            searchItem.StartDate = Convert.ToDateTime(msFromStartDate)
        Else
            searchItem.StartDate = Nothing
        End If
        If msToStartDate IsNot Nothing AndAlso Not msToStartDate.Equals("") AndAlso Not msToStartDate.Equals("NULL") Then
            searchItem.EndDate = Convert.ToDateTime(msToStartDate)
        Else
            searchItem.EndDate = Nothing
        End If
        If msIdentifier IsNot Nothing AndAlso Not msIdentifier.Equals("") Then
            searchItem.Identifier = msIdentifier
        End If
        If msItem_Description IsNot Nothing AndAlso Not msItem_Description.Equals("") Then
            searchItem.ItemDescription = msItem_Description
        End If

        searchItem.AutoApplyFlag = msAutoApplyFlag
        searchItem.AutoApplyDate = AutoApplyDateUDTE.Value
        searchItem.BatchDescription = ConvertQuotes(BatchDescriptionTextBox.Text)

        logger.Debug("BuildSearchItem Exit")

        Return searchItem
    End Function

    Private Sub LoadDataTable(ByVal searchItem As PriceBatchSearchBO)
        logger.Debug("LoadDataTable Enter")

        If Len(msStores) = 0 Then
            MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        Cursor.Current = Cursors.WaitCursor

        mdv = New System.Data.DataView(PriceBatchSearchDAO.GetHeaderSearchData(searchItem, Me.Text))
        ugrdList.DataSource = mdv

        'This may or may not be required.
        If mdv.Count > 0 Then
            'Set the first item to selected.
            ugrdList.Rows(0).Selected = True

            'display appropriate buttons given the search results
            DisplayButtons()

            'update row color if it has an invalid date
            MarkRowsWithInvalidDate(ugrdList, ResourcesPricing.MaxSmalldatetimeDate)
        Else
            MsgBox(ResourcesIRMA.GetString("None Found"), MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
        End If

ExitSub:
        logger.Debug("LoadDataTable Exit")
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
    End Sub

    ''' <summary>   
    ''' Updates the UltraGrid to visually indicate if any of the rows (batches) has any date fields with a value greater than the max allowed
    ''' </summary>
    ''' <param name="ultraGrid">UltraGrid object to be updated</param>
    ''' <param name="maxDate">the maximum allowable date value</param>
    ''' <remarks></remarks>
    Private Sub MarkRowsWithInvalidDate(ultraGrid As UltraGrid, maxDate As Date)
        For i As Integer = 0 To ultraGrid.Rows.Count - 1
            Dim errorText = DoesBatchRowHaveInvalidDate(ultraGrid.Rows(i), maxDate)
            If errorText IsNot String.Empty Then
                ultraGrid.Rows(i).Appearance.BackColor = Color.Orange
                ultraGrid.Rows(i).ToolTipText = errorText
            End If
        Next
    End Sub

    ''' <summary>   
    ''' Checks whether UltraGrid row representing a batch has an Apply, Start or SaleEnd date greater than the max
    ''' </summary>
    ''' <param name="row">UltraGrid row object to be inspected</param>
    ''' <param name="maxDate">the maximum allowable date value</param>
    ''' <returns>String.Empty if dates are valid or String with error information if a date is invalid </returns>
    ''' <remarks></remarks>
    Private Function DoesBatchRowHaveInvalidDate(row As UltraGridRow, maxDate As Date) As String

        Dim invalidDateError As String = String.Empty

        If CDate(row.Cells("StartDate").Value) > maxDate _
            Or CDate(row.Cells("ApplyDate").Value) > maxDate _
            Or (Not IsDBNull(row.Cells("SaleEndDate").Value) AndAlso CDate(row.Cells("SaleEndDate").Value) > maxDate) Then

            invalidDateError = row.Cells("BatchDescription").Value & " has an invalid "

            If CDate(row.Cells("StartDate").Value) > maxDate Then
                invalidDateError &= " StartDate"
            End If
            If CDate(row.Cells("ApplyDate").Value) > maxDate Then
                If CDate(row.Cells("StartDate").Value) > maxDate Then
                    invalidDateError &= " &"
                End If
                invalidDateError &= " ApplyDate"
            End If
            If Not IsDBNull(row.Cells("SaleEndDate").Value) AndAlso CDate(row.Cells("SaleEndDate").Value) > maxDate Then
                If CDate(row.Cells("StartDate").Value) > maxDate Or CDate(row.Cells("ApplyDate").Value) > maxDate Then
                    invalidDateError &= " &"
                End If
                invalidDateError &= " SaleEndDate"
            End If
            invalidDateError &= "."
            invalidDateError &= " Date cannot be after " & maxDate
        End If

        Return invalidDateError
    End Function

    ''' <summary>
    ''' buttons are displayed based on regional and/or store override settings found in the InstanceDataFlags/InstanceDataFlagsStoreOverride tables
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayButtons()
        logger.Debug("DisplayButtons Enter")

        'display "maintenance" group buttons
        cmdMaintain(1).Visible = gbBatchBuildOnly Or gbItemAdministrator Or gbPriceBatchProcessor Or gbSuperUser 'edit/view batch
        cmdMaintain(2).Visible = gbBatchBuildOnly Or gbItemAdministrator Or gbPriceBatchProcessor Or gbSuperUser 'delete batch
        cmdMaintain(3).Visible = gbBatchBuildOnly Or gbItemAdministrator Or gbPriceBatchProcessor Or gbSuperUser 'remove all batch details
        cmdMaintain(4).Visible = True 'reports
        cmdMaintain(5).Visible = gbPriceBatchProcessor Or gbSuperUser 'STATUS: ready for stores
        cmdMaintain(6).Visible = gbPriceBatchProcessor Or gbSuperUser 'STATUS: package
        cmdMaintain(7).Visible = gbPriceBatchProcessor Or gbSuperUser  'STATUS: print tags

        'display the print and apply buttons by default.  the logic below will hide them in needed
        cmdProcess(0).Visible = True  'print shelf tags
        cmdProcess(1).Visible = True  'apply batches

        If gbItemAdministrator Then
            fraMaintain.Visible = True
        Else
            fraProcess.Left = 0
            fraMaintain.Visible = False
        End If

        If gbPriceBatchProcessor Then
            fraProcess.Visible = True

            If gbItemAdministrator Then
                cmdMaintain(7).Visible = False
                fraMaintain.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdMaintain(0).Width) * cmdMaintain.Count)
                fraProcess.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(fraMaintain.Left) + VB6.PixelsToTwipsX(fraMaintain.Width))
            End If

            If Not IsDisplayButton("ApplyBatches") Then
                'hide just 'Apply Batches' button
                cmdProcess(1).Visible = False

                'turn off the "Mark All As Printed" button since the print tags button is now enabled.  Leaving the button in just in case it's needed later.
                cmdProcess(2).Visible = False
            End If
        Else
            'hide button panel that contains Pring Shelf Tags & Apply Batches buttons
            fraProcess.Visible = False

            If Not IsDisplayButton("PrintShelfTags") Then
                ' Also hide the Ready for Stores button because it will invoke Print Shelf Tags, which is hidden from this user role;
                ' This will prevent allowing batches to be put into a status that no user can get it out of.
                cmdMaintain(5).Visible = False

                If gbBatchBuildOnly Then
                    cmdMaintain(6).Visible = False
                End If
            End If

            'no need to check for 'Apply Changes' button because users that fall in this block do not have access to that button
        End If
        logger.Debug("DisplayButtons Exit")
    End Sub

    ''' <summary>
    ''' checks the BypassPrintShelfTags and BypassApplyChanges columns in the data grid for the '*' value;
    ''' this value is used to indicate if the corresponding button should be displayed to the user in the button strip below the grid
    ''' </summary>
    ''' <param name="columnName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsDisplayButton(ByVal columnName As String) As Boolean
        Dim display As Boolean
        Dim currentRowIndex As Integer
        logger.Debug("IsDisplayButton Enter")
        'loop through results and look for a true value in the appropriate column;
        'true value indicated by a '*'
        For currentRowIndex = 0 To ugrdList.Rows.Count - 1
            If ugrdList.Rows(currentRowIndex).Cells(columnName).Value.GetType IsNot GetType(DBNull) AndAlso
                ugrdList.Rows(currentRowIndex).Cells(columnName).Value = "*" Then
                display = True
                Exit For
            End If
        Next
        logger.Debug("IsDisplayButton Exit")
        Return display
    End Function

    Public Function GetSubTeamName(ByRef lSubTeam_No As Long) As String
        Dim iLoop As Integer
        Dim sSubTeam As String
        sSubTeam = String.Empty
        logger.Debug("GetSubTeamName Enter")
        For iLoop = 0 To cmbSubTeam.Items.Count - 1
            If VB6.GetItemData(cmbSubTeam, iLoop) = lSubTeam_No Then
                sSubTeam = VB6.GetItemString(cmbSubTeam, iLoop)
                Exit For
            End If
        Next iLoop
        GetSubTeamName = sSubTeam
        logger.Debug("IsDisplayButton Exit")
    End Function

    Public Sub PopulateBatchStatusDropDown(ByRef cmb As System.Windows.Forms.ComboBox)
        Dim iLoop As Short
        logger.Debug("PopulateBatchStatusDropDown Enter")
        cmb.Items.Clear()
        For iLoop = 0 To cmbStatus.Items.Count - 1
            cmb.Items.Add(New VB6.ListBoxItem(VB6.GetItemString(cmbStatus, iLoop), VB6.GetItemData(cmbStatus, iLoop)))
        Next iLoop
        logger.Debug("PopulateBatchStatusDropDown Exit")
    End Sub

    Private Sub cmbStatus_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbStatus.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        logger.Debug("cmbStatus_KeyPress Enter")
        If KeyAscii = 8 Then
            cmbStatus.SelectedIndex = -1
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        logger.Debug("cmbStatus_KeyPress Exit")
    End Sub

    Private Sub cmbSubTeam_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress
        logger.Debug("cmbSubTeam_KeyPress Enter")
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        If KeyAscii = 8 Then
            cmbSubTeam.SelectedIndex = -1
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        logger.Debug("cmbSubTeam_KeyPress Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdMaintain_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMaintain.Click
        logger.Debug("cmdMaintain_Click Enter")

        Dim Index As Short = cmdMaintain.GetIndex(eventSender)
        Dim nTotalSelRows As Short
        Dim i As Short
        Dim bWrongStatus As Boolean
        Dim bRefresh As Boolean
        Dim headerDAO As New PriceBatchHeaderDAO
        Dim header As New PriceBatchHeaderBO
        Dim newStatusDesc As String = String.Empty
        Dim doSearch As Boolean
        Dim selectAllInGrid As Boolean = chkSelectAll.Checked
        Dim storeNumber As Integer
        Dim chgType As String = String.Empty

        Select Case Index
            Case 0 'Add batch
                logger.Info("Processing Add Batch command")
                frmPricingBatchItemSearch.ShowDialog()
                frmPricingBatchItemSearch.Dispose()
            Case 1 'Edit batch
                logger.Info("Processing Edit Batch command")

                If ugrdList.Selected.Rows.Count <> 1 Then
                    MsgBox(ResourcesIRMA.GetString("SelectSingleRow"), MsgBoxStyle.Information, Me.Text)
                    Exit Sub
                Else
                    ' populate the PriceBatchHeaderBO for the selected batch and display the details form
                    header = New PriceBatchHeaderBO
                    header.PopulateFromSelectedRow(ugrdList.Selected, 0)
                    frmPricingBatchDetail.BatchHeader = header
                    frmPricingBatchDetail.SetBatchHeaderInfo(True)
                    frmPricingBatchDetail.IgnoreNoTagLogic = IgnoreNoTagLogic

                    frmPricingBatchDetail.ShowDialog()
                    doSearch = frmPricingBatchDetail.IsBatchInfoRefreshed
                    _operationSucceededWhileIgnoringNoTagLogic = frmPricingBatchDetail.NoTagLogicWasIgnored
                    frmPricingBatchDetail.Dispose()
                    ' Only do automatic search if the user changed the header info
                    If doSearch Then Call LoadDataTable(BuildSearchItem())
                End If
            Case 2, 3 'Delete or Remove batch
                If Index = 2 Then
                    logger.Info("Processing Delete Batch command")
                Else
                    logger.Info("Processing Remove All Batch Details command")
                End If

                nTotalSelRows = ugrdList.Selected.Rows.Count

                If nTotalSelRows <= 0 Then
                    logger.Info(ResourcesIRMA.GetString("MustSelect"))
                    MsgBox(ResourcesIRMA.GetString("MustSelect"), MsgBoxStyle.Information, Me.Text)
                    Exit Sub
                Else
                    Select Case Index
                        Case 2
                            logger.Info(ResourcesIRMA.GetString("DeleteBatches"))
                            If MsgBox(ResourcesPricing.GetString("DeleteBatches"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then Exit Sub
                        Case 3
                            logger.Info(ResourcesIRMA.GetString("RemoveDetail"))
                            If MsgBox(ResourcesPricing.GetString("RemoveDetail"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then Exit Sub
                    End Select
                End If

                Dim pbhIds As String = String.Empty
                Dim deletablePbhIds As String = String.Empty

                For i = 0 To nTotalSelRows - 1

                    If ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value < 3 Then
                        bRefresh = True
                        Select Case Index
                            Case 2
                                pbhIds = pbhIds + "," + ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value.ToString()

                                'logger.Info("EXEC DeletePriceBatch Start: PriceBatchHeaderID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value.ToString())
                                'SQLExecute("EXEC DeletePriceBatch " & ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                                'logger.Info("EXEC DeletePriceBatch End")
                            Case 3
                                logger.Info("EXEC DeletePriceBatchCutDetail Start: PriceBatchHeaderID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value.ToString())
                                SQLExecute("EXEC DeletePriceBatchCutDetail " & ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                                logger.Info("EXEC DeletePriceBatchCutDetail End")
                        End Select
                    Else
                        logger.Info("Unable to delete or remove selected batch, wrong state: PriceBatchHeaderID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value.ToString() + ", PriceBatchStatusID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value.ToString())
                        bWrongStatus = True
                    End If
                Next

                If Len(pbhIds) > 0 Then
                    pbhIds = pbhIds.Substring(1)
                    deletablePbhIds = GetDeletablePBHIds(pbhIds)

                    If Len(deletablePbhIds) > 0 Then
                        logger.Info("EXEC DeletePriceBatch Start: PriceBatchHeaderID in " + deletablePbhIds)
                        SQLExecute("EXEC DeletePriceBatch '" & deletablePbhIds + "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        logger.Info("EXEC DeletePriceBatch End")
                    End If

                    If Len(pbhIds) > Len(deletablePbhIds) Then
                        MsgBox(ResourcesPricing.GetString("BatchesCannotBeDeleted"), MsgBoxStyle.Information, Me.Text)
                    End If
                End If

                If bRefresh Then
                    logger.Info("LoadDataTable Start")
                    Call LoadDataTable(BuildSearchItem())
                    logger.Info("LoadDataTable End")
                End If

                If bWrongStatus Then
                    logger.Info("BatchesWrongStatus")
                    MsgBox(ResourcesPricing.GetString("BatchesWrongStatus"), MsgBoxStyle.Information, Me.Text)
                End If

            Case 4 'Reports
                logger.Info("Processing Reports command")
                frmPricingBatchReports.ShowDialog()

                'frmPricingBatchReports = Nothing
                frmPricingBatchReports.Dispose()

            Case 5 'Ready
                logger.Info("Processing Ready for Stores command")

                noTagItemsExcluded = False

                ' -- SET PRICE BATCH STATUS = READY
                nTotalSelRows = ugrdList.Selected.Rows.Count

                If nTotalSelRows <= 0 Then
                    logger.Info(ResourcesIRMA.GetString("MustSelect"))
                    MsgBox(ResourcesIRMA.GetString("MustSelect"), MsgBoxStyle.Information, Me.Text)
                    Exit Sub
                End If

                For i = 0 To nTotalSelRows - 1
                    If ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value = "2" Then
                        'set header id and new status id
                        header = New PriceBatchHeaderBO
                        header.PriceBatchHeaderId = ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value
                        header.PriceBatchStatusID = 3

                        'update PriceBatchHeader.PriceBatchStatusID
                        logger.Info("Updating batch status to READY: PriceBatchHeaderID=" + header.PriceBatchHeaderId.ToString)
                        newStatusDesc = headerDAO.UpdatePriceBatchStatus(header)

                        ugrdList.Selected.Rows(i).Cells("PriceBatchStatusDesc").Value = newStatusDesc
                        ' ******* If a delete batch, the status will be automatically updated to Printed ********
                        If newStatusDesc = "Printed" And Not ugrdList.Selected.Rows(i).Cells("PrintShelfTags").Value = "" Then
                            header.PriceBatchStatusID = 4
                            ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value = 4
                        Else
                            ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value = 3
                        End If

                        'check InstanceData to see if next batching status step should be skipped: Print Shelf Tags;
                        'the column PrintShelfTags will have a null value if step is to be bypassed
                        If ugrdList.Selected.Rows(i).Cells("PrintShelfTags").Value = String.Empty Then
                            'bypass printing tags: perform print shelf tag action for user
                            logger.Info("Auto-processing Print Shelf Tags action for the user: PriceBatchHeaderID=" + header.PriceBatchHeaderId.ToString)

                            If ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value = "3" AndAlso gbPriceBatchProcessor Then
                                storeNumber = CInt(ugrdList.Selected.Rows(i).Cells("Store_No").Value)
                                chgType = ugrdList.Selected.Rows(i).Cells("ItemChgTypeDesc").Value.ToString().ToUpper()
                                'although this step is being bypassed, steps may need to be performed to
                                'perform the actual printing of shelf tags
                                If InstanceDataDAO.IsFlagActiveCached("BypassPrintShelfTags_PerformPrintLogic") _
                                    AndAlso
                                    (
                                       Not (chgType = "DELETE" _
                                            Or InstanceDataDAO.IsFlagActive("GlobalPriceManagement", storeNumber)
                                        ) _
                                         Or chgType = "DELETE"
                                    ) Then
                                    logger.Debug("PerformPrintLogic Start: PriceBatchHeaderID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value.ToString + ", BypassPrintShelfTags_PerformPrintLogic= TRUE")
                                    PerformPrintLogic(rowIndex:=i, isReprint:=False)
                                    logger.Debug("PerformPrintLogic End")
                                Else
                                    logger.Info("Store Number:" + storeNumber.ToString() + " is on GPM thus PrintBatches will not be sent to SLAW(" + "PriceBatchHeaderID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value.ToString + ")")
                                End If

                                'set new status id
                                header.PriceBatchStatusID = 4

                                'update PriceBatchHeader.PriceBatchStatusID
                                logger.Info("Updating batch status to PRINTED: PriceBatchHeaderID =" + header.PriceBatchHeaderId.ToString)

                                newStatusDesc = headerDAO.UpdatePriceBatchStatus(header)

                                ugrdList.Selected.Rows(i).Cells("PriceBatchStatusDesc").Value = newStatusDesc
                                ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value = 4

                                'NEXT CHECK --> (can only bypass to 'Sent' status if the status is currently in 'Printed' status
                                'check InstanceData to see if next batching status step should be skipped: Apply Batches;
                                'the column ApplyBatches will have a null value if step is to be bypassed
                                If ugrdList.Selected.Rows(i).Cells("ApplyBatches").Value = String.Empty Then
                                    logger.Info("Auto-processing Apply Batches action for the user: PriceBatchHeaderID=" + header.PriceBatchHeaderId.ToString)

                                    'bypass applying batches: perform apply batches action for user
                                    If ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value = "4" Then
                                        'set new status id
                                        header.PriceBatchStatusID = 5

                                        'update PriceBatchHeader.PriceBatchStatusID
                                        logger.Info("Updating batch status to SENT: PriceBatchHeaderID=" + header.PriceBatchHeaderId.ToString)
                                        newStatusDesc = headerDAO.UpdatePriceBatchStatus(header)

                                        ugrdList.Selected.Rows(i).Cells("PriceBatchStatusDesc").Value = newStatusDesc
                                        ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value = 5
                                        _operationSucceededWhileIgnoringNoTagLogic = True
                                    Else
                                        logger.Info("Unable to move selected batch to SENT, wrong state: PriceBatchHeaderID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value.ToString() + ", PriceBatchStatusID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value.ToString())
                                        bWrongStatus = True
                                    End If
                                End If
                            Else
                                logger.Info("Unable to move selected batch to PRINTED, wrong state: PriceBatchHeaderID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value.ToString() + ", PriceBatchStatusID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value.ToString())
                                bWrongStatus = True
                            End If
                        End If
                    Else
                        logger.Info("Unable to move selected batch to READY, wrong state: PriceBatchHeaderID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value.ToString() + ", PriceBatchStatusID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value.ToString())
                        bWrongStatus = True
                    End If
                Next

                If frmPricingPrintSignInfo.FailedPrintRequests.Count > 0 Then
                    'Dim failedPrintRequests As String = String.Join(",", frmPricingPrintSignInfo.FailedPrintRequests.Select(Function(f) f.BatchName & ": Store " & f.StoreNumber & " - " & f.SubteamName))
                    Dim failedPrintRequests As String = frmPricingPrintSignInfo.FailedPrintRequests.Select(Function(f) f.BatchName & ": Store " & f.StoreNumber & " - " & f.SubteamName).Aggregate(Function(a, b) a + b)
                    logger.Info(String.Format("Failed print requests: {0}", failedPrintRequests))
                    FailedRequestsDialog.HandleError("The following print requests failed to process.  This information has also been written to the local IRMA client log file.", failedPrintRequests, frmPricingPrintSignInfo.SlawApiErrorMessage)
                    _operationSucceededWhileIgnoringNoTagLogic = False
                End If

                frmPricingPrintSignInfo.Reset()

                If bWrongStatus Then
                    logger.Info(ResourcesPricing.GetString("BatchesWrongStatus"))
                    MsgBox(ResourcesPricing.GetString("BatchesWrongStatus"), MsgBoxStyle.Information, Me.Text)
                    _operationSucceededWhileIgnoringNoTagLogic = False
                ElseIf noTagItemsExcluded Then
                    MsgBox("Print batch requests were not sent to SLAW for all items in these batches.  Please check the no-tag exclusion report for more detail.", MsgBoxStyle.Information, Me.Text)
                End If

            Case 6 'Package
                logger.Info("Processing Package command")
                ' -- SET PRICE BATCH STATUS = PACKAGED
                nTotalSelRows = ugrdList.Selected.Rows.Count

                If nTotalSelRows <= 0 Then
                    logger.Info(ResourcesPricing.GetString("MustSelect"))
                    MsgBox(ResourcesIRMA.GetString("MustSelect"), MsgBoxStyle.Information, Me.Text)
                    Exit Sub
                Else
                    'check for existing OR pending item change
                    Try
                        If DoesBatchesHavePendingPriceChange() Then
                            MsgBox(ResourcesPricing.GetString("PriceBatchConflict"), MsgBoxStyle.Exclamation, Me.Text)
                            Exit Sub
                        End If
                    Catch ex As Exception
                        MsgBox(ResourcesPricing.GetString("PricingBatchConflictsExceptionError"), MsgBoxStyle.Exclamation, Me.Text)
                        logger.Error(ResourcesPricing.GetString("PriceBatchConflict") + Environment.NewLine + "Message: " + ex.Message)
                        Exit Sub
                    End Try
                    If MsgBox(ResourcesPricing.GetString("PackageBatches"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                        logger.Info(ResourcesPricing.GetString("MustSelect"))
                        Exit Sub
                    End If
                End If

                Dim messagesForBatchesWithInvalidDates As New List(Of String)
                For i = 0 To nTotalSelRows - 1

                    Dim invalidDateErrorMsg = DoesBatchRowHaveInvalidDate(ugrdList.Selected.Rows(i), ResourcesPricing.MaxSmalldatetimeDate)
                    If (invalidDateErrorMsg Is String.Empty) Then

                        If ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value = "1" Then
                            'set header id 
                            header.PriceBatchHeaderId = ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value
                            header.ItemChgTypeID = IIf(IsDBNull(ugrdList.Selected.Rows(i).Cells("ItemChgTypeID").Value), 0, ugrdList.Selected.Rows(i).Cells("ItemChgTypeID").Value)
                            header.StoreNumber = ugrdList.Selected.Rows(i).Cells("Store_No").Value

                            'update PriceBatchHeader.PriceBatchStatusID
                            logger.Info("Packaging selected batch: PriceBatchHeaderID =" + header.PriceBatchHeaderId.ToString)
                            newStatusDesc = headerDAO.UpdatePriceBatchPackage(header)

                            ugrdList.Selected.Rows(i).Cells("PriceBatchStatusDesc").Value = newStatusDesc
                            ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value = 2

                            ' Only create Mammoth ItemLocale events if it's a 'New' Change Type (ItemChgTypeID = 1)
                            If header.ItemChgTypeID = 1 Then
                                headerDAO.InsertMammothItemLocaleEvents(header)
                            End If
                        Else ' PriceBatchStatusID IsNot 1
                            logger.Info("Unable to move selected batch to PACKAGED, wrong state: PriceBatchHeaderID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value.ToString() + ", PriceBatchStatusID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value.ToString())
                            bWrongStatus = True
                        End If
                    Else 'invalidDateErrorMsg IsNot String.Empty
                        logger.Info("Unable to move selected batch to PACKAGED for PriceBatchHeaderID=" & ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value.ToString() & invalidDateErrorMsg)
                        messagesForBatchesWithInvalidDates.Add(invalidDateErrorMsg)
                    End If
                Next

                If bWrongStatus Then
                    logger.Info(ResourcesPricing.GetString("MustSelect"))
                    MsgBox(ResourcesPricing.GetString("BatchesWrongStatus"), MsgBoxStyle.Information, Me.Text)
                    Exit Sub
                End If

                If messagesForBatchesWithInvalidDates.Count > 0 Then
                    MsgBox("One or more batches had invalid start dates and could not be processed." & Environment.NewLine & String.Join(",", messagesForBatchesWithInvalidDates.ToArray()), MsgBoxStyle.Information, Me.Text)
                End If

                'refresh the grid after the batches are packaged
                'a refresh is performed here because the package step processes subteam exceptions, and the subteam for the batch
                'might have changed - this change should be shown to the user
                RunSearch()
            Case 7
                logger.Info("Processing Print Tags command")
                If ugrdList.Selected.Rows.Count <> 1 Then
                    logger.Info("Select a single batch")
                    MsgBox("Select a single batch.", MsgBoxStyle.Information, Me.Text)
                    Exit Sub
                ElseIf ugrdList.Selected.Rows(0).Cells("PriceBatchStatusID").Value > 1 Then
                    logger.Debug("cmdProcess_Click Start")
                    cmdProcess_Click(cmdProcess.Item(0), New System.EventArgs())
                    logger.Debug("cmdProcess_Click End")
                Else
                    logger.Info(ResourcesPricing.GetString("BatchStatusNoPrint"))
                    MsgBox(ResourcesPricing.GetString("BatchStatusNoPrint"), MsgBoxStyle.Information, Me.Text)
                End If
        End Select

        If selectAllInGrid Then
            chkSelectAll.Checked = True
        End If

        NotifyIfNoTagLogicWasIgnored()
    End Sub


    Private Function DoesBatchesHavePendingPriceChange() As Boolean
        Dim conflictingBatches As New List(Of DataRow)

        If Not InstanceDataDAO.IsFlagActive("EnablePriceBatchingConflictDectection") Then
            Return False
        End If

        Try

            'getting the header ids for selected batches with item changes
            Dim selectedItemChangeHeaderIds As List(Of Integer) = (From batch As UltraGridRow In ugrdList.Selected.Rows
                                                                   Where batch.Cells("PriceBatchStatusID").Value = "1" AndAlso Not IsDBNull(batch.Cells("ItemChgTypeID").Value) AndAlso batch.Cells("ItemChgTypeID").Value <> 1
                                                                   Select CInt(batch.Cells("PriceBatchHeaderID").Value)).ToList()

            'if no item changes were selected then skip checking
            If Not (selectedItemChangeHeaderIds.Any()) Then
                Return False
            End If

            Dim selectedPriceChangeHeaderIds As List(Of Integer) = (From batch As UltraGridRow In ugrdList.Selected.Rows
                                                                    Where batch.Cells("PriceBatchStatusID").Value = "1" AndAlso Not IsDBNull(batch.Cells("PriceChgTypeID").Value)
                                                                    Select CInt(batch.Cells("PriceBatchHeaderID").Value)).ToList()

            'get pbd table
            Dim priceBatchDetailQuery As DataTable
            priceBatchDetailQuery = PriceBatchDetailDAO.GetPriceBatchDetailsForBuildingOrSent()

            'get the items within the selected itemChange batches
            Dim selectedItemChangeDetails As List(Of DataRow) = (From item As DataRow In priceBatchDetailQuery
                                                                 Where selectedItemChangeHeaderIds.Contains(item("PriceBatchHeaderID"))
                                                                 Select item).ToList()

            'get the items within the selected priceChange batches
            Dim selectedPriceChangeDetails As List(Of DataRow) = (From item As DataRow In priceBatchDetailQuery
                                                                  Where selectedPriceChangeHeaderIds.Contains(item("PriceBatchHeaderID"))
                                                                  Select item).ToList()

            'compare the selected item change batches to any selected price change batches
            Dim rowComparer As New PricingBatchDataRowComparer

            conflictingBatches.AddRange(selectedItemChangeDetails.Intersect(selectedPriceChangeDetails, rowComparer))
            'get all items from priceChange batches that are in sent status
            Dim existingSentPriceChangeDetails As List(Of DataRow) = (From item As DataRow In priceBatchDetailQuery
                                                                      Where item("PriceBatchStatusID") = 5 AndAlso Not IsDBNull(item("PriceChgTypeID"))
                                                                      Select item).ToList()
            'compare the selected item change batches to existing price change batches in sent status in the database 
            conflictingBatches.AddRange(selectedItemChangeDetails.Intersect(existingSentPriceChangeDetails, rowComparer))

            'logging
            If conflictingBatches.Any() Then
                Dim conflictedBatchHeaderIDs = String.Join(",", conflictingBatches.Select(Function(x) x("PriceBatchHeaderID")))
                Dim conflictedPriceScanCodes = String.Join(",", conflictingBatches.Select(Function(x) x("Identifier")))
                Dim conflictMessage As String = String.Format(
                    "Prevented the following batches: {0} from being sent. There is at least one price change batched for the following scan codes: {1}",
                    conflictedBatchHeaderIDs,
                    conflictedPriceScanCodes)
                logger.Info(conflictMessage)
            End If
        Catch ex As Exception
            logger.Error("Failed to check If any existing price batches conflict with selected batches", ex)
            Throw
        End Try

        Return conflictingBatches.Any()
    End Function

    Private Sub cmdProcess_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdProcess.Click
        logger.Debug("cmdProcess_Click Enter")

        Dim index As Short = cmdProcess.GetIndex(eventSender)
        Dim i As Integer
        Dim x As Integer
        Dim nTotalSelRows As Short
        Dim bWrongStatus As Boolean
        Dim headerDAO As New PriceBatchHeaderDAO
        Dim header As New PriceBatchHeaderBO
        Dim newStatusDesc As String = String.Empty

        Select Case index
            Case 0
                ' -- PRINT SHELF TAGS & SET PRICE BATCH STATUS = PRINTED
                If ugrdList.Selected.Rows.Count <> 1 Then
                    If (MessageBox.Show(String.Format("Automatically send print requests for the {0} selected batches?", ugrdList.Selected.Rows.Count), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)) = DialogResult.Yes Then
                        Cursor.Current = Cursors.WaitCursor

                        For i = 0 To ugrdList.Selected.Rows.Count - 1
                            PerformPrintLogic(rowIndex:=i, isReprint:=True)
                        Next

                        Cursor.Current = Cursors.WaitCursor

                        If frmPricingPrintSignInfo.FailedPrintRequests.Count > 0 Then
                            Dim failedPrintRequests As String = String.Join(Environment.NewLine, frmPricingPrintSignInfo.FailedPrintRequests.Select(Function(f) f.BatchName & ": Store " & f.StoreNumber & " - " & f.SubteamName))
                            logger.Info(String.Format("Failed print requests: {0}", failedPrintRequests))
                            FailedRequestsDialog.HandleError("The following print requests failed to process.  This information has also been written to the local IRMA client log file.", failedPrintRequests, frmPricingPrintSignInfo.SlawApiErrorMessage)
                        ElseIf noTagItemsExcluded Then
                            MsgBox("Print batch requests were not sent to SLAW for all items in these batches.  Please check the no-tag exclusion report for more detail.", MsgBoxStyle.Information, Me.Text)
                        Else
                            MessageBox.Show("The print requests have been submitted successfully.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            _operationSucceededWhileIgnoringNoTagLogic = True
                        End If

                        frmPricingPrintSignInfo.Reset()
                    End If
                ElseIf ugrdList.Selected.Rows(i).Cells("PrintShelfTags").Value.GetType Is GetType(DBNull) Then
                    'SKIP THIS STEP IF SELECTED ROW DOES NOT REQUIRE PRINTING
                    logger.Info(ResourcesPricing.GetString("BatchNoPrintRequired"))
                    MsgBox(ResourcesPricing.GetString("BatchNoPrintRequired"), MsgBoxStyle.Information, Me.Text)
                    _operationSucceededWhileIgnoringNoTagLogic = True
                    Exit Sub
                Else
                    If ugrdList.Selected.Rows(0).Cells("PriceBatchStatusID").Value > 1 Then
                        ' populate the PriceBatchHeaderBO for the selected batch and display the details form
                        header = New PriceBatchHeaderBO
                        header.PopulateFromSelectedRow(ugrdList.Selected, 0)
                        frmPricingBatchDetail.BatchHeader = header
                        frmPricingBatchDetail.SetBatchHeaderInfo(True)
                        frmPricingBatchDetail.IgnoreNoTagLogic = IgnoreNoTagLogic
                        logger.Info("Displaying frmPricingBatchDetail to user for shelf tag printing: PriceBatchHeaderID =" + header.PriceBatchHeaderId.ToString)
                        frmPricingBatchDetail.ShowDialog()
                        _operationSucceededWhileIgnoringNoTagLogic = frmPricingBatchDetail.NoTagLogicWasIgnored
                        frmPricingBatchDetail.Dispose()

                        'check InstanceData to see if next batching status step should be skipped: Apply Batches;
                        'the column ApplyBatches will have a null value if step is to be bypassed
                        If ugrdList.Selected.Rows(i).Cells("ApplyBatches").Value.GetType Is GetType(DBNull) Then
                            logger.Info("Auto-processing Apply Batches action for the user: PriceBatchHeaderID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value.ToString)
                            'bypass applying batches: perform apply batches action for user
                            If ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value = "4" Then
                                'set header id and new status id 
                                header.PriceBatchHeaderId = ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value
                                header.PriceBatchStatusID = 5

                                'update PriceBatchHeader.PriceBatchStatusID
                                logger.Info("Updating batch status to SENT: PriceBatchHeaderID=" + header.PriceBatchHeaderId.ToString)
                                newStatusDesc = headerDAO.UpdatePriceBatchStatus(header)

                                ugrdList.Selected.Rows(i).Cells("PriceBatchStatusDesc").Value = newStatusDesc
                                ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value = 5
                                _operationSucceededWhileIgnoringNoTagLogic = True
                            Else
                                logger.Info("Unable to move selected batch to SENT, wrong state: PriceBatchHeaderID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value.ToString() + ", PriceBatchStatusID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value.ToString())
                                bWrongStatus = True
                                _operationSucceededWhileIgnoringNoTagLogic = False
                            End If
                        End If

                        If bWrongStatus Then
                            logger.Info(ResourcesPricing.GetString("BatchesWrongStatus"))
                            MsgBox(ResourcesPricing.GetString("BatchesWrongStatus"), MsgBoxStyle.Information, Me.Text)
                            _operationSucceededWhileIgnoringNoTagLogic = False
                        End If

                        Call LoadDataTable(BuildSearchItem())
                    Else
                        logger.Info(ResourcesPricing.GetString("BatchStatusNoPrint"))
                        MsgBox(ResourcesPricing.GetString("BatchStatusNoPrint"), MsgBoxStyle.Information, Me.Text)
                        _operationSucceededWhileIgnoringNoTagLogic = True
                    End If
                End If

            Case 1
                ' -- SET PRICE BATCH STATUS = SENT
                nTotalSelRows = ugrdList.Selected.Rows.Count

                If nTotalSelRows <= 0 Then
                    logger.Info(ResourcesPricing.GetString("MustSelect"))
                    MsgBox(ResourcesIRMA.GetString("MustSelect"), MsgBoxStyle.Information, Me.Text)
                    Exit Sub
                Else
                    If MsgBox(ResourcesPricing.GetString("ApplyBatchesPOS"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                        logger.Info(ResourcesPricing.GetString("ApplyBatchesPOS"))
                        Exit Sub
                    End If
                End If

                For i = 0 To nTotalSelRows - 1
                    If ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value = "4" Then
                        'set header id and new status id 
                        header.PriceBatchHeaderId = ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value
                        header.PriceBatchStatusID = 5

                        'update PriceBatchHeader.PriceBatchStatusID
                        logger.Info("Updating batch status to SENT: PriceBatchHeaderID=" + header.PriceBatchHeaderId.ToString)
                        newStatusDesc = headerDAO.UpdatePriceBatchStatus(header)

                        ugrdList.Selected.Rows(i).Cells("PriceBatchStatusDesc").Value = newStatusDesc
                        ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value = 5

                        _operationSucceededWhileIgnoringNoTagLogic = True
                    Else
                        logger.Info("Unable to move selected batch to SENT, wrong state: PriceBatchHeaderID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchHeaderID").Value.ToString() + ", PriceBatchStatusID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value.ToString())
                        bWrongStatus = True
                    End If
                Next

                If bWrongStatus Then
                    _operationSucceededWhileIgnoringNoTagLogic = False
                    logger.Info(ResourcesPricing.GetString("BatchesWrongStatus"))
                    MsgBox(ResourcesPricing.GetString("BatchesWrongStatus"), MsgBoxStyle.Information, Me.Text)
                End If
            Case 2
                ' -- MARK ALL AS PRICE BATCH STATUS = PRINTED
                If ugrdList.Selected.Rows.Count <= 0 Then
                    logger.Info(ResourcesPricing.GetString("MustSelect"))
                    MsgBox(ResourcesIRMA.GetString("MustSelect"), MsgBoxStyle.Information, Me.Text)
                    Exit Sub
                ElseIf ugrdList.Selected.Rows(i).Cells("PrintShelfTags").Value.GetType Is GetType(DBNull) Then
                    'SKIP THIS STEP IF SELECTED ROW DOES NOT REQUIRE PRINTING
                    logger.Info(ResourcesPricing.GetString("BatchNoPrintRequired"))
                    MsgBox(ResourcesPricing.GetString("BatchNoPrintRequired"), MsgBoxStyle.Information, Me.Text)
                    Exit Sub
                Else
                    For x = 0 To ugrdList.Selected.Rows.Count - 1
                        If ugrdList.Selected.Rows(x).Cells("PriceBatchStatusID").Value > 1 Then
                            ' populate the PriceBatchHeaderBO for the selected batch and display the details form
                            header = New PriceBatchHeaderBO
                            header.PopulateFromSelectedRow(ugrdList.Selected, x)
                            frmPricingBatchDetail.BatchHeader = header
                            frmPricingBatchDetail.SetBatchHeaderInfo(True)
                            frmPricingBatchDetail.IgnoreNoTagLogic = IgnoreNoTagLogic
                            frmPricingBatchDetail.MarkAsPrinted()
                            logger.Info("Displaying frmPricingBatchDetail to user for shelf tag printing: PriceBatchHeaderID=" + header.PriceBatchHeaderId.ToString)

                            'check InstanceData to see if next batching status step should be skipped: Apply Batches;
                            'the column ApplyBatches will have a null value if step is to be bypassed
                            If ugrdList.Selected.Rows(x).Cells("ApplyBatches").Value.GetType Is GetType(DBNull) Then
                                logger.Info("Auto-processing Apply Batches action for the user: PriceBatchHeaderID=" + ugrdList.Selected.Rows(x).Cells("PriceBatchHeaderID").Value.ToString)
                                'bypass applying batches: perform apply batches action for user
                                If ugrdList.Selected.Rows(x).Cells("PriceBatchStatusID").Value = "4" Then
                                    'set header id and new status id 
                                    header.PriceBatchHeaderId = ugrdList.Selected.Rows(x).Cells("PriceBatchHeaderID").Value
                                    header.PriceBatchStatusID = 5

                                    'update PriceBatchHeader.PriceBatchStatusID
                                    logger.Info("Updating batch status to SENT: PriceBatchHeaderID=" + header.PriceBatchHeaderId.ToString)
                                    newStatusDesc = headerDAO.UpdatePriceBatchStatus(header)

                                    ugrdList.Selected.Rows(x).Cells("PriceBatchStatusDesc").Value = newStatusDesc
                                    ugrdList.Selected.Rows(x).Cells("PriceBatchStatusID").Value = 5
                                Else
                                    logger.Info("Unable to move selected batch to SENT, wrong state: PriceBatchHeaderID=" + ugrdList.Selected.Rows(x).Cells("PriceBatchHeaderID").Value.ToString() + ", PriceBatchStatusID=" + ugrdList.Selected.Rows(x).Cells("PriceBatchStatusID").Value.ToString())
                                    bWrongStatus = True
                                End If
                            End If

                            If bWrongStatus Then
                                _operationSucceededWhileIgnoringNoTagLogic = False
                                logger.Info(ResourcesPricing.GetString("BatchesWrongStatus"))
                                MsgBox(ResourcesPricing.GetString("BatchesWrongStatus"), MsgBoxStyle.Information, Me.Text)
                            End If
                        Else
                            'Do Nothing - no need to prompt about wrong batch state, code just ignores it and moves to the next record
                        End If
                    Next

                    Call LoadDataTable(BuildSearchItem())
                End If
        End Select

        NotifyIfNoTagLogicWasIgnored()

        logger.Debug("cmdProcess_Click Exit")
    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        RunSearch()
    End Sub

    Private Sub RunSearch()
        logger.Debug("RunSearch Enter")

        Dim lZone_ID As Integer

        msStores = String.Empty
        msState = String.Empty
        msItemChgTypeID = String.Empty
        msPriceChgTypeID = String.Empty
        msPriceBatchStatusID = String.Empty
        msSubTeam_No = String.Empty
        msFromStartDate = String.Empty
        msToStartDate = String.Empty
        msIdentifier = String.Empty
        msItem_Description = String.Empty
        msAutoApplyFlag = String.Empty

        Select Case True
            Case optSelection(0).Checked
                If ucmbStoreList.CheckedRows.Count <= 0 Then
                    logger.Info(String.Format(ResourcesIRMA.GetString("Required"), optSelection(0).Text))
                    MsgBox(String.Format(ResourcesIRMA.GetString("Required"), optSelection(0).Text), MsgBoxStyle.Information, Me.Text)
                    Exit Sub
                Else
                    msStores = ComboValue(ucmbStoreList)
                End If

            Case optSelection(1).Checked
                If cmbZones.SelectedIndex = -1 Then
                    logger.Info(String.Format(ResourcesIRMA.GetString("Required"), optSelection(1).Text))
                    MsgBox(String.Format(ResourcesIRMA.GetString("Required"), optSelection(1).Text), MsgBoxStyle.Information, Me.Text)
                    Exit Sub
                Else
                    lZone_ID = CInt(ComboValue(cmbZones))
                End If

            Case optSelection(2).Checked
                If cmbState.SelectedIndex = -1 Then
                    logger.Info(String.Format(ResourcesIRMA.GetString("Required"), optSelection(2).Text))
                    MsgBox(String.Format(ResourcesIRMA.GetString("Required"), optSelection(2).Text), MsgBoxStyle.Information, Me.Text)
                    Exit Sub
                Else
                    msState = cmbState.Text
                End If
        End Select

        If AutoYesRadioButton.Checked = True Then
            msAutoApplyFlag = 1
        ElseIf AutoNoRadioButton.Checked = True Then
            msAutoApplyFlag = 0
        End If

        If Len(msStores) = 0 Then msStores = GetStoreListString(lZone_ID, msState, optSelection(3).Checked, optSelection(4).Checked, optSelection(5).Checked)

        Select Case True
            Case optType(0).Checked 'New
                msItemChgTypeID = "1"
            Case optType(1).Checked 'Item
                msItemChgTypeID = "2"
            Case optType(2).Checked 'Delete
                msItemChgTypeID = "3"
            Case optType(3).Checked 'Price

            Case optType(4).Checked 'Offer
                msItemChgTypeID = "4"
            Case optType(5).Checked 'All
                msItemChgTypeID = "5"
        End Select

        If Not (optType(2).Checked Or optType(4).Checked Or optType(5).Checked) Then
            msPriceChgTypeID = CType(cmbPriceType.SelectedItem, PriceChgTypeBO).PriceChgTypeID
        ElseIf optType(4).Checked Then
            msPriceChgTypeID = "2"
        End If

        msPriceBatchStatusID = ComboValue(cmbStatus)

        msSubTeam_No = ComboValue(cmbSubTeam)

        If dtpStartDate.IsDateValid Then
            msFromStartDate = dtpStartDate.Value
        End If

        If dtpEndDate.IsDateValid Then
            msToStartDate = dtpEndDate.Value
        End If

        If Len(txtIdentifier.Text) > 0 Then
            msIdentifier = txtIdentifier.Text
        End If

        If Len(txtDescription.Text) > 0 Then
            msItem_Description = ConvertQuotes(txtDescription.Text)
        End If

        logger.Info("BuildSearchItem Start")
        Call LoadDataTable(BuildSearchItem())
        logger.Info("BuildSearchItem End")

        ugrdList.Focus()

        logger.Debug("RunSearch Exit")
    End Sub

    Private Sub frmPricingBatch_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        logger.Debug("frmPricingBatch_FormClosed Enter")

        If Not (mrsBatch Is Nothing) Then
            If mrsBatch.State = ADODB.ObjectStateEnum.adStateOpen Then mrsBatch.Close()
        End If

        msStores = ""
        msState = ""
        msItemChgTypeID = ""
        msPriceChgTypeID = ""
        msPriceBatchStatusID = ""
        msSubTeam_No = ""
        msFromStartDate = ""
        msToStartDate = ""
        msIdentifier = ""
        msItem_Description = ""
        logger.Debug("frmPricingBatch_FormClosed Exit")
    End Sub

    Private Sub OptSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSelection.CheckedChanged
        If IsInitializing Then Exit Sub

        logger.Debug("frmPricingBatch_FormClosed Enter")

        If eventSender.Checked Then
            Dim Index As Short = optSelection.GetIndex(eventSender)

            SetActive(ucmbStoreList, False)
            SetActive(cmbZones, False)
            SetActive(cmbState, False)

            Select Case Index
                Case 0 'Store
                    SetActive(ucmbStoreList, True)
                Case 1 'Zone
                    SetActive(cmbZones, True)
                Case 2 'State
                    SetActive(cmbState, True)
            End Select

        End If
        logger.Debug("frmPricingBatch_FormClosed Exit")
    End Sub

    Private Sub optType_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optType.CheckedChanged
        If IsInitializing Then Exit Sub

        logger.Debug("optType_CheckedChanged Enter")

        If eventSender.Checked Then
            Dim Index As Short = optType.GetIndex(eventSender)
            cmbPriceType.Enabled = Not (optType(2).Checked Or optType(4).Checked Or optType(5).Checked)
        End If

        logger.Debug("optType_CheckedChanged Exit")

    End Sub

    Private Sub txtIdentifier_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtIdentifier.Enter
        HighlightText(txtIdentifier)
    End Sub

    Private Sub txtIdentifier_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtIdentifier.KeyPress
        logger.Debug("txtIdentifier_KeyPress Enter")
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtIdentifier.Tag), txtIdentifier, 0, 0, 0)

        If Chr(KeyAscii) = ResourcesIRMA.GetString("Zero") And Len(Trim(txtIdentifier.Text)) = 0 Then KeyAscii = 0

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        logger.Debug("txtIdentifier_KeyPress End")
    End Sub

    Public Sub PopulateRetailStoreDropDown(ByRef cmb As Infragistics.Win.UltraWinGrid.UltraCombo)
        ReplicateUltraCombo(Me.ucmbStoreList, cmb)
        SetStoreListCombo(cmb)
    End Sub

    Public Sub PopulateRetailStoreZoneDropDown(ByRef cmb As System.Windows.Forms.ComboBox)
        ReplicateCombo(Me.cmbZones, cmb)
    End Sub

    Public Sub PopulateRetailStoreStateDropDown(ByRef cmb As System.Windows.Forms.ComboBox)
        ReplicateCombo(Me.cmbState, cmb)
    End Sub

    Public Sub PopulateSubTeamDropDown(ByRef cmb As System.Windows.Forms.ComboBox)
        ReplicateCombo(Me.cmbSubTeam, cmb)
    End Sub

    Private Function GetStoreListString(ByRef lZone_ID As Integer, ByRef msState As String, ByRef bMega_Stores As Boolean, ByRef bWFM_Stores As Boolean, ByRef bAllStores As Boolean) As String
        GetStoreListString = GetStoreListString(dtStores, lZone_ID, msState, bMega_Stores, bWFM_Stores, bAllStores)
    End Function

    Public Function GetStoreListString(ByRef dt As DataTable, ByRef lZone_ID As Integer, ByRef msState As String, ByRef bMega_Stores As Boolean, ByRef bWFM_Stores As Boolean, ByRef bAllStores As Boolean) As String
        logger.Debug("GetStoreListString Enter")

        Dim sStores As String
        Dim sFilter As String

        sStores = String.Empty
        sFilter = String.Empty

        If lZone_ID > 0 Then
            sFilter = "Zone_ID = " & lZone_ID
        ElseIf Len(msState) > 0 Then
            sFilter = "State = '" & msState & "'"
        ElseIf bMega_Stores Then
            sFilter = "Mega_Store = 1"
        ElseIf bWFM_Stores Then
            sFilter = "WFM_Store = 1"
        End If

        Dim dt2 As DataTable

        If dt Is Nothing Then
            dt2 = Me.dtStores
        Else
            dt2 = dt
        End If

        For Each r As DataRow In dt2.Select(sFilter)
            If Len(sStores) > 0 Then
                sStores = sStores & "|" & r("Store_No").ToString()
            Else
                sStores = r("Store_No").ToString()
            End If
        Next

        logger.Debug("GetStoreListString Exit")
        Return sStores
    End Function

    Public Function GetStoreListAll() As Integer()
        logger.Debug("GetStoreListAll Enter")
        Dim result() As Integer
        Dim i As Integer

        ReDim result(dtStores.Rows.Count - 1)
        For Each r As DataRow In dtStores.Rows
            result(i) = r("Store_No").ToString()
            i = i + 1
        Next

        GetStoreListAll = VB6.CopyArray(result)

        logger.Debug("GetStoreListAll Exit")
    End Function

    Public Sub GetBatchInfo(ByRef lPriceBatchHeaderID As Integer, ByRef lStore_No As Integer, ByRef lSubTeam_No As Integer, ByRef iItemChgTypeID As Short, ByRef iPriceChgTypeID As Short, ByRef dStartDate As Date)

        If Not (mdv Is Nothing) Then

            logger.Debug("GetBatchInfo Enter")

            mdv.RowFilter = "PriceBatchHeaderID = " & lPriceBatchHeaderID

            lStore_No = CInt(mdv.Item(0).Item("Store_No"))
            lSubTeam_No = CInt(mdv.Item(0).Item("SubTeam_No"))
            iItemChgTypeID = IIf(IsDBNull(mdv.Item(0).Item("ItemChgTypeID")), 0, mdv.Item(0).Item("ItemChgTypeID"))
            iPriceChgTypeID = IIf(IsDBNull(mdv.Item(0).Item("PriceChgTypeID")), 0, mdv.Item(0).Item("PriceChgTypeID"))
            dStartDate = CDate(mdv.Item(0).Item("StartDate"))
            mdv.RowFilter = String.Empty

            logger.Debug("GetBatchInfo Exit")
        End If
    End Sub

    Private Sub cmbZones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbZones.KeyPress
        logger.Debug("cmbZones_KeyPress Enter")

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
        logger.Debug("cmbState_KeyPress Enter")
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

    Private Sub ugrdList_AfterRowActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdList.AfterRowActivate
        logger.Debug("ugrdList_AfterRowActivate Enter")
        Dim activeRow As Infragistics.Win.UltraWinGrid.UltraGridRow = Me.ugrdList.ActiveRow

        chkSelectAll.Checked = False

        ' disable the "Eraser" button if selected batch is an Epromotion.
        Try
            _cmdMaintain_2.Enabled = Not activeRow.Cells("ItemChgTypeId").Value.ToString().Equals("4")
        Catch ex As Exception
            _cmdMaintain_2.Enabled = True
        End Try

        logger.Debug("ugrdList_AfterRowActivate Exit")
    End Sub

    Private Sub ugrdList_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdList.DoubleClickRow
        logger.Debug("ugrdList_DoubleClickRow Enter")

        If fraMaintain.Visible Or (gbAccountant And Not gbSuperUser) Then
            cmdMaintain_Click(cmdMaintain.Item(1), New System.EventArgs())
        Else
            cmdProcess_Click(cmdProcess.Item(0), New System.EventArgs())
        End If

        logger.Debug("ugrdList_DoubleClickRow Exit")
    End Sub

    ''' <summary>
    ''' performs the action that opening the print tags screens would do, but does this automatically for the user
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PerformPrintLogic(ByVal rowIndex As Integer, ByVal isReprint As Boolean)
        logger.Debug("PerformPrintLogic Enter")

        Dim header As New PriceBatchHeaderBO

        ' populate the PriceBatchHeaderBO for the selected batch.
        header = New PriceBatchHeaderBO
        header.PopulateFromSelectedRow(ugrdList.Selected, rowIndex)
        frmPricingBatchDetail.BatchHeader = header
        frmPricingBatchDetail.SetBatchHeaderInfo(True)
        frmPricingBatchDetail.IgnoreNoTagLogic = IgnoreNoTagLogic

        ' hide print form from user view.
        frmPricingBatchDetail.Hide()

        ' now auto-select all item rows and call the child form.
        frmPricingBatchDetail.CallPrintSignInfoForm(isAutoSelectAllItems:=True, isHideChildForm:=True, byPassNoTagRules:=IgnoreNoTagLogic, isReprint:=isReprint)

        If frmPricingBatchDetail.NoTagItemsExcluded Then
            Me.noTagItemsExcluded = True
        End If

        frmPricingBatchDetail.Dispose()

        logger.Debug("PerformPrintLogic Exit")
    End Sub

    Private Sub _optSelection_4_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_4.Leave
        optSelection(5).Focus()
    End Sub

    Private Sub BatchDescriptionTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles BatchDescriptionTextBox.KeyPress
        If Asc(e.KeyChar) = Keys.Enter Then
            RunSearch()
            e.Handled = True
        End If
    End Sub

    Private Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        If chkSelectAll.Checked Then
            ugrdList.Selected.Rows.AddRange(ugrdList.Rows.All)
        Else
            ugrdList.Selected.Rows.Clear()
        End If
    End Sub

    Private Sub ugrdList_InitializeLayout(sender As Object, e As InitializeLayoutEventArgs) Handles ugrdList.InitializeLayout
        Dim layout As UltraGridLayout = e.Layout
        layout.RowSelectorImages.DataChangedImage = Nothing
        layout.RowSelectorImages.ActiveAndDataChangedImage = layout.RowSelectorImages.ActiveRowImage
    End Sub

    Private Sub ugrdList_AfterSelectChange(sender As Object, e As AfterSelectChangeEventArgs) Handles ugrdList.AfterSelectChange
        'disable print button
        Dim i As Integer

        For i = 0 To ugrdList.Selected.Rows.Count - 1
            If ugrdList.Selected.Rows(i).Cells("PriceBatchStatusID").Value >= 5 _
                And (ugrdList.Selected.Rows(i).Cells("ItemChgTypeDesc").Value = "Item" _
                    Or ugrdList.Selected.Rows(i).Cells("ItemChgTypeDesc").Value = "Price" _
                    Or ugrdList.Selected.Rows(i).Cells("ItemChgTypeDesc").Value = "New") _
            Then
                cmdProcess(0).Enabled = True
            Else
                cmdProcess(0).Enabled = False
                Exit Sub
            End If
        Next

        'End If
    End Sub

    ' Custom comparer for the PriceBatchDetail DataRow
    Private Class PricingBatchDataRowComparer
        Implements IEqualityComparer(Of DataRow)

        Public Function Equals1(
            ByVal x As DataRow,
            ByVal y As DataRow
            ) As Boolean Implements IEqualityComparer(Of DataRow).Equals

            ' Check whether the compared objects reference the same data.
            If x Is y Then Return True

            'Check whether any of the compared objects is null.
            If x Is Nothing OrElse y Is Nothing Then Return False

            ' Check whether the products' properties are equal.
            Return (x("Store_no").Equals(y("Store_no")) AndAlso (x("item_key").Equals(y("item_key"))))
        End Function

        Public Function GetHashCode1(
            ByVal row As DataRow
            ) As Integer Implements IEqualityComparer(Of DataRow).GetHashCode

            ' Check whether the object is null.
            If row Is Nothing Then Return 0

            ' Get hash code for the Name field if it is not null.
            Dim hashProductName =
                If(row("Store_no") Is Nothing, 0, row("Store_no").GetHashCode())

            ' Get hash code for the Code field.
            Dim hashProductCode = row("item_key").GetHashCode()

            ' Calculate the hash code for the product.
            Return hashProductName Xor hashProductCode
        End Function
    End Class
End Class


