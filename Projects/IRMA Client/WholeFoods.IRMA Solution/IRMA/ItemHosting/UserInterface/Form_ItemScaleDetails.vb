Option Strict Off

Imports System.Text
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports log4net

<Microsoft.VisualBasic.ComClass()> Public Class Form_ItemScaleDetails
    'Private nutrifactForm As Form_Nutrifact
    Private extraTextForm As ExtraTextEdit
    Private _scaleDetailsBO As ScaleDetailsBO
    Private _scaleExtraTextBO As ScaleExtraTextBO
    Private _formOwnsData As Boolean = True
    Private hasChanges As Boolean
    Private hasNutrifactsChanges As Boolean
    Private hasIngredientsChanges As Boolean
    Private hasAllergensChanges As Boolean
    Private hasStorageDataChanges As Boolean
    Private isLoading As Boolean
    Private nutrifactsAreReadyOnly As Boolean
    Private allergensAndIngredientsAreReadyOnly As Boolean?
    Private _nutrifactsID As Integer
    Private _scaleIngredientsID As Integer
    Private _scaleAllergensID As Integer
    Private _scaleStorageDataID As Integer
    Private addNutrifacts As Boolean
    Private addIngredients As Boolean
    Private addAllergens As Boolean
    Private addStorageData As Boolean

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Dim WithEvents itemScaleOverride As Form_ItemScaleDetailsOverride
    Private this As Object

#Region "Constructor"

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.ScaleDetailsBO = New ScaleDetailsBO
        Me.FormOwnsData = True

    End Sub
#End Region

#Region "Properties"

    Public Property ScaleDetailsBO() As ScaleDetailsBO
        Get
            Return _scaleDetailsBO
        End Get
        Set(ByVal value As ScaleDetailsBO)
            _scaleDetailsBO = value

        End Set
    End Property

    Public Property FormOwnsData() As Boolean
        Get
            Return _formOwnsData
        End Get
        Set(ByVal value As Boolean)
            _formOwnsData = value
        End Set
    End Property

    Public Property ItemIdentifier() As String
        Get
            Return Me.ScaleDetailsBO.ItemIdentifier
        End Get
        Set(ByVal value As String)
            Me.ScaleDetailsBO.ItemIdentifier = value
        End Set
    End Property

    Public Property ItemKey() As Integer
        Get
            Return Me.ScaleDetailsBO.ItemKey
        End Get
        Set(ByVal value As Integer)
            Me.ScaleDetailsBO.ItemKey = value
        End Set
    End Property

    Public Property StoreJurisdictionId() As String
        Get
            Return Me.ScaleDetailsBO.StoreJurisdictionID
        End Get
        Set(ByVal value As String)
            Me.ScaleDetailsBO.StoreJurisdictionID = value
        End Set
    End Property

    Public Property StoreJurisdictionDesc() As String
        Get
            Return Me.ScaleDetailsBO.StoreJurisdictionDesc
        End Get
        Set(ByVal value As String)
            Me.ScaleDetailsBO.StoreJurisdictionDesc = value
        End Set
    End Property

    Public Property NutrifactsID() As Integer
        Get
            Return Me.ScaleDetailsBO.Nutrifact
        End Get
        Set(value As Integer)
            Me.ScaleDetailsBO.Nutrifact = value
        End Set
    End Property

    Public Property ScaleIngredientsID() As Integer
        Get
            Return _scaleIngredientsID
        End Get
        Set(value As Integer)
            _scaleIngredientsID = value
        End Set
    End Property

    Public Property ScaleAllergensID() As Integer
        Get
            Return _scaleAllergensID
        End Get
        Set(value As Integer)
            _scaleAllergensID = value
        End Set
    End Property

    Public Property ScaleStorageDataID() As Integer
        Get
            Return _scaleStorageDataID
        End Get
        Set(value As Integer)
            _scaleStorageDataID = value
        End Set
    End Property

#End Region

#Region "Methods"
    Private Sub InitializeData()

        logger.Debug("InitializeData Entry")

        ' only if an item key has been provided
        If Me.FormOwnsData Then
            isLoading = True

            ' populate drop downs
            LoadCombos()

            ' get existing scale details for current item
            LoadDetails()

            isLoading = False
            SetDataChanges(False)

            ' enable/disable buttons
            If InstanceDataDAO.IsFlagActive("UseStoreJurisdictions") Then
                Button_Jurisdiction.Enabled = True
            Else
                Button_Jurisdiction.Enabled = False
            End If


        End If

        SetPermissions()

        If (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser Then
            EnableCountAndWeightBoxes()
        End If

        'ExtraTextButton.Enabled = True
        logger.Debug("InitializeData Exit")

    End Sub

    Private Sub SetPermissions()
        Dim IsEditable As Boolean = False

        IsEditable = (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser

        For Each c As Control In Me.Controls
            ' set all checkboxes
            If (TypeOf (c) Is CheckBox) Then
                c.Enabled = IsEditable
            End If

            ' set all textboxes
            If (TypeOf (c) Is TextBox) Then
                c.Enabled = IsEditable
            End If

            ' set all comboboxes
            If (TypeOf (c) Is ComboBox) Then
                Debug.WriteLine(c.Name)
                c.Enabled = IsEditable
            End If

            ' set all listboxes
            If (TypeOf (c) Is ListBox) Then
                c.Enabled = IsEditable
            End If

            If (TypeOf (c) Is GroupBox) Then
                For Each i As Control In c.Controls
                    If (TypeOf (i) Is ComboBox) Then
                        i.Enabled = IsEditable
                    End If
                    If (TypeOf (i) Is CheckBox) Then
                        i.Enabled = IsEditable
                    End If
                Next
            End If

        Next

        Me.General_GroupBox.Enabled = IsEditable
        Me.UOM_GroupBox.Enabled = IsEditable
        Me.GroupBox1.Enabled = IsEditable
        Me.GeneralGroupBox.Enabled = IsEditable
        Me.CaloriesGroupBox.Enabled = IsEditable
        Me.FatGroupBox.Enabled = IsEditable
        Me.NutritionGroupBox.Enabled = IsEditable
        Me.Ingredients_GroupBox.Enabled = IsEditable
        Me.Allergens_GroupBox.Enabled = IsEditable

        ByCountNumericEditor.Enabled = IsEditable
        ShelfLifeNumericEditor.Enabled = IsEditable
        ExtraTextButton.Enabled = IsEditable
        ExtraTextSearchButton.Enabled = IsEditable

        If IsNothing(allergensAndIngredientsAreReadyOnly) Then
            allergensAndIngredientsAreReadyOnly = InstanceDataDAO.IsFlagActive("EnableAllergenAndIngredientIntegration")
        End If

        If allergensAndIngredientsAreReadyOnly Then
            LockIngredientsAndAllergens()
        End If

        If InstanceDataDAO.IsFlagActive("EnableStorageData") Then
            Me.Storage_GroupBox.Enabled = IsEditable
        Else
            ScaleItemTabs.TabPages.Remove(StorageDataTab)
        End If

    End Sub

    Private Sub LoadCombos()

        logger.Debug("LoadCombos Entry")

        ScaleDetailsDAO.GetScaleDetailCombos(Me.ScaleDetailsBO)

        If Me.ScaleDetailsBO.ScaleLabelStyleList.Count > 0 Then
            LabelStyleCombo.DataSource = Me.ScaleDetailsBO.ScaleLabelStyleList
            LabelStyleCombo.DisplayMember = "Description"
            LabelStyleCombo.ValueMember = "ID"
            LabelStyleCombo.SelectedIndex = -1
        End If

        If Me.ScaleDetailsBO.ScaleEatByList.Count > 0 Then
            EatByCombo.DataSource = Me.ScaleDetailsBO.ScaleEatByList
            EatByCombo.DisplayMember = "Description"
            EatByCombo.ValueMember = "ID"
            EatByCombo.SelectedIndex = -1
        End If

        If Me.ScaleDetailsBO.ScaleGradeList.Count > 0 Then
            GradeCombo.DataSource = Me.ScaleDetailsBO.ScaleGradeList
            GradeCombo.DisplayMember = "Description"
            GradeCombo.ValueMember = "ID"
            GradeCombo.SelectedIndex = -1
        End If

        If Me.ScaleDetailsBO.ScaleRandomWeightTypeList.Count > 0 Then
            RandomWeightCombo.DataSource = Me.ScaleDetailsBO.ScaleRandomWeightTypeList
            RandomWeightCombo.DisplayMember = "Description"
            RandomWeightCombo.ValueMember = "ID"
            RandomWeightCombo.SelectedIndex = -1
        End If

        If Me.ScaleDetailsBO.ScaleTareAlternateList.Count > 0 Then
            AltTareCombo.DataSource = Me.ScaleDetailsBO.ScaleTareAlternateList
            AltTareCombo.DisplayMember = "Description"
            AltTareCombo.ValueMember = "ID"
            AltTareCombo.SelectedIndex = -1
        End If

        If Me.ScaleDetailsBO.ScaleTareList.Count > 0 Then
            TareCombo.DataSource = Me.ScaleDetailsBO.ScaleTareList
            TareCombo.DisplayMember = "Description"
            TareCombo.ValueMember = "ID"
            TareCombo.SelectedIndex = -1
        End If

        If Me.ScaleDetailsBO.ScaleUOMList.Count > 0 Then
            ScaleUOMCombo.DataSource = Me.ScaleDetailsBO.ScaleUOMList
            ScaleUOMCombo.DisplayMember = "Description"
            ScaleUOMCombo.ValueMember = "ID"
            ScaleUOMCombo.SelectedIndex = -1
        End If

        logger.Debug("LoadCombos Exit")

    End Sub

    Private Sub BindDataToControls()

        logger.Debug("BindDataToControls Entry")

        'If Not IsNothing(Me.ScaleDetailsBO.Nutrifact) And Me.ScaleDetailsBO.Nutrifact > 0 Then
        '    Me.NutrifactsID = Me.ScaleDetailsBO.Nutrifact
        'End If

        ' map control values to business object properties
        'For Each itm As ScaleNutrifactBO In NutrifactCombo.Items
        '    If itm.ID = Me.ScaleDetailsBO.Nutrifact Then
        '        NutrifactCombo.SelectedItem = itm
        '    End If
        'Next

        If Not Me.ScaleDetailsBO.ExtraText = Nothing Then
            ExtraTextValueLabel.Text = Me.ScaleDetailsBO.ExtraText.ToString
            ExtraTextValueLabel.Tag = Me.ScaleDetailsBO.ExtraTextID
        Else
            ExtraTextValueLabel.Tag = -1
            ExtraTextValueLabel.Text = String.Empty
        End If

        If Not Me.ScaleDetailsBO.Ingredient = Nothing Then
            IngredientsValueLabel.Text = Me.ScaleDetailsBO.Ingredient.ToString
            IngredientsValueLabel.Tag = Me.ScaleDetailsBO.IngredientID
            IngredientsAddBtn.Enabled = False
        Else
            IngredientsValueLabel.Tag = -1
            IngredientsValueLabel.Text = String.Empty
            IngredientsEditBtn.Enabled = False
        End If

        If Not Me.ScaleDetailsBO.Allergen = Nothing Then
            AllergensValueLabel.Text = Me.ScaleDetailsBO.Allergen.ToString
            AllergensValueLabel.Tag = Me.ScaleDetailsBO.AllergenID
            AllergensAddBtn.Enabled = False
        Else
            AllergensValueLabel.Tag = -1
            AllergensValueLabel.Text = String.Empty
            AllergensEditBtn.Enabled = False
        End If

        For Each itm As ScaleTareBO In TareCombo.Items
            If itm.ID = Me.ScaleDetailsBO.Tare Then TareCombo.SelectedItem = itm
        Next

        For Each itm As ScaleTareBO In AltTareCombo.Items
            If itm.ID = Me.ScaleDetailsBO.TareAlternate Then AltTareCombo.SelectedItem = itm
        Next

        For Each itm As ScaleLabelStyleBO In LabelStyleCombo.Items
            If itm.ID = Me.ScaleDetailsBO.LabelStyle Then LabelStyleCombo.SelectedItem = itm
        Next

        For Each itm As ScaleEatByBO In EatByCombo.Items
            If itm.ID = Me.ScaleDetailsBO.EatBy Then EatByCombo.SelectedItem = itm
        Next

        For Each itm As ScaleGradeBO In GradeCombo.Items
            If itm.ID = Me.ScaleDetailsBO.Grade Then GradeCombo.SelectedItem = itm
        Next

        For Each itm As ScaleRandomWeightTypeBO In RandomWeightCombo.Items
            If itm.ID = Me.ScaleDetailsBO.RandomWeightType Then RandomWeightCombo.SelectedItem = itm
        Next

        For Each itm As ScaleUOMsBO In ScaleUOMCombo.Items
            If itm.ID = Me.ScaleDetailsBO.UOM Then ScaleUOMCombo.SelectedItem = itm
        Next

        FixedWeightTextbox.Text = Me.ScaleDetailsBO.FixedWeight

        If Me.ScaleDetailsBO.ByCount > 0 Then
            ByCountNumericEditor.Value = Me.ScaleDetailsBO.ByCount
        End If

        PrintBlankShelfLifeCheckBox.Checked = Me.ScaleDetailsBO.PrintBlankShelfLife
        PrintBlankPackDateCheckBox.Checked = Me.ScaleDetailsBO.PrintBlankPackDate
        PrintBlankShelfEatByCheckBox.Checked = Me.ScaleDetailsBO.PrintBlankShelfEatBy
        PrintBlankTotalPriceCheckBox.Checked = Me.ScaleDetailsBO.PrintBlankTotalPrice
        PrintBlankUnitPriceCheckBox.Checked = Me.ScaleDetailsBO.PrintBlankUnitPrice
        PrintBlankWeightCheckBox.Checked = Me.ScaleDetailsBO.PrintBlankWeight
        ForceTareCheckBox.Checked = Me.ScaleDetailsBO.ForceTare
        ScaleDesc1TextBox.Text = Me.ScaleDetailsBO.ScaleDescription1
        ScaleDesc2TextBox.Text = Me.ScaleDetailsBO.ScaleDescription2
        ScaleDesc3TextBox.Text = Me.ScaleDetailsBO.ScaleDescription3
        ScaleDesc4TextBox.Text = Me.ScaleDetailsBO.ScaleDescription4
        ShelfLifeNumericEditor.Value = Me.ScaleDetailsBO.ShelfLifeLength

        logger.Debug("BindDataToControls Exit")
    End Sub

    Private Sub LoadDetails()
        logger.Debug("LoadDetails Entry")

        ScaleDetailsDAO.GetScaleDetails(Me.ScaleDetailsBO)

        'If Me.ScaleDetailsBO.Nutrifact = 0 Then 'new scale item - let the DAO set the nutrifactid to null 
        '    Me.ScaleDetailsBO.Nutrifact = -1
        'End If

        BindDataToControls()

        logger.Debug("LoadDetails Exit")
    End Sub

    Private Function ApplyChanges() As Boolean

        logger.Debug(" ApplyChanges Entry")

        Dim isSuccessful As Boolean = True
        Dim updateIdList As String = ""
        Dim updateInfoList As String = ""
        Dim rollbackIdList As String = ""
        Dim rollbackInfoList As String = ""

        ' Tom Lux, 2/9/10, TFS 11978, 3.5.9.
        ' Check for pending batches for the item being edited and prompt user if found.
        ' We first check to see if there are any pending batches for the item being edited.
        ' A return value of TRUE means one or more batches were found, so we need to confirm the item and batch(es) modification with the user.
        If ItemDAO.GetBatchesInSentState(Me.ItemKey, updateIdList, updateInfoList, rollbackIdList, rollbackInfoList) Then

            ' Before actually touching batches, we prompt user to confirm they want to proceed, saving the changes and rolling-back the batches.
            ' A return value of TRUE means the user wants to save changes and rollback batches.
            ' The user is only prompted if the specific list (update or rollback) is not empty.
            ' If the user says "NO" to either the update or rollback prompt, we abort the save procss,
            ' but return TRUE, so the calling function doesn't think an error occurred while trying to save the data.
            ' This function (ApplyChanges) is called when the OK button is clicked, meaning save changes and close the form,
            ' and when the close button on the window is clicked by the user (form is closing).
            ' So, we also set the 'hasChanges' flag checked by ok-clicked and form-closing functions.
            If updateIdList.Length > 0 Then
                If Not ItemBO.UserConfirmedSaveChangeAndModifyBatches(Me.ItemIdentifier, updateInfoList, BatchModificationType.Update) Then
                    SetDataChanges(False)
                    Return True
                End If
            End If
            If rollbackIdList.Length > 0 Then
                If Not ItemBO.UserConfirmedSaveChangeAndModifyBatches(Me.ItemIdentifier, rollbackInfoList, BatchModificationType.Update) Then
                    SetDataChanges(False)
                    Return True
                End If
            End If
        End If

        ' map control values to business object properties
        If Not ExtraTextValueLabel.Text = String.Empty Then
            Me.ScaleDetailsBO.ExtraTextID = CInt(ExtraTextValueLabel.Tag.ToString())
        Else
            Me.ScaleDetailsBO.ExtraTextID = -1
        End If

        If TareCombo.SelectedIndex > -1 Then
            Me.ScaleDetailsBO.Tare = CInt(TareCombo.SelectedValue.ToString())
        Else
            Me.ScaleDetailsBO.Tare = -1
        End If

        If AltTareCombo.SelectedIndex > -1 Then
            Me.ScaleDetailsBO.TareAlternate = CInt(AltTareCombo.SelectedValue.ToString())
        Else
            Me.ScaleDetailsBO.TareAlternate = -1
        End If

        If LabelStyleCombo.SelectedIndex > -1 Then
            Me.ScaleDetailsBO.LabelStyle = CInt(LabelStyleCombo.SelectedValue.ToString())
        Else
            Me.ScaleDetailsBO.LabelStyle = -1
        End If

        If EatByCombo.SelectedIndex > -1 Then
            Me.ScaleDetailsBO.EatBy = CInt(EatByCombo.SelectedValue.ToString())
        Else
            Me.ScaleDetailsBO.EatBy = -1
        End If

        If GradeCombo.SelectedIndex > -1 Then
            Me.ScaleDetailsBO.Grade = CInt(GradeCombo.SelectedValue.ToString())
        Else
            Me.ScaleDetailsBO.Grade = -1
        End If

        If RandomWeightCombo.SelectedIndex > -1 Then
            Me.ScaleDetailsBO.RandomWeightType = CInt(RandomWeightCombo.SelectedValue.ToString())
        Else
            Me.ScaleDetailsBO.RandomWeightType = -1
        End If

        If ScaleUOMCombo.SelectedIndex > -1 Then
            If FixedWeightTextbox.Enabled = True And FixedWeightTextbox.Text.Length = 0 Then
                'prompt user that this is required
                MsgBox(String.Format(ResourcesIRMA.GetString("Required (in General Tab 1)"), FixedWeightLabel.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
                isSuccessful = False
                Exit Function
            ElseIf ByCountNumericEditor.Enabled = True Then
                If Not ByCountNumericEditor.Value Is Nothing Then
                    If ByCountNumericEditor.Value.ToString().Length = 0 Then
                        'prompt user that this is required
                        MsgBox(String.Format(ResourcesIRMA.GetString("Required (in General Tab)"), ByCountLabel.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
                        logger.Info(String.Format(ResourcesIRMA.GetString("Required (in General Tab)"), ByCountLabel.Text.Replace(":", "")))
                        isSuccessful = False
                        logger.Debug("ApplyChanges Exit " & isSuccessful)
                        Exit Function
                    End If
                Else
                    If CType(ScaleUOMCombo.SelectedItem, ScaleUOMsBO).Description.ToUpper().StartsWith("BY COUNT") Then
                        'prompt user that this is required
                        MsgBox(String.Format(ResourcesIRMA.GetString("Required (in General Tab)"), ByCountLabel.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
                        logger.Info(String.Format(ResourcesIRMA.GetString("Required (in General Tab)"), ByCountLabel.Text.Replace(":", "")))
                        isSuccessful = False
                        logger.Debug("ApplyChanges Exit " & isSuccessful)
                        Exit Function
                    Else
                        isSuccessful = True
                    End If


                    ' Return isSuccessful
                    'Exit Function
                End If
            End If
            Me.ScaleDetailsBO.UOM = CInt(ScaleUOMCombo.SelectedValue.ToString())
        Else
            Me.ScaleDetailsBO.UOM = -1
        End If
        If FixedWeightTextbox.Enabled = True And FixedWeightTextbox.Text.Length > 0 Then
            Me.ScaleDetailsBO.FixedWeight = FixedWeightTextbox.Text
        Else
            'clear the value
            Me.ScaleDetailsBO.FixedWeight = String.Empty
        End If
        If ByCountNumericEditor.Enabled = True Then
            If Not ByCountNumericEditor.Value Is Nothing Then
                If ByCountNumericEditor.Value.ToString().Length > 0 Then
                    Me.ScaleDetailsBO.ByCount = CInt(ByCountNumericEditor.Value.ToString())
                End If
            End If
        Else
            'clear the value
            Me.ScaleDetailsBO.ByCount = 0
        End If

        Me.ScaleDetailsBO.PrintBlankShelfLife = PrintBlankShelfLifeCheckBox.Checked
        Me.ScaleDetailsBO.PrintBlankPackDate = PrintBlankPackDateCheckBox.Checked
        Me.ScaleDetailsBO.PrintBlankShelfEatBy = PrintBlankShelfEatByCheckBox.Checked
        Me.ScaleDetailsBO.PrintBlankTotalPrice = PrintBlankTotalPriceCheckBox.Checked
        Me.ScaleDetailsBO.PrintBlankUnitPrice = PrintBlankUnitPriceCheckBox.Checked
        Me.ScaleDetailsBO.PrintBlankWeight = PrintBlankWeightCheckBox.Checked
        Me.ScaleDetailsBO.ForceTare = ForceTareCheckBox.Checked
        Me.ScaleDetailsBO.ScaleDescription1 = ScaleDesc1TextBox.Text
        Me.ScaleDetailsBO.ScaleDescription2 = ScaleDesc2TextBox.Text
        Me.ScaleDetailsBO.ScaleDescription3 = ScaleDesc3TextBox.Text
        Me.ScaleDetailsBO.ScaleDescription4 = ScaleDesc4TextBox.Text
        Me.ScaleDetailsBO.ShelfLifeLength = CInt(ShelfLifeNumericEditor.Value.ToString())

        ' clear ByCount value if UOM is not By Count
        If ScaleUOMCombo.SelectedIndex > -1 Then
            If Not CType(ScaleUOMCombo.SelectedItem, ScaleUOMsBO).Description.ToUpper().StartsWith("BY COUNT") Then
                Me.ByCountNumericEditor.Value = 0
                Me.ScaleDetailsBO.ByCount = Me.ByCountNumericEditor.Value
            End If
        End If


        If Me.FormOwnsData Then
            ' call save method on DAO object
            ScaleDetailsDAO.SaveScaleData(Me.ScaleDetailsBO)
        End If

        ' Tom Lux, 2/9/10, TFS 11978, 3.5.9.
        ' This will modify pending batches for the item being edited, if it exists in a pending batch.
        ItemBO.UpdateOrRollbackBatches(Me.ItemIdentifier, updateIdList, updateInfoList, rollbackIdList, rollbackInfoList)

        InitializeData()

        logger.Debug("ApplyChanges Exit " & isSuccessful)

        Return isSuccessful
    End Function

#End Region

#Region "Events"

    Private Sub Form_ItemScaleDetails_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        logger.Debug("Form_ItemScaleDetails_Load Entry")

        Me.CenterToScreen()
        Me.IdentifierValueLabel.Text = Me.ScaleDetailsBO.ItemIdentifier
        Me.Label_DefaultJurisdictionValue.Text = Me.ScaleDetailsBO.StoreJurisdictionDesc

        InitializeData()

        nutrifactsAreReadyOnly = InstanceDataDAO.IsFlagActive("EnableNutrifactIntegration")

        logger.Debug("Form_ItemScaleDetails_Load Exit")
    End Sub

    Private Sub ScaleItemTabs_Selected(sender As Object, e As TabControlEventArgs) Handles ScaleItemTabs.Selected

        Dim SelectedTab As String = e.TabPage.Text
        Select Case SelectedTab
            Case "Nutrifacts"
                Me.LoadNutrifacts(Me.NutrifactsID)
                UpdateNutrifactsControls(nutrifactsAreReadyOnly)
                SetNutrifactsDataChanges(False)
            Case "Ingredients"
                Me.LoadIngredients(glItemID)
                SetIngredientsDataChanges(False)
            Case "Allergens"
                Me.LoadAllergens(glItemID)
                SetAllergensDataChanges(False)
            Case "Storage Data"
                Me.LoadStorageData(glItemID)
                SetStorageDataChanges(False)
        End Select

    End Sub

    Private Sub Form_ItemScaleDetails_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        logger.Debug("Form_ItemScaleDetails_FormClosing Entry")

        Dim IsEditable As Boolean = False
        IsEditable = (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser
        If IsEditable = False Then
            SetDataChanges(False)
        End If

        If hasChanges Then
            'prompt the user to save changes
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                If ApplyChanges() = False Then e.Cancel = True
            End If
        End If

        logger.Debug("Form_ItemScaleDetails_FormClosing Exit")

    End Sub

    Private Sub ScaleUOMCombo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScaleUOMCombo.SelectedIndexChanged
        logger.Debug("ScaleUOMCombo_SelectedIndexChanged Entry")

        SetDataChanges(True)
        EnableCountAndWeightBoxes()

        logger.Debug("ScaleUOMCombo_SelectedIndexChanged Exit")
    End Sub

    Private Sub EnableCountAndWeightBoxes()

        logger.Debug("EnableCountAndWeightBoxes Entry")

        Dim IsEditable As Boolean = True
        IsEditable = (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser

        If ScaleUOMCombo.SelectedIndex > -1 Then
            If CType(ScaleUOMCombo.SelectedItem, ScaleUOMsBO).Description.ToUpper().StartsWith("BY COUNT") And IsEditable Then
                ByCountNumericEditor.Enabled = True
                FixedWeightTextbox.Text = String.Empty
                FixedWeightTextbox.Enabled = False
            ElseIf CType(ScaleUOMCombo.SelectedItem, ScaleUOMsBO).Description.ToUpper().StartsWith("FIXED WEIGHT") And IsEditable Then
                ByCountNumericEditor.Value = Nothing
                ByCountNumericEditor.Enabled = False
                FixedWeightTextbox.Enabled = True
            Else
                ByCountNumericEditor.Value = Nothing
                ByCountNumericEditor.Enabled = False
                FixedWeightTextbox.Text = String.Empty
                FixedWeightTextbox.Enabled = False
            End If
        End If

        logger.Debug("EnableCountAndWeightBoxes Exit")

    End Sub

    Private Sub ByCountNumericEditor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ByCountNumericEditor.Click
        ByCountNumericEditor.SelectAll()
    End Sub

    Private Sub ByCountNumericEditor_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ByCountNumericEditor.Enter
        ByCountNumericEditor.SelectAll()
    End Sub

    Private Sub ByCountNumericEditor_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ByCountNumericEditor.ValueChanged
        SetDataChanges(True)
    End Sub

    Private Sub ExtraTextButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        logger.Debug("ExtraTextButton_Click Entry")

        Me.extraTextForm = New ExtraTextEdit

        Dim _extraTextBO As New ScaleExtraTextBO
        _extraTextBO = ScaleExtraTextDAO.GetExtraTextByItem(glItemID, Nothing)

        Me.extraTextForm.ItemKey = Me.ScaleDetailsBO.ItemKey
        Me.extraTextForm.CurrentExtraTextBO = _extraTextBO

        Me.extraTextForm.ShowDialog(Me)

        ' update the object
        Me.ScaleDetailsBO.ExtraTextID = Me.extraTextForm.CurrentExtraTextBO.ID
        Me.ScaleDetailsBO.ExtraText = Me.extraTextForm.CurrentExtraTextBO.ExtraText

        ' update the display
        ExtraTextValueLabel.Tag = Me.extraTextForm.CurrentExtraTextBO.ID
        ExtraTextValueLabel.Text = Me.extraTextForm.CurrentExtraTextBO.ExtraText

        extraTextForm.Close()
        extraTextForm.Dispose()

        logger.Debug("ExtraTextButton_Click Exit")

    End Sub

    Private Sub ExtraTextSearchButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        logger.Debug("ExtraTextSearchButton_Click Entry")

        Dim itemSearch As New frmItemSearch
        Dim extraTextBO As New ScaleExtraTextBO
        Dim currentItemID As Integer

        currentItemID = glItemID

        itemSearch.ShowDialog()
        itemSearch.Close()
        itemSearch.Dispose()

        ' look up the scale_extratext record for the selected item
        ' set combo to match this one
        extraTextBO = ScaleExtraTextDAO.GetExtraTextByItem(glItemID, Nothing)
        glItemID = currentItemID

        If extraTextBO.ID = -1 Then
            Exit Sub
        ElseIf extraTextBO.ID = 0 Then
            ' warn the user that this item has no nutrifact record associated with it
            MsgBox(ResourcesItemHosting.GetString("NoScaleExtraText"), MsgBoxStyle.Critical, Me.Text)
            logger.Info(ResourcesItemHosting.GetString("NoScaleExtraText"))
        Else
            Me.ScaleDetailsBO.ExtraText = extraTextBO.ExtraText
            Me.ScaleDetailsBO.ExtraTextID = extraTextBO.ID
            Me.ScaleDetailsBO.Description = extraTextBO.Description
            ExtraTextValueLabel.Tag = extraTextBO.ID
            ExtraTextValueLabel.Text = Me.ScaleDetailsBO.ExtraText
        End If

        logger.Debug("ExtraTextSearchButton_Click Exit")

    End Sub

    Private Sub ShelfLifeNumericEditor_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ShelfLifeNumericEditor.SelectAll()
    End Sub

    Private Sub ShelfLifeNumericEditor_Enter(ByVal sender As Object, ByVal e As System.EventArgs)
        ShelfLifeNumericEditor.SelectAll()
    End Sub

    Private Sub ShelfLifeNumericEditor_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SetDataChanges(True)
    End Sub

    Private Sub CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintBlankShelfLifeCheckBox.CheckedChanged, PrintBlankWeightCheckBox.CheckedChanged, PrintBlankUnitPriceCheckBox.CheckedChanged, PrintBlankTotalPriceCheckBox.CheckedChanged, PrintBlankShelfEatByCheckBox.CheckedChanged, PrintBlankPackDateCheckBox.CheckedChanged
        SetDataChanges(True)
    End Sub

    Private Sub TextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FixedWeightTextbox.TextChanged
        SetDataChanges(True)
    End Sub

    Private Sub Combo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RandomWeightCombo.SelectedIndexChanged
        SetDataChanges(True)
    End Sub

    Private Sub ExtraTextValueLabel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SetDataChanges(True)
    End Sub
#End Region

    ''' <summary>
    ''' Open the Form_ItemScaleDetailsOverride form to allow the user to manage the store jurisdiction override data for the item-scale data.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Jurisdiction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Jurisdiction.Click

        logger.Debug("Button_Jurisdiction_Click Entry")

        ' If there have been changes, prompt the user to save before continuing.
        If hasChanges Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                ApplyChanges()
                SetDataChanges(False)
            End If
        End If

        itemScaleOverride = New Form_ItemScaleDetailsOverride
        itemScaleOverride.DefaultScaleData = _scaleDetailsBO

        itemScaleOverride.ShowDialog(Me)
        itemScaleOverride.Close()
        itemScaleOverride.Dispose()

        logger.Debug("Button_Jurisdiction_Click Exit")

    End Sub

    Private Sub ButtonExit_Click(sender As System.Object, e As System.EventArgs) Handles ButtonExit.Click
        ' If there have been changes, prompt the user to save before canceling.
        If hasChanges Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                If ApplyChanges() Then
                    Me.Close()
                End If
            Else
                ' Just close without saving - change the edit flags so the user is not prompted for save
                ' again on the form closing event.
                SetDataChanges(False)
                Me.Close()
            End If
        Else
            Me.Close()
        End If
    End Sub

    Private Sub ButtonSaveScaleInformation_Click(sender As System.Object, e As System.EventArgs) Handles ButtonSaveScaleInformation.Click
        Dim SelectedTab As String = Me.ScaleItemTabs.SelectedTab.Text

        Select Case SelectedTab
            Case "General"
                If ApplyChanges() Then
                    MsgBox("All changes have been saved.", MsgBoxStyle.Information, Me.Text)
                    SetDataChanges(False)
                End If
            Case "Nutrifacts"
                If NutrifactCombo.SelectedIndex < 0 Then
                    Me.NutrifactsID = -1 'Set NutrifactID to -1 so that it gets set to a DBNull when the scale info is saved
                    If ApplyChanges() Then 'ApplyChanges will save ItemScale record with the Nutrifact_ID as NULL since the NutrifactID < 0
                        MsgBox("The Nutrifact has been removed from this scale item successfully.", MsgBoxStyle.Information, Me.Text)
                        SetNutrifactsDataChanges(False)
                    End If
                ElseIf Me.ApplyNutrifactsChanges() AndAlso ApplyChanges() Then
                    MsgBox("All nutrifacts changes have been saved.", MsgBoxStyle.Information, Me.Text)
                    SetNutrifactsDataChanges(False)
                Else
                    MsgBox("Nutrifacts changes have not been saved.", MsgBoxStyle.Exclamation, Me.Text)
                End If
            Case "Ingredients"
                'Save Ingredients
                If Me.ApplyIngredientsChanges() Then
                    MsgBox("All ingredients changes have been saved.", MsgBoxStyle.Information, Me.Text)
                    SetIngredientsDataChanges(False)
                End If
            Case "Allergens"
                'Save Allergens
                If Me.ApplyAllergensChanges() Then
                    MsgBox("All allergens changes have been saved.", MsgBoxStyle.Information, Me.Text)
                    SetAllergensDataChanges(False)
                End If
            Case "Storage Data"
                'Save Storage Data
                If Me.ApplyStorageDataChanges() Then
                    MsgBox("All storage data changes have been saved.", MsgBoxStyle.Information, Me.Text)
                    SetStorageDataChanges(False)
                End If
        End Select
    End Sub

    Private Sub SetDataChanges(ByVal dataChangesExist As Boolean)
        If (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser Then
            hasChanges = dataChangesExist
            ButtonSaveScaleInformation.Enabled = dataChangesExist
        End If
    End Sub
    Private Sub SetNutrifactsDataChanges(ByVal dataChangesExist As Boolean)
        If (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser Then
            hasNutrifactsChanges = dataChangesExist
            ButtonSaveScaleInformation.Enabled = dataChangesExist
        End If

    End Sub

    Private Sub SetIngredientsDataChanges(ByVal dataChangesExist As Boolean)
        If (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser Then
            hasIngredientsChanges = dataChangesExist
            ButtonSaveScaleInformation.Enabled = dataChangesExist
        End If

    End Sub

    Private Sub SetAllergensDataChanges(ByVal dataChangesExist As Boolean)
        If (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser Then
            hasAllergensChanges = dataChangesExist
            ButtonSaveScaleInformation.Enabled = dataChangesExist
        End If

    End Sub

    Private Sub SetStorageDataChanges(ByVal dataChangesExist As Boolean)
        If (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser Then
            hasStorageDataChanges = dataChangesExist
            ButtonSaveScaleInformation.Enabled = dataChangesExist
        End If
    End Sub

    Private Sub LockIngredientsAndAllergens()
        IngredientsAddBtn.Enabled = False
        AllergensAddBtn.Enabled = False
        Me.Ingredients_GroupBox.Enabled = False
        Me.Allergens_GroupBox.Enabled = False
    End Sub

    Private Sub IngredientsEditButton_Click(sender As Object, e As EventArgs) Handles IngredientsEditBtn.Click
        Dim ingredientsBO = ScaleIngredientsDAO.GetIngredientsByItem(glItemID)

        Dim ingredientsForm = New IngredientsForm(ingredientsBO) With {.NutrifactsAreReadOnly = nutrifactsAreReadyOnly}
        ingredientsForm.ShowDialog(Me)
        IngredientsValueLabel.Text = ingredientsBO.Description
    End Sub

    Private Sub IngredientsAddBtn_Click(sender As Object, e As EventArgs) Handles IngredientsAddBtn.Click
        Dim ingredientsCreateForm = New IngredientsCreateForm With {.ItemKey = glItemID}
        ingredientsCreateForm.ShowDialog(Me)

        If ingredientsCreateForm.DialogResult = DialogResult.OK Then
            IngredientsValueLabel.Text = ingredientsCreateForm.IngredientsBO.Description
            IngredientsAddBtn.Enabled = False
            IngredientsEditBtn.Enabled = True
        End If
    End Sub

    Private Sub AllergensEditButton_Click(sender As Object, e As EventArgs) Handles AllergensEditBtn.Click
        Dim allergensBO = ScaleAllergensDAO.GetAllergensByItem(glItemID)

        Dim allergensForm = New AllergensForm(allergensBO) With {.NutrifactsAreReadOnly = nutrifactsAreReadyOnly}
        allergensForm.ShowDialog(Me)
        AllergensValueLabel.Text = allergensBO.Description
    End Sub

    Private Sub AllergensAddBtn_Click(sender As Object, e As EventArgs) Handles AllergensAddBtn.Click
        Dim allergensCreateForm = New AllergensCreateForm With {.ItemKey = glItemID}
        allergensCreateForm.ShowDialog(Me)

        If allergensCreateForm.DialogResult = DialogResult.OK Then
            AllergensValueLabel.Text = allergensCreateForm.AllergensBO.Description
            AllergensAddBtn.Enabled = False
            AllergensEditBtn.Enabled = True
        End If
    End Sub



    Private Sub LoadNutrifacts(ByVal NutrifactID As Integer)
        If LabelFormatCombo.DataSource Is Nothing Then
            LabelFormatCombo.DataSource = ScaleLabelFormatDAO.GetComboList()
        End If

        If LabelFormatCombo.Items.Count > 0 Then
            LabelFormatCombo.DisplayMember = "Description"
            LabelFormatCombo.ValueMember = "ID"
            LabelFormatCombo.SelectedIndex = -1
        End If

        If NutrifactCombo.DataSource Is Nothing Then
            NutrifactCombo.DataSource = ScaleNutrifactDAO.GetNutrifactComboList()
        End If

        If NutrifactCombo.Items.Count > 0 Then
            NutrifactCombo.DisplayMember = "Description"
            NutrifactCombo.ValueMember = "ID"
            isLoading = False

            If NutrifactID > 0 Then
                ' load with matching record from calling screen
                NutrifactCombo.SelectedValue = NutrifactID
                PopulateNutrifactsControls(NutrifactID)
            Else
                Me.NutrifactsID = NutrifactID
                ClearNutrifacts()
            End If
        Else
            ClearNutrifacts()
        End If
    End Sub

    Private Sub LoadIngredients(ByVal ItemID As Integer)

        Dim ingredientsBO = ScaleIngredientsDAO.GetIngredientsByItem(glItemID)
        Me.ScaleIngredientsID = ingredientsBO.ID
        Me.IngredientsTxt.Text = Trim(ingredientsBO.Ingredients)

        If ingredientsBO.ID = 0 Then
            addIngredients = True
        Else
            addIngredients = False
        End If

    End Sub

    Private Sub LoadAllergens(ByVal ItemID As Integer)

        Dim allergensBO = ScaleAllergensDAO.GetAllergensByItem(glItemID)
        Me.ScaleAllergensID = allergensBO.ID
        Me.AllergensTxt.Text = Trim(allergensBO.Allergens)

        If allergensBO.ID = 0 Then
            addAllergens = True
        Else
            addAllergens = False
        End If

    End Sub

    Private Sub LoadStorageData(ByVal ItemID As Integer)

        Dim storageDataBO = ScaleStorageDataDAO.GetStorageDataByItem(glItemID)

        Me.ScaleStorageDataID = storageDataBO.ID
        Me.StorageDataTxt.Text = Trim(storageDataBO.StorageData)
        Me.txtDescription.Text = storageDataBO.Description
        Me.txtDescription.Enabled = False
        If storageDataBO.ID = 0 Then
            addStorageData = True
        Else
            addStorageData = False
        End If

    End Sub

    Private Sub PopulateNutrifactsControls(ByVal nutrifact_ID As Integer)
        Dim scaleNutriFact As ScaleNutrifactBO
        scaleNutriFact = ScaleNutrifactDAO.GetNutriFact(nutrifact_ID)

        BetaCaroteneNumericEditor.Value = scaleNutriFact.Betacarotene
        BiotinNumericEditor.Value = scaleNutriFact.Biotin
        CalciumNumericEditor.Value = scaleNutriFact.Calcium
        CalFatNumericEditor.Value = scaleNutriFact.CaloriesFat
        CalSatFatNumericEditor.Value = scaleNutriFact.CaloriesSaturatedFat
        CalTotalNumericEditor.Value = scaleNutriFact.Calories
        CalTransFatNumericEditor.Value = scaleNutriFact.CaloriesFromTransFat
        CarbsNumericEditor.Value = scaleNutriFact.TotalCarbohydrateWeight
        CarbsPercentNumericEditor.Value = scaleNutriFact.TotalCarbohydratePercent
        ChlorideNumericEditor.Value = scaleNutriFact.Chloride
        CholesterolNumericEditor.Value = scaleNutriFact.CholesterolWeight
        CholesterolPercentNumericEditor.Value = scaleNutriFact.CholesterolPercent
        ChromiumNumericEditor.Value = scaleNutriFact.Chromium
        CopperNumericEditor.Value = scaleNutriFact.Copper
        NutrifactDescriptionTextbox.Text = scaleNutriFact.Description
        FatMonoNumericEditor.Value = scaleNutriFact.MonounsaturatedFat
        FatOmega3NumericEditor.Value = scaleNutriFact.Om3Fatty
        FatOmega6NumericEditor.Value = scaleNutriFact.Om6Fatty
        FatPolyNumericEditor.Value = scaleNutriFact.PolyunsaturatedFat
        FatSatNumericEditor.Value = scaleNutriFact.SaturatedFatWeight
        FatTransNumericEditor.Value = scaleNutriFact.TransfatWeight
        FatSatPercentNumericEditor.Value = scaleNutriFact.SaturatedFatPercent
        FatTotalNumericEditor.Value = scaleNutriFact.TotalFatWeight
        FatTotalPercentNumericEditor.Value = scaleNutriFact.TotalFatPercentage
        FiberNumericEditor.Value = scaleNutriFact.DietaryFiberWeight
        FiberPercentNumericEditor.Value = scaleNutriFact.DietaryFiberPercent
        FolateNumericEditor.Value = scaleNutriFact.Folate
        InsolFiberNumericEditor.Value = scaleNutriFact.InsolubleFiber
        IodineNumericEditor.Value = scaleNutriFact.Iodine
        IronNumericEditor.Value = scaleNutriFact.Iron
        If LabelFormatCombo.Items.Count > 0 Then LabelFormatCombo.SelectedValue = scaleNutriFact.Scale_LabelFormat_ID
        MagnesiumNumericEditor.Value = scaleNutriFact.Magnesium
        ManganeseNumericEditor.Value = scaleNutriFact.Manganese
        MolybdenumNumericEditor.Value = scaleNutriFact.Molybdenum
        NiacinNumericEditor.Value = scaleNutriFact.Niacin
        OtherCarbsNumericEditor.Value = scaleNutriFact.OtherCarbohydrates
        PantAcidNumericEditor.Value = scaleNutriFact.PantothenicAcid
        PerContainerTextBox.Text = scaleNutriFact.ServingPerContainer
        PhosphorousNumericEditor.Value = scaleNutriFact.Phosphorous
        PotassiumNumericEditor.Value = scaleNutriFact.PotassiumWeight
        PotassiumPercentNumericEditor.Value = scaleNutriFact.PotassiumPercent
        ProteinNumericEditor.Value = scaleNutriFact.ProteinWeight
        ProteinPercentNumericEditor.Value = scaleNutriFact.ProteinPercent
        RiboflavinNumericEditor.Value = scaleNutriFact.Riboflavin
        SeleniumNumericEditor.Value = scaleNutriFact.Selenium
        ServingUnitsNumericEditor.Value = scaleNutriFact.ServingUnits
        SizeNumericEditor.Value = scaleNutriFact.ServingsPerPortion
        SizeTextBox.Text = scaleNutriFact.ServingSizeDesc
        SizeWeightNumericEditor.Value = scaleNutriFact.SizeWeight
        SodiumNumericEditor.Value = scaleNutriFact.SodiumWeight
        SodiumPercentNumericEditor.Value = scaleNutriFact.SodiumPercent
        SolFiberNumericEditor.Value = scaleNutriFact.SolubleFiber
        StarchNumericEditor.Value = scaleNutriFact.Starch
        SugarAlcoholNumericEditor.Value = scaleNutriFact.SugarAlcohol
        SugarNumericEditor.Value = scaleNutriFact.Sugar
        ThiaminNumericEditor.Value = scaleNutriFact.Thiamin
        TransfatNumericEditor.Value = scaleNutriFact.Transfat
        VitaminANumericEditor.Value = scaleNutriFact.VitaminA
        VitaminB12NumericEditor.Value = scaleNutriFact.VitaminB12
        VitaminB6NumericEditor.Value = scaleNutriFact.VitaminB6
        VitaminCNumericEditor.Value = scaleNutriFact.VitaminC
        VitaminDNumericEditor.Value = scaleNutriFact.VitaminD
        VitaminENumericEditor.Value = scaleNutriFact.VitaminE
        VitaminKNumericEditor.Value = scaleNutriFact.VitaminK
        ZincNumericEditor.Value = scaleNutriFact.Zinc

        SetNutrifactsDataChanges(False)
    End Sub
    Private Sub ClearNutrifacts()
        If NutrifactCombo.Items.Count > 0 AndAlso NutrifactCombo.SelectedIndex <> -1 Then NutrifactCombo.SelectedIndex = -1
        NutrifactDescriptionTextbox.Text = String.Empty

        BetaCaroteneNumericEditor.Value = 0
        BiotinNumericEditor.Value = 0
        CalciumNumericEditor.Value = 0
        CalFatNumericEditor.Value = 0
        CalSatFatNumericEditor.Value = 0
        CalTotalNumericEditor.Value = 0
        CalTransFatNumericEditor.Value = 0
        CarbsNumericEditor.Value = 0
        CarbsPercentNumericEditor.Value = 0
        ChlorideNumericEditor.Value = 0
        CholesterolNumericEditor.Value = 0
        CholesterolPercentNumericEditor.Value = 0
        ChromiumNumericEditor.Value = 0
        CopperNumericEditor.Value = 0
        FatMonoNumericEditor.Value = 0
        FatOmega3NumericEditor.Value = 0
        FatOmega6NumericEditor.Value = 0
        FatPolyNumericEditor.Value = 0
        FatSatNumericEditor.Value = 0
        FatTransNumericEditor.Value = 0
        FatSatPercentNumericEditor.Value = 0
        FatTotalNumericEditor.Value = 0
        FatTotalPercentNumericEditor.Value = 0
        FiberNumericEditor.Value = 0
        FiberPercentNumericEditor.Value = 0
        FolateNumericEditor.Value = 0
        InsolFiberNumericEditor.Value = 0
        IodineNumericEditor.Value = 0
        IronNumericEditor.Value = 0
        If LabelFormatCombo.Items.Count > 0 Then LabelFormatCombo.SelectedValue = -1
        MagnesiumNumericEditor.Value = 0
        ManganeseNumericEditor.Value = 0
        MolybdenumNumericEditor.Value = 0
        NiacinNumericEditor.Value = 0
        OtherCarbsNumericEditor.Value = 0
        PantAcidNumericEditor.Value = 0
        PerContainerTextBox.Text = "VARIED"
        PhosphorousNumericEditor.Value = 0
        PotassiumNumericEditor.Value = 0
        PotassiumPercentNumericEditor.Value = 0
        ProteinNumericEditor.Value = 0
        ProteinPercentNumericEditor.Value = 0
        RiboflavinNumericEditor.Value = 0
        SeleniumNumericEditor.Value = 0
        ServingUnitsNumericEditor.Value = 1
        SizeNumericEditor.Value = 0
        SizeTextBox.Text = String.Empty
        SizeWeightNumericEditor.Value = 0
        SodiumNumericEditor.Value = 0
        SodiumPercentNumericEditor.Value = 0
        SolFiberNumericEditor.Value = 0
        StarchNumericEditor.Value = 0
        SugarAlcoholNumericEditor.Value = 0
        SugarNumericEditor.Value = 0
        ThiaminNumericEditor.Value = 0
        TransfatNumericEditor.Value = 0
        VitaminANumericEditor.Value = 0
        VitaminB12NumericEditor.Value = 0
        VitaminB6NumericEditor.Value = 0
        VitaminCNumericEditor.Value = 0
        VitaminDNumericEditor.Value = 0
        VitaminENumericEditor.Value = 0
        VitaminKNumericEditor.Value = 0
        ZincNumericEditor.Value = 0

        NutrifactDescriptionTextbox.Focus()

    End Sub

    Private Sub UpdateNutrifactsControls(ByVal IsReadOnly As Boolean)

        Dim ControlState As Boolean
        If IsReadOnly = True Then
            ControlState = False
        Else
            ControlState = True
        End If

        ButtonNutrifactRemove.Enabled = ControlState
        NutrifactCombo.Enabled = ControlState
        NutrifactDescriptionTextbox.Enabled = ControlState
        BetaCaroteneNumericEditor.Enabled = ControlState
        BiotinNumericEditor.Enabled = ControlState
        CalciumNumericEditor.Enabled = ControlState
        CalFatNumericEditor.Enabled = ControlState
        CalSatFatNumericEditor.Enabled = ControlState
        CalTotalNumericEditor.Enabled = ControlState
        CalTransFatNumericEditor.Enabled = ControlState
        CarbsNumericEditor.Enabled = ControlState
        CarbsPercentNumericEditor.Enabled = ControlState
        ChlorideNumericEditor.Enabled = ControlState
        CholesterolNumericEditor.Enabled = ControlState
        CholesterolPercentNumericEditor.Enabled = ControlState
        ChromiumNumericEditor.Enabled = ControlState
        CopperNumericEditor.Enabled = ControlState
        FatMonoNumericEditor.Enabled = ControlState
        FatOmega3NumericEditor.Enabled = ControlState
        FatOmega6NumericEditor.Enabled = ControlState
        FatPolyNumericEditor.Enabled = ControlState
        FatSatNumericEditor.Enabled = ControlState
        FatTransNumericEditor.Enabled = ControlState
        FatSatPercentNumericEditor.Enabled = ControlState
        FatTotalNumericEditor.Enabled = ControlState
        FatTotalPercentNumericEditor.Enabled = ControlState
        FiberNumericEditor.Enabled = ControlState
        FiberPercentNumericEditor.Enabled = ControlState
        FolateNumericEditor.Enabled = ControlState
        InsolFiberNumericEditor.Enabled = ControlState
        IodineNumericEditor.Enabled = ControlState
        IronNumericEditor.Enabled = ControlState
        LabelFormatCombo.Enabled = True
        MagnesiumNumericEditor.Enabled = ControlState
        ManganeseNumericEditor.Enabled = ControlState
        MolybdenumNumericEditor.Enabled = ControlState
        NiacinNumericEditor.Enabled = ControlState
        OtherCarbsNumericEditor.Enabled = ControlState
        PantAcidNumericEditor.Enabled = ControlState
        PerContainerTextBox.Enabled = ControlState
        PhosphorousNumericEditor.Enabled = ControlState
        PotassiumNumericEditor.Enabled = ControlState
        PotassiumPercentNumericEditor.Enabled = ControlState
        ProteinNumericEditor.Enabled = ControlState
        ProteinPercentNumericEditor.Enabled = ControlState
        RiboflavinNumericEditor.Enabled = ControlState
        SeleniumNumericEditor.Enabled = ControlState
        ServingUnitsNumericEditor.Enabled = ControlState
        SizeNumericEditor.Enabled = ControlState
        SizeTextBox.Enabled = ControlState
        SizeWeightNumericEditor.Enabled = ControlState
        SodiumNumericEditor.Enabled = ControlState
        SodiumPercentNumericEditor.Enabled = ControlState
        SolFiberNumericEditor.Enabled = ControlState
        StarchNumericEditor.Enabled = ControlState
        SugarAlcoholNumericEditor.Enabled = ControlState
        SugarNumericEditor.Enabled = ControlState
        ThiaminNumericEditor.Enabled = ControlState
        TransfatNumericEditor.Enabled = ControlState
        VitaminANumericEditor.Enabled = ControlState
        VitaminB12NumericEditor.Enabled = ControlState
        VitaminB6NumericEditor.Enabled = ControlState
        VitaminCNumericEditor.Enabled = ControlState
        VitaminDNumericEditor.Enabled = ControlState
        VitaminENumericEditor.Enabled = ControlState
        VitaminKNumericEditor.Enabled = ControlState
        ZincNumericEditor.Enabled = ControlState

        SetDataChanges(False)

    End Sub

    Private Sub NutrifactCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles NutrifactCombo.SelectedIndexChanged

        If NutrifactCombo.SelectedIndex > -1 Then
            Me.NutrifactsID = CType(NutrifactCombo.SelectedItem, ScaleNutrifactBO).ID
            PopulateNutrifactsControls(Me.NutrifactsID)
            SetNutrifactsDataChanges(True)
        End If

    End Sub

#Region "Nutrifacts Controls"

    Private Sub NutrifactDescriptionCombo_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles NutrifactCombo.SelectionChangeCommitted
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub NutrifactCombo_ValueMemberChanged(sender As Object, e As EventArgs) Handles NutrifactCombo.ValueMemberChanged
        SetDataChanges(True)
    End Sub

    Private Sub PerContainerTextBox_TextChanged(sender As Object, e As EventArgs) Handles PerContainerTextBox.TextChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub NutrifactDescriptionTextbox_TextChanged(sender As Object, e As EventArgs) Handles NutrifactDescriptionTextbox.TextChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub SizeNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles SizeNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub SizeWeightNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles SizeWeightNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub LabelFormatCombo_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles LabelFormatCombo.SelectionChangeCommitted
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub SizeTextBox_TextChanged(sender As Object, e As EventArgs) Handles SizeTextBox.TextChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub CalTotalNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles CalTotalNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub CalFatNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles CalFatNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub CalSatFatNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles CalSatFatNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub CalTransFatNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles CalTransFatNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub FatTotalNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles FatTotalNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub FatSatNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles FatSatNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub FatTransNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles FatTransNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub FatOmega3NumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles FatOmega3NumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub FatOmega6NumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles FatOmega6NumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub FatTotalPercentNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles FatTotalPercentNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub FatSatPercentNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles FatSatPercentNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub TransfatNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles TransfatNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub FatMonoNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles FatMonoNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub FatPolyNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles FatPolyNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub CholesterolNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles CholesterolNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub SodiumNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles SodiumNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub PotassiumNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles PotassiumNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub CarbsNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles CarbsNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub FiberNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles FiberNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub ProteinNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles ProteinNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub StarchNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles StarchNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub SolFiberNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles SolFiberNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub InsolFiberNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles InsolFiberNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub SugarNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles SugarNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub SugarAlcoholNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles SugarAlcoholNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub OtherCarbsNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles OtherCarbsNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub CholesterolPercentNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles CholesterolPercentNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub SodiumPercentNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles SodiumPercentNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub PotassiumPercentNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles PotassiumPercentNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub CarbsPercentNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles CarbsPercentNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub FiberPercentNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles FiberPercentNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub ProteinPercentNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles ProteinPercentNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub VitaminANumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles VitaminANumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub BetaCaroteneNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles BetaCaroteneNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub VitaminCNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles VitaminCNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub CalciumNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles CalciumNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub IronNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles IronNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub VitaminDNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles VitaminDNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub VitaminENumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles VitaminENumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub ThiaminNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles ThiaminNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub RiboflavinNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles RiboflavinNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub NiacinNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles NiacinNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub VitaminB6NumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles VitaminB6NumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub ChromiumNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles ChromiumNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub ManganeseNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles ManganeseNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub SeleniumNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles SeleniumNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub FolateNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles FolateNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub VitaminB12NumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles VitaminB12NumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub BiotinNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles BiotinNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub PantAcidNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles PantAcidNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub PhosphorousNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles PhosphorousNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub IodineNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles IodineNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub MagnesiumNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles MagnesiumNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub ZincNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles ZincNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub CopperNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles CopperNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub ChlorideNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles ChlorideNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub VitaminKNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles VitaminKNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub

    Private Sub MolybdenumNumericEditor_ValueChanged(sender As Object, e As EventArgs) Handles MolybdenumNumericEditor.ValueChanged
        SetNutrifactsDataChanges(True)
    End Sub
#End Region
    Private Function ApplyNutrifactsChanges() As Boolean
        Dim requiredMessage As StringBuilder = New StringBuilder()
        Dim isSuccessful As Boolean = False

        If ScaleDetailsBO.ItemScaleID < 1 Then
            MessageBox.Show("Scale record has not been created yet.  Please save data into the General tab first.", "Scale Details", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return isSuccessful
        End If

        If hasNutrifactsChanges = True AndAlso Me.NutrifactsID > 0 Then
            If NutrifactDescriptionTextbox.Text.Length = 0 Then
                requiredMessage.Append(String.Format(ResourcesIRMA.GetString("Required"), DescriptionLabel.Text.Replace(":", "")))
                requiredMessage.Append(vbLf)
            End If
            If LabelFormatCombo.SelectedIndex = -1 Then
                requiredMessage.Append(String.Format(ResourcesIRMA.GetString("Required"), LabelFormatLabel.Text.Replace(":", "")))
                requiredMessage.Append(vbLf)
            End If
            If requiredMessage.Length > 0 Then
                MsgBox(requiredMessage.ToString(), MsgBoxStyle.Critical, Me.Text)
                Exit Function
            End If

            Dim scaleNutrifact As New ScaleNutrifactBO()
            Dim alternateJurisdiction As Boolean = False
            PopulateNutrifactBO(scaleNutrifact)

            If ScaleNutrifactDAO.Save(scaleNutrifact, alternateJurisdiction) Then
                ' reload the combo
                Me.LoadNutrifacts(Me.NutrifactsID)
                ' select the most recently added/edited item
                If NutrifactCombo.Items.Count > 0 Then
                    Dim index As Integer
                    For Each item As ScaleNutrifactBO In NutrifactCombo.Items
                        If item.Description = scaleNutrifact.Description Then
                            NutrifactCombo.SelectedIndex = index
                            Exit For
                        End If
                        index = index + 1
                    Next
                End If

                If Me.LabelFormatCombo.Items.Count > 0 Then
                    Me.LabelFormatCombo.SelectedValue = scaleNutrifact.Scale_LabelFormat_ID
                End If

                isSuccessful = True
            Else
                ' Indicate that this is a duplicate
                MsgBox(String.Format(ResourcesIRMA.GetString("Duplicate"), NutrifactDescriptionTextbox.Text), MsgBoxStyle.Critical, Me.Text)
                ClearNutrifacts()
            End If

            scaleNutrifact = Nothing
            hasNutrifactsChanges = False
        End If

        Return isSuccessful
    End Function

    Private Function ApplyIngredientsChanges() As Boolean

        Dim IngredientsBO As New IngredientsBO()
        Dim isSuccessful As Boolean = False

        If ScaleDetailsBO.ItemScaleID < 1 Then
            MessageBox.Show("Scale record has not been created yet.  Please save data into the General tab first.", "Scale Details", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return isSuccessful
        End If

        If IsIngredientsInputValid() Then
            IngredientsBO.ID = Me.ScaleIngredientsID
            IngredientsBO.Ingredients = IngredientsTxt.Text.Trim()
            IngredientsBO.Description = ""
            IngredientsBO.LabelTypeID = 0

            If addIngredients = True Then
                ScaleIngredientsDAO.AddIngredientsToItem(ItemKey, IngredientsBO)
            Else
                ScaleIngredientsDAO.UpdateIngredients(IngredientsBO)
            End If

            isSuccessful = True
        Else
            MessageBox.Show("Description, Ingredients, and Label Type cannot be empty.")
        End If

        Return isSuccessful
    End Function

    Private Function ApplyAllergensChanges() As Boolean
        Dim AllergensBO As New AllergensBO()
        Dim isSuccessful As Boolean = False

        If ScaleDetailsBO.ItemScaleID < 1 Then
            MessageBox.Show("Scale record has not been created yet.  Please save data into the General tab first.", "Scale Details", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return isSuccessful
        End If

        If IsAllergensInputValid() Then
            AllergensBO.ID = Me.ScaleAllergensID
            AllergensBO.Allergens = AllergensTxt.Text.Trim()
            AllergensBO.Description = ""
            AllergensBO.LabelTypeID = 0

            If addAllergens = True Then
                ScaleAllergensDAO.AddAllergensToItem(ItemKey, AllergensBO)
            Else
                ScaleAllergensDAO.UpdateAllergens(AllergensBO)

            End If

            isSuccessful = True
        Else
            MessageBox.Show("Description, Allergens, and Label Type cannot be empty.")
        End If

        Return isSuccessful
    End Function
    Private Function ApplyStorageDataChanges() As Boolean
        Dim StorageDataBO As New StorageDataBO()
        Dim isSuccessful As Boolean = False

        If ScaleDetailsBO.ItemScaleID < 1 Then
            MessageBox.Show("Scale record has not been created yet.  Please save data into the General tab first.", "Scale Details", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return isSuccessful
        End If

        If IsStorageDataInputValid() Then
            StorageDataBO.ID = Me.ScaleStorageDataID
            StorageDataBO.StorageData = StorageDataTxt.Text.Trim()
            StorageDataBO.Description = txtDescription.Text

            If addStorageData = True Then
                ScaleStorageDataDAO.AddStorageDataToItem(ItemKey, StorageDataBO)
                StorageDataBO = ScaleStorageDataDAO.GetStorageDataByItem(glItemID)
                Me.ScaleStorageDataID = StorageDataBO.ID
                addStorageData = False
            Else
                ScaleStorageDataDAO.UpdateStorageData(StorageDataBO)
            End If

            isSuccessful = True
        Else
            MessageBox.Show("Storage Data cannot be empty.")
        End If

        Return isSuccessful
    End Function
    Private Sub PopulateNutrifactBO(ByRef scaleNutriFact As ScaleNutrifactBO)

        If NutrifactCombo.SelectedIndex > -1 Then scaleNutriFact.ID = CInt(NutrifactCombo.SelectedValue.ToString())
        scaleNutriFact.Betacarotene = CInt(BetaCaroteneNumericEditor.Value.ToString())
        scaleNutriFact.Biotin = CInt(BiotinNumericEditor.Value.ToString())
        scaleNutriFact.Calcium = CInt(CalciumNumericEditor.Value.ToString())
        scaleNutriFact.CaloriesFat = CInt(CalFatNumericEditor.Value.ToString())
        scaleNutriFact.CaloriesSaturatedFat = CInt(CalSatFatNumericEditor.Value.ToString())
        scaleNutriFact.Calories = CInt(CalTotalNumericEditor.Value.ToString())
        scaleNutriFact.CaloriesFromTransFat = CInt(CalTransFatNumericEditor.Value.ToString())
        scaleNutriFact.TotalCarbohydrateWeight = CarbsNumericEditor.Value.ToString()
        scaleNutriFact.TotalCarbohydratePercent = CInt(CarbsPercentNumericEditor.Value.ToString())
        scaleNutriFact.Chloride = CInt(ChlorideNumericEditor.Value.ToString())
        scaleNutriFact.CholesterolWeight = CholesterolNumericEditor.Value.ToString()
        scaleNutriFact.CholesterolPercent = CInt(CholesterolPercentNumericEditor.Value.ToString())
        scaleNutriFact.Chromium = CInt(ChromiumNumericEditor.Value.ToString())
        scaleNutriFact.Copper = CInt(CopperNumericEditor.Value.ToString())
        scaleNutriFact.Description = NutrifactDescriptionTextbox.Text
        scaleNutriFact.MonounsaturatedFat = CInt(FatMonoNumericEditor.Value.ToString())
        scaleNutriFact.Om3Fatty = FatOmega3NumericEditor.Value.ToString()
        scaleNutriFact.Om6Fatty = FatOmega6NumericEditor.Value.ToString()
        scaleNutriFact.PolyunsaturatedFat = CInt(FatPolyNumericEditor.Value.ToString())
        scaleNutriFact.SaturatedFatWeight = FatSatNumericEditor.Value.ToString()
        scaleNutriFact.SaturatedFatPercent = CInt(FatSatPercentNumericEditor.Value.ToString())
        scaleNutriFact.TotalFatWeight = FatTotalNumericEditor.Value.ToString()
        scaleNutriFact.TotalFatPercentage = CInt(FatTotalPercentNumericEditor.Value.ToString())
        scaleNutriFact.DietaryFiberWeight = FiberNumericEditor.Value.ToString()
        scaleNutriFact.DietaryFiberPercent = CInt(FiberPercentNumericEditor.Value.ToString())
        scaleNutriFact.Folate = CInt(FolateNumericEditor.Value.ToString())
        scaleNutriFact.InsolubleFiber = InsolFiberNumericEditor.Value.ToString()
        scaleNutriFact.Iodine = CInt(IodineNumericEditor.Value.ToString())
        scaleNutriFact.Iron = CInt(IronNumericEditor.Value.ToString())
        scaleNutriFact.Scale_LabelFormat_ID = CInt(LabelFormatCombo.SelectedValue.ToString())
        scaleNutriFact.Magnesium = CInt(MagnesiumNumericEditor.Value.ToString())
        scaleNutriFact.Manganese = CInt(ManganeseNumericEditor.Value.ToString())
        scaleNutriFact.Molybdenum = CInt(MolybdenumNumericEditor.Value.ToString())
        scaleNutriFact.Niacin = CInt(NiacinNumericEditor.Value.ToString())
        scaleNutriFact.OtherCarbohydrates = OtherCarbsNumericEditor.Value.ToString()
        scaleNutriFact.PantothenicAcid = CInt(PantAcidNumericEditor.Value.ToString())
        scaleNutriFact.ServingPerContainer = PerContainerTextBox.Text
        scaleNutriFact.Phosphorous = CInt(PhosphorousNumericEditor.Value.ToString())
        scaleNutriFact.PotassiumWeight = PotassiumNumericEditor.Value.ToString()
        scaleNutriFact.PotassiumPercent = CInt(PotassiumPercentNumericEditor.Value.ToString())
        scaleNutriFact.ProteinWeight = ProteinNumericEditor.Value.ToString()
        scaleNutriFact.ProteinPercent = CInt(ProteinPercentNumericEditor.Value.ToString())
        scaleNutriFact.Riboflavin = CInt(RiboflavinNumericEditor.Value.ToString())
        scaleNutriFact.Selenium = CInt(SeleniumNumericEditor.Value.ToString())
        scaleNutriFact.ServingUnits = CInt(ServingUnitsNumericEditor.Value.ToString())
        scaleNutriFact.ServingsPerPortion = SizeNumericEditor.Value
        scaleNutriFact.ServingSizeDesc = SizeTextBox.Text
        scaleNutriFact.SizeWeight = CInt(SizeWeightNumericEditor.Value.ToString())
        scaleNutriFact.SodiumWeight = SodiumNumericEditor.Value.ToString()
        scaleNutriFact.SodiumPercent = CInt(SodiumPercentNumericEditor.Value.ToString())
        scaleNutriFact.SolubleFiber = SolFiberNumericEditor.Value.ToString()
        scaleNutriFact.Starch = StarchNumericEditor.Value.ToString()
        scaleNutriFact.SugarAlcohol = SugarAlcoholNumericEditor.Value.ToString()
        scaleNutriFact.Sugar = SugarNumericEditor.Value.ToString()
        scaleNutriFact.Thiamin = CInt(ThiaminNumericEditor.Value.ToString())
        scaleNutriFact.Transfat = CInt(TransfatNumericEditor.Value.ToString())
        scaleNutriFact.TransfatWeight = FatTransNumericEditor.Value
        scaleNutriFact.VitaminA = CInt(VitaminANumericEditor.Value.ToString())
        scaleNutriFact.VitaminB12 = CInt(VitaminB12NumericEditor.Value.ToString())
        scaleNutriFact.VitaminB6 = CInt(VitaminB6NumericEditor.Value.ToString())
        scaleNutriFact.VitaminC = CInt(VitaminCNumericEditor.Value.ToString())
        scaleNutriFact.VitaminD = CInt(VitaminDNumericEditor.Value.ToString())
        scaleNutriFact.VitaminE = CInt(VitaminENumericEditor.Value.ToString())
        scaleNutriFact.VitaminK = CInt(VitaminKNumericEditor.Value.ToString())
        scaleNutriFact.Zinc = CInt(ZincNumericEditor.Value.ToString())

    End Sub

    Private Sub DescriptionTxt_TextChanged(sender As Object, e As EventArgs)
        SetIngredientsDataChanges(True)
    End Sub

    Private Sub LabelTypeCbx_SelectionChangeCommitted(sender As Object, e As EventArgs)
        SetIngredientsDataChanges(True)
    End Sub

    Private Sub IngredientsTxt_TextChanged(sender As Object, e As EventArgs) Handles IngredientsTxt.TextChanged
        SetIngredientsDataChanges(True)
    End Sub

    Private Function IsIngredientsInputValid() As Boolean
        Return Not String.IsNullOrWhiteSpace(IngredientsTxt.Text)
    End Function

    Private Function IsAllergensInputValid() As Boolean
        Return Not String.IsNullOrWhiteSpace(AllergensTxt.Text)
    End Function

    Private Function IsStorageDataInputValid() As Boolean
        Return Not String.IsNullOrWhiteSpace(StorageDataTxt.Text)
    End Function

    Private Sub AllergensDescriptionTxt_TextChanged(sender As Object, e As EventArgs)
        SetAllergensDataChanges(True)
    End Sub

    Private Sub AllergensLabelTypeCbx_SelectionChangeCommitted(sender As Object, e As EventArgs)
        SetAllergensDataChanges(True)
    End Sub

    Private Sub AllergensTxt_TextChanged(sender As Object, e As EventArgs) Handles AllergensTxt.TextChanged
        SetAllergensDataChanges(True)
    End Sub

    Private Sub StorageDataTxt_TextChanged(sender As Object, e As EventArgs) Handles StorageDataTxt.TextChanged
        SetStorageDataChanges(True)
    End Sub

    Private Sub ScaleDesc1TextBox_TextChanged(sender As Object, e As EventArgs) Handles ScaleDesc1TextBox.TextChanged
        SetDataChanges(True)
    End Sub

    Private Sub ScaleDesc2TextBox_TextChanged(sender As Object, e As EventArgs) Handles ScaleDesc2TextBox.TextChanged
        SetDataChanges(True)
    End Sub

    Private Sub ScaleDesc3TextBox_TextChanged(sender As Object, e As EventArgs) Handles ScaleDesc3TextBox.TextChanged
        SetDataChanges(True)
    End Sub

    Private Sub ScaleDesc4TextBox_TextChanged(sender As Object, e As EventArgs) Handles ScaleDesc4TextBox.TextChanged
        SetDataChanges(True)
    End Sub

    Private Sub TareCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TareCombo.SelectedValueChanged
        SetDataChanges(True)
    End Sub

    Private Sub AltTareCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles AltTareCombo.SelectedIndexChanged
        SetDataChanges(True)
    End Sub

    Private Sub LabelStyleCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LabelStyleCombo.SelectedIndexChanged
        SetDataChanges(True)
    End Sub
    Private Sub EatByCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles EatByCombo.SelectedIndexChanged
        SetDataChanges(True)
    End Sub

    Private Sub GradeCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GradeCombo.SelectedIndexChanged
        SetDataChanges(True)
    End Sub

    Private Sub ShelfLifeNumericEditor_ValueChanged_1(sender As Object, e As EventArgs) Handles ShelfLifeNumericEditor.ValueChanged
        SetDataChanges(True)
    End Sub

    Private Sub ForceTareCheckBox_CheckStateChanged(sender As Object, e As EventArgs) Handles ForceTareCheckBox.CheckStateChanged
        SetDataChanges(True)
    End Sub

    Private Sub ExtraTextButton_Click_1(sender As Object, e As EventArgs) Handles ExtraTextButton.Click
        logger.Debug("ExtraTextButton_Click Entry")

        Me.extraTextForm = New ExtraTextEdit

        Dim _extraTextBO As New ScaleExtraTextBO
        _extraTextBO = ScaleExtraTextDAO.GetExtraTextByItem(glItemID, Nothing)

        Me.extraTextForm.ItemKey = Me.ScaleDetailsBO.ItemKey
        Me.extraTextForm.CurrentExtraTextBO = _extraTextBO

        Me.extraTextForm.ShowDialog(Me)

        'update the Save Changes button if update was made
        If Me.extraTextForm.CurrentExtraTextBO.ID > 0 AndAlso Me.ScaleDetailsBO.ExtraTextID <> Me.extraTextForm.CurrentExtraTextBO.ID Then
            SetDataChanges(True)
        End If

        ' update the object
        Me.ScaleDetailsBO.ExtraTextID = Me.extraTextForm.CurrentExtraTextBO.ID
        Me.ScaleDetailsBO.ExtraText = Me.extraTextForm.CurrentExtraTextBO.ExtraText

        ' update the display
        ExtraTextValueLabel.Tag = Me.extraTextForm.CurrentExtraTextBO.ID
        ExtraTextValueLabel.Text = Me.extraTextForm.CurrentExtraTextBO.ExtraText

        extraTextForm.Close()
        extraTextForm.Dispose()

        logger.Debug("ExtraTextButton_Click Exit")
    End Sub

    Private Sub ExtraTextSearchButton_Click_1(sender As Object, e As EventArgs) Handles ExtraTextSearchButton.Click
        logger.Debug("ExtraTextSearchButton_Click Entry")

        Dim itemSearch As New frmItemSearch
        Dim extraTextBO As New ScaleExtraTextBO
        Dim currentItemID As Integer

        currentItemID = glItemID

        itemSearch.ShowDialog()
        itemSearch.Close()
        itemSearch.Dispose()

        ' look up the scale_extratext record for the selected item
        ' set combo to match this one
        extraTextBO = ScaleExtraTextDAO.GetExtraTextByItem(glItemID, Nothing)
        glItemID = currentItemID

        If extraTextBO.ID = -1 Then
            Exit Sub
        ElseIf extraTextBO.ID = 0 Then
            ' warn the user that this item has no nutrifact record associated with it
            MsgBox(ResourcesItemHosting.GetString("NoScaleExtraText"), MsgBoxStyle.Critical, Me.Text)
            logger.Info(ResourcesItemHosting.GetString("NoScaleExtraText"))
        Else
            Me.ScaleDetailsBO.ExtraText = extraTextBO.ExtraText
            Me.ScaleDetailsBO.ExtraTextID = extraTextBO.ID
            Me.ScaleDetailsBO.Description = extraTextBO.Description
            ExtraTextValueLabel.Tag = extraTextBO.ID
            ExtraTextValueLabel.Text = Me.ScaleDetailsBO.ExtraText

            SetDataChanges(True)

        End If

        logger.Debug("ExtraTextSearchButton_Click Exit")
    End Sub

    Private Sub ButtonNutrifactRemove_Click(sender As Object, e As EventArgs) Handles ButtonNutrifactRemove.Click
        ' Clear form for UX purposes.  The ClearNutrifacts method also sets the Nutrifact combo to -1
        If Me.NutrifactCombo.SelectedIndex >= 0 Then
            ClearNutrifacts()
        End If
    End Sub
End Class