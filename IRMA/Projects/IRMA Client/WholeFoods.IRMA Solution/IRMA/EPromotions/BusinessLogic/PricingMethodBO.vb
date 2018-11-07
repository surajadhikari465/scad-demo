Option Explicit On
Option Strict On

Imports WholeFoods.IRMA.EPromotions.BusinessLogic.Common
Namespace WholeFoods.IRMA.EPromotions.BusinessLogic
    Public Class PricingMethodBO
        Inherits BO_IRMABase
        ' Class: PricingMethodBO
        ' Description: Provides storage and business functions for Pricing Method data.
        ' Created: 21 April 06

#Region "Property Definitions"

        Private _PricingMethodID As Integer
        Public Property PricingMethodID() As Integer
            Get
                Return _PricingMethodID
            End Get
            Set(ByVal value As Integer)
                _PricingMethodID = value
            End Set
        End Property

        Private _PricingMethodName As String
        Public Property Name() As String
            Get
                Return _PricingMethodName
            End Get
            Set(ByVal value As String)
                _PricingMethodName = value
            End Set
        End Property

        Private _EnableOfferEditor As Boolean
        Public Property EnableOfferEditor() As Boolean
            Get
                Return _EnableOfferEditor
            End Get
            Set(ByVal value As Boolean)
                _EnableOfferEditor = value
            End Set
        End Property

        Private _EnablePromoScreen As Boolean
        Public Property EnablePromoScreen() As Boolean
            Get
                Return _EnablePromoScreen
            End Get
            Set(ByVal value As Boolean)
                _EnablePromoScreen = value
            End Set
        End Property

        Private _EnableSaleMultiple As Boolean
        Public Property EnableSaleMultiple() As Boolean
            Get
                Return _EnableSaleMultiple
            End Get
            Set(ByVal value As Boolean)
                _EnableSaleMultiple = value
            End Set
        End Property

        Private _EnableEarnedRegMultiple As Boolean
        Public Property EnableEarnedRegMultiple() As Boolean
            Get
                Return _EnableEarnedRegMultiple
            End Get
            Set(ByVal value As Boolean)
                _EnableEarnedRegMultiple = value
            End Set
        End Property

        Private _EarnedRegMultipleDefault As Integer
        Public Property EarnedRegMultipleDefault() As Integer
            Get
                Return _EarnedRegMultipleDefault
            End Get
            Set(ByVal value As Integer)
                _EarnedRegMultipleDefault = value
            End Set
        End Property

        Private _EnableEarnedSaleMultiple As Boolean
        Public Property EnableEarnedSaleMultiple() As Boolean
            Get
                Return _EnableEarnedSaleMultiple
            End Get
            Set(ByVal value As Boolean)
                _EnableEarnedSaleMultiple = value
            End Set
        End Property

        Private _EarnedSaleMultipleDefault As Integer
        Public Property EarnedSaleMultipleDefault() As Integer
            Get
                Return _EarnedSaleMultipleDefault
            End Get
            Set(ByVal value As Integer)
                _EarnedSaleMultipleDefault = value
            End Set
        End Property

        Private _EnableEarnedLimit As Boolean
        Public Property EnableEarnedLimit() As Boolean
            Get
                Return _EnableEarnedLimit
            End Get
            Set(ByVal value As Boolean)
                _EnableEarnedLimit = value
            End Set
        End Property

        Private _EarnedLimitDefault As Integer
        Public Property EarnedLimitDefault() As Integer
            Get
                Return _EarnedLimitDefault
            End Get
            Set(ByVal value As Integer)
                _EarnedLimitDefault = value
            End Set
        End Property

#End Region

#Region "Constructors"
        ''' <summary>
        ''' empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub


#End Region

#Region "Business Rules"

#End Region

    End Class
End Namespace
