Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Data
Imports System.Text
Imports Microsoft.WindowsCE.Forms
Imports System.ServiceModel

Public Class ItemDiscount

    Private mySession As Session
    Private serviceFault As ParsedCFFaultException = Nothing
    Private serviceCallSuccess As Boolean
    Private dtsTypes As DataTable
    Private _upc As String = ""
    Private _desc As String = ""
    Private _prevDiscountAmount As Double = 0.0
    Private _prevDiscountType As Integer = 0
    Private _prevDiscountReasonCode As Integer = 0
    Private _discountAmount As Double = 0.0
    Private _discountType As Integer = 0
    Private _discountReasonCode As Integer = 0

    Public WriteOnly Property UPC() As String
        Set(ByVal value As String)
            _upc = value
        End Set
    End Property

    Public WriteOnly Property Desc() As String
        Set(ByVal value As String)
            _desc = value
        End Set
    End Property

    Public WriteOnly Property CurrentDiscountAmount() As Double
        Set(ByVal value As Double)
            _prevDiscountAmount = value
        End Set
    End Property

    Public WriteOnly Property CurrentDiscountType() As Integer
        Set(ByVal value As Integer)
            _prevDiscountType = value
        End Set
    End Property

    Public WriteOnly Property CurrentDiscountReasonCode() As Integer
        Set(ByVal value As Integer)
            _prevDiscountReasonCode = value
        End Set
    End Property

    Public ReadOnly Property DiscountAmount() As Double
        Get
            Return _discountAmount
        End Get
    End Property

    Public ReadOnly Property DiscountType() As Integer
        Get
            Return _discountType
        End Get
    End Property

    Public ReadOnly Property DiscountReasonCode() As Integer
        Get
            Return _discountReasonCode
        End Get
    End Property


#Region " Constructors"
    Public Sub New(ByVal session As Session)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.mySession = session

        AlignText()

    End Sub

#End Region

    Private Sub ItemDiscount_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            ' Attempt a service call to populate the reason code list.
            serviceCallSuccess = True

            PopulateReasonCodes()


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
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "ItemDiscount_Load")
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "ItemDiscount_Load")
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "ItemDiscount_Load")
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort
        End Try

        If Not serviceCallSuccess Then
            ' The method PopulateReasonCodes failed.  End the method and allow the form to close.
            Exit Sub
        End If

        cmbReasonCode.BindingContext = New BindingContext()
        cmbReasonCode.DataSource = mySession.DiscountReasonCodeList
        cmbReasonCode.DisplayMember = "Description"
        cmbReasonCode.ValueMember = "Code"

        lblPercent.Visible = False

        ' Load existing discount info.
        If _prevDiscountReasonCode <> 0 Then cmbReasonCode.SelectedValue = _prevDiscountReasonCode
        txtDiscount.Text = CStr(_prevDiscountAmount)
        cmbDiscountType.SelectedIndex = _prevDiscountType

        lblUPCValue.Text = _upc
        lblDescValue.Text = _desc

        cmbDiscountType.Focus()
        Cursor.Current = Cursors.Default

    End Sub

    Public Sub PopulateReasonCodes()

        Dim dtReasonCodes As DataTable
        Dim dr As DataRow

        Dim reasonCodesResult As ListsReasonCode() = Me.mySession.WebProxyClient.GetDiscountReasonCodes()

        If (reasonCodesResult.Length > 0) Then

            dtReasonCodes = New DataTable

            dtReasonCodes.Columns.Add(New DataColumn("Code"))
            dtReasonCodes.Columns.Add(New DataColumn("Description"))
            dr = dtReasonCodes.NewRow()
            dr.Item("Code") = -1
            dr.Item("Description") = "Select a Reason Code:"
            dtReasonCodes.Rows.Add(dr)

            For Each reasonCode In reasonCodesResult
                dr = dtReasonCodes.NewRow()

                dr.Item("Code") = reasonCode.ReasonCodeID
                dr.Item("Description") = reasonCode.ReasonCode + "  - " + reasonCode.Description

                dtReasonCodes.Rows.Add(dr)
            Next

            mySession.DiscountReasonCodeList = dtReasonCodes

        End If
    End Sub

    Private Sub mmuCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mmuCancel.Click

        Me._discountType = 0
        Me._discountAmount = 0.0
        Me._discountReasonCode = 0

        Me.DialogResult = Windows.Forms.DialogResult.None
        Me.Close()
    End Sub

    Private Sub mmuAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mmuAccept.Click

        If Not IsNumeric(Trim(txtDiscount.Text)) Then
            MessageBox.Show("Please enter a valid discount amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        Dim discount As Double = CDbl(Trim(txtDiscount.Text))
        If discount < 0 Then
            MessageBox.Show("Please enter a positive discount amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        If cmbDiscountType.SelectedIndex = 1 And discount > 99.99 Then
            MessageBox.Show("Please enter a valid cash discount (99.99 max).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        If cmbDiscountType.SelectedIndex = 2 And discount > 100 Then
            MessageBox.Show("Please enter a valid percent discount amount (0 to 100).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        If cmbDiscountType.SelectedIndex <> 0 And cmbReasonCode.SelectedIndex = 0 Then
            MessageBox.Show("Please select a reason code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        Me._discountAmount = CDbl(Trim(txtDiscount.Text))
        Me._discountType = cmbDiscountType.SelectedIndex
        Me._discountReasonCode = cmbReasonCode.SelectedValue

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub cmbDiscountType_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDiscountType.SelectedValueChanged
        lblPercent.Visible = False
        If cmbDiscountType.SelectedIndex = 0 Then
            txtDiscount.Text = String.Empty
            txtDiscount.Enabled = False
            mmuAccept.Enabled = False
        ElseIf cmbDiscountType.SelectedIndex = 2 Then
            txtDiscount.Text = String.Empty
            txtDiscount.Enabled = True
            mmuAccept.Enabled = True
            lblPercent.Visible = True
        Else
            txtDiscount.Enabled = True
            mmuAccept.Enabled = True
        End If
    End Sub

    Private Sub AlignText()
        lblUPC.TextAlign = ContentAlignment.TopRight
        lblDesc.TextAlign = ContentAlignment.TopRight
        lblDiscount.TextAlign = ContentAlignment.TopRight
        lblDiscountType.TextAlign = ContentAlignment.TopRight
    End Sub
End Class