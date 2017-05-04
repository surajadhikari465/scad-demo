Imports log4net

Namespace WholeFoods.IRMA.Ordering.BusinessLogic
    Public Class OrderHeaderInvoiceBO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Private _orderHeaderID As Integer = -1
        Private _invoiceNumber As String = Nothing
        Private _invoiceDate As Date = Nothing
        Private _vendorDocId As String = Nothing
        Private _vendorDocDate As Date = Nothing
        Private _partialShippment As Boolean = False

#Region "Property Accessors"
        Public Property OrderHeaderID() As Integer
            Get
                Return _orderHeaderID
            End Get
            Set(ByVal value As Integer)
                _orderHeaderID = value
            End Set
        End Property

        Public Property InvoiceNumber() As String
            Get
                Return _invoiceNumber
            End Get
            Set(ByVal value As String)
                _invoiceNumber = value
            End Set
        End Property

        Public Property InvoiceDate() As Date
            Get
                Return _invoiceDate
            End Get
            Set(ByVal value As Date)
                _invoiceDate = value
            End Set
        End Property

        Public Property VendorDocId() As String
            Get
                Return _vendorDocId
            End Get
            Set(ByVal value As String)
                _vendorDocId = value
            End Set
        End Property

        Public Property VendorDocDate() As Date
            Get
                Return _vendorDocDate
            End Get
            Set(ByVal value As Date)
                _vendorDocDate = value
            End Set
        End Property

        Public Property PartialShippment() As Boolean
            Get
                Return _partialShippment
            End Get
            Set(ByVal value As Boolean)
                _partialShippment = value
            End Set
        End Property
#End Region
    End Class
End Namespace
