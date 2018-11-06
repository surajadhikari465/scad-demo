Option Explicit On
Option Strict On

Imports WholeFoods.IRMA.EPromotions.BusinessLogic.Common
Imports WholeFoods.IRMA.EPromotions.DataAccess
Imports WholeFoods.IRMA.Pricing.DataAccess
Imports System.ComponentModel   ' Need for BindingList 

Namespace WholeFoods.IRMA.EPromotions.BusinessLogic
    Public Class PromotionalOfferUpdateBO
        Inherits BO_IRMABase

#Region "Constructors"
        ''' <summary>
        ''' empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

#End Region

#Region "Property Definitions"
        Private _offerID As Integer
        Public Property OfferID() As Integer
            Get
                Return _offerID
            End Get
            Set(ByVal value As Integer)
                _offerID = value
            End Set
        End Property

        Private _listOriginalItems As New BindingList(Of ItemGroupMemberBO)
        Public Property OriginalItems() As BindingList(Of ItemGroupMemberBO)
            Get
                Return _listOriginalItems
            End Get
            Set(ByVal value As BindingList(Of ItemGroupMemberBO))
                _listOriginalItems = value
            End Set
        End Property

        Private _listOriginalStores As New ArrayList
        Public Property OriginalStores() As ArrayList
            Get
                Return _listOriginalStores
            End Get
            Set(ByVal value As ArrayList)
                _listOriginalStores = value
            End Set
        End Property

        Private _listUpdatedItems As New BindingList(Of ItemGroupMemberBO)
        Public Property UpdatedItems() As BindingList(Of ItemGroupMemberBO)
            Get
                Return _listUpdatedItems
            End Get
            Set(ByVal value As BindingList(Of ItemGroupMemberBO))
                _listUpdatedItems = value
            End Set
        End Property

        Private _listUpdatedStores As New ArrayList
        Public Property UpdatedStores() As ArrayList
            Get
                Return _listUpdatedStores
            End Get
            Set(ByVal value As ArrayList)
                _listUpdatedStores = value
            End Set
        End Property

        Private _listItemDetails As New BindingList(Of ItemGroupMemberBO)
        Public Property ItemDetails() As BindingList(Of ItemGroupMemberBO)
            Get
                Return _listItemDetails
            End Get
            Set(ByVal value As BindingList(Of ItemGroupMemberBO))
                _listItemDetails = value
            End Set
        End Property



#End Region

#Region "Business Rules"
        Public Function HasUnbatchedDetails(ByVal StoreID As Integer) As Boolean
            Dim retval As Boolean = False
            Dim pricebatchDAO As New PriceBatchDetailDAO

            retval = pricebatchDAO.GetUnbatchedPriceDetails(Me.OfferID, StoreID) > 0

            Return retval
        End Function

        Public Function IsPushed(ByVal StoreID As Integer) As Boolean
            Dim retval As Boolean = False
            Dim offerDAO As New PromotionOfferDAO

            Try
                retval = offerDAO.GetStoreActiveFlag(Me.OfferID, StoreID)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "PromotionOfferUpdateBO:IsPushed")
                retval = True
            End Try

            Return retval
        End Function
#End Region

    End Class
End Namespace