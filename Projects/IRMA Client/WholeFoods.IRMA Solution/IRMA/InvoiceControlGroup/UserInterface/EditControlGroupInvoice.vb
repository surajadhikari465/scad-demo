Imports log4net
Imports WholeFoods.IRMA.Common.BusinessLogic.ValidationBO
Imports WholeFoods.IRMA.Common.DataAccess.ValidationDAO
Imports WholeFoods.IRMA.InvoiceControlGroup.BusinessLogic
Imports WholeFoods.IRMA.InvoiceControlGroup.DataAccess

Public Class EditControlGroupInvoice
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private pbLoading As Boolean

    ' Business object for the current control group invoice being displayed
    Private _currentInvoice As ControlGroupInvoiceBO = Nothing

    ' ID for the current control group this invoice is assigned to
    Private _currentControlGroupID As Integer = -1

    ' Flags to track changes to the data
    Private dataChanges As Boolean = False
    Private dataLoading As Boolean = False
    Private editExisting As Boolean = False


#Region "Property Accessors"
    Public Property CurrentInvoice() As ControlGroupInvoiceBO
        Get
            Return _currentInvoice
        End Get
        Set(ByVal value As ControlGroupInvoiceBO)
            _currentInvoice = value
        End Set
    End Property
    Public Property CurrentControlGroupID() As Integer
        Get
            Return _currentControlGroupID
        End Get
        Set(ByVal value As Integer)
            _currentControlGroupID = value
        End Set
    End Property

#End Region

#Region "Form Loading"
    ''' <summary>
    ''' Load the form, pre-filling the data if the user is editing an existing invoice.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub EditControlGroupInvoice_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        logger.Debug("EditControlGroupInvoice_Load entry")
        ' Set the data loading flag to true so that the data change events are not processed during initialization
        dataLoading = True

        If _currentInvoice IsNot Nothing Then
            ' The user is editing an existing invoice.  Update the data on the screen.
            editExisting = True
            PopulateInvoiceData()
            ' Calculate the invoice totals for display.
            CalculateTotals()
        Else
            ' The user is creating a new invoice.  
            editExisting = False
            DefaultInvoiceData()
        End If

        ' Update the UI controls based on the type of invoice that is currently selected.
        FormatFormControlsForDocumentType()

        ' Default the data changes flag to false
        dataChanges = False
        dataLoading = False

        logger.Debug("EditControlGroupInvoice_Load exit")
    End Sub

    ''' <summary>
    ''' Fill in the UI data for an existing invoice.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateInvoiceData()
        logger.Debug("PopulateInvoiceData entry")
        If _currentInvoice.InvoiceType = ControlGroupInvoiceType.Vendor Then
            RadioButton_DocumentType_Invoice.Checked = True
            RadioButton_DocumentType_3rdPartyFreight.Enabled = False
        ElseIf _currentInvoice.InvoiceType = ControlGroupInvoiceType.ThirdPartyFreight Then
            RadioButton_DocumentType_3rdPartyFreight.Checked = True
            RadioButton_DocumentType_Invoice.Enabled = False
        End If
        If _currentInvoice.VendorID <> -1 Then
            UltraNumericEditor_VendorID.Value = _currentInvoice.VendorID
        End If
        If _currentInvoice.VendorKey IsNot Nothing Then
            TextBox_VendorKey.Text = _currentInvoice.VendorKey
        End If
        If _currentInvoice.PONum <> -1 Then
            UltraNumericEditor_PONum.Value = _currentInvoice.PONum
        End If
        If _currentInvoice.InvoiceNum IsNot Nothing Then
            TextBox_InvoiceNum.Text = _currentInvoice.InvoiceNum
        End If
        If _currentInvoice.InvoiceDate <> Nothing Then
            UltraDateTime_InvoiceDate.Value = _currentInvoice.InvoiceDate
        End If
        CheckBox_CreditInvoice.Checked = _currentInvoice.CreditInv
        UltraNumericEditor_InvFrghtTot.Value = _currentInvoice.InvoiceFreight()
        UltraNumericEditor_InvoiceCost.Value = _currentInvoice.InvoiceCost.ToString()

        logger.Debug("PopulateInvoiceData exit")
    End Sub

    ''' <summary>
    ''' Fill in the default UI data for a new invoice.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DefaultInvoiceData()
        logger.Debug("DefaultInvoiceData entry")
        ' Default to vendor invoice
        RadioButton_DocumentType_Invoice.Checked = True
        logger.Debug("DefaultInvoiceData exit")
    End Sub

    ''' <summary>
    ''' Show/Hide UI controls and set tool tips based on the type of invoice being displayed.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormatFormControlsForDocumentType()
        logger.Debug("FormatFormControlsForDocumentType entry")

        If RadioButton_DocumentType_Invoice.Checked Then
            Me.Label_CostSubtotal.Visible = True
            Me.Label_CostTotal.Visible = True
            Me.TextBox_TotalInvoiceCost.Visible = True
            Me.Label_InvoiceCost.Visible = True
            Me.UltraNumericEditor_InvoiceCost.Visible = True
            Me.ToolTip1.SetToolTip(Me.Label_InvFrghtTot, "Invoice Freight, Not 3rd Party Freight")
        ElseIf RadioButton_DocumentType_3rdPartyFreight.Checked Then
            Me.Label_CostSubtotal.Visible = False
            Me.Label_CostTotal.Visible = False
            Me.TextBox_TotalInvoiceCost.Visible = False
            Me.Label_InvoiceCost.Visible = False
            Me.UltraNumericEditor_InvoiceCost.Value = 0
            Me.UltraNumericEditor_InvoiceCost.Visible = False
            Me.ToolTip1.SetToolTip(Me.Label_InvFrghtTot, "3rd Party Freight Total")
        End If

        logger.Debug("FormatFormControlsForDocumentType exit")
    End Sub
#End Region
    ''' <summary>
    ''' Recalculates the total invoice cost and invoice total amounts as the invoice cost and invoice freight
    ''' amounts are changed.  The updated totals are displayed on the UI.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CalculateTotals()
        logger.Debug("CalculateTotals entry")
        ' Get the current values entered by the user.
        Dim currentInvoiceCost As Decimal = CDec(Me.UltraNumericEditor_InvoiceCost.Value)
        Dim currentInvoiceFreight As Decimal = CDec(Me.UltraNumericEditor_InvFrghtTot.Value)

        ' Sum of the cost subtotal entries.  IRMA no longer supports multiple subteams for the same invoice,
        ' so this will always be equal to the cost subtotal value.
        Me.TextBox_TotalInvoiceCost.Text = VB6.Format(currentInvoiceCost, "#####0.00##")
        ' Sum of the freight total and the cost total.
        Me.TextBox_InvoiceTotal.Text = VB6.Format(currentInvoiceCost + currentInvoiceFreight, "#####0.00##")

        logger.Debug("CalculateTotals exit")
    End Sub

#Region "Search button events"
    ''' <summary>
    ''' Search for an existing vendor.  The Common vendor search screen is used to perform the search.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_SearchVendor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_SearchVendor.Click
        logger.Debug("Button_SearchVendor_Click entry")
        '-- Set glVendorID to none found before beginning the search
        glVendorID = 0
        '-- Set the search type
        giSearchType = iSearchVendorCompany

        '-- Open the search form
        Dim fSearch As New frmSearch
        fSearch.Text = "Vendor Search"
        fSearch.ShowDialog()
        fSearch.Close()
        fSearch.Dispose()

        '-- if the glVendorID is not zero, then something was found
        If glVendorID <> 0 Then
            Me.UltraNumericEditor_VendorID.Value = glVendorID
            Me.TextBox_VendorKey.Text = ""
        Else
            Me.UltraNumericEditor_VendorID.Value = DBNull.Value
            Me.TextBox_VendorKey.Text = ""
        End If

        logger.Debug("Button_SearchVendor_Click exit")
    End Sub

    ''' <summary>
    ''' Search for an existing purchase order.  The OrderSearch screen used to perform the search 
    ''' is shared with the ordering code.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_SearchOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_SearchOrder.Click
        logger.Debug("Button_SearchOrder_Click entry")
        '-- Set glOrderHeaderID to none found before beginning the search
        glOrderHeaderID = 0

        '-- Open the search form
        Dim fSearch As New frmOrdersSearch
        fSearch.ShowDialog()
        fSearch.Close()
        fSearch.Dispose()

        '-- if the glOrderHeaderID is not zero, then something was found
        If glOrderHeaderID <> 0 Then
            Me.UltraNumericEditor_PONum.Value = glOrderHeaderID
        Else
            Me.UltraNumericEditor_PONum.Value = DBNull.Value
        End If
        logger.Debug("Button_SearchOrder_Click exit")
    End Sub
#End Region

#Region "Form closing and exit button events"
    ''' <summary>
    ''' On exit, prompt the user to save their changes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exit.Click
        logger.Debug("Button_Exit_Click entry")
        ' Close the form.  The user is prompted to save any changes when the form is closed.
        Me.Close()
        logger.Debug("Button_Exit_Click exit")
    End Sub

    ''' <summary>
    ''' On form closing, prompt the user to save their changes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub EditControlGroupInvoice_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("EditControlGroupInvoice_FormClosing entry")
        ' Are there any changes that need to be saved to the current control invoice before the form is closed?
        If dataChanges Then
            'prompt the user to save changes
            If MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                If Not ApplyChanges() Then
                    ' Do not close the form; the save did not succeed.
                    e.Cancel = True
                End If
            End If
        End If
        logger.Debug("EditControlGroupInvoice_FormClosing exit")
    End Sub

    ''' <summary>
    ''' Refreshes the _currentInvoice ControlGroupInvoiceBO with the current data values from the
    ''' UI screen.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateControlGroupBOWithFormData()
        logger.Debug("PopulateControlGroupBOWithFormData entry")

        _currentInvoice = New ControlGroupInvoiceBO()
        _currentInvoice.UpdateUserId = giUserID

        If RadioButton_DocumentType_Invoice.Checked Then
            _currentInvoice.InvoiceType = ControlGroupInvoiceType.Vendor
            If Me.UltraNumericEditor_InvoiceCost.Value IsNot Nothing AndAlso Not Me.UltraNumericEditor_InvoiceCost.Value Is DBNull.Value Then
                _currentInvoice.InvoiceCost = CDec(Me.UltraNumericEditor_InvoiceCost.Value)
            Else
                _currentInvoice.InvoiceCost = -1
            End If
            If Me.UltraNumericEditor_InvFrghtTot.Value IsNot Nothing AndAlso Not Me.UltraNumericEditor_InvFrghtTot.Value Is DBNull.Value Then
                _currentInvoice.InvoiceFreight = CDec(Me.UltraNumericEditor_InvFrghtTot.Value)
            Else
                _currentInvoice.InvoiceFreight = -1
            End If
        ElseIf RadioButton_DocumentType_3rdPartyFreight.Checked Then
            _currentInvoice.InvoiceType = ControlGroupInvoiceType.ThirdPartyFreight
            ' Note: The invoice cost for 3rd party freight invoices is set by the freight value on the UI
            If Me.UltraNumericEditor_InvFrghtTot.Value IsNot Nothing AndAlso Not Me.UltraNumericEditor_InvFrghtTot.Value Is DBNull.Value Then
                _currentInvoice.InvoiceCost = CDec(Me.UltraNumericEditor_InvFrghtTot.Value)
            Else
                _currentInvoice.InvoiceCost = -1
            End If
            _currentInvoice.InvoiceFreight = 0
        End If

        If Not UltraDateTime_InvoiceDate.Value Is Nothing AndAlso Not UltraDateTime_InvoiceDate.Value Is DBNull.Value Then
            _currentInvoice.InvoiceDate = CDate(UltraDateTime_InvoiceDate.Value)
        Else
            _currentInvoice.InvoiceDate = Nothing
        End If

        _currentInvoice.InvoiceNum = Trim(TextBox_InvoiceNum.Text)

        _currentInvoice.InvoiceTotal = CDec(FixNumber(Me.TextBox_InvoiceTotal.Text))

        If Me.UltraNumericEditor_PONum.Value IsNot Nothing AndAlso Not Me.UltraNumericEditor_PONum.Value Is DBNull.Value Then
            _currentInvoice.PONum = CInt(UltraNumericEditor_PONum.Value)
        Else
            _currentInvoice.PONum = -1
        End If

        _currentInvoice.VendorKey = Trim(TextBox_VendorKey.Text)

        If Me.UltraNumericEditor_VendorID.Value IsNot Nothing AndAlso Not Me.UltraNumericEditor_VendorID.Value Is DBNull.Value Then
            _currentInvoice.VendorID = CInt(Me.UltraNumericEditor_VendorID.Value)
        Else
            _currentInvoice.VendorID = -1
        End If

        _currentInvoice.CreditInv = CheckBox_CreditInvoice.Checked

        logger.Debug("PopulateControlGroupBOWithFormData exit")
    End Sub

    ''' <summary>
    ''' Performs the actual save to the database, after all validation has completed.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveControlGroupInvoice() As Boolean
        logger.Debug("SaveControlGroupInvoice entry")
        Dim success As Boolean
        Dim validationCode As Integer
        If editExisting Then
            logger.Info("Saving updates to an existing control group invoice: OrderInvoice_ControlGroup_ID=" + _currentControlGroupID.ToString() + ", InvoiceType=" + _currentInvoice.InvoiceType.ToString() + ", PONumber (OrderHeader_ID)=" + _currentInvoice.PONum.ToString() + ", VendorID=" + _currentInvoice.VendorID.ToString())
            validationCode = ControlGroupDAO.UpdateControlGroupInvoice(_currentInvoice, _currentControlGroupID)
        Else
            logger.Info("Saving a new control group invoice: OrderInvoice_ControlGroup_ID=" + _currentControlGroupID.ToString() + ", InvoiceType=" + _currentInvoice.InvoiceType.ToString() + ", PONumber (OrderHeader_ID)=" + _currentInvoice.PONum.ToString() + ", VendorID=" + _currentInvoice.VendorID.ToString())
            validationCode = ControlGroupDAO.CreateControlGroupInvoice(_currentInvoice, _currentControlGroupID)
        End If

        ' Was the save a success?
        Dim valdiatonBO As WholeFoods.IRMA.Common.BusinessLogic.ValidationBO
        If validationCode <> 0 AndAlso WholeFoods.IRMA.Common.DataAccess.ValidationDAO.IsErrorCode(validationCode) Then
            ' An error was encountered.  The record was not saved.
            success = False
            valdiatonBO = WholeFoods.IRMA.Common.DataAccess.ValidationDAO.GetValidationCodeDetails(validationCode)
            logger.Info("Control group invoice was not saved due to validation error.  ValidationCode=" + validationCode.ToString() + ", Description=" + valdiatonBO.ValidationCodeDesc)
            MsgBox(String.Format(ResourcesControlGroup.GetString("msg_SaveControlGroupInvoiceError"), Environment.NewLine, valdiatonBO.ValidationCodeDesc.Replace("Closing Invoice Control Group:", "")), MsgBoxStyle.Critical, Me.Text)
        ElseIf validationCode <> 0 Then
            ' A warning was encountered.  Should the save be backed out?
            valdiatonBO = WholeFoods.IRMA.Common.DataAccess.ValidationDAO.GetValidationCodeDetails(validationCode)
            logger.Info("Control group invoice was saved, but a validation warning was returned.  ValidationCode=" + validationCode.ToString() + ", Description=" + valdiatonBO.ValidationCodeDesc)
            If MsgBox(String.Format(ResourcesControlGroup.GetString("msg_SaveControlGroupInvoiceWarning"), Environment.NewLine, valdiatonBO.ValidationCodeDesc.Replace("Closing Invoice Control Group:", "")), MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                ' the user wants to back out the save
                success = False
                logger.Info("User elected to delete the invoice due to the warning.  Control group invoice is being deleted: OrderInvoice_ControlGroup_ID=" + _currentControlGroupID.ToString() + ", InvoiceType=" + _currentInvoice.InvoiceType.ToString() + ", OrderHeader_ID=" + _currentInvoice.PONum.ToString())
                ControlGroupDAO.DeleteControlGroupInvoice(_currentInvoice, _currentControlGroupID)
            Else
                success = True
                logger.Info("User elected to continue with the save of the invoice after seeing the warning.")
            End If
        Else
            ' The record was successfully saved
            logger.Info("Control group invoice was saved successfully.  ValidationCode=" + validationCode.ToString())
            success = True
        End If
        logger.Debug("SaveControlGroupInvoice exit: success=" + success.ToString)
        Return success
    End Function

    ''' <summary>
    ''' Save the changes for the current control group invoice to the database.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ApplyChanges() As Boolean
        logger.Debug("ApplyChanges entry")
        Dim success As Boolean
        ' Populate the business object with the current data before saving the change.
        PopulateControlGroupBOWithFormData()

        ' Check for required fields and data entry errors
        Select Case _currentInvoice.ValidateData(editExisting, _currentControlGroupID)
            Case ControlGroupInvoiceValidation.Error_VendorRequired
                success = False
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_VendorKey.Text.Replace(":", "") + " or " + Me.Label_VendorID.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            Case ControlGroupInvoiceValidation.Error_VendorIdAndKeyMismatch
                success = False
                MsgBox(ResourcesControlGroup.GetString("msg_VendorKeyAndVendorIdMismatch"), MsgBoxStyle.Critical, Me.Text)
            Case ControlGroupInvoiceValidation.Error_VendorKeyNotUnique
                success = False
                MsgBox(ResourcesControlGroup.GetString("msg_VendorKeyMapsToDuplicateVendors"), MsgBoxStyle.Critical, Me.Text)
            Case ControlGroupInvoiceValidation.Error_PONumRequired
                success = False
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_PONum.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            Case ControlGroupInvoiceValidation.Error_InvoiceNumRequired
                success = False
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_InvoiceNum.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            Case ControlGroupInvoiceValidation.Error_InvoiceDateRequired
                success = False
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_Date.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            Case ControlGroupInvoiceValidation.Error_InvoiceFreightRequired
                success = False
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_InvFrghtTot.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            Case ControlGroupInvoiceValidation.Error_InvoiceCostRequired
                success = False
                If _currentInvoice.InvoiceType = ControlGroupInvoiceType.Vendor Then
                    MsgBox(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_InvoiceCost.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
                Else
                    MsgBox(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_InvFrghtTot.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
                End If
            Case ControlGroupInvoiceValidation.Error_DupInvoiceInControlGroup
                success = False
                logger.Info("Control group invoice was not saved due to validation error.  The type of invoice already exists in the current control group for the purchase order.  The existing invoice should be edited instead of creating a new entry.")
                MsgBox(String.Format(ResourcesControlGroup.GetString("msg_SaveControlGroupInvoiceError"), Environment.NewLine, ResourcesControlGroup.GetString("msg_DuplicateInvoiceError")), MsgBoxStyle.Critical, Me.Text)
            Case ControlGroupInvoiceValidation.Warning_DupInvoiceInOpenControlGroup
                logger.Info("User presented warning message.  The type of invoice already exists for the purchase order in another open control group.")
                If MsgBox(ResourcesControlGroup.GetString("msg_DuplicateInvoiceWarning"), MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                    ' Perform the save
                    success = SaveControlGroupInvoice()
                Else
                    ' Do not save
                    logger.Info("User decided not to save the invoice to the control group after reviewing the warning.")
                    success = False
                End If
            Case ControlGroupInvoiceValidation.Success
                ' Perform the save
                success = SaveControlGroupInvoice()
        End Select

        ' Reset the data changes flag since all changes have been saved or an error was displayed to the user.
        dataChanges = False

        logger.Debug("ApplyChanges exit: success=" + success.ToString)
        Return success
    End Function

#End Region

#Region "Data Change Events"
    Private Sub RadioButton_DocumentType_Invoice_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_DocumentType_Invoice.CheckedChanged
        logger.Debug("RadioButton_DocumentType_Invoice_CheckedChanged entry")
        If Not dataLoading Then
            dataChanges = True
            ' The invoice type changed so controls on the form may need to be enabled/disabled.
            FormatFormControlsForDocumentType()
            ClearData()
        End If
        logger.Debug("RadioButton_DocumentType_Invoice_CheckedChanged exit")
    End Sub

    Private Sub RadioButton_DocumentType_3rdPartyFreight_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_DocumentType_3rdPartyFreight.CheckedChanged
        logger.Debug("RadioButton_DocumentType_3rdPartyFreight_CheckedChanged entry")
        If Not dataLoading Then
            dataChanges = True
            ' The invoice type changed so controls on the form may need to be enabled/disabled.
            FormatFormControlsForDocumentType()
            ClearData()
        End If
        logger.Debug("RadioButton_DocumentType_3rdPartyFreight_CheckedChanged exit")
    End Sub

    Private Sub UltraNumericEditor_InvFrghtTot_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraNumericEditor_InvFrghtTot.ValueChanged
        logger.Debug("UltraNumericEditor_InvFrghtTot_ValueChanged entry")
        If Not dataLoading Then
            dataChanges = True
            ' The user updated the Freight amount, so the calculated values should be refreshed.
            ' If the value was set to NULL, set it to zero so the cast in the calculate functions works.
            If Me.UltraNumericEditor_InvFrghtTot.Value Is DBNull.Value Then
                Me.UltraNumericEditor_InvFrghtTot.Value = 0
            End If
            CalculateTotals()
        End If
        logger.Debug("UltraNumericEditor_InvFrghtTot_ValueChanged exit")
    End Sub

    Private Sub UltraNumericEditor_InvoiceCost_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraNumericEditor_InvoiceCost.ValueChanged
        logger.Debug("UltraNumericEditor_InvoiceCost_ValueChanged entry")
        If Not dataLoading Then
            dataChanges = True
            ' The user updated the invoice amount, so the calculated values should be refreshed.
            ' If the value was set to NULL, set it to zero so the cast in the calculate functions works.
            If Me.UltraNumericEditor_InvoiceCost.Value Is DBNull.Value Then
                Me.UltraNumericEditor_InvoiceCost.Value = 0
            End If
            CalculateTotals()
        End If
        logger.Debug("UltraNumericEditor_InvoiceCost_ValueChanged exit")
    End Sub

    Private Sub TextBox_VendorKey_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_VendorKey.TextChanged
        logger.Debug("TextBox_VendorKey_TextChanged entry")
        If Not dataLoading Then
            dataChanges = True
        End If
        logger.Debug("TextBox_VendorKey_TextChanged exit")
    End Sub

    Private Sub UltraNumericEditor_VendorID_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraNumericEditor_VendorID.ValueChanged
        logger.Debug("UltraNumericEditor_VendorID_ValueChanged entry")
        If Not dataLoading Then
            dataChanges = True
        End If
        logger.Debug("UltraNumericEditor_VendorID_ValueChanged exit")
    End Sub

    Private Sub UltraNumericEditor_PONum_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraNumericEditor_PONum.ValueChanged
        logger.Debug("UltraNumericEditor_PONum_ValueChanged entry")
        If Not dataLoading Then
            dataChanges = True
        End If
        logger.Debug("UltraNumericEditor_PONum_ValueChanged exit")
    End Sub

    Private Sub TextBox_InvoiceNum_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_InvoiceNum.TextChanged
        logger.Debug("TextBox_InvoiceNum_TextChanged entry")
        If Not dataLoading Then
            dataChanges = True
        End If
        logger.Debug("TextBox_InvoiceNum_TextChanged exit")
    End Sub

    Private Sub UltraDateTime_InvoiceDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraDateTime_InvoiceDate.ValueChanged
        logger.Debug("UltraDateTime_InvoiceDate_ValueChanged entry")
        If Not dataLoading Then
            dataChanges = True
        End If
        logger.Debug("UltraDateTime_InvoiceDate_ValueChanged exit")
    End Sub

    Private Sub CheckBox_CreditInvoice_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_CreditInvoice.CheckedChanged
        logger.Debug("CheckBox_CreditInvoice_CheckedChanged entry")
        If Not dataLoading Then
            dataChanges = True
        End If
        logger.Debug("CheckBox_CreditInvoice_CheckedChanged exit")
    End Sub

    Private Sub ClearData()
        TextBox_VendorKey.Text = ""
        UltraNumericEditor_VendorID.Value = Nothing
        UltraNumericEditor_PONum.Value = Nothing
        TextBox_InvoiceNum.Text = ""
        UltraDateTime_InvoiceDate.Value = Nothing
        CheckBox_CreditInvoice.CheckState = CheckState.Unchecked
        UltraNumericEditor_InvFrghtTot.Value = 0
        UltraNumericEditor_InvoiceCost.Value = 0
    End Sub

#End Region


End Class
