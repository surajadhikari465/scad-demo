Option Explicit On
Option Strict On

Imports Infragistics.Win.UltraWinGrid
Imports System.ComponentModel   ' Need for BindingList 
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.IRMA.EPromotions.DataAccess
Imports WholeFoods.IRMA.TaxHosting.DataAccess       ' for Tax Class
Imports WholeFoods.IRMA.TaxHosting.BusinessLogic    ' for Tax Class
Imports WholeFoods.IRMA.ItemHosting.DataAccess      ' For Item Locking
Imports System.Linq

Public Class Form_PromotionOfferEditor
    'Implements System.ComponentModel.INotifyPropertyChanged
    Public Enum formState
        NewOffer
        EditOffer
        ChangedOffer
    End Enum


    Dim _Loading As Boolean = False                             ' used to suppress events when initializing controls
    Dim WithEvents _offerCurrent As New PromotionOfferBO        ' caches offer currently being operated on
    Dim _formStores As Form_PromotionOffer_AssociateStores = New Form_PromotionOffer_AssociateStores()
    Dim _PromotionalOfferPriceBatchDetail As New PromotionalOfferPriceBatchDetailBO
    Dim _ReadOnlyOffer As Boolean = True                         ' indicates if offer screen is Read Only

#Region "Private Events within this form"
    ''' <summary>
    ''' This event is raised when the form's state changes 
    ''' </summary>
    ''' <remarks></remarks>
    Public Event StateChange()
    '(ByVal sender As Object, _
    '        ByVal e As System.ComponentModel.PropertyChangedEventArgs) _
    '        Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
#End Region

#Region "Properties"
    Dim _State As formState
    ''' <summary>
    ''' State of the Form (New or Edit)
    ''' </summary>
    ''' <value></value>
    ''' <returns>Form state</returns>
    Public Property State() As formState
        Get
            Return _State
        End Get
        Set(ByVal value As formState)
            _State = value
            RaiseEvent StateChange() '(Me, New System.ComponentModel.PropertyChangedEventArgs("State"))
        End Set
    End Property

    Private _PromotionOfferID As Integer
    Public Property PromotionOfferID() As Integer
        Get
            Return _PromotionOfferID
        End Get
        Set(ByVal value As Integer)
            _PromotionOfferID = value
        End Set
    End Property

#End Region

#Region "Public Methods"
    ''' <summary>
    ''' Display non-populated form to create new offer
    ''' </summary>
    ''' <remarks></remarks>
    Public Function NewOffer() As Integer

        CreatePromotionalOffer()
        BindingSource_PromotionOffer.DataSource = _offerCurrent

        ' create price batch detail mangement object, submit 0 as ID to initialize the Original lists
        _PromotionalOfferPriceBatchDetail = New PromotionalOfferPriceBatchDetailBO
        _PromotionalOfferPriceBatchDetail.PreEditInitialization(0, PromotionalOfferPriceBatchDetailBO.InitializeType.SingleOffer)

        With Me
            .State = formState.NewOffer
            InitializeForm()
            .ShowDialog(MyBase.ParentForm)
        End With

    End Function

    Public Function EditOffer(ByVal offerID As Integer) As Integer
        Dim offerDAO As New PromotionOfferDAO
        Dim offerList As PromotionOfferBOList

        With Me
            _Loading = True
            .State = formState.NewOffer
            offerList = offerDAO.GetPromotionalOfferList(offerID)
            If offerList.Count > 0 Then
                ._offerCurrent = offerList(0)
            Else
                ._offerCurrent = Nothing
            End If

            ' set current offer as datasource
            BindingSource_PromotionOffer.DataSource = _offerCurrent
            _Loading = False

            InitializeForm()

            If offerList.Count > 0 Then
                ._offerCurrent = offerList(0)

                ' create new Price Batch detail management object
                _PromotionalOfferPriceBatchDetail = New PromotionalOfferPriceBatchDetailBO
                _PromotionalOfferPriceBatchDetail.PreEditInitialization(_offerCurrent.PromotionOfferID, PromotionalOfferPriceBatchDetailBO.InitializeType.SingleOffer)

                ' Load Store information
                If _formStores.LoadedStoresCount = 0 Then
                    _formStores.AssignStores(_offerCurrent.PromotionOfferID, _offerCurrent.PriceMethodID, False)
                End If

            End If

            ' Display form if it is not already visible
            If Not .Visible Then
                .ShowDialog(MyBase.ParentForm)
            End If

        End With

    End Function


#End Region

    Public Sub ParseDollarValue(ByVal sender As Object, ByVal cevent As ConvertEventArgs)


        If Not cevent.DesiredType Is GetType(Single) Then
            Exit Sub
        End If


        ' Convert the string back to decimal using the shared Parse method. 

        cevent.Value = Single.Parse(cevent.Value.ToString, NumberStyles.Number, Nothing)

    End Sub


    Public Sub FormatDollarValue(ByVal sender As Object, ByVal cevent As ConvertEventArgs)
        ' The method converts only to string type. Test this using the DesiredType. 
        If Not cevent.DesiredType Is GetType(String) Then
            Exit Sub
        End If


        ' Use the ToString method to format the value as currency ("c"). 
        cevent.Value = CType(cevent.Value, Single).ToString("##0.00")


    End Sub




#Region "Private Methods"
    ''' <summary>
    ''' Sets initial values based on whether or not the for is Read Only
    ''' </summary>
    ''' <param name="ForceEditOfLocked">used to override another user's edit lock </param>
    ''' <remarks></remarks>
    Private Sub InitializeForm(Optional ByVal ForceEditOfLocked As Boolean = False)

        ' This will override the formating of TextBox_RewardAmount and force the value to be displayed with Zeros out to 2 decimal places.
        ' declare the binding object 
        Dim b As New Binding("Text", BindingSource_PromotionOffer.DataSource, "RewardAmount")
        AddHandler b.Parse, AddressOf ParseDollarValue
        ' This takes care of displaing data 
        AddHandler b.Format, AddressOf FormatDollarValue
        Me.TextBox_Amount.DataBindings.Clear()
        Me.TextBox_Amount.DataBindings.Add(b)


        Dim offerDAO As New PromotionOfferDAO

        'set up button and title bar labels
        LoadText()

        'load data to form control
        BindData()


        ' deafult unlock button to disabled
        Button_Unlock.Enabled = False

        ' If another user is editing this offer
        ' OR there is a batch pending for this offer, set the offer screen to readonly
        _ReadOnlyOffer = False
        If _offerCurrent.BatchPending(_offerCurrent.PromotionOfferID) Then
            _ReadOnlyOffer = True

            'display msg
            MessageBox.Show(String.Format(ResourcesEPromotions.GetString("msg_offerHasUnsentPriceBatchDetails")), Me.Text, MessageBoxButtons.OK)
        End If

        If Not _ReadOnlyOffer And _
            ((_offerCurrent.IsEditing > 0) And _offerCurrent.IsEditing <> giUserID And Not ForceEditOfLocked) Then
            _ReadOnlyOffer = True

            ' get name of locking user
            Dim userEditing As String = GetInvUserFullName(_offerCurrent.IsEditing)
            MessageBox.Show(String.Format(ResourcesEPromotions.GetString("msg_offerIsBeingEdited"), userEditing), Me.Text, MessageBoxButtons.OK)

            ' Enable the unlock button if the user has sufficient permissions
            Button_Unlock.Enabled = (gbLockAdministrator Or gbSuperUser)
        End If

        ' set controls based on ReadOnlyOffer value
        Button_Cancel.Visible = Not _ReadOnlyOffer
        Button_AddMandatory.Enabled = Not _ReadOnlyOffer
        Button_AddMeetOne.Enabled = Not _ReadOnlyOffer
        Button_AddRewardGroup.Enabled = Not _ReadOnlyOffer
        ' Button_EditMandatory.Enabled = Not _ReadOnlyOffer
        ' Button_EditMeetOne.Enabled = Not _ReadOnlyOffer
        ' Button_EditRewardGroup.Enabled = Not _ReadOnlyOffer
        Button_DeleteMandatory.Enabled = Not _ReadOnlyOffer
        Button_DeleteMeetOne.Enabled = Not _ReadOnlyOffer
        Button_RewardDelete.Enabled = Not _ReadOnlyOffer
        DateTimePicker_StartDate.Enabled = Not _ReadOnlyOffer
        DateTimePicker_EndDate.Enabled = Not _ReadOnlyOffer
        ComboBox_PricingMethod.Enabled = Not _ReadOnlyOffer
        ComboBox_RewardType.Enabled = Not _ReadOnlyOffer
        ComboBox_TaxClass.Enabled = Not _ReadOnlyOffer
		cmbSubTeam.Enabled = Not _ReadOnlyOffer
		TextBox_Amount.ReadOnly = _ReadOnlyOffer
		TextBox_Description.ReadOnly = _ReadOnlyOffer
		TextBox_ReferenceCode.ReadOnly = _ReadOnlyOffer

		If _ReadOnlyOffer Then
			Button_Ok.Text = "Close"

			UltraGrid_MandatoryRequirements.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
			UltraGrid_MeetOneRequirements.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
			UltraGrid_Reward.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
		Else
			' set button text to reflect read/write
			Button_Ok.Text = "OK"

			' set ultragrids to updateable
			UltraGrid_MandatoryRequirements.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
			UltraGrid_MeetOneRequirements.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
			UltraGrid_Reward.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True

			'set Offer isEditing flag
			offerDAO.SetOfferEditFlag(_offerCurrent.PromotionOfferID, giUserID)

			Button_Cancel.Visible = True
			Button_Ok.Text = "OK"
		End If

		' *** REGION SPECIFIC ***
		' If this is a UK instance, do not present 'Item' reward Type and hide Reward Grid. 
		'Also shrink reward frame and form
		If gsRegionCode.Equals("EU") Then
			GroupBox_Reward.Height = 58
			Button_Cancel.Top = 499
			Button_Ok.Top = 499
			Button_Unlock.Top = 499
			Me.Height = 554
		End If

	End Sub

	''' <summary>
	''' Load descriptive text used on form (labels, captions, etc) from resource file
	''' </summary>
	''' <remarks></remarks>
	Private Sub LoadText()
		'// TODO pull text from resource file
		'Label_Amount.Text = ResourcesEPromotions.GetString("label_Amount")
		'Label_Description
		'Label_From
		'Label_To
		Me.Text = ResourcesEPromotions.GetString("label_titlebar_PromotionalOfferEditor")

	End Sub

	''' <summary>
	''' Bind data to form controls
	''' </summary>
	''' <remarks></remarks>
	Private Sub BindData()
		' bind offer detail combos to lookup data
		BindPricingMethodData()
		BindRewardTypeData()
		BindTaxClassComboBox()
		BindSubTeamComboBox()

		' initialize Grids
		BindGrids()
		BindGridDetails()
		SetGridFilters()
	End Sub

	''' <summary>
	''' Set up Reward Type Combo Box
	''' </summary>
	''' <remarks></remarks>
	Private Sub BindRewardTypeData()
		Dim rewardDAO As New RewardTypeDAO
		Dim rewardtypeList As ArrayList = rewardDAO.GetRewardTypeList

		_Loading = True

		' *** REGION SPECIFIC ***
		' Remove Item from Reward Type combo
		If gsRegionCode.Equals("EU") Then
			Dim rewardtypeBO As Object
			For Each rewardtypeBO In rewardtypeList
				If CType(rewardtypeBO, RewardTypeBO).RewardTypeID = 3 Then ' RewardTypeID for 'Item'
					rewardtypeList.Remove(CType(rewardtypeBO, RewardTypeBO))
					Exit For
				End If
			Next
			For Each rewardtypeBO In rewardtypeList
				If CType(rewardtypeBO, RewardTypeBO).RewardTypeID = 2 Then ' RewardTypeID for 'Discount'
					rewardtypeList.Remove(CType(rewardtypeBO, RewardTypeBO))
					Exit For
				End If
			Next

		End If

		UI_Utils.FillComboBox(ComboBox_RewardType, rewardtypeList, "RewardTypeID", "Name", , False)

		_Loading = False

	End Sub

	''' <summary>
	''' Set up Loss cat Code (Tax Class) Combo Box
	''' </summary>
	''' <remarks></remarks>
	Private Sub BindTaxClassComboBox()
		' setup tax class drop down
		Dim taxClassDAO As New TaxClassDAO
		Dim taxClassList As ArrayList = taxClassDAO.GetTaxClassList

		ComboBox_TaxClass.DataSource = taxClassList
		ComboBox_TaxClass.ValueMember = "TaxClassID"
		ComboBox_TaxClass.DisplayMember = "TaxClassDesc"

	End Sub

	''' <summary>
	''' Set up Loss Dept Code (subteam) Combo Box
	''' </summary>
	''' <remarks></remarks>
	Private Sub BindSubTeamComboBox()
		cmbSubTeam.DataSource = SubTeamDAO.GetSubteams()
	End Sub

	''' <summary>
	''' Set up Pricing Method Combo Box
	''' </summary>
	''' <remarks></remarks>
	Private Sub BindPricingMethodData()
		Dim pricingmethodDAO As New PricingMethodDAO
		Dim pricingmethodList As BindingList(Of PricingMethodBO) = pricingmethodDAO.GetPricingMethodList(, True)

		BindingSource_PricingMethod.DataSource = pricingmethodList


	End Sub

	''' <summary>
	''' Populate Purchase requirements and Reward data grids with those Offer members who
	''' are currently associated with the curent offer 
	''' </summary>
	''' <remarks></remarks>
	Private Sub BindGrids()
		Dim offerDAO As New PromotionOfferDAO

		BindingSource_Requirements.DataSource = offerDAO.GetPromotionalOfferMembersList(Me._offerCurrent.PromotionOfferID)
		'BindingSource_RewardPromotionGroups.DataSource = offerDAO.GetPromotionalOfferMembersList(Me._offerCurrent.PromotionOfferID, , True)

	End Sub

	Private Sub SetGridFilters()
		' Make sure deleted Members are not displayed in the grid
		With UltraGrid_MeetOneRequirements.DisplayLayout.Bands(0)
			.Columns("isDeleted").AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
			.Columns("JoinLogic").AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
			.Columns("Purpose").AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
			.ColumnFilters("isDeleted").FilterConditions.Add(FilterComparisionOperator.NotEquals, True)
			.ColumnFilters("JoinLogic").FilterConditions.Add(FilterComparisionOperator.Equals, PromotionOfferMemberJoinLogic.MeetOne)
			.ColumnFilters("Purpose").FilterConditions.Add(FilterComparisionOperator.Equals, PromotionOfferMemberPurpose.Requirement)
		End With

		With UltraGrid_MandatoryRequirements.DisplayLayout.Bands(0)
			.Columns("isDeleted").AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
			.Columns("JoinLogic").AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
			.Columns("Purpose").AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
			.ColumnFilters("isDeleted").FilterConditions.Add(FilterComparisionOperator.NotEquals, True)
			.ColumnFilters("JoinLogic").FilterConditions.Add(FilterComparisionOperator.Equals, PromotionOfferMemberJoinLogic.Mandatory)
			.ColumnFilters("Purpose").FilterConditions.Add(FilterComparisionOperator.Equals, PromotionOfferMemberPurpose.Requirement)
		End With

		With UltraGrid_Reward.DisplayLayout.Bands(0)
			.Columns("isDeleted").AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
			.Columns("Purpose").AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
			.ColumnFilters("isDeleted").FilterConditions.Add(FilterComparisionOperator.NotEquals, True)
			.ColumnFilters("Purpose").FilterConditions.Add(FilterComparisionOperator.Equals, PromotionOfferMemberPurpose.Reward)
		End With

	End Sub

	''' <summary>
	''' Binds data to detail columns in datagrids - Make sure to execute after loading requirements in order to
	''' accurately popukate the Group comboboxes with exclusively available Groups
	''' </summary>
	''' <remarks></remarks>
	Private Sub BindGridDetails()
		Dim valuelistMeetOneRequirementLogic As New Infragistics.Win.ValueList
		Dim valuelistMeetOneRequirementGroup As New Infragistics.Win.ValueList
		Dim valuelistMandatoryRequirementLogic As New Infragistics.Win.ValueList
		Dim valuelistMandatoryRequirementGroup As New Infragistics.Win.ValueList
		Dim valuelistRewardLogic As New Infragistics.Win.ValueList
		Dim valuelistRewardGroup As New Infragistics.Win.ValueList

		' Add group valuelists to collection - will be populated via For Next Loop a bit later
		Dim collectionGroupList As New Collection
		With collectionGroupList
			.Add(valuelistMeetOneRequirementGroup)
			.Add(valuelistMandatoryRequirementGroup)
			.Add(valuelistRewardGroup)
		End With

		' Populate Promotion Group Binding Source - used to build value lists for
		' DropDown columns in grids
		Dim offerDAO As New PromotionOfferDAO
		Dim groupList As BindingList(Of ItemGroupBO) = offerDAO.GetPromotionalGroupList
		BindingSource_PromotionGroups.DataSource = groupList

		' Populate the group lists for all grids simultaneously
		Dim valuelistGroup As Infragistics.Win.ValueList
		Dim groupBO As ItemGroupBO
		For Each valuelistGroup In collectionGroupList
			' Populate PurchaseRequirement item group valuelist
			With valuelistGroup
				For Each groupBO In BindingSource_PromotionGroups
					.ValueListItems.Add(groupBO.GroupID, groupBO.GroupName)
				Next

				.DisplayStyle = Infragistics.Win.ValueListDisplayStyle.DisplayText
			End With
		Next

		' Remove Groups that a re currently in use in the offer from the group comboboxes of the grid that does not 
		' already conatin the Group
		Dim offerMember As PromotionOfferMemberBO
		Dim i As Integer
		For Each offerMember In BindingSource_Requirements
			If Not offerMember.isDeleted Then
				Select Case offerMember.JoinLogic
					Case PromotionOfferMemberJoinLogic.MeetOne
						With valuelistMandatoryRequirementGroup.ValueListItems
							For i = (.Count - 1) To 0 Step -1
								If CType(.Item(i).DataValue, Integer) = offerMember.GroupID Then
									.Remove(i)
								End If
							Next
						End With


					Case PromotionOfferMemberJoinLogic.Mandatory
						With valuelistMeetOneRequirementGroup.ValueListItems
							For i = (.Count - 1) To 0 Step -1
								If CType(.Item(i).DataValue, Integer) = offerMember.GroupID Then
									.Remove(i)
								End If
							Next
						End With

				End Select
			End If

		Next

		' assign value logic & group valuelists to columns in all grids
		With UltraGrid_MeetOneRequirements
			.DisplayLayout.Bands(0).Columns("GroupID").ValueList = valuelistMeetOneRequirementGroup
		End With

		With UltraGrid_MandatoryRequirements
			.DisplayLayout.Bands(0).Columns("GroupID").ValueList = valuelistMandatoryRequirementGroup
		End With

		With UltraGrid_Reward
			.DisplayLayout.Bands(0).Columns("GroupID").ValueList = valuelistRewardGroup
		End With


	End Sub




	''' <summary>
	''' Make sure Amount and Quantity only accept numeric values
	''' </summary>
	''' <remarks></remarks>
	Private Sub TextBoxValidateForNumeric(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) _
		Handles TextBox_Amount.Validating

		Dim value As Single
		Dim display As String


		' If no entry, fill in with "1"
		If CType(sender, TextBox).Text.Trim.Length = 0 Then
			CType(sender, TextBox).Text = "1"
		End If

		' Check for numeric text
		If Not IsNumeric(CType(sender, TextBox).Text.Trim) Then
			e.Cancel = True
			DisplayErrorMessage("You must enter a valid currency value greater than 0.")
			CType(sender, TextBox).Text = "1"
			Exit Sub
		End If

		Try
			value = Single.Parse(CType(sender, TextBox).Text.Trim)
		Catch ex As Exception
			e.Cancel = True
			DisplayErrorMessage("You must enter a valid currency value greater than 0.")
			CType(sender, TextBox).Text = "1"
			Exit Sub
		End Try

		If value <= 0 Then
			e.Cancel = True
			DisplayErrorMessage("You must enter a valid currency value greater than 0.")
			CType(sender, TextBox).Text = "1"
			Exit Sub
		End If

		If value > 999.99 Then
			value = 999.99
		End If

		display = Math.Round(value, 2).ToString("#00.00")

		CType(sender, TextBox).Text = display
		'   CType(sender, TextBox).Update()


	End Sub

	''' <summary>
	''' Enter "New record" mode if the user types in the Description combo or clicks the
	''' "New" button in the Offer section
	''' </summary>
	''' <remarks></remarks>
	Private Sub CreatePromotionalOffer()

		' Create BO for New Offer
		_offerCurrent = New PromotionOfferBO

		With _offerCurrent
			.Desc = ""
			If ComboBox_PricingMethod.Items.Count > 0 Then
				.PriceMethodID = CType(ComboBox_PricingMethod.Items(0), PricingMethodBO).PricingMethodID
			End If

			If ComboBox_RewardType.Items.Count > 0 Then
				.RewardID = CType(ComboBox_RewardType.Items(0), RewardTypeBO).RewardTypeID
			End If

			.UserID = giUserID
			.StartDate = DateTime.Now.Date
			.EndDate = .StartDate.Add(TimeSpan.FromDays(1)).Date
		End With

		' Clear Grids - Selected value should be 0
		BindGrids()
		SetGridFilters()

	End Sub

	''' <summary>
	''' Saves any changes made to data 
	''' </summary>
	''' <remarks></remarks>
	Private Function SaveAllChanges() As Boolean
		Dim success As Boolean = True
		Dim updateStatus As PromotionOfferDAO.EPromotionsUpdateStatus = PromotionOfferDAO.EPromotionsUpdateStatus.Success
		Dim offerDAO As New PromotionOfferDAO
		Dim offerBO As New PromotionOfferBO
		Dim offerMember As PromotionOfferMemberBO
		Dim message As New StringBuilder
		Dim offerChanged As Boolean = False

		' Set working offerBO to currently selected offer
		offerBO = _offerCurrent

		' If no invalid data is reported, update the DB
		success = ValidateFormData()
		If success Then

			' create transaction so all pending changes are commited or none are
			Dim transact As SqlTransaction = offerDAO.GetTransaction()

			' Try-Catch for DB operations
			Try

				'Perform offer insert or update, if necessary
				If offerBO.IsDirty Then
					' cache the fact that the Offer was dirty for later
					offerChanged = True

					If offerBO.IsNew Then
						updateStatus = offerDAO.InsertPromotionalOffer(offerBO, transact)
					Else
						updateStatus = offerDAO.UpdatePromotionalOffer(offerBO, transact)
					End If

					' unless EPromotionsUpdateStatus.Success is returned, set success to fail
					' We check the value of updateStatus at the end to see if we need to reload the data because of concurrency conflict
					Select Case updateStatus
						Case PromotionOfferDAO.EPromotionsUpdateStatus.ConcurrencyConflict, PromotionOfferDAO.EPromotionsUpdateStatus.Fail
							success = False
					End Select
				End If

				' Perform Requirement inserts, updates and deletes where necessary
				If success Then
					For Each offerMember In BindingSource_Requirements
						If offerMember.isDeleted Then
							' Remove deleted requirement groups
							success = offerDAO.DeleteOfferMember(offerMember.OfferMemberID, transact)
						Else
							If offerMember.IsDirty Then
								If offerMember.IsNew Then
									offerMember.OfferID = offerBO.PromotionOfferID
									updateStatus = offerDAO.InsertPromotionalOfferMember(offerMember, transact)
								Else
									updateStatus = offerDAO.UpdatePromotionalOfferMember(offerMember, transact)
								End If

								' unless EPromotionsUpdateStatus.Success is returned, set success to fail
								' We check the value of updateStatus at the end to see if we need to reload the data because of concurrency conflict
								Select Case updateStatus
									Case PromotionOfferDAO.EPromotionsUpdateStatus.ConcurrencyConflict, PromotionOfferDAO.EPromotionsUpdateStatus.Fail
										success = False
								End Select

							End If
						End If

						'  Bail out of loop if operation fails
						If Not success Then Exit For
					Next

					If success Then
						transact.Commit()

						' Save Store associations
						With _formStores
							.SaveChanges(offerBO.PromotionOfferID)
							.Close()
						End With

						' Notify PromotionalOfferPriceBatchDetail object of the newly assigned offerID
						_PromotionalOfferPriceBatchDetail.UpdateZeroID(offerBO.PromotionOfferID)

						' Create Price Batch Details
						If _PromotionalOfferPriceBatchDetail.MaintainPriceBatchDetail() Then
							MessageBox.Show("Price Batch Detail records could not be built.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
						End If
					Else
						transact.Rollback()
					End If

				End If

			Catch ex As Exception
				success = False

				If Not transact.Connection Is Nothing Then
					transact.Rollback()
				End If

				'display error msg
				MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)

			End Try
		End If

		If updateStatus = PromotionOfferDAO.EPromotionsUpdateStatus.ConcurrencyConflict Then
			' reload the offer to reflect current DB state
			Dim offerList As PromotionOfferBOList

			offerList = offerDAO.GetPromotionalOfferList(_offerCurrent.PromotionOfferID)
			If offerList.Count > 0 Then
				_offerCurrent = offerList(0)
			Else
				_offerCurrent = Nothing
			End If
			BindingSource_PromotionOffer.DataSource = _offerCurrent
			BindGrids()
		End If

		Return success
	End Function



	''' <summary>
	''' Builds message string from Promotional Offer status list
	''' </summary>
	''' <remarks></remarks>
	Private Function BuildOfferValidationMessage(ByVal statusList As ArrayList) As String
		Dim statusEnum As IEnumerator
		Dim message As New StringBuilder
		Dim currentStatus As PromotionOfferStatus

		' Loop through possible validation errors and build message string containing all errors
		statusEnum = statusList.GetEnumerator
		While statusEnum.MoveNext
			currentStatus = CType(statusEnum.Current, PromotionOfferStatus)

			Select Case currentStatus
				Case PromotionOfferStatus.Error_Required_Description
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_description_required"), _offerCurrent.Desc))
					message.Append(Environment.NewLine)
				Case PromotionOfferStatus.Error_Required_PricingMethod
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_pricingmethod_required"), _offerCurrent.Desc))
					message.Append(Environment.NewLine)
				Case PromotionOfferStatus.Error_Required_RewardType
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_rewardtype_required"), _offerCurrent.Desc))
					message.Append(Environment.NewLine)
				Case PromotionOfferStatus.Error_Required_ReferenceCode
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_referencecode_required"), _offerCurrent.Desc))
					message.Append(Environment.NewLine)
				Case PromotionOfferStatus.Error_Required_LossVatType
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_lossvat_required"), _offerCurrent.Desc))
					message.Append(Environment.NewLine)
				Case PromotionOfferStatus.Error_Required_LossDeptCode
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_lossdept_required"), _offerCurrent.Desc))
					message.Append(Environment.NewLine)
				Case PromotionOfferStatus.Error_Invalid_StartEndDate
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_StartEndDate_invalid")))
					message.Append(Environment.NewLine)
				Case PromotionOfferStatus.Error_ThreeItemsRequired
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_validation_notNumeric")))
					message.Append(Environment.NewLine)
				Case PromotionOfferStatus.Error_TwoGroupsRequired
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_validation_toofewGroups"), "2", Me.ComboBox_PricingMethod.SelectedText))
					message.Append(Environment.NewLine)
				Case PromotionOfferStatus.Error_ThreeGroupsRequired
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_validation_toofewGroups"), "3", Me.ComboBox_PricingMethod.SelectedText))
					message.Append(Environment.NewLine)
				Case PromotionOfferStatus.Error_Unspecified
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_unspecifiedError")))
					message.Append(Environment.NewLine)
				Case PromotionOfferStatus.Error_Required_OfferRequirements
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_no_offer_requierments")))
					message.Append(Environment.NewLine)
				Case PromotionOfferStatus.Error_Duplicate_PromotionName
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_duplicate_promotionname"), _offerCurrent.Desc))
					message.Append(Environment.NewLine)
				Case PromotionOfferStatus.Error_Invalid_StartDate
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_invalid_startdate"), DateTime.Now.Date.ToShortDateString()))
					message.Append(Environment.NewLine)
				Case PromotionOfferStatus.Error_RewardValue
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_invalid_rewardvalue")))
					message.Append(Environment.NewLine)

					' TODO - other messages
			End Select
		End While

		Return message.ToString

	End Function

	''' <summary>
	''' Builds message string from Promotional Offer Member status list
	''' </summary>
	''' <remarks></remarks>
	Private Function BuildOfferMemberValidationMessage(ByVal statusList As ArrayList) As String
		Dim statusEnum As IEnumerator
		Dim message As New StringBuilder
		Dim currentStatus As PromotionOfferMemberStatus

		' Loop through possible validation errors and build message string containing all errors
		statusEnum = statusList.GetEnumerator
		While statusEnum.MoveNext
			currentStatus = CType(statusEnum.Current, PromotionOfferMemberStatus)

			Select Case currentStatus
				Case PromotionOfferMemberStatus.Error_Required_Description
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_validation_required")))
					message.Append(Environment.NewLine)
				Case PromotionOfferMemberStatus.Error_Required_PriceMethodID
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_validation_notNumeric")))
					message.Append(Environment.NewLine)
				Case PromotionOfferMemberStatus.Error_Required_RewardID
					message.Append(String.Format(ResourcesEPromotions.GetString("msg_validation_notNumeric")))
					message.Append(Environment.NewLine)
					' TODO - other messages
			End Select
		End While

		Return message.ToString

	End Function

	''' <summary>
	''' Validates data currently loaded in form
	''' </summary>
	''' <remarks></remarks>
	Private Function ValidateFormData() As Boolean
		Dim success As Boolean = True
		Dim offerDAO As New PromotionOfferDAO
		Dim offerBO As New PromotionOfferBO
		Dim offerMember As PromotionOfferMemberBO
		Dim message As New StringBuilder
		Dim status As New ArrayList
		status.Add(PromotionOfferStatus.Error_Required_OfferRequirements)

		' Set working offerBO to currently selected offer
		offerBO = _offerCurrent

		' Try-Catch block encapsulates object validation logic
		Try
			' Validate Requirement Groups
			For Each offerMember In BindingSource_Requirements
				If offerMember.IsDirty Then
					message.Append(BuildOfferMemberValidationMessage(offerMember.ValidateData))
				End If
			Next

			' See if any changes need to be saved for current offer
			If offerBO.IsDirty Then
				' Validate current offer data
				message.Append(BuildOfferValidationMessage(offerBO.ValidateData))

			End If

			Dim RequirementCount As Integer = 0
			For Each item As PromotionOfferMemberBO In BindingSource_Requirements
				If item.isDeleted = False Then RequirementCount += 1
			Next

			If RequirementCount = 0 Then
				message.Append(BuildOfferValidationMessage(status))
			End If

			' if the message length is 0, the object is valid
			If message.Length = 0 Then
				success = True
			Else
				success = False

				'display error msg
				MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)

			End If
		Catch ex As Exception
			success = False

			'display error msg
			MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
		End Try

		Return success

	End Function

	''' <summary>
	''' Manage the creation of the new object explicitly here to ensure we can set the proper default values 
	''' for JoinLogic and GroupID
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub AddNewPurchaseRequirement(ByVal sender As Object, ByVal e As System.ComponentModel.AddingNewEventArgs) _
		Handles BindingSource_Requirements.AddingNew
		' Create New Object and set defaults to valid values. Otherwsie the combo boxes in the grid will throw
		' an error
		Dim requirementNew As New PromotionOfferMemberBO

		Try
			With requirementNew
				.Quantity = 1
				.GroupID = CInt(UltraGrid_MeetOneRequirements.DisplayLayout.Bands(0).Columns("GroupID").ValueList.GetValue(0))
				.MarkNew()
				.OfferID = _offerCurrent.PromotionOfferID
			End With
		Catch

		End Try
		' Set New Object to our newly created BO with defaults
		e.NewObject = requirementNew

	End Sub

	''' <summary>
	''' Returns an arraylist that contains all of the GroupIDs for the Groups who are curently used in the offer
	''' </summary>
	''' <param name="Purpose"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Private Function ReturnUsedGroups(ByVal Purpose As PromotionOfferMemberPurpose) As ArrayList

		Dim list As ArrayList = New ArrayList


		For Each item As PromotionOfferMemberBO In BindingSource_Requirements.List
			If item.isDeleted = False Then
				If item.Purpose = Purpose Then
					list.Add(item.GroupID)
				End If
			End If
		Next

		Return list

	End Function

	Private Function PresaveValidate(ByVal PriceMethodID As Integer) As Boolean
		Dim success As Boolean = True
		Dim msg As String

		If Not _formStores Is Nothing Then
			' Check if there are associated stores who do not support the new Pricing Method
			msg = _formStores.ValidateSelectedStoresForPricingMethod(PriceMethodID)

			If msg.Length > 0 Then
				MessageBox.Show(String.Format(ResourcesEPromotions.GetString("msg_storesInvalidForPricingMethod"), msg, MessageBoxButtons.OK, MessageBoxIcon.Stop))
				success = False
			End If

			If success Then
				If _offerCurrent.BatchPending(_offerCurrent.PromotionOfferID) Then
					MessageBox.Show(String.Format(ResourcesEPromotions.GetString("msg_offerPublished"), msg, MessageBoxButtons.OK, MessageBoxIcon.Stop))
					success = False
				End If
			End If
		End If

		Return success

	End Function
#End Region

#Region "Form Events"
	Private Sub Button_EditGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_EditMeetOne.Click
		'Dim ComboCell As DataGridViewComboBoxCell
		'Dim frm As Form_PromotionItemGroupEditor

		'If UltraGrid_MeetOneRequirements.Selected.Rows.Count > 0 Then
		'    ComboCell = CType(UltraGrid_MeetOneRequirements.Selected.Rows(0).Cells("PromotionOfferID").Value, DataGridViewComboBoxCell)
		'    frm = New Form_PromotionItemGroupEditor(CType(ComboCell.Value, Integer))
		'    frm.ShowDialog()
		'    frm.Dispose()
		'Else
		'    MessageBox.Show(ResourcesEPromotions.GetString("msg_RowNotSelectedError"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
		'End If

		If UltraGrid_MeetOneRequirements.ActiveRow IsNot Nothing Then
			Dim result As Boolean = False
			Dim frmEditItemGroup As Form_EditItemGroup = New Form_EditItemGroup(CType(BindingSource_Requirements.Current, PromotionOfferMemberBO))
			Dim DeletedGroupIds As String = String.Empty


			For Each group As PromotionOfferMemberBO In BindingSource_Requirements
				If group.isDeleted Then
					DeletedGroupIds += group.GroupID & ","
				End If
			Next
			If DeletedGroupIds.Length > 0 Then
				DeletedGroupIds = DeletedGroupIds.Remove(DeletedGroupIds.Length - 1, 1)
			End If
			If Not CType(BindingSource_Requirements.Current, PromotionOfferMemberBO).isDeleted Then
				result = frmEditItemGroup.EditGroup(DeletedGroupIds, _ReadOnlyOffer)
				If result Then
					'BindGrids()
					BindGridDetails()
					SetGridFilters()
				End If
			End If

		End If

	End Sub


	Private Sub Button_Ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Ok.Click

		' If the button text has been switched to 'close', do not perform Save operations
		If Button_Ok.Text = "OK" Then
			If PresaveValidate(_offerCurrent.PriceMethodID) AndAlso SaveAllChanges() Then
				Me.Close()
			End If
		Else
			Me.Close()
		End If

	End Sub

	Private Sub Button_AddMeetOne_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddMeetOne.Click

		Dim PromotionOfferMember As PromotionOfferMemberBO
		Dim meetoneMember As Form_Promotion_AddRequirement = New Form_Promotion_AddRequirement(_offerCurrent, PromotionOfferMemberJoinLogic.MeetOne, PromotionOfferMemberPurpose.Requirement, ReturnUsedGroups(PromotionOfferMemberPurpose.Requirement))
		PromotionOfferMember = meetoneMember.AddGroup()
		meetoneMember.Dispose()

		If Not PromotionOfferMember Is Nothing Then
			BindingSource_Requirements.Add(PromotionOfferMember)
		End If

	End Sub

	Private Sub Button_AddMandatory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_AddMandatory.Click

		Dim PromotionOfferMember As PromotionOfferMemberBO
		Dim mandatoryMember As Form_Promotion_AddRequirement = New Form_Promotion_AddRequirement(_offerCurrent, PromotionOfferMemberJoinLogic.Mandatory, PromotionOfferMemberPurpose.Requirement, ReturnUsedGroups(PromotionOfferMemberPurpose.Requirement))
		PromotionOfferMember = mandatoryMember.AddGroup()
		mandatoryMember.Dispose()
		If Not PromotionOfferMember Is Nothing Then
			BindingSource_Requirements.Add(PromotionOfferMember)
		End If
	End Sub

	Private Sub Button_AddRewardGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_AddRewardGroup.Click
		Dim rewardMember As Form_Promotion_AddRequirement = New Form_Promotion_AddRequirement(_offerCurrent, PromotionOfferMemberJoinLogic.Mandatory, PromotionOfferMemberPurpose.Reward, ReturnUsedGroups(PromotionOfferMemberPurpose.Reward))

		Dim PromotionOfferMember As PromotionOfferMemberBO
		PromotionOfferMember = rewardMember.AddGroup()
		rewardMember.Dispose()
		If Not PromotionOfferMember Is Nothing Then
			PromotionOfferMember.Purpose = PromotionOfferMemberPurpose.Reward
			BindingSource_Requirements.Add(PromotionOfferMember)
		End If
	End Sub

	Private Sub Button_DeleteMeetOne_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_DeleteMeetOne.Click

		If UltraGrid_MeetOneRequirements.ActiveRow IsNot Nothing Then
			If CType(BindingSource_Requirements.Current, PromotionOfferMemberBO).JoinLogic = PromotionOfferMemberJoinLogic.MeetOne Then
				CType(BindingSource_Requirements.Current, PromotionOfferMemberBO).MarkDeleted()
				SetGridFilters()

				' reflect change in grid drop downs
				BindGridDetails()
			End If
		End If

	End Sub

	Private Sub Button_DeleteMandatory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_DeleteMandatory.Click

		If UltraGrid_MandatoryRequirements.ActiveRow IsNot Nothing Then
			If CType(BindingSource_Requirements.Current, PromotionOfferMemberBO).JoinLogic = PromotionOfferMemberJoinLogic.Mandatory Then
				CType(BindingSource_Requirements.Current, PromotionOfferMemberBO).MarkDeleted()
				SetGridFilters()

				' reflect change in grid drop downs
				BindGridDetails()
			End If
		End If

	End Sub

	Private Sub Button_RewardDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RewardDelete.Click

		If UltraGrid_Reward.ActiveRow IsNot Nothing Then
			CType(BindingSource_Requirements.Current, PromotionOfferMemberBO).MarkDeleted()
			SetGridFilters()
		End If

	End Sub

	Private Sub Button_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
		Me.Close()
	End Sub



	Private Sub Button_EditMandatory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_EditMandatory.Click
		If UltraGrid_MandatoryRequirements.ActiveRow IsNot Nothing Then
			Dim result As Boolean = False
			Dim frmEditItemGroup As Form_EditItemGroup = New Form_EditItemGroup(CType(BindingSource_Requirements.Current, PromotionOfferMemberBO))
			Dim DeletedGroupIds As String = String.Empty


			For Each group As PromotionOfferMemberBO In BindingSource_Requirements
				If group.isDeleted Then
					DeletedGroupIds += group.GroupID & ","
				End If
			Next
			If DeletedGroupIds.Length > 0 Then
				DeletedGroupIds = DeletedGroupIds.Remove(DeletedGroupIds.Length - 1, 1)
			End If
			If Not CType(BindingSource_Requirements.Current, PromotionOfferMemberBO).isDeleted Then
				result = frmEditItemGroup.EditGroup(DeletedGroupIds, _ReadOnlyOffer)
				If result Then
					'BindGrids()
					BindGridDetails()
					SetGridFilters()
				End If
			End If
		End If

	End Sub

	Private Sub Button_ManageGroups_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ManageGroups.Click
		Dim frm As Form_ManageGroups = New Form_ManageGroups(_offerCurrent, _ReadOnlyOffer)
		frm.ShowDialog()
		frm.Dispose()

		' refresh grid controls to reflect added or deleted groups
		BindGridDetails()

	End Sub



	Private Sub Button_EditRewardGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_EditRewardGroup.Click
		Dim DeletedGroupIds As String = String.Empty

		If UltraGrid_MandatoryRequirements.ActiveRow IsNot Nothing Then
			For Each group As PromotionOfferMemberBO In BindingSource_Requirements
				If group.isDeleted Then
					DeletedGroupIds += group.GroupID & ","
				End If
			Next
			If DeletedGroupIds.Length > 0 Then
				DeletedGroupIds = DeletedGroupIds.Remove(DeletedGroupIds.Length - 1, 1)
			End If

			Dim result As Boolean = False
			Dim frmEditItemGroup As Form_EditItemGroup = New Form_EditItemGroup(CType(BindingSource_Requirements.Current, PromotionOfferMemberBO))
			result = frmEditItemGroup.EditGroup(DeletedGroupIds, _ReadOnlyOffer)
			'If result Then
			BindGrids()
			SetGridFilters()
			'End If
		End If
	End Sub

	Private Sub UltraGrid_MandatoryRequirements_AfterRowActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles UltraGrid_MandatoryRequirements.AfterRowActivate

		' make sure if a row is active in this grid, it is not active in the other grids
		UltraGrid_MeetOneRequirements.ActiveRow = Nothing
		UltraGrid_MeetOneRequirements.Selected.Rows.Clear()
		UltraGrid_Reward.ActiveRow = Nothing
		UltraGrid_Reward.Selected.Rows.Clear()

	End Sub

	Private Sub UltraGrid_MeetOneRequirements_AfterRowActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles UltraGrid_MeetOneRequirements.AfterRowActivate

		' make sure if a row is active in this grid, it is not active in the other grids
		UltraGrid_MandatoryRequirements.ActiveRow = Nothing
		UltraGrid_MandatoryRequirements.Selected.Rows.Clear()
		UltraGrid_Reward.ActiveRow = Nothing
		UltraGrid_Reward.Selected.Rows.Clear()

	End Sub

	Private Sub UltraGrid_Reward_AfterRowActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles UltraGrid_Reward.AfterRowActivate

		' make sure if a row is active in this grid, it is not active in the other grids
		UltraGrid_MeetOneRequirements.ActiveRow = Nothing
		UltraGrid_MeetOneRequirements.Selected.Rows.Clear()
		UltraGrid_MandatoryRequirements.ActiveRow = Nothing
		UltraGrid_MandatoryRequirements.Selected.Rows.Clear()

	End Sub

	Private Sub Button_AssociateStores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AssociateStores.Click

		With _formStores
			If .LoadedStoresCount = 0 Then
				.AssignStores(_offerCurrent.PromotionOfferID, _offerCurrent.PriceMethodID, True)
			Else
				.RefreshStoresForPricingMethod(_offerCurrent.PriceMethodID)
				.Show()
				.BringToFront()
				.TopMost = True
			End If
		End With

	End Sub

	Private Sub Form_PromotionOfferEditor_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		Dim offerDAO As New PromotionOfferDAO

		' do not reset flag if we are closing because another user is editing the offer.
		If Not _ReadOnlyOffer Then
			'set Offer isEditing flag to -1
			offerDAO.SetOfferEditFlag(_offerCurrent.PromotionOfferID, -1)
		End If

		If Not _formStores.IsDisposed Then
			_formStores.Dispose()
		End If

	End Sub

	Private Sub ComboBox_PricingMethod_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboBox_PricingMethod.Validating

		If Not PresaveValidate(CType(CType(sender, ComboBox).SelectedValue, Integer)) Then
			ComboBox_PricingMethod.SelectedValue = _offerCurrent.PriceMethodID
		End If

	End Sub

	Private Sub UltraGrid_MandatoryRequirements_BeforeCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs) Handles UltraGrid_MandatoryRequirements.BeforeCellUpdate
		Dim GroupID As Integer

		' if the updated cell is GroupID, validate that the Group does not already exist as a requirement   
		'If CType(sender, UltraGrid).ActiveCell.Column.Key.ToUpper = "GROUPID" Then
		If e.Cell.Column.Key.ToUpper = "GROUPID" Then
			GroupID = CType(e.NewValue, Integer)

			If ReturnUsedGroups(PromotionOfferMemberPurpose.Requirement).Contains(GroupID) Then
				MessageBox.Show("This Group is used in an existing requirement. Please choose another.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
				e.Cancel = True
			End If
		End If

	End Sub

	Private Sub BindingSource_Requirements_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles BindingSource_Requirements.ListChanged

		Select Case e.ListChangedType
			Case ListChangedType.ItemAdded, ListChangedType.ItemChanged, ListChangedType.ItemDeleted
				' reflect change in grid drop downs - Deletes handled in DELETE buttons
				BindGridDetails()
		End Select
	End Sub

#End Region

	Private Sub ComboBox_TaxClass_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_TaxClass.DropDown
		Dim MaxLen As Integer = 200
		Dim Len As Integer = 0
		For Each item As TaxClassBO In ComboBox_TaxClass.Items
			Len = TextRenderer.MeasureText(item.TaxClassDesc, ComboBox_TaxClass.Font).Width
			If Len > MaxLen Then MaxLen = Len
		Next
		ComboBox_TaxClass.DropDownWidth = MaxLen
	End Sub

	Private Sub cmbSubTeam_DropDown(ByVal sender As Object, ByVal e As EventArgs) Handles cmbSubTeam.DropDown
		If (cmbSubTeam.DataSource IsNot Nothing) Then
			cmbSubTeam.DropDownWidth = cmbSubTeam.DataSource.Max(Function(x) TextRenderer.MeasureText(x.SubTeamName, cmbSubTeam.Font).Width) + 5
		End If
	End Sub

	Private Sub EndDateValidation(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles DateTimePicker_EndDate.Validating
		'If DateTimePicker_EndDate.Value <= DateTimePicker_StartDate.Value.Date Then
		'    e.Cancel = True
		'    DisplayErrorMessage("End Date for the promotion can not be before the Start Date.")
		'End If
	End Sub

	Private Sub GridCellChange(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles UltraGrid_MeetOneRequirements.CellChange, UltraGrid_MandatoryRequirements.CellChange, UltraGrid_Reward.CellChange
		Dim value As Integer

		If e.Cell.Column.Header.Caption = "Quantity" Then
			If e.Cell.Text.Trim <> "" Then
				Try
					value = Int32.Parse(e.Cell.Text)
				Catch ex As Exception
					e.Cell.Value = e.Cell.OriginalValue
				End Try
				If value <= 0 Then
					e.Cell.Value = 1
				End If
			End If
		End If
	End Sub

	Private Sub BeforeCellUpdate(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs) Handles UltraGrid_MeetOneRequirements.BeforeCellUpdate, UltraGrid_MandatoryRequirements.BeforeCellUpdate, UltraGrid_Reward.BeforeCellUpdate
		If e.Cell.Column.Header.Caption = "Quantity" Then
			If e.Cell.Text.Trim() = "" Then e.Cancel = True
		End If
	End Sub

	Private Sub TextBox_Description_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_Description.TextChanged
		If Not _Loading Then
			If TextBox_Description.Text <> _offerCurrent.Desc Then
				_offerCurrent.DescriptionChanged = True
			End If
		End If
	End Sub

	Private Sub Form_PromotionOfferEditor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		' Clear all tabstops. 
		For Each item As Control In Me.Controls
			Try
				item.TabStop = False
				item.TabIndex = 0
			Catch ex As Exception

			End Try
		Next

		' Reapply correct tabstops.
		EditTabStop(GroupBox_PromotionalOffer, True, 0)
		EditTabStop(TextBox_Description, True, 1)
		EditTabStop(TextBox_ReferenceCode, True, 2)
		EditTabStop(ComboBox_TaxClass, True, 3)
		EditTabStop(cmbSubTeam, True, 4)
		EditTabStop(ComboBox_PricingMethod, True, 5)
        EditTabStop(DateTimePicker_StartDate, True, 6)
        EditTabStop(DateTimePicker_EndDate, True, 7)
        EditTabStop(Button_ManageGroups, True, 8)
        EditTabStop(Button_AssociateStores, True, 9)

        EditTabStop(GroupBox, True, 10)
        EditTabStop(Button_AddMeetOne, True, 11)
        EditTabStop(Button_EditMeetOne, True, 12)
        EditTabStop(Button_DeleteMeetOne, True, 13)
        EditTabStop(Button_AddMandatory, True, 14)
        EditTabStop(Button_EditMandatory, True, 15)
        EditTabStop(Button_DeleteMandatory, True, 16)

        EditTabStop(GroupBox_Reward, True, 17)
        EditTabStop(ComboBox_RewardType, True, 18)
        EditTabStop(TextBox_Amount, True, 19)

        If Not gsRegionCode.Equals("EU") Then
            EditTabStop(Button_AddRewardGroup, True, 20)
            EditTabStop(Button_EditRewardGroup, True, 21)
            EditTabStop(Button_RewardDelete, True, 22)
            EditTabStop(Button_Cancel, True, 23)
            EditTabStop(Button_Ok, True, 24)
        Else
            EditTabStop(Button_AddRewardGroup, False, 1)
            EditTabStop(Button_EditRewardGroup, False, 1)
            EditTabStop(Button_RewardDelete, False, 1)
            EditTabStop(Button_Cancel, True, 23)
            EditTabStop(Button_Ok, True, 24)
        End If


        TextBox_Description.Focus()

    End Sub

    Private Sub EditTabStop(ByVal oControl As Control, ByVal value As Boolean, ByVal index As Integer)
        oControl.TabStop = value
        oControl.TabIndex = index
    End Sub

	Private Sub Button_Unlock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Unlock.Click
		If MsgBox(ResourcesEPromotions.GetString("msg_unlockOffer"), CType(MsgBoxStyle.YesNo + MsgBoxStyle.Question, Microsoft.VisualBasic.MsgBoxStyle), Me.Text) = MsgBoxResult.Yes Then
			InitializeForm(True)
		End If
	End Sub
End Class
