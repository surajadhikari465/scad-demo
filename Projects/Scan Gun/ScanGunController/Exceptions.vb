Namespace Exception
    Public Class InvalidLogonException
        Inherits System.Exception
        Public Sub New(ByVal Message As String)
            MyBase.New(Message)
        End Sub
    End Class
    Public Class InventoryLocationItemExistsException
        Inherits System.Exception
        Public Sub New()
            MyBase.New("Item exists in location")
        End Sub
    End Class
    Namespace OrderItemQueue
        Public Class NotSoldException
            Inherits System.Exception
            Public Sub New()
                MyBase.New("Item not sold in store")
            End Sub
        End Class
    End Namespace
    Public Class ItemInventorySubTeamException
        Inherits System.Exception
        Public Sub New()
            MyBase.New("Subteam cannot inventory this item")
        End Sub
    End Class
    Namespace CycleCount
        Public Class BeforeStartScanException
            Inherits System.Exception
            Dim m_start_scan As Date
            Public ReadOnly Property StartScan() As Date
                Get
                    Return Me.m_start_scan
                End Get
            End Property
            Public Sub New(ByVal StartScan As Date)
                MyBase.New("Cannot add count before Cycle Count Header Start Scan")
                Me.m_start_scan = StartScan
            End Sub
        End Class
        Public Class ExceedsEndScanException
            Inherits System.Exception
            Dim m_end_scan As Date
            Public ReadOnly Property EndScan() As Date
                Get
                    Return Me.m_end_scan
                End Get
            End Property
            Public Sub New(ByVal EndScan As Date)
                MyBase.New("Master count end scan exceeded")
                Me.m_end_scan = EndScan
            End Sub
        End Class
        Public Class NoMasterException
            Inherits System.Exception
            Public Sub New()
                MyBase.New("No open master")
            End Sub
        End Class
    End Namespace
    Namespace Order
        Public Class NotFoundException
            Inherits System.Exception
            Public Sub New()
                MyBase.New("Order not found")
            End Sub
        End Class
        Public Class ClosedException
            Inherits System.Exception
            Public Sub New()
                MyBase.New("Order closed")
            End Sub
        End Class
        Public Class WrongStoreException
            Inherits System.Exception
            Public Sub New()
                MyBase.New("Order is for another store")
            End Sub
        End Class
        Public Class NotSentException
            Inherits System.Exception
            Public Sub New()
                MyBase.New("Order not sent")
            End Sub
        End Class
    End Namespace
    Namespace OrderItem
        Public Class MissingException
            Inherits System.Exception
            Public Sub New(ByVal sMessage As String)
                MyBase.New(sMessage)
            End Sub
        End Class
        Public Class ReceivedWeightMissingException
            Inherits System.Exception
            Public Sub New()
                MyBase.New("Received weight is required")
            End Sub
        End Class
    End Namespace
    Public Class UnitsOutOfRangeException
        Inherits System.Exception
        Public Sub New()
            MyBase.New("Units must be < 10000")
        End Sub
    End Class
    Public Class AverageUnitWeightMissingException
        Inherits System.Exception
        Public Sub New()
            MyBase.New("Item does not have Unit Average Weight")
        End Sub
    End Class
End Namespace