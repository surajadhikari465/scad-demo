Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports System.Text
Imports WholeFoods.IRMA.Common.DataAccess
Imports Infragistics.Win.UltraWinEditors

Public Class Form_Nutrifact
    Private hasChanged As Boolean
    Private isLoading As Boolean
    Private _nutrifactID As Integer
    Private formIsReadOnly As Boolean

#Region "Properties"

    Public Property NutrifactID() As Integer
        Get
            Return _nutrifactID
        End Get
        Set(ByVal value As Integer)
            _nutrifactID = value
        End Set
    End Property

    Private _forAlternateJurisdiction As Boolean
    Public Property ForAlternateJurisdiction() As Boolean
        Get
            Return _forAlternateJurisdiction
        End Get
        Set(ByVal value As Boolean)
            _forAlternateJurisdiction = value
        End Set
    End Property

#End Region

#Region "Methods"

    Private Sub LoadCombo()
        LabelFormatCombo.DataSource = ScaleLabelFormatDAO.GetComboList()
        If LabelFormatCombo.Items.Count > 0 Then
            LabelFormatCombo.DisplayMember = "Description"
            LabelFormatCombo.ValueMember = "ID"
            LabelFormatCombo.SelectedIndex = -1
        End If

        isLoading = True
        DescriptionCombo.DataSource = ScaleNutrifactDAO.GetNutrifactComboList()

        If DescriptionCombo.Items.Count > 0 Then

            Dim scaleNutrifact As New ScaleNutrifactBO()
            scaleNutrifact = CType(DescriptionCombo.SelectedItem, ScaleNutrifactBO)

            DescriptionCombo.DisplayMember = "Description"
            DescriptionCombo.ValueMember = "ID"
            isLoading = False

            If NutrifactID > 0 Then
                ' load with matching record from calling screen
                DescriptionCombo.SelectedValue = NutrifactID
                PopulateControls(CInt(DescriptionCombo.SelectedValue.ToString()))
            Else
                Clear()
            End If

        Else
            Clear()
            isLoading = False

        End If
    End Sub

    Private Sub Clear()
        If DescriptionCombo.Items.Count > 0 Then DescriptionCombo.SelectedIndex = -1
        DescriptionTextbox.Text = String.Empty

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

        DescriptionTextbox.Focus()
        hasChanged = False

    End Sub

    Private Sub ApplyChanges()
        Dim requiredMessage As StringBuilder = New StringBuilder()

        If hasChanged = True Then
            If DescriptionTextbox.Text.Length = 0 Then
                requiredMessage.Append(String.Format(ResourcesIRMA.GetString("Required"), DescriptionLabel.Text.Replace(":", "")))
                requiredMessage.Append(vbLf)
            End If
            If LabelFormatCombo.SelectedIndex = -1 Then
                requiredMessage.Append(String.Format(ResourcesIRMA.GetString("Required"), LabelFormatLabel.Text.Replace(":", "")))
                requiredMessage.Append(vbLf)
            End If
            If requiredMessage.Length > 0 Then
                MsgBox(requiredMessage.ToString(), MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If

            Dim scaleNutrifact As New ScaleNutrifactBO()
            Dim alternateJurisdiction As Boolean = True
            PopulateBO(scaleNutrifact)

            If ScaleNutrifactDAO.Save(scaleNutrifact, alternateJurisdiction) Then
                ' reload the combo
                LoadCombo()
                ' select the most recently added/edited item
                If DescriptionCombo.Items.Count > 0 Then
                    Dim index As Integer
                    For Each item As ScaleNutrifactBO In DescriptionCombo.Items
                        If item.Description = scaleNutrifact.Description Then
                            DescriptionCombo.SelectedIndex = index
                            Exit For
                        End If
                        index = index + 1
                    Next
                End If

                If Me.LabelFormatCombo.Items.Count > 0 Then
                    Me.LabelFormatCombo.SelectedValue = scaleNutrifact.Scale_LabelFormat_ID
                End If

                MsgBox("Changes have been applied successfully.", MsgBoxStyle.Information, Me.Text)

            Else
                ' Indicate that this is a duplicate
                MsgBox(String.Format(ResourcesIRMA.GetString("Duplicate"), DescriptionTextbox.Text), MsgBoxStyle.Critical, Me.Text)
                Clear()
            End If

            scaleNutrifact = Nothing
            hasChanged = False
        End If
    End Sub
    Private Sub PopulateControls(ByVal nutrifact_ID As Integer)
        If isLoading Then Exit Sub

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
        If DescriptionCombo.Items.Count > 0 Then DescriptionCombo.SelectedValue = scaleNutriFact.ID
        DescriptionTextbox.Text = scaleNutriFact.Description
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


    End Sub

    Private Sub PopulateBO(ByRef scaleNutriFact As ScaleNutrifactBO)

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
        If DescriptionCombo.SelectedIndex > -1 Then scaleNutriFact.ID = CInt(DescriptionCombo.SelectedValue.ToString())
        scaleNutriFact.Description = DescriptionTextbox.Text
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
#End Region

#Region "Form/Button Events"

    Private Sub Form_Nutrifact_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If hasChanged Then
            'prompt the user to save changes
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                ApplyChanges()
            End If
        End If

    End Sub

    Private Sub Form_Nutrifact_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadCombo()

        formIsReadOnly = InstanceDataDAO.IsFlagActive("EnableNutrifactIntegration")

        If formIsReadOnly And Not ForAlternateJurisdiction Then
            LockForm()
        End If

        hasChanged = False
    End Sub

    Private Sub ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyButton.Click
        ApplyChanges()
    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click
        If hasChanged Then
            'prompt the user to save changes
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                ApplyChanges()
            End If
        End If
        Clear()
    End Sub

    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseButton.Click
        Me.Close()
    End Sub

#End Region

#Region "Control Events"

    Private Sub DescriptionCombo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DescriptionCombo.SelectedIndexChanged
        Dim nutrifactID As Integer = 0

        If DescriptionCombo.SelectedIndex > -1 Then
            nutrifactID = CType(DescriptionCombo.SelectedItem, ScaleNutrifactBO).ID
            PopulateControls(nutrifactID)
        Else
            Clear()
        End If

        hasChanged = True
    End Sub

    Private Sub LabelFormatCombo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelFormatCombo.SelectedIndexChanged
        hasChanged = True
    End Sub

    Private Sub DescriptionTextbox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DescriptionTextbox.TextChanged
        hasChanged = True
    End Sub

    Private Sub SizeTextbox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SizeTextBox.TextChanged
        hasChanged = True
    End Sub

    Private Sub PerContainerTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PerContainerTextBox.TextChanged
        hasChanged = True
    End Sub

    Private Sub NumericEditor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PantAcidNumericEditor.Click, ZincNumericEditor.Click, VitaminKNumericEditor.Click, VitaminENumericEditor.Click, VitaminDNumericEditor.Click, VitaminCNumericEditor.Click, VitaminB6NumericEditor.Click, VitaminB12NumericEditor.Click, VitaminANumericEditor.Click, TransfatNumericEditor.Click, ThiaminNumericEditor.Click, SugarNumericEditor.Click, SugarAlcoholNumericEditor.Click, StarchNumericEditor.Click, SolFiberNumericEditor.Click, SodiumPercentNumericEditor.Click, SodiumNumericEditor.Click, SizeWeightNumericEditor.Click, SizeNumericEditor.Click, ServingUnitsNumericEditor.Click, SeleniumNumericEditor.Click, RiboflavinNumericEditor.Click, ProteinPercentNumericEditor.Click, ProteinNumericEditor.Click, PotassiumPercentNumericEditor.Click, PotassiumNumericEditor.Click, PhosphorousNumericEditor.Click, OtherCarbsNumericEditor.Click, NiacinNumericEditor.Click, MolybdenumNumericEditor.Click, ManganeseNumericEditor.Click, MagnesiumNumericEditor.Click, IronNumericEditor.Click, IodineNumericEditor.Click, InsolFiberNumericEditor.Click, FolateNumericEditor.Click, FiberPercentNumericEditor.Click, FiberNumericEditor.Click, FatTotalPercentNumericEditor.Click, FatTotalNumericEditor.Click, FatSatPercentNumericEditor.Click, FatSatNumericEditor.Click, FatPolyNumericEditor.Click, FatOmega6NumericEditor.Click, FatOmega3NumericEditor.Click, FatMonoNumericEditor.Click, CopperNumericEditor.Click, ChromiumNumericEditor.Click, CholesterolPercentNumericEditor.Click, CholesterolNumericEditor.Click, ChlorideNumericEditor.Click, CarbsPercentNumericEditor.Click, CarbsNumericEditor.Click, CalTransFatNumericEditor.Click, CalTotalNumericEditor.Click, CalSatFatNumericEditor.Click, CalFatNumericEditor.Click, CalciumNumericEditor.Click, BiotinNumericEditor.Click, BetaCaroteneNumericEditor.Click, FatTransNumericEditor.Click
        CType(sender, Infragistics.Win.UltraWinEditors.UltraNumericEditor).SelectAll()
    End Sub
    Private Sub NumericEditor_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles PantAcidNumericEditor.Enter, ZincNumericEditor.Enter, VitaminKNumericEditor.Enter, VitaminENumericEditor.Enter, VitaminDNumericEditor.Enter, VitaminCNumericEditor.Enter, VitaminB6NumericEditor.Enter, VitaminB12NumericEditor.Enter, VitaminANumericEditor.Enter, TransfatNumericEditor.Enter, ThiaminNumericEditor.Enter, SugarNumericEditor.Enter, SugarAlcoholNumericEditor.Enter, StarchNumericEditor.Enter, SolFiberNumericEditor.Enter, SodiumPercentNumericEditor.Enter, SodiumNumericEditor.Enter, SizeWeightNumericEditor.Enter, SizeNumericEditor.Enter, ServingUnitsNumericEditor.Enter, SeleniumNumericEditor.Enter, RiboflavinNumericEditor.Enter, ProteinPercentNumericEditor.Enter, ProteinNumericEditor.Enter, PotassiumPercentNumericEditor.Enter, PotassiumNumericEditor.Enter, PhosphorousNumericEditor.Enter, OtherCarbsNumericEditor.Enter, NiacinNumericEditor.Enter, MolybdenumNumericEditor.Enter, ManganeseNumericEditor.Enter, MagnesiumNumericEditor.Enter, IronNumericEditor.Enter, IodineNumericEditor.Enter, InsolFiberNumericEditor.Enter, FolateNumericEditor.Enter, FiberPercentNumericEditor.Enter, FiberNumericEditor.Enter, FatTotalPercentNumericEditor.Enter, FatTotalNumericEditor.Enter, FatSatPercentNumericEditor.Enter, FatSatNumericEditor.Enter, FatPolyNumericEditor.Enter, FatOmega6NumericEditor.Enter, FatOmega3NumericEditor.Enter, FatMonoNumericEditor.Enter, CopperNumericEditor.Enter, ChromiumNumericEditor.Enter, CholesterolPercentNumericEditor.Enter, CholesterolNumericEditor.Enter, ChlorideNumericEditor.Enter, CarbsPercentNumericEditor.Enter, CarbsNumericEditor.Enter, CalTransFatNumericEditor.Enter, CalTotalNumericEditor.Enter, CalSatFatNumericEditor.Enter, CalFatNumericEditor.Enter, CalciumNumericEditor.Enter, BiotinNumericEditor.Enter, BetaCaroteneNumericEditor.Enter, FatTransNumericEditor.Enter
        CType(sender, Infragistics.Win.UltraWinEditors.UltraNumericEditor).SelectAll()
    End Sub
    Private Sub NumericEditor_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PantAcidNumericEditor.ValueChanged, ZincNumericEditor.ValueChanged, VitaminKNumericEditor.ValueChanged, VitaminENumericEditor.ValueChanged, VitaminDNumericEditor.ValueChanged, VitaminCNumericEditor.ValueChanged, VitaminB6NumericEditor.ValueChanged, VitaminB12NumericEditor.ValueChanged, VitaminANumericEditor.ValueChanged, TransfatNumericEditor.ValueChanged, ThiaminNumericEditor.ValueChanged, SugarNumericEditor.ValueChanged, SugarAlcoholNumericEditor.ValueChanged, StarchNumericEditor.ValueChanged, SolFiberNumericEditor.ValueChanged, SodiumPercentNumericEditor.ValueChanged, SodiumNumericEditor.ValueChanged, SizeWeightNumericEditor.ValueChanged, SizeNumericEditor.ValueChanged, ServingUnitsNumericEditor.ValueChanged, SeleniumNumericEditor.ValueChanged, RiboflavinNumericEditor.ValueChanged, ProteinPercentNumericEditor.ValueChanged, ProteinNumericEditor.ValueChanged, PotassiumPercentNumericEditor.ValueChanged, PotassiumNumericEditor.ValueChanged, PhosphorousNumericEditor.ValueChanged, OtherCarbsNumericEditor.ValueChanged, NiacinNumericEditor.ValueChanged, MolybdenumNumericEditor.ValueChanged, ManganeseNumericEditor.ValueChanged, MagnesiumNumericEditor.ValueChanged, IronNumericEditor.ValueChanged, IodineNumericEditor.ValueChanged, InsolFiberNumericEditor.ValueChanged, FolateNumericEditor.ValueChanged, FiberPercentNumericEditor.ValueChanged, FiberNumericEditor.ValueChanged, FatTotalPercentNumericEditor.ValueChanged, FatTotalNumericEditor.ValueChanged, FatSatPercentNumericEditor.ValueChanged, FatSatNumericEditor.ValueChanged, FatPolyNumericEditor.ValueChanged, FatOmega6NumericEditor.ValueChanged, FatOmega3NumericEditor.ValueChanged, FatMonoNumericEditor.ValueChanged, CopperNumericEditor.ValueChanged, ChromiumNumericEditor.ValueChanged, CholesterolPercentNumericEditor.ValueChanged, CholesterolNumericEditor.ValueChanged, ChlorideNumericEditor.ValueChanged, CarbsPercentNumericEditor.ValueChanged, CarbsNumericEditor.ValueChanged, CalTransFatNumericEditor.ValueChanged, CalTotalNumericEditor.ValueChanged, CalSatFatNumericEditor.ValueChanged, CalFatNumericEditor.ValueChanged, CalciumNumericEditor.ValueChanged, BiotinNumericEditor.ValueChanged, BetaCaroteneNumericEditor.ValueChanged, FatTransNumericEditor.ValueChanged
        hasChanged = True
    End Sub

#End Region

    Private Sub LockForm()
        For Each control As Control In Me.Controls
            If control.GetType() = GetType(GroupBox) Then
                For Each groupBoxControl As Control In control.Controls
                    If groupBoxControl.GetType() = GetType(UltraNumericEditor) Then
                        Dim ultraNumericEditor As UltraNumericEditor = CType(groupBoxControl, UltraNumericEditor)
                        ultraNumericEditor.ReadOnly = True
                    End If
                Next
            End If
        Next

        Me.ApplyButton.Enabled = True
        Me.AddButton.Enabled = False

        Me.DescriptionTextbox.ReadOnly = True
        Me.LabelFormatCombo.Enabled = True
        Me.ServingUnitsNumericEditor.ReadOnly = True
        Me.PerContainerTextBox.ReadOnly = True
        Me.SizeNumericEditor.ReadOnly = True
        Me.SizeWeightNumericEditor.ReadOnly = True
        Me.SizeTextBox.ReadOnly = True
    End Sub

End Class