Imports System.Windows.Forms
Imports System
Imports System.Text
Imports System.Linq
Imports System.Data
Imports Microsoft.WindowsCE.Forms
Imports WholeFoods.Mobile.IRMA.Common
Imports System.ServiceModel

Public Class ReceiveDocumentScan
    Inherits HandheldHardware.ScanForm

    Public subteam As String
    Public store As String

    Private notify As Notification = Nothing
    Private LastError As ParsedCFFaultException = Nothing
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
    Private bError As Boolean = False
    Private myItemKey As String = String.Empty
    Private myItem As StoreItem
    Private myQty As Double = 0
    Private qUnit As Integer
    Private ItemIsLoaded As Boolean = False
    Private quantityUnits As New List(Of ListsItemUnit)
    Private discountAmount As Double = 0.0
    Private discountType As Integer = 0
    Private discountReasonCode As Integer = 0

#Region " Constructors"
    Public Sub New(ByVal session As Session)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.KeyPreview = True
        Me.mySession = session

        'init HH scanner
        If Not mySession.MyScanner Is Nothing Then
            mySession.MyScanner.restoreScannerSettings()
        End If
        mySession.MyScanner = New HandheldHardware.HandheldScanner(Me)
        If (mySession.MyScanner.HHType = HandheldHardware.Scanner.UNKNOWN) Then
            Messages.ScannerNotAvailable()
        End If

        'update scanned
        Dim fileWriter As ReceiveDocumentFileWriter = New ReceiveDocumentFileWriter(Me.mySession)
        scannedItems = fileWriter.GetScannedItemHash()

        AlignText()

    End Sub
#End Region

#Region " Form Events"

    Private Sub ReceiveDocumentScan_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'load all store and subteam selections
        Me.subTeamLabel.Text = mySession.Subteam
        Me.StoreTeamLabel2.Text = mySession.StoreName
        Me.invoiceNum.Text = mySession.DSDInvoice
        Me.vendor.Text = mySession.DSDVendorName
        Me.chkDiscount.Checked = False
        Me.btnDiscount.Enabled = False
        Me.txtUPC.Focus()

        If mySession.ActionType = Enums.ActionType.ReceiveDocumentCredit Then
            ComboBoxQuantityUnit.Enabled = True
        End If

        Try
            ' Attempt a service call to load the quantity unit ComboBox.
            quantityUnits = mySession.WebProxyClient.GetItemUnits().ToList()

            ' Explicitly handle service faults, timeouts, and connection failures.  
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "ReceiveDocumentScan_Load")
            Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "ReceiveDocumentScan_Load")
            Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "ReceiveDocumentScan_Load")
            Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Finally
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    Private Sub ReceiveDocumentScan_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Select Case (e.KeyCode)
            Case (Keys.Tab)
                If Me.txtUPC.Focused And Not String.IsNullOrEmpty(Me.txtUPC.Text) Then
                    Me.cmdSearch_Click(sender, e)
                End If
        End Select
    End Sub

    Private Sub ReceiveDocumetScan_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        Dim keyPressed As Integer = -1

        Select Case (e.KeyCode)
            Case (Keys.Enter)
                If Me.txtUPC.Focused And Not String.IsNullOrEmpty(Me.txtUPC.Text) Then
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

    Private Sub txtQty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtQty.KeyPress

        If myItem Is Nothing Then
            e.Handled = True
            Exit Sub
        End If

        If myItem.CostedByWeight Or myItem.SoldByWeight Then
            'allow numbers and 1 decimal to be entered
            If Char.IsDigit(e.KeyChar) _
                Or e.KeyChar = Chr(ASCII_PERIOD) _
                Or e.KeyChar = Chr(ASCII_BACKSPACE) Then
                'check for existing decimal point
                If e.KeyChar = Chr(ASCII_PERIOD) And txtQty.Text.Contains(Chr(ASCII_PERIOD)) Then
                    e.Handled = True
                End If
                'little erik doesn't want 6 chars for non decimals
                If e.KeyChar = Chr(ASCII_PERIOD) Or txtQty.Text.Contains(Chr(ASCII_PERIOD)) Then
                    txtQty.MaxLength = 6
                Else
                    txtQty.MaxLength = 5
                End If
                'check the positioning of the char being input
                If txtQty.Text.Contains(Chr(ASCII_PERIOD)) And e.KeyChar <> Chr(ASCII_BACKSPACE) Then
                    Dim iSelPos As Integer = txtQty.SelectionStart
                    Dim iDecPos As Integer = txtQty.Text.IndexOf(Chr(ASCII_PERIOD)) + 1
                    If Len(Mid(txtQty.Text, iDecPos, Len(txtQty.Text))) > 2 And iSelPos = Len(txtQty.Text) Then
                        e.Handled = True
                    End If
                End If
            Else
                e.Handled = True
            End If
        Else
            'allow only numbers
            If Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(ASCII_BACKSPACE)) Then
                e.Handled = True
            End If
        End If

    End Sub
#End Region

#Region " Control Events"

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Cursor.Current = Cursors.WaitCursor

        If String.IsNullOrEmpty(txtUPC.Text) Or lblDescription.Text = String.Empty Then
            MessageBox.Show("Please enter an item UPC", "UPC Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Cursor.Current = Cursors.Default
            Exit Sub
        End If

        If txtQty.Text = String.Empty Then
            MessageBox.Show("Please enter an item quantity.", "Quantity Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Cursor.Current = Cursors.Default
            Exit Sub
        End If

        If (chkDiscount.Checked = True) And discountType = 0 Then
            MessageBox.Show("Please enter all discount information.", "Discount Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Cursor.Current = Cursors.Default
            Exit Sub
        End If

        ValidateQuanity()

        mySession.DSDInvoice = invoiceNum.Text

        Try
            SaveItem(txtUPC.Text, myItemKey, txtQty.Text, lblPkgVal.Text, lblDescription.Text, True)
        Catch ex As NullReferenceException
            Messages.EmptyItem()
        End Try

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub txtUpc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUPC.TextChanged
        If txtUPC.Text.Length > 0 Then
            cmdSearch.Enabled = True
        Else
            cmdSearch.Enabled = False
        End If

        Me.lblDescription.Text = String.Empty
        Me.lblPkgVal.Text = String.Empty
        Me.txtQty.Text = String.Empty
        ComboBoxQuantityUnit.DataSource = Nothing
    End Sub

    Private Sub MenuItemClearSession_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemClearSession.Click
        Cursor.Current = Cursors.WaitCursor

        Try

            'delete session
            Dim resp As MsgBoxResult = Messages.DeleteSession()

            If (resp = MsgBoxResult.Yes) Then
                Dim fileWriter As ReceiveDocumentFileWriter = New ReceiveDocumentFileWriter(Me.mySession)

                fileWriter.DeleteFile(fileWriter.MakeFilePath(Me.mySession.SessionName))
                If (Not Me.mySession.SessionName = Nothing) Then
                    Me.mySession.SessionName = Nothing
                End If

                Me.Close()
            End If

        Catch ex As NullReferenceException
            MessageBox.Show("No session file is loaded.  Nothing has been deleted.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show("An unknown error has occured while clearing the session.  Please launch the Receiving Document module and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Me.Close()
        Finally
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Search()
    End Sub

    Private Sub mnuReview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReview.Click
        Cursor.Current = Cursors.WaitCursor

        Dim cont As Boolean = True

        Try
            If String.IsNullOrEmpty(txtQty.Text) = False Then
                SaveItem(txtUPC.Text, myItemKey, txtQty.Text, lblPkgVal.Text, lblDescription.Text, True)
            End If

        Catch ex As NullReferenceException

            Dim resp As MsgBoxResult = Messages.NullItem()

            If (resp = MsgBoxResult.Cancel) Then
                cont = False
            End If

        End Try

        Dim receiveDocument As ReceiveDocumentReview = New ReceiveDocumentReview(Me.mySession)

        If (cont) Then
            If receiveDocument.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Me.Close()
            End If
            receiveDocument.Dispose()
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub MenuItemViewSavedSession_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemViewSavedSession.Click
        mySession.IsLoadedSession = False
        Me.DialogResult = Windows.Forms.DialogResult.None
    End Sub

    Private Sub txtUpc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUPC.GotFocus
        Me.txtUPC.SelectAll()
    End Sub

    Private Sub txtQty_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.GotFocus
        Me.txtQty.SelectAll()
    End Sub

    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        ClearScreen()
    End Sub

    Private Sub MenuItemBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemBack.Click
        mySession.IsLoadedSession = False
        Me.Close()
    End Sub

    Private Sub btnDiscount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDiscount.Click
        If String.IsNullOrEmpty(txtUPC.Text) Or lblDescription.Text = "" Or txtQty.Text = "" Then
            MessageBox.Show("Please enter an item quantity.", "Quantity Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        If chkDiscount.Checked = True Then
            Dim frmDiscount As ItemDiscount = New ItemDiscount(Me.mySession)
            frmDiscount.UPC = txtUPC.Text
            frmDiscount.Desc = lblDescription.Text
            frmDiscount.CurrentDiscountAmount = discountAmount
            frmDiscount.CurrentDiscountType = discountType
            frmDiscount.CurrentDiscountReasonCode = discountReasonCode

            Dim res As DialogResult = frmDiscount.ShowDialog()

            If res = Windows.Forms.DialogResult.None Then
                discountType = 0
            ElseIf res = Windows.Forms.DialogResult.OK Then
                discountType = frmDiscount.DiscountType
                discountReasonCode = frmDiscount.DiscountReasonCode
                discountAmount = frmDiscount.DiscountAmount
            End If

            frmDiscount.Dispose()
        End If
    End Sub

    Private Sub chkDiscount_CheckStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDiscount.CheckStateChanged
        If chkDiscount.Checked = False Then
            discountAmount = 0
            discountType = 0
            discountReasonCode = 0
            btnDiscount.Enabled = False
        Else
            btnDiscount.Enabled = True
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

            Search()

            If Me.txtUPC.Text.Equals(lastUpcScanned) Then
                If Me.txtQty.Focused And Not String.IsNullOrEmpty(Me.txtQty.Text) And IsNumeric(Me.txtQty.Text) Then
                    If CInt(Me.txtQty.Text) > 0 Then
                        Me.txtQty.Text = CInt(Me.txtQty.Text) + 1
                    End If
                End If
            ElseIf String.IsNullOrEmpty(lastUpcScanned) Then
                Me.txtQty.Text = "1"
            End If

            lastUpcScanned = Me.txtUPC.Text
            lastItemKeyScanned = myItemKey
            lastQtyScanned = Me.txtQty.Text
            lastUOMScanned = Me.lblPkgVal.Text
            lastDescScanned = Me.lblDescription.Text

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

            Me.txtUPC.Text = upc

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
    Private Sub SaveItem(ByVal upc As String, ByVal itemKey As String, ByVal qty As Double, ByVal uom As String, ByVal desc As String, ByVal replaceQty As Boolean)
        Dim discount As Double = discountAmount

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
                    If myItem.CostedByWeight Or myItem.SoldByWeight Then
                        Dim myqty As Double = Double.Parse(qty)
                    Else
                        Dim myqty As Integer = Integer.Parse(qty)
                    End If
                Catch ex As Exception
                    Messages.QtyNumberException()
                    Exit Sub
                End Try

                Dim sessName As String
                Dim fileWriter As ReceiveDocumentFileWriter = New ReceiveDocumentFileWriter(Me.mySession)
                Dim bScan As Boolean = True

                If mySession.IsLoadedSession = False Then
                    sessName = fileWriter.GenerateSessionName()
                    mySession.SessionName = sessName
                End If

                If Not fileWriter.SessionFileExists() Then
                    If discountType = 3 Then discount = qty
                    fileWriter.SaveItem(mySession.GetSessionName(), upc, itemKey, qty, uom, desc, myItem.CostedByWeight, True, qUnit, discount, discountType, discountReasonCode) '//file does not exist yet
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
                            If discountType = 3 Then discount = qty
                            fileWriter.SaveItem(mySession.SessionName, upc, itemKey, qty, uom, desc, myItem.CostedByWeight, False, qUnit, discount, discountType, discountReasonCode)
                        Else
                            bScan = False
                        End If

                    Else
                        If discountType = 3 Then discount = qty
                        fileWriter.SaveItem(mySession.SessionName, upc, itemKey, qty, uom, desc, myItem.CostedByWeight, False, qUnit, discount, discountType, discountReasonCode)
                    End If
                End If

                If bScan Then
                    scannedItems.Add(upc, qty)
                    Me.ClearScreen()
                Else
                    MsgBox(String.Format("Receiving Document for UPC {0} was not saved.", upc), MsgBoxStyle.Information, Me.Text)
                End If
            End If

        Catch ex As Exception
            MsgBox("An unexpected error occurred: " + ex.Message)
        End Try

    End Sub

    Private Sub Search()
        Cursor.Current = Cursors.WaitCursor

        Dim tmpStr As String
        Dim abort As Boolean = False
        ItemIsLoaded = False

        If Not String.IsNullOrEmpty(txtUPC.Text) Then

            Dim currentScannedUpc = GetUpc(Me.txtUPC.Text)

            'check for scale item
            Me.txtUPC.Text = ScaleItemCheck(currentScannedUpc)

            Try
                ' Attempt a service call to get item information and quantity units.
                serviceCallSuccess = True

                myItem = mySession.WebProxyClient.GetStoreItem(mySession.StoreNo, mySession.SubteamKey, mySession.UserID, Nothing, Me.txtUPC.Text)

                ' Explicitly handle service faults, timeouts, and connection failures.  
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
                ' The call to GetStoreItemFailed.  End the method and return control to the user.
                Cursor.Current = Cursors.Default
                Exit Sub
            End If

            If Exceptions.ValidIdentifier(myItem) Then
                If Exceptions.CanInventory(myItem) = False Then
                    'Messages.InvalidOrderItemCanInventory(mySession.SubteamKey, Me.txtUPC.Text)
                    Messages.InvalidOrderItemCanInventory(mySession.Subteam, txtUPC.Text, myItem.ItemDescription)
                    ClearScreen()
                ElseIf myItem.IsItemAuthorized = False Then
                    MsgBox("This item is not authorized for this store.", MsgBoxStyle.Information, "Item Not Authorized")
                    ClearScreen()
                ElseIf myItem.DiscontinueItem = True And mySession.ActionType = Enums.ActionType.ReceiveDocument Then
                    MsgBox("This item is discontinued.", MsgBoxStyle.Information, "Discontinued Item")
                    ClearScreen()
                ElseIf myItem.VendorID.Equals(mySession.DSDVendorID) = False Then
                    Messages.InvalidItemForVendor(mySession.DSDVendorName, Me.txtUPC.Text)
                    ClearScreen()
                Else
                    Me.lblDescription.Text = myItem.ItemDescription
                    mySession.StoreVendorID = myItem.StoreVendorID
                    ComboBoxQuantityUnit.DisplayMember = "Unit_Name"
                    ComboBoxQuantityUnit.ValueMember = "Unit_Id"
                    ComboBoxQuantityUnit.DataSource = quantityUnits
                    ComboBoxQuantityUnit.SelectedValue = quantityUnits.Where(Function(unit) unit.Unit_Abbreviation = myItem.VendorUnitName).FirstOrDefault().Unit_Id
                    ItemIsLoaded = True

                    If myItem.CostedByWeight Or myItem.SoldByWeight Then
                        lblQty.Text = "Weight:"
                    End If

                    myItemKey = myItem.ItemKey

                    If (scannedItems.Contains(txtUPC.Text)) Then
                        myQty = scannedItems.Item(txtUPC.Text)
                    Else
                        myQty = 0
                    End If

                    Me.lblPkgVal.Text = myItem.PackageDesc1 & "/" & myItem.PackageDesc2.ToString("0.000") & " " & myItem.PackageUnitAbbr

                    tmpStr = myItem.RetailSubteamNo

                    If myItem.CostedByWeight Or myItem.SoldByWeight Then
                        txtQty.MaxLength = 6
                    Else
                        txtQty.MaxLength = 3
                    End If

                    Me.txtQty.Focus()
                    Me.txtQty.SelectAll()
                End If
            Else
                myItem = Nothing
                Messages.ItemNotFound()
                ClearScreen()
            End If

        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ValidateQuanity()
        Dim valid As Boolean = False

        Try
            If myItem.CostedByWeight Then  ' trap negative qty  :Richard Ainsley, TFS 4515, 1/24/2012
                Dim weight As Double = CDbl(txtQty.Text)
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

                    Dim response = MsgBox("Your weight is " + CStr(weight) + ".  Please confirm.", mystyle, "Verify Weight")
                    If response = MsgBoxResult.No Then
                        Cursor.Current = Cursors.Default
                        Exit Sub
                    End If
                End If

            ElseIf txtQty.Text <> String.Empty Then

                Dim quantity As Double = CDbl(txtQty.Text)
                If quantity < 0 Then
                    MsgBox(Messages.NEG_AMT, MsgBoxStyle.Information, "Negative Amount Error")
                    Cursor.Current = Cursors.Default
                    valid = True
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

                    Dim response = MsgBox("Your quantity is " + CStr(quantity) + ", please confirm.", mystyle, "Verify Quantity")
                    If response = MsgBoxResult.No Then
                        Cursor.Current = Cursors.Default
                        Exit Sub
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox("Invalid quantity amount.  Please try again. " & ex.Message, MsgBoxStyle.Information, "Invalid Quantity")
            Exit Sub
        End Try
    End Sub

    Private Sub ClearScreen()
        Cursor.Current = Cursors.WaitCursor

        Me.txtQty.Text = "1"
        Me.txtUPC.Text = String.Empty
        Me.lblPkgVal.Text = String.Empty
        Me.lblDescription.Text = String.Empty
        ComboBoxQuantityUnit.DataSource = Nothing
        Me.chkDiscount.Checked = False
        Me.discountType = 0
        Me.discountReasonCode = 0
        Me.discountAmount = 0.0
        Me.txtUPC.Focus()

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub AlignText()
        invoice1.TextAlign = ContentAlignment.TopRight
        Label1.TextAlign = ContentAlignment.TopRight
        Label2.TextAlign = ContentAlignment.TopRight
        Label3.TextAlign = ContentAlignment.TopRight
        lblPkg.TextAlign = ContentAlignment.TopRight
        lblQty.TextAlign = ContentAlignment.TopRight
    End Sub
#End Region

    Private Sub ComboBoxQuantityUnit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxQuantityUnit.SelectedIndexChanged
        qUnit = ComboBoxQuantityUnit.SelectedValue
    End Sub

End Class