Imports System.Data.SqlClient
Imports WholeFoods.IRMA.TaxHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.TaxHosting.DataAccess
    Public Class TaxClassDAO

        ''' <summary>
        ''' Read the complete list of TaxClass objects.
        ''' </summary>
        ''' <exception cref="DataFactoryException" />
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTaxClasses() As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("TaxHosting_GetTaxClass")
        End Function

        ''' <summary>
        ''' Read complete list of TaxClass data and return ArrayList of TaxClassBO objects
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTaxClassList() As ArrayList
            Dim taxClassList As New ArrayList
            Dim taxClassBO As TaxClassBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("TaxHosting_GetTaxClass")

                While results.Read
                    taxClassBO = New TaxClassBO()
                    taxClassBO.TaxClassId = results.GetInt32(results.GetOrdinal("TaxClassID"))
                    taxClassBO.TaxClassDesc = results.GetString(results.GetOrdinal("TaxClassDesc"))
                    taxClassList.Add(taxClassBO)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return taxClassList
        End Function

        Public Sub DeleteData(ByVal TaxClassID As Integer)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "TaxClassID"
                currentParam.Value = TaxClassID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("TaxHosting_DeleteTaxClass", paramList, True)
            Catch ex As Exception
                'TODO handle exception
            End Try
        End Sub

        ''' <summary>
        ''' Save changes to the DataSet to the database (inserts, updates, deletes)
        ''' </summary>
        ''' <param name="dataSet"></param>
        ''' <remarks></remarks>
        Public Sub SaveTaxClasses(ByRef dataSet As DataSet)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            factory.UpdateDataSet(dataSet, "TaxHosting_GetTaxClass", True)
        End Sub

        Public Sub AddDefaultTaxFlags(ByVal TaxClassDesc As String)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "TaxClassDesc"
                currentParam.Value = TaxClassDesc
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("TaxHosting_InsertDefaultTaxFlags", paramList)
            Catch exception As Exception
                Throw exception
            End Try

        End Sub

    End Class
End Namespace
