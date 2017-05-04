
Option Explicit On
Option Strict On

Imports WholeFoods.IRMA.EPromotions.BusinessLogic.Common

Namespace WholeFoods.IRMA.EPromotions.BusinessLogic
    Public Class RewardTypeBO
        Inherits BO_IRMABase
        ' Class: RewardTypeBO
        ' Description: Provides storage and business functions for Reward Type data.
        ' *** CURRENTLY *** Reward Type is not hosted in the DB
        ' Created: 21 April 06

#Region "Property Definitions"


        Private _RewardTypeID As Integer
        Public Property RewardTypeID() As Integer
            Get
                Return _RewardTypeID
            End Get
            Set(ByVal value As Integer)
                _RewardTypeID = value
            End Set
        End Property


        Private _RewardTypeName As String
        Public Property Name() As String
            Get
                Return _RewardTypeName
            End Get
            Set(ByVal value As String)
                _RewardTypeName = value
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
