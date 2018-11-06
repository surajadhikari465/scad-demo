Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports log4net
Imports WholeFoods.ServiceLibrary.WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess


Namespace WholeFoods.IRMA.Replenishment.EInvoicing.BusinessLogic
    Public Class EInvoicing_InvoiceBO
        Implements IDisposable

        Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Properties"
        Private _VendorId As String
        Public Property VendorId() As String
            Get
                Return _VendorId
            End Get
            Set(ByVal value As String)
                _VendorId = value
            End Set
        End Property


        Private _Invoice_Num As String
        Public Property Invoice_Num() As String
            Get
                Return _Invoice_Num
            End Get
            Set(ByVal value As String)
                _Invoice_Num = value
            End Set
        End Property


        Private _PONum As String
        Public Property PONum() As String
            Get
                Return _PONum
            End Get
            Set(ByVal value As String)
                _PONum = value
            End Set
        End Property


        Private _Store_Num As String
        Public Property Store_Num() As String
            Get
                Return _Store_Num
            End Get
            Set(ByVal value As String)
                _Store_Num = value
            End Set
        End Property


        Private _Invoice_Date As DateTime
        Public Property Invoice_Date() As DateTime
            Get
                Return _Invoice_Date
            End Get
            Set(ByVal value As DateTime)
                _Invoice_Date = value
            End Set
        End Property

#End Region


        Public Function CreateInvoiceRecord(ByVal invnum As String, ByVal vendorid As String, ByVal storenum As Integer, ByVal po_num As String, ByVal invoicedate As DateTime, ByVal invoicexml As String) As String

            ' create a recrod in einvoicing_Invoices and return the identity value.
            logger.InfoFormat("CreatingInvoiceRecord: {0},{1},{2},{3},{4}", invnum, vendorid, storenum, po_num, invoicedate)
            Dim inv As EInvoicing_InvoiceDAO = New EInvoicing_InvoiceDAO
            Dim retval As String = String.Empty

            retval = inv.InsertInvoiceRecord(invnum, vendorid, storenum, po_num, invoicedate, invoicexml)
            inv.Dispose()

            Return retval

        End Function

        Public Function IsDuplicateEinvoice(ByVal invnum As String, ByVal vendorid As String, ByVal ponum As String) As String
            ' Overloaded Method for backwards compatability. ForceLoad = Fasle if its not passed.
            Return IsDuplicateEinvoice(invnum, vendorid, ponum, False)
        End Function
        Public Function IsDuplicateEinvoice(ByVal invnum As String, ByVal vendorid As String, ByVal ponum As String, ByVal ForceLoad As Boolean) As String

            Dim inv As EInvoicing_InvoiceDAO = New EInvoicing_InvoiceDAO
            Dim retval As String = String.Empty

            retval = inv.IsDuplicateEInvoice(invnum, vendorid, ponum, ForceLoad)
            inv.Dispose()

            Return retval

        End Function
        Public Sub InsertLineItemElement(ByVal invoiceid As String, ByVal lineitemid As String, ByVal name As String, ByVal value As String)
            Dim lineitem As EInvoicing_InvoiceDAO = New EInvoicing_InvoiceDAO
            lineitem.InsertLineItemElement(invoiceid, lineitemid, name, value)
            lineitem.Dispose()
        End Sub

        Public Sub InsertHeaderElement(ByVal invoiceid As String, ByVal name As String, ByVal value As String)
            Dim header As EInvoicing_InvoiceDAO = New EInvoicing_InvoiceDAO
            header.InsertHeaderElement(invoiceid, name, value)
            header.Dispose()
        End Sub

        Public Sub InsertSummaryElement(ByVal invoiceid As String, ByVal name As String, ByVal value As String)
            Dim summary As EInvoicing_InvoiceDAO = New EInvoicing_InvoiceDAO
            summary.InsertSummaryElement(invoiceid, name, value)
            summary.Dispose()
        End Sub


        Public Sub ImportLineItemInformation(ByVal _item As Hashtable, ByVal _knownItemElements As List(Of String), ByVal _invoiceid As Integer, ByVal _lineitemid As Integer)
            Dim sbcolumns As StringBuilder = New StringBuilder
            Dim sbvalues As StringBuilder = New StringBuilder

            For Each key As Object In _item.Keys

                ' if the current dataelement is a known item element, use it when creating the item record.
                If _knownItemElements.Contains(key.ToString().ToLower()) Then

                    sbcolumns.AppendFormat("{0},", key.ToString())
                    sbvalues.AppendFormat("{0},", IIf(_item(key).ToString().Equals(""), "null", "'" & _item(key).ToString().Replace("'", "''") & "'"))

                End If
            Next

            Dim DAO As EInvoicing_InvoiceDAO = New EInvoicing_InvoiceDAO()
            Try
                DAO.InsertLineItemRecord(sbcolumns.ToString(), sbvalues.ToString(), _invoiceid, _lineitemid)
            Catch ex As Exception
                EInvoicing_CurrentInvoice.LineItemColumns = sbcolumns.ToString
                logger.InfoFormat("## Import LineItem Error [columns]: {0}", sbcolumns.ToString)
                logger.InfoFormat("## Import LineItem Error  [values]: {0}", sbvalues.ToString)
                Throw
            Finally
                DAO.Dispose()
            End Try


        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free managed resources when explicitly called
                    Me.Invoice_Date = Nothing
                    Me.Invoice_Num = Nothing
                    Me.PONum = Nothing
                    Me.Store_Num = Nothing
                    Me.VendorId = Nothing
                End If

                ' TODO: free shared unmanaged resources
            End If
            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace