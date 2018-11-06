Imports log4net
Imports Infragistics.Win.UltraWinGrid
Imports System.Text
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.StoreAdmin.DataAccess
Imports WholeFoods.IRMA.Administration.BatchMgmt.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.Jobs

Public Class Form_ChangeBatchState

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private searchCriteria As BatchSearchBO

    ''' <summary>
    ''' Load the form, pre-filling the combo boxes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ChangeBatchState_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        logger.Debug("Form_ChangeBatchState_Load entry")

        ' load the combo boxes
        BindStoreData()

        logger.Debug("Form_ChangeBatchState_Load exit")
    End Sub

    ''' <summary>
    ''' Populate the combo boxes for the store, zone, and state selections.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindStoreData()
        logger.Debug("BindStoreData entry")

        ' Read the Store data
        Dim storeList As ArrayList() = StoreDAO.GetRetailStores()

        ComboBox_Store.DataSource = CType(storeList(0), ArrayList)
        ComboBox_Store.DisplayMember = "StoreName"
        ComboBox_Store.ValueMember = "StoreNo"
        ComboBox_Store.SelectedIndex = -1

        ' Read the Zone data
        ComboBox_Zones.DataSource = CType(storeList(1), ArrayList)
        ComboBox_Zones.DisplayMember = "ZoneName"
        ComboBox_Zones.ValueMember = "ZoneId"
        ComboBox_Zones.SelectedIndex = -1

        ' Read the State data
        ComboBox_State.DataSource = CType(storeList(2), ArrayList)
        ComboBox_State.DisplayMember = "State"
        ComboBox_State.ValueMember = "State"
        ComboBox_State.SelectedIndex = -1

        logger.Debug("BindStoreData exit")
    End Sub

    ''' <summary>
    ''' Populate the search criteria business object with the data entered by the user on the form.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateBatchSearchCriteria()
        logger.Debug("PopulateBatchSearchCriteria entry")

        searchCriteria = New BatchSearchBO
        Dim storeList As New StringBuilder
        Dim statusList As New StringBuilder

        ' Stores
        If RadioButton_AllStores.Checked Then
            ' Add all of the stores to the list
            Dim currentStore As StoreBO
            Dim storeEnum As IEnumerator = ComboBox_Store.Items.GetEnumerator()
            While (storeEnum.MoveNext())
                currentStore = CType(storeEnum.Current, StoreBO)
                storeList.Append(searchCriteria.ListSeparator)
                storeList.Append(currentStore.StoreNo.ToString)
            End While
        ElseIf RadioButton_SingleStore.Checked Then
            storeList.Append(searchCriteria.ListSeparator)
            storeList.Append(CType(ComboBox_Store.SelectedItem, StoreBO).StoreNo())
        ElseIf RadioButton_Zone.Checked Then
            Dim selectedZone As ZoneBO = CType(ComboBox_Zones.SelectedItem, ZoneBO)
            storeList = selectedZone.BuildStoreList(searchCriteria.ListSeparator)
        ElseIf RadioButton_State.Checked Then
            Dim selectedState As StateBO = CType(ComboBox_State.SelectedItem, StateBO)
            storeList = selectedState.BuildStoreList(searchCriteria.ListSeparator)
        End If
        searchCriteria.StoreList = storeList.ToString

        ' Batch Status
        If CheckBox_Ready.Checked Then
            statusList.Append(searchCriteria.ListSeparator)
            statusList.Append("3")
        End If
        If CheckBox_Printed.Checked Then
            statusList.Append(searchCriteria.ListSeparator)
            statusList.Append("4")
        End If
        If CheckBox_Sent.Checked Then
            statusList.Append(searchCriteria.ListSeparator)
            statusList.Append("5")
        End If
        searchCriteria.BatchStatusList = statusList.ToString

        ' Batch Description
        searchCriteria.BatchDesc = TextBox_BatchDescription.Text

        logger.Debug("PopulateBatchSearchCriteria exit")
    End Sub

    ''' <summary>
    ''' Run the search and refresh the data grid with the results.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RunSearch()
        logger.Debug("RunSearch entry")

        ' Run the search
        logger.Info("RunSearch: Searching for batches available to re-package")
        Dim batchList As ArrayList = BatchDAO.GetHeaderSearchData(searchCriteria)
        logger.Info("RunSearch: Count of batches returned=" + batchList.Count.ToString)

        ' Refresh the data grid based on the search results
        UltraGrid_Batches.DataSource = batchList

        ' Format the data grid
        FormatDataGrid()

        logger.Debug("RunSearch exit")
    End Sub

    ''' <summary>
    ''' Format the data grid, configuring the columns to hide and display and the
    ''' column order.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormatDataGrid()
        logger.Debug("FormatDataGrid entry")

        If UltraGrid_Batches.Rows.Count > 0 Then
            ' Default the first row to selected
            If UltraGrid_Batches.Rows.Count > 0 Then
                UltraGrid_Batches.Rows(0).Selected = True
            End If
        Else
            ' Let the user know that no matching results were found
            MsgBox("No batches were found matching your search criteria.", MsgBoxStyle.OkOnly, Me.Text)
        End If

        logger.Debug("FormatDataGrid exit")
    End Sub

    ''' <summary>
    ''' Update the batches grid with the set of batches that match the user's search criteria.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Search.Click
        logger.Debug("Button_Search_Click entry")

        ' Validate the search data
        Dim isValid As Boolean = True
        If RadioButton_SingleStore.Checked AndAlso ComboBox_Store.SelectedIndex = -1 Then
            MsgBox(String.Format(ResourcesAdministration.GetString("msg_required"), RadioButton_SingleStore.Text), MsgBoxStyle.Critical, Me.Text)
            isValid = False
        Else
            If RadioButton_Zone.Checked AndAlso ComboBox_Zones.SelectedIndex = -1 Then
                MsgBox(String.Format(ResourcesAdministration.GetString("msg_required"), RadioButton_Zone.Text), MsgBoxStyle.Critical, Me.Text)
                isValid = False
            Else
                If RadioButton_State.Checked AndAlso ComboBox_State.SelectedIndex = -1 Then
                    MsgBox(String.Format(ResourcesAdministration.GetString("msg_required"), RadioButton_State.Text), MsgBoxStyle.Critical, Me.Text)
                    isValid = False
                Else
                    If Not CheckBox_Printed.Checked AndAlso Not CheckBox_Ready.Checked AndAlso Not CheckBox_Sent.Checked Then
                        MsgBox(String.Format(ResourcesAdministration.GetString("msg_required"), GroupBox_BatchState.Text), MsgBoxStyle.Critical, Me.Text)
                        isValid = False
                    End If
                End If
            End If
        End If

        If isValid Then
            ' Populate the search criteria business object
            PopulateBatchSearchCriteria()

            ' Run the search
            RunSearch()
        End If

        logger.Debug("Button_Search_Click exit")
    End Sub

    Private Function IsPushProcessRunning() As Boolean
        logger.Debug("IsPushProcessRunning entry")

        Dim pushProcessRunning As Boolean = False
        Dim scaleJobStatus As JobStatusBO = JobStatusDAO.GetJobStatus("ScalePushJob")
        Dim posJobStatus As JobStatusBO = JobStatusDAO.GetJobStatus("POSPushJob")
        If (scaleJobStatus IsNot Nothing AndAlso scaleJobStatus.Status = DBJobStatus.Running) Or _
           (posJobStatus IsNot Nothing AndAlso posJobStatus.Status = DBJobStatus.Running) Then
            pushProcessRunning = True
        End If

        logger.Debug("IsPushProcessRunning exit: pushProcessRunning=" + pushProcessRunning.ToString)
        Return pushProcessRunning
    End Function

    ''' <summary>
    ''' Move the selected batches back to the packaged state.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Package_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Package.Click
        logger.Debug("Button_Package_Click entry")

        '-- Make sure at least one batch was selected
        If UltraGrid_Batches.Selected.Rows.Count >= 1 Then

            ' Confirm the state change
            If MsgBox("Are you sure you want to move " + UltraGrid_Batches.Selected.Rows.Count.ToString + " batches back to the Packaged state?", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then

                ' If the scale or pos push job is running, the batches in the SENT state are not processed.
                ' Keep track of the number of batches that are skipped due to this.
                Dim skipSentBatchesList As New StringBuilder

                ' Keep track of any batches where the state has changed since the UI was populated and 
                ' the repackage request was submitted.
                Dim skipChangedStatesList As New StringBuilder

                ' Move each selected batch to the packaged state
                Dim currentBatchHeader As BatchHeaderBO
                Dim batchEnum As IEnumerator = UltraGrid_Batches.Selected.Rows.GetEnumerator()
                While batchEnum.MoveNext
                    currentBatchHeader = New BatchHeaderBO(CType(batchEnum.Current, UltraGridRow))

                    ' Verify the batch state has not changed 
                    If currentBatchHeader.BatchStatusID <> BatchDAO.GetCurrentPriceBatchStatus(currentBatchHeader) Then

                        ' The batch state has changed since the user submitted theire request, so they can't make the update.
                        If skipChangedStatesList.Length > 0 Then
                            skipChangedStatesList.Append(", ")
                        End If

                        skipChangedStatesList.Append(currentBatchHeader.PriceBatchHeaderID.ToString)

                        Dim logMsg As New StringBuilder()

                        logMsg.Append("Button_Package_Click - Skipping the update for a batch because the batch state has changed: PriceBatchHeaderID=")
                        logMsg.Append(currentBatchHeader.PriceBatchHeaderID.ToString)
                        logger.Info(logMsg.ToString)
                    Else

                        ' If the current status is SENT, verify POS Push job is not actively running
                        If currentBatchHeader.BatchStatusID = 5 AndAlso IsPushProcessRunning() Then

                            ' The Scale or POS Push job is currently running, so the user cannot update batches in the Sent state.
                            If skipSentBatchesList.Length > 0 Then
                                skipSentBatchesList.Append(", ")
                            End If

                            skipSentBatchesList.Append(currentBatchHeader.PriceBatchHeaderID.ToString)

                            Dim logMsg As New StringBuilder()

                            logMsg.Append("Button_Package_Click - Skipping the update for a batch in the SENT state because the scale and/or POS push job is running: PriceBatchHeaderID=")
                            logMsg.Append(currentBatchHeader.PriceBatchHeaderID.ToString)
                            logger.Info(logMsg.ToString)
                        Else

                            ' Set the updated status value to PACKAGED
                            currentBatchHeader.BatchStatusID = 2

                            ' Update the status
                            logger.Info("Button_Package_Click - Updating the batch status to PACKAGED for PriceBatchHeaderID=" + currentBatchHeader.PriceBatchHeaderID.ToString)
                            BatchDAO.UpdatePriceBatchStatus(currentBatchHeader)
                        End If
                    End If
                End While

                ' Inform the user if any batches were skipped during processing
                If skipSentBatchesList.Length > 0 Then
                    Dim errorMsg As New StringBuilder
                    errorMsg.Append("The Scale Push or POS Push job is currently running.  ")
                    errorMsg.Append("The following batches in the SENT state were not moved back to PACKAGED:")
                    errorMsg.Append(Environment.NewLine)
                    errorMsg.Append(skipSentBatchesList)
                    errorMsg.Append(Environment.NewLine)
                    errorMsg.Append(Environment.NewLine)
                    errorMsg.Append("The updates from these batches are currently being sent to the Scale and/or POS systems.")
                    MsgBox(errorMsg.ToString, MsgBoxStyle.Critical, Me.Text)
                End If
                If skipChangedStatesList.Length > 0 Then
                    Dim errorMsg As New StringBuilder
                    errorMsg.Append("The state of some batches was changed by another application during the processing of this request.  ")
                    errorMsg.Append("The following batches were not moved back to PACKAGED:")
                    errorMsg.Append(Environment.NewLine)
                    errorMsg.Append(skipChangedStatesList)
                    errorMsg.Append(Environment.NewLine)
                    MsgBox(errorMsg.ToString, MsgBoxStyle.Critical, Me.Text)
                End If

                ' Run the search to refresh the grid
                RunSearch()
            End If
        Else
            ' Alert the user that they must select at least one batch
            MsgBox("A batch from the list must be selected.", MsgBoxStyle.Exclamation, Me.Text)
        End If

        logger.Debug("Button_Package_Click exit")
    End Sub

#Region "Data change events"
    Private Sub RadioButton_SingleStore_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_SingleStore.CheckedChanged
        ' Enable the store combo box; disable the others
        ComboBox_Store.Enabled = True

        ComboBox_Zones.Enabled = False
        ComboBox_Zones.SelectedIndex = -1

        ComboBox_State.Enabled = False
        ComboBox_State.SelectedIndex = -1
    End Sub

    Private Sub RadioButton_Zone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_Zone.CheckedChanged
        ' Enable the zone combo box; disable the others
        ComboBox_Store.Enabled = False
        ComboBox_Store.SelectedIndex = -1

        ComboBox_Zones.Enabled = True

        ComboBox_State.Enabled = False
        ComboBox_State.SelectedIndex = -1
    End Sub

    Private Sub RadioButton_State_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_State.CheckedChanged
        ' Enable the state combo box; disable the others
        ComboBox_Store.Enabled = False
        ComboBox_Store.SelectedIndex = -1

        ComboBox_Zones.Enabled = False
        ComboBox_Zones.SelectedIndex = -1

        ComboBox_State.Enabled = True
    End Sub

    Private Sub RadioButton_AllStores_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_AllStores.CheckedChanged
        ' Disable all of the combo boxes
        ComboBox_Store.Enabled = False
        ComboBox_Store.SelectedIndex = -1

        ComboBox_Zones.Enabled = False
        ComboBox_Zones.SelectedIndex = -1

        ComboBox_State.Enabled = False
        ComboBox_State.SelectedIndex = -1
    End Sub
#End Region

End Class