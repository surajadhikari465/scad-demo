Imports System.Linq
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess

Public Class ItemNutritionOverride

    Private _identifier As String
    Private currentNutrifactId As Integer
    Private formIsReadOnly As Boolean
    Private _isDirty As Boolean
    Private _extraTextBO As ItemExtraTextBO
    Private _ingredientsBO As ItemIngredientBO
    Private _allergensBO As ItemAllergenBO
    Private _defaultJurisdictionId As Integer
    Private IsInitializing As Boolean

    Public Property ExtraTextBO() As ItemExtraTextBO
        Get
            Return _extraTextBO
        End Get
        Set(value As ItemExtraTextBO)
            _extraTextBO = value
        End Set
    End Property

    Public Property IngredientsBO() As ItemIngredientBO
        Get
            Return _ingredientsBO
        End Get
        Set(value As ItemIngredientBO)
            _ingredientsBO = value
        End Set
    End Property

    Public Property AllergensBO() As ItemAllergenBO
        Get
            Return _allergensBO
        End Get
        Set(value As ItemAllergenBO)
            _allergensBO = value
        End Set
    End Property

    Public Property DefaultJurisdictionId() As Integer
        Get
            Return _defaultJurisdictionId
        End Get
        Set(ByVal value As Integer)
            _defaultJurisdictionId = value
        End Set
    End Property

    Public Property Identifier() As String
        Get
            Return _identifier
        End Get
        Set(ByVal value As String)
            _identifier = value
        End Set
    End Property

    Public Property IsDirty()
        Get
            Return _isDirty
        End Get
        Set(value)
            _isDirty = value
        End Set
    End Property

    Public Sub New(ByVal identifier As String)
        IsInitializing = True
        InitializeComponent()
        IsInitializing = False

        Me.Identifier = identifier
    End Sub

    Private Sub ButtonExit_Click(sender As Object, e As EventArgs) Handles ButtonExit.Click
        Me.Close()
    End Sub

    Private Sub ItemNutritionOverride_Load(sender As Object, e As EventArgs) Handles Me.Load
        IsInitializing = True
        Me.Text = Trim(Me.Text) & " - " & Me.Identifier

        Dim storeJurisdiction As New StoreJurisdictionDAO
        ComboBox_AltJurisdiction.DisplayMember = "StoreJurisdictionDesc"
        ComboBox_AltJurisdiction.ValueMember = "StoreJurisdictionID"
        ComboBox_AltJurisdiction.DataSource = storeJurisdiction.GetJurisdictionList(Me.DefaultJurisdictionId)

        Me.LoadExtraTextOverride(ComboBox_AltJurisdiction.SelectedValue)
        IsInitializing = False
    End Sub

    Private Sub ButtonNutrifactAddOrEdit_Click(sender As Object, e As EventArgs) Handles ButtonNutrifactAddOrEdit.Click
        Dim previousSelection As ScaleNutrifactBO = Nothing
        Dim lastViewedNutrifact As ScaleNutrifactBO = Nothing

        Using nutrifactForm As New Form_Nutrifact()
            If ComboBoxNutrifacts.SelectedIndex > -1 Then
                previousSelection = CType(ComboBoxNutrifacts.SelectedItem, ScaleNutrifactBO)
                nutrifactForm.NutrifactID = previousSelection.ID
            End If

            nutrifactForm.ShowDialog()

            If nutrifactForm.DescriptionCombo.SelectedIndex > -1 Then
                lastViewedNutrifact = CType(nutrifactForm.DescriptionCombo.SelectedItem, ScaleNutrifactBO)
            End If
        End Using

        ComboBoxNutrifacts.DataSource = ScaleNutrifactDAO.GetNutrifactComboList()

        If lastViewedNutrifact IsNot Nothing Then
            Dim lastViewedIndex As Integer = ComboBoxNutrifacts.FindStringExact(lastViewedNutrifact.Description)
            ComboBoxNutrifacts.SelectedIndex = lastViewedIndex
        Else
            ComboBoxNutrifacts.SelectedIndex = -1
        End If
    End Sub

    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        Dim saveSuccess As Boolean = False

        Select Case Me.ItemNutritionTabs.SelectedTab.Text
            Case "Extra Text"
                saveSuccess = Me.SaveExtraTextOverride(ComboBox_AltJurisdiction.SelectedValue)
            Case "NutriFacts"
                saveSuccess = Me.SaveNutrifacts(ComboBox_AltJurisdiction.SelectedValue)
            Case "Ingredients"
                saveSuccess = Me.SaveIngredients(ComboBox_AltJurisdiction.SelectedValue)
            Case "Allergens"
                saveSuccess = Me.SaveAllergens(ComboBox_AltJurisdiction.SelectedValue)
        End Select

        If (saveSuccess) Then
            MessageBox.Show("Changes have been saved successfully.", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ButtonSave.Enabled = False
        End If
    End Sub

    Private Function SaveExtraTextOverride(overrideJurisdictionId As Integer) As Boolean
        ' validate before saving
        Dim statusList As ArrayList = ScaleNutrifactDAO.ValidatexExtraText(txtExtraText.Text.Trim())
        Dim statusEnum As IEnumerator = statusList.GetEnumerator
        Dim currentStatus As ScaleNutrifactsValidationStatus

        While statusEnum.MoveNext
            currentStatus = CType(statusEnum.Current, ScaleNutrifactsValidationStatus)
            Select Case currentStatus
                Case ScaleNutrifactsValidationStatus.Error_ExtraTextInvalidCharacters
                    NotifyOfInvalidCharacters(ExtraTextTab.Text, txtExtraText, ItemSignAttributeDAO.INVALID_CHARACTERS)
                    Return False
            End Select
        End While

        ' save
        ScaleNutrifactDAO.InsertOrUpdateItemExtraText(glItemID, ExtraTextBO.ExtraTextID, ExtraTextLabelTypeCbx.SelectedValue, txtExtraText.Text.Trim(), overrideJurisdictionId)
        Return True

    End Function

    Private Function SaveNutrifacts(overrideJurisdictionId As Integer) As Boolean
        If ComboBoxNutrifacts.SelectedIndex = -1 Then
            ScaleNutrifactDAO.DeleteItemNutrifact(glItemID, overrideJurisdictionId)
        Else
            ScaleNutrifactDAO.InsertOrUpdateItemNutrifact(glItemID, ComboBoxNutrifacts.SelectedValue, overrideJurisdictionId)
        End If
        Return True
    End Function

    Private Function SaveIngredients(overrideJurisdictionId As Integer) As Boolean
        ' validate before saving
        Dim statusList As ArrayList = ScaleNutrifactDAO.ValidateIngredientsText(IngredientsTxt.Text)
        Dim statusEnum As IEnumerator = statusList.GetEnumerator
        Dim currentStatus As ScaleNutrifactsValidationStatus

        While statusEnum.MoveNext
            currentStatus = CType(statusEnum.Current, ScaleNutrifactsValidationStatus)
            Select Case currentStatus
                Case ScaleNutrifactsValidationStatus.Error_IngredientsTextInvalidCharacters
                    NotifyOfInvalidCharacters(Label11.Text.Replace(" :", ""), IngredientsTxt, ItemSignAttributeDAO.INVALID_CHARACTERS)
                    Return False
            End Select
        End While

        ScaleNutrifactDAO.InsertOrUpdateItemIngredient(glItemID, IngredientsBO.ScaleIngredientID, IngredientsTxt.Text, overrideJurisdictionId)
        Return True
    End Function

    Private Function SaveAllergens(overrideJurisdictionId As Integer) As Boolean
        ' validate before saving
        Dim statusList As ArrayList = ScaleNutrifactDAO.ValidatexExtraText(AllergensTxt.Text)
        Dim statusEnum As IEnumerator = statusList.GetEnumerator
        Dim currentStatus As ScaleNutrifactsValidationStatus

        While statusEnum.MoveNext
            currentStatus = CType(statusEnum.Current, ScaleNutrifactsValidationStatus)
            Select Case currentStatus
                Case ScaleNutrifactsValidationStatus.Error_ExtraTextInvalidCharacters
                    NotifyOfInvalidCharacters(Label8.Text.Replace(" :", ""), AllergensTxt, ItemSignAttributeDAO.INVALID_CHARACTERS)
                    Return False
            End Select
        End While

        ScaleNutrifactDAO.InsertOrUpdateItemAllergen(glItemID, AllergensBO.ScaleAllergenID, AllergensTxt.Text, overrideJurisdictionId)
        Return True
    End Function

    Private Sub LockForm()
        Dim SelectedTab As String = Me.ItemNutritionTabs.SelectedTab.Text
        Select Case SelectedTab
            Case "NutriFacts"
                Me.ComboBoxNutrifacts.Enabled = False
                Me.ButtonSave.Enabled = False
                Me.ButtonNutrifactRemove.Enabled = False
                Me.ButtonNutrifactAddOrEdit.Text = "View"
                If ComboBoxNutrifacts.SelectedIndex = -1 Then
                    Me.ButtonNutrifactAddOrEdit.Enabled = False
                End If
            Case "Ingredients"
                Me.IngredientsLabelTypeCbx.Enabled = False
                Me.IngredientsTxt.Enabled = False
                Me.ButtonSave.Enabled = False
            Case "Allergens"
                Me.AllergensLabelTypeCbx.Enabled = False
                Me.AllergensTxt.Enabled = False
                Me.ButtonSave.Enabled = False
        End Select
    End Sub

    Private Sub LoadDataBasedOnActiveTab(selectedTabText As String)
        Select Case selectedTabText
            Case "Extra Text"
                Me.LoadExtraTextOverride(ComboBox_AltJurisdiction.SelectedValue)
            Case "NutriFacts"
                Me.LoadNutriFactsOverride(ComboBox_AltJurisdiction.SelectedValue)
            Case "Ingredients"
                Me.LoadIngredientsOverride(ComboBox_AltJurisdiction.SelectedValue)
            Case "Allergens"
                Me.LoadAllergensOverride(ComboBox_AltJurisdiction.SelectedValue)
        End Select
    End Sub

    Private Sub LoadExtraTextOverride(overrideJurisdictionId As Integer)
        Me.ExtraTextLabelTypeCbx.DataSource = ScaleLabelTypeDAO.GetComboList()

        If Me.ExtraTextLabelTypeCbx.Items.Count > 0 Then
            Me.ExtraTextLabelTypeCbx.DisplayMember = "Description"
            Me.ExtraTextLabelTypeCbx.ValueMember = "ID"
            Me.ExtraTextLabelTypeCbx.SelectedIndex = -1
        End If

        Me.ExtraTextBO = ScaleExtraTextDAO.GetExtraTextForNonScaleItemByItem(glItemID, overrideJurisdictionId)
        Me.txtExtraText.Text = Me.ExtraTextBO.ExtraText
        Me.ExtraTextLabelTypeCbx.SelectedValue = Me.ExtraTextBO.Scale_LabelType_ID
    End Sub

    Private Sub LoadNutriFactsOverride(overrideJurisdictionId As Integer)
        LabelIdentifierValue.Text = Me.Identifier

        Dim nutrifacts As ScaleNutrifactBO() = ScaleNutrifactDAO.GetNutrifactComboList().ToArray(GetType(ScaleNutrifactBO))
        ComboBoxNutrifacts.DataSource = nutrifacts
        ComboBoxNutrifacts.DisplayMember = "Description"
        ComboBoxNutrifacts.ValueMember = "ID"

        Dim currentNutrifactId As Integer = ScaleNutrifactDAO.GetNutriFactByItem(glItemID, overrideJurisdictionId, False)

        If currentNutrifactId > 0 Then
            Dim nutrifactDescription As String = nutrifacts.Single(Function(n) n.ID = currentNutrifactId).Description
            ComboBoxNutrifacts.SelectedIndex = ComboBoxNutrifacts.FindStringExact(nutrifactDescription)
        Else
            ComboBoxNutrifacts.SelectedIndex = -1
        End If

        ButtonSave.Enabled = False

        formIsReadOnly = InstanceDataDAO.IsFlagActive("EnableNutrifactIntegration")

        If formIsReadOnly Then
            LockForm()
        End If
    End Sub

    Private Sub LoadIngredientsOverride(overrideJurisdictionId As Integer)
        Me.IngredientsLabelTypeCbx.DataSource = ScaleLabelTypeDAO.GetComboList()

        If Me.IngredientsLabelTypeCbx.Items.Count > 0 Then
            Me.IngredientsLabelTypeCbx.DisplayMember = "Description"
            Me.IngredientsLabelTypeCbx.ValueMember = "ID"
            Me.IngredientsLabelTypeCbx.SelectedIndex = -1
        End If

        Me.IngredientsBO = ScaleExtraTextDAO.GetIngredientForNonScaleItemByItem(glItemID, overrideJurisdictionId)
        Me.IngredientsTxt.Text = Me.IngredientsBO.Ingredients

        formIsReadOnly = InstanceDataDAO.IsFlagActive("EnableAllergenAndIngredientIntegration")

        If formIsReadOnly Then
            LockForm()
        End If

    End Sub

    Private Sub LoadAllergensOverride(overrideJurisdictionId As Integer)
        Me.AllergensLabelTypeCbx.DataSource = ScaleLabelTypeDAO.GetComboList()

        If Me.AllergensLabelTypeCbx.Items.Count > 0 Then
            Me.AllergensLabelTypeCbx.DisplayMember = "Description"
            Me.AllergensLabelTypeCbx.ValueMember = "ID"
            Me.AllergensLabelTypeCbx.SelectedIndex = -1
        End If

        Me.AllergensBO = ScaleExtraTextDAO.GetAllergenForNonScaleItemByItem(glItemID, overrideJurisdictionId)
        Me.AllergensTxt.Text = Me.AllergensBO.Allergens

        formIsReadOnly = InstanceDataDAO.IsFlagActive("EnableAllergenAndIngredientIntegration")

        If formIsReadOnly Then
            LockForm()
        End If

    End Sub

    Private Sub ComboBox_AltJurisdiction_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_AltJurisdiction.SelectedIndexChanged
        If Not IsInitializing Then
            Dim selectedTabText = ItemNutritionTabs.SelectedTab.Text
            Me.LoadDataBasedOnActiveTab(selectedTabText)
        End If
    End Sub

    Private Sub ItemNutritionTabs_Selected(sender As Object, e As TabControlEventArgs) Handles ItemNutritionTabs.Selected
        Dim selectedTab As String = e.TabPage.Text
        Me.LoadDataBasedOnActiveTab(selectedTab)
    End Sub

    Private Sub ComboBoxNutrifacts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxNutrifacts.SelectedIndexChanged
        If Not formIsReadOnly Then
            ButtonSave.Enabled = True
        End If
    End Sub

    Private Sub ButtonNutrifactRemove_Click(sender As Object, e As EventArgs) Handles ButtonNutrifactRemove.Click
        ComboBoxNutrifacts.SelectedIndex = -1
    End Sub

    Private Sub ExtraTextLabelTypeCbx_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ExtraTextLabelTypeCbx.SelectionChangeCommitted
        Me.IsDirty = True
        Me.UpdateSaveButtonStatus()
    End Sub
    Private Sub txtExtraText_TextChanged(sender As Object, e As EventArgs) Handles txtExtraText.TextChanged
        Me.IsDirty = True
        Me.UpdateSaveButtonStatus()
    End Sub
    Private Sub IngredientsLabelTypeCbx_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles IngredientsLabelTypeCbx.SelectionChangeCommitted
        Me.IsDirty = True
        Me.UpdateSaveButtonStatus()
    End Sub
    Private Sub IngredientsTxt_TextChanged(sender As Object, e As EventArgs) Handles IngredientsTxt.TextChanged
        Me.IsDirty = True
        Me.UpdateSaveButtonStatus()
    End Sub
    Private Sub AllergensLabelTypeCbx_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles AllergensLabelTypeCbx.SelectionChangeCommitted
        Me.IsDirty = True
        Me.UpdateSaveButtonStatus()
    End Sub
    Private Sub AllergensTxt_TextChanged(sender As Object, e As EventArgs) Handles AllergensTxt.TextChanged
        Me.IsDirty = True
        Me.UpdateSaveButtonStatus()
    End Sub
    Private Sub ButtonExtraTextLabelTypeRemove_Click(sender As Object, e As EventArgs) Handles ButtonExtraTextLabelTypeRemove.Click
        Me.ExtraTextLabelTypeCbx.SelectedIndex = -1
    End Sub

    Private Sub UpdateSaveButtonStatus()
        If Me.IsDirty Then
            Me.ButtonSave.Enabled = True
        Else
            Me.ButtonSave.Enabled = False
        End If
    End Sub

    Private Sub NotifyOfInvalidCharacters(ByRef sFieldCaption As String, ByRef ctlControl As System.Windows.Forms.Control, ByRef sInvalidCharacters As String)
        Dim validationErrorMsg As String = String.Format(ResourcesIRMA.GetString("InvalidCharacters"), sFieldCaption, sInvalidCharacters)
        MsgBox(validationErrorMsg, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, Me.Text)
        ctlControl.Focus()
    End Sub
End Class