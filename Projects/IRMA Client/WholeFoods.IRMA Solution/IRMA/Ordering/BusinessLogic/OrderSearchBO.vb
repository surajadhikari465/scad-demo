Imports log4net

Namespace WholeFoods.IRMA.Ordering.BusinessLogic
    Public Enum OrderSearchStatus
        Success
        Error_InvoiceStartAndEndDateRequired
        Error_InvoiceStartDateInFuture
        Error_InvoiceEndDateInFuture
        Error_InvoiceEndDateAfterStartDate
        Error_OrderStartAndEndDateRequired
        Error_OrderStartDateInFuture
        Error_OrderEndDateInFuture
        Error_OrderEndDateAfterStartDate
    End Enum

    Public Class OrderSearchBO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Private _orderHeaderID As Integer = -1
        Private _orderInvoiceControlGroupID As Integer = -1
        Private _vendorID As Integer = -1
        Private _vendorKey As String = Nothing
        Private _invoiceNumber As String = Nothing
        Private _invoiceDateStart As Date = Date.MinValue
        Private _invoiceDateEnd As Date = Date.MinValue
        Private _orderDateStart As Date = Date.MinValue
        Private _orderDateEnd As Date = Date.MinValue
        Private _eInvoiceOnly As Boolean = False
        Private _vin As String = Nothing
        Private _identifier As String = Nothing
        Private _resolutionCodeID As Integer = 0


#Region "Property Accessors"
        Public Property ResolutionCodeID() As Integer
            Get
                Return _resolutionCodeID
            End Get
            Set(ByVal value As Integer)
                _resolutionCodeID = value
            End Set
        End Property

        Public Property EInvoiceOnly() As Boolean
            Get
                Return _eInvoiceOnly
            End Get
            Set(ByVal value As Boolean)
                _eInvoiceOnly = value
            End Set
        End Property

        Public Property OrderHeaderID() As Integer
            Get
                Return _orderHeaderID
            End Get
            Set(ByVal value As Integer)
                _orderHeaderID = value
            End Set
        End Property

        Public Property OrderInvoiceControlGroupID() As Integer
            Get
                Return _orderInvoiceControlGroupID
            End Get
            Set(ByVal value As Integer)
                _orderInvoiceControlGroupID = value
            End Set
        End Property

        Public Property VendorID() As Integer
            Get
                Return _vendorID
            End Get
            Set(ByVal value As Integer)
                _vendorID = value
            End Set
        End Property

        Public Property VendorKey() As String
            Get
                Return _vendorKey
            End Get
            Set(ByVal value As String)
                _vendorKey = value
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
        Public Property InvoiceDateStart() As Date
            Get
                Return _invoiceDateStart
            End Get
            Set(ByVal value As Date)
                _invoiceDateStart = value
            End Set
        End Property
        Public Property InvoiceDateEnd() As Date
            Get
                Return _invoiceDateEnd
            End Get
            Set(ByVal value As Date)
                _invoiceDateEnd = value
            End Set
        End Property
        Public Property OrderDateStart() As Date
            Get
                Return _orderDateStart
            End Get
            Set(ByVal value As Date)
                _orderDateStart = value
            End Set
        End Property
        Public Property OrderDateEnd() As Date
            Get
                Return _orderDateEnd
            End Get
            Set(ByVal value As Date)
                _orderDateEnd = value
            End Set
        End Property
        Public Property VIN() As String
            Get
                Return _vin
            End Get
            Set(ByVal value As String)
                _vin = value
            End Set
        End Property
        Public Property Identifier() As String
            Get
                Return _identifier
            End Get
            Set(ByVal value As String)
                _identifier = value
            End Set
        End Property
#End Region

#Region "Business Rule Validation"
        Public Function ValidateSuspendedInvoiceSearchData() As OrderSearchStatus
            Dim status As OrderSearchStatus = OrderSearchStatus.Success

            Dim now As Date = Date.Now
            Dim timeSpan As TimeSpan
            ' If one of the invoice search dates is entered, both must be entered and both must be past dates.
            If _invoiceDateStart <> Date.MinValue Or _invoiceDateEnd <> Date.MinValue Then
                ' Are both dates entered?
                If _invoiceDateStart = Date.MinValue Or _invoiceDateEnd = Date.MinValue Then
                    status = OrderSearchStatus.Error_InvoiceStartAndEndDateRequired
                    Return status
                Else
                    ' Are both dates today or earlier?
                    timeSpan = now.Subtract(_invoiceDateStart)
                    If timeSpan.Days < 0 Then
                        status = OrderSearchStatus.Error_InvoiceStartDateInFuture
                        Return status
                    Else
                        timeSpan = now.Subtract(_invoiceDateEnd)
                        If timeSpan.Days < 0 Then
                            status = OrderSearchStatus.Error_InvoiceEndDateInFuture
                            Return status
                        Else
                            ' Is the start date before or equal to the end date?
                            timeSpan = _invoiceDateEnd.Subtract(_invoiceDateStart)
                            If timeSpan.Days < 0 Then
                                status = OrderSearchStatus.Error_InvoiceEndDateAfterStartDate
                                Return status
                            End If
                        End If
                    End If
                End If
            End If

            ' If one of the order search dates is entered, both must be entered and both must be past dates.
            If _orderDateStart <> Date.MinValue Or _orderDateEnd <> Date.MinValue Then
                ' Are both dates entered?
                If _orderDateStart = Date.MinValue Or _orderDateEnd = Date.MinValue Then
                    status = OrderSearchStatus.Error_OrderStartAndEndDateRequired
                Else
                    ' Are both dates today or earlier?
                    timeSpan = now.Subtract(_orderDateStart)
                    If timeSpan.Days < 0 Then
                        status = OrderSearchStatus.Error_OrderStartDateInFuture
                    Else
                        timeSpan = now.Subtract(_orderDateEnd)
                        If timeSpan.Days < 0 Then
                            status = OrderSearchStatus.Error_OrderEndDateInFuture
                        Else
                            ' Is the start date before or equal to the end date?
                            timeSpan = _orderDateEnd.Subtract(_orderDateStart)
                            If timeSpan.Days < 0 Then
                                status = OrderSearchStatus.Error_OrderEndDateAfterStartDate
                            End If
                        End If
                    End If
                End If
            End If

            Return status
        End Function
#End Region
    End Class
End Namespace
