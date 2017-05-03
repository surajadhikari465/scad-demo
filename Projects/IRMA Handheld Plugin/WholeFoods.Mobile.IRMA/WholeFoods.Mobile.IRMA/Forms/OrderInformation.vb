Public Class OrderInformation
    Public WriteOnly Property OrderNotes() As String
        Set(ByVal value As String)
            LabelOrderNotesField.Text = value
        End Set
    End Property

    Public WriteOnly Property Buyer() As String
        Set(ByVal value As String)
            LabelBuyerField.Text = value
        End Set
    End Property

    Public WriteOnly Property OrderDate() As String
        Set(ByVal value As String)
            LabelOrderDateField.Text = value
        End Set
    End Property

    Public WriteOnly Property IsCreditOrder() As Boolean
        Set(ByVal value As Boolean)
            If value Then
                CheckBoxCredit.Checked = True
                CheckBoxProduct.Checked = False
            End If
        End Set
    End Property

    Public Sub New(ByRef order As Order)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        CheckBoxCredit.Checked = False
        CheckBoxProduct.Checked = True

        AlignText()
    End Sub

    Private Sub OrderInformation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Assign values to the UI controls.
    End Sub

    Private Sub ButtonOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Me.Close()
    End Sub

    Private Sub AlignText()
        LabelOrderDate.TextAlign = ContentAlignment.TopRight
        LabelBuyerTitle.TextAlign = ContentAlignment.TopRight
    End Sub
    
End Class