Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class ItemSignAttributeForm

    Private _itemKey As Integer
    Private _identifier As String
    Private _description As String
    Private _itemDeleted As Boolean
    Private _itemSignAtribute As ItemSignAttributeBO = Nothing
    Private _currentItemSignAtribute As ItemSignAttributeBO = Nothing

    Public Property ItemKey() As Integer
        Get
            Return _itemKey
        End Get
        Set(ByVal value As Integer)
            _itemKey = value
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

    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

    Public Property ItemDeleted() As Boolean
        Get
            Return _itemDeleted
        End Get
        Set(ByVal value As Boolean)
            _itemDeleted = value
        End Set
    End Property

    Private Sub ItemAttributeUpdateForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BindControls()
        ApplyAccess()
    End Sub

    Private Sub txtTagUom_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTagUom.KeyPress
        '97 - 122 = Ascii codes for simple letters
        '65 - 90  = Ascii codes for capital letters
        '48 - 57  = Ascii codes for numbers

        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        ComposeCurrentItemSignAttribute()

        If (IsNothing(_itemSignAtribute.Exclusive) And Not IsNothing(_currentItemSignAtribute.Exclusive)) Or
            (IsNothing(_currentItemSignAtribute.Exclusive) And Not IsNothing(_itemSignAtribute.Exclusive)) Or
            (IsNothing(_itemSignAtribute.ColorAdded) And Not IsNothing(_currentItemSignAtribute.ColorAdded)) Or
            (IsNothing(_currentItemSignAtribute.ColorAdded) And Not IsNothing(_itemSignAtribute.ColorAdded)) Or
            (IsNothing(_itemSignAtribute.TagUom) And Not IsNothing(_currentItemSignAtribute.TagUom)) Or
            (IsNothing(_currentItemSignAtribute.TagUom) And Not IsNothing(_itemSignAtribute.TagUom)) Or
            _itemSignAtribute.Locality <> _currentItemSignAtribute.Locality Or
            _itemSignAtribute.SignRomanceLong <> _currentItemSignAtribute.SignRomanceLong Or
            _itemSignAtribute.SignRomanceShort <> _currentItemSignAtribute.SignRomanceShort Or
            _itemSignAtribute.ChicagoBaby <> _currentItemSignAtribute.ChicagoBaby Or
            _itemSignAtribute.Exclusive <> _currentItemSignAtribute.Exclusive Or
            _itemSignAtribute.ColorAdded <> _currentItemSignAtribute.ColorAdded Or
            _itemSignAtribute.TagUom <> _currentItemSignAtribute.TagUom Then

            If MessageBox.Show("Value changed on item sign attribute." & vbCrLf & "Are you sure you want to exit the form without save?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                Exit Sub
            End If
        End If

        Me.Close()
    End Sub

    Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
        SaveChanges()
        Me.Close()
    End Sub

    ''' <summary>
    ''' Disabling all input fields for users without SuperUser or Item Admin rights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ApplyAccess()
        If Not ((gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser) Then
            For Each ctr As Control In Me.frameIrma.Controls
                If TypeOf (ctr) Is ComboBox Or TypeOf (ctr) Is TextBox Then
                    ctr.Enabled = False
                End If
            Next
            For Each ctr As Control In Me.frameIcon.Controls
                If TypeOf (ctr) Is DateTimePicker Or TypeOf (ctr) Is Infragistics.Win.UltraWinEditors.UltraDateTimeEditor Then
                    ctr.Enabled = False
                End If
            Next
            cmdUpdate.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Create data sources and bind to this form's controls.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindControls()
        Me.txtIdentifier.Text = _identifier
        Me.txtItemDesc.Text = _description

        _itemKey = ItemSignAttributeDAO.Instance.GetItemKeyByIdentifier(_identifier)
        _itemSignAtribute = ItemSignAttributeDAO.GetItemSignAttributeByItemKey(_itemKey)

        txtLocality.Text = _itemSignAtribute.Locality
        txtSignRomanceLong.Text = _itemSignAtribute.SignRomanceLong
        txtSignRomanceShort.Text = _itemSignAtribute.SignRomanceShort
        txtChicagoBaby.Text = _itemSignAtribute.ChicagoBaby

        If _itemSignAtribute.TagUom.HasValue Then
            txtTagUom.Text = _itemSignAtribute.TagUom.ToString()
        End If

        If _itemSignAtribute.Exclusive.HasValue Then
            udtExclusive.Value = _itemSignAtribute.Exclusive
        End If

        If _itemSignAtribute.ColorAdded.HasValue Then
            If _itemSignAtribute.ColorAdded Then
                cbxColorAdded.SelectedIndex = 1
            Else
                cbxColorAdded.SelectedIndex = 2
            End If
        End If

        If _itemSignAtribute.AirChilled.HasValue Then
            If _itemSignAtribute.AirChilled Then
                cbxAirChilled.SelectedIndex = 1
            Else
                cbxAirChilled.SelectedIndex = 2
            End If
        End If

        If _itemSignAtribute.Biodynamic.HasValue Then
            If _itemSignAtribute.Biodynamic Then
                cbxBiodynamic.SelectedIndex = 1
            Else
                cbxBiodynamic.SelectedIndex = 2
            End If
        End If

        If _itemSignAtribute.CheeseRaw.HasValue Then
            If _itemSignAtribute.CheeseRaw Then
                cbxCheeseRaw.SelectedIndex = 1
            Else
                cbxCheeseRaw.SelectedIndex = 2
            End If
        End If

        If _itemSignAtribute.DryAged.HasValue Then
            If _itemSignAtribute.DryAged Then
                cbxDryAged.SelectedIndex = 1
            Else
                cbxDryAged.SelectedIndex = 2
            End If
        End If

        If _itemSignAtribute.GlutenFree.HasValue Then
            If _itemSignAtribute.GlutenFree Then
                cbxGlutenFree.SelectedIndex = 1
            Else
                cbxGlutenFree.SelectedIndex = 2
            End If
        End If

        If _itemSignAtribute.GrassFed.HasValue Then
            If _itemSignAtribute.GrassFed Then
                cbxGrassFed.SelectedIndex = 1
            Else
                cbxGrassFed.SelectedIndex = 2
            End If
        End If

        If _itemSignAtribute.FreeRange.HasValue Then
            If _itemSignAtribute.FreeRange Then
                cbxFreeRange.SelectedIndex = 1
            Else
                cbxFreeRange.SelectedIndex = 2
            End If
        End If

        If _itemSignAtribute.Kosher.HasValue Then
            If _itemSignAtribute.Kosher Then
                cbxKosher.SelectedIndex = 1
            Else
                cbxKosher.SelectedIndex = 2
            End If
        End If

        If _itemSignAtribute.MadeInHouse.HasValue Then
            If _itemSignAtribute.MadeInHouse Then
                cbxMadeInHouse.SelectedIndex = 1
            Else
                cbxMadeInHouse.SelectedIndex = 2
            End If
        End If

        If _itemSignAtribute.Msc.HasValue Then
            If _itemSignAtribute.Msc Then
                cbxMsc.SelectedIndex = 1
            Else
                cbxMsc.SelectedIndex = 2
            End If

        End If

        If _itemSignAtribute.NonGmo.HasValue Then
            If _itemSignAtribute.NonGmo Then
                cbxNonGmo.SelectedIndex = 1
            Else
                cbxNonGmo.SelectedIndex = 2
            End If
        End If

        If _itemSignAtribute.PastureRaised.HasValue Then
            If _itemSignAtribute.PastureRaised Then
                cbxPastureRaised.SelectedIndex = 1
            Else
                cbxPastureRaised.SelectedIndex = 2
            End If
        End If

        If _itemSignAtribute.PremiumBodyCare.HasValue Then
            If _itemSignAtribute.PremiumBodyCare Then
                cbxPremiumBodyCare.SelectedIndex = 1
            Else
                cbxPremiumBodyCare.SelectedIndex = 2
            End If
        End If

        If _itemSignAtribute.Vegan.HasValue Then
            If _itemSignAtribute.Vegan Then
                cbxVegan.SelectedIndex = 1
            Else
                cbxVegan.SelectedIndex = 2
            End If
        End If

        If _itemSignAtribute.Vegetarian.HasValue Then
            If _itemSignAtribute.Vegetarian Then
                cbxVegetarian.SelectedIndex = 1
            Else
                cbxVegetarian.SelectedIndex = 2
            End If
        End If

        If _itemSignAtribute.WholeTrade.HasValue Then
            If _itemSignAtribute.WholeTrade Then
                cbxWholeTrade.SelectedIndex = 1
            Else
                cbxWholeTrade.SelectedIndex = 2
            End If
        End If

        txtAnimalWelfareRating.Text = _itemSignAtribute.AnimalWelfareRating
        txtCheeseMilkType.Text = _itemSignAtribute.CheeseMilkType
        txtEcoScaleRating.Text = _itemSignAtribute.EcoScaleRating
        txtFreshOrFrozen.Text = _itemSignAtribute.FreshOrFrozen
        txtHealthyEatingRating.Text = _itemSignAtribute.HealthyEatingRating
        txtSeafoodCatchType.Text = _itemSignAtribute.SeafoodCatchType

        If _itemDeleted Then
            cmdUpdate.Enabled = False
            txtLocality.Enabled = False
            txtSignRomanceLong.Enabled = False
            txtSignRomanceShort.Enabled = False
            txtChicagoBaby.Enabled = False
            udtExclusive.Enabled = False
            cbxColorAdded.Enabled = False
            txtTagUom.Enabled = False
        End If
    End Sub

    Private Sub SaveChanges()
        ComposeCurrentItemSignAttribute()

        If (IsNothing(_itemSignAtribute.Exclusive) And Not IsNothing(_currentItemSignAtribute.Exclusive)) Or
            (IsNothing(_currentItemSignAtribute.Exclusive) And Not IsNothing(_itemSignAtribute.Exclusive)) Or
            (IsNothing(_itemSignAtribute.ColorAdded) And Not IsNothing(_currentItemSignAtribute.ColorAdded)) Or
            (IsNothing(_currentItemSignAtribute.ColorAdded) And Not IsNothing(_itemSignAtribute.ColorAdded)) Or
            (IsNothing(_itemSignAtribute.TagUom) And Not IsNothing(_currentItemSignAtribute.TagUom)) Or
            (IsNothing(_currentItemSignAtribute.TagUom) And Not IsNothing(_itemSignAtribute.TagUom)) Or
            _itemSignAtribute.Locality <> _currentItemSignAtribute.Locality Or
            _itemSignAtribute.SignRomanceLong <> _currentItemSignAtribute.SignRomanceLong Or
            _itemSignAtribute.SignRomanceShort <> _currentItemSignAtribute.SignRomanceShort Or
            _itemSignAtribute.ChicagoBaby <> _currentItemSignAtribute.ChicagoBaby Or
            _itemSignAtribute.Exclusive <> _currentItemSignAtribute.Exclusive Or
            _itemSignAtribute.ColorAdded <> _currentItemSignAtribute.ColorAdded Or
            _itemSignAtribute.TagUom <> _currentItemSignAtribute.TagUom Then

            _currentItemSignAtribute.ItemSignAttributeID = _itemSignAtribute.ItemSignAttributeID
            _currentItemSignAtribute.ItemKey = _itemKey

            ItemSignAttributeDAO.Save(_currentItemSignAtribute)
        End If
    End Sub

    Private Sub ComposeCurrentItemSignAttribute()
        _currentItemSignAtribute = New ItemSignAttributeBO()

        _currentItemSignAtribute.Locality = txtLocality.Text.Trim()
        _currentItemSignAtribute.SignRomanceLong = txtSignRomanceLong.Text.Trim()
        _currentItemSignAtribute.SignRomanceShort = txtSignRomanceShort.Text.Trim()
        _currentItemSignAtribute.ChicagoBaby = txtChicagoBaby.Text.Trim()

        If Not IsNothing(udtExclusive.Value) Then
            _currentItemSignAtribute.Exclusive = udtExclusive.Value
        End If

        If cbxColorAdded.SelectedIndex = 1 Then
            _currentItemSignAtribute.ColorAdded = True
        ElseIf cbxColorAdded.SelectedIndex = 2 Then
            _currentItemSignAtribute.ColorAdded = False
        End If

        If Not String.IsNullOrEmpty(txtTagUom.Text.Trim()) Then
            _currentItemSignAtribute.TagUom = Integer.Parse(txtTagUom.Text.Trim())
        End If
    End Sub
End Class