Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Public Class Form_ManageItemAttributes

    Private _attributeIdentifiersList As BusinessObjectCollection = Nothing
    Private _attributeIdentifiersHash As BusinessObjectCollection = Nothing
    Private _availableFields As ArrayList = Nothing

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        'close form
        Me.Close()
    End Sub

    Private Sub Form_ManageItemAttributes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadData()
        BindControls()
    End Sub

    Private Sub LoadData()
        _attributeIdentifiersList = AttributeIdentifierDAO.Instance.GetAllAttributeIdentifiers()
        _attributeIdentifiersHash = New BusinessObjectCollection()
        For Each instance As AttributeIdentifier In _attributeIdentifiersList
            _attributeIdentifiersHash.Add(instance.FieldType, instance)
        Next
    End Sub
    Private Sub BindControls()
        Me.Combo_Available_Attribute_Fields.DataSource = PopulateAvailable_Fields()
        Me.Combo_Field_Types.DataSource = PopulateFiedTypes()
    End Sub

    

    Private Sub Combo_Available_Attribute_Fields_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Combo_Available_Attribute_Fields.SelectedIndexChanged
        Dim Choice As String = Me.Combo_Available_Attribute_Fields.SelectedItem.ToString
        Dim Identifier As AttributeIdentifier
        If _attributeIdentifiersHash.ContainsKey(Choice) Then
            Identifier = CType(_attributeIdentifiersHash.ItemByKey(Choice), AttributeIdentifier)
        ElseIf Not Choice.Equals("Choose Field") Then
            Identifier = New AttributeIdentifier()
            Identifier.DefaultValue = ""
            Identifier.FieldValues = ""
            If Choice.StartsWith("Text") Then
                Identifier.MaxWidth = 10
            End If
            Identifier.FieldType = Choice
        Else
            Identifier = New AttributeIdentifier()
            Identifier.ScreenText = ""
            Identifier.MaxWidth = 10
            Identifier.DefaultValue = ""
            Identifier.FieldType = ("Choose a Field")
        End If
        UpdateForm(Identifier)
    End Sub

    Private Sub Combo_Field_Types_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Combo_Field_Types.SelectedIndexChanged
        Dim Choice As String = Me.Combo_Field_Types.SelectedItem.ToString
        If Choice.Equals("Text Field") Then
            Me.Number_Max_Width.Enabled = True
            Me.Text_Field_Values.Enabled = False
        Else
            Me.Number_Max_Width.Enabled = False
            Me.Text_Field_Values.Enabled = True
        End If
    End Sub

    Private Sub UpdateForm(ByVal Identifier As AttributeIdentifier)
        Me.Text_Screen_Text.Text = Identifier.ScreenText
        If Identifier.FieldType.StartsWith("Check") Or Identifier.FieldType.StartsWith("Date") Then
            Me.Combo_Field_Types.Enabled = False
            Me.Number_Max_Width.Enabled = False
            Me.Text_Default_Value.Enabled = False
            Me.Text_Default_Value.Text = ""
            Me.Text_Field_Values.Enabled = False
            Me.Text_Field_Values.Text = ""
        ElseIf Identifier.FieldType.StartsWith("Text") Then
            Me.Combo_Field_Types.Enabled = True
            If Identifier.ComboBox = False Then
                Dim index As Integer
                index = Me.Combo_Field_Types.FindString("Text Field")
                Me.Combo_Field_Types.SelectedIndex = index
                Me.Number_Max_Width.Enabled = True
                Me.Number_Max_Width.Value = Identifier.MaxWidth
                Me.Text_Field_Values.Enabled = False
            Else
                Dim index As Integer
                index = Me.Combo_Field_Types.FindString("Combo Box")
                Me.Combo_Field_Types.SelectedIndex = index
                Me.Number_Max_Width.Enabled = False
                Me.Text_Field_Values.Enabled = True
                Me.Text_Field_Values.Text = Identifier.FieldValues
            End If
            Me.Text_Default_Value.Enabled = True
            Me.Text_Default_Value.Text = Identifier.DefaultValue
        Else
            Me.Combo_Field_Types.Enabled = False
            Me.Number_Max_Width.Enabled = False
            Me.Text_Default_Value.Enabled = False
            Me.Text_Default_Value.Text = ""
            Me.Text_Field_Values.Enabled = False
            Me.Text_Field_Values.Text = ""
        End If

    End Sub

    Private Function PopulateAvailable_Fields() As ArrayList
        _availableFields = New ArrayList()
        _availableFields.Add("Choose Field")
        'Add Check box fields
        _availableFields.Add("CheckBox1")
        _availableFields.Add("CheckBox2")
        _availableFields.Add("CheckBox3")
        _availableFields.Add("CheckBox4")
        _availableFields.Add("CheckBox5")
        _availableFields.Add("CheckBox6")
        _availableFields.Add("CheckBox7")
        _availableFields.Add("CheckBox8")
        _availableFields.Add("CheckBox9")
        _availableFields.Add("CheckBox10")
        _availableFields.Add("CheckBox11")
        _availableFields.Add("CheckBox12")
        _availableFields.Add("CheckBox13")
        _availableFields.Add("CheckBox14")
        _availableFields.Add("CheckBox15")
        _availableFields.Add("CheckBox16")
        _availableFields.Add("CheckBox17")
        _availableFields.Add("CheckBox18")
        _availableFields.Add("CheckBox19")
        _availableFields.Add("CheckBox20")
        'Add Text fields
        _availableFields.Add("Text1")
        _availableFields.Add("Text2")
        _availableFields.Add("Text3")
        _availableFields.Add("Text4")
        _availableFields.Add("Text5")
        _availableFields.Add("Text6")
        _availableFields.Add("Text7")
        _availableFields.Add("Text8")
        _availableFields.Add("Text9")
        _availableFields.Add("Text10")
        'Add Date Time fields
        _availableFields.Add("DateTime1")
        _availableFields.Add("DateTime2")
        _availableFields.Add("DateTime3")
        _availableFields.Add("DateTime4")
        _availableFields.Add("DateTime5")
        _availableFields.Add("DateTime6")
        _availableFields.Add("DateTime7")
        _availableFields.Add("DateTime8")
        _availableFields.Add("DateTime9")
        _availableFields.Add("DateTime10")

        Return _availableFields
    End Function

    Private Function PopulateFiedTypes() As ArrayList
        Dim AvaialbleFieldTypes As ArrayList = New ArrayList()
        AvaialbleFieldTypes.Add("Text Field")
        AvaialbleFieldTypes.Add("Combo Box")
        Return AvaialbleFieldTypes
    End Function

    Private Sub Button_Create_Attribute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Create_Attribute.Click
        Dim isvalid As Boolean = True
        If Me.Text_Default_Value.Enabled And Me.Text_Field_Values.Enabled Then
            If Not Me.Text_Field_Values.Text.Contains(Me.Text_Default_Value.Text) Then
                MessageBox.Show("Field Values must contain the Default Value")
                isvalid = False
            End If
        End If
        If isvalid = True And Me.Combo_Field_Types.SelectedText.Equals("Combo Box") And Me.Text_Field_Values.Text.Equals("") Then
            MessageBox.Show("You must enter Field Values if combo box is selected")
            isvalid = False
        End If
        If isvalid = True And Me.Text_Field_Values.Enabled And Not Me.Text_Field_Values.Text.Equals("") Then
            If Not Me.Text_Field_Values.Text.Contains(",") Then
                MessageBox.Show("Field Values must be comma seperated")
                isvalid = False
            End If
        End If

        If isvalid Then
            SaveData()
        End If
    End Sub

    Private Sub SaveData()
        Dim Choice As String = CType(Me.Combo_Available_Attribute_Fields.SelectedItem, String)
        Dim Identifier As AttributeIdentifier
        If Not Choice.Equals("Choose Field") Then
            If _attributeIdentifiersHash.ContainsKey(Choice) Then
                Identifier = CType(_attributeIdentifiersHash.ItemByKey(Choice), AttributeIdentifier)
            Else
                Identifier = New AttributeIdentifier()
                _attributeIdentifiersHash.Add(Choice, Identifier)
            End If
            Identifier.ScreenText = Me.Text_Screen_Text.Text
            Identifier.FieldType = Choice
            If Choice.StartsWith("Text") Then
                If Me.Combo_Field_Types.SelectedItem.ToString.Equals("Text Field") Then
                    Identifier.ComboBox = False
                    Identifier.MaxWidth = CType(Me.Number_Max_Width.Value, Integer)
                Else
                    Identifier.ComboBox = True
                    Identifier.DefaultValue = Me.Text_Default_Value.Text
                    Identifier.FieldValues = Me.Text_Field_Values.Text
                End If
            End If
            Identifier.Save()
        End If
    End Sub

    Private Sub Label_Default_Value_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label_Default_Value.Click

    End Sub

    Private Sub Text_Default_Value_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Text_Default_Value.TextChanged

    End Sub

    
End Class