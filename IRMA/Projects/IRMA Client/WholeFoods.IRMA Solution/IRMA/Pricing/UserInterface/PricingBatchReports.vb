Option Strict Off
Option Explicit On

Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports log4net
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility
Imports Infragistics.Win.UltraWinGrid

Friend Class frmPricingBatchReports

    Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
    Private mrsStore As ADODB.Recordset
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Private Sub cmbStatus_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbStatus.KeyPress
        logger.Debug("cmbStatus_KeyPress Enter")
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then
            cmbStatus.SelectedIndex = -1
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        logger.Debug("cmbStatus_KeyPress Exit")
    End Sub

	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Me.Close()

    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        logger.Debug("cmdReport_Click Enter")
        Dim sStores As String
        Dim lZone_ID As Integer
        Dim sState As String
        Dim sItemChgTypeID As String
        Dim sPriceChgTypeID As String
        Dim sSubTeam_No As String
        Dim sStartDate As String
        Dim sEndDate As String
        Dim sPriceBatchStatusID As String
        Dim sReportURL As String

        sStores = String.Empty
        sState = String.Empty
        sItemChgTypeID = String.Empty
        sPriceChgTypeID = String.Empty
        sSubTeam_No = String.Empty
        sStartDate = String.Empty
        sEndDate = String.Empty
        sPriceBatchStatusID = String.Empty
        sReportURL = String.Empty

        Select Case True
            Case optSelection(0).Checked
                sStores = ComboValue(ucmbStoreList)

            Case optSelection(1).Checked
                If cmbZones.SelectedIndex = -1 Then
                    logger.Info(String.Format(ResourcesIRMA.GetString("Required"), optSelection(1).Text))
                    MsgBox(String.Format(ResourcesIRMA.GetString("Required"), optSelection(1).Text), MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                Else
                    lZone_ID = CInt(ComboValue(cmbZones))
                End If

            Case optSelection(2).Checked
                If cmbState.SelectedIndex = -1 Then
                    logger.Info(String.Format(ResourcesIRMA.GetString("Required"), optSelection(2).Text))
                    MsgBox(String.Format(ResourcesIRMA.GetString("Required"), optSelection(2).Text), MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                Else
                    sState = cmbState.Text
                End If
        End Select

        If Len(sStores) = 0 Then
            sStores = frmPricingBatch.GetStoreListString(mrsStore, lZone_ID, sState, optSelection(3).Checked, optSelection(4).Checked, optSelection(5).Checked)
        End If

        Select Case True
            Case optType(0).Checked 'New
                sItemChgTypeID = "1"
            Case optType(1).Checked 'Item
                sItemChgTypeID = "2"
            Case optType(2).Checked 'Delete
                sItemChgTypeID = "3"
            Case optType(3).Checked 'Price
                sItemChgTypeID = "0"
            Case optType(4).Checked 'Offer
                sItemChgTypeID = "4"
        End Select

        If Not (optType(2).Checked Or optType(4).Checked) Then
            sPriceChgTypeID = CType(cmbPriceType.SelectedItem, PriceChgTypeBO).PriceChgTypeID
        Else
            sPriceChgTypeID = "0"
        End If

		sSubTeam_No = If(cmbSubTeam.SelectedItem Is Nothing, 0, cmbSubTeam.SelectedItem.SubTeamNo).ToString()

		If IsValidDate(dtpStartDate.Value) Then
			sStartDate = CDate(dtpStartDate.Value).ToString(ResourcesIRMA.GetString("DateStringFormat"))
		Else
			logger.Info(String.Format(ResourcesIRMA.GetString("Required"), "Start Date"))
			MsgBox(String.Format(ResourcesIRMA.GetString("Required"), "Start Date"), MsgBoxStyle.Critical, Me.Text)
			dtpStartDate.Focus()
			Exit Sub
		End If

		If IsValidDate(dtpEndDate.Value) Then
			sEndDate = CDate(dtpEndDate.Value).ToString(ResourcesIRMA.GetString("DateStringFormat"))
		ElseIf (dtpEndDate.Enabled = True) Then
			logger.Info(String.Format(ResourcesIRMA.GetString("Required"), "End Date"))
			MsgBox(String.Format(ResourcesIRMA.GetString("Required"), "End Date"), MsgBoxStyle.Critical, Me.Text)
			dtpEndDate.Focus()
			Exit Sub
		End If

		If Convert.ToDateTime(sEndDate) < Convert.ToDateTime(sStartDate) Then
			MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Critical, Me.Text)
			dtpStartDate.Focus()
			Exit Sub
		End If

		sPriceBatchStatusID = ComboValue(cmbStatus)

		Select Case True
			Case optReport(0).Checked
				sReportURL = "BatchPricingItemSummary&rs:Command=Render&rc:Parameters=false&StoreList=" & sStores & "&StoreListSeparator=|" & "&SubTeam_No" & IIf(sSubTeam_No = "NULL", ":isnull=true", "=" & sSubTeam_No) & "&StartDate" & IIf(sStartDate = "NULL", ":isnull=true", "=" & sStartDate) & "&EndDate" & IIf(sEndDate = "NULL", ":isnull=true", "=" & sEndDate)
			Case optReport(1).Checked
				sReportURL = "BatchPricingItemDetail&rs:Command=Render&rc:Parameters=false&StoreList=" & sStores & "&StoreListSeparator=|" & "&SubTeam_No" & IIf(sSubTeam_No = "NULL", ":isnull=true", "=" & sSubTeam_No) & "&StartDate" & IIf(sStartDate = "NULL", ":isnull=true", "=" & sStartDate) & "&EndDate" & IIf(sEndDate = "NULL", ":isnull=true", "=" & sEndDate) & "&ItemChgTypeID=" & sItemChgTypeID & "&PriceChgTypeID=" & sPriceChgTypeID
			Case optReport(2).Checked
				sReportURL = "BatchPricingBatchSummary&rs:Command=Render&rc:Parameters=false&StoreList=" & sStores & "&StoreListSeparator=|" & "&SubTeam_No" & IIf(sSubTeam_No = "NULL", ":isnull=true", "=" & sSubTeam_No) & "&StartDate" & IIf(sStartDate = "NULL", ":isnull=true", "=" & sStartDate) & "&EndDate" & IIf(sEndDate = "NULL", ":isnull=true", "=" & sEndDate) & "&PriceBatchStatusID" & IIf(sPriceBatchStatusID = "NULL", ":isnull=true", "=" & sPriceBatchStatusID)
			Case optReport(3).Checked
				sReportURL = "BatchDetailReport&rs:Command=Render&rc:Parameters=false&StoreList=" & sStores & "&StoreListSeparator=|" & "&Subteam_No" & IIf(sSubTeam_No = "NULL", ":isnull=true", "=" & sSubTeam_No) & "&StartDate" & IIf(sStartDate = "NULL", ":isnull=true", "=" & sStartDate) & "&EndDate" & IIf(sEndDate = "NULL", ":isnull=true", "=" & sEndDate) & "&BatchStatusID" & IIf(sPriceBatchStatusID = "NULL", ":isnull=true", "=" & sPriceBatchStatusID)
		End Select

		logger.Info("ReportingServicesReport " + sReportURL)
		Call ReportingServicesReport(sReportURL)

		logger.Debug("cmdReport_Click Exit")
	End Sub

	Private Sub frmPricingBatchReports_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		logger.Debug("frmPricingBatchReports_Load Enter")
		CenterForm(Me)

		If (Not frmPricingBatch.Visible) Then

			logger.Debug("EXEC GetRetailStores")

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim results As DataTable = Nothing

			Try

				results = factory.GetStoredProcedureDataTable("GetRetailStores")
				ucmbStoreList.DataSource = results

				ucmbStoreList.DisplayMember = "StoreAbbr"
				ucmbStoreList.ValueMember = "Store_No"

				SetStoreListCombo(ucmbStoreList)

			Catch e As DataFactoryException
				'send message about exception
				ErrorHandler.ProcessError(WholeFoods.Utility.ErrorType.DataFactoryException, SeverityLevel.Warning, e)
			End Try

			logger.Debug("EXEC GetRetailStores Done")

			If results.Rows.Count > 0 Then
				Dim distinctZoneTable As DataTable = results.DefaultView.ToTable(True, "Zone_ID", "Zone_Name")
				cmbZones.DataSource = distinctZoneTable
				cmbZones.ValueMember = "Zone_ID"
				cmbZones.DisplayMember = "Zone_Name"
				cmbZones.SelectedIndex = -1

				Dim distinctStateTable As DataTable = results.DefaultView.ToTable(True, "State")
				cmbState.DataSource = distinctStateTable
				cmbState.ValueMember = "State"
				cmbState.DisplayMember = "State"
				cmbState.SelectedIndex = -1
			End If

			cmbSubTeam.DataSource = SubTeamDAO.GetSubteams()
			Try
				logger.Debug("EXEC GetPriceBatchStatusList Start")
				gRSRecordset = SQLOpenRecordSet("EXEC GetPriceBatchStatusList ", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
				logger.Debug("EXEC GetPriceBatchStatusList End")
				Do While Not gRSRecordset.EOF
					cmbStatus.Items.Add(New VB6.ListBoxItem(gRSRecordset.Fields("PriceBatchStatusDesc").Value, gRSRecordset.Fields("PriceBatchStatusID").Value))
					gRSRecordset.MoveNext()
				Loop
			Finally
				If gRSRecordset IsNot Nothing Then
					gRSRecordset.Close()
					gRSRecordset = Nothing
				End If
			End Try

			If glStore_Limit > 0 Then SetCombo(ucmbStoreList, glStore_Limit)

			LoadPriceTypeCombo()
		Else
			cmbSubTeam.DataSource = SubTeamDAO.GetSubteams()
			frmPricingBatch.PopulateRetailStoreDropDown((Me.ucmbStoreList))
            frmPricingBatch.PopulateRetailStoreZoneDropDown((Me.cmbZones))
            frmPricingBatch.PopulateRetailStoreStateDropDown((Me.cmbState))
            frmPricingBatch.PopulateBatchStatusDropDown((Me.cmbStatus))
            LoadPriceTypeCombo()
        End If

        SetActive(cmbZones, False)
        SetActive(cmbState, False)

        optReport(0).Checked = True

        dtpStartDate.MinDate = #1/1/1995#
        'dtpStartDate.MaxDate = System.DateTime.Today     'commented out per bug 408:Unable to set a future date on the Batching Pricing Reports screen
        dtpStartDate.Value = System.DateTime.Today

        dtpEndDate.MinDate = #1/1/1995#
        'dtpEndDate.MaxDate = System.DateTime.Today     'commented out per bug 408:Unable to set a future date on the Batching Pricing Reports screen
        dtpEndDate.Value = System.DateTime.Today

        If ucmbStoreList.Rows.Count > 0 Then
            If glStore_Limit > 0 Then
                SetCombo(ucmbStoreList, glStore_Limit)
                SetActive(ucmbStoreList, False)
            Else
                ucmbStoreList.SelectedRow = Nothing
            End If
        End If

        If Not CheckAllStoreSelectionEnabled() Then
            _optSelection_5.Visible = False
        End If

        logger.Debug("frmPricingBatchReports_Load Exit")
    End Sub

    Private Sub optReport_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optReport.CheckedChanged
        logger.Debug("optReport_CheckedChanged Enter")
        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then
            Dim Index As Short = optReport.GetIndex(eventSender)

            Dim i As Short

            Select Case Index
                Case 0
                    For i = optType.LBound To optType.UBound
                        optType(i).Enabled = False
                    Next
                    cmbPriceType.Enabled = False
                    dtpEndDate.Enabled = True
                    SetActive(cmbStatus, False)
                Case 1
                    For i = optType.LBound To optType.UBound
                        optType(i).Enabled = True
                    Next
                    dtpEndDate.Enabled = True
                    cmbPriceType.Enabled = True
                    SetActive(cmbStatus, False)
                Case 2
                    For i = optType.LBound To optType.UBound
                        optType(i).Enabled = False
                    Next
                    cmbPriceType.Enabled = False
                    dtpEndDate.Enabled = True
                    SetActive(cmbStatus, True)
                Case 3
                    For i = optType.LBound To optType.UBound
                        optType(i).Enabled = False
                    Next
                    cmbPriceType.Enabled = False
                    dtpEndDate.Enabled = True
                    SetActive(cmbStatus, True)
            End Select


        End If
        logger.Debug("optReport_CheckedChanged End")
    End Sub

    Private Sub OptSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSelection.CheckedChanged
        logger.Debug("optReport_CheckedChanged Enter")
        If Me.IsInitializing = True Then Exit Sub

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
        logger.Debug("optReport_CheckedChanged End")
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
        logger.Debug("cmbState_KeyPress End")
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
        logger.Debug("LoadPriceTypeCombo Enter")
        Dim priceChgDAO As New PriceChgTypeDAO
        Dim priceChgList As ArrayList = PriceChgTypeDAO.GetPriceChgTypeList(True, False)

        cmbPriceType.DataSource = priceChgList
        If priceChgList.Count > 0 Then
            cmbPriceType.DisplayMember = "PriceChgTypeDesc"
            cmbPriceType.ValueMember = "PriceChgTypeID"
        End If
        logger.Debug("LoadPriceTypeCombo Exit")
    End Sub

End Class
