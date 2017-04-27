Imports WholeFoods.IRMA.ItemHosting.DataAccess
Public Class AllergensCreateForm
    Public Property AllergensBO As AllergensBO
    Public Property ItemKey As Integer

    Sub New()
        InitializeComponent()

        AllergensBO = New AllergensBO()

        DescriptionTxt.Focus()
    End Sub

    Private Sub AllergensCreateForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.LabelTypeCbx.DataSource = ScaleLabelTypeDAO.GetComboList()

        If Me.LabelTypeCbx.Items.Count > 0 Then
            Me.LabelTypeCbx.DisplayMember = "Description"
            Me.LabelTypeCbx.ValueMember = "ID"
            Me.LabelTypeCbx.SelectedIndex = -1
        End If
    End Sub

    Private Sub AddRecordBtn_Click(sender As Object, e As EventArgs) Handles AddRecordBtn.Click
        If IsInputValid() Then
            Me.AllergensBO.Description = DescriptionTxt.Text.Trim()
            Me.AllergensBO.Allergens = AllergensTxt.Text.Trim()
            Me.AllergensBO.LabelTypeID = CInt(LabelTypeCbx.SelectedValue)

            ScaleAllergensDAO.AddAllergensToItem(ItemKey, AllergensBO)

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show("Description, Allergens, and Label Type cannot be empty.")
        End If
    End Sub

    Private Function IsInputValid() As Boolean
        Return Not String.IsNullOrWhiteSpace(DescriptionTxt.Text) _
            And Not String.IsNullOrWhiteSpace(AllergensTxt.Text) _
            And Not Me.LabelTypeCbx.SelectedIndex = -1
    End Function

    Private Sub CancelBtn_Click(sender As Object, e As EventArgs) Handles CancelBtn.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class