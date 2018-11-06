Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Data
Imports System.Text
Imports Microsoft.WindowsCE.Forms
Imports System.IO
Imports System.ServiceModel

Public Class TransferMain
    Private mySession As Session
    Private serviceFault As ParsedCFFaultException = Nothing
    Private serviceCallSuccess As Boolean
    Private serverDateTime As DateTime
    Private Const TransferSessionKeptDays As Double = 0

#Region " Constructors"
    Public Sub New(ByVal session As Session)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.mySession = session
        AlignText()

    End Sub

#End Region

    Private Sub TransferMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'load all store and subteam selections
        cmbFromStore.DataSource = mySession.Stores
        cmbFromStore.DisplayMember = "DisplayMember"
        cmbFromStore.ValueMember = "ValueMember"

        cmbFromSubTeam.DataSource = mySession.Subteams
        cmbFromSubTeam.DisplayMember = "DisplayMember"
        cmbFromSubTeam.ValueMember = "ValueMember"

        If mySession.StoresWithVendorId Is Nothing Then
            Cursor.Current = Cursors.WaitCursor

            Try
                ' Attempt a service call to populate the StoresWithVendorId property and get the server's DateTime information.
                serviceCallSuccess = True

                PopulateStoresWithVendorId()
                serverDateTime = mySession.WebProxyClient.GetSystemDate()


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
                err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "TransferMain_Load")
                serviceCallSuccess = False
                Me.DialogResult = Windows.Forms.DialogResult.Abort

            Catch ex As CommunicationException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "TransferMain_Load")
                serviceCallSuccess = False
                Me.DialogResult = Windows.Forms.DialogResult.Abort

            Catch ex As Exception
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(ex.Message, "TransferMain_Load")
                serviceCallSuccess = False
                Me.DialogResult = Windows.Forms.DialogResult.Abort

            Finally
                Cursor.Current = Cursors.Default

            End Try

            If Not serviceCallSuccess Then
                ' The call to GetStores failed.  End the method and return to the previous form.
                Exit Sub
            End If

        End If

        'cmbToStore.BindingContext = New BindingContext()
        cmbToStore.DataSource = mySession.StoresWithVendorId
        cmbToStore.DisplayMember = "DisplayMember"
        cmbToStore.ValueMember = "ValueMember" 

        cmbToSubTeam.BindingContext = New BindingContext()
        cmbToSubTeam.DataSource = mySession.Subteams
        cmbToSubTeam.DisplayMember = "DisplayMember"
        cmbToSubTeam.ValueMember = "ValueMember"

        ' they can only change their location if they are a superuser, a coordinator, or have ALL stores in their IRMA setup
        cmbToStore.Enabled = mySession.UserAllStoresAccess

        If mySession.ProductTypes Is Nothing Then
            mySession.PopulateProductType()
        End If

        cmbProductType.DataSource = mySession.ProductTypes
        cmbProductType.DisplayMember = "DisplayMember"
        cmbProductType.ValueMember = "ValueMember"
        AddHandler cmbProductType.SelectedValueChanged, AddressOf cmbProductType_SelectedValueChanged
        If serverDateTime = Nothing Then
            serverDateTime = mySession.WebProxyClient.GetSystemDate()
        End If
        dtpExpectedDate.MinDate = serverDateTime.Date
        InitializeScreenValues()
        CheckForSavedSession()

    End Sub

    Private Sub cmbProductType_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ProductTypeId As Integer
        ProductTypeId = Enums.ProductType.OtherSupplies

        If cmbProductType.SelectedItem IsNot Nothing And cmbProductType.SelectedValue = ProductTypeId Then
            If mySession.SupplySubteams Is Nothing Then
                Try
                    ' Attempt a service call to populate supply subteams.
                    serviceCallSuccess = True

                    PopulateSupplySubteams(ProductTypeId)


                    ' Explicitly handle service faults, timeouts, and connection failures.  
                Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                    serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                    Dim err As New ErrorHandler(serviceFault)
                    err.ShowErrorNotification()
                    serviceCallSuccess = False

                Catch ex As TimeoutException
                    Dim err As New ErrorHandler()
                    err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "cmbProductType_SelectedValueChanged")
                    serviceCallSuccess = False

                Catch ex As CommunicationException
                    Dim err As New ErrorHandler()
                    err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "cmbProductType_SelectedValueChanged")
                    serviceCallSuccess = False

                Catch ex As Exception
                    Dim err As New ErrorHandler()
                    err.DisplayErrorMessage(ex.Message, "cmbProductType_SelectedValueChanged")
                    serviceCallSuccess = False
                End Try

                If Not serviceCallSuccess Then
                    ' The call to GetSubteamByProductType failed.  Reset the ComboBox and allow the user to retry.
                    cmbProductType.SelectedIndex = -1
                    Exit Sub
                End If

            End If

            cmbSupplyType.DataSource = mySession.SupplySubteams
            cmbSupplyType.DisplayMember = "DisplayMember"
            cmbSupplyType.ValueMember = "ValueMember"
            cmbSupplyType.SelectedIndex = 0

            lblSupplyType.Visible = True
            cmbSupplyType.Visible = True
        Else
            lblSupplyType.Visible = False
            cmbSupplyType.Visible = False
        End If
    End Sub

    Private Sub mmuNewOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mmuNewOrder.Click
        mySession.IsLoadedSession = False
        InitializeScreenValues()
    End Sub

    Private Sub mmuCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mmuCreate.Click

        If (String.IsNullOrEmpty(Me.cmbFromStore.SelectedValue.ToString()) Or _
                Me.cmbFromStore.SelectedIndex = 0 Or _
                Me.cmbFromSubTeam.SelectedIndex = 0 Or _
                String.IsNullOrEmpty(Me.cmbFromSubTeam.SelectedValue.ToString())) Then
            MessageBox.Show("Please select a 'Transfer From' store/subteam to continue.", "Error")
        ElseIf (String.IsNullOrEmpty(Me.cmbToStore.SelectedValue.ToString()) Or _
                    Me.cmbToStore.SelectedIndex = 0 Or _
                    Me.cmbToSubTeam.SelectedIndex = 0 Or _
                    String.IsNullOrEmpty(Me.cmbToSubTeam.SelectedValue.ToString())) Then
            MessageBox.Show("Please select a 'Transfer To' store/subteam to continue.", "Error")
        ElseIf (Me.cmbToStore.SelectedIndex = Me.cmbFromStore.SelectedIndex And _
                    Me.cmbToSubTeam.SelectedValue.ToString() = Me.cmbFromSubTeam.SelectedValue.ToString()) Then
            MessageBox.Show("'Transfer From' and 'Transfer To' stores/subteams cannot be the same.", "Error")
        ElseIf (dtpExpectedDate.Value > serverDateTime.Date And _
                    Me.cmbToStore.SelectedIndex = Me.cmbFromStore.SelectedIndex) Then
            MessageBox.Show("Expected Date for intra-store transfers needs to be set to today's date.", "Error")
        Else
            'Initialize set selected store/subteam
            mySession.transferVendorId = 0
            mySession.SetTransferFromSubteamKey(Me.cmbFromSubTeam.SelectedValue & "," & Me.cmbFromSubTeam.Text)
            mySession.SetTransferToSubteamKey(Me.cmbToSubTeam.SelectedValue & "," & Me.cmbToSubTeam.Text)
            mySession.SetTransferFromStore(Me.cmbFromStore.SelectedValue & "," & Me.cmbFromStore.Text)
            mySession.SetTransferToStore(Me.cmbToStore.SelectedValue & "," & Me.cmbToStore.Text)
            mySession.TransferExpectedDate = dtpExpectedDate.Value
            mySession.SelectedProductType = cmbProductType.SelectedValue
            If cmbSupplyType.Visible Then
                mySession.SelectedSupplySubteam = cmbSupplyType.SelectedValue
            Else
                mySession.SelectedSupplySubteam = 0
            End If

            'Delete unsent older transfer files (XML files created for transfer orders)
            mySession.StartTime = serverDateTime
            Dim fileWriter As TransferFileWriter = New TransferFileWriter(Me.mySession)
            mySession.SessionName = fileWriter.GenerateSessionName()
            fileWriter.DeleteOlderSessions(TransferSessionKeptDays, serverDateTime.Date)

            Dim transferScan As TransferScan = New TransferScan(Me.mySession)
            Dim res As DialogResult = transferScan.ShowDialog()

            If res = Windows.Forms.DialogResult.Abort Then
                Me.DialogResult = Windows.Forms.DialogResult.Abort
            ElseIf res = Windows.Forms.DialogResult.OK Then
                Me.Close()
            End If

            transferScan.Dispose()
        End If
    End Sub

    Private Sub CheckForSavedSession()
        Try
            Dim fileWriter As TransferFileWriter = New TransferFileWriter(Me.mySession)
            If fileWriter.SavedSessionExists Then
                Dim myValues As String() = fileWriter.PREVIOUS_SESSION.Split("_")
                If MessageBox.Show("Would you like to reload your previous Order Session? (From " & _
                                   myValues(0) & " / " & myValues(1) & " to " & myValues(2) & " / " & myValues(3) & ")" & vbCrLf & _
                                   "Clicking No will delete the old session.", _
                                   "Previous Session Exists", MessageBoxButtons.YesNo, _
                                   MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.No Then
                    If MessageBox.Show("Are you sure you want to delete your saved Order?" & vbCrLf & _
                                      "(From " & myValues(0) & " / " & myValues(1) & " to " & myValues(2) & " / " & myValues(3) & ")", "Delete Session?", _
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Hand, _
                                      MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        fileWriter.DeleteFile(fileWriter.MakeFilePath(fileWriter.PREVIOUS_SESSION))
                        If (Not Me.mySession.SessionName = Nothing) Then
                            Me.mySession.SessionName = Nothing
                        End If
                        Me.Close()
                    Else
                        Me.Close()
                    End If
                Else
                    Cursor.Current = Cursors.WaitCursor
                    Dim newSession As Session = New Session(mySession.ServiceURI)
                    newSession.Region = mySession.Region
                    newSession.MyScanner = Me.mySession.MyScanner
                    Me.mySession = fileWriter.GetFileSession(fileWriter.MakeFilePath(fileWriter.PREVIOUS_SESSION), newSession)
                    mySession.IsLoadedSession = True
                    Cursor.Current = Cursors.Default
                    ShowTransferReview()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("Could not edit saved Transfer Order Session: " + ex.Message)
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Sub mmuExitTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mmuExitTransfer.Click
        mySession.ClearTransferSession()
        mySession.IsLoadedSession = False
        Me.Close()
    End Sub

    Private Sub mmuExitIRMA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mmuExitIRMA.Click
        Me.DialogResult = Windows.Forms.DialogResult.Abort
    End Sub

    Private Sub PopulateSupplySubteams(ByVal ProductType_ID As Integer)

        Dim dtSubteams As DataTable
        Dim dr As DataRow

        Dim subteamResult As ListsSubteam() = Me.mySession.WebProxyClient.GetSubteamByProductType(ProductType_ID)

        If (subteamResult.Length > 0) Then

            dtSubteams = New DataTable

            dtSubteams.Columns.Add(New DataColumn("DisplayMember"))
            dtSubteams.Columns.Add(New DataColumn("ValueMember"))

            For Each Subteam In subteamResult
                dr = dtSubteams.NewRow()

                dr.Item("DisplayMember") = Subteam.SubteamName
                dr.Item("ValueMember") = Subteam.SubteamNo

                dtSubteams.Rows.Add(dr)
            Next

            mySession.SupplySubteams = dtSubteams

        End If
    End Sub

    Private Sub PopulateStoresWithVendorId()
        Dim dtStores As DataTable
        Dim dr As DataRow

        '----TRANSFER TO STORES----
        Dim storeResult As ListsStore() = Me.mySession.WebProxyClient.GetStores(True)

        If (storeResult.Length > 0) Then

            dtStores = New DataTable
            dtStores.Columns.Add(New DataColumn("DisplayMember"))
            dtStores.Columns.Add(New DataColumn("ValueMember"))

            For Each store In storeResult

                dr = dtStores.NewRow()
                dr.Item("DisplayMember") = store.StoreName
                dr.Item("ValueMember") = store.StoreNo

                'If store.StoreName = Me.mySession.StoreName Then
                '    Me.mySession.StoreVendorID = store.StoreNo 'Transfer To store vender ID
                'End If
                dtStores.Rows.Add(dr)

            Next

            dr = dtStores.NewRow()
            dr.Item("DisplayMember") = "-- Select Store --"
            dr.Item("ValueMember") = -1
            dtStores.Rows.InsertAt(dr, 0)

            mySession.StoresWithVendorId = dtStores

        End If
    End Sub

    Private Sub InitializeScreenValues()
        cmbFromStore.SelectedValue = mySession.StoreNo
        cmbFromSubTeam.SelectedValue = mySession.SubteamKey

        Dim table As DataTable = Me.mySession.StoresWithVendorId
        Dim index As Integer = 0
        Dim displayItem As String
        For index = 0 To cmbToStore.Items.Count - 1
            displayItem = table.Rows(index)(cmbToStore.DisplayMember).ToString()
            If displayItem = Me.mySession.StoreName Then
                cmbToStore.SelectedIndex = index
                Exit For
            End If
        Next

        cmbToSubTeam.SelectedValue = mySession.SubteamKey
        cmbProductType.SelectedIndex = 0

        lblSupplyType.Visible = False
        cmbSupplyType.Visible = False

        dtpExpectedDate.Value = serverDateTime.Date

        Me.cmbFromStore.Focus()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ShowTransferReview()

        Dim reviewTransfer As TransferReview = New TransferReview(Me.mySession)

        Dim res As DialogResult = reviewTransfer.ShowDialog()

        If res = Windows.Forms.DialogResult.Abort Then
            Me.DialogResult = Windows.Forms.DialogResult.Abort
        ElseIf res = Windows.Forms.DialogResult.OK Then
            Me.Close()
        End If

        reviewTransfer.Dispose()

    End Sub

    Private Sub AlignText()
        lblProductType.TextAlign = ContentAlignment.TopRight
        lblSupplyType.TextAlign = ContentAlignment.TopRight
        lblExpectedDate.TextAlign = ContentAlignment.TopRight
    End Sub

End Class