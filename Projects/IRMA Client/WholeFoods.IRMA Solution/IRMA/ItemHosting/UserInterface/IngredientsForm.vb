Imports WholeFoods.IRMA.ItemHosting.DataAccess
Public Class IngredientsForm
    Public Property IngredientsBO As IngredientsBO
    Public Property NutrifactsAreReadOnly As Boolean

    Sub New(ingredientsBO As IngredientsBO)
        InitializeComponent()

        Me.IngredientsBO = ingredientsBO

        DescriptionTxt.Text = ingredientsBO.Description
        IngredientsTxt.Text = ingredientsBO.Ingredients
    End Sub

    Private Sub IngredientsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.LabelTypeCbx.DataSource = ScaleLabelTypeDAO.GetComboList()

        If Me.LabelTypeCbx.Items.Count > 0 Then
            Me.LabelTypeCbx.DisplayMember = "Description"
            Me.LabelTypeCbx.ValueMember = "ID"
            Me.LabelTypeCbx.SelectedValue = IngredientsBO.LabelTypeID
        End If

        If NutrifactsAreReadOnly Then
            SaveChangesBtn.Enabled = False
            DescriptionTxt.ReadOnly = True
            IngredientsTxt.ReadOnly = True
            LabelTypeCbx.Enabled = False
        End If
    End Sub

    Private Sub SaveChangesBtn_Click(sender As Object, e As EventArgs) Handles SaveChangesBtn.Click
        If IsInputValid() Then
            IngredientsBO.Ingredients = IngredientsTxt.Text.Trim()
            IngredientsBO.Description = DescriptionTxt.Text.Trim()
            IngredientsBO.LabelTypeID = CInt(LabelTypeCbx.SelectedValue)

            ScaleIngredientsDAO.UpdateIngredients(IngredientsBO)

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show("Description, Ingredients, and Label Type cannot be empty.")
        End If
    End Sub

    Private Sub CancelBtn_Click(sender As Object, e As EventArgs) Handles CancelBtn.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Function IsInputValid() As Boolean
        Return Not String.IsNullOrWhiteSpace(IngredientsTxt.Text) _
            And Not String.IsNullOrWhiteSpace(DescriptionTxt.Text) _
            And Not Me.LabelTypeCbx.SelectedIndex = -1
    End Function
End Class