Imports System.Windows.Forms
Imports System
Imports System.Linq
Imports System.Data
Imports WholeFoods.Mobile.IRMA.Common
Imports System.ServiceModel

Public Class TransferScan
    Inherits HandheldHardware.ScanForm

    Public subteam As String
    Public store As String
    Private mySession As Session
    Private serviceFault As ParsedCFFaultException = Nothing
    Private serviceCallSuccess As Boolean
    Private sessionUtility As SessionUtility = New SessionUtility
    Private allowSubteamKeys As Hashtable = New Hashtable
    Private scannedItems As Hashtable = New Hashtable
    Private lastUpcScanned As String = String.Empty
    Private lastItemKeyScanned As String = String.Empty
    Private lastQtyScanned As String = String.Empty
    Private lastUOMScanned As String = String.Empty
    Private lastDescScanned As String = String.Empty
    Private myItem As StoreItem
    Private myItemKey As String = String.Empty
    Private myQty As Double = 0
    Private ItemIsLoaded As Boolean = False
    Private reasonCodes As ReasonCode()

#Region " Constructors"

    Public Sub New(ByVal session As Session)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.KeyPreview = True
        Me.mySession = session
        AlignText()

        'init HH scanner
        If Not mySession.MyScanner Is Nothing Then
            mySession.MyScanner.restoreScannerSettings()
        End If
        mySession.MyScanner = New HandheldHardware.HandheldScanner(Me)
        If (mySession.MyScanner.HHType = HandheldHardware.Scanner.UNKNOWN) Then
            Messages.ScannerNotAvailable()
        End If

        'update scanned
        Dim fileWriter As TransferFileWriter = New TransferFileWriter(Me.mySession)
        scannedItems = fileWriter.getScannedItemHash()

    End Sub
#End Region

#Region " Form Events"

    Private Sub TransferScan_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.frmStatus.Visible = False
        Me.txtItemCost.Visible = False
        Me.lblCost.Text = "Vendor Cost"

        Try
            reasonCodes = mySession.WebProxyClient.GetReasonCodesByType(Enums.ReasonCodeType.CA.ToString())

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
            Exit Sub
        End If

        ' Leave index 0 blank so the user has a way to clear the selection.
        cmbReason.Items.Add(String.Empty)
        For Each reasonCode As ReasonCode In reasonCodes
            cmbReason.Items.Add(reasonCode.ReasonCodeAbbreviation)
        Next

        lnlReason.Visible = False
        cmbReason.SelectedIndex = 0
        cmbReason.Visible = False

        'set focus on UPC
        Me.txtUpc.Focus()

    End Sub

    Private Sub TransferScan_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Select Case (e.KeyCode)

            Case (Keys.Tab)

                If Me.txtUpc.Focused And Not String.IsNullOrEmpty(Me.txtUpc.Text) Then

                    Me.cmdSearch_Click(sender, e)

                End If

        End Select
    End Sub

    Private Sub TransferScan_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        Dim keyPressed As Integer = -1

        Select Case (e.KeyCode)

            Case (Keys.Enter)

                If Me.txtUpc.Focused And Not String.IsNullOrEmpty(Me.txtUpc.Text) Then

                    Me.cmdSearch_Click(sender, e)

                ElseIf Me.txtQty.Focused And Not String.IsNullOrEmpty(Me.txtQty.Text) And IsNumeric(Me.txtQty.Text) And ItemIsLoaded Then

                    ' restet this so if the same item is scanned, it doesn't add to the previous saved qty
                    lastQtyScanned = 0

                    Me.cmdSave_Click(sender, e)
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

    Private Sub mnuMenu_Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMenu_Back.Click
        mySession.IsLoadedSession = False
        Me.Close()
    End Sub

    Private Sub lnlReason_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnlReason.Click
        Dim reasonCodeDescription = New ReasonCodeDescription(reasonCodes)
        reasonCodeDescription.ShowDialog()
    End Sub

    Private Sub txtQty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtQty.KeyPress
        If myItem Is Nothing Then
            e.Handled = True
            Exit Sub
        End If
        If myItem.CostedByWeight Or myItem.SoldByWeight Then
            'allow numbers and 1 decimal to be entered
            If Char.IsDigit(e.KeyChar) _
                Or e.KeyChar = Chr(46) _
                Or e.KeyChar = Chr(8) Then
                'check for existing decimal point
                If e.KeyChar = Chr(46) And txtQty.Text.Contains(Chr(46)) Then
                    e.Handled = True
                End If

                If e.KeyChar = Chr(46) Or txtQty.Text.Contains(Chr(46)) Then
                    txtQty.MaxLength = 6
                Else
                    txtQty.MaxLength = 5
                End If

                'check the positioning of the char being input
                If txtQty.Text.Contains(Chr(46)) And e.KeyChar <> Chr(8) Then
                    Dim iSelPos As Integer = txtQty.SelectionStart
                    Dim iDecPos As Integer = txtQty.Text.IndexOf(Chr(46)) + 1
                    If Len(Mid(txtQty.Text, iDecPos, Len(txtQty.Text))) > 2 And iSelPos = Len(txtQty.Text) Then
                        e.Handled = True
                    End If
                End If
            Else
                e.Handled = True
            End If
        Else
            'allow only numbers
            If Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8)) Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub txtItemCost_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemCost.KeyPress
        If myItem Is Nothing Then
            e.Handled = True
            Exit Sub
        End If

        'allow only numbers, backspace, and one decimal point
        If Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8) Or e.KeyChar = Chr(46)) Then
            e.Handled = True
        ElseIf e.KeyChar = Chr(46) And txtItemCost.Text.Contains(Chr(46)) Then
            e.Handled = True
        End If
    End Sub

    Private Sub mnuMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMenu.Click
        Me.DialogResult = Windows.Forms.DialogResult.None
    End Sub

    Private Sub txtUpc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.txtUpc.SelectAll()
    End Sub

    Private Sub txtQty_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.txtQty.SelectAll()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try

            If Len(lblItemCost.Text) > 0 And Len(txtItemCost.Text) > 0 Then
                Dim reasonCode As Integer = Common.GetReasonCodeID(reasonCodes, cmbReason.SelectedItem)
                If Double.Parse(lblItemCost.Text) > 0 Or Double.Parse(txtItemCost.Text) > 0 Then
                    SaveItem(txtUpc.Text, myItemKey, txtQty.Text, myItem.retailUnitId, lblRetailUnitName.Text, lblVendorPackValue.Text, Double.Parse(lblItemCost.Text), Double.Parse(txtItemCost.Text), reasonCode, lblDescription.Text, myItem.SoldByWeight, True)
                Else
                    Messages.ZeroCostItem()
                End If
            Else
                Messages.ZeroCostItem()
            End If

        Catch ex As NullReferenceException
            Messages.EmptyItem()
        End Try
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Search()
    End Sub

    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        ClearScreen()
    End Sub

    Private Sub mnuMenu_DeleteOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMenu_DeleteOrder.Click
        Cursor.Current = Cursors.WaitCursor

        Try
            'delete session
            Dim resp As MsgBoxResult = Messages.DeleteOrder()

            If (resp = MsgBoxResult.Yes) Then
                Dim fileWriter As TransferFileWriter = New TransferFileWriter(Me.mySession)
                fileWriter.DeleteFile(fileWriter.MakeFilePath(Me.mySession.SessionName))
                If (Not Me.mySession.SessionName = Nothing) Then
                    Me.mySession.SessionName = Nothing
                End If
                Me.Close()
            End If

        Catch exp As Exception
            Me.Close()

        End Try

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub mnuMenu_SaveOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMenu_SaveOrder.Click

        'Show saved filename
        Dim fileWriter As TransferFileWriter = New TransferFileWriter(Me.mySession)

        MessageBox.Show("Order saved to file: " + fileWriter.MakeFilePath(Me.mySession.SessionName), "Info")

        Me.Close()

    End Sub

    Private Sub mmuReview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mmuReview.Click
        Cursor.Current = Cursors.WaitCursor

        Dim cont As Boolean = True
        Dim rtransfer As TransferReview = New TransferReview(Me.mySession)

        If (cont) Then
            Dim res As DialogResult = rtransfer.ShowDialog()
            If res = Windows.Forms.DialogResult.OK Or res = Windows.Forms.DialogResult.Cancel Then
                Me.Close()
            End If
            rtransfer.Dispose()
        End If

        Cursor.Current = Cursors.Default
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

            Search()

            Me.txtQty.Focus()

            If Me.txtUpc.Text.Equals(lastUpcScanned) Then

                If Me.txtQty.Focused And Not String.IsNullOrEmpty(Me.txtQty.Text) And IsNumeric(Me.txtQty.Text) Then

                    If CInt(Me.txtQty.Text) > 0 Then
                        Me.txtQty.Text = CInt(Me.txtQty.Text) + 1
                    End If

                End If

            ElseIf String.IsNullOrEmpty(lastUpcScanned) Then

                Me.txtQty.Text = "1"

            End If

            lastUpcScanned = Me.txtUpc.Text
            lastItemKeyScanned = myItemKey
            lastQtyScanned = Me.txtQty.Text
            lastUOMScanned = Me.lblVendorPackValue.Text
            lastDescScanned = Me.lblDescription.Text

            Me.txtQty.SelectAll()

        Catch e As Exception

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

            If (Not (frmStatus.Text.Equals("*** Scan failed ***")) And (Not (frmStatus.Text.Equals("Scan complete!")))) Then

                'StatusBar1.Visible = True
                'StatusBar1.Text = "User abandoned"

            End If
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

    Public Overrides Sub UpdateUPCText(ByVal upc As String)
        Try

            Me.txtUpc.Text = upc

        Catch ex As Exception

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

#End Region

#Region " Private Methods"

    Private Sub SaveItem(ByVal upc As String, ByVal itemKey As String, ByVal qty As Double, ByVal retailUomId As Integer, ByVal retailUom As String, ByVal vendorPack As String, ByVal vendorCost As Double, ByVal adjustedCost As Double, ByVal adjustedReason As Integer, ByVal desc As String, ByVal soldByWeight As Boolean, ByVal replaceQty As Boolean)

        Try
            'check UPC and DESC are not empty
            If (String.IsNullOrEmpty(upc) = False And String.IsNullOrEmpty(desc) = False) Then

                'update qty if upc already saved
                Dim keys As System.Collections.ICollection = scannedItems.Keys
                If (scannedItems.Contains(upc)) Then
                    If Not replaceQty Then
                        qty = qty + myQty
                    End If
                    scannedItems.Remove(upc)
                End If

                'check qty field is a number
                Try
                    If myItem.SoldByWeight Then
                        Dim myqty As Double = Double.Parse(qty)
                    Else
                        Dim myqty As Integer = Integer.Parse(qty)
                    End If
                Catch ex As Exception
                    Messages.QtyNumberException()
                    Exit Sub
                End Try

                If vendorCost = 0 And adjustedReason <= 0 Then
                    Messages.AdjustedCostReasonMissing()
                    cmbReason.Focus()
                    Exit Sub
                End If

                Dim sessName As String
                Dim fileWriter As TransferFileWriter = New TransferFileWriter(Me.mySession)
                Dim bScan As Boolean = True
                Dim totCost As Double = 0

                If vendorCost > 0 Then
                    totCost = qty * vendorCost
                ElseIf adjustedCost > 0 Then
                    totCost = qty * adjustedCost
                End If

                'TFS 5500, 7383 - Faisal Ahmed
                If mySession.IsLoadedSession = False Then
                    sessName = fileWriter.GenerateSessionName()
                    mySession.SessionName = sessName
                End If

                If Not fileWriter.SessionFileExists() Then
                    fileWriter.SaveItem(mySession.SessionName, upc, itemKey, qty, retailUomId, retailUom, vendorPack, vendorCost, adjustedCost, adjustedReason, totCost, desc, soldByWeight, True) '//file does not exist yet
                Else
                    If fileWriter.isPreviouslyScanned(upc) Then
                        Dim sMsgBoxQty As String = String.Empty

                        'parse file to get value for msgbox below
                        Dim datalist As List(Of XNode) = New List(Of XNode)

                        fileWriter.GetShrinkList(datalist, mySession.SessionName)

                        For Each item As XElement In datalist
                            If item.Attribute("UPC") = upc Then
                                sMsgBoxQty = item.Attribute("QTY")
                            End If
                        Next

                        Dim uMessage As New dlgQuestion
                        uMessage.BodyText = String.Format("{0} of this item already queued in this session.  Tap Add, Overwrite, or Cancel", sMsgBoxQty)
                        uMessage.ShowDialog()

                        Dim dlgResult As String = uMessage.Result
                        uMessage = Nothing

                        If dlgResult = "A" Then
                            qty = qty + Val(sMsgBoxQty)
                        End If

                        If dlgResult <> "C" Then
                            fileWriter.SaveItem(mySession.SessionName, upc, itemKey, qty, retailUomId, retailUom, vendorPack, vendorCost, adjustedCost, adjustedReason, totCost, desc, soldByWeight, False)
                        Else
                            bScan = False
                        End If

                    Else
                        fileWriter.SaveItem(mySession.SessionName, upc, itemKey, qty, retailUomId, retailUom, vendorPack, vendorCost, adjustedCost, adjustedReason, totCost, desc, soldByWeight, False)
                    End If
                End If

                If bScan Then
                    scannedItems.Add(upc, qty)
                    MsgBox(String.Format("Transfer for UPC {0} saved successfully with Qty of {1}.", upc, qty), MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Transfer Saved")
                    Me.ClearScreen()
                Else
                    MsgBox(String.Format("Transfer for UPC {0} was not saved.", upc), MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Transfer Save Cancelled")
                End If
            End If

        Catch exp As Exception

        End Try

    End Sub

    Private Sub ClearScreen()

        Cursor.Current = Cursors.WaitCursor

        Me.txtQty.Text = 1
        Me.txtUpc.Text = ""
        Me.lblVendorPackValue.Text = ""
        Me.lblDescription.Text = ""
        Me.lblRetailUnitName.Text = ""
        Me.lblItemCost.Text = ""
        Me.txtItemCost.Text = ""
        lblItemCost.Visible = True
        Me.txtItemCost.Visible = False
        Me.lblCost.Text = "Vendor Cost"
        lnlReason.Visible = False
        cmbReason.SelectedIndex = -1
        cmbReason.Visible = False
        Me.txtUpc.Focus()
        myItem = Nothing
        myQty = 0
        Cursor.Current = Cursors.Default
        Me.lblIQueuedQty.Text = "Queued:"
    End Sub

    Private Sub Search()
        Cursor.Current = Cursors.WaitCursor

        Dim result As StoreItem = Nothing
        ItemIsLoaded = False

        If Not String.IsNullOrEmpty(txtUpc.Text) Then
            Try
                ' Attempt a service call to search for an item.
                serviceCallSuccess = True

                'check for scale item
                Me.txtUpc.Text = ScaleItemCheck(Me.txtUpc.Text)
                result = Me.mySession.WebProxyClient.GetTransferItem(0, Me.txtUpc.Text, mySession.SelectedProductType, Convert.ToInt32(mySession.TransferFromStoreNo), mySession.transferVendorId, Convert.ToInt32(mySession.TransferFromSubteamKey), mySession.SelectedSupplySubteam.ToString())

                ' Explicitly handle service faults, timeouts, and connection failures.  If this search fails, allow the user to retry.
            Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                Dim err As New ErrorHandler(serviceFault)
                err.ShowErrorNotification()
                serviceCallSuccess = False

            Catch ex As TimeoutException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "Search")
                serviceCallSuccess = False

            Catch ex As CommunicationException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "Search")
                serviceCallSuccess = False

            Catch ex As Exception
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(ex.Message, "Search")
                serviceCallSuccess = False
            End Try

            If Not serviceCallSuccess Then
                ' The call to GetTransferItem failed.  End the method and allow the user to search again.
                Cursor.Current = Cursors.Default
                Exit Sub
            End If

            If Not IsNothing(result) And result.ItemKey > 0 Then
                myItem = result

                If myItem.GLAcct = 0 And mySession.SelectedProductType <> Enums.ProductType.Product Then
                    If mySession.SelectedProductType = Enums.ProductType.PackagingSupplies Then
                        Messages.subteamMissingGLAcct(myItem.RetailSubteamName, "Packaging")
                    Else
                        Messages.subteamMissingGLAcct(myItem.RetailSubteamName, "Supplies")
                    End If
                    ClearScreen()
                Else
                    If Me.mySession.transferVendorId = 0 Then
                        Me.mySession.transferVendorId = myItem.VendorID
                    End If

                    Me.lblDescription.Text = myItem.ItemDescription

                    myItemKey = myItem.ItemKey

                    If (scannedItems.Contains(txtUpc.Text)) Then
                        myQty = scannedItems.Item(txtUpc.Text)
                    Else
                        myQty = 0
                    End If

                    lblIQueuedQty.Text = "Queued: " & Str(myQty)
                    Me.lblVendorPackValue.Text = myItem.vendorPack
                    Me.lblRetailUnitName.Text = myItem.retailUnitName

                    If (IsNothing(mySession.SelectedSupplySubteam) Or mySession.SelectedSupplySubteam = 0) And mySession.SelectedProductType <> Enums.ProductType.Product Then
                        mySession.SelectedSupplySubteam = myItem.RetailSubteamNo
                    End If

                    If myItem.SoldByWeight Then
                        txtQty.MaxLength = 6
                    Else
                        txtQty.MaxLength = 3
                    End If

                    If myItem.vendorCost <= 0 Then
                        Messages.ZeroCostItem()
                        lblItemCost.Text = "0"
                        txtItemCost.Text = "0"
                        lblCost.Text = "Adjusted Cost:"
                        lblItemCost.Visible = False
                        txtItemCost.Visible = True
                        lnlReason.Visible = True
                        cmbReason.Visible = True
                    Else
                        lblItemCost.Text = myItem.vendorCost
                        txtItemCost.Text = "0"
                        lblCost.Text = "Vendor Cost:"
                        lblItemCost.Visible = True
                        txtItemCost.Visible = False
                        lnlReason.Visible = False
                        cmbReason.Visible = False
                        cmbReason.SelectedIndex = 0
                    End If

                    Me.txtQty.Focus()
                    Me.txtQty.SelectAll()

                    ItemIsLoaded = True
                    End If
            Else
                    myItem = Nothing
                    Messages.ItemNotFound()
                    ClearScreen()

            End If
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ShowTransferReview()
        Dim rtransfer As TransferReview = New TransferReview(Me.mySession)

        Dim res As DialogResult = rtransfer.ShowDialog()

        If res = Windows.Forms.DialogResult.Abort Then
            Me.DialogResult = Windows.Forms.DialogResult.Abort
        ElseIf res = Windows.Forms.DialogResult.OK Or res = Windows.Forms.DialogResult.Cancel Then
            Me.Close()
        End If

        rtransfer.Dispose()

    End Sub
    Private Sub AlignText()
        lblUPC.TextAlign = ContentAlignment.TopRight
        lblDesc.TextAlign = ContentAlignment.TopRight
    End Sub
#End Region

End Class