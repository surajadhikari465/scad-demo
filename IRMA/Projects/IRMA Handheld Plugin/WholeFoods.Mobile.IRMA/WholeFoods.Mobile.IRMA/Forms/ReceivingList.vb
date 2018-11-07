Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Reflection
Imports System.ServiceModel

Public Class ReceivingList

    Private mySession As Session
    Private serviceFault As ParsedCFFaultException = Nothing
    Private serviceCallSuccess As Boolean
    Private PONumber As Integer
    Private order As Order
    Private reasonCodes As ReasonCode()
    Private eInvoiceMismatchItems As New List(Of RecListItem)
    Private eInvoiceExceptionItems As New List(Of RecListItem)
    Private eInvoiceExceptionsItemsLoaded As Boolean
    Private notReceivedItems As New List(Of RecListItem)
    Private itemsReceived As Boolean
    Private eInvoiceMismatchItemsDrawn As Boolean
    Private Const warningLimit As Integer = 10

    Private CurrentIndex As Integer = 0
    Private CurrentStartIndex As Integer
    Private CurrentStartIndexTab_0 As Integer

    Private CurrentTabIndex As Integer = 0

    Private TotalItems As Integer = 0
    Private MaxItemsPerPage As Integer = 3

    ' The overall height of a three-row record.
    Private iRecordHeight As Integer = 144

    ' The height of the UPC & Desc header row.
    Private iRow1Height As Integer = 45

    ' The height of the quantity header row.
    Private iRow2Height As Integer = 75

    Private originalReasoncodes(MaxItemsPerPage) As String

    Public Sub New(ByRef session As Session, ByRef order As Order, ByRef reasonCodes As ReasonCode())
        InitializeComponent()

        Me.mySession = session
        Me.PONumber = order.OrderHeader_ID
        Me.order = order
        Me.reasonCodes = reasonCodes

        ' Determine if any items have been received.
        Dim query = From orderItem In order.OrderItems _
                    Where orderItem.QuantityReceived > 0 Or orderItem.Total_Weight > 0 _
                    Select True
        itemsReceived = If(query.Count = 0, False, True)

        LoadOrderDetails()
        FormatHeader()

        ' Begin on the eInvoiceMismatch tab by default.
        tabReceivingList.SelectedIndex = 0

    End Sub

    Private Sub FormatHeader()
        lblPONumberValue.Text = PONumber.ToString()

        lblTotalOrdered.ForeColor = System.Drawing.Color.SeaGreen
        lblTotalReceived.ForeColor = System.Drawing.Color.SeaGreen
        lblTotaleInvoiced.ForeColor = System.Drawing.Color.SeaGreen

    End Sub

    Private Sub LoadOrderDetails()

        Dim iQtyOrdered As Integer = 0
        Dim iQtyReceived As Integer = 0
        Dim iQtyEInvoiced As Integer = 0

        Dim oi As OrderItem
        For i As Integer = 0 To order.OrderItems.Length - 1
            oi = order.OrderItems(i)
            iQtyOrdered = iQtyOrdered + oi.QuantityOrdered
            iQtyReceived = iQtyReceived + oi.QuantityReceived
            iQtyEInvoiced = iQtyEInvoiced + oi.eInvoiceQuantity
        Next

        lblOrdered.Text = iQtyOrdered.ToString
        lblReceived.Text = iQtyReceived.ToString
        lblEinvoiced.Text = iQtyEInvoiced.ToString

        ' If no items have been received, disable Partial Shipment functionality.
        If iQtyReceived = 0 Then
            mnuPartialShipping.Enabled = False
        End If

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

    ''' <summary>
    ''' Populates the Not Received tab of the Receiving List window
    ''' </summary>
    ''' <returns>List of Items that haven't been received</returns>
    ''' <remarks>And item is marked as not received if the quantity received is Null/Nothing</remarks>
    Private Function PopulateNotReceivedList() As List(Of RecListItem)
        Dim items As New List(Of RecListItem)
        Dim oi As OrderItem

        Dim orderItems As OrderItem() = order.OrderItems

        For i As Integer = 0 To orderItems.Length() - 1
            oi = orderItems(i)

            Dim recItem As New RecListItem()

            If oi.ReceivingDiscrepancyReasonCodeID = 0 Then
                recItem.Code = String.Empty
            Else
                Dim query = From code In reasonCodes _
                            Where code.ReasonCodeID = oi.ReceivingDiscrepancyReasonCodeID _
                            Select code.ReasonCodeAbbreviation
                recItem.Code = query.First
            End If

            If oi.QuantityReceived = Nothing Then
                recItem.OrderItemID = oi.OrderItem_ID
                recItem.UPC = oi.Identifier
                recItem.Ordered = oi.QuantityOrdered
                recItem.Received = oi.QuantityReceived
                recItem.eQty = oi.eInvoiceQuantity
                recItem.Description = oi.Item_Description
                recItem.Weight = oi.Total_Weight
                recItem.eWeight = oi.eInvoiceWeight

                items.Add(recItem)
            End If
        Next

        Return items
    End Function

    Private Function PopulateEInvoiceExceptionList() As List(Of RecListItem)
        Dim items As New List(Of RecListItem)
        Dim oi As OrderItem

        Dim orderItems As OrderItem() = Me.mySession.WebProxyClient.GetReceivingListEinvoiceExceptions(PONumber)

        For i As Integer = 0 To orderItems.Length() - 1
            oi = orderItems(i)
            Dim recItem As New RecListItem()

            If oi.ReceivingDiscrepancyReasonCodeID = 0 Then
                recItem.Code = String.Empty
            Else
                Dim query = From code In reasonCodes _
                           Where code.ReasonCodeID = oi.ReceivingDiscrepancyReasonCodeID _
                           Select code.ReasonCodeAbbreviation
                recItem.Code = query.First
            End If

            recItem.OrderItemID = oi.OrderItem_ID
            recItem.UPC = oi.Identifier
            recItem.Ordered = oi.QuantityOrdered
            recItem.Received = oi.QuantityReceived
            recItem.eQty = oi.eInvoiceQuantity
            recItem.Description = oi.Item_Description
            recItem.Weight = oi.Total_Weight
            recItem.eWeight = oi.eInvoiceWeight

            items.Add(recItem)
        Next

        Return items
    End Function

    Private Function PopulateEInvoiceMismatchedList() As List(Of RecListItem)
        Dim items As New List(Of RecListItem)

        Dim oi As OrderItem
        Dim orderItems As OrderItem() = order.OrderItems

        For i As Integer = 0 To orderItems.Length() - 1
            Dim recItem As New RecListItem()

            oi = orderItems(i)

            If oi.ReceivingDiscrepancyReasonCodeID = 0 Then
                recItem.Code = String.Empty
            Else
                Dim query = From code In reasonCodes _
                            Where code.ReasonCodeID = oi.ReceivingDiscrepancyReasonCodeID _
                            Select code.ReasonCodeAbbreviation
                recItem.Code = query.First
            End If

            recItem.UPC = oi.Identifier
            recItem.OrderItemID = oi.OrderItem_ID
            recItem.Ordered = oi.QuantityOrdered
            recItem.Received = oi.QuantityReceived
            recItem.eQty = oi.eInvoiceQuantity
            recItem.Description = oi.Item_Description
            recItem.Weight = oi.Total_Weight
            recItem.eWeight = oi.eInvoiceWeight

            If order.EinvoiceID > 0 Then
                'e-Invoice loaded
                'If oi.CatchweightRequired Then
                '    If oi.Total_Weight <> oi.eInvoiceWeight Then 'And oi.ReceivingDiscrepancyReasonCodeID = 0 Then
                '        'weighted item
                '        items.Add(recItem)
                '    End If
                'Else
                '    If oi.QuantityReceived <> oi.eInvoiceQuantity Then 'And oi.ReceivingDiscrepancyReasonCodeID = 0 Then
                '        'non-weighted item
                '        items.Add(recItem)
                '    End If
                'End If

                '@@ 4.8.5.1 Fix
                If oi.QuantityReceived <> oi.eInvoiceQuantity And (oi.QuantityReceived + oi.eInvoiceQuantity) > 0 Then
                    items.Add(recItem)
                End If

            Else
                'e-invoice not loaded - compare ordered vs. received quantity (incl. not received items with received quantity equals 0 -- fix for bug #9847 )
                If oi.QuantityReceived <> oi.QuantityOrdered Then
                    items.Add(recItem)
                End If
            End If
        Next

        Return items
    End Function
    Public Sub DisplayReceivingList(ByVal OrderItems As List(Of RecListItem), ByVal tabNo As Integer)

        Dim success As Boolean = False
        Dim NoOfItemsDisplayed As Integer = 0

        If tabNo = 0 Then
            CurrentStartIndexTab_0 = CurrentStartIndex
        End If

        btnNext.Enabled = True
        btnPrev.Enabled = True

        TotalItems = OrderItems.Count

        If TotalItems <= MaxItemsPerPage Then
            btnPrev.Enabled = False
            btnNext.Enabled = False
        End If

        If CurrentStartIndex = 0 Then
            btnPrev.Enabled = False
        End If

        If CurrentStartIndex + MaxItemsPerPage < TotalItems Then
            btnNext.Enabled = True
        Else
            btnNext.Enabled = False
        End If

        For i As Integer = 0 To MaxItemsPerPage - 1
            originalReasoncodes(i) = String.Empty
        Next

        If OrderItems.Count > 0 Then

            NoOfItemsDisplayed = MaxItemsPerPage
            If CurrentStartIndex + 3 > TotalItems Then
                NoOfItemsDisplayed = TotalItems Mod MaxItemsPerPage
            End If

            Try
                Me.tabReceivingList.TabPages(tabNo).Controls.Clear()

                For i As Integer = 0 To NoOfItemsDisplayed - 1

                    'Header Labels
                    Dim hdrIdentifier = New Label
                    Dim hdrOrdered = New Label
                    Dim hdrReceived = New Label
                    Dim hdrEQty = New Label
                    Dim hdrDescription = New Label
                    Dim hdrCode = New Label
                    Dim hdrWeight = New Label
                    Dim hdrEWeight = New Label

                    hdrIdentifier.Text = "UPC:"
                    hdrDescription.Text = "Desc:"
                    hdrOrdered.Text = "Ord"
                    hdrReceived.Text = "Recd"
                    hdrEQty.Text = "eQty"
                    If tabNo = 0 Then
                        hdrCode.Text = "Code"
                    Else
                        hdrCode.Text = ""
                    End If
                    hdrWeight.Text = "Wt"
                    hdrEWeight.Text = "eWt"

                    'Data Labels
                    Dim Identifier = New Label
                    Dim Ordered = New Label
                    Dim Received = New Label
                    Dim eQty = New Label
                    Dim Description = New Label
                    Dim Weight = New Label
                    Dim eWeight = New Label

                    Dim Code = New ComboBox
                    For j As Integer = 0 To reasonCodes.Count - 1
                        Code.Items.Add(reasonCodes(j).ReasonCodeAbbreviation)
                    Next

                    'First Row
                    hdrIdentifier.Location = New Point(5, 5 + i * iRecordHeight)
                    Identifier.Location = New Point(5 + 30, 5 + i * iRecordHeight)
                    hdrDescription.Location = New Point(5 + 160, 5 + i * iRecordHeight)
                    Description.Location = New Point(5 + 200, 5 + i * iRecordHeight)

                    hdrIdentifier.Size = New Size(30, 40)
                    Identifier.Size = New Size(130, 40)
                    hdrDescription.Size = New Size(40, 40)
                    Description.Size = New Size(Me.tabReceivingList.Width - 235, 40)

                    hdrIdentifier.BackColor = Color.LightGray
                    hdrIdentifier.ForeColor = Color.Blue
                    Identifier.BackColor = Color.LightGray
                    hdrDescription.BackColor = Color.LightGray
                    hdrDescription.ForeColor = Color.Blue
                    Description.BackColor = Color.LightGray

                    'Second Row
                    hdrOrdered.Location = New Point(5, iRow1Height + i * iRecordHeight)
                    hdrReceived.Location = New Point(5 + 75, iRow1Height + i * iRecordHeight)
                    hdrEQty.Location = New Point(5 + 150, iRow1Height + i * iRecordHeight)
                    hdrWeight.Location = New Point(5 + 225, iRow1Height + i * iRecordHeight)
                    hdrEWeight.Location = New Point(5 + 300, iRow1Height + i * iRecordHeight)
                    hdrCode.Location = New Point(5 + 375, iRow1Height + i * iRecordHeight)

                    hdrOrdered.Size = New Size(75, 25)
                    hdrReceived.Size = New Size(75, 25)
                    hdrEQty.Size = New Size(75, 25)
                    hdrWeight.Size = New Size(75, 25)
                    hdrEWeight.Size = New Size(75, 25)
                    hdrCode.Size = New Size(Me.tabReceivingList.Width - 410, 25)

                    hdrCode.BackColor = Color.LightBlue
                    hdrWeight.BackColor = Color.LightBlue
                    hdrEWeight.BackColor = Color.LightBlue
                    hdrOrdered.BackColor = Color.LightBlue
                    hdrReceived.BackColor = Color.LightBlue
                    hdrEQty.BackColor = Color.LightBlue

                    'Third Row
                    Ordered.Location = New Point(5, iRow2Height + i * iRecordHeight)
                    Received.Location = New Point(5 + 75, iRow2Height + i * iRecordHeight)
                    eQty.Location = New Point(5 + 150, iRow2Height + i * iRecordHeight)
                    Weight.Location = New Point(5 + 225, iRow2Height + i * iRecordHeight)
                    eWeight.Location = New Point(5 + 300, iRow2Height + i * iRecordHeight)
                    Code.Location = New Point(5 + 365, iRow2Height + i * iRecordHeight)

                    Ordered.Size = New Size(75, 25)
                    Received.Size = New Size(75, 25)
                    eQty.Size = New Size(75, 25)
                    Weight.Size = New Size(75, 25)
                    eWeight.Size = New Size(65, 25)

                    Code.Size = New Size(Me.tabReceivingList.Width - 405, 25)

                    'Format
                    hdrIdentifier.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrDescription.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrOrdered.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrReceived.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrEQty.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrCode.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrWeight.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrEWeight.Font = New Font("Tahoma", 5, FontStyle.Bold)

                    Identifier.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    Ordered.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    Received.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    eQty.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    Description.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    Weight.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    eWeight.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    Code.Font = New Font("Tahoma", 7, FontStyle.Regular)

                    'Data Assignments
                    Identifier.Text = OrderItems(i + CurrentStartIndex).UPC
                    Ordered.Text = FormatNumber(OrderItems(i + CurrentStartIndex).Ordered, 0)
                    Received.Text = FormatNumber(OrderItems(i + CurrentStartIndex).Received, 0)
                    eQty.Text = FormatNumber(OrderItems(i + CurrentStartIndex).eQty, 0)
                    Description.Text = OrderItems(i + CurrentStartIndex).Description
                    Weight.Text = FormatNumber(OrderItems(i + CurrentStartIndex).Weight, 2)
                    eWeight.Text = FormatNumber(OrderItems(i + CurrentStartIndex).eWeight, 2)

                    Code.Text = OrderItems(i + CurrentStartIndex).Code

                    'Save the original reason codes before rendering the screen
                    originalReasoncodes(i) = Code.Text

                    If order.EinvoiceID > 0 Then
                        If Code.Text = String.Empty Then
                            Code.BackColor = System.Drawing.Color.Yellow
                        End If
                    End If

                    Code.Tag = CStr(OrderItems(i + CurrentStartIndex).OrderItemID)

                    If tabNo = 0 Then
                        AddHandler Code.SelectedIndexChanged, AddressOf Me.CodeComboBox_SelectedIndexChanged
                    End If

                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrIdentifier)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrOrdered)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrReceived)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrEQty)

                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(Identifier)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(Ordered)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(Received)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(eQty)

                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrDescription)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrCode)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrWeight)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrEWeight)

                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(Description)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(Weight)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(eWeight)

                    If tabNo = 0 Then
                        Me.tabReceivingList.TabPages(tabNo).Controls.Add(Code)
                    End If
                Next

                success = True

            Catch ex As Exception
                MessageBox.Show("Unable to display receiving list info." + ex.Message)
            End Try
        End If
    End Sub


    Public Sub DisplayReceivingList_Old(ByVal items As List(Of RecListItem), ByVal tabNo As Integer)

        Dim success As Boolean = False

        If items.Count > 0 Then

            Try
                For i As Integer = 0 To items.Count - 1

                    'Header Labels
                    Dim hdrIdentifier = New Label
                    Dim hdrOrdered = New Label
                    Dim hdrReceived = New Label
                    Dim hdrEQty = New Label
                    Dim hdrDescription = New Label
                    Dim hdrCode = New Label
                    Dim hdrWeight = New Label
                    Dim hdrEWeight = New Label

                    hdrIdentifier.Text = "UPC:"
                    hdrDescription.Text = "Desc:"
                    hdrOrdered.Text = "Ord"
                    hdrReceived.Text = "Recd"
                    hdrEQty.Text = "eQty"
                    If tabNo = 0 Then
                        hdrCode.Text = "Code"
                    Else
                        hdrCode.Text = ""
                    End If
                    hdrWeight.Text = "Wt"
                    hdrEWeight.Text = "eWt"

                    'Data Labels
                    Dim Identifier = New Label
                    Dim Ordered = New Label
                    Dim Received = New Label
                    Dim eQty = New Label
                    Dim Description = New Label
                    Dim Weight = New Label
                    Dim eWeight = New Label

                    Dim Code = New ComboBox
                    For j As Integer = 0 To reasonCodes.Count - 1
                        Code.Items.Add(reasonCodes(j).ReasonCodeAbbreviation)
                    Next

                    'First Row
                    hdrIdentifier.Location = New Point(5, 5 + i * iRecordHeight)
                    Identifier.Location = New Point(5 + 30, 5 + i * iRecordHeight)
                    hdrDescription.Location = New Point(5 + 160, 5 + i * iRecordHeight)
                    Description.Location = New Point(5 + 200, 5 + i * iRecordHeight)

                    hdrIdentifier.Size = New Size(30, 40)
                    Identifier.Size = New Size(130, 40)
                    hdrDescription.Size = New Size(40, 40)
                    Description.Size = New Size(Me.tabReceivingList.Width - 235, 40)

                    hdrIdentifier.BackColor = Color.LightGray
                    hdrIdentifier.ForeColor = Color.Blue
                    Identifier.BackColor = Color.LightGray
                    hdrDescription.BackColor = Color.LightGray
                    hdrDescription.ForeColor = Color.Blue
                    Description.BackColor = Color.LightGray

                    'Second Row
                    hdrOrdered.Location = New Point(5, iRow1Height + i * iRecordHeight)
                    hdrReceived.Location = New Point(5 + 75, iRow1Height + i * iRecordHeight)
                    hdrEQty.Location = New Point(5 + 150, iRow1Height + i * iRecordHeight)
                    hdrWeight.Location = New Point(5 + 225, iRow1Height + i * iRecordHeight)
                    hdrEWeight.Location = New Point(5 + 300, iRow1Height + i * iRecordHeight)
                    hdrCode.Location = New Point(5 + 375, iRow1Height + i * iRecordHeight)

                    hdrOrdered.Size = New Size(75, 25)
                    hdrReceived.Size = New Size(75, 25)
                    hdrEQty.Size = New Size(75, 25)
                    hdrWeight.Size = New Size(75, 25)
                    hdrEWeight.Size = New Size(75, 25)
                    hdrCode.Size = New Size(Me.tabReceivingList.Width - 410, 25)

                    hdrCode.BackColor = Color.LightBlue
                    hdrWeight.BackColor = Color.LightBlue
                    hdrEWeight.BackColor = Color.LightBlue
                    hdrOrdered.BackColor = Color.LightBlue
                    hdrReceived.BackColor = Color.LightBlue
                    hdrEQty.BackColor = Color.LightBlue

                    'Third Row
                    Ordered.Location = New Point(5, iRow2Height + i * iRecordHeight)
                    Received.Location = New Point(5 + 75, iRow2Height + i * iRecordHeight)
                    eQty.Location = New Point(5 + 150, iRow2Height + i * iRecordHeight)
                    Weight.Location = New Point(5 + 225, iRow2Height + i * iRecordHeight)
                    eWeight.Location = New Point(5 + 300, iRow2Height + i * iRecordHeight)
                    Code.Location = New Point(5 + 365, iRow2Height + i * iRecordHeight)

                    Ordered.Size = New Size(75, 25)
                    Received.Size = New Size(75, 25)
                    eQty.Size = New Size(75, 25)
                    Weight.Size = New Size(75, 25)
                    eWeight.Size = New Size(65, 25)

                    Code.Size = New Size(Me.tabReceivingList.Width - 405, 25)

                    'Format
                    hdrIdentifier.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrDescription.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrOrdered.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrReceived.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrEQty.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrCode.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrWeight.Font = New Font("Tahoma", 5, FontStyle.Bold)
                    hdrEWeight.Font = New Font("Tahoma", 5, FontStyle.Bold)

                    Identifier.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    Ordered.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    Received.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    eQty.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    Description.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    Weight.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    eWeight.Font = New Font("Tahoma", 6, FontStyle.Bold)
                    Code.Font = New Font("Tahoma", 7, FontStyle.Regular)

                    'Data Assignments
                    Identifier.Text = items(i).UPC
                    Ordered.Text = FormatNumber(items(i).Ordered, 0)
                    Received.Text = FormatNumber(items(i).Received, 0)
                    eQty.Text = FormatNumber(items(i).eQty, 0)
                    Description.Text = items(i).Description
                    Weight.Text = FormatNumber(items(i).Weight, 2)
                    eWeight.Text = FormatNumber(items(i).eWeight, 2)

                    Code.Text = items(i).Code

                    If order.EinvoiceID > 0 Then
                        If Code.Text = String.Empty Then
                            Code.BackColor = System.Drawing.Color.Yellow
                        End If
                    End If

                    Code.Tag = CStr(items(i).OrderItemID)

                    If tabNo = 0 Then
                        AddHandler Code.SelectedIndexChanged, AddressOf Me.CodeComboBox_SelectedIndexChanged
                    End If

                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrIdentifier)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrOrdered)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrReceived)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrEQty)

                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(Identifier)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(Ordered)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(Received)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(eQty)

                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrDescription)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrCode)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrWeight)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(hdrEWeight)

                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(Description)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(Weight)
                    Me.tabReceivingList.TabPages(tabNo).Controls.Add(eWeight)

                    If tabNo = 0 Then
                        Me.tabReceivingList.TabPages(tabNo).Controls.Add(Code)
                    End If
                Next

                success = True

            Catch ex As Exception
                MessageBox.Show("Unable to display receiving list info." + ex.Message)
            End Try
        End If
    End Sub

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        Cursor.Current = Cursors.WaitCursor

        Try
            ' Attempt a service library call to update the reason codes.
            serviceCallSuccess = True

            If UpdateInMemoryReasonCodes() = False Then Exit Sub

            If UpdateReasonCodes(True) Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If

            ' Explicitly handle service faults, timeouts, and connection failures.  If initialization fails, 
            ' return to the Main Menu.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            serviceCallSuccess = False

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "UpdateReasonCodes")
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            serviceCallSuccess = False

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "UpdateReasonCodes")
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            serviceCallSuccess = False

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "UpdateReasonCodes")
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            serviceCallSuccess = False

        Finally
            Cursor.Current = Cursors.Default

        End Try

        If Not serviceCallSuccess Then
            ' The call to UpdateReasonCodes failed.  End the method and return to ReceiveOrder.
            Exit Sub
        End If

    End Sub

    Private Sub mnuPartialReceive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPartialShipping.Click

        If MsgBox("Close this order as a Partial Shipment?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Me.Text) = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim saveResult As Boolean
        Dim closeResult As New WholeFoods.Mobile.IRMA.Result

        order.InvoiceNumber = Nothing
        order.InvoiceDate = Nothing
        order.VendorDocID = Nothing
        order.VendorDocDate = Nothing
        order.InvoiceCost = Nothing
        order.PartialShipment = True

        Cursor.Current = Cursors.WaitCursor
        Try
            ' Attempt a service call to close the order as a partial shipment.
            serviceCallSuccess = True

            saveResult = Common.UpdateOrderBeforeClose(order, mySession)
            If (saveResult = False) Then
                MessageBox.Show("Unable to process the save request.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Else
                closeResult = Common.CloseOrder(order, mySession)
                If closeResult.Flag = True Then
                    Dim message As String = "You have marked this PO as a Partial Shipment.  It will remain in suspended status until it is re-opened and received in full."
                    MessageBox.Show(message, "Partial Shipment", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
                Else
                    MessageBox.Show(closeResult.ErrorMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1)
                End If
            End If

            ' Explicitly handle service faults, timeouts, and connection failures.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "mnuPartialReceive_Click")
            serviceCallSuccess = False

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "mnuPartialReceive_Click")
            serviceCallSuccess = False

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "mnuPartialReceive_Click")
            serviceCallSuccess = False

        Finally
            Cursor.Current = Cursors.Default

        End Try

        If Not serviceCallSuccess Then
            ' The call to close the order as a partial shipment failed.  End the method and allow the user to retry.
            Exit Sub
        End If

        Me.DialogResult = Windows.Forms.DialogResult.Yes
        Me.Close()
    End Sub

    Private Function UpdateInMemoryReasonCodes(Optional ByVal performValidation As Boolean = True) As Boolean
        Dim str As String = String.Empty
        Dim OrderItemID As Int64 = 0
        Dim i As Integer = CurrentStartIndexTab_0
        Dim j As Integer = 0

        If CurrentTabIndex <> 0 And performValidation Then
            Return True
            Exit Function
        End If

        For Each Ctrl As Control In Me.tabReceivingList.TabPages(0).Controls
            If Ctrl.GetType() Is GetType(ComboBox) Then
                Dim cbox As ComboBox = CType(Ctrl, ComboBox)

                If cbox.Text <> originalReasoncodes(j) Then
                    ' Only show error message if the order is an eInvoice order and a reason code is missing.
                    If cbox.Text = String.Empty And order.EinvoiceID <> Nothing And performValidation = True Then
                        MessageBox.Show(Messages.QUANTITY_DISCREPANCY, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                        Return False
                        Exit Function
                    End If

                    ' Only populate the string for combo boxes that have text
                    If cbox.Text <> String.Empty Then
                        Dim reasonCode As Integer = Common.GetReasonCodeID(reasonCodes, cbox.SelectedItem)
                        order.OrderItems(i).ReceivingDiscrepancyReasonCodeID = reasonCode
                        eInvoiceMismatchItems(i).Code = cbox.Text
                    End If
                End If

                i = i + 1
                j = j + 1
            End If
        Next

        Return True

    End Function
    ''' <summary>
    ''' Updates all receiving codes to the OrderItem table on the mismatched items tab
    ''' </summary>
    ''' <remarks>Will update both einvoice and non-einvoice orders</remarks>
    Private Function UpdateReasonCodes(Optional ByVal IsMenuOptionBack As Boolean = False) As Boolean
        Dim str As String = String.Empty
        Dim OrderItemID As Int64 = 0
        Dim i As Integer = 0

        If eInvoiceMismatchItemsDrawn Then
            For Each item As RecListItem In eInvoiceMismatchItems

                If IsMenuOptionBack = False Then
                    ' Only show error message if the order is an eInvoice order and a reason code is missing.
                    If order.EinvoiceID <> Nothing And eInvoiceMismatchItems(i).Code = String.Empty Then
                        MessageBox.Show(Messages.QUANTITY_DISCREPANCY, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                        Return False
                        Exit Function
                    End If
                End If

                OrderItemID = eInvoiceMismatchItems(i).OrderItemID

                If eInvoiceMismatchItems(i).Code <> String.Empty Then
                    Dim reasonCode As Integer = Common.GetReasonCodeID(reasonCodes, eInvoiceMismatchItems(i).Code)
                    str = str + CStr(OrderItemID) + "," + CStr(reasonCode) + "|"
                End If

                i = i + 1
            Next

        Else

            If IsMenuOptionBack = False Then

                Dim discrepancyQuery = From item In eInvoiceMismatchItems _
                               Where item.eQty <> item.Received And item.Code = String.Empty _
                               Select True

                If order.EinvoiceID <> Nothing And discrepancyQuery.Count() > 0 Then
                    MessageBox.Show(Messages.QUANTITY_DISCREPANCY, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                    Return False
                    Exit Function
                End If

            End If

        End If

        ' Only update receiving discrepancy codes if they exist.
        ' This accounts for non-einvoice orders where a receiving discrepancy code is not required but is still an option.
        Dim result As WholeFoods.Mobile.IRMA.Result
        If str <> String.Empty Then
            result = Me.mySession.WebProxyClient.UpdateReceivingDiscrepancyCode(str, "|", ",")
        End If

        Return True

    End Function

    Private Sub mnuCloseOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCloseOrder.Click
        Cursor.Current = Cursors.WaitCursor

        Try
            If UpdateInMemoryReasonCodes() = False Then Exit Sub

            ' Update reason codes, but still allow access to Invoice Data if no items have been received.
            If itemsReceived = False OrElse UpdateReasonCodes() Then

                ' Successful - Open InvoiceData form.
                Dim invoiceData As New InvoiceData(mySession, order)
                Cursor.Current = Cursors.Default

                invoiceData.MenuText = "Receiving List"
                invoiceData.ShowDialog()

                If invoiceData.DialogResult = Windows.Forms.DialogResult.Yes Then
                    Me.DialogResult = Windows.Forms.DialogResult.Yes
                    Me.Close()

                ElseIf invoiceData.DialogResult = Windows.Forms.DialogResult.No Then
                    Me.DialogResult = Windows.Forms.DialogResult.No
                    Me.Close()
                End If

            End If

            ' Explicitly handle service faults, timeouts, and connection failures.  If this call to UpdateResonCodes fails, 
            ' end the click event and allow the user to try again.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "mnuCloseOrder_Click")
            serviceCallSuccess = False

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "mnuCloseOrder_Click")
            serviceCallSuccess = False

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "mnuCloseOrder_Click")
            serviceCallSuccess = False

        Finally
            Cursor.Current = Cursors.Default

        End Try

    End Sub

    Private Sub tabReceivingList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabReceivingList.SelectedIndexChanged

        ' This event will fire before the order object has been assigned.  If the order is nothing, skip this event.
        If order Is Nothing Then
            Exit Sub
        End If

        UpdateInMemoryReasonCodes(False)

        CurrentStartIndex = 0

        'Invoice Mismatch tab is selected.  Populate the eInvoiceMismatchItems list.
        If tabReceivingList.SelectedIndex = 0 Then
            CurrentStartIndexTab_0 = 0
            eInvoiceMismatchItems = PopulateEInvoiceMismatchedList()

            Cursor.Current = Cursors.WaitCursor
            CurrentTabIndex = 0
            DisplayReceivingList(eInvoiceMismatchItems, 0)
            eInvoiceMismatchItemsDrawn = True
            Cursor.Current = Cursors.Default
        End If

        'eInvoiceExceptions tab is selected.  Populate the notReceiving list.
        If tabReceivingList.SelectedIndex = 1 Then
            Cursor.Current = Cursors.WaitCursor

            Try
                ' Attempt a service call to populate the eInvoice exceptions list.
                serviceCallSuccess = True

                eInvoiceExceptionItems = PopulateEInvoiceExceptionList()

                ' Explicitly handle service faults, timeouts, and connection failures.  If this call fails, 
                ' return to ReceiveOrder.
            Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                Dim err As New ErrorHandler(serviceFault)
                err.ShowErrorNotification()
                Me.DialogResult = Windows.Forms.DialogResult.Abort
                serviceCallSuccess = False

            Catch ex As TimeoutException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "GetReceivingListEinvoiceExceptions")
                Me.DialogResult = Windows.Forms.DialogResult.Abort
                serviceCallSuccess = False

            Catch ex As CommunicationException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "GetReceivingListEinvoiceExceptions")
                Me.DialogResult = Windows.Forms.DialogResult.Abort
                serviceCallSuccess = False

            Catch ex As Exception
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(ex.Message, "GetReceivingListEinvoiceExceptions")
                Me.DialogResult = Windows.Forms.DialogResult.Abort
                serviceCallSuccess = False

            Finally
                Cursor.Current = Cursors.Default

            End Try

            If Not serviceCallSuccess Then
                ' The call to GetReceivingListEinvoiceExceptions failed.  End the method and fall back to ReceiveOrder.
                Exit Sub
            End If

            CurrentTabIndex = 1
            DisplayReceivingList(eInvoiceExceptionItems, 1)
            eInvoiceExceptionsItemsLoaded = True
        End If

        'NotReceived tab is selected.  Populate the notReceiving list.
        If tabReceivingList.SelectedIndex = 2 Then
            notReceivedItems = PopulateNotReceivedList()

            Cursor.Current = Cursors.WaitCursor
            CurrentTabIndex = 2
            DisplayReceivingList(notReceivedItems, 2)
            Cursor.Current = Cursors.Default
        End If
    End Sub

    Private Sub btnPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrev.Click

        If CurrentStartIndex > 0 Then
            If UpdateInMemoryReasonCodes() = False Then Exit Sub

            CurrentStartIndex = CurrentStartIndex - MaxItemsPerPage

            If CurrentTabIndex = 0 Then
                DisplayReceivingList(eInvoiceMismatchItems, 0)
            ElseIf CurrentTabIndex = 1 Then
                DisplayReceivingList(eInvoiceExceptionItems, 1)
            Else
                DisplayReceivingList(notReceivedItems, 2)
            End If

        End If
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        If CurrentStartIndex <= TotalItems - MaxItemsPerPage Then
            If UpdateInMemoryReasonCodes() = False Then Exit Sub

            CurrentStartIndex = CurrentStartIndex + MaxItemsPerPage

            If CurrentTabIndex = 0 Then
                DisplayReceivingList(eInvoiceMismatchItems, 0)
            ElseIf CurrentTabIndex = 1 Then
                DisplayReceivingList(eInvoiceExceptionItems, 1)
            Else
                DisplayReceivingList(notReceivedItems, 2)
            End If
        End If
    End Sub
End Class

Public Class RecListItem

    Private _UPC As Int64
    Private _Ordered As Decimal
    Private _Received As Decimal
    Private _eQty As Decimal
    Private _Description As String
    Private _Code As String
    Private _Weight As Decimal
    Private _eWeight As Decimal
    Private _orderItemID As Int64

    Public Sub New()

    End Sub
    Public Property OrderItemID() As Int64
        Get
            Return _orderItemID
        End Get
        Set(ByVal value As Int64)
            _orderItemID = value
        End Set
    End Property

    Public Property UPC() As Int64
        Get
            Return _UPC
        End Get
        Set(ByVal value As Int64)
            _UPC = value
        End Set
    End Property


    Public Property Ordered() As Decimal
        Get
            Return _Ordered
        End Get
        Set(ByVal value As Decimal)
            _Ordered = value
        End Set
    End Property

    Public Property Received() As Decimal
        Get
            Return _Received
        End Get
        Set(ByVal value As Decimal)
            _Received = value
        End Set
    End Property

    Public Property eQty() As Decimal
        Get
            Return _eQty
        End Get
        Set(ByVal value As Decimal)
            _eQty = value
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


    Public Property Code() As String
        Get
            Return _Code
        End Get
        Set(ByVal value As String)
            _Code = value
        End Set
    End Property


    Public Property Weight() As Decimal
        Get
            Return _Weight
        End Get
        Set(ByVal value As Decimal)
            _Weight = value
        End Set
    End Property


    Public Property eWeight() As Decimal
        Get
            Return _eWeight
        End Get
        Set(ByVal value As Decimal)
            _eWeight = value
        End Set
    End Property

End Class
