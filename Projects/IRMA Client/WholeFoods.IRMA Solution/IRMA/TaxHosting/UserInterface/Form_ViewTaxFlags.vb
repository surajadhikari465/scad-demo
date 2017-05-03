Imports WholeFoods.IRMA.TaxHosting.BusinessLogic
Imports WholeFoods.IRMA.TaxHosting.DataAccess

Public Class Form_ViewTaxFlags

    Private _taxClassID As Integer
    Private _itemKey As Integer
    Private _isJurisdictionComboBound As Boolean = False

    Public Property TaxClassID() As Integer
        Get
            Return _taxClassID
        End Get
        Set(ByVal value As Integer)
            _taxClassID = value
        End Set
    End Property

    Public Property ItemKey() As Integer
        Get
            Return _itemKey
        End Get
        Set(ByVal value As Integer)
            _itemKey = value
        End Set
    End Property

    Private Sub Form_ViewTaxFlags_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'center form
        Me.CenterToScreen()

        'get grid data: tax flags + tax overrides
        BindData()
    End Sub

    ''' <summary>
    ''' bind data to form control
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindData()
        'get jurisdiction list
        BindTaxJurisdictionCombo()

        'get tax flag & override data for selected tax class and tax jurisdiction
        BindTaxFlagGrid()

        'hide tax override section if no item key provided
        If Me.ItemKey <= 0 Then
            Me.Label_TaxFlagOverrides.Visible = False
            Me.UltraGrid_TaxFlagOverride.Visible = False
        Else
            BindTaxOverrideGrid()
        End If
    End Sub

    ''' <summary>
    ''' bind data to tax jurisdiction combo box
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindTaxJurisdictionCombo()
        Dim jurisdictionDAO As New TaxJurisdictionDAO
        Dim jurisdictionList As ArrayList = jurisdictionDAO.GetJurisdictionList(Me.TaxClassID)
        Me.ComboBox_TaxJurisdiction.DataSource = jurisdictionList

        If jurisdictionList.Count > 0 Then
            Me.ComboBox_TaxJurisdiction.ValueMember = "TaxJurisdictionID"
            Me.ComboBox_TaxJurisdiction.DisplayMember = "TaxJurisdictionDesc"
        End If

        _isJurisdictionComboBound = True
    End Sub

    ''' <summary>
    ''' bind data to tax flag grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindTaxFlagGrid()
        Dim taxFlagDAO As New TaxFlagDAO
        Dim taxFlagBO As New TaxFlagBO

        taxFlagBO.TaxClassId = Me.TaxClassID
        taxFlagBO.TaxJurisdictionId = CType(Me.ComboBox_TaxJurisdiction.SelectedValue, Integer)

        UltraGrid_TaxFlag.DataSource = taxFlagDAO.GetTaxFlagList(taxFlagBO, Nothing)

        If UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns.Count > 0 Then
            'hide columns
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxClassId").Hidden = True
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxClassDesc").Hidden = True
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxJurisdictionId").Hidden = True
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxJurisdictionDesc").Hidden = True
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("ResetActiveFlags").Hidden = True
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxPercent").Hidden = True
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("POSID").Hidden = True

            'sort columns in correct order
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxFlagKey").Header.VisiblePosition = 0
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxFlagValue").Header.VisiblePosition = 1

            'set column names
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxFlagKey").Header.Caption = ResourcesTaxHosting.GetString("label_header_taxFlagKey")
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxFlagValue").Header.Caption = ResourcesTaxHosting.GetString("label_header_taxFlagValue")
        End If
    End Sub

    ''' <summary>
    ''' bind data to tax override grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindTaxOverrideGrid()
        Dim taxOverrideDAO As New TaxOverrideDAO
        Me.UltraGrid_TaxFlagOverride.DataSource = taxOverrideDAO.GetTaxOverrideList(Me.ItemKey)

        If UltraGrid_TaxFlagOverride.DisplayLayout.Bands(0).Columns.Count > 0 Then
            'hide columns
            UltraGrid_TaxFlagOverride.DisplayLayout.Bands(0).Columns("ItemKey").Hidden = True
            UltraGrid_TaxFlagOverride.DisplayLayout.Bands(0).Columns("StoreNo").Hidden = True

            'sort columns in correct order
            UltraGrid_TaxFlagOverride.DisplayLayout.Bands(0).Columns("StoreName").Header.VisiblePosition = 0
            UltraGrid_TaxFlagOverride.DisplayLayout.Bands(0).Columns("TaxFlagKey").Header.VisiblePosition = 1
            UltraGrid_TaxFlagOverride.DisplayLayout.Bands(0).Columns("TaxFlagValue").Header.VisiblePosition = 2

            'set column names
            UltraGrid_TaxFlagOverride.DisplayLayout.Bands(0).Columns("StoreName").Header.Caption = ResourcesTaxHosting.GetString("label_header_storeNo")
            UltraGrid_TaxFlagOverride.DisplayLayout.Bands(0).Columns("TaxFlagKey").Header.Caption = ResourcesTaxHosting.GetString("label_header_taxFlagKey")
            UltraGrid_TaxFlagOverride.DisplayLayout.Bands(0).Columns("TaxFlagValue").Header.Caption = ResourcesTaxHosting.GetString("label_header_taxFlagValue")
        End If
    End Sub

    Private Sub Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OK.Click
        Me.Close()
    End Sub

    Private Sub ComboBox_TaxJurisdiction_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_TaxJurisdiction.SelectedIndexChanged
        If _isJurisdictionComboBound Then
            'rebind tax flag grid w/ new jurisdiction; tax overrides won't change because this is based on item key
            BindTaxFlagGrid()
        End If
    End Sub
End Class