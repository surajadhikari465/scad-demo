Imports System.Text
Imports log4net
Imports WholeFoods.ServiceLibrary.WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess

Namespace WholeFoods.IRMA.Replenishment.EInvoicing.BusinessLogic
    Public Class EInvoicing_HeaderBO
        Implements IDisposable

#Region " Properties "

        '        Private _version As String
        '        Public Property version() As String
        '            Get
        '                Return _version
        '            End Get
        '            Set(ByVal value As String)
        '                _version = value
        '            End Set
        '        End Property


        '        Private _vendor_id As String
        '        Public Property vendor_id() As String
        '            Get
        '                Return _vendor_id
        '            End Get
        '            Set(ByVal value As String)
        '                _vendor_id = value
        '            End Set
        '        End Property


        '        Private _usage_indicator As String
        '        Public Property usage_indicator() As String
        '            Get
        '                Return _usage_indicator
        '            End Get
        '            Set(ByVal value As String)
        '                _usage_indicator = value
        '            End Set
        '        End Property


        '        Private _file_type As String
        '        Public Property file_type() As String
        '            Get
        '                Return _file_type
        '            End Get
        '            Set(ByVal value As String)
        '                _file_type = value
        '            End Set
        '        End Property


        '        Private _trans_dt As DateTime
        '        Public Property trans_dt() As DateTime
        '            Get
        '                Return _trans_dt
        '            End Get
        '            Set(ByVal value As DateTime)
        '                _trans_dt = value
        '            End Set
        '        End Property


        '        Private _store As String
        '        Public Property store() As String
        '            Get
        '                Return _store
        '            End Get
        '            Set(ByVal value As String)
        '                _store = value
        '            End Set
        '        End Property


        '        Private _store_num As Integer
        '        Public Property store_num() As Integer
        '            Get
        '                Return _store_num
        '            End Get
        '            Set(ByVal value As Integer)
        '                _store_num = value
        '            End Set
        '        End Property


        '        Private _store_dept As Integer
        '        Public Property store_dept() As Integer
        '            Get
        '                Return _store_dept
        '            End Get
        '            Set(ByVal value As Integer)
        '                _store_dept = value
        '            End Set
        '        End Property


        '        Private _street1 As String
        '        Public Property street1() As String
        '            Get
        '                Return _street1
        '            End Get
        '            Set(ByVal value As String)
        '                _street1 = value
        '            End Set
        '        End Property


        '        Private _building As String
        '        Public Property building() As String
        '            Get
        '                Return _building
        '            End Get
        '            Set(ByVal value As String)
        '                _building = value
        '            End Set
        '        End Property


        '        Private _street2 As String
        '        Public Property street2() As String
        '            Get
        '                Return _street2
        '            End Get
        '            Set(ByVal value As String)
        '                _street2 = value
        '            End Set
        '        End Property


        '        Private _city As String
        '        Public Property city() As String
        '            Get
        '                Return _city
        '            End Get
        '            Set(ByVal value As String)
        '                _city = value
        '            End Set
        '        End Property


        '        Private _area As String
        '        Public Property area() As String
        '            Get
        '                Return _area
        '            End Get
        '            Set(ByVal value As String)
        '                _area = value
        '            End Set
        '        End Property


        '        Private _state As String
        '        Public Property state() As String
        '            Get
        '                Return _state
        '            End Get
        '            Set(ByVal value As String)
        '                _state = value
        '            End Set
        '        End Property


        '        Private _postal As String
        '        Public Property postal() As String
        '            Get
        '                Return _postal
        '            End Get
        '            Set(ByVal value As String)
        '                _postal = value
        '            End Set
        '        End Property


        '        Private _country As String
        '        Public Property country() As String
        '            Get
        '                Return _country
        '            End Get
        '            Set(ByVal value As String)
        '                _country = value
        '            End Set
        '        End Property


        '        Private _invoice_num As String
        '        Public Property invoice_num() As String
        '            Get
        '                Return _invoice_num
        '            End Get
        '            Set(ByVal value As String)
        '                _invoice_num = value
        '            End Set
        '        End Property


        '        Private _invoice_date As DateTime
        '        Public Property invoice_date() As DateTime
        '            Get
        '                Return _invoice_date
        '            End Get
        '            Set(ByVal value As DateTime)
        '                _invoice_date = value
        '            End Set
        '        End Property


        '        Private _cust_num As Integer
        '        Public Property cust_num() As Integer
        '            Get
        '                Return _cust_num
        '            End Get
        '            Set(ByVal value As Integer)
        '                _cust_num = value
        '            End Set
        '        End Property


        '        Private _po_num As Integer
        '        Public Property po_num() As Integer
        '            Get
        '                Return _po_num
        '            End Get
        '            Set(ByVal value As Integer)
        '                _po_num = value
        '            End Set
        '        End Property


        '        Private _invoice_amt As Decimal
        '        Public Property invoice_amt() As Decimal
        '            Get
        '                Return _invoice_amt
        '            End Get
        '            Set(ByVal value As Decimal)
        '                _invoice_amt = value
        '            End Set
        '        End Property


        '        Private _currency As String
        '        Public Property currency() As String
        '            Get
        '                Return _currency
        '            End Get
        '            Set(ByVal value As String)
        '                _currency = value
        '            End Set
        '        End Property


        '        Private _shipvia As String
        '        Public Property shipvia() As String
        '            Get
        '                Return _shipvia
        '            End Get
        '            Set(ByVal value As String)
        '                _shipvia = value
        '            End Set
        '        End Property


        '        Private _order_date As DateTime
        '        Public Property order_date() As DateTime
        '            Get
        '                Return _order_date
        '            End Get
        '            Set(ByVal value As DateTime)
        '                _order_date = value
        '            End Set
        '        End Property


        '        Private _order_num As Integer
        '        Public Property order_num() As Integer
        '            Get
        '                Return _order_num
        '            End Get
        '            Set(ByVal value As Integer)
        '                _order_num = value
        '            End Set
        '        End Property


        '        Private _est_delivery_date As DateTime
        '        Public Property est_delivery_date() As DateTime
        '            Get
        '                Return _est_delivery_date
        '            End Get
        '            Set(ByVal value As DateTime)
        '                _est_delivery_date = value
        '            End Set
        '        End Property


        '        Private _reference As String
        '        Public Property reference() As String
        '            Get
        '                Return _reference
        '            End Get
        '            Set(ByVal value As String)
        '                _reference = value
        '            End Set
        '        End Property


        '        Private _lineCount As Integer
        '        Public Property lineCount() As Integer
        '            Get
        '                Return _lineCount
        '            End Get
        '            Set(ByVal value As Integer)
        '                _lineCount = value
        '            End Set
        '        End Property



#End Region

        Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
        Public Sub ImportHeaderInformation(ByVal _header As Hashtable, ByVal _invoiceid As Integer)


            Dim sbcolumns As StringBuilder = New StringBuilder
            Dim sbvalues As StringBuilder = New StringBuilder

            For Each key As Object In _header.Keys
                sbcolumns.AppendFormat("{0},", key.ToString())
                sbvalues.AppendFormat("{0},", IIf(_header(key).ToString().Equals(""), "null", "'" & _header(key).ToString().Replace("'", "''") & "'"))
            Next


            Dim DAO As EInvoicing_HeaderDAO = New EInvoicing_HeaderDAO()
            Try
                DAO.InsertInvoiceHeaderRecord(sbcolumns.ToString(), sbvalues.ToString(), _invoiceid)
                EInvoicing_CurrentInvoice.HeaderItemColumns = String.Empty
            Catch ex As Exception

                EInvoicing_CurrentInvoice.HeaderItemColumns = sbcolumns.ToString()

                logger.InfoFormat("## Import Header Error [columns]: {0}", sbcolumns.ToString)
                logger.InfoFormat("## Import Header Error  [values]: {0}", sbvalues.ToString)
                Throw
            Finally
                DAO.Dispose()
            End Try



        End Sub




        Public Sub ImportInvoiceSACCodeInformation(ByVal _summary As Hashtable, ByVal _knownSACCodes As List(Of String), ByVal _invoiceid As String)
            Dim sbcolumns As StringBuilder = New StringBuilder
            Dim sbvalues As StringBuilder = New StringBuilder
            Dim dao As EInvoicing_InvoiceDAO = New EInvoicing_InvoiceDAO


            For Each key As Object In _summary.Keys
                If Not _summary(key).ToString().Equals(String.Empty) Then  ' skip if the value is NULL, we dont need to insert it.
                    If _knownSACCodes.Contains(key.ToString().ToLower()) Then ' is this a valid SACCode?
                        dao.InsertSummaryElement(_invoiceid, key.ToString(), IIf(_summary(key).ToString().Equals(""), "", _summary(key).ToString()).ToString())
                    End If
                End If
            Next
            dao.Dispose()

        End Sub

        Public Sub ImportItemSACCodeInformation(ByVal _item As Hashtable, ByVal _knownSACCodes As List(Of String), ByVal _invoiceid As String, ByVal _lineitemid As String)
            Dim sbcolumns As StringBuilder = New StringBuilder
            Dim sbvalues As StringBuilder = New StringBuilder

            Dim dao As EInvoicing_InvoiceDAO = New EInvoicing_InvoiceDAO

            For Each key As Object In _item.Keys
                If Not _item(key).ToString().Equals(String.Empty) Then  ' skip if the value is NULL. we dont need to insert it.
                    If _knownSACCodes.Contains(key.ToString().ToLower()) Then ' is this a valid SACCode?
                        dao.InsertLineItemElement(_invoiceid, _lineitemid, key.ToString(), CType(IIf(_item(key).ToString().Equals(""), "", _item(key).ToString()), String))
                    End If
                End If

            Next

        End Sub

#Region " IDisposable Support "
        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free managed resources when explicitly called
                End If

                ' TODO: free shared unmanaged resources
            End If
            Me.disposedValue = True
        End Sub


        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace