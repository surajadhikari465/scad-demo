Imports System.Windows.Forms
Imports System
Imports System.Linq
Imports System.Data
Imports Microsoft.WindowsCE.Forms
Imports System.Text
Imports WholeFoods.Mobile.IRMA.Common
Imports System.ServiceModel

Public Class ItemScan
    Inherits HandheldHardware.ScanForm

    Private myItem As StoreItem
    Private myItemInfo As StoreItemOrderInfo
    Private mySession As Session
    Private serviceFault As ParsedCFFaultException = Nothing
    Private serviceCallSuccess As Boolean
    Private lastUpcScanned As String = String.Empty
    Private lastItemKeyScanned As String = String.Empty
    Private lastQtyScanned As String = String.Empty
    Private lastUOMScanned As String = String.Empty
    Private lastDescScanned As String = String.Empty
    Private CanScanDSDItem As Boolean = False


#Region " Constructors"

    Public Sub New(ByVal session As Session, Optional ByVal ScanDSDItem As Boolean = False)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.KeyPreview = True
        Me.mySession = session
        Me.CanScanDSDItem = ScanDSDItem
        AlignText()

        ' init HH scanner.
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

    Private Sub ItemScan_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'labels
        Me.Text = mySession.ActionType.ToString
        mnuMenu_ExitItem.Text = "Exit " & mySession.ActionType.ToString
        Me.lblStoreVal.Text = mySession.StoreName
        Me.lblSubTeamVal.Text = mySession.Subteam

        'visibility
        Me.frmStatus.Visible = False
        SetVisibility("load")

        'set focus on UPC
        Me.txtUpc.Focus()
    End Sub

    Private Sub ItemScan_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Dim keyPressed As Integer = -1

        Select Case (e.KeyCode)
            Case Keys.Enter Or Keys.Tab
                If Me.txtUpc.Focused And Not String.IsNullOrEmpty(Me.txtUpc.Text) Then
                    Me.cmdSearch_Click(sender, e)
                ElseIf Me.txtQty.Focused And Not String.IsNullOrEmpty(Me.txtQty.Text) And IsNumeric(Me.txtQty.Text) Then
                    Me.mnuAction_Click(sender, e)
                End If

            Case (Keys.Up)
                If Me.txtQty.Focused And Not String.IsNullOrEmpty(Me.txtQty.Text) And IsNumeric(Me.txtQty.Text) Then
                    Me.txtQty.Text = CInt(Me.txtQty.Text) + 1
                    Me.txtQty.SelectAll()
                End If

            Case (Keys.Down)
                If Me.txtQty.Focused And Not String.IsNullOrEmpty(Me.txtQty.Text) And IsNumeric(Me.txtQty.Text) Then
                    If CInt(Me.txtQty.Text) > 0 Then
                        Me.txtQty.Text = CInt(Me.txtQty.Text) - 1
                    End If

                    Me.txtQty.SelectAll()
                End If
        End Select

    End Sub

#End Region

#Region " Control Events"

    Private Sub txtUpc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUpc.GotFocus
        Me.txtUpc.SelectAll()
    End Sub

    Private Sub txtQty_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.GotFocus
        Me.txtQty.SelectAll()
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Search()
    End Sub

    Private Sub mnuMenu_ClearScreen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMenu_Clear.Click
        ClearScreen()
    End Sub

    Private Sub mnuMenu_ExitItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMenu_ExitItem.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub mnuMenu_ExitIRMA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = Windows.Forms.DialogResult.Abort
    End Sub

    Private Sub mnuAction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAction.Click
        Try
            Select Case mySession.ActionType

                Case Enums.ActionType.Order
                    If Validate(Enums.ActionType.Order) Then
                        SaveItem(Enums.ActionType.Order)
                        ClearScreen()
                        ItemScan_Load(sender, e)
                    Else
                        Exit Sub
                    End If

                Case Enums.ActionType.Credit
                    If Validate(Enums.ActionType.Credit) Then
                        SaveItem(Enums.ActionType.Credit)
                        ClearScreen()
                    Else
                        Exit Sub
                    End If

                Case Enums.ActionType.Transfer
                    If Validate(Enums.ActionType.Transfer) Then
                        SaveItem(Enums.ActionType.Transfer)
                        ClearScreen()
                    Else
                        Exit Sub
                    End If

                Case Else
                    Me.DialogResult = Windows.Forms.DialogResult.OK
            End Select

            ' Explicitly handle service faults, timeouts, and connection failures.  If the save request fails, allow the user to retry.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "mnuAction_Click")

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "mnuAction_Click")

        Catch ex As NullReferenceException
            Messages.EmptyItem()

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "mnuAction_Click")
        End Try

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

            If txtUpc.TextLength > 0 Then
                Search()
                Me.txtQty.Focus()
            Else
                txtUpc.Focus()
            End If

            If Me.txtQty.Focused And Not String.IsNullOrEmpty(Me.txtQty.Text) And IsNumeric(Me.txtQty.Text) Then
                If CInt(Me.txtQty.Text) > 0 And mySession.ActionType <> Enums.ActionType.Order Then
                    Me.txtQty.Text = CInt(Me.txtQty.Text) + 1
                End If
            End If

            Me.txtQty.SelectAll()

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
            Messages.ShowException(ex)
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
            Messages.ShowException(ex)
        End Try
    End Sub

    Public Overrides Sub UpdateUPCText(ByVal upc As String)
        Try
            Me.txtUpc.Text = upc
        Catch ex As Exception
            Messages.ShowException(ex)
        End Try
    End Sub

    Public Overrides Sub IsTriggerDown(ByVal isDown As Boolean)
        Try
            If (isDown) Then

                frmStatus.Visible = True
                frmStatus.Text = "Scan now..."
            End If

        Catch ex As Exception
            Messages.ShowException(ex)
        End Try
    End Sub

#End Region

#Region " Private Methods"

    Private Function Validate(ByVal action As Enums.ActionType) As Boolean
        Select Case action

            Case Enums.ActionType.Order
                'validate if upc previously scanned
                If myItemInfo.QtyOnQueue > 0 Then
                    Dim response As MsgBoxResult = Messages.UPCScanned(myItem.Identifier, myItem.ItemDescription)

                    If response = MsgBoxResult.No Then
                        Return False
                    End If
                End If

            Case Enums.ActionType.Credit
                'validate if upc previously scanned
                If myItemInfo.QtyOnQueueCredit > 0 Then
                    Dim response As MsgBoxResult = Messages.UPCScanned(myItem.Identifier, myItem.ItemDescription)

                    If response = MsgBoxResult.No Then
                        Return False
                    End If
                End If

            Case Enums.ActionType.Transfer
                'validate if upc previously scanned
                If myItemInfo.QtyOnQueueTransfer > 0 Then
                    Dim response As MsgBoxResult = Messages.UPCScanned(myItem.Identifier, myItem.ItemDescription)

                    If response = MsgBoxResult.No Then
                        Return False
                    End If
                End If

        End Select

        Try
            'validate Qty
            If Not String.IsNullOrEmpty(Me.txtQty.Text) Then
                If IsNumeric(Me.txtQty.Text) And CInt(Me.txtQty.Text) > 0 Then
                    If Not myItem.CostedByWeight Then
                        Try
                            Dim myqty As Integer = Integer.Parse(Me.txtQty.Text)
                            Return True
                        Catch ex As Exception
                            Messages.QtyNumberException()
                            Return False
                        End Try
                    Else
                        Return True
                    End If
                ElseIf IsNumeric(Me.txtQty.Text) And CInt(Me.txtQty.Text) = 0 And Enums.ActionType.Order Then
                    Dim response = Messages.DeleteItemInQueue()
                    If response = MsgBoxResult.No Then
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Messages.QtyNumberException()
                    Return False
                End If
            Else
                Messages.QtyNumberException()
                Return False
            End If
        Catch ex As Exception
            Messages.ShowException(ex)
            Return False
        End Try
    End Function

    Private Sub SaveItem(ByVal action As Enums.ActionType)
        Select Case action

            Case Enums.ActionType.Order
                Me.mySession.WebProxyClient.AddToOrderQueue(False, False, txtQty.Text, myItem.VendorUnitId, mySession.UserID, myItem)

            Case Enums.ActionType.Credit
                Me.mySession.WebProxyClient.AddToOrderQueue(False, True, txtQty.Text, myItem.VendorUnitId, mySession.UserID, myItem)

            Case Enums.ActionType.Transfer
                Me.mySession.WebProxyClient.AddToOrderQueue(True, False, txtQty.Text, myItem.VendorUnitId, mySession.UserID, myItem)
                
        End Select

        lastUpcScanned = Me.txtUpc.Text
        lastQtyScanned = Me.txtQty.Text
        lastUOMScanned = Me.lblPkgVal.Text
        lastDescScanned = Me.lblDescriptionVal.Text
    End Sub

    Private Sub Search()
        Cursor.Current = Cursors.WaitCursor

        Dim iResult As Integer = 0
        Dim sMsg As String = ""
        Dim IsDSDVendor As Boolean = False

        If Not String.IsNullOrEmpty(txtUpc.Text) Then

            Dim currentScannedUpc = GetUpc(Me.txtUpc.Text)
            'check for scale item
            Me.txtUpc.Text = ScaleItemCheck(currentScannedUpc)

            Try
                IsDSDVendor = Me.mySession.WebProxyClient.IsDSDStoreVendorByUPC(Me.txtUpc.Text, mySession.StoreNo)

                If IsDSDVendor And CanScanDSDItem = False Then
                    Messages.CannotScanForDSDVendor() 'disable PO creation for DSD Vendors 
                    ClearScreen()
                Else
                    Dim sSubteam As String = mySession.SubteamKey
                    If mySession.ActionType = Enums.ActionType.PrintSign Or _
                        mySession.ActionType = Enums.ActionType.ItemCheck Then
                        sSubteam = Nothing
                    End If

                    myItem = Me.mySession.WebProxyClient.GetStoreItem(mySession.StoreNo, sSubteam, mySession.UserID, Nothing, Me.txtUpc.Text)
                    If Exceptions.ValidIdentifier(myItem) Then
                        Select Case mySession.ActionType
                            Case Enums.ActionType.PrintSign, Enums.ActionType.ItemCheck

                                'set visibility
                                SetVisibility("action")

                                'populate form
                                PopulateItemInformation()
                                txtQty.Focus()

                                'send to print queue
                                If mySession.ActionType = Enums.ActionType.PrintSign Then
                                    If chkSkipConfirm.Checked Then
                                        PrintItem(lblItemKeyVal.Text)
                                    Else
                                        'confirm
                                        If myItem.OnSale Then
                                            sMsg = "SubTeam: " + Me.lblItemSubTeamVal.Text + _
                                                vbCrLf + "Pkg: " + Me.lblPkgVal.Text + _
                                                vbCrLf + "Price: " + Me.lblPriceVal.Text + _
                                                vbCrLf + "Sale Price: " + Me.lblSaleVal.Text + _
                                                vbCrLf + "Sale Dates: " + Me.lblSaleDatesVal.Text + _
                                                vbCrLf + "Print now?"
                                        Else
                                            sMsg = "SubTeam: " + Me.lblItemSubTeamVal.Text + _
                                                vbCrLf + "Pkg: " + Me.lblPkgVal.Text + _
                                                vbCrLf + "Price: " + Me.lblPriceVal.Text + _
                                                vbCrLf + "Print now?"
                                        End If
                                        If MsgBox(sMsg, MsgBoxStyle.YesNo, "Print Tag") = MsgBoxResult.Yes Then
                                            PrintItem(lblItemKeyVal.Text)
                                        Else
                                            Me.txtUpc.Focus()
                                            Me.txtUpc.SelectAll()
                                            lblPrintedVal.Text = 0
                                        End If
                                    End If
                                End If

                            Case Else
                                If Not Exceptions.CanInventory(myItem) Then
                                    Messages.InvalidOrderItemCanInventory(mySession.SubteamKey, myItem.Identifier, myItem.ItemDescription)
                                    ClearScreen()
                                ElseIf Not myItem.IsItemAuthorized Then
                                    MsgBox("This item is not authorized for this store.", MsgBoxStyle.Critical, "Item Not Authorized")
                                    ClearScreen()
                                Else
                                    'set visibility
                                    SetVisibility("action")

                                    'populate form
                                    PopulateItemInformation()
                                    txtQty.Focus()
                                End If
                        End Select
                    Else
                        'invalid identifier
                        Messages.ItemNotFound()
                        ClearScreen()
                    End If

                End If

                ' Explicitly handle service faults, timeouts, and connection failures.  If this search routine fails, allow the user to retry.
            Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                Dim err As New ErrorHandler(serviceFault)
                err.ShowErrorNotification()

            Catch ex As TimeoutException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "Search")

            Catch ex As CommunicationException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "Search")

            Catch ex As Exception
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(ex.Message, "Search")
            End Try

        Else
            Me.txtUpc.Focus()
            Me.txtUpc.SelectAll()
            ClearScreen()
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ClearScreen()
        Cursor.Current = Cursors.WaitCursor

        Me.txtQty.Text = ""
        Me.txtUpc.Text = ""
        Me.lblDescriptionVal.Text = ""
        Me.lblItemKeyVal.Text = ""

        Me.lblPkgVal.Text = ""
        Me.lblItemSubTeamVal.Text = ""
        Me.lblPriceTypeVal.Text = ""
        Me.lblPriceVal.Text = ""
        Me.lblSaleVal.Text = ""
        Me.lblSaleDatesVal.Text = ""

        Me.lblPrimaryVendorVal.Text = ""

        Me.lblOrderedVal.Text = "0"
        Me.lblQueuedVal.Text = "0"
        Me.lblReceivedVal.Text = "0"

        lblLastReceivedDateVal.Text = String.Empty

        mnuAction.Enabled = False

        Me.txtUpc.Focus()

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub PrintItem(ByVal iItemKey As Integer)
        Dim iScanGunType As Integer = 2

        mySession.WebProxyClient.AddToReprintSignQueue(mySession.UserID, iScanGunType, iItemKey.ToString, "|", mySession.StoreNo)

        lblPrintedVal.Text = 1
        txtUpc.Text = ""
        txtUpc.Focus()
    End Sub

    Private Sub SetVisibility(ByVal operation As String)
        'action button options
        If mySession.ActionType = Enums.ActionType.Order Or mySession.ActionType = Enums.ActionType.Transfer Or mySession.ActionType = Enums.ActionType.Credit Then
            mnuAction.Text = "Save"
        Else
            mnuAction.Text = "Exit"
        End If

        'form object visibility
        Select Case operation

            Case "load"

                Dim aryHiddenControls() As String = New String() {"txtUpc", "lblUpc", "cmdSearch"}
                If mySession.ActionType = Enums.ActionType.PrintSign Then
                    aryHiddenControls = New String() {"txtUpc", "lblUpc", "cmdSearch", "chkSkipConfirm"}
                End If
                Array.Sort(aryHiddenControls)
                ToggleControlVisibility(aryHiddenControls, Me.Controls, False)

            Case "sale"
                lblItemKeyVal.Visible = False
                lblSale.Visible = False
                lblSaleVal.Visible = False
                lblSaleDates.Visible = False
                lblSaleDatesVal.Visible = False

            Case "action"
                mnuAction.Enabled = True

                Select Case mySession.ActionType

                    Case Enums.ActionType.Order, Enums.ActionType.Transfer, Enums.ActionType.Credit
                        Dim aryHiddenControls() As String = New String() {"lblItemKeyVal", "lblPrice", "lblPriceVal", "lblSale", "lblSaleVal", "lblSaleDates", "lblSaleDatesVal", "lblPrinted", "lblPrintedVal", "chkSkipConfirm", "DisplayItemMov", "DisplayEInvoiceQty"}
                        Array.Sort(aryHiddenControls)
                        ToggleControlVisibility(aryHiddenControls, Me.Controls)

                    Case Enums.ActionType.ItemCheck
                        Dim aryHiddenControls() As String = New String() {"lblItemKeyVal", "lblQty", "txtQty", "lblUomVal", "chkSkipConfirm", "DisplayItemMov", "DisplayEInvoiceQty"}
                        Array.Sort(aryHiddenControls)
                        ToggleControlVisibility(aryHiddenControls, Me.Controls)

                    Case Enums.ActionType.PrintSign
                        frmStatus.Hide()
                        Dim aryHiddenControls() As String = New String() {"lblItemKeyVal", "lblOrdered", "lblOrderedVal", "lblLastReceivedDate", "lblLastReceivedDateVal", "lblQueued", "lblQueuedVal", "lblReceived", "lblReceivedVal", "lblQty", "txtQty", "lblUomVal", "DisplayItemMov", "DisplayEInvoiceQty"}
                        Array.Sort(aryHiddenControls)
                        ToggleControlVisibility(aryHiddenControls, Me.Controls)
                End Select

            Case Else
                Dim aryHiddenControls() As String = New String() {"lblItemKeyVal", "DisplayItemMov", "DisplayEInvoiceQty"}
                Array.Sort(aryHiddenControls)
                ToggleControlVisibility(aryHiddenControls, Me.Controls)
        End Select
    End Sub

    Private Sub PopulateItemInformation()
        'store item info
        Me.lblItemKeyVal.Text = myItem.ItemKey

        If (myItem.ItemDescription.Length > 50) Then
            Me.lblDescriptionVal.Text = myItem.ItemDescription.Substring(0, 50)
        Else
            Me.lblDescriptionVal.Text = myItem.ItemDescription
        End If

        Me.lblPriceTypeVal.Text = myItem.PriceChgTypeDesc
        Me.lblPkgVal.Text = myItem.PackageDesc1 & " / " & myItem.PackageDesc2 & " " & myItem.PackageUnitAbbr
        Me.lblPriceVal.Text = myItem.Multiple & " / " & myItem.Price.ToString()
        Me.lblPrimaryVendorVal.Text = myItem.VendorName
        Me.lblUomVal.Text = myItem.VendorUnitName

        If myItem.OnSale = True Then
            Me.lblSaleVal.Text = myItem.SaleMultiple.ToString + " / " + myItem.SalePrice.ToString()
            Me.lblSaleDatesVal.Text = myItem.SaleStartDate + " - " + myItem.SaleEndDate
        Else
            SetVisibility("sale")
        End If

        If mySession.ActionType = Enums.ActionType.Order Then
            If myItem.OnSale = True Then
                Me.lblSaleDates.Visible = True
                Me.lblSaleDatesVal.Visible = True
            End If
            DisplayItemMov.Visible = True
            DisplayEInvoiceQty.Visible = True
        End If

        'order item info
        myItemInfo = Me.mySession.WebProxyClient.GetStoreItemOrderInfo(mySession.StoreNo, mySession.SubteamKey, lblItemKeyVal.Text)
        Me.lblOrderedVal.Text = myItemInfo.QtyOnOrder

        'TFS 3847, Faisal Ahmed, 01/11/2012
        If myItemInfo.LastReceivedDate <> "1/1/1900" Then
            Me.lblLastReceivedDateVal.Text = myItemInfo.LastReceivedDate.ToString("d")
        End If

        Select Case mySession.ActionType
            Case Enums.ActionType.Credit
                Me.lblQueuedVal.Text = myItemInfo.QtyOnQueueCredit

                If myItemInfo.QtyOnQueueCredit > 0 Then
                    Me.txtQty.Text = myItemInfo.QtyOnQueueCredit
                End If
            Case Enums.ActionType.Transfer
                Me.lblQueuedVal.Text = myItemInfo.QtyOnQueueTransfer

                If myItemInfo.QtyOnQueueTransfer > 0 Then
                    Me.txtQty.Text = myItemInfo.QtyOnQueueTransfer
                End If
            Case Enums.ActionType.Order
                Me.lblQueuedVal.Text = myItemInfo.QtyOnQueue

                'If myItemInfo.QtyOnQueue > 0 Then
                '    Me.txtQty.Text = CStr(myItemInfo.QtyOnQueue + 1.0)
                'Else
                Me.txtQty.Text = 1
                'End If
            Case Else
                Me.lblQueuedVal.Text = myItemInfo.QtyOnQueue

                If myItemInfo.QtyOnQueue > 0 Then
                    Me.txtQty.Text = myItemInfo.QtyOnQueue
                End If
        End Select

        Me.lblReceivedVal.Text = myItemInfo.LastReceived

        'subteam logic
        If IsNothing(myItem.TransferToSubteamName) Then
            Me.lblItemSubTeamVal.Text = myItem.RetailSubteamName
        Else
            Me.lblItemSubTeamVal.Text = myItem.TransferToSubteamName
        End If

        'qty label logic
        If myItem.SoldByWeight Then
            Me.lblQty.Text = "Weight:"
        Else
            Me.lblQty.Text = "Qty:"
        End If
    End Sub

    Private Sub DisplayItemMov_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayItemMov.Click
        Dim uMessage As New dlgItemMovements(mySession, Me.txtUpc.Text)
        Dim itemMovementLoad As Boolean

        Try
            itemMovementLoad = uMessage.dlgItemMovement_Load(myItem.VendorID, Me.lblDescriptionVal.Text, Me.lblPkgVal.Text)

            ' Explicitly handle service faults, timeouts, and connection failures.  
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "DisplayItemMov_Click")

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "DisplayItemMov_Click")

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "DisplayItemMov_Click")
        End Try

        If itemMovementLoad = True Then
            uMessage.ShowDialog()
            uMessage.Close()
            uMessage.Dispose()
        Else
            MessageBox.Show("No movement is available for this item.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        End If

    End Sub

    Private Sub DisplayEInvoiceQty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayEInvoiceQty.Click
        Dim uMessage As New dlgBilledQuantity(mySession, Me.txtUpc.Text)
        Dim displayBilledQuantity As Boolean

        Try
            displayBilledQuantity = uMessage.DisplayBilledQuantity(myItem.VendorID, Me.lblDescriptionVal.Text, Me.lblPkgVal.Text)

            ' Explicitly handle service faults, timeouts, and connection failures.  
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "DisplayEInvoiceQty_Click")
            serviceCallSuccess = False

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "DisplayEInvoiceQty_Click")
            serviceCallSuccess = False

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "DisplayEInvoiceQty_Click")
            serviceCallSuccess = False
        End Try

        If Not serviceCallSuccess Then
            ' A call in DisplayBilledQuantity() failed.  End the method.
            Exit Sub
        End If

        If displayBilledQuantity = True Then
            uMessage.ShowDialog()
            uMessage.Close()
            uMessage.Dispose()
        Else
            MessageBox.Show("No open orders have an eInvoice loaded for this item.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        End If
    End Sub

#End Region

    Private Sub AlignText()
        lblOrdered.TextAlign = ContentAlignment.TopRight
        lblQueued.TextAlign = ContentAlignment.TopRight
        lblReceived.TextAlign = ContentAlignment.TopRight
        lblLastReceivedDate.TextAlign = ContentAlignment.TopRight
        lblQty.TextAlign = ContentAlignment.TopRight
        lblUpc.TextAlign = ContentAlignment.TopRight
        lblPriceType.TextAlign = ContentAlignment.TopRight
        lblPkg.TextAlign = ContentAlignment.TopRight
        lblItemSubTeam.TextAlign = ContentAlignment.TopRight
        lblPrice.TextAlign = ContentAlignment.TopRight
        lblSale.TextAlign = ContentAlignment.TopRight
        lblSaleDates.TextAlign = ContentAlignment.TopRight
    End Sub

End Class