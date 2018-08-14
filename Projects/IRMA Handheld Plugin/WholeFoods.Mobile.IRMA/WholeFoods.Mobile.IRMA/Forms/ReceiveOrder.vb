Imports System.Windows.Forms
Imports System
Imports System.Text
Imports System.Linq
Imports System.Data
Imports Microsoft.WindowsCE.Forms
Imports WholeFoods.Mobile.IRMA.Common
Imports System.ServiceModel


Public Class ReceiveOrder
    Inherits HandheldHardware.ScanForm

    Private serviceFault As ParsedCFFaultException = Nothing
    Private mySession As Session
    Private order As WholeFoods.Mobile.IRMA.Order
    Private oi As WholeFoods.Mobile.IRMA.OrderItem
    Private poList() As ListsExternalOrder = Nothing
    Private isNewOrder As Boolean = True
    Private orderLoaded As Boolean
    Private itemLoaded As Boolean
    Private reasonCodes As ReasonCode()
    'Private refusedItemReasonCodes As ReasonCode()
    Private itemsReceived As Boolean
    Private serviceCallSuccess As Boolean
    Private costedByWeight As Boolean
    'Private invalidItemRefused As Boolean = False
    'Private incorrectItemRefused As Boolean = False
    'Private OB_Code As Integer = -1
    'Private II_Code As Integer = -1
    Private incorrectStoreItem As StoreItem = New StoreItem

    Public Property IsCostedByWeightItem() As Boolean
        Get
            Return CostedByWeight
        End Get
        Set(ByVal value As Boolean)
            costedByWeight = value
        End Set
    End Property

#Region " Constructors"

    Public Sub New(ByVal session As Session)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.KeyPreview = True
        Me.mySession = session
        AlignText()

        ' Initialize HH scanner.
        If Not mySession.MyScanner Is Nothing Then
            mySession.MyScanner.restoreScannerSettings()
        End If
        mySession.MyScanner = New HandheldHardware.HandheldScanner(Me)
        If (mySession.MyScanner.HHType = HandheldHardware.Scanner.UNKNOWN) Then
            Messages.ScannerNotAvailable()
        End If

    End Sub

#End Region

#Region " Form Events"

    Private Sub ReceiveOrder_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        InitializeForm()
    End Sub

    Private Sub InitializeForm()
        ClearScreen()

        ' Set labels.
        Me.Text = mySession.ActionType.ToString
        mnuMenu_ExitReceiveOrder.Text = "Exit " & mySession.ActionType.ToString
        Me.lblStoreVal.Text = mySession.StoreName
        Me.lblSubTeamVal.Text = mySession.Subteam

        ' Set visibility.
        Me.frmStatus.Visible = False

        ' Populate reason code list.
        Try
            reasonCodes = mySession.WebProxyClient.GetReasonCodesByType(Enums.ReasonCodeType.RD.ToString())
            'refusedReasonCodes = mySession.WebProxyClient.GetReasonCodesByType(Enums.ReasonCodeType.RI.ToString())

            ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
            'For Each rc In refusedReasonCodes
            '    If rc.ReasonCodeAbbreviation = "OB" Then
            '        OB_Code = rc.ReasonCodeID
            '    End If
            '    If rc.ReasonCodeAbbreviation = "II" Then
            '        II_Code = rc.ReasonCodeID
            '    End If
            '    If OB_Code > -1 And II_Code > -1 Then Exit For
            'Next

            ' Explicitly handle service faults, timeouts, and connection failures.  If initialization fails, 
            ' return to the Main Menu.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "InitializeForm")
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "InitializeForm")
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "InitializeForm")
            Me.DialogResult = Windows.Forms.DialogResult.Abort
        End Try

        If reasonCodes Is Nothing Then
            ' Service call failed.  Return to Main Menu.
            Cursor.Current = Cursors.Default
            Exit Sub
        End If

        ' Leave index 0 blank so the user has a way to clear the selection.
        ComboBoxReasonCode.Items.Add(String.Empty)
        For Each reasonCode As ReasonCode In reasonCodes
            ComboBoxReasonCode.Items.Add(reasonCode.ReasonCodeAbbreviation)
        Next

        ' Set focus on PO# field.
        Me.txtExternalPONumber.Focus()

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ReceiveOrder_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Dim keyPressed As Integer = -1

        Select Case e.KeyCode
            Case Keys.Escape
                order = Nothing
                ClearScreen()
                Me.txtExternalPONumber.Focus()

            Case Keys.Enter Or Keys.Tab
                If Me.txtExternalPONumber.Focused And Not String.IsNullOrEmpty(Me.txtExternalPONumber.Text) Then
                    SearchForExternalPO()
                    If Not orderLoaded Then
                        Me.txtExternalPONumber.Focus()
                    End If
                ElseIf Me.txtUpc.Focused And Not String.IsNullOrEmpty(Me.txtUpc.Text) Then
                    cmdSearchItem_Click(sender, e)
                    If itemLoaded Then
                        Me.txtQtyReceived.Focus()
                    End If
                ElseIf Me.txtQtyReceived.Focused And Not String.IsNullOrEmpty(Me.txtQtyReceived.Text) Then
                    If Me.txtWeightReceived.Enabled Then
                        Me.txtWeightReceived.Focus()
                    Else
                        Me.mnuReceive_Click(sender, e)
                    End If
                ElseIf Me.txtWeightReceived.Focused And Not String.IsNullOrEmpty(Me.txtWeightReceived.Text) Then
                    Me.mnuReceive_Click(sender, e)
                End If

            Case Keys.Up
                If Me.txtQtyReceived.Focused And Not String.IsNullOrEmpty(Me.txtQtyReceived.Text) And IsNumeric(Me.txtQtyReceived.Text) Then
                    Me.txtQtyReceived.Text = CInt(Me.txtQtyReceived.Text) + 1
                    Me.txtQtyReceived.SelectAll()
                End If

            Case Keys.Down
                If Me.txtQtyReceived.Focused And Not String.IsNullOrEmpty(Me.txtQtyReceived.Text) And IsNumeric(Me.txtQtyReceived.Text) Then
                    If CInt(Me.txtQtyReceived.Text) > 0 Then
                        Me.txtQtyReceived.Text = CInt(Me.txtQtyReceived.Text) - 1
                    End If

                    Me.txtQtyReceived.SelectAll()
                End If
        End Select
    End Sub

#End Region

#Region " Control Events"

    Private Sub txtExternalPONumber_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExternalPONumber.GotFocus
        Me.txtExternalPONumber.SelectAll()
    End Sub

    Private Sub txtUpc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.txtUpc.SelectAll()
    End Sub

    Private Sub txtQtyReceived_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQtyReceived.GotFocus
        Me.txtQtyReceived.SelectAll()
    End Sub

    Private Sub txtWeightReceived_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWeightReceived.GotFocus
        Me.txtWeightReceived.SelectAll()
    End Sub

    Private Sub mnuMenu_ClearScreen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMenu_Clear.Click
        ClearScreen()
    End Sub

    Private Sub mnuMenu_ExitReceiveOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMenu_ExitReceiveOrder.Click
        Me.order = Nothing
        ClearScreen()
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub cmdSearchItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearchItem.Click
        'Reset the background color of the Weight textbox
        Me.txtWeightReceived.BackColor = System.Drawing.Color.White

        'incorrectItemRefused = False
        'invalidItemRefused = False

        ' cmdSearchItem logic:
        '   >> If no PO# is entered, scanning a UPC will open the form FindOrderByItem.
        '   >> If FindOrderByItem successfully finds an order, it will populate the PO# and UPC fields here.
        '   >> If PO# and UPC are both populated, the PO# and Item searches will run automatically.



        If orderLoaded Then
            ' An order is loaded.  Search this order for the chosen item.
            SearchItem()
        Else
            ' No order is loaded.  Search for orders which contain the UPC.
            txtExternalPONumber.Text = String.Empty
            FindOrdersByUPC()
        End If

    End Sub

    Private Sub mnuReceive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReceive.Click

        Dim result As Result
        Dim tempQty As Decimal

        ' Verify that a valid item is currently loaded.
        If Not itemLoaded Then 'And Not invalidItemRefused And Not incorrectItemRefused Then
            MsgBox("No item is loaded.  Please search for an item on this PO.", MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
        End If

        Cursor.Current = Cursors.WaitCursor

        ' trap non-numeric qty  :Richard Ainsley, TFS 4515, 1/24/2012
        If Not IsNumeric(txtQtyReceived.Text) Then
            Messages.ReceiveNumericErrorQty()
            Cursor.Current = Cursors.Default
            Exit Sub
        ElseIf txtWeightReceived.Enabled And Not IsNumeric(txtWeightReceived.Text) Then
            Messages.ReceiveNumericErrorWeight()
            Cursor.Current = Cursors.Default
            Me.txtWeightReceived.Focus()
            Exit Sub
        End If

        ' Use the reason code abbreviation to get the reason code ID.
        Dim reasonCode As Integer = Common.GetReasonCodeID(reasonCodes, ComboBoxReasonCode.SelectedItem)

        Try
            ' TFS 8457, 03/27/2013, Faisal Ahmed - Added invalid item to refused item list
            ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
            'If invalidItemRefused = True Then

            '    If cmbUOM.Text = "" Then
            '        MsgBox("Please enter UOM.", MsgBoxStyle.Information, "UOM Error")
            '        ClearScreen(True)
            '        Cursor.Current = Cursors.Default
            '        Exit Sub
            '    End If

            '    Dim result1 As New WholeFoods.Mobile.IRMA.Result
            '    result1 = Me.mySession.WebProxyClient.InsertOrderItemRefused( _
            '            order.OrderHeader_ID, _
            '            0, _
            '            Trim(txtUpc.Text), _
            '            String.Empty, _
            '            String.Empty, _
            '            Trim(cmbUOM.Text), _
            '            CDec(Trim(txtQtyReceived.Text)), _
            '            0, _
            '            II_Code)

            '    ClearScreen(True)
            '    Cursor.Current = Cursors.Default
            '    Exit Sub
            'End If

            ' TFS 8457, 03/27/2013, Faisal Ahmed - Added incorrect item to refused item list
            ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
            'If incorrectItemRefused = True Then
            '    Dim result1 As New WholeFoods.Mobile.IRMA.Result
            '    result1 = Me.mySession.WebProxyClient.InsertOrderItemRefused( _
            '            order.OrderHeader_ID, _
            '            0, _
            '            Trim(txtUpc.Text), _
            '            String.Empty, _
            '            Trim(lblDescriptionVal.Text), _
            '            incorrectStoreItem.PackageUnitAbbr, _
            '            CDec(Trim(txtQtyReceived.Text)), _
            '            0, _
            '            II_Code)

            '    ClearScreen(True)
            '    Cursor.Current = Cursors.Default
            '    Exit Sub
            'End If

            ' trap negative qty  :Richard Ainsley, TFS 4515, 1/24/2012
            If txtWeightReceived.Text <> "" Then
                Dim weight As Double = CDbl(txtWeightReceived.Text)
                If weight < 0.01 Then
                    MsgBox(Messages.POSTIVE_WEIGHT_AMT, MsgBoxStyle.Information, "Positive Weight Error")
                    Cursor.Current = Cursors.Default
                    Exit Sub
                End If

                If weight > 9999.99 Then
                    MsgBox(Messages.EXCESS_WEIGHT_AMT, MsgBoxStyle.Information, "Excess Weight Error")
                    Cursor.Current = Cursors.Default
                    Exit Sub
                End If

                If weight > 999.99 Then
                    Dim mystyle = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question

                    Dim response = MsgBox("Your weight is " + CStr(weight) + ".  Please confirm.", mystyle, "Verify Wieght")
                    If response = MsgBoxResult.No Then
                        Cursor.Current = Cursors.Default
                        Exit Sub
                    End If
                End If
            End If

            If txtQtyReceived.Text <> "" Then
                Dim quantity As Double = CDbl(txtQtyReceived.Text)
                If quantity < 0 Then
                    MsgBox(Messages.NEG_AMT, MsgBoxStyle.Information, "Negative Amount Error")
                    Cursor.Current = Cursors.Default
                    Exit Sub
                End If

                If quantity < 1.0 Then
                    MsgBox(Messages.POSTIVE_QUANTITY_AMT, MsgBoxStyle.Information, "Positive Amount Error")
                    Cursor.Current = Cursors.Default
                    Exit Sub
                End If

                If quantity > 9999 Then
                    MsgBox(Messages.EXCESS_QUANTITY_AMT, MsgBoxStyle.Information, "Excess Quantity Error")
                    Cursor.Current = Cursors.Default
                    Exit Sub
                End If

                If quantity > 999 Then
                    Dim mystyle = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question

                    Dim response = MsgBox("Your quantity is " + CStr(quantity) + ".  Please confirm.", mystyle, "Verify Quantity")
                    If response = MsgBoxResult.No Then
                        Cursor.Current = Cursors.Default
                        Exit Sub
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox("Invalid quantity.  Please try again. " & ex.Message, MsgBoxStyle.Information, "Value Not Numeric")
            Exit Sub
        End Try

        Dim sQtyReceivedSoFar As String = oi.QuantityReceived.ToString("0.####")
        Dim sWeightReceivedSoFar As String = oi.Total_Weight.ToString("0.####")

        Try
            ' This Try/Catch block will encompass any calls made to ReceiveOrderItem.  If there are any failures during this process, the receiver will need
            ' to search for the PO again so that the order object is synced with the database.
            serviceCallSuccess = True

            If oi.QuantityReceived > 0 Then
                Cursor.Current = Cursors.Default
                'Dim refusedQuantity As Decimal
                Dim uMessage As New dlgQuestion
                Dim messageStr As String

                ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
                'refusedQuantity = mySession.WebProxyClient.GetRefusedQuantity(order.OrderHeader_ID, oi.Identifier)
                sQtyReceivedSoFar = (oi.QuantityReceived).ToString("0.####")

                If txtWeightReceived.Enabled = False Then
                    messageStr = sQtyReceivedSoFar + " " + oi.OrderUOMAbbr.ToString
                    uMessage.BodyText = String.Format("This item has been previously scanned with a value of {0}: Tap Add, Overwrite, or Cancel", messageStr)
                Else
                    messageStr = sQtyReceivedSoFar + ", " + sWeightReceivedSoFar + " " + oi.OrderUOMAbbr.ToString
                    uMessage.BodyText = String.Format("This item has been previously scanned with a value of {0}: Tap Add, Overwrite, or Cancel", messageStr)
                End If
                uMessage.ShowDialog()

                Dim dlgResult As String = uMessage.Result
                uMessage = Nothing

                Cursor.Current = Cursors.WaitCursor

                If dlgResult = "A" Then
                    ' Add
                    If txtWeightReceived.Enabled Then
                        oi.Total_Weight += CDbl(txtWeightReceived.Text)
                        tempQty = oi.QuantityReceived
                        oi.QuantityReceived += CInt(txtQtyReceived.Text)

                        'If received qty > ordered qty, pop up a message for receiver to confirm.
                        If oi.QuantityReceived > CInt(lblOrderedVal.Text) Then
                            Dim response = MsgBox("Quantity Received (" + oi.QuantityReceived.ToString("F0") + ") is greater than Quantity Ordered (" + RTrim(lblOrderedVal.Text) + ").  Continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton2, "Verify Quantity")
                            If response = MsgBoxResult.No Then
                                Cursor.Current = Cursors.Default
                                oi.QuantityReceived = tempQty
                                Exit Sub
                            End If
                        End If

                        ' If the quantities now match, clear the reason code.
                        If (oi.QuantityReceived = oi.QuantityOrdered) Or (oi.QuantityReceived = oi.eInvoiceQuantity) Then
                            oi.ReceivingDiscrepancyReasonCodeID = 0
                            reasonCode = 0
                        Else
                            oi.ReceivingDiscrepancyReasonCodeID = GetReasonCodeID(reasonCodes, ComboBoxReasonCode.SelectedItem)
                        End If

                        result = mySession.WebProxyClient.ReceiveOrderItem(oi.QuantityReceived, oi.Total_Weight, DateTime.Now, False, oi.OrderItem_ID, reasonCode, oi.Package_Desc1, mySession.UserID)

                        ' Check to see if ReceiveOrder actually failed because order is already closed.
                        If result.ErrorCode = 1 Then
                            MsgBox(result.ErrorMessage, MsgBoxStyle.OkOnly, "Warning!")

                            ' Empty the form and order object.
                            Me.order = Nothing
                            ClearScreen()
                            Cursor.Current = Cursors.Default
                            Exit Sub
                        End If
                    Else
                        tempQty = oi.QuantityReceived
                        oi.QuantityReceived += CInt(txtQtyReceived.Text)
                        'oi.QuantityReceived += refusedQuantity

                        'If received qty > ordered qty, pop up a message for receiver to confirm.
                        If oi.QuantityReceived > CInt(lblOrderedVal.Text) Then

                            ' TFS 8457, 03/25/2013, Faisal Ahmed - If received quantity > ordered quantity then add execess quantity to refused item list
                            ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
                            Dim response = MsgBox("Quantity Received (" + oi.QuantityReceived.ToString("F0") + ") is greater than Quantity Ordered (" + RTrim(lblOrderedVal.Text) + ").  Continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton2, "Verify Quantity")
                            Cursor.Current = Cursors.Default
                            If response = MsgBoxResult.No Then
                                oi.QuantityReceived = tempQty
                                Exit Sub
                            End If
                        End If

                        ' If the quantities now match, clear the reason code.
                        If (oi.QuantityReceived = oi.QuantityOrdered) Or (oi.QuantityReceived = oi.eInvoiceQuantity) Then

                            oi.ReceivingDiscrepancyReasonCodeID = 0
                            reasonCode = 0

                            ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
                            'If refusedQuantity > 0 Then
                            '    oi.ReceivingDiscrepancyReasonCodeID = OB_Code
                            '    reasonCode = OB_Code
                            'Else
                            '    oi.ReceivingDiscrepancyReasonCodeID = 0
                            '    reasonCode = 0
                            'End If
                        Else
                            oi.ReceivingDiscrepancyReasonCodeID = GetReasonCodeID(reasonCodes, ComboBoxReasonCode.SelectedItem)
                        End If

                        result = mySession.WebProxyClient.ReceiveOrderItem(oi.QuantityReceived, 0, DateTime.Now, False, oi.OrderItem_ID, reasonCode, oi.Package_Desc1, mySession.UserID)

                        ' Check to see if ReceiveOrder actually failed because order is already closed.
                        If result.ErrorCode = 1 Then
                            MsgBox(result.ErrorMessage, MsgBoxStyle.OkOnly, "Warning!")

                            ' Empty the form and order object
                            Me.order = Nothing
                            ClearScreen()
                            Cursor.Current = Cursors.Default
                            Exit Sub
                        End If
                    End If

                ElseIf dlgResult = "O" Then
                    ' Override
                    If txtWeightReceived.Enabled Then
                        oi.Total_Weight = CDbl(txtWeightReceived.Text)

                        ' override total received qty with amount received this time
                        tempQty = oi.QuantityReceived
                        oi.QuantityReceived = CInt(txtQtyReceived.Text)

                        'If received qty > ordered qty, pop up a message for receiver to confirm.
                        If oi.QuantityReceived > CInt(lblOrderedVal.Text) Then

                            Dim response = MsgBox("Quantity Received (" + oi.QuantityReceived.ToString("F0") + ") is greater than Quantity Ordered (" + RTrim(lblOrderedVal.Text) + ").  Continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton2, "Verify Quantity")
                            If response = MsgBoxResult.No Then
                                Cursor.Current = Cursors.Default
                                oi.QuantityReceived = tempQty
                                Exit Sub
                            End If
                        End If

                        ' If the quantities now match, clear the reason code.
                        If (oi.QuantityReceived = oi.QuantityOrdered) Or (oi.QuantityReceived = oi.eInvoiceQuantity) Then
                            oi.ReceivingDiscrepancyReasonCodeID = 0
                            reasonCode = 0
                        Else
                            oi.ReceivingDiscrepancyReasonCodeID = GetReasonCodeID(reasonCodes, ComboBoxReasonCode.SelectedItem)
                        End If

                        result = mySession.WebProxyClient.ReceiveOrderItem(txtQtyReceived.Text, txtWeightReceived.Text, DateTime.Now, False, oi.OrderItem_ID, reasonCode, oi.Package_Desc1, mySession.UserID)

                        ' Check to see if ReceiveOrder actually failed because order is already closed.
                        If result.ErrorCode = 1 Then
                            MsgBox(result.ErrorMessage, MsgBoxStyle.OkOnly, "Warning!")

                            ' Empty the form and order object.
                            Me.order = Nothing
                            ClearScreen()
                            Cursor.Current = Cursors.Default
                            Exit Sub
                        End If

                    Else
                        tempQty = oi.QuantityReceived
                        oi.QuantityReceived = CInt(txtQtyReceived.Text)

                        'If received qty > ordered qty, pop up a message for receiver to confirm.
                        If oi.QuantityReceived > CInt(lblOrderedVal.Text) Then
                            Dim style = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question

                            ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
                            ' TFS 8457, 03/25/2013, Faisal Ahmed - If received quantity > ordered quantity then add execess quantity to refused item list
                            Dim response = MsgBox("Quantity Received (" + oi.QuantityReceived.ToString("F0") + ") is greater than Quantity Ordered (" + RTrim(lblOrderedVal.Text) + ").  Continue?", style, "Verify Quantity")
                            Cursor.Current = Cursors.Default
                            If response = MsgBoxResult.No Then
                                oi.QuantityReceived = tempQty
                                Exit Sub
                            End If
                        End If

                        ' If the quantities now match, clear the reason code.
                        If (oi.QuantityReceived = oi.QuantityOrdered) Or (oi.QuantityReceived = oi.eInvoiceQuantity) Then

                            oi.ReceivingDiscrepancyReasonCodeID = 0
                            reasonCode = 0

                            ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
                            'If refusedQuantity > 0 Then
                            '    oi.ReceivingDiscrepancyReasonCodeID = OB_Code
                            '    reasonCode = OB_Code
                            'Else
                            '    oi.ReceivingDiscrepancyReasonCodeID = 0
                            '    reasonCode = 0
                            'End If
                        Else
                            oi.ReceivingDiscrepancyReasonCodeID = GetReasonCodeID(reasonCodes, ComboBoxReasonCode.SelectedItem)
                        End If

                        result = mySession.WebProxyClient.ReceiveOrderItem(oi.QuantityReceived, 0, DateTime.Now, False, oi.OrderItem_ID, reasonCode, oi.Package_Desc1, mySession.UserID)

                        ' Check to see if ReceiveOrder actually failed because order is already closed.
                        If result.ErrorCode = 1 Then
                            MsgBox(result.ErrorMessage, MsgBoxStyle.OkOnly, "Warning!")

                            ' Empty the form and order object.
                            Me.order = Nothing
                            ClearScreen()
                            Cursor.Current = Cursors.Default
                            Exit Sub
                        End If
                    End If
                End If

            ElseIf oi.QuantityReceived = 0 Then
                ' ie. first time to receive item  :Richard Ainsley, TFS 4515, 1/24/2012
                If txtWeightReceived.Enabled Then
                    oi.Total_Weight += CDbl(txtWeightReceived.Text)
                    oi.QuantityReceived += CInt(txtQtyReceived.Text)

                    'If received qty > ordered qty, pop up a message for receiver to confirm.
                    If oi.QuantityReceived > CInt(lblOrderedVal.Text) Then

                        Dim response = MsgBox("Quantity Received (" + oi.QuantityReceived.ToString("F0") + ") is greater than Quantity Ordered (" + RTrim(lblOrderedVal.Text) + ").  Continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton2, "Verify Quantity")
                        If response = MsgBoxResult.No Then
                            Cursor.Current = Cursors.Default
                            oi.QuantityReceived -= CInt(txtQtyReceived.Text)
                            Exit Sub
                        End If
                    End If

                    ' If the quantities match, clear the reason code.
                    If (oi.QuantityReceived = oi.QuantityOrdered) Or (oi.QuantityReceived = oi.eInvoiceQuantity) Then
                        oi.ReceivingDiscrepancyReasonCodeID = 0
                        reasonCode = 0
                    Else
                        oi.ReceivingDiscrepancyReasonCodeID = GetReasonCodeID(reasonCodes, ComboBoxReasonCode.SelectedItem)
                    End If

                    result = mySession.WebProxyClient.ReceiveOrderItem(txtQtyReceived.Text, txtWeightReceived.Text, DateTime.Now, False, oi.OrderItem_ID, reasonCode, oi.Package_Desc1, mySession.UserID)

                    ' Check to see if ReceiveOrder actually failed because order is already closed.
                    If result.ErrorCode = 1 Then
                        MsgBox(result.ErrorMessage, MsgBoxStyle.OkOnly, "Warning!")

                        ' Empty the form and order object.
                        Me.order = Nothing
                        ClearScreen()
                        Cursor.Current = Cursors.Default
                        Exit Sub
                    End If
                Else
                    oi.QuantityReceived += CInt(txtQtyReceived.Text)

                    'If received qty > ordered qty, pop up a message for receiver to confirm.
                    If oi.QuantityReceived > CInt(lblOrderedVal.Text) Then

                        Dim response = MsgBox("Quantity Received (" + oi.QuantityReceived.ToString("F0") + ") is greater than Quantity Ordered (" + RTrim(lblOrderedVal.Text) + ").  Continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton2, "Verify Quantity")
                        If response = MsgBoxResult.No Then
                            Cursor.Current = Cursors.Default
                            oi.QuantityReceived -= CInt(txtQtyReceived.Text)
                            Exit Sub
                            'Else
                            '    ComboBoxReasonCode.SelectedItem = "OB"
                        End If
                    End If

                    ' If the quantities now match, clear the reason code.
                    If (oi.QuantityReceived = oi.QuantityOrdered) Or (oi.QuantityReceived = oi.eInvoiceQuantity) Then
                        oi.ReceivingDiscrepancyReasonCodeID = 0
                        reasonCode = 0
                    Else
                        oi.ReceivingDiscrepancyReasonCodeID = GetReasonCodeID(reasonCodes, ComboBoxReasonCode.SelectedItem)
                    End If

                    result = mySession.WebProxyClient.ReceiveOrderItem(oi.QuantityReceived, 0, DateTime.Now, False, oi.OrderItem_ID, reasonCode, oi.Package_Desc1, mySession.UserID)

                    ' Check to see if ReceiveOrder actually failed because order is already closed.
                    If result.ErrorCode = 1 Then
                        Dim response = MsgBox(result.ErrorMessage, MsgBoxStyle.OkOnly, "Warning!")

                        ' Empty the form and order object.
                        Me.order = Nothing
                        ClearScreen()
                        Cursor.Current = Cursors.Default
                        Exit Sub
                    End If
                End If
            End If

            ' Explicitly handle service faults, timeouts, and connection failures.  If a failure occurs, the order object will not be synced with the database.
            ' The receiver will need to search for the PO again to re-sync the order object and attempt the scan again.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "mnuReceive_Click")
            serviceCallSuccess = False

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "mnuReceive_Click")
            serviceCallSuccess = False

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "mnuReceive_Click")
            serviceCallSuccess = False
        End Try

        If serviceCallSuccess Then
            ' There were no service failures during ReceiveOrderItem.  Prepare the form for the next item.
            ClearScreen(True)
            Cursor.Current = Cursors.Default
        Else
            ' There was some kind of failure during the service call.  Clear the screen.
            ClearScreen()
            Cursor.Current = Cursors.Default
        End If

    End Sub

#End Region

#Region " Public Events"

    Public Overrides Sub UpdateControlsOnScanCompleteEvent()
        Try
            frmStatus.Text = "Scan complete!"

            For Each c As Control In Me.Controls
                c.Refresh()
            Next

            frmStatus.Visible = False

            If Not order Is Nothing And txtExternalPONumber.Focused Then
                SearchForExternalPO()
            ElseIf Me.txtUpc.TextLength > 0 Then
                If orderLoaded AndAlso Not itemLoaded Then
                    'If orderLoaded Then
                    ' An order is loaded, but no item is currently loaded.  Search this order for the chosen item.
                    SearchItem()
                ElseIf Not orderLoaded Then
                    ' No order is loaded.  Search for orders which contain the UPC.
                    txtExternalPONumber.Text = String.Empty
                    FindOrdersByUPC()
                End If
            End If

            Me.txtQtyReceived.Focus()
        Catch ex As Exception
            Messages.ShowException(ex)
        End Try
    End Sub

    Public Overrides Sub UpdateControlsScanFailedEvent()
        Try
            frmStatus.Visible = True
            frmStatus.Text = "*** Scan failed ***"

            For Each c As Control In Me.Controls
                c.Refresh()
            Next
        Catch ex As ObjectDisposedException
        End Try
    End Sub

    Public Overrides Sub UpdateControlsOnScanTriggerEvent()
        Try
            For Each c As Control In Me.Controls
                c.Refresh()
            Next
        Catch ex As ObjectDisposedException
        End Try
    End Sub

    Public Overrides Sub UpdateControlsOnScanFailedEvent()
        Try
            frmStatus.Visible = True
            frmStatus.Text = "*** Scan failed ***"

            For Each c As Control In Me.Controls
                c.Refresh()
            Next
        Catch ex As ObjectDisposedException
        End Try
    End Sub

    Public Overrides Sub IsTriggerDown(ByVal isDown As Boolean)
        Try
            If (isDown) Then
                frmStatus.Visible = True
                frmStatus.Text = "Scan now..."
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Overrides Sub UpdateUPCText(ByVal value As String)
        Try
            Me.txtUpc.Text = value.TrimStart("0")
            If Not order Is Nothing And txtExternalPONumber.Focused Then
                Me.txtExternalPONumber.Text = value
            ElseIf Not itemLoaded Then
                ' Some scan guns add leading zeroes to the UPC.  Remove them before assigning the value.
                Me.txtUpc.Text = value.TrimStart("0")
            End If

        Catch ex As Exception
            Messages.ShowException(ex)
        End Try
    End Sub

#End Region

#Region " Private Methods"

    Private Sub ClearScreen(Optional ByVal saveOrderInfo As Boolean = False)
        Cursor.Current = Cursors.WaitCursor

        lblQtyReceived.Text = "Quantity:"
        lblPkgVal.Text = String.Empty
        lblPkgVal.ForeColor = Color.Black
        lblUOM.Visible = False
        cmbUOM.Visible = False

        If Not saveOrderInfo Then
            Me.lblItemSubTeam.Enabled = False
            Me.lblItemSubTeamVal.Text = String.Empty
            Me.lblItemSubTeamVal.Enabled = False
            Me.lblPrimaryVendorVal.Text = String.Empty
            Me.lblPrimaryVendorVal.Enabled = False
            Me.lblPrimaryVendor.Enabled = False
            Me.txtExternalPONumber.Text = String.Empty
            orderLoaded = False
            itemsReceived = False
        End If

        Me.cmdSearchItem.Enabled = False
        Me.txtUpc.Text = String.Empty
        Me.txtQtyReceived.Text = String.Empty
        Me.txtWeightReceived.Text = String.Empty
        Me.txtUpc.Text = String.Empty
        Me.lblDescriptionVal.Text = String.Empty
        Me.lblPkgVal.Text = String.Empty
        Me.lblOrderedVal.Text = String.Empty
        Me.lblReceivedVal.Text = String.Empty
        Me.lblReceived.ForeColor = Color.Black
        Me.lblDescriptionVal.Enabled = False
        Me.lblPkg.Enabled = False
        Me.lblPkgVal.Enabled = False
        Me.lblOrdered.Enabled = False
        Me.lblOrderedVal.Enabled = False
        Me.lblQtyReceived.Enabled = False
        Me.mnuReceive.Enabled = False
        Me.txtQtyReceived.Enabled = False
        Me.txtQtyReceived.BackColor = Color.LightGray
        Me.lblWeightReceived.Enabled = False
        Me.txtWeightReceived.Enabled = False
        Me.txtWeightReceived.BackColor = Color.LightGray
        Me.txtExternalPONumber.Visible = True
        Me.txtExternalPONumber.Enabled = True
        Me.ComboBoxReasonCode.Enabled = False
        Me.LinkLabelReasonCode.Enabled = False
        Me.lblEinvoiceQty.Enabled = False
        Me.lblEinvoiceQtyVal.Text = String.Empty
        Me.LabelEinvoiceUOM.Text = String.Empty
        Me.lblReceived.Enabled = False

        If txtExternalPONumber.Text.Length > 0 Then
            cmdSearchForExtPO.Enabled = False
            txtUpc.Focus()
        Else
            txtExternalPONumber.Focus()
        End If

        ComboBoxReasonCode.SelectedIndex = -1

        itemLoaded = False

        If Not orderLoaded Then
            MenuItemInvoiceData.Enabled = False
            MenuItemReceivingList.Enabled = False
            MenuItemOrderInfo.Enabled = False
            'MenuItemRefusedList.Enabled = False
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub SearchPO(ByVal IrmaPO As String, ByVal ExtPO As String)
        Cursor.Current = Cursors.WaitCursor

        Try
            ' Attempt to call the service library to update the order object.  If this fails, the user will need to attempt the search again.
            serviceCallSuccess = True

            ' Force reload of oi variable.
            isNewOrder = True

            ' Validate PO is present.
            If Len(IrmaPO) > 0 Then

                order = mySession.WebProxyClient.GetOrder(IrmaPO)

                ' Check that the PO# is valid.
                If order.Store_No = 0 Then
                    Messages.OrderNotExist(IrmaPO)
                    Cursor.Current = Cursors.Default
                    Exit Sub
                Else
                    ' Check if the order is for the current user's store.
                    If order.ShipToStoreNo <> mySession.StoreNo Then
                        Messages.WrongStoreOrderError(order.OrderHeader_ID.ToString, order.StoreCompanyName)
                        Cursor.Current = Cursors.Default
                        txtExternalPONumber.Text = String.Empty
                        Exit Sub

                        ' Check if the order is already closed.
                    ElseIf order.CloseDate > Date.MinValue Then

                        ' If it's closed, but is a partial shipment, then the order can be re-opened.
                        If order.PartialShipment Then
                            If MsgBox("This order was closed as a partial shipment.  Re-open the order now to scan more items?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                                Try
                                    Dim success As New Result
                                    Cursor.Current = Cursors.WaitCursor
                                    success = mySession.WebProxyClient.ReOpenOrder(order.OrderHeader_ID)
                                    Cursor.Current = Cursors.Default

                                    If success.Status = True Then
                                        MsgBox("The order has been re-opened.", MsgBoxStyle.Information, Me.Text)
                                        Cursor.Current = Cursors.WaitCursor
                                        SearchPO(order.OrderHeader_ID, Nothing)
                                        Cursor.Current = Cursors.Default
                                    End If
                                    Exit Sub
                                Catch ex As Exception
                                    Messages.ShowException(ex)
                                End Try
                            Else
                                Cursor.Current = Cursors.Default
                                txtExternalPONumber.Text = String.Empty
                                Exit Sub
                            End If
                        End If

                        ' Otherwise it can't be re-opened, and the user must search for another PO.
                        Messages.OrderAlreadyClosedError(order.OrderHeader_ID.ToString)
                        Cursor.Current = Cursors.Default
                        txtExternalPONumber.Text = String.Empty
                        Exit Sub

                        ' Make sure the order hasn't been deleted.
                    ElseIf order.DeletedOrder Then
                        Messages.DeletedOrderError(order.OrderHeader_ID)
                        Cursor.Current = Cursors.Default
                        txtExternalPONumber.Text = String.Empty
                        Exit Sub

                        ' Make sure the order has been sent.
                    ElseIf order.SentDate = Date.MinValue Then
                        Messages.OrderNotSentError(order.OrderHeader_ID)
                        Cursor.Current = Cursors.Default
                        txtExternalPONumber.Text = String.Empty
                        Exit Sub

                    End If
                End If

                Me.lblUPC.Enabled = True
                Me.txtUpc.Enabled = True
                txtUpc.BackColor = Color.White

                lblPrimaryVendor.Enabled = True
                lblPrimaryVendorVal.Enabled = True
                lblPrimaryVendorVal.Text = order.CompanyName

                lblItemSubTeam.Enabled = True
                lblItemSubTeamVal.Enabled = True
                lblItemSubTeamVal.Text = order.Transfer_To_SubTeamName

                If order.Return_Order Then
                    Me.lblItemSubTeamVal.Text = "(C) " & Me.lblItemSubTeamVal.Text
                Else
                    Me.lblItemSubTeamVal.Text = "(P) " & Me.lblItemSubTeamVal.Text
                End If

                orderLoaded = True
                MenuItemInvoiceData.Enabled = True
                MenuItemReceivingList.Enabled = True
                MenuItemOrderInfo.Enabled = True

                ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
                'Dim isRefusalAllowed As Boolean = mySession.WebProxyClient.IsRefusalAllowed(order.OrderHeader_ID)
                'MenuItemRefusedList.Enabled = isRefusalAllowed
            Else
                MessageBox.Show(Messages.NULL_PO, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                txtExternalPONumber.Focus()
            End If

            ' Explicitly handle service faults, timeouts, and connection failures.  If this search fails, the user will be allowed to retry the search again.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "SearchPO")
            serviceCallSuccess = False

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "SearchPO")
            serviceCallSuccess = False

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "SearchPO")
            serviceCallSuccess = False
        End Try


        If serviceCallSuccess Then
            ' The GetOrder call completed successfully.  Prepare the user to enter a UPC.
            Me.cmdSearchItem.Enabled = False
            txtUpc.Focus()
            Cursor.Current = Cursors.Default
        Else
            ' The GetOrder call failed.  Leave cmdSearchForExtPO enabled so that the user can easily retry the search.
            cmdSearchForExtPO.Enabled = True
            Cursor.Current = Cursors.Default
        End If

    End Sub

    Private Sub SearchForExternalPO()
        Cursor.Current = Cursors.WaitCursor

        cmdSearchForExtPO.Enabled = False
        cmdSearchItem.Enabled = False

        ' Show the disabled button as disabled.
        Application.DoEvents()

        ' Verify that the PO# value is an integer.
        Try
            Integer.Parse(txtExternalPONumber.Text)
        Catch ex As FormatException
            MessageBox.Show("Please enter a numeric value for the PO# with no special characters.", "Invalid PO#", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Cursor.Current = Cursors.Default
            txtExternalPONumber.Text = String.Empty
            txtExternalPONumber.Focus()
            Exit Sub
        Catch ex As OverflowException
            MessageBox.Show("The PO# value is too large.  Please enter a smaller value.", "Invalid PO#", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Cursor.Current = Cursors.Default
            txtExternalPONumber.Text = String.Empty
            txtExternalPONumber.Focus()
            Exit Sub
        End Try

        Try
            ' Attempt to call the service library for any external orders that may match the PO#.  If the call fails, allow the user to retry the search.
            serviceCallSuccess = True

            ' Validate PO is present.
            If Len(Me.txtExternalPONumber.Text) > 0 Then

                Dim ExtID As Integer = CInt(Me.txtExternalPONumber.Text)
                Dim Store_No As Integer = mySession.StoreNo
                Dim SelectedIrmaPO As Integer = 0
                poList = Nothing

                poList = mySession.WebProxyClient.GetExternalOrders(ExtID, Store_No)

                If poList.Length = 0 Then
                    order = mySession.WebProxyClient.GetOrder(ExtID)
                    If order.Store_No = 0 Then
                        Messages.OrderNotExist(txtExternalPONumber.Text)
                        txtExternalPONumber.Text = String.Empty
                    Else
                        If order.ShipToStoreNo <> mySession.StoreNo Then
                            Messages.WrongStoreOrderError(order.OrderHeader_ID.ToString, order.StoreCompanyName)
                            txtExternalPONumber.Text = String.Empty
                        ElseIf order.CloseDate > Date.MinValue And Not order.PartialShipment Then
                            Messages.OrderAlreadyClosedError(order.OrderHeader_ID.ToString)
                            txtExternalPONumber.Text = String.Empty
                        ElseIf order.DeletedOrder Then
                            Messages.DeletedOrderError(order.OrderHeader_ID)
                            txtExternalPONumber.Text = String.Empty
                        ElseIf order.SentDate = Date.MinValue Then
                            Messages.OrderNotSentError(order.OrderHeader_ID)
                            txtExternalPONumber.Text = String.Empty
                        End If
                    End If
                    Cursor.Current = Cursors.Default
                    Exit Sub

                ElseIf poList.Length >= 1 Then
                    If poList.Length > 1 Then
                        Dim multiplePOSelector As New dlgMultiplePOSelector(poList)
                        Cursor.Current = Cursors.Default

                        If multiplePOSelector.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                            cmdSearchForExtPO.Enabled = True
                            txtExternalPONumber.Text = String.Empty
                            txtExternalPONumber.Focus()
                            Exit Sub
                        Else
                            Dim orderIndex As Integer = multiplePOSelector.DataGridPOSelector.CurrentRowIndex
                            Cursor.Current = Cursors.WaitCursor

                            Me.lblExternalID.Text = poList(orderIndex).Source
                            SelectedIrmaPO = poList(orderIndex).OrderHeader_ID

                            SearchPO(SelectedIrmaPO, ExtID)
                            Me.lblExternalID.Visible = True
                            Exit Sub
                        End If

                    Else
                        Me.lblExternalID.Text = "PO#"
                        SelectedIrmaPO = poList(0).OrderHeader_ID

                        If txtExternalPONumber.Text <> Nothing Then
                            SearchPO(SelectedIrmaPO, ExtID)
                        End If
                        Me.lblExternalID.Visible = True
                        Exit Sub
                    End If

                    Me.lblExternalID.Visible = True
                End If

                txtUpc.BackColor = Color.White
            Else
                MessageBox.Show(Messages.NULL_PO, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                txtExternalPONumber.Focus()
            End If

            ' Explicitly handle service faults, timeouts, and connection failures.  Allow the user to retry the search if the service call fails.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "SearchForExternalPO")
            serviceCallSuccess = False

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "SearchForExternalPO")
            serviceCallSuccess = False

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "SearchForExternalPO")
            serviceCallSuccess = False
        End Try

        If serviceCallSuccess Then
            ' The call to GetExternalOrders() succeeded.  
            Me.cmdSearchItem.Enabled = False
            Me.cmdSearchForExtPO.Enabled = True
            Cursor.Current = Cursors.Default
        Else
            ' There was a failure during the service call.  Allow the user to retry the search.
            cmdSearchForExtPO.Enabled = True
            txtExternalPONumber.Focus()
            Cursor.Current = Cursors.Default
        End If

    End Sub

    Private Sub SearchItem()
        Cursor.Current = Cursors.WaitCursor
        cmdSearchItem.Enabled = False

        ' Force the cmdSearchItem.Enabled = False as seen by the user.  The button will activate again if user keys in text.
        Application.DoEvents()

        ' Check for blank search field.
        If Me.txtUpc.Text = String.Empty Then
            MessageBox.Show(Messages.NULL_UPC, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Cursor.Current = Cursors.Default
            Exit Sub
        End If

        Dim currentScannedUpc = GetUpc(Me.txtUpc.Text)
        ' Verify that the UPC value is an integer (needs to be 64-bit to avoid overflow).
        Try
            Int64.Parse(currentScannedUpc)
        Catch ex As FormatException
            MessageBox.Show("Please enter a numeric value for the UPC with no special characters.", "Invalid UPC", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Cursor.Current = Cursors.Default
            txtUpc.Text = String.Empty
            txtUpc.Focus()
            Exit Sub
        Catch ex As OverflowException
            MessageBox.Show("The UPC value is too large.  Please enter a smaller value.", "Invalid UPC", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Cursor.Current = Cursors.Default
            txtUpc.Text = String.Empty
            txtUpc.Focus()
            Exit Sub
        End Try

        ' Check for scale item.
        Me.txtUpc.Text = ScaleItemCheck(currentScannedUpc)

        Dim storeItem As StoreItem = Nothing
        Try
            ' Attempt to call the service library for item information.
            serviceCallSuccess = True

            Me.IsCostedByWeightItem = False
            storeItem = Me.mySession.WebProxyClient.GetStoreItem(mySession.StoreNo, mySession.SubteamKey, mySession.UserID, Nothing, Me.txtUpc.Text)
            Me.IsCostedByWeightItem = storeItem.CostedByWeight

            ' Explicitly handle service faults, timeouts, and connection failures.  Allow the user to retry the search if the service call fails.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "SearchItem")
            serviceCallSuccess = False

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "SearchItem")
            serviceCallSuccess = False

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "SearchItem")
            serviceCallSuccess = False
        End Try

        If Not serviceCallSuccess Then
            ' The service call failed.  End the method and allow the user to retry the search.
            cmdSearchItem.Enabled = True
            txtUpc.Focus()
            Cursor.Current = Cursors.Default
            Exit Sub
        End If

        If Not Exceptions.ValidIdentifier(storeItem) Then

            ' TFS 8457, 03/27/2013, Faisal Ahmed - Add the invalid item to refused item list.
            ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
            'Dim style = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question
            'Dim response = MsgBox("Item was not found.  This item will be added to the refused items page.  Proceed?", style, "Invalid Item")
            'If response = MsgBoxResult.Yes Then
            '    lblQtyReceived.Text = "Ref. Qty:"
            '    Me.lblQtyReceived.Enabled = True
            '    Me.mnuReceive.Enabled = True
            '    Me.txtQtyReceived.Enabled = True
            '    Me.txtQtyReceived.BackColor = Color.White
            '    invalidItemRefused = True
            '    ComboBoxReasonCode.SelectedIndex = ComboBoxReasonCode.Items.IndexOf("II")
            '    ComboBoxReasonCode.Enabled = False

            '    lblUOM.Visible = True
            '    cmbUOM.Visible = True

            '    Cursor.Current = Cursors.Default
            '    Exit Sub
            'End If

            Messages.ItemNotFound()
            txtUpc.Focus()
            txtUpc.SelectAll()
        Else
            If oi Is Nothing Then
                isNewOrder = False
                oi = GetOrderItem(Me.txtUpc.Text, storeItem)
            ElseIf isNewOrder Then
                isNewOrder = False
                oi = GetOrderItem(Me.txtUpc.Text, storeItem)
            ElseIf oi.Identifier <> Me.txtUpc.Text Then
                oi = GetOrderItem(Me.txtUpc.Text, storeItem)
            End If

            If Not oi Is Nothing Then
                PopulateItemInformation()

                Me.lblDescriptionVal.Enabled = True
                Me.lblPkg.Enabled = True
                Me.lblPkgVal.Enabled = True
                Me.lblItemSubTeam.Enabled = True
                Me.lblItemSubTeamVal.Enabled = True
                Me.lblPrimaryVendor.Enabled = True
                Me.lblPrimaryVendorVal.Enabled = True
                Me.lblOrdered.Enabled = True
                Me.lblOrderedVal.Enabled = True
                Me.lblQtyReceived.Enabled = True
                Me.mnuReceive.Enabled = True
                Me.txtQtyReceived.Enabled = True
                Me.txtQtyReceived.BackColor = Color.White

                Me.ComboBoxReasonCode.Enabled = True
                Me.LinkLabelReasonCode.Enabled = True

                If oi.IsReceivedWeightRequired Then
                    Me.lblWeightReceived.Enabled = True
                    Me.txtWeightReceived.Enabled = True
                    Me.txtWeightReceived.BackColor = Color.White
                Else
                    Me.lblWeightReceived.Enabled = False
                    Me.txtWeightReceived.Enabled = False
                    Me.txtWeightReceived.BackColor = Color.LightGray
                End If

                ' Adjust eInvoice qty control if there is a value
                If oi.eInvoiceQuantity > 0 Then
                    Me.lblEinvoiceQty.Enabled = True
                    Me.lblEinvoiceQtyVal.Enabled = True
                    Me.lblEinvoiceQty.BackColor = Color.PaleGreen
                    Me.lblEinvoiceQtyVal.BackColor = Color.PaleGreen
                    Me.lblEinvoiceQtyUOM.Text = oi.eInvoiceQuantityUnit
                End If

                If oi.ReceivingDiscrepancyReasonCodeID = 0 Then
                    Me.ComboBoxReasonCode.SelectedIndex = -1
                Else
                    Dim query = From code In reasonCodes _
                                Where code.ReasonCodeID = oi.ReceivingDiscrepancyReasonCodeID _
                                Select code.ReasonCodeAbbreviation
                    ComboBoxReasonCode.SelectedIndex = ComboBoxReasonCode.Items.IndexOf(query.Single)
                End If

                itemLoaded = True

            Else
                ' TFS 8457, 03/27/2013, Faisal Ahmed - Add the item which is not in the PO to refused item list.
                ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
                'Dim style = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question
                'Dim response = MsgBox("Item is not on this PO and will be added to refused items page.  Proceed?", style, "Item Not in PO")
                'If response = MsgBoxResult.Yes Then
                '    lblQtyReceived.Text = "Ref. Qty:"
                '    Me.lblQtyReceived.Enabled = True
                '    Me.mnuReceive.Enabled = True
                '    Me.txtQtyReceived.Enabled = True
                '    Me.txtQtyReceived.BackColor = Color.White
                '    incorrectItemRefused = True
                '    ComboBoxReasonCode.SelectedIndex = ComboBoxReasonCode.Items.IndexOf("II")
                '    ComboBoxReasonCode.Enabled = False
                '    incorrectStoreItem = storeItem
                '    PopulateIncorrectItemInformation(incorrectStoreItem)
                '    lblUOM.Visible = True
                '    cmbUOM.Visible = True

                '    Cursor.Current = Cursors.Default
                '    Exit Sub
                'End If

                Messages.ItemNotInPO()
                txtUpc.Text = String.Empty
                txtUpc.Focus()
            End If
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Function GetOrderItem(ByVal sIdentifier As String, ByVal sItem As StoreItem) As WholeFoods.Mobile.IRMA.OrderItem
        Dim oi As WholeFoods.Mobile.IRMA.OrderItem = Nothing

        If (order.OrderItems.Count > 0) Then
            For Each oi In order.OrderItems
                If (oi.Item_Key = sItem.ItemKey) Then
                    Return oi
                End If
            Next
            Return Nothing
        Else
            Return Nothing
            Exit Function
        End If
    End Function

    Private Sub PopulateIncorrectItemInformation(ByVal si As StoreItem)
        Me.lblDescriptionVal.Text = si.ItemDescription
        Me.lblPkgVal.Text = si.PackageDesc1 & " / " & si.PackageDesc2 & " " & si.PackageUnitAbbr.ToUpper
        Me.lblItemSubTeamVal.Text = order.Transfer_To_SubTeamName.ToString

        If order.Return_Order Then
            Me.lblItemSubTeamVal.Text = "(C) " & Me.lblItemSubTeamVal.Text
        Else
            Me.lblItemSubTeamVal.Text = "(P) " & Me.lblItemSubTeamVal.Text
        End If

        Me.lblPrimaryVendorVal.Text = order.CompanyName.ToString
        Me.lblPkgVal.Text = "Item Not In Order"
        Me.lblPkgVal.ForeColor = Color.Red
    End Sub

    Private Sub PopulateItemInformation()
        If Not (oi Is Nothing) Then

            Me.lblDescriptionVal.Text = oi.Item_Description.ToString
            Me.lblPkgVal.Text = oi.Package_Desc1.ToString & " / " & oi.Package_Desc2.ToString & " " & oi.PackageUnitAbbr.ToUpper
            Me.lblItemSubTeamVal.Text = order.Transfer_To_SubTeamName.ToString

            If order.Return_Order Then
                Me.lblItemSubTeamVal.Text = "(C) " & Me.lblItemSubTeamVal.Text
            Else
                Me.lblItemSubTeamVal.Text = "(P) " & Me.lblItemSubTeamVal.Text
            End If

            Me.lblPrimaryVendorVal.Text = order.CompanyName.ToString
            Me.lblOrderedVal.Text = FormatNumber(oi.QuantityOrdered, 0).ToString()
            Me.lblOrderedUOM.Text = oi.OrderUOMAbbr.ToString
            Me.lblReceivedVal.Text = FormatNumber(oi.QuantityReceived, 0).ToString()
            Me.lblReceived.Enabled = False
            Me.lblReceivedVal.Enabled = False

            If (oi.QuantityReceived <> oi.eInvoiceQuantity And order.EinvoiceID <> Nothing) Or (oi.QuantityReceived <> oi.QuantityOrdered) Then
                Me.lblReceived.ForeColor = Color.Red
                Me.lblReceived.Enabled = True
                Me.lblReceivedVal.ForeColor = Color.Red
                Me.lblReceivedVal.Enabled = True
            Else
                Me.lblReceived.ForeColor = Color.Black
                Me.lblReceived.Enabled = True
                Me.lblReceivedVal.ForeColor = Color.Black
                Me.lblReceivedVal.Enabled = True
            End If

            'TFS 4914, 10/24/2012, Faisal Ahmed - clears the quanty/weight text boxes when user scans a new UPC.
            txtQtyReceived.Text = String.Empty
            txtWeightReceived.Text = String.Empty

            If oi.IsReceivedWeightRequired Then
                Me.txtQtyReceived.Text = 1   ' default amount to add to the qty already received, user may change it.
                Me.txtWeightReceived.Text = IIf(oi.Total_Weight > 0, oi.Total_Weight, String.Empty)
                Me.txtQtyReceived.Focus()
                'Me.txtQtyReceived.ScrollToCaret()
            Else
                Me.txtQtyReceived.Text = 1  ' default quantity to add to the qty already received, user may change it.
                Me.txtQtyReceived.Focus()
                Me.txtQtyReceived.SelectAll()
                'Me.txtQtyReceived.ScrollToCaret()
            End If

            ' If an item is cost by weight and not catch weight required, then the weight received will be
            ' populated with the default 
            If Me.IsCostedByWeightItem And (Not oi.CatchweightRequired) Then
                Me.txtWeightReceived.Text = CInt(Me.txtQtyReceived.Text) * IIf(IsNumeric(oi.Package_Desc1), oi.Package_Desc1, 0)
            End If

            Me.lblEinvoiceQtyVal.Text = FormatNumber(oi.eInvoiceQuantity, 0).ToString()

        End If
    End Sub

#End Region

    Private Sub cmdSearchForExtPO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearchForExtPO.Click
        SearchForExternalPO()
    End Sub

    Private Sub txtUpc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUpc.TextChanged
        If txtUpc.Text.Length > 0 Then
            cmdSearchItem.Enabled = True
        Else
            cmdSearchItem.Enabled = False
        End If

        Me.lblDescriptionVal.Text = String.Empty
        Me.lblPkgVal.Text = String.Empty
        Me.lblOrderedVal.Text = String.Empty
        Me.lblReceivedVal.Text = String.Empty
        Me.txtQtyReceived.Text = String.Empty
        Me.txtWeightReceived.Text = String.Empty
        Me.lblOrderedUOM.Text = String.Empty
        Me.LabelEinvoiceUOM.Text = String.Empty
        Me.lblEinvoiceQtyVal.Text = String.Empty
        Me.lblEinvoiceQtyVal.BackColor = Color.Gainsboro
        Me.lblEinvoiceQty.BackColor = Color.Gainsboro
        Me.lblEinvoiceQtyUOM.Text = String.Empty
        Me.ComboBoxReasonCode.SelectedIndex = -1

        itemLoaded = False
    End Sub

    Private Sub txtExternalPONumber_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExternalPONumber.TextChanged
        Me.lblExternalID.Text = "PO#"
        order = Nothing

        lblPrimaryVendor.Enabled = False
        lblPrimaryVendorVal.Enabled = False
        lblPrimaryVendorVal.Text = String.Empty

        lblItemSubTeam.Enabled = False
        lblItemSubTeamVal.Enabled = False
        lblItemSubTeamVal.Text = String.Empty

        orderLoaded = False
        itemsReceived = False

        If txtExternalPONumber.Text.Length > 0 Then
            Me.cmdSearchForExtPO.Enabled = True
        Else
            Me.cmdSearchForExtPO.Enabled = False
        End If
    End Sub

    Private Sub MenuItemInvoiceData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemInvoiceData.Click
        Cursor.Current = Cursors.WaitCursor

        ' If this is an eInvoice vendor, and there are eInvoice discrepancies, prevent access to the Invoice Data screen.
        Dim eInvoiceDiscrepancy As Boolean = False

        If Not orderLoaded Then Exit Sub

        Try
            ' Attempt to refresh the order object.  If this fails, the user will be able to immediately retry.
            order = Me.mySession.WebProxyClient.GetOrder(order.OrderHeader_ID)

            ' Explicitly handle service faults, timeouts, and connection failures.  If this order update fails, the user will be allowed to retry the action again.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "MenuItemInvoiceData_Click")
            serviceCallSuccess = False

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "MenuItemInvoiceData_Click")
            serviceCallSuccess = False

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "MenuItemInvoiceData_Click")
            serviceCallSuccess = False
        Finally
            Cursor.Current = Cursors.Default
        End Try

        If Not serviceCallSuccess Then
            ' The service call to GetOrder failed.  End the method and allow the user to retry the last action.
            Exit Sub
        End If

        If order.EinvoiceID <> Nothing Then

            For Each item As OrderItem In order.OrderItems

                If item.QuantityReceived > 0 Then
                    itemsReceived = True
                End If

                'If item.IsReceivedWeightRequired Then
                '    'If item.IsReceivedWeightRequired Then
                '    If item.Total_Weight <> item.eInvoiceWeight And item.ReceivingDiscrepancyReasonCodeID = 0 Then 'And item.QuantityReceived > 0 Then
                '        ' Weighted item discrepancy.
                '        eInvoiceDiscrepancy = True
                '    End If

                'Else
                '    If item.QuantityReceived <> item.eInvoiceQuantity And item.ReceivingDiscrepancyReasonCodeID = 0 Then 'And item.QuantityReceived > 0 Then
                '        ' Non-weighted item discrepancy.
                '        eInvoiceDiscrepancy = True
                '    End If
                'End If

                '@@ 4.8.5.1 Fix
                If item.QuantityReceived <> item.eInvoiceQuantity And (item.QuantityReceived + item.eInvoiceQuantity) > 0 And item.ReceivingDiscrepancyReasonCodeID = 0 Then
                    ' Non-weighted item discrepancy.
                    eInvoiceDiscrepancy = True
                End If
            Next

            If eInvoiceDiscrepancy And itemsReceived Then

                If MsgBox("Reason codes are required for some items on this order.  You may review these items on the Receiving List screen.  Go there now?" _
                       , MsgBoxStyle.OkCancel, Me.Text) = MsgBoxResult.Ok Then

                    MenuItemReceivingList_Click(sender, e)
                End If
                Exit Sub

            End If

        End If

        ' Verify the order is not closed already unless it's a partial shipment.  This is in place to prevent 'sync' issues with the client.
        If order.CloseDate <> Nothing And order.PartialShipment = 0 Then
            MsgBox("This order is already closed and is not a partial shipment." + vbNewLine + "Please input a different PO#, or use the IRMA client to re-open this order.", MsgBoxStyle.OkOnly, "Warning!")

            ' Empty the form and order object.
            Me.order = Nothing
            ClearScreen()
            Cursor.Current = Cursors.Default
            Exit Sub

        End If

        ' TFS 8457, 03/30/2013, Faisal Ahmed - Forces users to navigate to refused items screen if there is refused item with no invoice cost or refused quantity.
        ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
        'Dim IsValidRefusedList = mySession.WebProxyClient.IsValidRefusedItemList(order.OrderHeader_ID)
        'If Not IsValidRefusedList Then
        '    If MsgBox("This PO contains refused items.  Review these items now?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Warning!") = MsgBoxResult.No Then
        '        Exit Sub
        '    Else
        '        MenuItemRefusedList_Click(sender, e)
        '    End If
        'End If

        Dim invoiceData = New InvoiceData(mySession, order)
        invoiceData.ShowDialog()

        If invoiceData.DialogResult = Windows.Forms.DialogResult.Yes Then
            ' Order was just closed - start with empty form.
            Me.order = Nothing
            ClearScreen()
            Cursor.Current = Cursors.Default

        ElseIf invoiceData.DialogResult = Windows.Forms.DialogResult.No Then
            ' Order was just re-opened - start with PO entered and order object re-loaded.
            txtExternalPONumber.Text = order.OrderHeader_ID.ToString()
            SearchPO(order.OrderHeader_ID.ToString(), Nothing)
            Cursor.Current = Cursors.Default

        End If
    End Sub

    Private Sub LinkLabelReasonCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LinkLabelReasonCode.Click
        Dim reasonCodeDescription = New ReasonCodeDescription(reasonCodes)
        reasonCodeDescription.ShowDialog()
    End Sub

    Private Sub MenuItemReceivingList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemReceivingList.Click
        Cursor.Current = Cursors.WaitCursor

        Try
            ' Attempt a service call to refresh the order object.
            serviceCallSuccess = True

            order = Me.mySession.WebProxyClient.GetOrder(order.OrderHeader_ID)

            ' Explicitly handle service faults, timeouts, and connection failures.  If this order update fails, the user will be allowed to retry the action again.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "MenuItemReceivingList_Click")
            serviceCallSuccess = False

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "MenuItemReceivingList_Click")
            serviceCallSuccess = False

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "MenuItemReceivingList_Click")
            serviceCallSuccess = False
        End Try

        If Not serviceCallSuccess Then
            ' The call to GetOrder failed.  End the method and allow the user to retry.
            Cursor.Current = Cursors.Default
            Exit Sub
        End If


        ' Verify the order is not closed already unless it's a partial shipment.  This is in place to prevent 'sync' issues between the client and handheld.
        If order.CloseDate <> Nothing And order.PartialShipment = 0 Then
            MsgBox("This order is already closed and is not a partial shipment." + vbNewLine + "Please input a different PO#, or use the IRMA client to re-open this order.", MsgBoxStyle.OkOnly, "Warning!")

            ' Empty the form and order object.
            Me.order = Nothing
            ClearScreen()
            Cursor.Current = Cursors.Default

            Exit Sub

        End If

        Dim receivingList As New ReceivingList(Me.mySession, Me.order, Me.reasonCodes)
        Cursor.Current = Cursors.Default

        receivingList.ShowDialog()

        If receivingList.DialogResult = Windows.Forms.DialogResult.Yes Then
            ' An order was just closed - start with empty form.
            Me.order = Nothing
            ClearScreen()
            Cursor.Current = Cursors.Default

        ElseIf receivingList.DialogResult = Windows.Forms.DialogResult.No Then
            ' An order was just re-opened - start with PO entered and order object re-loaded.
            txtExternalPONumber.Text = order.OrderHeader_ID.ToString()
            cmdSearchForExtPO_Click(sender, e)
            Cursor.Current = Cursors.Default

        ElseIf receivingList.DialogResult = Windows.Forms.DialogResult.OK Then
            ' The user has returned from the Receiving List.  If any reason codes were updated, reset the oi object so that
            ' the changes can be reflected in the UI.
            If itemLoaded Then
                oi = Nothing
                SearchItem()
            End If
        End If
    End Sub

    Private Sub FindOrdersByUPC()

        Dim currentScannedUpc = GetUpc(Me.txtUpc.Text)
        ' Verify that the UPC value is an integer (needs to be 64-bit to avoid overflow).
        Try

            Int64.Parse(currentScannedUpc)
        Catch ex As FormatException
            MessageBox.Show("Please enter a numeric value for the UPC with no special characters.", "Invalid UPC", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Cursor.Current = Cursors.Default
            txtUpc.Text = String.Empty
            txtUpc.Focus()
            Exit Sub
        Catch ex As OverflowException
            MessageBox.Show("The UPC value is too large.  Please enter a smaller value.", "Invalid UPC", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Cursor.Current = Cursors.Default
            txtUpc.Text = String.Empty
            txtUpc.Focus()
            Exit Sub
        End Try

        Dim findOrderByItem As New FindOrderByItem(mySession, currentScannedUpc)

        If findOrderByItem.ShowDialog() = Windows.Forms.DialogResult.OK Then
            ' A PO# was chosen by the user.  Populate the value and search for that PO#.
            txtExternalPONumber.Text = findOrderByItem.PoNumber
            SearchForExternalPO()

            ' If the order search is a success, search for the item as well.
            If orderLoaded Then
                SearchItem()
            End If

        Else
            ' No PO# was chosen.  Clear the UPC TextBox.
            txtUpc.Text = String.Empty
        End If

        findOrderByItem.Dispose()

    End Sub

    Private Sub AlignText()

        ' The designer does not seem to be saving alignment changes properly.  For now, they will be set programmatically.
        lblExternalID.TextAlign = ContentAlignment.TopRight
        lblUPC.TextAlign = ContentAlignment.TopRight
        lblPkg.TextAlign = ContentAlignment.TopRight
        lblItemSubTeam.TextAlign = ContentAlignment.TopRight
        lblPrimaryVendor.TextAlign = ContentAlignment.TopRight
        lblQtyReceived.TextAlign = ContentAlignment.TopRight
        lblWeightReceived.TextAlign = ContentAlignment.TopRight
        LinkLabelReasonCode.TextAlign = ContentAlignment.TopRight
        lblOrdered.TextAlign = ContentAlignment.TopRight
        lblReceived.TextAlign = ContentAlignment.TopRight
        lblEinvoiceQty.TextAlign = ContentAlignment.TopRight
        lblDescriptionVal.TextAlign = ContentAlignment.TopCenter

    End Sub

    Private Sub MenuItemRefusedList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRefusedList.Click
        Cursor.Current = Cursors.WaitCursor

        If Not orderLoaded Then Exit Sub

        Try
            ' Attempt a service call to refresh the order object.
            serviceCallSuccess = True

            order = Me.mySession.WebProxyClient.GetOrder(order.OrderHeader_ID)

            ' Explicitly handle service faults, timeouts, and connection failures.  If this order update fails, the user will be allowed to retry the action again.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "MenuItemRefusedList_Click")
            serviceCallSuccess = False

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "MenuItemRefusedList_Click")
            serviceCallSuccess = False

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "MenuItemRefusedList_Click")
            serviceCallSuccess = False

        End Try

        If Not serviceCallSuccess Then
            ' The call to GetOrder failed.  End the method and allow the user to retry the last action.
            Cursor.Current = Cursors.Default
            Exit Sub
        End If

        Try
            ' Attempt to initialize the OrderItemsRefused form.
            serviceCallSuccess = True

            Dim frm As New OrderItemsRefused(Me.mySession, order, order.CompanyName)
            frm.ShowDialog()
            frm.Dispose()

            ' Explicitly handle service faults, timeouts, and connection failures.  If this initialization fails, allow the user to retry.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "MenuItemRefusedList_Click")
            serviceCallSuccess = False

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "MenuItemRefusedList_Click")
            serviceCallSuccess = False

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "MenuItemRefusedList_Click")
            serviceCallSuccess = False

        Finally
            Cursor.Current = Cursors.Default

        End Try

    End Sub

    Private Sub txtQtyReceived_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQtyReceived.TextChanged
        If Not oi Is Nothing Then
            If Me.IsCostedByWeightItem Then
                If (Not oi.CatchweightRequired) Then
                    Me.txtWeightReceived.Text = IIf(IsNumeric(Me.txtQtyReceived.Text), Me.txtQtyReceived.Text, 0) * IIf(IsNumeric(oi.Package_Desc1), oi.Package_Desc1, 0)
                Else
                    Me.txtWeightReceived.BackColor = System.Drawing.Color.Yellow
                End If
            End If    
        End If
    End Sub

    Private Sub MenuItemOrderInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemOrderInfo.Click
        Dim orderInfo As New OrderInformation(order)

        orderInfo.OrderNotes = order.Notes
        orderInfo.IsCreditOrder = order.Return_Order
        orderInfo.Buyer = order.CreatedByName
        orderInfo.OrderDate = order.OrderDate.ToShortDateString()

        mnuMenu.Enabled = False
        orderInfo.ShowDialog()
        orderInfo.Dispose()
        mnuMenu.Enabled = True
    End Sub
End Class