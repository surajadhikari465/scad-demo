Imports System.Windows.Forms
Imports System
Imports System.Text
Imports System.Linq
Imports System.Data
Imports Microsoft.WindowsCE.Forms
Imports WholeFoods.Mobile.IRMA.Common
Imports WholeFoods.Mobile.IRMA.Enums
Imports System.Runtime.InteropServices
Imports System.ServiceModel
Imports System.Text.RegularExpressions


Public Class InvoiceData

    Private session As Session
    Private serviceFault As ParsedCFFaultException = Nothing
    Private serviceCallSuccess As Boolean
    Private order As Order
    Private itemsReceived As Boolean
    Private charges As New DataTable("Charges")
    Private allocatedCharges As New Decimal
    Private nonAllocatedCharges As New Decimal
    Private eInvoiceNonAllocatedCharges As New Decimal
    Private lineItemCharges As New Decimal
    Private serverDateTime As New DateTime
    Private refusalReasonCodes As ReasonCode()
    Private invoiceCharges As InvoiceCharge()
    Private currencies As Currency()
    Private currencyCode As String
    Private enumDocumentType As DocumentType
    Private eInvoiceInvoiceTotal As Decimal
    Private sustainabilityRankingRequiredItems As Integer
    Private orderSuspended As Boolean
    'Private refusedTotalCost As Decimal

    Private _menuTitle As String = "Receive Order"

    Public WriteOnly Property MenuText() As String
        Set(ByVal value As String)
            _menuTitle = value
        End Set
    End Property

    Public Sub New(ByRef session As Session, ByRef order As Order)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.session = session
        Me.order = order

        AlignText()

    End Sub

    Private Sub InvoiceData_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Cursor.Current = Cursors.WaitCursor

        MenuItemReceiveOrder.Text = _menuTitle

        Try
            ' There are several service calls needed to initialize this form.  If any of them fails, the user will go back to the last
            ' form she was on.
            serviceCallSuccess = True

            ' Call UpdateOrderHeaderCosts and refresh the order object (would be nice to be able to do this without having to call the service library again).
            session.WebProxyClient.UpdateOrderHeaderCosts(order.OrderHeader_ID)

            ' Refresh the order object to stay in sync with the database.
            order = session.WebProxyClient.GetOrder(order.OrderHeader_ID)

            ' Get the server date (in case the handheld has reset back to an earlier date).  This date is already timezone adjusted when it is returned from the service.
            serverDateTime = session.WebProxyClient.GetSystemDate()

            ' Get the number of unranked sustainability-ranking-required items.
            sustainabilityRankingRequiredItems = session.WebProxyClient.CountSustainabilityRankingRequiredItems(order.OrderHeader_ID)

            ' Get all currencies used by the region.
            currencies = session.WebProxyClient.GetCurrencies()

            ' Populate the list of receiving refusal codes.
            refusalReasonCodes = session.WebProxyClient.GetReasonCodesByType(Enums.ReasonCodeType.RR.ToString())

            ' Get the total refused item cost.
            ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
            'refusedTotal = session.WebProxyClient.GetRefusedTotal(order.OrderHeader_ID)

            ' Explicitly handle service faults, timeouts, and connection failures.  If this initialization block fails, the user will
            ' fall back to the last form she was on.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "InvoiceData_Load")
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "InvoiceData_Load")
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "InvoiceData_Load")
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Finally
            Cursor.Current = Cursors.Default
        End Try

        If Not serviceCallSuccess Then
            ' One of the initialization service calls failed.  End the method and close the form.
            Exit Sub
        End If

        ' Check for unranked sustainability-required items (this check is not required for transfer orders).
        If order.OrderType_Id <> Enums.enumOrderType.Transfer And sustainabilityRankingRequiredItems > 0 Then
            MsgBox("Sustainability ranking is required for some items on this order.  Please close it in the IRMA client.", MsgBoxStyle.Critical, Me.Text)
            Me.DialogResult = Windows.Forms.DialogResult.Yes
        End If

        ' Load the currency ComboBox with all available currencies.
        LoadCurrencies()

        ' Load any invoice charges.
        LoadChargesTable()

        ' Display the subteam name.
        LabelSubteam.Text = order.Transfer_To_SubTeamName

        ' If an eInvoice is present, save a couple key values so that they can be re-used if needed.
        If order.EinvoiceID <> Nothing And order.EinvoiceID > 0 Then

            ' Save the original eInvoice invoice total if one is available.  This is used if the user needs to reparse the eInvoice.
            eInvoiceInvoiceTotal = order.InvoiceCost

            ' Save the non-allocated charges from the eInvoice.  This matches the client behavior when the user manually adds charges
            ' to an eInvoice order.
            eInvoiceNonAllocatedCharges = nonAllocatedCharges

        End If

        ' Determine if the order is opened or closed.
        If order.CloseDate = Nothing Then

            ' Order is open.  Check if the vendor is eInvoice required.  If it is, and there is no eInvoice loaded, the order must be closed as type None.
            If order.EinvoiceRequired And order.EinvoiceID = Nothing Then
                MsgBox("An eInvoice is required for this vendor, but none is loaded.  Document type None will be chosen for this invoice.", MsgBoxStyle.Information, Me.Text)
                SetupFormFields(DocumentType.None, False)
                RadioButtonInvoice.Enabled = False
                RadioButtonOther.Enabled = False

            Else
                ' The form should be loaded by default as document type Invoice.  Populate the form with whatever data is available.
                SetupFormFields(DocumentType.Invoice, False)
            End If

        Else
            ' Order is closed.
            If order.CloseDate <> Nothing And order.InvoiceNumber <> Nothing Then

                ' Set to Invoice
                RadioButtonInvoice.Checked = True
                SetupFormFields(Enums.DocumentType.Invoice, True)

            ElseIf order.CloseDate <> Nothing And order.VendorDocID <> Nothing And order.InvoiceNumber = Nothing Then

                ' Set to Other
                RadioButtonOther.Checked = True
                SetupFormFields(DocumentType.Other, True)

            ElseIf order.CloseDate <> Nothing And order.InvoiceNumber = Nothing And order.VendorDocID = Nothing Then

                'Set to None
                RadioButtonNone.Checked = True
                SetupFormFields(DocumentType.None, True)

            End If

            MenuItemCloseOrReOpenOrder.Text = "Re-Open Order"

        End If

        ' Determine if any items have been received - if not, then Refuse functionality will be activated.
        Dim query = From orderItem In order.OrderItems _
                    Where orderItem.QuantityReceived > 0 Or orderItem.Total_Weight > 0 _
                    Select True
        itemsReceived = If(query.Count = 0, False, True)

        If Not itemsReceived Then
            If order.Sent Then
                ButtonRefuse.Enabled = True
                MenuItemCloseOrReOpenOrder.Enabled = False
            End If
        End If
    End Sub

    ''' <summary>
    ''' Populates the InvoiceData form with existing Invoice Information.
    ''' </summary>
    ''' <remarks>If order is not closed, the information will be blank.</remarks>
    Private Sub PopulateInvoiceDataForm()

        TextBoxInvoiceNumber.Text = order.InvoiceNumber

        If order.InvoiceDate <> Nothing Then
            DateTimePickerInvoiceDate.Value = order.InvoiceDate
        Else
            DateTimePickerInvoiceDate.Value = serverDateTime.Date
        End If

        ' Logic for populating the invoice total TextBox:
        ' >> If the order is transfer or distribution, then invoice cost = ordered cost.
        ' >> If an eInvoice is loaded, then invoice cost is the eInvoice total plus any eInvoice non-allocated charges.
        ' >> If there is no eInvoice, leave the field blank.
        ' >> Otherwise, set it to whatever value the user has typed in.
        If order.OrderType_Id = Enums.enumOrderType.Transfer Or order.OrderType_Id = Enums.enumOrderType.Distribution Then
            TextBoxInvoiceTotal.Text = FormatNumber(order.OrderedCost, 2)
        ElseIf order.EinvoiceID <> Nothing Then
            TextBoxInvoiceTotal.Text = FormatNumber(eInvoiceInvoiceTotal + eInvoiceNonAllocatedCharges, 2)
        ElseIf order.InvoiceCost = 0 Then
            TextBoxInvoiceTotal.Text = String.Empty
        Else
            TextBoxInvoiceTotal.Text = FormatNumber(order.InvoiceCost, 2)
        End If

        TextBoxChargesTotal.Text = FormatNumber(CalculateChargesTotal(), 2).ToString()
        TextBoxSubteam.Text = FormatNumber(CalculateSubteamTotal(), 2).ToString()

        ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
        'TextBoxRefusedTotal.Text = FormatNumber(refusedTotal, 2).ToString()

        TextBoxCostDifference.Text = FormatNumber(CalculateCostDifference(), 2).ToString()

        Dim currencyCodeQuery = From currency In currencies _
                                Where currency.CurrencyID = order.CurrencyID _
                                Select currency.CurrencyCode

        ' CurrencyID can be null (strangely enough).  If the currencyCodeQuery returns no results (null), then set the currency to USD (or GBP if region is UK).
        If currencyCodeQuery.Count = 0 Then
            Select Case session.Region
                Case "EU", "UK"
                    ComboBoxCurrency.SelectedItem = "GBP"
                Case Else
                    ComboBoxCurrency.SelectedItem = "USD"
            End Select
        Else
            ComboBoxCurrency.SelectedItem = currencyCodeQuery.Single
        End If

    End Sub

    Private Sub PopulateOtherDataForm()

        TextBoxInvoiceNumber.Text = order.VendorDocID

        If order.VendorDocDate <> Nothing Then
            DateTimePickerInvoiceDate.Value = order.VendorDocDate
        Else
            DateTimePickerInvoiceDate.Value = serverDateTime.Date
        End If

    End Sub

    ''' <summary>
    ''' Determines which fields on the form are enabled and greyed out based on the document type.
    ''' </summary>
    ''' <param name="documentType">DocumentType enum</param>
    ''' <param name="closed">Is Order Closed?</param>
    ''' <remarks></remarks>
    Private Sub SetupFormFields(ByVal documentType As Enums.DocumentType, ByVal closed As Boolean)

        If closed Then
            ' If order is closed, all fields are read only or disabled.
            RadioButtonInvoice.Enabled = False
            RadioButtonOther.Enabled = False
            RadioButtonNone.Enabled = False
            TextBoxInvoiceNumber.ReadOnly = True
            DateTimePickerInvoiceDate.Enabled = False
            TextBoxInvoiceTotal.ReadOnly = True
            TextBoxChargesTotal.ReadOnly = True
            TextBoxSubteam.ReadOnly = True
            TextBoxCostDifference.ReadOnly = True
            ButtonAddCharge.Enabled = False
            ButtonRemoveCharge.Enabled = False
            ButtonRefuse.Enabled = False
            ButtonReparseEinvoice.Enabled = False

        Else
            ' Order is not closed: determine which fields are enabled based on document type.
            Select Case documentType
                Case documentType.Invoice
                    enumDocumentType = documentType.Invoice
                    RadioButtonInvoice.Checked = True

                    ' Active fields:
                    If order.EinvoiceRequired Then
                        TextBoxInvoiceNumber.ReadOnly = True
                        DateTimePickerInvoiceDate.Enabled = False
                        TextBoxInvoiceTotal.ReadOnly = True
                    Else
                        TextBoxInvoiceNumber.ReadOnly = False
                        DateTimePickerInvoiceDate.Enabled = True
                        TextBoxInvoiceTotal.ReadOnly = False
                    End If

                    ButtonAddCharge.Enabled = True
                    ButtonRemoveCharge.Enabled = True

                    If order.EinvoiceID <> Nothing And order.EinvoiceID > 0 Then
                        ButtonReparseEinvoice.Enabled = True
                    Else
                        ButtonReparseEinvoice.Enabled = False
                    End If

                    ' Read-Only/Calculated fields:
                    TextBoxChargesTotal.ReadOnly = True
                    TextBoxSubteam.ReadOnly = True
                    TextBoxRefusedTotal.ReadOnly = True
                    TextBoxCostDifference.ReadOnly = True

                Case documentType.Other
                    enumDocumentType = documentType.Other
                    RadioButtonOther.Checked = True

                    ' Active fields:
                    TextBoxInvoiceNumber.ReadOnly = False
                    TextBoxInvoiceNumber.Text = String.Empty

                    DateTimePickerInvoiceDate.Enabled = True

                    ButtonAddCharge.Enabled = True
                    ButtonRemoveCharge.Enabled = True

                    ' Read-Only/Calculated fields:
                    TextBoxInvoiceTotal.ReadOnly = True
                    TextBoxInvoiceTotal.Text = String.Empty

                    TextBoxChargesTotal.ReadOnly = True
                    TextBoxChargesTotal.Text = String.Empty

                    TextBoxSubteam.ReadOnly = True
                    TextBoxSubteam.Text = String.Empty

                    TextBoxRefusedTotal.ReadOnly = True
                    TextBoxRefusedTotal.Text = String.Empty

                    TextBoxCostDifference.ReadOnly = True
                    TextBoxCostDifference.Text = String.Empty

                    ButtonReparseEinvoice.Enabled = False

                Case documentType.None
                    enumDocumentType = documentType.None
                    RadioButtonNone.Checked = True

                    ' All fields are read-only, inactive, and empty.
                    TextBoxInvoiceNumber.ReadOnly = True
                    TextBoxInvoiceNumber.Text = Nothing

                    DateTimePickerInvoiceDate.Enabled = False
                    DateTimePickerInvoiceDate.Value = serverDateTime.Date

                    TextBoxInvoiceTotal.ReadOnly = True
                    TextBoxInvoiceTotal.Text = String.Empty

                    TextBoxChargesTotal.ReadOnly = True
                    TextBoxChargesTotal.Text = String.Empty

                    TextBoxSubteam.ReadOnly = True
                    TextBoxSubteam.Text = String.Empty

                    TextBoxRefusedTotal.ReadOnly = True
                    TextBoxRefusedTotal.Text = String.Empty

                    TextBoxCostDifference.ReadOnly = True
                    TextBoxCostDifference.Text = String.Empty

                    ButtonAddCharge.Enabled = False
                    ButtonRemoveCharge.Enabled = False
                    ButtonReparseEinvoice.Enabled = False
            End Select
        End If

    End Sub

    ''' <summary>
    ''' Loads the charges table with any allocated or non-allocated charges existing on the order.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadChargesTable()

        ' Initialization.
        allocatedCharges = 0.0
        nonAllocatedCharges = 0.0
        lineItemCharges = 0.0

        ' Create table columns.  Clear everything first in case this is a reload.
        charges.Rows.Clear()
        charges.Columns.Clear()
        charges.Columns.Add("Type")
        charges.Columns.Add("GLAccount")
        charges.Columns.Add("Description")
        charges.Columns.Add("Value")

        ' Get the charges, add them to the DataTable, and set the source of the DataGrid.
        Cursor.Current = Cursors.WaitCursor

        Try
            ' Attempt a service call to load order invoice charges.
            serviceCallSuccess = True

            invoiceCharges = session.WebProxyClient.GetOrderInvoiceCharges(order.OrderHeader_ID)


            ' Explicitly handle service faults, timeouts, and connection failures.  If we can't load charges, the user will
            ' fall back to the last form she was on.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "LoadChargesTable")
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "LoadChargesTable")
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "LoadChargesTable")
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Finally
            Cursor.Current = Cursors.Default

        End Try

        If Not serviceCallSuccess Then
            ' Unable to retrieve the charges from the database.  End the method and return to the previous form.
            Exit Sub
        End If

        For Each charge As InvoiceCharge In invoiceCharges
            ' Add the charge to the table.
            charges.Rows.Add(If(charge.SACType = "Not Allocated", "Not Alloc.", charge.SACType), If(charge.GLPurchaseAccount = 0, String.Empty, charge.GLPurchaseAccount.ToString()), charge.Description, FormatNumber(charge.ChargeValue, 2))

            ' Sum the charges based on type.
            Select Case charge.SACType.ToString()
                Case "Allocated"
                    ' For SACType allocated, only sum charges.  Disregard allowances.
                    If charge.IsAllowance = "False" Then
                        allocatedCharges = allocatedCharges + charge.ChargeValue
                    End If
                Case "Not Allocated"
                    nonAllocatedCharges = nonAllocatedCharges + charge.ChargeValue
                Case "Line Item"
                    lineItemCharges = lineItemCharges + charge.ChargeValue
            End Select
        Next

        DataGridCharges.DataSource = charges

        ' Re-calculate the affected totals.
        TextBoxChargesTotal.Text = FormatNumber(CalculateChargesTotal(), 2).ToString()
        TextBoxCostDifference.Text = FormatNumber(CalculateCostDifference(), 2).ToString()
        TextBoxSubteam.Text = FormatNumber(CalculateSubteamTotal(), 2).ToString()

    End Sub

    Private Function CalculateChargesTotal() As Decimal
        Return FormatNumber(allocatedCharges + nonAllocatedCharges + lineItemCharges, 2)
    End Function

    Private Function CalculateCostDifference() As Decimal
        ' The logic in this method comes directly from OrderStatus.vb in the client.  It should be kept in sync with that code.

        Dim totalCharges As Decimal = nonAllocatedCharges + allocatedCharges + lineItemCharges

        ' First subtract the charges from the Invoice Cost and compare it to the Adjusted Received Cost (rounded
        ' to 2 decimal places to prevent rounding errors).  If they are same, then return 0 for the difference.
        If order.InvoiceCost - totalCharges = FormatNumber(order.AdjustedReceivedCost, 2) Then
            Return 0.0
        Else
            ' Otherwise, the business logic is that the difference is calculated by subtracting the Adjusted Received Cost
            ' from the Invoice Total, and then subtracting charges.
            If allocatedCharges + nonAllocatedCharges > 0 Then
                Return FormatNumber((order.InvoiceCost - order.AdjustedReceivedCost) - totalCharges, 2)
            Else
                Return FormatNumber((order.InvoiceCost - order.AdjustedReceivedCost) + totalCharges, 2)
            End If
        End If
    End Function

    Private Function CalculateSubteamTotal() As Decimal
        ' Subteam cost is Invoice Total Cost - Freight - Total Charges.
        Dim totalCharges As Decimal = nonAllocatedCharges + allocatedCharges + lineItemCharges
        Return order.InvoiceCost - order.InvoiceFreight - totalCharges
    End Function

    ''' <summary>
    ''' Populates order object with data from the form which the user filled out
    ''' and validates the data.
    ''' </summary>
    ''' <remarks>Is needed prior to calling CloseOrder method.</remarks>
    Private Sub PopulateOrderInformation()

        If enumDocumentType = DocumentType.Invoice Then

            order.InvoiceNumber = TextBoxInvoiceNumber.Text
            order.InvoiceDate = DateTimePickerInvoiceDate.Value

            ' Non-allocated charges and invoice freight are not considered for matching purposes, so they need to
            ' be subtracted from the invoice total before the order is closed.  This follows the logic in OrderStatus.vb
            ' in the IRMA client.
            order.InvoiceCost = TextBoxInvoiceTotal.Text - nonAllocatedCharges - order.InvoiceFreight

            order.VendorDocID = Nothing
            order.VendorDocDate = Nothing

        ElseIf enumDocumentType = DocumentType.Other Then

            order.InvoiceNumber = Nothing
            order.InvoiceDate = Nothing
            order.InvoiceCost = Nothing
            order.VendorDocID = TextBoxInvoiceNumber.Text
            order.VendorDocDate = DateTimePickerInvoiceDate.Value

        ElseIf enumDocumentType = DocumentType.None Then

            order.InvoiceNumber = Nothing
            order.InvoiceDate = Nothing
            order.InvoiceCost = Nothing
            order.VendorDocID = Nothing
            order.VendorDocDate = Nothing

        End If

        ' Partial shipment is always false since that is chosen outside of the InvoiceData form.
        order.PartialShipment = False

    End Sub

    Private Sub MenuItemReceiveOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemReceiveOrder.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub MenuItemCloseOrReOpenOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemCloseOrReOpenOrder.Click

        ' Is order closed?
        If order.CloseDate = Nothing Then

            ' The order is not closed.  First, check that the invoice number is alphanumeric.
            If AlphanumericValidationFails(TextBoxInvoiceNumber.Text) Then
                MsgBox("The invoice number may not contain the character '|'.  Please enter an alphanumeric value.", MsgBoxStyle.Information, "Invalid Invoice#")
                TextBoxInvoiceNumber.SelectAll()
                Exit Sub
            End If

            ' Second, check that the invoice date is not in the future.  Then, verify that the user wants to close 
            ' the order, and display the invoice date for verification.
            If DateTimePickerInvoiceDate.Value.Date > serverDateTime.Date Then
                MsgBox("The invoice date cannot be a future date.  Please choose another invoice date.", MsgBoxStyle.Information, "Invalid Invoice Date")
                DateTimePickerInvoiceDate.Value = serverDateTime.Date
                DateTimePickerInvoiceDate.Focus()
                Exit Sub
            End If

            Dim msgResponse As MsgBoxResult = Messages.CloseOrder(DateTimePickerInvoiceDate.Value, enumDocumentType)
            If msgResponse = MsgBoxResult.Cancel Then
                Exit Sub
            End If

            ' Validate invoice fields before closing
            If Not ValidateInvoiceInformation() Then
                Exit Sub
            End If

            ' Verify user wants to receive the order even if there are items not received
            If Not ItemsNotReceived() Then
                Exit Sub
            End If

            Cursor.Current = Cursors.WaitCursor

            ' Populate order object with data from the form.
            PopulateOrderInformation()


            Try
                ' Make sure invoice number does not exist already for that vendor.
                serviceCallSuccess = True

                If DuplicateInvoiceNumber() Then
                    Cursor.Current = Cursors.Default
                    Exit Sub
                End If


                ' Explicitly handle service faults, timeouts, and connection failures.  If a failure occurs, allow the user to retry
                ' the last action.
            Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                Dim err As New ErrorHandler(serviceFault)
                err.ShowErrorNotification()
                serviceCallSuccess = False

            Catch ex As TimeoutException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "DuplicateInvoiceNumber")
                serviceCallSuccess = False

            Catch ex As CommunicationException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "DuplicateInvoiceNumber")
                serviceCallSuccess = False

            Catch ex As Exception
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(ex.Message, "DuplicateInvoiceNumber")
                serviceCallSuccess = False
            End Try

            If Not serviceCallSuccess Then
                ' The DuplicateInvoiceNumber service call failed.  End the method and allow the user to retry.
                Cursor.Current = Cursors.Default
                Exit Sub
            End If


            Try
                ' If everything looks good, prepare the order to be closed.
                serviceCallSuccess = True

                If Not Common.UpdateOrderBeforeClose(order, session) Then
                    Cursor.Current = Cursors.Default
                    Exit Sub
                End If

                ' Explicitly handle service faults, timeouts, and connection failures.  If a failure occurs, allow the user to retry
                ' the last action.
            Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                Dim err As New ErrorHandler(serviceFault)
                err.ShowErrorNotification()
                serviceCallSuccess = False

            Catch ex As TimeoutException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "UpdateOrderBeforeClose")
                serviceCallSuccess = False

            Catch ex As CommunicationException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "UpdateOrderBeforeClose")
                serviceCallSuccess = False

            Catch ex As Exception
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(ex.Message, "UpdateOrderBeforeClose")
                serviceCallSuccess = False
            End Try

            If Not serviceCallSuccess Then
                ' The UpdateOrderBeforeClose service call failed.  End the method and allow the user to retry.
                Cursor.Current = Cursors.Default
                Exit Sub
            End If


            Try
                ' Close the order.  Result.Flag represents Suspended status.
                serviceCallSuccess = True
                orderSuspended = False

                Dim result As Result = Common.CloseOrder(order, session)
                If result.Flag Then
                    Messages.OrderSuspended()
                    orderSuspended = True
                End If

                ' Explicitly handle service faults, timeouts, and connection failures.  If a failure occurs, allow the user to retry
                ' the last action.
            Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                Dim err As New ErrorHandler(serviceFault)
                err.ShowErrorNotification()
                serviceCallSuccess = False

            Catch ex As TimeoutException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "CloseOrder")
                serviceCallSuccess = False

            Catch ex As CommunicationException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "CloseOrder")
                serviceCallSuccess = False

            Catch ex As Exception
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(ex.Message, "CloseOrder")
                serviceCallSuccess = False
            End Try

            If Not serviceCallSuccess Then
                ' The CloseOrder call failed.  End the method and allow the user to retry.
                Cursor.Current = Cursors.Default
                Exit Sub
            End If

            If orderSuspended Then
                ' The order did not pass suspension.  Close the form.
                Cursor.Current = Cursors.Default
                Me.DialogResult = Windows.Forms.DialogResult.Yes
                Me.Close()
            Else
                ' The order passed suspension.  Notify the user and close the form.
                Cursor.Current = Cursors.Default
                MsgBox("The order has been approved and closed.", MsgBoxStyle.Information, Me.Text)
                Me.DialogResult = Windows.Forms.DialogResult.Yes
                Me.Close()
            End If


        Else
            ' Re-open order:
            Dim msgResponse As MsgBoxResult = Messages.ReopenOrder()
            If msgResponse = MsgBoxResult.Cancel Then
                Exit Sub
            End If

            Cursor.Current = Cursors.WaitCursor

            Dim resultOpen As Result = session.WebProxyClient.ReOpenOrder(order.OrderHeader_ID)
            If resultOpen.Flag Then
                Messages.ReOpenOrderSuccessful()
                Me.DialogResult = Windows.Forms.DialogResult.No
                Me.Close()
            End If

        End If

    End Sub

    ''' <summary>
    ''' Validates the form fields based on document type chosen.
    ''' </summary>
    ''' <returns>True: If all fields are valid. False: If user needs to provide more information.</returns>
    ''' <remarks>Verify Invoice number and Invoice Total.</remarks>
    Private Function ValidateInvoiceInformation() As Boolean

        If enumDocumentType = DocumentType.Invoice Then

            If TextBoxInvoiceNumber.Text = Nothing Then
                Messages.InvoiceNumberMissing()
                Return False

            ElseIf TextBoxInvoiceTotal.Text = Nothing Then
                Messages.InvoiceTotalMissing()
                Return False

            ElseIf TextBoxInvoiceTotal.Text = 0 Then
                Dim msgResponse As MsgBoxResult = Messages.InvoiceTotalZero()
                If msgResponse = MsgBoxResult.No Then
                    Return False
                End If

            End If

        ElseIf enumDocumentType = DocumentType.Other Then

            If TextBoxInvoiceNumber.Text = Nothing Then
                Messages.DocumentNumberMissing()
                Return False

            End If
        End If

        Return True

    End Function

    ''' <summary>
    ''' Checks if there are any items not received on the order.
    ''' </summary>
    ''' <returns>True: User wants to proceed anyway, or no items like this exist; False: If user does not want to proceed;</returns>
    ''' <remarks></remarks>
    Private Function ItemsNotReceived() As Boolean

        ' Select all OrderItems that have not been received.
        Dim itemsNotRcvd = From oi In order.OrderItems _
                           Where oi.QuantityReceived = Nothing _
                           Select oi

        If itemsNotRcvd.Count > 0 Then

            ' There are items with no received quantity.  Get confirmation from the user to proceed.
            Dim msgResult As MsgBoxResult = Messages.NotAllItemsReceived()

            If msgResult = MsgBoxResult.No Then
                Return False
            End If

        End If

        Return True

    End Function

    ''' <summary>
    ''' Checks the database if the Invoice is a duplicate for the VendorID.
    ''' </summary>
    ''' <returns>True: If there is a duplicate; False: If there is no duplicate;</returns>
    ''' <remarks></remarks>
    Private Function DuplicateInvoiceNumber() As Boolean
        Dim result As Result = session.WebProxyClient.CheckInvoiceNumber(order.Vendor_ID, order.InvoiceNumber, order.OrderHeader_ID)

        If result.Flag Then
            MsgBox("The order cannot be closed.  This invoice number has already been used for this vendor.", MsgBoxStyle.Information, Me.Text)
            Return True
        End If

        Return False
    End Function

    ''' <summary>
    ''' Gets all available currency information from the database.
    ''' </summary>
    ''' <remarks>This method populates the Currency ComboBox.</remarks>
    Private Sub LoadCurrencies()
        currencies = session.WebProxyClient.GetCurrencies()

        For Each currency As Currency In currencies
            ComboBoxCurrency.Items.Add(currency.CurrencyCode)
        Next
    End Sub

    Private Sub ButtonAddCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAddCharge.Click
        Cursor.Current = Cursors.WaitCursor
        Dim addInvoiceCharge As New AddInvoiceCharge(session, order.OrderHeader_ID)
        Cursor.Current = Cursors.Default

        addInvoiceCharge.ShowDialog()

        If addInvoiceCharge.DialogResult = Windows.Forms.DialogResult.OK Then
            LoadChargesTable()
            PopulateInvoiceDataForm()
        End If
    End Sub

    Private Sub ButtonRemoveCharge_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRemoveCharge.Click

        Dim index As Integer
        Dim chargeID As Integer

        ' Make sure there are charges present.  Then determine which charge was selected, and get its chargeID.
        If charges.Rows.Count > 0 Then
            index = DataGridCharges.CurrentRowIndex
            chargeID = invoiceCharges(index).ChargeID
        Else
            MsgBox("There are no charges to remove.", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        ' Confirm the decision and remove the charge.
        If MsgBox("Delete " + invoiceCharges(index).Description.ToString + " " + Format(invoiceCharges(index).ChargeValue, "C") + "?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
            Cursor.Current = Cursors.WaitCursor

            Try
                ' Attempt a service call to remove a charge from an invoice.
                serviceCallSuccess = True

                Dim success As Result = session.WebProxyClient.RemoveInvoiceCharge(chargeID)

                If Not success.Status Then
                    MsgBox("Error removing charge.", MsgBoxStyle.Exclamation, Me.Text)
                Else
                    LoadChargesTable()
                End If


                ' Explicitly handle service faults, timeouts, and connection failures.  If initialization fails, 
                ' return to the Main Menu.
            Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                Dim err As New ErrorHandler(serviceFault)
                err.ShowErrorNotification()
                serviceCallSuccess = False

            Catch ex As TimeoutException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "ButtonRemoveCharge_Click")
                serviceCallSuccess = False

            Catch ex As CommunicationException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "ButtonRemoveCharge_Click")
                serviceCallSuccess = False

            Catch ex As Exception
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(ex.Message, "ButtonRemoveCharge_Click")
                serviceCallSuccess = False

            Finally
                Cursor.Current = Cursors.Default

            End Try

            If Not serviceCallSuccess Then
                ' The call to RemoveInvoiceCharge failed.  End the method and allow the user to retry.
                Exit Sub
            End If

        End If

    End Sub

    Private Sub ButtonReparseEinvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReparseEinvoice.Click
        If MsgBox("Reparse eInvoice?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Me.Text) = MsgBoxResult.Yes Then
            Cursor.Current = Cursors.WaitCursor

            Try
                ' Attempt a service library call to reparse the eInvoice and refresh the order object.
                serviceCallSuccess = True

                ' Reparse the eInvoice, then reload the invoice information.
                Dim result As Result = session.WebProxyClient.ReparseEinvoice(order.EinvoiceID)
                order = session.WebProxyClient.GetOrder(order.OrderHeader_ID)

                ' Explicitly handle service faults, timeouts, and connection failures.  If initialization fails, 
                ' return to the Main Menu.
            Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                Dim err As New ErrorHandler(serviceFault)
                err.ShowErrorNotification()
                serviceCallSuccess = False

            Catch ex As TimeoutException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "ButtonReparseEinvoice_Click")
                serviceCallSuccess = False

            Catch ex As CommunicationException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "ButtonReparseEinvoice_Click")
                serviceCallSuccess = False

            Catch ex As Exception
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(ex.Message, "ButtonReparseEinvoice_Click")
                serviceCallSuccess = False

            Finally
                Cursor.Current = Cursors.Default

            End Try

            If Not serviceCallSuccess Then
                ' The reparse service call failed.  End the method and allow the user to retry.
                Exit Sub
            End If

            ' Re-assign the eInvoiceTotal variable so that we don't lose track of this value.
            eInvoiceInvoiceTotal = order.InvoiceCost

            LoadChargesTable()
            PopulateInvoiceDataForm()

            MsgBox("Reparse successful.", MsgBoxStyle.Information, Me.Text)

        End If
    End Sub

    Private Sub ButtonRefuse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRefuse.Click

        ' Allow the user to choose the reason code from a grid, then call the service library function.
        Dim reasonCodeSelection As New ReasonCodeSelection(refusalReasonCodes)
        reasonCodeSelection.ShowDialog()

        If reasonCodeSelection.DialogResult = Windows.Forms.DialogResult.OK Then
            Dim selectedReasonCode As ReasonCode = refusalReasonCodes(reasonCodeSelection.DataGridReasonCode.CurrentRowIndex)

            If MsgBox("Refuse PO " + order.OrderHeader_ID.ToString() + " with Reason Code " + selectedReasonCode.ReasonCodeAbbreviation + "?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                Cursor.Current = Cursors.WaitCursor
                Try
                    ' Attempt a service call to refuse the order.
                    serviceCallSuccess = True

                    Dim result As Result = session.WebProxyClient.RefuseReceiving(order.OrderHeader_ID, session.UserID, selectedReasonCode.ReasonCodeID)

                    If result.Status = True Then
                        ' Refusal was successful.
                        MsgBox("PO " + order.OrderHeader_ID.ToString() + " has been refused.", MsgBoxStyle.Information, Me.Text)
                        Me.DialogResult = Windows.Forms.DialogResult.Yes
                    End If

                    ' Explicitly handle service faults, timeouts, and connection failures.  If the call fails, allow the user to retry.
                Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                    serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                    Dim err As New ErrorHandler(serviceFault)
                    err.ShowErrorNotification()
                    serviceCallSuccess = False

                Catch ex As TimeoutException
                    Dim err As New ErrorHandler()
                    err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "ButtonRefuse_Click")
                    serviceCallSuccess = False

                Catch ex As CommunicationException
                    Dim err As New ErrorHandler()
                    err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "ButtonRefuse_Click")
                    serviceCallSuccess = False

                Catch ex As Exception
                    Dim err As New ErrorHandler()
                    err.DisplayErrorMessage(ex.Message, "ButtonRefuse_Click")
                    serviceCallSuccess = False

                Finally
                    Cursor.Current = Cursors.Default

                End Try

                If Not serviceCallSuccess Then
                    ' The call to RefuseReceiving failed.  End the method and allow the user to retry.
                    Exit Sub
                End If

            End If
        End If
    End Sub

    Private Sub TextBoxInvoiceTotal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxInvoiceTotal.TextChanged

        ' Allow only numeric characters and positive invoice totals.  
        If TextBoxInvoiceTotal.Text <> Nothing Then
            If Not IsNumeric(TextBoxInvoiceTotal.Text) Then
                MsgBox("Please enter a numeric value for the Invoice Total.", MsgBoxStyle.Information, Me.Text)
                TextBoxInvoiceTotal.Text = String.Empty
                Exit Sub
            End If

            If CDec(TextBoxInvoiceTotal.Text) < 0.0 Then
                Messages.InvoiceTotalNegative()
                TextBoxInvoiceTotal.Text = String.Empty
                Exit Sub
            End If

            order.InvoiceCost = TextBoxInvoiceTotal.Text

        Else
            order.InvoiceCost = 0

        End If

        TextBoxCostDifference.Text = FormatNumber(CalculateCostDifference(), 2).ToString()
        TextBoxSubteam.Text = FormatNumber(CalculateSubteamTotal(), 2).ToString()

    End Sub

    Private Sub RadioButtons_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles RadioButtonInvoice.CheckedChanged, RadioButtonOther.CheckedChanged, RadioButtonNone.CheckedChanged

        ' Setup the form based on document type chosen
        Dim rb As RadioButton = CType(sender, RadioButton)
        If rb.Checked Then

            If rb.Text = "Invoice" Then
                ' Populate fields for an Invoice type
                SetupFormFields(DocumentType.Invoice, False)
                PopulateInvoiceDataForm()

            ElseIf rb.Text = "Other" Then
                ' Populate the fields for an Other type
                SetupFormFields(DocumentType.Other, False)
                PopulateOtherDataForm()

            ElseIf rb.Text = "None" Then
                SetupFormFields(DocumentType.None, False)

            End If
        End If
    End Sub

    Private Sub TextBoxInvoiceNumber_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxInvoiceNumber.TextChanged

        ' Don't allow the field to be cleared if there is an eInvoice.
        If order.EinvoiceID <> Nothing Then
            Exit Sub
        Else
            order.InvoiceNumber = TextBoxInvoiceNumber.Text
        End If

    End Sub

    Private Sub DateTimePickerInvoiceDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePickerInvoiceDate.ValueChanged

        ' Don't allow the field to be cleared if there is an eInvoice.
        If order.EinvoiceID <> Nothing Then
            Exit Sub
        Else
            order.InvoiceDate = DateTimePickerInvoiceDate.Value
        End If

    End Sub

    Private Sub DateTimePickerInvoiceDate_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles DateTimePickerInvoiceDate.Validating

        ' Prevent the user from entering a future invoice date.
        If DateTimePickerInvoiceDate.Value.Date > serverDateTime.Date Then
            MsgBox("The invoice date cannot be a future date.  Please choose another invoice date.", MsgBoxStyle.Exclamation, "Invalid Invoice Date")
            DateTimePickerInvoiceDate.Value = serverDateTime.Date
            DateTimePickerInvoiceDate.Focus()
            Exit Sub
        End If

    End Sub

    Private Sub AlignText()

        ' The designer does not seem to be saving alignment changes properly.  For now, they will be set programmatically.
        LabelInvoiceNumber.TextAlign = ContentAlignment.TopRight
        LabelDate.TextAlign = ContentAlignment.TopRight
        LabelInvoiceTotal.TextAlign = ContentAlignment.TopRight
        LabelChargesTotal.TextAlign = ContentAlignment.TopRight
        LabelSubteam.TextAlign = ContentAlignment.TopRight
        LabelRefusedTotal.TextAlign = ContentAlignment.TopRight
        LabelCostDifference.TextAlign = ContentAlignment.TopRight
        LabelCharges.TextAlign = ContentAlignment.TopCenter

    End Sub

    Private Sub TextBoxInvoiceNumber_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBoxInvoiceNumber.Validating
        ' Only allow alphanumeric characters.  This is done by using a regex expressions which checks if any character
        ' in the string is NOT alphanumeric.

        If AlphanumericValidationFails(TextBoxInvoiceNumber.Text) Then
            MsgBox("The invoice number may not contain the character '|'.  Please enter an alphanumeric value.", MsgBoxStyle.Information, "Invalid Invoice#")
            TextBoxInvoiceNumber.SelectAll()
            e.Cancel = True
        End If
    End Sub

    Private Function AlphanumericValidationFails(ByVal text As String) As Boolean

        ' This is the regex for only allowing alphanumeric characters.  It may come in handy one day.
        'Dim regex As New Regex("[^a-zA-Z0-9]+")

        ' Disallow the pipe character.
        Dim regex As New Regex("[|]+")
        If regex.IsMatch(text) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub TextBoxInvoiceNumber_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBoxInvoiceNumber.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = ASCII_PIPE Then
            e.Handled = True
        End If
    End Sub

End Class