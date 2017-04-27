
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports System.Reflection

Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic


    ''' <summary>
    ''' This class is loaded with any default values for item attributes
    ''' that appear as fields on the Add Item form.
    ''' It is critical that the property names of this class are the same
    ''' as the corresponding column names in the item table in the database.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ItemAttributeDefaultsBO

#Region "Property Definitions"

        Private _item_Key As Integer
        Private _category_ID As Integer
        Private _prodHierarchyLevel4_ID As Integer

        Private _taxClassID As Integer = -1
        Private _labelType_ID As Integer = -1
        Private _retail_Sale As Boolean = False
        Private _costedByWeight As Boolean = False

        Private _managedBy As Integer
        Private _discountable As Boolean = False
        Private _foodStamps As Boolean = False
        Private _restrictedHours As Boolean = False
        Private _ageRestrict As Boolean = False

#End Region

#Region "Constructors"

        ''' <summary>
        ''' empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

#End Region

#Region "Property Access Methods"

        Public Property Category_ID() As Integer
            Get
                Return _category_ID
            End Get
            Set(ByVal value As Integer)
                _category_ID = value
            End Set
        End Property

        Public Property Item_Key() As Integer
            Get
                Return _item_Key
            End Get
            Set(ByVal value As Integer)
                _item_Key = value

                If value > 0 Then
                    SetAttributeDefaults()
                End If
            End Set
        End Property

        Public Property LabelType_ID() As Integer
            Get
                Return _labelType_ID
            End Get
            Set(ByVal value As Integer)
                _labelType_ID = value
            End Set
        End Property

        Public Property ProdHierarchyLevel4_ID() As Integer
            Get
                Return _prodHierarchyLevel4_ID
            End Get
            Set(ByVal value As Integer)
                _prodHierarchyLevel4_ID = value
            End Set
        End Property
        Public Property Retail_Sale() As Boolean
            Get
                Return _retail_Sale
            End Get
            Set(ByVal value As Boolean)
                _retail_Sale = value
            End Set
        End Property

        Public Property TaxClassID() As Integer
            Get
                Return _taxClassID
            End Get
            Set(ByVal value As Integer)
                _taxClassID = value
            End Set
        End Property

        Public Property CostedByWeight() As Boolean
            Get
                Return _costedByWeight
            End Get
            Set(ByVal value As Boolean)
                _costedByWeight = value
            End Set
        End Property

        Public Property ManagedBy() As Integer
            Get
                Return _managedBy
            End Get
            Set(ByVal value As Integer)
                _managedBy = value
            End Set
        End Property

        Public Property Discountable() As Boolean
            Get
                Return _discountable
            End Get
            Set(ByVal value As Boolean)
                _discountable = value
            End Set
        End Property

        Public Property Food_Stamps() As Boolean
            Get
                Return _foodStamps
            End Get
            Set(ByVal value As Boolean)
                _foodStamps = value
            End Set
        End Property

        Public Property Age_Restrict() As Boolean
            Get
                Return _ageRestrict
            End Get
            Set(ByVal value As Boolean)
                _ageRestrict = value
            End Set
        End Property

        Public Property Restricted_Hours() As Boolean
            Get
                Return _restrictedHours
            End Get
            Set(ByVal value As Boolean)
                _restrictedHours = value
            End Set
        End Property
#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Set the default values, if any, for the item attributes that appear on the AddItem form. 
        ''' </summary>

        Public Sub SetAttributeDefaults()

            Dim newItemBOClass As Type = Me.GetType
            Dim defaultableProperty As PropertyInfo

            Dim itemDefaultValues As ArrayList = _
                ItemDefaultValueDAO.GetItemDefaultValues(Me.Category_ID, Me.ProdHierarchyLevel4_ID)

            Dim itemDefaultValue As ItemDefaultValueBO
            For Each itemDefaultValue In itemDefaultValues

                defaultableProperty = newItemBOClass.GetProperty(itemDefaultValue.FieldName)

                If Not IsNothing(defaultableProperty) Then
                    ' found the property so set it

                    Select Case itemDefaultValue.DbDataType
                        Case 1 ' Integer
                            defaultableProperty.SetValue(Me, CType(itemDefaultValue.Value, Integer), Nothing)
                        Case 2 ' String
                            defaultableProperty.SetValue(Me, itemDefaultValue.Value, Nothing)
                        Case 3 ' Boolean
                            defaultableProperty.SetValue(Me, CType(itemDefaultValue.Value, Boolean), Nothing)
                        Case 4 ' Decimal
                            defaultableProperty.SetValue(Me, CType(itemDefaultValue.Value, Decimal), Nothing)
                        Case 5 ' Date
                            defaultableProperty.SetValue(Me, DateTime.Parse(itemDefaultValue.Value), Nothing)
                    End Select

                Else
                    ' alert user to error

                End If

            Next

        End Sub

#End Region

#Region "Data Access"

        ''' <summary>
        ''' Load the Item.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Load() As DataSet

            Return Nothing

        End Function

        ''' <summary>
        ''' Save the Item.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Save() As DataSet

            Return Nothing

        End Function

#End Region

    End Class

End Namespace
