Namespace Exception
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
                MyBase.New("No Cycle Count Master")
            End Sub
        End Class
    End Namespace
    Public Class InvalidIdentifierException
        Inherits System.Exception
        Public Sub New()
            MyBase.New("Item not found")
        End Sub
    End Class
    Public Class InvalidLogonException
        Inherits System.Exception
        Public Sub New(ByVal sMessage As String)
            MyBase.New(sMessage)
        End Sub
    End Class
    Public Class InvalidOrderException
        Inherits System.Exception
        Public Sub New(ByVal sMessage As String)
            MyBase.New(sMessage)
        End Sub
    End Class
    Public Class InventoryLocationItemExistsException
        Inherits System.Exception
        Public Sub New()
            MyBase.New("Item exists in location")
        End Sub
    End Class
    Public Class ItemInventorySubTeamException
        Inherits System.Exception
        Public Sub New()
            MyBase.New("Subteam cannot inventory this item")
        End Sub
    End Class
    Namespace Order
        Public Class NotFoundException
            Inherits System.Exception
            Public Sub New()
                MyBase.New("Order not found")
            End Sub
            Public Sub New(ByVal OrderHeaderID As Long)
                MyBase.New("Order " & OrderHeaderID & " not found" & Microsoft.VisualBasic.vbCrLf)
            End Sub
        End Class
        Public Class NotSentException
            Inherits System.Exception
            Public Sub New()
                MyBase.New("Order not sent")
            End Sub
        End Class
        Public Class InvoiceMissingPONum
            Inherits System.Exception
            Public Sub New()
                MyBase.New("Each invoice should have a value for the ponum field.  Please review the file on the server and determine the extent of the issue." & Microsoft.VisualBasic.vbCrLf)
            End Sub
        End Class
        Public Class InvoiceAlreadyUploadedToAP
            Inherits System.Exception
            Public Sub New(ByVal OrderHeaderID As Long)
                MyBase.New("This invoice cannot be processed because the purchase order, " & OrderHeaderID & ", for this invoice has already been uploaded to the Accounts Payable system.  Please review the file on the server, remove the invoice and its subordinate nodes from the file, then place the updated file back into the main folder so that the other invoices can be processed." & Microsoft.VisualBasic.vbCrLf)
            End Sub
        End Class
        Public Class InvoicePONotFound
            Inherits System.Exception
            Public Sub New(ByVal OrderHeaderID As Long)
                MyBase.New("This invoice cannot be processed because the purchase order number, " & OrderHeaderID & ", cannot be found in the database.  Please review the file on the server, remove the invoice and its subordinate nodes from the file, then place the updated file back into the main folder so that the other invoices can be processed." & Microsoft.VisualBasic.vbCrLf)
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
    Namespace OrderItemQueue
        Public Class NotSoldException
            Inherits System.Exception
            Public Sub New()
                MyBase.New("Item not sold in store")
            End Sub
        End Class
    End Namespace
    Namespace Warehouse
        Public Class NegativeReceivingCorrectionNotReceivedException
            Inherits System.Exception
            Public Sub New()
                MyBase.New("Negative correction not allowed because total received would be negative")
            End Sub
        End Class
        Public Class NegativeReceivingCorrectionNotOrderedException
            Inherits System.Exception
            Public Sub New()
                MyBase.New("Item was not ordered - negative correction not allowed")
            End Sub
        End Class
    End Namespace
End Namespace

