Imports WholeFoods.IRMA.EPromotions.BusinessLogic.Common
Imports WholeFoods.IRMA.EPromotions.DataAccess
Public Enum ItemGroup_GroupLogic
    [Or] = 0
    [And] = 1
End Enum
Namespace WholeFoods.IRMA.EPromotions.BusinessLogic
    Public Class ItemGroupBO
        Inherits BO_IRMABase
        ' Class: PromotionGroupBO
        ' Description: Provides storage and business functions for Promotional Group data.
        ' Created: 17 April 06

#Region "Property Definitions"

        Private _GroupID As Integer
        Public Property GroupID() As Integer
            Get
                Return _GroupID
            End Get
            Set(ByVal value As Integer)
                _GroupID = value
            End Set
        End Property

        Private _GroupName As String
        Public Property GroupName() As String
            Get
                Return _GroupName
            End Get
            Set(ByVal value As String)
                _GroupName = value
            End Set
        End Property

        Private _GroupLogic As ItemGroup_GroupLogic
        Public Property GroupLogic() As ItemGroup_GroupLogic
            Get
                Return _GroupLogic
            End Get
            Set(ByVal value As ItemGroup_GroupLogic)
                _GroupLogic = value
            End Set
        End Property

        Private _CreateDate As Date
        Public Property CreateDate() As Date
            Get
                Return _CreateDate
            End Get
            Set(ByVal value As Date)
                _CreateDate = value
            End Set
        End Property

        Private _ModifiedDate As Date
        Public Property ModifiedDate() As Date
            Get
                Return _ModifiedDate
            End Get
            Set(ByVal value As Date)
                _ModifiedDate = value
            End Set
        End Property

        Private _UserID As Integer
        Public Property UserID() As Integer
            Get
                Return _UserID
            End Get
            Set(ByVal value As Integer)
                _UserID = value
            End Set
        End Property

        Private _PromotionCount As Integer
        Public Property PromotionCount() As Integer
            Get
                Return _PromotionCount
            End Get
            Set(ByVal value As Integer)
                _PromotionCount = value
            End Set
        End Property

        Private _ActivePromotionCount As Integer
        Public Property ActivePromotionCount() As Integer
            Get
                Return _ActivePromotionCount
            End Get
            Set(ByVal value As Integer)
                _ActivePromotionCount = value
            End Set
        End Property

        Private _PendingPromotionCount As Integer
        Public Property PendingPromotionCount() As Integer
            Get
                Return _PendingPromotionCount
            End Get
            Set(ByVal value As Integer)
                _PendingPromotionCount = value
            End Set
        End Property

#End Region

#Region "Constructors"

#End Region

#Region "Methods"

        Public Function CheckGroupEditStatus() As Boolean
            Dim GroupDAO As ItemGroupDAO = New ItemGroupDAO
            Dim retval As Boolean = False

            Return retval
        End Function
#End Region

#Region "Business Rules"
        Public Function ValidateItemGroup(ByVal ItemGroup As ItemGroupBO, ByVal DeletedGroupIds As String) As Boolean
            Dim retval As Boolean = True
            Dim ErrorMessage As String = String.Empty
            Dim GroupDAO As ItemGroupMemberDAO = New ItemGroupMemberDAO

            If ItemGroup.GroupName.Length < 1 Then
                retval = False
                ErrorMessage += "Group Name cannot be null." & vbCrLf
            End If

            If Not GroupDAO.ValidateNewGroupName(ItemGroup.GroupName, DeletedGroupIds) Then
                retval = False
                ErrorMessage += "A group already exists with this name." & vbCrLf
            End If

            If ErrorMessage.Length > 0 Then
                MessageBox.Show(ErrorMessage, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            End If
            Return retval

        End Function
#End Region
    End Class
End Namespace
