Imports System.Windows.Forms
Imports System
Imports System.Text
Imports System.Linq
Imports System.Data
Imports Microsoft.WindowsCE.Forms
Imports WholeFoods.Mobile.IRMA.Common
Imports System.ServiceModel

Public Class FindOrderByItem
    
    Private session As Session
    Private serviceFault As ParsedCFFaultException = Nothing
    Private serviceCallSuccess As Boolean
    Private storeNumber As Integer
    Private subteam As Integer
    Private dataTable As New DataTable("Orders")

    Private _poNumber As String
    Public Property PoNumber() As String
        Get
            Return _poNumber
        End Get
        Set(ByVal value As String)
            _poNumber = value
        End Set
    End Property

    Private _upc As String
    Public Property UPC() As String
        Get
            Return _upc
        End Get
        Set(ByVal value As String)
            _upc = value
        End Set
    End Property


    Public Sub New(ByRef session As Session, ByVal UPC As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.KeyPreview = True
        Me.session = session
        Me.UPC = UPC
        Me.storeNumber = Integer.Parse(session.StoreNo)

        AlignText()

    End Sub

    Private Sub FindOrderByItem_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' TFS 12037 - An unexplained and unreproducable NullReference exception is occurring sporadically during this load event in 4.7.0.  Until we
        ' have better logging capabilities, a try/catch will be used for this event to prevent total app annihilation.
        Try
            'If Me.session.CurrentScreen = IRMA.Session.CurrentScreenType.ReceivingDocumentScan Then
            '    Me.Close()
            '    Exit Sub
            'End If

            LabelStore.Text = session.StoreName
            LabelUPCValue.Text = UPC

            SetupDataGrid()
            RefreshDataTable(UPC)
        Catch ex As Exception
            MsgBox("An unexpected error occurred.  Please contact support with any details about the item scanned and the steps leading up to this error.", MsgBoxStyle.Exclamation, Me.Text)
            MsgBox("Error details: " + ex.Message + Environment.NewLine + ex.InnerException.Message, MsgBoxStyle.Information, Me.Text)
            Me.Close()
        End Try

    End Sub

    Private Sub SetupDataGrid()
        dataTable.Columns.Add("PO")
        dataTable.Columns.Add("Ord.Cost")
        dataTable.Columns.Add("Exp.Date")
        dataTable.Columns.Add("Subteam")
        dataTable.Columns.Add("Einv")
    End Sub

    Private Sub RefreshDataTable(ByVal UPC As String)
        Cursor.Current = Cursors.WaitCursor

        Dim orderArray As Order() = Nothing

        Try
            ' Attempt a service library call to get orders containing a given identifier.
            serviceCallSuccess = True

            orderArray = session.WebProxyClient.GetOrderHeaderByIdentifier(UPC, storeNumber)


            ' Explicitly handle service faults, timeouts, and connection failures.  If a failure occurs, allow the user to retry
            ' the last action.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "RefreshDataTable")
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "RefreshDataTable")
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "RefreshDataTable")
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort
        End Try

        If Not serviceCallSuccess Then
            ' The call to GetOrderHeaderByIdentifier failed.  Return to ReceiveOrder.
            Cursor.Current = Cursors.Default
            Exit Sub
        End If

        If orderArray.Count() = 0 Then
            MsgBox("No open orders found.", MsgBoxStyle.Information, Me.Text)
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        Else
            dataTable.Clear()
            For Each order As Order In orderArray
                ' Due to formatting constraints, we take only a ten-character substring of the subteam name and remove all spaces.
                dataTable.Rows.Add(order.OrderHeader_ID, FormatNumber(order.OrderedCost, 2), order.Expected_Date, order.SubTeam_Name.Substring(0, If(order.SubTeam_Name.Length < 10, order.SubTeam_Name.Length, 10)).Replace(" ", ""), If(order.EinvoiceID <> 0, "Y", "N"))
            Next
            DataGridPONumber.DataSource = dataTable
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub MenuItemExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemExit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub AlignText()
        LabelUPC.TextAlign = ContentAlignment.TopRight
        LabelUPCValue.TextAlign = ContentAlignment.TopCenter
    End Sub

    Private Sub MenuItemSelectPO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemSelectPO.Click
        If MsgBox("Select PO# " + DataGridPONumber.Item(DataGridPONumber.CurrentRowIndex, 0).ToString + "?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Me.Text) = MsgBoxResult.Yes Then
            Me.PoNumber = DataGridPONumber.Item(DataGridPONumber.CurrentRowIndex, 0).ToString
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

End Class