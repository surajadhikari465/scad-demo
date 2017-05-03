Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.IRMA.Common
Imports System.Linq
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Configuration

Namespace IRMA

    <DataContract()>
    Public Class Einvoice

        <DataMember()>
        Public Property eInvoiceID As Integer

        Public Shared Function ReparseEinvoice(ByVal eInvoiceID As Integer) As Result

            logger.Info("ReparseEinvoice() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim result As New Result

            Try
                Dim eInvoice As New EInvoicingJob
                eInvoice.LoadEInvoicingDataFromString(eInvoice.GetEInvoiceXML(eInvoiceID))
                eInvoice.ClearEInvoiceData(eInvoiceID)
                eInvoice.ParseInvoicesFromXML(eInvoice.XMLData, eInvoiceID)

                result.FunctionName = "ReparseEinvoice"
                result.Status = True

                Return result

            Catch ex As Exception
                With result
                    .FunctionName = "ReparseEinvoice"
                    .Exception = ex.Message
                    .ErrorMessage = "ReparseEinvoice() failed."
                    .Status = False
                End With

                Return result

            Finally
                connectionCleanup(factory)

            End Try

        End Function

    End Class
End Namespace