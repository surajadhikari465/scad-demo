Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.IRMA.Common
Imports System.Linq
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Configuration

Namespace IRMA

    <DataContract()>
    Public Class Currency

        <DataMember()>
        Public Property CurrencyID As Integer
        <DataMember()>
        Public Property CurrencyCode As String
        <DataMember()>
        Public Property CurrencyName As String

        Public Shared Function GetCurrencies() As List(Of Currency)

            logger.Info("GetCurrencies() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim dataTable As DataTable
            Dim currencies As New List(Of Currency)

            Try
                dataTable = factory.GetStoredProcedureDataTable("GetCurrencies")

                For Each dataRow As DataRow In dataTable.Rows
                    Dim currency As New Currency

                    currency.CurrencyID = dataRow.Item("CurrencyID")
                    currency.CurrencyCode = dataRow.Item("CurrencyCode")
                    currency.CurrencyName = dataRow.Item("CurrencyName")

                    currencies.Add(currency)
                Next

                Return currencies
            Catch ex As Exception
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try

        End Function

    End Class
End Namespace