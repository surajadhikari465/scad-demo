Imports System.Windows.Forms
Imports System
Imports System.Linq
Imports System.Data
Imports WholeFoods.Mobile.IRMA.Common
Imports System.ServiceModel

Public Class ShrinkScan
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
        Dim fileWriter As ShrinkFileWriter = New ShrinkFileWriter(Me.mySession)
        scannedItems = fileWriter.getScannedItemHash()

    End Sub

#End Region

#Region " Form Events"

    Private Sub ScanShrink_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.IrmaShrinkLabel.Text = "IRMA Shrink: " + mySession.ShrinkSubType
        Me.StoreTeamLabel.Text = mySession.StoreName + " " + mySession.Subteam

        Me.frmStatus.Visible = False

        'set focus on UPC
        Me.txtUpc.Focus()

    End Sub

    Private Sub ShrinkScan_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        Select Case (e.KeyCode)
            Case (Keys.Tab)
                If Me.txtUpc.Focused And Not String.IsNullOrEmpty(Me.txtUpc.Text) Then
                    Me.cmdSearch_Click(sender, e)
                End If
        End Select

    End Sub

    Private Sub ShrinkScan_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp

        Dim keyPressed As Integer = -1

        Select Case (e.KeyCode)
            Case (Keys.Enter)
                If Me.txtUpc.Focused And Not String.IsNullOrEmpty(Me.txtUpc.Text) Then
                    Me.cmdSearch_Click(sender, e)
                ElseIf Me.txtQty.Focused And Not String.IsNullOrEmpty(Me.txtQty.Text) And IsNumeric(Me.txtQty.Text) Then

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

    Private Sub mnuMenu_ClearSession_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMenu_ClearSession.Click

        Cursor.Current = Cursors.WaitCursor

        Try
            'delete session
            Dim resp As MsgBoxResult = Messages.DeleteSession()

            If (resp = MsgBoxResult.Yes) Then

                If (Me.mySession.SessionName = Nothing) Then
                    MessageBox.Show("No session exists.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
                Else
                    Dim fileWriter As ShrinkFileWriter = New ShrinkFileWriter(Me.mySession)
                    fileWriter.DeleteFile(fileWriter.MakeFilePath(Me.mySession.SessionName))
                    If (Not Me.mySession.SessionName = Nothing) Then
                        Me.mySession.SessionName = Nothing
                    End If
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                End If

            End If
        Catch ex As Exception

            MsgBox("An error occurred while clearing the session: " + ex.Message, MsgBoxStyle.Exclamation, "Error")
            Me.Close()

        End Try

        Cursor.Current = Cursors.Default

    End Sub

    Private Sub mnuReview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReview.Click

        If CDbl(txtQty.Text) > 999 Then
            MessageBox.Show(Messages.QUANTITY_AMT_ERROR, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        If CDbl(txtQty.Text) = 0 Then
            MessageBox.Show(Messages.QUANTITY_ZERO_ERROR, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        ' Prompt for confirmation if the chosen quantity is 99 or greater.
        If CDbl(txtQty.Text) >= 99 Then
            If MsgBox("You have entered a shrink quantity of " + txtQty.Text + ".  Is this correct?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, Me.Text) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        Cursor.Current = Cursors.WaitCursor
        Dim cont As Boolean = True

        Try
            SaveItem(txtUpc.Text, myItemKey, txtQty.Text, lblUOM.Text, lblDescription.Text, True, mySession.ShrinkSubTypeID, _
                     mySession.ShrinkSubType, mySession.ShrinkAdjId, mySession.ShrinkTypeId, mySession.ShrinkType)

        Catch ex As NullReferenceException
            Dim resp As MsgBoxResult = Messages.NullItem()

            If (resp = MsgBoxResult.Cancel) Then
                cont = False
            End If
        End Try

        Dim rshrink As ShrinkReview = New ShrinkReview(Me.mySession)

        If (cont) Then
            If rshrink.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Me.Close()
            End If
            rshrink.Dispose()
        End If

        Cursor.Current = Cursors.Default

    End Sub

    Private Sub txtUpc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUpc.GotFocus
        Me.txtUpc.SelectAll()
    End Sub

    Private Sub txtQty_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.GotFocus
        Me.txtQty.SelectAll()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        ' Check that txtQty is numeric.
        If Not IsNumeric(txtQty.Text) Then
            MessageBox.Show("Please enter a numeric value for the quantity.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        If CDbl(txtQty.Text) > 999 Then
            MessageBox.Show(Messages.QUANTITY_AMT_ERROR, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        If CDbl(txtQty.Text) = 0 Then
            MessageBox.Show(Messages.QUANTITY_ZERO_ERROR, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        ' Prompt for confirmation if the chosen quantity is 99 or greater.
        If CDbl(txtQty.Text) >= 99 Then
            If MsgBox("You have entered a shrink quantity of " + txtQty.Text + ".  Is this correct?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, Me.Text) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        If (String.IsNullOrEmpty(txtUpc.Text)) Then
            MessageBox.Show("Please scan item.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        Try
            SaveItem(txtUpc.Text, myItemKey, txtQty.Text, lblUOM.Text, lblDescription.Text, True, mySession.ShrinkSubTypeID, mySession.ShrinkSubType, _
                     mySession.ShrinkAdjId, mySession.ShrinkTypeId, mySession.ShrinkType)

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
            lastUOMScanned = Me.lblUOM.Text
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

    Private Sub SaveItem(ByVal upc As String, ByVal itemKey As String, ByVal qty As Double, ByVal uom As String, ByVal desc As String, ByVal replaceQty As Boolean, _
                         ByVal shrinkSubTypeID As Integer, ByVal shrinkSubType As String, _
                         ByVal shrinkAdjId As Integer, ByVal shrinkTypeId As String, ByVal shrinkType As String)

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
                Dim fileWriter As ShrinkFileWriter = New ShrinkFileWriter(Me.mySession)
                Dim bScan As Boolean = True

                'TFS 5500, 7383 - Faisal Ahmed
                If mySession.IsLoadedSession = False Then
                    sessName = fileWriter.GenerateSessionName()
                    mySession.SessionName = sessName

                End If

                If Not fileWriter.SessionFileExists() Then
                    fileWriter.SaveItem(mySession.GetSessionName(), upc, itemKey, qty, uom, desc, myItem.CostedByWeight, True, shrinkSubTypeID, shrinkSubType, shrinkAdjId, shrinkTypeId, shrinkType) '//file does not exist yet
                Else
                    If fileWriter.isPreviouslyScanned(upc, shrinkSubTypeID.ToString) Then
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
                        uMessage.BodyText = String.Format("{0} of this item already queued in this session. Tap Add, Overwrite, or Cancel.", sMsgBoxQty)
                        uMessage.ShowDialog()

                        Dim dlgResult As String = uMessage.Result
                        uMessage = Nothing

                        If dlgResult = "A" Then
                            qty = qty + Val(sMsgBoxQty)
                        End If

                        If dlgResult <> "C" Then
                            fileWriter.SaveItem(mySession.SessionName, upc, itemKey, qty, uom, desc, myItem.CostedByWeight, False, shrinkSubTypeID, shrinkSubType, shrinkAdjId, shrinkTypeId, shrinkType)
                        Else
                            bScan = False
                        End If

                    Else
                        fileWriter.SaveItem(mySession.SessionName, upc, itemKey, qty, uom, desc, myItem.CostedByWeight, False, shrinkSubTypeID, shrinkSubType, shrinkAdjId, shrinkTypeId, shrinkType)
                    End If
                End If

                If bScan Then
                    scannedItems.Add(upc, qty)
                    If Not chkSkipConfirm.Checked Then
                        MsgBox(String.Format("Shrink for UPC {0} saved successfully with qty of {1}", upc, qty), MsgBoxStyle.OkOnly, "Shrink Saved")
                    End If
                    Me.ClearScreen()
                Else
                    MsgBox(String.Format("Shrink for UPC {0} was not saved.", upc), MsgBoxStyle.OkOnly, "Shrink Save Cancelled")
                End If
            End If

        Catch exp As Exception
        End Try

    End Sub

    Private Sub ClearScreen()
        Cursor.Current = Cursors.WaitCursor

        Me.txtQty.Text = 1
        Me.txtUpc.Text = ""
        Me.lblUOM.Text = ""
        Me.lblDescription.Text = ""
        Me.lblRetailUnit.Text = ""
        Me.txtUpc.Focus()
        myItem = Nothing
        myQty = 0
        Me.lblIQueuedQty.Text = "Queued:"

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub Search()
        Cursor.Current = Cursors.WaitCursor

        Dim tmpStr As String
        Dim abort As Boolean = False

        If Not String.IsNullOrEmpty(txtUpc.Text) Then

            Try
                ' Attempt a service call to search for an item.
                serviceCallSuccess = True
                Dim currentScannedUpc = GetUpc(Me.txtUpc.Text)

                'check for scale item
                Me.txtUpc.Text = ScaleItemCheck(currentScannedUpc)
                myItem = Me.mySession.WebProxyClient.GetStoreItem(mySession.StoreNo, mySession.SubteamKey, mySession.UserID, Nothing, Me.txtUpc.Text)


                ' Explicitly handle service faults, timeouts, and connection failures.  If this search routine fails, allow the user to retry.
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
                ' The call to GetStoreItem failed.  Return control to the user and allow the user to retry.
                Cursor.Current = Cursors.Default
                txtUpc.Focus()
                Exit Sub
            End If

            If Exceptions.ValidIdentifier(myItem) Then

                Me.lblDescription.Text = myItem.ItemDescription

                myItemKey = myItem.ItemKey

                If (scannedItems.Contains(txtUpc.Text)) Then
                    myQty = scannedItems.Item(txtUpc.Text)
                Else
                    myQty = 0
                End If

                lblIQueuedQty.Text = "Queued: " & Str(myQty)

                Me.lblUOM.Text = myItem.PackageDesc1 & "/" & myItem.PackageDesc2.ToString("0.000") & " " & myItem.PackageUnitAbbr
                Me.lblRetailUnit.Text = "Retail: " & myItem.retailUnitName

                tmpStr = myItem.RetailSubteamNo

                If myItem.CostedByWeight Or myItem.SoldByWeight Then
                    txtQty.MaxLength = 6
                Else
                    txtQty.MaxLength = 3
                End If

                'check subteam
                If Not tmpStr.Equals(mySession.SubteamKey) Then

                    'verify session subteam is not restricted to same-subteam items
                    If mySession.IsSubTeamUnrestricted(mySession.SubteamKey) Then

                        If Not allowSubteamKeys.ContainsKey(tmpStr) Then

                            If Not mySession.SupressInvalidSubteamWarning = 1 Then

                                Dim response As MsgBoxResult = Messages.invalidItemSubteamException(myItem.RetailSubteamName, mySession.Subteam)

                                If (response = MsgBoxResult.No) Then
                                    abort = True
                                Else
                                    allowSubteamKeys.Add(tmpStr, Nothing)
                                End If

                                response = Messages.supressSubteamExceptionWarning

                                If response = MsgBoxResult.Yes Then

                                    mySession.SupressInvalidSubteamWarning = 1

                                End If

                            Else
                                allowSubteamKeys.Add(tmpStr, Nothing)

                            End If
                        End If

                    Else
                        Messages.subteamRestricted(mySession.Subteam, myItem.RetailSubteamName)
                        abort = True

                    End If
                End If

                If abort Then
                    ClearScreen()
                Else
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

#End Region

    Private Sub mnuMenu_ExitShrink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMenu_ExitShrink.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub mnuMenu_ExitIRMA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMenu_ExitIRMA.Click
        Me.DialogResult = Windows.Forms.DialogResult.Abort
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
                'little erik doesn't want 6 chars for non decimals
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

    Private Sub AlignText()
        Label1.TextAlign = ContentAlignment.TopRight
        Label2.TextAlign = ContentAlignment.TopRight
        Label3.TextAlign = ContentAlignment.TopRight
        Label4.TextAlign = ContentAlignment.TopRight
    End Sub

    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMenu_ChangesubType.Click
        Me.Close()
    End Sub

End Class