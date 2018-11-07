Imports WholeFoods.IRMA.EPromotions.BusinessLogic.Common

Namespace WholeFoods.IRMA.EPromotions.BusinessLogic
    Public Class SubTeamBO
        Inherits BO_IRMABase
        ' Class: SubTeamBO
        ' Description: Provides storage and business functions for Subteam data.
        ' Created: 21 June 06

#Region "Constructors"
        ''' <summary>
        ''' empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()


        End Sub

#End Region

#Region "Property Definitions"

        Private _SubTeamNo As Integer
        Public Property SubTeamNo() As Integer
            Get
                Return _SubTeamNo
            End Get
            Set(ByVal value As Integer)
                MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "SubTeamNo")
                _SubTeamNo = value
            End Set
        End Property

        Private _SubTeamName As String
        Public Property SubTeamName() As String
            Get
                Return _SubTeamName
            End Get
            Set(ByVal value As String)
                MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "SubTeamName")
                _SubTeamName = value
            End Set
        End Property

        Private _Unrestricted As Boolean
        Public Property Unrestricted() As Boolean
            Get
                Return _Unrestricted
            End Get
            Set(ByVal value As Boolean)
                If value <> _Unrestricted Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "Unrestricted")
                End If
                _Unrestricted = value
            End Set
        End Property

#End Region

#Region "Business Rules"

#End Region

    End Class
End Namespace
