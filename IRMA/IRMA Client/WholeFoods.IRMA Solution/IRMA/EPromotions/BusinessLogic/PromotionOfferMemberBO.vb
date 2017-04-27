Imports WholeFoods.IRMA.EPromotions.BusinessLogic.Common

#Region "Enumerations"

Public Enum PromotionOfferMemberStatus
    Valid
    Error_Required_Description
    Error_Required_PriceMethodID
    Error_Required_StartDate
    Error_Required_EndDate
    Error_Required_RewardID
    Error_Required_RewardAmount
    Error_NotNumeric_RewardAmount
End Enum

Public Enum PromotionOfferMemberJoinLogic As Byte
    MeetOne = 0 ' used to be OR
    Mandatory = 1 ' used to be AND
End Enum

Public Enum PromotionOfferMemberPurpose As Byte
    Requirement = 0
    Reward = 1
End Enum

#End Region

Namespace WholeFoods.IRMA.EPromotions.BusinessLogic
    Public Class PromotionOfferMemberBO
        Inherits BO_IRMABase
        ' Class: PromotionOfferMemberBO
        ' Description: Provides storage and business functions for Promotional Offer data.
        ' Created: 17 April 06

#Region "Constructors"
        ''' <summary>
        ''' empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()


        End Sub

#End Region

#Region "Property Definitions"

        Private _OfferMemberID As Integer
        Public Property OfferMemberID() As Integer
            Get
                Return _OfferMemberID
            End Get
            Set(ByVal value As Integer)
                _OfferMemberID = value
            End Set
        End Property

        Private _OfferID As Integer
        Public Property OfferID() As Integer
            Get
                Return _OfferID
            End Get
            Set(ByVal value As Integer)
                _OfferID = value
            End Set
        End Property

        Private _GroupID As Integer
        Public Property GroupID() As Integer
            Get
                Return _GroupID
            End Get
            Set(ByVal value As Integer)
                If value <> _GroupID Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "GroupID")
                End If
                _GroupID = value
            End Set
        End Property

        Private _Quantity As Integer
        Public Property Quantity() As Integer
            Get
                Return _Quantity
            End Get
            Set(ByVal value As Integer)
                If value <> _Quantity Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "Quantity")
                End If
                _Quantity = value
            End Set
        End Property

        Private _Purpose As PromotionOfferMemberPurpose
        Public Property Purpose() As PromotionOfferMemberPurpose
            Get
                Return _Purpose
            End Get
            Set(ByVal value As PromotionOfferMemberPurpose)
                If value <> _Purpose Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "Purpose")
                End If
                _Purpose = value
            End Set
        End Property

        Private _JoinLogic As PromotionOfferMemberJoinLogic
        Public Property JoinLogic() As PromotionOfferMemberJoinLogic
            Get
                Return _JoinLogic
            End Get
            Set(ByVal value As PromotionOfferMemberJoinLogic)
                If value <> _JoinLogic Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "JoinLogic")
                End If
                _JoinLogic = value
            End Set
        End Property

        Private _Modified As Date
        Public Property Modified() As Date
            Get
                Return _Modified
            End Get
            Set(ByVal value As Date)
                If value <> _Modified Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "Modified")
                End If
                _Modified = value
            End Set
        End Property

        Private _UserID As Integer
        Public Property UserID() As Integer
            Get
                Return _UserID
            End Get
            Set(ByVal value As Integer)
                If value <> _UserID Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "UserID")
                End If
                _UserID = value
            End Set
        End Property


        Private _GroupName As String
        Public Property GroupName() As String
            Get
                Return _GroupName
            End Get
            Set(ByVal value As String)
                If value <> _GroupName Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "Groupname")
                End If

                _GroupName = value
            End Set
        End Property


        Private _CreateDate As DateTime
        Public Property CreateDate() As DateTime
            Get
                Return _CreateDate
            End Get
            Set(ByVal value As DateTime)
                If value <> _CreateDate Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "CreateDate")
                End If

                _CreateDate = value
            End Set
        End Property


        Private _ModifiedDate As DateTime
        Public Property ModifiedDate() As DateTime
            Get
                Return _ModifiedDate
            End Get
            Set(ByVal value As DateTime)
                If value <> _ModifiedDate Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "ModifiedDate")
                End If
                _ModifiedDate = value
            End Set
        End Property



#End Region

#Region "Business Rules"
        ''' <summary>
        ''' validates data elements of current instance of PromotionOfferBO object
        ''' </summary>
        ''' <returns>ArrayList of PromotionOfferStatus values</returns>
        ''' <remarks></remarks>
        Public Function ValidateData() As ArrayList
            Dim statusList As New ArrayList

            'If Me.TaxFlagKey Is Nothing Or Me.TaxFlagKey.Trim.Equals("") Then
            '    statusList.Add(TaxFlagStatus.Error_Required_TaxFlagKey)
            'End If

            'pos ID must be numeric
            'If Not IsNumeric(Me.POSId) Then
            '    statusList.Add(TaxFlagStatus.Error_NotNumeric_POSID)
            'End If

            If statusList.Count = 0 Then
                statusList.Add(PromotionOfferMemberStatus.Valid)
            End If

            Return statusList
        End Function


#End Region

    End Class
End Namespace
