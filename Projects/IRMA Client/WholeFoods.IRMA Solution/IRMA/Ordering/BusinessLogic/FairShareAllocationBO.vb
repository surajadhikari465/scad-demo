Namespace WholeFoods.IRMA.Ordering.BusinessLogic

    Public Class FairShareAllocationBO

#Region " Enumerations"

        Public Enum OrderSubteamType
            All = -1
            NonRetail = 1
            Retail = 0
        End Enum

        Public Enum WarehouseStatus
            All = -1
            Sent = 1
            NotSent = 0
        End Enum

        Public Enum BOH
            All = 0
            GreaterThanZero = 1
            LessThanEqualZero = 2
            LessThanZero = 3
            GreaterThanEqualZero = 4
        End Enum

        Public Enum PreOrder
            All = -1
            PreOrder = 1
            NonPreOrder = 0
        End Enum

#End Region

        Public Class AllocationSession

#Region " Public Properties"

            Private _username As String
            Public Property Username() As String
                Get
                    Return _username
                End Get
                Set(ByVal value As String)
                    _username = value
                End Set
            End Property

            Private _orderSubteamType As OrderSubteamType
            Public Property OrderSubteamTypeOption() As OrderSubteamType
                Get
                    Return _orderSubteamType
                End Get
                Set(ByVal value As OrderSubteamType)
                    _orderSubteamType = value
                End Set
            End Property

            Private _preOrder As PreOrder
            Public Property PreOrderOption() As PreOrder
                Get
                    Return _preOrder
                End Get
                Set(ByVal value As PreOrder)
                    _preOrder = value
                End Set
            End Property

            Private _warehouse As Integer
            Public Property Warehouse() As Integer
                Get
                    Return _warehouse
                End Get
                Set(ByVal value As Integer)
                    _warehouse = value
                End Set
            End Property

            Private _subteamNo As Integer
            Public Property SubteamNo() As Integer
                Get
                    Return _subteamNo
                End Get
                Set(ByVal value As Integer)
                    _subteamNo = value
                End Set
            End Property

#End Region

#Region " Public Constructors"

            Public Sub New(ByVal sUsername As String, ByVal iWarehouse As Integer, ByVal enumOrderSubteamType As OrderSubteamType, ByVal enumPreOrder As PreOrder, ByVal iSubTeamNo As Integer)

                Me._username = sUsername
                Me._warehouse = iWarehouse
                Me._orderSubteamType = enumOrderSubteamType
                Me._preOrder = enumPreOrder
                Me._subteamNo = iSubTeamNo

            End Sub

#End Region

        End Class

    End Class

End Namespace
