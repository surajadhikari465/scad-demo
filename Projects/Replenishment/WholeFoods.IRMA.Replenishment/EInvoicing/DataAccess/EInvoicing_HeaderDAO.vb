Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility


Namespace WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess
    Public Class EInvoicing_HeaderDAO
        Implements IDisposable

        Public Sub InsertInvoiceHeaderRecord(ByVal columns As String, ByVal values As String, ByVal _invoiceid As Integer)

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

            factory.ExecuteStoredProcedure("EInvoicing_InsertInvoiceHeaderRecord", paramList)


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

