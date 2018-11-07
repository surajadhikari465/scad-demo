Option Strict Off
Option Explicit On

Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports System.Linq
Imports WholeFoods.IRMA.Common.DataAccess

Public Class Form_ItemScaleDetailsOverride
    ' Flag set when the form is initializing so the on change events are not processed.
    Private IsInitializing As Boolean

    ' Set the permissions flag.
    Private IsEditable As Boolean

    ' Stores the item-scale data for the default jurisdiction.
    Private _defaultScaleData As ScaleDetailsBO

    ' Stores the item-scale data for the currently selected override jurisdiction.
    Private _overrideScaleData As ScaleDetailsBO

    ' Flag to track changes to the data.
    Private _dataChanges As Boolean

    ' Form to manage the extra text data.
    Private extraTextForm As ExtraTextEdit
    Private _scaleBO As ScaleExtraTextBO

    ' Form to add/edit nutrifact information.
    Private nutrifactForm As Form_Nutrifact

    Private nutrifactsAreReadyOnly As Boolean

#Region "Form Load"
    Private Sub Form_ItemScaleDetailsOverride_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Initialize the override object and populate the data for the combo boxes
        _overrideScaleData = New ScaleDetailsBO
        ScaleDetailsDAO.GetScaleDetailCombos(_overrideScaleData)

        IsInitializing = True

        ' Set the item identifier
        TextBox_Identifier.Text = _defaultScaleData.ItemIdentifier


        Try
            ' Populate the combo boxes
            Dim storeJurisdiction As New StoreJurisdictionDAO
            Dim jurisdictionList As New ArrayList
            jurisdictionList = storeJurisdiction.GetJurisdictionList(CInt(_defaultScaleData.StoreJurisdictionID))

            If jurisdictionList.Count > 0 Then
                ComboBox_AltJurisdiction.DataSource = jurisdictionList
                ComboBox_AltJurisdiction.DisplayMember = "StoreJurisdictionDesc"
                ComboBox_AltJurisdiction.ValueMember = "StoreJurisdictionID"
            End If
            

            If _overrideScaleData.ScaleTareList.Count > 0 Then
                ComboBox_Tare.DataSource = _overrideScaleData.ScaleTareList
                ComboBox_Tare.DisplayMember = "Description"
                ComboBox_Tare.ValueMember = "ID"
                ComboBox_Tare.SelectedIndex = -1
            End If

            If _overrideScaleData.ScaleNutrifactList.Count > 0 Then
                ComboBoxNutrifact.DataSource = _overrideScaleData.ScaleNutrifactList
                ComboBoxNutrifact.DisplayMember = "Description"
                ComboBoxNutrifact.ValueMember = "ID"
                ComboBoxNutrifact.SelectedIndex = -1
            End If

            If _overrideScaleData.ScaleTareAlternateList.Count > 0 Then
                ComboBoxAlternateTare.DataSource = _overrideScaleData.ScaleTareAlternateList
                ComboBoxAlternateTare.DisplayMember = "Description"
                ComboBoxAlternateTare.ValueMember = "ID"
                ComboBoxAlternateTare.SelectedIndex = -1
            End If

            If _overrideScaleData.ScaleLabelStyleList.Count > 0 Then
                ComboBox_LabelStyle.DataSource = _overrideScaleData.ScaleLabelStyleList
                ComboBox_LabelStyle.DisplayMember = "Description"
                ComboBox_LabelStyle.ValueMember = "ID"
                ComboBox_LabelStyle.SelectedIndex = -1
            End If

            If _overrideScaleData.ScaleRandomWeightTypeList.Count > 0 Then
                RandomWeightCombo.DataSource = _overrideScaleData.ScaleRandomWeightTypeList
                RandomWeightCombo.DisplayMember = "Description"
                RandomWeightCombo.ValueMember = "ID"
                RandomWeightCombo.SelectedIndex = -1
            End If

            If _overrideScaleData.ScaleUOMList.Count > 0 Then
                ComboBox_ScaleUOM.DataSource = _overrideScaleData.ScaleUOMList
                ComboBox_ScaleUOM.DisplayMember = "Description"
                ComboBox_ScaleUOM.ValueMember = "ID"
                ComboBox_ScaleUOM.SelectedIndex = -1
            End If

            If _overrideScaleData.ScaleEatByList.Count > 0 Then
                ComboBoxEatBy.DataSource = _overrideScaleData.ScaleEatByList
                ComboBoxEatBy.DisplayMember = "Description"
                ComboBoxEatBy.ValueMember = "ID"
                ComboBoxEatBy.SelectedIndex = -1
            End If

            If _overrideScaleData.ScaleGradeList.Count > 0 Then
                ComboBoxGrade.DataSource = _overrideScaleData.ScaleGradeList
                ComboBoxGrade.DisplayMember = "Description"
                ComboBoxGrade.ValueMember = "ID"
                ComboBoxGrade.SelectedIndex = -1
            End If

            IsInitializing = False

            ' Pre-fill the jurisdiction data, if it exists
            PopulateScaleData(ComboBox_AltJurisdiction.SelectedValue)

            SetPermissions()

            If gbItemAdministrator Or gbSuperUser Then
                EnableCountAndWeightBoxes()
            End If

        Catch ex As Exception
            MessageBox.Show("Error occurred while loading the form: " & ex.Message)
        End Try
        

    End Sub
#End Region

#Region "Property accessors"
    Public Property DefaultScaleData() As ScaleDetailsBO
        Get
            Return _defaultScaleData
        End Get
        Set(ByVal value As ScaleDetailsBO)
            _defaultScaleData = value
        End Set
    End Property
#End Region

    ''' <summary>
    ''' Lookup the override values in the database for the selected jurisdiction and
    ''' display them to the user.
    ''' </summary>
    ''' <param name="overrideJurisdictionId"></param>
    ''' <remarks></remarks>
    Private Sub PopulateScaleData(ByVal overrideJurisdictionId As Integer)
        ' Initialize the override object for the scale data
        _overrideScaleData = New ScaleDetailsBO

        ' Populate the item key and jurisdiction id for the object
        _overrideScaleData.ItemKey = _defaultScaleData.ItemKey
        _overrideScaleData.ItemIdentifier = _defaultScaleData.ItemIdentifier
        _overrideScaleData.StoreJurisdictionID = ComboBox_AltJurisdiction.SelectedValue

        ' Read all of the override data for the item and finish populating the override BOs
        Dim results As SqlDataReader = StoreJurisdictionDAO.GetStoreScaleOverrideData(_defaultScaleData.ItemKey, ComboBox_AltJurisdiction.SelectedValue)

        If Not results.HasRows Then
            ' This is a new record for this jurisdiction.  Populate ID values with -1.  Otherwise, .NET will default these values to 0, which is a valid key and does
            ' not represent the absence of a value.
            _overrideScaleData.ExtraTextID = -1
            _overrideScaleData.Tare = -1
            _overrideScaleData.LabelStyle = -1
            _overrideScaleData.UOM = -1
            _overrideScaleData.RandomWeightType = -1
            _overrideScaleData.TareAlternate = -1
            _overrideScaleData.EatBy = -1
            _overrideScaleData.Grade = -1
            _overrideScaleData.Nutrifact = -1
            ShelfLifeNumericEditor.Value = Nothing
        End If

        While results.Read
            If (Not results.IsDBNull(results.GetOrdinal("Scale_Description1"))) Then
                _overrideScaleData.ScaleDescription1 = results.GetString(results.GetOrdinal("Scale_Description1"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Scale_Description2"))) Then
                _overrideScaleData.ScaleDescription2 = results.GetString(results.GetOrdinal("Scale_Description2"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Scale_Description3"))) Then
                _overrideScaleData.ScaleDescription3 = results.GetString(results.GetOrdinal("Scale_Description3"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Scale_Description4"))) Then
                _overrideScaleData.ScaleDescription4 = results.GetString(results.GetOrdinal("Scale_Description4"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Scale_ExtraText_ID"))) Then
                _overrideScaleData.ExtraTextID = results.GetInt32(results.GetOrdinal("Scale_ExtraText_ID"))
            Else
                _overrideScaleData.ExtraTextID = -1
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Scale_ExtraText"))) Then
                _overrideScaleData.ExtraText = results.GetString(results.GetOrdinal("Scale_ExtraText"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Scale_Tare_ID"))) Then
                _overrideScaleData.Tare = results.GetInt32(results.GetOrdinal("Scale_Tare_ID"))
            Else
                _overrideScaleData.Tare = -1
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Scale_LabelStyle_ID"))) Then
                _overrideScaleData.LabelStyle = results.GetInt32(results.GetOrdinal("Scale_LabelStyle_ID"))
            Else
                _overrideScaleData.LabelStyle = -1
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Scale_ScaleUOMUnit_ID"))) Then
                _overrideScaleData.UOM = results.GetInt32(results.GetOrdinal("Scale_ScaleUOMUnit_ID"))
            Else
                _overrideScaleData.UOM = -1
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Scale_RandomWeightType_ID"))) Then
                _overrideScaleData.RandomWeightType = results.GetInt32(results.GetOrdinal("Scale_RandomWeightType_ID"))
            Else
                _overrideScaleData.RandomWeightType = -1
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Scale_FixedWeight"))) Then
                _overrideScaleData.FixedWeight = results.GetString(results.GetOrdinal("Scale_FixedWeight"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Scale_ByCount"))) Then
                _overrideScaleData.ByCount = results.GetInt32(results.GetOrdinal("Scale_ByCount"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ShelfLife_Length"))) Then
                _overrideScaleData.ShelfLifeLength = results.GetInt16(results.GetOrdinal("ShelfLife_Length"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ForceTare"))) Then
                _overrideScaleData.ForceTare = results.GetBoolean(results.GetOrdinal("ForceTare"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Scale_Alternate_Tare_ID"))) Then
                _overrideScaleData.TareAlternate = results.GetInt32(results.GetOrdinal("Scale_Alternate_Tare_ID"))
            Else
                _overrideScaleData.TareAlternate = -1
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Scale_EatBy_ID"))) Then
                _overrideScaleData.EatBy = results.GetInt32(results.GetOrdinal("Scale_EatBy_ID"))
            Else
                _overrideScaleData.EatBy = -1
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Scale_Grade_ID"))) Then
                _overrideScaleData.Grade = results.GetInt32(results.GetOrdinal("Scale_Grade_ID"))
            Else
                _overrideScaleData.Grade = -1
            End If
            If (Not results.IsDBNull(results.GetOrdinal("PrintBlankEatBy"))) Then
                _overrideScaleData.PrintBlankShelfEatBy = results.GetBoolean(results.GetOrdinal("PrintBlankEatBy"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("PrintBlankPackDate"))) Then
                _overrideScaleData.PrintBlankPackDate = results.GetBoolean(results.GetOrdinal("PrintBlankPackDate"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("PrintBlankShelfLife"))) Then
                _overrideScaleData.PrintBlankShelfLife = results.GetBoolean(results.GetOrdinal("PrintBlankShelfLife"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("PrintBlankTotalPrice"))) Then
                _overrideScaleData.PrintBlankTotalPrice = results.GetBoolean(results.GetOrdinal("PrintBlankTotalPrice"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("PrintBlankUnitPrice"))) Then
                _overrideScaleData.PrintBlankUnitPrice = results.GetBoolean(results.GetOrdinal("PrintBlankUnitPrice"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("PrintBlankWeight"))) Then
                _overrideScaleData.PrintBlankWeight = results.GetBoolean(results.GetOrdinal("PrintBlankWeight"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Nutrifact_ID"))) Then
                _overrideScaleData.Nutrifact = results.GetInt32(results.GetOrdinal("Nutrifact_ID"))
            Else
                _overrideScaleData.Nutrifact = -1
            End If
        End While

        ' Populate the form with override data for the selected jurisdiction
        PopulateScaleInformation(_overrideScaleData)

        ' Default the data change flags for the newly selected jurisdiction
        SetDataChanges(False)
    End Sub

    ''' <summary>
    ''' Refresh the UI data with the values from the ScaleDetailsBO.
    ''' </summary>
    ''' <param name="scaleData"></param>
    ''' <remarks></remarks>
    Private Sub PopulateScaleInformation(ByRef scaleData As ScaleDetailsBO)
        TextBox_ScaleDesc1.Text = scaleData.ScaleDescription1
        TextBox_ScaleDesc2.Text = scaleData.ScaleDescription2
        TextBox_ScaleDesc3.Text = scaleData.ScaleDescription3
        TextBox_ScaleDesc4.Text = scaleData.ScaleDescription4

        If Not scaleData.ExtraText = Nothing Then
            Label_ExtraTextValue.Text = scaleData.ExtraText.ToString
            Label_ExtraTextValue.Tag = scaleData.ExtraTextID
        Else
            Label_ExtraTextValue.Tag = -1
            Label_ExtraTextValue.Text = String.Empty
        End If

        If Not scaleData.Nutrifact = -1 Then
            For Each itm As ScaleNutrifactBO In ComboBoxNutrifact.Items
                If itm.ID = scaleData.Nutrifact Then ComboBoxNutrifact.SelectedItem = itm
            Next
        Else
            ComboBoxNutrifact.SelectedIndex = -1
        End If


        If scaleData.Tare = -1 Then
            ComboBox_Tare.SelectedIndex = -1
        Else
            For Each itm As ScaleTareBO In ComboBox_Tare.Items
                If itm.ID = scaleData.Tare Then
                    ComboBox_Tare.SelectedItem = itm
                End If
            Next
        End If

        If scaleData.TareAlternate = -1 Then
            ComboBoxAlternateTare.SelectedIndex = -1
        Else
            For Each itm As ScaleTareBO In ComboBoxAlternateTare.Items
                If itm.ID = scaleData.TareAlternate Then
                    ComboBoxAlternateTare.SelectedItem = itm
                End If
            Next
        End If

        If scaleData.EatBy = -1 Then
            ComboBoxEatBy.SelectedIndex = -1
        Else
            For Each itm As ScaleEatByBO In ComboBoxEatBy.Items
                If itm.ID = scaleData.EatBy Then
                    ComboBoxEatBy.SelectedItem = itm
                End If
            Next
        End If

        If scaleData.Grade = -1 Then
            ComboBoxGrade.SelectedIndex = -1
        Else
            For Each itm As ScaleGradeBO In ComboBoxGrade.Items
                If itm.ID = scaleData.Grade Then
                    ComboBoxGrade.SelectedItem = itm
                End If
            Next
        End If

        If scaleData.LabelStyle = -1 Then
            ComboBox_LabelStyle.SelectedIndex = -1
        Else
            For Each itm As ScaleLabelStyleBO In ComboBox_LabelStyle.Items
                If itm.ID = scaleData.LabelStyle Then
                    ComboBox_LabelStyle.SelectedItem = itm
                End If
            Next
        End If

        ShelfLifeNumericEditor.Value = scaleData.ShelfLifeLength

        If scaleData.RandomWeightType = -1 Then
            RandomWeightCombo.SelectedIndex = -1
        Else
            For Each itm As ScaleRandomWeightTypeBO In RandomWeightCombo.Items
                If itm.ID = scaleData.RandomWeightType Then
                    RandomWeightCombo.SelectedItem = itm
                End If
            Next
        End If

        If scaleData.UOM = -1 Then
            ComboBox_ScaleUOM.SelectedIndex = -1
        Else
            For Each itm As ScaleUOMsBO In ComboBox_ScaleUOM.Items
                If itm.ID = scaleData.UOM Then
                    ComboBox_ScaleUOM.SelectedItem = itm
                End If
            Next
        End If

        FixedWeightTextbox.Text = scaleData.FixedWeight

        If scaleData.ByCount > 0 Then
            ByCountNumericEditor.Value = scaleData.ByCount
        Else
            ByCountNumericEditor.Value = Nothing
        End If

        CheckBoxForceTare.Checked = scaleData.ForceTare

        CheckBoxPrintBlankShelfLife.Checked = scaleData.PrintBlankShelfLife
        CheckBoxPrintBlankPackDate.Checked = scaleData.PrintBlankPackDate
        CheckBoxPrintBlankShelfEatBy.Checked = scaleData.PrintBlankShelfEatBy
        CheckBoxPrintBlankTotalPrice.Checked = scaleData.PrintBlankTotalPrice
        CheckBoxPrintBlankUnitPrice.Checked = scaleData.PrintBlankUnitPrice
        CheckBoxPrintBlankWeight.Checked = scaleData.PrintBlankWeight

        ' Enable/disable input values according to the data
        EnableCountAndWeightBoxes()

    End Sub

    ''' <summary>
    ''' Enforce the business rules to enable/disable the fixed weight and by count input values.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnableCountAndWeightBoxes()
        If ComboBox_ScaleUOM.SelectedIndex > -1 Then
            If CType(ComboBox_ScaleUOM.SelectedItem, ScaleUOMsBO).Description.ToUpper().StartsWith("BY COUNT") Then
                ByCountNumericEditor.Enabled = True
                FixedWeightTextbox.Text = String.Empty
                FixedWeightTextbox.Enabled = False
            ElseIf CType(ComboBox_ScaleUOM.SelectedItem, ScaleUOMsBO).Description.ToUpper().StartsWith("FIXED WEIGHT") Then
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
    End Sub

    Private Sub SetPermissions()
        IsEditable = False
        IsEditable = gbItemAdministrator Or gbSuperUser

        TextBox_ScaleDesc1.Enabled = IsEditable
        TextBox_ScaleDesc2.Enabled = IsEditable
        TextBox_ScaleDesc3.Enabled = IsEditable
        TextBox_ScaleDesc4.Enabled = IsEditable
        Button_ExtraTextSearch.Enabled = IsEditable
        Button_ExtraTextEdit.Enabled = IsEditable
        ComboBox_Tare.Enabled = IsEditable
        ComboBoxAlternateTare.Enabled = IsEditable
        ComboBox_LabelStyle.Enabled = IsEditable
        ShelfLifeNumericEditor.Enabled = IsEditable
        RandomWeightCombo.Enabled = IsEditable
        ComboBox_ScaleUOM.Enabled = IsEditable
        FixedWeightTextbox.Enabled = IsEditable
        ByCountNumericEditor.Enabled = IsEditable
        Button_RefreshScaleInfo.Enabled = IsEditable
        ComboBox_AltJurisdiction.Enabled = IsEditable
        CheckBoxForceTare.Enabled = IsEditable
        ComboBoxEatBy.Enabled = IsEditable
        ComboBoxGrade.Enabled = IsEditable
        CheckBoxPrintBlankShelfEatBy.Enabled = IsEditable
        CheckBoxPrintBlankPackDate.Enabled = IsEditable
        CheckBoxPrintBlankShelfLife.Enabled = IsEditable
        CheckBoxPrintBlankTotalPrice.Enabled = IsEditable
        CheckBoxPrintBlankUnitPrice.Enabled = IsEditable
        CheckBoxPrintBlankWeight.Enabled = IsEditable
        ComboBoxNutrifact.Enabled = IsEditable
        ButtonNutrifactAddEdit.Enabled = IsEditable
        ButtonNutrifactRemove.Enabled = IsEditable
        ButtonNutrifactSearch.Enabled = IsEditable
    End Sub

#Region "Changes to Data"
    Private Sub TextBox_ScaleDesc1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_ScaleDesc1.TextChanged
        SetDataChanges(True)
    End Sub

    Private Sub TextBox_ScaleDesc2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_ScaleDesc2.TextChanged
        SetDataChanges(True)
    End Sub

    Private Sub TextBox_ScaleDesc3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_ScaleDesc3.TextChanged
        SetDataChanges(True)
    End Sub

    Private Sub TextBox_ScaleDesc4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_ScaleDesc4.TextChanged
        SetDataChanges(True)
    End Sub

    Private Sub ComboBox_Tare_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Tare.SelectedIndexChanged
        SetDataChanges(True)
    End Sub

    Private Sub ComboBoxAlternateTare_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBoxAlternateTare.SelectedIndexChanged
        SetDataChanges(True)
    End Sub

    Private Sub ComboBox_LabelStyle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_LabelStyle.SelectedIndexChanged
        SetDataChanges(True)
    End Sub

    Private Sub ComboBox_ScaleUOM_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_ScaleUOM.SelectedIndexChanged
        SetDataChanges(True)

        ' Enable/disable input values according to the new Scale UOM value
        EnableCountAndWeightBoxes()
    End Sub

    Private Sub Label_ExtraTextValue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label_ExtraTextValue.TextChanged
        SetDataChanges(True)
    End Sub

    Private Sub ShelfLifeNumericEditor_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShelfLifeNumericEditor.ValueChanged
        SetDataChanges(True)
    End Sub

    Private Sub RandomWeightCombo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RandomWeightCombo.SelectedIndexChanged
        SetDataChanges(True)
    End Sub

    Private Sub FixedWeightTextbox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FixedWeightTextbox.TextChanged
        SetDataChanges(True)
    End Sub

    Private Sub ByCountNumericEditor_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ByCountNumericEditor.ValueChanged
        SetDataChanges(True)
    End Sub

    Private Sub CheckBoxForceTare_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBoxForceTare.CheckedChanged
        SetDataChanges(True)
    End Sub

    Private Sub ComboBoxEatBy_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBoxEatBy.SelectedIndexChanged
        SetDataChanges(True)
    End Sub

    Private Sub ComboBoxGrade_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBoxGrade.SelectedIndexChanged
        SetDataChanges(True)
    End Sub

    Private Sub CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxPrintBlankShelfLife.CheckedChanged, CheckBoxPrintBlankWeight.CheckedChanged, CheckBoxPrintBlankUnitPrice.CheckedChanged, CheckBoxPrintBlankTotalPrice.CheckedChanged, CheckBoxPrintBlankShelfEatBy.CheckedChanged, CheckBoxPrintBlankPackDate.CheckedChanged
        SetDataChanges(True)
    End Sub

    Private Sub ComboBoxNutrifact_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBoxNutrifact.SelectedIndexChanged
        SetDataChanges(True)
    End Sub

#End Region

    ''' <summary>
    ''' Save the updates to the database for the override data.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function SaveChanges() As Boolean
        Dim saveSuccess As Boolean = False
        If Me.ComboBox_AltJurisdiction.SelectedValue > 0 Then
            ' Populate the override business objects with the current form data
            ' Do not reset the override jurisdiction id values here.  They are set when the combo box is
            ' first selected.  If the user is changing their selection, we want to save the values from the
            ' previous selection first.
            _overrideScaleData.ScaleDescription1 = TextBox_ScaleDesc1.Text
            _overrideScaleData.ScaleDescription2 = TextBox_ScaleDesc2.Text
            _overrideScaleData.ScaleDescription3 = TextBox_ScaleDesc3.Text
            _overrideScaleData.ScaleDescription4 = TextBox_ScaleDesc4.Text

            _overrideScaleData.PrintBlankPackDate = CheckBoxPrintBlankPackDate.Checked
            _overrideScaleData.PrintBlankShelfEatBy = CheckBoxPrintBlankShelfEatBy.Checked
            _overrideScaleData.PrintBlankShelfLife = CheckBoxPrintBlankShelfLife.Checked
            _overrideScaleData.PrintBlankTotalPrice = CheckBoxPrintBlankTotalPrice.Checked
            _overrideScaleData.PrintBlankUnitPrice = CheckBoxPrintBlankUnitPrice.Checked
            _overrideScaleData.PrintBlankWeight = CheckBoxPrintBlankWeight.Checked

            If Not Label_ExtraTextValue.Text = String.Empty Then
                _overrideScaleData.ExtraTextID = CInt(Label_ExtraTextValue.Tag)
            Else
                _overrideScaleData.ExtraTextID = -1
            End If

            If ComboBoxNutrifact.SelectedIndex > -1 Then
                _overrideScaleData.Nutrifact = CInt(ComboBoxNutrifact.SelectedValue)
            Else
                _overrideScaleData.Nutrifact = -1
            End If

            If ComboBox_Tare.SelectedIndex > -1 Then
                _overrideScaleData.Tare = CInt(ComboBox_Tare.SelectedValue)
            Else
                _overrideScaleData.Tare = -1
            End If

            If ComboBoxAlternateTare.SelectedIndex > -1 Then
                _overrideScaleData.TareAlternate = CInt(ComboBoxAlternateTare.SelectedValue)
            Else
                _overrideScaleData.TareAlternate = -1
            End If

            If ComboBox_LabelStyle.SelectedIndex > -1 Then
                _overrideScaleData.LabelStyle = CInt(ComboBox_LabelStyle.SelectedValue)
            Else
                _overrideScaleData.LabelStyle = -1
            End If

            If ComboBoxEatBy.SelectedIndex > -1 Then
                _overrideScaleData.EatBy = CInt(ComboBoxEatBy.SelectedValue)
            Else
                _overrideScaleData.EatBy = -1
            End If

            If ComboBoxGrade.SelectedIndex > -1 Then
                _overrideScaleData.Grade = CInt(ComboBoxGrade.SelectedValue)
            Else
                _overrideScaleData.Grade = -1
            End If

            _overrideScaleData.ShelfLifeLength = CInt(ShelfLifeNumericEditor.Value.ToString())

            If RandomWeightCombo.SelectedIndex > -1 Then
                _overrideScaleData.RandomWeightType = CInt(RandomWeightCombo.SelectedValue)
            Else
                _overrideScaleData.RandomWeightType = -1
            End If

            If ComboBox_ScaleUOM.SelectedIndex > -1 Then
                _overrideScaleData.UOM = CInt(ComboBox_ScaleUOM.SelectedValue)
            Else
                _overrideScaleData.UOM = -1
            End If

            If FixedWeightTextbox.Enabled = True And FixedWeightTextbox.Text.Length > 0 Then
                _overrideScaleData.FixedWeight = FixedWeightTextbox.Text
            Else
                'clear the value
                _overrideScaleData.FixedWeight = String.Empty
            End If

            If ByCountNumericEditor.Enabled = True Then
                If Not ByCountNumericEditor.Value Is Nothing Then
                    If ByCountNumericEditor.Value.ToString().Length > 0 Then
                        _overrideScaleData.ByCount = CInt(ByCountNumericEditor.Value.ToString())
                    End If
                End If
            Else
                'clear the value
                _overrideScaleData.ByCount = 0
            End If

            If CheckBoxForceTare.Enabled = True Then
                _overrideScaleData.ForceTare = CheckBoxForceTare.Checked
            End If

            ' Include validation that exists on Form_ItemScaleDetails.vb 
            If _overrideScaleData.UOM > -1 Then
                If FixedWeightTextbox.Enabled = True And FixedWeightTextbox.Text.Length = 0 Then
                    'prompt user that this is required
                    MsgBox(String.Format(ResourcesIRMA.GetString("Required"), FixedWeightLabel.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
                    saveSuccess = False
                    Exit Function
                ElseIf ByCountNumericEditor.Enabled = True Then
                    If Not ByCountNumericEditor.Value Is Nothing Then
                        If ByCountNumericEditor.Value.ToString().Length = 0 Then
                            'prompt user that this is required
                            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), ByCountLabel.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
                            saveSuccess = False
                            Exit Function
                        End If
                    Else
                        'prompt user that this is required
                        MsgBox(String.Format(ResourcesIRMA.GetString("Required"), ByCountLabel.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
                        saveSuccess = False
                        Exit Function
                    End If
                End If
            End If

            ' clear ByCount value if UOM is not By Count
            If ComboBox_ScaleUOM.SelectedIndex > -1 Then
                If Not CType(ComboBox_ScaleUOM.SelectedItem, ScaleUOMsBO).Description.ToUpper().StartsWith("BY COUNT") Then
                    Me.ByCountNumericEditor.Value = 0
                    Me._overrideScaleData.ByCount = Me.ByCountNumericEditor.Value
                End If
            End If

            ' Save the changes to the database
            StoreJurisdictionDAO.SaveStoreScaleOverrideData(_overrideScaleData.ItemKey, _overrideScaleData.StoreJurisdictionID, _overrideScaleData)
            saveSuccess = True

            ' Reset the change flags since all changes have been saved
            SetDataChanges(False)
        Else
            MsgBox("Changes cannot be saved. Missing alternate jurisdication information.", MsgBoxStyle.Exclamation, Me.Text)
        End If
        

        Return saveSuccess
    End Function

    ''' <summary>
    ''' The user changed the selected jurisdiction.  Save the current changes, if there are any,
    ''' and then reload the data for the new jurisdiction.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ComboBox_AltJurisdiction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_AltJurisdiction.SelectedIndexChanged
        If Not IsInitializing Then
            ' If the user is selecting another jurisdiction, prompt them to save any changes to the current
            ' jurisdiction first.
            If _dataChanges Then
                Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = Windows.Forms.DialogResult.Yes Then
                    If SaveChanges() Then
                        ' Pre-fill the UI values for the newly selected jurisdiction.
                        PopulateScaleData(ComboBox_AltJurisdiction.SelectedValue)
                    Else
                        ' Remain on the previous selection and allow the user to correct the
                        ' values for saving.  Set the change flags to false before resetting the 
                        ' selection so the user is not prompted to save again.
                        Dim previousChangesFlag As Boolean = _dataChanges
                        SetDataChanges(False)
                        ComboBox_AltJurisdiction.SelectedValue = _overrideScaleData.StoreJurisdictionID
                        SetDataChanges(previousChangesFlag)
                    End If
                Else
                    ' Pre-fill the UI values for the newly selected jurisdiction, without a save.
                    PopulateScaleData(ComboBox_AltJurisdiction.SelectedValue)
                End If
            Else
                ' There are no changes to save.  Reload the UI values for the new selection.
                PopulateScaleData(ComboBox_AltJurisdiction.SelectedValue)
            End If
        End If
    End Sub

    ''' <summary>
    ''' The user is closing the form.  Prompt them to save changes first, if there have been any.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ItemScaleDetailsOverride_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' If there have been changes, prompt the user to save before exiting.
        If _dataChanges And IsEditable Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                If SaveChanges() = False Then
                    ' Do not close the form if the save was not successful.
                    e.Cancel = True
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' This button allows the user to populate the item override values with the values from the default
    ''' item jurisdiction.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_RefreshScaleInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RefreshScaleInfo.Click
        ' Populate the UI with default item data
        Dim result As DialogResult = MessageBox.Show(ResourcesItemHosting.GetString("msg_confirm_refreshOverrideScaleData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = Windows.Forms.DialogResult.Yes Then
            PopulateScaleInformation(_defaultScaleData)
        End If
    End Sub

    ''' <summary>
    ''' This button allows the user to search for an extra text record and associate it with the override data.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_ExtraTextSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ExtraTextSearch.Click
        Dim itemSearch As New frmItemSearch
        Dim extraTextBO As New ScaleExtraTextBO
        Dim currentItemID As Integer

        ' record the global item key before the search so it can be reset
        currentItemID = glItemID

        ' Allow the user to search for the item they want to use for the extra text record
        itemSearch.ShowDialog()
        itemSearch.Close()
        itemSearch.Dispose()

        ' look up the scale_extratext record for the selected item
        ' set combo to match this one
        extraTextBO = ScaleExtraTextDAO.GetExtraTextByItem(glItemID, CInt(_overrideScaleData.StoreJurisdictionID))

        ' reset the global item key
        glItemID = currentItemID

        If extraTextBO.ID = 0 Then
            ' warn the user that this item has no extra text record associated with it
            MsgBox(ResourcesItemHosting.GetString("NoScaleExtraText"), MsgBoxStyle.Critical, Me.Text)
        Else
            ' populate the scale business object with the extra text record
            _overrideScaleData.ExtraText = extraTextBO.Description
            _overrideScaleData.ExtraTextID = extraTextBO.ID

            ' fill in the UI values
            Label_ExtraTextValue.Text = _overrideScaleData.ExtraText
            Label_ExtraTextValue.Tag = _overrideScaleData.ExtraTextID
        End If
    End Sub

    ''' <summary>
    ''' This button allows the user to add or edit extra text records.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_ExtraTextEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ExtraTextEdit.Click
        ' Display the extra text form to the user, pre-filling with the current extra text id from the UI screen
        Me.extraTextForm = New ExtraTextEdit

        Dim _extraTextBO As New ScaleExtraTextBO
        _extraTextBO = ScaleExtraTextDAO.GetExtraTextByItem(glItemID, CInt(_overrideScaleData.StoreJurisdictionID))

        Me.extraTextForm.ItemKey = Me.DefaultScaleData.ItemKey
        Me.extraTextForm.CurrentExtraTextBO = _extraTextBO

        Me.extraTextForm.ShowDialog(Me)

        ' update the scale business object 
        _overrideScaleData.ExtraTextID = Me.extraTextForm.CurrentExtraTextBO.ID
        _overrideScaleData.ExtraText = Me.extraTextForm.CurrentExtraTextBO.ExtraText

        ' update the display
        Label_ExtraTextValue.Tag = _overrideScaleData.ExtraTextID
        Label_ExtraTextValue.Text = _overrideScaleData.ExtraText

        ' close the extra text form
        extraTextForm.Close()
        extraTextForm.Dispose()
    End Sub

    ''' <summary>
    ''' The user is exiting the form.  Prompt them to save changes if there are any.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ButtonExit_Click(sender As System.Object, e As System.EventArgs) Handles ButtonExit.Click
        ' If there have been changes, prompt the user to save before canceling.
        If _dataChanges And IsEditable Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then

                If SaveChanges() Then
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

    ''' <summary>
    ''' Save any changes without prompting.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ButtonSaveScaleOverrideInformation_Click(sender As System.Object, e As System.EventArgs) Handles ButtonSaveScaleOverrideInformation.Click
        If SaveChanges() Then
            MsgBox("All changes have been saved.", MsgBoxStyle.Information, Me.Text)
            SetDataChanges(False)
        End If
    End Sub

    Private Sub SetDataChanges(ByVal dataChangesExist As Boolean)
        _dataChanges = dataChangesExist
        ButtonSaveScaleOverrideInformation.Enabled = dataChangesExist
    End Sub

    Private Sub ButtonNutrifactSearch_Click(sender As System.Object, e As System.EventArgs) Handles ButtonNutrifactSearch.Click

        Dim itemSearch As New frmItemSearch
        Dim nutrifactId As Integer
        Dim currentItemId As Integer

        currentItemId = glItemID

        itemSearch.ShowDialog()
        itemSearch.Close()
        itemSearch.Dispose()

        ' Look up the nutrifact record for the selected item and set the CombBox to match it.
        nutrifactId = ScaleNutrifactDAO.GetNutriFactByItem(glItemID, ComboBox_AltJurisdiction.SelectedValue)
        glItemID = currentItemId

        If nutrifactId = -1 Then
            Exit Sub
        ElseIf nutrifactId = 0 Then
            ' Warn the user that this item has no nutrifact record associated with it.
            MsgBox(ResourcesItemHosting.GetString("NoNutrifact"), MsgBoxStyle.Critical, Me.Text)
        Else
            If ComboBoxNutrifact.Items.Count > 0 Then
                ComboBoxNutrifact.SelectedValue = nutrifactId
            End If
        End If

    End Sub

    Private Sub ButtonNutrifactAddEdit_Click(sender As System.Object, e As System.EventArgs) Handles ButtonNutrifactAddEdit.Click

        Dim previousSelection As ScaleNutrifactBO = Nothing
        Dim editedNutrifactId As Integer = -1

        nutrifactForm = New Form_Nutrifact()
        nutrifactForm.ForAlternateJurisdiction = True

        If ComboBoxNutrifact.SelectedIndex > -1 Then
            previousSelection = CType(ComboBoxNutrifact.SelectedItem, ScaleNutrifactBO)
            nutrifactForm.NutrifactID = previousSelection.ID
        End If

        nutrifactForm.ShowDialog(Me)

        ' Save the last nutrifactId being edited by the user so the combo box can be pre-filled.
        If nutrifactForm.DescriptionCombo.SelectedIndex > -1 Then
            editedNutrifactId = CType(nutrifactForm.DescriptionCombo.SelectedItem, ScaleNutrifactBO).ID
        End If

        nutrifactForm.Close()
        nutrifactForm.Dispose()

        'Reload CombBox.
        ComboBoxNutrifact.DataSource = ScaleNutrifactDAO.GetNutrifactComboList()

        ' If there were no nutrifact records when the form was loaded,
        ' the ComboBox needs to be initialized in the same way as in the Load event.
        If ComboBoxNutrifact.Items.Count > 0 Then
            ComboBoxNutrifact.DisplayMember = "Description"
            ComboBoxNutrifact.ValueMember = "ID"
        End If

        If editedNutrifactId > -1 Then
            ' Set to the last option edited by the user onthe nutrifact screen
            For Each itm As ScaleNutrifactBO In ComboBoxNutrifact.Items
                If itm.ID = editedNutrifactId Then ComboBoxNutrifact.SelectedItem = itm
            Next

            Dim nutrifactQuery = (From nutrifact As ScaleNutrifactBO In ComboBoxNutrifact.Items _
                                  Where nutrifact.ID = editedNutrifactId
                                  Select nutrifact).Single

            ComboBoxNutrifact.SelectedItem = nutrifactQuery
        Else
            ' Otherwise, set to the first option.
            ComboBoxNutrifact.SelectedIndex = -1
        End If

    End Sub

    Private Sub ButtonNutrifactRemove_Click(sender As System.Object, e As System.EventArgs) Handles ButtonNutrifactRemove.Click
        ComboBoxNutrifact.SelectedIndex = -1
    End Sub

    Private Sub LockNutrifacts()
        Me.ComboBoxNutrifact.Enabled = False
        Me.ButtonNutrifactRemove.Enabled = False
        Me.ButtonNutrifactSearch.Enabled = False
        Me.ButtonNutrifactAddEdit.Text = "View"

        If ComboBoxNutrifact.SelectedIndex = -1 Then
            Me.ButtonNutrifactAddEdit.Enabled = False
        End If
    End Sub

End Class
