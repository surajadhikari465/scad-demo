Option Strict Off
Option Explicit On

Imports log4net
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.Ordering.DataAccess

Friend Class frmOrderAdd
	Inherits System.Windows.Forms.Form

	Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	Private Const iTransferFromID As Short = 2
	Private Const iTransferToID As Short = 3
	Private Const iTransferSuppliesToID As Short = 4

	Dim IsInitializing As Boolean
	Dim m_bVendorCredit As Boolean
	Dim m_bFormLoading As Boolean
	Dim m_abSubTeam_From_UnRestricted() As Boolean
	Dim m_abSubTeam_To_UnRestricted() As Boolean
	Dim m_bIsVendorDistributorManafacturer As Boolean

	Private Sub cmbField_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbField.KeyPress
		logger.Debug("cmbField_KeyPress Entry")

		Dim KeyAscii As Short = Asc(e.KeyChar)
		Dim Index As Short = cmbField.GetIndex(sender)

		If KeyAscii = ASCII_BACKSPACE Then
			cmbField(Index).SelectedIndex = -1
		End If

		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = ASCII_NULL Then
			e.Handled = True
		End If

		logger.Debug("cmbField_KeyPress Exit")
	End Sub

	Private Sub cmbField_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbField.SelectedIndexChanged
		logger.Debug("cmbField_SelectedIndexChanged Entry")

		If Me.IsInitializing Then Exit Sub

		Dim Index As Short = cmbField.GetIndex(eventSender)

		Select Case Index
			'Purchasing Location
			Case iOrderHeaderPurchaseLocation_ID

				'Make Ship To Location the same
				cmbField(iOrderHeaderReceiveLocation_ID).SelectedIndex = cmbField(iOrderHeaderPurchaseLocation_ID).SelectedIndex

				If cmbField(iOrderHeaderReceiveLocation_ID).SelectedIndex > -1 Then
					If (Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Transfer) Then
						' if vendor is a distributor, load only non-retail subteams (storesubteams minus zonesubteams)
						' note, if/when user selects purchaser as the same distributor/manufacturing facility, need to open
						' list back up to all storesubteams - but still follow the manufacturing/non-retail subteam rules
						If (m_bIsVendorDistributorManafacturer) Then
							If (VB6.GetItemData(cmbField(iOrderHeaderReceiveLocation_ID), cmbField(iOrderHeaderReceiveLocation_ID).SelectedIndex) = glVendorID) Then
								Call LoadSubTeamByType(enumSubTeamType.StoreByVendorID, cmbField(2), m_abSubTeam_From_UnRestricted, glVendorID, 0, chkSubteam.Checked)
							Else
								Call LoadSubTeamByType(enumSubTeamType.StoreMinusSupplier, cmbField(2), m_abSubTeam_From_UnRestricted, glStoreNo, 0, chkSubteam.Checked)
							End If
						End If
					End If

					'Load the purchasing store's subteam list from the StoreSubteam table
					Call PopulateTransferToSubTeam()

					SetActive(cmbField(3), True)
				Else
					cmbField(3).Items.Clear()
					SetActive(cmbField(3), False)
				End If

			Case 2
				'Transfer From
				If cmbField(2).SelectedIndex = -1 Then cmbField(3).SelectedIndex = -1
		End Select

		logger.Debug("cmbField_SelectedIndexChanged Exit")
	End Sub

	Private Sub PopulateTransferToSubTeam()
		logger.Debug("PopulateTransferToSubTeam Entry")

		Dim lReceivingVendorID As Integer

		If (cmbField(iOrderHeaderReceiveLocation_ID).SelectedIndex > -1) Then
			lReceivingVendorID = VB6.GetItemData(cmbField(iOrderHeaderReceiveLocation_ID), cmbField(iOrderHeaderReceiveLocation_ID).SelectedIndex)
		Else
			lReceivingVendorID = 0
		End If

		If (Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Purchase Or Global_Renamed.geOrderType = enumOrderType.Flowthru) And (lReceivingVendorID > 0) Then
			' Determine if Purchaser is a distributor/manufacturer
			If (DeterminePurchaserFacility(lReceivingVendorID) = True) Then
				If Global_Renamed.geProductType = enumProductType.OtherSupplies Then
					' if Other supplies, load the Transfer To (labeled "Supply Type") with only "other supplies" subteams
					Call LoadSubTeamByType(enumSubTeamType.Supplies, cmbField(3), Nothing, -1, -1, chkSubteam.Checked)
					' load the SupplyTranserToSubTeam (labeled "Transfer To") with only "other supplies" subteams
					Call LoadSubTeamByType(enumSubTeamType.Supplies, cmbField(4), Nothing, -1, -1, chkSubteam.Checked)
				Else
					Call LoadSubTeamByType(enumSubTeamType.All, cmbField(3), m_abSubTeam_To_UnRestricted, lReceivingVendorID, 0, chkSubteam.Checked)
				End If
			Else
				'Load the purchasing store's subteam list from the StoreSubteam table
				If Global_Renamed.geProductType = enumProductType.OtherSupplies Then
					' if Other supplies, load the Transfer To (labeled "Supply Type") with only "other supplies" subteams
					Call LoadSubTeamByType(enumSubTeamType.Supplies, cmbField(3), Nothing, -1, -1, chkSubteam.Checked)
					' load the SupplyTranserToSubTeam (labeled "Transfer To") with All subteams
					Call LoadSubTeamByType(enumSubTeamType.All, cmbField(4), Nothing, -1, -1, chkSubteam.Checked)
				Else
					LoadSubTeamByType(enumSubTeamType.StoreByVendorID, cmbField(3), m_abSubTeam_To_UnRestricted, lReceivingVendorID, -1, chkSubteam.Checked) 'Load Vendor Store Subteam
				End If
			End If
		ElseIf (lReceivingVendorID > 0) Then
			'Load the purchasing store's subteam list from the StoreSubteam table
			If Global_Renamed.geProductType = enumProductType.OtherSupplies Then
				' if Other supplies, load the Transfer To (labeled "Supply Type") with only "other supplies" subteams
				Call LoadSubTeamByType(enumSubTeamType.Supplies, cmbField(3), m_abSubTeam_To_UnRestricted, -1, -1, chkSubteam.Checked)
				' load the SupplyTranserToSubTeam (labeled "Transfer To") with All subteams
				Call LoadSubTeamByType(enumSubTeamType.All, cmbField(4), m_abSubTeam_To_UnRestricted, -1, -1, chkSubteam.Checked)
			Else
				LoadSubTeamByType(enumSubTeamType.StoreByVendorID, cmbField(3), m_abSubTeam_To_UnRestricted, lReceivingVendorID, -1, chkSubteam.Checked) 'Load Vendor Store Subteam
			End If
		End If

		logger.Debug("PopulateTransferToSubTeam Exit")
	End Sub

	Private Sub cmbProductType_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbProductType.SelectedIndexChanged
		logger.Debug("cmbProductType_SelectedIndexChanged Entry")

		If Me.IsInitializing = True Then Exit Sub

		' assign the new value
		Global_Renamed.geProductType = VB6.GetItemData(cmbProductType, cmbProductType.SelectedIndex)

		If (m_bFormLoading = True) Then Exit Sub

		' This handles the Transfer-From SubTeam list
		If (Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Distribution) Then
			Call DisplayDistribution()
		ElseIf (Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Transfer) Then
			Call DisplayTransfer()
			'Note:  Purchase orders don't have Transfer-From Subteam
		End If

		' This handles the Transfer-To SubTeam list
		Call PopulateTransferToSubTeam()

		Select Case Global_Renamed.geProductType
			Case enumProductType.OtherSupplies
				_lblLabel_2.Text = "Supply Type :"
				_lblLabel_10.Visible = True
				_cmbField_4.Visible = True
			Case Else
				_lblLabel_2.Text = "Transfer To :"
				_lblLabel_10.Visible = False
				_cmbField_4.Visible = False
		End Select

		logger.Debug("cmbProductType_SelectedIndexChanged Exit")
	End Sub

	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
		logger.Debug("cmdAdd_Click Entry")

		Dim rsOrder As DAO.Recordset
		Dim vExpectedDate As Object
		Dim bErrored As Boolean
		Dim lSubTeamNo As Integer
		Dim bPurchIsRegional As Boolean
		Dim bCurrencyMatch As Boolean

		If Me.cmbField(iOrderHeaderPurchaseLocation_ID).SelectedIndex = -1 Then
			logger.Info("cmdAdd_Click Must select a purchase order")
			MsgBox("Please select a Purchasing Store.", MsgBoxStyle.Information, Me.Text)
			logger.Debug("cmdAdd_Click Exit")
			Exit Sub
		End If

		'-- Make sure that they aren't shipping and receiving at same location
		If cmbField(iOrderHeaderPurchaseLocation_ID).SelectedIndex <> cmbField(iOrderHeaderReceiveLocation_ID).SelectedIndex Then
			If MsgBox("Should the shipping and purchasing locations be different?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, Me.Text) = MsgBoxResult.No Then
				cmbField(iOrderHeaderPurchaseLocation_ID).Focus()
				logger.Debug("cmdAdd_Click Exit")
				Exit Sub
			End If
		End If

		If Not (Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Purchase Or Global_Renamed.geOrderType = enumOrderType.Flowthru) Then
			'-- Make sure a transfer from subteam is selected
			If cmbField(2).SelectedIndex = -1 Then
				logger.Info("cmdAdd_Click - Transfer From is required")
				MsgBox("Please choose a Transfer From store.", MsgBoxStyle.Information, Me.Text)
				cmbField(2).Focus()
				logger.Debug("cmdAdd_Click Exit")
				Exit Sub
			End If
		End If

		'  BSR - Make sure "Other Supplies" validates against the Transer To dropdown
		'  this dropdown is multipurposed.
		If Global_Renamed.geProductType = enumProductType.OtherSupplies Then
			If cmbField(4).SelectedIndex = -1 Then
				logger.Info("cmdAdd_Click - Transfer To is required")
				MsgBox("Please choose a Transfer To store.", MsgBoxStyle.Information, Me.Text)
				cmbField(4).Focus()
				logger.Debug("cmdAdd_Click Exit")
				Exit Sub
			End If
		End If

		'-- Make sure a transfer to subteam is selected, always
		If cmbField(3).SelectedIndex = -1 Then
			logger.Info("cmdAdd_Click - Transfer To is required")
			If Not Global_Renamed.geProductType = enumProductType.OtherSupplies Then
				MsgBox("Please choose a Transfer To store.", MsgBoxStyle.Information, Me.Text)
			Else
				MsgBox("Please choose a Supply Type.", MsgBoxStyle.Information, Me.Text)
			End If
			cmbField(3).Focus()
			logger.Debug("cmdAdd_Click Exit")
			Exit Sub
		End If

		'Determine if the purchaser is a regional store.
		bPurchIsRegional = DeterminePurchaserStore(VB6.GetItemData(cmbField(iOrderHeaderPurchaseLocation_ID), cmbField(iOrderHeaderPurchaseLocation_ID).SelectedIndex))

		Dim Purchasing_Store_Vendor_ID As Integer = ComboValue(_cmbField_1)
		Dim IsDSDVendorForStore As Boolean = OrderingDAO.CheckDSDVendorWithPurchasingStore(glVendorID, Purchasing_Store_Vendor_ID)

		If IsDSDVendorForStore Then
			logger.Info("CmdAdd_Click - Vendor is DSD Vendor for Purchasing Store.  Can't create order.")
			MsgBox("Cannot create a purchase order for a Guaranteed Sale Supplier.  Please use IRMA Mobile to create a Receiving Document for this vendor.", MsgBoxStyle.Information, "Cannot Create Order")
			logger.Info("CmdAdd_Click Exit")
			Exit Sub
		End If

		' check subteam choices
		If Not (Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Purchase Or Global_Renamed.geOrderType = enumOrderType.Flowthru) Then
			' make sure that subteam-from is same as subteam-to if both of the subteams are not manufacturing subteams (both are retail subteams)
			If (m_abSubTeam_From_UnRestricted(cmbField(2).SelectedIndex) = False) And (m_abSubTeam_To_UnRestricted(cmbField(3).SelectedIndex) = False) Then

			Else
				' one of the subteams IS a manufacturing subteam
				If Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Distribution Then
					' for distribution, either the to-subteam is manufacturing OR Both are mfg.
					' so if only the from-subteam is manufacturing... that is not allowed.
					If (m_abSubTeam_To_UnRestricted(cmbField(3).SelectedIndex) = False) Then
						If bPurchIsRegional Then
							logger.Info("CmdAdd_Click - In Case of distribution order, You cannot distribute from a manufacturing subteam to a non-manufacturing subteam.")
							MsgBox("Cannot distribute from a manufacturing subteam to a non-manufacturing subteam.", MsgBoxStyle.Information, Me.Text)
							cmbField(3).Focus()
							logger.Debug("cmdAdd_Click Exit")
							Exit Sub
						End If
					End If
					' if this is a transfer in the same store, the subteams MUST be different
				ElseIf Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Transfer Then
					'If this is an intra-store transfer
					If VB6.GetItemData(cmbField(iOrderHeaderReceiveLocation_ID), cmbField(iOrderHeaderReceiveLocation_ID).SelectedIndex) = glVendorID Then
						'Tranfer From and To must be different
						If cmbField(2).Text = cmbField(3).Text Then
							logger.Info("CmdAdd_Click - In case of Transfer orders, Transfer From " & cmbField(2).Text & "and To " & cmbField(3).Text & " must be different when transfering within the same facility. ")
							MsgBox("Transfer From and Transfer To should be different when transfering within the same facility.", MsgBoxStyle.Information, Me.Text)
							cmbField(3).Focus()
							Exit Sub
						End If
					End If
				End If
			End If
		End If

		If m_bVendorCredit = True Then
			If MsgBox("Is this a return order?", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton2, "Return Order?") = MsgBoxResult.No Then
				m_bVendorCredit = False
			End If
		End If

		'Set up list of handled errors - all user-defined validations done on the server
		Dim lIgnoreErrNum(1) As Integer
		lIgnoreErrNum(0) = 50002
		lIgnoreErrNum(1) = 50003

		'-- Add the new record
		If cmbField(2).SelectedIndex > -1 Then
			On Error Resume Next
			SQLExecute3("EXEC InsertOrder " & glVendorID & ", " & CShort(Global_Renamed.geOrderType) & ", " & CShort(Global_Renamed.geProductType) & ", " & ComboValue(cmbField(iOrderHeaderPurchaseLocation_ID)) & ", " & ComboValue(cmbField(iOrderHeaderReceiveLocation_ID)) & ", " & ComboValue(cmbField(2)) & ", " & ComboValue(cmbField(3)) & ", 0, " & DateDiff(DateInterval.Day, SystemDateTime(True), dtpStartDate.Value) & ", " & giUserID & ", " & System.Math.Abs(CInt(m_bVendorCredit)) & ",0," & ComboValue(cmbField(4)), DAO.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)
			If Err.Number <> 0 Then
				logger.Error(" CmdAdd_Click - error in EXEC InsertOrder, " & Err.Description)
				MsgBox(Err.Description, MsgBoxStyle.Exclamation, Me.Text)
				bErrored = True
			End If
			On Error GoTo 0
		Else
			On Error Resume Next
			'TFS Task 8316 - Set Fax to 0 not 1, now that email PO transmission method is also available
			SQLExecute3("EXEC InsertOrder " & glVendorID & ", " & CShort(Global_Renamed.geOrderType) & ", " & CShort(Global_Renamed.geProductType) & ", " & ComboValue(cmbField(iOrderHeaderPurchaseLocation_ID)) & ", " & ComboValue(cmbField(iOrderHeaderReceiveLocation_ID)) & ", " & "NULL, " & ComboValue(cmbField(3)) & ", 0, " & DateDiff(DateInterval.Day, SystemDateTime(True), dtpStartDate.Value) & ", " & giUserID & ", " & System.Math.Abs(CInt(m_bVendorCredit)) & ",0," & ComboValue(cmbField(4)), DAO.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)
			'Only error raised back from above routine is 50002
			If Err.Number <> 0 Then
				logger.Error("CmdAdd_Click -  Error in EXEC InsertOrder, " & Err.Description)
				MsgBox(Err.Description, MsgBoxStyle.Exclamation, Me.Text)
				bErrored = True
			End If
			On Error GoTo 0

		End If

		If Not bErrored Then

			glOrderHeaderID = -1

			'-- Go back to the previous form
			Me.Close()
		End If

		logger.Debug("cmdAdd_Click Exit")
	End Sub

	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		logger.Debug("cmdExit_Click Entry")
		Me.Close()
		logger.Debug("cmdExit_Click Exit")
	End Sub

	Private Sub frmOrderAdd_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		logger.Debug("frmOrderAdd_Load Entry")

		m_bFormLoading = True

		'-- Center the form
		CenterForm(Me)

		'-- See if it is a distribution or transfer order
		If DetermineVendorInternal() Then
			If ((gbDistribution_Center) Or (gbManufacturer)) Then
				m_bIsVendorDistributorManafacturer = True
				Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Distribution
			Else
				Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Transfer
				'-- Determine whether transfer orders are allowed
				Dim disableTransferOrders As Boolean = InstanceDataDAO.IsFlagActive("DisableTransferOrders")
				If disableTransferOrders Then
					logger.Info("frmOrderAdd_Load - Transfer Orders are disabled")
					MsgBox("Transfer orders are not enabled for this implementation of IRMA.  Please choose a Distribution Center or External Vendor", MsgBoxStyle.Information, Me.Text)
					logger.Debug("frmOrderAdd_Load Exit")
					Me.Close()
				End If
			End If
		Else
			' if no records returned, then this is an external vendor
			Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Purchase
		End If

		SetActive(cmbField(3), False) 'Until the purchasing location is picked

		'-- Set up the Product Type combo
		Call PopulateProductType(cmbProductType)
		Global_Renamed.geProductType = Global_Renamed.enumProductType.Product

		'-- Set Order Type option buttons
		Call DisplayOrderType()

		'-- Load combo boxes
		If Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Purchase Then
			gbExcludeNot_Available = False
			m_bVendorCredit = True
			Call LoadRegionCustomer(cmbField(iOrderHeaderPurchaseLocation_ID))
			Call LimitStore()

			SetActive(cmbField(2), False)
		Else
			gbExcludeNot_Available = True
			dtpStartDate.Value = DateAdd(DateInterval.Day, 1, System.DateTime.Today)

			If Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Distribution Then
				Call DisplayDistribution()
			Else ' Transfer
				Call DisplayTransfer()
			End If

			SetActive(cmbField(2), True)
		End If

		'-- Set defaults
		If cmbField(2).Items.Count = 1 Then
			cmbField(2).SelectedIndex = 0
		End If

		txtField(1).Text = ReturnVendorName(glVendorID)

		dtpStartDate.MinDate = System.DateTime.Today
		dtpStartDate.Value = System.DateTime.Today

		m_bFormLoading = False

		logger.Debug("frmOrderAdd_Load Exit")
	End Sub

	Private Sub DisplayOrderType()
		logger.Debug("DisplayOrderType Entry")

		' Clear the controls and disable
		optPurchase.Checked = False
		optDistribution.Checked = False
		optTransfer.Checked = False
		optFlowthru.Checked = False
		optPurchase.Enabled = False
		optDistribution.Enabled = False
		optTransfer.Enabled = False

		' Turn one on, and enable if appropriate
		Select Case Global_Renamed.geOrderType

			Case Global_Renamed.enumOrderType.Distribution
				' default value
				optDistribution.Checked = True
				optDistribution.Enabled = True

				' allow user to change to transfer (but not if transfers are disabled)
				Dim disableTransferOrders As Boolean = InstanceDataDAO.IsFlagActive("DisableTransferOrders")
				If disableTransferOrders Then
					optTransfer.Enabled = False
				Else
					optTransfer.Enabled = True
				End If
			Case Global_Renamed.enumOrderType.Purchase
				optPurchase.Checked = True
				optPurchase.Enabled = True
			Case Global_Renamed.enumOrderType.Transfer
				optTransfer.Checked = True
				' all controls stay disabled
		End Select

		logger.Debug("DisplayOrderType Exit")
	End Sub

	Private Sub optDistribution_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optDistribution.CheckedChanged
		logger.Debug("optDistribution_CheckedChanged Entry")

		If Me.IsInitializing = True Then Exit Sub
		If eventSender.Checked Then
			' assign new order type
			Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Distribution
			If Not m_bFormLoading Then
				' Note: This click event cannot only be fired when the vendor is a distributor/manufacturer
				Call DisplayDistribution()
				cmbField(iOrderHeaderPurchaseLocation_ID).Focus()
			End If
		End If

		logger.Debug("optDistribution_CheckedChanged Exit")
	End Sub

	Private Sub optTransfer_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optTransfer.CheckedChanged
		logger.Debug("optTransfer_CheckedChanged Entry")

		If Me.IsInitializing = True Then Exit Sub
		If eventSender.Checked Then
			' assign new order type
			Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Transfer
			If Not m_bFormLoading Then
				' Note: This click event cannot only be fired when the vendor is a distributor/manufacturer
				Call DisplayTransfer()
				cmbField(iOrderHeaderPurchaseLocation_ID).Focus()
			End If
		End If

		logger.Debug("optTransfer_CheckedChanged Exit")
	End Sub

	Private Sub DisplayTransfer()
		logger.Debug("DisplayTransfer Entry")

		' Load internal customers as purchasers
		Call LoadRegionCustomer(cmbField(iOrderHeaderPurchaseLocation_ID))
		Call LimitStore()

		' if vendor is a distributor, load only non-retail subteams (storesubteams minus non-mfg zonesubteams)
		' note, if/when user selects purchaser as the same distributor/manufacturing facility, need to open
		' list back up to all storesubteams - but still follow the manufacturing/non-retail subteam rules
		If m_bIsVendorDistributorManafacturer Then
			Call LoadSubTeamByType(enumSubTeamType.StoreMinusSupplier, cmbField(2), m_abSubTeam_From_UnRestricted, glStoreNo, 0, chkSubteam.Checked)
		Else
			' Change From-Subteam to storeSubTeams
			Call LoadSubTeamByType(enumSubTeamType.Store, cmbField(2), m_abSubTeam_From_UnRestricted, glStoreNo, 0, chkSubteam.Checked)
		End If

		logger.Debug("DisplayTransfer Exit")
	End Sub

	Private Sub DisplayDistribution()
		logger.Debug("DisplayDistribution Entry")

		' Load
		Call LoadStoreCustomer(cmbField(iOrderHeaderPurchaseLocation_ID), glStoreNo)
		Call LimitStore()

		' Change From-Subteam to supplier subteams (zoneSubTeams)
		Call LoadSubTeamByType(enumSubTeamType.Supplier, cmbField(2), m_abSubTeam_From_UnRestricted, glStoreNo, 0, chkSubteam.Checked)

		logger.Debug("DisplayDistribution Exit")
	End Sub

	Private Sub UnselectCombos()
		logger.Debug("UnselectCombos Entry")

		If (cmbField(iOrderHeaderPurchaseLocation_ID).Enabled = True) Then
			' unselect the purchasing and receiving values
			cmbField(iOrderHeaderPurchaseLocation_ID).SelectedIndex = -1
			' which means we also need to clear the Transfer-To SubTeam combo
			cmbField(3).Items.Clear()
			cmbField(iOrderHeaderReceiveLocation_ID).SelectedIndex = -1
		End If

		logger.Debug("UnselectCombos Exit")
	End Sub

	Private Sub LimitStore()
		logger.Debug("LimitStore Entry")

		Dim l As Integer

		Call UnselectCombos()

		ReplicateCombo(cmbField(iOrderHeaderPurchaseLocation_ID), cmbField(iOrderHeaderReceiveLocation_ID))

		'-- Limit them to one store
		If (gbBuyer Or gbDistributor) And (Not gbCoordinator) And (glVendor_Limit > 0) Then
			For l = 0 To cmbField(iOrderHeaderPurchaseLocation_ID).Items.Count - 1
				If VB6.GetItemData(cmbField(iOrderHeaderPurchaseLocation_ID), l) = glVendor_Limit Then
					SetActive(cmbField(iOrderHeaderPurchaseLocation_ID), True)
					SetActive(cmbField(iOrderHeaderReceiveLocation_ID), True)
					cmbField(iOrderHeaderPurchaseLocation_ID).SelectedIndex = l
					cmbField(iOrderHeaderReceiveLocation_ID).SelectedIndex = l
					SetActive(cmbField(iOrderHeaderPurchaseLocation_ID), False)
					SetActive(cmbField(iOrderHeaderReceiveLocation_ID), False)
					Exit For
				End If
			Next l
		End If

		logger.Debug("LimitStore Exit")
	End Sub

	Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
		CType(eventSender, TextBox).SelectAll()
	End Sub

	Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
		logger.Debug("txtField_KeyPress Entry")

		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Dim Index As Short = txtField.GetIndex(eventSender)

		KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)

		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = ASCII_NULL Then
			eventArgs.Handled = True
		End If

		logger.Debug("txtField_KeyPress Exit")
	End Sub

	Private Sub LoadOnlyDistributionCenters()
		logger.Debug("LoadOnlyDistributionCenters Entry")

		cmbField(iOrderHeaderPurchaseLocation_ID).Items.Clear()
		cmbField(iOrderHeaderReceiveLocation_ID).Items.Clear()

		Call LoadRegionCustomerAsDC(cmbField(iOrderHeaderPurchaseLocation_ID))
		Call LoadRegionCustomerAsDC(cmbField(iOrderHeaderReceiveLocation_ID))

		logger.Debug("LoadOnlyDistributionCenters Exit")
	End Sub

	Private Sub LoadAllStoreVendors()
		logger.Debug("LoadAllStoreVendors Entry")

		cmbField(iOrderHeaderPurchaseLocation_ID).Items.Clear()
		cmbField(iOrderHeaderReceiveLocation_ID).Items.Clear()

		Call LoadRegionCustomer(cmbField(iOrderHeaderPurchaseLocation_ID))
		Call LoadRegionCustomer(cmbField(iOrderHeaderReceiveLocation_ID))

		logger.Debug("LoadAllStoreVendors Exit")
	End Sub

	Private Sub optFlowthrough_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optFlowthru.CheckedChanged
		logger.Debug("optDistribution_CheckedChanged Entry")

		If Me.IsInitializing = True Then Exit Sub
		If sender.Checked Then
			' assign new order type
			Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Flowthru
			If Not m_bFormLoading Then
				' Note: This click event cannot only be fired when the vendor is a distributor/manufacturer
				Call DisplayDistribution()
				cmbField(iOrderHeaderPurchaseLocation_ID).Focus()
			End If
		End If

		logger.Debug("optDistribution_CheckedChanged Exit")
	End Sub

	Private Sub optPurchase_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optPurchase.CheckedChanged
		logger.Debug("optPurchase_CheckedChanged Entry")

		Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Purchase
		LoadAllStoreVendors()

		logger.Debug("optPurchase_CheckedChanged Exit")
	End Sub

	Private Sub chkSubteam_Click(sender As Object, e As EventArgs) Handles chkSubteam.Click
		RefreshSubteamCombo(cmbField(2), m_abSubTeam_From_UnRestricted, chkSubteam.Checked)
		RefreshSubteamCombo(cmbField(3), m_abSubTeam_To_UnRestricted, chkSubteam.Checked)
		RefreshSubteamCombo(cmbField(4), m_abSubTeam_To_UnRestricted, chkSubteam.Checked)
	End Sub
End Class