Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Reflection
Imports System.ServiceModel

Public Class OrderItemsRefused

    Private mySession As Session
    Private serviceFault As ParsedCFFaultException = Nothing
    Private serviceCallSuccess As Boolean
    Private PONumber As Integer
    Private _vendorName As String
    Private order As Order
    Private reasonCodes As ReasonCode()
    Private previousTabIndex As Integer = 0
    Private refusedItems As New List(Of RefusedListItem)
    Private refusedItems2 As New List(Of RefusedListItem)
    Private Const warningLimit As Integer = 10

    ' The overall height of a three-row record.
    Private iRecordHeight As Integer = 140

    ' The height of the UPC & VIN header row.
    Private iRow1Height As Integer = 35

    ' The height of the Desc header row.
    Private iRow2Height As Integer = 65

    ' The height of the quantity header row.
    Private iRow3Height As Integer = 90

    Public Sub New(ByRef session As Session, ByRef order As Order, Optional ByVal vendorName As String = "")

        InitializeComponent()

        Me.mySession = session
        Me.order = order
        Me.PONumber = order.OrderHeader_ID
        Me._vendorName = vendorName

        reasonCodes = mySession.WebProxyClient.GetReasonCodesByType(Enums.ReasonCodeType.RI.ToString())

        FormatHeader()

        tabRefusedList.SelectedIndex = 0
        tabRefusedList.TabPages(0).Text = " Vendor: " + _vendorName

    End Sub

    Private Sub FormatHeader()
        lblPONumberValue.Text = PONumber.ToString()
    End Sub

    Private Sub CodeComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If TypeOf sender Is ComboBox Then
            Dim cb As ComboBox = DirectCast(sender, ComboBox)
            If order.EinvoiceID > 0 Then
                If cb.SelectedItem <> String.Empty Then
                    cb.BackColor = System.Drawing.Color.White
                Else
                    cb.BackColor = System.Drawing.Color.Yellow
                End If
            End If
        End If
    End Sub

    Private Function PopulateExcessQuantityList() As List(Of RefusedListItem)
        Dim items As New List(Of RefusedListItem)
        Dim oir As New OrderItemRefused

        Dim refusedItems As OrderItemRefused() = Me.mySession.WebProxyClient.GetOrderItemsRefused(PONumber)

        For i As Integer = 0 To refusedItems.Length() - 1
            oir = refusedItems(i)

            Dim refusedItem As New RefusedListItem()

            If oir.DiscrepancyCodeID = 0 Then
                refusedItem.DiscrepancyCode = String.Empty
            Else
                refusedItem.DiscrepancyCode = Common.GetReasonCodeAbbreviation(reasonCodes, oir.DiscrepancyCodeID)
            End If

            refusedItem.OrderItemRefusedID = oir.OrderItemRefusedID
            refusedItem.Identifier = oir.Identifier
            refusedItem.Description = oir.Description
            refusedItem.VendorItemNumber = oir.VendorItemNumber
            refusedItem.Unit = oir.Unit
            refusedItem.QuantityOrdered = oir.QuantityOrdered
            refusedItem.QuantityReceived = oir.QuantityReceived
            refusedItem.eInvoiceQuantity = oir.eInvoiceQuantity
            refusedItem.eInvoiceCost = oir.eInvoiceCost
            refusedItem.InvoiceCost = oir.InvoiceCost
            refusedItem.InvoiceQuantity = oir.InvoiceQuantity
            refusedItem.RefusedQuantity = oir.RefusedQuantity
            refusedItem.UserAddedEntry = oir.UserAddedEntry

            If oir.eInvoice_ID <= 0 Then
                refusedItem.eInvoiceExceptionEntry = False
            Else
                refusedItem.eInvoiceExceptionEntry = True
            End If

            items.Add(refusedItem)
        Next

        Return items
    End Function

    Public Sub DisplayRefusedItemList(ByVal items As List(Of RefusedListItem), ByVal tabNo As Integer)
        Dim success As Boolean = False

        If items.Count > 0 Then
            Try
                For i As Integer = 0 To items.Count - 1

                    'Header Labels
                    Dim hdrIdentifier = New Label
                    Dim hdrVIN = New Label
                    Dim hdrDescription = New Label

                    Dim hdrOrdered = New Label
                    Dim hdrReceived = New Label
                    Dim hdrInvoiceQuantity = New Label
                    Dim hdrCode = New Label
                    Dim hdrRefusedQuantity = New Label
                    Dim hdrCost = New Label

                    hdrIdentifier.Text = "UPC:"
                    hdrVIN.Text = "VIN:"
                    hdrDescription.Text = "Desc:"

                    hdrOrdered.Text = "Ord"
                    hdrReceived.Text = "Recd"
                    hdrInvoiceQuantity.Text = "Inv Qty"
                    hdrCode.Text = "Code"
                    hdrRefusedQuantity.Text = "Ref. Qty"
                    hdrCost.Text = "Cost"

                    'Data Labels
                    Dim Identifier = New Label
                    Dim VIN = New Label
                    Dim Description = New Label

                    Dim Ordered = New Label
                    Dim Received = New Label
                    Dim invoiceQuantity = New Label

                    Dim refusedQuantity = New TextBox
                    Dim cost = New TextBox

                    If items(i).eInvoiceExceptionEntry = True Then
                        cost.Enabled = False
                    End If

                    Dim Code = New ComboBox
                    For j As Integer = 0 To reasonCodes.Count - 1
                        Code.Items.Add(reasonCodes(j).ReasonCodeAbbreviation)
                    Next

                    'First Row
                    hdrIdentifier.Location = New Point(5, 5 + i * iRecordHeight)
                    Identifier.Location = New Point(5 + 35, 5 + i * iRecordHeight)
                    hdrVIN.Location = New Point(5 + 165, 5 + i * iRecordHeight)
                    VIN.Location = New Point(5 + 200, 5 + i * iRecordHeight)

                    hdrIdentifier.Size = New Size(35, 30)
                    Identifier.Size = New Size(130, 30)
                    hdrVIN.Size = New Size(35, 30)
                    VIN.Size = New Size(Me.tabRefusedList.Width - 235, 30)

                    If items(i).eInvoiceExceptionEntry = True Then
                        hdrIdentifier.BackColor = Color.LightCoral
                        hdrVIN.BackColor = Color.LightCoral
                        VIN.BackColor = Color.LightCoral
                        Identifier.BackColor = Color.LightCoral
                    Else
                        If items(i).UserAddedEntry = True Then
                            hdrIdentifier.BackColor = Color.LightGreen
                            hdrVIN.BackColor = Color.LightGreen
                            VIN.BackColor = Color.LightGreen
                            Identifier.BackColor = Color.LightGreen
                        Else
                            hdrIdentifier.BackColor = Color.LightGray
                            hdrVIN.BackColor = Color.LightGray
                            VIN.BackColor = Color.LightGray
                            Identifier.BackColor = Color.LightGray
                        End If
                    End If
                    hdrIdentifier.ForeColor = Color.Blue
                    hdrVIN.ForeColor = Color.Blue


                    'Second Row
                    hdrDescription.Location = New Point(5, iRow1Height + i * iRecordHeight)
                    Description.Location = New Point(5 + 40, iRow1Height + i * iRecordHeight)

                    hdrDescription.Size = New Size(40, 30)
                    Description.Size = New Size(Me.tabRefusedList.Width - 75, 30)

                    hdrDescription.BackColor = Color.LightGray
                    hdrDescription.ForeColor = Color.Blue

                    Description.BackColor = Color.LightGray

                    'Third Row
                    hdrOrdered.Location = New Point(5, iRow2Height + i * iRecordHeight)
                    hdrReceived.Location = New Point(5 + 55, iRow2Height + i * iRecordHeight)
                    hdrInvoiceQuantity.Location = New Point(5 + 130, iRow2Height + i * iRecordHeight)
                    hdrRefusedQuantity.Location = New Point(5 + 205, iRow2Height + i * iRecordHeight)
                    hdrCost.Location = New Point(5 + 290, iRow2Height + i * iRecordHeight)
                    hdrCode.Location = New Point(5 + 375, iRow2Height + i * iRecordHeight)

                    hdrOrdered.Size = New Size(55, 25)
                    hdrReceived.Size = New Size(75, 25)
                    hdrInvoiceQuantity.Size = New Size(75, 25)
                    hdrRefusedQuantity.Size = New Size(85, 25)
                    hdrCost.Size = New Size(85, 25)
                    hdrCode.Size = New Size(Me.tabRefusedList.Width - 410, 25)

                    hdrCode.BackColor = Color.LightBlue
                    hdrRefusedQuantity.BackColor = Color.LightBlue
                    hdrCost.BackColor = Color.LightBlue
                    hdrOrdered.BackColor = Color.LightBlue
                    hdrReceived.BackColor = Color.LightBlue
                    hdrInvoiceQuantity.BackColor = Color.LightBlue

                    'Fourth Row
                    Ordered.Location = New Point(5, iRow3Height + i * iRecordHeight)
                    Received.Location = New Point(5 + 55, iRow3Height + i * iRecordHeight)
                    invoiceQuantity.Location = New Point(5 + 130, iRow3Height + i * iRecordHeight)
                    refusedQuantity.Location = New Point(5 + 205, iRow3Height + i * iRecordHeight)
                    cost.Location = New Point(5 + 290, iRow3Height + i * iRecordHeight)
                    Code.Location = New Point(5 + 365, iRow3Height + i * iRecordHeight)

                    Ordered.Size = New Size(55, 25)
                    Received.Size = New Size(75, 25)
                    invoiceQuantity.Size = New Size(75, 25)
                    refusedQuantity.Size = New Size(70, 25)
                    cost.Size = New Size(65, 25)

                    Code.Size = New Size(Me.tabRefusedList.Width - 405, 25)

                    'Format
                    hdrIdentifier.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrVIN.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrDescription.Font = New Font("Tahoma", 5, FontStyle.Bold)

                    hdrOrdered.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    hdrReceived.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    hdrInvoiceQuantity.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    hdrCode.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    hdrRefusedQuantity.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    hdrCost.Font = New Font("Tahoma", 6, FontStyle.Bold)

                    Identifier.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    VIN.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    Description.Font = New Font("Tahoma", 6, FontStyle.Bold)

                    Ordered.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    Received.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    invoiceQuantity.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    refusedQuantity.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    cost.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    Code.Font = New Font("Tahoma", 7, FontStyle.Regular)

                    'Data Assignments
                    Identifier.Text = items(i).Identifier
                    VIN.Text = items(i).VendorItemNumber
                    Description.Text = items(i).Description

                    Ordered.Text = FormatNumber(items(i).QuantityOrdered, 0)
                    Received.Text = FormatNumber(items(i).RefusedQuantity, 0)
                    invoiceQuantity.Text = FormatNumber(items(i).InvoiceQuantity, 0)

                    refusedQuantity.Text = FormatNumber(items(i).RefusedQuantity, 0)
                    refusedQuantity.Tag = items(i).UserAddedEntry
                    cost.Text = FormatNumber(items(i).InvoiceCost, 2)

                    Code.Text = items(i).DiscrepancyCode

                    If items(i).eInvoiceExceptionEntry = False Then
                        If CDec(Trim(cost.Text)) <= 0.0 Then
                            cost.BackColor = System.Drawing.Color.Red
                        End If
                    End If

                    If CDec(Trim(refusedQuantity.Text)) <= 0.0 Then
                        refusedQuantity.BackColor = System.Drawing.Color.Red
                    End If

                    Code.Tag = CStr(items(i).OrderItemRefusedID)

                    If tabNo = 0 Then
                        AddHandler Code.SelectedIndexChanged, AddressOf Me.CodeComboBox_SelectedIndexChanged
                    End If

                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(hdrIdentifier)
                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(hdrVIN)
                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(VIN)
                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(hdrDescription)
                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(Description)

                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(hdrOrdered)
                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(hdrReceived)
                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(hdrInvoiceQuantity)

                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(Ordered)
                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(Received)
                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(invoiceQuantity)

                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(hdrCode)
                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(hdrRefusedQuantity)
                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(hdrCost)

                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(Identifier)
                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(refusedQuantity)
                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(cost)
                    Me.tabRefusedList.TabPages(tabNo).Controls.Add(Code)
                Next

                success = True

            Catch ex As Exception
                MsgBox("Unexpected error when attempting to display refused items.", MsgBoxStyle.Critical, "Error")
            End Try
        End If
    End Sub

    Private Function UpdateData() As Boolean
        Dim str As String = String.Empty
        Dim OrderItemRefusedID As Int64 = 0
        Dim i As Integer = 0
        Dim ctrlIndex As Integer = 0

        For Each Ctrl As Control In Me.tabRefusedList.TabPages(0).Controls
            If Ctrl.GetType() Is GetType(ComboBox) Then
                Dim cbox As ComboBox = CType(Ctrl, ComboBox)

                Dim costTextBox As TextBox = CType(Me.tabRefusedList.TabPages(0).Controls(ctrlIndex - 1), TextBox)
                Dim refusedQuantityTextBox As TextBox = CType(Me.tabRefusedList.TabPages(0).Controls(ctrlIndex - 2), TextBox)
                Dim upcLabel As Label = CType(Me.tabRefusedList.TabPages(0).Controls(ctrlIndex - 3), Label)

                Dim upc As String = Trim(upcLabel.Text)

                OrderItemRefusedID = refusedItems(i).OrderItemRefusedID

                If costTextBox.Enabled = True Then
                    If refusedQuantityTextBox.Tag = True Then
                        If CDec(Trim(refusedQuantityTextBox.Text)) <= 0 Then
                            If MsgBox("UPC [" + upc + "] with 0 refused quantity will be deleted. Continue?", MsgBoxStyle.YesNo, "Refused Items") = MsgBoxResult.No Then
                                Return False
                            End If
                        End If
                    Else
                        If CDec(Trim(refusedQuantityTextBox.Text)) <= 0 Then
                            MsgBox("Refused quantity must be greater than 0 for UPC [" + upc + "]", MsgBoxStyle.DefaultButton1, "Refused Items")
                            Return False
                        End If
                    End If
                    If CDec(Trim(costTextBox.Text)) <= 0 Then
                        MsgBox("Inovice Cost must be greater than 0 for UPC [" + upc + "]", MsgBoxStyle.DefaultButton1, "Refused Items")
                        Return False
                    End If
                Else
                    If CDec(Trim(refusedQuantityTextBox.Text)) <= 0 Then
                        MsgBox("Refused quantity for eInvoice exception item with UPC [" + upc + "] must be greater than 0", MsgBoxStyle.DefaultButton1, "Refused Items")
                        Return False
                    End If
                End If

                Dim reasonCode As Integer = Common.GetReasonCodeID(reasonCodes, cbox.SelectedItem)
                str = str + CStr(OrderItemRefusedID) + "," + CStr(reasonCode) + "," + Trim(costTextBox.Text) + "," + Trim(refusedQuantityTextBox.Text) + "|"

                i = i + 1
            End If
            ctrlIndex = ctrlIndex + 1
        Next

        Dim result As WholeFoods.Mobile.IRMA.Result
        If str <> String.Empty Then
            result = Me.mySession.WebProxyClient.UpdateRefusedItemsList(str, "|", ",")

            If result.Status = False Then
                MsgBox("Update failed", MsgBoxStyle.Critical)
                Return False
            End If
        End If

        Return True

    End Function

    Private Sub RefreshScreen()
        If tabRefusedList.SelectedIndex = 0 Then

            Me.tabRefusedList.TabPages(0).Controls.Clear()

            refusedItems.Clear()

            txtTotalRefused.Text = FormatNumber(mySession.WebProxyClient.GetRefusedTotal(PONumber), 2)

            refusedItems = PopulateExcessQuantityList()
 
            Cursor.Current = Cursors.WaitCursor

            DisplayRefusedItemList(refusedItems, 0)

            Cursor.Current = Cursors.Default
        End If
    End Sub

    Private Sub tabRefusedList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabRefusedList.SelectedIndexChanged
        If order Is Nothing Or PONumber = 0 Then
            Exit Sub
        End If

        Try
            RefreshScreen()

            ' Explicitly handle service faults, timeouts, and connection failures.  If this method fails, the user will
            ' be returned to the previous form.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "tabRefusedList_SelectedIndexChanged")
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "tabRefusedList_SelectedIndexChanged")
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "tabRefusedList_SelectedIndexChanged")
            Me.DialogResult = Windows.Forms.DialogResult.Abort
        End Try

    End Sub

    Private Sub mnuUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUpdate.Click
        Try
            If UpdateData() Then
                MsgBox("Update successful.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information)
                RefreshScreen()
            End If

            ' Explicitly handle service faults, timeouts, and connection failures.  If this update fails, the user will
            ' be returned to the previous form.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "mnuUpdate_Click")
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "mnuUpdate_Click")
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "mnuUpdate_Click")
            Me.DialogResult = Windows.Forms.DialogResult.Abort
        End Try

    End Sub

    Private Sub mnuAddRefusedItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddRefusedItem.Click
        Dim frmAdd As AddRefusedItem = New AddRefusedItem(Me.mySession)
        frmAdd.OrderHeaderID = PONumber
        Dim res As DialogResult = frmAdd.ShowDialog()
        frmAdd.Dispose()

        Try
            RefreshScreen()

            ' Explicitly handle service faults, timeouts, and connection failures.  If this method fails, the user will
            ' be returned to the previous form.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "mnuAddRefusedItem_Click")
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "mnuAddRefusedItem_Click")
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "mnuAddRefusedItem_Click")
            Me.DialogResult = Windows.Forms.DialogResult.Abort
        End Try

    End Sub

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Yes
        Me.Close()
    End Sub
End Class

Public Class RefusedListItem

    Private _OrderItemRefusedID As Integer
    Private _Identifier As String
    Private _VendorItemNumber As String
    Private _Description As String
    Private _Unit As String
    Private _QuantityOrdered As Decimal
    Private _QuantityReceived As Decimal
    Private _eInvoiceQuantity As Decimal
    Private _eInvoiceCost As Decimal
    Private _InvoiceQuantity As Decimal
    Private _InvoiceCost As Decimal
    Private _RefusedQuantity As Decimal
    Private _DiscrepancyCode As String
    Private _UserAddedEntry As Boolean
    Private _eInvoiceExceptionEntry As Boolean

    Public Sub New()

    End Sub

    Public Property OrderItemRefusedID() As Integer
        Get
            Return _orderItemRefusedID
        End Get
        Set(ByVal value As Integer)
            _orderItemRefusedID = value
        End Set
    End Property

    Public Property Identifier() As String
        Get
            Return _Identifier
        End Get
        Set(ByVal value As String)
            _Identifier = value
        End Set
    End Property

    Public Property VendorItemNumber() As String
        Get
            Return _VendorItemNumber
        End Get
        Set(ByVal value As String)
            _VendorItemNumber = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Public Property Unit() As String
        Get
            Return _Unit
        End Get
        Set(ByVal value As String)
            _Unit = value
        End Set
    End Property

    Public Property QuantityOrdered() As Decimal
        Get
            Return _QuantityOrdered
        End Get
        Set(ByVal value As Decimal)
            _QuantityOrdered = value
        End Set
    End Property

    Public Property QuantityReceived() As Decimal
        Get
            Return _QuantityReceived
        End Get
        Set(ByVal value As Decimal)
            _QuantityReceived = value
        End Set
    End Property

    Public Property eInvoiceQuantity() As Decimal
        Get
            Return _eInvoiceQuantity
        End Get
        Set(ByVal value As Decimal)
            _eInvoiceQuantity = value
        End Set
    End Property

    Public Property eInvoiceCost() As Decimal
        Get
            Return _eInvoiceCost
        End Get
        Set(ByVal value As Decimal)
            _eInvoiceCost = value
        End Set
    End Property
    
    Public Property InvoiceQuantity() As Decimal
        Get
            Return _InvoiceQuantity
        End Get
        Set(ByVal value As Decimal)
            _InvoiceQuantity = value
        End Set
    End Property

    Public Property InvoiceCost() As Decimal
        Get
            Return _InvoiceCost
        End Get
        Set(ByVal value As Decimal)
            _InvoiceCost = value
        End Set
    End Property

    Public Property RefusedQuantity() As Decimal
        Get
            Return _RefusedQuantity
        End Get
        Set(ByVal value As Decimal)
            _RefusedQuantity = value
        End Set
    End Property

    Public Property DiscrepancyCode() As String
        Get
            Return _DiscrepancyCode
        End Get
        Set(ByVal value As String)
            _DiscrepancyCode = value
        End Set
    End Property

    Public Property UserAddedEntry() As Boolean
        Get
            Return _UserAddedEntry
        End Get
        Set(ByVal value As Boolean)
            _UserAddedEntry = value
        End Set
    End Property

    Public Property eInvoiceExceptionEntry() As Boolean
        Get
            Return _eInvoiceExceptionEntry
        End Get
        Set(ByVal value As Boolean)
            _eInvoiceExceptionEntry = value
        End Set
    End Property
End Class
