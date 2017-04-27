Imports WholeFoods.IRMA.EPromotions.BusinessLogic.Common
Imports System.ComponentModel   ' Need for BindingList 

Namespace WholeFoods.IRMA.EPromotions.BusinessLogic
    Public Class PromotionalOfferStoreBO
        Inherits BO_IRMABase


        Private _StoreNo As String
        Public Property StoreNo() As String
            Get
                Return _StoreNo
            End Get
            Set(ByVal value As String)
                _StoreNo = value
            End Set
        End Property


        Private _StoreName As String
        Public Property StoreName() As String
            Get
                Return _StoreName
            End Get
            Set(ByVal value As String)
                _StoreName = value
            End Set
        End Property


        Private _OfferId As Integer
        Public Property OfferId() As Integer
            Get
                Return _OfferId
            End Get
            Set(ByVal value As Integer)
                _OfferId = value
            End Set
        End Property

        Private _IsActive As Boolean
        Public Property IsActive() As Boolean
            Get
                Return _IsActive
            End Get
            Set(ByVal value As Boolean)
                _IsActive = value
            End Set
        End Property

        Private _IsAssigned As Boolean
        Public Property IsAssigned() As Boolean
            Get
                Return _IsAssigned
            End Get
            Set(ByVal value As Boolean)
                _IsAssigned = value
            End Set
        End Property


        Private _IsSelected As Boolean
        Public Property IsSelected() As Boolean
            Get
                Return _IsSelected
            End Get
            Set(ByVal value As Boolean)
                _IsSelected = value
            End Set
        End Property


        Public Overrides Function ToString() As String
            Dim RetVal As String
            If _IsActive Then
                RetVal = _StoreName & " *"
            Else
                RetVal = _StoreName
            End If
            Return RetVal
        End Function



        Public Sub New()
        End Sub

    End Class



End Namespace