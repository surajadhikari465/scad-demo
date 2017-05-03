Imports WholeFoods.IRMA.ItemHosting.DataAccess
Public Class AllergensForm
    Public Property AllergenBO As AllergensBO
    Public Property NutrifactsAreReadOnly As Boolean

    Sub New(allergenBO As AllergensBO)

        InitializeComponent()

        Me.AllergenBO = allergenBO

        DescriptionTxt.Text = allergenBO.Description
        AllergensTxt.Text = allergenBO.Allergens
    End Sub

    Private Sub AllergensForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.LabelTypeCbx.DataSource = ScaleLabelTypeDAO.GetComboList()

        If Me.LabelTypeCbx.Items.Count > 0 Then
            Me.LabelTypeCbx.DisplayMember = "Description"
            Me.LabelTypeCbx.ValueMember = "ID"
            Me.LabelTypeCbx.SelectedValue = AllergenBO.LabelTypeID
        End If

        If NutrifactsAreReadOnly Then
            SaveChangesBtn.Enabled = False
            DescriptionTxt.ReadOnly = True
            AllergensTxt.ReadOnly = True
            LabelTypeCbx.Enabled = False
        End If
    End Sub

    Private Sub SaveChangesBtn_Click(sender As Object, e As EventArgs) Handles SaveChangesBtn.Click
        If IsInputValid() Then
            AllergenBO.Description = DescriptionTxt.Text.Trim()
            AllergenBO.Allergens = AllergensTxt.Text.Trim()
            AllergenBO.LabelTypeID = CInt(LabelTypeCbx.SelectedValue)

            ScaleAllergensDAO.UpdateAllergens(AllergenBO)

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show("Description, Allergens, and Label Type cannot be empty.")
        End If
    End Sub

    Private Sub CancelBtn_Click(sender As Object, e As EventArgs) Handles CancelBtn.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Function IsInputValid() As Boolean
        Return Not String.IsNullOrWhiteSpace(AllergensTxt.Text) _
            And Not String.IsNullOrWhiteSpace(DescriptionTxt.Text) _
            And Not Me.LabelTypeCbx.SelectedIndex = -1
    End Function
End Class