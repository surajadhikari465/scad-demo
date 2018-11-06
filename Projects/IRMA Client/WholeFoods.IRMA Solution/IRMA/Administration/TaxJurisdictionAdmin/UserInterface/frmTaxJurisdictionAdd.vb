Public Class TaxJurisdictionAdd
#Region "Properties"
    Private mTaxJurisdiction As New TaxJurisdictionAdminBO
    Public ReadOnly Property NewTaxJurisdiction() As TaxJurisdictionAdminBO
        Get
            Return mTaxJurisdiction
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

    Private Sub frmTaxJurisdictionAdd_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim TaxJurisdictionList As New TaxJurisdictionAdminDAO
        Me.cboCloneTaxJurisdiction.DataSource = TaxJurisdictionList.GetJurisdictionList()
        Me.cboCloneTaxJurisdiction.DisplayMember = "TaxJurisdictionDesc"
        Me.cboCloneTaxJurisdiction.ValueMember = "TaxJurisdictionID"
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        mTaxJurisdiction.OldTaxJurisdicitonID = CInt(Me.cboCloneTaxJurisdiction.SelectedValue)
        mTaxJurisdiction.TaxJurisdictionDesc = Me.txtTaxJurisdictionDesc.Text
        mTaxJurisdiction = mTaxJurisdiction.InsertClone
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

#End Region
End Class