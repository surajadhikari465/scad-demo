Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Imports System.Xml
Imports log4net
Imports WholeFoods.ServiceLibrary.WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess

Namespace WholeFoods.IRMA.Replenishment.EInvoicing.BusinessLogic

    Public Class EInvoicingBO
        Implements IDisposable

        Private Structure KnownElement
            Public ElementName As String
            Public NeedsConfig As Boolean
        End Structure

        Public Function checkForError(ByVal InvoiceId As String) As Boolean
            Dim retval As Boolean = False
            Dim dt As DataTable
            Dim DAO As EInvoicingDAO = New EInvoicingDAO

            dt = DAO.CheckForError(InvoiceId)

            If dt.Rows.Count > 0 Then
                EInvoicing_CurrentInvoice.ErrorCode = dt.Rows(0)("ErrorCode_Id").ToString()
                EInvoicing_CurrentInvoice.ErrorMessage = dt.Rows(0)("ErrorMessage").ToString()
                retval = True
            Else
                EInvoicing_CurrentInvoice.ErrorCode = String.Empty
                EInvoicing_CurrentInvoice.ErrorMessage = String.Empty
                retval = False
            End If

            Return retval
        End Function

        Public Function getKnownElements() As List(Of String)
            Dim DAO As EInvoicingDAO = New EInvoicingDAO

            Dim _list As List(Of String) = New List(Of String)
            For Each row As DataRow In DAO.GetKnownElements.Rows
                _list.Add(row("ElementName").ToString().ToLower())
            Next

            Return _list
        End Function

        Public Function getKnownSACCodes() As List(Of String)
            Dim DAO As EInvoicingDAO = New EInvoicingDAO

            Dim _list As List(Of String) = New List(Of String)
            For Each row As DataRow In DAO.GetKnownSACCodes.Rows
                _list.Add(row("ElementName").ToString().ToLower())
            Next

            Return _list
        End Function

        Public Function getKnownHeaderElements() As List(Of String)
            Dim DAO As EInvoicingDAO = New EInvoicingDAO

            Dim _list As List(Of String) = New List(Of String)
            For Each row As DataRow In DAO.GetKnownHeaderElements.Rows
                _list.Add(row("ElementName").ToString().ToLower())
            Next

            Return _list
        End Function

        Public Function getKnownitemElements() As List(Of String)
            Dim DAO As EInvoicingDAO = New EInvoicingDAO

            Dim _list As List(Of String) = New List(Of String)
            For Each row As DataRow In DAO.GetKnownItemElements.Rows
                _list.Add(row("ElementName").ToString().ToLower())
            Next

            Return _list
        End Function


        Public Function ValidateDataElements(ByRef invoicexml As XmlNode, ByVal InvoiceId As Integer) As Boolean
            Dim ds As DataSet = New DataSet
            Dim DataElements As List(Of String) = New List(Of String)
            Dim KnownElements As List(Of String) = New List(Of String)
            Dim UnknownElements As List(Of String) = New List(Of String)
            Dim DAO As EInvoicingDAO = New EInvoicingDAO
            Dim AllElementsOK As Boolean = True

            ' get a list of known elements from the einvoicing_config table
            For Each row As DataRow In DAO.GetKnownElements.Rows
                'Dim i As KnownElement = New KnownElement
                KnownElements.Add(row("ElementName").ToString().ToLower())
            Next

            ' get a list of elements from the data file
            ds.ReadXml(New XmlTextReader(New StringReader(invoicexml.OuterXml)))
            For Each t As DataTable In ds.Tables
                ' exclude the lineitems table because this is an index table created by the readxml function. not needed at this time.
                If Not t.TableName.ToLower().Equals("lineitems") Then
                    For Each c As DataColumn In t.Columns
                        If Not c.ColumnName.ToLower().Equals("invoice_id") And Not c.ColumnName.ToLower().Equals("lineitems_id") Then
                            DataElements.Add(c.ColumnName.ToLower())
                        End If
                    Next
                End If
            Next

            'make sure all elements in the file are in the config table
            For Each element As String In DataElements
                If Not KnownElements.Contains(element) Then
                    AllElementsOK = False
                    UnknownElements.Add(element)
                    'TODO: INSERT missing element into config
                End If
            Next

            EInvoicing_CurrentInvoice.UnknownElements = UnknownElements
            ' If Not AllElementsOK Then
            'Me.SetInvoiceStatus(InvoiceId, 1, "Suspended")
            'End If

            ds.Dispose()
            Return AllElementsOK


        End Function
        Public Function MatchInvoiceToPO(ByVal InvoiceID As Integer, ByVal UpdateIRMAInvoiceData As Boolean) As Boolean
            Dim returnvalue As Boolean

            Dim EInvoicing As EInvoicingDAO = New EInvoicingDAO
            returnvalue = EInvoicing.MatchInvoiceToPO(InvoiceID, UpdateIRMAInvoiceData)
            EInvoicing.Dispose()

            Return returnvalue
        End Function
        Public Sub SetInvoiceStatus(ByVal InvoiceId As Integer, ByVal ErrorCode As Integer, ByVal Status As String)
            Dim DAO As EInvoicingDAO = New EInvoicingDAO
            DAO.SetInvoiceStatus(InvoiceId, ErrorCode, Status)
            DAO.Dispose()
        End Sub

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