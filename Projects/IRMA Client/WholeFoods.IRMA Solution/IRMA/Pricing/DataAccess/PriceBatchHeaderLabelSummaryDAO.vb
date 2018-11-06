Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Pricing.DataAccess

    Public Class PriceBatchHeaderLabelSummaryDAO

        Public Shared Function GetLabelSummaryData(ByVal itemKeys As String, ByVal separator As Char, ByVal priceBatchHeaderID As Integer) As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim row As DataRow
            Dim table As New DataTable("LabelSummary")
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            ' this limit is in place because of the math performed in PriceBatchItemSearch during processing -
            ' anything larger causes an overflow exception
            Dim iLoop As Integer
            Dim MaxLoop As Integer = 1073741823

            Try

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemList"
                currentParam.Value = itemKeys
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ItemListSeparator"
                currentParam.Value = separator
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                If priceBatchHeaderID = 0 Then
                    ' Execute the stored procedure 
                    results = factory.GetStoredProcedureDataReader("GetItem_LabelSummary", paramList)
                Else
                    currentParam = New DBParam
                    currentParam.Name = "PriceBatchHeaderID"
                    currentParam.Value = priceBatchHeaderID
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    ' Execute the stored procedure 
                    results = factory.GetStoredProcedureDataReader("GetPriceBatchHeader_LabelSummary", paramList)
                End If

                'add columns to table
                table.Columns.Add(New DataColumn("LabelType_ID", GetType(Integer)))
                table.Columns.Add(New DataColumn("LabelTypeDesc", GetType(String)))
                table.Columns.Add(New DataColumn("Item_Count", GetType(Integer)))
                table.Columns.Add(New DataColumn("PriceBatchHeaderID", GetType(Integer)))


                'If results.HasRows = False Then Return table

                While (results.Read) AndAlso (iLoop < MaxLoop)
                    iLoop = iLoop + 1
                    row = table.NewRow

                    If results.GetValue(results.GetOrdinal("LabelType_ID")).GetType IsNot GetType(DBNull) Then
                        row("LabelType_ID") = results.GetInt32(results.GetOrdinal("LabelType_ID"))
                    End If
                    If results.GetValue(results.GetOrdinal("LabelTypeDesc")).GetType IsNot GetType(DBNull) Then
                        row("LabelTypeDesc") = results.GetString(results.GetOrdinal("LabelTypeDesc"))
                    End If
                    If results.GetValue(results.GetOrdinal("Item_Count")).GetType IsNot GetType(DBNull) Then
                        row("Item_Count") = results.GetInt32(results.GetOrdinal("Item_Count"))
                    End If
                    If priceBatchHeaderID > 0 Then
                        If results.GetValue(results.GetOrdinal("PriceBatchHeaderID")).GetType IsNot GetType(DBNull) Then
                            row("PriceBatchHeaderID") = results.GetInt32(results.GetOrdinal("PriceBatchHeaderID"))
                        End If
                    End If

                    'row("Selected") = 0

                    table.Rows.Add(row)
                End While

                ' display a message to the user if there were more results that are not being displayed
                If results.Read Then
                    MsgBox("More data is available." & vbCrLf & "For more data, please limit search criteria.", MsgBoxStyle.Exclamation, "Notice!")
                End If

                'Catch e As Exception
                'TODO handle exception
                'Throw e
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return table
        End Function

    End Class

End Namespace
