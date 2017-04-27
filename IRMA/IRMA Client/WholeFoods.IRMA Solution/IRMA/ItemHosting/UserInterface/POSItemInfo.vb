Imports System.Text
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class POSItemInfo
    Private _itemIdentifier As String
    Private _posItemBO As POSItemBO
    Private _hasChanges As Boolean

#Region "property access methods"

    Public Property ItemIdentifier() As String
        Get
            Return _itemIdentifier
        End Get
        Set(ByVal value As String)
            _itemIdentifier = value
        End Set
    End Property

    Public Property POSItemBO() As POSItemBO
        Get
            Return _posItemBO
        End Get
        Set(ByVal value As POSItemBO)
            _posItemBO = value
        End Set
    End Property

#End Region

#Region "Events raised by this form"
    ''' <summary>
    ''' This event is raised when a child form makes a change that requires the
    ''' data on the calling form to be updated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UpdateCallingForm(ByVal posItemData As POSItemBO)
#End Region

#Region "form events"
    Private Sub POSItemInfo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CenterToScreen()

        'set item identifier on form for user display
        Me.Label_IdentifierValue.Text = _itemIdentifier

        ' pre-fill the form with existing values
        InitializeData()
    End Sub

    Private Sub POSItemInfo_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If _hasChanges Then
            'prompt the user to save changes
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                If ApplyChanges() Then
                    'close form
                    Me.Close()
                Else
                    e.Cancel = True
                End If
            End If
        End If
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub CheckBox_PriceRequired_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_PriceRequired.CheckedChanged
        _hasChanges = True
    End Sub

    Private Sub CheckBox_QuantityRequired_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_QuantityRequired.CheckedChanged
        _hasChanges = True
    End Sub

    Private Sub CheckBox_FoodStamps_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_FoodStamps.CheckedChanged
        _hasChanges = True
    End Sub

    Private Sub CheckBox_NCR_QuantityProhibit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_QuantityProhibit.CheckedChanged
        _hasChanges = True
    End Sub

    Private Sub TextBox_NCR_GroupList_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_GroupList.TextChanged
        _hasChanges = True
    End Sub

    Private Sub CheckBox_CaseDiscount_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_CaseDiscount.CheckedChanged
        _hasChanges = True
    End Sub

    Private Sub CheckBox_CouponMultiplier_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_CouponMultiplier.CheckedChanged
        _hasChanges = True
    End Sub

    Private Sub CheckBox_FSAEligible_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_FSAEligible.CheckedChanged
        _hasChanges = True
    End Sub

    Private Sub TextBox_MiscTransRefund_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_MiscTransRefund.TextChanged
        _hasChanges = True
    End Sub

    Private Sub TextBox_MiscTransSale_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_MiscTransSale.TextChanged
        _hasChanges = True
    End Sub

    Private Sub TextBox_IceTare_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_IceTare.TextChanged
        _hasChanges = True
    End Sub

    Private Sub TextBox_ProductCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_ProductCode.TextChanged
        _hasChanges = True
    End Sub

    Private Sub TextBox_unitPriceCategory_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_UnitPriceCategory.TextChanged
        _hasChanges = True
    End Sub




#End Region

    ''' <summary>
    ''' fill in form elements w/ existing data
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeData()
        'fill in form elements w/ data if any exists
        Me.CheckBox_FoodStamps.Checked = _posItemBO.FoodStamps
        Me.CheckBox_QuantityProhibit.Checked = _posItemBO.QuantityProhibit
        Me.CheckBox_QuantityRequired.Checked = _posItemBO.QuantityRequired
        Me.CheckBox_PriceRequired.Checked = _posItemBO.PriceRequired
        Me.TextBox_GroupList.Text = _posItemBO.GroupList

        Me.CheckBox_CaseDiscount.Checked = _posItemBO.CaseDiscount
        Me.CheckBox_CouponMultiplier.Checked = _posItemBO.CouponMultiplier
        Me.TextBox_IceTare.Text = _posItemBO.IceTare
        Me.TextBox_MiscTransRefund.Text = _posItemBO.MiscTransactionRefund
        Me.TextBox_MiscTransSale.Text = _posItemBO.MiscTransactionSale
        Me.TextBox_UnitPriceCategory.Text = _posItemBO.UnitPriceCategory
        Me.TextBox_ProductCode.Text = _posItemBO.ProductCode
        Me.CheckBox_FSAEligible.Checked = _posItemBO.FSAEligible

        'Disable controls if the item has been validated through Icon
        If _posItemBO.IsValidated Then
            SetActive(Me.CheckBox_FoodStamps, False)
        End If

        HideOptions()

        'initialize the hasChanges flag
        _hasChanges = False
    End Sub

    ''' <summary>
    ''' Hides unused fields as determined by Instance Data flags.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub HideOptions()
        ' NOTE: The ItemOverride.vb form should be kept in sync with any updates made to this logic.
        Me.CheckBox_CaseDiscount.Visible = InstanceDataDAO.IsFlagActive("ShowItemPOSCaseDiscount")
        Me.CheckBox_CouponMultiplier.Visible = InstanceDataDAO.IsFlagActive("ShowItemPOSCouponMultipler")

        Me.TextBox_IceTare.Visible = InstanceDataDAO.IsFlagActive("ShowItemPOSIceTare")
        Me.Label_IceTare.Visible = Me.TextBox_IceTare.Visible

        Me.TextBox_MiscTransRefund.Visible = InstanceDataDAO.IsFlagActive("ShowItemPOSMiscTransRefund")
        Me.Label_MiscTransRefund.Visible = Me.TextBox_MiscTransRefund.Visible

        Me.TextBox_MiscTransSale.Visible = InstanceDataDAO.IsFlagActive("ShowItemPOSMiscTransSale")
        Me.Label_MiscTransSale.Visible = Me.TextBox_MiscTransSale.Visible

        Me.TextBox_ProductCode.Visible = InstanceDataDAO.IsFlagActive("ShowItemPOSProductCode")
        Me.Label_ProductCode.Visible = Me.TextBox_ProductCode.Visible

        Me.TextBox_UnitPriceCategory.Visible = InstanceDataDAO.IsFlagActive("ShowItemPOSUnitPriceCategory")
        Me.Label_UnitPriceCategory.Visible = Me.TextBox_UnitPriceCategory.Visible

    End Sub

    ''' <summary>
    ''' validate the data and save the changes if it is valid
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ApplyChanges() As Boolean
        Dim success As Boolean = False
        Dim currentPOSItem As New POSItemBO
        Dim errorMsg As New StringBuilder
        Dim statusList As ArrayList
        Dim statusEnum As IEnumerator
        Dim currentStatus As POSItemStatus

        'set data into business object container
        currentPOSItem.ItemKey = _posItemBO.ItemKey
        currentPOSItem.FoodStamps = Me.CheckBox_FoodStamps.Checked
        currentPOSItem.PriceRequired = Me.CheckBox_PriceRequired.Checked
        currentPOSItem.QuantityProhibit = Me.CheckBox_QuantityProhibit.Checked
        currentPOSItem.QuantityRequired = Me.CheckBox_QuantityRequired.Checked
        currentPOSItem.CaseDiscount = Me.CheckBox_CaseDiscount.Checked
        currentPOSItem.CouponMultiplier = Me.CheckBox_CouponMultiplier.Checked
        currentPOSItem.FSAEligible = Me.CheckBox_FSAEligible.Checked
        currentPOSItem.ProductCode = Me.TextBox_ProductCode.Text

        currentPOSItem.IceTare = Me.TextBox_IceTare.Text
        currentPOSItem.GroupList = Me.TextBox_GroupList.Text
        currentPOSItem.MiscTransactionSale = Me.TextBox_MiscTransSale.Text
        currentPOSItem.MiscTransactionRefund = Me.TextBox_MiscTransRefund.Text

        'validate the data (required fields, formatting, etc)
        'ensure that NCR Group List is a valid numeric value
        If ScaleBO.ValidateNumericValue(Me.TextBox_GroupList.Text) Then
            currentPOSItem.GroupList = Me.TextBox_GroupList.Text
        ElseIf Me.TextBox_GroupList.Text IsNot Nothing AndAlso Not Me.TextBox_GroupList.Text.Trim.Equals("") Then
            errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_GroupList.Text))
            errorMsg.Append(Environment.NewLine)
        End If

        ' ensure MiscTransSale is a numeric value
        If ScaleBO.ValidateNumericValue(Me.TextBox_MiscTransSale.Text) Then
            currentPOSItem.MiscTransactionSale = Me.TextBox_MiscTransSale.Text
        ElseIf Me.TextBox_MiscTransSale.Text IsNot Nothing AndAlso Not Me.TextBox_MiscTransSale.Text.Trim.Equals("") Then
            errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_MiscTransSale.Text))
            errorMsg.Append(Environment.NewLine)
        End If

        ' ensure MiscTransRefund is a numeric value
        If ScaleBO.ValidateNumericValue(Me.TextBox_MiscTransRefund.Text) Then
            currentPOSItem.MiscTransactionRefund = Me.TextBox_MiscTransRefund.Text
        ElseIf Me.TextBox_MiscTransRefund.Text IsNot Nothing AndAlso Not Me.TextBox_MiscTransRefund.Text.Trim.Equals("") Then
            errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_MiscTransRefund.Text))
            errorMsg.Append(Environment.NewLine)
        End If

        ' ensure UnitPriceCategory is a numeric value
        If ScaleBO.ValidateNumericValue(Me.TextBox_UnitPriceCategory.Text) Then
            currentPOSItem.UnitPriceCategory = Me.TextBox_UnitPriceCategory.Text
        ElseIf Me.TextBox_UnitPriceCategory.Text IsNot Nothing AndAlso Not Me.TextBox_UnitPriceCategory.Text.Trim.Equals("") Then
            errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_UnitPriceCategory.Text))
            errorMsg.Append(Environment.NewLine)
        End If

        'validate other data (required fields, etc)
        statusList = currentPOSItem.ValidateData()

        'loop through possible validation erorrs and build message string containing all errors
        statusEnum = statusList.GetEnumerator
        While statusEnum.MoveNext
            currentStatus = CType(statusEnum.Current, POSItemStatus)

            Select Case currentStatus
                Case POSItemStatus.Error_GroupListNotIntegerFormat
                    errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_nonNegativeInteger"), Me.Label_GroupList.Text))
                    errorMsg.Append(Environment.NewLine)
                Case POSItemStatus.Error_QtyRequiredAndProhibitBothTrue
                    errorMsg.Append(String.Format(ResourcesItemHosting.GetString("msg_warning_QtyReqAndProhibit"), Me.CheckBox_QuantityProhibit.Text, Me.CheckBox_QuantityRequired.Text))
                    errorMsg.Append(Environment.NewLine)
                Case POSItemStatus.Error_IceTareNotIntegerFormat
                    errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_nonNegativeInteger"), Me.Label_IceTare.Text))
                    errorMsg.Append(Environment.NewLine)
                Case POSItemStatus.Error_MiscTransSaleNotIntegerFormat
                    errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_MiscTransSale.Text))
                    errorMsg.Append(Environment.NewLine)
                Case POSItemStatus.Error_MiscTransRefundNotIntegerFormat
                    errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_MiscTransRefund.Text))
                    errorMsg.Append(Environment.NewLine)
                Case POSItemStatus.Error_UnitPriceCategoryNotIntegerFormat
                    errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_nonNegativeInteger"), Me.Label_UnitPriceCategory.Text))
                    errorMsg.Append(Environment.NewLine)
            End Select

        End While

        If errorMsg.Length <= 0 Then
            'save data
            ItemDAO.UpdatePOSItemData(currentPOSItem)
            success = True

            'data has been saved - reset the edit flag
            _hasChanges = False

            ' Raise event - allows the data on the parent form to be refreshed
            RaiseEvent UpdateCallingForm(currentPOSItem)
        Else
            'display error msg
            MessageBox.Show(errorMsg.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        Return success
    End Function

End Class
