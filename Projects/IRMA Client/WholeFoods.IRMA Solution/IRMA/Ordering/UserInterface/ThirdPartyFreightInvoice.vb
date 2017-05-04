Imports log4net
Imports WholeFoods.IRMA.Common.BusinessLogic.ValidationBO
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess.ValidationDAO
Imports WholeFoods.IRMA.InvoiceThirdPartyFreight.BusinessLogic
Imports WholeFoods.IRMA.InvoiceThirdPartyFreight.DataAccess

Public Class frmThirdPartyFreightInvoice
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private pbLoading As Boolean

    ' Business object for the current Third Party Freight invoice being displayed
    Private _currentInvoice As ThirdPartyFreightInvoiceBO = Nothing

    Private OrderHeaderID As Integer = 0

    ' Flags to track changes to the data
    Private dataChanges As Boolean = False
    Private dataLoading As Boolean = False
    Private editExisting As Boolean = False


#Region "Property Accessors"
    Public Property CurrentInvoice() As ThirdPartyFreightInvoiceBO
        Get
            Return _currentInvoice
        End Get
        Set(ByVal value As ThirdPartyFreightInvoiceBO)
            _currentInvoice = value
        End Set
    End Property

    Public Property CurrentOrderHeaderID() As Integer
        Get
            Return OrderHeaderID
        End Get
        Set(ByVal value As Integer)
            OrderHeaderID = value
        End Set
    End Property

#End Region

#Region "Form Loading"

    ''' <summary>
    ''' Pass in the m_lOrderHeader_ID of the current order.
    ''' Load the form, pre-filling the data if the user is editing an existing invoice.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetInvoice(ByVal iOrderHeaderID As Integer)
        logger.Debug("SetInvoice entry: iOrderHeaderID=" + iOrderHeaderID.ToString)
        ' Set the data loading flag to true so that the data change events are not processed during initialization
        dataLoading = True

        OrderHeaderID = iOrderHeaderID

        _currentInvoice = ThirdPartyFreightDAO.Get3PartyFreightInvoice(OrderHeaderID)

        If _currentInvoice IsNot Nothing Then
            ' The user is editing an existing invoice.  Update the data on the screen.
            editExisting = True
        Else
            editExisting = False
            _currentInvoice = New ThirdPartyFreightInvoiceBO()
            _currentInvoice.PONum = OrderHeaderID
        End If
        PopulateInvoiceData()

        If _currentInvoice.UploadedDate <> Nothing Then
            ' make the form read-only
            TextBox_VendorKey.ReadOnly = True
            UltraNumericEditor_VendorID.ReadOnly = True
            TextBox_InvoiceNum.ReadOnly = True
            UltraDateTime_InvoiceDate.ReadOnly = True
            UltraNumericEditor_InvFrghtTot.ReadOnly = True

            TextBox_VendorKey.BackColor = UltraNumericEditor_PONum.Appearance.BackColor
            TextBox_InvoiceNum.BackColor = UltraNumericEditor_PONum.Appearance.BackColor

            Button_SearchVendor.Enabled = False

            TextBox_VendorKey.TabStop = False
            UltraNumericEditor_VendorID.TabStop = False
            TextBox_InvoiceNum.TabStop = False
            UltraDateTime_InvoiceDate.TabStop = False
            UltraNumericEditor_InvFrghtTot.TabStop = False

            txtUploadedDate.Visible = True
            lblUploaded.Visible = True
        Else
            ' enable the controls on the form
            TextBox_VendorKey.ReadOnly = False
            UltraNumericEditor_VendorID.ReadOnly = False
            TextBox_InvoiceNum.ReadOnly = False
            UltraDateTime_InvoiceDate.ReadOnly = False
            UltraNumericEditor_InvFrghtTot.ReadOnly = False

            TextBox_VendorKey.BackColor = Color.White
            TextBox_InvoiceNum.BackColor = Color.White

            Button_SearchVendor.Enabled = True

            TextBox_VendorKey.TabStop = True
            UltraNumericEditor_VendorID.TabStop = True
            TextBox_InvoiceNum.TabStop = True
            UltraDateTime_InvoiceDate.TabStop = True
            UltraNumericEditor_InvFrghtTot.TabStop = True

            txtUploadedDate.Visible = False
            lblUploaded.Visible = False
        End If

        ' Default the data changes flag to false
        dataChanges = False
        dataLoading = False

        logger.Debug("SetInvoice exit")
    End Sub

    ''' <summary>
    ''' Fill in the UI data for an existing invoice.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateInvoiceData()
        logger.Debug("PopulateInvoiceData entry")
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
        UltraNumericEditor_InvFrghtTot.Value = _currentInvoice.InvoiceCost

        If _currentInvoice.UploadedDate <> Nothing Then
            txtUploadedDate.Text = Format(_currentInvoice.UploadedDate, "M/d/yyyy")
        End If

        logger.Debug("PopulateInvoiceData exit")
    End Sub

#End Region

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
    Private Sub frmThirdPartyFreightInvoice_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("frmThirdPartyFreightInvoice_FormClosing entry")
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
        logger.Debug("frmThirdPartyFreightInvoice_FormClosing exit")
    End Sub

    ''' <summary>
    ''' Refreshes the _currentInvoice ThirdPartyFreightInvoiceBO with the current data values from the
    ''' UI screen.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateThirdPartyFreightBOWithFormData()
        logger.Debug("PopulateThirdPartyFreightBOWithFormData entry")

        _currentInvoice = New ThirdPartyFreightInvoiceBO()
        _currentInvoice.UpdateUserId = giUserID

        ' Note: The invoice cost for 3rd party freight invoices is set by the freight value on the UI
        If Me.UltraNumericEditor_InvFrghtTot.Value IsNot Nothing AndAlso Not Me.UltraNumericEditor_InvFrghtTot.Value Is DBNull.Value Then
            _currentInvoice.InvoiceCost = CDec(Me.UltraNumericEditor_InvFrghtTot.Value)
        Else
            _currentInvoice.InvoiceCost = -1
        End If

        If Not UltraDateTime_InvoiceDate.Value Is Nothing AndAlso Not UltraDateTime_InvoiceDate.Value Is DBNull.Value Then
            _currentInvoice.InvoiceDate = CDate(UltraDateTime_InvoiceDate.Value)
        Else
            _currentInvoice.InvoiceDate = Nothing
        End If

        _currentInvoice.InvoiceNum = Trim(TextBox_InvoiceNum.Text)

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

        logger.Debug("PopulateThirdPartyFreightBOWithFormData exit")
    End Sub

    ''' <summary>
    ''' Performs the actual save to the database, after all validation has completed.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveThirdPartyFreightInvoice() As Boolean
        logger.Debug("SaveThirdPartyFreightInvoice entry")
        Dim success As Boolean
        Dim validationCode As Integer
        If editExisting Then
            logger.Info("Saving updates to an existing Third Party Freight invoice: PONumber (OrderHeader_ID)=" + _currentInvoice.PONum.ToString() + ", VendorID=" + _currentInvoice.VendorID.ToString())
            validationCode = ThirdPartyFreightDAO.UpdateThirdPartyFreightInvoice(_currentInvoice)
        Else
            logger.Info("Saving a new Third Party Freight invoice: PONumber (OrderHeader_ID)=" + _currentInvoice.PONum.ToString() + ", VendorID=" + _currentInvoice.VendorID.ToString())
            validationCode = ThirdPartyFreightDAO.CreateThirdPartyFreightInvoice(_currentInvoice)
        End If

        ' Was the save a success?
        If validationCode <> 0 Or WholeFoods.IRMA.Common.DataAccess.ValidationDAO.IsErrorCode(validationCode) Then
            Dim validationBO As WholeFoods.IRMA.Common.BusinessLogic.ValidationBO
            ' An error was encountered.  The record was not saved.
            success = False
            validationBO = WholeFoods.IRMA.Common.DataAccess.ValidationDAO.GetValidationCodeDetails(validationCode)
            logger.Info("Third Party Freight invoice was not saved due to validation error.  ValidationCode=" + validationCode.ToString() + ", Description=" + validationBO.ValidationCodeDesc)
            MsgBox(String.Format(ResourcesOrdering.GetString("msg_SaveThirdPartyFreightInvoiceError"), Environment.NewLine, validationBO.ValidationCodeDesc.Replace("Closing Invoice Third Party Freight:", "")), MsgBoxStyle.Critical, Me.Text)
        Else
            ' The record was successfully saved
            logger.Info("Third Party Freight invoice was saved successfully.  ValidationCode=" + validationCode.ToString())
            success = True
        End If
        logger.Debug("SaveThirdPartyFreightInvoice exit: success=" + success.ToString)
        Return success
    End Function

    Private Function InvoiceExists() As Boolean


    End Function

    ''' <summary>
    ''' Save the changes for the current Third Party Freight invoice to the database.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ApplyChanges() As Boolean
        logger.Debug("ApplyChanges entry")
        Dim success As Boolean
        ' Populate the business object with the current data before saving the change.
        PopulateThirdPartyFreightBOWithFormData()

        ' Check for required fields and data entry errors
        Select Case _currentInvoice.ValidateData(editExisting)
            Case ThirdPartyFreightInvoiceValidation.Error_VendorRequired
                success = False
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_VendorKey.Text.Replace(":", "") + " or " + Me.Label_VendorID.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            Case ThirdPartyFreightInvoiceValidation.Error_VendorIdAndKeyMismatch
                success = False
                MsgBox(ResourcesOrdering.GetString("msg_VendorKeyAndVendorIdMismatch"), MsgBoxStyle.Critical, Me.Text)
            Case ThirdPartyFreightInvoiceValidation.Error_VendorKeyNotUnique
                success = False
                MsgBox(ResourcesOrdering.GetString("msg_VendorKeyMapsToDuplicateVendors"), MsgBoxStyle.Critical, Me.Text)
            Case ThirdPartyFreightInvoiceValidation.Error_PONumRequired
                success = False
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_PONum.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            Case ThirdPartyFreightInvoiceValidation.Error_InvoiceNumRequired
                success = False
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_InvoiceNum.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            Case ThirdPartyFreightInvoiceValidation.Error_InvoiceDateRequired
                success = False
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_Date.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            Case ThirdPartyFreightInvoiceValidation.Error_InvoiceFreightRequired
                success = False
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_InvFrghtTot.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            Case ThirdPartyFreightInvoiceValidation.Error_InvoiceCostRequired
                success = False
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_InvFrghtTot.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            Case ThirdPartyFreightInvoiceValidation.Error_InvoiceNumPreviouslyUsed
                success = False
                MsgBox(ResourcesOrdering.GetString("msg_InvoiceNumPreviouslyUsed"), MsgBoxStyle.Critical, Me.Text)
            Case ThirdPartyFreightInvoiceValidation.Error_InvalidVendor
                success = False
                MsgBox(ResourcesOrdering.GetString("msg_InvalidVendor"), MsgBoxStyle.Critical, Me.Text)

            Case ThirdPartyFreightInvoiceValidation.Warning_InvoiceCostOverThreshold
                'MsgBox(String.Format(ResourcesOrdering.GetString("msg_3PFInvoiceAmt"), Format(CDec(daoConfig.GetConfigValue("3PartyFreightInvoiceValidationAmt")), "0.00")), MsgBoxStyle.Information, Me.Text)
                If MsgBox(ResourcesOrdering.GetString("msg_3PFInvoiceAmt"), MsgBoxStyle.OkCancel, Me.Text) = MsgBoxResult.Ok Then
                    ' Perform the save
                    success = SaveThirdPartyFreightInvoice()
                    ' Reset the data changes flag since all changes have been saved or an error was displayed to the user.
                    dataChanges = False
                Else
                    success = False
                End If

            Case ThirdPartyFreightInvoiceValidation.Success
                ' Perform the save
                success = SaveThirdPartyFreightInvoice()
                ' Reset the data changes flag since all changes have been saved or an error was displayed to the user.
                SetInvoice(OrderHeaderID)
                dataChanges = False
            Case Else
                success = False
                MsgBox(ResourcesOrdering.GetString("msg_UnexpectedInvoiceStatus"), MsgBoxStyle.Critical, Me.Text)
        End Select

        logger.Debug("ApplyChanges exit: success=" + success.ToString)
        Return success
    End Function

#End Region

#Region "Data Change Events"

    Private Sub UltraNumericEditor_InvFrghtTot_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraNumericEditor_InvFrghtTot.ValueChanged
        logger.Debug("UltraNumericEditor_InvFrghtTot_ValueChanged entry")
        If Not dataLoading Then
            dataChanges = True
            ' The user updated the Freight amount, so the calculated values should be refreshed.
            ' If the value was set to NULL, set it to zero so the cast in the calculate functions works.
            If Me.UltraNumericEditor_InvFrghtTot.Value Is DBNull.Value Then
                Me.UltraNumericEditor_InvFrghtTot.Value = 0
            End If
        End If
        logger.Debug("UltraNumericEditor_InvFrghtTot_ValueChanged exit")
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

#End Region

End Class