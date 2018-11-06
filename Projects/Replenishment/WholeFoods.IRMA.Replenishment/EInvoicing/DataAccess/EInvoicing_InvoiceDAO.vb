Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess
    Public Class EInvoicing_InvoiceDAO
        Implements IDisposable


        Public Function IsDuplicateEInvoice(ByVal Invoice_Num As String, ByVal Vendor_id As String, ByVal PONum As String) As String
            ' Overloaded Method for backwards compatability. ForceLoad = Fasle if its not passed.
            Return IsDuplicateEInvoice(Invoice_Num, Vendor_id, PONum, False)
        End Function


        Public Function IsDuplicateEInvoice(ByVal Invoice_Num As String, ByVal Vendor_id As String, ByVal PONum As String, ByVal ForcedLoad As Boolean) As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim dt As DataTable
            Dim retval As String
            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "Invoice_Num"
            currentParam.Value = Invoice_Num
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "VendorId"
            currentParam.Value = Vendor_id
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PONum"
            currentParam.Value = PONum
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ForceLoad"
            currentParam.Value = IIf(ForcedLoad, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            dt = factory.GetStoredProcedureDataTable("EInvoicing_IsDuplicateEInvoice", paramList)
            retval = dt.Rows(0)("ReturnValue").ToString()
            dt.Dispose()
            Return retval
        End Function

       

        Public Function InsertInvoiceRecord(ByVal Invoice_Num As String, ByVal Vendor_id As String, ByVal Store_Num As Integer, ByVal po_num As String, ByVal Invoice_Date As DateTime, ByVal invoicexml As String) As String
            Dim outputParameters As ArrayList = Nothing
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "Invoice_Num"
            currentParam.Value = IIf(Invoice_Num Is Nothing, "0", Invoice_Num)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "VendorId"
            currentParam.Value = IIf(Vendor_id Is Nothing, "0", Vendor_id)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_Num"
            currentParam.Value = Store_Num
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "po_num"
            currentParam.Value = IIf(po_num Is Nothing, "0", po_num)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Invoice_Date"
            currentParam.Value = Invoice_Date
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            'currentParam = New DBParam
            'currentParam.Name = "FileId"
            'currentParam.Value = FileId
            'currentParam.Type = DBParamType.Int
            'paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceXML"
            currentParam.Value = invoicexml
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceId"
            currentParam.Value = Nothing
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            outputParameters = factory.ExecuteStoredProcedure("EInvoicing_CreateInvoiceRecord", paramList)

            Return outputParameters(0).ToString()
        End Function
        Public Sub InsertLineItemElement(ByVal InvoiceId As String, ByVal LineItemId As String, ByVal name As String, ByVal value As String)
            Dim outputParameters As ArrayList = Nothing
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "InvoiceId"
            currentParam.Value = CInt(InvoiceId)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LineItemId"
            currentParam.Value = CInt(LineItemId)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ElementName"
            currentParam.Value = name
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ElementValue"
            currentParam.Value = value
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("EInvoicing_InsertLineItemElement", paramList)

        End Sub


        Public Sub InsertHeaderElement(ByVal InvoiceId As String, ByVal name As String, ByVal value As String)
            Dim outputParameters As ArrayList = Nothing
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "InvoiceId"
            currentParam.Value = CInt(InvoiceId)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)



            currentParam = New DBParam
            currentParam.Name = "ElementName"
            currentParam.Value = name
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ElementValue"
            currentParam.Value = value
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("EInvoicing_InsertHeaderElement", paramList)
        End Sub
        Public Sub InsertSummaryElement(ByVal InvoiceId As String, ByVal name As String, ByVal value As String)
            Dim outputParameters As ArrayList = Nothing
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "InvoiceId"
            currentParam.Value = CInt(InvoiceId)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)



            currentParam = New DBParam
            currentParam.Name = "ElementName"
            currentParam.Value = name
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ElementValue"
            currentParam.Value = value
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("EInvoicing_InsertSummaryElement", paramList)
        End Sub
        Private disposedValue As Boolean = False        ' To detect redundant calls




        Public Sub InsertLineItemRecord(ByVal columns As String, ByVal values As String, ByVal _invoiceid As Integer, ByVal _lineitemid As Integer)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam = Nothing


            ' Trim trailing commas
            If columns.EndsWith(",") Then
                columns = columns.Remove(columns.Length - 1, 1)
            End If
            If values.EndsWith(",") Then
                values = values.Remove(values.Length - 1, 1)
            End If


            currentParam = New DBParam
            currentParam.Name = "columns"
            currentParam.Value = columns
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "values"
            currentParam.Value = values
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceId"
            currentParam.Value = _invoiceid
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LineItemId"
            currentParam.Value = _lineitemid
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("EInvoicing_InsertLineItemRecord", paramList)

        End Sub



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
