Imports System.Windows.Forms
Imports System
Imports System.Linq
Imports System.Data
Imports System.ServiceModel
Imports WholeFoods.Mobile.IRMA.Common


Public Class CycleCountTeam
    Inherits HandheldHardware.ScanForm

    Private mySession As Session
    Private serviceFault As ParsedCFFaultException = Nothing
    Private serviceCallSuccess As Boolean
    Private myItem As StoreItem
    Private myCycleCountInfo As CycleCountInfo
    Private myCycleCount As CycleCount
    Private myCycleCountHeader As InternalCycleCountHeader
    Private lastUpcScanned As String = String.Empty
    Private lastItemKeyScanned As String = String.Empty
    Private lastQtyScanned As String = String.Empty
    Private lastUOMScanned As String = String.Empty
    Private lastDescScanned As String = String.Empty


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

    End Sub

#End Region

#Region " Form Events"

    Private Sub CycleCount_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'labels
        Me.Text = mySession.ActionType.ToString
        mnuMenu_ExitCycleCount.Text = "Exit " & mySession.ActionType.ToString
        Me.lblStoreVal.Text = mySession.StoreName
        Me.lblSubTeamVal.Text = mySession.Subteam

        'visibility
        Me.frmStatus.Visible = False
        SetVisibility(True)

        Select Case mySession.ActionType
            Case Enums.ActionType.CycleCountLocation

                'load locations
                Dim ds As New List(Of InventoryLocation)

                Try
                    serviceCallSuccess = True
                    ds = mySession.WebProxyClient.GetInventorylocations(mySession.StoreNo, mySession.SubteamKey).ToList

                    ' Explicitly handle service faults, timeouts, and connection failures.  
                Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                    serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                    Dim err As New ErrorHandler(serviceFault)
                    err.ShowErrorNotification()
                    serviceCallSuccess = False

                Catch ex As TimeoutException
                    Dim err As New ErrorHandler()
                    err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "CycleCount_Load")
                    serviceCallSuccess = False

                Catch ex As CommunicationException
                    Dim err As New ErrorHandler()
                    err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "CycleCount_Load")
                    serviceCallSuccess = False

                Catch ex As Exception
                    Dim err As New ErrorHandler()
                    err.DisplayErrorMessage(ex.Message, "CycleCount_Load")
                    serviceCallSuccess = False
                End Try

                If Not serviceCallSuccess Then
                    ' The call to GetInventoryLocations failed.  Exit the form.
                    Me.Close()
                    Exit Sub
                End If

                ds.Insert(0, New WholeFoods.Mobile.IRMA.InventoryLocation)
                ds(0).InvLocName = "-- Select Location --"
                ds(0).InvLocID = 0

                cmbLocation.DisplayMember = "InvLocName"
                cmbLocation.ValueMember = "InvLocID"
                cmbLocation.DataSource = ds

                'set focus on location
                Me.cmbLocation.Focus()

            Case Else
                'set focus on UPC
                Me.txtUpc.Focus()
        End Select

    End Sub

    Private Sub CycleCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Select Case (e.KeyCode)
            Case (Keys.Tab)
                If Me.txtUpc.Focused And Not String.IsNullOrEmpty(Me.txtUpc.Text) Then
                    Me.cmdSearch_Click(sender, e)
                End If
        End Select
    End Sub

    Private Sub CycleCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        Dim keyPressed As Integer = -1

        Select Case (e.KeyCode)

            Case (Keys.Enter)
                If Me.txtUpc.Focused And Not String.IsNullOrEmpty(Me.txtUpc.Text) Then
                    Me.cmdSearch_Click(sender, e)
                ElseIf Me.txtQty.Focused And Not String.IsNullOrEmpty(Me.txtQty.Text) And IsNumeric(Me.txtQty.Text) Then

                    ' restet this so if the same item is scanned, it doesn't add to the previous saved qty
                    lastQtyScanned = 0

                    Me.mnuCount_Click(sender, e)
                    Me.ClearScreen()
                ElseIf Me.txtQty.Focused And (Not String.IsNullOrEmpty(Me.txtQty.Text) Or Not IsNumeric(Me.txtQty.Text)) Then
                    Messages.QtyNumberException()
                    Me.txtQty.Focus()
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
    Private Sub mnuMenu_ClearScreen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMenu_Clear.Click
        ClearScreen()
    End Sub

    Private Sub mnuMenu_ExitCycleCount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMenu_ExitCycleCount.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub mnuCount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCount.Click
        Try
            If Validate() Then
                AddCount(Enums.ActionType.Order)
                ClearScreen()
                txtUpc.Focus()
            Else
                Exit Sub
            End If
        Catch ex As NullReferenceException
            Messages.EmptyItem()
        End Try
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Search()
    End Sub

    Private Sub cmbLocation_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLocation.SelectedValueChanged
        ClearScreen()

        If cmbLocation.SelectedValue > 0 Then
            txtUpc.BackColor = Color.White
            txtUpc.Enabled = True
            cmdSearch.Enabled = True
            txtUpc.Focus()
        Else
            txtUpc.BackColor = Color.LightGray
            txtUpc.Enabled = False
            cmdSearch.Enabled = True
        End If
    End Sub

    Private Sub txtQty_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.GotFocus
        Me.txtQty.SelectAll()
    End Sub

    Private Sub txtUpc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUpc.GotFocus
        Me.txtUpc.SelectAll()
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

            Dim itemFound As Boolean = Search()

            Me.txtQty.Focus()

            If itemFound Then
                If Me.txtUpc.Text.Equals(lastUpcScanned) Then
                    If Me.txtQty.Focused And Not String.IsNullOrEmpty(Me.txtQty.Text) And IsNumeric(Me.txtQty.Text) Then
                        If CInt(Me.txtQty.Text) > 0 Then
                            Me.txtQty.Text = CInt(Me.txtQty.Text) + 1
                        End If
                    Else
                        Me.txtQty.Text = "1"
                    End If
                Else
                    Me.txtQty.Text = "1"
                End If

                lastUpcScanned = Me.txtUpc.Text
                lastQtyScanned = Me.txtQty.Text
                lastUOMScanned = Me.lblPkgVal.Text
                lastDescScanned = Me.lblDescriptionVal.Text

                Me.txtQty.SelectAll()
            Else
                ClearScreen()
                txtUpc.Focus()
            End If

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

    Private Function Validate() As Boolean
        Try
            If Not String.IsNullOrEmpty(Me.txtQty.Text) And IsNumeric(Me.txtQty.Text) And CInt(Me.txtQty.Text) > 0 Then
                Return True
            End If
        Catch ex As Exception
            txtQty.Text = ""
            Messages.QtyNumberException()
            Return False
            Exit Function
        End Try

        Return Exceptions.MaximumQty(txtQty.Text)
    End Function

    Private Sub ClearScreen()
        Cursor.Current = Cursors.WaitCursor

        Me.txtUpc.Text = ""
        Me.lblDescriptionVal.Text = ""
        Me.lblPkgVal.Text = ""
        Me.lblItemSubTeamVal.Text = ""
        Me.lblPrimaryVendorVal.Text = ""
        Me.txtQty.Text = ""
        Me.lblTotalVal.Text = ""

        Me.txtUpc.Focus()

        Cursor.Current = Cursors.Default
    End Sub

    Private Function Search() As Boolean
        Cursor.Current = Cursors.WaitCursor

        'declare variables
        Dim found As Boolean = True
        Dim charArray As Char() = {"0"c}
        Dim sIdentifier As String = ""

        If Not String.IsNullOrEmpty(txtUpc.Text) Then

            Dim currentScannedUpc = GetUpc(Me.txtUpc.Text)

            'trim only leading zeros
            Me.txtUpc.Text = currentScannedUpc.TrimStart(charArray)

            'check if scale item and if so, format it to have 00000 at the end
            If txtUpc.Text.Length = 11 And txtUpc.Text.StartsWith("2") Then
                sIdentifier = txtUpc.Text.Remove(6, 5)
                sIdentifier = sIdentifier.PadRight(11, "0")
            Else
                sIdentifier = txtUpc.Text
            End If

            Try
                serviceCallSuccess = True

                'execute service method
                myItem = Me.mySession.WebProxyClient.GetStoreItem(mySession.StoreNo, mySession.SubteamKey, mySession.UserID, Nothing, sIdentifier)

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
                ' The call to GetStoreItem failed.  Exit the method.
                Return False
            End If

            'consume service method
            Try
                If Not IsNothing(myItem) And Not IsNothing(myItem.Identifier) Then

                    Try
                        'populate form
                        PopulateInformation()

                        ' Explicitly handle service faults, timeouts, and connection failures.  
                    Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                        serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                        Dim err As New ErrorHandler(serviceFault)
                        err.ShowErrorNotification()
                        found = False

                    Catch ex As TimeoutException
                        Dim err As New ErrorHandler()
                        err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "PopulateInformation")
                        found = False

                    Catch ex As CommunicationException
                        Dim err As New ErrorHandler()
                        err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "PopulateInformation")
                        found = False

                    Catch ex As Exception
                        Dim err As New ErrorHandler()
                        err.DisplayErrorMessage(ex.Message, "PopulateInformation")
                        found = False
                    End Try


                    'cycle master
                    Try
                        GetCycleCount()
                        If myCycleCount Is Nothing Then
                            MessageBox.Show("No open master cycle count exists.  Please use the IRMA client to create the master cycle count.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                            found = False
                            Exit Function
                        End If

                        ' Explicitly handle service faults, timeouts, and connection failures.  
                    Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                        serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                        Dim err As New ErrorHandler(serviceFault)
                        err.ShowErrorNotification()
                        found = False

                    Catch ex As TimeoutException
                        Dim err As New ErrorHandler()
                        err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "GetCycleCount")
                        found = False

                    Catch ex As CommunicationException
                        Dim err As New ErrorHandler()
                        err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "GetCycleCount")
                        found = False

                    Catch ex As Exception
                        Dim err As New ErrorHandler()
                        err.DisplayErrorMessage(ex.Message, "GetCycleCount")
                        found = False
                    End Try

                    'cycle header
                    Try
                        GetHeader()
                    Catch ex As Exception
                        Messages.ShowException(ex)
                        found = False
                    End Try

                    'create header
                    Try
                        If IsNothing(myCycleCountHeader) Then
                            AddHeader()
                        End If

                        ' Explicitly handle service faults, timeouts, and connection failures.  
                    Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                        serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                        Dim err As New ErrorHandler(serviceFault)
                        err.ShowErrorNotification()
                        found = False

                    Catch ex As TimeoutException
                        Dim err As New ErrorHandler()
                        err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "AddHeader")
                        found = False

                    Catch ex As CommunicationException
                        Dim err As New ErrorHandler()
                        err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "AddHeader")
                        found = False

                    Catch ex As Exception
                        Dim err As New ErrorHandler()
                        err.DisplayErrorMessage(ex.Message, "AddHeader")
                        found = False
                    End Try

                    'visibility
                    SetVisibility(False)

                    'qty label logic
                    If myItem.SoldByWeight Then
                        Me.lblTotal.Text = "Total Wgt:"
                        Me.lblQty.Text = "Wgt:"
                    Else
                        Me.lblTotal.Text = "Total Qty:"
                        Me.lblQty.Text = "Qty:"
                    End If

                    txtQty.Focus()
                Else
                    'item cannot be added
                    Messages.ItemNotFound()
                    ClearScreen()
                    found = False
                    txtUpc.Focus()
                End If
            Catch ex As Exception
                Messages.ShowException(ex)
                found = False
            End Try
        Else
            Me.txtUpc.Focus()
            Me.txtUpc.SelectAll()
            ClearScreen()
        End If

        Cursor.Current = Cursors.Default

        Return found
    End Function

    Private Sub AddCount(ByVal action As Enums.ActionType)
        Cursor.Current = Cursors.WaitCursor

        Try
            If Not IsNothing(myCycleCount) And myCycleCount.ID > 0 Then
                Select Case myItem.SoldByWeight
                    Case True
                        Me.mySession.WebProxyClient.AddCycleCountItem(lblItemKeyVal.Text, 0, txtQty.Text, myItem.PackageDesc1, IIf(myItem.VendorUnitName = "Case", True, False), myCycleCountHeader.ID, cmbLocation.SelectedValue)
                    Case False
                        Me.mySession.WebProxyClient.AddCycleCountItem(lblItemKeyVal.Text, txtQty.Text, 0, myItem.PackageDesc1, IIf(myItem.VendorUnitName = "Case", True, False), myCycleCountHeader.ID, cmbLocation.SelectedValue)
                End Select
            End If

            ' Explicitly handle service faults, timeouts, and connection failures.  
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "AddCount")

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "AddCount")

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "AddCount")

        Finally
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Sub GetHeader()
        If Not IsNothing(myCycleCount.InternalCycleHeaders) Then
            Dim cchQuery = From cch In myCycleCount.InternalCycleHeaders Where cch.InventoryLocationID = cmbLocation.SelectedValue

            If cchQuery.Count > 0 Then
                myCycleCountHeader = cchQuery.First
            End If
        End If
    End Sub

    Private Sub AddHeader()
        myCycleCountHeader = Me.mySession.WebProxyClient.CreateCycleCountHeader(myCycleCount.ID, Date.Now, cmbLocation.SelectedValue, False)
    End Sub

    Private Sub GetCycleCount()
        myCycleCount = Me.mySession.WebProxyClient.GetCycleCount(mySession.StoreNo, mySession.SubteamKey)
    End Sub

    Private Sub PopulateInformation()

        'cycle count info
        myCycleCountInfo = Me.mySession.WebProxyClient.GetStoreItemCycleCountInfo(myItem, cmbLocation.SelectedValue)

        'store item info
        Me.lblItemKeyVal.Text = myItem.ItemKey
        Me.lblDescriptionVal.Text = myItem.ItemDescription
        Me.lblPkgVal.Text = myItem.PackageDesc1 & " / " & myItem.PackageDesc2 & " " & myItem.PackageUnitAbbr
        Me.lblPrimaryVendorVal.Text = myItem.VendorName.ToString

        If IsNothing(myItem.TransferToSubteamName) Then
            Me.lblItemSubTeamVal.Text = mySession.Subteam
        Else
            Me.lblItemSubTeamVal.Text = myItem.TransferToSubteamName
        End If

        Me.lblTotalVal.Text = IIf(myCycleCountInfo.Quantity > 0, myCycleCountInfo.Quantity, myCycleCountInfo.Weight)

    End Sub

    Private Sub SetVisibility(Optional ByVal initialLoad As Boolean = False)

        'action button options
        If Not initialLoad Then

            'form object visibility
            Select Case mySession.ActionType

                Case Enums.ActionType.CycleCountTeam
                    Dim aryHiddenControls() As String = New String() {"lblItemKeyVal"}
                    Array.Sort(aryHiddenControls)
                    Common.ToggleControlVisibility(aryHiddenControls, Me.Controls)
                Case Else
                    Dim aryHiddenControls() As String = New String() {"lblItemKeyVal"}
                    Common.ToggleControlVisibility(aryHiddenControls, Me.Controls)

            End Select

        Else
            Dim aryHiddenControls() As String = New String() {"lblItemKeyVal", "lblDescriptionVal", "lblItemSubTeam", "lblItemSubTeamVal", "lblPkg", "lblPkgVal", "lblPriceType", "lblPriceTypeVal", "lblPrimaryVendor", "lblPrimaryVendorVal", "lblQty", "lblTotal", "lblTotalVal", "txtQty"}
            Common.ToggleControlVisibility(aryHiddenControls, Me.Controls)
        End If
    End Sub

#End Region

End Class