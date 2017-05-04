
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    ''' <summary>
    ''' Generated Business object base class for the ItemAttribute db table.
    '''
    ''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
    '''
    ''' Created By:	James Winfield
    ''' Created   :	Feb 26, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ItemSignAttributeBO

#Region "Persistent Fields and Properties"

        Private _itemSignAttributeID As System.Int32?
        'Private _isItemSignAttributeIDNull As Boolean
        Private _itemKey As System.Int32
        Private _isItemKeyNull As Boolean

        Private _airChilled As System.Boolean?
        Private _biodynamic As System.Boolean?
        Private _cheeseRaw As System.Boolean?
        Private _colorAdded As System.Boolean?
        Private _dryAged As System.Boolean?
        Private _glutenFree As System.Boolean?
        Private _grassFed As System.Boolean?
        Private _freeRange As System.Boolean?
        Private _kosher As System.Boolean?
        Private _madeInHouse As System.Boolean?
        Private _msc As System.Boolean?
        Private _NonGmo As System.Boolean?
        Private _pastureRaised As System.Boolean?
        Private _premiumBodyCare As System.Boolean?
        Private _vegan As System.Boolean?
        Private _vegetarian As System.Boolean?
        Private _wholeTrade As System.Boolean?

        Private _chicagoBaby As System.String
        Private _locality As System.String
        Private _signRomanceLong As System.String
        Private _signRomanceShort As System.String
        Private _animalWelfareRating As System.String
        Private _cheeseMilkType As System.String
        Private _ecoScaleRating As System.String
        Private _freshOrFrozen As System.String
        Private _healthyEatingRating As System.String
        Private _seafoodCatchType As System.String

        Private _udtExclusive As System.DateTime?

        Private _txtTagUom As System.Int32?

        Public Overridable Property ItemSignAttributeID() As System.Int32?
            Get
                Return _itemSignAttributeID
            End Get
            Set(ByVal value As System.Int32?)
                _itemSignAttributeID = value
            End Set
        End Property
        Public Overridable Property ItemKey() As System.Int32
            Get
                Return _itemKey
            End Get
            Set(ByVal value As System.Int32)
                _itemKey = value
            End Set
        End Property
        Public Overridable Property AirChilled() As System.Boolean?
            Get
                Return _airChilled
            End Get
            Set(ByVal value As System.Boolean?)
                _airChilled = value
            End Set
        End Property
        Public Overridable Property Biodynamic() As System.Boolean?
            Get
                Return _biodynamic
            End Get
            Set(ByVal value As System.Boolean?)
                _biodynamic = value
            End Set
        End Property
        Public Overridable Property CheeseRaw() As System.Boolean?
            Get
                Return _cheeseRaw
            End Get
            Set(ByVal value As System.Boolean?)
                _cheeseRaw = value
            End Set
        End Property
        Public Overridable Property ColorAdded() As System.Boolean?
            Get
                Return _colorAdded
            End Get
            Set(ByVal value As System.Boolean?)
                _colorAdded = value
            End Set
        End Property
        Public Overridable Property DryAged() As System.Boolean?
            Get
                Return _dryAged
            End Get
            Set(ByVal value As System.Boolean?)
                _dryAged = value
            End Set
        End Property
        Public Overridable Property GlutenFree() As System.Boolean?
            Get
                Return _glutenFree
            End Get
            Set(ByVal value As System.Boolean?)
                _glutenFree = value
            End Set
        End Property
        Public Overridable Property GrassFed() As System.Boolean?
            Get
                Return _grassFed
            End Get
            Set(ByVal value As System.Boolean?)
                _grassFed = value
            End Set
        End Property
        Public Overridable Property FreeRange() As System.Boolean?
            Get
                Return _freeRange
            End Get
            Set(ByVal value As System.Boolean?)
                _freeRange = value
            End Set
        End Property
        Public Overridable Property Kosher() As System.Boolean?
            Get
                Return _kosher
            End Get
            Set(ByVal value As System.Boolean?)
                _kosher = value
            End Set
        End Property
        Public Overridable Property MadeInHouse() As System.Boolean?
            Get
                Return _madeInHouse
            End Get
            Set(ByVal value As System.Boolean?)
                _madeInHouse = value
            End Set
        End Property
        Public Overridable Property Msc() As System.Boolean?
            Get
                Return _msc
            End Get
            Set(ByVal value As System.Boolean?)
                _msc = value
            End Set
        End Property
        Public Overridable Property NonGmo() As System.Boolean?
            Get
                Return _NonGmo
            End Get
            Set(ByVal value As System.Boolean?)
                _NonGmo = value
            End Set
        End Property
        Public Overridable Property PastureRaised() As System.Boolean?
            Get
                Return _pastureRaised
            End Get
            Set(ByVal value As System.Boolean?)
                _pastureRaised = value
            End Set
        End Property
        Public Overridable Property PremiumBodyCare() As System.Boolean?
            Get
                Return _premiumBodyCare
            End Get
            Set(ByVal value As System.Boolean?)
                _premiumBodyCare = value
            End Set
        End Property
        Public Overridable Property Vegan() As System.Boolean?
            Get
                Return _vegan
            End Get
            Set(ByVal value As System.Boolean?)
                _vegan = value
            End Set
        End Property
        Public Overridable Property Vegetarian() As System.Boolean?
            Get
                Return _vegetarian
            End Get
            Set(ByVal value As System.Boolean?)
                _vegetarian = value
            End Set
        End Property
        Public Overridable Property WholeTrade() As System.Boolean?
            Get
                Return _wholeTrade
            End Get
            Set(ByVal value As System.Boolean?)
                _wholeTrade = value
            End Set
        End Property
        Public Overridable Property AnimalWelfareRating() As System.String
            Get
                Return _animalWelfareRating
            End Get
            Set(ByVal value As System.String)
                _animalWelfareRating = value
            End Set
        End Property
        Public Overridable Property CheeseMilkType() As System.String
            Get
                Return _cheeseMilkType
            End Get
            Set(ByVal value As System.String)
                _cheeseMilkType = value
            End Set
        End Property
        Public Overridable Property ChicagoBaby() As System.String
            Get
                Return _chicagoBaby
            End Get
            Set(ByVal value As System.String)
                _chicagoBaby = value
            End Set
        End Property
        Public Overridable Property Locality() As System.String
            Get
                Return _locality
            End Get
            Set(ByVal value As System.String)
                _locality = value
            End Set
        End Property
        Public Overridable Property EcoScaleRating() As System.String
            Get
                Return _ecoScaleRating
            End Get
            Set(ByVal value As System.String)
                _ecoScaleRating = value
            End Set
        End Property
        Public Overridable Property FreshOrFrozen() As System.String
            Get
                Return _freshOrFrozen
            End Get
            Set(ByVal value As System.String)
                _freshOrFrozen = value
            End Set
        End Property
        Public Overridable Property HealthyEatingRating() As System.String
            Get
                Return _healthyEatingRating
            End Get
            Set(ByVal value As System.String)
                _healthyEatingRating = value
            End Set
        End Property
        Public Overridable Property SeafoodCatchType() As System.String
            Get
                Return _seafoodCatchType
            End Get
            Set(ByVal value As System.String)
                _seafoodCatchType = value
            End Set
        End Property
        Public Overridable Property SignRomanceLong() As System.String
            Get
                Return _signRomanceLong
            End Get
            Set(ByVal value As System.String)
                _signRomanceLong = value
            End Set
        End Property
        Public Overridable Property SignRomanceShort() As System.String
            Get
                Return _signRomanceShort
            End Get
            Set(ByVal value As System.String)
                _signRomanceShort = value
            End Set
        End Property
        Public Overridable Property Exclusive() As System.DateTime?
            Get
                Return _udtExclusive
            End Get
            Set(ByVal value As System.DateTime?)
                _udtExclusive = value
            End Set
        End Property
        Public Overridable Property TagUom() As System.Int32?
            Get
                Return _txtTagUom
            End Get
            Set(ByVal value As System.Int32?)
                _txtTagUom = value
            End Set
        End Property
#End Region
    End Class

End Namespace

