Public Class CurrencyAdd

#Region "Properties"
    Private mCurrency As New Currency
    Public ReadOnly Property NewCurrency() As Currency
        Get
            Return mCurrency
        End Get
    End Property
#End Region

#Region "Subs and Functions"
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        InitializeForm()

    End Sub

    Private Sub InitializeForm()

    End Sub
#End Region

#Region "Event Handlers"
    Private Sub frmCurrencyAdd_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        mCurrency.Code = Me.txtCurrencyCode.Text
        mCurrency.Description = Me.txtCurrencyDesc.Text
        mCurrency.NewCurrency()
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
#End Region


End Class